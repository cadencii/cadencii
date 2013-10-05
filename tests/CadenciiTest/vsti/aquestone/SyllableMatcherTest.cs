using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using cadencii;

namespace cadencii.test.vsti.aquestone
{
    [TestFixture]
    class SyllableMatcherTest
    {
        [Test]
        public void testFind()
        {
            var matcher = new SyllableMatcher();
            Assert.AreEqual("a", matcher.find("a"));
            Assert.AreEqual("a", matcher.find("A"));
            Assert.AreEqual("a", matcher.find("あ"));
            Assert.AreEqual("a", matcher.find("ア"));
            Assert.AreEqual("a", matcher.find("ｱ"));
            Assert.AreEqual("ga", matcher.find("が"));
            Assert.AreEqual("ga", matcher.find("か゛"));
            Assert.AreEqual("ga", matcher.find("かﾞ"));
            Assert.AreEqual("ga", matcher.find("ｶﾞ"));
            Assert.AreEqual("", matcher.find("＠"));
        }
    }
}
