/*
 * OtoArgs.cs
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
#if JAVA
package org.kbinani.cadencii;
#else
using System;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 原音設定の引数．
    /// </summary>
    public struct OtoArgs {
        public String fileName;
        public String Alias;
        public float msOffset;
        public float msConsonant;
        public float msBlank;
        public float msPreUtterance;
        public float msOverlap;

        public boolean equals( OtoArgs obj ) {
#if JAVA
            if ( obj == null ) {
                return false;
            }
#endif
            if ( this.fileName != null && this.fileName.Equals( obj.fileName ) &&
                 this.Alias != null && this.Alias.Equals( obj.Alias ) &&
                 this.msOffset == obj.msOffset &&
                 this.msConsonant == obj.msConsonant &&
                 this.msBlank == obj.msBlank &&
                 this.msPreUtterance == obj.msPreUtterance &&
                 this.msOverlap == obj.msOverlap ) {
                return true;
            } else {
                return false;
            }
        }
    }

#if !JAVA
}
#endif
