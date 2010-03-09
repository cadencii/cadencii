/*
 * EditedZoneUnit.cs
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
#if JAVA
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii {
#endif

#if JAVA
    public class EditedZoneUnit implements Clonable, Comparable<EditedZoneUnit> {
#else
    public class EditedZoneUnit : ICloneable, IComparable<EditedZoneUnit> {
#endif
        public int start;
        public int end;

        private EditedZoneUnit(){
        }

        public EditedZoneUnit( int start, int end ){
            this.start = start;
            this.end = end;
        }

        public int compareTo( EditedZoneUnit item ) {
            return this.start - item.start;
        }

#if !JAVA
        public int CompareTo( EditedZoneUnit item ) {
            return compareTo( item );
        }
#endif

        public Object clone() {
            return new EditedZoneUnit( start, end );
        }

#if !JAVA
        public object Clone(){
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
