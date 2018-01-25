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

        /// <summary>
        ///   Connect to a peer.
        /// </summary>
        /// <param name="address">
        ///   An ipfs <see cref="MultiAddress"/>, such as
        ///  <c>/ip4/104.131.131.82/tcp/4001/ipfs/QmaCpDMGvV2BGHeYERUEnRQAwe3N8SzbUtfsmvsqQLuvuJ</c>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        Task ConnectAsync(MultiAddress address, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Disconnect from a peer.
        /// </summary>
        /// <param name="address">
        ///   An ipfs <see cref="MultiAddress"/>, such as
        ///  <c>/ip4/104.131.131.82/tcp/4001/ipfs/QmaCpDMGvV2BGHeYERUEnRQAwe3N8SzbUtfsmvsqQLuvuJ</c>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        Task DisconnectAsync(MultiAddress address, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Adds a new address filter.
        /// </summary>
        /// <param name="address">
        ///   An allowed address.  For example "/ip4/104.131.131.82" or
        ///   "/ip4/192.168.0.0/ipcidr/16".
        /// </param>
        /// <param name="persist">
        ///   If <b>true</b> the filter will persist across daemon reboots. 
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   the address filter that was added.
        /// </returns>
        /// <seealso href="https://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing"/>
        Task<MultiAddress> AddAddressFilterAsync(MultiAddress address, bool persist = false, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   List all the address filters.
        /// </summary>
        /// <param name="persist">
        ///   If <b>true</b> only persisted filters are listed.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   a sequence of addresses filters.
        /// </returns>
        /// <seealso href="https://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing"/>
        Task<IEnumerable<MultiAddress>> ListAddressFiltersAsync(bool persist = false, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Delete the specified address filter.
        /// </summary>
        /// <param name="address">
        ///   For example "/ip4/104.131.131.82" or
        ///   "/ip4/192.168.0.0/ipcidr/16".
        /// </param>
        /// <param name="persist">
        ///   If <b>true</b> the filter is also removed from the persistent store. 
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   the address filter that was removed.
        /// </returns>
        /// <seealso href="https://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing"/>
        Task<MultiAddress> RemoveAddressFilterAsync(MultiAddress address, bool persist = false, CancellationToken cancel = default(CancellationToken));

    }
}
