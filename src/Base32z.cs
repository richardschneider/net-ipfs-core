using SimpleBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   Base32 encoding designed to be easier for human use and more compact.
    /// </summary>
    /// <remarks>
    ///   Commonly referred to as 'z-base-32'.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Base32#z-base-32"/>
    public static class Base32z
    {
        static readonly Base32Alphabet alphabet =
            new Base32Alphabet("ybndrfg8ejkmcpqxot1uwisza345h769");

        /// <summary>
        ///   The encoder/decoder for z-base-32.
        /// </summary>
        public static readonly SimpleBase.Base32 Codec = new SimpleBase.Base32(alphabet);
    }
}
