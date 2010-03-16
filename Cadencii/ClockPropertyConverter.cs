#if ENABLE_PROPERTY
/*
 * ClockPropertyConverter.cs
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

    using boolean = System.Boolean;

    public class ClockPropertyConverter : TypeConverter {
        public override boolean CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( ClockProperty ) ) {
                return true;
            }
            return base.CanConvertTo( context, destinationType );
        }

        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType ) {
            if ( destinationType == typeof( String ) && value is ClockProperty ) {
                ClockProperty cp = (ClockProperty)value;
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
                ClockProperty obj = new ClockProperty();
                obj.setStr( (String)value );
                return obj;
            }
            return base.ConvertFrom( context, culture, value );
        }
    }

}
#endif
