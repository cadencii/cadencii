/*
 * DynamicsMode.js
 * Copyright (C) 2010 kbinani
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
if( org.kbinani.vsq.DynamicsMode == undefined ){

    /**
     * VOCALOID1における、ダイナミクスモードを表す定数を格納するためのクラスです。
     */
    org.kbinani.vsq.DynamicsMode = new function(){
    };

    /**
     * デフォルトのダイナミクスモードです。DYNカーブが非表示になるモードです。
     */
    org.kbinani.vsq.DynamicsMode.Standard = 0;
    /**
     * エキスパートモードです。DYNカーブが表示されます。
     */
    org.kbinani.vsq.DynamicsMode.Expert = 1;

}
