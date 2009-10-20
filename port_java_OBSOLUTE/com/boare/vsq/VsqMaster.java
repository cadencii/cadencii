/*
 * VsqMaster.cs
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

import com.boare.corlib.*;

/// <summary>
/// vsqファイルのメタテキストの[Master]に記録される内容を取り扱う
/// </summary>
public class VsqMaster implements Cloneable {
    public int preMeasure;

    public static String getXmlElementName( String name ){
        if( name.equals( "preMeasure" ) ){
            return "PreMeasure";
        }
        return name;
    }

    public Object clone() {
        VsqMaster res = new VsqMaster( preMeasure );
        return res;
    }

    public VsqMaster(){
        this( 1 );
    }

    /// <summary>
    /// プリメジャー値を指定したコンストラクタ
    /// </summary>
    /// <param name="pre_measure"></param>
    public VsqMaster( int pre_measure ) {
        this.preMeasure = pre_measure;
    }

    /// <summary>
    /// テキストファイルからのコンストラクタ
    /// </summary>
    /// <param name="sr">読み込み元</param>
    /// <param name="last_line">最後に読み込んだ行が返されます</param>
    public VsqMaster( TextMemoryStream sr, StringBuilder last_line ) {
        preMeasure = 0;
        String[] spl;
        last_line.setLength( 0 );
        last_line.append( sr.readLine() );
        while ( !last_line.toString().startsWith( "[" ) ) {
            spl = last_line.toString().split( "=" );
            if( spl[0].equals( "PreMeasure" ) ){
                preMeasure = Integer.parseInt( spl[1] );
            }
            if ( sr.peek() < 0 ) {
                break;
            }
            last_line.setLength( 0 );
            last_line.append( sr.readLine() );
        }
    }

    /// <summary>
    /// インスタンスの内容をテキストファイルに出力します
    /// </summary>
    /// <param name="sw">出力先</param>
    public void write( TextMemoryStream sw ) {
        sw.writeLine( "[Master]" );
        sw.writeLine( "PreMeasure=" + preMeasure );
    }
}
