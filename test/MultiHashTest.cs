using Ipfs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        public void Parsing_Unknown_Hash_Number()
        {
            MultiHash.HashingAlgorithm unknown = null;
            EventHandler<UnknownHashingAlgorithmEventArgs> unknownHandler = (s, e) => { unknown = e.Algorithm; };
            var ms = new MemoryStream(new byte[] { 0x01, 0x02, 0x0a, 0x0b });
            MultiHash.UnknownHashingAlgorithm += unknownHandler;
            try
            {
                var mh = new MultiHash(ms);
                Assert.AreEqual("ipfs-1", mh.Algorithm.Name);
                Assert.AreEqual("ipfs-1", mh.Algorithm.ToString());
                Assert.AreEqual(1, mh.Algorithm.Number);
                Assert.AreEqual(2, mh.Algorithm.DigestSize);
                Assert.AreEqual(0xa, mh.Digest[0]);
                Assert.AreEqual(0xb, mh.Digest[1]);
                Assert.IsNotNull(unknown, "unknown handler not called");
                Assert.AreEqual("ipfs-1", unknown.Name);
                Assert.AreEqual(1, unknown.Number);
                Assert.AreEqual(2, unknown.DigestSize);
            }
            finally
            {
                MultiHash.UnknownHashingAlgorithm -= unknownHandler;
            }
        }

        [TestMethod]
        public void Parsing_Wrong_Digest_Size()
        {
            var ms = new MemoryStream(new byte[] { 0x11, 0x02, 0x0a, 0x0b });
            ExceptionAssert.Throws<InvalidDataException>(() => new MultiHash(ms));
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
        public void Compute_Hash_Stream()
        {
            var hello = new MemoryStream(Encoding.UTF8.GetBytes("Hello, world."));
            hello.Position = 0;
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

            var mh1 = MultiHash.ComputeHash(hello, "sha1");
            Assert.IsTrue(mh1.Matches(hello));
            Assert.IsFalse(mh1.Matches(hello1));

            var mh2 = MultiHash.ComputeHash(hello, "sha2-512");
            Assert.IsTrue(mh2.Matches(hello));
            Assert.IsFalse(mh2.Matches(hello1));
        }

        [TestMethod]
        public void Matches_Stream()
        {
            var hello = new MemoryStream(Encoding.UTF8.GetBytes("Hello, world."));
            var hello1 = new MemoryStream(Encoding.UTF8.GetBytes("Hello, world"));
            hello.Position = 0;
            var mh = MultiHash.ComputeHash(hello);

            hello.Position = 0;
            Assert.IsTrue(mh.Matches(hello));

            hello1.Position = 0;
            Assert.IsFalse(mh.Matches(hello1));
        }

        [TestMethod]
        public void HashingAlgorithm_Bad_Name()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => MultiHash.HashingAlgorithm.Define(null, 1, 1));
            ExceptionAssert.Throws<ArgumentNullException>(() => MultiHash.HashingAlgorithm.Define("", 1, 1));
            ExceptionAssert.Throws<ArgumentNullException>(() => MultiHash.HashingAlgorithm.Define("   ", 1, 1));
        }

        [TestMethod]
        public void HashingAlgorithm_Name_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => MultiHash.HashingAlgorithm.Define("sha1", 0x11, 1));
        }

        [TestMethod]
        public void HashingAlgorithm_Number_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => MultiHash.HashingAlgorithm.Define("sha1-x", 0x11, 1));
        }

        [TestMethod]
        public void HashingAlgorithms_Are_Enumerable()
        {
            Assert.IsTrue(5 <= MultiHash.HashingAlgorithm.All.Count());
        }

    }
}
