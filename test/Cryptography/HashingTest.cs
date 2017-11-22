using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ipfs.Cryptography
{
    [TestClass]
    public class HashingTest
    {
        class TestVector
        {
            public string Algorithm { get; set; }
            public string Input { get; set; }
            public string Digest { get; set; }
        }

        TestVector[] TestVectors = new TestVector[]
        {
            // From <see href="http://asecuritysite.com/encryption/sha3"/>
            new TestVector {
                Algorithm = "keccak-512",
                Input = "",
                Digest = "0eab42de4c3ceb9235fc91acffe746b29c29a8c366b7c60e4e67c466f36a4304c00fa9caf9d87976ba469bcbe06713b435f091ef2769fb160cdab33d3670680e"
            },

            // From https://csrc.nist.gov/Projects/Cryptographic-Algorithm-Validation-Program/Secure-Hashing#sha3vsha3vss
            //new TestVector {
            //    Algorithm = "sha3-512",
            //    Input = "37d518",
            //    Digest = "4aa96b1547e6402c0eee781acaa660797efe26ec00b4f2e0aec4a6d10688dd64cbd7f12b3b6c7f802e2096c041208b9289aec380d1a748fdfcd4128553d781e3"
            //},
        };

        /// <summary>
        ///   Test vectors for SHA-3 from various sources.
        /// </summary>
        [TestMethod]
        public void CheckHashes()
        {
            foreach (var v in TestVectors)
            {
                var actual = MultiHash
                    .GetHashAlgorithm(v.Algorithm)
                    .ComputeHash(v.Input.ToHexBuffer());
                Assert.AreEqual(v.Digest, actual.ToHexString(), v.Algorithm);
            }
        }
    }
}
