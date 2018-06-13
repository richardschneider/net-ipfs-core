using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ipfs.Cryptography
{
    class IdentityHash : HashAlgorithm
    {
        byte[] digest;

        public override void Initialize()
        {
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (digest == null)
            {
                digest = new byte[cbSize];
                Buffer.BlockCopy(array, ibStart, digest, 0, cbSize);
                return;
            }

            var buffer = new byte[digest.Length + cbSize];
            Buffer.BlockCopy(digest, 0, buffer, digest.Length, digest.Length);
            Buffer.BlockCopy(array, ibStart, digest, digest.Length, cbSize);
            digest = buffer;
        }

        protected override byte[] HashFinal()
        {
            return digest;
        }
    }
}
