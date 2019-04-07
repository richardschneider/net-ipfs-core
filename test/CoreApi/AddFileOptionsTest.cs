using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ipfs.CoreApi
{
    [TestClass]
    public class AddFileOptionsTests
    {
        [TestMethod]
        public void Defaults()
        {
            var options = new AddFileOptions();

            Assert.AreEqual(true, options.Pin);
            Assert.AreEqual(256 * 1024, options.ChunkSize);
            Assert.AreEqual(MultiHash.DefaultAlgorithmName, options.Hash);
            Assert.AreEqual(false, options.OnlyHash);
            Assert.AreEqual(false, options.RawLeaves);
            Assert.AreEqual(false, options.Trickle);
            Assert.AreEqual(false, options.Wrap);
            Assert.IsNull(options.Progress);
            Assert.IsNull(options.ProtectionKey);
        }

        [TestMethod]
        public void Setting()
        {
            var options = new AddFileOptions
            {
                Pin = false,
                ChunkSize = 2 * 1024,
                Hash = "sha2-512",
                OnlyHash = true,
                RawLeaves = true,
                Progress = new Progress<TransferProgress>(),
                Trickle = true,
                Wrap = true,
                ProtectionKey = "secret"
            };

            Assert.AreEqual(false, options.Pin);
            Assert.AreEqual(2 * 1024, options.ChunkSize);
            Assert.AreEqual("sha2-512", options.Hash);
            Assert.AreEqual(true, options.OnlyHash);
            Assert.AreEqual(true, options.RawLeaves);
            Assert.AreEqual(true, options.Trickle);
            Assert.AreEqual(true, options.Wrap);
            Assert.IsNotNull(options.Progress);
            Assert.AreEqual("secret", options.ProtectionKey);
        }

    }
}
