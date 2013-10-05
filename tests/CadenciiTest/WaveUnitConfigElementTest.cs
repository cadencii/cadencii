using System;
using System.Windows.Forms;
using NUnit.Framework;

namespace cadencii
{
    class WaveUnitConfigElementStub : WaveUnitConfigElement
    {
        public void setNakedKey(string key)
        {
            this.key = key;
        }

        public string getNakedKey()
        {
            return this.key;
        }

        public void setNakedValue(string value)
        {
            this.value = value;
        }

        public string getNakedValue()
        {
            return this.value;
        }
    }

    [TestFixture]
    public class WaveUnitConfigElementTest
    {
        #region key関係のテスト
        [Test]
        public void testGetKey()
        {
            WaveUnitConfigElementStub e = new WaveUnitConfigElementStub();
            e.setNakedKey("Foo");
            Assert.AreEqual("Foo", e.getKey());

            e.setNakedKey(null);
            Assert.NotNull(e.getKey());
            Assert.AreEqual("", e.getKey());
        }

        [Test]
        public void testSetKey()
        {
            WaveUnitConfigElementStub e = new WaveUnitConfigElementStub();
            e.setNakedKey(null);
            e.setKey("Bar");
            Assert.AreEqual("Bar", e.getNakedKey());
        }

        [Test, ExpectedException(ExpectedMessage = "key must not be empty")]
        public void testSetKeyWithEmpty()
        {
            WaveUnitConfigElement e = new WaveUnitConfigElement();
            e.setKey("");
        }

        [Test, ExpectedException(ExpectedMessage = "key must not be null")]
        public void testSetKeyWithNull()
        {
            WaveUnitConfigElement e = new WaveUnitConfigElement();
            e.setKey(null);
        }

        [Test, ExpectedException(ExpectedMessage = "key must not contain \"" + WaveUnitConfigElement.SEPARATOR + "\"")]
        public void testSetKeyWithInvalidCharacter()
        {
            WaveUnitConfigElement e = new WaveUnitConfigElement();
            e.setKey("A" + WaveUnitConfigElement.SEPARATOR);
        }

        #endregion


        #region value関係のテスト

        [Test]
        public void testGetValue()
        {
            WaveUnitConfigElementStub e = new WaveUnitConfigElementStub();
            e.setNakedValue("Foo");
            Assert.AreEqual("Foo", e.getValue());

            e.setValue(null);
            Assert.NotNull(e.getValue());
            Assert.AreEqual("", e.getValue());
        }

        [Test, ExpectedException(ExpectedMessage = "value must not contain \"" + WaveUnitConfigElement.SEPARATOR + "\"")]
        public void testSetValueWithInvalidCharacter()
        {
            WaveUnitConfigElement e = new WaveUnitConfigElement();
            e.setValue("B" + WaveUnitConfigElement.SEPARATOR);
        }

        #endregion


        #region toStringのテスト

        [Test]
        public void testToString()
        {
            WaveUnitConfigElement e = new WaveUnitConfigElement();
            e.setKey("A");
            e.setValue("B");
            Assert.AreEqual("A:B", e.toString());
        }

        #endregion
    }
}
