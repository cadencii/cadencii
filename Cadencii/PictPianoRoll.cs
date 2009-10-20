/*
 * PictPianoRoll.cs
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
#if JAVA
package org.kbinani.Cadencii;
#else
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.awt;
using bocoree.util;

namespace Boare.Cadencii
{
    using boolean = System.Boolean;
    using java = bocoree;
#endif

    public class PictPianoRoll : PictureBox
    {
        public event KeyEventHandler BKeyDown;
        public event KeyEventHandler BKeyUp;

        private readonly Color s_brs_192_192_192 = new Color( 192, 192, 192 );
        private readonly Color s_brs_a098_000_000_000 = new Color( 0, 0, 0, 98 );
        private readonly Color s_brs_106_108_108 = new Color( 106, 108, 108 );
        private readonly Color s_brs_180_180_180 = new Color( 180, 180, 180 );
        private readonly Color s_brs_212_212_212 = new Color( 212, 212, 212 );
        private readonly Color s_brs_125_123_124 = new Color( 125, 123, 124 );
        private readonly Color s_brs_240_240_240 = new Color( 240, 240, 240 );
        private readonly Color s_brs_011_233_244 = new Color( 11, 233, 244 );
        private readonly Color s_brs_182_182_182 = new Color( 182, 182, 182 );
        private readonly Color s_brs_072_077_098 = new Color( 72, 77, 98 );
        private readonly Color s_brs_153_153_153 = new Color( 153, 153, 153 );
        private readonly Color s_brs_147_147_147 = new Color( 147, 147, 147 );
        private readonly Color s_brs_000_255_214 = new Color( 0, 255, 214 );
        private readonly Color s_pen_112_112_112 = new Color( 112, 112, 112 );
        private readonly Color s_pen_118_123_138 = new Color( 118, 123, 138 );
        private readonly Color s_pen_LU = new Color( 106, 52, 255, 128 );
        private readonly Color s_pen_RD = new Color( 40, 47, 255, 204 );
        private readonly Color s_pen_161_157_136 = new Color( 161, 157, 136 );
        private readonly Color s_pen_209_204_172 = new Color( 209, 204, 172 );
        private readonly Color s_pen_160_160_160 = new Color( 160, 160, 160 );
        private readonly Color s_pen_105_105_105 = new Color( 105, 105, 105 );
        private readonly Color s_pen_106_108_108 = new Color( 106, 108, 108 );
        private readonly Color s_pen_212_212_212 = new Color( 212, 212, 212 );
        private readonly Color s_pen_051_051_000 = new Color( 51, 51, 0 );
        private readonly Color s_pen_125_123_124 = new Color( 125, 123, 124 );
        private readonly Color s_pen_187_187_255 = new Color( 187, 187, 255 );
        private readonly Color s_pen_007_007_151 = new Color( 7, 7, 151 );
        private readonly Color s_pen_a136_000_000_000 = new Color( 0, 0, 0, 136 );
        public readonly Color[] s_HIDDEN = new Color[]{
            new Color( 181, 162, 123 ),
            new Color( 179, 181, 123 ),
            new Color( 157, 181, 123 ),
            new Color( 135, 181, 123 ),
            new Color( 123, 181, 133 ),
            new Color( 123, 181, 154 ),
            new Color( 123, 181, 176 ),
            new Color( 123, 164, 181 ),
            new Color( 123, 142, 181 ),
            new Color( 125, 123, 181 ),
            new Color( 169, 123, 181 ),
            new Color( 181, 123, 171 ),
            new Color( 181, 123, 149 ),
            new Color( 181, 123, 127 ),
            new Color( 181, 140, 123 ),
            new Color( 181, 126, 123 ) };
        private readonly Color s_note_fill = new Color( 181, 220, 86 );
        private readonly BasicStroke s_pen_dashed_171_171_171 = new BasicStroke( 1.0f, 0, 0, 10.0f, new float[] { 3.0f }, 0.0f );
        private readonly Font s_F9PT = new Font( java.awt.Font.SANS_SERIF, java.awt.Font.PLAIN, 9 );
        /// <summary>
        /// パフォーマンスカウンタ用バッファの容量
        /// </summary>
        private const int _NUM_PCOUNTER = 50;
        /// <summary>
        /// 表情線の先頭部分のピクセル幅
        /// </summary>
        private const int _PX_ACCENT_HEADER = 21;

        protected override void OnKeyDown( KeyEventArgs e )
        {
            if ( BKeyDown != null )
            {
                BKeyDown( this, e );
            }
        }

        protected override void OnKeyUp( KeyEventArgs e )
        {
            if ( BKeyUp != null )
            {
                BKeyUp( this, e );
            }
        }

        protected override void OnMouseDown( MouseEventArgs e )
        {
            base.OnMouseDown( e );
            this.Focus();
        }

        protected override void OnPaint( PaintEventArgs pe )
        {
            base.OnPaint( pe );
            paint( new Graphics2D( pe.Graphics ) );
        }

        private void paint( Graphics2D g )
        {
            Dimension window_size = new Dimension( this.Width, this.Height );
            System.Drawing.Point p = this.PointToClient( Control.MousePosition );
            Point mouse_position = new Point( p.X, p.Y );
            int start_draw_x = AppManager.startToDrawX;
            int start_draw_y = (AppManager.mainWindow != null) ? AppManager.mainWindow.getStartToDrawY() : 0;

            int width = window_size.width;
            int height = window_size.height;
            int track_height = AppManager.editorConfig.PxTrackHeight;
            int half_track_height = track_height / 2;
            // [screen_x] = 67 + [clock] * ScaleX - StartToDrawX + 6
            // [screen_y] = -1 * ([note] - 127) * TRACK_HEIGHT - StartToDrawY
            //
            // [screen_x] = [clock] * _scalex + 73 - StartToDrawX
            // [screen_y] = -[note] * TRACK_HEIGHT + 127 * TRACK_HEIGHT - StartToDrawY
            int xoffset = 6 + AppManager.KEY_LENGTH - start_draw_x;
            int yoffset = 127 * track_height - start_draw_y;
            //      ↓
            // [screen_x] = [clock] * _scalex + xoffset
            // [screen_y] = -[note] * TRACK_HEIGHT + yoffset
            int y, dy;
            float scalex = AppManager.scaleX;
            float inv_scalex = 1f / scalex;

            if ( AppManager.getSelectedEventCount() > 0 && AppManager.inputTextBox.Enabled )
            {
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                int event_x = (int)(original.Clock * scalex + xoffset);
                int event_y = -original.ID.Note * track_height + yoffset;
                AppManager.inputTextBox.Left = event_x + 4;
                AppManager.inputTextBox.Top = event_y + 2;
            }

            Color black = AppManager.editorConfig.PianorollColorVocalo2Black.getColor();
            Color white = AppManager.editorConfig.PianorollColorVocalo2White.getColor();
            Color bar = AppManager.editorConfig.PianorollColorVocalo2Bar.getColor();
            Color beat = AppManager.editorConfig.PianorollColorVocalo2Beat.getColor();
            String renderer = "";

            VsqFileEx vsq = AppManager.getVsqFile();
            EditMode edit_mode = AppManager.getEditMode();

            if ( vsq != null )
            {
                renderer = vsq.Track.get( AppManager.getSelected() ).getCommon().Version;
            }

            if ( renderer.StartsWith( VSTiProxy.RENDERER_UTU0 ) )
            {
                black = AppManager.editorConfig.PianorollColorUtauBlack.getColor();
                white = AppManager.editorConfig.PianorollColorUtauWhite.getColor();
                bar = AppManager.editorConfig.PianorollColorUtauBar.getColor();
                beat = AppManager.editorConfig.PianorollColorUtauBeat.getColor();
            }
            else if ( renderer.StartsWith( VSTiProxy.RENDERER_DSB2 ) )
            {
                black = AppManager.editorConfig.PianorollColorVocalo1Black.getColor();
                white = AppManager.editorConfig.PianorollColorVocalo1White.getColor();
                bar = AppManager.editorConfig.PianorollColorVocalo1Bar.getColor();
                beat = AppManager.editorConfig.PianorollColorVocalo1Beat.getColor();
            }
            else if ( renderer.StartsWith( VSTiProxy.RENDERER_STR0 ) )
            {
                black = AppManager.editorConfig.PianorollColorStraightBlack.getColor();
                white = AppManager.editorConfig.PianorollColorStraightWhite.getColor();
                bar = AppManager.editorConfig.PianorollColorStraightBar.getColor();
                beat = AppManager.editorConfig.PianorollColorStraightBeat.getColor();
            }

            #region ピアノロール周りのスクロールバーなど
            // スクロール画面背景
            int calculated_height = height;
            if ( calculated_height > 0 )
            {
                g.setColor( Color.white );
                g.fillRect( 3, 0, width, calculated_height );
                g.setColor( s_brs_240_240_240 );
                g.fillRect( 3, 0, AppManager.KEY_LENGTH, calculated_height );
            }
            // ピアノロールとカーブエディタの境界
            g.setColor( s_pen_112_112_112 );
            g.drawLine( 2, height - 1, width - 1, height - 1 );
            // 横スクロールバー
            //hScroll.BarLength = (int)((width - 152) * inv_scalex);
            // 縦スクロールバー
            calculated_height = height - 14;//height - 93 - 14;
            if ( calculated_height > 0 )
            {
                //vScroll.BarLength = (int)((float)vScroll.Maximum * vScroll.Height / (AppManager.EditorConfig.PxTrackHeight * 128f));
            }
            #endregion

            #region ピアノロール本体
            if ( vsq != null )
            {
                int odd = -1;
                y = 128 * track_height - start_draw_y;
                dy = -track_height;
                for ( int i = 0; i <= 127; i++ )
                {
                    odd++;
                    if ( odd == 12 )
                    {
                        odd = 0;
                    }
                    int order = (i - odd) / 12 - 2;
                    y += dy;
                    if ( y > height )
                    {
                        continue;
                    }
                    else if ( 0 > y + track_height )
                    {
                        break;
                    }
                    boolean note_is_whitekey = VsqNote.isNoteWhiteKey( i );

                    #region ピアノロール背景
                    Color b = Color.black;
                    Color border;
                    boolean paint_required = true;
                    if ( order == -2 || order == -1 || (6 <= order && order <= 8) )
                    {
                        if ( note_is_whitekey )
                        {
                            b = s_brs_180_180_180;
                        }
                        else
                        {
                            b = s_brs_106_108_108;
                        }
                        border = s_brs_106_108_108;
                    }
                    else if ( order == 5 || order == 0 )
                    {
                        if ( note_is_whitekey )
                        {
                            b = s_brs_212_212_212;
                        }
                        else
                        {
                            b = s_brs_180_180_180;
                        }
                        border = new Color( 150, 152, 150 );
                    }
                    else
                    {
                        if ( note_is_whitekey )
                        {
                            //paint_required = false;
                            b = white;// s_brs_240_240_240;
                        }
                        else
                        {
                            b = black;// s_brs_212_212_212;
                        }
                        border = new Color( 210, 205, 172 );
                    }
                    if ( paint_required )
                    {
                        g.setColor( b );
                        g.fillRect( AppManager.KEY_LENGTH, y, width - AppManager.KEY_LENGTH, track_height + 1 );
                    }
                    if ( odd == 0 || odd == 5 )
                    {
                        g.setColor( border );
                        g.drawLine( AppManager.KEY_LENGTH, y + track_height,
                                    width, y + track_height );
                    }
                    #endregion

                    #region プリメジャー部分のピアノロール背景
                    int premeasure_start_x = xoffset;
                    if ( premeasure_start_x < AppManager.KEY_LENGTH )
                    {
                        premeasure_start_x = AppManager.KEY_LENGTH;
                    }
                    int premeasure_end_x = (int)(AppManager.getVsqFile().getPreMeasureClocks() * scalex + xoffset);
                    if ( premeasure_start_x >= AppManager.KEY_LENGTH )
                    {
                        if ( note_is_whitekey )
                        {
                            g.setColor( s_brs_153_153_153 );
                            g.fillRect( premeasure_start_x, y,
                                        premeasure_end_x - premeasure_start_x, track_height + 1 );
                        }
                        else
                        {
                            g.setColor( s_brs_106_108_108 );
                            g.fillRect( premeasure_start_x, y,
                                        premeasure_end_x - premeasure_start_x, track_height + 1 );
                        }
                        if ( odd == 0 || odd == 5 )
                        {
                            g.setColor( s_pen_106_108_108 );
                            g.drawLine( premeasure_start_x, y + track_height,
                                        premeasure_end_x, y + track_height );
                        }
                    }
                    #endregion

                }
            }

            //ピアノロールと鍵盤部分の縦線
            int hilighted_note = -1;
            g.setColor( s_pen_212_212_212 );
            g.drawLine( AppManager.KEY_LENGTH, 0,
                        AppManager.KEY_LENGTH, height - 15 );
            int odd2 = -1;
            y = 128 * track_height - start_draw_y;
            dy = -track_height;
            for ( int i = 0; i <= 127; i++ )
            {
                odd2++;
                if ( odd2 == 12 )
                {
                    odd2 = 0;
                }
                y += dy;
                if ( y > height )
                {
                    continue;
                }
                else if ( y + track_height < 0 )
                {
                    break;
                }

                #region 鍵盤部分
                g.setColor( s_pen_212_212_212 );
                g.drawLine( 3, y, AppManager.KEY_LENGTH, y );
                boolean hilighted = false;
                if ( edit_mode == EditMode.ADD_ENTRY )
                {
                    if ( AppManager.addingEvent.ID.Note == i )
                    {
                        hilighted = true;
                        hilighted_note = i;
                    }
                }
                else if ( edit_mode == EditMode.EDIT_LEFT_EDGE || edit_mode == EditMode.EDIT_RIGHT_EDGE )
                {
#if DEBUG
                    //bocoree.debug.push_log( "(AppManager.LastSelectedEvent==null)=" + (AppManager.LastSelectedEvent == null) );
                    //bocoree.debug.push_log( "(AppManager.LastSelectedEvent.Original==null)=" + (AppManager.LastSelectedEvent.Original == null) );
#endif
                    if ( AppManager.getLastSelectedEvent().original.ID.Note == i )
                    { //TODO: ここでNullpointer exception
                        hilighted = true;
                        hilighted_note = i;
                    }
                }
                else
                {
                    if ( 3 <= mouse_position.x && mouse_position.x <= width - 17 &&
                        0 <= mouse_position.y && mouse_position.y <= height - 1 )
                    {
                        if ( y <= mouse_position.y && mouse_position.y < y + track_height )
                        {
                            hilighted = true;
                            hilighted_note = i;
                        }
                    }
                }
                if ( hilighted )
                {
                    g.setColor( AppManager.getHilightColor() );
                    g.fillRect( 35, y, AppManager.KEY_LENGTH - 35, track_height );
                }
                if ( odd2 == 0 || hilighted )
                {
                    g.setColor( s_brs_072_077_098 );
                    g.setFont( AppManager.baseFont8 );
                    g.drawString( VsqNote.getNoteString( i ), 42, y + half_track_height - AppManager.baseFont8OffsetHeight + 1 );
                }
                if ( !VsqNote.isNoteWhiteKey( i ) )
                {
                    g.setColor( s_brs_125_123_124 );
                    g.fillRect( 0, y, 34, track_height );
                }
                #endregion
            }
            g.setClip( null );

            g.clipRect( AppManager.KEY_LENGTH, 0, width - AppManager.KEY_LENGTH, height );
            #region 小節ごとの線
            int dashed_line_step = AppManager.getPositionQuantizeClock();
            //using ( Pen pen_bar = new Pen( bar ) )
            //using ( Pen pen_beat = new Pen( beat ) ) {
            for ( Iterator itr = vsq.getBarLineIterator( AppManager.clockFromXCoord( width ) ); itr.hasNext(); )
            {
                VsqBarLineType blt = (VsqBarLineType)itr.next();
                int local_clock_step = 1920 / blt.getLocalDenominator();
                int x = (int)(blt.clock() * scalex + xoffset);
                if ( blt.isSeparator() )
                {
                    //ピアノロール上
                    g.setColor( bar );
                    g.drawLine( x, 0, x, height );
                }
                else
                {
                    //ピアノロール上
                    g.setColor( beat );
                    g.drawLine( x, 0, x, height );
                }
                if ( dashed_line_step > 1 && AppManager.isGridVisible() )
                {
                    int numDashedLine = local_clock_step / dashed_line_step;
                    g.setColor( beat );
                    for ( int i = 1; i < numDashedLine; i++ )
                    {
                        int x2 = (int)((blt.clock() + i * dashed_line_step) * scalex + xoffset);
                        g.drawLine( x2, 0, x2, height );
                    }
                }
            }
            //}
            #endregion

            #region トラックのエントリを描画
            if ( AppManager.drawObjects != null )
            {
                if ( AppManager.isOverlay() )
                {
                    // まず、選択されていないトラックの簡易表示を行う
                    lock ( AppManager.drawObjects )
                    {
                        int c = AppManager.drawObjects.size();
                        for ( int i = 0; i < c; i++ )
                        {
                            if ( i == AppManager.getSelected() - 1 )
                            {
                                continue;
                            }
                            Vector<DrawObject> target_list = AppManager.drawObjects.get( i );
                            int j_start = AppManager.drawStartIndex[i];
                            boolean first = true;
                            int shift_center = AppManager.editorConfig.PxTrackHeight / 2;
                            int target_list_count = target_list.size();
                            for ( int j = j_start; j < target_list_count; j++ )
                            {
                                DrawObject dobj = target_list.get( j );
                                int x = dobj.pxRectangle.x - start_draw_x;
                                y = dobj.pxRectangle.y - start_draw_y;
                                int lyric_width = dobj.pxRectangle.width;
                                if ( x + lyric_width < 0 )
                                {
                                    continue;
                                }
                                else if ( width < x )
                                {
                                    break;
                                }
                                if ( AppManager.isPlaying() && first )
                                {
                                    AppManager.drawStartIndex[i] = j;
                                    first = false;
                                }
                                if ( y + track_height < 0 || y > height )
                                {
                                    continue;
                                }
                                g.setColor( AppManager.HILIGHT[i] );
                                g.drawLine( x + 1, y + shift_center,
                                            x + lyric_width - 1, y + shift_center );
                                g.setColor( s_HIDDEN[i] );
                                g.drawPolygon( new int[] { x, x + 1, x + lyric_width - 1, x + lyric_width, x + lyric_width - 1, x + 1, x },
                                               new int[] { y + shift_center, y + shift_center - 1, y + shift_center - 1, y + shift_center, y + shift_center + 1, y + shift_center + 1, y + shift_center },
                                               7 );
                            }
                        }
                    }
                }

                // 選択されているトラックの表示を行う
                int selected = AppManager.getSelected();
                boolean show_lyrics = AppManager.editorConfig.ShowLyric;
                boolean show_exp_line = AppManager.editorConfig.ShowExpLine;
                if ( selected >= 1 )
                {
                    Shape r = g.getClip();
                    g.clipRect( AppManager.KEY_LENGTH, 0,
                                this.Width - AppManager.KEY_LENGTH, this.Height );
                    int j_start = AppManager.drawStartIndex[selected - 1];

                    boolean first = true;
                    lock ( AppManager.drawObjects )
                    { //ここでロックを取得しないと、描画中にUpdateDrawObjectのサイズが0になる可能性がある
                        if ( selected - 1 < AppManager.drawObjects.size() )
                        {
                            Vector<DrawObject> target_list = AppManager.drawObjects.get( selected - 1 );
                            int c = target_list.size();
                            for ( int j = j_start; j < c; j++ )
                            {
                                DrawObject dobj = target_list.get( j );
                                int x = dobj.pxRectangle.x - start_draw_x;
                                y = dobj.pxRectangle.y - start_draw_y;
                                int lyric_width = dobj.pxRectangle.width;
                                if ( x + lyric_width < 0 )
                                {
                                    continue;
                                }
                                else if ( width < x )
                                {
                                    break;
                                }
                                if ( AppManager.isPlaying() && first )
                                {
                                    AppManager.drawStartIndex[selected - 1] = j;
                                    first = false;
                                }
                                if ( y + 2 * track_height < 0 || y > height )
                                {
                                    continue;
                                }
                                Color id_fill;
                                if ( AppManager.getSelectedEventCount() > 0 )
                                {
                                    boolean found = AppManager.isSelectedEventContains( AppManager.getSelected(), dobj.internalID );
                                    if ( found )
                                    {
                                        id_fill = AppManager.getHilightColor();
                                    }
                                    else
                                    {
                                        id_fill = s_note_fill;
                                    }
                                }
                                else
                                {
                                    id_fill = s_note_fill;
                                }
                                g.setColor( id_fill );
                                g.fillRect( x, y + 1, lyric_width, track_height - 1 );
                                Font lyric_font = dobj.symbolProtected ? AppManager.baseFont10Bold : AppManager.baseFont10;
                                if ( dobj.overlappe )
                                {
                                    g.setColor( s_pen_125_123_124 );
                                    g.drawRect( x, y + 1, lyric_width, track_height - 1 );
                                    if ( show_lyrics )
                                    {
                                        g.setFont( lyric_font );
                                        g.setColor( s_brs_147_147_147 );
                                        g.drawString( dobj.text, x + 1, y + half_track_height - AppManager.baseFont10OffsetHeight + 1 );
                                    }
                                }
                                else
                                {
                                    g.setColor( s_pen_125_123_124 );
                                    g.drawRect( x, y + 1, lyric_width, track_height - 1 );
                                    if ( show_lyrics )
                                    {
#if DEBUG
                                        g.setFont( lyric_font );
                                        g.setColor( Color.black );
                                        g.drawString( dobj.text + "(ID:" + dobj.internalID + ")", x + 1, y + half_track_height - AppManager.baseFont10OffsetHeight + 1 );
#else
                                        g.setFont( lyric_font );
                                        g.setColor( Color.black );
                                        g.drawString( dobj.text, x + 1, y + half_track_height - AppManager.baseFont10OffsetHeight + 1 );
#endif
                                    }
                                    if ( show_exp_line && lyric_width > 21 )
                                    {
                                        #region 表情線
                                        DrawAccentLine( g, new Point( x, y + track_height + 1 ), dobj.accent );
                                        int vibrato_start = x + lyric_width;
                                        int vibrato_end = x;
                                        if ( dobj.pxVibratoDelay <= lyric_width )
                                        {
                                            int vibrato_delay = dobj.pxVibratoDelay;
                                            int vibrato_width = dobj.pxRectangle.width - vibrato_delay;
                                            vibrato_start = x + vibrato_delay;
                                            vibrato_end = x + vibrato_delay + vibrato_width;
                                            if ( vibrato_start - x < 21 )
                                            {
                                                vibrato_start = x + 21;
                                            }
                                        }
                                        g.setColor( s_pen_051_051_000 );
                                        g.drawLine( x + 21, y + track_height + 7,
                                                    vibrato_start, y + track_height + 7 );
                                        if ( dobj.pxVibratoDelay <= lyric_width )
                                        {
                                            int next_draw = vibrato_start;
                                            if ( vibrato_start < vibrato_end )
                                            {
                                                drawVibratoLine( g,
                                                                 new Point( vibrato_start, y + track_height + 1 ),
                                                                 vibrato_end - vibrato_start );
                                            }
                                        }
                                        #endregion
                                    }
                                    // ビブラートがあれば
                                    if ( AppManager.editorConfig.ViewAtcualPitch )
                                    {
                                        if ( dobj.vibRate != null )
                                        {
                                            int vibrato_delay = dobj.pxVibratoDelay;
                                            int vibrato_width = dobj.pxRectangle.width - vibrato_delay;
                                            int vibrato_start = x + vibrato_delay;
                                            int vibrato_end = x + vibrato_delay + vibrato_width;
                                            int cl_sx = AppManager.clockFromXCoord( vibrato_start );
                                            int cl_ex = AppManager.clockFromXCoord( vibrato_end );
                                            drawVibratoPitchbend( g,
                                                                  dobj.vibRate,
                                                                  dobj.vibStartRate,
                                                                  dobj.vibDepth,
                                                                  dobj.vibStartDepth,
                                                                  dobj.note,
                                                                  vibrato_start,
                                                                  vibrato_width );
                                        }
                                    }
                                }
                            }
                        }
                    }
                    g.setClip( r );
                }

                // 編集中のエントリを表示
                if ( edit_mode == EditMode.ADD_ENTRY ||
                     edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY ||
                     edit_mode == EditMode.REALTIME )
                {
                    if ( AppManager.addingEvent != null )
                    {
                        int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
                        y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
                        if ( AppManager.addingEvent.ID.Length <= 0 )
                        {
                            //g.setStroke( s_pen_dashed_171_171_171 );
                            g.setColor( new Color( 171, 171, 171 ) );
                            g.drawRect( x, y, 10, track_height - 1 );
                            //g.setStroke( new BasicStroke() );
                        }
                        else
                        {
                            int length = (int)(AppManager.addingEvent.ID.Length * scalex);
                            g.setColor( s_pen_a136_000_000_000 );
                            g.drawRect( x, y, length, track_height - 1 );
                        }
                    }
                }
                else if ( edit_mode == EditMode.EDIT_VIBRATO_DELAY )
                {
                    int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
                    y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
                    int length = (int)(AppManager.addingEvent.ID.Length * scalex);
                    g.setColor( s_pen_a136_000_000_000 );
                    g.drawRect( x, y, length, track_height - 1 );
                }
                else if ( (edit_mode == EditMode.MOVE_ENTRY ||
                           edit_mode == EditMode.MOVE_ENTRY_WHOLE ||
                           edit_mode == EditMode.EDIT_LEFT_EDGE ||
                           edit_mode == EditMode.EDIT_RIGHT_EDGE) && AppManager.getSelectedEventCount() > 0 )
                {
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); )
                    {
                        SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                        int x = (int)(ev.editing.Clock * scalex + xoffset);
                        y = -ev.editing.ID.Note * track_height + yoffset + 1;
                        if ( ev.editing.ID.Length == 0 )
                        {
                            g.setColor( new Color( 171, 171, 171 ) );
                            g.setStroke( s_pen_dashed_171_171_171 );
                            g.drawRect( x, y, 10, track_height - 1 );
                            g.setStroke( new BasicStroke() );
                        }
                        else
                        {
                            int length = (int)(ev.editing.ID.Length * scalex);
                            g.setColor( s_pen_a136_000_000_000 );
                            g.drawRect( x, y, length, track_height - 1 );
                        }
                    }

                    if ( edit_mode == EditMode.MOVE_ENTRY_WHOLE )
                    {
                        int clock_start = AppManager.wholeSelectedInterval.Start;
                        int clock_end = AppManager.wholeSelectedInterval.End;
                        int x_start = AppManager.xCoordFromClocks( AppManager.wholeSelectedIntervalStartForMoving );
                        int x_end = AppManager.xCoordFromClocks( AppManager.wholeSelectedIntervalStartForMoving + (clock_end - clock_start) );
                        g.setColor( s_brs_a098_000_000_000 );
                        g.drawLine( x_start, 0, x_start, height );
                        g.drawLine( x_end, 0, x_end, height );
                    }
                }
            }
            #endregion

            g.setClip( null );

            #endregion

            #region 音符編集時の補助線
            if ( edit_mode == EditMode.ADD_ENTRY )
            {
                #region EditMode.AddEntry
                int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
                y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
                int length;
                if ( AppManager.addingEvent.ID.Length == 0 )
                {
                    length = 10;
                }
                else
                {
                    length = (int)(AppManager.addingEvent.ID.Length * scalex);
                }
                x += length;
                g.setColor( s_pen_LU );
                g.drawLine( x, 0, x, y - 1 );
                g.drawLine( x, y + track_height, x, height );
                g.setColor( s_pen_RD );
                g.drawLine( x + 1, 0, x + 1, y - 1 );
                g.drawLine( x + 1, y + track_height, x + 1, height );
                #endregion
            }
            else if ( edit_mode == EditMode.MOVE_ENTRY || edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE )
            {
                #region EditMode.MoveEntry || EditMode.MoveEntryWaitMove
                if ( AppManager.getSelectedEventCount() > 0 )
                {
                    VsqEvent last = AppManager.getLastSelectedEvent().editing;
                    int x = (int)(last.Clock * scalex + xoffset);
                    y = -last.ID.Note * track_height + yoffset + 1;
                    int length = (int)(last.ID.Length * scalex);
                    // 縦線
                    g.setColor( s_pen_LU );
                    g.drawLine( x, 0, x, y - 1 );
                    g.drawLine( x, y + track_height, x, height );
                    // 横線
                    g.drawLine( AppManager.KEY_LENGTH, y + track_height / 2 - 2,
                                x - 1, y + track_height / 2 - 2 );
                    g.drawLine( x + length + 1, y + track_height / 2 - 2,
                                width, y + track_height / 2 - 2 );
                    // 縦線
                    g.setColor( s_pen_RD );
                    g.drawLine( x + 1, 0, x + 1, y - 1 );
                    g.drawLine( x + 1, y + track_height,
                                x + 1, height );
                    // 横線
                    g.drawLine( AppManager.KEY_LENGTH, y + track_height / 2 - 1,
                                x - 1, y + track_height / 2 - 1 );
                    g.drawLine( x + length + 1, y + track_height / 2 - 1,
                                width, y + track_height / 2 - 1 );
                }
                #endregion
            }
            else if ( edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY )
            {
                #region EditMode.MoveEntry
                int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
                y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
                int length = (int)(AppManager.addingEvent.ID.Length * scalex);
                // 縦線
                g.setColor( s_pen_LU );
                g.drawLine( x, 0, x, y - 1 );
                g.drawLine( x, y + track_height, x, height );
                // 横線
                g.drawLine( AppManager.KEY_LENGTH, y + track_height / 2 - 2,
                            x - 1, y + track_height / 2 - 2 );
                g.drawLine( x + length + 1, y + track_height / 2 - 2,
                            width, y + track_height / 2 - 2 );
                // 縦線
                g.setColor( s_pen_RD );
                g.drawLine( x + 1, 0, x + 1, y - 1 );
                g.drawLine( x + 1, y + track_height, x + 1, height );
                // 横線
                g.drawLine( AppManager.KEY_LENGTH, y + track_height / 2 - 1,
                            x - 1, y + track_height / 2 - 1 );
                g.drawLine( x + length + 1, y + track_height / 2 - 1,
                            width, y + track_height / 2 - 1 );
                #endregion
            }
            else if ( edit_mode == EditMode.EDIT_LEFT_EDGE )
            {
                #region EditMode.EditLeftEdge
                VsqEvent last = AppManager.getLastSelectedEvent().editing;
                int x = (int)(last.Clock * scalex + xoffset);
                y = -last.ID.Note * track_height + yoffset + 1;
                g.setColor( s_pen_LU );
                g.drawLine( x, 0, x, y - 1 );
                g.drawLine( x, y + track_height, x, height );
                g.setColor( s_pen_RD );
                g.drawLine( x + 1, 0, x + 1, y - 1 );
                g.drawLine( x + 1, y + track_height, x + 1, height );
                #endregion
            }
            else if ( edit_mode == EditMode.EDIT_RIGHT_EDGE )
            {
                #region EditMode.EditRightEdge
                VsqEvent last = AppManager.getLastSelectedEvent().editing;
                int x = (int)(last.Clock * scalex + xoffset);
                y = -last.ID.Note * track_height + yoffset + 1;
                int length = (int)(last.ID.Length * scalex);
                x += length;
                g.setColor( s_pen_LU );
                g.drawLine( x, 0, x, y - 1 );
                g.drawLine( x, y + track_height, x, height );
                g.setColor( s_pen_RD );
                g.drawLine( x + 1, 0, x + 1, y - 1 );
                g.drawLine( x + 1, y + track_height, x + 1, height );
                #endregion
            }
            else if ( edit_mode == EditMode.EDIT_VIBRATO_DELAY )
            {
                #region EditVibratoDelay
                int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
                y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
                g.setColor( s_pen_LU );
                g.drawLine( x, 0, x, y - 1 );
                g.drawLine( x, y + track_height, x, height );
                g.setColor( s_pen_RD );
                g.drawLine( x + 1, 0, x + 1, y - 1 );
                g.drawLine( x + 1, y + track_height, x + 1, height );
                double max_length = AppManager.addingEventLength - _PX_ACCENT_HEADER / scalex;
                double drate = AppManager.addingEvent.ID.Length / max_length;
                if ( drate > 0.99 )
                {
                    drate = 1.00;
                }
                int rate = (int)(drate * 100.0);
                String percent = rate + "%";
                Dimension size = Util.measureString( percent, s_F9PT );
                int delay_x = (int)((AppManager.addingEvent.Clock + AppManager.addingEvent.ID.Length - AppManager.addingEventLength + AppManager.addingEvent.ID.VibratoDelay) * scalex + xoffset);
                Rectangle pxArea = new Rectangle( delay_x,
                                                  (int)(y + track_height * 2.5),
                                                  (int)(size.width * 1.2),
                                                  (int)(size.height * 1.2) );
                g.setColor( s_brs_192_192_192 );
                g.fillRect( pxArea.x, pxArea.y, pxArea.width, pxArea.height );
                g.setColor( Color.black );
                g.drawRect( pxArea.x, pxArea.y, pxArea.width, pxArea.height );
                // StringFormat sf = new StringFormat();
                //sf.Alignment = StringAlignment.Center;
                //sf.LineAlignment = StringAlignment.Center;
                g.setFont( s_F9PT );
                g.drawString( percent, pxArea.x, pxArea.y );// , sf );
                #endregion
            }

            // マウス位置での音階名
            if ( hilighted_note >= 0 )
            {
                int align = 1;
                int valign = 0;
                g.setColor( Color.black );
                PortUtil.drawStringEx( g,
                                       VsqNote.getNoteString( hilighted_note ),
                                       AppManager.baseFont10Bold,
                                       new Rectangle( mouse_position.x - 110, mouse_position.y - 50, 100, 100 ),
                                       align,
                                       valign );
            }
            #endregion

            #region 外枠
            // 左(外側)
            g.setColor( s_pen_160_160_160 );
            g.drawLine( 0, 0, 0, height );
            // 左(内側)
            g.setColor( s_pen_105_105_105 );
            g.drawLine( 1, 0, 1, height );
            #endregion

            #region pictPianoRoll_Paintより
            // マーカー
            int marker_x = (int)(AppManager.getCurrentClock() * AppManager.scaleX + 6 + AppManager.KEY_LENGTH - AppManager.startToDrawX);
            if ( AppManager.KEY_LENGTH <= marker_x && marker_x <= this.Width )
            {
                g.setColor( Color.white );
                g.setStroke( new BasicStroke( 2f ) );
                g.drawLine( marker_x, 0, marker_x, this.Height );
                g.setStroke( new BasicStroke() );
            }

            /*DateTime dnow = DateTime.Now;
            for ( int i = 0; i < _NUM_PCOUNTER - 1; i++ )
            {
                m_performance[i] = m_performance[i + 1];
            }
            m_performance[_NUM_PCOUNTER - 1] = (float)dnow.Subtract( m_last_ignitted ).TotalSeconds;
            m_last_ignitted = dnow;
            float sum = 0f;
            for ( int i = 0; i < _NUM_PCOUNTER; i++ )
            {
                sum += m_performance[i];
            }
            m_fps = _NUM_PCOUNTER / sum;*/

            if ( AppManager.isWholeSelectedIntervalEnabled() )
            {
                //int start = AppManager.xCoordFromClocks( AppManager.wholeSelectedInterval.Start );
                //int end = AppManager.xCoordFromClocks( AppManager.wholeSelectedInterval.End );
                int start = (int)(AppManager.wholeSelectedInterval.Start * scalex) + xoffset + 2;
                if ( start < AppManager.KEY_LENGTH )
                {
                    start = AppManager.KEY_LENGTH;
                }
                int end = (int)(AppManager.wholeSelectedInterval.End * scalex) + xoffset + 2;
                if ( start < end )
                {
                    g.setColor( new Color( 0, 0, 0, 98 ) );
                    g.fillRect( start, 0, end - start, this.Height );
                }
            }
            else if ( AppManager.isPointerDowned )
            {
                System.Drawing.Point pmouse = this.PointToClient( Control.MousePosition );
                Point mouse = new Point( pmouse.X, pmouse.Y );
                int tx, ty, twidth, theight;
                int lx = AppManager.mouseDownLocation.x - AppManager.startToDrawX;
                if ( lx < mouse.x )
                {
                    tx = lx;
                    twidth = mouse.x - lx;
                }
                else
                {
                    tx = mouse.x;
                    twidth = lx - mouse.x;
                }
                int ly = AppManager.mouseDownLocation.y - start_draw_y;
                if ( ly < mouse.y )
                {
                    ty = ly;
                    theight = mouse.y - ly;
                }
                else
                {
                    ty = mouse.y;
                    theight = ly - mouse.y;
                }
                if ( tx < AppManager.KEY_LENGTH )
                {
                    int txold = tx;
                    tx = AppManager.KEY_LENGTH;
                    twidth -= (tx - txold);
                }
                Rectangle rc = new Rectangle( tx, ty, twidth, theight );
                Color pen = new Color( 0, 0, 0, 200 );
                g.setColor( new Color( 0, 0, 0, 100 ) );
                g.fillRect( rc.x, rc.y, rc.width, rc.height );
#if !JAVA
                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#endif
                g.setStroke( new BasicStroke( 1.0f, 0, BasicStroke.JOIN_ROUND ) );
                g.setColor( pen );
                g.drawRect( rc.x, rc.y, rc.width, rc.height );
            }
