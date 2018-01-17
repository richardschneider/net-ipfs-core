using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs.Cryptography
{
    /// <summary>
    ///   Thin wrapper around bouncy castle digests.
    /// </summary>
    /// <remarks>
    ///   Makes a Bouncy Caslte IDigest speak .Net HashAlgorithm.
    /// </remarks>
    internal class BouncyDigest : System.Security.Cryptography.HashAlgorithm
    {
        Org.BouncyCastle.Crypto.IDigest digest;

        /// <summary>
        ///   Wrap the bouncy castle digest.
        /// </summary>
        public BouncyDigest(Org.BouncyCastle.Crypto.IDigest digest)
        {
            this.digest = digest;
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            digest.Reset();
        }

        /// <inheritdoc/>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            digest.BlockUpdate(array, ibStart, cbSize);
        }

        /// <inheritdoc/>
        protected override byte[] HashFinal()
        {
            var output = new byte[digest.GetDigestSize()];
            digest.DoFinal(output, 0);
            return output;
        }
    }
}
 