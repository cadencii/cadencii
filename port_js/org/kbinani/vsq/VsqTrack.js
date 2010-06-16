/*
 * VsqTrack.js
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
if( org.kbinani.vsq.VsqTrack == undefined ){

    /// <summary>
    /// Stores the data of a vsq track.
    /// </summary>
    org.kbinani.vsq.VsqTrack = function(){
        this.Tag = "";
        this.MetaText = null;
        if( arguments.length == 0 ){
            this._init_0();
        }else if( arguments.length == 2 ){
            if( typeof( arguments[0] ) == "string" ){
                this._init_2a( arguments[0], arguments[1] );
            }else{
                this._init_2b( arguments[0], arguments[1] );
            }
        }else if( arguments.length == 3 ){
            this._init_3( arguments[0], arguments[1], arguments[2] );
        }
    };

    org.kbinani.vsq.VsqTrack.IndexIterator = function(){
        /**
         * [VsqEventList]
         */
        this._list;
        this._pos = -1;
        this._kindSinger = false;
        this._kindNote = false;
        this._kindCrescend = false;
        this._kindDecrescend = false;
        this._kindDynaff = false;
        if( arguments.length == 2 ){
            this._init_2( arguments[0], arguments[1] );
        }
    };

    org.kbinani.vsq.VsqTrack.IndexIterator.prototype = {
        /**
         * @param list [VsqEventList]
         * @param iterator_kind[int]
         * @return [IndexIterator]
         */
        _init_2 : function( list, iterator_kind ) {
            this._list = list;
            this._pos = -1;
            this._kindSinger = (iterator_kind & org.kbinani.vsq.IndexIteratorKind.SINGER) == org.kbinani.vsq.IndexIteratorKind.SINGER;
            this._kindNote = (iterator_kind & org.kbinani.vsq.IndexIteratorKind.NOTE) == org.kbinani.vsq.IndexIteratorKind.NOTE;
            this._kindCrescend = (iterator_kind & org.kbinani.vsq.IndexIteratorKind.CRESCEND) == org.kbinani.vsq.IndexIteratorKind.CRESCEND;
            this._kindDecrescend = (iterator_kind & org.kbinani.vsq.IndexIteratorKind.DECRESCEND) == org.kbinani.vsq.IndexIteratorKind.DECRESCEND;
            this._kindDynaff = (iterator_kind & org.kbinani.vsq.IndexIteratorKind.DYNAFF) == org.kbinani.vsq.IndexIteratorKind.DYNAFF;
            return this;
        },

        /**
         * @return [bool]
         */
        hasNext : function() {
            var count = this._list.getCount();
            for ( var i = this._pos + 1; i < count; i++ ) {
                var item = this._list.getElement( i );
                if ( kindSinger ){
                    if ( item.ID.type == org.kbinani.vsq.VsqIDType.Singer ) {
                        return true;
                    }
                }
                if ( kindNote ) {
                    if ( item.ID.type == org.kbinani.vsq.VsqIDType.Anote ) {
                        return true;
                    }
                }
                if ( item.ID.type == org.kbinani.vsq.VsqIDType.Aicon && item.ID.IconDynamicsHandle != null && item.ID.IconDynamicsHandle.IconID != null ) {
                    var iconid = item.ID.IconDynamicsHandle.IconID;
                    if ( kindDynaff ) {
                        if ( iconid.indexOf( org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_DYNAFF ) === 0 ) {
                            return true;
                        }
                    }
                    if ( kindCrescend ) {
                        if ( iconid.indexOf( org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_CRESCEND ) === 0 ) {
                            return true;
                        }
                    }
                    if ( kindDecrescend ) {
                        if ( iconid.indexOf( org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_DECRESCEND ) === 0 ) {
                            return true;
                        }
                    }
                }
            }
            return false;
        },

        /**
         * @return [int]
         */
        next : function() {
            var count = this._list.getCount();
            for ( var i = this._pos + 1; i < count; i++ ) {
                var item = this._list.getElement( i );
                if ( kindSinger ) {
                    if ( item.ID.type == org.kbinani.vsq.VsqIDType.Singer ) {
                        this._pos = i;
                        return i;
                    }
                }
                if ( kindNote ) {
                    if ( item.ID.type == org.kbinani.vsq.VsqIDType.Anote ) {
                        this._pos = i;
                        return i;
                    }
                }
                if ( kindDynaff || kindCrescend || kindDecrescend ) {
                    if ( item.ID.type == org.kbinani.vsq.VsqIDType.Aicon && item.ID.IconDynamicsHandle != null && item.ID.IconDynamicsHandle.IconID != null ) {
                        var iconid = item.ID.IconDynamicsHandle.IconID;
                        if ( kindDynaff ) {
                            if ( iconid.indexOf( org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_DYNAFF ) === 0 ) {
                                this._pos = i;
                                return i;
                            }
                        }
                        if ( kindCrescend ) {
                            if ( iconid.indexOf( org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_CRESCEND ) === 0 ) {
                                this._pos = i;
                                return i;
                            }
                        }
                        if ( kindDecrescend ) {
                            if ( iconid.indexOf( org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_DECRESCEND ) === 0 ) {
                                this._pos = i;
                                return i;
                            }
                        }
                    }
                }
            }
            return -1;
        },

        /**
         * @return [void]
         */
        remove : function() {
            if ( 0 <= this._pos && this._pos < this._list.getCount() ) {
                this._list.splice( this._pos, 1 );
            }
        },
    };

    org.kbinani.vsq.VsqTrack.SingerEventIterator = function(){
        this._m_list = null;
        this._m_pos = -1;
        if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }
    };

    org.kbinani.vsq.VsqTrack.SingerEventIterator.prototype = {
        /**
         * @param list [VsqEventList]
         * @return [SingerEventIterator]
         */
        _init_1 : function( list ) {
            this._m_list = list;
            this._m_pos = -1;
            return this;
        },

        /**
         * @return [bool]
         */
        hasNext : function() {
            var num = this._m_list.getCount();
            for ( var i = this._m_pos + 1; i < num; i++ ) {
                if ( this._m_list.getElement( i ).ID.type == org.kbinani.vsq.VsqIDType.Singer ) {
                    return true;
                }
            }
            return false;
        },

        /**
         * @return [VsqEvent]
         */
        next : function() {
            var num = this._m_list.getCount();
            for ( var i = this._m_pos + 1; i < num; i++ ) {
                var item = this._m_list.getElement( i );
                if ( item.ID.type == org.kbinani.vsq.VsqIDType.Singer ) {
                    this._m_pos = i;
                    return item;
                }
            }
            return null;
        },

        /**
         * @return [void]
         */
        remove : function() {
            if ( 0 <= this._m_pos && this._m_pos < this._m_list.getCount() ) {
                this._m_list.splice( this._m_pos, 1 );
            }
        },
    };

    org.kbinani.vsq.VsqTrack.NoteEventIterator = function(){
        this._m_list = null;
        this._m_pos = -1;
        if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }
    };

    org.kbinani.vsq.VsqTrack.NoteEventIterator.prototype = {
        /**
         * @param list [VsqEventList]
         * @return [NoteEventIterator]
         */
        _init_1 : function( list ) {
            this._m_list = list;
            this._m_pos = -1;
            return this;
        },

        /**
         * @return [bool]
         */
        hasNext : function() {
            var count = this._m_list.getCount();
            for ( var i = this._m_pos + 1; i < count; i++ ) {
                if ( this._m_list.getElement( i ).ID.type == org.kbinani.vsq.VsqIDType.Anote ) {
                    return true;
                }
            }
            return false;
        },

        /**
         * @return [VsqEvent]
         */
        next : function() {
            var count = this._m_list.getCount();
            for ( var i = this._m_pos + 1; i < count; i++ ) {
                var item = this._m_list.getElement( i );
                if ( item.ID.type == org.kbinani.vsq.VsqIDType.Anote ) {
                    this._m_pos = i;
                    return item;
                }
            }
            return null;
        },

        /**
         * @return [void]
         */
        remove : function() {
            if ( 0 <= this._m_pos && this._m_pos < this._m_list.getCount() ) {
                this._m_list.splice( this._m_pos, 1 );
            }
        },
    };

    org.kbinani.vsq.VsqTrack.DynamicsEventIterator = function(){
        this._m_list = null;
        this._m_pos = -1;
        if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }
    };

    org.kbinani.vsq.VsqTrack.DynamicsEventIterator.prototype = {
        /**
         * @param list [VsqEventList]
         * @return [DynamicsEventIterator]
         */
        _init_1 : function( list ) {
            this._m_list = list;
            this._m_pos = -1;
        },

        /**
         * @return [bool]
         */
        hasNext : function() {
            var c = this._m_list.getCount();
            for ( var i = this._m_pos + 1; i < c; i++ ) {
                if ( this._m_list.getElement( i ).ID.type == org.kbinani.vsq.VsqIDType.Aicon ) {
                    return true;
                }
            }
            return false;
        },

        /**
         * @return [VsqEvent]
         */
        next : function() {
            var c = this._m_list.getCount();
            for ( var i = this._m_pos + 1; i < c; i++ ) {
                var item = this._m_list.getElement( i );
                if ( item.ID.type == org.kbinani.vsq.VsqIDType.Aicon ) {
                    this._m_pos = i;
                    return item;
                }
            }
            return null;
        },

        /**
         * @return [void]
         */
        remove : function() {
            if ( 0 <= this._m_pos && this._m_pos < this._m_list.getCount() ) {
                this._m_list.splice( this._m_pos, 1 );
            }
        },
    };

    org.kbinani.vsq.VsqTrack.EventIterator = function(){
        this._m_list = null;
        this._m_pos = -1;
        if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }
    };

    org.kbinani.vsq.VsqTrack.EventIterator.prototype = {
        /**
         * @param list [VsqEventList]
         * @return [EventIterator]
         */
        _init_1 : function( list ) {
            this._m_list = list;
            this._m_pos = -1;
            return this;
        },

        /**
         * @return [bool]
         */
        hasNext : function() {
            if ( 0 <= this._m_pos + 1 && this._m_pos + 1 < this._m_list.getCount() ) {
                return true;
            }
            return false;
        },

        /**
         * @return [VsqEvent]
         */
        next : function() {
            this._m_pos++;
            return this._m_list.getElement( this._m_pos );
        },

        /**
         * @return [void]
         */
        remove : function() {
            if ( 0 <= this._m_pos && this._m_pos < this._m_list.getCount() ) {
                this._m_list.splice( this._m_pos, 1 );
            }
        },
    };

    org.kbinani.vsq.VsqTrack.prototype = {
        /**
         * 指定された種類のイベントのインデクスを順に返すイテレータを取得します．
         *
         * @param iterator_kind [int]
         * @return [IndexIterator]
         */
        indexIterator : function( iterator_kind ) {
            if ( this.MetaText == null ) {
                return new org.kbinani.vsq.VsqTrack.IndexIterator( new org.kbinani.vsq.VsqEventList(), iterator_kind );
            } else {
                return new org.kbinani.vsq.VsqTrack.IndexIterator( this.MetaText.getEventList(), iterator_kind );
            }
        },

        /**
         * このトラックの再生モードを取得します．
         *
         * @return [int] PlayMode.PlayAfterSynthまたはPlayMode.PlayWithSynth
         */
        getPlayMode : function() {
            if ( this.MetaText == null ){
                return org.kbinani.vsq.PlayMode.PlayWithSynth;
            }
            if ( this.MetaText.Common == null ){
                return org.kbinani.vsq.PlayMode.PlayWithSynth;
            }
            if ( this.MetaText.Common.LastPlayMode != org.kbinani.vsq.PlayMode.PlayAfterSynth && 
                 this.MetaText.Common.LastPlayMode != org.kbinani.vsq.PlayMode.PlayWithSynth ) {
                this.MetaText.Common.LastPlayMode = org.kbinani.vsq.PlayMode.PlayWithSynth;
            }
            return this.MetaText.Common.LastPlayMode;
        },

        /**
         * このトラックの再生モードを設定します．
         *
         * @param value [int] PlayMode.PlayAfterSynth, PlayMode.PlayWithSynth, またはPlayMode.Offのいずれかを指定します
         * @return [void]
         */
        setPlayMode : function( value ) {
            if ( this.MetaText == null ) return;
            if ( this.MetaText.Common == null ) {
                this.MetaText.Common = new org.kbinani.vsq.VsqCommon( "Miku", 128, 128, 128, org.kbinani.vsq.DynamicsMode.Expert, value );
                return;
            }
            if ( value == org.kbinani.vsq.PlayMode.Off ) {
                if ( this.MetaText.Common.PlayMode != org.kbinani.vsq.PlayMode.Off ) {
                    this.MetaText.Common.LastPlayMode = this.MetaText.Common.PlayMode;
                }
            } else {
                this.MetaText.Common.LastPlayMode = value;
            }
            this.MetaText.Common.PlayMode = value;
        },

        /**
         * このトラックがレンダリングされるかどうかを取得します．
         *
         * @return [bool]
         */
        isTrackOn : function() {
            if ( this.MetaText == null ) return true;
            if ( this.MetaText.Common == null ) return true;
            return this.MetaText.Common.PlayMode != org.kbinani.vsq.PlayMode.Off;
        },

        /**
         * このトラックがレンダリングされるかどうかを設定します，
         *
         * @param value [bool]
         */
        setTrackOn : function( value ) {
            if ( this.MetaText == null ) return;
            if ( this.MetaText.Common == null ) {
                this.MetaText.Common = new org.kbinani.vsq.VsqCommon( "Miku", 128, 128, 128, org.kbinani.vsq.DynamicsMode.Expert, value ? org.kbinani.vsq.PlayMode.PlayWithSynth : org.kbinani.vsq.PlayMode.Off );
            }
            if ( value ) {
                if ( this.MetaText.Common.LastPlayMode != org.kbinani.vsq.PlayMode.PlayAfterSynth &&
                     this.MetaText.Common.LastPlayMode != org.kbinani.vsq.PlayMode.PlayWithSynth ) {
                    this.MetaText.Common.LastPlayMode = org.kbinani.vsq.PlayMode.PlayWithSynth;
                }
                this.MetaText.Common.PlayMode = this.MetaText.Common.LastPlayMode;
            } else {
                if ( this.MetaText.Common.PlayMode == org.kbinani.vsq.PlayMode.PlayAfterSynth ||
                     this.MetaText.Common.PlayMode == org.kbinani.vsq.PlayMode.PlayWithSynth ) {
                    this.MetaText.Common.LastPlayMode = this.MetaText.Common.PlayMode;
                }
                this.MetaText.Common.PlayMode = org.kbinani.vsq.PlayMode.Off;
            }
        },

        /**
         * このトラックの名前を取得します．
         *
         * @return [string]
         */
        getName : function() {
            if ( this.MetaText == null || (this.MetaText != null && this.MetaText.Common == null) ) {
                return "Master Track";
            } else {
                return this.MetaText.Common.Name;
            }
        },

        /**
         * このトラックの名前を設定します．
         *
         * @param value [string]
         */
        setName : function( value ) {
            if ( this.MetaText != null ) {
                if ( this.MetaText.Common == null ) {
                    this.MetaText.Common = new org.kbinani.vsq.VsqCommon();
                }
                this.MetaText.Common.Name = value;
            }
        },

        /**
         * このトラックの，指定したゲートタイムにおけるピッチベンドを取得します．単位はCentです．
         *
         * @param clock [int] ピッチベンドを取得するゲートタイム
         * @return [double]
         */
        getPitchAt : function( clock ) {
            var inv2_13 = 1.0 / 8192.0;
            var pit = this.MetaText.PIT.getValue( clock );
            var pbs = this.MetaText.PBS.getValue( clock );
            return pit * pbs * inv2_13 * 100.0;
        },

        /**
         * クレッシェンド，デクレッシェンド，および強弱記号をダイナミクスカーブに反映させます．
         * この操作によって，ダイナミクスカーブに設定されたデータは全て削除されます．
         * @return [void]
         */
        reflectDynamics : function() {
            var dyn = this.getCurve( "dyn" );
            dyn.clear();
            for ( var itr = this.getDynamicsEventIterator(); itr.hasNext(); ) {
                var item = itr.next();
                var handle = item.ID.IconDynamicsHandle;
                if ( handle == null ) {
                    continue;
                }
                var clock = item.Clock;
                var length = item.ID.getLength();

                if ( handle.isDynaffType() ) {
                    // 強弱記号
                    dyn.add( clock, handle.getStartDyn() );
                } else {
                    // クレッシェンド，デクレッシェンド
                    var start_dyn = dyn.getValue( clock );

                    // 範囲内のアイテムを削除
                    var count = dyn.size();
                    for ( var i = count - 1; i >= 0; i-- ) {
                        var c = dyn.getKeyClock( i );
                        if ( clock <= c && c <= clock + length ) {
                            dyn.removeElementAt( i );
                        } else if ( c < clock ) {
                            break;
                        }
                    }

                    var bplist = handle.getDynBP();
                    if ( bplist == null || (bplist != null && bplist.getCount() <= 0) ) {
                        // カーブデータが無い場合
                        var a = 0.0;
                        if ( length > 0 ) {
                            a = (handle.getEndDyn() - handle.getStartDyn()) / length;
                        }
                        var last_val = start_dyn;
                        for ( var i = clock; i < clock + length; i++ ) {
                            var val = start_dyn + org.kbinani.PortUtil.castToInt( a * (i - clock) );
                            if ( val < dyn.getMinimum() ) {
                                val = dyn.getMinimum();
                            } else if ( dyn.getMaximum() < val ) {
                                val = dyn.getMaximum();
                            }
                            if ( last_val != val ) {
                                dyn.add( i, val );
                                last_val = val;
                            }
                        }
                    } else {
                        // カーブデータがある場合
                        var last_val = handle.getStartDyn();
                        var last_clock = clock;
                        var bpnum = bplist.getCount();
                        var last = start_dyn;

                        // bplistに指定されている分のデータ点を追加
                        for ( var i = 0; i < bpnum; i++ ) {
                            var point = bplist.getElement( i );
                            var pointClock = clock + org.kbinani.PortUtil.castToInt( length * point.X );
                            if ( pointClock <= last_clock ) {
                                continue;
                            }
                            var pointValue = point.Y;
                            var a = (pointValue - last_val) / (pointClock - last_clock);
                            for ( var j = last_clock; j <= pointClock; j++ ) {
                                var val = start_dyn + org.kbinani.PortUtil.castToInt( (j - last_clock) * a );
                                if ( val < dyn.getMinimum() ) {
                                    val = dyn.getMinimum();
                                } else if ( dyn.getMaximum() < val ) {
                                    val = dyn.getMaximum();
                                }
                                if ( val != last ) {
                                    dyn.add( j, val );
                                    last = val;
                                }
                            }
                            last_val = point.Y;
                            last_clock = pointClock;
                        }

                        // bplistの末尾から，clock => clock + lengthまでのデータ点を追加
                        var last2 = last;
                        if ( last_clock < clock + length ) {
                            var a = (handle.getEndDyn() - last_val) / (clock + length - last_clock);
                            for ( var j = last_clock; j < clock + length; j++ ) {
                                var val = last2 + org.kbinani.PortUtil.castToInt( (j - last_clock) * a );
                                if ( val < dyn.getMinimum() ) {
                                    val = dyn.getMinimum();
                                } else if ( dyn.getMaximum() < val ) {
                                    val = dyn.getMaximum();
                                }
                                if ( val != last ) {
                                    dyn.add( j, val );
                                    last = val;
                                }
                            }
                        }
                    }
                }
            }
        },

        /**
         * 指定したゲートタイムにおいて、歌唱を担当している歌手のVsqEventを取得します．
         *
         * @param clock [int]
         * @return [VsqEvent]
         */
        getSingerEventAt : function( clock ) {
            var last = null;
            for ( var itr = this.getSingerEventIterator(); itr.hasNext(); ) {
                var item = itr.next();
                if ( clock < item.Clock ) {
                    return last;
                }
                last = item;
            }
            return last;
        },

        /**
         * このトラックに設定されているイベントを，ゲートタイム順に並べ替えます．
         *
         * @reutrn [void]
         */
        sortEvent : function() {
            this.MetaText.Events.sort();
        },

        /**
         * 歌手変更イベントを，曲の先頭から順に返すIteratorを取得します．
         *
         * @return [SingerEventIterator]
         */
        getSingerEventIterator : function() {
            return new org.kbinani.vsq.VsqTrack.SingerEventIterator( this.MetaText.getEventList() );
        },

        /**
         * 音符イベントを，曲の先頭から順に返すIteratorを取得します．
         *
         * @return [NoteEventIterator]
         */
        getNoteEventIterator : function() {
            if ( this.MetaText == null ) {
                return new org.kbinani.vsq.VsqTrack.NoteEventIterator( new org.kbinani.vsq.VsqEventList() );
            } else {
                return new org.kbinani.vsq.VsqTrack.NoteEventIterator( this.MetaText.getEventList() );
            }
        },

        /**
         * クレッシェンド，デクレッシェンド，および強弱記号を表すイベントを，曲の先頭から順に返すIteratorを取得します．
         *
         * @return [DynamicsEventIterator]
         */
        getDynamicsEventIterator : function() {
            if ( this.MetaText == null ) {
                return new org.kbinani.vsq.VsqTrack.DynamicsEventIterator( new org.kbinani.vsq.VsqEventList() );
            } else {
                return new org.kbinani.vsq.VsqTrack.DynamicsEventIterator( this.MetaText.getEventList() );
            }
        },

        /**
        //TODO: VsqTrack#printMetaText関連のメソッド
        *
         * このトラックのメタテキストをストリームに出力します．
         *
         * @param sw [ITextWriter]
        /// <param name="encode"></param>
        /// <param name="eos"></param>
        /// <param name="start"></param>
        public void printMetaText( ITextWriter sw, int eos, int start )
#if JAVA
            throws IOException
#endif
 {
            MetaText.print( sw, eos, start );
        }

        /// <summary>
        /// このトラックのメタテキストを，指定されたファイルに出力します．
        /// </summary>
        /// <param name="file"></param>
        public void printMetaText( String file, String encoding )
#if JAVA
            throws IOException
#endif
        {
            TextStream tms = new TextStream();
            int count = MetaText.getEventList().getCount();
            int clLast = MetaText.getEventList().getElement( count - 1 ).Clock + 480;
            MetaText.print( tms, clLast, 0 );
            BufferedWriter sw = null;
            try {
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( file ), encoding ) );
                tms.setPointer( -1 );
                while ( tms.ready() ) {
                    String line = tms.readLine().ToString();
                    sw.write( line );
                    sw.newLine();
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "VsqTrack#printMetaText; ex=" + ex );
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "VsqTrack#printMetaText; ex2=" + ex2 );
                    }
                }
            }
        }*/

        /**
         * このトラックのMasterを取得します．
         *
         * @return [VsqMaster]
         */
        getMaster : function() {
            return this.MetaText.master;
        },

        /**
         * このトラックのMasterを設定します．
         *
         * @param value [VsqMaster]
         */
        setMaster : function( value ) {
            this.MetaText.master = value;
        },

        /**
         * このトラックのMixerを取得します．
         *
         * @return [VsqMixer]
         */
        getMixer : function() {
            return this.MetaText.mixer;
        },

        /**
         * このトラックのMixerを設定します．
         *
         * @param value [VsqMixer]
         */
        setMixer : function( value ) {
            this.MetaText.mixer = value;
        },

        /**
         * Commonを取得します
         *
         * @return [VsqCommon]
         */
        getCommon : function() {
            return this.MetaText.Common;
        },

        /**
         * レンダラーを変更します
         *
         * @param new_renderer [string]
         * @param singers [Array<VsqID>]
         */
        changeRenderer : function( new_renderer, singers ) {
            var default_id = null;
            var singers_size = singers.length;
            if ( singers_size <= 0 ) {
                default_id = new org.kbinani.vsq.VsqID();
                default_id.type = org.kbinani.vsq.VsqIDType.Singer;
                var singer_handle = new org.kbinani.vsq.IconHandle();
                singer_handle.IconID = "$0701" + org.kbinani.PortUtil.sprintf( "%04X", 0 );
                singer_handle.IDS = "Unknown";
                singer_handle.Index = 0;
                singer_handle.Language = 0;
                singer_handle.setLength( 1 );
                singer_handle.Original = 0;
                singer_handle.Program = 0;
                singer_handle.Caption = "";
                default_id.IconHandle = singer_handle;
            } else {
                default_id = singers[0];
            }

            for ( var itr = this.getSingerEventIterator(); itr.hasNext(); ) {
                var ve = itr.next();
                var singer_handle = ve.ID.IconHandle;
                var program = singer_handle.Program;
                var found = false;
                for ( var i = 0; i < singers_size; i++ ) {
                    var id = singers[i];
                    if ( program == singer_handle.Program ) {
                        ve.ID = id.clone();
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    var add = default_id.clone();
                    add.IconHandle.Program = program;
                    ve.ID = add;
                }
            }
            this.MetaText.Common.Version = new_renderer;
        },

        /**
         * このトラックが保持している，指定されたカーブのBPListを取得します
         *
         * @param curve [string]
         */
        getCurve : function( curve ) {
            return this.MetaText.getElement( curve );
        },

        /**
         * @param curve [string]
         * @param value [VsqBPList]
         * @return [void]
         */
        setCurve : function( curve, value ) {
            this.MetaText.setElement( curve, value );
        },

        /**
         * @return [int]
         */
        getEventCount : function() {
            return this.MetaText.getEventList().getCount();
        },

        /**
         * @param index [int]
         * @return [VsqEvent]
         */
        getEvent : function( index ) {
            return this.MetaText.getEventList().getElement( index );
        },

        /**
         * @param internal_id [int]
         * @return [VsqEvent]
         */
        findEventFromID : function( internal_id ) {
            return this.MetaText.getEventList().findFromID( internal_id );
        },

        /**
         * @param internal_id [int]
         * @return [int]
         */
        findEventIndexFromID : function( internal_id ) {
            return this.MetaText.getEventList().findIndexFromID( internal_id );
        },

        /**
         * @param index [int]
         * @param item [VsqEvent]
         * @return [void]
         */
        setEvent : function( index, item ) {
            this.MetaText.getEventList().setElement( index, item );
        },

        /**
         * @param item [VsqEvent]
         * @return [int]
         */
        addEvent : function( item ) {
            return this.MetaText.getEventList().add( item );
        },

        /**
         * @param item [VsqEvent]
         * @param internal_id [int:
         * @return [void]
         */
        addEvent : function( item, internal_id ) {
            this.MetaText.Events.add( item, internal_id );
        },

        /**
         * @return [EventIterator]
         */
        getEventIterator : function() {
            return new org.kbinani.vsq.VsqTrack.EventIterator( this.MetaText.getEventList() );
        },

        /**
         * @param index [int]
         * @return [void]
         */
        removeEvent : function( index ) {
            this.MetaText.getEventList().removeAt( index );
        },

        /**
         * このインスタンスのコピーを作成します
         *
         * @return [object]
         */
        clone : function() {
            var res = new org.kbinani.vsq.VsqTrack();
            res.setName( this.getName() );
            if ( this.MetaText != null ) {
                res.MetaText = this.MetaText.clone();
            }
            res.Tag = this.Tag;
            return res;
        },

        /**
         * 歌詞の文字数を調べます
         * @return [int]
         */
        getLyricLength : function() {
            var counter = 0;
            for ( var i = 0; i < this.MetaText.getEventList().getCount(); i++ ) {
                if ( this.MetaText.getEventList().getElement( i ).ID.type == org.kbinani.vsq.VsqIDType.Anote ) {
                    counter++;
                }
            }
            return counter;
        },

        /**
         * Master Trackを構築
         * @param tempo [int]
         * @param numerator [int]
         * @param denominator [int]
         * @return [VsqTrack]
         */
        _init_3 : function( tempo, numerator, denominator ){
            this.MetaText = null;
        },

        /**
         * Master Trackでないトラックを構築。
         * @param name [string]
         * @param singer [string]
         * @return [VsqTrack]
         */
        _init_2a : function( name, singer ){
            this._initCor( name, singer );
        },

        /**
         * @param midi_event [Array<MidiEvent>]
         * @param encoding [string]
         */
        _init_2b : function( midi_event, encoding ){
            var track_name = "";

            var sw = null;
            sw = new org.kbinani.vsq.TextStream();
            var count = midi_event.length;
            var buffer = new Array(); // Vector<Integer>();
            for ( var i = 0; i < count; i++ ) {
                var item = midi_event[i];
                if ( item.firstByte == 0xff && item.data.length > 0 ) {
                    // meta textを抽出
                    var type = item.data[0];
                    if ( type == 0x01 || type == 0x03 ) {
                        if ( type == 0x01 ) {
                            var colon_count = 0;
                            for ( var j = 0; j < item.data.length - 1; j++ ) {
                                var d = item.data[j + 1];
                                if ( d == 0x3a ) {
                                    colon_count++;
                                    if ( colon_count <= 2 ) {
                                        continue;
                                    }
                                }
                                if ( colon_count < 2 ) {
                                    continue;
                                }
                                buffer.push( d );
                            }

                            var index_0x0a = org.kbinani.PortUtil.arrayIndexOf( buffer, 0x0a );
                            while ( index_0x0a >= 0 ) {
                                var cpy = new Array( index_0x0a );
                                for ( var j = 0; j < index_0x0a; j++ ) {
                                    cpy[j] = 0xff & buffer[0];
                                    buffer.shift();
                                }

                                var line = org.kbinani.Cp932.convertToUTF8( cpy );
//alert( "VsqTrack#_init_2b; line=" + line );
                                sw.writeLine( line );
                                buffer.shift();
                                index_0x0a = org.kbinani.PortUtil.arrayIndexOf( buffer, 0x0a );
                            }
                        } else {
                            for ( var j = 0; j < item.data.length - 1; j++ ) {
                                buffer.push( item.data[j + 1] );
                            }
                            var c = buffer.length;
                            var d = new Array( c );
                            for ( var j = 0; j < c; j++ ) {
                                d[j] = 0xff & buffer[j];
                            }
                            track_name = org.kbinani.Cp932.convertToUTF8( d );
                            buffer.splice( 0, buffer.length );
                        }
                    }
                } else {
                    continue;
                }
            }
            // oketa ketaoさんありがとう =>
            var remain = buffer.length;
            if ( remain > 0 ) {
                var cpy = new Array( remain );
                for ( var j = 0; j < remain; j++ ) {
                    cpy[j] = 0xff & buffer[j];
                }
                var line = org.kbinani.Cp932.convertToUTF8( cpy );
                sw.writeLine( line );
            }
            // <=
            //sw.rewind();
//alert( "VsqTrack#_init_2b; sw.toString()=" + sw.toString() );
            this.MetaText = new org.kbinani.vsq.VsqMetaText( sw );
            this.setName( track_name );
        },

        /**
         * デフォルトのコンストラクタ
         * @return [VsqTrack]
         */
        _init_0 : function(){
            this._intiCor( "Voice1", "Miku" );
        },

        /**
         * @param name [string]
         * @param singer [string]
         */
        _initCor : function( name, singer ){
            this.MetaText = new org.kbinani.vsq.VsqMetaText( name, singer );
        },
    };

}
