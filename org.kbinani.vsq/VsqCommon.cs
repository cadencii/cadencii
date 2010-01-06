/*
 * VsqCommon.cs
 * Copyright (C) 2008-2010 kbinani
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
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.io;

namespace org.kbinani.vsq {

    using boolean = System.Boolean;
#endif

    /// <summary>
    /// vsqファイルのメタテキストの[Common]セクションに記録される内容を取り扱う
    /// </summary>
#if JAVA
    public class VsqCommon implements Cloneable, Serializable {
#else
    [Serializable]
    public class VsqCommon : ICloneable {
#endif
        public String Version;
        public String Name;
        public String Color;
        public int DynamicsMode;
        public int PlayMode = 1;

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public Object clone() {
            String[] spl = PortUtil.splitString( Color, new char[] { ',' }, 3 );
            int r = PortUtil.parseInt( spl[0] );
            int g = PortUtil.parseInt( spl[1] );
            int b = PortUtil.parseInt( spl[2] );
            VsqCommon res = new VsqCommon( Name, r, g, b, DynamicsMode, PlayMode );
            res.Version = Version;
            return res;
        }

        /// <summary>
        /// 各パラメータを指定したコンストラクタ
        /// </summary>
        /// <param name="name">トラック名</param>
        /// <param name="color">Color値（意味は不明）</param>
        /// <param name="dynamics_mode">DynamicsMode（デフォルトは1）</param>
        /// <param name="play_mode">PlayMode（デフォルトは1）</param>
        public VsqCommon( String name, int red, int green, int blue, int dynamics_mode, int play_mode ) {
            this.Version = "DSB301";
            this.Name = name;
            this.Color = red + "," + green + "," + blue;
            this.DynamicsMode = dynamics_mode;
            this.PlayMode = play_mode;
        }

#if JAVA
        public VsqCommon(){
            this(  "Miku", 179, 181, 123, 1, 1 );
#else
        public VsqCommon()
            : this( "Miku", 179, 181, 123, 1, 1 ) {
#endif
        }

        /// <summary>
        /// MetaTextのテキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込むテキストファイル</param>
        /// <param name="last_line">読み込んだ最後の行が返される</param>
        public VsqCommon( TextMemoryStream sr, ByRef<String> last_line ) {
            Version = "";
            Name = "";
            Color = "0,0,0";
            DynamicsMode = 0;
            PlayMode = 1;
            last_line.value = sr.readLine().ToString();
            String[] spl;
            while ( !last_line.value.StartsWith( "[" ) ) {
                spl = PortUtil.splitString( last_line.value, new char[] { '=' } );
                String search = spl[0];
                if ( search.Equals( "Version" ) ) {
                    this.Version = spl[1];
                } else if ( search.Equals( "Name" ) ) {
                    this.Name = spl[1];
                } else if ( search.Equals( "Color" ) ) {
                    this.Color = spl[1];
                } else if ( search.Equals( "DynamicsMode" ) ) {
                    this.DynamicsMode = PortUtil.parseInt( spl[1] );
                } else if ( search.Equals( "PlayMode" ) ) {
                    this.PlayMode = PortUtil.parseInt( spl[1] );
                }
                if ( sr.peek() < 0 ) {
                    break;
                }
                last_line.value = sr.readLine().ToString();
            }
        }

        /// <summary>
        /// MetaTextのテキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込むテキストファイル</param>
        /// <param name="last_line">読み込んだ最後の行が返される</param>
        public VsqCommon( TextStream sr, ByRef<String> last_line ) {
            Version = "";
            Name = "";
            Color = "0,0,0";
            DynamicsMode = 0;
            PlayMode = 1;
            last_line.value = sr.readLine();
            String[] spl;
            while ( !last_line.value.StartsWith( "[" ) ) {
                spl = PortUtil.splitString( last_line.value, new char[] { '=' } );
                String search = spl[0];
                if ( search.Equals( "Version" ) ) {
                    this.Version = spl[1];
                } else if ( search.Equals( "Name" ) ) {
                    this.Name = spl[1];
                } else if ( search.Equals( "Color" ) ) {
                    this.Color = spl[1];
                } else if ( search.Equals( "DynamicsMode" ) ) {
                    this.DynamicsMode = PortUtil.parseInt( spl[1] );
                } else if ( search.Equals( "PlayMode" ) ) {
                    this.PlayMode = PortUtil.parseInt( spl[1] );
                }
                if ( !sr.ready() ) {
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
            sw.writeLine( "[Common]" );
            sw.writeLine( "Version=" + Version );
            sw.writeLine( "Name=" + Name );
            sw.writeLine( "Color=" + Color );
            sw.writeLine( "DynamicsMode=" + DynamicsMode );
            sw.writeLine( "PlayMode=" + PlayMode );
        }

        /// <summary>
        /// VsqCommon構造体を構築するテストを行います
        /// </summary>
        /// <returns>テストに成功すればtrue、そうでなければfalse</returns>
        public static boolean test()
#if JAVA
            throws IOException
#endif
        {
            String fpath = PortUtil.createTempFile();
            BufferedWriter sw = new BufferedWriter( new FileWriter( fpath ) );
            sw.write( "Version=DSB301" );
            sw.newLine();
            sw.write( "Name=Voice1" );
            sw.newLine();
            sw.write( "Color=181,162,123" );
            sw.newLine();
            sw.write( "DynamicsMode=1" );
            sw.newLine();
            sw.write( "PlayMode=1" );
            sw.newLine();
            sw.write( "[Master]" );
            sw.newLine();
            sw.close();

            VsqCommon vsqCommon = null;
            ByRef<String> last_line = new ByRef<String>( "" );
            TextMemoryStream sr = null;
            try {
                sr = new TextMemoryStream( fpath, "UTF8" );
                vsqCommon = new VsqCommon( sr, last_line );
            } catch ( Exception ex ) {
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
            if ( vsqCommon == null ) {
                vsqCommon = new VsqCommon();
            }

            boolean result;
            if ( vsqCommon.Version.Equals( "DSB301" ) &&
                vsqCommon.Name.Equals( "Voice1" ) &&
                vsqCommon.Color.Equals( "181,162,123" ) &&
                vsqCommon.DynamicsMode == 1 &&
                vsqCommon.PlayMode == 1 &&
                last_line.value.Equals( "[Master]" ) ) {
                result = true;
            } else {
                result = false;
            }

            PortUtil.deleteFile( fpath );
            return result;
        }
    }

#if !JAVA
}
#endif