#if MONITOR_FPS
            e.Graphics.DrawString(
                m_fps.ToString( "000.000" ),
                new Font( "Verdana", 40, FontStyle.Bold ),
                Brushes.Red,
                new PointF( 0, 0 ) );
#endif
            #endregion

        }

        /// <summary>
        /// アクセントを表す表情線を、指定された位置を基準点として描き込みます
        /// </summary>
        /// <param name="g"></param>
        /// <param name="accent"></param>
        private void DrawAccentLine( Graphics2D g, Point origin, int accent )
        {
            int x0 = origin.x + 1;
            int y0 = origin.y + 10;
            int height = 4 + accent * 4 / 100;
            //SmoothingMode sm = g.SmoothingMode;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            g.setColor( Color.black );
            g.drawPolygon( new int[] { x0, x0 + 2, x0 + 8, x0 + 13, x0 + 16, x0 + 20 },
                           new int[] { y0, y0, y0 - height, y0, y0, y0 - 4 },
                           6 );
            //g.SmoothingMode = sm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">描画に使用するグラフィクス</param>
        /// <param name="rate">Vibrato Rate</param>
        /// <param name="start_rate">Vibrato Rateの開始値</param>
        /// <param name="depth">Vibrato Depth</param>
        /// <param name="start_depth">Vibrato Depthの開始値</param>
        /// <param name="note">描画する音符のノートナンバー</param>
        /// <param name="clock_start">ビブラートが始まるクロック位置</param>
        /// <param name="clock_width">ビブラートのクロック長さ</param>
        private void drawVibratoPitchbend( Graphics2D g,
                                           VibratoBPList rate,
                                           int start_rate,
                                           VibratoBPList depth,
                                           int start_depth,
                                           int note,
                                           int x_start,
                                           int px_width )
        {
            if ( rate == null || depth == null )
            {
                return;
            }
            int y0 = AppManager.mainWindow != null ? AppManager.mainWindow.yCoordFromNote( note - 0.5f ) : 0;
            float px_track_height = AppManager.editorConfig.PxTrackHeight;
            VsqFileEx vsq = AppManager.getVsqFile();
            int clock_start = AppManager.clockFromXCoord( x_start );
            int clock_end = AppManager.clockFromXCoord( x_start + px_width );
            int tempo = vsq.getTempoAt( clock_start );
            Vector<PointD> ret = FormMain.getVibratoPoints( vsq,
                                                 rate,
                                                 start_rate,
                                                 depth,
                                                 start_depth,
                                                 clock_start,
                                                 clock_end - clock_start,
                                                 (float)(tempo * 1e-6 / 480.0) );
            int count = ret.size();
            if ( count >= 2 )
            {
                int[] drawx = new int[count];
                int[] drawy = new int[count];
                for ( int i = 0; i < count; i++ )
                {
                    PointD p = ret.get( i );
                    drawx[i] = AppManager.xCoordFromClocks( vsq.getClockFromSec( p.getX() ) );
                    drawy[i] = (int)(p.getY() * px_track_height + y0);
                }
#if !JAVA
                System.Drawing.Drawing2D.SmoothingMode sm = g.nativeGraphics.SmoothingMode;
                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#endif
                g.setColor( Color.blue );
                g.drawPolygon( drawx, drawy, count );
#if !JAVA
                g.nativeGraphics.SmoothingMode = sm;
#endif
            }
        }

        private void drawVibratoLine( Graphics2D g, Point origin, int vibrato_length )
        {
            int x0 = origin.x + 1;
            int y0 = origin.y + 10;
            int clipx = origin.x + 1;
            int clip_length = vibrato_length;
            if ( clipx < AppManager.KEY_LENGTH )
            {
                clipx = AppManager.KEY_LENGTH;
                clip_length = origin.x + 1 + vibrato_length - AppManager.KEY_LENGTH;
                if ( clip_length <= 0 )
                {
                    return;
                }
            }
            //SmoothingMode sm = g.SmoothingMode;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            const int _UWID = 10;
            int count = vibrato_length / _UWID + 1;
            int[] _BASE_X = new int[] { x0 - _UWID, x0 + 2 - _UWID, x0 + 4 - _UWID, x0 + 7 - _UWID, x0 + 9 - _UWID, x0 + 10 - _UWID };
            int[] _BASE_Y = new int[] { y0 - 4, y0 - 7, y0 - 7, y0 - 1, y0 - 1, y0 - 4 };
            Shape old = g.getClip();
            g.clipRect( clipx, origin.y + 10 - 8, clip_length, 10 );
            g.setColor( Color.black );
            for ( int i = 0; i < count; i++ )
            {
                for ( int j = 0; j < _BASE_X.Length; j++ )
                {
                    _BASE_X[j] += _UWID;
                }
                g.drawPolygon( _BASE_X, _BASE_Y, _BASE_X.Length );
            }
            //g.SmoothingMode = sm;
            g.setClip( old );
        }
    }

#if !JAVA
}
#endif
