using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    ///   <para>
    ///   Adds the following extension methods to <see cref="Stream"/>
    ///    <list type="bullet">
    ///      <item><description><see cref="ReadVarint32"/></description></item>
    ///      <item><description><see cref="ReadVarint64"/></description></item>
    ///      <item><description><see cref="WriteVarint"/></description></item>
    ///    </list>
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
            stream.WriteVarintAsync(value).Wait();
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
            return stream.ReadVarint32Async().Result;
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
            return stream.ReadVarint64Async().Result;
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
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///   When <paramref name="value"/> is negative.
        /// </exception>
        public static async Task WriteVarintAsync(this Stream stream, long value, CancellationToken cancel = default(CancellationToken))
        {
            if (value < 0)
                throw new NotSupportedException("Negative values are not allowed for a Varint");

            var bytes = new byte[10];
            int i = 0;
            do
            {
                byte v = (byte)(value & 0x7F);
                if (value > 0x7F)
                    v |= 0x80;
                bytes[i++] = v;
                value >>= 7;
            } while (value != 0);
            await stream.WriteAsync(bytes, 0, i, cancel);
        }

        /// <summary>
        ///   Reads a variable integer from the stream. 
        /// </summary>
        /// <param name="stream">
        ///   A varint encoded <see cref="Stream"/>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result
        ///   is the integer value in the <paramref name="stream"/>.
        /// </returns>
        /// <exception cref="InvalidDataException">
        ///   When the varint value is greater than <see cref="Int32.MaxValue"/>.
        /// </exception>
        public static async Task<int> ReadVarint32Async(this Stream stream, CancellationToken cancel = default(CancellationToken))
        {
            var value = await stream.ReadVarint64Async(cancel);
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
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <exception cref="InvalidDataException">
        ///   When the varint value is greater than <see cref="Int64.MaxValue"/>.
        /// </exception>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result
        ///   is the integer value in the <paramref name="stream"/>.
        /// </returns>
        public static async Task<long> ReadVarint64Async(this Stream stream, CancellationToken cancel = default(CancellationToken))
        {
            long value = 0;
            int shift = 0;
            int bytesRead = 0;
            var buffer = new byte[1];
            while (true)
            {
                if (1 != await stream.ReadAsync(buffer, 0, 1, cancel))
                {
                    throw new InvalidDataException("Varint is not terminated");
                }
                if (++bytesRead > 9)
                    throw new InvalidDataException("Varint value is bigger than an Int64.MaxValue");
                var b = buffer[0];
                value |= (long)(b & 0x7F) << shift;
                if (b < 0x80)
                    return value;
                shift += 7;
            }
        }

    }
}
