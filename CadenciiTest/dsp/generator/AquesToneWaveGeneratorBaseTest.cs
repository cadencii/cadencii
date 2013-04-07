using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using com.github.cadencii;

namespace cadencii.test.dsp.generator
{
    [TestFixture]
    class AquesToneWaveGeneratorBaseTest
    {
        [Test]
        public void testEventQueueSequence()
        {
            var sequence = new EventQueueSequence();

            {
                Assert.False( sequence.keyIterator().hasNext() );
            }

            {
                var queue = sequence.get( 0 );
                var iterator = sequence.keyIterator();
                Assert.True( iterator.hasNext() );
                Assert.AreEqual( 0, iterator.next() );
                Assert.False( iterator.hasNext() );
            }
        }
    }
}
