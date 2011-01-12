/*
* VsqMixer.cs
* Copyright (C) 2008-2011 kbinani
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
import java.util.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;

namespace org.kbinani.vsq
{
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// vsqファイルのメタテキストの[Mixer]セクションに記録される内容を取り扱う
    /// </summary>
#if JAVA
    public class VsqMixer implements Cloneable, Serializable {
#else
    [Serializable]
    public class VsqMixer : ICloneable
    {
#endif
        public int MasterFeder;
        public int MasterPanpot;
        public int MasterMute;
        public int OutputMode;

        /// <summary>
        /// vsqファイルの各トラックのfader, panpot, muteおよびoutputmode値を保持します
        /// </summary>
        public Vector<VsqMixerEntry> Slave = new Vector<VsqMixerEntry>();

        /// <summary>
        /// このクラスの指定した名前のプロパティが総称型引数を用いる型である場合に，
        /// その型の限定名を返します．それ以外の場合は空文字を返します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getGenericTypeName( String name )
        {
            if ( name != null ) {
                if ( name.Equals( "Slave" ) ) {
                    return "org.kbinani.vsq.VsqMixerEntry";
                }
            }
            return "";
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティを，XMLシリアライズ時に無視するかどうかを表す
        /// ブール値を返します．デフォルトの実装では戻り値は全てfalseです．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static boolean isXmlIgnored( String name )
        {
            return false;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getXmlElementName( String name )
        {
            return name;
        }

        public Object clone()
        {
            VsqMixer res = new VsqMixer( MasterFeder, MasterPanpot, MasterMute, OutputMode );
            res.Slave = new Vector<VsqMixerEntry>();
            for ( Iterator<VsqMixerEntry> itr = Slave.iterator(); itr.hasNext(); ) {
                VsqMixerEntry item = itr.next();
                res.Slave.add( (VsqMixerEntry)item.clone() );
            }
            return res;
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif

        /// <summary>
        /// 各パラメータを指定したコンストラクタ
        /// </summary>
        /// <param name="master_fader">MasterFader値</param>
        /// <param name="master_panpot">MasterPanpot値</param>
        /// <param name="master_mute">MasterMute値</param>
        /// <param name="output_mode">OutputMode値</param>
        public VsqMixer( int master_fader, int master_panpot, int master_mute, int output_mode )
        {
            this.MasterFeder = master_fader;
            this.MasterMute = master_mute;
            this.MasterPanpot = master_panpot;
            this.OutputMode = output_mode;
            Slave = new Vector<VsqMixerEntry>();
        }

#if JAVA
        public VsqMixer(){
            this( 0, 0, 0, 0 );
#else
        public VsqMixer()
            : this( 0, 0, 0, 0 )
        {
#endif
        }

        /// <summary>
        /// テキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込み対象</param>
        /// <param name="last_line">最後に読み込んだ行が返されます</param>
        public VsqMixer( TextStream sr, ByRef<String> last_line )
        {
            MasterFeder = 0;
            MasterPanpot = 0;
            MasterMute = 0;
            OutputMode = 0;
            //Tracks = 1;
            int tracks = 0;
            String[] spl;
            String buffer = "";
            last_line.value = sr.readLine();
            while ( !last_line.value.StartsWith( "[" ) ) {
                spl = PortUtil.splitString( last_line.value, new char[] { '=' } );
                if ( spl[0].Equals( "MasterFeder" ) ) {
                    MasterFeder = PortUtil.parseInt( spl[1] );
                } else if ( spl[0].Equals( "MasterPanpot" ) ) {
                    MasterPanpot = PortUtil.parseInt( spl[1] );
                } else if ( spl[0].Equals( "MasterMute" ) ) {
                    MasterMute = PortUtil.parseInt( spl[1] );
                } else if ( spl[0].Equals( "OutputMode" ) ) {
                    OutputMode = PortUtil.parseInt( spl[1] );
                } else if ( spl[0].Equals( "Tracks" ) ) {
                    tracks = PortUtil.parseInt( spl[1] );
                } else {
                    if ( spl[0].StartsWith( "Feder" ) ||
                         spl[0].StartsWith( "Panpot" ) ||
                         spl[0].StartsWith( "Mute" ) ||
                         spl[0].StartsWith( "Solo" ) ) {
                        buffer += spl[0] + "=" + spl[1] + "\n";
                    }
                }
                if ( !sr.ready() ) {
                    break;
                }
                last_line.value = sr.readLine().ToString();
            }

            Slave = new Vector<VsqMixerEntry>();
            for ( int i = 0; i < tracks; i++ ) {
                Slave.add( new VsqMixerEntry( 0, 0, 0, 0 ) );
            }
            spl = PortUtil.splitString( buffer, new String[] { "\n" }, true );
            String[] spl2;
            for ( int i = 0; i < spl.Length; i++ ) {
                String ind = "";
                int index;
                spl2 = PortUtil.splitString( spl[i], new char[] { '=' } );
                if ( spl2[0].StartsWith( "Feder" ) ) {
                    ind = spl2[0].Replace( "Feder", "" );
                    index = PortUtil.parseInt( ind );
                    Slave.get( index ).Feder = PortUtil.parseInt( spl2[1] );
                } else if ( spl2[0].StartsWith( "Panpot" ) ) {
                    ind = spl2[0].Replace( "Panpot", "" );
                    index = PortUtil.parseInt( ind );
                    Slave.get( index ).Panpot = PortUtil.parseInt( spl2[1] );
                } else if ( spl2[0].StartsWith( "Mute" ) ) {
                    ind = spl2[0].Replace( "Mute", "" );
                    index = PortUtil.parseInt( ind );
                    Slave.get( index ).Mute = PortUtil.parseInt( spl2[1] );
                } else if ( spl2[0].StartsWith( "Solo" ) ) {
                    ind = spl2[0].Replace( "Solo", "" );
                    index = PortUtil.parseInt( ind );
                    Slave.get( index ).Solo = PortUtil.parseInt( spl2[1] );
                }

            }
        }

        /// <summary>
        /// このインスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力対象</param>
        public void write( ITextWriter sw )
#if JAVA
            throws java.io.IOException
#endif
        {
            sw.writeLine( "[Mixer]" );
            sw.writeLine( "MasterFeder=" + MasterFeder );
            sw.writeLine( "MasterPanpot=" + MasterPanpot );
            sw.writeLine( "MasterMute=" + MasterMute );
            sw.writeLine( "OutputMode=" + OutputMode );
            int count = Slave.size();
            sw.writeLine( "Tracks=" + count );
            for ( int i = 0; i < count; i++ ) {
                VsqMixerEntry item = Slave.get( i );
                sw.writeLine( "Feder" + i + "=" + item.Feder );
                sw.writeLine( "Panpot" + i + "=" + item.Panpot );
                sw.writeLine( "Mute" + i + "=" + item.Mute );
                sw.writeLine( "Solo" + i + "=" + item.Solo );
            }
        }
    }

#if !JAVA
}
#endif
