/*
 * VibratoVariationConverter.cs
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
using System.Windows.Forms;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.java.util;
using bocoree.java.io;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public class VibratoVariationConverter : TypeConverter {
        public override bool GetStandardValuesSupported( ITypeDescriptorContext context ) {
            return true;
        }

        public override StandardValuesCollection GetStandardValues( ITypeDescriptorContext context ) {
            SynthesizerType type = SynthesizerType.VOCALOID2;
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq != null ) {
                if ( vsq.Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                    type = SynthesizerType.VOCALOID1;
                }
            }
            Vector<VibratoVariation> list = new Vector<VibratoVariation>();
            list.add( new VibratoVariation( VibratoVariation.empty.description ) );
            for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                VibratoConfig vconfig = (VibratoConfig)itr.next();
                list.add( new VibratoVariation( vconfig.contents.getDisplayString() ) );
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
                        if ( vsq.Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                            type = SynthesizerType.VOCALOID1;
                        }
                        String svalue = (String)value;
                        for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                            VibratoConfig vconfig = (VibratoConfig)itr.next();
                            String display_string = vconfig.contents.getDisplayString();
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
