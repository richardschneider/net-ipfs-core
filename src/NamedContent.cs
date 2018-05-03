using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   Content that has an associated name.
    /// </summary>
    /// <seealso cref="CoreApi.INameApi"/>
    public class NamedContent
    {
        /// <summary>
        ///   Path to the name.
        /// </summary>
        /// <value>
        ///   Typically <c>/ipns/...</c>.
        /// </value>
        public string NamePath { get; set; }

        /// <summary>
        ///   Path to the content.
        /// </summary>
        /// <value>
        ///   Typically <c>/ipfs/...</c>.
        /// </value>
        public string ContentPath { get; set; }
    }
}
