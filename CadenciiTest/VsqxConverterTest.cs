using System;
using System.IO;
using org.kbinani.vsq;
using org.kbinani.cadencii;
using org.kbinani.xml;
using NUnit.Framework;

namespace org.kbinani.cadencii
{
    [TestFixture]
    class VsqxConverterTest
    {
        [Test]
        public void readFromVsqx()
        {
            VsqFile vsq = VsqxConverter.readFromVsqx( "./fixture/track1.vsqx" );

            // トラック数
            Assert.AreEqual( 2, vsq.Track.size() );

            // プリメジャー
            Assert.AreEqual( 4, vsq.getPreMeasure() );

            // イベント数
            VsqTrack track = vsq.Track.get( 1 );
            Assert.AreEqual( 4, track.getEventCount() );

            // 歌手変更が正しく読み込まれているか
            // 1個目はデフォルトの歌手変更なのでスルー
            var singerChange = track.getEvent( 1 );
            Assert.AreEqual( 7680, singerChange.Clock );
            Assert.Null( singerChange.ID.IconDynamicsHandle );
            Assert.Null( singerChange.ID.LyricHandle );
            Assert.Null( singerChange.ID.NoteHeadHandle );
            Assert.Null( singerChange.ID.VibratoHandle );
            Assert.AreEqual( "VY1V3", singerChange.ID.IconHandle.IDS );
            Assert.AreEqual( "$07010000", singerChange.ID.IconHandle.IconID );
            Assert.AreEqual( 0, singerChange.ID.IconHandle.Language );
            Assert.AreEqual( 0, singerChange.ID.IconHandle.Program );
            
            // 1つめの音符イベントが正しく読み込まれているか
            var firstEvent = track.getEvent( 2 );
            Assert.AreEqual( 7680 + 0, firstEvent.Clock );
            Assert.AreEqual( 48, firstEvent.ID.Note );
            Assert.AreEqual( 480, firstEvent.ID.getLength() );

            Assert.AreEqual( 62, firstEvent.ID.Dynamics );
            Assert.AreEqual( 50, firstEvent.ID.DEMaccent );
            Assert.AreEqual( 8, firstEvent.ID.PMBendDepth );
            Assert.AreEqual( 0, firstEvent.ID.PMBendLength );
            Assert.AreEqual( 50, firstEvent.ID.DEMdecGainRate );
            Assert.AreEqual( false, firstEvent.ID.isFallPortamento() );
            Assert.AreEqual( false, firstEvent.ID.isRisePortamento() );
            
            Assert.Null( firstEvent.ID.IconDynamicsHandle );

            Assert.AreEqual( "わ", firstEvent.ID.LyricHandle.L0.Phrase );
            Assert.AreEqual( "w a", firstEvent.ID.LyricHandle.L0.getPhoneticSymbol() );
            Assert.AreEqual( true, firstEvent.ID.LyricHandle.L0.PhoneticSymbolProtected );
            Assert.Null( firstEvent.ID.NoteHeadHandle );

            Assert.AreEqual( "$04040000", firstEvent.ID.VibratoHandle.IconID );
            Assert.AreEqual( 316, firstEvent.ID.VibratoHandle.getLength() );
            var depthBP = firstEvent.ID.VibratoHandle.getDepthBP();
            Assert.AreEqual( 1, depthBP.getCount() );
            Assert.AreEqual( 0.0f, depthBP.getElement( 0 ).X );
            Assert.AreEqual( 64, depthBP.getElement( 0 ).Y );
            var rateBP = firstEvent.ID.VibratoHandle.getRateBP();
            Assert.AreEqual( 1, rateBP.getCount() );
            Assert.AreEqual( 0.0f, rateBP.getElement( 0 ).X );
            Assert.AreEqual( 50, rateBP.getElement( 0 ).Y );
            Assert.AreEqual( 164, firstEvent.ID.VibratoDelay );

            Assert.Null( firstEvent.ID.IconHandle );

            // 2つめの音符イベントが正しく読み込まれているか
            var secondEvent = track.getEvent( 3 );
            Assert.AreEqual( 7680 + 480, secondEvent.Clock );
            Assert.AreEqual( 50, secondEvent.ID.Note );
            Assert.AreEqual( 960, secondEvent.ID.getLength() );
            Assert.AreEqual( 63, secondEvent.ID.Dynamics );
            Assert.AreEqual( 50, secondEvent.ID.DEMaccent );
            Assert.AreEqual( 8, secondEvent.ID.PMBendDepth );
            Assert.AreEqual( 0, secondEvent.ID.PMBendLength );
            Assert.AreEqual( 50, secondEvent.ID.DEMdecGainRate );
            Assert.AreEqual( true, secondEvent.ID.isFallPortamento() );
            Assert.AreEqual( false, secondEvent.ID.isRisePortamento() );
            
            Assert.Null( secondEvent.ID.IconDynamicsHandle );
            Assert.AreEqual( "は", secondEvent.ID.LyricHandle.L0.Phrase );
            Assert.AreEqual( "h a", secondEvent.ID.LyricHandle.L0.getPhoneticSymbol() );
            Assert.AreEqual( false, secondEvent.ID.LyricHandle.L0.PhoneticSymbolProtected );
            Assert.Null( secondEvent.ID.NoteHeadHandle );

            Assert.AreEqual( "$04040004", secondEvent.ID.VibratoHandle.IconID );
            Assert.AreEqual( 624, secondEvent.ID.VibratoHandle.getLength() );
            depthBP = secondEvent.ID.VibratoHandle.getDepthBP();
            Assert.AreEqual( 1, depthBP.getCount() );
            Assert.AreEqual( 0.0f, depthBP.getElement( 0 ).X );
            Assert.AreEqual( 64, depthBP.getElement( 0 ).Y );
            rateBP = secondEvent.ID.VibratoHandle.getRateBP();
            Assert.AreEqual( 1, secondEvent.ID.VibratoHandle.getRateBP().getCount() );
            Assert.AreEqual( 0.0f, rateBP.getElement( 0 ).X );
            Assert.AreEqual( 64, rateBP.getElement( 0 ).Y );
            Assert.AreEqual( 336, secondEvent.ID.VibratoDelay );

            Assert.Null( secondEvent.ID.IconHandle );

            // トラック名
            Assert.AreEqual( "Track", track.getName() );

            // テンポ変更
            Assert.AreEqual( 2, vsq.TempoTable.size() );
            Assert.AreEqual( 0, vsq.TempoTable.get( 0 ).Clock );
            Assert.AreEqual( 500000, vsq.TempoTable.get( 0 ).Tempo );
            Assert.AreEqual( 8640, vsq.TempoTable.get( 1 ).Clock );
            Assert.AreEqual( 1199760, vsq.TempoTable.get( 1 ).Tempo );

            // 拍子変更
            Assert.AreEqual( 2, vsq.TimesigTable.size() );
            Assert.AreEqual( 0, vsq.TimesigTable.get( 0 ).Clock );
            Assert.AreEqual( 4, vsq.TimesigTable.get( 0 ).Numerator );
            Assert.AreEqual( 4, vsq.TimesigTable.get( 0 ).Denominator );
            Assert.AreEqual( 9600, vsq.TimesigTable.get( 1 ).Clock );
            Assert.AreEqual( 3, vsq.TimesigTable.get( 1 ).Numerator );
            Assert.AreEqual( 4, vsq.TimesigTable.get( 1 ).Denominator );

            // コントロールカーブ
            // DYN
            var dyn = track.getCurve( "DYN" );
            Assert.AreEqual( 1, dyn.size() );
            Assert.AreEqual( 720, dyn.getKeyClock( 0 ) );
            Assert.AreEqual( 96, dyn.getElement( 0 ) );

            // BRE
            var bre = track.getCurve( "BRE" );
            Assert.AreEqual( 1, bre.size() );
            Assert.AreEqual( 720, bre.getKeyClock( 0 ) );
            Assert.AreEqual( 102, bre.getElement( 0 ) );

            // BRI

            // CLE

            // OPE
            var ope = track.getCurve( "OPE" );
            Assert.AreEqual( 2, ope.size() );
            Assert.AreEqual( 7680 + 0, ope.getKeyClock( 0 ) );
            Assert.AreEqual( 127, ope.getElement( 0 ) );
            Assert.AreEqual( 7680 + 480, ope.getKeyClock( 1 ) );
            Assert.AreEqual( 127, ope.getElement( 1 ) );

            // GEN

            // POR

            // PIT

            // PBS

            // Mixerが正しく読み込まれているか
            Assert.AreEqual( 2, vsq.Mixer.MasterFeder );

            Assert.AreEqual( 1, vsq.Mixer.Slave.size() );
            Assert.AreEqual( 1, vsq.Mixer.Slave.get( 0 ).Solo );
            Assert.AreEqual( 0, vsq.Mixer.Slave.get( 0 ).Mute );
            Assert.AreEqual( 64, vsq.Mixer.Slave.get( 0 ).Panpot );
            Assert.AreEqual( 1, vsq.Mixer.Slave.get( 0 ).Feder );
        }
    }
}
