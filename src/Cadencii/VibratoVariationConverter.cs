#if ENABLE_PROPERTY
/*
 * VibratoVariationConverter.cs
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
using System.Collections.Generic;
using cadencii.java.util;
using cadencii.vsq;



namespace cadencii
{

    public class VibratoVariationConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return isStandardValuesSupported();
        }

        public bool isStandardValuesSupported()
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(getStandardValues().ToArray());
        }

        public List<VibratoVariation> getStandardValues()
        {
            // ビブラート種類の候補値を列挙
            List<VibratoVariation> list = new List<VibratoVariation>();
            list.Add(new VibratoVariation(VibratoVariation.empty.description));

            if (AppManager.editorConfig.UseUserDefinedAutoVibratoType) {
                // ユーザー定義の中から選ぶ場合
                int size = AppManager.editorConfig.AutoVibratoCustom.Count;
#if DEBUG
                sout.println("VibratoVariationConverter#GetStandardValues; size=" + size);
#endif
                for (int i = 0; i < size; i++) {
                    VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom[i];
#if DEBUG
                    sout.println("VibratoVariationConverter#GetStandardValues; handle.getDisplayString()=" + handle.getDisplayString());
#endif
                    list.Add(new VibratoVariation(handle.getDisplayString()));
                }
            } else {
                // VOCALOID1, VOCALOID2のシステム定義の中から選ぶ場合
                SynthesizerType type = SynthesizerType.VOCALOID2;
                VsqFileEx vsq = AppManager.getVsqFile();
                if (vsq != null) {
                    RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[AppManager.getSelected()]);
                    if (kind == RendererKind.VOCALOID1) {
                        type = SynthesizerType.VOCALOID1;
                    }
                }
                foreach (var vconfig in VocaloSysUtil.vibratoConfigIterator(type)) {
                    list.Add(new VibratoVariation(vconfig.getDisplayString()));
                }
            }
            return list;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) {
                return true;
            } else {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is VibratoVariation) {
                return convertTo((VibratoVariation)value);
            } else {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public string convertTo(Object value)
        {
            if (value == null) {
                return "";
            } else {
                VibratoVariation vv = (VibratoVariation)value;
                return vv.description;
            }
        }

        public override Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
        {
            if (value is string) {
                return convertFrom((string)value);
            } else {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public VibratoVariation convertFrom(string value)
        {
            if (value.Equals(VibratoVariation.empty.description)) {
                return new VibratoVariation(VibratoVariation.empty.description);
            } else {
                if (AppManager.editorConfig.UseUserDefinedAutoVibratoType) {
                    int size = AppManager.editorConfig.AutoVibratoCustom.Count;
                    for (int i = 0; i < size; i++) {
                        string display_string = AppManager.editorConfig.AutoVibratoCustom[i].getDisplayString();
                        if (value.Equals(display_string)) {
                            return new VibratoVariation(display_string);
                        }
                    }
                } else {
                    SynthesizerType type = SynthesizerType.VOCALOID2;
                    VsqFileEx vsq = AppManager.getVsqFile();
                    if (vsq != null) {
                        RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[AppManager.getSelected()]);
                        if (kind == RendererKind.VOCALOID1) {
                            type = SynthesizerType.VOCALOID1;
                        }
                        foreach (var vconfig in VocaloSysUtil.vibratoConfigIterator(type)) {
                            string display_string = vconfig.getDisplayString();
                            if (value.Equals(display_string)) {
                                return new VibratoVariation(display_string);
                            }
                        }
                    }
                }
                return new VibratoVariation(VibratoVariation.empty.description);
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) {
                return true;
            } else {
                return base.CanConvertFrom(context, sourceType);
            }
        }
    }

}
#endif
