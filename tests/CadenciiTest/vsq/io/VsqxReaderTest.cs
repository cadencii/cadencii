using System;
using System.IO;
using cadencii.vsq;
using cadencii;
using cadencii.vsq.io;
using cadencii.xml;
using NUnit.Framework;

namespace cadencii
{
    [TestFixture]
    class VsqxReaderTest
    {
        [Test]
        public void readFromVsqxMultiTrack()
        {
            VsqFile vsq = VsqxReader.readFromVsqx("./fixture/track2.vsqx");

            Assert.AreEqual(3, vsq.Track.Count);

            // 1トラック目
            var track = vsq.Track[1];
            Assert.AreEqual(3, track.getEventCount());
            Assert.AreEqual(VsqIDType.Singer, track.getEvent(1).ID.type);
            Assert.AreEqual("VY1V3", track.getEvent(1).ID.IconHandle.IDS);
            Assert.AreEqual(VsqIDType.Anote, track.getEvent(2).ID.type);
            Assert.AreEqual("ど", track.getEvent(2).ID.LyricHandle.L0.Phrase);

            // 2トラック目
            track = vsq.Track[2];
            Assert.AreEqual(3, track.getEventCount());
            Assert.AreEqual(VsqIDType.Singer, track.getEvent(1).ID.type);
            Assert.AreEqual("VY1V3", track.getEvent(1).ID.IconHandle.IDS);
            Assert.AreEqual(VsqIDType.Anote, track.getEvent(2).ID.type);
            Assert.AreEqual("み", track.getEvent(2).ID.LyricHandle.L0.Phrase);
        }

