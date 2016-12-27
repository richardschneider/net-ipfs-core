using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipfs
{

    /// <summary>
    ///   A link to another node in the IPFS Merkle DAG.
    /// </summary>
    public class DagLink
    {
        /// <summary>
        ///   Create a new instance of <see cref="DagLink"/> classs.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hash"></param>
        /// <param name="size"></param>
        public DagLink(string name, string hash, long size)
        {
            this.Name = name;
            this.Hash = hash;
            this.Size = size;
        }

        /// <summary>
        ///   A name associated with the linked node.
        /// </summary>
        /// <remarks>
        ///   IPFS considers a <b>null</b> name different from a <see cref="string.Empty"/>
        ///   name;
        /// </remarks>
        public string Name { get; private set; }

        /// <summary>
        ///   The <see cref="MultiHash"/> of the linked node.
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        ///   The size in bytes of the linked node.
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        ///   Writes the binary representation of the link to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="Stream"/> to write to.
        /// </param>
        public void Write(Stream stream)
        {
            using (var cos = new CodedOutputStream(stream, true))
            {
                Write(cos);
            }
        }

        /// <summary>
        ///   Writes the binary representation of the link to the specified <see cref="CodedOutputStream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="CodedOutputStream"/> to write to.
        /// </param>
        public void Write(CodedOutputStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            stream.WriteTag(1, WireFormat.WireType.LengthDelimited);
            var mh = new MultiHash(Hash);
            stream.WriteLength(mh.Algorithm.DigestSize + 2); // + 2 bytes for digest size
            mh.Write(stream);

            if (Name != null)
            {
                stream.WriteTag(2, WireFormat.WireType.LengthDelimited);
                stream.WriteString(Name);
            }

            stream.WriteTag(3, WireFormat.WireType.Varint);
            stream.WriteInt64(Size);
        }

        /// <summary>
        ///   Returns the IPFS binary representation as a byte array.
        /// </summary>
        /// <returns>
        ///   A byte array.
        /// </returns>
        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            {
                Write(ms);
                return ms.ToArray();
            }
        }

    }
}