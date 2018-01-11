using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Ipfs
{

    [TestClass]
    public class PeerTest
    {
        const string marsId = "QmSoLMeWqB7YGVLJN3pNLQpmmEk35v6wYtsMGLzSr5QBU3";
        const string plutoId = "QmSoLPppuBtQSGwKDZT2M73ULpjvfd3aZ6ha4oFGL1KrGM";
        const string marsPublicKey = "CAASogEwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAKGUtbRQf+a9SBHFEruNAUatS/tsGUnHuCtifGrlbYPELD3UyyhWf/FYczBCavx3i8hIPEW2jQv4ehxQxi/cg9SHswZCQblSi0ucwTBFr8d40JEiyB9CcapiMdFQxdMgGvXEOQdLz1pz+UPUDojkdKZq8qkkeiBn7KlAoGEocnmpAgMBAAE=";
        static string marsAddress = "/ip4/10.1.10.10/tcp/29087/ipfs/QmSoLMeWqB7YGVLJN3pNLQpmmEk35v6wYtsMGLzSr5QBU3";

        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("", new Peer().ToString());
            Assert.AreEqual(marsId, new Peer { Id = marsId }.ToString());
        }

        [TestMethod]
        public void DefaultValues()
        {
            var peer = new Peer();
            Assert.AreEqual(null, peer.Id);
            Assert.AreEqual(0, peer.Addresses.Count());
            Assert.AreEqual("unknown/0.0", peer.ProtocolVersion);
            Assert.AreEqual("unknown/0.0", peer.AgentVersion);
            Assert.AreEqual(null, peer.PublicKey);
            Assert.AreEqual(false, peer.IsValid()); // missing peer ID
            Assert.AreEqual(null, peer.ConnectedAddress);
            Assert.IsFalse(peer.Latency.HasValue);
        }

        [TestMethod]
        public void ConnectedPeer()
        {
            var peer = new Peer
            {
                ConnectedAddress = new MultiAddress(marsAddress),
                Latency = TimeSpan.FromHours(3.03 * 2)
            };
            Assert.AreEqual(marsAddress, peer.ConnectedAddress.ToString());
            Assert.AreEqual(3.03 * 2, peer.Latency.Value.TotalHours);
        }

        [TestMethod]
        public void Validation_No_Id()
        {
            var peer = new Peer();
            Assert.AreEqual(false, peer.IsValid());
        }

        [TestMethod]
        public void Validation_With_Id()
        {
            Peer peer = marsId;
            Assert.AreEqual(true, peer.IsValid());
        }

        [TestMethod]
        public void Validation_With_Id_Pubkey()
        {
            var peer = new Peer
            {
                Id = marsId,
                PublicKey = marsPublicKey
            };
            Assert.AreEqual(true, peer.IsValid());
        }

        [TestMethod]
        public void Validation_With_Id_Invalid_Pubkey()
        {
            var peer = new Peer
            {
                Id = plutoId,
                PublicKey = marsPublicKey
            };
            Assert.AreEqual(false, peer.IsValid());
        }

        [TestMethod]
        public void Value_Equality()
        {
            var a0 = new Peer { Id = marsId };
            var a1 = new Peer { Id = marsId };
            var b = new Peer { Id = plutoId };
            Peer c = null;
            Peer d = null;

            Assert.IsTrue(c == d);
            Assert.IsFalse(c == b);
            Assert.IsFalse(b == c);

            Assert.IsFalse(c != d);
            Assert.IsTrue(c != b);
            Assert.IsTrue(b != c);

#pragma warning disable 1718
            Assert.IsTrue(a0 == a0);
            Assert.IsTrue(a0 == a1);
            Assert.IsFalse(a0 == b);

#pragma warning disable 1718
            Assert.IsFalse(a0 != a0);
            Assert.IsFalse(a0 != a1);
            Assert.IsTrue(a0 != b);

            Assert.IsTrue(a0.Equals(a0));
            Assert.IsTrue(a0.Equals(a1));
            Assert.IsFalse(a0.Equals(b));

            Assert.AreEqual(a0, a0);
            Assert.AreEqual(a0, a1);
            Assert.AreNotEqual(a0, b);

            Assert.AreEqual<Peer>(a0, a0);
            Assert.AreEqual<Peer>(a0, a1);
            Assert.AreNotEqual<Peer>(a0, b);

            Assert.AreEqual(a0.GetHashCode(), a0.GetHashCode());
            Assert.AreEqual(a0.GetHashCode(), a1.GetHashCode());
            Assert.AreNotEqual(a0.GetHashCode(), b.GetHashCode());
        }


        [TestMethod]
        public void Implicit_Conversion_From_String()
        {
            Peer a = marsId;
            Assert.IsInstanceOfType(a, typeof(Peer));
        }

    }
}

