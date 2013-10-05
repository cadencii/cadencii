using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using cadencii;

namespace cadencii.test.enums
{
    [TestFixture]
    class RendererKindUtilTest
    {
        [Test]
        public void testGetString()
        {
            Assert.AreEqual("VOCALOID1", RendererKindUtil.getString(RendererKind.VOCALOID1));
            Assert.AreEqual("VOCALOID1", RendererKindUtil.getString(RendererKind.VOCALOID1_100));
            Assert.AreEqual("VOCALOID1", RendererKindUtil.getString(RendererKind.VOCALOID1_101));
            Assert.AreEqual("VOCALOID2", RendererKindUtil.getString(RendererKind.VOCALOID2));
            Assert.AreEqual("vConnect-STAND", RendererKindUtil.getString(RendererKind.VCNT));
            Assert.AreEqual("UTAU", RendererKindUtil.getString(RendererKind.UTAU));
            Assert.AreEqual("AquesTone", RendererKindUtil.getString(RendererKind.AQUES_TONE));
            Assert.AreEqual("AquesTone2", RendererKindUtil.getString(RendererKind.AQUES_TONE2));
            Assert.AreEqual("", RendererKindUtil.getString(RendererKind.NULL));
        }

        [Test]
        public void testFromString()
        {
            Assert.AreEqual(RendererKind.VOCALOID1, RendererKindUtil.fromString("VOCALOID1"));
            Assert.AreEqual(RendererKind.VOCALOID1, RendererKindUtil.fromString("VOCALOID1 [1.0]"));
            Assert.AreEqual(RendererKind.VOCALOID1, RendererKindUtil.fromString("VOCALOID1 [1.1]"));
            Assert.AreEqual(RendererKind.VOCALOID2, RendererKindUtil.fromString("VOCALOID2"));
            Assert.AreEqual(RendererKind.VCNT, RendererKindUtil.fromString("vConnect-STAND"));
            Assert.AreEqual(RendererKind.UTAU, RendererKindUtil.fromString("UTAU"));
            Assert.AreEqual(RendererKind.AQUES_TONE, RendererKindUtil.fromString("AquesTone"));
            Assert.AreEqual(RendererKind.AQUES_TONE2, RendererKindUtil.fromString("AquesTone2"));
            Assert.AreEqual(RendererKind.NULL, RendererKindUtil.fromString(null));
            Assert.AreEqual(RendererKind.NULL, RendererKindUtil.fromString("hogehoge"));
        }
    }
}
