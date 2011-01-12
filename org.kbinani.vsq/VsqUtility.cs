#if JAVA
package org.kbinani.cadencii;
#else
using System;

namespace org
{
    namespace kbinani
    {
        namespace vsq
        {
            using boolean = System.Boolean;
#endif

            public class VsqUtility
            {
                public static boolean compare( String a, String b )
                {
#if JAVA
                    if ( a == null || b == null ) {
                        return false;
                    }
                    return a.Equals( b );
#else
                    return a == b;
#endif
                }
            }

#if !JAVA
        }
    }
}
#endif
