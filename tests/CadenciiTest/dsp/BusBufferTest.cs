using System;
using System.Linq;
using cadencii;
using cadencii.dsp.v2;
using NUnit.Framework;

namespace cadencii.test.dsp
{
    [TestFixture]
    class BusBufferTest
    {
        class BusBufferStub : BusBuffer
        {
            public BusBufferStub(int channel, int buffer_size)
                : base(channel, buffer_size)
            { }

            public float[] getBuffer()
            {
                return buffer_;
            }
        }

        [Test]
        public void zeroFillTest()
        {
            var buffer = new BusBufferStub(2, 412);

            // fill dummy data
            const float DUMMY_DATA = 0.4f;
            float[] raw_buffer = buffer.getBuffer();
            Assert.True(raw_buffer.All((_1) => _1 == 0.0f));
            for (int i = 0; i < raw_buffer.Length; ++i) {
                raw_buffer[i] = DUMMY_DATA;
            }

            buffer.zeroFill(1, 410);

            Assert.AreEqual(raw_buffer[0], DUMMY_DATA);
            Assert.AreEqual(raw_buffer[1], DUMMY_DATA);
            Assert.True(raw_buffer.Skip(2).Take(410 * 2).All((_1) => _1 == 0.0f));
            Assert.AreEqual(raw_buffer[raw_buffer.Length - 2], DUMMY_DATA);
            Assert.AreEqual(raw_buffer[raw_buffer.Length - 1], DUMMY_DATA);
        }

        [Test]
        public void zeroFillWithZeroLengthTest()
        {
            var buffer = new BusBuffer(2, 100);
            Assert.DoesNotThrow(() => buffer.zeroFill(0, 0));
            Assert.DoesNotThrow(() => buffer.zeroFill(0, -1));
        }

        [Test]
        public void mixFromTest()
        {
            const int CHANNEL = 2;
            const int LENGTH = 100;

            var buffer = new BusBufferStub(CHANNEL, LENGTH);
            var raw_buffer = buffer.getBuffer();
            fillRandomData(raw_buffer);

            var source = new BusBufferStub(CHANNEL, LENGTH);
            var source_raw_buffer = source.getBuffer();
            fillRandomData(source_raw_buffer);

            const int SOURCE_OFFSET = 10;
            const int OFFSET = 20;
            const int MIX_LENGTH = 10;

            float[] expected = new float[CHANNEL * LENGTH];
            for (int i = 0; i < raw_buffer.Length; ++i) {
                expected[i] = raw_buffer[i];
            }
            for (int i = 0; i < MIX_LENGTH; ++i) {
                for (int ch = 0; ch < CHANNEL; ++ch) {
                    expected[i * CHANNEL + OFFSET * CHANNEL + ch] += source_raw_buffer[i * CHANNEL + SOURCE_OFFSET * CHANNEL + ch];
                }
            }

            buffer.mixFrom(source, SOURCE_OFFSET, OFFSET, MIX_LENGTH);
            float[] actual = buffer.getBuffer();

            bool all_equals = true;
            for (int i = 0; i < expected.Length; ++i) {
                if (expected[i] != actual[i]) {
                    all_equals = false;
                    break;
                }
            }
            Assert.True(all_equals);
        }

        private void fillRandomData(float[] data)
        {
            var rand = new Random();
            for (int i = 0; i < data.Length; ++i) {
                data[i] = (float)rand.NextDouble();
            }
        }
    }
}
