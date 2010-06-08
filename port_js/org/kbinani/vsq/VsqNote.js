/*
 * VsqNote.js
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
if( org.kbinani.vsq.VsqNote == undefined ){
	
    /// <summary>
    /// 音階を表現するためのクラス
    /// </summary>
	org.kbinani.vsq.VsqNote = function( note ){
		this.Value = note;
	};

	org.kbinani.vsq.VsqNote.ALTER = new Array( 0, 1, 0, -1, 0, 0, 1, 0, 1, 0, -1, 0, 0 );
    org.kbinani.vsq.VsqNote._KEY_TYPE = new Array(
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true,
        false,
        true,
        true,
        false,
        true );

	/*
	 * @param note [int]
	 * @returns [String]
	 */
    org.kbinani.vsq.VsqNote.getNoteString = function( note ) {
        var odd = note % 12;
        var order = (note - odd) / 12 - 2;
        switch ( odd ) {
            case 0:
                return "C" + order;
            case 1:
                return "C#" + order;
            case 2:
                return "D" + order;
            case 3:
                return "Eb" + order;
            case 4:
                return "E" + order;
            case 5:
                return "F" + order;
            case 6:
                return "F#" + order;
            case 7:
                return "G" + order;
            case 8:
                return "G#" + order;
            case 9:
                return "A" + order;
            case 10:
                return "Bb" + order;
            case 11:
                return "B" + order;
            default:
                return "";
        }
    };

    /// <summary>
    /// 指定した音階が、ピアノの白鍵かどうかを返します
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    org.kbinani.vsq.VsqNote.isNoteWhiteKey = function( note ) {
        if ( 0 <= note && note <= 127 ) {
            return org.kbinani.vsq.VsqNote._KEY_TYPE[note];
        } else {
            var odd = note % 12;
            switch ( odd ) {
                case 1:
                case 3:
                case 6:
                case 8:
                case 10:
                    return false;
                default:
                    return true;
            }
        }
    };

    /// <summary>
    /// C#4なら+1, C4なら0, Cb4なら-1
    /// </summary>
    /// <param name="note"></param>
    /// <returns>[int]</returns>
    org.kbinani.vsq.VsqNote.getNoteAlter = function( note ) {
        return org.kbinani.vsq.VsqNote.ALTER[note % 12];
    };

    /// <summary>
    /// ノート#のオクターブ部分の表記を調べます．
    /// 例：C4 => 4, D#4 => 4
    /// </summary>
    /// <param name="note"></param>
    /// <returns>[int]</returns>
    org.kbinani.vsq.VsqNote.getNoteOctave = function( note ) {
        var odd = note % 12;
        return (note - odd) / 12 - 2;
    };

    /// <summary>
    /// ノートのオクターブ，変化記号を除いた部分の文字列表記を調べます．
    /// 例：C4 => "C", D#4 => "D"
    /// </summary>
    /// <param name="note">[int]</param>
    /// <returns>[String]</returns>
    org.kbinani.vsq.VsqNote.getNoteStringBase = function( note ) {
        var odd = note % 12;
        switch ( odd ) {
            case 0:
            case 1:
                return "C";
            case 2:
                return "D";
            case 3:
            case 4:
                return "E";
            case 5:
            case 6:
                return "F";
            case 7:
            case 8:
                return "G";
            case 9:
                return "A";
            case 10:
            case 11:
                return "B";
            default:
                return "";
        }
    };

	org.kbinani.vsq.VsqNote.prototype = {
        /// <summary>
        /// このインスタンスが表す音階が、ピアノの白鍵かどうかを返します
        /// </summary>
        isWhiteKey : function() {
            return isNoteWhiteKey( this.Value );
        },

        toString : function() {
            return getNoteString( this.Value );
        },
    }

}
