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
        init : function(){
        },

        /**
         * @param time [double]
         * @return [double]
         */
        getClockFromSec : function( time ) {
            // timeにおけるテンポを取得
            var tempo = baseTempo;
            var base_clock = 0;
            var base_time = 0f;
            var c = _array.length;
            if ( c == 0 ) {
                tempo = org.kbinani.TempoVector.baseTempo;
                base_clock = 0;
                base_time = 0.0;
            } else if ( c == 1 ) {
                tempo = this._array[0].Tempo;
                base_clock = this._array[0].Clock;
                base_time = this._array[0].Time;
            } else {
                for ( var i = c - 1; i >= 0; i-- ) {
                    var item = _array[i];
                    if ( item.Time < time ) {
                        return item.Clock + (time - item.Time) * this.gatetimePerQuater * 1000000.0 / item.Tempo;
                    }
                }
            }
            var dt = time - base_time;
            return base_clock + dt * this.gatetimePerQuater * 1000000.0 / tempo;
        },

        /**
         * @return [void]
         */
        updateTempoInfo : function() {
            var c = this._array.length;
            if ( c == 0 ) {
                _array.push( new org.kbinani.vsq.TempoTableEntry().init( 0, this.baseTempo, 0.0 ) );
            }
            Collections.sort( this );
            TempoTableEntry item0 = get( 0 );
            if ( item0.Clock != 0 ) {
                item0.Time = (double)baseTempo * (double)item0.Clock / (gatetimePerQuater * 1000000.0);
            } else {
                item0.Time = 0.0;
            }
            double prev_time = item0.Time;
            int prev_clock = item0.Clock;
            int prev_tempo = item0.Tempo;
            double inv_tpq_sec = 1.0 / (gatetimePerQuater * 1000000.0);
            for ( int i = 1; i < c; i++ ) {
                TempoTableEntry itemi = get( i );
                itemi.Time = prev_time + (double)prev_tempo * (double)(itemi.Clock - prev_clock) * inv_tpq_sec;
                prev_time = itemi.Time;
                prev_tempo = itemi.Tempo;
                prev_clock = itemi.Clock;
            }
        }

        public double getSecFromClock( double clock ) {
            int c = size();
            for ( int i = c - 1; i >= 0; i-- ) {
                TempoTableEntry item = get( i );
                if ( item.Clock < clock ) {
                    double init = item.Time;
                    double dclock = clock - item.Clock;
                    double sec_per_clock1 = item.Tempo * 1e-6 / 480.0;
                    return init + dclock * sec_per_clock1;
                }
            }

            double sec_per_clock = baseTempo * 1e-6 / 480.0;
            return clock * sec_per_clock;
        }
    }

#if !JAVA
}
#endif
