using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ipfs
{
    [TestClass]
    public class Base58Test
    {
        [TestMethod]
        public void Encode()
        {
            Assert.AreEqual("jo91waLQA1NNeBmZKUF", Base58.Encode(Encoding.UTF8.GetBytes("this is a test")));
        }

        [TestMethod]
        public void Decode()
        {
            Assert.AreEqual("this is a test", Encoding.UTF8.GetString(Base58.Decode("jo91waLQA1NNeBmZKUF")));
        }
    }
}
