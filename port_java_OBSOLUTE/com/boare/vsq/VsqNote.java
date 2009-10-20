/*
 * VsqNote.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

/// <summary>
/// 音階を表現するためのクラス
/// </summary>
public class VsqNote {
    /// <summary>
    /// このインスタンスが表す音階のノート値
    /// </summary>
    public int value;
    private static final boolean[] _KEY_TYPE = new boolean[] { 
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
        true,
    };

    /// <summary>
    /// 音階のノート値からのコンストラクタ。
    /// </summary>
    /// <param name="note">この音階を初期化するためのノート値</param>
    public VsqNote( int note ) {
        value = note;
    }

    /// <summary>
    /// このインスタンスが表す音階が、ピアノの白鍵かどうかを返します
    /// </summary>
    public boolean isWhiteKey() {
        return isNoteWhiteKey( value );
    }

    /// <summary>
    /// 指定した音階が、ピアノの白鍵かどうかを返します
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    public static boolean isNoteWhiteKey( int note ) {
        if ( 0 <= note && note <= 127 ) {
            return _KEY_TYPE[note];
        } else {
            int odd = note % 12;
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
    }

    public static String getNoteString( int note ) {
        int odd = note % 12;
        int order = (note - odd) / 12 - 2;
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
    }

    public String toString() {
        return getNoteString( value );
    }
}
