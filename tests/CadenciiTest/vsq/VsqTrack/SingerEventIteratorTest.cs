using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using cadencii.vsq;

namespace cadencii.test.vsq.VsqTrack
{
    [TestFixture]
    class SingerEventIteratorTest : cadencii.vsq.VsqTrack
    {
        private VsqEventList fixture;

        [SetUp]
        public void setUp()
        {
            fixture = new VsqEventList();
            {
                var item = new VsqEvent(0, new VsqID());
                item.ID.type = VsqIDType.Singer;
                fixture.add(item, 0);
            }
            {
                var item = new VsqEvent(480, new VsqID());
                item.ID.type = VsqIDType.Anote;
                fixture.add(item, 1);
            }
            {
                var item = new VsqEvent(480, new VsqID());
                item.ID.type = VsqIDType.Singer;
                fixture.add(item, 2);
            }
            {
                var item = new VsqEvent(1920, new VsqID());
                item.ID.type = VsqIDType.Singer;
                fixture.add(item, 3);
            }
        }

        [Test]
        public void testWithoutRange()
        {
            var iterator = new cadencii.vsq.VsqTrack.SingerEventIterator(fixture);
            Assert.True(iterator.hasNext());
            {
                var actual = iterator.next();
                Assert.AreEqual(0, actual.Clock);
                Assert.AreEqual(0, actual.InternalID);
                Assert.AreEqual(VsqIDType.Singer, actual.ID.type);
            }
            Assert.True(iterator.hasNext());
            {
                var actual = iterator.next();
                Assert.AreEqual(480, actual.Clock);
                Assert.AreEqual(2, actual.InternalID);
                Assert.AreEqual(VsqIDType.Singer, actual.ID.type);
            }
            Assert.True(iterator.hasNext());
            {
                var actual = iterator.next();
                Assert.AreEqual(1920, actual.Clock);
                Assert.AreEqual(3, actual.InternalID);
                Assert.AreEqual(VsqIDType.Singer, actual.ID.type);
            }
            Assert.False(iterator.hasNext());
            Assert.Null(iterator.next());
        }

        [Test]
        public void testWithRange()
        {
            var iterator = new cadencii.vsq.VsqTrack.SingerEventIterator(fixture, 1, 481);
            Assert.True(iterator.hasNext());
            {
                var actual = iterator.next();
                Assert.AreEqual(480, actual.Clock);
                Assert.AreEqual(2, actual.InternalID);
                Assert.AreEqual(VsqIDType.Singer, actual.ID.type);
            }
            Assert.False(iterator.hasNext());
            Assert.Null(iterator.next());
        }

        [Test]
        public void testWithStart()
        {
            var iterator = new cadencii.vsq.VsqTrack.SingerEventIterator(fixture, 481);
            Assert.True(iterator.hasNext());
            {
                var actual = iterator.next();
                Assert.AreEqual(1920, actual.Clock);
                Assert.AreEqual(3, actual.InternalID);
                Assert.AreEqual(VsqIDType.Singer, actual.ID.type);
            }
            Assert.False(iterator.hasNext());
            Assert.Null(iterator.next());
        }
    }
}
