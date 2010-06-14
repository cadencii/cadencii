/*
 * VsqCommon.js
 * Copyright (C) 2008-2010 kbinani
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
if( org.kbinani.vsq.VsqCommon == undefined ){

    /**
     * vsqファイルのメタテキストの[Common]セクションに記録される内容を取り扱う
     */

    /**
     * overload1
     * @param text_stream [TextStream]
     * @param last_line [ByRef<String>]
     *
     * overload2
     * @param name [String]
     * @param red [int]
     * @param green [int]
     * @param blue [int]
     * @param dynamics_mode [int]
     * @param play_mode [int]
     */
    org.kbinani.vsq.VsqCommon = function(){
        this.Version = "DSB301";
        this.Name = "Miku";
        this.Color = "179,181,123";
        /**
         * Dynamicsカーブを表示するモード(Expert)なら1、しない(Standard)なら0。
         */
        this.DynamicsMode = org.kbinani.vsq.DynamicsMode.Expert;
        /**
         * Play With Synthesisなら1、Play After Synthesiなら0、Offなら-1。
         */
        this.PlayMode = org.kbinani.vsq.PlayMode.PlayWithSynth;
        /**
         * PlayModeがOff(-1)にされる直前に，PlayAfterSynthかPlayWithSynthのどちらが指定されていたかを記憶しておく．
         */
        this.LastPlayMode = org.kbinani.vsq.PlayMode.PlayWithSynth;
        if( arguments.length == 2 ){
            this._init_2( arguments[0], arguments[1], arguments[2], arguments[3] );
        }else if( arguments.length == 6 ){
            this._init_6( arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5] );
        }
    };

    org.kbinani.vsq.VsqCommon.prototype = {
        /**
         * @param sr [TextStream]
         * @param last_line [ByRef<string>]
         * @return [void]
         */
        _init_2 : function( sr, last_line ){
            this.Version = "";
            this.Name = "";
            this.Color = "0,0,0";
            this.DynamicsMode = 0;
            this.PlayMode = 1;
            last_line.value = sr.readLine();
            while ( last_line.value.charAt( 0 ) != "[" ) {
                var spl = last_line.value.split( "=" );
                var search = spl[0];
                if ( search == "Version" ) {
                    this.Version = spl[1];
                } else if ( search == "Name" ) {
                    this.Name = spl[1];
                } else if ( search == "Color" ) {
                    this.Color = spl[1];
                } else if ( search == "DynamicsMode" ) {
                    this.DynamicsMode = parseInt( spl[1], 10 );
                } else if ( search == "PlayMode" ) {
                    this.PlayMode = parseInt( spl[1], 10 );
                }
                if ( !sr.ready() ) {
                    break;
                }
                last_line.value = sr.readLine();
            }
        },

        _init_6 : function( name, r, g, b, dynamics_mode, play_mode ){
            this.Version = "DSB301";
            this.Name = arguments[0];
            this.Color = arguments[1] + "," + arguments[2] + "," + arguments[3];
            this.DynamicsMode = arguments[4];
            this.PlayMode = arguments[5];
        },

        clone : function() {
            var spl = this.Color.split( "," );
            var r = parseInt( spl[0], 10 );
            var g = parseInt( spl[1], 10 );
            var b = parseInt( spl[2], 10 );
            var res = new VsqCommon( this.Name, r, g, b, this.DynamicsMode, this.PlayMode );
            res.Version = this.Version;
            res.LastPlayMode = this.LastPlayMode;
            return res;
        },

        /**
         * インスタンスの内容をテキストファイルに出力します
         * @param sw [ITextWriter] 出力先
         * @return [void]
         */
        write : function( sw ){
            sw.writeLine( "[Common]" );
            sw.writeLine( "Version=" + this.Version );
            sw.writeLine( "Name=" + this.Name );
            sw.writeLine( "Color=" + this.Color );
            sw.writeLine( "DynamicsMode=" + this.DynamicsMode );
            sw.writeLine( "PlayMode=" + this.PlayMode );
        },
    };

}
