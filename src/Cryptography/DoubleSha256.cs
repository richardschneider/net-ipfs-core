using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ipfs.Cryptography
{
    class DoubleSha256 : HashAlgorithm
    {
        HashAlgorithm digest = SHA256.Create();
        byte[] round1;

        public override void Initialize()
        {
            digest.Initialize();
            round1 = null;
        }

        public override int HashSize => digest.HashSize;

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (round1 != null)
                throw new NotSupportedException("Already called.");

            round1 = digest.ComputeHash(array, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            digest.Initialize();
            return digest.ComputeHash(round1);
        }

    }
}
