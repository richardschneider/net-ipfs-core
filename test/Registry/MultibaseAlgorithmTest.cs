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
    public class MultiBaseAlgorithmTest
    {
        [TestMethod]
        public void Bad_Name()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => MultiBaseAlgorithm.Register(null, '?'));
            ExceptionAssert.Throws<ArgumentNullException>(() => MultiBaseAlgorithm.Register("", '?'));
            ExceptionAssert.Throws<ArgumentNullException>(() => MultiBaseAlgorithm.Register("   ", '?'));
        }

        [TestMethod]
        public void Name_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => MultiBaseAlgorithm.Register("base58btc", 'z'));
        }

        [TestMethod]
        public void Code_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => MultiBaseAlgorithm.Register("base58btc-x", 'z'));
        }

        [TestMethod]
        public void Algorithms_Are_Enumerable()
        {
            Assert.AreNotEqual(0, MultiBaseAlgorithm.All.Count());
        }

        [TestMethod]
        public void Roundtrip_All_Algorithms()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };

            foreach (var alg in MultiBaseAlgorithm.All)
            {
                var s = alg.Encode(bytes);
                CollectionAssert.AreEqual(bytes, alg.Decode(s), alg.Name);
            }
        }

        [TestMethod]
        public void Name_Is_Also_ToString()
        {
            foreach (var alg in MultiBaseAlgorithm.All)
            {
                Assert.AreEqual(alg.Name, alg.ToString());
            }
        }

        [TestMethod] 
        public void Known_But_NYI()
        {
            var alg = MultiBaseAlgorithm.Register("nyi", 'n');
            try
            {
                ExceptionAssert.Throws<NotImplementedException>(() => alg.Encode(null));
                ExceptionAssert.Throws<NotImplementedException>(() => alg.Decode(null));
            }
            finally
            {
                MultiBaseAlgorithm.Deregister(alg);
            }
        }
    }
}
