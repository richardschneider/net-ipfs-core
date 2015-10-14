using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Common.Logging;

namespace Ipfs
{
    /// <summary> 
    ///   A protocol for differentiating outputs from various well-established cryptographic hash functions, 
    ///   addressing size + encoding considerations.
    /// </summary>
    /// <seealso href="https://github.com/jbenet/multihash"/>
    public class MultiHash
    {
        static readonly ILog log = LogManager.GetLogger<MultiHash>();

        /// <summary>
        ///   Metadata on an IPFS hashing algorithm.
        /// </summary>
        public class HashingAlgorithm
        {
            internal static Dictionary<string, HashingAlgorithm> Names = new Dictionary<string, HashingAlgorithm>();
            internal static HashingAlgorithm[] Numbers = new HashingAlgorithm[0x100];

            /// <summary>
            ///   The name of the algorithm.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            ///   The IPFS number assigned to the hashing algorithm.
            /// </summary>
            /// <remarks>
            ///   0x00-0x0f reserved for application specific functions. <br/>
            ///   0x10-0x3f reserved for SHA standard functions.
            /// </remarks>
            public byte Number { get; private set; }

            /// <summary>
            ///   The size, in bytes, of the digest value.
            /// </summary>
            public byte DigestSize { get; private set; }

            /// <summary>
            ///   Returns a cryptographic hash algorithm that can compute
            ///   a hash (digest).
            /// </summary>
            public Func<HashAlgorithm> Hasher { get; private set; }

            /// <summary>
            ///   Use <see cref="Define"/> to create a new instance of a <see cref="HashingAlgorithm"/>.
            /// </summary>
            HashingAlgorithm()
            {
            }

            /// <summary>
            ///   The <see cref="Name"/> of the hashing algorithm.
            /// </summary>
            public override string ToString()
            {
                return Name;
            }

            /// <summary>
            ///   Define a new IPFS hashing algorithm.
            /// </summary>
            /// <param name="name">
            ///   The name of the algorithm.
            /// </param>
            /// <param name="number">
            ///   The IPFS number assigned to the hashing algorithm.
            /// </param>
            /// <param name="digestSize">
            ///   The size, in bytes, of the digest value.
            /// </param>
            /// <returns>
            ///   A new <see cref="HashingAlgorithm"/>.
            /// </returns>
            public static HashingAlgorithm Define(string name, byte number, byte digestSize, Func<HashAlgorithm> hasher = null)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException("name");
                if (Names.ContainsKey(name))
                    throw new ArgumentException(string.Format("The IPFS hashing algorithm '{0}' is already defined.", name));
                if (Numbers[number] != null)
                    throw new ArgumentException(string.Format("The IPFS hashing algorithm number '{0}' is already defined.", number));
                if (hasher == null)
                    hasher = () => { throw new NotImplementedException(string.Format("The IPFS hashing algorithm '{0}' is not implemented.", name)); };

                var a = new HashingAlgorithm
                {
                    Name = name,
                    Number = number,
                    DigestSize = digestSize,
                    Hasher = hasher
                };
                Names[name] = a;
                Numbers[number] = a;

                return a;
            }

            /// <summary>
            ///   A sequence consisting of all <see cref="HashingAlgorithm">hashing algorithms</see>.
            /// </summary>
            public static IEnumerable<HashingAlgorithm> All
            {
                get { return Names.Values; }
            }
        }

        /// <summary>
        ///   Defines the standard hash algorithms for IPFS.
        /// </summary>
        static MultiHash()
        {
            HashingAlgorithm.Define("sha1", 0x11, 20, () => { return new SHA1Managed(); });
            HashingAlgorithm.Define("sha2-256", 0x12, 32, () => { return new SHA256Managed(); });
            HashingAlgorithm.Define("sha2-512", 0x13, 64, () => { return new SHA512Managed(); });
            HashingAlgorithm.Define("sha3", 0x14, 64);
            HashingAlgorithm.Define("blake2b", 0x40, 64);
            HashingAlgorithm.Define("blake2s", 0x41, 32);
        }

