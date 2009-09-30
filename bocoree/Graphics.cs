/*
 * Graphics.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Drawing;

namespace bocoree {

    public class Graphics2D {
        private System.Drawing.Graphics m_graphics;
        private Color m_color = Color.Black;
        private Pen m_pen = Pens.Black;
        private SolidBrush m_brush = new SolidBrush( Color.Black );

        public Graphics2D( System.Drawing.Graphics g ) {
            m_graphics = g;
        }

        public void clearRect( int x, int y, int width, int height ) {
            m_graphics.FillRectangle( System.Drawing.Brushes.White, x, y, width, height );
        }

        public void drawLine( int x1, int y1, int x2, int y2 ) {
            m_graphics.DrawLine( m_pen, x1, y1, x2, y2 );
        }

        public void drawRect( int x, int y, int width, int height ) {
            m_graphics.DrawRectangle( m_pen, x, y, width, height );
        }

        public void fillRect( int x, int y, int width, int height ) {
            m_graphics.FillRectangle( m_brush, x, y, width, height );
        }

        public void drawOval( int x, int y, int width, int height ) {
            m_graphics.DrawEllipse( m_pen, x, y, width, height );
        }

        public void fillOval( int x, int y, int width, int height ) {
            m_graphics.FillRectangle( m_brush, x, y, width, height );
        }

        public void setColor( System.Drawing.Color c ) {
            m_color = c;
            m_pen.Color = c;
            m_brush.Color = c;
        }

        public Color getColor() {
            return m_color;
        }
    }

}
