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
        /// <value>
        ///   The number of blocks in the <see cref="IBlockRepositoryApi">repository</see>.
        /// </value>
        public ulong NumObjects;

        /// <summary>
        ///   The total number bytes in the repository.
        /// </summary>
        /// <value>
        ///   The total number bytes in the <see cref="IBlockRepositoryApi">repository</see>.
        /// </value>
        public ulong RepoSize;

        /// <summary>
        ///   The fully qualified path to the repository.
        /// </summary>
        /// <value>
        ///   The directory of the <see cref="IBlockRepositoryApi">repository</see>.
        /// </value>
        public string RepoPath;

        /// <summary>
        ///   The version number of the repository.
        /// </summary>
        /// <value>
        ///  The version number of the <see cref="IBlockRepositoryApi">repository</see>.
        /// </value>
        public string Version;

        /// <summary>
        ///   The maximum number of bytes allowed in the repository.
        /// </summary>
        /// <value>
        ///  Max bytes allowed in the <see cref="IBlockRepositoryApi">repository</see>.
        /// </value>
        public ulong StorageMax;

    }
}
