using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Logging;
using Google.Protobuf;
using Newtonsoft.Json;

namespace Ipfs
{
    /// <summary>
    ///   A set of steps describing how to build up a connection.
    /// </summary>
    /// <remarks>
    ///   A multi address emphasizes explicitness, self-description, and
    ///   portability. It allows applications to treat addresses as opaque tokens
    ///    which avoids making assumptions about the address representation (e.g. length).
    ///   <para>
    ///   A multi address is represented as a series of protocol codes and values pairs.  For example,
    ///   an IPFS file at a sepcific address over ipv4 and tcp is 
    ///   "/ip4/10.1.10.10/tcp/29087/ipfs/QmVcSqVEsvm5RR9mBLjwpb2XjFVn5bPdPL69mL8PH45pPC".
    ///   </para>
    ///   <para>
    ///   A multi address is considered immutablle and value type equality is implemented.
    ///   </para>
    /// </remarks>
    /// <seealso href="https://github.com/multiformats/multiaddr"/>
    [JsonConverter(typeof(MultiAddress.Json))]
    public class MultiAddress : IEquatable<MultiAddress>
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="MultiAddress"/> class.
        /// </summary>
        public MultiAddress()
        {
            Protocols = new List<NetworkProtocol>();
        }

        /// <summary>
        ///   The components of the <b>MultiAddress</b>.
        /// </summary>
        public List<NetworkProtocol> Protocols { get; private set; }

