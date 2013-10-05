using System;
using System.Collections.Generic;
using System.Text;
using cadencii;
using NUnit.Framework;

namespace cadencii.test
{
    [TestFixture]
    class KanaDeRomanizationTest
    {
        [Test]
        public void test_hiragana2katakana()
        {
            Assert.AreEqual("ア", KanaDeRomanization.hiragana2katakana("あ"));
            Assert.AreEqual("ア", KanaDeRomanization.hiragana2katakana("ア"));
            Assert.AreEqual("ガ", KanaDeRomanization.hiragana2katakana("かﾞ"));
            Assert.AreEqual("ガ", KanaDeRomanization.hiragana2katakana("カ゛"));
            Assert.AreEqual("パ", KanaDeRomanization.hiragana2katakana("は゜"));
            Assert.AreEqual("パ", KanaDeRomanization.hiragana2katakana("ハﾟ"));
        }
    }
}
