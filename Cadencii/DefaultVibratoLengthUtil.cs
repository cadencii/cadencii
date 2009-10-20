#if JAVA
package org.kbinani.Cadencii;

#else
using System;

namespace Boare.Cadencii {
#endif

    public class DefaultVibratoLengthUtil {
        public static String toString( DefaultVibratoLength value ) {
            if ( value == DefaultVibratoLength.L50 ) {
                return "50";
            } else if ( value == DefaultVibratoLength.L66 ) {
                return "66";
            } else if ( value == DefaultVibratoLength.L75 ) {
                return "75";
            } else {
                return "100";
            }
        }
    }

#if !JAVA
}
#endif
