/*
 * VibratoVariation.cs
 * Copyright (C) 2009-2010 kbinani
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
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace org.kbinani.cadencii {

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
