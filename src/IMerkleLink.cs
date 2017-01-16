
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
        /// <remarks>
        ///   IPFS considers a <b>null</b> name different from a <see cref="string.Empty"/>
        ///   name;
        /// </remarks>
        string Name { get; }

        /// <summary>
        ///   The string representation of the <see cref="MultiHash"/> of the linked node.
        /// </summary>
        string Hash { get; }

        /// <summary>
        ///   The size in bytes of the linked node.
        /// </summary>
        long Size { get; }

    }
}