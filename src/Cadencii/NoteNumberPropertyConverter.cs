#if ENABLE_PROPERTY
/*
 * NoteNumberPropertyConverter.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using System.Globalization;
using cadencii;
using cadencii.vsq;



namespace cadencii
{

    public class NoteNumberPropertyConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(NoteNumberProperty)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is NoteNumberProperty) {
                return convertTo((NoteNumberProperty)value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        {
            if (value is string) {
                return convertFrom((string)value);
            } else {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public string convertTo(Object value)
        {
            if (value == null) {
                return "";
            } else if (value is NoteNumberProperty) {
                NoteNumberProperty nnp = (NoteNumberProperty)value;
                string ret = getNoteString(nnp.noteNumber);
                return ret;
            } else {
                return "";
            }
        }

        public NoteNumberProperty convertFrom(string value)
        {
            NoteNumberProperty obj = new NoteNumberProperty();
            obj.noteNumber = NoteNumberPropertyConverter.parse(value);
            return obj;
        }

        private static string getNoteString(int note)
        {
            string[] jp = new string[] { "ハ", "嬰ハ", "ニ", "変ホ", "ホ", "ヘ", "嬰へ", "ト", "嬰ト", "イ", "変ロ", "ロ" };
            string[] jpfixed = new string[] { "ド", "ド#", "レ", "ミb", "ミ", "ファ", "ファ#", "ソ", "ソ#", "ラ", "シb", "シ", };
            string[] de = { "C", "Cis", "D", "Es", "E", "F", "Fis", "G", "Gis", "A", "Hes", "H" };
            if (AppManager.editorConfig != null) {
                int odd = note % 12;
                int order = (note - odd) / 12 - 2;
                NoteNumberExpressionType exp_type = AppManager.editorConfig.PropertyWindowStatus.LastUsedNoteNumberExpression;
                if (exp_type == NoteNumberExpressionType.Numeric) {
                    return note + "";
                } else if (exp_type == NoteNumberExpressionType.International) {
                    return VsqNote.getNoteString(note);
                } else if (exp_type == NoteNumberExpressionType.Japanese) {
                    return jp[odd] + order;
                } else if (exp_type == NoteNumberExpressionType.JapaneseFixedDo) {
                    return jpfixed[odd] + order;
                } else if (exp_type == NoteNumberExpressionType.Deutsche) {
                    return de[odd] + order;
                }
            } else {
                return VsqNote.getNoteString(note);
            }
            return "";
        }

        public static int parse(string value)
        {
            if (value.Equals("")) {
                return 60;
            }

            value = value.ToUpper();
            try {
                int draft_note_number = int.Parse(value);
                if (AppManager.editorConfig != null) {
                    AppManager.editorConfig.PropertyWindowStatus.LastUsedNoteNumberExpression = NoteNumberExpressionType.Numeric;
                }
                return draft_note_number;
            } catch (Exception ex) {
            }

            int scale = 3;
            int offset = 0;
            bool doubled = false;
            int odd = 0;
            bool first = true;
            NoteNumberExpressionType exp_type = NoteNumberExpressionType.International;
            while (true) {
                int trim = 1;

                if (value.StartsWith("AS")) {
                    offset = -1;
                    odd = 9;
                    trim = 2;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if (value.StartsWith("ASAS") || value.StartsWith("ASES")) {
                    offset = -1;
                    doubled = true;
                    odd = 9;
                    trim = 4;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if (value.StartsWith("ISIS")) {
                    offset = 1;
                    doubled = true;
                    trim = 4;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if (value.StartsWith("IS")) {
                    offset = 1;
                    trim = 2;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if (value.StartsWith("ESES")) {
                    if (first) {
                        odd = 4;
                    }
                    offset = -1;
                    doubled = true;
                    trim = 4;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if (value.StartsWith("ES")) {
                    if (first) {
                        offset = -1;
                        odd = 4;
                    } else {
                        offset = -1;
                    }
                    trim = 2;
                    exp_type = NoteNumberExpressionType.Deutsche;
                } else if (value.StartsWith("嬰")) {
                    offset = 1;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("変")) {
                    offset = -1;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("重")) {
                    doubled = true;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("C")) {
                    odd = 0;
                    exp_type = NoteNumberExpressionType.International;
                } else if (value.StartsWith("ド") || value.StartsWith("ど")) {
                    odd = 0;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith("は") || value.StartsWith("ハ") || value.StartsWith("ﾊ")) {
                    odd = 0;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("ﾄﾞ")) {
                    odd = 0;
                    trim = 2;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith("D")) {
                    odd = 2;
                    exp_type = NoteNumberExpressionType.International;
                } else if (value.StartsWith("レ") || value.StartsWith("れ") || value.StartsWith("ﾚ")) {
                    odd = 2;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith("に") || value.StartsWith("ニ") || value.StartsWith("ﾆ")) {
                    odd = 2;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("E")) {
                    odd = 4;
                    exp_type = NoteNumberExpressionType.International;
                } else if (value.StartsWith("ミ") || value.StartsWith("み") || value.StartsWith("ﾐ")) {
                    odd = 4;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith("ほ") || value.StartsWith("ホ") || value.StartsWith("ﾎ")) {
                    odd = 4;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("F")) {
                    odd = 5;
                    exp_type = NoteNumberExpressionType.International;
                } else if (value.StartsWith("ヘ") || value.StartsWith("へ") || value.StartsWith("ﾍ")) {
                    odd = 5;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("ファ") || value.StartsWith("ふぁ") || value.StartsWith("ﾌｧ")) {
                    odd = 5;
                    trim = 2;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith("G")) {
                    odd = 7;
                    exp_type = NoteNumberExpressionType.International;
                } else if (value.StartsWith("ソ") || value.StartsWith("そ") || value.StartsWith("ｿ")) {
                    odd = 7;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith("と") || value.StartsWith("ト") || value.StartsWith("ﾄ")) {
                    odd = 7;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("A")) {
                    odd = 9;
                    exp_type = NoteNumberExpressionType.International;
                } else if (value.StartsWith("ラ") || value.StartsWith("ら") || value.StartsWith("ﾗ")) {
                    odd = 9;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith("い") || value.StartsWith("イ") || value.StartsWith("ｲ")) {
                    odd = 9;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("H")) {
                    odd = 11;
                    exp_type = NoteNumberExpressionType.International;
                } else if (value.StartsWith("シ") || value.StartsWith("し") || value.StartsWith("ｼ")) {
                    odd = 11;
                    exp_type = NoteNumberExpressionType.JapaneseFixedDo;
                } else if (value.StartsWith("ろ") || value.StartsWith("ロ") || value.StartsWith("ﾛ")) {
                    odd = 11;
                    exp_type = NoteNumberExpressionType.Japanese;
                } else if (value.StartsWith("B")) {
                    if (first) {
                        odd = 11;
                        exp_type = NoteNumberExpressionType.International;
                    } else {
                        offset = -1;
                    }
                } else if (value.StartsWith("#") || value.StartsWith("♯") || value.StartsWith("＃")) {
                    offset = 1;
                } else if (value.StartsWith("♭")) {
                    offset = -1;
                }
                first = false;
                int len = value.Length;
                if (len == trim) {
                    break;
                }

                value = value.Substring(trim);
                int draft_scale;
                try {
                    draft_scale = int.Parse(value);
                    scale = draft_scale;
                    break;
                } catch (Exception ex) {
                }
            }
            if (AppManager.editorConfig != null) {
                if (exp_type == NoteNumberExpressionType.International &&
                    AppManager.editorConfig.PropertyWindowStatus.LastUsedNoteNumberExpression == NoteNumberExpressionType.Deutsche) {
                    // do nothing
                } else {
                    AppManager.editorConfig.PropertyWindowStatus.LastUsedNoteNumberExpression = exp_type;
                }
            }
            return 12 * scale + 2 * 12 + odd + offset * (doubled ? 2 : 1);
        }
    }

}
#endif
