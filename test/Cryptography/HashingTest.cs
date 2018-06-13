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
            new TestVector
            {
                Algorithm = "sha1",
                Input = "68656c6c6f", // "hello" in hex
                Digest = "aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d"
            },

            // From https://bitcoin.stackexchange.com/questions/5671/how-do-you-perform-double-sha-256-encoding/5677
            new TestVector
            {
                Algorithm = "sha2-256",
                Input = "68656c6c6f", // "hello" in hex
                Digest = "2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824"
            },
            new TestVector
            {
                Algorithm = "dbl-sha2-256",
                Input = "68656c6c6f", // "hello" in hex
                Digest = "9595c9df90075148eb06860365df33584b75bff782a510c6cd4883a419833d50"
            },

            // From <see href="http://asecuritysite.com/encryption/sha3"/>
            new TestVector {
                Algorithm = "keccak-512",
                Input = "",
                Digest = "0eab42de4c3ceb9235fc91acffe746b29c29a8c366b7c60e4e67c466f36a4304c00fa9caf9d87976ba469bcbe06713b435f091ef2769fb160cdab33d3670680e"
            },

            // From https://csrc.nist.gov/Projects/Cryptographic-Algorithm-Validation-Program/Secure-Hashing#sha3vsha3vss
            new TestVector {
                Algorithm = "sha3-512",
                Input = "",
                Digest = "a69f73cca23a9ac5c8b567dc185a756e97c982164fe25859e0d1dcc1475c80a615b2123af1f5f94c11e3e9402c3ac558f500199d95b6d3e301758586281dcd26"
            },
            new TestVector {
                Algorithm = "sha3-512",
                Input = "37d518",
                Digest = "4aa96b1547e6402c0eee781acaa660797efe26ec00b4f2e0aec4a6d10688dd64cbd7f12b3b6c7f802e2096c041208b9289aec380d1a748fdfcd4128553d781e3"
            },
            new TestVector
            {
                Algorithm = "shake-128",
                Input = "",
                Digest = "7f9c2ba4e88f827d616045507605853e"
            },
            new TestVector
            {
                Algorithm = "shake-128",
                Input = "0e",
                Digest = "fa996dafaa208d72287c23bc4ed4bfd5"
            },
            new TestVector
            {
                Algorithm = "shake-128",
                Input = "fd6dd3b63dc7b9664895c51fc17c57d59c349621dd3c5694a3cc404c660c2cc47d83d2f0e3d2a28a3aa2f0a710db54",
                Digest = "c8db32bf81bf75621db30264750954f8"
            },

            // From https://en.wikipedia.org/wiki/MD4
            new TestVector
            {
                Algorithm = "md4",
                Input = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog").ToHexString(),
                Digest = "1bee69a46ba811185c194762abaeae90",
            },

            // From https://en.wikipedia.org/wiki/MD5
            new TestVector
            {
                Algorithm = "md5",
                Input = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog").ToHexString(),
                Digest = "9e107d9d372bb6826bd81d3542a419d6",
            },
            new TestVector
            {
                Algorithm = "identity",
                Input = "",
                Digest = ""
            },
            new TestVector
            {
                Algorithm = "identity",
                Input = "00",
                Digest = "00"
            },
            new TestVector
            {
                Algorithm = "identity",
                Input = "0001",
                Digest = "0001"
            },
        };

        /// <summary>
        ///   Test vectors from various sources.
        /// </summary>
        [TestMethod]
        public void CheckHashes()
        {
            foreach (var v in TestVectors)
            {
                var actual = MultiHash
                    .GetHashAlgorithm(v.Algorithm)
                    .ComputeHash(v.Input.ToHexBuffer());
                Assert.AreEqual(v.Digest, actual.ToHexString(), $"{v.Algorithm} for '{v.Input}'");
            }
        }
    }
}
