using Ipfs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Google.Protobuf;

namespace Ipfs
{
    [TestClass]
    public class NetworkProtocolTest
    {
        [TestMethod]
        public void Stringing()
        {
            Assert.AreEqual("/tcp/8080", new MultiAddress("/tcp/8080").Protocols[0].ToString());
        }



        [TestMethod]
        public void Register_Name_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => NetworkProtocol.Register<NameExists>());
        }

        [TestMethod]
        public void Register_Code_Already_Exists()
        {
            ExceptionAssert.Throws<ArgumentException>(() => NetworkProtocol.Register<CodeExists>());
        }

        class NameExists : NetworkProtocol
        {
            public override string Name { get { return "tcp"; } }
            public override uint Code { get { return 0x7FFF; } }
            public override void ReadValue(CodedInputStream stream) { }
            public override void ReadValue(TextReader stream) { }
            public override void WriteValue(CodedOutputStream stream) { }
        }

        class CodeExists : NetworkProtocol
        {
            public override string Name { get { return "x-tcp"; } }
            public override uint Code { get { return 6; } }
            public override void ReadValue(CodedInputStream stream) { }
            public override void ReadValue(TextReader stream) { }
            public override void WriteValue(CodedOutputStream stream) { }
        }

    }
}
