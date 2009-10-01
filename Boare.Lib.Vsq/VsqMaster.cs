/*
* VsqMetaText/Master.cs
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
using System.IO;
using System.Text;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    /// <summary>
    /// vsqファイルのメタテキストの[Master]に記録される内容を取り扱う
    /// </summary>
    [Serializable]
    public class VsqMaster : ICloneable {
        public int PreMeasure;

        public object Clone() {
            VsqMaster res = new VsqMaster( PreMeasure );
            return res;
        }

        public VsqMaster()
            : this( 1 ) {
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
        public VsqMaster( TextMemoryStream sr, ref String last_line ) {
            PreMeasure = 0;
            String[] spl;
            last_line = sr.readLine();
            while ( !last_line.StartsWith( "[" ) ) {
                spl = PortUtil.splitString( last_line, new char[] { '=' } );
                switch ( spl[0] ) {
                    case "PreMeasure":
                        this.PreMeasure = PortUtil.parseInt( spl[1] );
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
            sw.writeLine( "[Master]" );
            sw.writeLine( "PreMeasure=" + PreMeasure );
        }

        /// <summary>
        /// VsqMasterのインスタンスを構築するテストを行います
        /// </summary>
        /// <returns>テストに成功すればtrue、そうでなければfalseを返します</returns>
        public static boolean test() {
            String fpath = Path.GetTempFileName();
            using ( StreamWriter sw = new StreamWriter( fpath, false, Encoding.Unicode ) ) {
                sw.WriteLine( "PreMeasure=2" );
                sw.WriteLine( "[Mixer]" );
            }

            boolean result;
            using ( TextMemoryStream sr = new TextMemoryStream( fpath, Encoding.Unicode ) ) {
                String last_line = "";
                VsqMaster vsqMaster = new VsqMaster( sr, ref last_line );
                if ( vsqMaster.PreMeasure == 2 &&
                    last_line.Equals( "[Mixer]" ) ) {
                    result = true;
                } else {
                    result = false;
                }
            }
            PortUtil.deleteFile( fpath );
            return result;
        }
    }

}
