/*
 * NoteNumberProperty.cs
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

    using boolean = Boolean;

    [TypeConverter( typeof( NoteNumberPropertyConverter ) )]
    public class NoteNumberProperty {
        public int noteNumber = 60;

        public override boolean Equals( object obj ) {
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

}
