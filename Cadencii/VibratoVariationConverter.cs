#if ENABLE_PROPERTY
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
#if JAVA

package cadencii;

import java.util.*;
import cadencii.*;
import cadencii.componentmodel.*;
import cadencii.vsq.*;

#else

using System;
using System.ComponentModel;
using cadencii.java.util;
using cadencii.vsq;

namespace cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class VibratoVariationConverter extends TypeConverter<VibratoVariation>
#else
    public class VibratoVariationConverter : TypeConverter
#endif
    {
#if !JAVA
        public override bool GetStandardValuesSupported( ITypeDescriptorContext context )
        {
            return isStandardValuesSupported();
        }
#endif

#if JAVA
        @Override
#endif
        public boolean isStandardValuesSupported()
        {
            return true;
        }

#if !JAVA
        public override StandardValuesCollection GetStandardValues( ITypeDescriptorContext context )
        {
            return new StandardValuesCollection( getStandardValues().toArray( new VibratoVariation[] { } ) );
        }
#endif

#if JAVA
        @Override
#endif
        public Vector<VibratoVariation> getStandardValues()
        {
            // ビブラート種類の候補値を列挙
            Vector<VibratoVariation> list = new Vector<VibratoVariation>();
            list.add( new VibratoVariation( VibratoVariation.empty.description ) );

            if ( AppManager.editorConfig.UseUserDefinedAutoVibratoType ) {
                // ユーザー定義の中から選ぶ場合
                int size = AppManager.editorConfig.AutoVibratoCustom.size();
#if DEBUG
                sout.println( "VibratoVariationConverter#GetStandardValues; size=" + size );
#endif
                for ( int i = 0; i < size; i++ ) {
                    VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom.get( i );
#if DEBUG
                    sout.println( "VibratoVariationConverter#GetStandardValues; handle.getDisplayString()=" + handle.getDisplayString() );
#endif
                    list.add( new VibratoVariation( handle.getDisplayString() ) );
                }
            } else {
                // VOCALOID1, VOCALOID2のシステム定義の中から選ぶ場合
                SynthesizerType type = SynthesizerType.VOCALOID2;
                VsqFileEx vsq = AppManager.getVsqFile();
                if ( vsq != null ) {
                    RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                    if ( kind == RendererKind.VOCALOID1 ) {
                        type = SynthesizerType.VOCALOID1;
                    }
                }
                for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                    VibratoHandle vconfig = itr.next();
                    list.add( new VibratoVariation( vconfig.getDisplayString() ) );
                }
            }
            return list;
        }

#if !JAVA
        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
        {
            if ( destinationType == typeof( String ) ) {
                return true;
            } else {
                return base.CanConvertTo( context, destinationType );
            }
        }
#endif

#if !JAVA
        public override Object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, Type destinationType )
        {
            if ( destinationType == typeof( String ) && value is VibratoVariation ) {
                return convertTo( (VibratoVariation)value );
            } else {
                return base.ConvertTo( context, culture, value, destinationType );
            }
        }
#endif

#if JAVA
        @Override
#endif
        public String convertTo( Object value )
        {
            if( value == null ){
                return "";
            }else{
                VibratoVariation vv = (VibratoVariation)value;
                return vv.description;
            }
        }

#if !JAVA
        public override Object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value )
        {
            if ( value is String ) {
                return convertFrom( (String)value );
            } else {
                return base.ConvertFrom( context, culture, value );
            }
        }
#endif

#if JAVA
        @Override
#endif
        public VibratoVariation convertFrom( String value )
        {
            if ( value.Equals( VibratoVariation.empty.description ) ) {
                return new VibratoVariation( VibratoVariation.empty.description );
            } else {
                if ( AppManager.editorConfig.UseUserDefinedAutoVibratoType ) {
                    int size = AppManager.editorConfig.AutoVibratoCustom.size();
                    for ( int i = 0; i < size; i++ ) {
                        String display_string = AppManager.editorConfig.AutoVibratoCustom.get( i ).getDisplayString();
                        if ( value.Equals( display_string ) ) {
                            return new VibratoVariation( display_string );
                        }
                    }
                } else {
                    SynthesizerType type = SynthesizerType.VOCALOID2;
                    VsqFileEx vsq = AppManager.getVsqFile();
                    if ( vsq != null ) {
                        RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                        if ( kind == RendererKind.VOCALOID1 ) {
                            type = SynthesizerType.VOCALOID1;
                        }
                        for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                            VibratoHandle vconfig = itr.next();
                            String display_string = vconfig.getDisplayString();
                            if ( value.Equals( display_string ) ) {
                                return new VibratoVariation( display_string );
                            }
                        }
                    }
                }
                return new VibratoVariation( VibratoVariation.empty.description );
            }
        }

#if !JAVA
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
        {
            if ( sourceType == typeof( String ) ) {
                return true;
            } else {
                return base.CanConvertFrom( context, sourceType );
            }
        }
#endif
    }

#if !JAVA
}
#endif
#endif
