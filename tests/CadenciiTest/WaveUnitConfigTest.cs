using System;
using System.IO;
using cadencii;
using cadencii.xml;
using NUnit.Framework;

namespace cadencii
{
    [TestFixture]
    class WaveUnitConfigTest
    {
        [Test]
        public void testPutElement()
        {
            WaveUnitConfig item = new WaveUnitConfig();
            item.putElement("a", "B");
            Assert.AreEqual("\na:B", item.getConfigString());

            item.putElement("a", "C");
            Assert.AreEqual("\na:C", item.getConfigString());

            item.putElement("a", null);
            Assert.AreEqual("\na:", item.getConfigString());

            item.putElement("b", "A");
            Assert.AreEqual("\na:\nb:A", item.getConfigString());
        }

        [Test, ExpectedException]
        public void testPutElementWithNullKey()
        {
            WaveUnitConfig item = new WaveUnitConfig();
            item.putElement(null, "a");
        }

        [Test, ExpectedException]
        public void testPutElementWithEmptyKey()
        {
            WaveUnitConfig item = new WaveUnitConfig();
            item.putElement("", "a");
        }

        [Test]
        public void testGetElement()
        {
            WaveUnitConfig item = new WaveUnitConfig();
            item.putElement("a", "A");
            item.putElement("b", null);
            item.putElement("c", "");

            Assert.AreEqual("A", item.getElement("a"));
            Assert.AreEqual("", item.getElement("b"));
            Assert.AreEqual("", item.getElement("c"));

            Assert.AreEqual("", item.getElement("NON_EXIST_KEY"));
            Assert.AreEqual("", item.getElement(null));
        }

        [Test]
        public void testXmlSerialization()
        {
            XmlSerializer xs = new XmlSerializer(typeof(WaveUnitConfig));
            WaveUnitConfig item = new WaveUnitConfig();
            item.putElement("a", "A");
            string actualPath = PortUtil.createTempFile();
            using (FileStream fs = new FileStream(actualPath, FileMode.Create, FileAccess.Write)) {
                xs.serialize(fs, item);
            }

            Console.WriteLine(File.ReadAllText(actualPath));
            byte[] actual = File.ReadAllBytes(actualPath);
            byte[] expected = File.ReadAllBytes("./expected/WaveUnitConfig.xml");
            Assert.AreEqual(expected, actual);

            File.Delete(actualPath);
        }
    }
}
