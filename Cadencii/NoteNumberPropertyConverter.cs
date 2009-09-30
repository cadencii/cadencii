/*
 * NoteNumberPropertyConverter.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using System.Globalization;

using Boare.Lib.Vsq;

namespace Boare.Cadencii {

    using boolean = Boolean;

    public class NoteNumberPropertyConverter : TypeConverter {
        public override boolean CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( NoteNumberProperty ) ) {
                return true;
            }
            return base.CanConvertTo( context, destinationType );
        }

        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType ) {
            if ( destinationType == typeof( String ) && value is NoteNumberProperty ) {
                NoteNumberProperty obj = (NoteNumberProperty)value;
                String ret = getNoteString( obj.noteNumber );
                return ret;
            }
            return base.ConvertTo( context, culture, value, destinationType );
        }

        public override boolean CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {
            if ( sourceType == typeof( String ) ) {
                return true;
            }
            return base.CanConvertFrom( context, sourceType );
        }

        public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value ) {
            if ( value is String ) {
                NoteNumberProperty obj = new NoteNumberProperty();
                obj.noteNumber = NoteNumberPropertyConverter.parse( (String)value );
                return obj;
            }
            return base.ConvertFrom( context, culture, value );
        }

        private static String getNoteString( int note ) {
            String[] jp = new String[]{ "ハ", "嬰ハ", "ニ", "変ホ", "ホ", "ヘ", "嬰へ", "ト", "嬰ト", "イ", "変ロ", "ロ" };
            String[] jpfixed = new String[] { "ド", "ド#", "レ", "ミb", "ミ", "ファ", "ファ#", "ソ", "ソ#", "ラ", "シb", "シ", };
            String[] de = { "C", "Cis", "D", "Es", "E", "F", "Fis", "G", "Gis", "A", "Hes", "H" };
            if ( AppManager.editorConfig != null ) {
                int odd = note % 12;
                int order = (note - odd) / 12 - 2;
                switch ( AppManager.editorConfig.PropertyWindowStatus.LastUsedNoteNumberExpression ) {
                    case NoteNumberExpressionType.Numeric:
                        return note + "";
                    case NoteNumberExpressionType.International:
                        return VsqNote.getNoteString( note );
                    case NoteNumberExpressionType.Japanese:
                        return jp[odd] + order;
                    case NoteNumberExpressionType.JapaneseFixedDo:
                        return jpfixed[odd] + order;
                    case NoteNumberExpressionType.Deutsche:
                        return de[odd] + order;
                }
            } else {
                return VsqNote.getNoteString( note );
            }
            return "";
        }

        public static int parse( String value ) {
            if ( value.Equals( "" ) ) {
                return 60;
            }

            value = value.ToUpper();
            try {
                int draft_note_number = int.Parse( value );
                if ( AppManager.editorConfig != null ) {
                    AppManager.editorConfig.PropertyWindowStatus.LastUsedNoteNumberExpression = NoteNumberExpressionType.Numeric;
                }
                return draft_note_number;
            } catch {
            }

            int scale = 3;
            int offset = 0;
            boolean doubled = false;
            int odd = 0;
            boolean first = true;
            NoteNumberExpressionType exp_type = NoteNumberExpressionType.International;
            while ( true ) {
                int trim = 1;

                if ( value.StartsWith( "AS" ) ){
                    offset = -1;
                    odd = 9;
                    trim = 2;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if ( value.StartsWith( "ASAS" ) || value.StartsWith( "ASES" ) ){
                    offset = -1;
                    doubled = true;
                    odd = 9;
                    trim = 4;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if ( value.StartsWith( "ISIS" ) ){
                    offset = 1;
                    doubled = true;
                    trim = 4;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if ( value.StartsWith( "IS" ) ){
                    offset = 1;
                    trim = 2;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if ( value.StartsWith( "ESES" ) ){
                    if ( first ) {
                        odd = 4;
                    }
                    offset = -1;
                    doubled = true;
                    trim = 4;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if ( value.StartsWith( "ES" ) ){
                    if ( first ) {
                        offset = -1;
                        odd = 4;
                    } else {
                        offset = -1;
                    }
                    trim = 2;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if ( value.StartsWith( "嬰" ) ) {
                    offset = 1;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "変" ) ) {
                    offset = -1;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "重" ) ) {
                    doubled = true;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "C" ) ){
                    odd = 0;
                    exp_type = NoteNumberExpressionType.International;
                } else if ( value.StartsWith( "ド" ) || value.StartsWith( "ど" ) ){
                    odd = 0;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if ( value.StartsWith( "は" ) || value.StartsWith( "ハ" ) || value.StartsWith( "ﾊ" ) ) {
                    odd = 0;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "ﾄﾞ" ) ){
                    odd = 0;
                    trim = 2;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if ( value.StartsWith( "D" ) ){
                    odd = 2;
                    exp_type = NoteNumberExpressionType.International;
                } else if ( value.StartsWith( "レ" ) || value.StartsWith( "れ" ) || value.StartsWith( "ﾚ" ) ){
                    odd = 2;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if ( value.StartsWith( "に" ) || value.StartsWith( "ニ" ) || value.StartsWith( "ﾆ" ) ) {
                    odd = 2;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "E" ) ){
                    odd = 4;
                    exp_type = NoteNumberExpressionType.International;
                } else if ( value.StartsWith( "ミ" ) || value.StartsWith( "み" ) || value.StartsWith( "ﾐ" ) ){
                    odd = 4;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith( "ほ" ) || value.StartsWith( "ホ" ) || value.StartsWith( "ﾎ" ) ) {
                    odd = 4;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "F" ) ){
                    odd = 5;
                    exp_type = NoteNumberExpressionType.International;
                } else if ( value.StartsWith( "ヘ" ) || value.StartsWith( "へ" ) || value.StartsWith( "ﾍ" ) ){
                    odd = 5;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "ファ" ) || value.StartsWith( "ふぁ" ) || value.StartsWith( "ﾌｧ" ) ) {
                    odd = 5;
                    trim = 2;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if ( value.StartsWith( "G" ) ){
                    odd = 7;
                    exp_type = NoteNumberExpressionType.International;
                } else if ( value.StartsWith( "ソ" ) || value.StartsWith( "そ" ) || value.StartsWith( "ｿ" ) ){
                    odd = 7;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if ( value.StartsWith( "と" ) || value.StartsWith( "ト" ) || value.StartsWith( "ﾄ" ) ) {
                    odd = 7;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "A" ) ){
                    odd = 9;
                    exp_type = NoteNumberExpressionType.International;
                } else if ( value.StartsWith( "ラ" ) || value.StartsWith( "ら" ) || value.StartsWith( "ﾗ" ) ){
                    odd = 9;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if ( value.StartsWith( "い" ) || value.StartsWith( "イ" ) || value.StartsWith( "ｲ" ) ) {
                    odd = 9;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "H" ) ){
                    odd = 11;
                    exp_type = NoteNumberExpressionType.International;
                } else if ( value.StartsWith( "シ" ) || value.StartsWith( "し" ) || value.StartsWith( "ｼ" ) ){
                    odd = 11;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if ( value.StartsWith( "ろ" ) || value.StartsWith( "ロ" ) || value.StartsWith( "ﾛ" ) ) {
                    odd = 11;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if ( value.StartsWith( "B" ) ) {
                    if ( first ) {
                        odd = 11;
                        exp_type = NoteNumberExpressionType.International;
                    } else {
                        offset = -1;
                    }
                } else if ( value.StartsWith( "#" ) || value.StartsWith( "♯" ) || value.StartsWith( "＃" ) ) {
                    offset = 1;
                } else if ( value.StartsWith( "♭" ) ) {
                    offset = -1;
                }
                first = false;
                int len = value.Length;
                if ( len == trim ) {
                    break;
                }

                value = value.Substring( trim );
                int draft_scale;
                if ( int.TryParse( value, out draft_scale ) ) {
                    scale = draft_scale;
                    break;
                }
            }
            if ( AppManager.editorConfig != null ) {
                if ( exp_type                                             == NoteNumberExpressionType.International && 
                     AppManager.editorConfig.PropertyWindowStatus.LastUsedNoteNumberExpression == NoteNumberExpressionType.Deutsche ) {
                    // do nothing
                } else {
                    AppManager.editorConfig.PropertyWindowStatus.LastUsedNoteNumberExpression = exp_type;
                }
            }
            return 12 * scale + 2 * 12 + odd + offset * (doubled ? 2 : 1);
        }
    }

}
