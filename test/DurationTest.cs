using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Ipfs
{

    [TestClass]
    public class DurationTest
    {
        [TestMethod]
        public void Parsing_Examples()
        {
            Assert.AreEqual(TimeSpan.FromMilliseconds(300), Duration.Parse("300ms"));
            Assert.AreEqual(TimeSpan.FromHours(-1.5), Duration.Parse("-1.5h"));
            Assert.AreEqual(new TimeSpan(2, 45, 0), Duration.Parse("2h45m"));
            Assert.AreEqual(new TimeSpan(0, 1, 0) + TimeSpan.FromSeconds(4.483878032), Duration.Parse("1m4.483878032s"));
        }

        [TestMethod]
        public void Parsing_Zero()
        {
            Assert.AreEqual(TimeSpan.Zero, Duration.Parse("0s"));
            Assert.AreEqual(TimeSpan.Zero, Duration.Parse("0µs"));
            Assert.AreEqual(TimeSpan.Zero, Duration.Parse("0ns"));
            Assert.AreEqual(TimeSpan.Zero, Duration.Parse("n/a"));
            Assert.AreEqual(TimeSpan.Zero, Duration.Parse("unknown"));
            Assert.AreEqual(TimeSpan.Zero, Duration.Parse(""));
        }

        [TestMethod]
        public void Parsing_Negative()
        {
            Assert.AreEqual(TimeSpan.FromHours(-2), Duration.Parse("-1.5h30m"));
        }

        [TestMethod]
        public void Parsing_Submilliseconds()
        {
            // Note: resolution of TimeSpan is 100ns, e.g. 1 tick.
            Assert.AreEqual(TimeSpan.FromTicks(2), Duration.Parse("200ns"));
            Assert.AreEqual(TimeSpan.FromTicks(2000), Duration.Parse("200us"));
            Assert.AreEqual(TimeSpan.FromTicks(2000), Duration.Parse("200µs"));
        }

        [TestMethod]
        public void Parsing_MissingNumber()
        {
            ExceptionAssert.Throws<FormatException>(() =>
            {
                var _ = Duration.Parse("s");
            });
        }

        [TestMethod]
        public void Parsing_MissingUnit()
        {
            ExceptionAssert.Throws<FormatException>(() =>
            {
                var _ = Duration.Parse("1");
            }, "Missing IPFS duration unit.");
        }

        [TestMethod]
        public void Parsing_InvalidUnit()
        {
            ExceptionAssert.Throws<FormatException>(() =>
            {
                var _ = Duration.Parse("1jiffy");
            }, "Unknown IPFS duration unit 'jiffy'.");
        }
    }
}

