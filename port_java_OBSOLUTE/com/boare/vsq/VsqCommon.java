/*
 * VsqCommon.java
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

import java.awt.*;

/**
 * vsqファイルのメタテキストの[Common]セクションに記録される内容を取り扱う
 */
public class VsqCommon implements Cloneable {
    public String version;
    public String name;
    public String color;
    public int dynamicsMode;
    public int playMode;

    public Object clone() {
        String[] spl = color.split( ",", 3 );
        int r = Integer.parseInt( spl[0] );
        int g = Integer.parseInt( spl[1] );
        int b = Integer.parseInt( spl[2] );
        Color color_ = new Color( r, g, b );
        VsqCommon res = new VsqCommon( name, color_, dynamicsMode, playMode );
        res.version = version;
        return res;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "version" ) ){
            return "Version";
        }else if( name.equals( "name" ) ){
            return "Name";
        }else if( name.equals( "color" ) ){
            return "Color";
        }else if( name.equals( "dynamicsMode" ) ){
            return "DynamicsMode";
        }else if( name.equals( "playMode" ) ){
            return "PlayMode";
        }
        return name;
    }

    /// <summary>
    /// 各パラメータを指定したコンストラクタ
    /// </summary>
    /// <param name="name">トラック名</param>
    /// <param name="color">Color値（意味は不明）</param>
    /// <param name="dynamics_mode">DynamicsMode（デフォルトは1）</param>
    /// <param name="play_mode">PlayMode（デフォルトは1）</param>
    public VsqCommon( String name_, Color color_, int dynamics_mode, int play_mode ) {
        this.version = "DSB301";
        this.name = name;
        this.color = color_.getRed() + "," + color_.getGreen() + "," + color_.getBlue();
        this.dynamicsMode = dynamics_mode;
        this.playMode = play_mode;
    }


    /// <summary>
    /// MetaTextのテキストファイルからのコンストラクタ
    /// </summary>
    /// <param name="sr">読み込むテキストファイル</param>
    /// <param name="last_line">読み込んだ最後の行が返される</param>
    public VsqCommon( TextMemoryStream sr, StringBuilder last_line ) {
        version = "";
        name = "";
        color = "0,0,0";
        dynamicsMode = 0;
        playMode = 0;
        String line;
        String[] spl;
        while( (line = sr.readLine()) != null ){
            if( line.startsWith( "[" ) ){
                break;
            }
            spl = last_line.toString().split( "=" );
            if( spl[0].equals( "Version" ) ){
                this.version = spl[1];
            }else if( spl[0].equals( "Name" ) ){
                this.name = spl[1];
            }else if( spl[0].equals( "Color" ) ){
                this.color = spl[1];
            }else if( spl[0].equals( "DynamicsMode" ) ){
                this.dynamicsMode = Integer.parseInt( spl[1] );
            }else if( spl[0].equals( "PlayMode" ) ){
                this.playMode = Integer.parseInt( spl[1] );
            }
        }
        last_line.setLength( 0 );
        last_line.append( line );
    }


    /// <summary>
    /// インスタンスの内容をテキストファイルに出力します
    /// </summary>
    /// <param name="sw">出力先</param>
    public void write( TextMemoryStream sw ) {
        sw.writeLine( "[Common]" );
        sw.writeLine( "Version=" + version );
        sw.writeLine( "Name=" + name );
        sw.writeLine( "Color=" + color );
        sw.writeLine( "DynamicsMode=" + dynamicsMode );
        sw.writeLine( "PlayMode=" + playMode );
    }
}
