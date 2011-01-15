#if ENABLE_PROPERTY
/*
 * NoteNumberProperty.cs
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
package org.kbinani.cadencii;

import org.kbinani.componentmodel.*;
#else
using System;
using System.ComponentModel;

namespace org.kbinani.cadencii
{
    using boolean = Boolean;
#endif

#if JAVA
    public class NoteNumberProperty implements ITypeConverter
#else
    [TypeConverter( typeof( NoteNumberPropertyConverter ) )]
    public class NoteNumberProperty
#endif
    {
        public int noteNumber = 60;

#if JAVA
        public TypeConverter getTypeConverter()
        {
            return new NoteNumberPropertyConverter();
        }
#endif

#if !JAVA
        public override int GetHashCode()
        {
            return hashCode();
        }
#endif

        public int hashCode()
        {
            return noteNumber.GetHashCode();
        }

#if !JAVA
        public override boolean Equals( Object obj )
        {
            return equals( obj );
        }
#endif

        private boolean equals( Object obj )
        {
            if ( obj is NoteNumberProperty ) {
                if ( noteNumber == ((NoteNumberProperty)obj).noteNumber ) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return base.Equals( obj );
            }
        }
    }

#if !JAVA
}
#endif
#endif
