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

package com.github.cadencii;

import java.awt.*;

#else

namespace com.github
{
    namespace cadencii
    {
#if CSHARP
            using System;
            using com.github.cadencii.java.awt;
#endif
#endif

#if __cplusplus
            class WaveformZoomUiListener
#else
            public interface WaveformZoomUiListener
#endif
            {
#if __cplusplus
                virtual void receivePaintSignal( QPainter *g ){}
#else
                void receivePaintSignal( Graphics g );
#endif

#if __cplusplus
                virtual void receiveMouseDownSignal( int x, int y ){}
#else
                void receiveMouseDownSignal( int x, int y );
#endif

#if __cplusplus
                virtual void receiveMouseMoveSignal( int x, int y ){}
#else
                void receiveMouseMoveSignal( int x, int y );
#endif

#if __cplusplus
                virtual void receiveMouseUpSignal( int x, int y ){}
#else
                void receiveMouseUpSignal( int x, int y );
#endif
            };

#if JAVA

#else

    }
}

#endif
