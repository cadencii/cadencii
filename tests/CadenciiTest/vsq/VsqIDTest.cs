using System;
using System.IO;
using cadencii.vsq;
using cadencii;
using cadencii.xml;
using NUnit.Framework;

namespace cadencii
{
    [TestFixture]
    class VsqIDTest
    {
        [Test]
        public void getPortamentoUsage()
        {
            VsqID id = new VsqID();
            id.PMbPortamentoUse = 0;
            Assert.AreEqual(false, id.isRisePortamento());
            Assert.AreEqual(false, id.isFallPortamento());

            id.PMbPortamentoUse = 1;
            Assert.AreEqual(true, id.isRisePortamento());
            Assert.AreEqual(false, id.isFallPortamento());

            id.PMbPortamentoUse = 2;
            Assert.AreEqual(false, id.isRisePortamento());
            Assert.AreEqual(true, id.isFallPortamento());

            id.PMbPortamentoUse = 3;
            Assert.AreEqual(true, id.isRisePortamento());
            Assert.AreEqual(true, id.isFallPortamento());
        }

        [Test]
        public void setPortamentoUsage()
        {
            VsqID id = new VsqID();
            id.setRisePortamento(false);
            id.setFallPortamento(false);
            Assert.AreEqual(0, id.PMbPortamentoUse);

            id.setRisePortamento(true);
            id.setFallPortamento(false);
            Assert.AreEqual(1, id.PMbPortamentoUse);

            id.setRisePortamento(false);
            id.setFallPortamento(true);
            Assert.AreEqual(2, id.PMbPortamentoUse);

            id.setRisePortamento(true);
            id.setFallPortamento(true);
            Assert.AreEqual(3, id.PMbPortamentoUse);
        }
    }
}
