/*
 * WaveformZoomUiListener.cs
 * Copyright © 2011 kbinani
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

package org.kbinani.cadencii;

import java.awt.*;

#else

using System;

using org.kbinani.java.awt;

namespace org.kbinani.cadencii
{
#endif

    interface WaveformZoomUiListener
    {
        void receivePaintSignal( Graphics g );

        void receiveMouseDownSignal( int x, int y );

        void receiveMouseMoveSignal( int x, int y );

        void receiveMouseUpSignal( int x, int y );
    }

#if JAVA

#else

}

#endif
