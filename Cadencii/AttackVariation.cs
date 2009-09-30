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
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Boare.Cadencii {

    [TypeConverter( typeof( AttackVariationConverter ) )]
    public class AttackVariation {
        public string description = "";

        public AttackVariation() {
            description = "-";
        }

        public AttackVariation( string description ) {
            this.description = description;
        }

        public override bool Equals( object obj ) {
            if ( obj != null && obj is AttackVariation ) {
                return ((AttackVariation)obj).description.Equals( description );
            } else {
                return base.Equals( obj );
            }
        }
    }

}
