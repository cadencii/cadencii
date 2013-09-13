/*
 * EditedZoneCommand.cs
 * Copyright © 2010-2011 kbinani
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
package cadencii;

import java.util.*;
#else
using System;
using cadencii.java.util;

namespace cadencii {
#endif

    public class EditedZoneCommand {
        public Vector<EditedZoneUnit> mAdd;
        public Vector<EditedZoneUnit> mRemove;

        public EditedZoneCommand( int addStart, int addEnd )
#if JAVA
        {
#else
            :
#endif
            this( new EditedZoneUnit[] { new EditedZoneUnit( addStart, addEnd ) }, new EditedZoneUnit[] { } )
#if JAVA
            ;
#else
        {
#endif
        }

        public EditedZoneCommand( EditedZoneUnit[] add, EditedZoneUnit[] remove ) {
            this.mAdd = new Vector<EditedZoneUnit>();
            for ( int i = 0; i < add.Length; i++ ) {
                this.mAdd.add( (EditedZoneUnit)add[i].clone() );
            }
            this.mRemove = new Vector<EditedZoneUnit>();
            for ( int i = 0; i < remove.Length; i++ ) {
                this.mRemove.add( (EditedZoneUnit)remove[i].clone() );
            }
        }

        public EditedZoneCommand( Vector<EditedZoneUnit> add, Vector<EditedZoneUnit> remove )
#if JAVA
            {
#else
            :
#endif
            this( add.toArray( new EditedZoneUnit[] { } ), remove.toArray( new EditedZoneUnit[] { } ) )
#if JAVA
            ;
#else
        {
#endif
        }
    }

#if !JAVA
}
#endif
