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
using System;
using System.ComponentModel;

namespace cadencii
{

    [TypeConverter(typeof(NoteNumberPropertyConverter))]
    public class NoteNumberProperty
    {
        public int noteNumber = 60;

        public override int GetHashCode()
        {
            return hashCode();
        }

        public int hashCode()
        {
            return noteNumber.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            return equals(obj);
        }

        public bool equals(Object obj)
        {
            if (obj is NoteNumberProperty) {
                if (noteNumber == ((NoteNumberProperty)obj).noteNumber) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return base.Equals(obj);
            }
        }
    }

}
#endif
