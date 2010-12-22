#if JAVA
package org.kbinani.cadencii;
#else
namespace org {
    namespace kbinani {
        namespace vsq {
#endif

            public class VsqUtility {
                public static bool compare( string a, string b ) {
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
