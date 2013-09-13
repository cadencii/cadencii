/*
 * PropertyWindowListener.cs
 * Copyright © 2012 kbinani
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

package cadencii;

#else

namespace cadencii
{
#endif

    public interface PropertyWindowListener
    {
        void propertyWindowStateChanged();
        void propertyWindowLocationOrSizeChanged();
        void propertyWindowFormClosing();
    }

#if !JAVA
}
#endif
