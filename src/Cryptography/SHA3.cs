using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 1591 // No XML documentation

namespace SHA3
{
    public abstract class SHA3 :
#if PORTABLE
    IHashAlgorithm
#else
    System.Security.Cryptography.HashAlgorithm
#endif
    {
        #region Statics
        public static string DefaultHashName = "SHA512";

        protected static Dictionary<string, Func<SHA3>> HashNameMap;

        static SHA3()
        {
            HashNameMap = new Dictionary<string, Func<SHA3>>
            {
                //{ "SHA3-224", () => { return new SHA3224(); } }

            };
        }

        public static new SHA3 Create()
        {
            return Create(DefaultHashName);
        }

        public static new SHA3 Create(string hashName)
        {
            Func<SHA3> ctor;
            if (HashNameMap.TryGetValue(hashName, out ctor))
                return ctor();
            return null;
        }
        #endregion

        #region Implementation
        public const int KeccakB = 1600;
        public const int KeccakNumberOfRounds = 24;
        public const int KeccakLaneSizeInBits = 8 * 8;

        public readonly ulong[] RoundConstants;

        protected ulong[] state;
        protected byte[] buffer;
        protected int buffLength;
#if PORTABLE
        protected byte[] HashValue;
        protected int HashSizeValue;
#endif
        protected int keccakR;

        public int KeccakR
        {
            get
            {
                return keccakR;
            }
            protected set
            {
                keccakR = value;
            }
        }

        public int SizeInBytes
        {
            get
            {
                return KeccakR / 8;
            }
        }

        public int HashByteLength
        {
            get
            {
                return HashSizeValue / 8;
            }
        }

        public
#if !PORTABLE
        override
#endif
        bool CanReuseTransform
        {
            get
            {
                return true;
            }
        }

        protected SHA3(int hashBitLength)
        {
            if (hashBitLength != 224 && hashBitLength != 256 && hashBitLength != 384 && hashBitLength != 512)
                throw new ArgumentException("hashBitLength must be 224, 256, 384, or 512", "hashBitLength");
            Initialize();
            HashSizeValue = hashBitLength;
            switch (hashBitLength)
            {
                case 224:
                    KeccakR = 1152;
                    break;
                case 256:
                    KeccakR = 1088;
                    break;
                case 384:
                    KeccakR = 832;
                    break;
                case 512:
                    KeccakR = 576;
                    break;
            }
            RoundConstants = new ulong[]
            {
                0x0000000000000001UL,
                0x0000000000008082UL,
                0x800000000000808aUL,
                0x8000000080008000UL,
                0x000000000000808bUL,
                0x0000000080000001UL,
                0x8000000080008081UL,
                0x8000000000008009UL,
                0x000000000000008aUL,
                0x0000000000000088UL,
                0x0000000080008009UL,
                0x000000008000000aUL,
                0x000000008000808bUL,
                0x800000000000008bUL,
                0x8000000000008089UL,
                0x8000000000008003UL,
                0x8000000000008002UL,
                0x8000000000000080UL,
                0x000000000000800aUL,
                0x800000008000000aUL,
                0x8000000080008081UL,
                0x8000000000008080UL,
                0x0000000080000001UL,
                0x8000000080008008UL
            };
        }

        protected ulong ROL(ulong a, int offset)
        {
            return (((a) << ((offset) % KeccakLaneSizeInBits)) ^ ((a) >> (KeccakLaneSizeInBits - ((offset) % KeccakLaneSizeInBits))));
        }

        protected void AddToBuffer(byte[] array, ref int offset, ref int count)
        {
            int amount = Math.Min(count, buffer.Length - buffLength);
            Buffer.BlockCopy(array, offset, buffer, buffLength, amount);
            offset += amount;
            buffLength += amount;
            count -= amount;
        }

        public
#if !PORTABLE
        override
#endif
        byte[] Hash
        {
            get
            {
                return HashValue;
            }
        }

        public
#if !PORTABLE
        override
#endif
        int HashSize
        {
            get
            {
                return HashSizeValue;
            }
        }

        #endregion

        public
#if !PORTABLE
        override
#endif
        void Initialize()
        {
            buffLength = 0;
            state = new ulong[5 * 5];//1600 bits
            HashValue = null;
        }

        protected
#if !PORTABLE
        override
#endif
        void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (ibStart < 0)
                throw new ArgumentOutOfRangeException("ibStart");
            if (cbSize > array.Length)
                throw new ArgumentOutOfRangeException("cbSize");
            if (ibStart + cbSize > array.Length)
                throw new ArgumentOutOfRangeException("ibStart or cbSize");
        }

#if PORTABLE
        public abstract int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);
        public abstract byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);
#endif
    }
}