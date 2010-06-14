/*
 * VsqMetaText.cs
 * Copyright (C) 2008-2010 kbinani
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
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.VsqMetaText == undefined ){

    /**
     * vsqのメタテキストの中身を処理するためのクラス
     */
    org.kbinani.vsq.VsqMetaText = function(){
        /**
         * [VsqCommon]
         */
        this.Common = null;
        /**
         * [VsqMaster]
         */
        this.master = null;
        /**
         * [VsqMixer]
         */
        this.mixer = null;
        /**
         * [VsqEventList]
         */
        this.Events = null;
        /**
         *  PIT。ピッチベンド(pitchBendBPList)。default=0
         */
        this.PIT = null;
        /**
         *  PBS。ピッチベンドセンシティビティ(pitchBendSensBPList)。dfault=2
         */
        this.PBS = null;
        /**
         *  DYN。ダイナミクス(dynamicsBPList)。default=64
         */
        this.DYN = null;
        /**
         *  BRE。ブレシネス(epRResidualBPList)。default=0
         */
        this.BRE = null;
        /**
         *  BRI。ブライトネス(epRESlopeBPList)。default=64
         */
        this.BRI = null;
        /**
         *  CLE。クリアネス(epRESlopeDepthBPList)。default=0
         */
        this.CLE = null;
        this.reso1FreqBPList = null;
        this.reso2FreqBPList = null;
        this.reso3FreqBPList = null;
        this.reso4FreqBPList = null;
        this.reso1BWBPList = null;
        this.reso2BWBPList = null;
        this.reso3BWBPList = null;
        this.reso4BWBPList = null;
        this.reso1AmpBPList = null;
        this.reso2AmpBPList = null;
        this.reso3AmpBPList = null;
        this.reso4AmpBPList = null;
        /**
         *  Harmonics。(EpRSineBPList)default = 64
         */
        this.harmonics = null;
        /**
         *  Effect2 Depth。
         */
        this.fx2depth = null;
        /**
         *  GEN。ジェンダーファクター(genderFactorBPList)。default=64
         */
        this.GEN = null;
        /**
         *  POR。ポルタメントタイミング(portamentoTimingBPList)。default=64
         */
        this.POR = null;
        /**
         *  OPE。オープニング(openingBPList)。default=127
         */
        this.OPE = null;
        if( arguments.length == 0 ){
            this._init_0();
        }else if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }else if( arguments.length == 2 ){
            this._init_2( arguments[0], arguments[1] );
        }else if( arguments.length == 3 ){
            this._init_3( arguments[0], arguments[1], arguments[2] );
        }
    };

    org.kbinani.vsq.VsqMetaText.prototype = {
        /**
         * @return [object]
         */
        clone : function() {
            var res = new org.kbinani.vsq.VsqMetaText();
            if ( this.Common != null ) {
                res.Common = this.Common.clone();
            }
            if ( this.master != null ) {
                res.master = this.master.clone();
            }
            if ( this.mixer != null ) {
                res.mixer = this.mixer.clone();
            }
            if ( this.Events != null ) {
                res.Events = new org.kbinani.vsq.VsqEventList();
                for ( var i = 0; i < this.Events.getCount(); i++ ) {
                    var item = this.Events.getElement( i );
                    res.Events.push( item.clone(), item.InternalID );
                }
            }
            if ( this.PIT != null ) {
                res.PIT = this.PIT.clone();
            }
            if ( this.PBS != null ) {
                res.PBS = this.PBS.clone();
            }
            if ( this.DYN != null ) {
                res.DYN = this.DYN.clone();
            }
            if ( this.BRE != null ) {
                res.BRE = this.BRE.clone();
            }
            if ( this.BRI != null ) {
                res.BRI = this.BRI.clone();
            }
            if ( this.CLE != null ) {
                res.CLE = this.CLE.clone();
            }
            if ( this.reso1FreqBPList != null ) {
                res.reso1FreqBPList = this.reso1FreqBPList.clone();
            }
            if ( this.reso2FreqBPList != null ) {
                res.reso2FreqBPList = this.reso2FreqBPList.clone();
            }
            if ( this.reso3FreqBPList != null ) {
                res.reso3FreqBPList = this.reso3FreqBPList.clone();
            }
            if ( this.reso4FreqBPList != null ) {
                res.reso4FreqBPList = this.reso4FreqBPList.clone();
            }
            if ( this.reso1BWBPList != null ) {
                res.reso1BWBPList = this.reso1BWBPList.clone();
            }
            if ( this.reso2BWBPList != null ) {
                res.reso2BWBPList = this.reso2BWBPList.clone();
            }
            if ( this.reso3BWBPList != null ) {
                res.reso3BWBPList = this.reso3BWBPList.clone();
            }
            if ( this.reso4BWBPList != null ) {
                res.reso4BWBPList = this.reso4BWBPList.clone();
            }
            if ( this.reso1AmpBPList != null ) {
                res.reso1AmpBPList = this.reso1AmpBPList.clone();
            }
            if ( this.reso2AmpBPList != null ) {
                res.reso2AmpBPList = this.reso2AmpBPList.clone();
            }
            if ( this.reso3AmpBPList != null ) {
                res.reso3AmpBPList = this.reso3AmpBPList.clone();
            }
            if ( this.reso4AmpBPList != null ) {
                res.reso4AmpBPList = this.reso4AmpBPList.clone();
            }
            if ( this.harmonics != null ) {
                res.harmonics = this.harmonics.clone();
            }
            if ( this.fx2depth != null ) {
                res.fx2depth = this.fx2depth.clone();
            }
            if ( this.GEN != null ) {
                res.GEN = this.GEN.clone();
            }
            if ( this.POR != null ) {
                res.POR = this.POR.clone();
            }
            if ( this.OPE != null ) {
                res.OPE = this.OPE.clone();
            }
            return res;
        },

        /**
         * @return [VsqEventList]
         */
        getEventList : function() {
            return this.Events;
        },

        /**
         * @param curve [string]
         * @return [VsqBPList]
         */
        getElement : function( curve ) {
            var search = curve.toLowerCase();
            if ( search == "bre" ) {
                return this.BRE;
            } else if ( search == "bri" ) {
                return this.BRI;
            } else if ( search == "cle" ) {
                return this.CLE;
            } else if ( search == "dyn" ) {
                return this.DYN;
            } else if ( search == "gen" ) {
                return this.GEN;
            } else if ( search == "ope" ) {
                return this.OPE;
            } else if ( search == "pbs" ) {
                return this.PBS;
            } else if ( search == "pit" ) {
                return this.PIT;
            } else if ( search == "por" ) {
                return this.POR;
            } else if ( search == "harmonics" ) {
                return this.harmonics;
            } else if ( search == "fx2depth" ) {
                return this.fx2depth;
            } else if ( search == "reso1amp" ) {
                return this.reso1AmpBPList;
            } else if ( search == "reso1bw" ) {
                return this.reso1BWBPList;
            } else if ( search == "reso1freq" ) {
                return this.reso1FreqBPList;
            } else if ( search == "reso2amp" ) {
                return this.reso2AmpBPList;
            } else if ( search == "reso2bw" ) {
                return this.reso2BWBPList;
            } else if ( search == "reso2freq" ) {
                return this.reso2FreqBPList;
            } else if ( search == "reso3amp" ) {
                return this.reso3AmpBPList;
            } else if ( search == "reso3bw" ) {
                return this.reso3BWBPList;
            } else if ( search == "reso3freq" ) {
                return this.reso3FreqBPList;
            } else if ( search == "reso4amp" ) {
                return this.reso4AmpBPList;
            } else if ( search == "reso4bw" ) {
                return this.reso4BWBPList;
            } else if ( search == "reso4freq" ) {
                return this.reso4FreqBPList;
            } else {
                return null;
            }
        },

        /**
         * @param curve [string]
         * @value [VsqBPList]
         * @return [void]
         */
        setElement : function( curve, value ) {
            var search = curve.toLower();
            if ( search == "bre" ) {
                this.BRE = value;
            } else if ( search == "bri" ) {
                this.BRI = value;
            } else if ( search == "cle" ) {
                this.CLE = value;
            } else if ( search == "dyn" ) {
                this.DYN = value;
            } else if ( search == "gen" ) {
                this.GEN = value;
            } else if ( search == "ope" ) {
                this.OPE = value;
            } else if ( search == "pbs" ) {
                this.PBS = value;
            } else if ( search == "pit" ) {
                this.PIT = value;
            } else if ( search == "por" ) {
                this.POR = value;
            } else if ( search == "harmonics" ) {
                this.harmonics = value;
            } else if ( search == "fx2depth" ) {
                this.fx2depth = value;
            } else if ( search == "reso1amp" ) {
                this.reso1AmpBPList = value;
            } else if ( search == "reso1bw" ) {
                this.reso1BWBPList = value;
            } else if ( search == "reso1freq" ) {
                this.reso1FreqBPList = value;
            } else if ( search == "reso2amp" ) {
                this.reso2AmpBPList = value;
            } else if ( search == "reso2bw" ) {
                this.reso2BWBPList = value;
            } else if ( search == "reso2freq" ) {
                this.reso2FreqBPList = value;
            } else if ( search == "reso3amp" ) {
                this.reso3AmpBPList = value;
            } else if ( search == "reso3bw" ) {
                this.reso3BWBPList = value;
            } else if ( search == "reso3freq" ) {
                this.reso3FreqBPList = value;
            } else if ( search == "reso4amp" ) {
                this.reso4AmpBPList = value;
            } else if ( search == "reso4bw" ) {
                this.reso4BWBPList = value;
            } else if ( search == "reso4freq" ) {
                this.reso4FreqBPList = value;
            }
        },

        /**
         * Editor画面上で上からindex番目のカーブを表すBPListを求めます
         *
         * @param index [int]
         * @return [VsqBPList]
         */
        getCurve : function( index ) {
            switch ( index ) {
                case 1:
                    return this.DYN;
                case 2:
                    return this.BRE;
                case 3:
                    return this.BRI;
                case 4:
                    return this.CLE;
                case 5:
                    return this.OPE;
                case 6:
                    return this.GEN;
                case 7:
                    return this.POR;
                case 8:
                    return this.PIT;
                case 9:
                    return this.PBS;
                default:
                    return null;
            }
        },

        /**
         * Singerプロパティに指定されている
         *
         * @return [string]
         */
        getSinger : function() {
            for ( var itr = new org.kbinani.ArrayIterator( this.Events ); itr.hasNext(); ) {
                var item = itr.next();
                if ( item.ID.type == org.kbinani.vsq.VsqIDType.Singer ) {
                    return item.ID.IconHandle.IDS;
                }
            }
            return "";
        },

        /**
         * @param value [string]
         * @return [void]
         */
        setSinger : function( value ) {
            for ( var itr = new org.kbinani.ArrayIterator( this.Events ); itr.hasNext(); ) {
                var item = itr.next();
                if ( item.ID.type == org.kbinani.vsq.VsqIDType.Singer ) {
                    item.ID.IconHandle.IDS = value;
                    break;
                }
            }
        },

        /**
         * EOSイベントが記録されているクロックを取得します。
         *
         * @return [int]
         */
        getIndexOfEOS : function() {
            var result;
            if ( this.Events.getCount() > 0 ) {
                var ilast = this.Events.getCount() - 1;
                result = this.Events.getElement( ilast ).Clock;
            } else {
                result = -1;
            }
            return result;
        },

        /**
         * このインスタンスから、Handleのリストを作成すると同時に、Eventsに登録されているVsqEventのvalue値および各ハンドルのvalue値を更新します
         *
         * @return [Vector<VsqHandle>]
         */
        _buildHandleList : function() {
            var handle = new Array();
            var current_id = -1;
            var current_handle = -1;
            var add_quotation_mark = true;
            var is_vocalo1 = this.Common.Version.indexOf( "DSB2" ) === 0;
            var is_vocalo2 = this.Common.Version.indexOf( "DSB3" ) === 0;
            for ( var itr = new org.kbinani.ArrayIterator( this.Events ); itr.hasNext(); ) {
                var item = itr.next();
                current_id++;
                item.ID.value = current_id;
                // IconHandle
                if ( item.ID.IconHandle != null ) {
                    // TODO: VsqMetaText#_buildHandleList; ここの型チェックがある理由って何だっけ？
                    //if ( item.ID.IconHandle is IconHandle ) {
                        var ish = item.ID.IconHandle;
                        current_handle++;
                        var handle_item = ish.castToVsqHandle();
                        handle_item.Index = current_handle;
                        handle.push( handle_item );
                        item.ID.IconHandle_index = current_handle;
                        if ( is_vocalo1 ) {
                            var lang = org.kbinani.vsq.VocaloSysUtil.getLanguageFromName( ish.IDS );
                            add_quotation_mark = lang == org.kbinani.vsq.VsqVoiceLanguage.Japanese;
                        } else if ( is_vocalo2 ) {
                            var lang = org.kbinani.vsq.VocaloSysUtil.getLanguageFromName( ish.IDS );
                            add_quotation_mark = lang == org.kbinani.vsq.VsqVoiceLanguage.Japanese;
                        }
                    //}
                }
                // LyricHandle
                if ( item.ID.LyricHandle != null ) {
                    current_handle++;
                    var handle_item = item.ID.LyricHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle_item.addQuotationMark = add_quotation_mark;
                    handle.push( handle_item );
                    item.ID.LyricHandle_index = current_handle;
                }
                // VibratoHandle
                if ( item.ID.VibratoHandle != null ) {
                    current_handle++;
                    var handle_item = item.ID.VibratoHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.push( handle_item );
                    item.ID.VibratoHandle_index = current_handle;
                }
                // NoteHeadHandle
                if ( item.ID.NoteHeadHandle != null ) {
                    current_handle++;
                    var handle_item = item.ID.NoteHeadHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.push( handle_item );
                    item.ID.NoteHeadHandle_index = current_handle;
                }
                // IconDynamicsHandle
                if ( item.ID.IconDynamicsHandle != null ) {
                    current_handle++;
                    var handle_item = item.ID.IconDynamicsHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle_item.setLength( item.ID.getLength() );
                    handle.push( handle_item );
                    item.ID.IconHandle_index = current_handle;
                }
            }
            return handle;
        },

        /**
        //TODO: VsqMetaText#print関連のメソッドがまだ
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
            while ( i < temp.size() ) {
                VsqEvent item = temp.get( i );
                if ( !item.ID.Equals( VsqID.EOS ) ) {
                    String ids = "ID#" + PortUtil.formatDecimal( "0000", item.ID.value );
                    int clock = temp.get( i ).Clock;
                    while ( i + 1 < temp.size() && clock == temp.get( i + 1 ).Clock ) {
                        i++;
                        ids += ",ID#" + PortUtil.formatDecimal( "0000", temp.get( i + 1 ).ID.value );
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
        }*/

        /**
         * 何も無いVsqMetaTextを構築する。これは、Master Track用のMetaTextとしてのみ使用されるべき
         * @return [VsqMetaText]
         */
        _init_0 : function(){
        },

        /**
         * @param sr [TextStream]
         * @return [VsqMetaText]
         */
        _init_1 : function( sr ){
            var t_event_list = new Array();
            var __id = {};// new TreeMap<Integer, VsqID>();
            var __handle = {};// new TreeMap<Integer, VsqHandle>();
            this.PIT = new org.kbinani.vsq.VsqBPList( "pit", 0, -8192, 8191 );
            this.PBS = new org.kbinani.vsq.VsqBPList( "pbs", 2, 0, 24 );
            this.DYN = new org.kbinani.vsq.VsqBPList( "dyn", 64, 0, 127 );
            this.BRE = new org.kbinani.vsq.VsqBPList( "bre", 0, 0, 127 );
            this.BRI = new org.kbinani.vsq.VsqBPList( "bri", 64, 0, 127 );
            this.CLE = new org.kbinani.vsq.VsqBPList( "cle", 0, 0, 127 );
            this.reso1FreqBPList = new org.kbinani.vsq.VsqBPList( "reso1freq", 64, 0, 127 );
            this.reso2FreqBPList = new org.kbinani.vsq.VsqBPList( "reso2freq", 64, 0, 127 );
            this.reso3FreqBPList = new org.kbinani.vsq.VsqBPList( "reso3freq", 64, 0, 127 );
            this.reso4FreqBPList = new org.kbinani.vsq.VsqBPList( "reso4freq", 64, 0, 127 );
            this.reso1BWBPList = new org.kbinani.vsq.VsqBPList( "reso1bw", 64, 0, 127 );
            this.reso2BWBPList = new org.kbinani.vsq.VsqBPList( "reso2bw", 64, 0, 127 );
            this.reso3BWBPList = new org.kbinani.vsq.VsqBPList( "reso3bw", 64, 0, 127 );
            this.reso4BWBPList = new org.kbinani.vsq.VsqBPList( "reso4bw", 64, 0, 127 );
            this.reso1AmpBPList = new org.kbinani.vsq.VsqBPList( "reso1amp", 64, 0, 127 );
            this.reso2AmpBPList = new org.kbinani.vsq.VsqBPList( "reso2amp", 64, 0, 127 );
            this.reso3AmpBPList = new org.kbinani.vsq.VsqBPList( "reso3amp", 64, 0, 127 );
            this.reso4AmpBPList = new org.kbinani.vsq.VsqBPList( "reso4amp", 64, 0, 127 );
            this.harmonics = new org.kbinani.vsq.VsqBPList( "harmonics", 64, 0, 127 );
            this.fx2depth = new org.kbinani.vsq.VsqBPList( "fx2depth", 64, 0, 127 );
            this.GEN = new org.kbinani.vsq.VsqBPList( "gen", 64, 0, 127 );
            this.POR = new org.kbinani.vsq.VsqBPList( "por", 64, 0, 127 );
            this.OPE = new org.kbinani.vsq.VsqBPList( "ope", 127, 0, 127 );

            var last_line = new org.kbinani.ByRef( sr.readLine() );
            while ( true ) {
//alert( "VsqMetaText#_init_1; last_line.value=" + last_line.value );
                // "TextMemoryStreamから順次読込み"
                if ( last_line.value.length == 0 ) {
                    break;
                }
                if ( last_line.value == "[Common]" ) {
                    this.Common = new org.kbinani.vsq.VsqCommon( sr, last_line );
                } else if ( last_line.value == "[Master]" ) {
                    this.master = new org.kbinani.vsq.VsqMaster( sr, last_line );
                } else if ( last_line.value == "[Mixer]" ) {
                    this.mixer = new org.kbinani.vsq.VsqMixer( sr, last_line );
                } else if ( last_line.value == "[EventList]" ) {
                    last_line.value = sr.readLine();
                    while ( last_line.value.indexOf( "[" ) !== 0 ) {
                        var spl2 = last_line.value.split( '=' );
                        var clock = parseInt( spl2[0], 10 );
                        var id_number = -1;
                        if ( spl2[1] != "EOS" ) {
                            var ids = spl2[1].split( ',' );
                            for ( var i = 0; i < ids.length; i++ ) {
                                var spl3 = ids[i].split( '#' );
                                id_number = parseInt( spl3[1], 10 );
                                t_event_list.push( new org.kbinani.ValuePair( clock, id_number ) );
                            }
                        } else {
                            t_event_list.push( new org.kbinani.ValuePair( clock, -1 ) );
                        }
                        if ( !sr.ready() ) {
                            break;
                        } else {
                            last_line.value = sr.readLine();
                        }
                    }
                } else if ( last_line.value == "[PitchBendBPList]" ) {
                    last_line.value = this.PIT.appendFromText( sr );
                } else if ( last_line.value == "[PitchBendSensBPList]" ) {
                    last_line.value = this.PBS.appendFromText( sr );
                } else if ( last_line.value == "[DynamicsBPList]" ) {
                    last_line.value = this.DYN.appendFromText( sr );
                } else if ( last_line.value == "[EpRResidualBPList]" ) {
                    last_line.value = this.BRE.appendFromText( sr );
                } else if ( last_line.value == "[EpRESlopeBPList]" ) {
                    last_line.value = this.BRI.appendFromText( sr );
                } else if ( last_line.value == "[EpRESlopeDepthBPList]" ) {
                    last_line.value = this.CLE.appendFromText( sr );
                } else if ( last_line.value == "[EpRSineBPList]" ) {
                    last_line.value = this.harmonics.appendFromText( sr );
                } else if ( last_line.value == "[VibTremDepthBPList]" ) {
                    last_line.value = this.fx2depth.appendFromText( sr );
                } else if ( last_line.value == "[Reso1FreqBPList]" ) {
                    last_line.value = this.reso1FreqBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso2FreqBPList]" ) {
                    last_line.value = this.reso2FreqBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso3FreqBPList]" ) {
                    last_line.value = this.reso3FreqBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso4FreqBPList]" ) {
                    last_line.value = this.reso4FreqBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso1BWBPList]" ) {
                    last_line.value = this.reso1BWBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso2BWBPList]" ) {
                    last_line.value = this.reso2BWBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso3BWBPList]" ) {
                    last_line.value = this.reso3BWBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso4BWBPList]" ) {
                    last_line.value = this.reso4BWBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso1AmpBPList]" ) {
                    last_line.value = this.reso1AmpBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso2AmpBPList]" ) {
                    last_line.value = this.reso2AmpBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso3AmpBPList]" ) {
                    last_line.value = this.reso3AmpBPList.appendFromText( sr );
                } else if ( last_line.value == "[Reso4AmpBPList]" ) {
                    last_line.value = this.reso4AmpBPList.appendFromText( sr );
                } else if ( last_line.value == "[GenderFactorBPList]" ) {
                    last_line.value = this.GEN.appendFromText( sr );
                } else if ( last_line.value == "[PortamentoTimingBPList]" ) {
                    last_line.value = this.POR.appendFromText( sr );
                } else if ( last_line.value == "[OpeningBPList]" ) {
                    last_line.value = this.OPE.appendFromText( sr );
                } else {
                    var buffer = last_line.value;
                    buffer = buffer.replace( "[", "" );
                    buffer = buffer.replace( "]", "" );
                    var spl = buffer.split( '#' );
                    var index = parseInt( spl[1], 10 );
                    if ( last_line.value.indexOf( "[ID#" ) === 0 ) {
                        __id[index] = new org.kbinani.vsq.VsqID( sr, index, last_line );
                    } else if ( last_line.value.indexOf( "[h#" ) === 0 ) {
                        __handle[index] = new org.kbinani.vsq.VsqHandle( sr, index, last_line );
                    }
                }

                if ( !sr.ready() ) {
                    break;
                }
            }

            // まずhandleをidに埋め込み
            //var c = __id.size();
            for ( var i in __id ) {
                var id = __id[i];
alert( "VsqMetaText#_init_1; i=" + i + "; id.IconHandle_index=" + id.IconHandle_index + "; id.LyricHandle_index=" + id.LyricHandle_index + "; id.NoteHeadHandle_index=" + id.NoteHeadHandle_index );
                if ( __handle[id.IconHandle_index] != undefined ) {
                    if ( id.type == org.kbinani.vsq.VsqIDType.Singer ) {
                        id.IconHandle = __handle[id.IconHandle_index].castToIconHandle();
                    } else if ( id.type == org.kbinani.vsq.VsqIDType.Aicon ) {
                        id.IconDynamicsHandle = __handle[id.IconHandle_index].castToIconDynamicsHandle();
                    }
                }
                if ( __handle[id.LyricHandle_index] != undefined ) {
                    id.LyricHandle = __handle[id.LyricHandle_index].castToLyricHandle();
                }
                if ( __handle[id.VibratoHandle_index] != undefined ) {
                    id.VibratoHandle = __handle[id.VibratoHandle_index].castToVibratoHandle();
                }
                if ( __handle[id.NoteHeadHandle_index] != undefined ) {
                    id.NoteHeadHandle = __handle[id.NoteHeadHandle_index].castToNoteHeadHandle();
                }
            }

            // idをeventListに埋め込み
            this.Events = new org.kbinani.vsq.VsqEventList();
            var count = 0;
            for ( var i = 0; i < t_event_list.length; i++ ) {
                var clock = t_event_list[i].getKey();
                var id_number = t_event_list[i].getValue();
                if ( __id[id_number] != undefined ) {
                    count++;
                    this.Events.add( new org.kbinani.vsq.VsqEvent( clock, __id[id_number].clone() ), count );
                }
            }
            this.Events.sort();

            if ( this.Common == null ) {
                this.Common = new org.kbinani.vsq.VsqCommon();
            }
        },

        /**
         * 最初のトラック以外の一般のメタテキストを構築。(Masterが作られない)
         * @param name [string]
         * @param singer [string]
         * @return [VsqMetaText]
         */
        _init_2 : function( name, singer ){
            this._initCor( name, 0, singer, false );
        },

        /**
         * 最初のトラックのメタテキストを構築。(Masterが作られる)
         * @param name [string]
         * @param singer [string]
         * @param pre_measure [int]
         * @return [VsqMetaText]
         */
        _init_3 : function( name, singer, pre_measure ){
            this._initCor( name, pre_measure, singer, true );
        },

        /**
         * @param name [string]
         * @param pre_measure [int]
         * @param singer [string]
         * @param is_first_track [bool]
         * @return [VsqMetaText]
         */
        _initCor : function( name, pre_measure, singer, is_first_track ){
            this.Common = new org.kbinani.vsq.VsqCommon( name, 179, 181, 123, 1, 1 );
            this.PIT = new org.kbinani.vsq.VsqBPList( "pit", 0, -8192, 8191 );
            this.PBS = new org.kbinani.vsq.VsqBPList( "pbs", 2, 0, 24 );
            this.DYN = new org.kbinani.vsq.VsqBPList( "dyn", 64, 0, 127 );
            this.BRE = new org.kbinani.vsq.VsqBPList( "bre", 0, 0, 127 );
            this.BRI = new org.kbinani.vsq.VsqBPList( "bri", 64, 0, 127 );
            this.CLE = new org.kbinani.vsq.VsqBPList( "cle", 0, 0, 127 );
            this.reso1FreqBPList = new org.kbinani.vsq.VsqBPList( "reso1freq", 64, 0, 127 );
            this.reso2FreqBPList = new org.kbinani.vsq.VsqBPList( "reso2freq", 64, 0, 127 );
            this.reso3FreqBPList = new org.kbinani.vsq.VsqBPList( "reso3freq", 64, 0, 127 );
            this.reso4FreqBPList = new org.kbinani.vsq.VsqBPList( "reso4freq", 64, 0, 127 );
            this.reso1BWBPList = new org.kbinani.vsq.VsqBPList( "reso1bw", 64, 0, 127 );
            this.reso2BWBPList = new org.kbinani.vsq.VsqBPList( "reso2bw", 64, 0, 127 );
            this.reso3BWBPList = new org.kbinani.vsq.VsqBPList( "reso3bw", 64, 0, 127 );
            this.reso4BWBPList = new org.kbinani.vsq.VsqBPList( "reso4bw", 64, 0, 127 );
            this.reso1AmpBPList = new org.kbinani.vsq.VsqBPList( "reso1amp", 64, 0, 127 );
            this.reso2AmpBPList = new org.kbinani.vsq.VsqBPList( "reso2amp", 64, 0, 127 );
            this.reso3AmpBPList = new org.kbinani.vsq.VsqBPList( "reso3amp", 64, 0, 127 );
            this.reso4AmpBPList = new org.kbinani.vsq.VsqBPList( "reso4amp", 64, 0, 127 );
            this.harmonics = new org.kbinani.vsq.VsqBPList( "harmonics", 64, 0, 127 );
            this.fx2depth = new org.kbinani.vsq.VsqBPList( "fx2depth", 64, 0, 127 );
            this.GEN = new org.kbinani.vsq.VsqBPList( "gen", 64, 0, 127 );
            this.POR = new org.kbinani.vsq.VsqBPList( "por", 64, 0, 127 );
            this.OPE = new org.kbinani.vsq.VsqBPList( "ope", 127, 0, 127 );
            if ( is_first_track ) {
                this.master = new org.kbinani.vsq.VsqMaster( pre_measure );
            } else {
                this.master = null;
            }
            this.Events = new org.kbinani.vsq.VsqEventList();
            var id = new org.kbinani.vsq.VsqID( 0 );
            id.type = org.kbinani.vsq.VsqIDType.Singer;
            var ish = new org.kbinani.vsq.IconHandle();
            ish.IconID = "$07010000";
            ish.IDS = singer;
            ish.Original = 0;
            ish.Caption = "";
            ish.setLength( 1 );
            ish.Language = 0;
            ish.Program = 0;
            id.IconHandle = ish;
            this.Events.add( new org.kbiani.vsq.VsqEvent( 0, id ) );
        },
    };

    /**
     * Editor画面上で上からindex番目のカーブの名前を調べます
     *
     * @param index [int]
     * @return [string]
     */
    org.kbinani.vsq.VsqMetaText.getCurveName = function( index ) {
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
    };

}
