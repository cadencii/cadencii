/*
 * VsqMetaText/Common.cs
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
using System.Text;
using System.IO;
using System.Drawing;

using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    /// <summary>
    /// vsqファイルのメタテキストの[Common]セクションに記録される内容を取り扱う
    /// </summary>
    [Serializable]
    public class VsqCommon : ICloneable {
        public String Version;
        public String Name;
        public String Color;
        public int DynamicsMode;
        public int PlayMode = 1;

        public object Clone() {
            String[] spl = Color.Split( ",".ToCharArray(), 3 );
            int r = int.Parse( spl[0] );
            int g = int.Parse( spl[1] );
            int b = int.Parse( spl[2] );
            System.Drawing.Color color = System.Drawing.Color.FromArgb( r, g, b );
            VsqCommon res = new VsqCommon( Name, color, DynamicsMode, PlayMode );
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
        public VsqCommon( String name, Color color, int dynamics_mode, int play_mode ) {
            this.Version = "DSB301";
            this.Name = name;
            this.Color = color.R + "," + color.G + "," + color.B;
            this.DynamicsMode = dynamics_mode;
            this.PlayMode = play_mode;
        }

        public VsqCommon()
            : this( "Miku", System.Drawing.Color.FromArgb( 179, 181, 123 ), 1, 1 ) {
        }

        /// <summary>
        /// MetaTextのテキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込むテキストファイル</param>
        /// <param name="last_line">読み込んだ最後の行が返される</param>
        public VsqCommon( TextMemoryStream sr, ref String last_line ) {
            Version = "";
            Name = "";
            Color = "0,0,0";
            DynamicsMode = 0;
            PlayMode = 1;
            last_line = sr.readLine();
            String[] spl;
            while ( !last_line.StartsWith( "[" ) ) {
                spl = last_line.Split( new char[] { '=' } );
                switch ( spl[0] ) {
                    case "Version":
                        this.Version = spl[1];
                        break;
                    case "Name":
                        this.Name = spl[1];
                        break;
                    case "Color":
                        this.Color = spl[1];
                        break;
                    case "DynamicsMode":
                        this.DynamicsMode = int.Parse( spl[1] );
                        break;
                    case "PlayMode":
                        this.PlayMode = int.Parse( spl[1] );
                        break;
                }
                if ( sr.peek() < 0 ) {
                    break;
                }
                last_line = sr.readLine();
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
        public static boolean test() {
            String fpath = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter( fpath, false, Encoding.Unicode );
            sw.WriteLine( "Version=DSB301" );
            sw.WriteLine( "Name=Voice1" );
            sw.WriteLine( "Color=181,162,123" );
            sw.WriteLine( "DynamicsMode=1" );
            sw.WriteLine( "PlayMode=1" );
            sw.WriteLine( "[Master]" );
            sw.Close();

            VsqCommon vsqCommon;
            String last_line = "";
            using ( TextMemoryStream sr = new TextMemoryStream( fpath, Encoding.Unicode ) ) {
                vsqCommon = new VsqCommon( sr, ref last_line );
            }

            boolean result;
            if ( vsqCommon.Version.Equals( "DSB301" ) &&
                vsqCommon.Name.Equals( "Voice1" ) &&
                vsqCommon.Color.Equals( "181,162,123" ) &&
                vsqCommon.DynamicsMode == 1 &&
                vsqCommon.PlayMode == 1 &&
                last_line.Equals( "[Master]" ) ) {
                result = true;
            } else {
                result = false;
            }

            File.Delete( fpath );
            return result;
        }
    }

}
