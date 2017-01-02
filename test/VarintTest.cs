using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Ipfs
{
    [TestClass]
    public class VarintTest
    {
        [TestMethod]
        public void Zero()
        {
            var x = new byte[] { 0 };
            Assert.AreEqual(1, Varint.RequiredBytes(0));
            CollectionAssert.AreEqual(x, Varint.Encode(0));
            Assert.AreEqual(0, Varint.DecodeInt32(x));
        }
        
        [TestMethod]
        public void ThreeHundred()
        {
            var x = new byte[] { 0xAC, 0x02 };
            Assert.AreEqual(2, Varint.RequiredBytes(300));
            CollectionAssert.AreEqual(x, Varint.Encode(300));
            Assert.AreEqual(300, Varint.DecodeInt32(x));
        }

        [TestMethod]
        public void Decode_From_Offset()
        {
            var x = new byte[] { 0x00, 0xAC, 0x02 };
            Assert.AreEqual(300, Varint.DecodeInt32(x, 1));
        }

        [TestMethod]
        public void MaxLong()
        {
            var x = "ffffffffffffffff7f".ToHexBuffer();
            Assert.AreEqual(9, Varint.RequiredBytes(long.MaxValue));
            CollectionAssert.AreEqual(x, Varint.Encode(long.MaxValue));
            Assert.AreEqual(long.MaxValue, Varint.DecodeInt64(x));
        }

        [TestMethod]
        public void Encode_Negative()
        {
            ExceptionAssert.Throws<NotSupportedException>(() => Varint.Encode(-1));
        }

        [TestMethod]
        public void TooBig_Int32()
        {
            var bytes = Varint.Encode((long)Int32.MaxValue + 1);
            ExceptionAssert.Throws<InvalidDataException>(() => Varint.DecodeInt32(bytes));
        }

        [TestMethod]
        public void TooBig_Int64()
        {
            var bytes = "ffffffffffffffffff7f".ToHexBuffer();
            ExceptionAssert.Throws<InvalidDataException>(() => Varint.DecodeInt64(bytes));
        }

        [TestMethod]
        public void Unterminated()
        {
            var bytes = "ff".ToHexBuffer();
            ExceptionAssert.Throws<InvalidDataException>(() => Varint.DecodeInt64(bytes));
        }

        [TestMethod]
        public void Empty()
        {
            var bytes = new byte[0];
            ExceptionAssert.Throws<InvalidDataException>(() => Varint.DecodeInt64(bytes));
        }
    }
}
