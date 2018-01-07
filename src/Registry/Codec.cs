using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs.Registry
{
    /// <summary>
    ///   Metadata for IPFS multi-codec.
    /// </summary>
    /// <remarks>
    ///   IPFS assigns a unique <see cref="Name"/> and <see cref="Code"/> to codecs.
    ///   See <see href="https://github.com/multiformats/multicodec/blob/master/table.csv">table.csv</see> 
    ///   for the currently defined multi-codecs.
    /// </remarks>
    /// <seealso href="https://github.com/multiformats/multicodec"/>
    public class Codec
    {
        internal static Dictionary<string, Codec> Names = new Dictionary<string, Codec>();
        internal static Dictionary<int, Codec> Codes = new Dictionary<int, Codec>();

        /// <summary>
        ///   Register the standard multi-codecs for IPFS.
        /// </summary>
        /// <seealso href="https://github.com/multiformats/multicodec/blob/master/table.csv"/>
        static Codec()
        {
            Register("raw", 0x55);
            Register("cbor", 0x51);
            Register("protobuf", 0x50);
            Register("rlp", 0x60);
            Register("bencode", 0x63);
            Register("multicodec", 0x30);
            Register("multihash", 0x31);
            Register("multiaddr", 0x32);
            Register("multibase", 0x33);
            Register("dag-pb", 0x70);
            Register("dag-cbor", 0x71);
            Register("git-raw", 0x78);
            Register("eth-block", 0x90);
            Register("eth-block-list", 0x91);
            Register("eth-tx-trie", 0x92);
            Register("eth-tx", 0x93);
            Register("eth-tx-receipt-trie", 0x94);
            Register("eth-tx-receipt", 0x95);
            Register("eth-state-trie", 0x96);
            Register("eth-account-snapshot", 0x97);
            Register("eth-storage-trie", 0x98);
            Register("bitcoin-block", 0xb0);
            Register("bitcoin-tx", 0xb1);
            Register("zcash-block", 0xc0);
            Register("zcash-tx", 0xc1);
            Register("stellar-block", 0xd0);
            Register("stellar-tx", 0xd1);
            Register("torrent-info", 0x7b);
            Register("torrent-file", 0x7c);
            Register("ed25519-pub", 0xed);
        }

        /// <summary>
        ///   The name of the codec.
        /// </summary>
        /// <value>
        ///   A unique name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        ///   The IPFS code assigned to the codec.
        /// </summary>
        /// <value>
        ///   Valid codes at <see href="https://github.com/multiformats/multicodec/blob/master/table.csv"/>.
        /// </value>
        public int Code { get; private set; }

        /// <summary>
        ///   Use <see cref="Register"/> to create a new instance of a <see cref="Codec"/>.
        /// </summary>
        Codec()
        {
        }

        /// <summary>
        ///   The <see cref="Name"/> of the codec.
        /// </summary>
        /// <value>
        ///   The <see cref="Name"/> of the codec.
        /// </value>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        ///   Register a new IPFS codec.
        /// </summary>
        /// <param name="name">
        ///   The name of the codec.
        /// </param>
        /// <param name="code">
        ///   The IPFS code assigned to the codec.
        /// </param>
        /// <returns>
        ///   A new <see cref="Codec"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   When the <paramref name="name"/> or <paramref name="code"/> is already defined.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   When the <paramref name="name"/> is null or empty.
        /// </exception>
        public static Codec Register(string name, int code)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (Names.ContainsKey(name))
                throw new ArgumentException(string.Format("The IPFS codec name '{0}' is already defined.", name));
            if (Codes.ContainsKey(code))
                throw new ArgumentException(string.Format("The IPFS codec code '{0}' is already defined.", code));

            var a = new Codec
            {
                Name = name,
                Code = code
            };
            Names[name] = a;
            Codes[code] = a;

            return a;
        }
        /// <summary>
        ///   Remove an IPFS codec from the registry.
        /// </summary>
        /// <param name="codec">
        ///   The <see cref="Codec"/> to remove.
        /// </param>
        public static void Deregister(Codec codec)
        {
            Names.Remove(codec.Name);
            Codes.Remove(codec.Code);
        }

        /// <summary>
        ///   A sequence consisting of all codecs.
        /// </summary>
        /// <value>
        ///   All the registered codecs.
        /// </value>
        public static IEnumerable<Codec> All
        {
            get { return Names.Values; }
        }

    }
}
