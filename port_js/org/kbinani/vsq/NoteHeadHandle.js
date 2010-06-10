/*
 * NoteHeadHandle.js
 * Copyright (C) 2009-2010 kbinani
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
if( org.kbinani.vsq.NoteHeadHandle == undefined ){

    org.kbinani.vsq.NoteHeadHandle = function(){
        // copy of IconParameter.ctor =>
        /// <summary>
        /// アイコン設定の種類
        /// [ArticulationType]
        /// </summary>
        this.articulation = org.kbinani.vsq.ArticulationType.Dynaff;
        /// <summary>
        /// アイコンのボタンに使用される画像ファイルへの相対パス
        /// </summary>
        this.button = "";
        /// <summary>
        /// キャプション
        /// </summary>
        this.caption = "";

        /// <summary>
        /// ゲートタイム長さ
        /// </summary>
        this.length = 0;
        /// <summary>
        /// ビブラート深さの開始値
        /// </summary>
        this.startDepth = 64;
        /// <summary>
        /// ビブラート深さの終了値
        /// </summary>
        this.endDepth = 64;
        /// <summary>
        /// ビブラート速さの開始値
        /// </summary>
        this.startRate = 64;
        /// <summary>
        /// ビブラート速さの終了値
        /// </summary>
        this.endRate = 64;
        this.startDyn = 64;
        this.endDyn = 64;
        this.duration = 1;
        this.depth = 64;
        this.dynBP = null;
        this.depthBP = null;
        this.rateBP = null;
        this.buttonImageFullPath = "";
        // <=

        this.articulation = org.kbinani.vsq.ArticulationType.NoteAttack;
        this.Index = 0;
        this.IconID = "";
        this.IDS = "";
        this.Original = 0;
        if( arguments.length >= 3 ){
            this.IDS = arguments[0];
            this.IconID = arguments[1];
            this.Index = arguments[2];
        }
    };

    org.kbinani.vsq.NoteHeadHandle.prototype = new org.kbinani.vsq.IconParameter();

    /**
     * @return [String]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.toString = function() {
        return getDisplayString();
    };

    /**
     * @return [int]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.getDepth = function() {
        return this.depth;
    };

    /**
     * @param value [int]
     * @return [void]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.setDepth = function( value ) {
        this.depth = value;
    };

    /**
     * @return [int]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.getDuration = function() {
        return this.duration;
    };

    /**
     * @param value [int]
     * @return [void]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.setDuration = function( value ) {
        this.duration = value;
    };

    /**
     * @return [String]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.getCaption = function() {
        return this.caption;
    };

    /**
     * @param value [String]
     * @return [void]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.setCaption = function( value ) {
        this.caption = value;
    };

    /**
     * @return [int]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.getLength = function() {
        return this.length;
    };

    /**
     * @param value [int]
     * @return [void]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.setLength = function( value ) {
        this.length = value;
    };

    /**
     * @return [String]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.getDisplayString = function() {
        return this.IDS + this.caption;
    };

    /**
     * @return [object]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.clone = function() {
        var result = new org.kbinani.vsq.NoteHeadHandle();
        result.Index = this.Index;
        result.IconID = this.IconID;
        result.IDS = this.IDS;
        result.Original = this.Original;
        result.setCaption( getCaption() );
        result.setLength( getLength() );
        result.setDuration( getDuration() );
        result.setDepth( getDepth() );
        return result;
    };

    /**
     * @return [VsqHandle]
     */
    org.kbinani.vsq.NoteHeadHandle.prototype.castToVsqHandle = function() {
        var ret = new VsqHandle();
        ret.m_type = org.kbinani.vsq.VsqHandleType.NoteHeadHandle;
        ret.Index = this.Index;
        ret.IconID = this.IconID;
        ret.IDS = this.IDS;
        ret.Original = this.Original;
        ret.Caption = getCaption();
        ret.setLength( getLength() );
        ret.Duration = getDuration();
        ret.Depth = getDepth();
        return ret;
    };

}
