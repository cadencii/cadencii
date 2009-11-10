#if JAVA
package org.kbinani.Cadencii;
#else
using System;

namespace Boare.Cadencii {
#endif

    public class AutoVibratoMinLengthUtil {
        public static String toString( AutoVibratoMinLengthEnum value ) {
            if ( value == AutoVibratoMinLengthEnum.L1 ) {
                return "1";
            } else if ( value == AutoVibratoMinLengthEnum.L2 ) {
                return "2";
            } else if ( value == AutoVibratoMinLengthEnum.L3 ) {
                return "3";
            } else {
                return "4";
            }
        }

        public static int getValue( AutoVibratoMinLengthEnum value ) {
            if ( value == AutoVibratoMinLengthEnum.L1 ) {
                return 1;
            } else if ( value == AutoVibratoMinLengthEnum.L2 ) {
                return 2;
            } else if ( value == AutoVibratoMinLengthEnum.L3 ) {
                return 3;
            } else {
                return 4;
            }
        }
    }

#if !JAVA
}
#endif
