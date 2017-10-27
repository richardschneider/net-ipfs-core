using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipfs
{
    /// <summary>
    ///   A codec for a variable integer.
    /// </summary>
    /// <remarks>
    ///   A <b>VarInt</b> is encoded in network byte order (Big Endian). Each byte (except the last) contains 7 bits
    ///   of information with the most significant bit set to 1.  The last byte has MSB set to 0.
    ///   <para>
    ///   Negative values are not allowed.  When encountered a <see cref="NotSupportedException"/> is thrown.
    ///   </para>
    /// </remarks>
    /// <seealso href="https://developers.google.com/protocol-buffers/docs/encoding#varints"/>
    public static class Varint
    {
        /// <summary>
        ///   Convert the value to its variable integer encoding.
        /// </summary>
        /// <param name="value">
        ///   The value to convert.
        /// </param>
        /// <returns>
        ///   A byte array representing the <paramref name="value"/> as
        ///   a variable integer.
        /// </returns>
        public static byte[] Encode(long value)
        {
            using (var stream = new MemoryStream(64))
            {
                stream.WriteVarint(value);
                return stream.ToArray();
            }
        }

        /// <summary>
        ///   The number of bytes required to encode the value.
        /// </summary>
        /// <param name="value">A positive integer value.</param>
        /// <returns>
        ///   The number of bytes required to encode the value.
        /// </returns>
        public static int RequiredBytes(long value)
        {
            return Encode(value).Length;
        }

        /// <summary>
        ///   Convert the byte array to an <see cref="int"/>.
        /// </summary>
        /// <param name="bytes">
        ///   A varint encoded byte array containing the variable integer.
        /// </param>
        /// <param name="offset">
        ///   Offset into <paramref name="bytes"/> to start reading from.
        /// </param>
        /// <returns>The integer value.</returns>
        public static int DecodeInt32 (byte[] bytes, int offset = 0)
        {
            using (var stream = new MemoryStream(bytes, false))
            {
                stream.Position = offset;
                return stream.ReadVarint32();
            }
        }

        /// <summary>
        ///   Convert the byte array to a <see cref="long"/>.
        /// </summary>
        /// <param name="bytes">
        ///   A varint encoded byte array containing the variable integer.
        /// </param>
        /// <param name="offset">
        ///   Offset into <paramref name="bytes"/> to start reading from.
        /// </param>
        /// <returns>The integer value.</returns>
        public static long DecodeInt64(byte[] bytes, int offset = 0)
        {
            using (var stream = new MemoryStream(bytes, false))
            {
                stream.Position = offset;
                return stream.ReadVarint64();
            }
        }

        /// <summary>
        ///   Writes the variable integer encoding of the value to
        ///   a stream.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="Stream"/> to write to.
        /// </param>
        /// <param name="value">
        ///   A non-negative value to write.
        /// </param>
        /// <exception cref="NotSupportedException">
        ///   When <paramref name="value"/> is negative.
        /// </exception>
        public static void WriteVarint(this Stream stream, long value)
        {
            if (value < 0)
                throw new NotSupportedException("Negative values are not allowed for a Varint");

            do
            {
                byte v = (byte) (value & 0x7F);
                if (value > 0x7F)
                    v |= 0x80;
                stream.WriteByte(v);
                value >>= 7;
            } while (value != 0);
        }

        /// <summary>
        ///   Reads a variable integer from the stream. 
        /// </summary>
        /// <param name="stream">
        ///   A varint encoded <see cref="Stream"/>.
        /// </param>
        /// <exception cref="InvalidDataException">
        ///   When the varint value is greater than <see cref="Int32.MaxValue"/>.
        /// </exception>
        /// <returns>The integer value.</returns>
        public static int ReadVarint32(this Stream stream)
        {
            var value = stream.ReadVarint64();
            if (value > int.MaxValue)
                throw new InvalidDataException("Varint value is bigger than an Int32.MaxValue");
            return (int)value;
        }

        /// <summary>
        ///   Reads a variable integer from the stream. 
        /// </summary>
        /// <param name="stream">
        ///   A varint encoded <see cref="Stream"/>.
        /// </param>
        /// <exception cref="InvalidDataException">
        ///   When the varint value is greater than <see cref="Int64.MaxValue"/>.
        /// </exception>
        /// <returns>The integer value.</returns>
        public static long ReadVarint64(this Stream stream)
        {
            long value = 0;
            int shift = 0;
            int bytesRead = 0;
            while (true)
            {
                var b = stream.ReadByte();
                if (b == -1)
                    throw new InvalidDataException("Varint is not terminated");
                if (++bytesRead > 9)
                    throw new InvalidDataException("Varint value is bigger than an Int64.MaxValue");
                value |= (long)(b & 0x7F) << shift;
                if (b < 0x80)
                    return value;
                shift += 7;
            }

        }

    }
}
