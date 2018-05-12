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
        const double TicksPerNanosecond = (double)TimeSpan.TicksPerMillisecond * 0.000001;
        const double TicksPerMicrosecond = (double)TimeSpan.TicksPerMillisecond * 0.001;

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
                    return TimeSpan.FromTicks((long)(value * TicksPerMicrosecond));
                case "ns":
                    return TimeSpan.FromTicks((long)(value * TicksPerNanosecond));
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

        /// <summary>
        ///   Converts the <see cref="TimeSpan"/> to the equivalent string representation of an 
        ///   IPFS duration.
        /// </summary>
        /// <param name="duration">
        ///   The <see cref="TimeSpan"/> to convert.
        /// </param>
        /// <param name="zeroValue">
        ///   The string representation, when the <paramref name="duration"/> 
        ///   is equal to <see cref="TimeSpan.Zero"/>/
        /// </param>
        /// <returns>
        ///   The IPFS duration string representation.
        /// </returns>
        public static string Stringify(TimeSpan duration, string zeroValue = "0s")
        {
            if (duration.Ticks == 0)
                return zeroValue;

            var s = new StringBuilder();
            if (duration.Ticks < 0)
            {
                s.Append('-');
                duration = TimeSpan.FromTicks(-duration.Ticks);
            }

            Stringify(Math.Floor(duration.TotalHours), "h", s);
            Stringify(duration.Minutes, "m", s);
            Stringify(duration.Seconds, "s", s);
            Stringify(duration.Milliseconds, "ms", s);
            Stringify((long)((double)duration.Ticks / TicksPerMicrosecond) % 1000, "us", s);
            Stringify((long)((double)duration.Ticks / TicksPerNanosecond) % 1000, "ns", s);

            return s.ToString();
        }

        static void Stringify(double value, string unit, StringBuilder sb)
        {
            if (value == 0)
                return;
            sb.Append(value.ToString(CultureInfo.InvariantCulture));
            sb.Append(unit);
        }
    }
}
