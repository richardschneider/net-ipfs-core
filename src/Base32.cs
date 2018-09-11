using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   A codec for Base-32.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   A codec for Base-32, <see cref="Encode"/> and <see cref="Decode"/>.  Adds the extension method <see cref="ToBase32"/>
    ///   to encode a byte array and <see cref="FromBase32"/> to decode a Base-32 string.
    ///   </para>
    ///   <para>
    ///   <see cref="Encode"/> and <see cref="ToBase32"/> produce the lower case form of 
    ///   <see href="https://tools.ietf.org/html/rfc4648"/> with no padding.
    ///   <see cref="Decode"/> and <see cref="FromBase32"/> are case-insensitive and
    ///   allow optional padding.
    ///   </para>
    ///   <para>
    ///   A thin wrapper around <see href="https://github.com/ssg/SimpleBase"/>.
    ///   </para>
    /// </remarks>
    public static class Base32
    {
        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent string representation that is 
        ///   encoded with base-32 characters.
        /// </summary>s
        /// <param name="input">
        ///   An array of 8-bit unsigned integers.
        /// </param>
        /// <returns>
        ///   The string representation, in base 32, of the contents of <paramref name="input"/>.
        /// </returns>
        public static string Encode(byte[] input)
        {
            return SimpleBase.Base32.Rfc4648.Encode(input, false).ToLowerInvariant();
        }

        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent string representation that is 
        ///   encoded with base-32 digits.
        /// </summary>
        /// <param name="bytes">
        ///   An array of 8-bit unsigned integers.
        /// </param>
        /// <returns>
        ///   The string representation, in base 32, of the contents of <paramref name="bytes"/>.
        /// </returns>
        public static string ToBase32(this byte[] bytes)
        {
            return Encode(bytes);
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as base 32 digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="input">
        ///   The base 32 string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="input"/>.
        /// </returns>
        /// <remarks>
        ///   <paramref name="input"/> is case-insensitive and allows padding.
        /// </remarks>
        public static byte[] Decode(string input)
        {
            return SimpleBase.Base32.Rfc4648.Decode(input);
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as base 32 digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The base 32 string to convert; case-insensitive and allows padding.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        public static byte[] FromBase32(this string s)
        {
            return Decode(s);
        }
    }
}
