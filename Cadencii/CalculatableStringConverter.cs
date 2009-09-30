/*
 * CalculatableStringConverter.cs
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

    using boolean = System.Boolean;

    public class CalculatableStringConverter : TypeConverter {
        public override boolean CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( CalculatableString ) ) {
                return true;
            }
            return base.CanConvertTo( context, destinationType );
        }

        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType ) {
            if ( destinationType == typeof( String ) && value is CalculatableString ) {
                CalculatableString cp = (CalculatableString)value;
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
                CalculatableString obj = new CalculatableString();
                obj.str = (String)value;
                return obj;
            }
            return base.ConvertFrom( context, culture, value );
        }
    }

}