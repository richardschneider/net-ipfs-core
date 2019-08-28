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
        ///    A task that represents the asynchronous operation. The task's value is
        ///    the <see cref="Peer"/> information.
        /// </returns>
        Task<Peer> IdAsync(MultiHash peer = null, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get the version information.
        /// </summary>
        /// <returns>
        ///    A task that represents the asynchronous operation. The task's value is
        ///    a <see cref="Dictionary{TKey, TValue}"/> of values.
        /// </returns>
        Task<Dictionary<string, string>> VersionAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Stop the IPFS peer.
        /// </summary>
        /// <returns>
        ///    A task that represents the asynchronous operation.
        /// </returns>
        Task ShutdownAsync();

        /// <summary>
        ///   Resolve a name.
        /// </summary>
        /// <param name="name">
        ///   The name to resolve.
        /// </param>
        /// <param name="recursive">
        ///   Resolve until the result is an IPFS name. Defaults to <b>true</b>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   the resolved path as a <see cref="string"/>.
        /// </returns>
        /// <remarks>
        ///   The <paramref name="name"/> can be <see cref="Cid"/> + [path], "/ipfs/..." or
        ///   "/ipns/...".
        /// </remarks>
        Task<string> ResolveAsync(
            string name,
            bool recursive = true,
            CancellationToken cancel = default(CancellationToken)
            );

        /// <summary>
        ///   Send echo requests to a peer.
        /// </summary>
        /// <param name="peer">
        ///   The peer ID to receive the echo requests.
        /// </param>
        /// <param name="count">
        ///   The number of echo requests to send.  Defaults to 10.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   the sequence of <see cref="PingResult"/>.
        /// </returns>
        Task<IEnumerable<PingResult>> PingAsync(
            MultiHash peer,
            int count = 10,
            CancellationToken cancel = default(CancellationToken)
            );

        /// <summary>
        ///   Send echo requests to a peer.
        /// </summary>
        /// <param name="address">
        ///   The address of a peer to receive the echo requests.
        /// </param>
        /// <param name="count">
        ///   The number of echo requests to send.  Defaults to 10.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   the sequence of <see cref="PingResult"/>.
        /// </returns>
        Task<IEnumerable<PingResult>> PingAsync(
            MultiAddress address,
            int count = 10,
            CancellationToken cancel = default(CancellationToken)
            );
    }
}
