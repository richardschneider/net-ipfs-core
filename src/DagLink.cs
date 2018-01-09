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
    public class DagLink : IMerkleLink
    {
        /// <summary>
        ///   Create a new instance of <see cref="DagLink"/> class.
        /// </summary>
        /// <param name="name">The name associated with the linked node.</param>
        /// <param name="hash">The <see cref="Cid"/> of the linked node.</param>
        /// <param name="size">The serialised size (in bytes) of the linked node.</param>
        public DagLink(string name, Cid hash, long size)
        {
            this.Name = name;
            this.Hash = hash;
            this.Size = size;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="DagLink"/> class from the
        ///   specified <see cref="IMerkleLink"/>.
        /// </summary>
        /// <param name="link">
        ///   Some type of a Merkle link.
        /// </param>
        public DagLink(IMerkleLink link)
        {
            this.Name = link.Name;
            this.Hash = link.Hash;
            this.Size = link.Size;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="DagLink"/> class from the
        ///   specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   A <see cref="Stream"/> containing the binary representation of the
        ///   <b>DagLink</b>.
        /// </param>
        public DagLink(Stream stream)
        {
            Read(stream);
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="DagLink"/> class from the
        ///   specified <see cref="CodedInputStream"/>.
        /// </summary>
        /// <param name="stream">(
        ///   A <see cref="CodedInputStream"/> containing the binary representation of the
        ///   <b>DagLink</b>.
        /// </param>
        public DagLink(CodedInputStream stream)
        {
            Read(stream);
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public Cid Hash { get; private set; }

        /// <inheritdoc />
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
            Hash.Write(stream);

            if (Name != null)
            {
                stream.WriteTag(2, WireFormat.WireType.LengthDelimited);
                stream.WriteString(Name);
            }

            stream.WriteTag(3, WireFormat.WireType.Varint);
            stream.WriteInt64(Size);
        }

        void Read(Stream stream)
        {
            using (var cis = new CodedInputStream(stream, true))
            {
                Read(cis);
            }
        }

        void Read(CodedInputStream stream)
        {
            while (!stream.IsAtEnd)
            {
                var tag = stream.ReadTag();
                switch (WireFormat.GetTagFieldNumber(tag))
                {
                    case 1:
                        Hash = Cid.Read(stream);
                        break;
                    case 2:
                        Name = stream.ReadString();
                        break;
                    case 3:
                        Size = stream.ReadInt64();
                        break;
                    default:
                        throw new InvalidDataException("Unknown field number");
                }
            }
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