/*
 * QuantizeModeUtil.js
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.cadencii == undefined ) org.kbinani.cadencii = {};
if( org.kbinani.cadencii.QuantizeModeUtil == undefined ){

    org.kbinani.cadencii.QuantizeModeUtil = {};
    
    /**
     * @param quantize_mode [int]
     * @return [string]
     */
    org.kbinani.cadencii.QuantizeModeUtil.getString = function( quantize_mode ) {
        if ( quantize_mode == org.kbinani.cadencii.QuantizeMode.off ) {
            return "Off";
        } else if ( quantize_mode == org.kbinani.cadencii.QuantizeMode.p4 ) {
            return "1/4";
        } else if ( quantize_mode == org.kbinani.cadencii.QuantizeMode.p8 ) {
            return "1/8";
        } else if ( quantize_mode == org.kbinani.cadencii.QuantizeMode.p16 ) {
            return "1/16";
        } else if ( quantize_mode == org.kbinani.cadencii.QuantizeMode.p32 ) {
            return "1/32";
        } else if ( quantize_mode == org.kbinani.cadencii.QuantizeMode.p64 ) {
            return "1/64";
        } else if ( quantize_mode == org.kbinani.cadencii.QuantizeMode.p128 ) {
            return "1/128";
        } else {
            return "";
        }
    };

    /**
     * クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
     * @param qm [int]
     * @param triplet [bool]
     * @return [int]
     */
    org.kbinani.cadencii.QuantizeModeUtil.getQuantizeClock = function( qm, triplet ) {
        var ret = 1;
        if ( qm == org.kbinani.cadencii.QuantizeMode.p4 ) {
            ret = 480;
        } else if ( qm == org.kbinani.cadencii.QuantizeMode.p8 ) {
            ret = 240;
        } else if ( qm == org.kbinani.cadencii.QuantizeMode.p16 ) {
            ret = 120;
        } else if ( qm == org.kbinani.cadencii.QuantizeMode.p32 ) {
            ret = 60;
        } else if ( qm == org.kbinani.cadencii.QuantizeMode.p64 ) {
            ret = 30;
        } else if ( qm == org.kbinani.cadencii.QuantizeMode.p128 ) {
            ret = 15;
        } else {
            return 1;
        }
        if ( triplet ) {
            ret = ret * 2 / 3;
        }
        return ret;
    };

}
