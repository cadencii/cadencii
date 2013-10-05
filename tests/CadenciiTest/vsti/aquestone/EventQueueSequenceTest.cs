using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq;
using cadencii;

namespace cadencii.test.dsp.generator
{
    [TestFixture]
    class EventQueueSequenceTest
    {
        [Test]
        public void test()
        {
            var sequence = new EventQueueSequence();

            {
                Assert.False(sequence.keyIterator().Count() == 0);
            }

            {
                var queue = sequence.get(0);
                var iterator = sequence.keyIterator();
                Assert.True(iterator.Count() == 1);
                Assert.AreEqual(0, iterator.First());
            }
        }
    }
}
