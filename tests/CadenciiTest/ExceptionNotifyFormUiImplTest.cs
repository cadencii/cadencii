using System;
using NUnit.Framework;
using cadencii;

namespace cadencii
{
    [TestFixture]
    public class ExceptionNotifyFormUiImplTest : ExceptionNotifyFormUiImpl
    {
        public ExceptionNotifyFormUiImplTest()
            : base(new ExceptionNotifyFormController())
        {
        }

        [Test]
        public void testSetCancelButtonText()
        {
            string expected = "キャンセル";
            this.setCancelButtonText(expected);
            Assert.AreEqual(expected, base.buttonCancel.Text);
        }

        [Test]
        public void testSetSendButtonText()
        {
            string expected = "送信";
            this.setSendButtonText(expected);
            Assert.AreEqual(expected, base.buttonSend.Text);
        }

        [Test]
        public void testSetTitle()
        {
            string expected = "たいとる";
            this.setTitle(expected);
            Assert.AreEqual(expected, base.Text);
        }
    }
}
