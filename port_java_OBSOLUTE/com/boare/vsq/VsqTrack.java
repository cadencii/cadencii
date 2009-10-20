/*
 * VsqTrack.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

import java.util.*;
import java.text.*;
import java.io.*;
import com.boare.corlib.*;

/// <summary>
/// Stores the data of a vsq track.
/// </summary>
public class VsqTrack implements Cloneable {
    public String tag;
    /// <summary>
    /// トラックの名前。
    /// </summary>
    public String name;
    public VsqMetaText metaText;
    private int m_edited_start = Integer.MAX_VALUE;
    private int m_edited_end = Integer.MIN_VALUE;

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "name" ) ){
            return false;
        }else if( name.equals( "metaText" ) ){
            return false;
        }
        return true;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "name" ) ){
            return "Name";
        }else if( name.equals( "metaText" ) ){
            return "MetaText";
        }
        return name;
    }

    public void sortEvent() {
        metaText.events.sort();
    }

    /// <summary>
    /// 歌手変更イベントを，曲の先頭から順に返すIteratorを取得します
    /// </summary>
    /// <returns></returns>
    public Iterator getSingerEventIterator() {
        return new SingerEventIterator( metaText.getEventList() );
    }

    /// <summary>
    /// 音符イベントを，曲の先頭から順に返すIteratorを取得します
    /// </summary>
    /// <returns></returns>
    public Iterator getNoteEventIterator() {
        if ( metaText == null ) {
            return new NoteEventIterator( new VsqEventList() );
        } else {
            return new NoteEventIterator( metaText.getEventList() );
        }
    }

    /// <summary>
    /// メタテキストを，メモリー上のストリームに出力します
    /// </summary>
    /// <param name="sw"></param>
    /// <param name="encode"></param>
    /// <param name="eos"></param>
    /// <param name="start"></param>
    public void printMetaText( TextMemoryStream sw, int eos, int start ) throws IOException{
        metaText.print( sw, false, eos, start );
    }

    /// <summary>
    /// メタテキストを，指定されたファイルに出力します
    /// </summary>
    /// <param name="file"></param>
    public void printMetaText( String file ){
        try{
            TextMemoryStream tms = new TextMemoryStream();
            int count = metaText.getEventList().getCount();
            int clLast = metaText.getEventList().getElement( count - 1 ).clock + 480;
            metaText.print( tms, true, clLast, 0 );
            StreamWriter sw = new StreamWriter( file );
            tms.rewind();
            while ( tms.peek() >= 0 ) {
                String line = tms.readLine();
                sw.writeLine( line );
            }
        }catch( Exception ex ){
            System.out.println( "VsqTrack.printMetaText; ex=" + ex );
        }
    }

    /// <summary>
    /// Masterを取得します
    /// </summary>
    public VsqMaster getMaster() {
        return metaText.master;
    }

    public void setMaster( VsqMaster value ) {
        metaText.master = value;
    }

    /// <summary>
    /// Mixerを取得します
    /// </summary>
    public VsqMixer getMixer() {
        return metaText.mixer;
    }

    public void setMixer( VsqMixer value ) {
        metaText.mixer = value;
    }

    /// <summary>
    /// Commonを取得します
    /// </summary>
    /// <returns></returns>
    public VsqCommon getCommon() {
        return metaText.common;
    }

    /// <summary>
    /// 指定したトラックのレンダラーを変更します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="new_renderer"></param>
    /// <param name="singers"></param>
    public void changeRenderer( String new_renderer, Vector<VsqID> singers ) {
        VsqID default_id = null;
        int c = singers.size();
        if ( c <= 0 ) {
            default_id = new VsqID();
            default_id.type = VsqIDType.Singer;
            default_id.iconHandle = new IconHandle();
            default_id.iconHandle.iconID = "$0701" + (new DecimalFormat( "0000" )).format( 0 );
            default_id.iconHandle.IDS = "Unknown";
            default_id.iconHandle.index = 0;
            default_id.iconHandle.language = 0;
            default_id.iconHandle.length = 1;
            default_id.iconHandle.original = 0;
            default_id.iconHandle.program = 0;
            default_id.iconHandle.caption = "";
        } else {
            default_id = singers.get( 0 );
        }
        for ( Iterator itr = getSingerEventIterator(); itr.hasNext(); ) {
            VsqEvent ve = (VsqEvent)itr.next();
            int program = ve.id.iconHandle.program;
            boolean found = false;
            for ( int i = 0; i < c; i++ ) {
                if ( program == singers.get( i ).iconHandle.program ) {
                    ve.id = (VsqID)singers.get( i ).clone();
                    found = true;
                    break;
                }
            }
            if ( !found ) {
                VsqID add = (VsqID)default_id.clone();
                add.iconHandle.program = program;
                ve.id = add;
            }
        }
        metaText.common.version = new_renderer;
    }

    /// <summary>
    /// このトラックが保持している，指定されたカーブのBPListを取得します
    /// </summary>
    /// <param name="curve"></param>
    /// <returns></returns>
    public VsqBPList getCurve( String curve ) {
        return metaText.getElement( curve );
    }

    public void setCurve( String curve, VsqBPList value ) {
        metaText.setElement( curve, value );
    }

    public int getEventCount() {
        return metaText.getEventList().getCount();
    }

    public VsqEvent getEvent( int index ) {
        return metaText.getEventList().getElement( index );
    }

    public void setEvent( int index, VsqEvent item ) {
        metaText.getEventList().setElement( index, item );
    }

    public void addEvent( VsqEvent item ) {
        metaText.getEventList().add( item );
    }

    public Iterator getEventIterator() {
        return new EventIterator( metaText.getEventList() );
    }

    public void removeEvent( int index ) {
        metaText.getEventList().removeAt( index );
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
        m_edited_start = Integer.MAX_VALUE;
        m_edited_end = Integer.MIN_VALUE;
    }

    /// <summary>
    /// このインスタンスのコピーを作成します
    /// </summary>
    /// <returns></returns>
    public Object clone() {
        VsqTrack res = new VsqTrack();
        res.name = name;
        if ( metaText != null ) {
            res.metaText = (VsqMetaText)metaText.clone();
        }
        res.m_edited_start = m_edited_start;
        res.m_edited_end = m_edited_end;
        res.tag = tag;
        return res;
    }

    /// <summary>
    /// Master Trackを構築
    /// </summary>
    /// <param name="tempo"></param>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    public VsqTrack( int tempo, int numerator, int denominator ) {
        this.name = "Master Track";
        this.metaText = null;
    }

    /// <summary>
    /// Master Trackでないトラックを構築。
    /// </summary>
    /// <param name="name"></param>
    /// <param name="singer"></param>
    public VsqTrack( String name_, String singer ) {
        name = name_;
        metaText = new VsqMetaText( name, singer );
    }

    public VsqTrack(){
        this( "Voice1", "Miku" );
    }

    /// <summary>
    /// 歌詞の文字数を調べます
    /// </summary>
    /// <returns></returns>
    public int getLyricLength() {
        int counter = 0;
        VsqEventList item = metaText.getEventList();
        int c = item.getCount();
        for ( int i = 0; i < c; i++ ) {
            if ( item.getElement( i ).id.type == VsqIDType.Anote ) {
                counter++;
            }
        }
        return counter;
    }

    public VsqTrack( Vector<MidiEventEx> midi_event ) {
        name = "";
        try{
            TextMemoryStream sw = new TextMemoryStream();
            int midi_event_count = midi_event.size();
            for ( int i = 0; i < midi_event_count; i++ ) {
                MidiEventEx me = midi_event.get( i );
                if ( me.firstByte == 0xff && me.data.length > 0 ) {
                    // meta textを抽出
                    byte type = me.data[0];
                    if ( (0xff & type) == 0x01 || (0xff & type) == 0x03 ) {
                        char[] ch = new char[me.data.length - 1];
                        for ( int j = 1; j < me.data.length; j++ ) {
                            ch[j - 1] = (char)me.data[j];
                        }
                        String line = new String( ch );
                        if ( (0xff & type) == 0x01 ) {
                            int second_colon = line.indexOf( ':', 3 );
                            line = line.substring( second_colon + 1 );
                            line = line.replace( "\\n", "\n" );
                            String[] lines = Misc.splitString( line, "\n" );
                            int c = lines.length;
                            for ( int j = 0; j < c; j++ ) {
                                if ( j < c - 1 ) {
                                    sw.writeLine( lines[j] );
                                } else {
                                    sw.write( lines[j] );
                                }
                            }
                            //sw.write( line );
                        } else {
                            name = line;
                        }
                    }
                } else {
                    continue;
                }
            }
            sw.rewind();
            metaText = new VsqMetaText( sw );
        }catch( Exception ex ){
            System.out.println( "VsqTrack(Vector<MidiEventEx>); ex=" + ex );
        }
    }
}
