using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Ipfs
{
    [TestClass]
    public class CidTest
    {
        [TestMethod]
        public void HumanReadable()
        {
            var cid = new Cid { Hash = new MultiHash("QmXg9Pp2ytZ14xgmQjYEiHjVjMFXzCVVEcRTWJBmLgR39V") };
            Assert.AreEqual("base58btc cidv0 dag-pb sha2-256 QmXg9Pp2ytZ14xgmQjYEiHjVjMFXzCVVEcRTWJBmLgR39V", cid.ToString());
        }

        [TestMethod]
        public void MultiHash_is_Cid_V0()
        {
            var mh = new MultiHash("QmXg9Pp2ytZ14xgmQjYEiHjVjMFXzCVVEcRTWJBmLgR39V");
            Cid cid = mh;
            Assert.AreEqual(0, cid.Version);
            Assert.AreEqual("dag-pb", cid.ContentType);
            Assert.AreEqual("base58btc", cid.Encoding);
            Assert.AreSame(mh, cid.Hash);
        }

        [TestMethod]
        public void MultiHash_is_Cid_V1()
        {
            var hello = Encoding.UTF8.GetBytes("Hello, world.");
            var mh = MultiHash.ComputeHash(hello, "sha2-512");
            Cid cid = mh;
            Assert.AreEqual(1, cid.Version);
            Assert.AreEqual("dag-pb", cid.ContentType);
            Assert.AreEqual("base58btc", cid.Encoding);
            Assert.AreSame(mh, cid.Hash);
        }

        [TestMethod]
        public void Encode_V0()
        {
            var hash = "QmXg9Pp2ytZ14xgmQjYEiHjVjMFXzCVVEcRTWJBmLgR39V";
            Cid cid = new MultiHash(hash);
            Assert.AreEqual(hash, cid.Encode());
        }

        [TestMethod]
        public void Decode_V0()
        {
            var hash = "QmXg9Pp2ytZ14xgmQjYEiHjVjMFXzCVVEcRTWJBmLgR39V";
            var cid = Cid.Decode(hash);
            Assert.AreEqual(0, cid.Version);
            Assert.AreEqual("dag-pb", cid.ContentType);
            Assert.AreEqual("base58btc", cid.Encoding);
            Assert.AreEqual(hash, cid.Encode());
        }

    }
}
