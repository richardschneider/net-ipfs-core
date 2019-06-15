using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ipfs.CoreApi
{
    [TestClass]
    public class BitswapLedgerTest
    {
        [TestMethod]
        public void Defaults()
        {
            var ledger = new BitswapLedger();

            Assert.IsNull(ledger.Peer);
            Assert.AreEqual(0ul, ledger.BlocksReceived);
            Assert.AreEqual(0ul, ledger.BlocksSent);
            Assert.AreEqual(0ul, ledger.DataReceived);
            Assert.AreEqual(0ul, ledger.DataSent);
        }


    }
}
