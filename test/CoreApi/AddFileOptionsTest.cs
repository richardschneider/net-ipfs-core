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
        }
    }
}
