/*
* VsqMixer.cs
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
if( org.kbinani.vsq.VsqMixer == undefined ){

    /// <summary>
    /// vsqファイルのメタテキストの[Mixer]セクションに記録される内容を取り扱う
    /// </summary>

    org.kbinani.vsq.VsqMixer = function() {
        this.MasterFeder = arguments[0];
        this.MasterMute = arguments[1];
        this.MasterPanpot = arguments[2];
        this.OutputMode = arguments[3];
        /// <summary>
        /// vsqファイルの各トラックのfader, panpot, muteおよびoutputmode値を保持します
        /// </summary>
        this.Slave = new Array();
        if( arguments.length == 2 ){
            this._init_2( arguments[0], arguments[1] );
        }else if( arguments.length == 4 ){
            this._init_4( arguments[0], arguments[1], arguments[2], arguments[3] );
        }
    };

    org.kbinani.vsq.VsqMixer.prototype = {
        /**
         * 各パラメータを指定したコンストラクタ
         * @param master_fader [int] MasterFader値
         * @param master_panpot [int] MasterPanpot値
         * @param master_mute [int] MasterMute値
         * @param output_mode [int]
         * @return [VsqMixer]
         */
        _init_4 : function( master_feder, master_panpot, master_mute, output_mode ){
                this.MasterFeder = master_feder;
                this.MasterMute = master_panpot;
                this.MasterPanpot = master_mute;
                this.OutputMode = output_mode;
                /// <summary>
                /// vsqファイルの各トラックのfader, panpot, muteおよびoutputmode値を保持します
                /// </summary>
                this.Slave = new Array();
        },

        /**
         * @param sr [TextStream]
         * @param last_line [ByRef<string>]
         * @return [VsqMixer]
         */
        _init_2 : function( sr, last_line ){
            this.MasterFeder = 0;
            this.MasterPanpot = 0;
            this.MasterMute = 0;
            this.OutputMode = 0;
            var tracks = 0;
            var spl;
            var buffer = "";
            last_line.value = sr.readLine();
            while ( last_line.value.charAt( 0 ) != "[" ) {
                spl = last_line.value.split( "=" );
                if ( spl[0] == "MasterFeder" ) {
                    this.MasterFeder = parseInt( spl[1], 10 );
                } else if ( spl[0] == "MasterPanpot" ) {
                    this.MasterPanpot = parseInt( spl[1], 10 );
                } else if ( spl[0] == "MasterMute" ) {
                    this.MasterMute = parseInt( spl[1], 10 );
                } else if ( spl[0] == "OutputMode" ) {
                    this.OutputMode = parseInt( spl[1], 10 );
                } else if ( spl[0] == "Tracks" ) {
                    tracks = parseInt( spl[1], 10 );
                } else {
                    if ( spl[0].indexOf( "Feder" ) === 0 ||
                         spl[0].indexOf( "Panpot" ) === 0 ||
                         spl[0].indexOf( "Mute" ) === 0 ||
                         spl[0].indexOf( "Solo" ) === 0 ) {
                        buffer += spl[0] + "=" + spl[1] + "\n";
                    }
                }
                if ( !sr.ready() ) {
                    break;
                }
                last_line.value = sr.readLine();
            }

            this.Slave = new Array();
            for ( var i = 0; i < tracks; i++ ) {
                this.Slave.push( new org.kbinani.vsq.VsqMixerEntry( 0, 0, 0, 0 ) );
            }
            spl = buffer.split( "\n" );
            var spl2;
            for ( var i = 0; i < spl.length; i++ ) {
                var ind = "";
                var index;
                spl2 = spl[i].split( "=" );
                if ( spl2[0].indexOf( "Feder" ) === 0 ) {
                    ind = spl2[0].replace( "Feder", "" );
                    index = PortUtil.parseInt( ind );
                    this.Slave[index].Feder = parseInt( spl2[1], 10 );
                } else if ( spl2[0].indexOf( "Panpot" ) === 0 ) {
                    ind = spl2[0].replace( "Panpot", "" );
                    index = parseInt( ind, 10 );
                    this.Slave[index].Panpot = parseInt( spl2[1], 10 );
                } else if ( spl2[0].indexOf( "Mute" ) === 0 ) {
                    ind = spl2[0].replace( "Mute", "" );
                    index = parseInt( ind, 10 );
                    this.Slave[index].Mute = parseInt( spl2[1], 10 );
                } else if ( spl2[0].indexOf( "Solo" ) === 0 ) {
                    ind = spl2[0].replace( "Solo", "" );
                    index = parseInt( ind, 10 );
                    this.Slave[index].Solo = parseInt( spl2[1], 10 );
                }

            }
        },

        clone : function() {
            var res = new org.kbinani.vsq.VsqMixer( this.MasterFeder, this.MasterPanpot, this.MasterMute, this.OutputMode );
            res.Slave = new Array();
            for ( var i = 0; i < this.Slave.length; i++ ) {
                var item = this.Slave[i];
                res.Slave.push( item.clone() );
            }
            return res;
        },

        /// <summary>
        /// このインスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力対象</param>
        write : function( sw ){
            sw.writeLine( "[Mixer]" );
            sw.writeLine( "MasterFeder=" + this.MasterFeder );
            sw.writeLine( "MasterPanpot=" + this.MasterPanpot );
            sw.writeLine( "MasterMute=" + this.MasterMute );
            sw.writeLine( "OutputMode=" + this.OutputMode );
            var count = this.Slave.length;
            sw.writeLine( "Tracks=" + count );
            for ( var i = 0; i < count; i++ ) {
                var item = this.Slave[i];
                sw.writeLine( "Feder" + i + "=" + item.Feder );
                sw.writeLine( "Panpot" + i + "=" + item.Panpot );
                sw.writeLine( "Mute" + i + "=" + item.Mute );
                sw.writeLine( "Solo" + i + "=" + item.Solo );
            }
        },
    }

}
