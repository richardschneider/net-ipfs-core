using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Get statistics/diagnostics for the various core components.
    /// </summary>
    public interface IStatsApi
    {
        /// <summary>
        ///   Get statistics on network bandwidth.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   the current <see cref="BandwidthData"/>.
        /// </returns>
        /// <seealso cref="ISwarmApi"/>
        Task<BandwidthData> BandwidthAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get statistics on the blocks exchanged with other peers.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   the current <see cref="BitswapData"/>.
        /// </returns>
        /// <seealso cref="IBitswapApi"/>
        Task<BitswapData> BitswapAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get statistics on repository.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   the current <see cref="BandwidthData"/>.
        /// </returns>
        Task<RepositoryData> RepositoryAsync(CancellationToken cancel = default(CancellationToken));
    }
}
