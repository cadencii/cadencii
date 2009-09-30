/*
 * VsqID.cs
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

import java.text.*;

/// <summary>
/// メタテキストに埋め込まれるIDを表すクラス。
/// </summary>
public class VsqID implements Cloneable {
    public int value;
    public VsqIDType type;
    public int iconHandleIndex;
    public IconHandle iconHandle;
    public int length;
    public int note;
    public int dynamics;
    public int pmBendDepth;
    public int pmBendLength;
    public int pmbPortamentoUse;
    public int demDecGainRate;
    public int demAccent;
    public int lyricHandleIndex;
    public LyricHandle lyricHandle;
    public int vibratoHandleIndex;
    public VibratoHandle vibratoHandle;
    public int vibratoDelay;
    public int noteHeadHandleIndex;
    public NoteHeadHandle noteHeadHandle;
    public int pMeanOnsetFirstNote = 0x0a;
    public int vMeanNoteTransition = 0x0c;
    public int d4mean = 0x18;
    public int pMeanEndingNote = 0x0c;

    public static final VsqID EOS = new VsqID( -1 );

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "lyricHandleIndex" ) ){
            return true;
        }else if( name.equals( "vibratoHandleIndex" ) ){
            return true;
        }else if( name.equals( "noteHeadHandleIndex" ) ){
            return true;
        }else if( name.equals( "iconHandleIndex" ) ){
            return true;
        }else if( name.equals( "value" ) ){
            return true;
        }
        return false;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "lyricHandle" ) ){
            return "LyricHandle";
        }else if( name.equals( "type" ) ){
            return "Type";
        }else if( name.equals( "iconHandle" ) ){
            return "IconHandle";
        }else if( name.equals( "length" ) ){
            return "Length";
        }else if( name.equals( "note" ) ){
            return "Note";
        }else if( name.equals( "dynamics" ) ){
            return "Dynamics";
        }else if( name.equals( "pmBendDepth" ) ){
            return "PMBendDepth";
        }else if( name.equals( "pmBendLength" ) ){
            return "PMBendLength";
        }else if( name.equals( "pmbPortamentoUse" ) ){
            return "PMbPortamentoUse";
        }else if( name.equals( "demDecGainRate" ) ){
            return "DEMdecGainRate";
        }else if( name.equals( "demAccent" ) ){
            return "DEMaccent";
        }else if( name.equals( "vibratoHandle" ) ){
            return "VibratoHandle";
        }else if( name.equals( "noteHeadHandle" ) ){
            return "NoteHeadHandle";
        }else if( name.equals( "vibratoDelay" ) ){
            return "VibratoDelay";
        }
        return name;
    }

    /// <summary>
    /// このインスタンスの簡易コピーを取得します。
    /// </summary>
    /// <returns>このインスタンスの簡易コピー</returns>
    public Object clone() {
        VsqID result = new VsqID( this.value );
        result.type = this.type;
        if ( this.iconHandle != null ) {
            result.iconHandle = (IconHandle)this.iconHandle.clone();
        }
        result.length           = this.length;
        result.note             = this.note;
        result.dynamics         = this.dynamics;
        result.pmBendDepth      = this.pmBendDepth;
        result.pmBendLength     = this.pmBendLength;
        result.pmbPortamentoUse = this.pmbPortamentoUse;
        result.demDecGainRate   = this.demDecGainRate;
        result.demAccent        = this.demAccent;
        if ( this.lyricHandle != null ) {
            result.lyricHandle = (LyricHandle)this.lyricHandle.clone();
        }
        if ( this.vibratoHandle != null ) {
            result.vibratoHandle = (VibratoHandle)this.vibratoHandle.clone();
        }
        result.vibratoDelay = this.vibratoDelay;
        if ( noteHeadHandle != null ) {
            result.noteHeadHandle = (NoteHeadHandle)noteHeadHandle.clone();
        }
        return result;
    }

    /// <summary>
    /// IDの番号（ID#****の****）を指定したコンストラクタ。
    /// </summary>
    /// <param name="a_value">IDの番号</param>
    public VsqID( int a_value ) {
        value = a_value;
    }

    public VsqID(){
        this( 0 );
    }

    /// <summary>
    /// テキストファイルからのコンストラクタ
    /// </summary>
    /// <param name="sr">読み込み対象</param>
    /// <param name="value"></param>
    /// <param name="last_line">読み込んだ最後の行が返されます</param>
    public VsqID( TextMemoryStream sr, int value, StringBuilder last_line ) {
        String[] spl;
        this.value = value;
        this.type = VsqIDType.Unknown;
        this.iconHandleIndex = -2;
        this.lyricHandleIndex = -1;
        this.vibratoHandleIndex = -1;
        this.noteHeadHandleIndex = -1;
        this.length = 0;
        this.note = 0;
        this.dynamics = 0;
        this.pmBendDepth = 0;
        this.pmBendLength = 0;
        this.pmbPortamentoUse = 0;
        this.demDecGainRate = 0;
        this.demAccent = 0;
        //this.LyricHandle_index = -2;
        //this.VibratoHandle_index = -2;
        this.vibratoDelay = 0;
        last_line.setLength( 0 );
        last_line.append( sr.readLine() );
        while ( !last_line.toString().startsWith( "[" ) ) {
            spl = last_line.toString().split( "=" );
            String s = spl[0];
            if( s.equals( "Type" ) ){
                if ( spl[1].equals( "Anote" ) ) {
                    type = VsqIDType.Anote;
                } else if ( spl[1].equals( "Singer" ) ) {
                    type = VsqIDType.Singer;
                } else {
                    type = VsqIDType.Unknown;
                }
            }else if( s.equals( "Length" ) ){
                this.length = Integer.parseInt( spl[1] );
            }else if( s.equals( "Note#" ) ){
                this.note = Integer.parseInt( spl[1] );
            }else if( s.equals( "Dynamics" ) ){
                this.dynamics = Integer.parseInt( spl[1] );
            }else if( s.equals( "PMBendDepth" ) ){
                this.pmBendDepth = Integer.parseInt( spl[1] );
            }else if( s.equals( "PMBendLength" ) ){
                this.pmBendLength = Integer.parseInt( spl[1] );
            }else if( s.equals( "DEMdecGainRate" ) ){
                this.demDecGainRate = Integer.parseInt( spl[1] );
            }else if( s.equals( "DEMaccent" ) ){
                this.demAccent = Integer.parseInt( spl[1] );
            }else if( s.equals( "LyricHandle" ) ){
                this.lyricHandleIndex = VsqHandle.handleIndexFromString( spl[1] );
            }else if( s.equals( "IconHandle" ) ){
                this.iconHandleIndex = VsqHandle.handleIndexFromString( spl[1] );
            }else if( s.equals( "VibratoHandle" ) ){
                this.vibratoHandleIndex = VsqHandle.handleIndexFromString( spl[1] );
            }else if( s.equals( "VibratoDelay" ) ){
                this.vibratoDelay = Integer.parseInt( spl[1] );
            }else if( s.equals( "PMbPortamentoUse" ) ){
                pmbPortamentoUse = Integer.parseInt( spl[1] );
            }else if( s.equals( "NoteHeadHandle" ) ){
                noteHeadHandleIndex = VsqHandle.handleIndexFromString( spl[1] );
            }
            if ( sr.peek() < 0 ) {
                break;
            }
            last_line.setLength( 0 );
            last_line.append( sr.readLine() );
        }
    }

    public String toString() {
        String ret = "{Type=" + type;
        switch ( type ) {
            case Anote:
                ret += ", Length=" + length;
                ret += ", Note#=" + note;
                ret += ", Dynamics=" + dynamics;
                ret += ", PMBendDepth=" + pmBendDepth;
                ret += ", PMBendLength=" + pmBendLength;
                ret += ", PMbPortamentoUse=" + pmbPortamentoUse;
                ret += ", DEMdecGainRate=" + demDecGainRate;
                ret += ", DEMaccent=" + demAccent;
                if ( lyricHandle != null ) {
                    ret += ", LyricHandle=h#" + (new DecimalFormat( "0000" )).format( lyricHandleIndex );
                }
                if ( vibratoHandle != null ) {
                    ret += ", VibratoHandle=h#" + (new DecimalFormat( "0000" )).format( vibratoHandleIndex );
                    ret += ", VibratoDelay=" + vibratoDelay;
                }
                break;
            case Singer:
                ret += ", IconHandle=h#" + (new DecimalFormat( "0000" )).format( iconHandleIndex );
                break;
        }
        ret += "}";
        return ret;
    }

    /// <summary>
    /// インスタンスをテキストファイルに出力します
    /// </summary>
    /// <param name="sw">出力先</param>
    public void write( TextMemoryStream sw ) {
        sw.writeLine( "[ID#" + (new DecimalFormat( "0000" )).format( value ) + "]" );
        sw.writeLine( "Type=" + type );
        switch( type ){
            case Anote:
                sw.writeLine( "Length=" + length );
                sw.writeLine( "Note#=" + note );
                sw.writeLine( "Dynamics=" + dynamics );
                sw.writeLine( "PMBendDepth=" + pmBendDepth );
                sw.writeLine( "PMBendLength=" + pmBendLength );
                sw.writeLine( "PMbPortamentoUse=" + pmbPortamentoUse );
                sw.writeLine( "DEMdecGainRate=" + demDecGainRate );
                sw.writeLine( "DEMaccent=" + demAccent );
                if ( lyricHandle != null ) {
                    sw.writeLine( "LyricHandle=h#" + (new DecimalFormat( "0000" )).format( lyricHandleIndex ) );
                }
                if ( vibratoHandle != null ) {
                    sw.writeLine( "VibratoHandle=h#" + (new DecimalFormat( "0000" )).format( vibratoHandleIndex ) );
                    sw.writeLine( "VibratoDelay=" + vibratoDelay );
                }
                if ( noteHeadHandle != null ) {
                    sw.writeLine( "NoteHeadHandle=h#" + (new DecimalFormat( "0000" )).format( noteHeadHandleIndex ) );
                }
                break;
            case Singer:
                sw.writeLine( "IconHandle=h#" + (new DecimalFormat( "0000" )).format( iconHandleIndex ) );
                break;
        }
    }
}
