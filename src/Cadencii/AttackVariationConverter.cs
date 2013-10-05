/*
 * AttackVariationConverter.cs
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
using System.Windows.Forms;
using System.Collections.Generic;
using cadencii.vsq;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii
{

    public class AttackVariationConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return isStandardValuesSupported();
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            /*SynthesizerType type = SynthesizerType.VOCALOID2;
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq != null ) {
                RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ){
                    type = SynthesizerType.VOCALOID1;
                }
            }
            Vector<AttackVariation> list = new Vector<AttackVariation>();
            list.add( new AttackVariation() );
            for ( Iterator<NoteHeadHandle> itr = VocaloSysUtil.attackConfigIterator( type ); itr.hasNext(); ) {
                NoteHeadHandle aconfig = itr.next();
                list.add( new AttackVariation( aconfig.getDisplayString() ) );
            }
            return new StandardValuesCollection( list.toArray( new AttackVariation[] { } ) );*/
            return new StandardValuesCollection(getStandardValues());
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
            return convertTo(value);
        }

        public override Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
        {
            if (value is string) {
                string s = (string)value;
                return convertFrom(s);
            } else {
                return base.ConvertFrom(context, culture, value);
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

        public string convertTo(Object value)
        {
            if (value is AttackVariation) {
                return ((AttackVariation)value).mDescription;
            } else {
                return "";
            }
        }

        public AttackVariation convertFrom(string value)
        {
            if (value.Equals(new AttackVariation().mDescription)) {
                return new AttackVariation();
            } else {
                SynthesizerType type = SynthesizerType.VOCALOID2;
                VsqFileEx vsq = AppManager.getVsqFile();
                if (vsq != null) {
                    RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[AppManager.getSelected()]);
                    if (kind == RendererKind.VOCALOID1) {
                        type = SynthesizerType.VOCALOID1;
                    }
                    string svalue = (string)value;
                    foreach (var aconfig in VocaloSysUtil.attackConfigIterator(type)) {
                        string display_string = aconfig.getDisplayString();
                        if (svalue.Equals(display_string)) {
                            return new AttackVariation(display_string);
                        }
                    }
                }
                return new AttackVariation();
            }
        }

        public List<Object> getStandardValues()
        {
            SynthesizerType type = SynthesizerType.VOCALOID2;
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq != null) {
                RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[AppManager.getSelected()]);
                if (kind == RendererKind.VOCALOID1) {
                    type = SynthesizerType.VOCALOID1;
                }
            }
            List<Object> list = new List<Object>();
            list.Add(new AttackVariation());
            foreach (var aconfig in VocaloSysUtil.attackConfigIterator(type)) {
                list.Add(new AttackVariation(aconfig.getDisplayString()));
            }
            return list;//new StandardValuesCollection( list.toArray( new AttackVariation[] { } ) );
        }

        public bool isStandardValuesSupported()
        {
            return true;
        }

    }

}
