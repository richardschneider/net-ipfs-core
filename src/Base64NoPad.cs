using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   A codec for Base-64 (RFC 4648) with no padding.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   A codec for Base-64, <see cref="Encode"/> and <see cref="Decode"/>.  Adds the extension method <see cref="ToBase64NoPad"/>
    ///   to encode a byte array and <see cref="FromBase64NoPad"/> to decode a Base-64 string.
    ///   </para>
    /// </remarks>
    public static class Base64NoPad
    {
        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent string representation that is 
        ///   encoded with base-64 characters.
        /// </summary>s
        /// <param name="bytes">
        ///   An array of 8-bit unsigned integers.
        /// </param>
        /// <returns>
        ///   The string representation, in base 64, of the contents of <paramref name="bytes"/>.
        /// </returns>
        public static string Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=');
        }

        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent string representation that is 
        ///   encoded with base-64 digits.
        /// </summary>
        /// <param name="bytes">
        ///   An array of 8-bit unsigned integers.
        /// </param>s
        /// <returns>
        ///   The string representation, in base 64, of the contents of <paramref name="bytes"/>.
        /// </returns>
        public static string ToBase64NoPad(this byte[] bytes)
        {
            return Encode(bytes);
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as base 64 digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The base 64 string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        public static byte[] Decode(string s)
        {
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default: throw new Exception("Illegal base64 string!");
            }

            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as base 64 digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The base 64 string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        public static byte[] FromBase64NoPad(this string s)
        {
            return Decode(s);
        }
    }
}
