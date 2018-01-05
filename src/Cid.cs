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
        ///   base58btc, base64, etc.
        /// </value>
        /// <seealso href="https://github.com/multiformats/multibase"/>
        public string Encoding { get; set; } = "base58btc";

        /// <summary>
        ///   The content type or format of the data being addressed.
        /// </summary>
        /// <value>
        ///   dag-pb, dag-cbor, eth-block, etc.
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

        /// <inheritdoc/>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Encoding);
            sb.Append(':');
            sb.Append("cidv");
            sb.Append(Version);
            sb.Append(':');
            sb.Append(ContentType);
            if (Hash != null)
            {
                sb.Append(':');
                sb.Append(Hash.Algorithm.Name);
                sb.Append(':');
                sb.Append(Hash.ToBase58()); // TODO: Use the encoding
            }
            return sb.ToString();
        }

        /// <summary>
        ///   Implicit casting of a <see cref="MultiHash"/> to a <see cref="Cid"/>.
        /// </summary>
        /// <param name="hash">
        ///   A <see cref="MultiHash"/>.
        /// </param>
        /// <returns>
        ///   A new <see cref="Cid"/> v0 based on the <paramref name="hash"/>.
        /// </returns>
        static public implicit operator Cid(MultiHash hash)
        {
            return new Cid
            {
                Hash = hash,
                Version = 0,
                Encoding = "base58btc",
                ContentType = "dag-pb"
            };
        }

    }
}
