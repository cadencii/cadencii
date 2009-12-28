/*
 * VsqTrack.cs
 * Copyright (C) 2008-2009 kbinani
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
package org.kbinani.vsq;

import java.io.*;
import java.util.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// Stores the data of a vsq track.
    /// </summary>
#if JAVA
    public class VsqTrack implements Cloneable, Serializable {
#else
    [Serializable]
    public class VsqTrack : ICloneable {
#endif
        public String Tag;
        /// <summary>
        /// トラックの名前。
        /// </summary>
        //public String Name;
        public VsqMetaText MetaText;
        private int m_edited_start = int.MaxValue;
        private int m_edited_end = int.MinValue;

#if JAVA
        private class SingerEventIterator implements Iterator{
#else
        private class SingerEventIterator : Iterator {
#endif
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

            public Object next() {
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

#if JAVA
        private class NoteEventIterator implements Iterator{
#else
        private class NoteEventIterator : Iterator {
#endif
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

            public Object next() {
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

#if JAVA
        private class EventIterator implements Iterator{
#else
        private class EventIterator : Iterator {
#endif
            private VsqEventList m_list;
            private int m_pos;

            public EventIterator( VsqEventList list ) {
                m_list = list;
                m_pos = -1;
            }

            public boolean hasNext() {
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

#if !JAVA
        [Obsolete]
        public String Name {
            get {
                return getName();
            }
            set {
                setName( value );
            }
        }
#endif

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
        public void printMetaText( TextMemoryStream sw, int eos, int start )
#if JAVA
            throws IOException
#endif
 {
            MetaText.print( sw, eos, start );
        }

        /// <summary>
        /// メタテキストを，指定されたファイルに出力します
        /// </summary>
        /// <param name="file"></param>
        public void printMetaText( String file )
#if JAVA
            throws IOException
#endif
 {
            TextMemoryStream tms = new TextMemoryStream();
            int count = MetaText.getEventList().getCount();
            int clLast = MetaText.getEventList().getElement( count - 1 ).Clock + 480;
            MetaText.print( tms, clLast, 0 );
            BufferedWriter sw = null;
            try {
                sw = new BufferedWriter( new FileWriter( file ) );
                tms.rewind();
                while ( tms.peek() >= 0 ) {
                    String line = tms.readLine();
                    sw.write( line );
                    sw.newLine();
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "VsqTrack#printMetaText; ex=" + ex );
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "VsqTrack#printMetaText; ex2=" + ex2 );
                    }
                }
            }
        }

        /// <summary>
        /// Masterを取得します
        /// </summary>
        public VsqMaster getMaster() {
            return MetaText.master;
        }

        public void setMaster( VsqMaster value ) {
            MetaText.master = value;
        }

        /// <summary>
        /// Mixerを取得します
        /// </summary>
        public VsqMixer getMixer() {
            return MetaText.mixer;
        }

        public void setMixer( VsqMixer value ) {
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
            int singers_size = singers.size();
            if ( singers_size <= 0 ) {
                default_id = new VsqID();
                default_id.type = VsqIDType.Singer;
                default_id.IconHandle = new IconHandle();
                default_id.IconHandle.IconID = "$0701" + PortUtil.toHexString( 0, 4 );
                default_id.IconHandle.IDS = "Unknown";
                default_id.IconHandle.Index = 0;
                default_id.IconHandle.Language = 0;
                default_id.IconHandle.setLength( 1 );
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
                for ( int i = 0; i < singers_size; i++ ) {
                    VsqID id = singers.get( i );
                    if ( program == id.IconHandle.Program ) {
                        ve.ID = (VsqID)id.clone();
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
                res.MetaText = (VsqMetaText)MetaText.clone();
            }
            res.m_edited_start = m_edited_start;
            res.m_edited_end = m_edited_end;
            res.Tag = Tag;
            return res;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

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

#if JAVA
        public VsqTrack(){
            this( "Voice1", "Miku" );
#else
        public VsqTrack()
            : this( "Voice1", "Miku" ) {
#endif
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

        public VsqTrack( Vector<MidiEvent> midi_event, String encoding ) {
#if DEBUG
            org.kbinani.debug.push_log( "VsqTrack..ctor" );
#endif
            String track_name = "";

            TextMemoryStream sw = null;
            try {
                sw = new TextMemoryStream();
                int count = midi_event.size();
                Vector<Byte> buffer = new Vector<Byte>();
                for ( int i = 0; i < count; i++ ) {
                    MidiEvent item = midi_event.get( i );
                    if ( item.firstByte == 0xff && item.data.Length > 0 ) {
                        // meta textを抽出
                        byte type = item.data[0];
                        if ( type == 0x01 || type == 0x03 ) {
                            if ( type == 0x01 ) {
                                int colon_count = 0;
                                for ( int j = 0; j < item.data.Length - 1; j++ ) {
                                    byte d = item.data[j + 1];
                                    if ( d == 0x3a ) {
                                        colon_count++;
                                        if ( colon_count <= 2 ) {
                                            continue;
                                        }
                                    }
                                    if ( colon_count < 2 ) {
                                        continue;
                                    }
                                    buffer.add( d );
                                }

                                int index_0x0a = buffer.indexOf( 0x0a );
                                while ( index_0x0a >= 0 ) {
                                    byte[] cpy = new byte[index_0x0a];
                                    for ( int j = 0; j < index_0x0a; j++ ) {
                                        cpy[j] = buffer.get( 0 );
                                        buffer.removeElementAt( 0 );
                                    }

                                    String line = PortUtil.getDecodedString( encoding, cpy );
                                    sw.writeLine( line );
                                    buffer.removeElementAt( 0 );
                                    index_0x0a = buffer.indexOf( 0x0a );
                                }
                            } else {
                                for ( int j = 0; j < item.data.Length - 1; j++ ) {
                                    buffer.add( item.data[j + 1] );
                                }
                                track_name = PortUtil.getDecodedString( encoding, 
                                                                        PortUtil.convertByteArray( buffer.toArray( new Byte[] { } ) ) );
                                buffer.clear();
                            }
                        }
                    } else {
                        continue;
                    }
                }
                // oketa ketaoさんありがとう =>
                int remain = buffer.size();
                if ( remain > 0 ) {
                    byte[] cpy = new byte[remain];
                    for ( int j = 0; j < remain; j++ ) {
                        cpy[j] = buffer.get( j );
                    }
                    String line = PortUtil.getDecodedString( encoding, cpy );
                    sw.writeLine( line );
                }
                // <=
                sw.rewind();
                MetaText = new VsqMetaText( sw );
                setName( track_name );
            } catch ( Exception ex ) {
                PortUtil.println( "com.boare.vsq.VsqTrack#.ctor; ex=" + ex );
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }
    }

#if !JAVA
}
#endif
