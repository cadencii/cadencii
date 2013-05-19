/*
 * VsqxConverter.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
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

namespace cadencii.vsq
{

    /// <summary>
    /// VSQXファイルを読み込むクラスs
    /// </summary>
    public class VsqxConverter
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

            if ( false == fsys.isFileExists( filePath ) ) {
                throw new Exception( "file not found" );
            }

            var xml = new XmlDocument();
            xml.Load( filePath );

            // 音源テーブルを解釈
            var voiceTable = getVoiceTable( xml );

            // マスタートラックを解釈
            XmlElement masterTrack = xml.DocumentElement["masterTrack"];
            int preMeasure = str.toi( masterTrack["preMeasure"].InnerText );
            VsqFile result = new VsqFile( "", preMeasure, 4, 4, 500000 );

            // テンポ変更を読み取る
            result.TempoTable.clear();
            foreach ( XmlNode node in masterTrack.GetElementsByTagName( "tempo" ) ) {
                int posTick = str.toi( node["posTick"].InnerText );
                int bpm = str.toi( node["bpm"].InnerText );
                int tempo = (int)(6000000000L / bpm);
                TempoTableEntry tempoEntry = new TempoTableEntry( posTick, tempo, 0.0 );
                result.TempoTable.add( tempoEntry );
            }
            result.TempoTable.updateTempoInfo();
            // 拍子変更を読み取る
            result.TimesigTable.clear();
            foreach ( XmlNode node in masterTrack.GetElementsByTagName( "timeSig" ) ) {
                int posMes = str.toi( node["posMes"].InnerText );
                int numerator = str.toi( node["nume"].InnerText );
                int denominator = str.toi( node["denomi"].InnerText );
                TimeSigTableEntry timesigEntry = new TimeSigTableEntry( 0, numerator, denominator, posMes );
                result.TimesigTable.add( timesigEntry );
            }
            result.TimesigTable.updateTimesigInfo();

            // マスター以外のトラックを解釈
            foreach ( XmlNode node in xml.DocumentElement.GetElementsByTagName( "vsTrack" ) ) {
                int trackIndex = str.toi( node["vsTrackNo"].InnerText ) + 1;
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
                    if ( str.compare( child.Name, "musicalPart" ) ) {
                        parseMusicalPart( voiceTable, track, child );
                    }
                }
            }

            // MasterMixerをパース
            var mixer = xml.DocumentElement["mixer"];
            var masterUnit = mixer["masterUnit"];
            result.Mixer.MasterFeder = str.toi( masterUnit["vol"].InnerText );
            result.Mixer.MasterMute = 0;
            result.Mixer.MasterPanpot = 64;

            // SlaveMixerをパース
            result.Mixer.Slave.clear();
            for ( int i = 1; i < result.Track.size(); i++ ) {
                result.Mixer.Slave.add( null );
            }
            foreach ( XmlNode vsUnit in mixer.GetElementsByTagName( "vsUnit" ) ) {
                int vsTrackNo = str.toi( vsUnit["vsTrackNo"].InnerText );
                int mute = str.toi( vsUnit["mute"].InnerText );
                int solo = str.toi( vsUnit["solo"].InnerText );
                int pan = str.toi( vsUnit["pan"].InnerText );
                int vol = str.toi( vsUnit["vol"].InnerText );
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
            int offset = str.toi( musicalPart["posTick"].InnerText );

            // 歌手切り替え情報をパース
            foreach ( XmlNode singer in musicalPart.ChildNodes ) {
                if ( false == str.compare( singer.Name, "singer" ) ) {
                    continue;
                }
                int posTick = str.toi( singer["posTick"].InnerText );
                int bankSelect = str.toi( singer["vBS"].InnerText );
                int programChange = str.toi( singer["vPC"].InnerText );
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
                if ( false == str.compare( note.Name, "note" ) ) {
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
                if ( false == str.compare( ctrl.Name, "mCtrl" ) ) {
                    continue;
                }
                int posTick = str.toi( ctrl["posTick"].InnerText );
                string id = ctrl["attr"].Attributes["id"].Value;
                int value = str.toi( ctrl["attr"].InnerText );
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
            if ( dic.containsKey( attributes, "opening" ) ) {
                return dic.get( attributes, "opening" );
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
                int value = str.toi( attr.InnerText );
                dic.put( result, id, value );
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
            int posTick = str.toi( note["posTick"].InnerText );
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
                int value = str.toi( phnmsElement.Attributes["lock"].Value );
                symbolsProtected = value == 1;
            }
            item.ID.LyricHandle.L0.PhoneticSymbolProtected = symbolsProtected;

            item.ID.LyricHandle.L0.Phrase = lyric;
            item.ID.LyricHandle.L0.setPhoneticSymbol( symbols );

            item.ID.Note = str.toi( note["noteNum"].InnerText );
            item.ID.setLength( str.toi( note["durTick"].InnerText ) );
            item.ID.Dynamics = str.toi( note["velocity"].InnerText );

            var attributes = getNoteAttributes( note );
            if ( dic.containsKey( attributes, "accent" ) ) {
                item.ID.DEMaccent = attributes["accent"];
            }
            if ( dic.containsKey( attributes, "bendDep" ) ) {
                item.ID.PMBendDepth = attributes["bendDep"];
            }
            if ( dic.containsKey( attributes, "bendLen" ) ) {
                item.ID.PMBendLength = attributes["bendLen"];
            }
            if ( dic.containsKey( attributes, "decay" ) ) {
                item.ID.DEMdecGainRate = attributes["decay"];
            }
            if ( dic.containsKey( attributes, "fallPort" ) ) {
                item.ID.setFallPortamento( attributes["fallPort"] == 1 );
            }
            if ( dic.containsKey( attributes, "risePort" ) ) {
                item.ID.setRisePortamento( attributes["risePort"] == 1 );
            }

            // vibrato
            if ( dic.containsKey( attributes, "vibLen" ) && dic.containsKey( attributes, "vibType" ) ) {
                int lengthPercentage = dic.get( attributes, "vibLen" );
                int vibratoType = dic.get( attributes, "vibType" ) - 1;
                if ( lengthPercentage > 0 ) {
                    var vibratoHandle = new VibratoHandle();
                    int length = item.ID.getLength();
                    int duration = (int)(length * (lengthPercentage / 100.0));
                    vibratoHandle.setLength( duration );
                    item.ID.VibratoDelay = length - duration;
                    vibratoHandle.IconID = "$0404" + str.format( vibratoType, 4, 16 );

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
                if ( str.compare( id, type ) ) {
                    foreach ( XmlNode elem in seqAttr.ChildNodes ) {
                        if ( str.compare( elem.Name, "elem" ) ) {
                            int posNrm = str.toi( elem["posNrm"].InnerText );
                            int elv = str.toi( elem["elv"].InnerText );
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
                int bankSelect = str.toi( vVoice["vBS"].InnerText );
                int programChange = str.toi( vVoice["vPC"].InnerText );
                var name = vVoice["vVoiceName"].InnerText;
                var iconHandle = new IconHandle();
                iconHandle.IDS = name;
                iconHandle.Language = bankSelect;
                iconHandle.Program = programChange;
                iconHandle.IconID =
                    "$0701" + str.format( bankSelect, 2, 16 ) + str.format( programChange, 2, 16 );
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
