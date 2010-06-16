/*
* VsqMaster.js
* Copyright (C) 2008-2010 kbinani
*
* This file is part of org.kbinani.vsq.
*
* Boare.Lib.Vsq is free software; you can redistribute it and/or
* modify it under the terms of the GPLv3 License.
*
* Boare.Lib.Vsq is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.VsqMaster == undefined ){

    /// <summary>
    /// vsqファイルのメタテキストの[Master]に記録される内容を取り扱う
    /// </summary>

    org.kbinani.vsq.VsqMaster = function(){
        this.PreMeasure = 1;
        if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }else if( arguments.length == 2 ){
            this._init_2( arguments[0], arguments[1] );
        }
    };

    org.kbinani.vsq.VsqMaster.prototype = {
        /**
         *
         */
        _init_1 : function( pre_measure ){
            this.PreMeasure = pre_measure;
        },

        /**
         * @param sr [TextStream]
         * @param last_line [ByRef<string>]
         */
        _init_2 : function( sr, last_line ){
            this.PreMeasure = 0;
            var spl;
            last_line.value = sr.readLine();
            while ( last_line.value.indexOf( "[" ) != 0 ) {
                spl = last_line.value.split( "=" );
                if ( spl[0] == "PreMeasure" ) {
                    this.PreMeasure = parseInt( spl[1], 10 );
                }
                if ( !sr.ready() ) {
                    break;
                }
                last_line.value = sr.readLine();
            }
        },

        clone : function() {
            var res = new org.kbinani.vsq.VsqMaster( this.PreMeasure );
            return res;
        },

        /**
         * インスタンスの内容をテキストファイルに出力します
         * @param sw [ITextWriter] 出力先
         */
        write : function( sw ){
            sw.writeLine( "[Master]" );
            sw.writeLine( "PreMeasure=" + this.PreMeasure );
        },
    };

}
