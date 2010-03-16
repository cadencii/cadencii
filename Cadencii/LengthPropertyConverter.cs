#if ENABLE_PROPERTY
/*
 * LengthPropertyConverter.cs
 * Copyright (C) 2010 kbinani
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
using System.Globalization;
using System.Collections.Generic;

namespace org.kbinani.cadencii {

    using boolean = System.Boolean;

    public class LengthPropertyConverter : TypeConverter {
        public override boolean CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( LengthProperty ) ) {
                return true;
            }
            return base.CanConvertTo( context, destinationType );
        }

        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType ) {
            if ( destinationType == typeof( String ) && value is LengthProperty ) {
                LengthProperty cp = (LengthProperty)value;
                return cp.getIntValue() + "";
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
                String s = (String)value;
                int old = 0;
                SelectedEventEntry[] objs = null;

                if ( context != null && context.Instance != null ) {
                    if ( context.Instance is SelectedEventEntry ) {
                        objs = new SelectedEventEntry[] { (SelectedEventEntry)context.Instance };
                    } else if ( context.Instance is object[] ) {
                        List<SelectedEventEntry> list = new List<SelectedEventEntry>();
                        object[] tobjs = (object[])context.Instance;
                        for ( int i = 0; i < tobjs.Length; i++ ) {
                            if ( tobjs[i] is SelectedEventEntry ) {
                                list.Add( (SelectedEventEntry)tobjs[i] );
                            }
                        }
                        objs = list.ToArray();
                    }
                }


#if DEBUG
                if ( context != null && context.Instance != null ) {
                    PortUtil.println( "LengthPropertyConverter#ConvertFrom# context.Instance.GetType()=" + context.Instance.GetType() );
                }
#endif
                if ( context != null && context.Instance is SelectedEventEntry ) {
                    SelectedEventEntry obj = (SelectedEventEntry)context.Instance;
                    old = obj.editing.ID.getLength();
                }
                if ( s.StartsWith( "-" ) || s.StartsWith( "+" ) ) {
                    int delta = 0;
                    try {
                        delta = (int)AppManager.eval( 0, s );
                    } catch {
                        delta = 0;
                    }
#if DEBUG
                    PortUtil.println( "LengthPropertyConverter#ConvertFrom; delta=" + delta );
#endif
                    old += delta;
                } else {
                    try {
                        old = (int)AppManager.eval( 0.0, s );
                    } catch {
                        old = 0;
                    }
                }
                return new LengthProperty( old );
            }
            return base.ConvertFrom( context, culture, value );
        }
    }

}
#endif
