using Ipfs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ipfs
{
    [TestClass]
    public class MultiHashTest
    {
        [TestMethod]
        public void HashNames()
        {
            var mh = new MultiHash("sha1", new byte[20]);
            mh = new MultiHash("sha2-256", new byte[32]);
            mh = new MultiHash("sha2-512", new byte[64]);
        }

        [TestMethod]
        public void Unknown_Hash_Name()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => new MultiHash(null, new byte[0]));
            ExceptionAssert.Throws<ArgumentException>(() => new MultiHash("", new byte[0]));
            ExceptionAssert.Throws<ArgumentException>(() => new MultiHash("md5", new byte[0]));
        }

        [TestMethod]
        public void Invalid_Digest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => new MultiHash("sha1", null));
            ExceptionAssert.Throws<ArgumentException>(() => new MultiHash("sha1", new byte[0]));
            ExceptionAssert.Throws<ArgumentException>(() => new MultiHash("sha1", new byte[21]));
        }

        [TestMethod]
        public void Base58_Encode_Decode()
        {
            var mh = new MultiHash("QmPZ9gcCEpqKTo6aq61g2nXGUhM4iCL3ewB6LDXZCtioEB");
            Assert.AreEqual("sha2-256", mh.Algorithm.Name);
            Assert.AreEqual(32, mh.Digest.Length);
            Assert.AreEqual("QmPZ9gcCEpqKTo6aq61g2nXGUhM4iCL3ewB6LDXZCtioEB", mh.ToBase58());
        }

        [TestMethod]
        public void Compute_Hash_Array()
        {
            var hello = Encoding.UTF8.GetBytes("Hello, world.");
            var mh = MultiHash.ComputeHash(hello);
            Assert.AreEqual(MultiHash.DefaultAlgorithmName, mh.Algorithm.Name);
            Assert.IsNotNull(mh.Digest);
        }

        [TestMethod]
        public void Compute_Not_Implemented_Hash_Array()
        {
            MultiHash.HashingAlgorithm.Define("not-implemented", 0x0F, 32);
            var hello = Encoding.UTF8.GetBytes("Hello, world.");
            ExceptionAssert.Throws<NotImplementedException>(() => MultiHash.ComputeHash(hello, "not-implemented"));
        }

        [TestMethod]
        public void Matches_Array()
        {
            var hello = Encoding.UTF8.GetBytes("Hello, world.");
            var hello1 = Encoding.UTF8.GetBytes("Hello, world");
            var mh = MultiHash.ComputeHash(hello);
            Assert.IsTrue(mh.Matches(hello));
            Assert.IsFalse(mh.Matches(hello1));
        }

    }
}
