/*
* VsqMetaText/Mixer.cs
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
using System.IO;
using System.Text;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    /// <summary>
    /// vsqファイルのメタテキストの[Mixer]セクションに記録される内容を取り扱う
    /// </summary>
    [Serializable]
    public class VsqMixer : ICloneable {
        public int MasterFeder;
        public int MasterPanpot;
        public int MasterMute;
        public int OutputMode;

        /// <summary>
        /// vsqファイルの各トラックのfader, panpot, muteおよびoutputmode値を保持します
        /// </summary>
        public Vector<VsqMixerEntry> Slave = new Vector<VsqMixerEntry>();

        public object Clone() {
            VsqMixer res = new VsqMixer( MasterFeder, MasterPanpot, MasterMute, OutputMode );
            res.Slave = new Vector<VsqMixerEntry>();
            for ( Iterator itr = Slave.iterator(); itr.hasNext(); ) {
                VsqMixerEntry item = (VsqMixerEntry)itr.next();
                res.Slave.add( (VsqMixerEntry)item.Clone() );
            }
            return res;
        }

        /// <summary>
        /// 各パラメータを指定したコンストラクタ
        /// </summary>
        /// <param name="master_fader">MasterFader値</param>
        /// <param name="master_panpot">MasterPanpot値</param>
        /// <param name="master_mute">MasterMute値</param>
        /// <param name="output_mode">OutputMode値</param>
        public VsqMixer( int master_fader, int master_panpot, int master_mute, int output_mode ) {
            this.MasterFeder = master_fader;
            this.MasterMute = master_mute;
            this.MasterPanpot = master_panpot;
            this.OutputMode = output_mode;
            Slave = new Vector<VsqMixerEntry>();
        }

        public VsqMixer()
            : this( 0, 0, 0, 0 ) {
        }

        /// <summary>
        /// テキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込み対象</param>
        /// <param name="last_line">最後に読み込んだ行が返されます</param>
        public VsqMixer( TextMemoryStream sr, ref String last_line ) {
            MasterFeder = 0;
            MasterPanpot = 0;
            MasterMute = 0;
            OutputMode = 0;
            //Tracks = 1;
            int tracks = 0;
            String[] spl;
            String buffer = "";
            last_line = sr.readLine();
            while ( !last_line.StartsWith( "[" ) ) {
                spl = PortUtil.splitString( last_line, new char[] { '=' } );
                switch ( spl[0] ) {
                    case "MasterFeder":
                        MasterFeder = PortUtil.parseInt( spl[1] );
                        break;
                    case "MasterPanpot":
                        MasterPanpot = PortUtil.parseInt( spl[1] );
                        break;
                    case "MasterMute":
                        MasterMute = PortUtil.parseInt( spl[1] );
                        break;
                    case "OutputMode":
                        OutputMode = PortUtil.parseInt( spl[1] );
                        break;
                    case "Tracks":
                        tracks = PortUtil.parseInt( spl[1] );
                        break;
                    default:
                        if ( spl[0].StartsWith( "Feder" ) ||
                            spl[0].StartsWith( "Panpot" ) ||
                            spl[0].StartsWith( "Mute" ) ||
                            spl[0].StartsWith( "Solo" ) ) {
                            buffer += spl[0] + "=" + spl[1] + Environment.NewLine;
                        }
                        break;
                }
                if ( sr.peek() < 0 ) {
                    break;
                }
                last_line = sr.readLine();
            }

            Slave = new Vector<VsqMixerEntry>();
            for ( int i = 0; i < tracks; i++ ) {
                Slave.add( new VsqMixerEntry( 0, 0, 0, 0 ) );
            }
            spl = PortUtil.splitString( buffer, new String[] { Environment.NewLine }, true );
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
        public void write( TextMemoryStream sw ) {
            sw.writeLine( "[Mixer]" );
            sw.writeLine( "MasterFeder=" + MasterFeder );
            sw.writeLine( "MasterPanpot=" + MasterPanpot );
            sw.writeLine( "MasterMute=" + MasterMute );
            sw.writeLine( "OutputMode=" + OutputMode );
            sw.writeLine( "Tracks=" + Slave.size() );
            for ( int i = 0; i < Slave.size(); i++ ) {
                sw.writeLine( "Feder" + i + "=" + Slave.get( i ).Feder );
                sw.writeLine( "Panpot" + i + "=" + Slave.get( i ).Panpot );
                sw.writeLine( "Mute" + i + "=" + Slave.get( i ).Mute );
                sw.writeLine( "Solo" + i + "=" + Slave.get( i ).Solo );
            }
        }

        /// <summary>
        /// VsqMixerのインスタンスを構築するテストを行います
        /// </summary>
        /// <returns>テストに成功すればtrue、そうでなければfalseを返します</returns>
        public static boolean test() {
            String fpath = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter( fpath, false, Encoding.Unicode );
            sw.WriteLine( "MasterFeder=12" );
            sw.WriteLine( "MasterPanpot=13" );
            sw.WriteLine( "MasterMute=14" );
            sw.WriteLine( "OutputMode=15" );
            sw.WriteLine( "Tracks=8" );
            sw.WriteLine( "Feder0=1" );
            sw.WriteLine( "Panpot0=2" );
            sw.WriteLine( "Mute0=3" );
            sw.WriteLine( "Solo0=4" );
            sw.WriteLine( "Feder1=5" );
            sw.WriteLine( "Panpot1=6" );
            sw.WriteLine( "Mute1=7" );
            sw.WriteLine( "Solo1=8" );
            sw.WriteLine( "Feder2=9" );
            sw.WriteLine( "Panpot2=10" );
            sw.WriteLine( "Mute2=11" );
            sw.WriteLine( "Solo2=12" );
            sw.WriteLine( "Feder3=13" );
            sw.WriteLine( "Panpot3=14" );
            sw.WriteLine( "Mute3=15" );
            sw.WriteLine( "Solo3=16" );
            sw.WriteLine( "Feder4=17" );
            sw.WriteLine( "Panpot4=18" );
            sw.WriteLine( "Mute4=19" );
            sw.WriteLine( "Solo4=20" );
            sw.WriteLine( "Feder5=21" );
            sw.WriteLine( "Panpot5=22" );
            sw.WriteLine( "Mute5=23" );
            sw.WriteLine( "Solo5=24" );
            sw.WriteLine( "Feder6=25" );
            sw.WriteLine( "Panpot6=26" );
            sw.WriteLine( "Mute6=27" );
            sw.WriteLine( "Solo6=28" );
            sw.WriteLine( "Feder7=29" );
            sw.WriteLine( "Panpot7=30" );
            sw.WriteLine( "Mute7=31" );
            sw.WriteLine( "Solo7=32" );
            sw.WriteLine( "[EventList]" );
            sw.Close();

            TextMemoryStream sr = new TextMemoryStream( fpath, Encoding.Unicode );
            String last_line = "";
            VsqMixer vsqMixer = new VsqMixer( sr, ref last_line );

            if ( vsqMixer.MasterFeder == 12 &&
                vsqMixer.MasterPanpot == 13 &&
                vsqMixer.MasterMute == 14 &&
                vsqMixer.OutputMode == 15 &&
                vsqMixer.Slave.size() == 8 ) {
                for ( int i = 0; i < vsqMixer.Slave.size(); i++ ) {
                    int start = 4 * i;
                    if ( vsqMixer.Slave.get( i ).Feder != start + 1 ||
                        vsqMixer.Slave.get( i ).Panpot != start + 2 ||
                        vsqMixer.Slave.get( i ).Mute != start + 3 ||
                        vsqMixer.Slave.get( i ).Solo != start + 4 ) {
                        sr.close();
                        PortUtil.deleteFile( fpath );
                        return false;
                    }
                }
            } else {
                sr.close();
                PortUtil.deleteFile( fpath );
                return false;
            }
            sr.close();
            PortUtil.deleteFile( fpath );
            return true;
        }
    }

    /// <summary>
    /// VsqMixerのSlave要素に格納される各エントリ
    /// </summary>
    [Serializable]
    public class VsqMixerEntry : ICloneable {
        public int Feder;
        public int Panpot;
        public int Mute;
        public int Solo;

        public object Clone() {
            VsqMixerEntry res = new VsqMixerEntry( Feder, Panpot, Mute, Solo );
            return res;
        }

        /// <summary>
        /// 各パラメータを指定したコンストラクタ
        /// </summary>
        /// <param name="feder">Feder値</param>
        /// <param name="panpot">Panpot値</param>
        /// <param name="mute">Mute値</param>
        /// <param name="solo">Solo値</param>
        public VsqMixerEntry( int feder, int panpot, int mute, int solo ) {
            this.Feder = feder;
            this.Panpot = panpot;
            this.Mute = mute;
            this.Solo = solo;
        }

        public VsqMixerEntry()
            : this( 0, 0, 0, 0 ) {
        }
    }

}
