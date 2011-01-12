#if !JAVA
/*
 * VibratoVariation.cs
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
using System;
using System.ComponentModel;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    [TypeConverter( typeof( VibratoVariationConverter ) )]
    public class VibratoVariation {
        public static readonly VibratoVariation empty = new VibratoVariation();

        public String description = "";

        private VibratoVariation() {
            description = "-";
        }

        public VibratoVariation( String description ) {
            this.description = description;
        }

        public boolean equals( Object obj ){
            if ( obj != null && obj is VibratoVariation ) {
                return ((VibratoVariation)obj).description.Equals( description );
            } else {
                return base.Equals( obj );
            }
        }

#if !JAVA
        public override bool Equals( Object obj ) {
            return equals( obj );
        }
#endif

#if !JAVA
        public override int GetHashCode() {
            if ( description == null ) {
                return "".GetHashCode();
            } else {
                return description.GetHashCode();
            }
        }
#endif

        public Object clone() {
            return new VibratoVariation( this.description );
        }

#if !JAVA
        public Object Clone() {
            return clone();
        }
#endif

    }
}
#endif
