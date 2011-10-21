using System;
using System.IO;
using org.kbinani.vsq;
using org.kbinani.cadencii;
using org.kbinani.xml;
using NUnit.Framework;

namespace org.kbinani.cadencii
{
    [TestFixture]
    class VsqFileExTest
    {
        [Test]
        public void readFromVsqx()
        {
            //VsqFileEx vsq = VsqFileEx.readFromVsqx( "./fixture/track1.vsqx" );
            VsqFileEx vsq = VsqFileEx.readFromVsqx( @"E:\Documents and Settings\kbinani\My Documents\svn\cadencii\Cadencii\trunk\CadenciiTest\fixture\track1.vsqx" );

            // トラック数
            Assert.AreEqual( 2, vsq.Track.size() );

            // イベント数
            VsqTrack track = vsq.Track.get( 1 );
            Assert.AreEqual( 3, track.getEventCount() );

            // 歌手変更が正しく読み込まれているか
            var singerChange = track.getEvent( 0 );
            Assert.AreEqual( 0, singerChange.Clock );
            Assert.Null( singerChange.ID.IconDynamicsHandle );
            Assert.Null( singerChange.ID.LyricHandle );
            Assert.Null( singerChange.ID.NoteHeadHandle );
            Assert.Null( singerChange.ID.VibratoHandle );
            //TODO: 歌手名
            
            // 1つめの音符イベントが正しく読み込まれているか
            var firstEvent = track.getEvent( 1 );
            //TODO: clock
            //Assert.AreEqual( 7680 + 0, firstEvent.Clock );
            Assert.Null( firstEvent.ID.IconDynamicsHandle );
            //TODO: 歌詞
            //Assert.AreEqual( "わ", firstEvent.ID.LyricHandle.L0.Phrase );
            //TODO: 発音記号
            //Assert.AreEqual( "w a", firstEvent.ID.LyricHandle.L0.getPhoneticSymbol() );
            Assert.Null( firstEvent.ID.NoteHeadHandle );
            //TODO: vibrato
            Assert.Null( firstEvent.ID.IconHandle );

            // 2つめの音符イベントが正しく読み込まれているか
            var secondEvent = track.getEvent( 2 );
            //TODO: clock
            //Assert.AreEqual( 7680 + 480, secondEvent.Clock );
            Assert.Null( firstEvent.ID.IconDynamicsHandle );
            //TODO: 歌詞
            //Assert.AreEqual( "は", firstEvent.ID.LyricHandle.L0.Phrase );
            //TODO: 発音記号
            //Assert.AreEqual( "h a", firstEvent.ID.LyricHandle.L0.getPhoneticSymbol() );
            Assert.Null( firstEvent.ID.NoteHeadHandle );
            //TODO: vibrato
            Assert.Null( firstEvent.ID.IconHandle );

            //TODO: テストが書きかけなので失敗するようにしてある
            Assert.Fail();

            // トラック名

            // テンポ変更

            // 拍子変更
        }
    }
}
