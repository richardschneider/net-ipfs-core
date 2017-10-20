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
            // Some platforms do not support SHA3
            try
            {
                var SHA3_512_Hash = "0EAB42DE4C3CEB9235FC91ACFFE746B29C29A8C366B7C60E4E67C466F36A4304C00FA9CAF9D87976BA469BCBE06713B435F091EF2769FB160CDAB33D3670680E";

                Assert.AreEqual(SHA3_512_Hash, MultiHash.GetHashAlgorithm("sha3-512").ComputeHash(new byte[0]).ToHexString("X"), "sha3-512");
            }
            catch (NotImplementedException)
            {
                // eat it
            }
        }
    }
}
