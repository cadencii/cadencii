/*
 * WaveformZoomUi.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package cadencii;

#else

    namespace cadencii
    {

#if CSHARP
            using System;
            using cadencii;
#else
            using namespace org::kbinani::cadencii;
#endif
#endif

#if __cplusplus
            class WaveformZoomUi
#else
            public interface WaveformZoomUi
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

#endif