        /// <summary>
        ///   The default hasing algorithm is "sha2-256".
        /// </summary>
        public const string DefaultAlgorithmName = "sha2-256";

        /// <summary>
        ///   Occurs when an unknown hashing algorithm number is parsed.
        /// </summary>
        public static EventHandler<UnknownHashingAlgorithmEventArgs> UnknownHashingAlgorithm;

        /// <summary>
        ///   Creates a new instance of the <see cref="MultiHash"/> class with the
        ///   specified <see cref="HashingAlgorithm">Algorithm name</see> and <see cref="Digest"/> value.
        /// </summary>
        /// <param name="algorithmName">
        ///   A valid IPFS hashing algorithm name, e.g. "sha2-256" or "sha2-512".
        /// </param>
        /// <param name="digest">
        ///    The digest value as a byte array.
        /// </param>
        public MultiHash(string algorithmName, byte[] digest)
        {
            if (algorithmName == null)
                throw new ArgumentNullException("algorithmName");
            if (digest == null)
                throw new ArgumentNullException("digest");

            HashingAlgorithm a;
            if (!HashingAlgorithm.Names.TryGetValue(algorithmName, out a))
                throw new ArgumentException(string.Format("The IPFS hashing algorithm '{0}' is unknown.", algorithmName));
            Algorithm = a;

            if (Algorithm.DigestSize != digest.Length)
                throw new ArgumentException(string.Format("The digest size for '{0}' is {1} bytes, not {2}.", algorithmName, Algorithm.DigestSize, digest.Length));
            Digest = digest;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="MultiHash"/> class from the
        ///   specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">(
        ///   A <see cref="Stream"/> containing the binary representation of the
        ///   <b>MultiHash</b>.
        /// </param>
        /// <remarks>
        ///   Reads the binary representation of <see cref="MultiHash"/> from the <paramref name="stream"/>.
        ///   <para>
        ///   The binary representation is a 1-byte <see cref="HashingAlgorithm.Number"/>,
        ///   1-byte <see cref="HashingAlgorithm.DigestSize"/> followed by the <see cref="Digest"/>.
        ///   </para>
        ///   <note>
        ///   When an unknown <see cref="HashingAlgorithm.Number">hashing algorithm number</see> is encountered
        ///   a new <see cref="HashingAlgorithm.Define"/> is defined.  This algorithm does not support
        ///   <see cref="Matches">matching</see> nor <see cref="ComputeHash">computing a hash</see>.
        ///   <para>
        ///   This behaviour allows parsing of any well formed <see cref="MultiHash"/> even when
        ///   the hashing algorithm is unknown.
        ///   </para>
        ///   </note>
        /// </remarks>
        /// <seealso cref="Write"/>
        public MultiHash(Stream stream)
        {
            Read(stream);
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="MultiHash"/> class from the specified
        ///   <see cref="Base58"/> encoded <see cref="string"/>.
        /// </summary>
        /// <param name="s">
        ///   A <see cref="Base58"/> encoded <b>MultiHash</b>.
        /// </param>
        /// <remarks>
        ///   <note>
        ///   When an unknown <see cref="HashingAlgorithm.Number">hashing algorithm number</see> is encountered
        ///   a new <see cref="HashingAlgorithm.Define"/> is defined.  This algorithm does not support
        ///   <see cref="Matches">matching</see> nor <see cref="ComputeHash">computing a hash</see>.
        ///   <para>
        ///   This behaviour allows parsing of any well formed <see cref="MultiHash"/> even when
        ///   the hashing algorithm is unknown.
        ///   </para>
        ///   </note>
        /// </remarks>
        /// <seealso cref="ToBase58"/>
        public MultiHash(string s)
        {
            using (var ms = new MemoryStream(s.FromBase58()))
            {
                Read(ms);
            }
        }

        /// <summary>
        ///   The hashing algorithm.
        /// </summary>
        public HashingAlgorithm Algorithm { get; private set; }

        /// <summary>
        ///   The hashing algorithm's digest value.
        /// </summary>
        public byte[] Digest { get; private set; }

        /// <summary>
        ///   Writes the binary representation to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="Stream"/> to write to.
        /// </param>
        /// <remarks>
        ///   The binary representation is a 1-byte <see cref="HashingAlgorithm.Number"/>,
        ///   1-byte <see cref="HashingAlgorithm.DigestSize"/> followed by the <see cref="Digest"/>.
        /// </remarks>
        public void Write(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            stream.WriteByte(Algorithm.Number);
            stream.WriteByte(Algorithm.DigestSize);
            stream.Write(Digest, 0, Algorithm.DigestSize);
        }

        void Read(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            byte number = (byte) stream.ReadByte();
            byte digestSize = (byte) stream.ReadByte();

            Algorithm = HashingAlgorithm.Numbers[number];
            if (Algorithm == null)
            {
                Algorithm = HashingAlgorithm.Define("ipfs-" + number, number, digestSize);
                RaiseUnknownHashingAlgorithm(Algorithm);
            }
            else if (digestSize != Algorithm.DigestSize)
            {
                throw new InvalidDataException(string.Format("The digest size {0} is wrong for {1}; it should be {2}.", digestSize, Algorithm.Name, Algorithm.DigestSize));
            }

            Digest = new byte[digestSize];
            stream.Read(Digest, 0, Digest.Length);
        }

        /// <summary>
        ///   Returns the <see cref="Base58"/> encoding of the <see cref="MultiHash"/>.
        /// </summary>
        /// <returns></returns>
        public string ToBase58()
        {
            using (var ms = new MemoryStream())
            {
                Write(ms);
                return ms.ToArray().ToBase58();
            }
        }

        /// <summary>
        ///   Determines if the data matches the hash.
        /// </summary>
        /// <param name="data">
        ///   The data to check.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the data matches the <see cref="MultiHash"/>; otherwise, <b>false</b>.
        /// </returns>
        /// <remarks>
        ///   <b>Matches</b> is used to ensure data integrity.
        /// </remarks>
        public bool Matches(byte[] data)
        {
            var digest = Algorithm.Hasher().ComputeHash(data);
            if (Digest.Length != digest.Length)
                return false;
            for (int i = digest.Length - 1; 0 <= i; --i)
            {
                if (digest[i] != Digest[i])
                    return false;
            }
            return true;
        }

        void RaiseUnknownHashingAlgorithm(HashingAlgorithm algorithm)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat("Unknown hashing algorithm number 0x{0:x2}.", algorithm.Number);
 
            var handler = UnknownHashingAlgorithm;
            if (handler != null)
            {
                var args = new UnknownHashingAlgorithmEventArgs { Algorithm = algorithm };
                handler(this, args);
            }
        }

        public static MultiHash ComputeHash(byte[] data, string algorithmName = DefaultAlgorithmName)
        {
            if (algorithmName == null)
                throw new ArgumentNullException("algorithmName");
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");

            HashingAlgorithm a;
            if (!HashingAlgorithm.Names.TryGetValue(algorithmName, out a))
                throw new ArgumentException(string.Format("The IPFS hashing algorithm '{0}' is unknown.", algorithmName));

            var digest = a.Hasher().ComputeHash(data);
            return new MultiHash(algorithmName, digest);
        }

        public static MultiHash ComputeHash(Stream data, string algorithmName = DefaultAlgorithmName)
        {
            if (algorithmName == null)
                throw new ArgumentNullException("algorithmName");
            if (data == null)
                throw new ArgumentNullException("data");

            HashingAlgorithm a;
            if (!HashingAlgorithm.Names.TryGetValue(algorithmName, out a))
                throw new ArgumentException(string.Format("The IPFS hashing algorithm '{0}' is unknown.", algorithmName));

            throw new NotImplementedException();
        }

    }

    /// <summary>
    ///   Provides data for the unknown hashing algorithm event.
    /// </summary>
    public class UnknownHashingAlgorithmEventArgs : EventArgs
    {
        /// <summary>
        ///   The <see cref="Ipfs.MultiHash.HashingAlgorithm"/> that is defined for the
        ///   unknown hashing number.
        /// </summary>
        public Ipfs.MultiHash.HashingAlgorithm Algorithm { get; set; }
    }
}