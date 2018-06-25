using System;
using System.Collections.Generic;
using System.Text;
using Ipfs.Cryptography;
using System.Security.Cryptography;
using BC = Org.BouncyCastle.Crypto.Digests;


namespace Ipfs.Registry
{
    /// <summary>
    ///   Metadata and implemetations of a IPFS hashing algorithms.
    /// </summary>
    /// <remarks>
    ///   IPFS assigns a unique <see cref="Name"/> and <see cref="Code"/> to a hashing algorithm. 
    ///   See <see href="https://github.com/multiformats/multihash/blob/master/hashtable.csv">hashtable.csv</see>
    ///   for the currently defined hashing algorithms.
    ///   <para>
    ///   These algorithms are implemented:
    ///   <list type="bullet">
    ///   <item><description>blake2b-160, blake2b-256 blake2b-384 and blake2b-512</description></item>
    ///   <item><description>blake2s-128, blake2s-160, blake2s-224 a nd blake2s-256</description></item>
    ///   <item><description>keccak-224, keccak-256, keccak-384 and keccak-512</description></item>
    ///   <item><description>md4 and md5</description></item>
    ///   <item><description>sha1</description></item>
    ///   <item><description>sha2-256, sha2-512 and dbl-sha2-256</description></item>
    ///   <item><description>sha3-224, sha3-256, sha3-384 and sha3-512</description></item>
    ///   <item><description>shake-128 and shake-256</description></item>
    ///   </list>
    ///   </para>
    ///   <para>
    ///   The <c>identity</c> hash is also implemented;  which just returns the input bytes.
    ///   This is used to inline a small amount of data into a <see cref="Cid"/>.
    ///   </para>
    ///   <para>
    ///     Use <see cref="Register(string, int, int, Func{HashAlgorithm})"/> to add a new
    ///     hashing algorithm.
    ///   </para>
    /// </remarks>
    public class HashingAlgorithm
    {
        internal static Dictionary<string, HashingAlgorithm> Names = new Dictionary<string, HashingAlgorithm>();
        internal static Dictionary<int, HashingAlgorithm> Codes = new Dictionary<int, HashingAlgorithm>();

        /// <summary>
        ///   Register the standard hash algorithms for IPFS.
        /// </summary>
        /// <seealso href="https://github.com/multiformats/multihash/blob/master/hashtable.csv"/>
        static HashingAlgorithm()
        {
            Register("sha1", 0x11, 20, () => SHA1.Create());
            Register("sha2-256", 0x12, 32, () => SHA256.Create());
            Register("sha2-512", 0x13, 64, () => SHA512.Create());
            Register("dbl-sha2-256", 0x56, 32, () => new DoubleSha256());
            Register("keccak-224", 0x1A, 224 / 8, () => new KeccakManaged(224));
            Register("keccak-256", 0x1B, 256 / 8, () => new KeccakManaged(256));
            Register("keccak-384", 0x1C, 384 / 8, () => new KeccakManaged(384));
            Register("keccak-512", 0x1D, 512 / 8, () => new KeccakManaged(512));
            Register("sha3-224", 0x17, 224 / 8, () => new BouncyDigest(new BC.Sha3Digest(224)));
            Register("sha3-256", 0x16, 256 / 8, () => new BouncyDigest(new BC.Sha3Digest(256)));
            Register("sha3-384", 0x15, 384 / 8, () => new BouncyDigest(new BC.Sha3Digest(384)));
            Register("sha3-512", 0x14, 512 / 8, () => new BouncyDigest(new BC.Sha3Digest(512)));
            Register("shake-128", 0x18, 128 / 8, () => new BouncyDigest(new BC.ShakeDigest(128)));
            Register("shake-256", 0x19, 256 / 8, () => new BouncyDigest(new BC.ShakeDigest(256)));
            Register("blake2b-160", 0xb214, 160 / 8, () => new BouncyDigest(new BC.Blake2bDigest(160)));
            Register("blake2b-256", 0xb220, 256 / 8, () => new BouncyDigest(new BC.Blake2bDigest(256)));
            Register("blake2b-384", 0xb230, 384 / 8, () => new BouncyDigest(new BC.Blake2bDigest(384)));
            Register("blake2b-512", 0xb240, 512 / 8, () => new BouncyDigest(new BC.Blake2bDigest(512)));
            Register("blake2s-128", 0xb250, 128 / 8, () => new BouncyDigest(new BC.Blake2sDigest(128)));
            Register("blake2s-160", 0xb254, 160 / 8, () => new BouncyDigest(new BC.Blake2sDigest(160)));
            Register("blake2s-224", 0xb25c, 224 / 8, () => new BouncyDigest(new BC.Blake2sDigest(224)));
            Register("blake2s-256", 0xb260, 256 / 8, () => new BouncyDigest(new BC.Blake2sDigest(256)));
            Register("murmur3-32", 0x23, 32 / 8);
            Register("murmur3-128", 0x22, 128 / 8);
            Register("md4", 0xd4, 128 / 8, () => new BouncyDigest(new BC.MD4Digest()));
            Register("md5", 0xd5, 128 / 8, () => MD5.Create());
            Register("identity", 0, 0, () => new IdentityHash());
            RegisterAlias("id", "identity");
        }

