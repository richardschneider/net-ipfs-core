using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages the swarm of peers.
    /// </summary>
    /// <remarks>
    ///   The swarm is a sequence of connected peer nodes.
    /// </remarks>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/SWARM.md">Swarm API spec</seealso>
    public interface ISwarmApi
    {
        /// <summary>
        ///   Get the peers in the current swarm.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a sequence of peer nodes.
        /// </returns>
        Task<IEnumerable<Peer>> AddressesAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get the peers that are connected to this node.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a sequence of <see cref="Peer">Connected Peers</see>.
        /// </returns>
        Task<IEnumerable<Peer>> PeersAsync(CancellationToken cancel = default(CancellationToken));
    }
}
