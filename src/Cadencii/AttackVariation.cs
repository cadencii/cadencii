/*
 * AttackVariation.cs
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
using System.Drawing.Design;

namespace cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    @TypeConverterAnnotation( AttackVariationConverter.class )
#else
    [TypeConverter( typeof( AttackVariationConverter ) )]
#endif
    public class AttackVariation
    {
        public String mDescription = "";

        public AttackVariation() {
            mDescription = "-";
        }

        public AttackVariation( String description ) {
            this.mDescription = description;
        }

        public boolean equals( Object obj ) {
            if ( obj != null && obj is AttackVariation ) {
                return ((AttackVariation)obj).mDescription.Equals( mDescription );
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
            if ( mDescription == null ) {
                return "".GetHashCode();
            } else {
                return mDescription.GetHashCode();
            }
        }
#endif

    }

#if !JAVA
}
#endif
