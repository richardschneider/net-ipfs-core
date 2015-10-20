using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Google.ProtocolBuffers;

namespace Ipfs
{
    /// <summary>
    ///   Metadata on an IPFS network protocol.
    /// </summary>
    public abstract class NetworkProtocol
    {
        static readonly ILog log = LogManager.GetLogger<NetworkProtocol>();
        internal static Dictionary<string, Type> Names = new Dictionary<string, Type>();
        internal static Dictionary<uint, Type> Codes = new Dictionary<uint, Type>();

        /// <summary>
        ///   Registers the standard network protocols for IPFS.
        /// </summary>
        static NetworkProtocol()
        {
            NetworkProtocol.Register<Ipv4NetworkProtocol>();
            NetworkProtocol.Register<Ipv6NetworkProtocol>();
            NetworkProtocol.Register<TcpNetworkProtocol>();
            NetworkProtocol.Register<UdpNetworkProtocol>();
            NetworkProtocol.Register<IpfsNetworkProtocol>();
            NetworkProtocol.Register<HttpNetworkProtocol>();
            NetworkProtocol.Register<HttpsNetworkProtocol>();
            NetworkProtocol.Register<DccpNetworkProtocol>();
            NetworkProtocol.Register<SctpNetworkProtocol>();
        }

        /// <summary>
        ///   Register a network protocol for use.
        /// </summary>
        /// <typeparam name="T">
        ///   A <see cref="NetworkProtocol"/> to register.
        /// </typeparam>
        public static void Register<T>() where T : NetworkProtocol, new()
        {
            var protocol = new T();

            if (Names.ContainsKey(protocol.Name))
                throw new ArgumentException(string.Format("The IPFS network protocol '{0}' is already defined.", protocol.Name));
            if (Codes.ContainsKey(protocol.Code))
                throw new ArgumentException(string.Format("The IPFS network protocol code ({0}) is already defined.", protocol.Code));

            Names.Add(protocol.Name, typeof(T));
            Codes.Add(protocol.Code, typeof(T));

            if (log.IsDebugEnabled)
                log.DebugFormat("Registered '{0}' ({1}).", protocol.Name, protocol.Code);
        }

        /// <summary>
        ///   The name of the protocol.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///   The IPFS numeric code assigned to the network protocol.
        /// </summary>
        public abstract uint Code { get; }

        /// <summary>
        ///   The string value associated with the protocol.
        /// </summary>
        /// <remarks>
        ///   For tcp and udp this is the port number.  This can be <b>null</b> as is the case for http and https.
        /// </remarks>
        public string Value { get; set; }

        /// <summary>
        ///   Writes the binary representation to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="CodedOutputStream"/> to write to.
        /// </param>
        /// <remarks>
        ///   The binary representation of the <see cref="Value"/>.
        /// </remarks>
        public abstract void WriteValue(CodedOutputStream stream);

        /// <summary>
        ///   Writes the string representation to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="TextWriter"/> to write to.
        /// </param>
        /// <remarks>
        ///   The string representation of the optional <see cref="Value"/>.
        /// </remarks>
        public virtual void WriteValue(TextWriter stream)
        {
            if (Value != null)
            {
                stream.Write('/');
                stream.Write(Value);
            }
        }

        /// <summary>
        ///   Reads the binary representation from the specified <see cref="CodedInputStream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="CodedOutputStream"/> to read from.
        /// </param>
        /// <remarks>
        ///   The binary representation is an option <see cref="Value"/>.
        /// </remarks>
        public abstract void ReadValue(CodedInputStream stream);

        /// <summary>
        ///   Reads the string representation from the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="TextReader"/> to read from
        /// </param>
        /// <remarks>
        ///   The string representation is "/<see cref="Name"/>" followed by 
        ///   an optional "/<see cref="Value"/>".
        /// </remarks>
        public virtual void ReadValue(TextReader stream)
        {
            Value = string.Empty;
            int c;
            while (-1 != (c = stream.Read()) && c != '/')
            {
                Value += (char)c;
            }
        }

        /// <summary>
        ///   The <see cref="Name"/> and optional <see cref="Value"/> of the network protocol.
        /// </summary>
        public override string ToString()
        {
            using (var s = new StringWriter())
            {
                s.Write('/');
                s.Write(Name);
                WriteValue(s);
                return s.ToString();
            }
        }

    }

