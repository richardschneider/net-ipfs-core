using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipfs
{
    /// <summary>
    ///   Some data that is stored in IPFS.
    /// </summary>
    /// <remarks>
    ///   A <b>DataBlock</b> has an <see cref="Id">unique ID</see>
    ///   and some data (<see cref="IDataBlock.DataBytes"/> 
    ///   or <see cref="IDataBlock.DataStream"/>).
    ///   <para>
    ///   It is useful to talk about them as "blocks" in Bitswap 
    ///   and other things that do not care about what is being stored.
    ///   </para>
    /// </remarks>
    /// <seealso cref="IMerkleNode{Link}"/>
    public interface IDataBlock
    {

        /// <summary>
        ///   Contents as a byte array.
        /// </summary>
        /// <remarks>
        ///   It is never <b>null</b>.
        /// </remarks>
        /// <value>
        ///   The contents as a sequence of bytes.
        /// </value>
        byte[] DataBytes { get; }

        /// <summary>
        ///   Contents as a stream of bytes.
        /// </summary>
        /// <value>
        ///   The contents as a stream of bytes.
        /// </value>
        Stream DataStream { get; }

        /// <summary>
        ///   The unique ID of the data.
        /// </summary>
        /// <value>
        ///   A <see cref="Cid"/> of the content.
        /// </value>
        Cid Id { get; }

    }

}
