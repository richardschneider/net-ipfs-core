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

        [TestMethod]
        public void HashingAlgorithm_Bad_Alias()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => HashingAlgorithm.RegisterAlias(null, "sha1"));
            ExceptionAssert.Throws<ArgumentNullException>(() => HashingAlgorithm.RegisterAlias("", "sha1"));
            ExceptionAssert.Throws<ArgumentNullException>(() => HashingAlgorithm.RegisterAlias("   ", "sha1"));
        }

        [TestMethod]
        public void HashingAlgorithm_Alias_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => HashingAlgorithm.RegisterAlias("id", "identity"));
        }

        [TestMethod]
        public void HashingAlgorithm_Alias_Target_Does_Not_Exist()
        {
            ExceptionAssert.Throws<ArgumentException>(() => HashingAlgorithm.RegisterAlias("foo", "sha1-x"));
        }

        [TestMethod]
        public void HashingAlgorithm_Alias_Target_Is_Bad()
        {
            ExceptionAssert.Throws<ArgumentException>(() => HashingAlgorithm.RegisterAlias("foo", "  "));
        }
    }
}
