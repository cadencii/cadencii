/*
 * WaveformZoomUi.cs
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

import com.github.cadencii.windows.forms.*;

#else

namespace com.github
{
    namespace cadencii
    {

#if CSHARP
            using System;
            using com.github.cadencii.java.awt;
            using com.github.cadencii.windows.forms;
#else
            using namespace org::kbinani::cadencii;
#endif
#endif

#if __cplusplus
            class WaveformZoomUi
#else
            interface WaveformZoomUi
#endif
            {
#if __cplusplus
                virtual int getWidth(){}
#else
                int getWidth();
#endif

#if __cplusplus
                virtual int getHeight(){}
#else
                int getHeight();
#endif

#if __cplusplus
                virtual void setListener( WaveformZoomUiListener listener ){}
#else
                void setListener( WaveformZoomUiListener listener );
#endif

#if __cplusplus
                virtual void repaint(){}
#else
                void repaint();
#endif
            };

#if JAVA

#else

    }
}

#endif
