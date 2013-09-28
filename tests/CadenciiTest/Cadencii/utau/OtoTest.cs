using NUnit.Framework;
using cadencii;
using cadencii.vsq;
using cadencii.utau;

namespace cadencii.test.utau
{
    [TestFixture]
    class OtoTest
    {
        private readonly string root_ = "./fixture/utau_voice_db";

        [Test]
        public void readRootOto()
        {
            var oto = new Oto(root_ + "/oto.ini", root_);
            {
                var actual = oto.attachFileNameFromLyric("Ç†");
                Assert.AreEqual("Ç†", actual.Alias);
                Assert.AreEqual("Ç†.wav", actual.fileName);
                Assert.AreEqual(6f, actual.msOffset);
                Assert.AreEqual(52f, actual.msConsonant);
                Assert.AreEqual(69f, actual.msBlank);
                Assert.AreEqual(1f, actual.msPreUtterance);
                Assert.AreEqual(2f, actual.msOverlap);
            }
        }

        [Test]
        public void readSubdirectoryOto()
        {
            var oto = new Oto(root_ + "/A/oto.ini", root_);
            var actual = oto.attachFileNameFromLyric("ÇÌÅ™");
            Assert.AreEqual("ÇÌÅ™", actual.Alias);
            Assert.AreEqual(@"A\ÇÌÅ™.wav", actual.fileName);
        }
    }
}
