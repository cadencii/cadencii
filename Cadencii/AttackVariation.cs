/*
 * AttackVariation.cs
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
#if JAVA
package org.kbinani.cadencii;
#else
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

#if !JAVA
    [TypeConverter( typeof( AttackVariationConverter ) )]
#endif
    public class AttackVariation {
        public String description = "";

        public AttackVariation() {
            description = "-";
        }

        public AttackVariation( String description ) {
            this.description = description;
        }

        public boolean equals( Object obj ) {
#if JAVA
            if( obj != null && obj.getClass() == AttackVariation.class ){
#else
            if ( obj != null && obj is AttackVariation ) {
#endif
                return ((AttackVariation)obj).description.Equals( description );
            } else {
                return base.Equals( obj );
            }
        }

#if !JAVA
        public override bool Equals( object obj ) {
            return equals( obj );
        }
#endif
    }

#if !JAVA
}
#endif
