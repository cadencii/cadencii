using System;
using System.Windows.Forms;
using NUnit.Framework;
using org.kbinani.cadencii;

namespace org.kbinani.cadencii
{
    [TestFixture]
    public class ExceptionNotifyFormControllerTest : ExceptionNotifyFormController
    {
        [Test]
        public void testSetReportTarget()
        {
            Assert.AreEqual( Environment.OSVersion.ToString(), base.getSystemInfo() );
        }

        [Test]
        public void testExtractMessageString()
        {
            Exception ex = this.prepareException();
            string actual = base.extractMessageString( ex, 0 );
            Assert.AreEqual( "[exception-0] " + ex.Message + "\r\n" + ex.StackTrace + "\r\n", actual );
        }

        //[Test]
        public void test()
        {
            this.setReportTarget( this.prepareException() );
            this.getUi().showDialog( null );
        }

        private Exception prepareException()
        {
            try {
                int.Parse( "XYZ" );
            } catch ( Exception ex ) {
                return ex;
            }
            return null;
        }
    }
}
