using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   The result from sending a <see cref="IGenericApi.PingAsync(MultiHash, int, System.Threading.CancellationToken)"/>.
    /// </summary>
    public class PingResult
    {
        /// <summary>
        ///   Indicates success or failure.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///   The round trip time; nano second resolution.
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        ///   The text to echo.
        /// </summary>
        public string Text { get; set; }
    }
}
