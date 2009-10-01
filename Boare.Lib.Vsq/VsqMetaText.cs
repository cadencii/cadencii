/*
* VsqMetaText/VsqMetaText.cs
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
using System.Drawing;
using System.IO;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;
    using Integer = System.Int32;

    /// <summary>
    /// vsqのメタテキストの中身を処理するためのクラス
    /// </summary>
    [Serializable]
    public class VsqMetaText : ICloneable {
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

        public object Clone() {
            VsqMetaText res = new VsqMetaText();
            if ( Common != null ) {
                res.Common = (VsqCommon)Common.Clone();
            }
            if ( master != null ) {
                res.master = (VsqMaster)master.Clone();
            }
            if ( mixer != null ) {
                res.mixer = (VsqMixer)mixer.Clone();
            }
            if ( Events != null ) {
                res.Events = new VsqEventList();
                for ( Iterator itr = Events.iterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    res.Events.add( (VsqEvent)item.clone(), item.InternalID );
                }
            }
            if ( PIT != null ) {
                res.PIT = (VsqBPList)PIT.Clone();
            }
            if ( PBS != null ) {
                res.PBS = (VsqBPList)PBS.Clone();
            }
            if ( DYN != null ) {
                res.DYN = (VsqBPList)DYN.Clone();
            }
            if ( BRE != null ) {
                res.BRE = (VsqBPList)BRE.Clone();
            }
            if ( BRI != null ) {
                res.BRI = (VsqBPList)BRI.Clone();
            }
            if ( CLE != null ) {
                res.CLE = (VsqBPList)CLE.Clone();
            }
            if ( reso1FreqBPList != null ) {
                res.reso1FreqBPList = (VsqBPList)reso1FreqBPList.Clone();
            }
            if ( reso2FreqBPList != null ) {
                res.reso2FreqBPList = (VsqBPList)reso2FreqBPList.Clone();
            }
            if ( reso3FreqBPList != null ) {
                res.reso3FreqBPList = (VsqBPList)reso3FreqBPList.Clone();
            }
            if ( reso4FreqBPList != null ) {
                res.reso4FreqBPList = (VsqBPList)reso4FreqBPList.Clone();
            }
            if ( reso1BWBPList != null ) {
                res.reso1BWBPList = (VsqBPList)reso1BWBPList.Clone();
            }
            if ( reso2BWBPList != null ) {
                res.reso2BWBPList = (VsqBPList)reso2BWBPList.Clone();
            }
            if ( reso3BWBPList != null ) {
                res.reso3BWBPList = (VsqBPList)reso3BWBPList.Clone();
            }
            if ( reso4BWBPList != null ) {
                res.reso4BWBPList = (VsqBPList)reso4BWBPList.Clone();
            }
            if ( reso1AmpBPList != null ) {
                res.reso1AmpBPList = (VsqBPList)reso1AmpBPList.Clone();
            }
            if ( reso2AmpBPList != null ) {
                res.reso2AmpBPList = (VsqBPList)reso2AmpBPList.Clone();
            }
            if ( reso3AmpBPList != null ) {
                res.reso3AmpBPList = (VsqBPList)reso3AmpBPList.Clone();
            }
            if ( reso4AmpBPList != null ) {
                res.reso4AmpBPList = (VsqBPList)reso4AmpBPList.Clone();
            }
            if ( harmonics != null ) {
                res.harmonics = (VsqBPList)harmonics.Clone();
            }
            if ( fx2depth != null ) {
                res.fx2depth = (VsqBPList)fx2depth.Clone();
            }
            if ( GEN != null ) {
                res.GEN = (VsqBPList)GEN.Clone();
            }
            if ( POR != null ) {
                res.POR = (VsqBPList)POR.Clone();
            }
            if ( OPE != null ) {
                res.OPE = (VsqBPList)OPE.Clone();
            }
            return res;
        }

        public VsqEventList getEventList() {
            return Events;
        }

        internal VsqBPList getElement( String curve ) {
            switch ( curve.Trim().ToLower() ) {
                case "bre":
                    return this.BRE;
                case "bri":
                    return this.BRI;
                case "cle":
                    return this.CLE;
                case "dyn":
                    return this.DYN;
                case "gen":
                    return this.GEN;
                case "ope":
                    return this.OPE;
                case "pbs":
                    return this.PBS;
                case "pit":
                    return this.PIT;
                case "por":
                    return this.POR;
                case "harmonics":
                    return this.harmonics;
                case "fx2depth":
                    return this.fx2depth;
                case "reso1amp":
                    return this.reso1AmpBPList;
                case "reso1bw":
                    return this.reso1BWBPList;
                case "reso1freq":
                    return this.reso1FreqBPList;
                case "reso2amp":
                    return this.reso2AmpBPList;
                case "reso2bw":
                    return this.reso2BWBPList;
                case "reso2freq":
                    return this.reso2FreqBPList;
                case "reso3amp":
                    return this.reso3AmpBPList;
                case "reso3bw":
                    return this.reso3BWBPList;
                case "reso3freq":
                    return this.reso3FreqBPList;
                case "reso4amp":
                    return this.reso4AmpBPList;
                case "reso4bw":
                    return this.reso4BWBPList;
                case "reso4freq":
                    return this.reso4FreqBPList;
                default:
                    return null;
            }
        }

        internal void setElement( String curve, VsqBPList value ) {
            switch ( curve.Trim().ToLower() ) {
                case "bre":
                    this.BRE = value;
                    break;
                case "bri":
                    this.BRI = value;
                    break;
                case "cle":
                    this.CLE = value;
                    break;
                case "dyn":
                    this.DYN = value;
                    break;
                case "gen":
                    this.GEN = value;
                    break;
                case "ope":
                    this.OPE = value;
                    break;
                case "pbs":
                    this.PBS = value;
                    break;
                case "pit":
                    this.PIT = value;
                    break;
                case "por":
                    this.POR = value;
                    break;
                case "harmonics":
                    this.harmonics = value;
                    break;
                case "fx2depth":
                    this.fx2depth = value;
                    break;
                case "reso1amp":
                    this.reso1AmpBPList = value;
                    break;
                case "reso1bw":
                    this.reso1BWBPList = value;
                    break;
                case "reso1freq":
                    this.reso1FreqBPList = value;
                    break;
                case "reso2amp":
                    this.reso2AmpBPList = value;
                    break;
                case "reso2bw":
                    this.reso2BWBPList = value;
                    break;
                case "reso2freq":
                    this.reso2FreqBPList = value;
                    break;
                case "reso3amp":
                    this.reso3AmpBPList = value;
                    break;
                case "reso3bw":
                    this.reso3BWBPList = value;
                    break;
                case "reso3freq":
                    this.reso3FreqBPList = value;
                    break;
                case "reso4amp":
                    this.reso4AmpBPList = value;
                    break;
                case "reso4bw":
                    this.reso4BWBPList = value;
                    break;
                case "reso4freq":
                    this.reso4FreqBPList = value;
                    break;
                default:
#if DEBUG
                    Console.WriteLine( "VsqMetaText#setElement; warning:unknown curve; curve=" + curve );
#endif
                    break;
            }
        }

        /// <summary>
        /// Editor画面上で上からindex番目のカーブを表すBPListを求めます
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VsqBPList getCurve( int index ) {
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
        public static String getCurveName( int index ) {
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
        public String getSinger() {
            for ( Iterator itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.ID.type == VsqIDType.Singer ) {
                    return item.ID.IconHandle.IDS;
                }
            }
            return "";
        }

        public void setSinger( String value ) {
            for ( Iterator itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.ID.type == VsqIDType.Singer ) {
                    item.ID.IconHandle.IDS = value;
                    break;
                }
            }
        }

        /// <summary>
        /// EOSイベントが記録されているクロックを取得します。
        /// </summary>
        /// <returns></returns>
        public int getIndexOfEOS() {
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
        private Vector<VsqHandle> buildHandleList() {
            Vector<VsqHandle> handle = new Vector<VsqHandle>();
            int current_id = -1;
            int current_handle = -1;
            for ( Iterator itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                current_id++;
                item.ID.value = current_id;
                // IconHandle
                if ( item.ID.IconHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.IconHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.add( handle_item );
                    item.ID.IconHandle_index = current_handle;
                }
                // LyricHandle
                if ( item.ID.LyricHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.LyricHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
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
            }
            return handle;
        }

        /* /// <summary>
        /// このインスタンスから、IDとHandleのリストを構築します
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handle"></param>
        public void buildIDAndHandleList( out Vector<VsqID> id, out Vector<VsqHandle> handle ) {
            id = new Vector<VsqID>();
            handle = new Vector<VsqHandle>();
            int current_id = -1;
            int current_handle = -1;
            Vector<VsqEvent> events = new Vector<VsqEvent>();
            for ( Iterator itr = Events.iterator(); itr.hasNext(); ) {
                events.add( (VsqEvent)itr.next() );
            }
            Collections.sort( events );
            for ( int i = 0; i < events.size(); i++ ) {
                VsqEvent item = events.get( i );
                VsqID id_item = (VsqID)item.ID.clone();
                current_id++;
                item.ID.value = current_id;
                id_item.value = current_id;
                // IconHandle
                if ( item.ID.IconHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.IconHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.add( handle_item );
                    id_item.IconHandle_index = current_handle;
                }
                // LyricHandle
                if ( item.ID.LyricHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.LyricHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.add( handle_item );
                    id_item.LyricHandle_index = current_handle;
                }
                // VibratoHandle
                if ( item.ID.VibratoHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.VibratoHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.add( handle_item );
                    id_item.VibratoHandle_index = current_handle;
                }
                // NoteHeadHandle
                if ( item.ID.NoteHeadHandle != null ) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.NoteHeadHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.add( handle_item );
                    id_item.NoteHeadHandle_index = current_handle;
                }
                id.add( id_item );
            }
        }*/

        /// <summary>
        /// このインスタンスの内容を指定されたファイルに出力します。
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="encode"></param>
        public void print( TextMemoryStream sw, boolean encode, int eos, int start ) {
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
            for ( Iterator itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                item.write( sw );
            }
            for ( int i = 0; i < handle.size(); i++ ) {
                handle.get( i ).write( sw, encode );
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

        private Vector<VsqHandle> writeEventListCor( ITextWriter writer, int eos ) {
            Vector<VsqHandle> handles = buildHandleList();
            writer.writeLine( "[EventList]" );
            Vector<VsqEvent> temp = new Vector<VsqEvent>();
            for ( Iterator itr = Events.iterator(); itr.hasNext(); ) {
                temp.add( (VsqEvent)itr.next() );
            }
            Collections.sort( temp );
            int i = 0;
            while ( i < temp.size() ) {
                VsqEvent item = temp.get( i );
                if ( !item.ID.Equals( VsqID.EOS ) ) {
                    String ids = "ID#" + item.ID.value.ToString( "0000" );
                    int clock = temp.get( i ).Clock;
                    while ( i + 1 < temp.size() && clock == temp.get( i + 1 ).Clock ) {
                        i++;
                        ids += ",ID#" + temp.get( i + 1 ).ID.value.ToString( "0000" );
                    }
                    writer.writeLine( clock + "=" + ids );
                }
                i++;
            }
            writer.writeLine( eos + "=EOS" );
            return handles;
        }

        public Vector<VsqHandle> writeEventList( TextMemoryStream sw, int eos ) {
            return writeEventListCor( sw, eos );
        }

        public Vector<VsqHandle> writeEventList( StreamWriter stream_writer, int eos ) {
            return writeEventListCor( new WrappedStreamWriter( stream_writer ), eos );
        }

        /// <summary>
        /// 何も無いVsqMetaTextを構築する。これは、Master Track用のMetaTextとしてのみ使用されるべき
        /// </summary>
        public VsqMetaText() {
        }

        /// <summary>
        /// 最初のトラック以外の一般のメタテキストを構築。(Masterが作られない)
        /// </summary>
        public VsqMetaText( String name, String singer )
            : this( name, 0, singer, false ) {
        }

        /// <summary>
        /// 最初のトラックのメタテキストを構築。(Masterが作られる)
        /// </summary>
        /// <param name="pre_measure"></param>
        public VsqMetaText( String name, String singer, int pre_measure )
            : this( name, pre_measure, singer, true ) {
        }

        private VsqMetaText( String name, int pre_measure, String singer, boolean is_first_track ) {
            Common = new VsqCommon( name, Color.FromArgb( 179, 181, 123 ), 1, 1 );
            PIT = new VsqBPList( 0, -8192, 8191 );
            //PIT.add( 0, PIT.getDefault() );

            PBS = new VsqBPList( 2, 0, 24 );
            //PBS.add( 0, PBS.getDefault() );

            DYN = new VsqBPList( 64, 0, 127 );
            //DYN.add( 0, DYN.getDefault() );

            BRE = new VsqBPList( 0, 0, 127 );
            //BRE.add( 0, BRE.getDefault() );

            BRI = new VsqBPList( 64, 0, 127 );
            //BRI.add( 0, BRI.getDefault() );

            CLE = new VsqBPList( 0, 0, 127 );
            //CLE.add( 0, CLE.getDefault() );

            reso1FreqBPList = new VsqBPList( 64, 0, 127 );
            //reso1FreqBPList.add( 0, reso1FreqBPList.getDefault() );

            reso2FreqBPList = new VsqBPList( 64, 0, 127 );
            //reso2FreqBPList.add( 0, reso2FreqBPList.getDefault() );

            reso3FreqBPList = new VsqBPList( 64, 0, 127 );
            //reso3FreqBPList.add( 0, reso3FreqBPList.getDefault() );

            reso4FreqBPList = new VsqBPList( 64, 0, 127 );
            //reso4FreqBPList.add( 0, reso4FreqBPList.getDefault() );

            reso1BWBPList = new VsqBPList( 64, 0, 127 );
            //reso1BWBPList.add( 0, reso1BWBPList.getDefault() );

            reso2BWBPList = new VsqBPList( 64, 0, 127 );
            //reso2BWBPList.add( 0, reso2BWBPList.getDefault() );

            reso3BWBPList = new VsqBPList( 64, 0, 127 );
            //reso3BWBPList.add( 0, reso3BWBPList.getDefault() );

            reso4BWBPList = new VsqBPList( 64, 0, 127 );
            //reso4BWBPList.add( 0, reso4BWBPList.getDefault() );

            reso1AmpBPList = new VsqBPList( 64, 0, 127 );
            //reso1AmpBPList.add( 0, reso1AmpBPList.getDefault() );

            reso2AmpBPList = new VsqBPList( 64, 0, 127 );
            //reso2AmpBPList.add( 0, reso2AmpBPList.getDefault() );

            reso3AmpBPList = new VsqBPList( 64, 0, 127 );
            //reso3AmpBPList.add( 0, reso3AmpBPList.getDefault() );

            reso4AmpBPList = new VsqBPList( 64, 0, 127 );
            //reso4AmpBPList.add( 0, reso4AmpBPList.getDefault() );

            harmonics = new VsqBPList( 64, 0, 127 );
            //harmonics.add( 0, harmonics.getDefault() );

            fx2depth = new VsqBPList( 64, 0, 127 );

            GEN = new VsqBPList( 64, 0, 127 );
            //GEN.add( 0, GEN.getDefault() );

            POR = new VsqBPList( 64, 0, 127 );
            //POR.add( 0, POR.getDefault() );

            OPE = new VsqBPList( 127, 0, 127 );
            //OPE.add( 0, OPE.getDefault() );

            if ( is_first_track ) {
                master = new VsqMaster( pre_measure );
            } else {
                master = null;
            }
            Events = new VsqEventList();
            VsqID id = new VsqID( 0 );
            id.type = VsqIDType.Singer;
            id.IconHandle = new IconHandle();
            id.IconHandle.IconID = "$07010000";
            id.IconHandle.IDS = singer;
            id.IconHandle.Original = 0;
            id.IconHandle.Caption = "";
            id.IconHandle.Length = 1;
            id.IconHandle.Language = 0;
            id.IconHandle.Program = 0;
            Events.add( new VsqEvent( 0, id ) );
        }

        public VsqMetaText( TextMemoryStream sr ) {
            Vector<KeyValuePair<Integer, Integer>> t_event_list = new Vector<KeyValuePair<Integer, Integer>>();
            TreeMap<Integer, VsqID> __id = new TreeMap<Integer, VsqID>();
            TreeMap<Integer, VsqHandle> __handle = new TreeMap<Integer, VsqHandle>();
            PIT = new VsqBPList( 0, -8192, 8191 );
            PBS = new VsqBPList( 2, 0, 24 );
            DYN = new VsqBPList( 64, 0, 127 );
            BRE = new VsqBPList( 0, 0, 127 );
            BRI = new VsqBPList( 64, 0, 127 );
            CLE = new VsqBPList( 0, 0, 127 );
            reso1FreqBPList = new VsqBPList( 64, 0, 127 );
            reso2FreqBPList = new VsqBPList( 64, 0, 127 );
            reso3FreqBPList = new VsqBPList( 64, 0, 127 );
            reso4FreqBPList = new VsqBPList( 64, 0, 127 );
            reso1BWBPList = new VsqBPList( 64, 0, 127 );
            reso2BWBPList = new VsqBPList( 64, 0, 127 );
            reso3BWBPList = new VsqBPList( 64, 0, 127 );
            reso4BWBPList = new VsqBPList( 64, 0, 127 );
            reso1AmpBPList = new VsqBPList( 64, 0, 127 );
            reso2AmpBPList = new VsqBPList( 64, 0, 127 );
            reso3AmpBPList = new VsqBPList( 64, 0, 127 );
            reso4AmpBPList = new VsqBPList( 64, 0, 127 );
            harmonics = new VsqBPList( 64, 0, 127 );
            fx2depth = new VsqBPList( 64, 0, 127 );
            GEN = new VsqBPList( 64, 0, 127 );
            POR = new VsqBPList( 64, 0, 127 );
            OPE = new VsqBPList( 127, 0, 127 );

            String last_line = sr.readLine();
            while ( true ) {
                #region "TextMemoryStreamから順次読込み"
                if ( last_line.Length == 0 ) {
                    break;
                }
                switch ( last_line ) {
                    case "[Common]":
                        Common = new VsqCommon( sr, ref last_line );
                        break;
                    case "[Master]":
                        master = new VsqMaster( sr, ref last_line );
                        break;
                    case "[Mixer]":
                        mixer = new VsqMixer( sr, ref last_line );
                        break;
                    case "[EventList]":
                        last_line = sr.readLine();
                        while ( !last_line.StartsWith( "[" ) ) {
                            String[] spl2 = PortUtil.splitString( last_line, new char[] { '=' } );
                            int clock = PortUtil.parseInt( spl2[0] );
                            int id_number = -1;
                            if ( spl2[1] != "EOS" ) {
                                String[] ids = PortUtil.splitString( spl2[1], ',' );
                                for ( int i = 0; i < ids.Length; i++ ) {
                                    String[] spl3 = PortUtil.splitString( ids[i], new char[] { '#' } );
                                    id_number = PortUtil.parseInt( spl3[1] );
                                    t_event_list.add( new KeyValuePair<int, int>( clock, id_number ) );
                                }
                            } else {
                                t_event_list.add( new KeyValuePair<int, int>( clock, -1 ) );
                            }
                            if ( sr.peek() < 0 ) {
                                break;
                            } else {
                                last_line = sr.readLine();
                            }
                        }
                        break;
                    case "[PitchBendBPList]":
                        last_line = PIT.appendFromText( sr );
                        break;
                    case "[PitchBendSensBPList]":
                        last_line = PBS.appendFromText( sr );
                        break;
                    case "[DynamicsBPList]":
                        last_line = DYN.appendFromText( sr );
                        break;
                    case "[EpRResidualBPList]":
                        last_line = BRE.appendFromText( sr );
                        break;
                    case "[EpRESlopeBPList]":
                        last_line = BRI.appendFromText( sr );
                        break;
                    case "[EpRESlopeDepthBPList]":
                        last_line = CLE.appendFromText( sr );
                        break;
                    case "[EpRSineBPList]":
                        last_line = harmonics.appendFromText( sr );
                        break;
                    case "[VibTremDepthBPList]":
                        last_line = fx2depth.appendFromText( sr );
                        break;
                    case "[Reso1FreqBPList]":
                        last_line = reso1FreqBPList.appendFromText( sr );
                        break;
                    case "[Reso2FreqBPList]":
                        last_line = reso2FreqBPList.appendFromText( sr );
                        break;
                    case "[Reso3FreqBPList]":
                        last_line = reso3FreqBPList.appendFromText( sr );
                        break;
                    case "[Reso4FreqBPList]":
                        last_line = reso4FreqBPList.appendFromText( sr );
                        break;
                    case "[Reso1BWBPList]":
                        last_line = reso1BWBPList.appendFromText( sr );
                        break;
                    case "[Reso2BWBPList]":
                        last_line = reso2BWBPList.appendFromText( sr );
                        break;
                    case "[Reso3BWBPList]":
                        last_line = reso3BWBPList.appendFromText( sr );
                        break;
                    case "[Reso4BWBPList]":
                        last_line = reso4BWBPList.appendFromText( sr );
                        break;
                    case "[Reso1AmpBPList]":
                        last_line = reso1AmpBPList.appendFromText( sr );
                        break;
                    case "[Reso2AmpBPList]":
                        last_line = reso2AmpBPList.appendFromText( sr );
                        break;
                    case "[Reso3AmpBPList]":
                        last_line = reso3AmpBPList.appendFromText( sr );
                        break;
                    case "[Reso4AmpBPList]":
                        last_line = reso4AmpBPList.appendFromText( sr );
                        break;
                    case "[GenderFactorBPList]":
                        last_line = GEN.appendFromText( sr );
                        break;
                    case "[PortamentoTimingBPList]":
                        last_line = POR.appendFromText( sr );
                        break;
                    case "[OpeningBPList]":
                        last_line = OPE.appendFromText( sr );
                        break;
                    default:
                        String buffer = last_line;
                        buffer = buffer.Replace( "[", "" );
                        buffer = buffer.Replace( "]", "" );
                        String[] spl = PortUtil.splitString( buffer, new char[] { '#' } );
                        int index = PortUtil.parseInt( spl[1] );
                        if ( last_line.StartsWith( "[ID#" ) ) {
                            __id.put( index, new VsqID( sr, index, ref last_line ) );
                        } else if ( last_line.StartsWith( "[h#" ) ) {
                            __handle.put( index, new VsqHandle( sr, index, ref last_line ) );
                        }
                        break;
                #endregion
                }

                if ( sr.peek() < 0 ) {
                    break;
                }
            }

            // まずhandleをidに埋め込み
            for ( int i = 0; i < __id.size(); i++ ) {
                if ( __handle.containsKey( __id.get( i ).IconHandle_index ) ) {
                    __id.get( i ).IconHandle = __handle.get( __id.get( i ).IconHandle_index ).castToIconHandle();
                }
                if ( __handle.containsKey( __id.get( i ).LyricHandle_index ) ) {
                    __id.get( i ).LyricHandle = __handle.get( __id.get( i ).LyricHandle_index ).castToLyricHandle();
                }
                if ( __handle.containsKey( __id.get( i ).VibratoHandle_index ) ) {
                    __id.get( i ).VibratoHandle = __handle.get( __id.get( i ).VibratoHandle_index ).castToVibratoHandle();
                }
                if ( __handle.containsKey( __id.get( i ).NoteHeadHandle_index ) ) {
                    __id.get( i ).NoteHeadHandle = __handle.get( __id.get( i ).NoteHeadHandle_index ).castToNoteHeadHandle();
                }
            }

            // idをeventListに埋め込み
            Events = new VsqEventList();
            for ( int i = 0; i < t_event_list.size(); i++ ) {
                int clock = t_event_list.get( i ).Key;
                int id_number = t_event_list.get( i ).Value;
                if ( __id.containsKey( id_number ) ) {
                    Events.add( new VsqEvent( clock, (VsqID)__id.get( id_number ).clone() ) );
                }
            }

            if ( Common == null ) {
                Common = new VsqCommon();
            }
        }

        public static boolean test( String fpath ) {
            /*VsqMetaText metaText;
            using ( TextMemoryStream sr = new TextMemoryStream( fpath, Encoding.Unicode ) ) {
                metaText = new VsqMetaText( sr );
            }*/
            String result = "test.txt";

            StreamReader honmono = new StreamReader( fpath );
            TextMemoryStream copy = new TextMemoryStream();
            //metaText.print( copy, true, 1000, 100 );
            copy.rewind();
            while ( honmono.Peek() >= 0 && copy.peek() >= 0 ) {
                String hon = honmono.ReadLine();
                String cop = copy.readLine();
                if ( hon != cop ) {
                    Console.WriteLine( "honmono,copy=" + hon + "," + cop );
                    honmono.Close();
                    copy.close();
                    return false;
                }
            }
            honmono.Close();
            copy.close();

            return true;
        }
    }

}
