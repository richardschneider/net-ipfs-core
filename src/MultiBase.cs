using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ipfs.Registry;

namespace Ipfs
{
    /// <summary>
    ///   Self identifying base encodings.
    /// </summary>
    /// <remarks>
    ///   <b>MultiBase</b> is a protocol for distinguishing base encodings 
    ///   and other simple string encodings.  
    ///   See the <see cref="MultiBaseAlgorithm">registry</see> for supported algorithms.
    /// </remarks>
    /// <seealso href="https://github.com/multiformats/multibase"/>
    public static class MultiBase
    {
        /// <summary>
        ///   The default multi-base algorithm is "base58btc".
        /// </summary>
        public const string DefaultAlgorithmName = "base58btc";

        /// <summary>
        ///   Gets the <see cref="MultiBaseAlgorithm"/> with the specified IPFS multi-hash name.
        /// </summary>
        /// <param name="name">
        ///   The name of an algorithm, see 
        ///   <see href="https://github.com/multiformats/multibase/blob/master/multibase.csv"/> for
        ///   for IPFS defined names.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        ///   When <paramref name="name"/> is not registered.
        /// </exception>
        static MultiBaseAlgorithm GetAlgorithm(string name)
        {
            try
            {
                return MultiBaseAlgorithm.Names[name];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"MutiBase algorithm '{name}' is not registered");
            }
        }

        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent string representation.
        /// </summary>
        /// <param name="bytes">
        ///   An array of 8-bit unsigned integers.
        /// </param>
        /// <param name="algorithmName">
        ///   The name of the multi-base algorithm to use. See <see href="https://github.com/multiformats/multibase/blob/master/multibase.csv"/>.
        /// </param>
        /// <returns>
        ///   A <see cref="string"/> starting with the algorithm's <see cref="MultiBaseAlgorithm.Code"/> and
        ///   followed by the encoded string representation of the <paramref name="bytes"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        ///   When <paramref name="algorithmName"/> is not registered.
        /// </exception>
        public static string Encode(byte[] bytes, string algorithmName = DefaultAlgorithmName)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            var alg = GetAlgorithm(algorithmName);
            return alg.Code + alg.Encode(bytes);
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The multi-base string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        /// <exception cref="FormatException">
        ///   When the <paramref name="s"/> can not be decoded.
        /// </exception>
        public static byte[] Decode(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException("s");
            }
            if (s.Length < 2)
            {
                throw new FormatException($"MultiBase '{s}' is invalid; too short.");
            }

            MultiBaseAlgorithm.Codes.TryGetValue(s[0], out MultiBaseAlgorithm alg);
            if (alg == null)
            {
                throw new FormatException($"MultiBase '{s}' is invalid. The code is not registered.");
            }

            try
            {
                return alg.Decode(s.Substring(1));
            }
            catch (Exception e)
            {
                throw new FormatException($"MultiBase '{s}' is invalid; decode failed.", e);
            }
        }

    }
}
