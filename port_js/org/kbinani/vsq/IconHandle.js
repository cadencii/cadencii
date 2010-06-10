/*
 * IconHandle.js
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
if( org.kbinani.vsq.IconHandle == undefined ){

    /// <summary>
    /// 歌手設定を表します。
    /// </summary>

    /// <summary>
    /// 新しい歌手設定のインスタンスを初期化します。
    /// </summary>
    org.kbinani.vsq.IconHandle = function(){
        /// <summary>
        /// キャプション。
        /// </summary>
        this.Caption = "";
        /// <summary>
        /// この歌手設定を一意に識別するためのIDです。
        /// </summary>
        this.IconID = "";
        /// <summary>
        /// ユーザ・フレンドリー名。
        /// このフィールドの値は、他の歌手設定のユーザ・フレンドリー名と重複する場合があります。
        /// </summary>
        this.IDS = "";
        this.Index = 0;
        /// <summary>
        /// ゲートタイム長さ。
        /// </summary>
        this.Length = 0;
        this.Original = 0;
        this.Program = 0;
        this.Language = 0;
    };

    org.kbinani.vsq.IconHandle.ptototype = {
        /**
         * ゲートタイム長さを取得します。
         *
         * @return [int]
         */
        getLength : function() {
            return this.Length;
        },

        /**
         * ゲートタイム長さを設定します。
         *
         * @param value [int]
         * @return [void]
         */
        setLength : function( value ) {
            this.Length = value;
        },

        /**
         * このインスタンスと、指定された歌手変更のインスタンスが等しいかどうかを判定します。
         *
         * @param item [IconHandle] 比較対象の歌手変更。
         * @returns [bool] このインスタンスと、比較対象の歌手変更が等しければtrue、そうでなければfalseを返します。
         */
        equals : function( item ) {
            if ( item == null ) {
                return false;
            } else {
                return this.IconID === item.IconID;
            }
        },

        /**
         * このインスタンスのコピーを作成します。
         *
         * @return [object]
         */
        clone : function() {
            var ret = new org.kbinani.vsq.IconHandle();
            ret.Caption = this.Caption;
            ret.IconID = this.IconID;
            ret.IDS = this.IDS;
            ret.Index = this.Index;
            ret.Language = this.Language;
            ret.setLength( this.Length );
            ret.Original = this.Original;
            ret.Program = this.Program;
            return ret;
        },

        /**
         * この歌手設定のインスタンスを、VsqHandleに型キャストします。
         *
         * @return [VsqHandle]
         */
        castToVsqHandle : function() {
            var ret = new org.kbinani.vsq.VsqHandle();
            ret.m_type = org.kbinani.vsq.VsqHandleType.Singer;
            ret.Caption = this.Caption;
            ret.IconID = this.IconID;
            ret.IDS = this.IDS;
            ret.Index = this.Index;
            ret.Language = this.Language;
            ret.setLength( this.Length );
            ret.Program = this.Program;
            return ret;
        },
    };

}
