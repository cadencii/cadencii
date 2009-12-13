#if JAVA
package org.kbinani;

#else
using System;

namespace bocoree {
#endif

    public class InternalStdOut {
        public void println( String s ) {
#if JAVA
            System.out.println( s );
#else
            Console.Out.WriteLine( s );
#endif
        }
    }

#if !JAVA
}
#endif
