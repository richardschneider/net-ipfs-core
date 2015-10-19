using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ipfs
{
    /// <summary>
    ///   Metadata on an IPFS network protocol.
    /// </summary>
    public abstract class NetworkProtocol
    {
        static readonly ILog log = LogManager.GetLogger<NetworkProtocol>();
        internal static Dictionary<string, Type> Names = new Dictionary<string, Type>();
        internal static Dictionary<int, Type> Codes = new Dictionary<int, Type>();

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
        public abstract int Code { get; }

        /// <summary>
        ///   The value associated with the protocol.
        /// </summary>
        /// <remarks>
        ///   For tcp and udp this is the port number.  This can be <b>null</b> as is the case for http and https.
        /// </remarks>
        public string Value { get; set; }

        /// <summary>
        ///   Writes the binary representation to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="Stream"/> to write to.
        /// </param>
        /// <remarks>
        ///   The binary representation is a 1-byte <see cref="NetworkProtocol.Code"/> followed
        ///   by an optional value.
        /// </remarks>
        public virtual void Write(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            throw new NotImplementedException();
        }

        /// <summary>
        ///   Writes the string representation to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="TextWriter"/> to write to.
        /// </param>
        /// <remarks>
        ///   The string representation is "/<see cref="Name"/>" followed by 
        ///   an optional "/<see cref="Value"/>".
        /// </remarks>
        public virtual void Write(TextWriter stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            stream.Write('/');
            stream.Write(Name);
            if (Value != null)
            {
                stream.Write('/');
                stream.Write(Value);
            }
        }

        /// <summary>
        ///   Reads the binary representation deom the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The <see cref="Stream"/> to read from.
        /// </param>
        /// <remarks>
        ///   The binary representation is a 1-byte <see cref="NetworkProtocol.Code"/> followed
        ///   by an optional value.
        /// </remarks>
        public virtual void ReadValue(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            throw new NotImplementedException();
        }

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
            if (stream == null)
                throw new ArgumentNullException("stream");

            Value = string.Empty;
            int c;
            while (-1 != (c = stream.Read()) && c != '/')
            {
                Value += (char)c;
            }
        }

        /// <summary>
        ///   The <see cref="Name"/> and <see cref="Value"/> of the network protocol.
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

    class TcpNetworkProtocol : NetworkProtocol
    {
        public UInt16 Port { get; set; }
        public override string Name { get { return "tcp"; } }
        public override int Code { get { return 6; } }
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
    }

    class UdpNetworkProtocol : TcpNetworkProtocol
    {
        public override string Name { get { return "udp"; } }
        public override int Code { get { return 17; } }
    }

    class DccpNetworkProtocol : TcpNetworkProtocol
    {
        public override string Name { get { return "dccp"; } }
        public override int Code { get { return 33; } }
    }

    class SctpNetworkProtocol : TcpNetworkProtocol
    {
        public override string Name { get { return "sctp"; } }
        public override int Code { get { return 132; } }
    }

    class Ipv4NetworkProtocol : NetworkProtocol
    {
        public override string Name { get { return "ip4"; } }
        public override int Code { get { return 4; } }
    }

    class Ipv6NetworkProtocol : NetworkProtocol
    {

        public override string Name { get { return "ip6"; } }
        public override int Code { get { return 41; } }
    }

    class IpfsNetworkProtocol : NetworkProtocol
    {
        public override string Name { get { return "ipfs"; } }
        public override int Code { get { return 421; } }
    }

    class HttpNetworkProtocol : NetworkProtocol
    {
        public override string Name { get { return "http"; } }
        public override int Code { get { return 480; } }
        public override void ReadValue(Stream stream) { }
        public override void ReadValue(TextReader stream) { }
    }

    class HttpsNetworkProtocol : HttpNetworkProtocol
    {
        public override string Name { get { return "https"; } }
        public override int Code { get { return 443; } }
    }
}
