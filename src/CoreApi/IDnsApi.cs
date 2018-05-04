using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   DNS mapping to IPFS.
    /// </summary>
    /// <remarks>
    ///   Multihashes are hard to remember, but domain names are usually easy to
    ///   remember. To create memorable aliases for multihashes, DNS TXT
    ///   records can point to other DNS links, IPFS objects, IPNS keys, etc.
    /// </remarks>
    public interface IDnsApi
    {

        /// <summary>
        ///   Resolve a domain name to an IPFS path.
        /// </summary>
        /// <param name="name">
        ///   An domain name, such as "ipfs.io".
        /// </param>
        /// <param name="recursive">
        ///   Resolve until the result is not a DNS link. Defaults to <b>false</b>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   the resolved IPFS path as a <see cref="string"/>, such as 
        ///   <c>/ipfs/QmYNQJoKGNHTpPxCBPh9KkDpaExgd2duMa3aF6ytMpHdao</c>.
        /// </returns>
        Task<string> ResolveAsync(
            string name,
            bool recursive = false,
            CancellationToken cancel = default(CancellationToken)
            );
    }
}
