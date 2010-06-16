/*
 * VsqEvent.js
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.VsqEvent == undefined ){

    /// <summary>
    /// vsqファイルのメタテキスト内に記述されるイベント。
    /// </summary>
    org.kbinani.vsq.VsqEvent = function(){
        this.Tag = "";
        /// <summary>
        /// 内部で使用するインスタンス固有のID
        /// </summary>
        this.InternalID = -1;
        this.Clock = 0;
        /**
         * [VsqID]
         */
        this.ID = null;
        /**
         * [UstEvent]
         */
        this.UstEvent = null;//new UstEvent();
        if( arguments.length == 0 ){
            this._init_0();
        }else if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }else if( arguments.length == 2 ){
            this._init_2( arguments[0], arguments[1] );
        }
    };

    org.kbinani.vsq.VsqEvent.prototype = {
        /**
         * @param item [VsqEvent]
         * @return [bool]
         */
        equals : function( item ) {
            if ( this.Clock != item.Clock ) {
                return false;
            }
            if ( this.ID.type != item.ID.type ) {
                return false;
            }
            if ( this.ID.type == org.kbinani.vsq.VsqIDType.Anote ) {
                //
                if ( this.ID.Note != item.ID.Note ) return false;
                if ( this.ID.getLength() != item.ID.getLength() ) return false;
                if ( this.ID.d4mean != item.ID.d4mean ) return false;
                if ( this.ID.DEMaccent != item.ID.DEMaccent ) return false;
                if ( this.ID.DEMdecGainRate != item.ID.DEMdecGainRate ) return false;
                if ( this.ID.Dynamics != item.ID.Dynamics ) return false;
                if ( this.ID.LyricHandle == null && item.ID.LyricHandle != null ) return false;
                if ( this.ID.LyricHandle != null && item.ID.LyricHandle == null ) return false;
                if ( this.ID.LyricHandle != null && item.ID.LyricHandle != null ) {
                    if ( !this.ID.LyricHandle.L0.equalsForSynth( item.ID.LyricHandle.L0 ) ) return false;
                    var count = this.ID.LyricHandle.Trailing.length;
                    if ( count != item.ID.LyricHandle.Trailing.length ) return false;
                    for ( var k = 0; k < count; k++ ) {
                        if ( !this.ID.LyricHandle.Trailing[k].equalsForSynth( item.ID.LyricHandle.Trailing[k] ) ) return false;
                    }
                }
                if ( this.ID.NoteHeadHandle == null && item.ID.NoteHeadHandle != null ) return false;
                if ( this.ID.NoteHeadHandle != null && item.ID.NoteHeadHandle == null ) return false;
                if ( this.ID.NoteHeadHandle != null && item.ID.NoteHeadHandle != null ) {
                    if ( this.ID.NoteHeadHandle.IconID != item.ID.NoteHeadHandle.IconID ) return false;
                    if ( this.ID.NoteHeadHandle.getDepth() != item.ID.NoteHeadHandle.getDepth() ) return false;
                    if ( this.ID.NoteHeadHandle.getDuration() != item.ID.NoteHeadHandle.getDuration() ) return false;
                    if ( this.ID.NoteHeadHandle.getLength() != item.ID.NoteHeadHandle.getLength() ) return false;
                }
                if ( this.ID.PMBendDepth != item.ID.PMBendDepth ) return false;
                if ( this.ID.PMBendLength != item.ID.PMBendLength ) return false;
                if ( this.ID.PMbPortamentoUse != item.ID.PMbPortamentoUse ) return false;
                if ( this.ID.pMeanEndingNote != item.ID.pMeanEndingNote ) return false;
                if ( this.ID.pMeanOnsetFirstNote != item.ID.pMeanOnsetFirstNote ) return false;
                var hVibratoThis = this.ID.VibratoHandle;
                var hVibratoItem = item.ID.VibratoHandle;
                if ( hVibratoThis == null && hVibratoItem != null ) return false;
                if ( hVibratoThis != null && hVibratoItem == null ) return false;
                if ( hVibratoThis != null && hVibratoItem != null ) {
                    if ( this.ID.VibratoDelay != item.ID.VibratoDelay ) return false;
                    if ( hVibratoThis.IconID != hVibratoItem.IconID ) return false;
                    if ( hVibratoThis.getStartDepth() != hVibratoItem.getStartDepth() ) return false;
                    if ( hVibratoThis.getStartRate() != hVibratoItem.getStartRate() ) return false;
                    var vibRateThis = hVibratoThis.getRateBP();
                    var vibRateItem = hVibratoItem.getRateBP();
                    if ( vibRateThis == null && vibRateItem != null ) return false;
                    if ( vibRateThis != null && vibRateItem == null ) return false;
                    if ( vibRateThis != null && vibRateItem != null ) {
                        var numRateCount = vibRateThis.getCount();
                        if ( numRateCount != vibRateItem.getCount() ) return false;
                        for ( var k = 0; k < numRateCount; k++ ) {
                            var pThis = vibRateThis.getElement( k );
                            var pItem = vibRateItem.getElement( k );
                            if ( pThis.X != pItem.X ) return false;
                            if ( pThis.Y != pItem.Y ) return false;
                        }
                    }
                    var vibDepthThis = hVibratoThis.getDepthBP();
                    var vibDepthItem = hVibratoItem.getDepthBP();
                    if ( vibDepthThis == null && vibDepthItem != null ) return false;
                    if ( vibDepthThis != null && vibDepthItem == null ) return false;
                    if ( vibDepthThis != null && vibDepthItem != null ) {
                        var numDepthCount = vibDepthThis.getCount();
                        if ( numDepthCount != vibDepthItem.getCount() ) return false;
                        for ( var k = 0; k < numDepthCount; k++ ) {
                            var pThis = vibDepthThis.getElement( k );
                            var pItem = vibDepthItem.getElement( k );
                            if ( pThis.X != pItem.X ) return false;
                            if ( pThis.Y != pItem.Y ) return false;
                        }
                    }
                }
                if ( this.ID.vMeanNoteTransition != item.ID.vMeanNoteTransition ) return false;
            } else if ( this.ID.type == org.kbinani.vsq.VsqIDType.Singer ) {
                // シンガーイベントの比較
                if ( this.ID.IconHandle.Program != item.ID.IconHandle.Program ) return false;
            } else if ( this.ID.type == org.kbinani.vsq.VsqIDType.Aicon ) {
                if ( this.ID.IconDynamicsHandle.IconID != item.ID.IconDynamicsHandle.IconID ) return false;
                if ( this.ID.IconDynamicsHandle.isDynaffType() ) {
                    // 強弱記号
                } else {
                    // クレッシェンド・デクレッシェンド
                    if ( this.ID.getLength() != item.ID.getLength() ) return false;
                }
            }

            return true;
        },

        /*
        //TODO: VsqEvent#write関連のメソッド
        /// <summary>
        /// インスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力先</param>
        public void write( ITextWriter sw )
#if JAVA
            throws IOException
#endif
        {
            Vector<String> def = new Vector<String>( Arrays.asList( new String[]{ "Length",
                                                                   "Note#",
                                                                   "Dynamics",
                                                                   "PMBendDepth",
                                                                   "PMBendLength",
                                                                   "PMbPortamentoUse",
                                                                   "DEMdecGainRate",
                                                                   "DEMaccent" } ) );
            write( sw, def );
        }

        public void write( ITextWriter writer, Vector<String> print_targets )
#if JAVA
            throws IOException
#endif
        {
            writeCor( writer, print_targets );
        }

        public void write( BufferedWriter writer, Vector<String> print_targets )
#if JAVA
            throws IOException
#endif
        {
            writeCor( new WrappedStreamWriter( writer ), print_targets );
        }

        private void writeCor( ITextWriter writer, Vector<String> print_targets )
#if JAVA
            throws IOException
#endif
        {
            writer.writeLine( "[ID#" + PortUtil.formatDecimal( "0000", ID.value ) + "]" );
            writer.writeLine( "Type=" + ID.type );
            if ( ID.type == VsqIDType.Anote ) {
                if ( print_targets.contains( "Length" ) ) writer.writeLine( "Length=" + ID.getLength() );
                if ( print_targets.contains( "Note#" ) ) writer.writeLine( "Note#=" + ID.Note );
                if ( print_targets.contains( "Dynamics" ) ) writer.writeLine( "Dynamics=" + ID.Dynamics );
                if ( print_targets.contains( "PMBendDepth" ) ) writer.writeLine( "PMBendDepth=" + ID.PMBendDepth );
                if ( print_targets.contains( "PMBendLength" ) ) writer.writeLine( "PMBendLength=" + ID.PMBendLength );
                if ( print_targets.contains( "PMbPortamentoUse" ) ) writer.writeLine( "PMbPortamentoUse=" + ID.PMbPortamentoUse );
                if ( print_targets.contains( "DEMdecGainRate" ) ) writer.writeLine( "DEMdecGainRate=" + ID.DEMdecGainRate );
                if ( print_targets.contains( "DEMaccent" ) ) writer.writeLine( "DEMaccent=" + ID.DEMaccent );
                if ( print_targets.contains( "PreUtterance" ) ) writer.writeLine( "PreUtterance=" + UstEvent.PreUtterance );
                if ( print_targets.contains( "VoiceOverlap" ) ) writer.writeLine( "VoiceOverlap=" + UstEvent.VoiceOverlap );
                if ( ID.LyricHandle != null ) {
                    writer.writeLine( "LyricHandle=h#" + PortUtil.formatDecimal( "0000", ID.LyricHandle_index ) );
                }
                if ( ID.VibratoHandle != null ) {
                    writer.writeLine( "VibratoHandle=h#" + PortUtil.formatDecimal( "0000", ID.VibratoHandle_index ) );
                    writer.writeLine( "VibratoDelay=" + ID.VibratoDelay );
                }
                if ( ID.NoteHeadHandle != null ) {
                    writer.writeLine( "NoteHeadHandle=h#" + PortUtil.formatDecimal( "0000", ID.NoteHeadHandle_index ) );
                }
            } else if ( ID.type == VsqIDType.Singer ) {
                writer.writeLine( "IconHandle=h#" + PortUtil.formatDecimal( "0000", ID.IconHandle_index ) );
            } else if ( ID.type == VsqIDType.Aicon ) {
                writer.writeLine( "IconHandle=h#" + PortUtil.formatDecimal( "0000", ID.IconHandle_index ) );
                writer.writeLine( "Note#=" + ID.Note );
            }
        }*/

        /**
         * このオブジェクトのコピーを作成します
         *
         * @return [object]
         */
        clone : function() {
            var ret = new org.kbinani.vsq.VsqEvent( this.Clock, this.ID.clone() );
            ret.InternalID = this.InternalID;
            if ( this.UstEvent != null ) {
                ret.UstEvent = this.UstEvent.clone();
            }
            ret.Tag = this.Tag;
            return ret;
        },

        /**
         * @param item [VsqEvent]
         * @return [int]
         */
        compareTo : function( item ) {
            var ret = this.Clock - item.Clock;
            if ( ret == 0 ) {
                if ( this.ID != null && item.ID != null ) {
                    return this.ID.type - item.ID.type;
                } else {
                    return ret;
                }
            } else {
                return ret;
            }
        },

        /**
         * @param line [string]
         * @return [VsqEvent]
         */
        _init_1 : function( line ){
            var spl = line.split( '=' );
            this.Clock = parseInt( spl[0], 10 );
            if ( spl[1] == "EOS" ) {
                this.ID = org.kbinani.vsq.VsqID.EOS.clone();
            }
        },

        _init_0 : function(){
            this.Clock = 0;
            this.ID = new org.kbinani.vsq.VsqID();
            this.InternalID = 0;
        },

        /**
         * @param clcok [int]
         * @param id [VsqID]
         * @return [VsqEvent]
         */
        _init_2 : function( clock, id ){
            this.Clock = clock;
            this.ID = id;
            this.InternalID = 0;
        },
    };

    /**
     * 2つのVsqEventを比較します
     *
     * @param a [VsqEvent]
     * @param b [VsqEvent]
     * @return [int]
     */
    org.kbinani.vsq.VsqEvent.compare = function( a, b ){
        return a.compareTo( b );
    };

}
