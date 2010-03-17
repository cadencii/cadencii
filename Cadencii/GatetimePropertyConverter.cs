#if ENABLE_PROPERTY
/*
 * GatetimePropertyConverter.cs
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
using System.Globalization;

namespace org.kbinani.cadencii {
    using boolean = Boolean;

    public class GatetimePropertyConverter : ExpandableObjectConverter {
        //コンバータがオブジェクトを指定した型に変換できるか
        //（変換できる時はTrueを返す）
        //ここでは、CustomClass型のオブジェクトには変換可能とする
        public override boolean CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( GatetimeProperty ) ) {
                return true;
            }
            return base.CanConvertTo( context, destinationType );
        }

        //指定した値オブジェクトを、指定した型に変換する
        //CustomClass型のオブジェクトをString型に変換する方法を提供する
        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType ) {
#if DEBUG
            PortUtil.println( "GatetimePropertyConverter#ConvertTo" );
#endif
            if ( destinationType == typeof( String ) && value is GatetimeProperty ) {
                GatetimeProperty cp = (GatetimeProperty)value;
                return cp.Measure + " : " + cp.Beat + " : " + cp.Gate;
            }
            return base.ConvertTo( context, culture, value, destinationType );
        }

        //コンバータが特定の型のオブジェクトをコンバータの型に変換できるか
        //（変換できる時はTrueを返す）
        //ここでは、String型のオブジェクトなら変換可能とする
        public override boolean CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {
            if ( sourceType == typeof( String ) ) {
                return true;
            }
            return base.CanConvertFrom( context, sourceType );
        }

        //指定した値をコンバータの型に変換する
        //String型のオブジェクトをCustomClass型に変換する方法を提供する
        public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value ) {
#if DEBUG
            PortUtil.println( "GatetimePropretyConverter#ConvertFrom" );
#endif
            if ( value is String ) {
#if DEBUG
                if ( context != null ) {
                    if ( context.Instance != null ) {
                        PortUtil.println( "GatetimePropertyConverter#ConvertFrom; context.Instance.GetType()=" + context.Instance.GetType() );
                        if ( context.Instance.GetType() == typeof( object[] ) ) {
                            object[] objs = (object[])context.Instance;
                            for ( int i = 0; i < objs.Length; i++ ) {
                                object obj = objs[i];
                                if ( obj != null ) {
                                    PortUtil.println( "GatetimePropretyConverter#ConvertFrom; ((object[])context.Instance)[" + i + "].GetType()=" + ((object[])context.Instance)[i].GetType() );
                                }
                            }
                        }
                    }
                }
#endif
                String[] ss = ((String)value).Split( new char[] { ':' }, 3 );
                if ( ss.Length >= 3 ) {
                    try {
                        int measure = int.Parse( ss[0].Trim() );
                        int beat = int.Parse( ss[1].Trim() );
                        int gate = int.Parse( ss[2].Trim() );
                        return new GatetimeProperty( measure, beat, gate );
                    } catch {
                        return new GatetimeProperty();
                    }
                } else {
                    return new GatetimeProperty();
                }
            }
            return base.ConvertFrom( context, culture, value );
        }
    }

}
#endif
