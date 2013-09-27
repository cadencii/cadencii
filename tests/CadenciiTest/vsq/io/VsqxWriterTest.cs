using NUnit.Framework;
using System.Windows.Forms;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using cadencii.vsq;
using cadencii.vsq.io;

namespace cadencii.test.vsq.io
{
    [TestFixture]
    class VsqxWriterTest
    {
        [Test]
        public void writeSingerTest()
        {
            var sequence = new VsqFile("singer", 1, 4, 4, 500000);
            var premeasaure_clock = sequence.getPreMeasureClocks();

            var temporary = Path.GetTempFileName();
            var writer = new VsqxWriter();
            Assert.DoesNotThrow(() => writer.write(sequence, temporary));

            var text = File.ReadAllText(temporary);
            var document = XElement.Parse(text);
            XNamespace ns = "http://www.yamaha.co.jp/vocaloid/schema/vsq3/";
            var singer =
                document
                    .Descendants(ns + "vsTrack")
                    .First()
                    .Descendants(ns + "musicalPart")
                    .First()
                    .Descendants(ns + "singer")
                    .FirstOrDefault();
            Assert.NotNull(singer);
            var pos_tick = singer.Descendants(ns + "posTick").FirstOrDefault();
            Assert.NotNull(pos_tick);
            Assert.AreEqual("0", pos_tick.Value);
        }
    }
}
