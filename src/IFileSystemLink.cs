using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///    A link to another file system node in IPFS.
    /// </summary>
    public interface IFileSystemLink : IMerkleLink
    {
        /// <summary>
        ///   Determines if the link is a directory (folder).
        /// </summary>
        /// <value>
        ///   <b>true</b> if the link is a directory; Otherwise <b>false</b>,
        ///   the link is some type of a file.
        /// </value>
        bool IsDirectory { get; }

    }
}
