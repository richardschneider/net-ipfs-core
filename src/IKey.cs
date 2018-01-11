using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   Information about a cryptographic key.
    /// </summary>
    public interface IKey
    {
        /// <summary>
        ///   Unique identifier.
        /// </summary>
        /// <value>
        ///   The <see cref="MultiHash"/> of the key's public key.
        /// </value>
        MultiHash Id { get; }

        /// <summary>
        ///   The locally assigned name to the key.
        /// </summary>
        /// <value>
        ///   The name is only unique within the local peer node. The
        ///   <see cref="Id"/> is universally unique.
        /// </value>
        string Name { get; }
    }
}
