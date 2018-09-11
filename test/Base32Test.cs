using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ipfs
{
    [TestClass]
    public class Base32EncodeTests
    {
        byte[] getStringBytes(string x)
        {
            return Encoding.ASCII.GetBytes(x);
        }

        [TestMethod]
        public void Vector1()
        {
            Assert.AreEqual(string.Empty, Base32.Encode(getStringBytes(string.Empty)));
        }

        [TestMethod]
        public void Vector2()
        {
            Assert.AreEqual("my", Base32.Encode(getStringBytes("f")));
        }

        [TestMethod]
        public void Vector3()
        {
            Assert.AreEqual("mzxq", Base32.Encode(getStringBytes("fo")));
        }

        [TestMethod]
        public void Vector4()
        {
            Assert.AreEqual("mzxw6", Base32.Encode(getStringBytes("foo")));
        }

        [TestMethod]
        public void Vector5()
        {
            Assert.AreEqual("mzxw6yq", Base32.Encode(getStringBytes("foob")));
        }

        [TestMethod]
        public void Vector6()
        {
            Assert.AreEqual("mzxw6ytb", Base32.Encode(getStringBytes("fooba")));
        }

        [TestMethod]
        public void Vector7()
        {
            Assert.AreEqual("mzxw6ytboi", Base32.Encode(getStringBytes("foobar")));
        }
    }

    [TestClass]
    public class Base32DecodeTests
    {
        byte[] getStringBytes(string x)
        {
            return Encoding.ASCII.GetBytes(x);
        }

        [TestMethod]
        public void Vector1()
        {
            CollectionAssert.AreEqual(getStringBytes(string.Empty), Base32.Decode(string.Empty));
        }

        [TestMethod]
        public void Vector2()
        {
            CollectionAssert.AreEqual(getStringBytes("f"), Base32.Decode("MY======"));
        }

        [TestMethod]
        public void Vector3()
        {
            CollectionAssert.AreEqual(getStringBytes("fo"), Base32.Decode("MZXQ===="));
        }

        [TestMethod]
        public void Vector4()
        {
            CollectionAssert.AreEqual(getStringBytes("foo"), Base32.Decode("MZXW6==="));
        }

        [TestMethod]
        public void Vector5()
        {
            CollectionAssert.AreEqual(getStringBytes("foob"), Base32.Decode("MZXW6YQ="));
        }

        [TestMethod]
        public void Vector6()
        {
            CollectionAssert.AreEqual(getStringBytes("fooba"), Base32.Decode("MZXW6YTB"));
        }

        [TestMethod]
        public void Vector7()
        {
            CollectionAssert.AreEqual(getStringBytes("foobar"), Base32.Decode("MZXW6YTBOI======"));
        }
    }
}
