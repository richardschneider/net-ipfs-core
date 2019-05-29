using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages all the blocks stored locally.
    /// </summary>
    /// <seealso cref="IBlockApi"/>
    public interface IBlockRepositoryApi
    {
        /// <summary>
        ///   Perform a garbage collection sweep on the repo.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   TODO: not sure what this should return.
        /// </returns>
        Task RemoveGarage(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get statistics on the repository.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   the current <see cref="RepositoryData"/>.
        /// </returns>
        /// <remarks>
        ///   Same as <see cref="IStatsApi.RepositoryAsync(CancellationToken)"/>.
        /// </remarks>
        Task<RepositoryData> Statistics(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Verify all blocks in repo are not corrupted.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   TODO: not sure what this should return.
        /// </returns>
        Task Verify(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Gets the version number of the repo.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   the version number of the data block repository.
        /// </returns>
        Task<string> Version(CancellationToken cancel = default(CancellationToken));
    }
}
