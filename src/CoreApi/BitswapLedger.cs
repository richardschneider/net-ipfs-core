using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Statistics on the <see cref="IBitswapApi">bitswap</see> blocks exchanged with another <see cref="Peer"/>.
    /// </summary>
    /// <seealso cref="IBitswapApi.LedgerAsync"/>
    public class BitswapLedger
    {
        /// <summary>
        ///   The <see cref="Peer"/> that pertains to this ledger.
        /// </summary>
        /// <value>
        ///   The peer that is being monitored.
        /// </value>
        public Peer Peer;

        /// <summary>
        ///   The number of blocks sent by the <see cref="Peer"/> to us.
        /// </summary>
        /// <value>
        ///   The number of blocks.
        /// </value>
        public ulong BlocksReceived;

        /// <summary>
        ///   The number of blocks sent by us to the <see cref="Peer"/>.
        /// </summary>
        /// <value>
        ///   The number of blocks.
        /// </value>
        public ulong BlocksSent;

        /// <summary>
        ///   The number of bytes sent by the <see cref="Peer"/> to us.
        /// </summary>
        /// <value>
        ///   The number of bytes.
        /// </value>
        public ulong DataReceived;


        /// <summary>
        ///   The number of bytes sent by us to the <see cref="Peer"/>
        /// </summary>
        /// <value>
        ///   The number of bytes.
        /// </value>
        public ulong DataSent;

    }
}
