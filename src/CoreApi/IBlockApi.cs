
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{

    /// <summary>
    ///   Manages IPFS blocks.
    /// </summary>
    /// <remarks>
    ///   An IPFS Block is a byte sequence that represents an IPFS Object 
    ///   (i.e. serialized byte buffers). It is useful to talk about them as "blocks" in Bitswap 
    ///   and other things that do not care about what is being stored. 
    ///   <para>
    ///   It is also possible to store arbitrary stuff using ipfs block put/get as the API 
    ///   does not check for proper IPFS Object formatting.
    ///   </para>
    ///   <note>
    ///   This may be very good or bad, we haven't decided yet 😄
    ///   </note>
    /// </remarks>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/BLOCK.md">Block API spec</seealso>
    public interface IBlockApi
    {
        /// <summary>
        ///   Gets a IPFS block.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <param name="id">
        ///   The <see cref="Cid"/> of the block.
        /// </param>
        /// <returns>
        ///    A task that represents the asynchronous get operation. The task's value
        ///    contains the block's id and data.
        /// </returns>
        Task<IDataBlock> GetAsync(Cid id, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Stores a byte array as a IPFS block.
        /// </summary>
        /// <param name="data">
        ///   The byte array to send to the IPFS network.
        /// </param>
        /// <param name="contentType">
        ///   The content type or format of the <paramref name="data"/>; such as "raw" or "dag-db".
        ///   See <see cref="MultiCodec"/> for more details.
        /// </param>
        /// <param name="multiHash">
        ///   The <see cref="MultiHash"/> algorithm name used to produce the <see cref="Cid"/>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///    A task that represents the asynchronous put operation. The task's value
        ///    contains the block's id and data.
        /// </returns>
        Task<IDataBlock> PutAsync(
            byte[] data,
            string contentType = Cid.DefaultContentType,
            string multiHash = MultiHash.DefaultAlgorithmName,
            CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Information on a IPFS block.
        /// </summary>
        /// <param name="id">
        ///   The <see cref="Cid"/> of the block.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///    A task that represents the asynchronous operation. The task's value
        ///    contains the block's id and data.
        /// </returns>
        Task<IDataBlock> StatAsync(Cid id, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Remove a IPFS block.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <param name="id">
        ///   The <see cref="Cid"/> of the block.
        /// </param>
        /// <param name="ignoreNonexistent">
        ///   If <b>true</b> do not raise exception when <paramref name="id"/> does not
        ///   exist.  Default value is <b>false</b>.
        /// </param>
        /// <returns>
        ///   The awaited Task will return the deleted <paramref name="id"/> or
        ///   <see cref="string.Empty"/> if the hash does not exist and <paramref name="ignoreNonexistent"/>
        ///   is <b>true</b>.
        /// </returns>
        /// <remarks>
        ///   This removes the block from the local cache and does not affect other peers.
        /// </remarks>
        Task<string> RemoveAsync(Cid id, bool ignoreNonexistent = false, CancellationToken cancel = default(CancellationToken));
    }

}
