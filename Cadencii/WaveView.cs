/*
 * WaveView.cs
 * Copyright (C) 2009-2010 kbinani
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

import org.kbinani.windows.forms.*;

import java.awt.*;
import java.awt.image.*;
import org.kbinani.*;
import org.kbinani.media.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.media;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.awt.image;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class WaveView extends BPanel {
#else
    public class WaveView : BPanel {
#endif
        private WaveDrawContext[] drawer = new WaveDrawContext[16];

        public WaveView()
#if JAVA
        {
#else
            :
#endif
            base()
#if JAVA
            ;
#else
        {
#endif
#if !JAVA
            this.SetStyle( ControlStyles.DoubleBuffer, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            this.DoubleBuffered = true;
#endif
        }

        private void paint( Graphics2D g ) {
            int width = getWidth();
            Rectangle rc = new Rectangle( 0, 0, width, getHeight() );
            if ( AppManager.skipDrawingWaveformWhenPlaying && AppManager.isPlaying() ) {
                PortUtil.drawStringEx( g, 
                                       "(hidden for performance)", 
                                       AppManager.baseFont10,
                                       rc, 
                                       PortUtil.STRING_ALIGN_CENTER,
                                       PortUtil.STRING_ALIGN_CENTER );
                return;
            }
            int selected = AppManager.getSelected();
            WaveDrawContext context = drawer[selected - 1];
            if ( context == null ) {
                return;
            }
            context.draw( g,
                          Color.black,
                          rc,
                          AppManager.clockFromXCoord( AppManager.keyWidth ),
                          AppManager.clockFromXCoord( AppManager.keyWidth + width ),
                          AppManager.getVsqFile().TempoTable,
                          AppManager.scaleX );
        }

        public void unloadAll() {
            for ( int i = 0; i < drawer.Length; i++ ) {
                WaveDrawContext context = drawer[i];
                if ( context == null ) {
                    continue;
                }
                context.unload();
            }
        }

        public void reloadPartial( int track, String file, double sec_from, double sec_to ) {
            if ( track < 0 || drawer.Length <= track ) {
                return;
            }
            if ( drawer[track] == null ) {
                drawer[track] = new WaveDrawContext();
                drawer[track].load( file );
            } else {
                drawer[track].reloadPartial( file, sec_from, sec_to );
            }
        }

        public void load( int track, String wave_path ) {
            if ( track < 0 || drawer.Length <= track ) {
                return;
            }
            if ( drawer[track] == null ) {
                drawer[track] = new WaveDrawContext();
            }
            drawer[track].load( wave_path );
        }

        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint( e );
            paint( new Graphics2D( e.Graphics ) );
        }
    }

#if !JAVA
}
#endif
