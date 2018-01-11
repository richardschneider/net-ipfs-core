using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Some miscellaneous methods.
    /// </summary>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/MISCELLANEOUS.md">Generic API spec</seealso>
    public interface IGenericApi
    {
        /// <summary>
        ///   Information about an IPFS peer.
        /// </summary>
        /// <param name="peer">
        ///   The id of the IPFS peer.  If not specified (e.g. null), then the local
        ///   peer is used.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   Information on the peer node.
        /// </returns>
        Task<Peer> IdAsync(MultiHash peer = null, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get the version information.
        /// </summary>
        Task<Dictionary<string, string>> VersionAsync(CancellationToken cancel = default(CancellationToken));

    }
}
