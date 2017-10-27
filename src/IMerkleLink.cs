
namespace Ipfs
{

    /// <summary>
    ///   A link to another node in IPFS.
    /// </summary>
    public interface IMerkleLink
    {

        /// <summary>
        ///   A name associated with the linked node.
        /// </summary>
        /// <value>A <see cref="string"/> or <b>null</b>.</value>
        /// <remarks>
        ///   <note type="warning">
        ///   IPFS considers a <b>null</b> name different from a <see cref="string.Empty"/>
        ///   name;
        ///   </note>
        /// </remarks>
        string Name { get; }

        /// <summary>
        ///   The string representation of the <see cref="MultiHash"/> of the linked node.
        /// </summary>
        /// <value>
        ///   The unique ID of the linked node.
        /// </value>
        string Hash { get; }

        /// <summary>
        ///   The serialised size (in bytes) of the linked node.
        /// </summary>
        /// <value>Number of bytes.</value>
        long Size { get; }

    }
}