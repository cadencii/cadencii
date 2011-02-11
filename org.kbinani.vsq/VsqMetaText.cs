/*
* VsqMetaText.cs
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

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.vsq
{
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

    /// <summary>
    /// vsqのメタテキストの中身を処理するためのクラス
    /// </summary>
#if JAVA
    public class VsqMetaText implements Cloneable, Serializable
#else
    [Serializable]
    public class VsqMetaText : ICloneable
#endif
    {
        public VsqCommon Common;
        public VsqMaster master;
        public VsqMixer mixer;
        public VsqEventList Events;
        /// <summary>
        /// PIT。ピッチベンド(pitchBendBPList)。default=0
        /// </summary>
        public VsqBPList PIT;
        /// <summary>
        /// PBS。ピッチベンドセンシティビティ(pitchBendSensBPList)。dfault=2
        /// </summary>
        public VsqBPList PBS;
        /// <summary>
        /// DYN。ダイナミクス(dynamicsBPList)。default=64
        /// </summary>
        public VsqBPList DYN;
        /// <summary>
        /// BRE。ブレシネス(epRResidualBPList)。default=0
        /// </summary>
        public VsqBPList BRE;
        /// <summary>
        /// BRI。ブライトネス(epRESlopeBPList)。default=64
        /// </summary>
        public VsqBPList BRI;
        /// <summary>
        /// CLE。クリアネス(epRESlopeDepthBPList)。default=0
        /// </summary>
        public VsqBPList CLE;
        public VsqBPList reso1FreqBPList;
        public VsqBPList reso2FreqBPList;
        public VsqBPList reso3FreqBPList;
        public VsqBPList reso4FreqBPList;
        public VsqBPList reso1BWBPList;
        public VsqBPList reso2BWBPList;
        public VsqBPList reso3BWBPList;
        public VsqBPList reso4BWBPList;
        public VsqBPList reso1AmpBPList;
        public VsqBPList reso2AmpBPList;
        public VsqBPList reso3AmpBPList;
        public VsqBPList reso4AmpBPList;
        /// <summary>
        /// Harmonics。(EpRSineBPList)default = 64
        /// </summary>
        public VsqBPList harmonics;
        /// <summary>
        /// Effect2 Depth。
        /// </summary>
        public VsqBPList fx2depth;
        /// <summary>
        /// GEN。ジェンダーファクター(genderFactorBPList)。default=64
        /// </summary>
        public VsqBPList GEN;
        /// <summary>
        /// POR。ポルタメントタイミング(portamentoTimingBPList)。default=64
        /// </summary>
        public VsqBPList POR;
        /// <summary>
        /// OPE。オープニング(openingBPList)。default=127
        /// </summary>
        public VsqBPList OPE;

        public Object clone()
        {
            VsqMetaText res = new VsqMetaText();
            if ( Common != null ) {
                res.Common = (VsqCommon)Common.clone();
            }
            if ( master != null ) {
                res.master = (VsqMaster)master.clone();
            }
            if ( mixer != null ) {
                res.mixer = (VsqMixer)mixer.clone();
            }
            if ( Events != null ) {
                res.Events = new VsqEventList();
                for ( Iterator<VsqEvent> itr = Events.iterator(); itr.hasNext(); ) {
                    VsqEvent item = itr.next();
                    res.Events.add( (VsqEvent)item.clone(), item.InternalID );
                }
            }
            if ( PIT != null ) {
                res.PIT = (VsqBPList)PIT.clone();
            }
            if ( PBS != null ) {
                res.PBS = (VsqBPList)PBS.clone();
            }
            if ( DYN != null ) {
                res.DYN = (VsqBPList)DYN.clone();
            }
            if ( BRE != null ) {
                res.BRE = (VsqBPList)BRE.clone();
            }
            if ( BRI != null ) {
                res.BRI = (VsqBPList)BRI.clone();
            }
            if ( CLE != null ) {
                res.CLE = (VsqBPList)CLE.clone();
            }
            if ( reso1FreqBPList != null ) {
                res.reso1FreqBPList = (VsqBPList)reso1FreqBPList.clone();
            }
            if ( reso2FreqBPList != null ) {
                res.reso2FreqBPList = (VsqBPList)reso2FreqBPList.clone();
            }
            if ( reso3FreqBPList != null ) {
                res.reso3FreqBPList = (VsqBPList)reso3FreqBPList.clone();
            }
            if ( reso4FreqBPList != null ) {
                res.reso4FreqBPList = (VsqBPList)reso4FreqBPList.clone();
            }
            if ( reso1BWBPList != null ) {
                res.reso1BWBPList = (VsqBPList)reso1BWBPList.clone();
            }
            if ( reso2BWBPList != null ) {
                res.reso2BWBPList = (VsqBPList)reso2BWBPList.clone();
            }
            if ( reso3BWBPList != null ) {
                res.reso3BWBPList = (VsqBPList)reso3BWBPList.clone();
            }
            if ( reso4BWBPList != null ) {
                res.reso4BWBPList = (VsqBPList)reso4BWBPList.clone();
            }
            if ( reso1AmpBPList != null ) {
                res.reso1AmpBPList = (VsqBPList)reso1AmpBPList.clone();
            }
            if ( reso2AmpBPList != null ) {
                res.reso2AmpBPList = (VsqBPList)reso2AmpBPList.clone();
            }
            if ( reso3AmpBPList != null ) {
                res.reso3AmpBPList = (VsqBPList)reso3AmpBPList.clone();
            }
            if ( reso4AmpBPList != null ) {
                res.reso4AmpBPList = (VsqBPList)reso4AmpBPList.clone();
            }
            if ( harmonics != null ) {
                res.harmonics = (VsqBPList)harmonics.clone();
            }
            if ( fx2depth != null ) {
                res.fx2depth = (VsqBPList)fx2depth.clone();
            }
            if ( GEN != null ) {
                res.GEN = (VsqBPList)GEN.clone();
            }
            if ( POR != null ) {
                res.POR = (VsqBPList)POR.clone();
            }
            if ( OPE != null ) {
                res.OPE = (VsqBPList)OPE.clone();
            }
            return res;
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif

        public VsqEventList getEventList()
        {
            return Events;
        }

        public VsqBPList getElement( String curve )
        {
            String search = curve.Trim().ToLower();
            if ( str.compare( search, "bre" ) ) {
                return this.BRE;
            } else if ( str.compare( search, "bri" ) ) {
                return this.BRI;
            } else if ( str.compare( search, "cle" ) ) {
                return this.CLE;
            } else if ( str.compare( search, "dyn" ) ) {
                return this.DYN;
            } else if ( str.compare( search, "gen" ) ) {
                return this.GEN;
            } else if ( str.compare( search, "ope" ) ) {
                return this.OPE;
            } else if ( str.compare( search, "pbs" ) ) {
                return this.PBS;
            } else if ( str.compare( search, "pit" ) ) {
                return this.PIT;
            } else if ( str.compare( search, "por" ) ) {
                return this.POR;
            } else if ( str.compare( search, "harmonics" ) ) {
                return this.harmonics;
            } else if ( str.compare( search, "fx2depth" ) ) {
                return this.fx2depth;
            } else if ( str.compare( search, "reso1amp" ) ) {
                return this.reso1AmpBPList;
            } else if ( str.compare( search, "reso1bw" ) ) {
                return this.reso1BWBPList;
            } else if ( str.compare( search, "reso1freq" ) ) {
                return this.reso1FreqBPList;
            } else if ( str.compare( search, "reso2amp" ) ) {
                return this.reso2AmpBPList;
            } else if ( str.compare( search, "reso2bw" ) ) {
                return this.reso2BWBPList;
            } else if ( str.compare( search, "reso2freq" ) ) {
                return this.reso2FreqBPList;
            } else if ( str.compare( search, "reso3amp" ) ) {
                return this.reso3AmpBPList;
            } else if ( str.compare( search, "reso3bw" ) ) {
                return this.reso3BWBPList;
            } else if ( str.compare( search, "reso3freq" ) ) {
                return this.reso3FreqBPList;
            } else if ( str.compare( search, "reso4amp" ) ) {
                return this.reso4AmpBPList;
            } else if ( str.compare( search, "reso4bw" ) ) {
                return this.reso4BWBPList;
            } else if ( str.compare( search, "reso4freq" ) ) {
                return this.reso4FreqBPList;
            } else {
                return null;
            }
        }

        public void setElement( String curve, VsqBPList value )
        {
            String search = curve.Trim().ToLower();
            if ( str.compare( search, "bre" ) ) {
                this.BRE = value;
            } else if ( str.compare( search, "bri" ) ) {
                this.BRI = value;
            } else if ( str.compare( search, "cle" ) ) {
                this.CLE = value;
            } else if ( str.compare( search, "dyn" ) ) {
                this.DYN = value;
            } else if ( str.compare( search, "gen" ) ) {
                this.GEN = value;
            } else if ( str.compare( search, "ope" ) ) {
                this.OPE = value;
            } else if ( str.compare( search, "pbs" ) ) {
                this.PBS = value;
            } else if ( str.compare( search, "pit" ) ) {
                this.PIT = value;
            } else if ( str.compare( search, "por" ) ) {
                this.POR = value;
            } else if ( str.compare( search, "harmonics" ) ) {
                this.harmonics = value;
            } else if ( str.compare( search, "fx2depth" ) ) {
                this.fx2depth = value;
            } else if ( str.compare( search, "reso1amp" ) ) {
                this.reso1AmpBPList = value;
            } else if ( str.compare( search, "reso1bw" ) ) {
                this.reso1BWBPList = value;
            } else if ( str.compare( search, "reso1freq" ) ) {
                this.reso1FreqBPList = value;
            } else if ( str.compare( search, "reso2amp" ) ) {
                this.reso2AmpBPList = value;
            } else if ( str.compare( search, "reso2bw" ) ) {
                this.reso2BWBPList = value;
            } else if ( str.compare( search, "reso2freq" ) ) {
                this.reso2FreqBPList = value;
            } else if ( str.compare( search, "reso3amp" ) ) {
                this.reso3AmpBPList = value;
            } else if ( str.compare( search, "reso3bw" ) ) {
                this.reso3BWBPList = value;
            } else if ( str.compare( search, "reso3freq" ) ) {
                this.reso3FreqBPList = value;
            } else if ( str.compare( search, "reso4amp" ) ) {
                this.reso4AmpBPList = value;
            } else if ( str.compare( search, "reso4bw" ) ) {
                this.reso4BWBPList = value;
            } else if ( str.compare( search, "reso4freq" ) ) {
                this.reso4FreqBPList = value;
            } else {
#if DEBUG
                sout.println( "VsqMetaText#setElement; warning:unknown curve; curve=" + curve );
#endif
            }
        }

        /// <summary>
        /// Editor画面上で上からindex番目のカーブを表すBPListを求めます
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VsqBPList getCurve( int index )
        {
            switch ( index ) {
                case 1:
                return DYN;
                case 2:
                return BRE;
                case 3:
                return BRI;
                case 4:
                return CLE;
                case 5:
                return OPE;
                case 6:
                return GEN;
                case 7:
                return POR;
                case 8:
                return PIT;
                case 9:
                return PBS;
                default:
                return null;
            }
        }


        /// <summary>
        /// Editor画面上で上からindex番目のカーブの名前を調べます
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static String getCurveName( int index )
        {
            switch ( index ) {
                case 0:
                return "VEL";
                case 1:
                return "DYN";
                case 2:
                return "BRE";
                case 3:
                return "BRI";
                case 4:
                return "CLE";
                case 5:
                return "OPE";
                case 6:
                return "GEN";
                case 7:
                return "POR";
                case 8:
                return "PIT";
                case 9:
                return "PBS";
                default:
                return "";
            }
        }

        /// <summary>
        /// Singerプロパティに指定されている
        /// </summary>
        public String getSinger()
        {
            for ( Iterator<VsqEvent> itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if ( item.ID.type == VsqIDType.Singer ) {
                    return item.ID.IconHandle.IDS;
                }
            }
            return "";
        }

        public void setSinger( String value )
        {
            for ( Iterator<VsqEvent> itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if ( item.ID.type == VsqIDType.Singer ) {
                    ((IconHandle)item.ID.IconHandle).IDS = value;
                    break;
                }
            }
        }

        /// <summary>
        /// EOSイベントが記録されているクロックを取得します。
        /// </summary>
        /// <returns></returns>
        public int getIndexOfEOS()
        {
            int result;
            if ( Events.getCount() > 0 ) {
                int ilast = Events.getCount() - 1;
                result = Events.getElement( ilast ).Clock;
            } else {
                result = -1;
            }
            return result;
        }

        /// <summary>
        /// このインスタンスから、Handleのリストを作成すると同時に、Eventsに登録されているVsqEventのvalue値および各ハンドルのvalue値を更新します
        /// </summary>
        /// <returns></returns>
        private Vector<VsqHandle> buildHandleList()
        {
            Vector<VsqHandle> handle = new Vector<VsqHandle>();
            int current_id = -1;
            int current_handle = -1;
            boolean add_quotation_mark = true;
            boolean is_vocalo1 = str.startsWith( Common.Version, "DSB2" );
            boolean is_vocalo2 = str.startsWith( Common.Version, "DSB3" );
            for ( Iterator<VsqEvent> itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                current_id++;
                item.ID.value = current_id;
                // IconHandle
                if ( item.ID.IconHandle != null ) {
                    if ( item.ID.IconHandle is IconHandle ) {
                        IconHandle ish = (IconHandle)item.ID.IconHandle;
                        current_handle++;
                        VsqHandle handle_item = VsqHandle.castFromIconHandle( ish );
                        handle_item.Index = current_handle;
                        handle.add( handle_item );
                        item.ID.IconHandle_index = current_handle;
                        if ( is_vocalo1 ) {
                            VsqVoiceLanguage lang = VocaloSysUtil.getLanguageFromName( ish.IDS );
                            add_quotation_mark = lang == VsqVoiceLanguage.Japanese;
                        } else if ( is_vocalo2 ) {
                            VsqVoiceLanguage lang = VocaloSysUtil.getLanguageFromName( ish.IDS );
                            add_quotation_mark = lang == VsqVoiceLanguage.Japanese;
                        }
                    }
                }
                // LyricHandle
                if ( item.ID.LyricHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = VsqHandle.castFromLyricHandle( item.ID.LyricHandle );
                    handle_item.Index = current_handle;
                    handle_item.addQuotationMark = add_quotation_mark;
                    handle.add( handle_item );
                    item.ID.LyricHandle_index = current_handle;
                }
                // VibratoHandle
                if ( item.ID.VibratoHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.VibratoHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.add( handle_item );
                    item.ID.VibratoHandle_index = current_handle;
                }
                // NoteHeadHandle
                if ( item.ID.NoteHeadHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.NoteHeadHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.add( handle_item );
                    item.ID.NoteHeadHandle_index = current_handle;
                }
                // IconDynamicsHandle
                if ( item.ID.IconDynamicsHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.IconDynamicsHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle_item.setLength( item.ID.getLength() );
                    handle.add( handle_item );
                    item.ID.IconHandle_index = current_handle;
                }
            }
            return handle;
        }

        /// <summary>
        /// このインスタンスの内容を指定されたファイルに出力します。
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="encode"></param>
        public void print( ITextWriter sw, int eos, int start )
#if JAVA
            throws IOException
#endif
        {
            if ( Common != null ) {
                Common.write( sw );
            }
            if ( master != null ) {
                master.write( sw );
            }
            if ( mixer != null ) {
                mixer.write( sw );
            }
            Vector<VsqHandle> handle = writeEventList( sw, eos );
            for ( Iterator<VsqEvent> itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                item.write( sw );
            }
            for ( int i = 0; i < handle.size(); i++ ) {
                handle.get( i ).write( sw );
            }
            String version = Common.Version;
            if ( PIT.size() > 0 ) {
                PIT.print( sw, start, "[PitchBendBPList]" );
            }
            if ( PBS.size() > 0 ) {
                PBS.print( sw, start, "[PitchBendSensBPList]" );
            }
            if ( DYN.size() > 0 ) {
                DYN.print( sw, start, "[DynamicsBPList]" );
            }
            if ( BRE.size() > 0 ) {
                BRE.print( sw, start, "[EpRResidualBPList]" );
            }
            if ( BRI.size() > 0 ) {
                BRI.print( sw, start, "[EpRESlopeBPList]" );
            }
            if ( CLE.size() > 0 ) {
                CLE.print( sw, start, "[EpRESlopeDepthBPList]" );
            }
            if ( version.StartsWith( "DSB2" ) ) {
                if ( harmonics.size() > 0 ) {
                    harmonics.print( sw, start, "[EpRSineBPList]" );
                }
                if ( fx2depth.size() > 0 ) {
                    fx2depth.print( sw, start, "[VibTremDepthBPList]" );
                }

                if ( reso1FreqBPList.size() > 0 ) {
                    reso1FreqBPList.print( sw, start, "[Reso1FreqBPList]" );
                }
                if ( reso2FreqBPList.size() > 0 ) {
                    reso2FreqBPList.print( sw, start, "[Reso2FreqBPList]" );
                }
                if ( reso3FreqBPList.size() > 0 ) {
                    reso3FreqBPList.print( sw, start, "[Reso3FreqBPList]" );
                }
                if ( reso4FreqBPList.size() > 0 ) {
                    reso4FreqBPList.print( sw, start, "[Reso4FreqBPList]" );
                }

                if ( reso1BWBPList.size() > 0 ) {
                    reso1BWBPList.print( sw, start, "[Reso1BWBPList]" );
                }
                if ( reso2BWBPList.size() > 0 ) {
                    reso2BWBPList.print( sw, start, "[Reso2BWBPList]" );
                }
                if ( reso3BWBPList.size() > 0 ) {
                    reso3BWBPList.print( sw, start, "[Reso3BWBPList]" );
                }
                if ( reso4BWBPList.size() > 0 ) {
                    reso4BWBPList.print( sw, start, "[Reso4BWBPList]" );
                }

                if ( reso1AmpBPList.size() > 0 ) {
                    reso1AmpBPList.print( sw, start, "[Reso1AmpBPList]" );
                }
                if ( reso2AmpBPList.size() > 0 ) {
                    reso2AmpBPList.print( sw, start, "[Reso2AmpBPList]" );
                }
                if ( reso3AmpBPList.size() > 0 ) {
                    reso3AmpBPList.print( sw, start, "[Reso3AmpBPList]" );
                }
                if ( reso4AmpBPList.size() > 0 ) {
                    reso4AmpBPList.print( sw, start, "[Reso4AmpBPList]" );
                }
            }

            if ( GEN.size() > 0 ) {
                GEN.print( sw, start, "[GenderFactorBPList]" );
            }
            if ( POR.size() > 0 ) {
                POR.print( sw, start, "[PortamentoTimingBPList]" );
            }
            if ( version.StartsWith( "DSB3" ) ) {
                if ( OPE.size() > 0 ) {
                    OPE.print( sw, start, "[OpeningBPList]" );
                }
            }
        }

        private Vector<VsqHandle> writeEventListCor( ITextWriter writer, int eos )
#if JAVA
            throws IOException
#endif
        {
            Vector<VsqHandle> handles = buildHandleList();
            writer.writeLine( "[EventList]" );
            Vector<VsqEvent> temp = new Vector<VsqEvent>();
            for ( Iterator<VsqEvent> itr = Events.iterator(); itr.hasNext(); ) {
                temp.add( itr.next() );
            }
            Collections.sort( temp );
            int i = 0;
            while ( i < vec.size( temp ) ) {
                VsqEvent item = vec.get( temp, i );
                if ( !item.ID.Equals( VsqID.EOS ) ) {
                    String ids = "ID#" + PortUtil.formatDecimal( "0000", item.ID.value );
                    int clock = vec.get( temp, i ).Clock;
                    while ( i + 1 < vec.size( temp ) && clock == vec.get( temp, i + 1 ).Clock ) {
                        i++;
                        ids += ",ID#" + PortUtil.formatDecimal( "0000", vec.get( temp, i + 1 ).ID.value );
                    }
                    writer.writeLine( clock + "=" + ids );
                }
                i++;
            }
            writer.writeLine( eos + "=EOS" );
            return handles;
        }

        public Vector<VsqHandle> writeEventList( ITextWriter sw, int eos )
#if JAVA
            throws IOException
#endif
        {
            return writeEventListCor( sw, eos );
        }

        public Vector<VsqHandle> writeEventList( BufferedWriter stream_writer, int eos )
#if JAVA
            throws IOException
#endif
        {
            return writeEventListCor( new WrappedStreamWriter( stream_writer ), eos );
        }

        /// <summary>
        /// 何も無いVsqMetaTextを構築する。これは、Master Track用のMetaTextとしてのみ使用されるべき
        /// </summary>
        public VsqMetaText()
        {
        }

        /// <summary>
        /// 最初のトラック以外の一般のメタテキストを構築。(Masterが作られない)
        /// </summary>
        public VsqMetaText( String name, String singer )
#if JAVA
        {
#else
            :
#endif
 this( name, 0, singer, false )
#if JAVA
            ;
#else
        {
#endif
        }

        /// <summary>
        /// 最初のトラックのメタテキストを構築。(Masterが作られる)
        /// </summary>
        /// <param name="pre_measure"></param>
        public VsqMetaText( String name, String singer, int pre_measure )
#if JAVA
        {
#else
            :
#endif
 this( name, pre_measure, singer, true )
#if JAVA
            ;
#else
        {
#endif
        }

        private VsqMetaText( String name, int pre_measure, String singer, boolean is_first_track )
        {
            Common = new VsqCommon( name, 179, 181, 123, 1, 1 );
            PIT = new VsqBPList( "pit", 0, -8192, 8191 );
            PBS = new VsqBPList( "pbs", 2, 0, 24 );
            DYN = new VsqBPList( "dyn", 64, 0, 127 );
            BRE = new VsqBPList( "bre", 0, 0, 127 );
            BRI = new VsqBPList( "bri", 64, 0, 127 );
            CLE = new VsqBPList( "cle", 0, 0, 127 );
            reso1FreqBPList = new VsqBPList( "reso1freq", 64, 0, 127 );
            reso2FreqBPList = new VsqBPList( "reso2freq", 64, 0, 127 );
            reso3FreqBPList = new VsqBPList( "reso3freq", 64, 0, 127 );
            reso4FreqBPList = new VsqBPList( "reso4freq", 64, 0, 127 );
            reso1BWBPList = new VsqBPList( "reso1bw", 64, 0, 127 );
            reso2BWBPList = new VsqBPList( "reso2bw", 64, 0, 127 );
            reso3BWBPList = new VsqBPList( "reso3bw", 64, 0, 127 );
            reso4BWBPList = new VsqBPList( "reso4bw", 64, 0, 127 );
            reso1AmpBPList = new VsqBPList( "reso1amp", 64, 0, 127 );
            reso2AmpBPList = new VsqBPList( "reso2amp", 64, 0, 127 );
            reso3AmpBPList = new VsqBPList( "reso3amp", 64, 0, 127 );
            reso4AmpBPList = new VsqBPList( "reso4amp", 64, 0, 127 );
            harmonics = new VsqBPList( "harmonics", 64, 0, 127 );
            fx2depth = new VsqBPList( "fx2depth", 64, 0, 127 );
            GEN = new VsqBPList( "gen", 64, 0, 127 );
            POR = new VsqBPList( "por", 64, 0, 127 );
            OPE = new VsqBPList( "ope", 127, 0, 127 );
            if ( is_first_track ) {
                master = new VsqMaster( pre_measure );
            } else {
                master = null;
            }
            Events = new VsqEventList();
            VsqID id = new VsqID( 0 );
            id.type = VsqIDType.Singer;
            IconHandle ish = new IconHandle();
            ish.IconID = "$07010000";
            ish.IDS = singer;
            ish.Original = 0;
            ish.Caption = "";
            ish.setLength( 1 );
            ish.Language = 0;
            ish.Program = 0;
            id.IconHandle = ish;
            Events.add( new VsqEvent( 0, id ) );
        }

        public VsqMetaText( TextStream sr )
        {
            Vector<ValuePair<Integer, Integer>> t_event_list = new Vector<ValuePair<Integer, Integer>>();
            TreeMap<Integer, VsqID> __id = new TreeMap<Integer, VsqID>();
            TreeMap<Integer, VsqHandle> __handle = new TreeMap<Integer, VsqHandle>();
            PIT = new VsqBPList( "pit", 0, -8192, 8191 );
            PBS = new VsqBPList( "pbs", 2, 0, 24 );
            DYN = new VsqBPList( "dyn", 64, 0, 127 );
            BRE = new VsqBPList( "bre", 0, 0, 127 );
            BRI = new VsqBPList( "bri", 64, 0, 127 );
            CLE = new VsqBPList( "cle", 0, 0, 127 );
            reso1FreqBPList = new VsqBPList( "reso1freq", 64, 0, 127 );
            reso2FreqBPList = new VsqBPList( "reso2freq", 64, 0, 127 );
            reso3FreqBPList = new VsqBPList( "reso3freq", 64, 0, 127 );
            reso4FreqBPList = new VsqBPList( "reso4freq", 64, 0, 127 );
            reso1BWBPList = new VsqBPList( "reso1bw", 64, 0, 127 );
            reso2BWBPList = new VsqBPList( "reso2bw", 64, 0, 127 );
            reso3BWBPList = new VsqBPList( "reso3bw", 64, 0, 127 );
            reso4BWBPList = new VsqBPList( "reso4bw", 64, 0, 127 );
            reso1AmpBPList = new VsqBPList( "reso1amp", 64, 0, 127 );
            reso2AmpBPList = new VsqBPList( "reso2amp", 64, 0, 127 );
            reso3AmpBPList = new VsqBPList( "reso3amp", 64, 0, 127 );
            reso4AmpBPList = new VsqBPList( "reso4amp", 64, 0, 127 );
            harmonics = new VsqBPList( "harmonics", 64, 0, 127 );
            fx2depth = new VsqBPList( "fx2depth", 64, 0, 127 );
            GEN = new VsqBPList( "gen", 64, 0, 127 );
            POR = new VsqBPList( "por", 64, 0, 127 );
            OPE = new VsqBPList( "ope", 127, 0, 127 );

            ByRef<String> last_line = new ByRef<String>( sr.readLine() );
            while ( true ) {
                #region "TextMemoryStreamから順次読込み"
                if ( str.length( last_line.value ) == 0 ) {
                    break;
                }
                if ( str.compare( last_line.value, "[Common]" ) ) {
                    Common = new VsqCommon( sr, last_line );
                } else if ( str.compare( last_line.value, "[Master]" ) ) {
                    master = new VsqMaster( sr, last_line );
                } else if ( str.compare( last_line.value, "[Mixer]" ) ) {
                    mixer = new VsqMixer( sr, last_line );
                } else if ( str.compare( last_line.value, "[EventList]" ) ) {
                    last_line.value = sr.readLine();
                    while ( !str.startsWith( last_line.value, "[" ) ) {
                        String[] spl2 = PortUtil.splitString( last_line.value, new char[] { '=' } );
                        int clock = str.toi( spl2[0] );
                        int id_number = -1;
                        if ( !str.compare( spl2[1], "EOS" ) ) {
                            String[] ids = PortUtil.splitString( spl2[1], ',' );
                            for ( int i = 0; i < ids.Length; i++ ) {
                                String[] spl3 = PortUtil.splitString( ids[i], new char[] { '#' } );
                                id_number = str.toi( spl3[1] );
                                vec.add( t_event_list, new ValuePair<Integer, Integer>( clock, id_number ) );
                            }
                        } else {
                            vec.add( t_event_list, new ValuePair<Integer, Integer>( clock, -1 ) );
                        }
                        if ( !sr.ready() ) {
                            break;
                        } else {
                            last_line.value = sr.readLine();
                        }
                    }
                } else if ( str.compare( last_line.value, "[PitchBendBPList]" ) ) {
                    last_line.value = PIT.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[PitchBendSensBPList]" ) ) {
                    last_line.value = PBS.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[DynamicsBPList]" ) ) {
                    last_line.value = DYN.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[EpRResidualBPList]" ) ) {
                    last_line.value = BRE.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[EpRESlopeBPList]" ) ) {
                    last_line.value = BRI.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[EpRESlopeDepthBPList]" ) ) {
                    last_line.value = CLE.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[EpRSineBPList]" ) ) {
                    last_line.value = harmonics.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[VibTremDepthBPList]" ) ) {
                    last_line.value = fx2depth.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso1FreqBPList]" ) ) {
                    last_line.value = reso1FreqBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso2FreqBPList]" ) ) {
                    last_line.value = reso2FreqBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso3FreqBPList]" ) ) {
                    last_line.value = reso3FreqBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso4FreqBPList]" ) ) {
                    last_line.value = reso4FreqBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso1BWBPList]" ) ) {
                    last_line.value = reso1BWBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso2BWBPList]" ) ) {
                    last_line.value = reso2BWBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso3BWBPList]" ) ) {
                    last_line.value = reso3BWBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso4BWBPList]" ) ) {
                    last_line.value = reso4BWBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso1AmpBPList]" ) ) {
                    last_line.value = reso1AmpBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso2AmpBPList]" ) ) {
                    last_line.value = reso2AmpBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso3AmpBPList]" ) ) {
                    last_line.value = reso3AmpBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[Reso4AmpBPList]" ) ) {
                    last_line.value = reso4AmpBPList.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[GenderFactorBPList]" ) ) {
                    last_line.value = GEN.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[PortamentoTimingBPList]" ) ) {
                    last_line.value = POR.appendFromText( sr );
                } else if ( str.compare( last_line.value, "[OpeningBPList]" ) ) {
                    last_line.value = OPE.appendFromText( sr );
                } else {
                    String buffer = last_line.value;
                    buffer = buffer.Replace( "[", "" );
                    buffer = buffer.Replace( "]", "" );
#if DEBUG
                    sout.println( "VsqMetaText#.ctor; buffer=" + buffer );
#endif
                    String[] spl = PortUtil.splitString( buffer, new char[] { '#' } );
                    int index = str.toi( spl[1] );
                    if ( str.startsWith( last_line.value, "[ID#" ) ) {
                        __id.put( index, new VsqID( sr, index, last_line ) );
                    } else if ( str.startsWith( last_line.value, "[h#" ) ) {
                        __handle.put( index, new VsqHandle( sr, index, last_line ) );
                    }
                }
                #endregion

                if ( !sr.ready() ) {
                    break;
                }
            }

            // まずhandleをidに埋め込み
            int c = __id.size();
            for ( int i = 0; i < c; i++ ) {
                VsqID id = __id.get( i );
                if ( __handle.containsKey( id.IconHandle_index ) ) {
                    if ( id.type == VsqIDType.Singer ) {
                        id.IconHandle = __handle.get( id.IconHandle_index ).castToIconHandle();
                    } else if ( id.type == VsqIDType.Aicon ) {
                        id.IconDynamicsHandle = __handle.get( id.IconHandle_index ).castToIconDynamicsHandle();
                    }
                }
                if ( __handle.containsKey( id.LyricHandle_index ) ) {
                    id.LyricHandle = __handle.get( id.LyricHandle_index ).castToLyricHandle();
                }
                if ( __handle.containsKey( id.VibratoHandle_index ) ) {
                    id.VibratoHandle = __handle.get( id.VibratoHandle_index ).castToVibratoHandle();
                }
                if ( __handle.containsKey( id.NoteHeadHandle_index ) ) {
                    id.NoteHeadHandle = __handle.get( id.NoteHeadHandle_index ).castToNoteHeadHandle();
                }
            }

            // idをeventListに埋め込み
            Events = new VsqEventList();
            int count = 0;
            for ( int i = 0; i < vec.size( t_event_list ); i++ ) {
                ValuePair<Integer, Integer> item = vec.get( t_event_list, i );
                int clock = item.getKey();
                int id_number = item.getValue();
                if ( __id.containsKey( id_number ) ) {
                    count++;
                    Events.add( new VsqEvent( clock, (VsqID)__id.get( id_number ).clone() ), count );
                }
            }
            Events.sort();

            if ( Common == null ) {
                Common = new VsqCommon();
            }
        }
    }

#if !JAVA
}
#endif
