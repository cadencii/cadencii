#if JAVA
package org.kbinani.editotoini;

#else
using System;

namespace org.kbinani.editotoini {
#endif

    public class FlagType {
        public static readonly FlagType Offset = new FlagType( 0 );
        public static readonly FlagType Consonant = new FlagType( 1 );
        public static readonly FlagType Blank = new FlagType( 2 );
        public static readonly FlagType PreUtterance = new FlagType( 3 );
        public static readonly FlagType Overlap = new FlagType( 4 );

        private int m_value;

        private FlagType( int value ) {
            m_value = value;
        }

        public int getValue() {
            return m_value;
        }
    }

#if !JAVA
}
#endif
