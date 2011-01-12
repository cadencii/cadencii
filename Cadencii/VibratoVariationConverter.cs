#if !JAVA
/*
 * VibratoVariationConverter.cs
 * Copyright © 2009-2011 kbinani
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
            // ビブラート種類の候補値を列挙
            Vector<VibratoVariation> list = new Vector<VibratoVariation>();
            list.add( new VibratoVariation( VibratoVariation.empty.description ) );

            if ( AppManager.editorConfig.UseUserDefinedAutoVibratoType ) {
                // ユーザー定義の中から選ぶ場合
                int size = AppManager.editorConfig.AutoVibratoCustom.size();
#if DEBUG
                PortUtil.println( "VibratoVariationConverter#GetStandardValues; size=" + size );
#endif
                for ( int i = 0; i < size; i++ ) {
                    VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom.get( i );
#if DEBUG
                    PortUtil.println( "VibratoVariationConverter#GetStandardValues; handle.getDisplayString()=" + handle.getDisplayString() );
#endif
                    list.add( new VibratoVariation( handle.getDisplayString() ) );
                }
            } else {
                // VOCALOID1, VOCALOID2のシステム定義の中から選ぶ場合
                SynthesizerType type = SynthesizerType.VOCALOID2;
                VsqFileEx vsq = AppManager.getVsqFile();
                if ( vsq != null ) {
                    RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                    if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
                        type = SynthesizerType.VOCALOID1;
                    }
                }
                for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                    VibratoHandle vconfig = itr.next();
                    list.add( new VibratoVariation( vconfig.getDisplayString() ) );
                }
            }
            return new StandardValuesCollection( list.toArray( new VibratoVariation[] { } ) );
        }

        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( String ) ) {
                return true;
            } else {
                return base.CanConvertTo( context, destinationType );
            }
        }

        public override Object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, Type destinationType ) {
            if ( destinationType == typeof( String ) && value is VibratoVariation ) {
                return ((VibratoVariation)value).description;
            } else {
                return base.ConvertTo( context, culture, value, destinationType );
            }
        }

        public override Object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value ) {
            if ( value is String ) {
                if ( value.Equals( VibratoVariation.empty.description ) ) {
                    return new VibratoVariation( VibratoVariation.empty.description );
                } else {
                    String svalue = (String)value;
                    if ( AppManager.editorConfig.UseUserDefinedAutoVibratoType ) {
                        int size = AppManager.editorConfig.AutoVibratoCustom.size();
                        for ( int i = 0; i < size; i++ ) {
                            String display_string = AppManager.editorConfig.AutoVibratoCustom.get( i ).getDisplayString();
                            if ( svalue.Equals( display_string ) ) {
                                return new VibratoVariation( display_string );
                            }
                        }
                    } else {
                        SynthesizerType type = SynthesizerType.VOCALOID2;
                        VsqFileEx vsq = AppManager.getVsqFile();
                        if ( vsq != null ) {
                            RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                            if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
                                type = SynthesizerType.VOCALOID1;
                            }
                            for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                                VibratoHandle vconfig = itr.next();
                                String display_string = vconfig.getDisplayString();
                                if ( svalue.Equals( display_string ) ) {
                                    return new VibratoVariation( display_string );
                                }
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
            if ( sourceType == typeof( String ) ) {
                return true;
            } else {
                return base.CanConvertFrom( context, sourceType );
            }
        }
    }

}
#endif