        /// <summary>
        ///   Creates a new instance of the <see cref="MultiAddress"/> class with the string.
        /// </summary>
        /// <param name="s">
        ///   The string representation of a multi address, such as "/ip4/1270.0.01/tcp/5001".
        /// </param>
        public MultiAddress(string s) : this()
        {
            if (string.IsNullOrWhiteSpace(s))
                return;

            Read(new StringReader(s));
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="MultiAddress"/> class from the
        ///   specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   A <see cref="Stream"/> containing the binary representation of a
        ///   <b>MultiAddress</b>.
        /// </param>
        /// <remarks>
        ///   Reads the binary representation of <see cref="MultiAddress"/> from the <paramref name="stream"/>.
        ///   <para>
        ///   The binary representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        ///   </para>
        /// </remarks>
        public MultiAddress(Stream stream)
            : this()
        {
            Read(stream);
        }

        /// <summary>
        ///   Creates a deep copy of the multi address.
        /// </summary>
        /// <returns>
        ///   A new deep copy.
        /// </returns>
        public MultiAddress Clone()
        {
            return new MultiAddress(ToString());
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="MultiAddress"/> class from the
        ///   specified byte array.
        /// </summary>
        /// <param name="buffer">(
        ///   A byte array containing the binary representation of a
        ///   <b>MultiAddress</b>.
        /// </param>
        /// <remarks>
        ///   Reads the binary representation of <see cref="MultiAddress"/> from the <paramref name="buffer"/>.
        ///   <para>
        ///   The binary representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        ///   </para>
        /// </remarks>
        public MultiAddress(byte[] buffer)
            : this()
        {
            if (buffer == null || buffer.Length == 0)
                return;

            Read(new MemoryStream(buffer, false));
        }

        /// <summary>
        ///   Gets the peer ID of the multiaddress.
        /// </summary>
        /// <value>
        ///   The <see cref="Peer.Id"/> as a <see cref="MultiHash"/>.
        /// </value>
        /// <exception cref="Exception">
        ///   When the last <see cref="Protocols">protocol</see>
        ///   is not "ipfs" nor "p2p".
        /// </exception>
        /// <remarks>
        ///   The peer ID is contained in the last protocol that
        ///   is "ipfs" or "p2p".  For example, <c>/ip4/10.1.10.10/tcp/29087/ipfs/QmVcSqVEsvm5RR9mBLjwpb2XjFVn5bPdPL69mL8PH45pPC</c>.
        /// </remarks>
        public MultiHash PeerId
        {
            get
            {
                var protocol = Protocols
                    .LastOrDefault(p => p.Name == "ipfs" || p.Name == "p2p");
                if (protocol == null)
                {
                    throw new Exception($"'{this}' is missing the peer ID. Add the 'ipfs' or 'p2p' protocol.");
                }
                return protocol.Value;
            }
        }

        /// <summary>
        ///   Determines if the peer ID is present.
        /// </summary>
        /// <value>
        ///   <b>true</b> if the peer ID present; otherwise, <b>false</b>.
        /// </value>
        /// <remarks>
        ///   The peer ID is contained in the last protocol that
        ///   is "ipfs" or "p2p".  For example, <c>/ip4/10.1.10.10/tcp/29087/ipfs/QmVcSqVEsvm5RR9mBLjwpb2XjFVn5bPdPL69mL8PH45pPC</c>.
        /// </remarks>
        public bool HasPeerId
        {
            get
            {
                return Protocols.Any(p => p.Name == "ipfs" || p.Name == "p2p");
            }
        }

        /// <summary>
        ///   Gets a multiaddress that ends with the peer ID.
        /// </summary>
        /// <param name="peerId">
        ///   The peer ID to end the multiaddress with.
        /// </param>
        /// <returns>
        ///   Either the <c>this</c> multiadddress when it contains the
        ///   <paramref name="peerId"/> or a new <see cref="MultiAddress"/>
        ///   ending the <paramref name="peerId"/>.
        /// </returns>
        /// <exception cref="Exception">
        ///   When the mulltiaddress has the wrong peer ID.
        /// </exception>
        public MultiAddress WithPeerId(MultiHash peerId)
        {
            if (HasPeerId)
            {
                var id = PeerId;
                if (id != peerId)
                {
                    throw new Exception($"Expected a multiaddress with peer ID of '{peerId}', not '{id}'.");
                }
                return this;
            }

            return new MultiAddress(this.ToString() + $"/p2p/{peerId}");
        }

        /// <summary>
        ///   Gets a multiaddress without the peer ID.
        /// </summary>
        /// <returns>
        ///   Either the this multiaddress when it does not contain
        ///   a peer ID; or a new <see cref="MultiAddress"/> without the peer ID.
        /// </returns>
        public MultiAddress WithoutPeerId()
        {
            if (!HasPeerId)
            {
                return this;
            }
            var clone = Clone();
            clone.Protocols
                .RemoveAll(p => p.Name == "p2p" || p.Name == "ipfs");
            return clone;
        }

        /// <summary>
        ///   Writes the binary representation to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="Stream"/> to write to.
        /// </param>
        /// <remarks>
        ///   The binary representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        /// </remarks>
        public void Write(Stream stream)
        {
            var cos = new CodedOutputStream(stream, true);
            Write(cos);
            cos.Flush();
        }

        /// <summary>
        ///   Writes the binary representation to the specified <see cref="CodedOutputStream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="CodedOutputStream"/> to write to.
        /// </param>
        /// <remarks>
        ///   The binary representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        /// </remarks>
        public void Write(CodedOutputStream stream)
        {
            foreach (var protocol in Protocols)
            {
                stream.WriteInt64(protocol.Code);
                protocol.WriteValue(stream);
            }
        }

        /// <summary>
        ///   Writes the string representation to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="TextWriter"/> to write to.
        /// </param>
        /// <remarks>
        ///   The string representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        /// </remarks>
        public void Write(TextWriter stream)
        {
            foreach (var protocol in Protocols)
            {
                stream.Write('/');
                stream.Write(protocol.Name);
                protocol.WriteValue(stream);
            }
        }

        /// <summary>
        ///   Reads the binary representation of the the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="Stream"/> to read from.
        /// </param>
        /// <remarks>
        ///   The binary representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        /// </remarks>
        void Read(Stream stream)
        {
            Read(new CodedInputStream(stream, true));
        }

        /// <summary>
        ///   Reads the binary representation of the specified <see cref="CodedInputStream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="CodedInputStream"/> to read from.
        /// </param>
        /// <remarks>
        ///   The binary representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        /// </remarks>
        void Read(CodedInputStream stream)
        {
            Protocols.Clear();
            do
            {
                uint code = (uint)stream.ReadInt64();
                if (!NetworkProtocol.Codes.TryGetValue(code, out Type protocolType))
                    throw new InvalidDataException(string.Format("The IPFS network protocol code '{0}' is unknown.", code));
                var p = (NetworkProtocol)Activator.CreateInstance(protocolType);
                p.ReadValue(stream);
                Protocols.Add(p);
            } while (!stream.IsAtEnd);
        }

        /// <summary>
        ///   Reads the string representation from the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="TextReader"/> to read from
        /// </param>
        /// <remarks>
        ///   The string representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        /// </remarks>
        void Read(TextReader stream)
        {
            if (stream.Read() != '/')
            {
                throw new FormatException("An IFPS multiaddr must start with '/'.");
            }

            var name = new StringBuilder();
            Protocols.Clear();
            int c;
            while (true)
            {
                name.Clear();
                while (-1 != (c = stream.Read()) && c != '/')
                {
                    name.Append((char)c);
                }
                if (name.Length == 0)
                    break;
                if (!NetworkProtocol.Names.TryGetValue(name.ToString(), out Type protocolType))
                    throw new FormatException(string.Format("The IPFS network protocol '{0}' is unknown.", name.ToString()));
                var p = (NetworkProtocol)Activator.CreateInstance(protocolType);
                p.ReadValue(stream);
                Protocols.Add(p);
            }

            if (Protocols.Count == 0)
                throw new FormatException("The IFPS multiaddr has no protocol specified.");
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var that = obj as MultiAddress;
            return (that == null)
                ? false
                : this.Equals(that);
        }

        /// <inheritdoc />
        public bool Equals(MultiAddress that)
        {
            if (this.Protocols.Count != that.Protocols.Count)
                return false;
            for (int i = 0; i < Protocols.Count; ++i)
            {
                if (this.Protocols[i].Code != that.Protocols[i].Code)
                    return false;
                if (this.Protocols[i].Value != that.Protocols[i].Value)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///   Value equality.
        /// </summary>
        public static bool operator ==(MultiAddress a, MultiAddress b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (a is null) return false;
            if (b is null) return false;

            return a.Equals(b);
        }

        /// <summary>
        ///   Value inequality.
        /// </summary>
        public static bool operator !=(MultiAddress a, MultiAddress b)
        {
            if (object.ReferenceEquals(a, b)) return false;
            if (a is null) return true;
            if (b is null) return true;

            return !a.Equals(b);
        }

        /// <summary>
        ///   A sequence of <see cref="NetworkProtocol">network protocols</see> that is readable
        ///   to a human.
        /// </summary>
        public override string ToString()
        {
            using (var s = new StringWriter())
            {
                Write(s);
                return s.ToString();
            }
        }

        /// <summary>
        ///   Returns the IPFS binary representation as a byte array.
        /// </summary>
        /// <returns>
        ///   A byte array.
        /// </returns>
        /// <remarks>
        ///   The binary representation is a sequence of <see cref="NetworkProtocol">network protocols</see>.
        /// </remarks>
        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            {
                Write(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///   Implicit casting of a <see cref="string"/> to a <see cref="MultiAddress"/>.
        /// </summary>
        /// <param name="s">The string representation of a <see cref="MultiAddress"/>.</param>
        /// <returns>A new <see cref="MultiAddress"/>.</returns>
        static public implicit operator MultiAddress(string s)
        {
            return new MultiAddress(s);
        }

        /// <summary>
        ///   Try to create a <see cref="MultiAddress"/> from the specified
        ///   string.
        /// </summary>
        /// <param name="s">
        ///   The string representation of a multi address, such as "/ip4/1270.0.01/tcp/5001".
        /// </param>
        /// <returns>
        ///   <b>null</b> if the string cannot be parsed; otherwise a <see cref="MultiAddress"/>.
        /// </returns>
        public static MultiAddress TryCreate(string s)
        {
            try
            {
                return new MultiAddress(s);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///   Try to create a <see cref="MultiAddress"/> from the specified
        ///   the binary encoding.
        /// </summary>
        /// <param name="bytes">
        ///   The binary encoding of a multiaddress.
        /// </param>
        /// <returns>
        ///   <b>null</b> if the bytes cannot be parsed; otherwise a <see cref="MultiAddress"/>.
        /// </returns>
        public static MultiAddress TryCreate(byte[] bytes)
        {
            try
            {
                return new MultiAddress(bytes);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///   Conversion of a <see cref="MultiAddress"/> to and from JSON.
        /// </summary>
        /// <remarks>
        ///   The JSON is just a single string value.
        /// </remarks>
        class Json : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return true;
            }
            public override bool CanRead => true;
            public override bool CanWrite => true;
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var ma = value as MultiAddress;
                writer.WriteValue(ma?.ToString());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var s = reader.Value as string;
                return s == null ? null : new MultiAddress(s);
            }
        }


    }

}