using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs.Registry
{
    /// <summary>
    ///   Metadata and implemetations of an IPFS multi-base algorithm.
    /// </summary>
    /// <remarks>
    ///   IPFS assigns a unique <see cref="Name"/> and <see cref="Code"/> to multi-base algorithm.
    ///   See <see href="https://github.com/multiformats/multibase/blob/master/multibase.csv"/> for
    ///   the currently defined multi-base algorithms.
    ///   <para>
    ///   These algorithms are supported: base58btc, base58flickr, base64,
    ///   base64pad, base64url, base16, base32, base32z, base32pad, base32hex
    ///   and base32hexpad.
    ///   </para>
    /// </remarks>
    public class MultiBaseAlgorithm
    {
        internal static Dictionary<string, MultiBaseAlgorithm> Names = new Dictionary<string, MultiBaseAlgorithm>();
        internal static Dictionary<char, MultiBaseAlgorithm> Codes = new Dictionary<char, MultiBaseAlgorithm>();

        /// <summary>
        ///   Register the standard multi-base algorithms for IPFS.
        /// </summary>
        /// <seealso href="https://github.com/multiformats/multibase/blob/master/multibase.csv"/>
        static MultiBaseAlgorithm()
        {
            Register("base58btc", 'z',
                bytes => SimpleBase.Base58.Bitcoin.Encode(bytes),
                s => SimpleBase.Base58.Bitcoin.Decode(s));
            Register("base58flickr", 'Z',
                bytes => SimpleBase.Base58.Flickr.Encode(bytes),
                s => SimpleBase.Base58.Flickr.Decode(s));
            Register("base64", 'm',
                bytes => bytes.ToBase64NoPad(),
                s => s.FromBase64NoPad());
            Register("base64pad", 'M',
                bytes => Convert.ToBase64String(bytes),
                s => Convert.FromBase64String(s));
            Register("base64url", 'u',
                bytes => bytes.ToBase64Url(),
                s => s.FromBase64Url());
            Register("base16", 'f',
                bytes => SimpleBase.Base16.EncodeLower(bytes),
                s => SimpleBase.Base16.Decode(s));
            Register("base32", 'b',
                bytes => SimpleBase.Base32.Rfc4648.Encode(bytes, false).ToLowerInvariant(),
                s => SimpleBase.Base32.Rfc4648.Decode(s));
            Register("base32pad", 'c',
                bytes => SimpleBase.Base32.Rfc4648.Encode(bytes, true).ToLowerInvariant(),
                s => SimpleBase.Base32.Rfc4648.Decode(s));
            Register("base32hex", 'v',
                bytes => SimpleBase.Base32.ExtendedHex.Encode(bytes, false).ToLowerInvariant(),
                s => SimpleBase.Base32.ExtendedHex.Decode(s));
            Register("base32hexpad", 't',
                bytes => SimpleBase.Base32.ExtendedHex.Encode(bytes, true).ToLowerInvariant(),
                s => SimpleBase.Base32.ExtendedHex.Decode(s));
            Register("BASE16", 'F',
                bytes => SimpleBase.Base16.EncodeUpper(bytes),
                s => SimpleBase.Base16.Decode(s));
            Register("BASE32", 'B',
                bytes => SimpleBase.Base32.Rfc4648.Encode(bytes, false),
                s => SimpleBase.Base32.Rfc4648.Decode(s));
            Register("BASE32PAD", 'C',
                bytes => SimpleBase.Base32.Rfc4648.Encode(bytes, true),
                s => SimpleBase.Base32.Rfc4648.Decode(s));
            Register("BASE32HEX", 'V',
                bytes => SimpleBase.Base32.ExtendedHex.Encode(bytes, false),
                s => SimpleBase.Base32.ExtendedHex.Decode(s));
            Register("BASE32HEXPAD", 'T',
                bytes => SimpleBase.Base32.ExtendedHex.Encode(bytes, true),
                s => SimpleBase.Base32.ExtendedHex.Decode(s));
            Register("base32z", 'h',
                bytes => Base32z.Codec.Encode(bytes, false),
                s => Base32z.Codec.Decode(s));
            // Not supported
#if false
            Register("base1", '1');
            Register("base2", '0');
            Register("base8", '7');
            Register("base10", '9');
#endif
        }

        /// <summary>
        ///   The name of the algorithm.
        /// </summary>
        /// <value>
        ///   A unique name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        ///   The IPFS code assigned to the algorithm.
        /// </summary>
        /// <value>
        ///   Valid codes at <see href="https://github.com/multiformats/multibase/blob/master/multibase.csv"/>.
        /// </value>
        public char Code { get; private set; }

        /// <summary>
        ///   Returns a function that can return a string from a byte array.
        /// </summary>
        public Func<byte[], string> Encode { get; private set; }

        /// <summary>
        ///   Returns a function that can return a byte array from a string.
        /// </summary>
        public Func<string, byte[]> Decode { get; private set; }

        /// <summary>
        ///   Use <see cref="Register"/> to create a new instance of a <see cref="MultiBaseAlgorithm"/>.
        /// </summary>
        MultiBaseAlgorithm()
        {
        }

        /// <summary>
        ///   The <see cref="Name"/> of the algorithm.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        ///   Register a new IPFS algorithm.
        /// </summary>
        /// <param name="name">
        ///   The name of the algorithm.
        /// </param>
        /// <param name="code">
        ///   The IPFS code assigned to thealgorithm.
        /// </param>
        /// <param name="encode">
        ///   A <c>Func</c> to encode a byte array.  If not specified, then a <c>Func</c> is created to
        ///   throw a <see cref="NotImplementedException"/>.
        /// </param>
        /// <param name="decode">
        ///   A <c>Func</c> to decode a string.  If not specified, then a <c>Func</c> is created to
        ///   throw a <see cref="NotImplementedException"/>.
        /// </param>
        /// <returns>
        ///   A new <see cref="MultiBaseAlgorithm"/>.
        /// </returns>
        public static MultiBaseAlgorithm Register(
            string name, char code,
            Func<byte[], string> encode = null,
            Func<string, byte[]> decode = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (Names.ContainsKey(name))
                throw new ArgumentException(string.Format("The IPFS multi-base algorithm name '{0}' is already defined.", name));
            if (Codes.ContainsKey(code))
                throw new ArgumentException(string.Format("The IPFS multi-base algorithm code '{0}' is already defined.", code));
            if (encode == null) {
                encode = (bytes) => { throw new NotImplementedException(string.Format("The IPFS encode multi-base algorithm '{0}' is not implemented.", name)); };
            }
            if (decode == null)
            {
                decode = (s) => { throw new NotImplementedException(string.Format("The IPFS decode multi-base algorithm '{0}' is not implemented.", name)); };
            }

            var a = new MultiBaseAlgorithm
            {
                Name = name,
                Code = code,
                Encode = encode,
                Decode = decode
            };
            Names[name] = a;
            Codes[code] = a;

            return a;
        }

        /// <summary>
        ///   Remove an IPFS algorithm from the registry.
        /// </summary>
        /// <param name="algorithm">
        ///   The <see cref="MultiBaseAlgorithm"/> to remove.
        /// </param>
        public static void Deregister(MultiBaseAlgorithm algorithm)
        {
            Names.Remove(algorithm.Name);
            Codes.Remove(algorithm.Code);
        }

        /// <summary>
        ///   A sequence consisting of all algorithms.
        /// </summary>
        public static IEnumerable<MultiBaseAlgorithm> All
        {
            get { return Names.Values; }
        }

    }
}
