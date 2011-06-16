/*
 * WaveformZoomController.cs
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

#else

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
#endif

    class WaveformZoomController
    {
        private WaveView mWaveView = null;
        private FormMain mFormMain = null;
        private WaveformZoomPanel mUi = null;

        public WaveformZoomController( FormMain form_main, WaveView wave_view )
        {
            mWaveView = wave_view;
            mFormMain = form_main;

            mUi = new WaveformZoomPanel( this );
        }

        public void refreshScreen()
        {
            mFormMain.refreshScreen();
        }

        public void setAutoMaximize( boolean value )
        {
            mWaveView.setAutoMaximize( value );
        }

        public float getScale()
        {
            return mWaveView.getScale();
        }

        public void setScale( float value )
        {
            mWaveView.setScale( value );
        }

        public WaveformZoomPanel getUi()
        {
            return mUi;
        }
    }

#if JAVA

#else

}

#endif
