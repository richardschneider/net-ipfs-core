using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   The options when adding data to the IPFS file system.
    /// </summary>
    /// <seealso cref="IFileSystemApi"/>
    public class AddFileOptions
    {
        /// <summary>
        ///   Determines if the data is pinned to local storage.
        /// </summary>
        /// <value>
        ///   If <b>true</b> the data is pinned to local storage and will not be
        ///   garbage collected.  The default is <b>true</b>.
        /// </value>
        public bool Pin { get; set; } = true;

        /// <summary>
        ///   The maximum number of data bytes in a block.
        /// </summary>
        /// <value>
        ///   The default is 256 * 1024 (‭262,144) bytes.‬
        /// </value>
        public int ChunkSize { get; set; } = 256 * 1024;

        /// <summary>
        ///   Determines if the trickle-dag format is used for dag generation.
        /// </summary>
        /// <value>
        ///   The default is <b>false</b>.
        /// </value>
        public bool Trickle { get; set; } = false;

        /// <summary>
        ///   Determines if added file(s) are wrapped in a directory object.
        /// </summary>
        /// <value>
        ///   The default is <b>false</b>.
        /// </value>
        public bool Wrap { get; set; } = false;

        /// <summary>
        ///   Determines if raw blocks are used for leaf data blocks.
        /// </summary>
        /// <value>
        ///   The default is <b>false</b>.
        /// </value>
        /// <remarks>
        ///   <b>RawLeaves</b> and <see cref="ProtectionKey"/> are mutually exclusive.
        /// </remarks>
        public bool RawLeaves { get; set; } = false;

        /// <summary>
        ///   The hashing algorithm name to use.
        /// </summary>
        /// <value>
        ///   The <see cref="MultiHash"/> algorithm name used to produce the <see cref="Cid"/>.
        ///   Defaults to <see cref="MultiHash.DefaultAlgorithmName"/>.
        /// </value>
        /// <seealso cref="MultiHash"/>
        public string Hash { get; set; } = MultiHash.DefaultAlgorithmName;

        /// <summary>
        ///   The encoding algorithm name to use.
        /// </summary>
        /// <value>
        ///   The <see cref="MultiBase"/> algorithm name used to produce the <see cref="Cid"/>.
        ///   Defaults to <see cref="MultiBase.DefaultAlgorithmName"/>.
        /// </value>
        /// <seealso cref="MultiBase"/>
        public string Encoding { get; set; } = MultiBase.DefaultAlgorithmName;

        /// <summary>
        ///   Determines if only file information is produced.
        /// </summary>
        /// <value>
        ///   If <b>true</b> no data is added to IPFS.  The default is <b>false</b>.
        /// </value>
        public bool OnlyHash { get; set; } = false;

        /// <summary>
        ///   The key name used to protect (encrypt) the file contents.
        /// </summary>
        /// <value>
        ///   The name of an existing key.
        /// </value>
        /// <remarks>
        ///   <b>ProtectionKey</b> and <see cref="RawLeaves"/> are mutually exclusive.
        /// </remarks>
        /// <seealso cref="IKeyApi"/>
        public string ProtectionKey { get; set; }

        /// <summary>
        ///   Used to report the progress of a file transfer.
        /// </summary>
        public IProgress<TransferProgress> Progress { get; set; }
    }
}
