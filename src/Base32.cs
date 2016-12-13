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
    ///   Copied from <see href="https://github.com/kappa7194/base32"/>.  The nuget package
    ///   is not used, because the assembly is not signed.
    ///   </para>
    /// </remarks>
    public static class Base32
    {
        const byte BitsInBlock = 5;
        const byte BitsInByte = 8;
        const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        const char Padding = '=';

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
            if (input.Length == 0)
            {
                return string.Empty;
            }

            var output = new char[(int)decimal.Ceiling((input.Length / (decimal)BitsInBlock)) * BitsInByte];
            var position = 0;
            byte workingByte = 0, remainingBits = BitsInBlock;

            foreach (var currentByte in input)
            {
                workingByte = (byte)(workingByte | (currentByte >> (BitsInByte - remainingBits)));
                output[position++] = Alphabet[workingByte];

                if (remainingBits < BitsInByte - BitsInBlock)
                {
                    workingByte = (byte)((currentByte >> (BitsInByte - BitsInBlock - remainingBits)) & 31);
                    output[position++] = Alphabet[workingByte];
                    remainingBits += BitsInBlock;
                }

                remainingBits -= BitsInByte - BitsInBlock;
                workingByte = (byte)((currentByte << remainingBits) & 31);
            }

            if (position != output.Length)
            {
                output[position++] = Alphabet[workingByte];
            }

            while (position < output.Length)
            {
                output[position++] = Padding;
            }

            return new string(output);
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
        public static byte[] Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new byte[0];
            }

            input = input.TrimEnd(Padding);

            var output = new byte[input.Length * BitsInBlock / BitsInByte];
            var position = 0;
            byte workingByte = 0, bitsRemaining = BitsInByte;

            foreach (var currentChar in input.ToCharArray())
            {
                int mask;
                int currentCharPosition;
                currentCharPosition = Alphabet.IndexOf(currentChar);
                if (currentCharPosition < 0)
                {
                    if (currentChar == Padding)
                        currentCharPosition = 0;
                    else
                        throw new FormatException("Invalid base32 string");
                }
                if (bitsRemaining > BitsInBlock)
                {
                    mask = currentCharPosition << (bitsRemaining - BitsInBlock);
                    workingByte = (byte)(workingByte | mask);
                    bitsRemaining -= BitsInBlock;
                }
                else
                {
                    mask = currentCharPosition >> (BitsInBlock - bitsRemaining);
                    workingByte = (byte)(workingByte | mask);
                    output[position++] = workingByte;
                    workingByte = unchecked((byte)(currentCharPosition << (BitsInByte - BitsInBlock + bitsRemaining)));
                    bitsRemaining += BitsInByte - BitsInBlock;
                }
            }

            return output;
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as base 32 digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The base 32 string to convert.
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
