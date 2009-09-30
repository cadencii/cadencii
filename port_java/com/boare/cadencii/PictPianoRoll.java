/*
 * PictPianoRoll.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.cadencii.
 *
 * com.boare.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * com.boare.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

import java.awt.*;
import javax.swing.*;
import com.boare.vsq.*;

public class PictPianoRoll extends JPanel{
    public void paintComponent( Graphics g ){
        /*int start_draw_x = AppManager.StartToDrawX;
        int start_draw_y = StartToDrawY;

        int width = window_size.Width;
        int height = window_size.Height;
        int track_height = AppManager.EditorConfig.PxTrackHeight;
        int half_track_height = track_height / 2;
        // [screen_x] = 67 + [clock] * ScaleX - StartToDrawX + 6
        // [screen_y] = -1 * ([note] - 127) * TRACK_HEIGHT - StartToDrawY
        //
        // [screen_x] = [clock] * _scalex + 73 - StartToDrawX
        // [screen_y] = -[note] * TRACK_HEIGHT + 127 * TRACK_HEIGHT - StartToDrawY
        int xoffset = 6 + AppManager._KEY_LENGTH - start_draw_x;
        int yoffset = 127 * track_height - start_draw_y;
        //      ↓
        // [screen_x] = [clock] * _scalex + xoffset
        // [screen_y] = -[note] * TRACK_HEIGHT + yoffset
        int y, dy;
        float scalex = AppManager.ScaleX;
        float inv_scalex = 1f / scalex;

        if ( AppManager.SelectedEvent.Count > 0 && m_input_textbox.Enabled ) {
            VsqEvent original = AppManager.SelectedEvent.LastSelected.Original;
            int event_x = (int)(original.Clock * scalex + xoffset);
            int event_y = -original.ID.Note * track_height + yoffset;
            m_input_textbox.Left = event_x + 4;
            m_input_textbox.Top = event_y + 2;
        }*/

        Color black = Color.BLACK;
        Color white = Color.WHITE;
        String renderer = "";
        /*if ( AppManager.VsqFile != null ) {
            renderer = AppManager.VsqFile.Track[AppManager.Selected].getCommon().Version;
        }
        if ( renderer.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
            black = AppManager.EditorConfig.PianorollColorUtauBlack.Color;
            white = AppManager.EditorConfig.PianorollColorUtauWhite.Color;
        } else if ( renderer.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
            black = AppManager.EditorConfig.PianorollColorVocalo1Black.Color;
            white = AppManager.EditorConfig.PianorollColorVocalo1White.Color;
        } else {
            black = AppManager.EditorConfig.PianorollColorVocalo2Black.Color;
            white = AppManager.EditorConfig.PianorollColorVocalo2White.Color;
        }

        Dictionary<int, Rectangle> property_relateditems = new Dictionary<int, Rectangle>();
        #region ピアノロール周りのスクロールバーなど
        // スクロール画面背景
            int calculated_height = height;
            if ( calculated_height > 0 ) {
                g.FillRectangle( new SolidBrush( white ),
                                 new Rectangle( 3, 0, width, calculated_height ) );
                g.FillRectangle( s_brs_240_240_240,
                                 new Rectangle( 3, 0, AppManager._KEY_LENGTH, calculated_height ) );
            }
            // ピアノロールとカーブエディタの境界
            g.DrawLine( s_pen_112_112_112,
                        new Point( 2, height - 1 ),
                        new Point( width - 1, height - 1 ) );
            // 横スクロールバー
            //hScroll.BarLength = (int)((width - 152) * inv_scalex);
            // 縦スクロールバー
            calculated_height = height - 14;//height - 93 - 14;
            if ( calculated_height > 0 ) {
                //vScroll.BarLength = (int)((float)vScroll.Maximum * vScroll.Height / (AppManager.EditorConfig.PxTrackHeight * 128f));
            }
            #endregion

            #region ピアノロール本体
            if ( AppManager.VsqFile != null ) {
                int odd = -1;
                y = 128 * track_height - start_draw_y;
                dy = -track_height;
                for ( int i = 0; i <= 127; i++ ) {
                    odd++;
                    if ( odd == 12 ) {
                        odd = 0;
                    }
                    int order = (i - odd) / 12 - 2;
                    y += dy;
                    if ( y > height ) {
                        continue;
                    } else if ( 0 > y + track_height ) {
                        break;
                    }
                    bool note_is_whitekey = VsqNote.isNoteWhiteKey( i );

                    #region ピアノロール背景
                    SolidBrush b = new SolidBrush( Color.Black );
                    SolidBrush border;
                    bool paint_required = true;
                    if ( order == -2 || order == -1 || (6 <= order && order <= 8) ) {
                        if ( note_is_whitekey ) {
                            b = s_brs_180_180_180;
                        } else {
                            b = s_brs_106_108_108;
                        }
                        border = s_brs_106_108_108;
                    } else if ( order == 5 || order == 0 ) {
                        if ( note_is_whitekey ) {
                            b = s_brs_212_212_212;
                        } else {
                            b = s_brs_180_180_180;
                        }
                        border = new SolidBrush( Color.FromArgb( 150, 152, 150 ) );
                    } else {
                        if ( note_is_whitekey ) {
                            paint_required = false;
                        } else {
                            b = new SolidBrush( black );// = s_brs_212_212_212;
                        }
                        border = new SolidBrush( Color.FromArgb( 210, 205, 172 ) );
                    }
                    if ( paint_required ) {
                        g.FillRectangle( b,
                                         new Rectangle( AppManager._KEY_LENGTH, y, width - AppManager._KEY_LENGTH, track_height + 1 ) );
                    }
                    if ( odd == 0 || odd == 5 ) {
                        g.DrawLine( new Pen( border ),
                                    new Point( AppManager._KEY_LENGTH, y + track_height ),
                                    new Point( width, y + track_height ) );
                    }
                    #endregion

                    #region プリメジャー部分のピアノロール背景
                    int premeasure_start_x = xoffset;
                    if ( premeasure_start_x < AppManager._KEY_LENGTH ) {
                        premeasure_start_x = AppManager._KEY_LENGTH;
                    }
                    int premeasure_end_x = (int)(AppManager.VsqFile.getPreMeasureClocks() * scalex + xoffset);
                    if ( premeasure_start_x >= AppManager._KEY_LENGTH ) {
                        if ( note_is_whitekey ) {
                            g.FillRectangle( s_brs_153_153_153,
                                             new Rectangle( premeasure_start_x,
                                                            y,
                                                            premeasure_end_x - premeasure_start_x,
                                                            track_height + 1 ) );
                        } else {
                            g.FillRectangle( s_brs_106_108_108,
                                             new Rectangle( premeasure_start_x,
                                                            y,
                                                            premeasure_end_x - premeasure_start_x,
                                                            track_height + 1 ) );
                        }
                        if ( odd == 0 || odd == 5 ) {
                            g.DrawLine( s_pen_106_108_108,
                                        new Point( premeasure_start_x, y + track_height ),
                                        new Point( premeasure_end_x, y + track_height ) );
                        }
                    }
                    #endregion

                }
            }

            //ピアノロールと鍵盤部分の縦線
            g.DrawLine( s_pen_212_212_212,
                        new Point( AppManager._KEY_LENGTH, 0 ),
                        new Point( AppManager._KEY_LENGTH, height - 15 ) );
            int odd2 = -1;
            y = 128 * track_height - start_draw_y;
            dy = -track_height;
            for ( int i = 0; i <= 127; i++ ) {
                odd2++;
                if ( odd2 == 12 ) {
                    odd2 = 0;
                }
                y += dy;
                if ( y > height ) {
                    continue;
                } else if ( y + track_height < 0 ) {
                    break;
                }

                #region 鍵盤部分
                g.DrawLine( s_pen_212_212_212,
                            new Point( 3, y ),
                            new Point( AppManager._KEY_LENGTH, y ) );
                bool hilighted = false;
                if ( AppManager.EditMode == EditMode.AddEntry ) {
                    if ( m_adding.ID.Note == i ) {
                        hilighted = true;
                    }
                } else if ( AppManager.EditMode == EditMode.EditLeftEdge || AppManager.EditMode == EditMode.EditRightEdge ) {
#if DEBUG
                    //bocoree.debug.push_log( "(AppManager.LastSelectedEvent==null)=" + (AppManager.LastSelectedEvent == null) );
                    //bocoree.debug.push_log( "(AppManager.LastSelectedEvent.Original==null)=" + (AppManager.LastSelectedEvent.Original == null) );
#endif
                    if ( AppManager.SelectedEvent.LastSelected.Original.ID.Note == i ) { //TODO: ここでNullpointer exception
                        hilighted = true;
                    }
                } else {
                    if ( 3 <= mouse_position.X && mouse_position.X <= width - 17 &&
                        0 <= mouse_position.Y && mouse_position.Y <= height - 1 ) {
                        if ( y <= mouse_position.Y && mouse_position.Y < y + track_height ) {
                            hilighted = true;
                        }
                    }
                }
                if ( hilighted ) {
                    g.FillRectangle( AppManager.HilightBrush,
                                     new Rectangle( 35, y, AppManager._KEY_LENGTH - 35, track_height ) );
                }
                if ( odd2 == 0 || hilighted ) {
                    g.DrawString( VsqNote.getNoteString( i ),
                                  AppManager.BaseFont8,
                                  s_brs_072_077_098,
                                  new PointF( 42, y + half_track_height - AppManager.BaseFont8OffsetHeight + 1 ) );
                }
                if ( !VsqNote.isNoteWhiteKey( i ) ) {
                    g.FillRectangle( s_brs_125_123_124, new Rectangle( 0, y, 34, track_height ) );
                }
                #endregion
            }
            g.ResetClip();

            g.SetClip( new Rectangle( AppManager._KEY_LENGTH, 0, width - AppManager._KEY_LENGTH, height ) );
            #region 小節ごとの線
            int dashed_line_step = AppManager.GetPositionQuantizeClock();
            Color bar = AppManager.EditorConfig.PianorollColorVocalo2Bar.Color;
            Color beat = AppManager.EditorConfig.PianorollColorVocalo2Beat.Color;
            if ( renderer.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                bar = AppManager.EditorConfig.PianorollColorUtauBar.Color;
                beat = AppManager.EditorConfig.PianorollColorUtauBeat.Color;
            } else if ( renderer.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                bar = AppManager.EditorConfig.PianorollColorVocalo1Bar.Color;
                beat = AppManager.EditorConfig.PianorollColorVocalo1Beat.Color;
            }
            using ( Pen pen_bar = new Pen( bar ) )
            using ( Pen pen_beat = new Pen( beat ) ) {
                for ( Iterator itr = AppManager.VsqFile.getBarLineIterator( ClockFromXCoord( width ) ); itr.hasNext(); ) {
                    VsqBarLineType blt = (VsqBarLineType)itr.next();
                    int local_clock_step = 1920 / blt.getLocalDenominator();
                    int x = (int)(blt.clock() * scalex + xoffset);
                    if ( blt.isSeparator() ) {
                        //ピアノロール上
                        g.DrawLine( pen_bar,// s_pen_161_157_136,
                                    new Point( x, 0 ),
                                    new Point( x, height ) );
                    } else {
                        //ピアノロール上
                        g.DrawLine( pen_beat,// s_pen_209_204_172,
                                    new Point( x, 0 ),
                                    new Point( x, height ) );
                    }
                    if ( dashed_line_step > 1 && AppManager.GridVisible ) {
                        int numDashedLine = local_clock_step / dashed_line_step;
                        for ( int i = 1; i < numDashedLine; i++ ) {
                            int x2 = (int)((blt.clock() + i * dashed_line_step) * scalex + xoffset);
                            g.DrawLine( pen_beat,// s_pen_dashed_209_204_172,
                                        new Point( x2, 0 ),
                                        new Point( x2, height ) );
                        }
                    }
                }
            }
            #endregion

            #region トラックのエントリを描画
            if ( m_draw_objects != null ) {
                if ( AppManager.Overlay ) {
                    // まず、選択されていないトラックの簡易表示を行う
                    lock ( m_draw_objects ) {
                        for ( int i = 0; i < m_draw_objects.Count; i++ ) {
                            if ( i == AppManager.Selected - 1 ) {
                                continue;
                            }
                            int j_start = m_draw_start_index[i];
                            bool first = true;
                            int shift_center = AppManager.EditorConfig.PxTrackHeight / 2;
                            for ( int j = j_start; j < m_draw_objects[i].Count; j++ ) {
                                DrawObject dobj = m_draw_objects[i][j];
                                int x = dobj.pxRectangle.X - start_draw_x;
                                y = dobj.pxRectangle.Y - start_draw_y;
                                int lyric_width = dobj.pxRectangle.Width;
                                if ( x + lyric_width < 0 ) {
                                    continue;
                                } else if ( width < x ) {
                                    break;
                                }
                                if ( AppManager.Playing && first ) {
                                    m_draw_start_index[i] = j;
                                    first = false;
                                }
                                if ( y + track_height < 0 || y > height ) {
                                    continue;
                                }
                                g.DrawLine( new Pen( AppManager.s_HILIGHT[i] ),
                                            new Point( x + 1, y + shift_center ),
                                            new Point( x + lyric_width - 1, y + shift_center ) );
                                g.DrawPolygon( new Pen( s_HIDDEN[i] ),
                                               new Point[] { new Point( x, y + shift_center ), 
                                                             new Point( x + 1, y + shift_center - 1 ),
                                                             new Point( x + lyric_width - 1, y + shift_center - 1 ),
                                                             new Point( x + lyric_width, y + shift_center ),
                                                             new Point( x + lyric_width - 1, y + shift_center + 1 ),
                                                             new Point( x + 1, y + shift_center + 1 ), 
                                                             new Point( x, y + shift_center ) } );
                            }
                        }
                    }
                }

                // 選択されているトラックの表示を行う
                int selected = AppManager.Selected;
                bool show_lyrics = AppManager.EditorConfig.ShowLyric;
                bool show_exp_line = AppManager.EditorConfig.ShowExpLine;
                if ( selected >= 1 ) {
                    Region r = g.Clip.Clone();
                    g.Clip = new Region( new Rectangle( AppManager._KEY_LENGTH,
                                                        0,
                                                        pictPianoRoll.Width - AppManager._KEY_LENGTH,
                                                        pictPianoRoll.Height ) );
                    int j_start = m_draw_start_index[selected - 1];

                    bool first = true;
                    lock ( m_draw_objects ) { //ここでロックを取得しないと、描画中にUpdateDrawObjectのサイズが0になる可能性がある
                        if ( selected - 1 < m_draw_objects.Count ) {
                            for ( int j = j_start; j < m_draw_objects[selected - 1].Count; j++ ) {
                                DrawObject dobj = m_draw_objects[selected - 1][j];
                                int x = dobj.pxRectangle.X - start_draw_x;
                                y = dobj.pxRectangle.Y - start_draw_y;
                                int lyric_width = dobj.pxRectangle.Width;
                                if ( x + lyric_width < 0 ) {
                                    continue;
                                } else if ( width < x ) {
                                    break;
                                }
#if PROPRETY_WINDOW
                                if ( properties.ContainsKey( dobj.InternalID ) ) {
                                    property_relateditems.Add( dobj.InternalID, new Rectangle( x, y, lyric_width, track_height ) );
                                }
#endif
                                if ( AppManager.Playing && first ) {
                                    m_draw_start_index[selected - 1] = j;
                                    first = false;
                                }
                                if ( y + 2 * track_height < 0 || y > height ) {
                                    continue;
                                }
                                Color id_fill;
                                if ( AppManager.SelectedEvent.Count > 0 ) {
                                    bool found = AppManager.SelectedEvent.ContainsKey( AppManager.Selected, dobj.InternalID );
                                    if ( found ) {
                                        id_fill = AppManager.HilightColor;
                                    } else {
                                        id_fill = Color.FromArgb( 181, 220, 86 );
                                    }
                                } else {
                                    id_fill = Color.FromArgb( 181, 220, 86 );
                                }
                                g.FillRectangle(
                                    new SolidBrush( id_fill ),
                                    new Rectangle( x, y + 1, lyric_width, track_height - 1 ) );
                                Font lyric_font = dobj.SymbolProtected ? AppManager.BaseFont10Bold : AppManager.BaseFont10;
                                if ( dobj.Overwrapped ) {
                                    g.DrawRectangle( s_pen_125_123_124,
                                                     new Rectangle( x, y + 1, lyric_width, track_height - 1 ) );
                                    if ( show_lyrics ) {
                                        g.DrawString( dobj.Text,
                                                      lyric_font,
                                                      s_brs_147_147_147,
                                                      new PointF( x + 1, y + half_track_height - AppManager.BaseFont10OffsetHeight + 1 ) );
                                    }
                                } else {
                                    g.DrawRectangle( s_pen_125_123_124,
                                                     new Rectangle( x, y + 1, lyric_width, track_height - 1 ) );
                                    if ( show_lyrics ) {
                                        g.DrawString( dobj.Text,
                                                      lyric_font,
                                                      Brushes.Black,
                                                      new PointF( x + 1, y + half_track_height - AppManager.BaseFont10OffsetHeight + 1 ) );
                                    }
                                    if ( show_exp_line && lyric_width > 21 ) {
                                        #region 表情線
                                        DrawAccentLine( g, new Point( x, y + track_height + 1 ), dobj.Accent );
                                        int vibrato_start = x + lyric_width;
                                        int vibrato_end = x;
                                        if ( dobj.pxVibratoDelay <= lyric_width ) {
                                            int vibrato_delay = dobj.pxVibratoDelay;
                                            int vibrato_width = dobj.pxRectangle.Width - vibrato_delay;
                                            vibrato_start = x + vibrato_delay;
                                            vibrato_end = x + vibrato_delay + vibrato_width;
                                            if ( vibrato_start - x < 21 ) {
                                                vibrato_start = x + 21;
                                            }
                                        }
                                        g.DrawLine( s_pen_051_051_000,
                                                    new Point( x + 21, y + track_height + 7 ),
                                                    new Point( vibrato_start, y + track_height + 7 ) );
                                        if ( dobj.pxVibratoDelay <= lyric_width ) {
                                            int next_draw = vibrato_start;
                                            if ( vibrato_start < vibrato_end ) {
                                                DrawVibratoLine( g,
                                                                 new Point( vibrato_start, y + track_height + 1 ),
                                                                 vibrato_end - vibrato_start );
                                            }
                                        }
                                        #endregion
                                    }
                                    // ビブラートがあれば
                                    if ( AppManager.EditorConfig.ViewAtcualPitch ) {
                                        if ( dobj.VibRate != null ) {
                                            int vibrato_delay = dobj.pxVibratoDelay;
                                            int vibrato_width = dobj.pxRectangle.Width - vibrato_delay;
                                            int vibrato_start = x + vibrato_delay;
                                            int vibrato_end = x + vibrato_delay + vibrato_width;
                                            int cl_sx = ClockFromXCoord( vibrato_start );
                                            int cl_ex = ClockFromXCoord( vibrato_end );
                                            DrawVibratoEx( g,
                                                         dobj.VibRate,
                                                         dobj.VibStartRate,
                                                         dobj.VibDepth,
                                                         dobj.VibStartDepth,
                                                         dobj.Note,
                                                         vibrato_start,
                                                         vibrato_width );
                                        }
                                    }
                                }
                            }
                        }
                    }
                    g.Clip = r;
                }

                // 編集中のエントリを表示
                if ( AppManager.EditMode == EditMode.AddEntry ||
                     AppManager.EditMode == EditMode.AddFixedLengthEntry ||
                     AppManager.EditMode == EditMode.Realtime ) {
                    if ( m_adding != null ) {
                        int x = (int)(m_adding.Clock * scalex + xoffset);
                        y = -m_adding.ID.Note * track_height + yoffset + 1;
                        if ( m_adding.ID.Length <= 0 ) {
                            g.DrawRectangle( s_pen_dashed_171_171_171,
                                             new Rectangle( x, y, 10, track_height - 1 ) );
                        } else {
                            int length = (int)(m_adding.ID.Length * scalex);
                            g.DrawRectangle( s_pen_a136_000_000_000,
                                             new Rectangle( x, y, length, track_height - 1 ) );
                        }
                    }
                } else if ( AppManager.EditMode == EditMode.EditVibratoDelay ) {
                    int x = (int)(m_adding.Clock * scalex + xoffset);
                    y = -m_adding.ID.Note * track_height + yoffset + 1;
                    int length = (int)(m_adding.ID.Length * scalex);
                    g.DrawRectangle( s_pen_a136_000_000_000,
                                     new Rectangle( x, y, length, track_height - 1 ) );
                } else if ( (AppManager.EditMode == EditMode.MoveEntry ||
                       AppManager.EditMode == EditMode.EditLeftEdge ||
                       AppManager.EditMode == EditMode.EditRightEdge) && AppManager.SelectedEvent.Count > 0 ) {
                    foreach ( SelectedEventEntry ev in AppManager.SelectedEvent.GetEnumerator() ) {
                        int x = (int)(ev.Editing.Clock * scalex + xoffset);
                        y = -ev.Editing.ID.Note * track_height + yoffset + 1;
                        if ( ev.Editing.ID.Length == 0 ) {
                            g.DrawRectangle( s_pen_dashed_171_171_171,
                                             new Rectangle( x, y, 10, track_height - 1 ) );
                        } else {
                            int length = (int)(ev.Editing.ID.Length * scalex);
                            g.DrawRectangle( s_pen_a136_000_000_000,
                                             new Rectangle( x, y, length, track_height - 1 ) );
                        }
                    }
                }
            }
            #endregion

            g.ResetClip();

            #endregion

            #region 音符編集時の補助線
            if ( AppManager.EditMode == EditMode.AddEntry ) {
                #region EditMode.AddEntry
                int x = (int)(m_adding.Clock * scalex + xoffset);
                y = -m_adding.ID.Note * track_height + yoffset + 1;
                int length;
                if ( m_adding.ID.Length == 0 ) {
                    length = 10;
                } else {
                    length = (int)(m_adding.ID.Length * scalex);
                }
                x += length;
                g.DrawLine( s_pen_LU,
                            new Point( x, 0 ),
                            new Point( x, y - 1 ) );
                g.DrawLine( s_pen_LU,
                            new Point( x, y + track_height ),
                            new Point( x, height ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, 0 ),
                            new Point( x + 1, y - 1 ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, y + track_height ),
                            new Point( x + 1, height ) );
                #endregion
            } else if ( AppManager.EditMode == EditMode.MoveEntry || AppManager.EditMode == EditMode.MoveEntryWaitMove ) {
                #region EditMode.MoveEntry || EditMode.MoveEntryWaitMove
                if ( AppManager.SelectedEvent.Count > 0 ) {
                    VsqEvent last = AppManager.SelectedEvent.LastSelected.Editing;
                    int x = (int)(last.Clock * scalex + xoffset);
                    y = -last.ID.Note * track_height + yoffset + 1;
                    int length = (int)(last.ID.Length * scalex);
                    // 縦線
                    g.DrawLine( s_pen_LU,
                                new Point( x, 0 ),
                                new Point( x, y - 1 ) );
                    g.DrawLine( s_pen_LU,
                                new Point( x, y + track_height ),
                                new Point( x, height ) );
                    // 横線
                    g.DrawLine( s_pen_LU,
                                new Point( AppManager._KEY_LENGTH, y + track_height / 2 - 2 ),
                                new Point( x - 1, y + track_height / 2 - 2 ) );
                    g.DrawLine( s_pen_LU,
                                new Point( x + length + 1, y + track_height / 2 - 2 ),
                                new Point( width, y + track_height / 2 - 2 ) );
                    // 縦線
                    g.DrawLine( s_pen_RD,
                                new Point( x + 1, 0 ),
                                new Point( x + 1, y - 1 ) );
                    g.DrawLine( s_pen_RD,
                                new Point( x + 1, y + track_height ),
                                new Point( x + 1, height ) );
                    // 横線
                    g.DrawLine( s_pen_RD,
                                new Point( AppManager._KEY_LENGTH, y + track_height / 2 - 1 ),
                                new Point( x - 1, y + track_height / 2 - 1 ) );
                    g.DrawLine( s_pen_RD,
                                new Point( x + length + 1, y + track_height / 2 - 1 ),
                                new Point( width, y + track_height / 2 - 1 ) );
                }
                #endregion
            } else if ( AppManager.EditMode == EditMode.AddFixedLengthEntry ) {
                #region EditMode.MoveEntry
                int x = (int)(m_adding.Clock * scalex + xoffset);
                y = -m_adding.ID.Note * track_height + yoffset + 1;
                int length = (int)(m_adding.ID.Length * scalex);
                // 縦線
                g.DrawLine( s_pen_LU,
                            new Point( x, 0 ),
                            new Point( x, y - 1 ) );
                g.DrawLine( s_pen_LU,
                            new Point( x, y + track_height ),
                            new Point( x, height ) );
                // 横線
                g.DrawLine( s_pen_LU,
                            new Point( AppManager._KEY_LENGTH, y + track_height / 2 - 2 ),
                            new Point( x - 1, y + track_height / 2 - 2 ) );
                g.DrawLine( s_pen_LU,
                            new Point( x + length + 1, y + track_height / 2 - 2 ),
                            new Point( width, y + track_height / 2 - 2 ) );
                // 縦線
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, 0 ),
                            new Point( x + 1, y - 1 ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, y + track_height ),
                            new Point( x + 1, height ) );
                // 横線
                g.DrawLine( s_pen_RD,
                            new Point( AppManager._KEY_LENGTH, y + track_height / 2 - 1 ),
                            new Point( x - 1, y + track_height / 2 - 1 ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + length + 1, y + track_height / 2 - 1 ),
                            new Point( width, y + track_height / 2 - 1 ) );
                #endregion
            } else if ( AppManager.EditMode == EditMode.EditLeftEdge ) {
                #region EditMode.EditLeftEdge
                VsqEvent last = AppManager.SelectedEvent.LastSelected.Editing;
                int x = (int)(last.Clock * scalex + xoffset);
                y = -last.ID.Note * track_height + yoffset + 1;
                g.DrawLine( s_pen_LU,
                            new Point( x, 0 ),
                            new Point( x, y - 1 ) );
                g.DrawLine( s_pen_LU,
                            new Point( x, y + track_height ),
                            new Point( x, height ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, 0 ),
                            new Point( x + 1, y - 1 ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, y + track_height ),
                            new Point( x + 1, height ) );
                #endregion
            } else if ( AppManager.EditMode == EditMode.EditRightEdge ) {
                #region EditMode.EditRightEdge
                VsqEvent last = AppManager.SelectedEvent.LastSelected.Editing;
                int x = (int)(last.Clock * scalex + xoffset);
                y = -last.ID.Note * track_height + yoffset + 1;
                int length = (int)(last.ID.Length * scalex);
                x += length;
                g.DrawLine( s_pen_LU,
                            new Point( x, 0 ),
                            new Point( x, y - 1 ) );
                g.DrawLine( s_pen_LU,
                            new Point( x, y + track_height ),
                            new Point( x, height ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, 0 ),
                            new Point( x + 1, y - 1 ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, y + track_height ),
                            new Point( x + 1, height ) );
                #endregion
            } else if ( AppManager.EditMode == EditMode.EditVibratoDelay ) {
                #region EditVibratoDelay
                int x = (int)(m_adding.Clock * scalex + xoffset);
                y = -m_adding.ID.Note * track_height + yoffset + 1;
                g.DrawLine( s_pen_LU,
                            new Point( x, 0 ),
                            new Point( x, y - 1 ) );
                g.DrawLine( s_pen_LU,
                            new Point( x, y + track_height ),
                            new Point( x, height ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, 0 ),
                            new Point( x + 1, y - 1 ) );
                g.DrawLine( s_pen_RD,
                            new Point( x + 1, y + track_height ),
                            new Point( x + 1, height ) );
                double max_length = m_adding_length - _PX_ACCENT_HEADER / scalex;
                double drate = m_adding.ID.Length / max_length;
                if ( drate > 0.99 ) {
                    drate = 1.00;
                }
                int rate = (int)(drate * 100.0);
                string percent = rate + "%";
                SizeF size = AppManager.MeasureString( percent, s_F9PT );
                int delay_x = (int)((m_adding.Clock + m_adding.ID.Length - m_adding_length + m_adding.ID.VibratoDelay) * scalex + xoffset);
                Rectangle pxArea = new Rectangle( delay_x,
                                                  (int)(y + track_height * 2.5),
                                                  (int)(size.Width * 1.2),
                                                  (int)(size.Height * 1.2) );
                g.FillRectangle( s_brs_192_192_192, pxArea );
                g.DrawRectangle( Pens.Black, pxArea );
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString( percent, s_F9PT, Brushes.Black, pxArea, sf );
                #endregion
            }
            #endregion

            #region プロパティウィンドウ
#if PROPRETY_WINDOW
            SmoothingMode smold = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using ( Pen link = new Pen( Color.FromArgb( 127, Color.Black ), 2.0f ) ) {
                link.EndCap = LineCap.Round;
                link.StartCap = LineCap.Round;
                foreach ( int id in properties.Keys ) {
                    if ( property_relateditems.ContainsKey( id ) ) {
                        ContextProperty cp = properties[id];
                        Rectangle rc = property_relateditems[id];
                        int x1 = rc.X + cp.Shift.X;
                        int y1 = rc.Y + cp.Shift.Y;
                        g.FillEllipse( new SolidBrush( Color.FromArgb( 200, Color.Black ) ),
                                       new Rectangle( rc.X + rc.Width / 2 - 2, rc.Y + rc.Height / 2 - 2, 5, 5 ) );
                        g.DrawLine( link, rc.X + rc.Width / 2, rc.Y + rc.Height / 2, x1 + cp.Width / 2, y1 );
                        cp.Left = x1;
                        cp.Top = y1;
                        cp.DrawTo( g, new Point( x1, y1 ) );
                    }
                }
            }
            g.SmoothingMode = smold;
#endif
            #endregion
#if PROPRETY_WINDOW
        } // lock
#endif

            #region 外枠
            // 左(外側)
        g.DrawLine( s_pen_160_160_160,
                    new Point( 0, 0 ),
                    new Point( 0, height ) );
        // 左(内側)
        g.DrawLine( s_pen_105_105_105,
                    new Point( 1, 0 ),
                    new Point( 1, height ) );
        #endregion*/
    }
}
