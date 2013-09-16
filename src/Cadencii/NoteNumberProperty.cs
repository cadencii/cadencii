#if ENABLE_PROPERTY
/*
 * NoteNumberProperty.cs
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

import cadencii.componentmodel.*;

#else

using System;
using System.ComponentModel;

namespace cadencii
{
    using boolean = Boolean;
#endif

#if JAVA
    @TypeConverterAnnotation( NoteNumberPropertyConverter.class )
    public class NoteNumberProperty
#else
    [TypeConverter( typeof( NoteNumberPropertyConverter ) )]
    public class NoteNumberProperty
#endif
    {
        public int noteNumber = 60;

#if !JAVA
        public override int GetHashCode()
        {
            return hashCode();
        }
#endif

        public int hashCode()
        {
#if JAVA
            return Integer.valueOf( noteNumber ).hashCode();
#else
            return noteNumber.GetHashCode();
#endif
        }

#if !JAVA
        public override boolean Equals( Object obj )
        {
            return equals( obj );
        }
#endif

        public boolean equals( Object obj )
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
