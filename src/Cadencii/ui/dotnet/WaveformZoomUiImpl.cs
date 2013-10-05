/*
 * WaveformZoomUiImpl.cs
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
using System;
using System.Windows.Forms;

using cadencii.java.awt;
using cadencii.windows.forms;

namespace cadencii
{
    /// <summary>
    /// 波形表示の拡大・縮小を行うためのパネルです．
    /// </summary>
    class WaveformZoomUiImpl : UserControl, WaveformZoomUi
    {

        /// <summary>
        /// このビューのController
        /// </summary>
        private WaveformZoomUiListener mListener = null;
        private Graphics2D mGraphicsPanel2 = null;

        public void setListener(WaveformZoomUiListener listener)
        {
            mListener = listener;
        }

        public void repaint()
        {
            Refresh();
        }

        public int getHeight()
        {
            return Height;
        }

        public int getWidth()
        {
            return Width;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            if (mGraphicsPanel2 == null) {
                mGraphicsPanel2 = new Graphics2D(null);
            }
            mGraphicsPanel2.nativeGraphics = e.Graphics;
            Graphics g = mGraphicsPanel2;
            mListener.receivePaintSignal(g);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mListener.receiveMouseDownSignal(e.X, e.Y);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mListener.receiveMouseMoveSignal(e.X, e.Y);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mListener.receiveMouseUpSignal(e.X, e.Y);
        }
    }

}
