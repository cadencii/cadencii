/*
 * VsqHandle.java
 * Copyright (c) 2009 kbinani
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

import java.text.*;
import java.io.*;
import com.boare.corlib.*;

/// <summary>
/// ハンドルを取り扱います。ハンドルにはLyricHandle、VibratoHandle、IconHandleおよびNoteHeadHandleがある
/// </summary>
public class VsqHandle {
    public VsqHandleType m_type;
    public int index;
    public String iconID;
    public String IDS;
    public Lyric L0;
    public int original;
    public String caption;
    public int length;
    public int startDepth;
    public VibratoBPList depthBP;
    public int startRate;
    public VibratoBPList rateBP;
    public int language;
    public int program;
    public int duration;
    public int depth;

    public VsqHandle(){
    }

    public LyricHandle castToLyricHandle() {
        LyricHandle ret = new LyricHandle();
        ret.L0 = (Lyric)L0;
        ret.index = index;
        return ret;
    }

    public VibratoHandle castToVibratoHandle() {
        VibratoHandle ret = new VibratoHandle();
        ret.index = index;
        ret.caption = caption;
        ret.depthBP = (VibratoBPList)depthBP.clone();
        ret.iconID = iconID;
        ret.IDS = IDS;
        ret.index = index;
        ret.length = length;
        ret.original = original;
        ret.rateBP = (VibratoBPList)rateBP.clone();
        ret.startDepth = startDepth;
        ret.startRate = startRate;
        return ret;
    }

    public IconHandle castToIconHandle() {
        IconHandle ret = new IconHandle();
        ret.index = index;
        ret.caption = caption;
        ret.iconID = iconID;
        ret.IDS = IDS;
        ret.index = index;
        ret.language = language;
        ret.length = length;
        ret.original = original;
        ret.program = program;
        return ret;
    }

    public NoteHeadHandle castToNoteHeadHandle() {
        NoteHeadHandle ret = new NoteHeadHandle();
        ret.caption = caption;
        ret.depth = depth;
        ret.duration = duration;
        ret.iconID = iconID;
        ret.IDS = IDS;
        ret.length = length;
        ret.original = original;
        return ret;
    }

    /// <summary>
    /// インスタンスをストリームに書き込みます。
    /// encode=trueの場合、2バイト文字をエンコードして出力します。
    /// </summary>
    /// <param name="sw">書き込み対象</param>
    /// <param name="encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
    public void write( TextMemoryStream sw, boolean encode ) {
        sw.writeLine( this.toString( encode ) );
    }

    /// <summary>
    /// FileStreamから読み込みながらコンストラクト
    /// </summary>
    /// <param name="sr">読み込み対象</param>
    public VsqHandle( TextMemoryStream sr, int value, StringBuilder last_line ) {
try{
        this.index = value;
        String[] spl;
        String[] spl2;

        // default値で梅
        m_type = VsqHandleType.Vibrato;
        iconID = "";
        IDS = "normal";
        L0 = new Lyric( "" );
        original = 0;
        caption = "";
        length = 0;
        startDepth = 0;
        depthBP = null;
        int depth_bp_num = 0;
        startRate = 0;
        rateBP = null;
        int rate_bp_num = 0;
        language = 0;
        program = 0;
        duration = 0;
        depth = 64;

        String tmpDepthBPX = "";
        String tmpDepthBPY = "";
        String tmpRateBPX = "";
        String tmpRateBPY = "";

        // "["にぶち当たるまで読込む
        last_line.setLength( 0 );
        last_line.append( sr.readLine() );
        while ( !last_line.toString().startsWith( "[" ) ) {
            spl = Misc.splitString( last_line.toString(), "=" );
            if( spl[0].equals( "Language" ) ){
                m_type = VsqHandleType.Singer;
                language = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "Program" ) ){
                program = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "IconID" ) ){
                iconID = spl[1];
            }else if( spl[0].equals( "IDS" ) ){
                IDS = spl[1];
            }else if( spl[0].equals( "Original" ) ){
                original = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "Caption" ) ){
                caption = spl[1];
                for ( int i = 2; i < spl.length; i++ ) {
                    caption += "=" + spl[i];
                }
            }else if( spl[0].equals( "Length" ) ){
                length = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "StartDepth" ) ){
                startDepth = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "DepthBPNum" ) ){
                depth_bp_num = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "DepthBPX" ) ){
                tmpDepthBPX = spl[1];
            }else if( spl[0].equals( "DepthBPY" ) ){
                tmpDepthBPY = spl[1];
            }else if( spl[0].equals( "StartRate" ) ){
                m_type = VsqHandleType.Vibrato;
                startRate = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "RateBPNum" ) ){
                rate_bp_num = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "RateBPX" ) ){
                tmpRateBPX = spl[1];
            }else if( spl[0].equals( "RateBPY" ) ){
                tmpRateBPY = spl[1];
            }else if( spl[0].equals( "L0" ) ){
                m_type = VsqHandleType.Lyric;
                L0 = new Lyric( spl[1] );
            }else if( spl[0].equals( "Duration" ) ){
                m_type = VsqHandleType.NoteHeadHandle;
                duration = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "Depth" ) ){
                duration = Integer.parseInt( spl[1] );
            }
            if ( sr.peek() < 0 ) {
                break;
            }
            last_line.setLength( 0 );
            last_line.append( sr.readLine() );
        }
        /*if ( IDS != "normal" ) {
            m_type = VsqHandleType.Singer;
        } else if ( IconID != "" ) {
            m_type = VsqHandleType.Vibrato;
        } else {
            m_type = VsqHandleType.Lyric;
        }*/

        // RateBPX, RateBPYの設定
        if ( m_type == VsqHandleType.Vibrato ) {
            if ( rate_bp_num > 0 ) {
                float[] rate_bp_x = new float[rate_bp_num];
                spl2 = tmpRateBPX.split( "," );
                for ( int i = 0; i < rate_bp_num; i++ ) {
                    rate_bp_x[i] = Float.parseFloat( spl2[i] );
                }

                int[] rate_bp_y = new int[rate_bp_num];
                spl2 = tmpRateBPY.split( "," );
                for ( int i = 0; i < rate_bp_num; i++ ) {
                    rate_bp_y[i] = Integer.parseInt( spl2[i] );
                }
                rateBP = new VibratoBPList( rate_bp_x, rate_bp_y );
            } else {
                //m_rate_bp_x = null;
                //m_rate_bp_y = null;
                rateBP = new VibratoBPList();
            }

            // DepthBPX, DepthBPYの設定
            if ( depth_bp_num > 0 ) {
                float[] depth_bp_x = new float[depth_bp_num];
                spl2 = tmpDepthBPX.split( "," );
                for ( int i = 0; i < depth_bp_num; i++ ) {
                    depth_bp_x[i] = Float.parseFloat( spl2[i] );
                }

                int[] depth_bp_y = new int[depth_bp_num];
                spl2 = tmpDepthBPY.split( "," );
                for ( int i = 0; i < depth_bp_num; i++ ) {
                    depth_bp_y[i] = Integer.parseInt( spl2[i] );
                }
                depthBP = new VibratoBPList( depth_bp_x, depth_bp_y );
            } else {
                depthBP = new VibratoBPList();
                //m_depth_bp_x = null;
                //m_depth_bp_y = null;
            }
        } else {
            depthBP = new VibratoBPList();
            rateBP = new VibratoBPList();
        }
}catch( Exception ex ){
    System.out.println( "VsqHandle(TextMemoryStream,int,StringBuilder); ex=" + ex );
}
    }

    /// <summary>
    /// ハンドル指定子（例えば"h#0123"という文字列）からハンドル番号を取得します
    /// </summary>
    /// <param name="_string">ハンドル指定子</param>
    /// <returns>ハンドル番号</returns>
    public static int handleIndexFromString( String _string ) {
        String[] spl = _string.split( "#" );
        return Integer.parseInt( spl[1] );
    }

    /// <summary>
    /// インスタンスをテキストファイルに出力します
    /// </summary>
    /// <param name="sw">出力先</param>
    public void print( StreamWriter sw ) throws IOException{
        String result = this.toString();
        sw.writeLine( result );
    }

    /// <summary>
    /// インスタンスをコンソール画面に出力します
    /// </summary>
    private void print() {
        String result = this.toString();
        System.out.println( result );
    }

    /// <summary>
    /// インスタンスを文字列に変換します
    /// </summary>
    /// <param name="encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
    /// <returns>インスタンスを変換した文字列</returns>
    public String toString( boolean encode ) {
        String result = "";
        result += "[h#" + (new DecimalFormat( "0000" )).format( index ) + "]";
        switch ( m_type ) {
            case Lyric:
                result += "\nL0=" + L0.toString( encode );
                break;
            case Vibrato:
                int c_depth = depthBP.getCount();
                result += "\nIconID=" + iconID + "\n";
                result += "IDS=" + IDS + "\n";
                result += "Original=" + original + "\n";
                result += "Caption=" + caption + "\n";
                result += "Length=" + length + "\n";
                result += "StartDepth=" + startDepth + "\n";
                result += "DepthBPNum=" + c_depth + "\n";
                if ( c_depth > 0 ) {
                    result += "DepthBPX=" + (new DecimalFormat( "0.000000" )).format( depthBP.getElement( 0 ).x );
                    for ( int i = 1; i < c_depth; i++ ) {
                        result += "," + (new DecimalFormat( "0.000000" )).format( depthBP.getElement( i ).x );
                    }
                    result += "\nDepthBPY=" + depthBP.getElement( 0 ).y;
                    for ( int i = 1; i < c_depth; i++ ) {
                        result += "," + depthBP.getElement( i ).y;
                    }
                    result += "\n";
                }
                int c_rate = rateBP.getCount();
                result += "StartRate=" + startRate + "\n";
                result += "RateBPNum=" + c_rate;
                if ( c_rate > 0 ) {
                    result += "\nRateBPX=" + (new DecimalFormat( "0.000000" )).format( rateBP.getElement( 0 ).x );
                    for ( int i = 1; i < c_rate; i++ ) {
                        result += "," + (new DecimalFormat( "0.000000" )).format( rateBP.getElement( i ).x );
                    }
                    result += "\nRateBPY=" + rateBP.getElement( 0 ).y;
                    for ( int i = 1; i < c_rate; i++ ) {
                        result += "," + rateBP.getElement( i ).y;
                    }
                }
                break;
            case Singer:
                result += "\nIconID=" + iconID + "\n";
                result += "IDS=" + IDS + "\n";
                result += "Original=" + original + "\n";
                result += "Caption=" + caption + "\n";
                result += "Length=" + length + "\n";
                result += "Language=" + language + "\n";
                result += "Program=" + program;
                break;
            case NoteHeadHandle:
                result += "\nIconID=" + iconID + "\n";
                result += "IDS=" + IDS + "\n";
                result += "Original=" + original + "\n";
                result += "Caption=" + caption + "\n";
                result += "Length=" + length + "\n";
                result += "Duration=" + duration + "\n";
                result += "Depth=" + depth;
                break;
            default:
                break;
        }
        return result;
    }
}
