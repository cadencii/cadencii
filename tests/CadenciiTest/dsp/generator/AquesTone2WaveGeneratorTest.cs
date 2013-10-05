using NUnit.Framework;
using cadencii;
using cadencii.vsq;
using System;
using System.Collections.Generic;

namespace cadencii.test.dsp.generator
{
    class AquesTone2WaveGeneratorStub : AquesTone2WaveGenerator
    {
        public AquesTone2WaveGeneratorStub()
            : base(new AquesTone2Driver(""))
        {
        }

        public EventQueueSequence generateMidiEvent(VsqFileEx vsq, int track)
        {
            return base.generateMidiEvent(vsq, track);
        }

        public void reflectVibratoPitch(VsqEvent item, VsqBPList pitchBend, VsqBPList pitchBendSensitivity, TempoVector tempoTable)
        {
            base.reflectNoteEventPitch(item, pitchBend, pitchBendSensitivity, tempoTable);
        }
    }

    [TestFixture]
    class AquesTone2WaveGeneratorTest
    {
        [Test]
        public void testReflectVibratoPitch()
        {
            var tempoTable = new TempoVector();
            var pitchBend = new VsqBPList(CurveType.PIT.getName(), CurveType.PIT.getDefault(), CurveType.PIT.getMinimum(), CurveType.PIT.getMaximum());
            var pitchBendSensitivity = new VsqBPList(CurveType.PIT.getName(), CurveType.PIT.getDefault(), CurveType.PIT.getMinimum(), CurveType.PIT.getMaximum());

            pitchBend.add(0, 8191);
            pitchBendSensitivity.add(0, 2);

            var generator = new AquesTone2WaveGeneratorStub();
            var item = new VsqEvent(0, new VsqID());
            item.ID.type = VsqIDType.Anote;
            item.ID.VibratoHandle = new VibratoHandle();
            item.ID.VibratoHandle.StartRate = 0x40;
            item.ID.VibratoHandle.RateBP.clear();
            item.ID.VibratoHandle.StartDepth = 0x40;
            item.ID.VibratoHandle.DepthBP.clear();
            item.ID.setLength(480);
            item.ID.VibratoDelay = 430;
            item.ID.VibratoHandle.setLength(50);
            generator.reflectVibratoPitch(item, pitchBend, pitchBendSensitivity, tempoTable);

            {
                var expectedPit = new Dictionary<int, int> {
                    { 0, 8191 },
                    { 430, 5461 }, { 431, 5467 }, { 432, 5484 }, { 433, 5514 }, { 434, 5555 },
                    { 435, 5608 }, { 436, 5672 }, { 437, 5748 }, { 438, 5835 }, { 439, 5932 },
                    { 440, 6041 }, { 441, 6096 }, { 442, 6151 }, { 443, 6205 }, { 444, 6257 },
                    { 445, 6309 }, { 446, 6360 }, { 447, 6410 }, { 448, 6459 }, { 449, 6507 },
                    { 450, 6553 }, { 451, 6598 }, { 452, 6642 }, { 453, 6684 }, { 454, 6725 },
                    { 455, 6764 }, { 456, 6802 }, { 457, 6838 }, { 458, 6873 }, { 459, 6906 },
                    { 460, 6937 }, { 461, 6967 }, { 462, 6994 }, { 463, 7020 }, { 464, 7044 },
                    { 465, 7066 }, { 466, 7087 }, { 467, 7105 }, { 468, 7121 }, { 469, 7136 },
                    { 470, 7148 }, { 471, 6989 }, { 472, 6826 }, { 473, 6660 }, { 474, 6491 },
                    { 475, 6320 }, { 476, 6149 }, { 477, 5976 }, { 478, 5804 }, { 479, 5632 },
                    { 480, 8191 }
                };
                Assert.AreEqual(expectedPit.Count, pitchBend.size());
                int i = 0;
                foreach (var pitInfo in expectedPit) {
                    Assert.AreEqual(pitInfo.Key, pitchBend.getKeyClock(i));
                    Assert.AreEqual(pitInfo.Value, pitchBend.getElementA(i));
                    ++i;
                }
            }

            {
                var expectedPbs = new Dictionary<int, int> {
                    { 0, 2 }, { 430, 3 }, { 480, 2 }
                };
                Assert.AreEqual(expectedPbs.Count, pitchBendSensitivity.size());
                int i = 0;
                foreach (var pbsInfo in expectedPbs) {
                    Assert.AreEqual(pbsInfo.Key, pitchBendSensitivity.getKeyClock(i));
                    Assert.AreEqual(pbsInfo.Value, pitchBendSensitivity.getElementA(i));
                    ++i;
                }
            }
        }
    }

}
