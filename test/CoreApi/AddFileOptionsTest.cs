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
        }
    }
}
