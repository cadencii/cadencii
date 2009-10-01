/*
 * VsqTrack.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    /// <summary>
    /// Stores the data of a vsq track.
    /// </summary>
    [Serializable]
    public partial class VsqTrack : ICloneable {
        public String Tag;
        /// <summary>
        /// トラックの名前。
        /// </summary>
        //public String Name;
        public VsqMetaText MetaText;
        private int m_edited_start = int.MaxValue;
        private int m_edited_end = int.MinValue;

        private class SingerEventIterator : Iterator {
            VsqEventList m_list;
            int m_pos;

            public SingerEventIterator( VsqEventList list ) {
                m_list = list;
                m_pos = -1;
            }

            public boolean hasNext() {
                for ( int i = m_pos + 1; i < m_list.getCount(); i++ ) {
                    if ( m_list.getElement( i ).ID.type == VsqIDType.Singer ) {
                        return true;
                    }
                }
                return false;
            }

            public object next() {
                for ( int i = m_pos + 1; i < m_list.getCount(); i++ ) {
                    VsqEvent item = m_list.getElement( i );
                    if ( item.ID.type == VsqIDType.Singer ) {
                        m_pos = i;
                        return item;
                    }
                }
                return null;
            }

            public void remove() {
                if ( 0 <= m_pos && m_pos < m_list.getCount() ) {
                    m_list.removeAt( m_pos );
                }
            }
        }

        private class NoteEventIterator : Iterator {
            VsqEventList m_list;
            int m_pos;

            public NoteEventIterator( VsqEventList list ) {
                m_list = list;
                m_pos = -1;
            }

            public boolean hasNext() {
                int count = m_list.getCount();
                for ( int i = m_pos + 1; i < count; i++ ) {
                    if ( m_list.getElement( i ).ID.type == VsqIDType.Anote ) {
                        return true;
                    }
                }
                return false;
            }

            public object next() {
                int count = m_list.getCount();
                for ( int i = m_pos + 1; i < count; i++ ) {
                    VsqEvent item = m_list.getElement( i );
                    if ( item.ID.type == VsqIDType.Anote ) {
                        m_pos = i;
                        return item;
                    }
                }
                return null;
            }

            public void remove() {
                if ( 0 <= m_pos && m_pos < m_list.getCount() ) {
                    m_list.removeAt( m_pos );
                }
            }
        }

        private class EventIterator : Iterator{
            private VsqEventList m_list;
            private int m_pos;

            public EventIterator( VsqEventList list ) {
                m_list = list;
                m_pos = -1;
            }

            public Boolean hasNext() {
                if ( 0 <= m_pos + 1 && m_pos + 1 < m_list.getCount() ) {
                    return true;
                }
                return false;
            }

            public Object next() {
                m_pos++;
                return m_list.getElement( m_pos );
            }

            public void remove() {
                if ( 0 <= m_pos && m_pos < m_list.getCount() ) {
                    m_list.removeAt( m_pos );
                }
            }
        }

        public String getName() {
            if ( MetaText == null || (MetaText != null && MetaText.Common == null) ) {
                return "Master Track";
            } else {
                return MetaText.Common.Name;
            }
        }

        public void setName( String value ) {
            if ( MetaText != null ) {
                if ( MetaText.Common == null ) {
                    MetaText.Common = new VsqCommon();
                }
                MetaText.Common.Name = value;
            }
        }

        [Obsolete]
        public String Name {
            get {
                return getName();
            }
            set {
                setName( value );
            }
        }

        /// <summary>
        /// ピッチベンド。Cent単位
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public double getPitchAt( int clock ) {
            double inv2_13 = 1.0 / 8192.0;
            int pit = MetaText.PIT.getValue( clock );
            int pbs = MetaText.PBS.getValue( clock );
            return (double)pit * (double)pbs * inv2_13 * 100.0;
        }

        /// <summary>
        /// 指定したクロック位置において、歌唱を担当している歌手のVsqEventを返します。
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public VsqEvent getSingerEventAt( int clock ) {
            VsqEvent last = null;
            for ( Iterator itr = getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( clock < item.Clock ) {
                    return last;
                }
                last = item;
            }
            return last;
        }

        public void sortEvent() {
            MetaText.Events.sort();
        }

        /// <summary>
        /// 歌手変更イベントを，曲の先頭から順に返すIteratorを取得します
        /// </summary>
        /// <returns></returns>
        public Iterator getSingerEventIterator() {
            return new SingerEventIterator( MetaText.getEventList() );
        }

        /// <summary>
        /// 音符イベントを，曲の先頭から順に返すIteratorを取得します
        /// </summary>
        /// <returns></returns>
        public Iterator getNoteEventIterator() {
            if ( MetaText == null ) {
                return new NoteEventIterator( new VsqEventList() );
            } else {
                return new NoteEventIterator( MetaText.getEventList() );
            }
        }

        /// <summary>
        /// メタテキストを，メモリー上のストリームに出力します
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="encode"></param>
        /// <param name="eos"></param>
        /// <param name="start"></param>
        public void printMetaText( TextMemoryStream sw, int eos, int start ) {
            MetaText.print( sw, false, eos, start );
        }

        /// <summary>
        /// メタテキストを，指定されたファイルに出力します
        /// </summary>
        /// <param name="file"></param>
        public void printMetaText( String file ) {
            TextMemoryStream tms = new TextMemoryStream();
            int count = MetaText.getEventList().getCount();
            int clLast = MetaText.getEventList().getElement( count - 1 ).Clock + 480;
            MetaText.print( tms, true, clLast, 0 );
            using ( StreamWriter sw = new StreamWriter( file ) ) {
                tms.rewind();
                while ( tms.peek() >= 0 ) {
                    String line = tms.readLine();
                    sw.WriteLine( line );
                }
            }
        }

        /// <summary>
        /// Masterを取得します
        /// </summary>
        public VsqMaster getMaster() {
            return MetaText.master;
        }

        internal void setMaster( VsqMaster value ) {
            MetaText.master = value;
        }

        /// <summary>
        /// Mixerを取得します
        /// </summary>
        public VsqMixer getMixer() {
            return MetaText.mixer;
        }

        internal void setMixer( VsqMixer value ) {
            MetaText.mixer = value;
        }

        /// <summary>
        /// Commonを取得します
        /// </summary>
        /// <returns></returns>
        public VsqCommon getCommon() {
            return MetaText.Common;
        }

        /// <summary>
        /// 指定したトラックのレンダラーを変更します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="new_renderer"></param>
        /// <param name="singers"></param>
        public void changeRenderer( String new_renderer, Vector<VsqID> singers ) {
            VsqID default_id = null;
            if ( singers.size() <= 0 ) {
                default_id = new VsqID();
                default_id.type = VsqIDType.Singer;
                default_id.IconHandle = new IconHandle();
                default_id.IconHandle.IconID = "$0701" + 0.ToString( "0000" );
                default_id.IconHandle.IDS = "Unknown";
                default_id.IconHandle.Index = 0;
                default_id.IconHandle.Language = 0;
                default_id.IconHandle.Length = 1;
                default_id.IconHandle.Original = 0;
                default_id.IconHandle.Program = 0;
                default_id.IconHandle.Caption = "";
            } else {
                default_id = singers.get( 0 );
            }
            for ( Iterator itr = getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                int program = ve.ID.IconHandle.Program;
                boolean found = false;
                for ( int i = 0; i < singers.size(); i++ ) {
                    if ( program == singers.get( i ).IconHandle.Program ) {
                        ve.ID = (VsqID)singers.get( i ).clone();
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    VsqID add = (VsqID)default_id.clone();
                    add.IconHandle.Program = program;
                    ve.ID = add;
                }
            }
            MetaText.Common.Version = new_renderer;
        }

        /// <summary>
        /// このトラックが保持している，指定されたカーブのBPListを取得します
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public VsqBPList getCurve( String curve ) {
            return MetaText.getElement( curve );
        }

        public void setCurve( String curve, VsqBPList value ) {
            MetaText.setElement( curve, value );
        }

        public int getEventCount() {
            return MetaText.getEventList().getCount();
        }

        public VsqEvent getEvent( int index ) {
            return MetaText.getEventList().getElement( index );
        }

        public VsqEvent findEventFromID( int internal_id ) {
            return MetaText.getEventList().findFromID( internal_id );
        }

        public void setEvent( int index, VsqEvent item ) {
            MetaText.getEventList().setElement( index, item );
        }

        public void addEvent( VsqEvent item ) {
            MetaText.getEventList().add( item );
        }

        public Iterator getEventIterator() {
            return new EventIterator( MetaText.getEventList() );
        }

        public void removeEvent( int index ) {
            MetaText.getEventList().removeAt( index );
        }

        /// <summary>
        /// このトラックの，最後に編集が加えられた範囲の，開始位置（クロック）を取得します．
        /// </summary>
        public int getEditedStart() {
            return m_edited_start;
        }

        public void setEditedStart( int value ) {
            if ( value < m_edited_start ) {
                m_edited_start = value;
            }
        }

        /// <summary>
        /// このトラックの，最後に編集が加えられた範囲の，終了位置（クロック）を取得します．
        /// </summary>
        public int getEditedEnd() {
            return m_edited_end;
        }

        public void setEditedEnd( int value ) {
            if ( m_edited_end < value ) {
                m_edited_end = value;
            }
        }

        /// <summary>
        /// このトラックの，編集範囲（EditedStart, EditedEnd）をリセットします．
        /// </summary>
        public void resetEditedArea() {
            m_edited_start = int.MaxValue;
            m_edited_end = int.MinValue;
        }

        /// <summary>
        /// このインスタンスのコピーを作成します
        /// </summary>
        /// <returns></returns>
        public Object clone() {
            VsqTrack res = new VsqTrack();
            res.setName( getName() );
            if ( MetaText != null ) {
                res.MetaText = (VsqMetaText)MetaText.Clone();
            }
            res.m_edited_start = m_edited_start;
            res.m_edited_end = m_edited_end;
            res.Tag = Tag;
            return res;
        }

        public object Clone() {
            return clone();
        }

        /// <summary>
        /// Master Trackを構築
        /// </summary>
        /// <param name="tempo"></param>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public VsqTrack( int tempo, int numerator, int denominator ) {
            //this.Name = "Master Track";
            // metatextがnullのとき，トラック名はMaster Track
            this.MetaText = null;
        }

        /// <summary>
        /// Master Trackでないトラックを構築。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="singer"></param>
        public VsqTrack( String name, String singer ) {
            MetaText = new VsqMetaText( name, singer );
        }

        public VsqTrack()
            : this( "Voice1", "Miku" ) {
        }

        /// <summary>
        /// 歌詞の文字数を調べます
        /// </summary>
        /// <returns></returns>
        public int getLyricLength() {
            int counter = 0;
            for ( int i = 0; i < MetaText.getEventList().getCount(); i++ ) {
                if ( MetaText.getEventList().getElement( i ).ID.type == VsqIDType.Anote ) {
                    counter++;
                }
            }
            return counter;
        }

        public VsqTrack( Vector<Boare.Lib.Vsq.MidiEvent> midi_event, Encoding encoding ) {
#if DEBUG
            bocoree.debug.push_log( "VsqTrack..ctor" );
#endif
            String track_name = "";
            using ( TextMemoryStream sw = new TextMemoryStream() ) {
                for ( int i = 0; i < midi_event.size(); i++ ) {
                    if ( midi_event.get( i ).firstByte == 0xff && midi_event.get( i ).data.Length > 0 ) {
                        // meta textを抽出
                        byte type = midi_event.get( i ).data[0];
                        if ( type == 0x01 || type == 0x03 ) {
                            /*char[] ch = new char[midi_event.get( i ).data.Length - 1];
                            for ( int j = 1; j < midi_event.get( i ).data.Length; j++ ) {
                                ch[j - 1] = (char)midi_event.get( i ).data[j];
                            }
                            
                            String line = new String( ch );*/
                            byte[] dat = midi_event.get( i ).data;
                            String line = encoding.GetString( dat, 1, dat.Length - 1 );
                            if ( type == 0x01 ) {
                                int second_colon = line.IndexOf( ':', 3 );
                                line = line.Substring( second_colon + 1 );
                                line = line.Replace( "\\n", "\n" );
                                //line = line.Replace( "\n", Environment.NewLine );
                                String[] lines = PortUtil.splitString( line, '\n' );
                                int c = lines.Length;
                                for ( int j = 0; j < c; j++ ) {
                                    if ( j < c - 1 ) {
                                        sw.writeLine( lines[j] );
                                    } else {
                                        sw.write( lines[j] );
                                    }
                                }
                                //sw.write( line );
                            } else {
                                track_name = line;
                            }
                        }
                    } else {
                        continue;
                    }
                }
                sw.rewind();
                MetaText = new VsqMetaText( sw );
                setName( track_name );
            }
        }
    }

}
