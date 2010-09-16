/*
 * CircuitView.cs
 * Copyright (C) 2010 kbinani
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

import java.util.*;
import java.awt.*;
import javax.swing.*;
import org.kbinani.windows.forms.*;
#else
using org.kbinani.java.util;
using org.kbinani.java.awt;
using org.kbinani.javax.swing;
using org.kbinani.windows.forms;

//TODO: draftが格上げになったら消すこと
using org.kbinani.cadencii.draft;

namespace org.kbinani.cadencii {
#endif

    /// <summary>
    /// シンセサイザ等の接続回路を編集するためのコンポーネント
    /// </summary>
#if JAVA
    public class CircuitView extends BPictureBox {
#else
    public class CircuitView : BPictureBox {
        /// <summary>
        /// ポートを表す丸印の半径
        /// </summary>
        private const int PORT_RADIUS = 3;
        
        private Circuit mCircuit = null;
        private Graphics mGraphics = null;
#endif
        public void paint( Graphics g ) {
            int diam = PORT_RADIUS * 2 + 1;
            for( int i = 0; i < mCircuit.mUnits.size(); i++ ){
                WaveUnit unit = mCircuit.mUnits.get( i );
                Point pos = mCircuit.mConfig.DrawPosition.get( i );

                int ports_in = mCircuit.mNumPortsIn.get( i );
                int ports_out = mCircuit.mNumPortsOut.get( i );
                int width = WaveUnit.BASE_WIDTH;
                int height = WaveUnit.BASE_HEIGHT_PER_PORTS * ((ports_out > ports_in ? ports_out : ports_in) + 1);

                unit.paintTo( g, pos.x, pos.y, width, height );
                
                // 入力ポートの丸印を描く
                if ( ports_in > 0 ) {
                    int delta_h = height / ports_in;
                    for ( int k = 0; k < ports_in; k++ ) {
                        int x = pos.x - PORT_RADIUS;
                        int y = pos.y + delta_h / 2 + k * delta_h - PORT_RADIUS;
                        g.setColor( Color.white );
                        g.fillOval( x, y, diam, diam );
                        g.setColor( Color.black );
                        g.drawOval( x, y, diam, diam );
                    }
                }

                // 出力ポートの丸印を描く
                if ( ports_out > 0 ) {
                    int delta_h = height / ports_out;
                    for ( int k = 0; k < ports_out; k++ ) {
                        int x = pos.x + width - PORT_RADIUS;
                        int y = pos.y + delta_h / 2 + k * delta_h - PORT_RADIUS;
                        g.setColor( Color.white );
                        g.fillOval( x, y, diam, diam );
                        g.setColor( Color.black );
                        g.drawOval( x, y, diam, diam );
                    }
                }
            }
        }

        protected override void OnPaint( System.Windows.Forms.PaintEventArgs pevent ) {
            base.OnPaint( pevent );
            if ( mGraphics == null ) {
                mGraphics = new Graphics( pevent.Graphics );
            } else {
                mGraphics.nativeGraphics = pevent.Graphics;
            }
            paint( mGraphics );
        }

        public void setCircuit( Circuit value ) {
            mCircuit = value;
        }
    }

#if !JAVA
}
#endif
