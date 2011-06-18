/*
 * WaveformZoomUiImpl.cs
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

import org.kbinani.windows.forms.*;

#else

using System;

using org.kbinani.java.awt;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
    using BPaintEventArgs = System.Windows.Forms.PaintEventArgs;
#endif

    /// <summary>
    /// 波形表示の拡大・縮小を行うためのパネルです．
    /// </summary>
#if JAVA
    class WaveformZoomUiImpl extends BPanel implements WaveformZoomUi
#else
    class WaveformZoomUiImpl : BPanel, WaveformZoomUi
#endif
    {

        /// <summary>
        /// このビューのController
        /// </summary>
        private WaveformZoomUiListener mListener = null;
#if !JAVA
        private Graphics2D mGraphicsPanel2 = null;
#endif

        public void setListener( WaveformZoomUiListener listener )
        {
            mListener = listener;
        }

#if !JAVA
        protected override void OnPaint( System.Windows.Forms.PaintEventArgs e )
        {
            base.OnPaint( e );
            if ( mGraphicsPanel2 == null )
            {
                mGraphicsPanel2 = new Graphics2D( null );
            }
            mGraphicsPanel2.nativeGraphics = e.Graphics;
            Graphics g = mGraphicsPanel2;
            mListener.receivePaintSignal( g );
        }

        protected override void OnMouseDown( System.Windows.Forms.MouseEventArgs e )
        {
            base.OnMouseDown( e );
            mListener.receiveMouseDownSignal( e.X, e.Y );
        }

        protected override void OnMouseMove( System.Windows.Forms.MouseEventArgs e )
        {
            base.OnMouseMove( e );
            mListener.receiveMouseMoveSignal( e.X, e.Y );
        }

        protected override void OnMouseUp( System.Windows.Forms.MouseEventArgs e )
        {
            base.OnMouseUp( e );
            mListener.receiveMouseUpSignal( e.X, e.Y );
        }
#endif

    }

#if JAVA

#else

}

#endif