        /// <summary>
        ///   The name of the algorithm.
        /// </summary>
        /// <value>
        ///   A unique name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        ///   The IPFS number assigned to the hashing algorithm.
        /// </summary>
        /// <value>
        ///   Valid hash codes at <see href="https://github.com/multiformats/multihash/blob/master/hashtable.csv">hashtable.csv</see>.
        /// </value>
        public int Code { get; private set; }

        /// <summary>
        ///   The size, in bytes, of the digest value.
        /// </summary>
        /// <value>
        ///   The digest value size in bytes. Zero indicates that the digest
        ///   is non fixed.
        /// </value>
        public int DigestSize { get; private set; }

        /// <summary>
        ///   Returns a cryptographic hash algorithm that can compute
        ///   a hash (digest).
        /// </summary>
        public Func<HashAlgorithm> Hasher { get; private set; }

        /// <summary>
        ///   Use <see cref="Register"/> to create a new instance of a <see cref="HashingAlgorithm"/>.
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
        ///   Register a new IPFS hashing algorithm.
        /// </summary>
        /// <param name="name">
        ///   The name of the algorithm.
        /// </param>
        /// <param name="code">
        ///   The IPFS number assigned to the hashing algorithm.
        /// </param>
        /// <param name="digestSize">
        ///   The size, in bytes, of the digest value.
        /// </param>
        /// <param name="hasher">
        ///   A <c>Func</c> that returns a <see cref="HashAlgorithm"/>.  If not specified, then a <c>Func</c> is created to
        ///   throw a <see cref="NotImplementedException"/>.
        /// </param>
        /// <returns>
        ///   A new <see cref="HashingAlgorithm"/>.
        /// </returns>
        public static HashingAlgorithm Register(string name, int code, int digestSize, Func<HashAlgorithm> hasher = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (Names.ContainsKey(name))
                throw new ArgumentException(string.Format("The IPFS hashing algorithm '{0}' is already defined.", name));
            if (Codes.ContainsKey(code))
                throw new ArgumentException(string.Format("The IPFS hashing algorithm code 0x{0:x2} is already defined.", code));
            if (hasher == null)
                hasher = () => { throw new NotImplementedException(string.Format("The IPFS hashing algorithm '{0}' is not implemented.", name)); };

            var a = new HashingAlgorithm
            {
                Name = name,
                Code = code,
                DigestSize = digestSize,
                Hasher = hasher
            };
            Names[name] = a;
            Codes[code] = a;

            return a;
        }

        /// <summary>
        ///   Register an alias for an IPFS hashing algorithm.
        /// </summary>
        /// <param name="alias">
        ///   The alias name.
        /// </param>
        /// <param name="name">
        ///   The name of the existing algorithm.
        /// </param>
        /// <returns>
        ///   A new <see cref="HashingAlgorithm"/>.
        /// </returns>
        public static HashingAlgorithm RegisterAlias(string alias, string name)
        {
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentNullException("alias");
            if (Names.ContainsKey(alias))
                throw new ArgumentException(string.Format("The IPFS hashing algorithm '{0}' is already defined and cannot be used as an alias.", alias));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (!Names.TryGetValue(name, out HashingAlgorithm existing))
                throw new ArgumentException(string.Format("The IPFS hashing algorithm '{0}' is not defined.", name));

            var a = new HashingAlgorithm
            {
                Name = name,
                Code = existing.Code,
                DigestSize = existing.DigestSize,
                Hasher = existing.Hasher
            };
            Names[alias] = a;

            return a;
        }

        /// <summary>
        ///   Remove an IPFS hashing algorithm from the registry.
        /// </summary>
        /// <param name="algorithm">
        ///   The <see cref="HashingAlgorithm"/> to remove.
        /// </param>
        public static void Deregister(HashingAlgorithm algorithm)
        {
            Names.Remove(algorithm.Name);
            Codes.Remove(algorithm.Code);
        }

        /// <summary>
        ///   A sequence consisting of all <see cref="HashingAlgorithm">hashing algorithms</see>.
        /// </summary>
        /// <value>
        ///   The currently registered hashing algorithms.
        /// </value>
        public static IEnumerable<HashingAlgorithm> All
        {
            get { return Names.Values; }
        }

    }
}
