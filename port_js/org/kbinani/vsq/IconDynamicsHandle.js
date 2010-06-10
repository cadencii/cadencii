/*
 * IconDynamicsHandle.cs
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
if( org.kbinani.vsq.IconDynamicsHandle == undefined ){

    org.kbinani.vsq.IconDynamicsHandle = function(){
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

    };

    /// <summary>
    /// 強弱記号の場合の、IconIDの最初の5文字。
    /// </summary>
    org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_DYNAFF = "$0501";
    /// <summary>
    /// クレッシェンドの場合の、IconIDの最初の5文字。
    /// </summary>
    org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_CRESCEND = "$0502";
    /// <summary>
    /// デクレッシェンドの場合の、IconIDの最初の5文字。
    /// </summary>
    org.kbinani.vsq.IconDynamicsHandle.ICONID_HEAD_DECRESCEND = "$0503";

    org.kbinani.vsq.IconDynamicsHandle.prototype = new org.kbinani.vsq.IconParameter();

    /**
     * このハンドルが強弱記号を表すものかどうかを表すブール値を取得します。
     *
     * @return [bool]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.isDynaffType = function() {
        if ( this.IconID != null ) {
            return this.IconID.indexOf( this.ICONID_HEAD_DYNAFF ) === 0;
        } else {
            return false;
        }
    };

    /**
     * このハンドルがクレッシェンドを表すものかどうかを表すブール値を取得します。
     *
     * @return [bool]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.isCrescendType = function() {
        if ( this.IconID != null ) {
            return this.IconID.indexOf( this.ICONID_HEAD_CRESCEND ) === 0;
        } else {
            return false;
        }
    };

    /**
     * このハンドルがデクレッシェンドを表すものかどうかを表すブール値を取得します。
     *
     * @return [bool]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.isDecrescendType = function() {
        if ( this.IconID != null ) {
            return this.IconID.indexOf( this.ICONID_HEAD_DECRESCEND ) === 0;
        } else {
            return false;
        }
    };

    /**
     * このインスタンスのコピーを作成します。
     *
     * @return [object]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.clone = function() {
        var ret = new org.kbinani.vsq.IconDynamicsHandle();
        ret.IconID = this.IconID;
        ret.IDS = this.IDS;
        ret.Original = this.Original;
        ret.setCaption( getCaption() );
        ret.setStartDyn( getStartDyn() );
        ret.setEndDyn( getEndDyn() );
        if ( this.dynBP != null ){
            ret.setDynBP( this.dynBP.clone() );
        }
        ret.setLength( getLength() );
        return ret;
    };

    /**
     * この強弱記号設定のインスタンスを、VsqHandleに型キャストします。
     *
     * @return [VsqHandle]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.castToVsqHandle = function() {
        var ret = new org.kbinani.vsq.VsqHandle();
        ret.m_type = org.kbinani.vsq.VsqHandleType.DynamicsHandle;
        ret.IconID = this.IconID;
        ret.IDS = this.IDS;
        ret.Original = this.Original;
        ret.Caption = getCaption();
        ret.DynBP = getDynBP();
        ret.EndDyn = getEndDyn();
        ret.setLength( getLength() );
        ret.StartDyn = getStartDyn();
        return ret;
    };

    /**
     * キャプションを取得します。
     * @return [string]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.getCaption = function() {
        return this.caption;
    };

    /**
     * キャプションを設定します。
     * @param value [string]
     * @return [void]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.setCaption = function( value ) {
        this.caption = value;
    };

    /**
     * ゲートタイム長さを取得します。
     * @return [int]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.getLength = function() {
        return this.length;
    };

    /**
     * ゲートタイム長さを設定します。
     * @param value [int]
     * @return [void]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.setLength = function( value ) {
        this.length = value;
    };

    /**
     * DYNの開始値を取得します。
     * @return [int]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.getStartDyn = function() {
        return this.startDyn;
    };

    /**
     * DYNの開始値を設定します。
     * @param value [int]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.setStartDyn = function( value ) {
        this.startDyn = value;
    };

    /**
     * DYNの終了値を取得します。
     * @return [int]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.getEndDyn = function() {
        return this.endDyn;
    };

    /**
     * DYNの終了値を設定します。
     * @param value [int]
     * @return [void]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.setEndDyn = function( value ) {
        this.endDyn = value;
    };

    /**
     * DYNカーブを表すリストを取得します。
     * @return [VibratoBPList]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.getDynBP = function() {
        return this.dynBP;
    };

    /**
     * DYNカーブを表すリストを設定します。
     * @param value [VibratoBPList]
     * @return [void]
     */
    org.kbinani.vsq.IconDynamicsHandle.prototype.setDynBP = function( value ) {
        this.dynBP = value;
    };

}
