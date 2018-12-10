using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   The statistics for <see cref="IStatsApi.RepositoryAsync"/>.
    /// </summary>
    public class RepositoryData
    {
        /// <summary>
        ///   The number of blocks in the repository.
        /// </summary>
        public ulong NumObjects;

        /// <summary>
        ///   The total number bytes in the repository.
        /// </summary>
        public ulong RepoSize;

        /// <summary>
        ///   The fully qualified path to the repository.
        /// </summary>
        public string RepoPath;

        /// <summary>
        ///   The version number of the repository.
        /// </summary>
        public string Version;

        /// <summary>
        ///   The maximum number of bytes of the repository.
        /// </summary>
        public ulong StorageMax;

    }
}
