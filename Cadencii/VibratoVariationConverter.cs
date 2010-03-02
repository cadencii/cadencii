/*
 * VibratoVariationConverter.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using org.kbinani.java.util;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {


    public class VibratoVariationConverter : TypeConverter {
        public override bool GetStandardValuesSupported( ITypeDescriptorContext context ) {
            return true;
        }

        public override StandardValuesCollection GetStandardValues( ITypeDescriptorContext context ) {
            SynthesizerType type = SynthesizerType.VOCALOID2;
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq != null ) {
                RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
                    type = SynthesizerType.VOCALOID1;
                }
            }
            Vector<VibratoVariation> list = new Vector<VibratoVariation>();
            list.add( new VibratoVariation( VibratoVariation.empty.description ) );
            for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                VibratoHandle vconfig = (VibratoHandle)itr.next();
                list.add( new VibratoVariation( vconfig.getDisplayString() ) );
            }
            return new StandardValuesCollection( list.toArray( new VibratoVariation[] { } ) );
        }

        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( string ) ) {
                return true;
            } else {
                return base.CanConvertTo( context, destinationType );
            }
        }

        public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType ) {
            if ( destinationType == typeof( string ) && value is VibratoVariation ) {
                return ((VibratoVariation)value).description;
            } else {
                return base.ConvertTo( context, culture, value, destinationType );
            }
        }

        public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value ) {
            if ( value is string ) {
                if ( value.Equals( VibratoVariation.empty.description ) ) {
                    return new VibratoVariation( VibratoVariation.empty.description );
                } else {
                    SynthesizerType type = SynthesizerType.VOCALOID2;
                    VsqFileEx vsq = AppManager.getVsqFile();
                    if ( vsq != null ) {
                        RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                        if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
                            type = SynthesizerType.VOCALOID1;
                        }
                        String svalue = (String)value;
                        for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                            VibratoHandle vconfig = (VibratoHandle)itr.next();
                            String display_string = vconfig.getDisplayString();
                            if ( svalue.Equals( display_string ) ) {
                                return new VibratoVariation( display_string );
                            }
                        }
                    }
                    return new VibratoVariation( VibratoVariation.empty.description );
                }
            } else {
                return base.ConvertFrom( context, culture, value );
            }
        }

        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {
            if ( sourceType == typeof( string ) ) {
                return true;
            } else {
                return base.CanConvertFrom( context, sourceType );
            }
        }
    }

}
