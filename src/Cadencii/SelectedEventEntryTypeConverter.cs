#if ENABLE_PROPERTY
/*
 * SelectedEventEntryTypeConverter.cs
 * Copyright © 2010-2011 kbinani
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
using System;
using System.ComponentModel;

namespace cadencii
{
    public class SelectedEventEntryTypeConverter : TypeConverter
    {
        public SelectedEventEntryTypeConverter()
        {
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, Object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection buffClassProps;
            PropertyDescriptorCollection buffProps = TypeDescriptor.GetProperties(value, attributes, true);
            buffClassProps = new PropertyDescriptorCollection(null);

            foreach (PropertyDescriptor oPD in buffProps) {
                buffClassProps.Add(new SelectedEventEntryPropertyDescriptor(oPD));
            }
            return buffClassProps;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

}
#endif
