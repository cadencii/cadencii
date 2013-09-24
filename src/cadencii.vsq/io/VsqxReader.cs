/*
 * VsqxReader.cs
 * Copyright © 2011,2013 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package cadencii.vsq;

public class VsqxConverter
{
    public static VsqFile readFromVsqx( String filePath )
        throws UnsupportedOperationException
    {
        throw new UnsupportedOperationException();
    }
}

#else

using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace cadencii.vsq.io
{

    /// <summary>
    /// VSQXファイルを読み込むクラス
    /// </summary>
    public class VsqxReader
    {
        /// <summary>
        /// vsqxファイルを読み込み，新しいシーケンスオブジェクトを生成する
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <exception cref="System.Exception">読み込みに失敗した時スローされる</exception>
        /// <returns>生成したシーケンスオブジェクト</returns>
        public static VsqFile readFromVsqx( string filePath )
        {
            if ( filePath == null ) {
                throw new ArgumentNullException( "filePath" );
            }

            if ( false == File.Exists( filePath ) ) {
                throw new Exception( "file not found" );
            }

            var xml = new XmlDocument();
            xml.Load( filePath );

            // 音源テーブルを解釈
            var voiceTable = getVoiceTable( xml );

            // マスタートラックを解釈
            XmlElement masterTrack = xml.DocumentElement["masterTrack"];
            int preMeasure = int.Parse( masterTrack["preMeasure"].InnerText );
            VsqFile result = new VsqFile( "", preMeasure, 4, 4, 500000 );

            // テンポ変更を読み取る
            result.TempoTable.clear();
            foreach ( XmlNode node in masterTrack.GetElementsByTagName( "tempo" ) ) {
                int posTick = int.Parse( node["posTick"].InnerText );
                int bpm = int.Parse( node["bpm"].InnerText );
                int tempo = (int)(6000000000L / bpm);
                TempoTableEntry tempoEntry = new TempoTableEntry( posTick, tempo, 0.0 );
                result.TempoTable.add( tempoEntry );
            }
            result.TempoTable.updateTempoInfo();
            // 拍子変更を読み取る
            result.TimesigTable.clear();
            foreach ( XmlNode node in masterTrack.GetElementsByTagName( "timeSig" ) ) {
                int posMes = int.Parse( node["posMes"].InnerText );
                int numerator = int.Parse( node["nume"].InnerText );
                int denominator = int.Parse( node["denomi"].InnerText );
                TimeSigTableEntry timesigEntry = new TimeSigTableEntry( 0, numerator, denominator, posMes );
                result.TimesigTable.add( timesigEntry );
            }
            result.TimesigTable.updateTimesigInfo();

            // マスター以外のトラックを解釈
            foreach ( XmlNode node in xml.DocumentElement.GetElementsByTagName( "vsTrack" ) ) {
                int trackIndex = int.Parse( node["vsTrackNo"].InnerText ) + 1;
                VsqTrack track = null;
                if ( result.Track.size() <= trackIndex ) {
                    int amount = trackIndex + 1 - result.Track.size();
                    for ( int i = 0; i < amount; i++ ) {
                        result.Track.add( new VsqTrack( "", "" ) );
                    }
                }
                track = result.Track.get( trackIndex );
                track.setName( node["trackName"].InnerText );

                foreach ( XmlNode child in node.ChildNodes ) {
                    if ( child.Name == "musicalPart" ) {
                        parseMusicalPart( voiceTable, track, child );
                    }
                }
            }

            // MasterMixerをパース
            var mixer = xml.DocumentElement["mixer"];
            var masterUnit = mixer["masterUnit"];
            result.Mixer.MasterFeder = int.Parse( masterUnit["vol"].InnerText );
            result.Mixer.MasterMute = 0;
            result.Mixer.MasterPanpot = 64;

            // SlaveMixerをパース
            result.Mixer.Slave.clear();
            for ( int i = 1; i < result.Track.size(); i++ ) {
                result.Mixer.Slave.add( null );
            }
            foreach ( XmlNode vsUnit in mixer.GetElementsByTagName( "vsUnit" ) ) {
                int vsTrackNo = int.Parse( vsUnit["vsTrackNo"].InnerText );
                int mute = int.Parse( vsUnit["mute"].InnerText );
                int solo = int.Parse( vsUnit["solo"].InnerText );
                int pan = int.Parse( vsUnit["pan"].InnerText );
                int vol = int.Parse( vsUnit["vol"].InnerText );
                var slave = new VsqMixerEntry( vol, pan, mute, solo );
                result.Mixer.Slave.set( vsTrackNo, slave );
            }

            return result;
        }

        /// <summary>
        /// musicalPartを解釈し、パート内の情報をtrackに追加する
        /// </summary>
        /// <param name="voiceTable">音源情報のテーブル</param>
        /// <param name="track">追加先のトラック</param>
        /// <param name="musicalPart">解釈対象のmusicalPart</param>
        private static void parseMusicalPart( Dictionary<int, Dictionary<int, IconHandle>> voiceTable, VsqTrack track, XmlNode musicalPart )
        {
            int offset = int.Parse( musicalPart["posTick"].InnerText );

            // 歌手切り替え情報をパース
            foreach ( XmlNode singer in musicalPart.ChildNodes ) {
                if ( singer.Name != "singer" ) {
                    continue;
                }
                int posTick = int.Parse( singer["posTick"].InnerText );
                int bankSelect = int.Parse( singer["vBS"].InnerText );
                int programChange = int.Parse( singer["vPC"].InnerText );
                if ( voiceTable.ContainsKey( bankSelect ) && voiceTable[bankSelect].ContainsKey( programChange ) ) {
                    var iconHandle = voiceTable[bankSelect][programChange];
                    var item = new VsqEvent();
                    item.ID.IconHandle = (IconHandle)iconHandle.clone();
                    item.ID.type = VsqIDType.Singer;
                    item.Clock = offset + posTick;
                    track.addEvent( item );
                } else {
                    throw new Exception( "音源情報のparseに失敗しました。" );
                }
            }

            // ノート情報をパース
            foreach ( XmlNode note in musicalPart.ChildNodes ) {
                if ( note.Name != "note" ) {
                    continue;
                }
                var item = createNoteEvent( note, offset );
                track.addEvent( item );

                // OPEカーブを更新
                int ope = getOpening( note );
                var list = track.getCurve( "OPE" );
                list.add( item.Clock, ope );
            }

            // OPE以外のコントロールカーブをパース
            foreach ( XmlNode ctrl in musicalPart.ChildNodes ) {
                if ( ctrl.Name != "mCtrl" ) {
                    continue;
                }
                int posTick = int.Parse( ctrl["posTick"].InnerText );
                string id = ctrl["attr"].Attributes["id"].Value;
                int value = int.Parse( ctrl["attr"].InnerText );
                var list = track.getCurve( id );
                if ( list != null ) {
                    list.add( posTick, value );
                }
            }
        }

        /// <summary>
        /// noteを表すxml要素から、opening値を取得する
        /// </summary>
        /// <param name="node">note要素を表すxml要素</param>
        /// <returns>opening値</returns>
        private static int getOpening( XmlNode node )
        {
            var attributes = getNoteAttributes( node );
            if ( attributes.ContainsKey( "opening" ) ) {
                return attributes["opening"];
            } else {
                return 127;
            }
        }

        /// <summary>
        /// noteを表すxml要素から、attr要素の値を取得する
        /// </summary>
        /// <param name="note">note要素を表すxml要素</param>
        /// <returns>attrの要素名をキーとした値のリスト</returns>
        private static Dictionary<string, int> getNoteAttributes( XmlNode note )
        {
            var result = new Dictionary<string, int>();
            var noteStyle = note["noteStyle"];
            foreach ( XmlNode attr in noteStyle.GetElementsByTagName( "attr" ) ) {
                string id = attr.Attributes["id"].Value;
                int value = int.Parse( attr.InnerText );
                result[id] = value;
            }
            return result;
        }

        /// <summary>
        /// xml要素から音符イベントを生成する
        /// </summary>
        /// <param name="note">xml要素</param>
        /// <param name="tickOffset">指定したxml要素が所属しているmusicalPartの、オフセットtick数</param>
        /// <returns>生成した音符イベント</returns>
        private static VsqEvent createNoteEvent( XmlNode note, int tickOffset )
        {
            int posTick = int.Parse( note["posTick"].InnerText );
            VsqEvent item = new VsqEvent();
            item.Clock = posTick + tickOffset;
            item.ID = new VsqID();
            item.ID.type = VsqIDType.Anote;

            item.ID.LyricHandle = new LyricHandle();
            string lyric = note["lyric"].InnerText;
            XmlElement phnmsElement = note["phnms"];
            string symbols = phnmsElement.InnerText;
            bool symbolsProtected = false;
            if ( phnmsElement.HasAttribute( "lock" ) ) {
                int value = int.Parse( phnmsElement.Attributes["lock"].Value );
                symbolsProtected = value == 1;
            }
            item.ID.LyricHandle.L0.PhoneticSymbolProtected = symbolsProtected;

            item.ID.LyricHandle.L0.Phrase = lyric;
            item.ID.LyricHandle.L0.setPhoneticSymbol( symbols );

            item.ID.Note = int.Parse( note["noteNum"].InnerText );
            item.ID.setLength( int.Parse( note["durTick"].InnerText ) );
            item.ID.Dynamics = int.Parse( note["velocity"].InnerText );

            var attributes = getNoteAttributes( note );
            if ( attributes.ContainsKey( "accent" ) ) {
                item.ID.DEMaccent = attributes["accent"];
            }
            if ( attributes.ContainsKey( "bendDep" ) ) {
                item.ID.PMBendDepth = attributes["bendDep"];
            }
            if ( attributes.ContainsKey( "bendLen" ) ) {
                item.ID.PMBendLength = attributes["bendLen"];
            }
            if ( attributes.ContainsKey( "decay" ) ) {
                item.ID.DEMdecGainRate = attributes["decay"];
            }
            if ( attributes.ContainsKey( "fallPort" ) ) {
                item.ID.setFallPortamento( attributes["fallPort"] == 1 );
            }
            if ( attributes.ContainsKey( "risePort" ) ) {
                item.ID.setRisePortamento( attributes["risePort"] == 1 );
            }

            // vibrato
            if ( attributes.ContainsKey( "vibLen" ) && attributes.ContainsKey( "vibType" ) ) {
                int lengthPercentage = attributes["vibLen"];
                int vibratoType = attributes["vibType"] - 1;
                if ( lengthPercentage > 0 ) {
                    var vibratoHandle = new VibratoHandle();
                    int length = item.ID.getLength();
                    int duration = (int)(length * (lengthPercentage / 100.0));
                    vibratoHandle.setLength( duration );
                    item.ID.VibratoDelay = length - duration;
                    vibratoHandle.IconID = "$0404" + vibratoType.ToString("X4");

                    double delayRatio = (double)(length - duration) / (double)length;
                    // VibDepth
                    vibratoHandle.setDepthBP( getVibratoCurve( note, "vibDep", delayRatio ) );
                    // VibRate
                    vibratoHandle.setRateBP( getVibratoCurve( note, "vibRate", delayRatio ) );

                    item.ID.VibratoHandle = vibratoHandle;
                }
            }
            return item;
        }

        /// <summary>
        /// noteを表現するxml要素からVirbatoBPListを取得する
        /// </summary>
        /// <param name="note">xml要素</param>
        /// <param name="type">取得するVibratoBPListのタイプ。vibDepまたはvibRateを指定する</param>
        /// <param name="delayRatio">音符の長さに対する、音符の先頭位置からビブラート開始位置までの距離の比率</param>
        private static VibratoBPList getVibratoCurve( XmlNode note, string type, double delayRatio )
        {
            List<float> x = new List<float>();
            List<int> y = new List<int>();

            XmlElement noteStyle = note["noteStyle"];
            foreach ( XmlNode seqAttr in noteStyle.GetElementsByTagName( "seqAttr" ) ) {
                string id = seqAttr.Attributes["id"].Value;
                if ( id == type ) {
                    foreach ( XmlNode elem in seqAttr.ChildNodes ) {
                        if ( elem.Name == "elem" ) {
                            int posNrm = int.Parse( elem["posNrm"].InnerText );
                            int elv = int.Parse( elem["elv"].InnerText );
                            double pos = posNrm / 65535.0;
                            float actualPos = (float)(pos - delayRatio);
                            if ( actualPos < 0.0f ) {
                                actualPos = 0.0f;
                            } else if ( 1.0f < actualPos ) {
                                actualPos = 1.0f;
                            }
                            x.Add( actualPos );
                            y.Add( elv );
                        }
                    }
                }
            }

            return new VibratoBPList( x.ToArray(), y.ToArray() );
        }

        /// <summary>
        /// vsqxのxmlドキュメントに埋め込まれた音源情報のテーブルを取得する
        /// </summary>
        /// <param name="xml">xmlドキュメント</param>
        /// <returns>バンクセレクト、プログラムチェンジをキーとしたIconHandleのマップ</returns>
        private static Dictionary<int, Dictionary<int, IconHandle>> getVoiceTable( XmlDocument xml )
        {
            var result = new Dictionary<int, Dictionary<int, IconHandle>>();
            foreach ( XmlNode vVoice in xml.DocumentElement.GetElementsByTagName( "vVoice" ) ) {
                int bankSelect = int.Parse( vVoice["vBS"].InnerText );
                int programChange = int.Parse( vVoice["vPC"].InnerText );
                var name = vVoice["vVoiceName"].InnerText;
                var iconHandle = new IconHandle();
                iconHandle.IDS = name;
                iconHandle.Language = bankSelect;
                iconHandle.Program = programChange;
                iconHandle.IconID =
                    "$0701" + bankSelect.ToString("X2") + programChange.ToString("X2");
                if ( false == result.ContainsKey( bankSelect ) ) {
                    result.Add( bankSelect, new Dictionary<int, IconHandle>() );
                }
                result[bankSelect].Add( programChange, iconHandle );
            }
            return result;
        }
    }
}
#endif
