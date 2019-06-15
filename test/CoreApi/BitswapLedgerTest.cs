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
            Assert.AreEqual(0f, ledger.DebtRatio);
        }

        [TestMethod]
        public void DebtRatio_Positive()
        {
            var ledger = new BitswapLedger { DataSent = 1024 * 1024 };
            Assert.IsTrue(ledger.DebtRatio >= 1);
        }

        [TestMethod]
        public void DebtRatio_Negative()
        {
            var ledger = new BitswapLedger { DataReceived = 1024 * 1024 };
            Assert.IsTrue(ledger.DebtRatio < 1);
        }
    }
}
