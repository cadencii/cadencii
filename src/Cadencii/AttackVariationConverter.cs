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
#if JAVA

package cadencii;

import java.util.*;
import java.io.*;
import cadencii.*;
import cadencii.vsq.*;
import cadencii.componentmodel.*;

#else

using System;
using System.ComponentModel;
using System.Windows.Forms;
using cadencii.vsq;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class AttackVariationConverter extends TypeConverter
#else
    public class AttackVariationConverter : TypeConverter
#endif
    {
#if !JAVA
        public override bool GetStandardValuesSupported( ITypeDescriptorContext context ) {
            return isStandardValuesSupported();
        }

        public override StandardValuesCollection GetStandardValues( ITypeDescriptorContext context ) {
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
            return new StandardValuesCollection( getStandardValues() );
        }

        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( String ) ) {
                return true;
            } else {
                return base.CanConvertTo( context, destinationType );
            }
        }

        public override Object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, Type destinationType ) {
            return convertTo( value );
        }

        public override Object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value ) {
            if ( value is String ) {
                String s = (String)value;
                return convertFrom( s );
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
#endif

#if JAVA
        @Override
#endif
        public String convertTo( Object value )
        {
            if ( value is AttackVariation ) {
                return ((AttackVariation)value).mDescription;
            } else {
#if JAVA
                return super.convertTo( value );
#else
                return "";
#endif
            }
        }

#if JAVA
        @Override
#endif
        public AttackVariation convertFrom( String value )
        {
            if ( value.Equals( new AttackVariation().mDescription ) ) {
                return new AttackVariation();
            } else {
                SynthesizerType type = SynthesizerType.VOCALOID2;
                VsqFileEx vsq = AppManager.getVsqFile();
                if ( vsq != null ) {
                    RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                    if ( kind == RendererKind.VOCALOID1 ) {
                        type = SynthesizerType.VOCALOID1;
                    }
                    String svalue = (String)value;
                    for ( Iterator<NoteHeadHandle> itr = VocaloSysUtil.attackConfigIterator( type ); itr.hasNext(); ) {
                        NoteHeadHandle aconfig = itr.next();
                        String display_string = aconfig.getDisplayString();
                        if ( svalue.Equals( display_string ) ) {
                            return new AttackVariation( display_string );
                        }
                    }
                }
                return new AttackVariation();
            }
        }

#if JAVA
        @Override
#endif
        public Vector<Object> getStandardValues()
        {
            SynthesizerType type = SynthesizerType.VOCALOID2;
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq != null ) {
                RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                if ( kind == RendererKind.VOCALOID1 ){
                    type = SynthesizerType.VOCALOID1;
                }
            }
            Vector<Object> list = new Vector<Object>();
            list.Add( new AttackVariation() );
            for ( Iterator<NoteHeadHandle> itr = VocaloSysUtil.attackConfigIterator( type ); itr.hasNext(); ) {
                NoteHeadHandle aconfig = itr.next();
                list.Add( new AttackVariation( aconfig.getDisplayString() ) );
            }
            return list;//new StandardValuesCollection( list.toArray( new AttackVariation[] { } ) );
        }

#if JAVA
        @Override
#endif
        public boolean isStandardValuesSupported()
        {
            return true;
        }

    }

#if !JAVA
}
#endif