    class TcpNetworkProtocol : NetworkProtocol
    {
        public UInt16 Port { get; set; }
        public override string Name { get { return "tcp"; } }
        public override uint Code { get { return 6; } }
        public override void ReadValue(TextReader stream)
        {
            base.ReadValue(stream);
            try
            {
                Port = UInt16.Parse(Value);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format("'{0}' is not a valid port number.", Value), e);
            }
        }
        public override void ReadValue(CodedInputStream stream)
        {
            uint port = 0;
            stream.ReadUInt32(ref port);
            Value = port.ToString();
        }
        public override void WriteValue(CodedOutputStream stream)
        {
            stream.WriteUInt32NoTag(Port);
        }
    }

    class UdpNetworkProtocol : TcpNetworkProtocol
    {
        public override string Name { get { return "udp"; } }
        public override uint Code { get { return 17; } }
    }

    class DccpNetworkProtocol : TcpNetworkProtocol
    {
        public override string Name { get { return "dccp"; } }
        public override uint Code { get { return 33; } }
    }

    class SctpNetworkProtocol : TcpNetworkProtocol
    {
        public override string Name { get { return "sctp"; } }
        public override uint Code { get { return 132; } }
    }

    abstract class IpNetworkProtocol : NetworkProtocol
    {
        public IPAddress Address { get; set; }
        public override void ReadValue(TextReader stream)
        {
            base.ReadValue(stream);
            try
            {
                Address = IPAddress.Parse(Value);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format("'{0}' is not a valid IP address.", Value), e);
            }
        }
        public override void ReadValue(CodedInputStream stream)
        {
            var n = stream.ReadRawByte();
            var a = stream.ReadRawBytes(n);
            Address = new IPAddress(a);
            Value = Address.ToString();
        }
        public override void WriteValue(CodedOutputStream stream)
        {
            var ip = Address.GetAddressBytes();
            stream.WriteRawByte((byte)ip.Length);
            stream.WriteRawBytes(ip);
        }
    }

    class Ipv4NetworkProtocol : IpNetworkProtocol
    {
        public override string Name { get { return "ip4"; } }
        public override uint Code { get { return 4; } }
        public override void ReadValue(TextReader stream)
        {
            base.ReadValue(stream);
            if (Address.AddressFamily != AddressFamily.InterNetwork)
                throw new FormatException(string.Format("'{0}' is not a valid IPv4 address.", Value));
        }
    }

    class Ipv6NetworkProtocol : IpNetworkProtocol
    {
        public override string Name { get { return "ip6"; } }
        public override uint Code { get { return 41; } }
        public override void ReadValue(TextReader stream)
        {
            base.ReadValue(stream);
            if (Address.AddressFamily != AddressFamily.InterNetworkV6)
                throw new FormatException(string.Format("'{0}' is not a valid IPv6 address.", Value));
        }
    }

    class IpfsNetworkProtocol : NetworkProtocol
    {
        public MultiHash MultiHash { get; private set; }
        public override string Name { get { return "ipfs"; } }
        public override uint Code { get { return 421; } }
        public override void ReadValue(TextReader stream)
        {
            Value = stream.ReadToEnd();
            MultiHash = new MultiHash(Value);
        }
        public override void ReadValue(CodedInputStream stream)
        {
            MultiHash = new MultiHash(stream);
            Value = MultiHash.ToBase58();
        }
        public override void WriteValue(CodedOutputStream stream)
        {
            MultiHash.Write(stream);
        }
    }

    abstract class ValuelessNetworkProtocol : NetworkProtocol
    {
        public override void ReadValue(CodedInputStream stream) { }
        public override void ReadValue(TextReader stream) { }
        public override void WriteValue(CodedOutputStream stream) { }
    }

    class HttpNetworkProtocol : ValuelessNetworkProtocol
    {
        public override string Name { get { return "http"; } }
        public override uint Code { get { return 480; } }
    }

    class HttpsNetworkProtocol : ValuelessNetworkProtocol
    {
        public override string Name { get { return "https"; } }
        public override uint Code { get { return 443; } }
    }
}
