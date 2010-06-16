/*
* VsqMixerEntry.js
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
if( org.kbinani.vsq == undefined ) org.kbinanivsq = {};
if( org.kbinani.vsq.VsqMixerEntry == undefined ){

    /// <summary>
    /// VsqMixerのSlave要素に格納される各エントリ
    /// </summary>
    /// <summary>
    /// 各パラメータを指定したコンストラクタ
    /// </summary>
    /// <param name="feder">Feder値</param>
    /// <param name="panpot">Panpot値</param>
    /// <param name="mute">Mute値</param>
    /// <param name="solo">Solo値</param>
    org.kbinani.vsq.VsqMixerEntry = function( feder, panpot, mute, solo ){
        this.Feder = feder;
        this.Panpot = panpot;
        this.Mute = mute;
        this.Solo = solo;
    };

    org.kbinani.vsq.VsqMixerEntry.prototype = {
        _init_4 : function( feder, panpot, mute, solo ){
            this.Feder = feder;
            this.Panpot = panpot;
            this.Mute = mute;
            this.Solo = solo;
            return this;
        },

        clone : function() {
            var res = new org.kbinani.vsq.VsqMixerEntry( this.Feder, this.Panpot, this.Mute, this.Solo );
            return res;
        },
    };

}
