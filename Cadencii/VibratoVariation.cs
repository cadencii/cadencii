/*
 * VibratoVariation.cs
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

    [TypeConverter( typeof( VibratoVariationConverter ) )]
    public class VibratoVariation {
        public static readonly VibratoVariation empty = new VibratoVariation();

        public string description = "";

        private VibratoVariation() {
            description = "-";
        }

        public VibratoVariation( string description ) {
            this.description = description;
        }

        public override bool Equals( object obj ) {
            if ( obj != null && obj is VibratoVariation ) {
                return ((VibratoVariation)obj).description.Equals( description );
            } else {
                return base.Equals( obj );
            }
        }
    }

}
