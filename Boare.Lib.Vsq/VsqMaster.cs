/*
* VsqMaster.cs
* Copyright (c) 2008-2009 kbinani
*
* This file is part of org.kbinani.vsq.
*
* Boare.Lib.Vsq is free software; you can redistribute it and/or
* modify it under the terms of the BSD License.
*
* Boare.Lib.Vsq is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
#if JAVA
package org.kbinani.vsq;

import java.io.*;
import org.kbinani.*;
#else
using System;
using bocoree;
using bocoree.java.io;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// vsqファイルのメタテキストの[Master]に記録される内容を取り扱う
    /// </summary>
#if JAVA
    public class VsqMaster implements Cloneable, Serializable {
#else
    [Serializable]
    public class VsqMaster : ICloneable {
#endif
        public int PreMeasure;

        public Object clone() {
            VsqMaster res = new VsqMaster( PreMeasure );
            return res;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

#if JAVA
        public VsqMaster(){
            this( 1 );
#else
        public VsqMaster()
            : this( 1 ) {
#endif
        }

        /// <summary>
        /// プリメジャー値を指定したコンストラクタ
        /// </summary>
        /// <param name="pre_measure"></param>
        public VsqMaster( int pre_measure ) {
            this.PreMeasure = pre_measure;
        }

        /// <summary>
        /// テキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込み元</param>
        /// <param name="last_line">最後に読み込んだ行が返されます</param>
        public VsqMaster( TextMemoryStream sr, ByRef<String> last_line ) {
            PreMeasure = 0;
            String[] spl;
            last_line.value = sr.readLine();
            while ( !last_line.value.StartsWith( "[" ) ) {
                spl = PortUtil.splitString( last_line.value, new char[] { '=' } );
                if ( spl[0].Equals( "PreMeasure" ) ) {
                    this.PreMeasure = PortUtil.parseInt( spl[1] );
                }
                if ( sr.peek() < 0 ) {
                    break;
                }
                last_line.value = sr.readLine();
            }
        }

        /// <summary>
        /// インスタンスの内容をテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力先</param>
        public void write( TextMemoryStream sw ) {
            sw.writeLine( "[Master]" );
            sw.writeLine( "PreMeasure=" + PreMeasure );
        }

        /// <summary>
        /// VsqMasterのインスタンスを構築するテストを行います
        /// </summary>
        /// <returns>テストに成功すればtrue、そうでなければfalseを返します</returns>
        public static boolean test() {
            String fpath = PortUtil.createTempFile();
            BufferedWriter sw = null;
            try {
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( fpath ), "UTF8" ) );
                sw.write( "PreMeasure=2" );
                sw.newLine();
                sw.write( "[Mixer]" );
                sw.newLine();
            } catch ( Exception ex ) {
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }

            boolean result = false;
            TextMemoryStream sr = null;
            try {
                sr = new TextMemoryStream( fpath, "UTF8" );
                ByRef<String> last_line = new ByRef<String>( "" );
                VsqMaster vsqMaster = new VsqMaster( sr, last_line );
                if ( vsqMaster.PreMeasure == 2 &&
                    last_line.value.Equals( "[Mixer]" ) ) {
                    result = true;
                } else {
                    result = false;
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
            PortUtil.deleteFile( fpath );
            return result;
        }
    }

#if !JAVA
}
#endif
