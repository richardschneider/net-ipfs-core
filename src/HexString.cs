using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ipfs
{
    /// <summary>
    ///   A codec for <see href="https://en.wikipedia.org/wiki/Hexadecimal">Hexadecimal</see>.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   A codec for a hexadecimal string, <see cref="Encode"/> and <see cref="Decode"/>.  Adds the extension method <see cref="ToHexString"/>
    ///   to encode a byte array and <see cref="ToHexBuffer"/> to decode a hexadecimal <see cref="string"/>.
    ///   </para>
    /// </remarks>
    public static class HexString
    {
        static readonly string[] LowerCaseHexStrings = 
            Enumerable.Range(byte.MinValue, byte.MaxValue + 1)
            .Select(v => v.ToString("x2"))
            .ToArray();
        static readonly string[] UpperCaseHexStrings = 
            Enumerable.Range(byte.MinValue, byte.MaxValue + 1)
            .Select(v => v.ToString("X2"))
            .ToArray();
        static readonly Dictionary<string, byte> HexBytes =
            Enumerable.Range(byte.MinValue, byte.MaxValue + 1)
            .SelectMany(v => new [] { 
                new { Value = v, String = v.ToString("x2") } , 
                new { Value = v, String = v.ToString("X2") } 
            } ) 
            .Distinct()
            .ToDictionary(v => v.String, v => (byte) v.Value);


        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent hexadecimal string representation.
        /// </summary>
        /// <param name="buffer">
        ///   An array of <see cref="byte">8-bit unsigned integers</see>.
        /// </param>
        /// <param name="format">
        ///   One of the format specifiers ("G" and "x" for lower-case hex digits, or "X" for the upper-case).
        ///   The default is "G".
        /// </param>
        /// <returns>
        ///   The string representation, in hexadecimal, of the contents of <paramref name="buffer"/>.  
        /// </returns>
        public static string Encode(byte[] buffer, string format = "G")
        {
            string[] hexStrings;
            switch (format)
            {
                case "G":
                case "x":
                    hexStrings = LowerCaseHexStrings;
                    break;
                case "X":
                    hexStrings = UpperCaseHexStrings;
                    break;
                default:
                    throw new FormatException(string.Format("Invalid HexString format '{0}', only 'G', 'x' or 'X' are allowed.", format));
            }

            StringBuilder s = new StringBuilder(buffer.Length * 2);
            foreach (var v in buffer)
                s.Append(hexStrings[v]);
            return s.ToString();
        }

        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent hexadecimal string representation.
        /// </summary>
        /// <param name="buffer">
        ///   An array of <see cref="byte">8-bit unsigned integers</see>.
        /// </param>
        /// <param name="format">
        ///   One of the format specifiers ("G" and "x" for lower-case hex digits, or "X" for the upper-case).
        ///   The default is "G".
        /// </param>
        /// <returns>
        ///   The string representation, in hexadecimal, of the contents of <paramref name="buffer"/>.
        /// </returns>
        public static string ToHexString(this byte[] buffer, string format = "G")
        {
            return Encode(buffer, format);
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as hexadecimal digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The hexadecimal string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        public static byte[] Decode(string s)
        {
            int n = s.Length;
            if (n % 2 != 0)
                throw new InvalidDataException("The hex string length must be a multiple of 2.");

            var buffer = new byte[n / 2];
            for (int i = 0, j = 0; i < n; i += 2, j++)
            {
                var hex = s.Substring(i, 2);
                byte value;
                if (!HexBytes.TryGetValue(hex, out value))
                    throw new InvalidDataException(string.Format("'{0}' is not a valid hexadecimal byte.", hex));
                buffer[j] = value;
            }

            return buffer;
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as a hexadecimal string, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The hexadecimal string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        public static byte[] ToHexBuffer(this string s)
        {
            return Decode(s);
        }
    }
}
