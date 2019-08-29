using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Information on a DAG node.
    /// </summary>
    /// <seealso cref="IObjectApi"/>
    public class ObjectStat
    {
        /// <summary>
        ///   Number of links.
        /// </summary>
        public int LinkCount { get; set; }

        /// <summary>
        ///   Size of the links segment.
        /// </summary>
        public long LinkSize { get; set; }

        /// <summary>
        ///   Size of the raw, encoded data.
        /// </summary>
        public long BlockSize { get; set; }

        /// <summary>
        ///   Siz of the data segment.
        /// </summary>
        public long DataSize { get; set; }

        /// <summary>
        ///   Size of object and its references
        /// </summary>
        public long CumulativeSize { get; set; }
    }
}
