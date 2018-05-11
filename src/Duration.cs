using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   Parsing and stringifying of a <see cref="TimeSpan"/> according to IPFS.
    /// </summary>
    /// <remarks>
    ///   From the <see href="https://godoc.org/time#ParseDuration">go spec</see>.
    ///   <para>
    ///   A duration string is a possibly signed sequence of decimal numbers, 
    ///   each with optional fraction and a unit suffix, such as "300ms", "-1.5h" or "2h45m". 
    ///   Valid time units are "ns", "us" (or "µs"), "ms", "s", "m", "h".
    ///   </para>
    /// </remarks>
    public static class Duration
    {
        /// <summary>
        ///   Converts the string representation of an IPFS duration
        ///   into its <see cref="TimeSpan"/> equivalent.
        /// </summary>
        /// <param name="s">
        ///   A string that contains the duration to convert.
        /// </param>
        /// <returns>
        ///   A <see cref="TimeSpan"/> that is equivalent to <paramref name="s"/>.
        /// </returns>
        /// <exception cref="FormatException">
        ///   <paramref name="s"/> is not a valid IPFS duration.
        /// </exception>
        /// <remarks>
        ///   An empty string or "n/a" or "unknown" returns <see cref="TimeSpan.Zero"/>.
        ///   <para>
        ///   A duration string is a possibly signed sequence of decimal numbers, 
        ///   each with optional fraction and a unit suffix, such as "300ms", "-1.5h" or "2h45m". 
        ///   Valid time units are "ns", "us" (or "µs"), "ms", "s", "m", "h".
        ///   </para>
        /// </remarks>
        public static TimeSpan Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || s == "n/a" || s == "unknown")
                return TimeSpan.Zero;

            var result = TimeSpan.Zero;
            var negative = false;
            using (var sr = new StringReader(s))
            {
                if (sr.Peek() == '-')
                {
                    negative = true;
                    sr.Read();
                }
                while (sr.Peek() != -1)
                {
                    result += ParseComponent(sr);
                }
            }

            if (negative)
                return TimeSpan.FromTicks(-result.Ticks);

            return result;
        }

        static TimeSpan ParseComponent(StringReader reader)
        {
            var value = ParseNumber(reader);
            var unit = ParseUnit(reader);

            switch (unit)
            {
                case "h":
                    return TimeSpan.FromHours(value);
                case "m":
                    return TimeSpan.FromMinutes(value);
                case "s":
                    return TimeSpan.FromSeconds(value);
                case "ms":
                    return TimeSpan.FromMilliseconds(value);
                case "us":
                case "µs":
                    return TimeSpan.FromTicks((long)(value * (double)TimeSpan.TicksPerMillisecond * 0.001));
                case "ns":
                    return TimeSpan.FromTicks((long)(value * (double)TimeSpan.TicksPerMillisecond * 0.000001));
                case "":
                    throw new FormatException("Missing IPFS duration unit.");
                default:
                    throw new FormatException($"Unknown IPFS duration unit '{unit}'.");
            }
        }

        static double ParseNumber(StringReader reader)
        {
            var s = new StringBuilder();
            while (true)
            {
                var c = (char)reader.Peek();
                if (char.IsDigit(c) || c == '.')
                {
                    s.Append(c);
                    reader.Read();
                    continue;
                }
                return double.Parse(s.ToString(), CultureInfo.InvariantCulture);
            }
        }

        static string ParseUnit(StringReader reader)
        {
            var s = new StringBuilder();
            while (true)
            {
                var c = (char)reader.Peek();
                if (char.IsDigit(c) || c == '.' || c == (char)0xFFFF)
                    break;
                s.Append(c);
                reader.Read();
            }

            return s.ToString();
        }
    }
}
