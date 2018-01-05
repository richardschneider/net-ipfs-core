using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Google.Protobuf;

namespace Ipfs.Registry
{
    [TestClass]
    public class HashingAlgorithmTest
    {
        [TestMethod]
        public void HashingAlgorithm_Bad_Name()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => HashingAlgorithm.Register(null, 1, 1));
            ExceptionAssert.Throws<ArgumentNullException>(() => HashingAlgorithm.Register("", 1, 1));
            ExceptionAssert.Throws<ArgumentNullException>(() => HashingAlgorithm.Register("   ", 1, 1));
        }

        [TestMethod]
        public void HashingAlgorithm_Name_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => HashingAlgorithm.Register("sha1", 0x11, 1));
        }

        [TestMethod]
        public void HashingAlgorithm_Number_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => HashingAlgorithm.Register("sha1-x", 0x11, 1));
        }

        [TestMethod]
        public void HashingAlgorithms_Are_Enumerable()
        {
            Assert.IsTrue(5 <= HashingAlgorithm.All.Count());
        }

    }
}
