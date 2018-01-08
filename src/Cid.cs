using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///  Identifies some content, e.g. a Content ID.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   A Cid is a self-describing content-addressed identifier for distributed systems.
    ///   </para>
    ///   <para>
    ///   Initially, IPFS used a <see cref="MultiHash"/> as the CID and this is still supported as <see cref="Version"/> 0.
    ///   Version 1 adds a self describing structure to the multi-hash, see the <see href="https://github.com/ipld/cid">spec</see>. 
    ///   </para>
    ///   <note>
    ///   The <see cref="MultiHash.Algorithm">hashing algorithm</see> must be "sha2-256" for a version 0 CID.
    ///   </note>
    /// </remarks>
    /// <seealso href="https://github.com/ipld/cid"/>
    public class Cid : IEquatable<Cid>
    {
        /// <summary>
        ///   The version of the CID.
        /// </summary>
        /// <value>
        ///   0 or 1.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        ///   The encoding of the CID.
        /// </summary>
        /// <value>
        ///   base58btc, base64, etc.  Defaults to "base58btc",
        /// </value>
        /// <seealso cref="MultiBase"/>
        public string Encoding { get; set; } = "base58btc";

        /// <summary>
        ///   The content type or format of the data being addressed.
        /// </summary>
        /// <value>
        ///   dag-pb, dag-cbor, eth-block, etc.  Defaults to "dag-pb".
        /// </value>
        /// <seealso cref="MultiCodec"/>
        public string ContentType { get; set; } = "dag-pb";

        /// <summary>
        ///   The cryptographic hash of the content being addressed.
        /// </summary>
        /// <value>
        ///   The <see cref="MultiHash"/> of the content being addressed.
        /// </value>
        public MultiHash Hash { get; set; }

        /// <summary>
        ///   A CID that is readable by a human.
        /// </summary>
        /// <returns>
        ///  e.g. "base58btc cidv0 dag-pb sha2-256 Qm..."
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Encoding);
            sb.Append(' ');
            sb.Append("cidv");
            sb.Append(Version);
            sb.Append(' ');
            sb.Append(ContentType);
            if (Hash != null)
            {
                sb.Append(' ');
                sb.Append(Hash.Algorithm.Name);
                sb.Append(' ');
                sb.Append(Hash.ToBase58()); // TODO: Use the encoding
            }
            return sb.ToString();
        }

        /// <summary>
        ///   Converts the CID to its equivalent string representation.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="Cid"/>.
        /// </returns>
        /// <remarks>
        ///   For <see cref="Version"/> 0, this is equalivalent to the 
        ///   <see cref="MultiHash.ToBase58()">base58btc encoding</see>
        ///   of the <see cref="Hash"/>.
        /// </remarks>
        /// <seealso cref="Decode"/>
        public string Encode()
        {
            if (Version == 0)
            {
                return Hash.ToBase58();
            }

            using (var ms = new MemoryStream())
            {
                ms.WriteVarint(Version);
                ms.WriteMultiCodec(ContentType);
                Hash.Write(ms);
                return MultiBase.Encode(ms.ToArray(), Encoding);
            }
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>
        ///   to an equivalent <see cref="Cid"/> object.
        /// </summary>
        /// <param name="input">
        ///   The <see cref="Cid.Encode">CID encoded</see> string.
        /// </param>
        /// <returns>
        ///   A new <see cref="Cid"/> that is equivalent to <paramref name="input"/>.
        /// </returns>
        /// <exception cref="FormatException">
        ///   When the <paramref name="input"/> can not be decoded.
        /// </exception>
        /// <seealso cref="Encode"/>
        public static Cid Decode(string input)
        {
            try
            {
                // SHA2-256 MultiHash is CID v0.
                if (input.Length == 46 && input.StartsWith("Qm"))
                {
                    return (Cid)new MultiHash(input);
                }

                using (var ms = new MemoryStream(MultiBase.Decode(input), false))
                {
                    var version = ms.ReadVarint32();
                    if (version != 1)
                    {
                        throw new InvalidDataException($"Unknown CID version '{version}'.");
                    }
                    return new Cid
                    {
                        Version = version,
                        Encoding = Registry.MultiBaseAlgorithm.Codes[input[0]].Name,
                        ContentType = ms.ReadMultiCodec().Name,
                        Hash = new MultiHash(ms)
                    };
                }
            }
            catch (Exception e)
            {
                throw new FormatException($"Invalid CID '{input}'.", e);
            }
        }

        /// <summary>
        ///   Implicit casting of a <see cref="MultiHash"/> to a <see cref="Cid"/>.
        /// </summary>
        /// <param name="hash">
        ///   A <see cref="MultiHash"/>.
        /// </param>
        /// <returns>
        ///   A new <see cref="Cid"/> based on the <paramref name="hash"/>.  A <see cref="Version"/> 0
        ///   CID is returned if the <paramref name="hash"/> is "sha2-356"; otherwise <see cref="Version"/> 1.
        /// </returns>
        static public implicit operator Cid(MultiHash hash)
        {
            if (hash.Algorithm.Name == "sha2-256")
            {
                return new Cid
                {
                    Hash = hash,
                    Version = 0,
                    Encoding = "base58btc",
                    ContentType = "dag-pb"
                };
            }

            return new Cid
            {
                Version = 1,
                Hash = hash
            };
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Encode().GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var that = obj as Cid;
            return (that == null)
                ? false
                : this.Encode() == that.Encode();
        }

        /// <inheritdoc />
        public bool Equals(Cid that)
        {
            return this.Encode() == that.Encode();
        }

        /// <summary>
        ///   Value equality.
        /// </summary>
        public static bool operator ==(Cid a, Cid b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;

            return a.Equals(b);
        }

        /// <summary>
        ///   Value inequality.
        /// </summary>
        public static bool operator !=(Cid a, Cid b)
        {
            if (object.ReferenceEquals(a, b)) return false;
            if (object.ReferenceEquals(a, null)) return true;
            if (object.ReferenceEquals(b, null)) return true;

            return !a.Equals(b);
        }

        /// <summary>
        ///   Implicit casting of a <see cref="string"/> to a <see cref="Cid"/>.
        /// </summary>
        /// <param name="s">
        ///   A string encoded <b>Cid</b>.
        /// </param>
        /// <returns>
        ///   A new <see cref="Cid"/>.
        /// </returns>
        /// <remarks>
        ///    Equivalent to <code> Cid.Decode(s)</code>
        /// </remarks>
        static public implicit operator Cid(string s)
        {
            return Cid.Decode(s);
        }

    }
}
