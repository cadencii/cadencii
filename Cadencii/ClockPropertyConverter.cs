#if !JAVA
/*
 * ClockPropertyConverter.cs
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
using System.Globalization;

namespace Boare.Cadencii {
    using boolean = Boolean;

    public class ClockPropertyConverter : ExpandableObjectConverter {
        //コンバータがオブジェクトを指定した型に変換できるか
        //（変換できる時はTrueを返す）
        //ここでは、CustomClass型のオブジェクトには変換可能とする
        public override boolean CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( ClockProperty ) ) {
                return true;
            }
            return base.CanConvertTo( context, destinationType );
        }

        //指定した値オブジェクトを、指定した型に変換する
        //CustomClass型のオブジェクトをString型に変換する方法を提供する
        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType ) {
            if ( destinationType == typeof( String ) && value is ClockProperty ) {
                ClockProperty cp = (ClockProperty)value;
                return cp.getMeasure().getIntValue() + " : " + cp.getBeat().getIntValue() + " : " + cp.getGate().getIntValue();
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
            if ( value is String ) {
                String[] ss = ((String)value).Split( new char[] { ':' }, 3 );
                CalculatableString cs = new CalculatableString();
                cs.setStr( ss[0] );
                int measure = cs.getIntValue();
                cs.setStr( ss[1] );
                int beat = cs.getIntValue();
                cs.setStr( ss[2] );
                int gate = cs.getIntValue();
                return new ClockProperty( measure, beat, gate );
            }
            return base.ConvertFrom( context, culture, value );
        }
    }

}
#endif
