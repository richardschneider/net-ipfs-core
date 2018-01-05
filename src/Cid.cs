using System;
using System.Collections.Generic;
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
    public class Cid
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
        /// <seealso href="https://github.com/multiformats/multibase"/>
        public string Encoding { get; set; } = "base58btc";

        /// <summary>
        ///   The content type or format of the data being addressed.
        /// </summary>
        /// <value>
        ///   dag-pb, dag-cbor, eth-block, etc.  Defaults to "dag-pb".
        /// </value>
        /// <seealso href="https://github.com/multiformats/multicodec"/>
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

            throw new NotImplementedException();
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
        public static Cid Decode(string input)
        {
            // SHA2-256 MultiHash is CID v0.
            if (input.Length == 46 && input.StartsWith("Qm"))
            {
                return (Cid)new MultiHash(input);
            }

            throw new FormatException();
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

    }
}
