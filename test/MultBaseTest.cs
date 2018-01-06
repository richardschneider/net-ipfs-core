using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ipfs
{
    [TestClass]
    public class MultiBaseTest
    {
        [TestMethod]
        public void Codec()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            var bytes1 = MultiBase.Decode(MultiBase.Encode(bytes));
            var bytes2 = MultiBase.Decode(MultiBase.Encode(bytes, "base16"));
            CollectionAssert.AreEqual(bytes, bytes1);
            CollectionAssert.AreEqual(bytes, bytes2);
        }

        [TestMethod]
        public void Encode_Unknown_Algorithm()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            ExceptionAssert.Throws<KeyNotFoundException>(() => MultiBase.Encode(bytes, "unknown"));
        }

        [TestMethod]
        public void Decode_Bad_Formats()
        {
            ExceptionAssert.Throws<FormatException>(() => MultiBase.Decode("?"));
            ExceptionAssert.Throws<FormatException>(() => MultiBase.Decode("??"));
            ExceptionAssert.Throws<FormatException>(() => MultiBase.Decode("???"));
            ExceptionAssert.Throws<FormatException>(() => MultiBase.Decode("fXX"));
        }
    }
}
