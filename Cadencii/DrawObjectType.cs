#if JAVA
package org.kbinani.vsq;

#else
namespace org.kbinani.vsq {
#endif

    public enum DrawObjectType {
        Note,
        Dynaff,
        Crescend,
        Decrescend,
    }

#if !JAVA
}
#endif