        [Test]
        public void readFromVsqx()
        {
            VsqFile vsq = VsqxReader.readFromVsqx("./fixture/track1.vsqx");

            // トラック数
            Assert.AreEqual(2, vsq.Track.Count);

            // プリメジャー
            Assert.AreEqual(4, vsq.getPreMeasure());

            // イベント数
            // 最初のmusicalPartには歌手変更1個と音符2個
            // 2つ目のmusicalPartには歌手変更1個と音符1個が入っているはず
            VsqTrack track = vsq.Track[1];
            Assert.AreEqual(6, track.getEventCount());

            // 歌手変更が正しく読み込まれているか
            // 1個目はデフォルトの歌手変更なのでスルー
            var singerChange = track.getEvent(1);
            Assert.AreEqual(7680, singerChange.Clock);
            Assert.Null(singerChange.ID.IconDynamicsHandle);
            Assert.Null(singerChange.ID.LyricHandle);
            Assert.Null(singerChange.ID.NoteHeadHandle);
            Assert.Null(singerChange.ID.VibratoHandle);
            Assert.AreEqual("VY1V3", singerChange.ID.IconHandle.IDS);
            Assert.AreEqual("$07010000", singerChange.ID.IconHandle.IconID);
            Assert.AreEqual(0, singerChange.ID.IconHandle.Language);
            Assert.AreEqual(0, singerChange.ID.IconHandle.Program);

            // 1つめの音符イベントが正しく読み込まれているか
            var firstEvent = track.getEvent(2);
            Assert.AreEqual(7680 + 0, firstEvent.Clock);
            Assert.AreEqual(48, firstEvent.ID.Note);
            Assert.AreEqual(480, firstEvent.ID.getLength());

            Assert.AreEqual(62, firstEvent.ID.Dynamics);
            Assert.AreEqual(50, firstEvent.ID.DEMaccent);
            Assert.AreEqual(8, firstEvent.ID.PMBendDepth);
            Assert.AreEqual(0, firstEvent.ID.PMBendLength);
            Assert.AreEqual(50, firstEvent.ID.DEMdecGainRate);
            Assert.AreEqual(false, firstEvent.ID.isFallPortamento());
            Assert.AreEqual(false, firstEvent.ID.isRisePortamento());

            Assert.Null(firstEvent.ID.IconDynamicsHandle);

            Assert.AreEqual("わ", firstEvent.ID.LyricHandle.L0.Phrase);
            Assert.AreEqual("w a", firstEvent.ID.LyricHandle.L0.getPhoneticSymbol());
            Assert.AreEqual(true, firstEvent.ID.LyricHandle.L0.PhoneticSymbolProtected);
            Assert.Null(firstEvent.ID.NoteHeadHandle);

            Assert.AreEqual("$04040000", firstEvent.ID.VibratoHandle.IconID);
            Assert.AreEqual(316, firstEvent.ID.VibratoHandle.getLength());
            var depthBP = firstEvent.ID.VibratoHandle.getDepthBP();
            Assert.AreEqual(1, depthBP.getCount());
            Assert.AreEqual(0.0f, depthBP.getElement(0).X);
            Assert.AreEqual(64, depthBP.getElement(0).Y);
            var rateBP = firstEvent.ID.VibratoHandle.getRateBP();
            Assert.AreEqual(1, rateBP.getCount());
            Assert.AreEqual(0.0f, rateBP.getElement(0).X);
            Assert.AreEqual(50, rateBP.getElement(0).Y);
            Assert.AreEqual(164, firstEvent.ID.VibratoDelay);

            Assert.Null(firstEvent.ID.IconHandle);

            // 2つめの音符イベントが正しく読み込まれているか
            var secondEvent = track.getEvent(3);
            Assert.AreEqual(7680 + 480, secondEvent.Clock);
            Assert.AreEqual(50, secondEvent.ID.Note);
            Assert.AreEqual(960, secondEvent.ID.getLength());
            Assert.AreEqual(63, secondEvent.ID.Dynamics);
            Assert.AreEqual(50, secondEvent.ID.DEMaccent);
            Assert.AreEqual(8, secondEvent.ID.PMBendDepth);
            Assert.AreEqual(0, secondEvent.ID.PMBendLength);
            Assert.AreEqual(50, secondEvent.ID.DEMdecGainRate);
            Assert.AreEqual(true, secondEvent.ID.isFallPortamento());
            Assert.AreEqual(false, secondEvent.ID.isRisePortamento());

            Assert.Null(secondEvent.ID.IconDynamicsHandle);
            Assert.AreEqual("は", secondEvent.ID.LyricHandle.L0.Phrase);
            Assert.AreEqual("h a", secondEvent.ID.LyricHandle.L0.getPhoneticSymbol());
            Assert.AreEqual(false, secondEvent.ID.LyricHandle.L0.PhoneticSymbolProtected);
            Assert.Null(secondEvent.ID.NoteHeadHandle);

            Assert.AreEqual("$04040004", secondEvent.ID.VibratoHandle.IconID);
            Assert.AreEqual(624, secondEvent.ID.VibratoHandle.getLength());
            depthBP = secondEvent.ID.VibratoHandle.getDepthBP();
            Assert.AreEqual(1, depthBP.getCount());
            Assert.AreEqual(0.0f, depthBP.getElement(0).X);
            Assert.AreEqual(64, depthBP.getElement(0).Y);
            rateBP = secondEvent.ID.VibratoHandle.getRateBP();
            Assert.AreEqual(1, secondEvent.ID.VibratoHandle.getRateBP().getCount());
            Assert.AreEqual(0.0f, rateBP.getElement(0).X);
            Assert.AreEqual(64, rateBP.getElement(0).Y);
            Assert.AreEqual(336, secondEvent.ID.VibratoDelay);

            Assert.Null(secondEvent.ID.IconHandle);

            // 2つ目の歌手変更
            var singerChange2 = track.getEvent(4);
            Assert.AreEqual(10560, singerChange2.Clock);
            Assert.Null(singerChange2.ID.IconDynamicsHandle);
            Assert.Null(singerChange2.ID.LyricHandle);
            Assert.Null(singerChange2.ID.NoteHeadHandle);
            Assert.Null(singerChange2.ID.VibratoHandle);
            Assert.AreEqual("Miku(V2)", singerChange2.ID.IconHandle.IDS);
            Assert.AreEqual("$07010001", singerChange2.ID.IconHandle.IconID);
            Assert.AreEqual(0, singerChange2.ID.IconHandle.Language);
            Assert.AreEqual(1, singerChange2.ID.IconHandle.Program);

            // 3つめの音符イベントが正しく読み込まれているか
            var thirdEvent = track.getEvent(5);
            Assert.AreEqual(10560 + 665, thirdEvent.Clock);
            Assert.AreEqual(60, thirdEvent.ID.Note);
            Assert.AreEqual(480, thirdEvent.ID.getLength());
            Assert.AreEqual(64, thirdEvent.ID.Dynamics);
            Assert.AreEqual(50, thirdEvent.ID.DEMaccent);
            Assert.AreEqual(8, thirdEvent.ID.PMBendDepth);
            Assert.AreEqual(0, thirdEvent.ID.PMBendLength);
            Assert.AreEqual(50, thirdEvent.ID.DEMdecGainRate);
            Assert.AreEqual(false, thirdEvent.ID.isFallPortamento());
            Assert.AreEqual(false, thirdEvent.ID.isRisePortamento());

            Assert.Null(thirdEvent.ID.IconDynamicsHandle);
            Assert.AreEqual("a", thirdEvent.ID.LyricHandle.L0.Phrase);
            Assert.AreEqual("a", thirdEvent.ID.LyricHandle.L0.getPhoneticSymbol());
            Assert.AreEqual(false, thirdEvent.ID.LyricHandle.L0.PhoneticSymbolProtected);
            Assert.Null(thirdEvent.ID.NoteHeadHandle);

            Assert.AreEqual("$04040000", thirdEvent.ID.VibratoHandle.IconID);
            Assert.AreEqual(316, thirdEvent.ID.VibratoHandle.getLength());
            depthBP = thirdEvent.ID.VibratoHandle.getDepthBP();
            Assert.AreEqual(1, depthBP.getCount());
            Assert.AreEqual(0.0f, depthBP.getElement(0).X);
            Assert.AreEqual(64, depthBP.getElement(0).Y);
            rateBP = thirdEvent.ID.VibratoHandle.getRateBP();
            Assert.AreEqual(1, thirdEvent.ID.VibratoHandle.getRateBP().getCount());
            Assert.AreEqual(0.0f, rateBP.getElement(0).X);
            Assert.AreEqual(50, rateBP.getElement(0).Y);
            Assert.AreEqual(164, thirdEvent.ID.VibratoDelay);

            Assert.Null(thirdEvent.ID.IconHandle);

            // トラック名
            Assert.AreEqual("Track", track.getName());

            // テンポ変更
            Assert.AreEqual(2, vsq.TempoTable.Count);
            Assert.AreEqual(0, vsq.TempoTable[0].Clock);
            Assert.AreEqual(500000, vsq.TempoTable[0].Tempo);
            Assert.AreEqual(8640, vsq.TempoTable[1].Clock);
            Assert.AreEqual(1199760, vsq.TempoTable[1].Tempo);

            // 拍子変更
            Assert.AreEqual(2, vsq.TimesigTable.Count);
            Assert.AreEqual(0, vsq.TimesigTable[0].Clock);
            Assert.AreEqual(4, vsq.TimesigTable[0].Numerator);
            Assert.AreEqual(4, vsq.TimesigTable[0].Denominator);
            Assert.AreEqual(9600, vsq.TimesigTable[1].Clock);
            Assert.AreEqual(3, vsq.TimesigTable[1].Numerator);
            Assert.AreEqual(4, vsq.TimesigTable[1].Denominator);

            // コントロールカーブ
            // DYN
            var dyn = track.getCurve("DYN");
            Assert.AreEqual(1, dyn.size());
            Assert.AreEqual(720, dyn.getKeyClock(0));
            Assert.AreEqual(96, dyn.getElement(0));

            // BRE
            var bre = track.getCurve("BRE");
            Assert.AreEqual(1, bre.size());
            Assert.AreEqual(720, bre.getKeyClock(0));
            Assert.AreEqual(102, bre.getElement(0));

            // BRI

            // CLE

            // OPE
            var ope = track.getCurve("OPE");
            Assert.AreEqual(3, ope.size());
            Assert.AreEqual(7680 + 0, ope.getKeyClock(0));
            Assert.AreEqual(127, ope.getElement(0));
            Assert.AreEqual(7680 + 480, ope.getKeyClock(1));
            Assert.AreEqual(127, ope.getElement(1));
            Assert.AreEqual(10560 + 665, ope.getKeyClock(2));
            Assert.AreEqual(127, ope.getElement(2));

            // GEN

            // POR

            // PIT

            // PBS

            // Mixerが正しく読み込まれているか
            Assert.AreEqual(2, vsq.Mixer.MasterFeder);

            Assert.AreEqual(1, vsq.Mixer.Slave.Count);
            Assert.AreEqual(1, vsq.Mixer.Slave[0].Solo);
            Assert.AreEqual(0, vsq.Mixer.Slave[0].Mute);
            Assert.AreEqual(64, vsq.Mixer.Slave[0].Panpot);
            Assert.AreEqual(1, vsq.Mixer.Slave[0].Feder);
        }
    }
}
