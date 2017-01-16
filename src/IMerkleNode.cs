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
    ///   A node in the IPFS.
    /// </summary>
    /// <remarks>
    ///   A <b>MerkleNde</b> has opaque <see cref="DataBytes"/> or <see cref="DataStream"/>
    ///   and a set of navigable <see cref="Links"/>.
    /// </remarks>
    /// <typeparam name="Link">
    ///   The type of <see cref="IMerkleLink"/> used by this node.
    /// </typeparam>
    public interface IMerkleNode<out Link>
        where Link : IMerkleLink
    {

        /// <summary>
        ///   Links to other nodes.
        /// </summary>
        /// <value>
        ///   A sequence of <typeparamref name="Link"/>.
        /// </value>
        /// <remarks>
        ///   It is never <b>null</b>.
        ///   <para>
        ///   The links are sorted ascending by <typeparamref name="Link.Name"/>. A <b>null</b>
        ///   name is compared as "".
        ///   </para>
        /// </remarks>
        IEnumerable<Link> Links { get; }

        /// <summary>
        ///   Opaque data of the node as a byte array.
        /// </summary>
        /// <remarks>
        ///   It is never <b>null</b>.
        /// </remarks>
        byte[] DataBytes { get; }

        /// <summary>
        ///   Opaque data of the node as a stream of bytes.
        /// </summary>
        Stream DataStream { get; }

        /// <summary>
        ///   The string representation of the <see cref="MultiHash"/> of the node.
        /// </summary>
        string Hash { get; }

        /// <summary>
        ///   Returns a link to the node.
        /// </summary>
        /// <param name="name">
        ///   A <see cref="IMerkleLink.Name"/> for the link; defaults to "".
        /// </param>
        /// <returns>
        ///   A new <see cref="IMerkleLink"/> to the node.
        /// </returns>
        Link ToLink(string name = "");

    }

}
