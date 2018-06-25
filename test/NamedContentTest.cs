using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Google.Protobuf;

namespace Ipfs
{
    [TestClass]
    public class NamedContentTest
    {
        [TestMethod]
        public void Properties()
        {
            var nc = new NamedContent
            {
                ContentPath = "/ipfs/...",
                NamePath = "/ipns/..."
            };
            Assert.AreEqual("/ipfs/...", nc.ContentPath);
            Assert.AreEqual("/ipns/...", nc.NamePath);
        }

    }
}
