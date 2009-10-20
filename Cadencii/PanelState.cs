#if JAVA
package org.kbinani.Cadencii;
#else
namespace Boare.Cadencii
{
#endif

    public enum PanelState
    {
        Hidden,
        Window,
        Docked,
    }

#if !JAVA
}
#endif
