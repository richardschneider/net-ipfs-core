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
        /// <remarks>
        ///   A DNS TXT record with a "dnslink=..." entry is expected to exist.  The
        ///   value of the "dnslink" is an IPFS path to another IPFS object.
        ///   <para>
        ///   A DNS query is generated for both <paramref name="name"/> and
        ///   _dnslink.<paramref name="name"/>.
        ///   </para>
        /// </remarks>
        /// <example>
        ///   <c>ResolveAsync("ipfs.io", recursive: false)</c> produces "/ipns/website.ipfs.io". Whereas,
        ///   <c>ResolveAsync("ipfs.io", recursive: true)</c> produces "/ipfs/QmXZz6vQTMiu6UyGxVgpLB6xJdHvvUbhdWagJQNnxXAjpn".
        /// </example>
        Task<string> ResolveAsync(
            string name,
            bool recursive = false,
            CancellationToken cancel = default(CancellationToken)
            );
    }
}
