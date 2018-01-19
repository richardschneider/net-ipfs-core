using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   A node on the IPFS network.
    /// </summary>
    public class Peer : IEquatable<Peer>
    {
        static MultiAddress[] noAddress = new MultiAddress[0];
        const string unknown = "unknown/0.0";

        /// <summary>
        ///   Universally unique identifier.
        /// </summary>
        /// <value>
        ///   This is the <see cref="MultiHash"/> of the peer's protobuf encoded
        ///   <see cref="PublicKey"/>.
        /// </value>
        public MultiHash Id { get; set; }

        /// <summary>
        ///   The public key of the node.
        /// </summary>
        /// <value>
        ///   The base 64 encoding of the node's public key.  The default is <b>null</b>
        /// </value>
        /// <remarks>
        ///   The IPFS public key is the base-64 encoding of a protobuf encoding containing 
        ///   a type and the DER encoding of the PKCS Subject Public Key Info.
        /// </remarks>
        /// <seealso href="https://tools.ietf.org/html/rfc5280#section-4.1.2.7"/>
        public string PublicKey { get; set; }

        /// <summary>
        ///   The multiple addresses of the node.
        /// </summary>
        /// <value>
        ///   Where the peer can be found.  The default is an empty sequence.
        /// </value>
        public IEnumerable<MultiAddress> Addresses { get; set; } = noAddress;

        /// <summary>
        ///   The name and version of the IPFS software.
        /// </summary>
        /// <value>
        ///   For example "go-ipfs/0.4.17/".
        /// </value>
        /// <remarks>
        ///   There is no specification that describes the agent version string.  The default
        ///   is "unknown/0.0".
        /// </remarks>
        public string AgentVersion { get; set; } = unknown;

        /// <summary>
        ///  The name and version of the supported IPFS protocol.
        /// </summary>
        /// <value>
        ///   For example "ipfs/0.1.0".
        /// </value>
        /// <remarks>
        ///   There is no specification that describes the protocol version string. The default
        ///   is "unknown/0.0".
        /// </remarks>
        public string ProtocolVersion { get; set; } = unknown;

        /// <summary>
        ///   The <see cref="MultiAddress"/> that the peer is connected on.
        /// </summary>
        /// <value>
        ///   <b>null</b> when the peer is not connected to.
        /// </value>
        public MultiAddress ConnectedAddress { get; set; }

        /// <summary>
        /// The round-trip time it takes to get data from the peer.
        /// </summary>
        public TimeSpan? Latency { get; set; }

        /// <summary>
        ///   Determines if the information on the peer is valid.
        /// </summary>
        /// <returns>
        ///   <b>true</b> if all validation rules pass; otherwise <b>false</b>.
        /// </returns>
        /// <remarks>
        ///    Verifies that
        ///    <list type="bullet">
        ///      <item><description>The <see cref="Id"/> is defined</description></item>
        ///      <item><description>The <see cref="Id"/> is a hash of the <see cref="PublicKey"/></description></item>
        ///    </list>
        /// </remarks>
        public bool IsValid()
        {
            if (Id == null)
                return false;
            if (PublicKey != null && !Id.Matches(Convert.FromBase64String(PublicKey)))
                return false;

            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var that = obj as Peer;
            return (that == null)
                ? false
                : this.ToString() == that.ToString();
        }

        /// <inheritdoc />
        public bool Equals(Peer that)
        {
            return this.ToString() == that.ToString();
        }

        /// <summary>
        ///   Value equality.
        /// </summary>
        public static bool operator ==(Peer a, Peer b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;

            return a.Equals(b);
        }

        /// <summary>
        ///   Value inequality.
        /// </summary>
        public static bool operator !=(Peer a, Peer b)
        {
            if (object.ReferenceEquals(a, b)) return false;
            if (object.ReferenceEquals(a, null)) return true;
            if (object.ReferenceEquals(b, null)) return true;

            return !a.Equals(b);
        }

        /// <summary>
        ///   Returns the <see cref="Base58"/> encoding of the <see cref="Id"/>.
        /// </summary>
        /// <returns>
        ///   A Base58 representaton of the peer.
        /// </returns>
        public override string ToString()
        {
            return Id == null ? string.Empty : Id.ToBase58();
        }

        /// <summary>
        ///   Implicit casting of a <see cref="string"/> to a <see cref="Peer"/>.
        /// </summary>
        /// <param name="s">
        ///   A <see cref="Base58"/> encoded <see cref="Id"/>.
        /// </param>
        /// <returns>
        ///   A new <see cref="Peer"/>.
        /// </returns>
        /// <remarks>
        ///    Equivalent to <code>new Peer { Id = s }</code>
        /// </remarks>
        static public implicit operator Peer(string s)
        {
            return new Peer { Id = s };
        }
    }
}
