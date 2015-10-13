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

        /// <summary>
        ///    C# version of base58Test in <see href="https://github.com/ipfs/java-ipfs-api/blob/master/test/org/ipfs/Test.java"/>
        /// </summary>
        [TestMethod]
        public void Java()
        {
            String input = "QmPZ9gcCEpqKTo6aq61g2nXGUhM4iCL3ewB6LDXZCtioEB";
            byte[] output = Base58.Decode(input);
            String encoded = Base58.Encode(output);
            Assert.AreEqual(input, encoded);
        }
    }
}
