using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Data trading module for IPFS. Its purpose is to request blocks from and 
    ///   send blocks to other peers in the network.
    /// </summary>
    /// <remarks>
    ///   Bitswap has two primary jobs
    ///   <list type="bullet">
    ///     <item>
    ///       <description>
    ///       Attempt to acquire blocks from the network that  have been requested by the client
    ///       </description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///       Judiciously (though strategically) send blocks in its possession to other peers who want them
    ///       </description>
    ///     </item>
    ///   </list>
    /// </remarks>
    /// <seealso href="https://github.com/ipfs/specs/tree/master/bitswap">Bitswap spec</seealso>
    public interface IBitswapApi
    {
        /// <summary>
        ///   Gets a block from the IPFS network.
        /// </summary>
        /// <param name="id">
        ///   The <see cref="Cid"/> of the <see cref="IDataBlock">block</see>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous get operation. The task's value
        ///   contains the block's id and data.
        /// </returns>
        /// <remarks>
        ///   Waits for another peer to supply the block with the <paramref name="id"/>.
        /// </remarks>
        Task<IDataBlock> GetAsync(Cid id, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   The blocks that are needed by a peer.
        /// </summary>
        /// <param name="peer">
        ///   The id of an IPFS peer. If not specified (e.g. null), then the local
        ///   peer is used.        
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   contains the sequence of blocks needed by the <paramref name="peer"/>.
        /// </returns>
        Task<IEnumerable<Cid>> WantsAsync(MultiHash peer = null, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Remove the CID from the want list.
        /// </summary>
        /// <param name="id">
        ///   The content that is no longer needed.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation.
        /// </returns>
        /// <remarks>
        ///   Any outstanding <see cref="GetAsync(Cid, CancellationToken)"/> for the
        ///   <paramref name="id"/> are cancelled.
        /// </remarks>
        Task UnwantAsync(Cid id, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Gets information on the blocks exchanged with a specific <see cref="Peer"/>.
        /// </summary>
        /// <param name="peer">
        ///   The peer to get information on.  If the peer is unknown, then a ledger
        ///   with zeros is returned.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   contains the <see cref="BitswapLedger"/> for the <paramref name="peer"/>.
        /// </returns>
        Task<BitswapLedger> LedgerAsync(Peer peer, CancellationToken cancel = default(CancellationToken));
    }
}
