using System;
using System.Windows.Forms;
using NUnit.Framework;
using cadencii;

namespace cadencii
{
    [TestFixture]
    public class strTest
    {
        [Test]
        public void replace()
        {
            Assert.AreEqual( "abcde", str.replace( "[bcde", "[", "a" ) );
            Assert.AreEqual( "abc\ndef", str.replace( "abc\n\ndef",  "\n\n", "\n" ) );
            Assert.Null( str.replace( null, "", "" ) );
        }
    }
}
