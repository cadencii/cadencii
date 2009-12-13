#if JAVA
package org.kbinani;

#else
using System;

namespace bocoree {
#endif

    public class InternalStdErr {
        public void println( String s ) {
#if JAVA
            System.err.println( s );
#else
            Console.Error.WriteLine( s );
#endif
        }
    }

#if !JAVA
}
#endif
