using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   A codec for IPFS Base58.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   This is just thin wrapper of <see href="https://github.com/adamcaudill/Base58Check"/>.  However, its NuGet package
    ///   is <see href="https://github.com/adamcaudill/Base58Check/issues/2">not working</see>, so for now its a copy-and-paste.
    ///   </para>
    ///   <para>
    ///   This codec uses the BitCoin alphabet <b>not Facebook</b>.
    ///   </para>
    /// </remarks>
    public static class Base58
    {
        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent string representation that is 
        ///   encoded with base-58 digits.
        /// </summary>
        /// <param name="bytes">
        ///   An array of 8-bit unsigned integers.
        /// </param>
        /// <returns>
        ///   The string representation, in base 58, of the contents of <paramref name="bytes"/>.
        /// </returns>
        public static string Encode(byte[] bytes)
        {
            return Base58Check.Base58CheckEncoding.EncodePlain(bytes);
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as base 58 digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The base 58 string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        public static byte[] Decode(string s)
        {
            return Base58Check.Base58CheckEncoding.DecodePlain(s);
        }
    }
}
