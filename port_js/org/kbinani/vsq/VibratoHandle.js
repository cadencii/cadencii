/*
 * VibratoHandle.js
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
if( org.kbinani.vsq == undefined ) org.kbiani.vsq = {};
if( org.kbinani.vsq.VibratoHandle == undefined ){

    org.kbinani.vsq.VibratoHandle = function(){
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
        
        this.articulation = org.kbinani.vsq.ArticulationType.Vibrato;
        this.startRate = 64;
        this.startDepth = 64;
        this.rateBP = new org.kbinani.vsq.VibratoBPList();
        this.depthBP = new org.kbinani.vsq.VibratoBPList();
    };

    org.kbinani.vsq.VibratoHandle.prototype = new org.kbinani.vsq.IconParameter();
    
    /**
     * @return [void]
     */
    org.kbinani.vsq.VibratoHandle.prototype.toString = function(){
        return getDisplayString();
    };

    /**
     * @return [VibratoBPList]
     */
    org.kbinani.vsq.VibratoHandle.prototype.getRateBP = function(){
        return this.rateBP;
    };

    /**
     * @param value [VibratoBPList]
     * @return [void]
     */
    org.kbinani.vsq.VibratoHandle.prototype.setRateBP = function( value ) {
        this.rateBP = value;
    };

    /**
     * @return [string]
     */
    org.kbinani.vsq.VibratoHandle.prototype.getCaption = function() {
        return this.caption;
    };

    /**
     * @param value [string]
     * @return [void]
     */
    org.kbinani.vsq.VibratoHandle.prototype.setCaption = function( value ) {
        this.caption = value;
    };

    /**
     * @return [int]
     */
    org.kbinani.vsq.VibratoHandle.prototype.getStartRate = function() {
        return this.startRate;
    };

    /**
     * @param value [int]
     * @return [void]
     */
    org.kbinani.vsq.VibratoHandle.prototype.setStartRate = function( value ) {
        this.startRate = value;
    };

    /**
     * @return [VibratoBPList]
     */
    org.kbinani.vsq.VibratoHandle.prototype.getDepthBP = function() {
        return this.depthBP;
    };

    /**
     * @param value [VibratoBPList]
     * @return [void]
     */
    org.kbinani.vsq.VibratoHandle.prototype.setDepthBP = function( value ) {
        this.depthBP = value;
    };

    /**
     * @return [int]
     */
    org.kbinani.vsq.VibratoHandle.prototype.getStartDepth = function() {
        return this.startDepth;
    };

    /**
     * @param value [int]
     * @return [void]
     */
    org.kbinani.vsq.VibratoHandle.prototype.setStartDepth = function( value ) {
        this.startDepth = value;
    };

    /**
     * @return [int]
     */
    org.kbinani.vsq.VibratoHandle.prototype.getLength = function() {
        return this.length;
    };

    /**
     * @param value [int]
     * @return [void]
     */
    org.kbinani.vsq.VibratoHandle.prototype.setLength = function( value ) {
        this.length = value;
    };

    /**
     * @return [string]
     */
    org.kbinani.vsq.VibratoHandle.prototype.getDisplayString = function() {
        return this.caption;
    };

    /**
     * @return [object]
     */
    org.kbinani.vsq.VibratoHandle.prototype.clone = function() {
        var result = new org.kbinani.vsq.VibratoHandle();
        result.Index = this.Index;
        result.IconID = this.IconID;
        result.IDS = this.IDS;
        result.Original = this.Original;
        result.setCaption( this.caption );
        result.setLength( getLength() );
        result.setStartDepth( this.startDepth );
        if ( this.depthBP != null ) {
            result.setDepthBP( this.depthBP.clone() );
        }
        result.setStartRate( this.startRate );
        if ( this.rateBP != null ) {
            result.setRateBP( this.rateBP.clone() );
        }
        return result;
    };

    /**
     * @return [VsqHandle]
     */
    org.kbinani.vsq.VibratoHandle.prototype.castToVsqHandle = function() {
        var ret = new org.kbinani.vsq.VsqHandle();
        ret.m_type = org.kbinani.vsq.VsqHandleType.Vibrato;
        ret.Index = this.Index;
        ret.IconID = this.IconID;
        ret.IDS = this.IDS;
        ret.Original = this.Original;
        ret.Caption = this.caption;
        ret.setLength( getLength() );
        ret.StartDepth = this.startDepth;
        ret.StartRate = this.startRate;
        ret.DepthBP = this.depthBP.clone();
        ret.RateBP = this.rateBP.clone();
        return ret;
    };

}
