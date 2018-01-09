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
    public class DagNodeTest
    {
        [TestMethod]
        public void EmptyDAG()
        {
            var node = new DagNode((byte[]) null);
            Assert.AreEqual(0, node.DataBytes.Length);
            Assert.AreEqual(0, node.Links.Count());
            Assert.AreEqual(0, node.Size);
            Assert.AreEqual("QmdfTbBqBPQ7VNxZEYEj14VmRuZBkqFbiwReogJgS1zR1n", (string)node.Id);

            RoundtripTest(node);
        }

        [TestMethod]
        public void DataOnlyDAG()
        {
            var abc = Encoding.UTF8.GetBytes("abc");
            var node = new DagNode(abc);
            CollectionAssert.AreEqual(abc, node.DataBytes);
            Assert.AreEqual(0, node.Links.Count());
            Assert.AreEqual("QmXg9Pp2ytZ14xgmQjYEiHjVjMFXzCVVEcRTWJBmLgR39V", (string)node.Id);
            Assert.AreEqual(5, node.Size);

            RoundtripTest(node);
        }

        [TestMethod]
        public void LinkOnlyDAG()
        {
            var a = Encoding.UTF8.GetBytes("a");
            var anode = new DagNode(a);
            var alink = anode.ToLink("a");

            var node = new DagNode(null, new[] { alink });
            Assert.AreEqual(0, node.DataBytes.Length);
            Assert.AreEqual(1, node.Links.Count());
            Assert.AreEqual("QmVdMJFGTqF2ghySAmivGiQvsr9ZH7ujnNGBkLNNCe4HUE", (string)node.Id);
            Assert.AreEqual(43, node.Size);

            RoundtripTest(node);
        }

        [TestMethod]
        public void MultipleLinksOnlyDAG()
        {
            var a = Encoding.UTF8.GetBytes("a");
            var anode = new DagNode(a);
            var alink = anode.ToLink("a");

            var b = Encoding.UTF8.GetBytes("b");
            var bnode = new DagNode(b);
            var blink = bnode.ToLink("b");

            var node = new DagNode(null, new[] { alink, blink });
            Assert.AreEqual(0, node.DataBytes.Length);
            Assert.AreEqual(2, node.Links.Count());
            Assert.AreEqual("QmbNgNPPykP4YTuAeSa3DsnBJWLVxccrqLUZDPNQfizGKs", (string)node.Id);

            RoundtripTest(node);
        }

        [TestMethod]
        public void MultipleLinksDataDAG()
        {
            var a = Encoding.UTF8.GetBytes("a");
            var anode = new DagNode(a);
            var alink = anode.ToLink("a");

            var b = Encoding.UTF8.GetBytes("b");
            var bnode = new DagNode(b);
            var blink = bnode.ToLink("b");

            var ab = Encoding.UTF8.GetBytes("ab");
            var node = new DagNode(ab, new[] { alink, blink });
            CollectionAssert.AreEqual(ab, node.DataBytes);
            Assert.AreEqual(2, node.Links.Count());
            Assert.AreEqual("Qma5sYpEc9hSYdkuXpMDJYem95Mj7hbEd9C412dEQ4ZkfP", (string)node.Id);

            RoundtripTest(node);
        }

        [TestMethod]
        public void Links_are_Sorted()
        {
            var a = Encoding.UTF8.GetBytes("a");
            var anode = new DagNode(a);
            var alink = anode.ToLink("a");

            var b = Encoding.UTF8.GetBytes("b");
            var bnode = new DagNode(b);
            var blink = bnode.ToLink("b");

            var ab = Encoding.UTF8.GetBytes("ab");
            var node = new DagNode(ab, new[] { blink, alink });
            CollectionAssert.AreEqual(ab, node.DataBytes);
            Assert.AreEqual(2, node.Links.Count());
            Assert.AreEqual("Qma5sYpEc9hSYdkuXpMDJYem95Mj7hbEd9C412dEQ4ZkfP", (string)node.Id);
        }


        [TestMethod]
        public void Hashing_Algorithm()
        {
            var abc = Encoding.UTF8.GetBytes("abc");
            var node = new DagNode(abc, null, "sha2-512");
            CollectionAssert.AreEqual(abc, node.DataBytes);
            Assert.AreEqual(0, node.Links.Count());
            // Had to change the test, because sha2-512 always generates a CID V1.
            // Assert.AreEqual("8Vv347foTHxpLZDdguzcTp7mjBmySjWK1VF36Je7io4ZKHo28fefMFr28LSv757yTcaRzn1dRqPB6WWFpYvbd5YXca", (string)node.Id);
            Assert.AreEqual("zBunREkQwjzkZwU71RKBcv2n3XwkTehtzVLkeUPTKjtBDBs2pnGRHLcxt3bGqzX38BkuNom2bQquyPj5Fh7dBZV3UdXPU", (string)node.Id);
            Assert.AreEqual(5, node.Size);

            RoundtripTest(node);
        }

        [TestMethod]
        public void ToLink()
        {
            var abc = Encoding.UTF8.GetBytes("abc");
            var node = new DagNode(abc);
            var link = node.ToLink();
            Assert.AreEqual("", link.Name);
            Assert.AreEqual(node.Id, link.Id);
            Assert.AreEqual(node.Size, link.Size);
        }

        [TestMethod]
        public void ToLink_With_Name()
        {
            var abc = Encoding.UTF8.GetBytes("abc");
            var node = new DagNode(abc);
            var link = node.ToLink("abc");
            Assert.AreEqual("abc", link.Name);
            Assert.AreEqual(node.Id, link.Id);
            Assert.AreEqual(node.Size, link.Size);
        }

        [TestMethod]
        public void AddLink()
        {
            var a = Encoding.UTF8.GetBytes("a");
            var anode = new DagNode(a);

            var b = Encoding.UTF8.GetBytes("b");
            var bnode = new DagNode(b);

            var cnode = bnode.AddLink(anode.ToLink());
            Assert.IsFalse(Object.ReferenceEquals(bnode, cnode));
            Assert.AreEqual(1, cnode.DataBytes.Length);
            Assert.AreEqual(1, cnode.Links.Count());
            Assert.AreEqual(anode.Id, cnode.Links.First().Id);
            Assert.AreEqual(anode.Size, cnode.Links.First().Size);

            RoundtripTest(cnode);
        }

        [TestMethod]
        public void RemoveLink()
        {
            var a = Encoding.UTF8.GetBytes("a");
            var anode = new DagNode(a);

            var b = Encoding.UTF8.GetBytes("b");
            var bnode = new DagNode(b);

            var c = Encoding.UTF8.GetBytes("c");
            var cnode = new DagNode(c, new[] { anode.ToLink(), bnode.ToLink() });

            var dnode = cnode.RemoveLink(anode.ToLink());
            Assert.IsFalse(Object.ReferenceEquals(dnode, cnode));
            Assert.AreEqual(1, dnode.DataBytes.Length);
            Assert.AreEqual(1, dnode.Links.Count());
            Assert.AreEqual(bnode.Id, dnode.Links.First().Id);
            Assert.AreEqual(bnode.Size, dnode.Links.First().Size);

            RoundtripTest(cnode);
        }

        [TestMethod]
        public void Null_Stream()
        {
            ExceptionAssert.Throws(() => new DagNode((CodedInputStream)null));
            ExceptionAssert.Throws(() => new DagNode((Stream)null));
        }

        [TestMethod]
        public void Link_With_CID_V1()
        {
            var data = "124F0A4401551340309ECC489C12D6EB4CC40F50C902F2B4D0ED77EE511A7C7A9BCD3CA86D4CD86F989DD35BC5FF499670DA34255B45B0CFD830E81F605DCF7DC5542E93AE9CD76F120568656C6C6F180B0A020801"
                .ToHexBuffer();
            var ms = new MemoryStream(data, false);
            var node = new DagNode(ms);
            Assert.AreEqual("0801", node.DataBytes.ToHexString());
            Assert.AreEqual(1, node.Links.Count());
            var link = node.Links.First();
            Assert.AreEqual("hello", link.Name);
            Assert.AreEqual(1, link.Id.Version);
            Assert.AreEqual("raw", link.Id.ContentType);
            Assert.AreEqual("sha2-512", link.Id.Hash.Algorithm.Name);
            Assert.AreEqual(11, link.Size);
        }

        void RoundtripTest(DagNode a)
        {
            var ms = new MemoryStream();
            a.Write(ms);
            ms.Position = 0;
            var b = new DagNode(ms);
            CollectionAssert.AreEqual(a.DataBytes, b.DataBytes);
            CollectionAssert.AreEqual(a.ToArray(), b.ToArray());
            Assert.AreEqual(a.Links.Count(), b.Links.Count());
            a.Links.Zip(b.Links, (first, second) =>
            {
                Assert.AreEqual(first.Id, second.Id);
                Assert.AreEqual(first.Name, second.Name);
                Assert.AreEqual(first.Size, second.Size);
                return first;
            }).ToArray();

            using (var first = a.DataStream)
            using (var second = b.DataStream)
            {
                Assert.AreEqual(first.Length, second.Length);
                for (int i = 0; i < first.Length; ++i)
                {
                    Assert.AreEqual(first.ReadByte(), second.ReadByte());
                }
            }
        }
    }
}
