#if ENABLE_PROPERTY
/*
 * VsqEventItemProxyTypeConverter.cs
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

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    public class VsqEventItemProxyTypeConverter : TypeConverter {
        public VsqEventItemProxyTypeConverter() {
        }

        public override PropertyDescriptorCollection GetProperties( ITypeDescriptorContext context, object value, Attribute[] attributes ) {
            PropertyDescriptorCollection buffClassProps;
            PropertyDescriptorCollection buffProps = TypeDescriptor.GetProperties( value, attributes, true );
            buffClassProps = new PropertyDescriptorCollection( null );

            foreach ( PropertyDescriptor oPD in buffProps ) {
                buffClassProps.Add( new VsqEventItemProxyPropertyDescriptor( oPD ) );
            }
            return buffClassProps;
        }

        public override boolean GetPropertiesSupported( ITypeDescriptorContext context ) {
            return true;
        }
    }

}
#endif
