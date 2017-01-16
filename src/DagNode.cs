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
    ///   A node in the IPFS Merkle DAG.
    /// </summary>
    /// <remarks>
    ///   A <b>DagNode</b> has opaque <see cref="DagNode.DataBytes"/>
    ///   and a set of navigable <see cref="DagNode.Links"/>.
    /// </remarks>
    public class DagNode : IMerkleNode<DagLink>
    {
        string hash;
        string hashAlgorithm;
        long? size;

        /// <summary>
        ///   Create a new instance of a <see cref="DagNode"/> with the specified
        ///   <see cref="DagNode.DataBytes"/> and <see cref="DagNode.Links"/>
        /// </summary>
        /// <param name="data">
        ///   The opaque data, can be <b>null</b>
        /// </param>
        /// <param name="links">
        ///   The links to other nodes.
        /// </param>
        /// <param name="hashAlgorithm">
        ///   The name of the hashing algorithm to use; defaults to 
        ///   <see cref="MultiHash.DefaultAlgorithmName"/>.
        /// </param>
        public DagNode(byte[] data, IEnumerable<DagLink> links = null, string hashAlgorithm = MultiHash.DefaultAlgorithmName)
        {
            this.DataBytes = data == null ? new byte[0] : data;
            this.Links = (links == null ? new DagLink[0] : links)
                .OrderBy(link => link.Name == null ? "" : link.Name);
            this.hashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="DagNode"/> class from the
        ///   specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">(
        ///   A <see cref="Stream"/> containing the binary representation of the
        ///   <b>DagNode</b>.
        /// </param>
        public DagNode(Stream stream)
        {
            Read(stream);
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="DagNode"/> class from the
        ///   specified <see cref="CodedInputStream"/>.
        /// </summary>
        /// <param name="stream">(
        ///   A <see cref="CodedInputStream"/> containing the binary representation of the
        ///   <b>DagNode</b>.
        /// </param>
        public DagNode(CodedInputStream stream)
        {
            Read(stream);
        }

        /// <inheritdoc />
        /// <value>
        ///   A sequence of <see cref="DagLink"/>.
        /// </value>
        public IEnumerable<DagLink> Links { get; private set; }

        /// <inheritdoc />
        public byte[] DataBytes { get; private set; }

        /// <inheritdoc />
        public Stream DataStream
        {
            get
            {
                return new MemoryStream(DataBytes, false);
            }
        }

        /// <summary>
        ///   The serialised size in bytes of the node.
        /// </summary>
        public long Size
        {
            get
            {
                if (!size.HasValue)
                {
                    ComputeHash();
                }
                return size.Value;
            }
        }

        /// <inheritdoc />
        public string Hash
        {
            get
            {
                if (hash == null)
                {
                   ComputeHash();
                }
                return hash;
            }
        }

        /// <inheritdoc />
        /// <returns>
        ///   A new <see cref="DagLink"/> to the node.
        /// </returns>
        public DagLink ToLink(string name = "")
        {
            return new DagLink(name, Hash, Size);
        }

        /// <summary>
        ///   Adds a link.
        /// </summary>
        /// <param name="link">
        ///   The <see cref="DagLink"/> to add.
        /// </param>
        /// <returns>
        ///   A new <see cref="DagNode"/> with the existing and new
        ///   link.
        /// </returns>
        /// <remarks>
        ///   A <b>DagNode</b> is immutable.
        /// </remarks>
        public DagNode AddLink(DagLink link)
        {
            return AddLinks(new[] { link });
        }

        /// <summary>
        ///   Adds a sequence of links.
        /// </summary>
        /// <param name="links">
        ///   The sequence of <see cref="DagLink"/> to add.
        /// </param>
        /// <returns>
        ///   A new <see cref="DagNode"/> with the existing and new
        ///   links.
        /// </returns>
        /// <remarks>
        ///   A <b>DagNode</b> is immutable.
        /// </remarks>
        public DagNode AddLinks(IEnumerable<DagLink> links)
        {
            var all = Links.Union(links);
            return new DagNode(DataBytes, all, hashAlgorithm);
        }

        /// <summary>
        ///   Removes a link.
        /// </summary>
        /// <param name="link">
        ///   The <see cref="IMerkleLink"/> to remove.
        /// </param>
        /// <returns>
        ///   A new <see cref="DagNode"/> with the <paramref name="link"/>
        ///   removed.
        /// </returns>
        /// <remarks>
        ///   A <b>DagNode</b> is immutable.
        ///   <para>
        ///   No exception is raised if the <paramref name="link"/> does
        ///   not exist.
        ///   </para>
        /// </remarks>
        public DagNode RemoveLink(IMerkleLink link)
        {
            return RemoveLinks(new[] { link });
        }

        /// <summary>
        ///   Remove a sequence of links.
        /// </summary>
        /// <param name="links">
        ///   The sequence of <see cref="IMerkleLink"/> to remove.
        /// </param>
        /// <returns>
        ///   A new <see cref="DagNode"/> with the <paramref name="links"/>
        ///   removed.
        /// </returns>
        /// <remarks>
        ///   A <b>DagNode</b> is immutable.
        ///   <para>
        ///   No exception is raised if any of the <paramref name="links"/> do
        ///   not exist.
        ///   </para>
        /// </remarks>
        public DagNode RemoveLinks(IEnumerable<IMerkleLink> links)
        {
            var ignore = links.ToLookup(link => link.Hash);
            var some = Links.Where(link => !ignore.Contains(link.Hash));
            return new DagNode(DataBytes, some, hashAlgorithm);
        }

        /// <summary>
        ///   Writes the binary representation of the node to the specified <see cref="Stream"/>.
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
        ///   Writes the binary representation of the node to the specified <see cref="CodedOutputStream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="CodedOutputStream"/> to write to.
        /// </param>
        public void Write(CodedOutputStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            foreach (DagLink link in Links)
            {
                using (var linkStream = new MemoryStream())
                {
                    link.Write(linkStream);
                    var msg = linkStream.ToArray();
                    stream.WriteTag(2, WireFormat.WireType.LengthDelimited);
                    stream.WriteLength(msg.Length);
                    stream.WriteSomeBytes(msg);
                }
            }
            
            if (DataBytes.Length > 0)
            {
                stream.WriteTag(1, WireFormat.WireType.LengthDelimited);
                stream.WriteLength(DataBytes.Length);
                stream.WriteSomeBytes(DataBytes);
            }
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
            var links = new List<DagLink>();
            bool done = false;

            while (!stream.IsAtEnd && !done)
            {
                var tag = stream.ReadTag();
                switch(WireFormat.GetTagFieldNumber(tag))
                {
                    case 1:
                        DataBytes = stream.ReadSomeBytes(stream.ReadLength());
                        done = true;
                        break;
                    case 2:
                        using (var ms = new MemoryStream(stream.ReadSomeBytes(stream.ReadLength())))
                        {
                            links.Add(new DagLink(ms));
                        }
                        break;
                    default:
                        throw new InvalidDataException("Unknown field number");
                }
            }

            if (DataBytes == null)
                DataBytes = new byte[0];
            Links = links.ToArray();
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

        void ComputeHash()
        {
            using (var ms = new MemoryStream())
            {
                Write(ms);
                size = ms.Position;
                ms.Position = 0;
                hash = MultiHash.ComputeHash(ms, hashAlgorithm).ToBase58();
            }
        }
    }

}
