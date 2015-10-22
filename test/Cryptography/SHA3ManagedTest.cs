using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ipfs.Cryptography
{
    [TestClass]
    public class SHA3ManagedTest
    {
        /// <summary>
        ///   From <see href="http://asecuritysite.com/encryption/sha3"/>.
        /// </summary>
        [TestMethod]
        public void Hashing()
        {
            var SHA3_256_Hash = "C5D2460186F7233C927E7DB2DCC703C0E500B653CA82273B7BFAD8045D85A470";
            var SHA3_512_Hash = "0EAB42DE4C3CEB9235FC91ACFFE746B29C29A8C366B7C60E4E67C466F36A4304C00FA9CAF9D87976BA469BCBE06713B435F091EF2769FB160CDAB33D3670680E";

            Assert.AreEqual(SHA3_256_Hash, new SHA3.SHA3Managed(256).ComputeHash(new byte[0]).ToHexString("X"), "sha3-256");
            Assert.AreEqual(SHA3_512_Hash, new SHA3.SHA3Managed(512).ComputeHash(new byte[0]).ToHexString("X"), "sha3-512");
        }
    }
}
