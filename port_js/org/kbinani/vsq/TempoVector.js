/*
 * TempoVector.js
 * Copyright (C) 2010 kbinani
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
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.TempoVector == undefined ){

    /// <summary>
    /// テンポ情報を格納したテーブル．
    /// </summary>
    org.kbinani.vsq.TempoVector = function(){
        this._array = new Array();
    };

    org.kbinani.vsq.TempoVector.gatetimePerQuater = 480;
    org.kbinani.vsq.TempoVector.baseTempo = 500000; 

    org.kbinani.vsq.TempoVector.prototype = {
        /**
         * データ点を時刻順に並べ替えます
         * @return [void]
         */
        sort : function(){
            this._array.sort( org.kbinani.vsq.TempoTableEntry.compare );
        },

        /**
         * データ点を追加します
         * @param value [TempoTableEntry]
         * @return [void]
         */
        add : function( value ){
            this._array.push( value );
        },

        /**
         * データ点を追加します
         * @param value [TempoTableEntry]
         * @return [void]
         */
        push : function( value ){
            this.add( value );
        },

        /**
         * テンポ・テーブルに登録されているデータ点の個数を調べます
         * @return [int]
         */
        size : function(){
            return this._array.length;
        },

        /**
         * 第index番目のデータ点を取得します
         * @param index [int]
         * @return [TempoTableEntry]
         */
        get : function( index ){
            return this._array[index];
        },

        /**
         * データ点を設定します
         * @param index [int]
         * @param value [TempoTableEntry]
         * @return [void]
         */
        set : function( index, value ){
            this._array[index] = value;
        },

        /**
         * @param time [double]
         * @return [double]
         */
        getClockFromSec : function( time ) {
            // timeにおけるテンポを取得
            var tempo = baseTempo;
            var base_clock = 0;
            var base_time = 0.0;
            var c = this._array.length;
            if ( c == 0 ) {
                tempo = org.kbinani.vsq.TempoVector.baseTempo;
                base_clock = 0;
                base_time = 0.0;
            } else if ( c == 1 ) {
                tempo = this._array[0].Tempo;
                base_clock = this._array[0].Clock;
                base_time = this._array[0].Time;
            } else {
                for ( var i = c - 1; i >= 0; i-- ) {
                    var item = this._array[i];
                    if ( item.Time < time ) {
                        return item.Clock + (time - item.Time) * org.kbinani.vsq.TempoVector.gatetimePerQuater * 1000000.0 / item.Tempo;
                    }
                }
            }
            var dt = time - base_time;
            return base_clock + dt * org.kbinani.vsq.TempopVector.gatetimePerQuater * 1000000.0 / tempo;
        },

        /**
         * @return [void]
         */
        updateTempoInfo : function() {
            var c = this._array.length;
            if ( c == 0 ) {
                this._array.push( new org.kbinani.vsq.TempoTableEntry( 0, org.kbinani.vsq.TempoVector.baseTempo, 0.0 ) );
            }
            this._array.sort( org.kbinani.vsq.TempoTableEntry.compare );
            var item0 = this._array[0];
            if ( item0.Clock != 0 ) {
                item0.Time = org.kbinani.vsq.TempoVector.baseTempo * item0.Clock / (org.kbinani.vsq.TempoVector.gatetimePerQuater * 1000000.0);
            } else {
                item0.Time = 0.0;
            }
            var prev_time = item0.Time;
            var prev_clock = item0.Clock;
            var prev_tempo = item0.Tempo;
            var inv_tpq_sec = 1.0 / (org.kbinani.vsq.TempoVector.gatetimePerQuater * 1000000.0);
            for ( var i = 1; i < c; i++ ) {
                var itemi = this._array[i];
                itemi.Time = prev_time + prev_tempo * (itemi.Clock - prev_clock) * inv_tpq_sec;
                prev_time = itemi.Time;
                prev_tempo = itemi.Tempo;
                prev_clock = itemi.Clock;
            }
        },

        /**
         * @param clock [double]
         * @return [double]
         */
        getSecFromClock : function( clock ) {
            var c = this._array.length;
            for ( var i = c - 1; i >= 0; i-- ) {
                var item = this._array[i];
                if ( item.Clock < clock ) {
                    var init = item.Time;
                    var dclock = clock - item.Clock;
                    var sec_per_clock1 = item.Tempo * 1e-6 / 480.0;
                    return init + dclock * sec_per_clock1;
                }
            }

            var sec_per_clock = org.kbinani.vsq.TempoVector.baseTempo * 1e-6 / 480.0;
            return clock * sec_per_clock;
        },
    };

}
