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
        public Peer Peer { get; set; }

        /// <summary>
        ///   The number of blocks sent by the <see cref="Peer"/> to us.
        /// </summary>
        /// <value>
        ///   The number of blocks.
        /// </value>
        public ulong BlocksReceived { get; set; }

        /// <summary>
        ///   The number of blocks sent by us to the <see cref="Peer"/>.
        /// </summary>
        /// <value>
        ///   The number of blocks.
        /// </value>
        public ulong BlocksSent { get; set; }

        /// <summary>
        ///   The number of bytes sent by the <see cref="Peer"/> to us.
        /// </summary>
        /// <value>
        ///   The number of bytes.
        /// </value>
        public ulong DataReceived { get; set; }


        /// <summary>
        ///   The number of bytes sent by us to the <see cref="Peer"/>
        /// </summary>
        /// <value>
        ///   The number of bytes.
        /// </value>
        public ulong DataSent { get; set; }

        /// <summary>
        ///   The calculated debt to the peer.
        /// </summary>
        /// <value>
        ///   <see cref="DataSent"/> divided by <see cref="DataReceived"/>.
        /// </value>
        public float DebtRatio
        {
            get
            {
                return (float)DataSent / (float)(DataReceived + 1); // +1 is to prevent division by zero
            }
        }

    }
}
