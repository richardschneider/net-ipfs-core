using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Logging;
using Google.ProtocolBuffers;

namespace Ipfs
{
    /// <summary>
    ///   A standard way to represent a networks address that supports multiple network protocols.
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
    ///   Value type equality is implemented.
    ///   </para>
    /// </remarks>
    /// <seealso href="https://github.com/jbenet/multiaddr"/>
    public class MultiAddress : IEquatable<MultiAddress>
    {
        static readonly ILog log = LogManager.GetLogger<MultiAddress>();

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
                throw new ArgumentNullException("s");

            Read(new StringReader(s));
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="MultiAddress"/> class from the
        ///   specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">(
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
            var cos = CodedOutputStream.CreateInstance(stream);
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
                stream.WriteUInt32NoTag(protocol.Code);
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
            Read(CodedInputStream.CreateInstance(stream));
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
            while (!stream.IsAtEnd)
            {
                uint code = 0;
                stream.ReadUInt32(ref code);
                Type protocolType;
                if (!NetworkProtocol.Codes.TryGetValue(code, out protocolType))
                    throw new InvalidDataException(string.Format("The IPFS network protocol code '{0}' is unknown.", code));
                var p = (NetworkProtocol)Activator.CreateInstance(protocolType);
                p.ReadValue(stream);
                Protocols.Add(p);
            }
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
        public void Read(TextReader stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (stream.Read() != '/')
                throw new FormatException("An IFPS multiaddr must start with '/'.");

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
                Type protocolType;
                if (!NetworkProtocol.Names.TryGetValue(name.ToString(), out protocolType))
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
                : this.ToString() == that.ToString();
        }

        /// <inheritdoc />
        public bool Equals(MultiAddress that)
        {
            return this.ToString() == that.ToString();
        }

        /// <summary>
        ///   Value equality.
        /// </summary>
        public static bool operator ==(MultiAddress a, MultiAddress b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;

            return a.Equals(b);
        }

        /// <summary>
        ///   Value inequality.
        /// </summary>
        public static bool operator !=(MultiAddress a, MultiAddress b)
        {
            if (object.ReferenceEquals(a, b)) return false;
            if (object.ReferenceEquals(a, null)) return true;
            if (object.ReferenceEquals(b, null)) return true;

            return !a.Equals(b);
        }

        /// <summary>
        ///   A sequence of <see cref="NetworkProtocol">network protocols</see>.
        /// </summary>
        public override string ToString()
        {
            using (var s = new StringWriter())
            {
                Write(s);
                return s.ToString();
            }
        }

    }

}