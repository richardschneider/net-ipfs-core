using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Common.Logging;
using Google.Protobuf;
using Ipfs.Registry;

namespace Ipfs
{
    /// <summary> 
    ///   A protocol for differentiating outputs from various well-established cryptographic hash functions, 
    ///   addressing size + encoding considerations.
    /// </summary>
    /// <seealso href="https://github.com/jbenet/multihash"/>
    public class MultiHash : IEquatable<MultiHash>
    {
        static readonly ILog log = LogManager.GetLogger<MultiHash>();

        /// <summary>
        ///   The default hashing algorithm is "sha2-256".
        /// </summary>
        public const string DefaultAlgorithmName = "sha2-256";

        /// <summary>
        ///   Gets the <see cref="HashAlgorithm"/> with the specified IPFS multi-hash name.
        /// </summary>
        /// <param name="name">
        ///   The name of a hashing algorithm, see <see href="https://github.com/multiformats/multihash/blob/master/hashtable.csv"/>
        ///   for IPFS defined names.
        /// </param>
        public static HashAlgorithm GetHashAlgorithm(string name = DefaultAlgorithmName)
        {
            try
            {
                return HashingAlgorithm.Names[name].Hasher();
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Hash algorithm '{name}' is not registered");
            }
        }


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
        /// <param name="stream">
        ///   A <see cref="Stream"/> containing the binary representation of the
        ///   <b>MultiHash</b>.
        /// </param>
        /// <remarks>
        ///   Reads the binary representation of <see cref="MultiHash"/> from the <paramref name="stream"/>.
        ///   <para>
        ///   The binary representation is a <see cref="Varint"/> <see cref="HashingAlgorithm.Code"/>,
        ///   <see cref="Varint"/> <see cref="HashingAlgorithm.DigestSize"/> followed by the <see cref="Digest"/>.
        ///   </para>
        ///   <para>
        ///   When an unknown <see cref="HashingAlgorithm.Code">hashing algorithm number</see> is encountered
        ///   a new hashing algorithm is <see cref="HashingAlgorithm.Register">registered</see>.  This new algorithm does not support
        ///   matching nor computing a hash.
        ///   This behaviour allows parsing of any well formed <see cref="MultiHash"/> even when
        ///   the hashing algorithm is unknown.
        ///   </para>
        /// </remarks>
        public MultiHash(Stream stream)
        {
            Read(stream);
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="MultiHash"/> class from the
        ///   specified <see cref="CodedInputStream"/>.
        /// </summary>
        /// <param name="stream">
        ///   A <see cref="CodedInputStream"/> containing the binary representation of the
        ///   <b>MultiHash</b>.
        /// </param>
        /// <remarks>
        ///   Reads the binary representation of <see cref="MultiHash"/> from the <paramref name="stream"/>.
        ///   <para>
        ///   The binary representation is a <see cref="Varint"/> <see cref="HashingAlgorithm.Code"/>,
        ///   <see cref="Varint"/> <see cref="HashingAlgorithm.DigestSize"/> followed by the <see cref="Digest"/>.
        ///   </para>
        ///   <para>
        ///   When an unknown <see cref="HashingAlgorithm.Code">hashing algorithm number</see> is encountered
        ///   a new hashing algorithm is <see cref="HashingAlgorithm.Register">registered</see>.  This new algorithm does not support
        ///   matching nor computing a hash.
        ///   This behaviour allows parsing of any well formed <see cref="MultiHash"/> even when
        ///   the hashing algorithm is unknown.
        ///   </para>
        /// </remarks>
        public MultiHash(CodedInputStream stream)
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
        ///   <para>
        ///   When an unknown <see cref="HashingAlgorithm.Code">hashing algorithm number</see> is encountered
        ///   a new hashing algorithm is <see cref="HashingAlgorithm.Register">registered</see>.  This new algorithm does not support
        ///   matching nor computing a hash.
        ///   This behaviour allows parsing of any well formed <see cref="MultiHash"/> even when
        ///   the hashing algorithm is unknown.
        ///   </para>
        /// </remarks>
        /// <seealso cref="ToBase58"/>
        public MultiHash(string s)
        {
            using (var ms = new MemoryStream(s.FromBase58(), false))
            {
                Read(ms);
            }
        }

        /// <summary>
        ///   Implicit casting of a <see cref="string"/> to a <see cref="MultiHash"/>.
        /// </summary>
        /// <param name="s">
        ///   A <see cref="Base58"/> encoded <b>MultiHash</b>.
        /// </param>
        /// <returns>
        ///   A new <see cref="MultiHash"/>.
        /// </returns>
        /// <remarks>
        ///    Equivalent to <code>new MultiHash(s)</code>
        /// </remarks>
        static public implicit operator MultiHash(string s)
        {
            return new MultiHash(s);
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
        ///   Writes the binary representation of the multihash to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="Stream"/> to write to.
        /// </param>
        /// <remarks>
        ///   The binary representation is a 1-byte <see cref="HashingAlgorithm.Code"/>,
        ///   1-byte <see cref="HashingAlgorithm.DigestSize"/> followed by the <see cref="Digest"/>.
        /// </remarks>
        public void Write(Stream stream)
        {
            using (var cos = new CodedOutputStream(stream))
            {
                Write(cos);
            }
        }

        /// <summary>
        ///   Writes the binary representation of the multihash to the specified <see cref="CodedOutputStream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="CodedOutputStream"/> to write to.
        /// </param>
        /// <remarks>
        ///   The binary representation is a <see cref="Varint"/> <see cref="HashingAlgorithm.Code"/>,
        ///   <see cref="Varint"/> <see cref="HashingAlgorithm.DigestSize"/> followed by the <see cref="Digest"/>.
        /// </remarks>
        public void Write(CodedOutputStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            stream.WriteInt32(Algorithm.Code);
            stream.WriteLength(Algorithm.DigestSize);
            stream.WriteSomeBytes(Digest);
        }

        void Read(Stream stream)
        {
            using (var cis = new CodedInputStream(stream))
            {
                Read(cis);
            }
        }

        void Read(CodedInputStream stream)
        {
            var code = stream.ReadInt32();
            var digestSize = stream.ReadLength();

            HashingAlgorithm.Codes.TryGetValue(code, out HashingAlgorithm a);
            Algorithm = a;
            if (Algorithm == null)
            {
                Algorithm = HashingAlgorithm.Register("ipfs-" + code, code, digestSize);
                RaiseUnknownHashingAlgorithm(Algorithm);
            }
            else if (digestSize != Algorithm.DigestSize)
            {
                throw new InvalidDataException(string.Format("The digest size {0} is wrong for {1}; it should be {2}.", digestSize, Algorithm.Name, Algorithm.DigestSize));
            }

            Digest = stream.ReadSomeBytes(digestSize);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var that = obj as MultiHash;
            return (that == null)
                ? false
                : this.ToString() == that.ToString();
        }

        /// <inheritdoc />
        public bool Equals(MultiHash that)
        {
            return this.ToString() == that.ToString();
        }

        /// <summary>
        ///   Value equality.
        /// </summary>
        public static bool operator ==(MultiHash a, MultiHash b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;

            return a.Equals(b);
        }

        /// <summary>
        ///   Value inequality.
        /// </summary>
        public static bool operator !=(MultiHash a, MultiHash b)
        {
            if (object.ReferenceEquals(a, b)) return false;
            if (object.ReferenceEquals(a, null)) return true;
            if (object.ReferenceEquals(b, null)) return true;

            return !a.Equals(b);
        }

        /// <summary>
        ///   Returns the <see cref="Base58"/> encoding of the <see cref="MultiHash"/>.
        /// </summary>
        /// <returns>
        ///   A Base58 representaton of the MultiHash.
        /// </returns>
        public override string ToString()
        {
            return this.ToBase58();
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
        ///   Returns the IPFS binary representation as a byte array.
        /// </summary>
        /// <returns>
        ///   A byte array.
        /// </returns>
        /// <remarks>
        ///   The binary representation is a sequence of <see cref="MultiHash"/>.
        /// </remarks>
        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            {
                Write(ms);
                return ms.ToArray();
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
            for (int i = digest.Length - 1; 0 <= i; --i)
            {
                if (digest[i] != Digest[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        ///   Determines if the stream data matches the hash.
        /// </summary>
        /// <param name="data">
        ///   The <see cref="Stream"/> containing the data to check.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the data matches the <see cref="MultiHash"/>; otherwise, <b>false</b>.
        /// </returns>
        /// <remarks>
        ///   <b>Matches</b> is used to ensure data integrity.
        /// </remarks>
        public bool Matches(Stream data)
        {
            var digest = Algorithm.Hasher().ComputeHash(data);
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
                log.WarnFormat("Unknown hashing algorithm number 0x{0:x2}.", algorithm.Code);
 
            var handler = UnknownHashingAlgorithm;
            if (handler != null)
            {
                var args = new UnknownHashingAlgorithmEventArgs { Algorithm = algorithm };
                handler(this, args);
            }
        }

        /// <summary>
        ///   Generate the multihash for the specified byte array. 
        /// </summary>
        /// <param name="data">
        ///   The byte array containing the data to hash.
        /// </param>
        /// <param name="algorithmName">
        ///   The name of the hashing algorithm to use; defaults to <see cref="DefaultAlgorithmName"/>.
        /// </param>
        /// <returns>
        ///   A <see cref="MultiHash"/> for the <paramref name="data"/>.
        /// </returns>
        public static MultiHash ComputeHash(byte[] data, string algorithmName = DefaultAlgorithmName)
        {
            return new MultiHash(algorithmName, GetHashAlgorithm(algorithmName).ComputeHash(data));
        }

        /// <summary>
        ///   Generate the multihash for the specified <see cref="Stream"/>. 
        /// </summary>
        /// <param name="data">
        ///   The <see cref="Stream"/> containing the data to hash.
        /// </param>
        /// <param name="algorithmName">
        ///   The name of the hashing algorithm to use; defaults to <see cref="DefaultAlgorithmName"/>.
        /// </param>
        /// <returns>
        ///   A <see cref="MultiHash"/> for the <paramref name="data"/>.
        /// </returns>
        public static MultiHash ComputeHash(Stream data, string algorithmName = DefaultAlgorithmName)
        {
            return new MultiHash(algorithmName, GetHashAlgorithm(algorithmName).ComputeHash(data));
        }

    }

    /// <summary>
    ///   Provides data for the unknown hashing algorithm event.
    /// </summary>
    public class UnknownHashingAlgorithmEventArgs : EventArgs
    {
        /// <summary>
        ///   The <see cref="HashingAlgorithm"/> that is defined for the
        ///   unknown hashing number.
        /// </summary>
        public HashingAlgorithm Algorithm { get; set; }
    }
}