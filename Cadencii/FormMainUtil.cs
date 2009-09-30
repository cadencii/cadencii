/*
 * FormMainUtil.cs
 * Copyright (c) 2008-2009 kbinani
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Media;

using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    using boolean = Boolean;
    using Integer = Int32;
    using Long = Int64;

    partial class FormMain {
        public void updateBgmMenuState() {
            menuTrackBgm.DropDown.Items.Clear();
            int count = AppManager.getBgmCount();
            if ( count > 0 ) {
                for ( int i = 0; i < count; i++ ) {
                    BgmFile item = AppManager.getBgm( i );
                    ToolStripMenuItem menu = new ToolStripMenuItem( Path.GetFileName( item.file ) );
                    menu.ToolTipText = item.file;

                    ToolStripMenuItem menu_remove = new ToolStripMenuItem( _( "Remove" ) );
                    menu_remove.ToolTipText = item.file;
                    menu_remove.Tag = (int)i;
                    menu_remove.Click += new EventHandler( handleBgmRemove_Click );
                    menu.DropDown.Items.Add( menu_remove );

                    ToolStripMenuItem menu_start_after_premeasure = new ToolStripMenuItem( _( "Start After Premeasure" ) );
                    menu_start_after_premeasure.Name = "menu_start_after_premeasure" + i;
                    menu_start_after_premeasure.Tag = (int)i;
                    menu_start_after_premeasure.CheckOnClick = true;
                    menu_start_after_premeasure.Checked = item.startAfterPremeasure;
                    menu_start_after_premeasure.CheckedChanged += new EventHandler( handleBgmStartAfterPremeasure_CheckedChanged );
                    menu.DropDown.Items.Add( menu_start_after_premeasure );

                    ToolStripMenuItem menu_offset_second = new ToolStripMenuItem( _( "Set Offset Seconds" ) );
                    menu_offset_second.Tag = (int)i;
                    menu_offset_second.ToolTipText = item.readOffsetSeconds + " " + _( "seconds" );
                    menu_offset_second.Click += new EventHandler( handleBgmOffsetSeconds_Click );
                    menu.DropDown.Items.Add( menu_offset_second );

                    menuTrackBgm.DropDown.Items.Add( menu );
                }
                menuTrackBgm.DropDown.Items.Add( new ToolStripSeparator() );
            }
            ToolStripMenuItem menu_add = new ToolStripMenuItem( _( "Add" ) );
            menu_add.Click += new EventHandler( handleBgmAdd_Click );
            menuTrackBgm.DropDown.Items.Add( menu_add );
        }

        private void handleBgmOffsetSeconds_Click( object sender, EventArgs e ) {
            if ( !(sender is ToolStripMenuItem) ) {
                return;
            }
            ToolStripMenuItem parent = (ToolStripMenuItem)sender;
            if ( parent.Tag == null ) {
                return;
            }
            if ( !(parent.Tag is int) ) {
                return;
            }
            int index = (int)parent.Tag;
            Bitmap bmp;
            using ( InputBox ib = new InputBox( _( "Input Offset Seconds" ) ) ) {
                BgmFile item = AppManager.getBgm( index );
                ib.Location = GetFormPreferedLocation( ib );
                ib.Result = item.readOffsetSeconds + "";
                if ( ib.ShowDialog() != DialogResult.OK ) {
                    return;
                }
                double draft;
                if ( double.TryParse( ib.Result, out draft ) ) {
                    item.readOffsetSeconds = draft;
                    parent.ToolTipText = draft + " " + _( "seconds" );
                }
            }
        }

        private void handleBgmStartAfterPremeasure_CheckedChanged( object sender, EventArgs e ) {
            if ( !(sender is ToolStripMenuItem) ) {
                return;
            }
            ToolStripMenuItem parent = (ToolStripMenuItem)sender;
            if ( parent.Tag == null ) {
                return;
            }
            if ( !(parent.Tag is int) ) {
                return;
            }
            int index = (int)parent.Tag;
            AppManager.getBgm( index ).startAfterPremeasure = parent.Checked;
        }

        private void handleBgmAdd_Click( object sender, EventArgs e ) {
            if ( openWaveDialog.ShowDialog() != DialogResult.OK ) {
                return;
            }

            String file = openWaveDialog.FileName;

            // 既に開かれていたらキャンセル
            int count = AppManager.getBgmCount();
            boolean found = false;
            for ( int i = 0; i < count; i++ ) {
                BgmFile item = AppManager.getBgm( i );
                if ( file.Equals( item.file ) ) {
                    found = true;
                    break;
                }
            }
            if ( found ) {
                MessageBox.Show( string.Format( _( "file '{0}' is already registered as BGM." ), file ), 
                                 _( "Error" ),
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Exclamation );
                return;
            }

            // 登録
            AppManager.addBgm( file );
            updateBgmMenuState();
        }

        private void handleBgmRemove_Click( object sender, EventArgs e ) {
            if ( !(sender is ToolStripMenuItem) ) {
                return;
            }
            ToolStripMenuItem parent = (ToolStripMenuItem)sender;
            if ( parent.Tag == null ) {
                return;
            }
            if ( !(parent.Tag is int) ) {
                return;
            }
            int index = (int)parent.Tag;
            BgmFile bgm = AppManager.getBgm( index );
            if ( MessageBox.Show( string.Format( _( "remove '{0}'?" ), bgm.file ), 
                                  "Cadencii", 
                                  MessageBoxButtons.YesNo, 
                                  MessageBoxIcon.Question ) != DialogResult.Yes ) {
                return;
            }
            AppManager.removeBgm( index );
            updateBgmMenuState();
        }

        private void UpdatePropertyPanelState( PropertyPanelState.PanelState state ) {
            switch ( state ) {
                case PropertyPanelState.PanelState.Docked:
                    m_property_panel_container.Add( AppManager.propertyPanel );
                    AppManager.propertyWindow.Visible = false;
                    menuVisualProperty.Checked = true;
                    AppManager.editorConfig.PropertyWindowStatus.State = PropertyPanelState.PanelState.Docked;
                    splitContainerProperty.IsSplitterFixed = false;
                    splitContainerProperty.Panel1MinSize = _PROPERTY_DOCK_MIN_WIDTH;
                    splitContainerProperty.SplitterDistance = AppManager.editorConfig.PropertyWindowStatus.DockWidth;
                    AppManager.editorConfig.PropertyWindowStatus.WindowState = FormWindowState.Minimized;
                    AppManager.propertyWindow.WindowState = FormWindowState.Minimized;
                    break;
                case PropertyPanelState.PanelState.Hidden:
                    AppManager.propertyWindow.Visible = false;
                    menuVisualProperty.Checked = false;
                    if ( AppManager.editorConfig.PropertyWindowStatus.State == PropertyPanelState.PanelState.Docked ) {
                        AppManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.SplitterDistance;
                    }
                    AppManager.editorConfig.PropertyWindowStatus.State = PropertyPanelState.PanelState.Hidden;
                    splitContainerProperty.Panel1MinSize = 0;
                    splitContainerProperty.SplitterDistance = 0;
                    splitContainerProperty.IsSplitterFixed = true;
                    break;
                case PropertyPanelState.PanelState.Window:
#if DEBUG
                    Console.WriteLine( "UpatePropertyPanelState; state=Window; AppManager.PropertyWindow.WindowState=" + AppManager.propertyWindow.WindowState );
#endif
                    AppManager.propertyWindow.Visible = true;
                    if ( AppManager.propertyWindow.WindowState != FormWindowState.Normal ) {
                        AppManager.propertyWindow.WindowState = FormWindowState.Normal;
                    }
                    AppManager.propertyWindow.Controls.Add( AppManager.propertyPanel );
                    Point parent = this.Location;
                    Rectangle rc = AppManager.editorConfig.PropertyWindowStatus.Bounds;
                    Point property = rc.Location;
                    AppManager.propertyWindow.Bounds = new Rectangle( parent.X + property.X, parent.Y + property.Y, rc.Width, rc.Height );
                    normalizeFormLocation( AppManager.propertyWindow );
                    menuVisualProperty.Checked = true;
                    if ( AppManager.editorConfig.PropertyWindowStatus.State == PropertyPanelState.PanelState.Docked ) {
                        AppManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.SplitterDistance;
                    }
                    AppManager.editorConfig.PropertyWindowStatus.State = PropertyPanelState.PanelState.Window;
                    splitContainerProperty.Panel1MinSize = 0;
                    splitContainerProperty.SplitterDistance = 0;
                    splitContainerProperty.IsSplitterFixed = true;
                    AppManager.editorConfig.PropertyWindowStatus.WindowState = FormWindowState.Normal;
                    break;
            }
        }

        /// <summary>
        /// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
        /// 合致するようにシフトします
        /// </summary>
        /// <param name="work"></param>
        /// <param name="tempo"></param>
        private static void shiftClockToMatchWith( VsqFileEx target, VsqFile tempo ) {
            // まずクロック値を、リプレース後のモノに置き換え
            for ( int track = 1; track < target.Track.size(); track++ ) {
                // ノート・歌手イベントをシフト
                for ( Iterator itr = target.Track.get( track ).getEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( item.ID.type == VsqIDType.Singer && item.Clock == 0 ) {
                        continue;
                    }
                    int clock = item.Clock;
                    double sec_start = target.getSecFromClock( clock );
                    double sec_end = target.getSecFromClock( clock + item.ID.Length );
                    int clock_start = (int)tempo.getClockFromSec( sec_start );
                    int clock_end = (int)tempo.getClockFromSec( sec_end );
                    item.Clock = clock_start;
                    item.ID.Length = clock_end - clock_start;
                    if ( item.ID.VibratoHandle != null ) {
                        double sec_vib_start = target.getSecFromClock( clock + item.ID.VibratoDelay );
                        int clock_vib_start = (int)tempo.getClockFromSec( sec_vib_start );
                        item.ID.VibratoDelay = clock_vib_start - clock_start;
                        item.ID.VibratoHandle.Length = clock_end - clock_vib_start;
                    }
                }

                // コントロールカーブをシフト
                foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                    VsqBPList item = target.Track.get( track ).getCurve( ct.getName() );
                    if ( item == null ) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList( item.getDefault(), item.getMinimum(), item.getMaximum() );
                    for ( int i = 0; i < item.size(); i++ ) {
                        int clock = item.getKeyClock( i );
                        int value = item.getElement( i );
                        double sec = target.getSecFromClock( clock );
                        int clock_new = (int)tempo.getClockFromSec( sec );
                        repl.add( clock_new, value );
                    }
                    target.Track.get( track ).setCurve( ct.getName(), repl );
                }

                // ベジエカーブをシフト
                foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                    Vector<BezierChain> list = target.AttachedCurves.get( track - 1 ).get( ct );
                    if ( list == null ) {
                        continue;
                    }
                    for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                        BezierChain chain = (BezierChain)itr.next();
                        for ( Iterator itr2 = chain.points.iterator(); itr2.hasNext(); ) {
                            BezierPoint point = (BezierPoint)itr.next();
                            PointD bse = new PointD( tempo.getClockFromSec( target.getSecFromClock( point.getBase().X )),
                                                     point.getBase().Y );
                            double rx = point.getBase().X + point.controlRight.X;
                            double new_rx = tempo.getClockFromSec( target.getSecFromClock( rx ));
                            PointD ctrl_r = new PointD( new_rx - bse.X, point.controlRight.Y );

                            double lx = point.getBase().X + point.controlLeft.X;
                            double new_lx = tempo.getClockFromSec( target.getSecFromClock( lx ));
                            PointD ctrl_l = new PointD( new_lx - bse.X, point.controlLeft.Y );
                            point.setBase( bse );
                            point.controlLeft = ctrl_l;
                            point.controlRight = ctrl_r;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
        /// 合致するようにシフトします
        /// </summary>
        /// <param name="work"></param>
        /// <param name="tempo"></param>
        private static void ShiftClockToMatchWith( VsqFileEx target, VsqFile tempo, double premeasure_sec_tempo ) {
            double premeasure_sec_target = target.getSecFromClock( target.getPreMeasureClocks() );
#if DEBUG
            Console.WriteLine( "FormMain#ShiftClockToMatchWith; premeasure_sec_target=" + premeasure_sec_target + "; premeasre_sec_tempo=" + premeasure_sec_tempo );
#endif

            // テンポをリプレースする場合。
            // まずクロック値を、リプレース後のモノに置き換え
            for ( int track = 1; track < target.Track.size(); track++ ) {
                // ノート・歌手イベントをシフト
                for ( Iterator itr = target.Track.get( track ).getEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( item.ID.type == VsqIDType.Singer && item.Clock == 0 ) {
                        continue;
                    }
                    int clock = item.Clock;
                    double sec_start = target.getSecFromClock( clock ) - premeasure_sec_target + premeasure_sec_tempo;
                    double sec_end = target.getSecFromClock( clock + item.ID.Length ) - premeasure_sec_target + premeasure_sec_tempo;
                    int clock_start = (int)tempo.getClockFromSec( sec_start );
                    int clock_end = (int)tempo.getClockFromSec( sec_end );
                    item.Clock = clock_start;
                    item.ID.Length = clock_end - clock_start;
                    if ( item.ID.VibratoHandle != null ) {
                        double sec_vib_start = target.getSecFromClock( clock + item.ID.VibratoDelay ) - premeasure_sec_target + premeasure_sec_tempo;
                        int clock_vib_start = (int)tempo.getClockFromSec( sec_vib_start );
                        item.ID.VibratoDelay = clock_vib_start - clock_start;
                        item.ID.VibratoHandle.Length = clock_end - clock_vib_start;
                    }
                }

                // コントロールカーブをシフト
                foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                    VsqBPList item = target.Track.get( track ).getCurve( ct.getName() );
                    if ( item == null ) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList( item.getDefault(), item.getMinimum(), item.getMaximum() );
                    for ( int i = 0; i < item.size(); i++ ) {
                        int clock = item.getKeyClock( i );
                        int value = item.getElement( i );
                        double sec = target.getSecFromClock( clock ) - premeasure_sec_target + premeasure_sec_tempo;
                        if ( sec >= premeasure_sec_tempo ) {
                            int clock_new = (int)tempo.getClockFromSec( sec );
                            repl.add( clock_new, value );
                        }
                    }
                    target.Track.get( track ).setCurve( ct.getName(), repl );
                }

                // ベジエカーブをシフト
                foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                    Vector<BezierChain> list = target.AttachedCurves.get( track - 1 ).get( ct );
                    if ( list == null ) {
                        continue;
                    }
                    for ( Iterator itr = list.iterator(); itr.hasNext(); ){
                        BezierChain chain = (BezierChain)itr.next();
                        for ( Iterator itr2 = chain.points.iterator(); itr2.hasNext(); ){
                            BezierPoint point = (BezierPoint)itr2.next();
                            PointD bse = new PointD( tempo.getClockFromSec( target.getSecFromClock( point.getBase().X ) - premeasure_sec_target + premeasure_sec_tempo ),
                                                     point.getBase().Y );
                            double rx = point.getBase().X + point.controlRight.X;
                            double new_rx = tempo.getClockFromSec( target.getSecFromClock( rx ) - premeasure_sec_target + premeasure_sec_tempo );
                            PointD ctrl_r = new PointD( new_rx - bse.X, point.controlRight.Y );

                            double lx = point.getBase().X + point.controlLeft.X;
                            double new_lx = tempo.getClockFromSec( target.getSecFromClock( lx ) - premeasure_sec_target + premeasure_sec_tempo );
                            PointD ctrl_l = new PointD( new_lx - bse.X, point.controlLeft.Y );
                            point.setBase( bse );
                            point.controlLeft = ctrl_l;
                            point.controlRight = ctrl_r;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// メインメニュー項目の中から，Nameプロパティがnameであるものを検索します．見つからなければnullを返す．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ToolStripMenuItem SearchMenuItemFromName( String name ) {
            foreach( ToolStripItem tsi in menuStripMain.Items ){
                if ( tsi is ToolStripMenuItem ) {
                    ToolStripMenuItem ret = SearchMenuItemRecurse( name, (ToolStripMenuItem)tsi );
                    if ( ret != null ) {
                        return ret;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 指定されたメニューアイテムから，Nameプロパティがnameであるものを再帰的に検索します．見つからなければnullを返す
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        private ToolStripMenuItem SearchMenuItemRecurse( String name, ToolStripMenuItem tree ){
            if ( tree.Name.Equals( name ) ) {
                return tree;
            } else {
                foreach ( ToolStripItem tsi in tree.DropDownItems ) {
                    if ( tsi is ToolStripMenuItem ) {
                        if ( tsi.Name.Equals( name ) ){
                            return (ToolStripMenuItem)tsi;
                        }
                    }
                }
                foreach ( ToolStripItem tsi in tree.DropDownItems ) {
                    if ( tsi is ToolStripMenuItem ) {
                        ToolStripMenuItem ret = SearchMenuItemRecurse( name, (ToolStripMenuItem)tsi );
                        if ( ret != null ) {
                            return ret;
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// フォームのタイトルバーが画面内に入るよう、Locationを正規化します
        /// </summary>
        /// <param name="form"></param>
        public static void normalizeFormLocation( Form dlg ) {
            Rectangle rcScreen = Screen.GetWorkingArea( dlg );
            int top = dlg.Top;
            if ( top + dlg.Height > rcScreen.Y + rcScreen.Height ) {
                // ダイアログの下端が隠れる場合、位置をずらす
                top = rcScreen.Y + rcScreen.Height - dlg.Height;
            }
            if ( top < rcScreen.Top ) {
                // ダイアログの上端が隠れる場合、位置をずらす
                top = rcScreen.Top;
            }
            int left = dlg.Left;
            if ( left + dlg.Width > rcScreen.X + rcScreen.Width ) {
                left = rcScreen.X + rcScreen.Width - dlg.Width;
            }
            if ( left < rcScreen.Left ) {
                left = rcScreen.Left;
            }
            dlg.Top = top;
            dlg.Left = left;
        }

        /// <summary>
        /// フォームをマウス位置に出す場合に推奨されるフォーム位置を計算します
        /// </summary>
        /// <param name="dlg"></param>
        /// <returns></returns>
        private Point GetFormPreferedLocation( Form dlg ) {
            Point mouse = Control.MousePosition;
            Rectangle rcScreen = Screen.GetWorkingArea( this );
            int top = mouse.Y - dlg.Height / 2;
            if ( top + dlg.Height > rcScreen.Y + rcScreen.Height ) {
                // ダイアログの下端が隠れる場合、位置をずらす
                top = rcScreen.Y + rcScreen.Height - dlg.Height;
            }
            if ( top < rcScreen.Top ) {
                // ダイアログの上端が隠れる場合、位置をずらす
                top = rcScreen.Top;
            }
            int left = mouse.X - dlg.Width / 2;
            if ( left + dlg.Width > rcScreen.X + rcScreen.Width ) {
                left = rcScreen.X + rcScreen.Width - dlg.Width;
            }
            return new Point( left, top );
        }

        private void UpdateLayout() {
            int width = panel1.Width;
            int height = panel1.Height;

            // splitContainter1.Panel1->splitContainer2.Panel1
            if ( AppManager.editorConfig.OverviewEnabled ) {
                panel3.Height = _OVERVIEW_HEIGHT;
            } else {
                panel3.Height = 0;
            }
            panel3.Width = width;
            pictOverview.Left = AppManager.KEY_LENGTH;
            pictOverview.Width = panel3.Width - AppManager.KEY_LENGTH;
            pictOverview.Top = 0;
            pictOverview.Height = panel3.Height;

            picturePositionIndicator.Width = width;
            picturePositionIndicator.Height = _PICT_POSITION_INDICATOR_HEIGHT;

            hScroll.Width = width - pictureBox2.Width - pictureBox3.Width - trackBar.Width;
            hScroll.Height = _SCROLL_WIDTH;

            vScroll.Width = _SCROLL_WIDTH;
            vScroll.Height = height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH - panel3.Height;

            pictPianoRoll.Width = width - _SCROLL_WIDTH;
            pictPianoRoll.Height = height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH - panel3.Height;

            pictureBox3.Width = AppManager.KEY_LENGTH;
            pictureBox3.Height = _SCROLL_WIDTH;
            pictureBox2.Height = _SCROLL_WIDTH;
            trackBar.Height = _SCROLL_WIDTH;

            panel3.Top = 0;
            panel3.Left = 0;

            picturePositionIndicator.Top = panel3.Height;
            picturePositionIndicator.Left = 0;

            pictPianoRoll.Top = _PICT_POSITION_INDICATOR_HEIGHT + panel3.Height;
            pictPianoRoll.Left = 0;

            vScroll.Top = _PICT_POSITION_INDICATOR_HEIGHT + panel3.Height;
            vScroll.Left = width - _SCROLL_WIDTH;

            pictureBox3.Top = height - _SCROLL_WIDTH;
            pictureBox3.Left = 0;

            hScroll.Top = height - _SCROLL_WIDTH;
            hScroll.Left = pictureBox3.Width;

            trackBar.Top = height - _SCROLL_WIDTH;
            trackBar.Left = width - _SCROLL_WIDTH - trackBar.Width;

            pictureBox2.Top = height - _SCROLL_WIDTH;
            pictureBox2.Left = width - _SCROLL_WIDTH;

            // splitContainer1.Panel2
            //trackSelector.Width = splitContainer1.Panel2.Width - _SCROLL_WIDTH;
            //trackSelector.Height = splitContainer1.Panel2.Height;
        }

        public void updateRendererMenu() {
            if ( !VSTiProxy.isRendererAvailable( VSTiProxy.RENDERER_DSB2 ) ) {
                cMenuTrackTabRendererVOCALOID1.Image = Boare.Cadencii.Properties.Resources.slash;
                menuTrackRendererVOCALOID1.Image = Boare.Cadencii.Properties.Resources.slash;
            } else {
                cMenuTrackTabRendererVOCALOID1.Image = null;
                menuTrackRendererVOCALOID1.Image = null;
            }
            if ( !VSTiProxy.isRendererAvailable( VSTiProxy.RENDERER_DSB3 ) ) {
                cMenuTrackTabRendererVOCALOID2.Image = Boare.Cadencii.Properties.Resources.slash;
                menuTrackRendererVOCALOID2.Image = Boare.Cadencii.Properties.Resources.slash;
            } else {
                cMenuTrackTabRendererVOCALOID2.Image = null;
                menuTrackRendererVOCALOID2.Image = null;
            }
            if ( !VSTiProxy.isRendererAvailable( VSTiProxy.RENDERER_UTU0 ) ) {
                cMenuTrackTabRendererUtau.Image = Boare.Cadencii.Properties.Resources.slash;
                menuTrackRendererUtau.Image = Boare.Cadencii.Properties.Resources.slash;
            } else {
                cMenuTrackTabRendererUtau.Image = null;
                menuTrackRendererUtau.Image = null;
            }
            if ( !VSTiProxy.isRendererAvailable( VSTiProxy.RENDERER_STR0 ) ) {
                cMenuTrackTabRendererStraight.Image = Boare.Cadencii.Properties.Resources.slash;
                menuTrackRendererStraight.Image = Boare.Cadencii.Properties.Resources.slash;
            } else {
                cMenuTrackTabRendererStraight.Image = null;
                menuTrackRendererStraight.Image = null;
            }
        }

        private void DrawUtauVibrato( Graphics g, UstVibrato vibrato, int note, int clock_start, int clock_width ) {
            SmoothingMode old = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // 魚雷を描いてみる
            int y0 = yCoordFromNote( note - 0.5f );
            int x0 = xCoordFromClocks( clock_start );
            int px_width = xCoordFromClocks( clock_start + clock_width ) - x0;
            int boxheight = (int)(vibrato.Depth * 2 / 100.0f * AppManager.editorConfig.PxTrackHeight);
            int px_shift = (int)(vibrato.Shift / 100.0 * vibrato.Depth / 100.0 * AppManager.editorConfig.PxTrackHeight);

            // vibrato in
            int cl_vibin_end = clock_start + (int)(clock_width * vibrato.In / 100.0);
            int x_vibin_end = xCoordFromClocks( cl_vibin_end );
            Point ul = new Point( x_vibin_end, y0 - boxheight / 2 - px_shift );
            Point dl = new Point( x_vibin_end, y0 + boxheight / 2 - px_shift );
            g.DrawPolygon( Pens.Black, new Point[] { new Point( x0, y0 ), ul, dl } );

            // vibrato out
            int cl_vibout_start = clock_start + clock_width - (int)(clock_width * vibrato.Out / 100.0);
            int x_vibout_start = xCoordFromClocks( cl_vibout_start );
            Point ur = new Point( x_vibout_start, y0 - boxheight / 2 - px_shift );
            Point dr = new Point( x_vibout_start, y0 + boxheight / 2 - px_shift );
            g.DrawPolygon( Pens.Black, new Point[] { new Point( x0 + px_width, y0 ), ur, dr } );

            // box
            int boxwidth = x_vibout_start - x_vibin_end;
            if ( boxwidth > 0 ) {
                g.DrawPolygon( Pens.Black, new Point[] { ul, dl, dr, ur } );
            }

#if DEBUG
            StreamWriter sw = new StreamWriter( "list.txt" );
#endif
            // buf1に、vibrato in/outによる増幅率を代入
            float[] buf1 = new float[clock_width + 1];
            for ( int clock = clock_start; clock <= clock_start + clock_width; clock++ ) {
                buf1[clock - clock_start] = 1.0f;
            }
            // vibin
            if ( cl_vibin_end - clock_start > 0 ) {
                for ( int clock = clock_start; clock <= cl_vibin_end; clock++ ) {
                    int i = clock - clock_start;
                    buf1[i] *= i / (float)(cl_vibin_end - clock_start);
#if DEBUG
                    sw.WriteLine( "vibin: " + i + "\t" + buf1[i] );
#endif
                }
            }
            if ( clock_start + clock_width - cl_vibout_start > 0 ) {
                for ( int clock = clock_start + clock_width; clock >= cl_vibout_start; clock-- ) {
                    int i = clock - clock_start;
                    float v = (clock_start + clock_width - clock) / (float)(clock_start + clock_width - cl_vibout_start);
                    buf1[i] = buf1[i] * v;
#if DEBUG
                    sw.WriteLine( "vibout: " + i + "\t" + buf1[i] );
#endif
                }
            }

            // buf2に、shiftによるy座標のシフト量を代入
            float[] buf2 = new float[clock_width + 1];
            for ( int i = 0; i < clock_width; i++ ) {
                buf2[i] = px_shift * buf1[i];
            }
            try {
                double phase = 2.0 * Math.PI * vibrato.Phase / 100.0;
                double omega = 2.0 * Math.PI / vibrato.Period;   //角速度(rad/msec)
                double msec = AppManager.getVsqFile().getSecFromClock( clock_start - 1 ) * 1000.0;
                float px_track_height = AppManager.editorConfig.PxTrackHeight;
                phase -= (AppManager.getVsqFile().getSecFromClock( clock_start ) * 1000.0 - msec) * omega;
                for ( int clock = clock_start; clock <= clock_start + clock_width; clock++ ) {
                    int i = clock - clock_start;
                    double t_msec = AppManager.getVsqFile().getSecFromClock( clock ) * 1000.0;
                    phase += (t_msec - msec) * omega;
                    msec = t_msec;
                    buf2[i] += (float)(vibrato.Depth * 0.01f * px_track_height * buf1[i] * Math.Sin( phase ));
                }
                PointF[] list = new PointF[clock_width + 1];
                for ( int clock = clock_start; clock <= clock_start + clock_width; clock++ ) {
                    int i = clock - clock_start;
                    list[i] = new PointF( xCoordFromClocks( clock ), y0 + buf2[i] );
                }
#if DEBUG
                AppManager.debugWriteLine( "DrawUtauVibrato" );
                for ( int i = 0; i < list.Length; i++ ) {
                    sw.WriteLine( list[i].X + "\t" + list[i].Y );
                }
                sw.Close();
                sw = null;
#endif
                if ( list.Length >= 2 ) {
                    g.DrawLines( Pens.Red, list );
                }
                g.SmoothingMode = old;
            } catch ( OverflowException oex ) {
#if DEBUG
                AppManager.debugWriteLine( "DrawUtauVibrato; oex=" + oex );
#endif
            }
        }

        /// <summary>
        /// ビブラート用のデータ点のリストを取得します。返却されるリストは、{秒, ビブラートの振幅(ノートナンバー単位)}の値ペアとなっています
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="start_rate"></param>
        /// <param name="depth"></param>
        /// <param name="start_depth"></param>
        /// <param name="clock_start"></param>
        /// <param name="clock_width"></param>
        /// <returns></returns>
        public static Vector<PointF> getVibratoPoints( VsqFileEx vsq,
                                                     VibratoBPList rate,
                                                     int start_rate,
                                                     VibratoBPList depth,
                                                     int start_depth,
                                                     int clock_start,
                                                     int clock_width,
                                                     float sec_resolution ) {
            Vector<PointF> ret = new Vector<PointF>();
            double sec0 = vsq.getSecFromClock( clock_start );
            double sec1 = vsq.getSecFromClock( clock_start + clock_width );
            int count = (int)((sec1 - sec0) / sec_resolution);
            double phase = 0;
            start_rate = rate.getValue( 0.0f, start_rate );
            start_depth = depth.getValue( 0.0f, start_depth );
            float amplitude = start_depth * 2.5f / 127.0f / 2.0f; // ビブラートの振幅。
            float period = (float)Math.Exp( 5.24 - 1.07e-2 * start_rate ) * 2.0f / 1000.0f; //ビブラートの周期、秒
            float omega = (float)(2.0 * Math.PI / period); // 角速度(rad/sec)
            ret.add( new PointF( (float)sec0, 0.0f ) );
            double sec = sec0;
            float fadewidth = (float)(sec1 - sec0) * 0.2f;
            for ( int i = 1; i < count; i++ ) {
                double t_sec = sec0 + sec_resolution * i;
                double clock = vsq.getClockFromSec( t_sec );
                if ( sec0 <= t_sec && t_sec <= sec0 + fadewidth ) {
                    amplitude *= (float)(t_sec - sec0) / fadewidth;
                }
                if ( sec1 - fadewidth <= t_sec && t_sec <= sec1 ) {
                    amplitude *= (float)(sec1 - t_sec) / fadewidth;
                }
                phase += omega * (t_sec - sec);
                ret.add( new PointF( (float)t_sec, amplitude * (float)Math.Sin( phase ) ) );
                float v = (float)(clock - clock_start) / (float)clock_width;
                int r = rate.getValue( v, start_rate );
                int d = depth.getValue( v, start_depth );
                amplitude = d * 2.5f / 127.0f / 2.0f;
                period = (float)Math.Exp( 5.24 - 1.07e-2 * r ) * 2.0f / 1000.0f;
                omega = (float)(2.0 * Math.PI / period);
                sec = t_sec;
            }
            return ret;
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
        private void DrawVibratoEx( Graphics g,
                                 VibratoBPList rate,
                                 int start_rate,
                                 VibratoBPList depth,
                                 int start_depth,
                                 int note,
                                 int x_start,
                                 int px_width ) {
            if ( rate == null || depth == null ) {
                return;
            }
            int y0 = yCoordFromNote( note - 0.5f );
            float px_track_height = AppManager.editorConfig.PxTrackHeight;
            Pen pen = Pens.Blue;
            int clock_start = clockFromXCoord( x_start );
            int clock_end = clockFromXCoord( x_start + px_width );
            int tempo = AppManager.getVsqFile().getTempoAt( clock_start );
            Vector<PointF> ret = getVibratoPoints( AppManager.getVsqFile(),
                                                 rate,
                                                 start_rate,
                                                 depth,
                                                 start_depth,
                                                 clock_start,
                                                 clock_end - clock_start,
                                                 (float)(tempo * 1e-6 / 480.0) );
            if ( ret.size() >= 2 ) {
                Vector<PointF> draw = new Vector<PointF>();
                for ( int i = 0; i < ret.size(); i++ ) {
                    draw.add( new PointF( xCoordFromClocks( AppManager.getVsqFile().getClockFromSec( ret.get( i ).X ) ),
                                          ret.get( i ).Y * px_track_height + y0 ) );
                }
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLines( pen, draw.toArray( new PointF[]{} ) );
                g.SmoothingMode = sm;
            }
        }

        // listに登録されているToolStripを，座標の若い順にcontainerに追加します
        private void AddToolStripInPositionOrder( ToolStripPanel panel, Vector<ToolStrip> list ) {
            boolean[] reg = new boolean[list.size()];
            for ( int i = 0; i < reg.Length; i++ ) {
                reg[i] = false;
            }
            for ( int i = 0; i < list.size(); i++ ) {
                Point p = new Point( int.MaxValue, int.MaxValue );
                int index = -1;

                // x座標の小さいやつを探す
                for ( int j = 0; j < list.size(); j++ ) {
                    if ( !reg[j] ) {
                        if ( p.Y > list.get( j ).Location.Y ) {
                            index = j;
                            p = list.get( j ).Location;
                        }
                        if ( p.Y >= list.get( j ).Location.Y && p.X > list.get( j ).Location.X ) {
                            index = j;
                            p = list.get( j ).Location;
                        }
                    }
                }

                // コントロールを登録
                panel.Join( list.get( index ), list.get( index ).Location );
                reg[index] = true;
            }
        }

        private Cursor SynthCursor( Bitmap attach ) {
            Size s = Cursors.Arrow.Size;
            Bitmap ret = new Bitmap( s.Width, s.Height, PixelFormat.Format32bppRgb );
            Point hotspot = Cursors.Arrow.HotSpot;

            using ( Graphics g = Graphics.FromImage( ret ) ) {
                g.Clear( Color.Transparent );
                //g.Clear( Color.FromArgb( 255, 0, 255 ) );
                //Cursors.Arrow.Draw( g, new Rectangle( 0, 0, s.Width, s.Height ) );
                int nwid = attach.Size.Width;
                int nhei = attach.Size.Height;
                g.FillRectangle( Brushes.Red, new Rectangle( 10, 10, 20, 20 ) );
                //g.FillRectangle( Brushes.White, new Rectangle( s.Width - nwid, s.Height - nhei, nwid, nhei ) );
                //g.DrawImageUnscaled( attach, new Rectangle( s.Width - nwid, s.Height - nhei, nwid, nhei ) );
            }
            /*for ( int x = 0; x < attach.Width; x++ ) {
                for ( int y = 0; y < attach.Height; y++ ) {
                    Color c = attach.GetPixel( x, y );
                    if ( c.A < 255 ) {
                        attach.SetPixel( x, y, Color.FromArgb( 255, 0, 255 ) );
                    }
                }
            }*/
            Cursor ret2;// = CustomCursor.Create( ret, Cursors.Arrow.HotSpot, new Point( 0, 0 ) );
            Icon ic = Icon.FromHandle( ret.GetHicon() );
            ret2 = new Cursor( ic.Handle );
            return ret2;
#if DEBUG
            using ( FileStream fs = new FileStream( @"C:\icon.cur", FileMode.Create ) ) {
#else
            using ( MemoryStream fs = new MemoryStream() ) {
#endif
                CursorUtil.SaveAsCursor( ret, Cursors.Arrow.HotSpot, fs, Color.FromArgb( 255, 0, 255 ) );
                fs.Seek( 0, SeekOrigin.Begin );
                ret2 = new Cursor( fs );
            }
            return ret2;
        }

        /// <summary>
        /// Palette Toolの表示を更新します
        /// </summary>
        public void updatePaletteTool() {
            int count = 0;
            int num_has_dialog = 0;
            for ( Iterator itr = m_palette_tools.iterator(); itr.hasNext(); ){
                ToolStripButton item = (ToolStripButton)itr.next();
                toolStripTool.Items.Remove( item );
            }
            String lang = Messaging.Language;
            boolean first = true;
            for ( Iterator itr = PaletteToolServer.LoadedTools.keySet().iterator(); itr.hasNext(); ){
                String id = (String)itr.next();
                count++;
                IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                Bitmap icon = ipt.getIcon();
                String name = ipt.getName( lang );
                String desc = ipt.getDescription( lang );

                // toolStripPaletteTools
                ToolStripButton tsb;
                if ( icon == null ) {
                    tsb = new ToolStripButton( name );
                } else {
                    tsb = new ToolStripButton( name, icon );
                }
                tsb.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                tsb.ToolTipText = desc;
                tsb.Tag = id;
                tsb.CheckOnClick = false;
                tsb.Click += new EventHandler( commonStripPaletteTool_Clicked );
                if ( first ) {
                    ToolStripSeparator separator = new ToolStripSeparator();
                    m_palette_tools.add( separator );
                    toolStripTool.Items.Add( separator );
                    first = false;
                }
                m_palette_tools.add( tsb );
                toolStripTool.Items.Add( tsb );

                // cMenuTrackSelector
                ToolStripMenuItem tsmi = new ToolStripMenuItem( name );
                tsmi.ToolTipText = desc;
                tsmi.Tag = id;
                tsmi.Click += new EventHandler( commonStripPaletteTool_Clicked );
                cMenuTrackSelectorPaletteTool.DropDownItems.Add( tsmi );

                // cMenuPiano
                ToolStripMenuItem tsmi2 = new ToolStripMenuItem( name );
                tsmi2.ToolTipText = desc;
                tsmi2.Tag = id;
                tsmi2.Click += new EventHandler( commonStripPaletteTool_Clicked );
                cMenuPianoPaletteTool.DropDownItems.Add( tsmi2 );

                // menuSettingPaletteTool
                if ( ipt.hasDialog() ) {
                    ToolStripMenuItem tsmi3 = new ToolStripMenuItem( name );
                    tsmi3.Tag = id;
                    tsmi3.Click += new EventHandler( commonSettingPaletteTool );
                    menuSettingPaletteTool.DropDownItems.Add( tsmi3 );
                    num_has_dialog++;
                }
            }
            if ( count == 0 ) {
                cMenuTrackSelectorPaletteTool.Visible = false;
                cMenuPianoPaletteTool.Visible = false;
            }
            if ( num_has_dialog == 0 ) {
                menuSettingPaletteTool.Visible = false;
            }
        }

        private void commonSettingPaletteTool( object sender, EventArgs e ) {
            if ( sender is ToolStripMenuItem ) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
                if ( tsmi.Tag != null && tsmi.Tag is String ) {
                    String id = (String)tsmi.Tag;
                    if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                        object instance = PaletteToolServer.LoadedTools.get( id );
                        IPaletteTool ipt = (IPaletteTool)instance;
                        if ( ipt.openDialog() == DialogResult.OK ) {
                            XmlStaticMemberSerializer xsms = new XmlStaticMemberSerializer( instance.GetType() );
                            String dir = Path.Combine( AppManager.getApplicationDataPath(), "tool" );
                            if ( !Directory.Exists( dir ) ) {
                                Directory.CreateDirectory( dir );
                            }
                            String cfg = id + ".config";
                            String config = Path.Combine( dir, cfg );
                            using ( FileStream fs = new FileStream( config, FileMode.Create ) ) {
                                xsms.Serialize( fs );
                            }
                        }
                    }
                }
            }
        }

        public void updateCopyAndPasteButtonStatus() {
            // copy cut deleteの表示状態更新
            boolean selected_is_null = (AppManager.getSelectedEventCount() == 0) && 
                                       (AppManager.getSelectedTempoCount() == 0) &&
                                       (AppManager.getSelectedTimesigCount() == 0) &&
                                       (AppManager.getSelectedPointIDCount() == 0);

            cMenuTrackSelectorCopy.Enabled = AppManager.getSelectedPointIDCount() > 0;
            cMenuTrackSelectorCut.Enabled = AppManager.getSelectedPointIDCount() > 0;
            cMenuTrackSelectorDeleteBezier.Enabled = (AppManager.isCurveMode() && AppManager.getLastSelectedBezier() != null);
            cMenuTrackSelectorDelete.Enabled = AppManager.getSelectedPointIDCount() > 0; //todo: このへん。右クリック位置にベジエ制御点などがあった場合eneble=trueにする

            cMenuPianoCopy.Enabled = !selected_is_null;
            cMenuPianoCut.Enabled = !selected_is_null;
            cMenuPianoDelete.Enabled = !selected_is_null;

            menuEditCopy.Enabled = !selected_is_null;
            menuEditCut.Enabled = !selected_is_null;
            menuEditDelete.Enabled = !selected_is_null;

            ClipboardEntry ce = AppManager.getCopiedItems();
            int copy_started_clock = ce.copyStartedClock;
            TreeMap<CurveType, VsqBPList> copied_curve = ce.points;
            TreeMap<CurveType, Vector<BezierChain>> copied_bezier = ce.beziers;
            boolean copied_is_null = (ce.events.size() == 0) &&
                                  (ce.tempo.size() == 0) &&
                                  (ce.timesig.size() == 0) &&
                                  (copied_curve.size() == 0) &&
                                  (copied_bezier.size() == 0);
            boolean enabled = !copied_is_null;
            if ( copied_curve.size() == 1 ) {
                // 1種類のカーブがコピーされている場合→コピーされているカーブの種類と、現在選択されているカーブの種類とで、最大値と最小値が一致している場合のみ、ペースト可能
                CurveType ct = CurveType.Empty;
                for ( Iterator itr = copied_curve.keySet().iterator(); itr.hasNext(); ){
                    CurveType c = (CurveType)itr.next();
                    ct = c;
                }
                CurveType selected = trackSelector.SelectedCurve;
                if ( ct.getMaximum() == selected.getMaximum() &&
                     ct.getMinimum() == selected.getMinimum() &&
                     !selected.IsScalar && !selected.IsAttachNote ) {
                    enabled = true;
                } else {
                    enabled = false;
                }
            } else if ( copied_curve.size() >= 2 ) {
                // 複数種類のカーブがコピーされている場合→そのままペーストすればOK
                enabled = true;
            }
            cMenuTrackSelectorPaste.Enabled = enabled;
            cMenuPianoPaste.Enabled = enabled;
            menuEditPaste.Enabled = enabled;

            /*int copy_started_clock;
            boolean copied_is_null = (AppManager.GetCopiedEvent().Count == 0) &&
                                  (AppManager.GetCopiedTempo( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedTimesig( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedCurve( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedBezier( out copy_started_clock ).Count == 0);
            menuEditCut.Enabled = !selected_is_null;
            menuEditCopy.Enabled = !selected_is_null;
            menuEditDelete.Enabled = !selected_is_null;
            //stripBtnCopy.Enabled = !selected_is_null;
            //stripBtnCut.Enabled = !selected_is_null;

            if ( AppManager.GetCopiedEvent().Count != 0 ) {
                menuEditPaste.Enabled = (AppManager.CurrentClock >= AppManager.VsqFile.getPreMeasureClocks());
                //stripBtnPaste.Enabled = (AppManager.CurrentClock >= AppManager.VsqFile.getPreMeasureClocks());
            } else {
                menuEditPaste.Enabled = !copied_is_null;
                //stripBtnPaste.Enabled = !copied_is_null;
            }*/
        }

        /// <summary>
        /// 現在の編集データを全て破棄する。DirtyCheckは行われない。
        /// </summary>
        private void ClearExistingData() {
            AppManager.clearCommandBuffer();
            AppManager.clearSelectedBezier();
            AppManager.clearSelectedEvent();
            AppManager.clearSelectedTempo();
            AppManager.clearSelectedTimesig();
        }

        /// <summary>
        /// 保存されていない編集内容があるかどうかチェックし、必要なら確認ダイアログを出す。
        /// </summary>
        /// <returns>保存されていない保存内容などない場合、または、保存する必要がある場合で（保存しなくてよいと指定された場合または保存が行われた場合）にtrueを返す</returns>
        private boolean DirtyCheck() {
            if ( m_edited ) {
                String file = AppManager.getFileName();
                if ( file.Equals( "" ) ) {
                    file = "Untitled";
                } else {
                    file = Path.GetFileName( file );
                }
                DialogResult dr = MessageBox.Show( _( "Save this sequence?" ),
                                                   _( "Affirmation" ),
                                                   MessageBoxButtons.YesNoCancel,
                                                   MessageBoxIcon.Question );
                switch ( dr ) {
                    case DialogResult.Yes:
                        if ( AppManager.getFileName().Equals( "" ) ) {
                            DialogResult dr2 = DialogResult.Cancel;
                            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                                    if ( saveXmlVsqDialog.FileName != "" ) {
                                        fd.FileName = saveXmlVsqDialog.FileName;
                                    }
                                    fd.Filter = saveXmlVsqDialog.Filter;
                                    dr2 = fd.ShowDialog();
                                    if ( dr2 == DialogResult.OK ) {
                                        saveXmlVsqDialog.FileName = fd.FileName;
                                    }
                                }
                            } else {
                                dr2 = saveXmlVsqDialog.ShowDialog();
                            }

                            if ( dr2 == DialogResult.OK ) {
                                AppManager.saveTo( saveXmlVsqDialog.FileName );
                                return true;
                            } else {
                                return false;
                            }
                        } else {
                            AppManager.saveTo( AppManager.getFileName() );
                            return true;
                        }
                        break;
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            } else {
                return true;
            }
        }

        /// <summary>
        /// waveView用のwaveファイルを読込むスレッドで使用する
        /// </summary>
        /// <param name="arg"></param>
        private void LoadWaveThreadProc( object arg ) {
            String file = (String)arg;
            waveView.LoadWave( file );
        }

        /// <summary>
        /// menuVisualWaveform.Checkedの値をもとに、splitterContainer2の表示状態を更新します
        /// </summary>
        private void UpdateSplitContainer2Size() {
            if ( menuVisualWaveform.Checked ) {
                splitContainer2.Panel2MinSize = _SPL2_PANEL2_MIN_HEIGHT;
                splitContainer2.IsSplitterFixed = false;
                splitContainer2.SplitterWidth = _SPL_SPLITTER_WIDTH;
                if ( m_last_splitcontainer2_split_distance <= 0 || m_last_splitcontainer2_split_distance > splitContainer2.Height ) {
                    splitContainer2.SplitterDistance = (int)(splitContainer2.Height * 0.9);
                } else {
                    splitContainer2.SplitterDistance = m_last_splitcontainer2_split_distance;
                }
            } else {
                m_last_splitcontainer2_split_distance = splitContainer2.SplitterDistance;
                splitContainer2.Panel2MinSize = 0;
                splitContainer2.SplitterWidth = 0;
                splitContainer2.SplitterDistance = splitContainer2.Height;
                splitContainer2.IsSplitterFixed = true;
            }
        }

        /// <summary>
        /// trackSelectorに表示するコントロールのカーブの種類を、AppManager.EditorConfigの設定に応じて更新します
        /// </summary>
        private void UpdateTrackSelectorVisibleCurve() {
            if ( AppManager.editorConfig.CurveVisibleVelocity ) {
                trackSelector.addViewingCurve( CurveType.VEL );
            }
            if ( AppManager.editorConfig.CurveVisibleAccent ) {
                trackSelector.addViewingCurve( CurveType.Accent );
            }
            if ( AppManager.editorConfig.CurveVisibleDecay ) {
                trackSelector.addViewingCurve( CurveType.Decay );
            }
            if ( AppManager.editorConfig.CurveVisibleVibratoRate ) {
                trackSelector.addViewingCurve( CurveType.VibratoRate );
            }
            if ( AppManager.editorConfig.CurveVisibleVibratoDepth ) {
                trackSelector.addViewingCurve( CurveType.VibratoDepth );
            }
            if ( AppManager.editorConfig.CurveVisibleDynamics ) {
                trackSelector.addViewingCurve( CurveType.DYN );
            }
            if ( AppManager.editorConfig.CurveVisibleBreathiness ) {
                trackSelector.addViewingCurve( CurveType.BRE );
            }
            if ( AppManager.editorConfig.CurveVisibleBrightness ) {
                trackSelector.addViewingCurve( CurveType.BRI );
            }
            if ( AppManager.editorConfig.CurveVisibleClearness ) {
                trackSelector.addViewingCurve( CurveType.CLE );
            }
            if ( AppManager.editorConfig.CurveVisibleOpening ) {
                trackSelector.addViewingCurve( CurveType.OPE );
            }
            if ( AppManager.editorConfig.CurveVisibleGendorfactor ) {
                trackSelector.addViewingCurve( CurveType.GEN );
            }
            if ( AppManager.editorConfig.CurveVisiblePortamento ) {
                trackSelector.addViewingCurve( CurveType.POR );
            }
            if ( AppManager.editorConfig.CurveVisiblePit ) {
                trackSelector.addViewingCurve( CurveType.PIT );
            }
            if ( AppManager.editorConfig.CurveVisiblePbs ){
                trackSelector.addViewingCurve( CurveType.PBS );
            }
            if ( AppManager.editorConfig.CurveVisibleHarmonics ) {
                trackSelector.addViewingCurve( CurveType.harmonics );
            }
            if ( AppManager.editorConfig.CurveVisibleFx2Depth ) {
                trackSelector.addViewingCurve( CurveType.fx2depth );
            }
            if ( AppManager.editorConfig.CurveVisibleReso1 ) {
                trackSelector.addViewingCurve( CurveType.reso1freq );
                trackSelector.addViewingCurve( CurveType.reso1bw );
                trackSelector.addViewingCurve( CurveType.reso1amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso2 ) {
                trackSelector.addViewingCurve( CurveType.reso2freq );
                trackSelector.addViewingCurve( CurveType.reso2bw );
                trackSelector.addViewingCurve( CurveType.reso2amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso3 ) {
                trackSelector.addViewingCurve( CurveType.reso3freq );
                trackSelector.addViewingCurve( CurveType.reso3bw );
                trackSelector.addViewingCurve( CurveType.reso3amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso4 ) {
                trackSelector.addViewingCurve( CurveType.reso4freq );
                trackSelector.addViewingCurve( CurveType.reso4bw );
                trackSelector.addViewingCurve( CurveType.reso4amp );
            }
            if ( AppManager.editorConfig.CurveVisibleEnvelope ) {
                trackSelector.addViewingCurve( CurveType.Env );
            }
            splitContainer1.Panel2MinSize = trackSelector.PreferredMinSize;
            this.MinimumSize = GetWindowMinimumSize();
        }

        /// <summary>
        /// ウィンドウの表示内容に応じて、ウィンドウサイズの最小値を計算します
        /// </summary>
        /// <returns></returns>
        private Size GetWindowMinimumSize() {
            Size current_minsize = this.MinimumSize;
            Size client = this.ClientSize;
            Size current = this.Size;
            return new Size( current_minsize.Width,
                             splitContainer1.Panel2MinSize +
                                _SCROLL_WIDTH + _PICT_POSITION_INDICATOR_HEIGHT + pictPianoRoll.MinimumSize.Height +
                                toolStripContainer.TopToolStripPanel.Height +
                                menuStripMain.Height + statusStrip1.Height +
                                (current.Height - client.Height) +
                                20 );
        }

        /// <summary>
        /// 現在のm_input_textboxの状態を元に、歌詞の変更を反映させるコマンドを実行します
        /// </summary>
        private void ExecuteLyricChangeCommand() {
            if ( !m_input_textbox.Enabled ) {
                return;
            }
            if ( m_input_textbox.IsDisposed ) {
                return;
            }
            int selected = AppManager.getSelected();
            SelectedEventEntry last_selected_event = AppManager.getLastSelectedEvent();
            String original_phrase = last_selected_event.original.ID.LyricHandle.L0.Phrase;
            String original_symbol = last_selected_event.original.ID.LyricHandle.L0.getPhoneticSymbol();
            boolean symbol_protected = last_selected_event.original.ID.LyricHandle.L0.PhoneticSymbolProtected;
            boolean phonetic_symbol_edit_mode = ((TagLyricTextBox)m_input_textbox.Tag).PhoneticSymbolEditMode;
#if DEBUG
            AppManager.debugWriteLine( "    original_phase,symbol=" + original_phrase + "," + original_symbol );
            AppManager.debugWriteLine( "    phonetic_symbol_edit_mode=" + phonetic_symbol_edit_mode );
            AppManager.debugWriteLine( "    m_input_textbox.Text=" + m_input_textbox.Text );
#endif
            String phrase, phonetic_symbol;
            phrase = m_input_textbox.Text;
            if ( !phonetic_symbol_edit_mode ) {
                if ( AppManager.editorConfig.SelfDeRomanization ) {
                    phrase = KanaDeRomanization.Attach( phrase );
                }
            }
            if ( (phonetic_symbol_edit_mode && m_input_textbox.Text != original_symbol) ||
                 (!phonetic_symbol_edit_mode && phrase != original_phrase) ) {
                TagLyricTextBox kvp = (TagLyricTextBox)m_input_textbox.Tag;
                if ( phonetic_symbol_edit_mode ) {
                    phrase = kvp.BufferText;
                    phonetic_symbol = m_input_textbox.Text;
                    String[] spl = phonetic_symbol.Split( " ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
                    Vector<String> list = new Vector<String>();
                    foreach ( String s in spl ) {
                        if ( VsqPhoneticSymbol.isValidSymbol( s ) ) {
                            list.add( s );
                        }
                    }
                    phonetic_symbol = "";
                    boolean first = true;
                    for ( Iterator itr = list.iterator(); itr.hasNext(); ){
                        String s = (String)itr.next();
                        if ( first ) {
                            phonetic_symbol += s;
                            first = false;
                        } else {
                            phonetic_symbol += " " + s;
                        }
                    }
                    symbol_protected = true;
                } else {
                    if ( !symbol_protected ) {
                        SymbolTable.attatch( phrase, out phonetic_symbol );
                    } else {
                        phonetic_symbol = original_symbol;
                    }
                }
#if DEBUG
                AppManager.debugWriteLine( "    phrase,phonetic_symbol=" + phrase + "," + phonetic_symbol );
#endif
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandEventChangeLyric( selected,
                                                      AppManager.getLastSelectedEvent().original.InternalID,
                                                      phrase,
                                                      phonetic_symbol,
                                                      symbol_protected ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
            }
        }

        private void GameControlerRemove() {
            if ( m_timer != null ) {
                m_timer.Stop();
                m_timer.Dispose();
                m_timer = null;
            }
            m_game_mode = GameControlMode.DISABLED;
            UpdateGameControlerStatus();
        }

        private void GameControlerLoad() {
            try {
                boolean init_success = false;
                int num_joydev = winmmhelp.JoyInit();
                if ( num_joydev <= 0 ) {
                    init_success = false;
                } else {
                    init_success = true;
                }
                if ( init_success ) {
                    m_game_mode = GameControlMode.NORMAL;
                    stripLblGameCtrlMode.Image = null;
                    stripLblGameCtrlMode.Text = m_game_mode.ToString();
                    m_timer = new System.Windows.Forms.Timer();
                    m_timer.Interval = 10;
                    m_timer.Tick += new EventHandler( m_timer_Tick );
                    m_timer.Start();
                } else {
                    m_game_mode = GameControlMode.DISABLED;
                }
            } catch ( Exception ex ) {
                m_game_mode = GameControlMode.DISABLED;
#if DEBUG
                AppManager.debugWriteLine( "FormMain+ReloadGameControler" );
                AppManager.debugWriteLine( "    ex=" + ex );
#endif
            }
            UpdateGameControlerStatus();
        }

        private void ReloadMidiIn() {
#if DEBUG
            AppManager.debugWriteLine( "FormMain.ReloadMidiIn" );
#endif
            if ( m_midi_in != null ) {
                m_midi_in.MidiReceived -= m_midi_in_MidiReceived;
                m_midi_in.Dispose();
            }
            try {
                m_midi_in = new MidiInDevice( AppManager.editorConfig.MidiInPort.PortNumber );
                m_midi_in.MidiReceived += m_midi_in_MidiReceived;
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.debugWriteLine( "    ex=" + ex );
#endif
            }
            UpdateMidiInStatus();
        }

        private void m_midi_in_MidiReceived( DateTime time, byte[] data ) {
            if ( data.Length <= 2 ) {
                return;
            }
            if ( !AppManager.isPlaying() ) {
                return;
            }
            int code = data[0] & 0xf0;
#if DEBUG
            AppManager.debugWriteLine( "m_midi_in_MidiReceived" );
            AppManager.debugWriteLine( "    code=0x" + Convert.ToString( code, 16 ) );
#endif
            if ( code != 0x80 && code != 0x90 ) {
                return;
            }
            if ( code == 0x90 && data[2] == 0x00 ) {
                code = 0x80;//ベロシティ0のNoteOnはNoteOff
            }

            byte note = data[1];
            if ( code == 0x90 && AppManager.getEditMode() == EditMode.REALTIME ) {
                MidiPlayer.PlayImmediate( note );
            }

            int clock = AppManager.getCurrentClock();
            int unit = AppManager.getPositionQuantizeClock();
            if ( unit > 1 ) {
                int odd = clock % unit;
                int nclock = clock;
                nclock -= odd;
                if ( odd > unit / 2 ) {
                    nclock += unit;
                }
                clock = nclock;
            }

            if ( code == 0x80 ) {
                if ( m_adding != null ) {
                    int len = clock - m_adding.Clock;
                    if ( len <= 0 ) {
                        len = unit;
                    }
                    m_adding.ID.Length = len;
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( AppManager.getSelected(),
                                                                                                   m_adding ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    if ( !Edited ) {
                        Edited = true;
                    }
#if USE_DOBJ
                    UpdateDrawObjectList();
#endif
                }
            } else if ( code == 0x90 ) {
                m_adding = new VsqEvent( clock, new VsqID( 0 ) );
                m_adding.ID.type = VsqIDType.Anote;
                m_adding.ID.Dynamics = 64;
                m_adding.ID.VibratoHandle = null;
                m_adding.ID.LyricHandle = new LyricHandle( "a", "a" );
                m_adding.ID.Note = note;
                if ( AppManager.getEditMode() != EditMode.REALTIME ) {
                    KeySoundPlayer.Play( note );
                }
            }
        }

        /// <summary>
        /// 現在のゲームコントローラのモードに応じてstripLblGameCtrlModeの表示状態を更新します。
        /// </summary>
        private void UpdateGameControlerStatus() {
            if ( m_game_mode == GameControlMode.DISABLED ) {
                stripLblGameCtrlMode.Text = _( "Disabled" );
                stripLblGameCtrlMode.Image = (Bitmap)Properties.Resources.slash.Clone();
            } else if ( m_game_mode == GameControlMode.CURSOR ) {
                stripLblGameCtrlMode.Text = _( "Cursor" );
                stripLblGameCtrlMode.Image = null;
            } else if ( m_game_mode == GameControlMode.KEYBOARD ) {
                stripLblGameCtrlMode.Text = _( "Keyboard" );
                stripLblGameCtrlMode.Image = (Bitmap)Properties.Resources.piano.Clone();
            } else if ( m_game_mode == GameControlMode.NORMAL ) {
                stripLblGameCtrlMode.Text = _( "Normal" );
                stripLblGameCtrlMode.Image = null;
            }
        }

        private void UpdateMidiInStatus() {
            int midiport = AppManager.editorConfig.MidiInPort.PortNumber;
            bocoree.MIDIINCAPS[] devices = MidiInDevice.GetMidiInDevices();
            if ( midiport < 0 || devices.Length <= 0 ) {
                stripLblMidiIn.Text = _( "Disabled" );
                stripLblMidiIn.Image = (Bitmap)Properties.Resources.slash.Clone();
            } else {
                if ( midiport >= devices.Length ) {
                    midiport = 0;
                    AppManager.editorConfig.MidiInPort.PortNumber = midiport;
                }
                stripLblMidiIn.Text = devices[midiport].szPname;
                stripLblMidiIn.Image = (Bitmap)Properties.Resources.piano.Clone();
            }
        }

        /// <summary>
        /// スクリプトフォルダ中のスクリプトへのショートカットを作成する
        /// </summary>
        private void UpdateScriptShortcut() {
            TreeMap<String, ScriptInvoker> old = new TreeMap<String, ScriptInvoker>();
            foreach ( ToolStripItem item in menuScript.DropDownItems ) {
                if ( !(item is ToolStripMenuItem) ) {
                    continue;
                }
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if ( tsmi.DropDownItems.Count <= 0 ) {
                    continue;
                }
                if ( tsmi.DropDownItems[0].Tag != null && tsmi.DropDownItems[0].Tag is ScriptInvoker ) {
                    ScriptInvoker si = (ScriptInvoker)tsmi.DropDownItems[0].Tag;
                    old.put( si.ScriptFile, si );
                }
            }
            menuScript.DropDownItems.Clear();
            String script_path = Path.Combine( Application.StartupPath, "script" );
            if ( !Directory.Exists( script_path ) ) {
                Directory.CreateDirectory( script_path );
            }

            DirectoryInfo current = new DirectoryInfo( script_path );
            int count = 0;
            Vector<FileInfo> files = new Vector<FileInfo>( current.GetFiles( "*.txt" ) );
            files.addAll( current.GetFiles( "*.cs" ) );
            for ( Iterator itr = files.iterator(); itr.hasNext(); ){
                FileInfo fi = (FileInfo)itr.next();
                count++;
                String fname = fi.FullName;
                String scriptname = Path.GetFileNameWithoutExtension( fname );
                ToolStripMenuItem item = new ToolStripMenuItem( scriptname );
                ToolStripMenuItem dd_run = new ToolStripMenuItem( _( "Run" ) + "(&R)" );
                dd_run.Name = "menuScript" + scriptname + "Run";
                if ( old.containsKey( fname ) && old.get( fname ) != null ) {
                    dd_run.Tag = old.get( fname );
                } else {
                    ScriptInvoker si2 = new ScriptInvoker();
                    si2.FileTimestamp = DateTime.MinValue;
                    si2.ScriptFile = fname;
                    dd_run.Tag = si2;
                }
                dd_run.Click += new EventHandler( dd_run_Click );
                item.DropDownItems.Add( dd_run );
                menuScript.DropDownItems.Add( item );
            }
            old.clear();
            if ( count > 0 ) {
                menuScript.DropDownItems.Add( new ToolStripSeparator() );
            }
            menuScript.DropDownItems.Add( menuScriptUpdate );
            Misc.ApplyToolStripFontRecurse( menuScript, AppManager.editorConfig.BaseFont );
            ApplyShortcut();
        }

        private void dd_run_Click( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "dd_run_Click" );
#endif
            try {
                ScriptInvoker si = (ScriptInvoker)((ToolStripMenuItem)sender).Tag;
                String script_file = si.ScriptFile;
#if DEBUG
                AppManager.debugWriteLine( "    si.FileTimestamp=" + si.FileTimestamp );
                AppManager.debugWriteLine( "    File.GetLastWriteTimeUtc( script_file )=" + File.GetLastWriteTimeUtc( script_file ) );
#endif
                if ( si.FileTimestamp != File.GetLastWriteTimeUtc( script_file ) ||
                     si.ScriptType == null ||
                     si.Serializer == null ||
                     si.scriptDelegate == null ) {

                    si = AppManager.loadScript( script_file );
                    ((ToolStripMenuItem)sender).Tag = si;
#if DEBUG
                    AppManager.debugWriteLine( "    err_msg=" + si.ErrorMessage );
#endif
                }
                if ( si.scriptDelegate != null ) {
                    if ( AppManager.invokeScript( si ) ) {
                        Edited = true;
#if USE_DOBJ
                        UpdateDrawObjectList();
#endif
                        refreshScreen();
                    }
                } else {
                    using ( FormCompileResult dlg = new FormCompileResult( _( "Failed loading script." ), si.ErrorMessage ) ) {
                        dlg.ShowDialog();
                    }
                }
            } catch {
            }
        }

        /// <summary>
        /// プレイカーソルが見えるようスクロールする
        /// </summary>
        public void ensureCursorVisible() {
            // カーソルが画面内にあるかどうか検査
            int clock_left = clockFromXCoord( AppManager.KEY_LENGTH );
            int clock_right = clockFromXCoord( pictPianoRoll.Width );
            int uwidth = clock_right - clock_left;
            if ( AppManager.getCurrentClock() < clock_left || clock_right < AppManager.getCurrentClock() ) {
                int cl_new_center = (AppManager.getCurrentClock() / uwidth) * uwidth + uwidth / 2;
                float f_draft = cl_new_center - (pictPianoRoll.Width / 2 + 34 - 70) / AppManager.scaleX;
                if ( f_draft < 0f ) {
                    f_draft = 0;
                }
                int draft = (int)(f_draft);
                if ( draft < hScroll.Minimum ) {
                    draft = hScroll.Minimum;
                } else if ( hScroll.Maximum < draft ) {
                    draft = hScroll.Maximum;
                }
                if ( hScroll.Value != draft ) {
                    m_draw_start_index[AppManager.getSelected() - 1] = 0;
                    hScroll.Value = draft;
                }
            }
        }

        private void ProcessSpecialShortcutKey( PreviewKeyDownEventArgs e ) {
            if ( !m_input_textbox.Enabled ) {
                if ( e.KeyCode == Keys.Return ) {
                    if ( AppManager.isPlaying() ) {
                        timer.Stop();
                    }
                    AppManager.setPlaying( !AppManager.isPlaying() );
                } else if ( e.KeyCode == Keys.Space ) {
                    //m_
                } else if ( e.KeyCode == Keys.OemPeriod ) {
                    if ( AppManager.isPlaying() ) {
                        AppManager.setPlaying( false );
                    } else {
                        if ( !AppManager.startMarkerEnabled ) {
                            AppManager.setCurrentClock( 0 );
                        } else {
                            AppManager.setCurrentClock( AppManager.startMarker );
                        }
                        refreshScreen();
                    }
                } else if ( e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Right ) {
                    Forward();
                } else if ( e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Left ) {
                    Rewind();
                }
            }
            return;
            #region OLD CODES DO NOT REMOVE
            /*if ( AppManager.EditorConfig.Platform == Platform.Macintosh ) {
                if ( AppManager.EditorConfig.CommandKeyAsControl ) {
                    #region menuStripMain
                    if ( e.Alt && e.KeyCode == Keys.N && menuFileNew.Enabled ) {
                        this.menuFileNew_Click( menuFileNew, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.O && menuFileOpen.Enabled ) {
                        this.menuFileOpen_Click( menuFileOpen, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.S && menuFileSave.Enabled ) {
                        this.menuFileSave_Click( menuFileSave, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.Q && menuFileQuit.Enabled ) {
                        this.menuFileQuit_Click( menuFileQuit, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.Z && menuEditUndo.Enabled ) {
                        this.menuEditUndo_Click( menuEditUndo, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.Z && menuEditRedo.Enabled ) {
                        this.menuEditRedo_Click( this.menuEditRedo, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.X && this.menuEditCut.Enabled ) {
                        this.menuEditCut_Click( this.menuEditCut, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.C && this.menuEditCopy.Enabled ) {
                        this.menuEditCopy_Click( this.menuEditCopy, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.V && this.menuEditPaste.Enabled ) {
                        this.menuEditPaste_Click( this.menuEditPaste, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.A && this.menuEditSelectAll.Enabled ) {
                        this.menuEditSelectAll_Click( this.menuEditSelectAll, null );
                        return;
                    } else if ( e.Alt && e.Shift && this.menuEditSelectAllEvents.Enabled ) {
                        this.menuEditSelectAllEvents_Click( this.menuEditSelectAllEvents, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.V && this.menuHiddenEditPaste.Enabled ) {
                        this.menuHiddenEditPaste_Click( this.menuHiddenEditPaste, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.W && this.menuHiddenEditFlipToolPointerPencil.Enabled ) {
                        this.menuHiddenEditFlipToolPointerPencil_Click( this.menuHiddenEditFlipToolPointerPencil, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.E && this.menuHiddenEditFlipToolPointerEraser.Enabled ) {
                        this.menuHiddenEditFlipToolPointerEraser_Click( this.menuHiddenEditFlipToolPointerEraser, null );
                        return;
                    } else if ( (e.KeyCode & Keys.Clear) == Keys.Clear && e.Alt && e.Shift && this.menuHiddenVisualForwardParameter.Enabled ) {
                        this.menuHiddenVisualForwardParameter_Click( this.menuHiddenVisualForwardParameter, null );
                        return;
                    } else if ( (e.KeyCode & Keys.LButton) == Keys.LButton && (e.KeyCode & Keys.LineFeed) == Keys.LineFeed && e.Alt && e.Shift && this.menuHiddenVisualBackwardParameter.Enabled ) {
                        this.menuHiddenVisualBackwardParameter_Click( this.menuHiddenVisualBackwardParameter, null );
                        return;
                    } else if ( (e.KeyCode & Keys.Clear) == Keys.Clear && e.Alt && this.menuHiddenTrackNext.Enabled ) {
                        this.menuHiddenTrackNext_Click( this.menuHiddenTrackNext, null );
                        return;
                    } else if ( (e.KeyCode & Keys.LButton) == Keys.LButton && (e.KeyCode & Keys.LineFeed) == Keys.LineFeed && e.Alt && this.menuHiddenTrackBack.Enabled ) {
                        this.menuHiddenTrackBack_Click( this.menuHiddenTrackBack, null );
                        return;
                    }
                    #endregion

                    #region cMenuPiano
                    if ( e.Alt && e.KeyCode == Keys.Z && cMenuPianoUndo.Enabled ) {
                        this.cMenuPianoUndo_Click( this.cMenuPianoUndo, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.Z && this.cMenuPianoRedo.Enabled ) {
                        this.cMenuPianoRedo_Click( this.cMenuPianoRedo, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.X && this.cMenuPianoCut.Enabled ) {
                        this.cMenuPianoCut_Click( this.cMenuPianoCut, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.C && this.cMenuPianoCopy.Enabled ) {
                        this.cMenuPianoCopy_Click( this.cMenuPianoCopy, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.A && cMenuPianoSelectAll.Enabled ) {
                        this.cMenuPianoSelectAll_Click( this.cMenuPianoSelectAll, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.A && cMenuPianoSelectAllEvents.Enabled ) {
                        this.cMenuPianoSelectAllEvents_Click( this.cMenuPianoSelectAllEvents, null );
                        return;
                    }
                    #endregion

                    #region cMenuTrackSelector
                    if ( e.Alt && e.KeyCode == Keys.Z && cMenuTrackSelectorUndo.Enabled ) {
                        this.cMenuTrackSelectorUndo_Click( this.cMenuTrackSelectorUndo, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.Z && this.cMenuTrackSelectorRedo.Enabled ) {
                        this.cMenuTrackSelectorRedo_Click( this.cMenuTrackSelectorRedo, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.X && this.cMenuTrackSelectorCut.Enabled ) {
                        this.cMenuTrackSelectorCut_Click( this.cMenuTrackSelectorCut, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.C && this.cMenuTrackSelectorCopy.Enabled ) {
                        this.cMenuTrackSelectorCopy_Click( this.cMenuTrackSelectorCopy, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.V && this.cMenuTrackSelectorPaste.Enabled ) {
                        this.cMenuTrackSelectorPaste_Click( this.cMenuTrackSelectorPaste, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.A && this.cMenuTrackSelectorSelectAll.Enabled ) {
                        this.cMenuTrackSelectorSelectAll_Click( this.cMenuTrackSelectorSelectAll, null );
                        return;
                    }
                    #endregion
                } else {
                    boolean RButton = (e.KeyCode & Keys.RButton) == Keys.RButton;
                    boolean Clear = (e.KeyCode & Keys.Clear) == Keys.Clear;
                    boolean Return = (e.KeyCode & Keys.Return) == Keys.Return;
                    boolean Pause = (e.KeyCode & Keys.Pause) == Keys.Pause;
                    boolean FinalMode = (e.KeyCode & Keys.FinalMode) == Keys.FinalMode;
                    boolean Cancel = (e.KeyCode & Keys.Cancel) == Keys.Cancel;
                    boolean CapsLock = (e.KeyCode & Keys.CapsLock) == Keys.CapsLock;
                    boolean LButton = (e.KeyCode & Keys.LButton) == Keys.LButton;
                    boolean JunjaMode = (e.KeyCode & Keys.JunjaMode) == Keys.JunjaMode;
                    boolean LineFeed = (e.KeyCode & Keys.LineFeed) == Keys.LineFeed;
                    boolean ControlKey = (e.KeyCode & Keys.ControlKey) == Keys.ControlKey;
                    boolean XButton1 = (e.KeyCode & Keys.XButton1) == Keys.XButton1;

                    #region menuStripMain
                    if ( RButton && Clear && (e.KeyCode & Keys.N) == Keys.N && menuFileNew.Enabled ) {
                        this.menuFileNew_Click( menuFileNew, null );
                        return;
                    } else if ( RButton && Return && (e.KeyCode & Keys.O) == Keys.O && menuFileOpen.Enabled ) {
                        this.menuFileOpen_Click( menuFileOpen, null );
                        return;
                    } else if ( Pause && (e.KeyCode & Keys.S) == Keys.S && menuFileSave.Enabled ) {
                        this.menuFileSave_Click( menuFileSave, null );
                        return;
                    } else if ( ControlKey && (e.KeyCode & Keys.Q) == Keys.Q && menuFileQuit.Enabled ) {
                        this.menuFileQuit_Click( menuFileQuit, null );
                        return;
                    } else if ( RButton && FinalMode && (e.KeyCode & Keys.Z) == Keys.Z && menuEditUndo.Enabled ) {
                        this.menuEditUndo_Click( menuEditUndo, null );
                        return;
                    } else if ( RButton && FinalMode && e.Shift && (e.KeyCode & Keys.Z) == Keys.Z && menuEditRedo.Enabled ) {
                        this.menuEditRedo_Click( this.menuEditRedo, null );
                        return;
                    } else if ( FinalMode && (e.KeyCode & Keys.X) == Keys.X && this.menuEditCut.Enabled ) {
                        this.menuEditCut_Click( this.menuEditCut, null );
                        return;
                    } else if ( Cancel && (e.KeyCode & Keys.C) == Keys.C && this.menuEditCopy.Enabled ) {
                        this.menuEditCopy_Click( this.menuEditCopy, null );
                        return;
                    } else if ( RButton && CapsLock && (e.KeyCode & Keys.V) == Keys.V && this.menuEditPaste.Enabled ) {
                        this.menuEditPaste_Click( this.menuEditPaste, null );
                        return;
                    } else if ( LButton && (e.KeyCode & Keys.A) == Keys.A && this.menuEditSelectAll.Enabled ) {
                        this.menuEditSelectAll_Click( this.menuEditSelectAll, null );
                        return;
                    } else if ( LButton && e.Shift && (e.KeyCode & Keys.A) == Keys.A && this.menuEditSelectAllEvents.Enabled ) {
                        this.menuEditSelectAllEvents_Click( this.menuEditSelectAllEvents, null );
                        return;
                    } else if ( RButton && CapsLock && (e.KeyCode & Keys.V) == Keys.V && this.menuHiddenEditPaste.Enabled ) {
                        this.menuHiddenEditPaste_Click( this.menuHiddenEditPaste, null );
                        return;
                    } else if ( JunjaMode && (e.KeyCode & Keys.W) == Keys.W && this.menuHiddenEditFlipToolPointerPencil.Enabled ) {
                        this.menuHiddenEditFlipToolPointerPencil_Click( this.menuHiddenEditFlipToolPointerPencil, null );
                        return;
                    } else if ( XButton1 && (e.KeyCode & Keys.E) == Keys.E && this.menuHiddenEditFlipToolPointerEraser.Enabled ) {
                        this.menuHiddenEditFlipToolPointerEraser_Click( this.menuHiddenEditFlipToolPointerEraser, null );
                        return;
                    } else if ( Clear && e.Control && e.Shift && this.menuHiddenVisualForwardParameter.Enabled ) {
                        this.menuHiddenVisualForwardParameter_Click( this.menuHiddenVisualForwardParameter, null );
                        return;
                    } else if ( LButton && LineFeed && e.Control && e.Shift && this.menuHiddenVisualBackwardParameter.Enabled ) {
                        this.menuHiddenVisualBackwardParameter_Click( this.menuHiddenVisualBackwardParameter, null );
                        return;
                    } else if ( Clear && e.Control && this.menuHiddenTrackNext.Enabled ) {
                        this.menuHiddenTrackNext_Click( this.menuHiddenTrackNext, null );
                        return;
                    } else if ( LButton && LineFeed && e.Control && this.menuHiddenTrackBack.Enabled ) {
                        this.menuHiddenTrackBack_Click( this.menuHiddenTrackBack, null );
                        return;
                    }
                    #endregion

                    #region cMenuPiano
                    if ( RButton && FinalMode && (e.KeyCode & Keys.Z) == Keys.Z && cMenuPianoUndo.Enabled ) {
                        this.cMenuPianoUndo_Click( this.cMenuPianoUndo, null );
                        return;
                    } else if ( RButton && FinalMode && e.Shift && (e.KeyCode & Keys.Z) == Keys.Z && this.cMenuPianoRedo.Enabled ) {
                        this.cMenuPianoRedo_Click( this.cMenuPianoRedo, null );
                        return;
                    } else if ( FinalMode && (e.KeyCode & Keys.X) == Keys.X && this.cMenuPianoCut.Enabled ) {
                        this.cMenuPianoCut_Click( this.cMenuPianoCut, null );
                        return;
                    } else if ( Cancel && (e.KeyCode & Keys.C) == Keys.C && this.cMenuPianoCopy.Enabled ) {
                        this.cMenuPianoCopy_Click( this.cMenuPianoCopy, null );
                        return;
                    } else if ( LButton && (e.KeyCode & Keys.A) == Keys.A && cMenuPianoSelectAll.Enabled ) {
                        this.cMenuPianoSelectAll_Click( this.cMenuPianoSelectAll, null );
                        return;
                    } else if ( LButton && e.Shift && (e.KeyCode & Keys.A) == Keys.A && cMenuPianoSelectAllEvents.Enabled ) {
                        this.cMenuPianoSelectAllEvents_Click( this.cMenuPianoSelectAllEvents, null );
                        return;
                    }
                    #endregion

                    #region cMenuTrackSelector
                    if ( RButton && FinalMode && (e.KeyCode & Keys.Z) == Keys.Z && cMenuTrackSelectorUndo.Enabled ) {
                        this.cMenuTrackSelectorUndo_Click( this.cMenuTrackSelectorUndo, null );
                        return;
                    } else if ( RButton && FinalMode && e.Shift && (e.KeyCode & Keys.Z) == Keys.Z && this.cMenuTrackSelectorRedo.Enabled ) {
                        this.cMenuTrackSelectorRedo_Click( this.cMenuTrackSelectorRedo, null );
                        return;
                    } else if ( FinalMode && (e.KeyCode & Keys.X) == Keys.X && this.cMenuTrackSelectorCut.Enabled ) {
                        this.cMenuTrackSelectorCut_Click( this.cMenuTrackSelectorCut, null );
                        return;
                    } else if ( Cancel && (e.KeyCode & Keys.C) == Keys.C && this.cMenuTrackSelectorCopy.Enabled ) {
                        this.cMenuTrackSelectorCopy_Click( this.cMenuTrackSelectorCopy, null );
                        return;
                    } else if ( RButton && CapsLock && (e.KeyCode & Keys.V) == Keys.V && this.cMenuTrackSelectorPaste.Enabled ) {
                        this.cMenuTrackSelectorPaste_Click( this.cMenuTrackSelectorPaste, null );
                        return;
                    } else if ( LButton && e.Shift && (e.KeyCode & Keys.A) == Keys.A && this.cMenuTrackSelectorSelectAll.Enabled ) {
                        this.cMenuTrackSelectorSelectAll_Click( this.cMenuTrackSelectorSelectAll, null );
                        return;
                    }
                    #endregion
                }
            } else {
                #region menuStripMain
                if ( e.Control && e.KeyCode == Keys.N && menuFileNew.Enabled ) {
                    this.menuFileNew_Click( menuFileNew, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.O && menuFileOpen.Enabled ) {
                    this.menuFileOpen_Click( menuFileOpen, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.S && menuFileSave.Enabled ) {
                    this.menuFileSave_Click( menuFileSave, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.Q && menuFileQuit.Enabled ) {
                    this.menuFileQuit_Click( menuFileQuit, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.Z && menuEditUndo.Enabled ) {
                    this.menuEditUndo_Click( menuEditUndo, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.Z && menuEditRedo.Enabled ) {
                    this.menuEditRedo_Click( this.menuEditRedo, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.X && this.menuEditCut.Enabled ) {
                    this.menuEditCut_Click( this.menuEditCut, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.C && this.menuEditCopy.Enabled ) {
                    this.menuEditCopy_Click( this.menuEditCopy, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.V && this.menuEditPaste.Enabled ) {
                    this.menuEditPaste_Click( this.menuEditPaste, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.A && this.menuEditSelectAll.Enabled ) {
                    this.menuEditSelectAll_Click( this.menuEditSelectAll, null );
                    return;
                } else if ( e.Control && e.Shift && this.menuEditSelectAllEvents.Enabled ) {
                    this.menuEditSelectAllEvents_Click( this.menuEditSelectAllEvents, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.V && this.menuHiddenEditPaste.Enabled ) {
                    this.menuHiddenEditPaste_Click( this.menuHiddenEditPaste, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.W && this.menuHiddenEditFlipToolPointerPencil.Enabled ) {
                    this.menuHiddenEditFlipToolPointerPencil_Click( this.menuHiddenEditFlipToolPointerPencil, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.E && this.menuHiddenEditFlipToolPointerEraser.Enabled ) {
                    this.menuHiddenEditFlipToolPointerEraser_Click( this.menuHiddenEditFlipToolPointerEraser, null );
                    return;
                } else if ( e.Control && e.Alt && (e.KeyCode & Keys.PageDown) == Keys.PageDown && this.menuHiddenVisualForwardParameter.Enabled ) {
                    this.menuHiddenVisualForwardParameter_Click( this.menuHiddenVisualForwardParameter, null );
                    return;
                } else if ( e.Control && e.Alt && (e.KeyCode & Keys.PageUp) == Keys.PageUp && this.menuHiddenVisualBackwardParameter.Enabled ) {
                    this.menuHiddenVisualBackwardParameter_Click( this.menuHiddenVisualBackwardParameter, null );
                    return;
                } else if ( e.Control && (e.KeyCode & Keys.PageDown) == Keys.PageDown && this.menuHiddenTrackNext.Enabled ) {
                    this.menuHiddenTrackNext_Click( this.menuHiddenTrackNext, null );
                    return;
                } else if ( e.Control && (e.KeyCode & Keys.PageUp) == Keys.PageUp && this.menuHiddenTrackBack.Enabled ) {
                    this.menuHiddenTrackBack_Click( this.menuHiddenTrackBack, null );
                    return;
                }
                #endregion

                #region cMenuPiano
                if ( e.Control && e.KeyCode == Keys.Z && cMenuPianoUndo.Enabled ) {
                    this.cMenuPianoUndo_Click( this.cMenuPianoUndo, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.Z && this.cMenuPianoRedo.Enabled ) {
                    this.cMenuPianoRedo_Click( this.cMenuPianoRedo, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.X && this.cMenuPianoCut.Enabled ) {
                    this.cMenuPianoCut_Click( this.cMenuPianoCut, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.C && this.cMenuPianoCopy.Enabled ) {
                    this.cMenuPianoCopy_Click( this.cMenuPianoCopy, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.A && cMenuPianoSelectAll.Enabled ) {
                    this.cMenuPianoSelectAll_Click( this.cMenuPianoSelectAll, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.A && cMenuPianoSelectAllEvents.Enabled ) {
                    this.cMenuPianoSelectAllEvents_Click( this.cMenuPianoSelectAllEvents, null );
                    return;
                }
                #endregion

                #region cMenuTrackSelector
                if ( e.Control && e.KeyCode == Keys.Z && cMenuTrackSelectorUndo.Enabled ) {
                    this.cMenuTrackSelectorUndo_Click( this.cMenuTrackSelectorUndo, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.Z && this.cMenuTrackSelectorRedo.Enabled ) {
                    this.cMenuTrackSelectorRedo_Click( this.cMenuTrackSelectorRedo, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.X && this.cMenuTrackSelectorCut.Enabled ) {
                    this.cMenuTrackSelectorCut_Click( this.cMenuTrackSelectorCut, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.C && this.cMenuTrackSelectorCopy.Enabled ) {
                    this.cMenuTrackSelectorCopy_Click( this.cMenuTrackSelectorCopy, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.V && this.cMenuTrackSelectorPaste.Enabled ) {
                    this.cMenuTrackSelectorPaste_Click( this.cMenuTrackSelectorPaste, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.A && this.cMenuTrackSelectorSelectAll.Enabled ) {
                    this.cMenuTrackSelectorSelectAll_Click( this.cMenuTrackSelectorSelectAll, null );
                    return;
                }
                #endregion
            }*/
            #endregion
        }

        private void SetHScrollRange( int draft_length ) {
            const int _ARROWS = 40; // 両端の矢印の表示幅px（おおよその値）
            draft_length += 240;
            if ( draft_length > hScroll.Maximum ) {
                hScroll.Maximum = draft_length;
            }
            int large_change = (int)((pictPianoRoll.Width - AppManager.KEY_LENGTH) / (float)AppManager.scaleX);
            int box_width = (int)((hScroll.Width - _ARROWS) * large_change / (float)(hScroll.Maximum + large_change));
            if ( box_width < AppManager.editorConfig.MinimumScrollHandleWidth ) {
                box_width = AppManager.editorConfig.MinimumScrollHandleWidth;
                large_change = (int)((float)hScroll.Maximum * (float)box_width / (float)(hScroll.Width - _ARROWS - box_width));
            }
            if ( large_change > 0 ) {
                hScroll.LargeChange = large_change;
            }
        }

        private void SetVScrollRange( int draft_length ) {
            const int _ARROWS = 40; // 両端の矢印の表示幅px（おおよその値）
            if ( draft_length > vScroll.Maximum ) {
                vScroll.Maximum = draft_length;
            }
            int large_change = (int)pictPianoRoll.Height;
            int box_width = (int)((vScroll.Height - _ARROWS) * large_change / (float)(vScroll.Maximum + large_change));
            if ( box_width < AppManager.editorConfig.MinimumScrollHandleWidth ) {
                box_width = AppManager.editorConfig.MinimumScrollHandleWidth;
                large_change = (int)((float)vScroll.Maximum * (float)box_width / (float)(vScroll.Width - _ARROWS - box_width));
            }
            if ( large_change > 0 ) {
                vScroll.LargeChange = large_change;
            }
        }

        private static void ApplyRendererRecurse( Control control, ToolStripProfessionalRenderer renderer ) {
            ToolStrip ts = control as ToolStrip;
            if ( ts != null ) {
                if ( renderer == null ) {
                    ts.RenderMode = ToolStripRenderMode.System;
                } else {
                    ts.Renderer = renderer;
                }
            }
            MenuStrip ms = control as MenuStrip;
            if ( ms != null ) {
                if ( renderer == null ) {
                    ms.RenderMode = ToolStripRenderMode.System;
                } else {
                    ms.Renderer = renderer;
                }
            }
            foreach ( Control c in control.Controls ) {
                ApplyRendererRecurse( c, renderer );
            }
        }

        private void RefreshScreenCore() {
            pictPianoRoll.Refresh();
            picturePositionIndicator.Refresh();
            trackSelector.Refresh();
            if ( menuVisualWaveform.Checked ) {
                waveView.Draw();
                waveView.Refresh();
            }
            if ( AppManager.editorConfig.OverviewEnabled ) {
                pictOverview.Refresh();
            }
        }

        public void refreshScreen() {
//#if DEBUG
//            RefreshScreenCore();
//#else
            if ( !bgWorkScreen.IsBusy ) {
                bgWorkScreen.RunWorkerAsync();
            }
//#endif
        }

        public void flipMixerDialogVisible( boolean visible ) {
            AppManager.mixerWindow.Visible = visible;
            AppManager.editorConfig.MixerVisible = visible;
            menuVisualMixer.Checked = visible;
        }

        /// <summary>
        /// メニューのショートカットキーを、AppManager.EditorConfig.ShorcutKeysの内容に応じて変更します
        /// </summary>
        private void ApplyShortcut() {
            if ( AppManager.editorConfig.Platform == Platform.Macintosh ) {
                #region Platform.Macintosh
                String _CO = "";
                //if ( AppManager.EditorConfig.CommandKeyAsControl ) {
                _CO = new String( '\x2318', 1 );
                //} else {
                //_CO = "^";
                //}
                String _SHIFT = "⇧";
                //if ( AppManager.EditorConfig.CommandKeyAsControl ) {
                #region menuStripMain
                menuFileNew.ShortcutKeys = Keys.Alt | Keys.N;
                menuFileOpen.ShortcutKeys = Keys.Alt | Keys.O;
                menuFileSave.ShortcutKeys = Keys.Alt | Keys.S;
                menuFileQuit.ShortcutKeys = Keys.Alt | Keys.Q;

                menuEditUndo.ShortcutKeys = Keys.Alt | Keys.Z;
                menuEditRedo.ShortcutKeys = Keys.Alt | Keys.Shift | Keys.Z;
                menuEditCut.ShortcutKeys = Keys.Alt | Keys.X;
                menuEditCopy.ShortcutKeys = Keys.Alt | Keys.C;
                menuEditPaste.ShortcutKeys = Keys.Alt | Keys.V;
                menuEditSelectAll.ShortcutKeys = Keys.Alt | Keys.A;
                menuEditSelectAllEvents.ShortcutKeys = Keys.Alt | Keys.Shift | Keys.A;

                menuHiddenEditFlipToolPointerPencil.ShortcutKeys = Keys.Alt | Keys.W;
                menuHiddenEditFlipToolPointerEraser.ShortcutKeys = Keys.Alt | Keys.E;
                menuHiddenVisualForwardParameter.ShortcutKeys = Keys.Clear | Keys.Alt | Keys.Shift;
                menuHiddenVisualBackwardParameter.ShortcutKeys = Keys.LButton | Keys.LineFeed | Keys.Alt | Keys.Shift;
                menuHiddenTrackNext.ShortcutKeys = Keys.Clear | Keys.Alt;
                menuHiddenTrackBack.ShortcutKeys = Keys.LButton | Keys.LineFeed | Keys.Alt;
                #endregion

                #region cMenuPiano
                cMenuPianoUndo.ShortcutKeys = Keys.Alt | Keys.Z;
                cMenuPianoRedo.ShortcutKeys = Keys.Alt | Keys.Shift | Keys.Z;
                cMenuPianoCut.ShortcutKeys = Keys.Alt | Keys.X;
                cMenuPianoCopy.ShortcutKeys = Keys.Alt | Keys.C;
                cMenuPianoSelectAll.ShortcutKeys = Keys.Alt | Keys.A;
                cMenuPianoSelectAllEvents.ShortcutKeys = Keys.Alt | Keys.Shift | Keys.A;
                #endregion

                #region cMenuTrackSelector
                cMenuTrackSelectorUndo.ShortcutKeys = Keys.Alt | Keys.Z;
                cMenuTrackSelectorRedo.ShortcutKeys = Keys.Alt | Keys.Shift | Keys.Z;
                cMenuTrackSelectorCut.ShortcutKeys = Keys.Alt | Keys.X;
                cMenuTrackSelectorCopy.ShortcutKeys = Keys.Alt | Keys.C;
                cMenuTrackSelectorPaste.ShortcutKeys = Keys.Alt | Keys.V;
                cMenuTrackSelectorSelectAll.ShortcutKeys = Keys.Alt | Keys.Shift | Keys.A;
                #endregion
                /*} else {
                    #region menuStripMain
                    menuFileNew.ShortcutKeys = Keys.RButton | Keys.Clear | Keys.N;
                    menuFileOpen.ShortcutKeys = Keys.RButton | Keys.Return | Keys.O;
                    menuFileSave.ShortcutKeys = Keys.Pause | Keys.S;
                    menuFileQuit.ShortcutKeys = Keys.ControlKey | Keys.Q;

                    menuEditUndo.ShortcutKeys = Keys.RButton | Keys.FinalMode | Keys.Z;
                    menuEditRedo.ShortcutKeys = Keys.RButton | Keys.FinalMode | Keys.Shift | Keys.Z;
                    menuEditCut.ShortcutKeys = Keys.FinalMode | Keys.X;
                    menuEditCopy.ShortcutKeys = Keys.Cancel | Keys.C;
                    menuEditPaste.ShortcutKeys = Keys.RButton | Keys.CapsLock | Keys.V;
                    menuEditSelectAll.ShortcutKeys = Keys.LButton | Keys.A;
                    menuEditSelectAllEvents.ShortcutKeys = Keys.LButton | Keys.Shift | Keys.A;

                    menuHiddenEditPaste.ShortcutKeys = Keys.RButton | Keys.CapsLock | Keys.V;
                    menuHiddenEditFlipToolPointerPencil.ShortcutKeys = Keys.JunjaMode | Keys.W;
                    menuHiddenEditFlipToolPointerEraser.ShortcutKeys = Keys.XButton1 | Keys.E;
                    menuHiddenVisualForwardParameter.ShortcutKeys = Keys.Clear | Keys.Control | Keys.Shift;
                    menuHiddenVisualBackwardParameter.ShortcutKeys = Keys.LButton | Keys.LineFeed | Keys.Control | Keys.Shift;
                    menuHiddenTrackNext.ShortcutKeys = Keys.Clear | Keys.Control;
                    menuHiddenTrackBack.ShortcutKeys = Keys.LButton | Keys.LineFeed | Keys.Control;
                    #endregion

                    #region cMenuPiano
                    cMenuPianoUndo.ShortcutKeys = Keys.RButton | Keys.FinalMode | Keys.Z;
                    cMenuPianoRedo.ShortcutKeys = Keys.RButton | Keys.FinalMode | Keys.Shift | Keys.Z;
                    cMenuPianoCut.ShortcutKeys = Keys.FinalMode | Keys.X;
                    cMenuPianoCopy.ShortcutKeys = Keys.Cancel | Keys.C;
                    cMenuPianoSelectAll.ShortcutKeys = Keys.LButton | Keys.A;
                    cMenuPianoSelectAllEvents.ShortcutKeys = Keys.LButton | Keys.Shift | Keys.A;
                    #endregion

                    #region cMenuTrackSelector
                    cMenuTrackSelectorUndo.ShortcutKeys = Keys.RButton | Keys.FinalMode | Keys.Z;
                    cMenuTrackSelectorRedo.ShortcutKeys = Keys.RButton | Keys.FinalMode | Keys.Shift | Keys.Z;
                    cMenuTrackSelectorCut.ShortcutKeys = Keys.FinalMode | Keys.X;
                    cMenuTrackSelectorCopy.ShortcutKeys = Keys.Cancel | Keys.C;
                    cMenuTrackSelectorPaste.ShortcutKeys = Keys.RButton | Keys.CapsLock | Keys.V;
                    cMenuTrackSelectorSelectAll.ShortcutKeys = Keys.LButton | Keys.Shift | Keys.A;
                    #endregion
                }*/
                menuFileNew.ShortcutKeyDisplayString = _CO + "N";
                menuFileOpen.ShortcutKeyDisplayString = _CO + "O";
                menuFileSave.ShortcutKeyDisplayString = _CO + "S";
                menuFileQuit.ShortcutKeyDisplayString = _CO + "Q";

                menuEditUndo.ShortcutKeyDisplayString = _CO + "Z";
                menuEditRedo.ShortcutKeyDisplayString = _SHIFT + _CO + "Z";
                menuEditCut.ShortcutKeyDisplayString = _CO + "X";
                menuEditCopy.ShortcutKeyDisplayString = _CO + "C";
                menuEditPaste.ShortcutKeyDisplayString = _CO + "V";
                menuEditSelectAll.ShortcutKeyDisplayString = _CO + "A";
                menuEditSelectAllEvents.ShortcutKeyDisplayString = _SHIFT + _CO + "A";

                cMenuPianoUndo.ShortcutKeyDisplayString = _CO + "Z";
                cMenuPianoRedo.ShortcutKeyDisplayString = _SHIFT + _CO + "Z";
                cMenuPianoCut.ShortcutKeyDisplayString = _CO + "X";
                cMenuPianoCopy.ShortcutKeyDisplayString = _CO + "C";
                cMenuPianoSelectAll.ShortcutKeyDisplayString = _CO + "A";
                cMenuPianoSelectAllEvents.ShortcutKeyDisplayString = _SHIFT + _CO + "A";

                cMenuTrackSelectorUndo.ShortcutKeyDisplayString = _CO + "Z";
                cMenuTrackSelectorRedo.ShortcutKeyDisplayString = _SHIFT + _CO + "Z";
                cMenuTrackSelectorCut.ShortcutKeyDisplayString = _CO + "X";
                cMenuTrackSelectorCopy.ShortcutKeyDisplayString = _CO + "C";
                cMenuTrackSelectorPaste.ShortcutKeyDisplayString = _CO + "V";
                cMenuTrackSelectorSelectAll.ShortcutKeyDisplayString = _SHIFT + _CO + "A";
                #endregion
            } else {
                TreeMap<String, Keys[]> dict = AppManager.editorConfig.GetShortcutKeysDictionary();
                #region menuStripMain
                for ( Iterator itr = dict.keySet().iterator(); itr.hasNext(); ){
                    String key = (String)itr.next();
                    if ( key.Equals( "menuEditCopy" ) || key.Equals( "menuEditCut" ) || key.Equals( "menuEditPaste" ) ) {
                        continue;
                    }
                    ToolStripMenuItem menu = SearchMenuItemFromName( key );
                    if ( menu != null ) {
                        ApplyMenuItemShortcut( dict, menu, menu.Name );
                    }
                }
                if ( dict.containsKey( "menuEditCopy" ) ) {
                    ApplyMenuItemShortcut( dict, menuHiddenCopy, "menuEditCopy" );
                }
                if ( dict.containsKey( "menuEditCut" ) ) {
                    ApplyMenuItemShortcut( dict, menuHiddenCut, "menuEditCut" );
                }
                if ( dict.containsKey( "menuEditCopy" ) ) {
                    ApplyMenuItemShortcut( dict, menuHiddenPaste, "menuEditPaste" );
                }
                #endregion

                ValuePair<String, ToolStripMenuItem[]>[] work = new ValuePair<String, ToolStripMenuItem[]>[]{
                    new ValuePair<String, ToolStripMenuItem[]>( "menuEditUndo", new ToolStripMenuItem[]{ cMenuPianoUndo, cMenuTrackSelectorUndo } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuEditRedo", new ToolStripMenuItem[]{ cMenuPianoRedo, cMenuTrackSelectorRedo } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuEditCut", new ToolStripMenuItem[]{ cMenuPianoCut, cMenuTrackSelectorCut, menuEditCut } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuEditCopy", new ToolStripMenuItem[]{ cMenuPianoCopy, cMenuTrackSelectorCopy, menuEditCopy } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuEditPaste", new ToolStripMenuItem[]{ cMenuPianoPaste, cMenuTrackSelectorPaste, menuEditPaste } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuEditSelectAll", new ToolStripMenuItem[]{ cMenuPianoSelectAll, cMenuTrackSelectorSelectAll } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuEditSelectAllEvents", new ToolStripMenuItem[]{ cMenuPianoSelectAllEvents } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuEditDelete", new ToolStripMenuItem[]{ menuEditDelete } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuVisualGridline", new ToolStripMenuItem[]{ cMenuPianoGrid } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuJobLyric", new ToolStripMenuItem[]{ cMenuPianoImportLyric } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuLyricExpressionProperty", new ToolStripMenuItem[]{ cMenuPianoExpressionProperty } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuLyricVibratoProperty", new ToolStripMenuItem[]{ cMenuPianoVibratoProperty } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackOn", new ToolStripMenuItem[]{ cMenuTrackTabTrackOn } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackAdd", new ToolStripMenuItem[]{ cMenuTrackTabAdd } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackCopy", new ToolStripMenuItem[]{ cMenuTrackTabCopy } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackDelete", new ToolStripMenuItem[]{ cMenuTrackTabDelete } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackRenderCurrent", new ToolStripMenuItem[]{ cMenuTrackTabRenderCurrent } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackRenderAll", new ToolStripMenuItem[]{ cMenuTrackTabRenderAll } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackOverlay", new ToolStripMenuItem[]{ cMenuTrackTabOverlay } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackRendererVOCALOID1", new ToolStripMenuItem[]{ cMenuTrackTabRendererVOCALOID1 } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackRendererVOCALOID2", new ToolStripMenuItem[]{ cMenuTrackTabRendererVOCALOID2 } ),
                    new ValuePair<String, ToolStripMenuItem[]>( "menuTrackRendererUtau", new ToolStripMenuItem[]{ cMenuTrackTabRendererUtau } ),
                };
                foreach ( ValuePair<String, ToolStripMenuItem[]> item in work ) {
                    if ( dict.containsKey( item.Key ) ) {
                        Keys[] k = dict.get( item.Key );
                        String s = AppManager.getShortcutDisplayString( k );
                        if ( s != "" ) {
                            for ( int i = 0; i < item.Value.Length; i++ ) {
                                item.Value[i].ShortcutKeyDisplayString = s;
                            }
                        }
                    }
                }

                // スクリプトにショートカットを適用
#if DEBUG
                AppManager.debugWriteLine( "ApplyShortCut" );
#endif
                foreach ( ToolStripItem tsi in menuScript.DropDownItems ) {
                    if ( tsi is ToolStripMenuItem ) {
                        ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                        if ( tsmi.DropDownItems.Count == 1 && tsmi.DropDownItems[0] is ToolStripMenuItem ) {
                            ToolStripMenuItem dd_run = (ToolStripMenuItem)tsmi.DropDownItems[0];
#if DEBUG
                            AppManager.debugWriteLine( "    dd_run.name=" + dd_run.Name );
#endif
                            if ( dict.containsKey( dd_run.Name ) ) {
                                ToolStripMenuItem tsmi2 = (ToolStripMenuItem)tsmi;
                                ApplyMenuItemShortcut( dict, tsmi2, tsmi2.Name );
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// dictの中から
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="item"></param>
        /// <param name="item_name"></param>
        /// <param name="default_shortcut"></param>
        private void ApplyMenuItemShortcut( TreeMap<String, Keys[]> dict, ToolStripMenuItem item, String item_name ) {
            try {
                if ( dict.containsKey( item_name ) ) {
                    Keys k = Keys.None;
                    for ( int i = 0; i < dict.get( item_name ).Length; i++ ) {
                        k = k | dict.get( item_name )[i];
                    }
                    item.ShortcutKeys = k;
                } else {
                    item.ShortcutKeys = Keys.None;
                }
            } catch( Exception ex ) {
            }
        }

        /// <summary>
        /// ソングポジションを1小節進めます
        /// </summary>
        private void Forward() {
            boolean playing = AppManager.isPlaying();
            if ( playing ) {
                return;
            }
            int current = AppManager.getVsqFile().getBarCountFromClock( AppManager.getCurrentClock() ) + 1;
            int new_clock = AppManager.getVsqFile().getClockFromBarCount( current );
            if ( new_clock <= hScroll.Maximum + (pictPianoRoll.Width - AppManager.KEY_LENGTH) / AppManager.scaleX ) {
                AppManager.setCurrentClock( new_clock );
                ensureCursorVisible();
                AppManager.setPlaying( playing );
                refreshScreen();
            }
        }

        /// <summary>
        /// ソングポジションを1小節戻します
        /// </summary>
        private void Rewind() {
            boolean playing = AppManager.isPlaying();
            if ( playing ) {
                return;
            }
            int current = AppManager.getVsqFile().getBarCountFromClock( AppManager.getCurrentClock() );
            if ( current > 0 ) {
                current--;
            }
            int new_clock = AppManager.getVsqFile().getClockFromBarCount( current );
            AppManager.setCurrentClock( new_clock );
            ensureCursorVisible();
            AppManager.setPlaying( playing );
            refreshScreen();
        }

        /// <summary>
        /// cMenuPianoの固定長音符入力の各メニューのチェック状態をm_pencil_modeを元に更新します
        /// </summary>
        private void UpdateCMenuPianoFixed() {
            cMenuPianoFixed01.CheckState = CheckState.Unchecked;
            cMenuPianoFixed02.CheckState = CheckState.Unchecked;
            cMenuPianoFixed04.CheckState = CheckState.Unchecked;
            cMenuPianoFixed08.CheckState = CheckState.Unchecked;
            cMenuPianoFixed16.CheckState = CheckState.Unchecked;
            cMenuPianoFixed32.CheckState = CheckState.Unchecked;
            cMenuPianoFixed64.CheckState = CheckState.Unchecked;
            cMenuPianoFixed128.CheckState = CheckState.Unchecked;
            cMenuPianoFixedOff.CheckState = CheckState.Unchecked;
            cMenuPianoFixedTriplet.CheckState = CheckState.Unchecked;
            cMenuPianoFixedDotted.CheckState = CheckState.Unchecked;
            switch ( m_pencil_mode.Mode ) {
                case PencilModeEnum.L1:
                    cMenuPianoFixed01.CheckState = CheckState.Checked;
                    break;
                case PencilModeEnum.L2:
                    cMenuPianoFixed02.CheckState = CheckState.Checked;
                    break;
                case PencilModeEnum.L4:
                    cMenuPianoFixed04.CheckState = CheckState.Checked;
                    break;
                case PencilModeEnum.L8:
                    cMenuPianoFixed08.CheckState = CheckState.Checked;
                    break;
                case PencilModeEnum.L16:
                    cMenuPianoFixed16.CheckState = CheckState.Checked;
                    break;
                case PencilModeEnum.L32:
                    cMenuPianoFixed32.CheckState = CheckState.Checked;
                    break;
                case PencilModeEnum.L64:
                    cMenuPianoFixed64.CheckState = CheckState.Checked;
                    break;
                case PencilModeEnum.L128:
                    cMenuPianoFixed128.CheckState = CheckState.Checked;
                    break;
                case PencilModeEnum.Off:
                    cMenuPianoFixedOff.CheckState = CheckState.Checked;
                    break;
            }
            cMenuPianoFixedTriplet.CheckState = (m_pencil_mode.Triplet) ? CheckState.Checked : CheckState.Unchecked;
            cMenuPianoFixedDotted.CheckState = (m_pencil_mode.Dot) ? CheckState.Checked : CheckState.Unchecked;
        }

        private void ClearTempWave() {
            String tmppath = AppManager.getTempWaveDir();

            for ( int i = 1; i <= 16; i++ ) {
                String file = Path.Combine( tmppath, i + ".wav" );
                if ( File.Exists( file ) ) {
                    for ( int error = 0; error < 100; error++ ) {
                        try {
                            File.Delete( file );
                            break;
                        } catch ( Exception ex ) {
#if DEBUG
                            bocoree.debug.push_log( "FormMain+ClearTempWave()" );
                            bocoree.debug.push_log( "    ex=" + ex.ToString() );
                            bocoree.debug.push_log( "    error_count=" + error );
#endif
                            System.Threading.Thread.Sleep( 100 );
                        }
                    }
                }
            }
            String whd = Path.Combine( tmppath, UtauRenderingRunner.FILEBASE + ".whd" );
            if ( File.Exists( whd ) ) {
                try {
                    File.Delete( whd );
                } catch {
                }
            }
            String dat = Path.Combine( tmppath, UtauRenderingRunner.FILEBASE + ".dat" );
            if ( File.Exists( dat ) ) {
                try {
                    File.Delete( dat );
                } catch {
                }
            }
        }

        private void Render( int[] tracks ) {
            String tmppath = AppManager.getTempWaveDir();
            if ( !Directory.Exists( tmppath ) ) {
                Directory.CreateDirectory( tmppath );
            }
            String[] files = new String[tracks.Length];
            for ( int i = 0; i < tracks.Length; i++ ) {
                files[i] = Path.Combine( tmppath, tracks[i] + ".wav" );
            }
            using ( FormSynthesize dlg = new FormSynthesize( AppManager.getVsqFile(), AppManager.editorConfig.PreSendTime, tracks, files, AppManager.getVsqFile().TotalClocks + 240, false ) ) {
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    AppManager.getVsqFile().Track.get( AppManager.getSelected() ).resetEditedArea();
                }
                int[] finished = dlg.Finished;
                for ( int i = 0; i < finished.Length; i++ ) {
                    AppManager.setRenderRequired( finished[i], false );
                }
            }
        }

        private void PlayPreviewSound( int note ) {
            KeySoundPlayer.Play( note );
        }

        private void MouseHoverEventGenerator( object arg ) {
            int note = (int)arg;
            if ( AppManager.editorConfig.MouseHoverTime > 0 ) {
                Thread.Sleep( AppManager.editorConfig.MouseHoverTime );
            }
            KeySoundPlayer.Play( note );
        }

        public static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        public void applyLanguage() {
            try {
                openXmlVsqDialog.Filter = _( "XML-VSQ Format(*.xvsq)|*.xvsq" ) + "|" + _( "All Files(*.*)|*.*" );
            } catch {
                openXmlVsqDialog.Filter = "XML-VSQ Format(*.xvsq)|*.xvsq|All Files(*.*)|*.*";
            }
            try {
                saveXmlVsqDialog.Filter = _( "XML-VSQ Format(*.xvsq)|*.xvsq" ) + "|" + _( "All Files(*.*)|*.*" );
            } catch {
                saveXmlVsqDialog.Filter = "XML-VSQ Format(*.xvsq)|*.xvsq|All Files(*.*)|*.*";
            }
            try {
                openUstDialog.Filter = _( "UTAU Script Format(*.ust)|*.ust" ) + "|" + _( "All Files(*.*)|*.*" );
            } catch {
                openUstDialog.Filter = "UTAU Script Format(*.ust)|*.ust|All Files(*.*)|*.*";
            }
            try {
                openMidiDialog.Filter = _( "MIDI Format(*.mid)|*.mid" ) + "|" + _( "VSQ Format(*.vsq)|*.vsq" ) + "|" + _( "All Files(*.*)|*.*" );
            } catch {
                openMidiDialog.Filter = "MIDI Format(*.mid)|*.mid|VSQ Format(*.vsq)|*.vsq|All Files(*.*)|*.*";
            }
            try {
                saveMidiDialog.Filter = _( "MIDI Format(*.mid)|*.mid" ) + "|" + _( "VSQ Format(*.vsq)|*.vsq" ) + "|" + _( "All Files(*.*)|*.*" );
            } catch {
                saveMidiDialog.Filter = "MIDI Format(*.mid)|*.mid|VSQ Format(*.vsq)|*.vsq|All Files(*.*)|*.*";
            }
            try {
                openWaveDialog.Filter = _( "Wave File(*.wav)|*.wav" ) + "|" + _( "All Files(*.*)|*.*" );
            } catch {
                openWaveDialog.Filter = "Wave File(*.wav)|*.wav|All Files(*.*)|*.*";
            }

            stripLblGameCtrlMode.ToolTipText = _( "Game Controler" );
            this.Invoke( new VoidDelegate( UpdateGameControlerStatus ) );

            stripBtnPointer.Text = _( "Pointer" );
            stripBtnPointer.ToolTipText = _( "Pointer" );
            stripBtnPencil.Text = _( "Pencil" );
            stripBtnPencil.ToolTipText = _( "Pencil" );
            stripBtnLine.Text = _( "Line" );
            stripBtnLine.ToolTipText = _( "Line" );
            stripBtnEraser.Text = _( "Eraser" );
            stripBtnEraser.ToolTipText = _( "Eraser" );
            stripBtnCurve.Text = _( "Curve" );
            stripBtnCurve.ToolTipText = _( "Curve" );
            stripBtnGrid.Text = _( "Grid" );
            stripBtnGrid.ToolTipText = _( "Grid" );

            #region main menu
            menuFile.Text = _( "File" ) + "(&F)";
            menuFileNew.Text = _( "New" ) + "(&N)";
            menuFileOpen.Text = _( "Open" ) + "(&O)";
            menuFileOpenVsq.Text = _( "Open VSQ/Vocaloid Midi" ) + "(&V)";
            menuFileOpenUst.Text = _( "Open UTAU Project File" ) + "(&U)";
            menuFileSave.Text = _( "Save" ) + "(&S)";
            menuFileSaveNamed.Text = _( "Save As" ) + "(&A)";
            menuFileImport.Text = _( "Import" ) + "(&I)";
            menuFileImportVsq.Text = _( "VSQ / Vocaloid Midi" );
            menuFileExport.Text = _( "Export" ) + "(&E)";
            menuFileRecent.Text = _( "Recent Files" ) + "(&R)";
            menuFileQuit.Text = _( "Quit" ) + "(&Q)";

            menuEdit.Text = _( "Edit" ) + "(&E)";
            menuEditUndo.Text = _( "Undo" ) + "(&U)";
            menuEditRedo.Text = _( "Redo" ) + "(&R)";
            menuEditCut.Text = _( "Cut" ) + "(&T)";
            menuEditCopy.Text = _( "Copy" ) + "(&C)";
            menuEditPaste.Text = _( "Paste" ) + "(&P)";
            menuEditDelete.Text = _( "Delete" ) + "(&D)";
            menuEditAutoNormalizeMode.Text = _( "Auto Normalize Mode" ) + "(&N)";
            menuEditSelectAll.Text = _( "Select All" ) + "(&A)";
            menuEditSelectAllEvents.Text = _( "Select All Events" ) + "(&E)";

            menuVisual.Text = _( "View" ) + "(&V)";
            menuVisualControlTrack.Text = _( "Control Track" ) + "(&C)";
            menuVisualMixer.Text = _( "Mixer" ) + "(&X)";
            menuVisualWaveform.Text = _( "Waveform" ) + "(&W)";
            menuVisualProperty.Text = _( "Property Window" );
            menuVisualOverview.Text = _( "Navigation" ) + "(&V)";
            menuVisualGridline.Text = _( "Grid Line" ) + "(&G)";
            menuVisualStartMarker.Text = _( "Start Marker" ) + "(&S)";
            menuVisualEndMarker.Text = _( "End Marker" ) + "(&E)";
            menuVisualLyrics.Text = _( "Lyrics/Phoneme" ) + "(&L)";
            menuVisualNoteProperty.Text = _( "Note Expression/Vibrato" ) + "(&N)";
            menuVisualPitchLine.Text = _( "Pitch Line" ) + "(&P)";

            menuJob.Text = _( "Job" ) + "(&J)";
            menuJobNormalize.Text = _( "Normalize Notes" ) + "(&N)";
            menuJobInsertBar.Text = _( "Insert Bars" ) + "(&I)";
            menuJobDeleteBar.Text = _( "Delete Bars" ) + "(&D)";
            menuJobRandomize.Text = _( "Randomize" ) + "(&R)";
            menuJobConnect.Text = _( "Connect Notes" ) + "(&C)";
            menuJobLyric.Text = _( "Insert Lyrics" ) + "(&L)";
            menuJobRewire.Text = _( "Import ReWire Host Tempo" ) + "(&T)";
            menuJobRealTime.Text = _( "Start Realtime Input" );
            menuJobReloadVsti.Text = _( "Reload VSTi" ) + "(&R)";

            menuTrack.Text = _( "Track" ) + "(&T)";
            menuTrackOn.Text = _( "Track On" ) + "(&K)";
            menuTrackAdd.Text = _( "Add Track" ) + "(&A)";
            menuTrackCopy.Text = _( "Copy Track" ) + "(&C)";
            menuTrackChangeName.Text = _( "Rename Track" ) + "(&R)";
            menuTrackDelete.Text = _( "Delete Track" ) + "(&D)";
            menuTrackRenderCurrent.Text = _( "Render Current Track" ) + "(&T)";
            menuTrackRenderAll.Text = _( "Render All Tracks" ) + "(&S)";
            menuTrackOverlay.Text = _( "Overlay" ) + "(&O)";
            menuTrackRenderer.Text = _( "Renderer" );
            
            menuLyric.Text = _( "Lyrics" ) + "(&L)";
            menuLyricExpressionProperty.Text = _( "Note Expression Property" ) + "(&E)";
            menuLyricVibratoProperty.Text = _( "Note Vibrato Property" ) + "(&V)";
            menuLyricSymbol.Text = _( "Phoneme Transformation" ) + "(&T)";
            menuLyricDictionary.Text = _( "User Word Dictionary" ) + "(&C)";

            menuScript.Text = _( "Script" ) + "(&C)";
            menuScriptUpdate.Text = _( "Update Script List" ) + "(&U)";

            menuSetting.Text = _( "Setting" ) + "(&S)";
            menuSettingPreference.Text = _( "Preference" ) + "(&P)";
            menuSettingGameControler.Text = _( "Game Controler" ) + "(&G)";
            menuSettingGameControlerLoad.Text = _( "Load" ) + "(&L)";
            menuSettingGameControlerRemove.Text = _( "Remove" ) + "(&R)";
            menuSettingGameControlerSetting.Text = _( "Setting" ) + "(&S)";
            menuSettingShortcut.Text = _( "Shortcut Key" ) + "(&S)";
            menuSettingUtauVoiceDB.Text = _( "UTAU Voice DB" ) + "(&U)";
            menuSettingDefaultSingerStyle.Text = _( "Singing Style Defaults" ) + "(&D)";
            menuSettingPositionQuantize.Text = _( "Quantize" ) + "(&Q)";
            menuSettingPositionQuantizeOff.Text = _( "Off" );
            menuSettingPositionQuantizeTriplet.Text = _( "Triplet" );
            menuSettingLengthQuantize.Text = _( "Length" ) + "(&L)";
            menuSettingLengthQuantizeOff.Text = _( "Off" );
            menuSettingLengthQuantizeTriplet.Text = _( "Triplet" );
            menuSettingSingerProperty.Text = _( "Singer Properties" ) + "(&S)";
            menuSettingPaletteTool.Text = _( "Palette Tool" ) + "(&T)";

            menuHelp.Text = _( "Help" ) + "(&H)";
            menuHelpAbout.Text = _( "About Cadencii" ) + "(&A)";

            menuHiddenEditLyric.Text = _( "Start Lyric Input" );
            menuHiddenEditFlipToolPointerEraser.Text = _( "Chagne Tool Pointer / Eraser" );
            menuHiddenEditFlipToolPointerPencil.Text = _( "Change Tool Pointer / Pencil" );
            menuHiddenTrackBack.Text = _( "Previous Track" );
            menuHiddenTrackNext.Text = _( "Next Track" );
            menuHiddenVisualBackwardParameter.Text = _( "Previous Control Curve" );
            menuHiddenVisualForwardParameter.Text = _( "Next Control Curve" );
            #endregion

            #region cMenuPiano
            cMenuPianoPointer.Text = _( "Arrow" ) + "(&A)";
            cMenuPianoPencil.Text = _( "Pencil" ) + "(&W)";
            cMenuPianoEraser.Text = _( "Eraser" ) + "(&E)";
            cMenuPianoPaletteTool.Text = _( "Palette Tool" );

            cMenuPianoCurve.Text = _( "Curve" ) + "(&V)";

            cMenuPianoFixed.Text = _( "Note Fixed Length" ) + "(&N)";
            cMenuPianoFixedTriplet.Text = _( "Triplet" );
            cMenuPianoFixedOff.Text = _( "Off" );
            cMenuPianoFixedDotted.Text = _( "Dot" );
            cMenuPianoQuantize.Text = _( "Quantize" ) + "(&Q)";
            cMenuPianoQuantizeTriplet.Text = _( "Triplet" );
            cMenuPianoQuantizeOff.Text = _( "Off" );
            cMenuPianoLength.Text = _( "Length" ) + "(&L)";
            cMenuPianoLengthTriplet.Text = _( "Triplet" );
            cMenuPianoLengthOff.Text = _( "Off" );
            cMenuPianoGrid.Text = _( "Show/Hide Grid Line" ) + "(&S)";

            cMenuPianoUndo.Text = _( "Undo" ) + "(&U)";
            cMenuPianoRedo.Text = _( "Redo" ) + "(&R)";

            cMenuPianoCut.Text = _( "Cut" ) + "(&T)";
            cMenuPianoPaste.Text = _( "Paste" ) + "(&P)";
            cMenuPianoCopy.Text = _( "Copy" ) + "(&C)";
            cMenuPianoDelete.Text = _( "Delete" ) + "(&D)";

            cMenuPianoSelectAll.Text = _( "Select All" ) + "(&A)";
            cMenuPianoSelectAllEvents.Text = _( "Select All Events" ) + "(&E)";

            cMenuPianoExpressionProperty.Text = _( "Note Expression Property" ) + "(&P)";
            cMenuPianoVibratoProperty.Text = _( "Note Vibrato Property" );
            cMenuPianoImportLyric.Text = _( "Insert Lyrics" ) + "(&P)";
            #endregion

            #region cMenuTrackTab
            cMenuTrackTabTrackOn.Text = _( "Track On" ) + "(&K)";
            cMenuTrackTabAdd.Text = _( "Add Track" ) + "(&A)";
            cMenuTrackTabCopy.Text = _( "Copy Track" ) + "(&C)";
            cMenuTrackTabChangeName.Text = _( "Rename Track" ) + "(&R)";
            cMenuTrackTabDelete.Text = _( "Delete Track" ) + "(&D)";

            cMenuTrackTabRenderCurrent.Text = _( "Render Current Track" ) + "(&T)";
            cMenuTrackTabRenderAll.Text = _( "Render All Tracks" ) + "(&S)";
            cMenuTrackTabOverlay.Text = _( "Overlay" ) + "(&O)";
            cMenuTrackTabRenderer.Text = _( "Renderer" );
            #endregion

            #region cMenuTrackSelector
            cMenuTrackSelectorPointer.Text = _( "Arrow" ) + "(&A)";
            cMenuTrackSelectorPencil.Text = _( "Pencil" ) + "(&W)";
            cMenuTrackSelectorLine.Text = _( "Line" ) + "(&L)";
            cMenuTrackSelectorEraser.Text = _( "Eraser" ) + "(&E)";
            cMenuTrackSelectorPaletteTool.Text = _( "Palette Tool" );

            cMenuTrackSelectorCurve.Text = _( "Curve" ) + "(&V)";

            cMenuTrackSelectorUndo.Text = _( "Undo" ) + "(&U)";
            cMenuTrackSelectorRedo.Text = _( "Redo" ) + "(&R)";

            cMenuTrackSelectorCut.Text = _( "Cut" ) + "(&T)";
            cMenuTrackSelectorCopy.Text = _( "Copy" ) + "(&C)";
            cMenuTrackSelectorPaste.Text = _( "Paste" ) + "(&P)";
            cMenuTrackSelectorDelete.Text = _( "Delete" ) + "(&D)";
            cMenuTrackSelectorDeleteBezier.Text = _( "Delete Bezier Point" ) + "(&B)";

            cMenuTrackSelectorSelectAll.Text = _( "Select All Events" ) + "(&E)";
            #endregion

            stripLblGameCtrlMode.ToolTipText = _( "Game Controler" );

            // Palette Tool
#if DEBUG
            AppManager.debugWriteLine( "FormMain.ApplyLanguage; Messaging.Language=" + Messaging.Language );
#endif
            foreach ( ToolStripItem tsi in toolStripTool.Items ) {
                if ( tsi is ToolStripButton ) {
                    ToolStripButton tsb = (ToolStripButton)tsi;
                    if ( tsb.Tag != null && tsb.Tag is String ) {
                        String id = (String)tsb.Tag;
                        if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                            tsb.Text = ipt.getName( Messaging.Language );
                            tsb.ToolTipText = ipt.getDescription( Messaging.Language );
                        }
                    }
                }
            }

            foreach ( ToolStripItem tsi in cMenuPianoPaletteTool.DropDownItems ) {
                if ( tsi is ToolStripMenuItem ) {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                    if ( tsmi.Tag != null && tsmi.Tag is String ) {
                        String id = (String)tsmi.Tag;
                        if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                            tsmi.Text = ipt.getName( Messaging.Language );
                            tsmi.ToolTipText = ipt.getDescription( Messaging.Language );
                        }
                    }
                }
            }

            foreach ( ToolStripItem tsi in cMenuTrackSelectorPaletteTool.DropDownItems ) {
                if ( tsi is ToolStripMenuItem ) {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                    if ( tsmi.Tag != null && tsmi.Tag is String ) {
                        String id = (String)tsmi.Tag;
                        if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                            tsmi.Text = ipt.getName( Messaging.Language );
                            tsmi.ToolTipText = ipt.getDescription( Messaging.Language );
                        }
                    }
                }
            }

            foreach ( ToolStripItem tsi in menuSettingPaletteTool.DropDownItems ) {
                if ( tsi is ToolStripMenuItem ) {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                    if ( tsmi.Tag != null && tsmi.Tag is String ) {
                        String id = (String)tsmi.Tag;
                        if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                            tsmi.Text = ipt.getName( Messaging.Language );
                        }
                    }
                }
            }

            for ( Iterator itr = PaletteToolServer.LoadedTools.keySet().iterator(); itr.hasNext(); ){
                String id = (String)itr.next();
                IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                ipt.applyLanguage( Messaging.Language );
            }

            UpdateStripDDBtnSpeed();
        }

        private void ImportLyric() {
#if DEBUG
            AppManager.debugWriteLine( "ImportLyric" );
#endif
            int start = 0;
            int selectedid = AppManager.getLastSelectedEvent().original.InternalID;
            for ( int i = 0; i < AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventCount(); i++ ) {
                if ( selectedid == AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEvent( i ).InternalID ) {
                    start = i;
                    break;
                }
            }
            int count = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventCount() - 1 - start + 1;
#if DEBUG
            AppManager.debugWriteLine( "    count=" + count );
#endif
            using ( FormImportLyric dlg = new FormImportLyric( count ) ) {
                dlg.Location = GetFormPreferedLocation( dlg );
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    String[] phrases = dlg.GetLetters();
#if DEBUG
                    for ( int i = 0; i < phrases.Length; i++ ) {
                        AppManager.debugWriteLine( "    " + phrases[i] );
                    }
#endif
                    int min = Math.Min( count, phrases.Length );
                    String[] new_phrases = new String[min];
                    String[] new_symbols = new String[min];
                    for ( int i = 0; i < min; i++ ) {
                        new_phrases[i] = phrases[i];
                        SymbolTable.attatch( phrases[i], out new_symbols[i] );
                    }
                    VsqID[] new_ids = new VsqID[min];
                    int[] ids = new int[min];
                    for ( int i = start; i < start + min; i++ ) {
                        new_ids[i - start] = (VsqID)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEvent( i ).ID.clone();
                        new_ids[i - start].LyricHandle.L0.Phrase = new_phrases[i - start];
                        new_ids[i - start].LyricHandle.L0.setPhoneticSymbol( new_symbols[i - start] );
                        ids[i - start] = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEvent( i ).InternalID;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaintsRange( AppManager.getSelected(), ids, new_ids ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                    this.Refresh();
                }
            }
        }

        private void NoteVibratoProperty() {
            SelectedEventEntry item = AppManager.getLastSelectedEvent();
            if ( item == null ) {
                return;
            }

            VsqEvent ev = item.original;
            SynthesizerType type = SynthesizerType.VOCALOID2;
            if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                type = SynthesizerType.VOCALOID1;
            }
            using ( FormVibratoConfig dlg = new FormVibratoConfig( ev.ID.VibratoHandle, ev.ID.Length, AppManager.editorConfig.DefaultVibratoLength, type ) ) {
                dlg.Location = GetFormPreferedLocation( dlg );
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    VsqEvent edited = (VsqEvent)ev.clone();
                    if ( dlg.VibratoHandle != null ) {
                        edited.ID.VibratoHandle = (VibratoHandle)dlg.VibratoHandle.clone();
                        edited.ID.VibratoDelay = ev.ID.Length - dlg.VibratoHandle.Length;
                    } else {
                        edited.ID.VibratoHandle = null;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), ev.InternalID, (VsqID)edited.ID.clone() ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                    refreshScreen();
                }
            }
        }

        private void NoteExpressionProperty() {
            SelectedEventEntry item = AppManager.getLastSelectedEvent();
            if ( item == null ) {
                return;
            }

            VsqEvent ev = item.original;
            SynthesizerType type = SynthesizerType.VOCALOID2;
            if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                type = SynthesizerType.VOCALOID1;
            }
            using ( FormNoteExpressionConfig dlg = new FormNoteExpressionConfig( type, ev.ID.NoteHeadHandle ) ) {
                dlg.PMBendDepth = ev.ID.PMBendDepth;
                dlg.PMBendLength = ev.ID.PMBendLength;
                dlg.PMbPortamentoUse = ev.ID.PMbPortamentoUse;
                dlg.DEMdecGainRate = ev.ID.DEMdecGainRate;
                dlg.DEMaccent = ev.ID.DEMaccent;

                dlg.Location = GetFormPreferedLocation( dlg );

                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    VsqEvent edited = (VsqEvent)ev.clone();
                    edited.ID.PMBendDepth = dlg.PMBendDepth;
                    edited.ID.PMBendLength = dlg.PMBendLength;
                    edited.ID.PMbPortamentoUse = dlg.PMbPortamentoUse;
                    edited.ID.DEMdecGainRate = dlg.DEMdecGainRate;
                    edited.ID.DEMaccent = dlg.DEMaccent;
                    edited.ID.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), ev.InternalID, (VsqID)edited.ID.clone() ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                    refreshScreen();
                }
            }
        }

        private int NewHScrollValueFromWheelDelta( int delta ) {
            double new_val = (double)hScroll.Value - delta * AppManager.editorConfig.WheelOrder / (5.0 * AppManager.scaleX);
            if ( new_val < 0.0 ) {
                new_val = 0;
            }
            int draft = (int)new_val;
            if ( draft > hScroll.Maximum ) {
                draft = hScroll.Maximum;
            } else if ( draft < hScroll.Minimum ) {
                draft = hScroll.Minimum;
            }
            return draft;
        }

        #region 音符の編集関連
        public void selectAll() {
            AppManager.clearSelectedEvent();
            AppManager.clearSelectedTempo();
            AppManager.clearSelectedTimesig();
            AppManager.clearSelectedPoint();
            int min = int.MaxValue;
            int max = int.MinValue;
            int premeasure = AppManager.getVsqFile().getPreMeasureClocks();
            Vector<Integer> add_required = new Vector<Integer>();
            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                if ( premeasure <= ve.Clock ) {
                    add_required.add( ve.InternalID );
                    min = Math.Min( min, ve.Clock );
                    max = Math.Max( max, ve.Clock + ve.ID.Length );
                }
            }
            if ( add_required.size() > 0 ) {
                AppManager.addSelectedEventRange( add_required.toArray( new Integer[]{} ) );
            }
            foreach ( CurveType vct in AppManager.CURVE_USAGE ) {
                if ( vct.IsScalar || vct.IsAttachNote ) {
                    continue;
                }
                VsqBPList target = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( vct.Name );
                int count = target.size();
                if ( count >= 1 ) {
                    int[] keys = target.getKeys();
                    int max_key = keys[count - 1];
                    max = Math.Max( max, target.getValue( max_key ) );
                    for ( int i = 0; i < count; i++ ) {
                        int key = keys[i];
                        if ( premeasure <= key ) {
                            min = Math.Min( min, key );
                            break;
                        }
                    }
                }
            }
            if ( min < premeasure ) {
                min = premeasure;
            }
            if ( min < max ) {
                int stdx = AppManager.startToDrawX;
                min = xCoordFromClocks( min ) + stdx;
                max = xCoordFromClocks( max ) + stdx;
                AppManager.selectedRegion = new SelectedRegion( min );
                AppManager.selectedRegion.SetEnd( max );
                AppManager.selectedRegionEnabled = true;
            }
        }

        public void selectAllEvent() {
            AppManager.clearSelectedTempo();
            AppManager.clearSelectedTimesig();
            AppManager.clearSelectedEvent();
            AppManager.clearSelectedPoint();
            int premeasureclock = AppManager.getVsqFile().getPreMeasureClocks();
            Vector<Integer> add_required = new Vector<Integer>();
            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = (VsqEvent)itr.next();
                if ( ev.ID.type == VsqIDType.Anote && ev.Clock >= premeasureclock ) {
                    add_required.add( ev.InternalID );
                }
            }
            if ( add_required.size() > 0 ) {
                AppManager.addSelectedEventRange( add_required.toArray( new Integer[]{} ) );
            }
            refreshScreen();
        }

        private void DeleteEvent() {
#if DEBUG
            AppManager.debugWriteLine( "DeleteEvent()" );
            AppManager.debugWriteLine( "    m_input_textbox.Enabled=" + m_input_textbox.Enabled );
#endif
            if ( !m_input_textbox.Enabled ) {
                if ( AppManager.getSelectedEventCount() > 0 ) {
                    Vector<Integer> ids = new Vector<Integer>();
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                        SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                        ids.add( ev.original.InternalID );
                    }
                    VsqCommand run = VsqCommand.generateCommandEventDeleteRange( AppManager.getSelected(), ids.toArray( new Integer[]{} ) );
                    if ( AppManager.selectedRegionEnabled ) {
                        VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().Clone();
                        work.executeCommand( run );
                        int stdx = AppManager.startToDrawX;
                        int start_clock = clockFromXCoord( AppManager.selectedRegion.Start - stdx );
                        int end_clock = clockFromXCoord( AppManager.selectedRegion.End - stdx );
                        Vector<Vector<BPPair>> curves = new Vector<Vector<BPPair>>();
                        Vector<CurveType> types = new Vector<CurveType>();
                        foreach ( CurveType vct in AppManager.CURVE_USAGE ) {
                            if ( vct.IsScalar || vct.IsAttachNote ) {
                                continue;
                            }
                            Vector<BPPair> t = new Vector<BPPair>();
                            t.add( new BPPair( start_clock, work.Track.get( AppManager.getSelected() ).getCurve( vct.getName() ).getValue( start_clock ) ) );
                            t.add( new BPPair( end_clock, work.Track.get( AppManager.getSelected() ).getCurve( vct.getName() ).getValue( end_clock ) ) );
                            curves.add( t );
                            types.add( vct );
                        }
                        String[] strs = new String[types.size()];
                        for ( int i = 0; i < types.size(); i++ ) {
                            strs[i] = types.get( i ).Name;
                        }
                        CadenciiCommand delete_curve = new CadenciiCommand( VsqCommand.generateCommandTrackCurveEditRange( AppManager.getSelected(),
                                                                                                                           strs,
                                                                                                                           curves.toArray( new Vector<BPPair>[]{} ) ) );
                        work.executeCommand( delete_curve );
                        CadenciiCommand run2 = new CadenciiCommand( VsqCommand.generateCommandReplace( work ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run2 ) );
                        Edited = true;
                    } else {
                        CadenciiCommand run2 = new CadenciiCommand( run );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run2 ) );
                        Edited = true;
                        AppManager.clearSelectedEvent();
                    }
                    this.Refresh();
                } else if ( AppManager.getSelectedTempoCount() > 0 ) {
                    Vector<Integer> clocks = new Vector<Integer>();
                    for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ){
                        ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                        //SelectedTempoEntry value = AppManager.getSelectedTempo().get( key );
                        if ( item.Key <= 0 ) {
                            statusLabel.Text = _( "Cannot remove first symbol of track!" );
                            SystemSounds.Asterisk.Play();
                            return;
                        }
                        clocks.add( item.Key );
                    }
                    int[] dum = new int[clocks.size()];
                    for ( int i = 0; i < dum.Length; i++ ) {
                        dum[i] = -1;
                    }
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTempoRange( clocks.toArray( new Integer[]{} ), clocks.toArray( new Integer[]{} ), dum ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                    AppManager.clearSelectedTempo();
                    this.Refresh();
                } else if ( AppManager.getSelectedTimesigCount() > 0 ) {
#if DEBUG
                    AppManager.debugWriteLine( "    Timesig" );
#endif
                    int[] barcounts = new int[AppManager.getSelectedTimesigCount()];
                    int[] numerators = new int[AppManager.getSelectedTimesigCount()];
                    int[] denominators = new int[AppManager.getSelectedTimesigCount()];
                    int count = -1;
                    for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ){
                        ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                        int key = item.Key;
                        SelectedTimesigEntry value = item.Value;
                        count++;
                        barcounts[count] = key;
                        if ( key <= 0 ) {
                            statusLabel.Text = _( "Cannot remove first symbol of track!" );
                            SystemSounds.Asterisk.Play();
                            return;
                        }
                        numerators[count] = -1;
                        denominators[count] = -1;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandUpdateTimesigRange( barcounts, barcounts, numerators, denominators ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                    AppManager.clearSelectedTimesig();
                    this.Refresh();
                }
                if ( AppManager.getSelectedPointIDCount() > 0 ) {
#if DEBUG
                    AppManager.debugWriteLine( "    Curve" );
#endif
                    String curve;
                    if ( !trackSelector.SelectedCurve.IsAttachNote ) {
                        curve = trackSelector.SelectedCurve.getName();
                        VsqBPList list = (VsqBPList)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( curve ).clone();
                        Vector<Integer> remove_clock_queue = new Vector<Integer>();
                        int count = list.size();
                        for ( int i = 0; i < count; i++ ) {
                            VsqBPPair item = list.getElementB( i );
                            if ( AppManager.isSelectedPointContains( item.id ) ) {
                                remove_clock_queue.add( list.getKeyClock( i ) );
                            }
                        }
                        count = remove_clock_queue.size();
                        for ( int i = 0; i < count; i++ ) {
                            list.remove( remove_clock_queue.get( i ) );
                        }
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandTrackCurveReplace( AppManager.getSelected(),
                                                                                                                trackSelector.SelectedCurve.getName(),
                                                                                                                list ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                        Edited = true;
                    } else {
                        //todo: FormMain+DeleteEvent; VibratoDepth, VibratoRateの場合
                    }
                    AppManager.clearSelectedPoint();
                    refreshScreen();
                }
            }
        }

        private void pasteEvent() {
            int clock = AppManager.getCurrentClock();
            int unit = AppManager.getPositionQuantizeClock();
            int odd = clock % unit;
            clock -= odd;
            if ( odd > unit / 2 ) {
                clock += unit;
            }

            VsqCommand add_event = null; // VsqEventを追加するコマンド

            ClipboardEntry ce = AppManager.getCopiedItems();
            int copy_started_clock = ce.copyStartedClock;
            Vector<VsqEvent> copied_events = ce.events;
#if DEBUG
            Console.WriteLine( "FormMain#pasteEvent; copy_started_clock=" + copy_started_clock );
            Console.WriteLine( "FormMain#pasteEvent; copied_events.size()=" + copied_events.size() );
#endif
            if ( copied_events.size() != 0 ) {
                // VsqEventのペーストを行うコマンドを発行
                int dclock = clock - copy_started_clock;
                if ( clock >= AppManager.getVsqFile().getPreMeasureClocks() ) {
                    Vector<VsqEvent> paste = new Vector<VsqEvent>();
                    int count = copied_events.size();
                    for ( int i = 0; i < count; i++ ) {
                        VsqEvent item = (VsqEvent)copied_events.get( i ).clone();
                        item.Clock = copied_events.get( i ).Clock + dclock;
                        paste.add( item );
                    }
                    add_event = VsqCommand.generateCommandEventAddRange( AppManager.getSelected(), paste.toArray( new VsqEvent[]{} ) );
                }
            }
            Vector<TempoTableEntry> copied_tempo = ce.tempo;
            if ( copied_tempo.size() != 0 ) {
                // テンポ変更の貼付けを実行
                int dclock = clock - copy_started_clock;
                int count = copied_tempo.size();
                int[] clocks = new int[count];
                int[] tempos = new int[count];
                for ( int i = 0; i < count; i++ ) {
                    TempoTableEntry item = copied_tempo.get( i );
                    clocks[i] = item.Clock + dclock;
                    tempos[i] = item.Tempo;
                }
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTempoRange( clocks, clocks, tempos ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
                refreshScreen();
                return;
            }
            Vector<TimeSigTableEntry> copied_timesig = ce.timesig;
            if ( copied_timesig.size() > 0 ) {
                // 拍子変更の貼付けを実行
                int bar_count = AppManager.getVsqFile().getBarCountFromClock( clock );
                int min_barcount = copied_timesig.get( 0 ).BarCount;
                for ( Iterator itr = copied_timesig.iterator(); itr.hasNext(); ){
                    TimeSigTableEntry tste = (TimeSigTableEntry)itr.next();
                    min_barcount = Math.Min( min_barcount, tste.BarCount );
                }
                int dbarcount = bar_count - min_barcount;
                int count = copied_timesig.size();
                int[] barcounts = new int[count];
                int[] numerators = new int[count];
                int[] denominators = new int[count];
                for ( int i = 0; i < count; i++ ) {
                    TimeSigTableEntry item = copied_timesig.get( i );
                    barcounts[i] = item.BarCount + dbarcount;
                    numerators[i] = item.Numerator;
                    denominators[i] = item.Denominator;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTimesigRange( barcounts, barcounts, numerators, denominators ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
                refreshScreen();
                return;
            }

            // BPPairの貼付け
            VsqCommand edit_bpcurve = null; // BPListを変更するコマンド
            TreeMap<CurveType, VsqBPList> copied_curve = ce.points;
#if DEBUG
            Console.WriteLine( "FormMain#pasteEvent; copied_curve.size()=" + copied_curve.size() );
#endif
            if ( copied_curve.size() > 0 ) {
                int dclock = clock - copy_started_clock;

                TreeMap<String, VsqBPList> work = new TreeMap<String, VsqBPList>();
                for ( Iterator itr = copied_curve.keySet().iterator(); itr.hasNext(); ){
                    CurveType curve = (CurveType)itr.next();
                    VsqBPList list = copied_curve.get( curve );
#if DEBUG
                    AppManager.debugWriteLine( "FormMain#pasteEvent; curve=" + curve );
#endif
                    if ( curve.IsScalar ) {
                        continue;
                    }
                    if ( curve.IsAttachNote ) {
                        //todo: FormMain+PasteEvent; VibratoRate, VibratoDepthカーブのペースト処理
                    } else {
                        VsqBPList target = (VsqBPList)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( curve.getName() ).clone();
                        int count = list.size();
#if DEBUG
                        Console.WriteLine( "FormMain#pasteEvent; list.getCount()=" + count );
#endif
                        for ( int i = 0; i < count; i++ ) {
                            target.add( list.getKeyClock( i ) + dclock, list.getElementA( i ) );
                        }
                        if ( copied_curve.size() == 1 ) {
                            work.put( trackSelector.SelectedCurve.getName(), target );
                        } else {
                            work.put( curve.getName(), target );
                        }
                    }
                }
#if DEBUG
                Console.WriteLine( "FormMain#pasteEvent; work.size()=" + work.size() );
#endif
                if ( work.size() > 0 ) {
                    String[] curves = new String[work.size()];
                    VsqBPList[] bplists = new VsqBPList[work.size()];
                    int count = -1;
                    for ( Iterator itr = work.keySet().iterator(); itr.hasNext(); ){
                        String s = (String)itr.next();
                        count++;
                        curves[count] = s;
                        bplists[count] = work.get( s );
                    }
                    edit_bpcurve = VsqCommand.generateCommandTrackCurveReplaceRange( AppManager.getSelected(), curves, bplists );
                }
                AppManager.clearSelectedPoint();
            }

            // ベジエ曲線の貼付け
            CadenciiCommand edit_bezier = null;
            TreeMap<CurveType, Vector<BezierChain>> copied_bezier = ce.beziers;
#if DEBUG
            Console.WriteLine( "FormMain#pasteEvent; copied_bezier.size()=" + copied_bezier.size() );
#endif
            if ( copied_bezier.size() > 0 ) {
                int dclock = clock - copy_started_clock;
                BezierCurves attached_curve = (BezierCurves)AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).clone();
                TreeMap<CurveType, Vector<BezierChain>> command_arg = new TreeMap<CurveType, Vector<BezierChain>>();
                for ( Iterator itr = copied_bezier.keySet().iterator(); itr.hasNext(); ){
                    CurveType curve = (CurveType)itr.next();
                    if ( curve.IsScalar ) {
                        continue;
                    }
                    for ( Iterator itr2 = copied_bezier.get( curve ).iterator(); itr2.hasNext(); ){
                        BezierChain bc = (BezierChain)itr2.next();
                        BezierChain bc_copy = (BezierChain)bc.Clone();
                        for ( Iterator itr3 = bc_copy.points.iterator(); itr3.hasNext(); ){
                            BezierPoint bp = (BezierPoint)itr3.next();
                            bp.setBase( new PointD( bp.getBase().X + dclock, bp.getBase().Y ) );
                        }
                        attached_curve.mergeBezierChain( curve, bc_copy );
                    }
                    Vector<BezierChain> arg = new Vector<BezierChain>();
                    for ( Iterator itr2 = attached_curve.get( curve ).iterator(); itr2.hasNext(); ){
                        BezierChain bc = (BezierChain)itr2.next();
                        arg.add( bc );
                    }
                    command_arg.put( curve, arg );
                }
                edit_bezier = VsqFileEx.generateCommandReplaceAttachedCurveRange( AppManager.getSelected(), command_arg );
            }

            int commands = 0;
            commands += (add_event != null) ? 1 : 0;
            commands += (edit_bpcurve != null) ? 1 : 0;
            commands += (edit_bezier != null) ? 1 : 0;

#if DEBUG
            AppManager.debugWriteLine( "FormMain#pasteEvent; commands=" + commands );
            AppManager.debugWriteLine( "FormMain#pasteEvent; (add_event != null)=" + (add_event != null) );
            AppManager.debugWriteLine( "FormMain#pasteEvent; (edit_bpcurve != null)=" + (edit_bpcurve != null) );
            AppManager.debugWriteLine( "FormMain#pasteEvent; (edit_bezier != null)=" + (edit_bezier != null) );
#endif
            if ( commands == 1 ) {
                if ( add_event != null ) {
                    CadenciiCommand run = new CadenciiCommand( add_event );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                } else if ( edit_bpcurve != null ) {
                    CadenciiCommand run = new CadenciiCommand( edit_bpcurve );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                } else if ( edit_bezier != null ) {
                    AppManager.register( AppManager.getVsqFile().executeCommand( edit_bezier ) );
                }
                Edited = true;
                refreshScreen();
            } else if ( commands > 1 ) {
                VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().Clone();
                if ( add_event != null ) {
                    work.executeCommand( add_event );
                }
                if ( edit_bezier != null ) {
                    work.executeCommand( edit_bezier );
                }
                if ( edit_bpcurve != null ) {
                    // edit_bpcurveのVsqCommandTypeはTrackEditCurveRangeしかありえない
                    work.executeCommand( edit_bpcurve );
                }
                CadenciiCommand run = VsqFileEx.generateCommandReplace( work );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
                refreshScreen();
            }
        }

        /// <summary>
        /// アイテムのコピーを行います
        /// </summary>
        private void copyEvent() {
#if DEBUG
            AppManager.debugWriteLine( "FormMain#copyEvent" );
#endif
            AppManager.clearClipBoard();
            int min = int.MaxValue; // コピーされたアイテムの中で、最小の開始クロック

            if ( AppManager.selectedRegionEnabled ) {
#if DEBUG
                Console.WriteLine( "FormMain#copyEvent; selected with CTRL key" );
#endif
                int stdx = AppManager.startToDrawX;
                int start_clock = clockFromXCoord( AppManager.selectedRegion.Start - stdx );
                int end_clock = clockFromXCoord( AppManager.selectedRegion.End - stdx );
                ClipboardEntry ce = new ClipboardEntry();
                ce.copyStartedClock = start_clock;
                ce.points = new TreeMap<CurveType, VsqBPList>();
                ce.beziers = new TreeMap<CurveType, Vector<BezierChain>>();
                foreach ( CurveType vct in AppManager.CURVE_USAGE ) {
                    if ( vct.IsScalar || vct.IsAttachNote ) {
                        continue;
                    }
                    VsqBPList tmp = new VsqBPList();
                    Vector<BezierChain> tmp_bezier = new Vector<BezierChain>();
                    copyCurveCor( AppManager.getSelected(),
                                  vct,
                                  start_clock,
                                  end_clock,
                                  out tmp_bezier,
                                  out tmp );
#if DEBUG
                    AppManager.debugWriteLine( "CopyEvent; tmp.getCount()=" + tmp.size() + "; tmp_bezier.Count=" + tmp_bezier.size() );
#endif
                    ce.beziers.put( vct, tmp_bezier );
                    ce.points.put( vct, tmp );
                }

                if ( AppManager.getSelectedEventCount() > 0 ) {
                    Vector<VsqEvent> list = new Vector<VsqEvent>();
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                        SelectedEventEntry item = (SelectedEventEntry)itr.next();
                        if ( item.original.ID.type == VsqIDType.Anote ) {
                            min = Math.Min( item.original.Clock, min );
                            list.add( (VsqEvent)item.original.clone() );
                        }
                    }
                    ce.events = list;
                }
                AppManager.setClipboard( ce );
            } else if ( AppManager.getSelectedEventCount() > 0 ) {
                Vector<VsqEvent> list = new Vector<VsqEvent>();
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    min = Math.Min( item.original.Clock, min );
                    list.add( (VsqEvent)item.original.clone() );
                }
                AppManager.setCopiedEvent( list, min );
            } else if ( AppManager.getSelectedTempoCount() > 0 ) {
                Vector<TempoTableEntry> list = new Vector<TempoTableEntry>();
                for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ){
                    ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                    int key = item.Key;
                    SelectedTempoEntry value = item.Value;
                    min = Math.Min( value.original.Clock, min );
                    list.add( (TempoTableEntry)value.original.Clone() );
                }
                AppManager.setCopiedTempo( list, min );
            } else if ( AppManager.getSelectedTimesigCount() > 0 ) {
                Vector<TimeSigTableEntry> list = new Vector<TimeSigTableEntry>();
                for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ){
                    ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                    int key = item.Key;
                    SelectedTimesigEntry value = item.Value;
                    min = Math.Min( value.original.Clock, min );
                    list.add( (TimeSigTableEntry)value.original.Clone() );
                }
                AppManager.setCopiedTimesig( list, min );
            } else if ( AppManager.getSelectedPointIDCount() > 0 ) {
                ClipboardEntry ce = new ClipboardEntry();
                ce.points = new TreeMap<CurveType,VsqBPList>();
                ce.beziers = new TreeMap<CurveType, Vector<BezierChain>>();
                
                KeyValuePair<Integer, Integer> t = trackSelector.SelectedRegion;
                int start = t.Key;
                int end = t.Value;
                ce.copyStartedClock = start;
                Vector<BezierChain> tmp_bezier;
                VsqBPList tmp_bppair;
                copyCurveCor( AppManager.getSelected(),
                              trackSelector.SelectedCurve,
                              start,
                              end,
                              out tmp_bezier,
                              out tmp_bppair );
#if DEBUG
                AppManager.debugWriteLine( "FormMain#copyEvent; AppManager.selectedPointIDs.size()>0; tmp_bppair.getCount()=" + tmp_bppair.size() + "; tmp_bezier.Count=" + tmp_bezier.size() );
#endif
                if ( tmp_bezier.size() > 0 ) {
                    ce.beziers.put( trackSelector.SelectedCurve, tmp_bezier );
                    if ( tmp_bppair.size() > 0 ) {
                        ce.points.put( trackSelector.SelectedCurve, tmp_bppair );
                    }
                } else {
                    if ( tmp_bppair.size() > 0 ) {
                        ce.copyStartedClock = tmp_bppair.getKeyClock( 0 );
                        ce.points.put( trackSelector.SelectedCurve, tmp_bppair );
                    }
                }
                AppManager.setClipboard( ce );
            }
        }

        private void CutEvent() {
            // まずコピー
            copyEvent();

            // 選択されたノートイベントがあれば、まず、削除を行うコマンドを発行
            VsqCommand delete_event = null;
            boolean other_command_executed = false;
            if ( AppManager.getSelectedEventCount() > 0 ) {
                Vector<Integer> ids = new Vector<Integer>();
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    ids.add( item.original.InternalID );
                }
                delete_event = VsqCommand.generateCommandEventDeleteRange( AppManager.getSelected(), ids.toArray( new Integer[]{} ) );
            }

            // Ctrlキーを押しながらドラッグしたか、そうでないかで分岐
            if ( AppManager.selectedRegionEnabled || AppManager.getSelectedPointIDCount() > 0 ) {
                int stdx = AppManager.startToDrawX;
                int start_clock, end_clock;
                if ( AppManager.selectedRegionEnabled ) {
                    start_clock = clockFromXCoord( AppManager.selectedRegion.Start - stdx );
                    end_clock = clockFromXCoord( AppManager.selectedRegion.End - stdx );
                } else {
                    start_clock = trackSelector.SelectedRegion.Key;
                    end_clock = trackSelector.SelectedRegion.Value;
                }

                // クローンを作成
                VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().Clone();
                if ( delete_event != null ) {
                    // 選択されたノートイベントがあれば、クローンに対して削除を実行
                    work.executeCommand( delete_event );
                }

                // BPListに削除処理を施す
                Vector<Vector<BPPair>> curves = new Vector<Vector<BPPair>>();
                Vector<String> types = new Vector<String>();
                foreach ( CurveType vct in AppManager.CURVE_USAGE ) {
                    if ( vct.IsScalar || vct.IsAttachNote ) {
                        continue;
                    }
                    Vector<BPPair> t = new Vector<BPPair>();
                    t.add( new BPPair( start_clock, work.Track.get( AppManager.getSelected() ).getCurve( vct.getName() ).getValue( start_clock ) ) );
                    t.add( new BPPair( end_clock, work.Track.get( AppManager.getSelected() ).getCurve( vct.getName() ).getValue( end_clock ) ) );
                    curves.add( t );
                    types.add( vct.getName() );
                }
                CadenciiCommand delete_curve = new CadenciiCommand( VsqCommand.generateCommandTrackCurveEditRange( AppManager.getSelected(), types.toArray( new String[]{} ), curves.toArray( new Vector<BPPair>[]{} ) ) );
                work.executeCommand( delete_curve );

                // ベジエ曲線に削除処理を施す
                Vector<CurveType> target_curve = new Vector<CurveType>();
                if ( AppManager.selectedRegionEnabled ) {
                    // ctrlによる全選択モード
                    foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                        if ( ct.IsScalar || ct.IsAttachNote ) {
                            continue;
                        }
                        target_curve.add( ct );
                    }
                } else {
                    // 普通の選択モード
                    target_curve.add( trackSelector.SelectedCurve );
                }
                work.AttachedCurves.get( AppManager.getSelected() - 1 ).deleteBeziers( target_curve, start_clock, end_clock );

                // コマンドを発行し、実行
                CadenciiCommand run = VsqFileEx.generateCommandReplace( work );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                this.Edited = true;

                other_command_executed = true;
            } else if ( AppManager.getSelectedTempoCount() > 0 ) {
                // テンポ変更のカット
                int count = -1;
                int[] dum = new int[AppManager.getSelectedTempoCount()];
                int[] clocks = new int[AppManager.getSelectedTempoCount()];
                for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ){
                    ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                    int key = item.Key;
                    SelectedTempoEntry value = item.Value;
                    count++;
                    dum[count] = -1;
                    clocks[count] = value.original.Clock;
                }
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTempoRange( clocks, clocks, dum ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
                other_command_executed = true;
            } else if ( AppManager.getSelectedTimesigCount() > 0 ) {
                // 拍子変更のカット
                int[] barcounts = new int[AppManager.getSelectedTimesigCount()];
                int[] numerators = new int[AppManager.getSelectedTimesigCount()];
                int[] denominators = new int[AppManager.getSelectedTimesigCount()];
                int count = -1;
                for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ){
                    ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                    int key = item.Key;
                    SelectedTimesigEntry value = item.Value;
                    count++;
                    barcounts[count] = value.original.BarCount;
                    numerators[count] = -1;
                    denominators[count] = -1;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTimesigRange( barcounts, barcounts, numerators, denominators ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
                other_command_executed = true;
            }

            // 冒頭で作成した音符イベント削除以外に、コマンドが実行されなかった場合
            if ( delete_event != null && !other_command_executed ) {
                CadenciiCommand run = new CadenciiCommand( delete_event );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
            }

            refreshScreen();
        }

        private void copyCurveCor(
            int track,
            CurveType curve_type,
            int start,
            int end,
            out Vector<BezierChain> copied_chain,
            out VsqBPList copied_curve
        ) {
            copied_chain = new Vector<BezierChain>();
            for ( Iterator itr = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).get( curve_type ).iterator(); itr.hasNext(); ){
                BezierChain bc = (BezierChain)itr.next();
                int len = bc.points.size();
                if ( len < 2 ) {
                    continue;
                }
                int chain_start = (int)bc.points.get( 0 ).getBase().X;
                int chain_end = (int)bc.points.get( len - 1 ).getBase().X;
                if ( start < chain_start && chain_start < end && end < chain_end ) {
                    // (1) chain_start ~ end をコピー
                    copied_chain.add( bc.extractPartialBezier( chain_start, end ) );
                } else if ( chain_start <= start && end <= chain_end ) {
                    // (2) start ~ endをコピー
                    copied_chain.add( bc.extractPartialBezier( start, end ) );
                } else if ( chain_start < start && start < chain_end && chain_end <= end ) {
                    // (3) start ~ chain_endをコピー
                    copied_chain.add( bc.extractPartialBezier( start, chain_end ) );
                } else if ( start <= chain_start && chain_end <= end ) {
                    // (4) 全部コピーでOK
                    copied_chain.add( (BezierChain)bc.Clone() );
                }
            }

            copied_curve = new VsqBPList( curve_type.getDefault(), curve_type.getMinimum(), curve_type.getMaximum() );
            VsqBPList target = AppManager.getVsqFile().Track.get( track ).getCurve( curve_type.getName() );
            if ( copied_chain.size() > 0 ) {
                copied_curve.add( start, target.getValue( start ) );
                for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                    int clock = (int)itr.next();
                    if ( start < clock && clock <= end ) {
                        copied_curve.add( clock, target.getValue( clock ) );
                    }
                }
            } else {
                for( Iterator itr = AppManager.getSelectedPointIDIterator(); itr.hasNext(); ){
                    long id = (Long)itr.next();
                    VsqBPPairSearchContext context = target.findElement( id );
                    if ( context.point.id == id ) {
                        copied_curve.add( context.clock, context.point.value );
                    }
                }
            }
        }
        #endregion

        #region トラックの編集関連
        private void CopyTrackCore() {
            VsqTrack track = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).Clone();
            track.setName( track.getName() + " (1)" );
            CadenciiCommand run = VsqFileEx.generateCommandAddTrack( track,
                                                                     AppManager.getVsqFile().Mixer.Slave.get( AppManager.getSelected() - 1 ),
                                                                     AppManager.getVsqFile().Track.size(),
                                                                     AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) ); ;
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            Edited = true;
            AppManager.mixerWindow.updateStatus();
            refreshScreen();
        }

        private void ChangeTrackNameCore() {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
            m_txtbox_track_name = new TextBoxEx();
            m_txtbox_track_name.Visible = false;
            int selector_width = trackSelector.SelectorWidth;
            int x = AppManager.KEY_LENGTH + (AppManager.getSelected() - 1) * selector_width;
            m_txtbox_track_name.Location = new Point( x, trackSelector.Height - TrackSelector.OFFSET_TRACK_TAB + 1 );
            m_txtbox_track_name.BorderStyle = BorderStyle.None;
            m_txtbox_track_name.Text = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getName();
            m_txtbox_track_name.KeyUp += new KeyEventHandler( m_txtbox_track_name_KeyUp );
            m_txtbox_track_name.Size = new Size( selector_width, TrackSelector.OFFSET_TRACK_TAB );
            m_txtbox_track_name.Parent = trackSelector;
            m_txtbox_track_name.Visible = true;
            m_txtbox_track_name.Focus();
            m_txtbox_track_name.SelectAll();
        }

        private void DeleteTrackCore() {
            int selected = AppManager.getSelected();
            if ( MessageBox.Show(
                    String.Format( _( "Do you wish to remove track? {0} : '{1}'" ), selected, AppManager.getVsqFile().Track.get( selected ).getName() ),
                    _APP_NAME,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question ) == DialogResult.Yes ) {
                VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().Clone();
                CadenciiCommand run = VsqFileEx.generateCommandDeleteTrack( AppManager.getSelected() );
                temp.executeCommand( run );
                CadenciiCommand run2 = VsqFileEx.generateCommandReplace( temp );
                if ( AppManager.getSelected() >= 2 ) {
                    AppManager.setSelected( AppManager.getSelected() - 1 );
                }
                int total_tracks = AppManager.getVsqFile().Track.size();
                for ( int i = selected; i < total_tracks - 1; i++ ) {
                    AppManager.setRenderRequired( i, AppManager.getRenderRequired( i + 1 ) );
                }
                AppManager.setRenderRequired( total_tracks - 1, false );
                AppManager.register( AppManager.getVsqFile().executeCommand( run2 ) );
#if USE_DOBJ
                UpdateDrawObjectList();
#endif
                Edited = true;
                AppManager.mixerWindow.updateStatus();
                refreshScreen();
            }
        }

        private void AddTrackCore() {
            int i = AppManager.getVsqFile().Track.size();
            String name = "Voice" + i;
            String singer = "Miku";
            VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().Clone();
            CadenciiCommand run = VsqFileEx.generateCommandAddTrack( new VsqTrack( name, singer ),
                                                                     new VsqMixerEntry( 0, 0, 0, 0 ),
                                                                     i,
                                                                     new BezierCurves() );
            temp.executeCommand( run );
            CadenciiCommand run2 = VsqFileEx.generateCommandReplace( temp );
            AppManager.register( AppManager.getVsqFile().executeCommand( run2 ) );
#if USE_DOBJ
            UpdateDrawObjectList();
#endif
            Edited = true;
            AppManager.setSelected( i );
            AppManager.mixerWindow.updateStatus();
            refreshScreen();
        }
        #endregion

        /// <summary>
        /// length, positionの各Quantizeモードに応じて、表示状態を更新します
        /// </summary>
        private void ApplyQuantizeMode() {
            cMenuPianoQuantize04.CheckState = CheckState.Unchecked;
            cMenuPianoQuantize08.CheckState = CheckState.Unchecked;
            cMenuPianoQuantize16.CheckState = CheckState.Unchecked;
            cMenuPianoQuantize32.CheckState = CheckState.Unchecked;
            cMenuPianoQuantize64.CheckState = CheckState.Unchecked;
            cMenuPianoQuantize128.CheckState = CheckState.Unchecked;
            cMenuPianoQuantizeOff.CheckState = CheckState.Unchecked;

            stripDDBtnQuantize04.CheckState = CheckState.Unchecked;
            stripDDBtnQuantize08.CheckState = CheckState.Unchecked;
            stripDDBtnQuantize16.CheckState = CheckState.Unchecked;
            stripDDBtnQuantize32.CheckState = CheckState.Unchecked;
            stripDDBtnQuantize64.CheckState = CheckState.Unchecked;
            stripDDBtnQuantize128.CheckState = CheckState.Unchecked;
            stripDDBtnQuantizeOff.CheckState = CheckState.Unchecked;

            menuSettingPositionQuantize04.CheckState = CheckState.Unchecked;
            menuSettingPositionQuantize08.CheckState = CheckState.Unchecked;
            menuSettingPositionQuantize16.CheckState = CheckState.Unchecked;
            menuSettingPositionQuantize32.CheckState = CheckState.Unchecked;
            menuSettingPositionQuantize64.CheckState = CheckState.Unchecked;
            menuSettingPositionQuantize128.CheckState = CheckState.Unchecked;
            menuSettingPositionQuantizeOff.CheckState = CheckState.Unchecked;

            stripDDBtnQuantize.Text = "QUANTIZE " + QuantizeModeUtil.GetString( AppManager.editorConfig.PositionQuantize );
            switch ( AppManager.editorConfig.PositionQuantize ) {
                case QuantizeMode.p4:
                    cMenuPianoQuantize04.CheckState = CheckState.Checked;
                    stripDDBtnQuantize04.CheckState = CheckState.Checked;
                    menuSettingPositionQuantize04.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p8:
                    cMenuPianoQuantize08.CheckState = CheckState.Checked;
                    stripDDBtnQuantize08.CheckState = CheckState.Checked;
                    menuSettingPositionQuantize08.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p16:
                    cMenuPianoQuantize16.CheckState = CheckState.Checked;
                    stripDDBtnQuantize16.CheckState = CheckState.Checked;
                    menuSettingPositionQuantize16.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p32:
                    cMenuPianoQuantize32.CheckState = CheckState.Checked;
                    stripDDBtnQuantize32.CheckState = CheckState.Checked;
                    menuSettingPositionQuantize32.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p64:
                    cMenuPianoQuantize64.CheckState = CheckState.Checked;
                    stripDDBtnQuantize64.CheckState = CheckState.Checked;
                    menuSettingPositionQuantize64.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p128:
                    cMenuPianoQuantize128.CheckState = CheckState.Checked;
                    stripDDBtnQuantize128.CheckState = CheckState.Checked;
                    menuSettingPositionQuantize128.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.off:
                    cMenuPianoQuantizeOff.CheckState = CheckState.Checked;
                    stripDDBtnQuantizeOff.CheckState = CheckState.Checked;
                    menuSettingPositionQuantizeOff.CheckState = CheckState.Checked;
                    break;
            }
            cMenuPianoQuantizeTriplet.Checked = AppManager.editorConfig.PositionQuantizeTriplet;
            stripDDBtnQuantizeTriplet.Checked = AppManager.editorConfig.PositionQuantizeTriplet;
            menuSettingPositionQuantizeTriplet.Checked = AppManager.editorConfig.PositionQuantizeTriplet;

            cMenuPianoLength04.CheckState = CheckState.Unchecked;
            cMenuPianoLength08.CheckState = CheckState.Unchecked;
            cMenuPianoLength16.CheckState = CheckState.Unchecked;
            cMenuPianoLength32.CheckState = CheckState.Unchecked;
            cMenuPianoLength64.CheckState = CheckState.Unchecked;
            cMenuPianoLength128.CheckState = CheckState.Unchecked;
            cMenuPianoLengthOff.CheckState = CheckState.Unchecked;

            stripDDBtnLength04.CheckState = CheckState.Unchecked;
            stripDDBtnLength08.CheckState = CheckState.Unchecked;
            stripDDBtnLength16.CheckState = CheckState.Unchecked;
            stripDDBtnLength32.CheckState = CheckState.Unchecked;
            stripDDBtnLength64.CheckState = CheckState.Unchecked;
            stripDDBtnLength128.CheckState = CheckState.Unchecked;
            stripDDBtnLengthOff.CheckState = CheckState.Unchecked;

            menuSettingLengthQuantize04.CheckState = CheckState.Unchecked;
            menuSettingLengthQuantize08.CheckState = CheckState.Unchecked;
            menuSettingLengthQuantize16.CheckState = CheckState.Unchecked;
            menuSettingLengthQuantize32.CheckState = CheckState.Unchecked;
            menuSettingLengthQuantize64.CheckState = CheckState.Unchecked;
            menuSettingLengthQuantize128.CheckState = CheckState.Unchecked;
            menuSettingLengthQuantizeOff.CheckState = CheckState.Unchecked;

            stripDDBtnLength.Text = "LENGTH " + QuantizeModeUtil.GetString( AppManager.editorConfig.LengthQuantize );
            switch ( AppManager.editorConfig.LengthQuantize ) {
                case QuantizeMode.p4:
                    cMenuPianoLength04.CheckState = CheckState.Checked;
                    stripDDBtnLength04.CheckState = CheckState.Checked;
                    menuSettingLengthQuantize04.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p8:
                    cMenuPianoLength08.CheckState = CheckState.Checked;
                    stripDDBtnLength08.CheckState = CheckState.Checked;
                    menuSettingLengthQuantize08.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p16:
                    cMenuPianoLength16.CheckState = CheckState.Checked;
                    stripDDBtnLength16.CheckState = CheckState.Checked;
                    menuSettingLengthQuantize16.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p32:
                    cMenuPianoLength32.CheckState = CheckState.Checked;
                    stripDDBtnLength32.CheckState = CheckState.Checked;
                    menuSettingLengthQuantize32.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p64:
                    cMenuPianoLength64.CheckState = CheckState.Checked;
                    stripDDBtnLength64.CheckState = CheckState.Checked;
                    menuSettingLengthQuantize64.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.p128:
                    cMenuPianoLength128.CheckState = CheckState.Checked;
                    stripDDBtnLength128.CheckState = CheckState.Checked;
                    menuSettingLengthQuantize128.CheckState = CheckState.Checked;
                    break;
                case QuantizeMode.off:
                    cMenuPianoLengthOff.CheckState = CheckState.Checked;
                    stripDDBtnLengthOff.CheckState = CheckState.Checked;
                    menuSettingLengthQuantizeOff.CheckState = CheckState.Checked;
                    break;
            }
            cMenuPianoLengthTriplet.Checked = AppManager.editorConfig.LengthQuantizeTriplet;
            stripDDBtnLengthTriplet.Checked = AppManager.editorConfig.LengthQuantizeTriplet;
            menuSettingLengthQuantizeTriplet.Checked = AppManager.editorConfig.LengthQuantizeTriplet;
        }

        /// <summary>
        /// 現在選択されている編集ツールに応じて、メニューのチェック状態を更新します
        /// </summary>
        private void applySelectedTool() {
            EditTool tool = AppManager.getSelectedTool();
            foreach ( ToolStripItem tsi in toolStripTool.Items ) {
                if ( tsi is ToolStripButton ) {
                    ToolStripButton tsb = (ToolStripButton)tsi;
                    if ( tsb.Tag != null && tsb.Tag is String ) {
                        if ( tool == EditTool.PALETTE_TOOL ) {
                            String id = (String)tsb.Tag;
                            tsb.Checked = (AppManager.selectedPaletteTool.Equals( id ));
#if DEBUG
                            Console.WriteLine( "FormMain#applySelectedTool; tsb.Name=" + tsb.Name + "; tsb.Checked=" + tsb.Checked );
#endif
                        } else {
                            tsb.Checked = false;
                        }
                    }
                }
            }
            foreach ( ToolStripItem tsi in cMenuTrackSelectorPaletteTool.DropDownItems ) {
                if ( tsi is ToolStripMenuItem ) {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                    if ( tsmi.Tag != null && tsmi.Tag is String ) {
                        if ( tool == EditTool.PALETTE_TOOL ) {
                            String id = (String)tsmi.Tag;
                            tsmi.Checked = (AppManager.selectedPaletteTool.Equals( id ));
                        } else {
                            tsmi.Checked = false;
                        }
                    }
                }
            }

            foreach ( ToolStripItem tsi in cMenuPianoPaletteTool.DropDownItems ) {
                if ( tsi is ToolStripMenuItem ) {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                    if ( tsmi.Tag != null && tsmi.Tag is String ) {
                        if ( tool == EditTool.PALETTE_TOOL ) {
                            String id = (String)tsmi.Tag;
                            tsmi.Checked = (AppManager.selectedPaletteTool.Equals( id ));
                        } else {
                            tsmi.Checked = false;
                        }
                    }
                }
            }

            switch ( AppManager.getSelectedTool() ) {
                case EditTool.ARROW:
                    cMenuPianoPointer.CheckState = CheckState.Checked;
                    cMenuPianoPencil.CheckState = CheckState.Unchecked;
                    cMenuPianoEraser.CheckState = CheckState.Unchecked;

                    cMenuTrackSelectorPointer.CheckState = CheckState.Checked;
                    cMenuTrackSelectorPencil.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorLine.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorEraser.CheckState = CheckState.Unchecked;

                    stripBtnPointer.Checked = true;
                    stripBtnPencil.Checked = false;
                    stripBtnLine.Checked = false;
                    stripBtnEraser.Checked = false;
                    break;
                case EditTool.PENCIL:
                    cMenuPianoPointer.CheckState = CheckState.Unchecked;
                    cMenuPianoPencil.CheckState = CheckState.Checked;
                    cMenuPianoEraser.CheckState = CheckState.Unchecked;

                    cMenuTrackSelectorPointer.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorPencil.CheckState = CheckState.Checked;
                    cMenuTrackSelectorLine.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorEraser.CheckState = CheckState.Unchecked;

                    stripBtnPointer.Checked = false;
                    stripBtnPencil.Checked = true;
                    stripBtnLine.Checked = false;
                    stripBtnEraser.Checked = false;
                    break;
                case EditTool.ERASER:
                    cMenuPianoPointer.CheckState = CheckState.Unchecked;
                    cMenuPianoPencil.CheckState = CheckState.Unchecked;
                    cMenuPianoEraser.CheckState = CheckState.Checked;

                    cMenuTrackSelectorPointer.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorPencil.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorLine.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorEraser.CheckState = CheckState.Checked;

                    stripBtnPointer.Checked = false;
                    stripBtnPencil.Checked = false;
                    stripBtnLine.Checked = false;
                    stripBtnEraser.Checked = true;
                    break;
                case EditTool.LINE:
                    cMenuPianoPointer.CheckState = CheckState.Unchecked;
                    cMenuPianoPencil.CheckState = CheckState.Unchecked;
                    cMenuPianoEraser.CheckState = CheckState.Unchecked;

                    cMenuTrackSelectorPointer.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorPencil.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorLine.CheckState = CheckState.Checked;
                    cMenuTrackSelectorEraser.CheckState = CheckState.Unchecked;

                    stripBtnPointer.Checked = false;
                    stripBtnPencil.Checked = false;
                    stripBtnLine.Checked = true;
                    stripBtnEraser.Checked = false;
                    break;
                case EditTool.PALETTE_TOOL:
                    cMenuPianoPointer.CheckState = CheckState.Unchecked;
                    cMenuPianoPencil.CheckState = CheckState.Unchecked;
                    cMenuPianoEraser.CheckState = CheckState.Unchecked;

                    cMenuTrackSelectorPointer.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorPencil.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorLine.CheckState = CheckState.Unchecked;
                    cMenuTrackSelectorEraser.CheckState = CheckState.Unchecked;

                    stripBtnPointer.Checked = false;
                    stripBtnPencil.Checked = false;
                    stripBtnLine.Checked = false;
                    stripBtnEraser.Checked = false;
                    break;
            }
            cMenuPianoCurve.Checked = AppManager.isCurveMode();
            cMenuTrackSelectorCurve.Checked = AppManager.isCurveMode();
            stripBtnCurve.Checked = AppManager.isCurveMode();
        }

        /// <summary>
        /// 画面上のマウス位置におけるクロック値を元に，_toolbar_measureの場所表示文字列を更新します．
        /// </summary>
        /// <param name="mouse_pos_x"></param>
        private void UpdatePositionViewFromMousePosition( int clock ) {
            int barcount = AppManager.getVsqFile().getBarCountFromClock( clock );
            int numerator, denominator;
            AppManager.getVsqFile().getTimesigAt( clock, out numerator, out denominator );
            int clock_per_beat = 480 / 4 * denominator;
            int barcount_clock = AppManager.getVsqFile().getClockFromBarCount( barcount );
            int beat = (clock - barcount_clock) / clock_per_beat;
            int odd = clock - barcount_clock - beat * clock_per_beat;
#if OBSOLUTE
            m_toolbar_measure.Measure = (barcount - AppManager.VsqFile.PreMeasure + 1) + " : " + (beat + 1) + " : " + odd.ToString( "000" );
#else
            stripLblMeasure.Text = (barcount - AppManager.getVsqFile().getPreMeasure() + 1) + " : " + (beat + 1) + " : " + odd.ToString( "000" );
#endif
        }

#if USE_DOBJ
        /// <summary>
        /// 描画すべきオブジェクトのリスト，m_draw_objectsを更新します
        /// </summary>
        private void UpdateDrawObjectList() {
            // m_draw_objects
            if ( m_draw_objects == null ) {
                m_draw_objects = new Vector<Vector<DrawObject>>();
            }
            lock ( m_draw_objects ) {
                if ( AppManager.getVsqFile() == null ) {
                    return;
                }
                for ( int i = 0; i < m_draw_start_index.Length; i++ ) {
                    m_draw_start_index[i] = 0;
                }
                if ( m_draw_objects != null ) {
                    for ( Iterator itr = m_draw_objects.iterator(); itr.hasNext(); ){
                        Vector<DrawObject> list = (Vector<DrawObject>)itr.next();
                        list.clear();
                    }
                    m_draw_objects.clear();
                }

                int xoffset = 6 + AppManager.KEY_LENGTH;
                int yoffset = 127 * AppManager.editorConfig.PxTrackHeight;
                float scalex = AppManager.scaleX;
                using ( Font SMALL_FONT = new Font( AppManager.editorConfig.ScreenFontName, 8 ) ) {
                    for ( int track = 1; track < AppManager.getVsqFile().Track.size(); track++ ) {
                        Vector<DrawObject> tmp = new Vector<DrawObject>();
                        for ( Iterator itr = AppManager.getVsqFile().Track.get( track ).getEventIterator(); itr.hasNext(); ) {
                            VsqEvent ev = (VsqEvent)itr.next();
                            int timesig = ev.Clock;
                            if ( ev.ID.LyricHandle != null ) {
                                int length = ev.ID.Length;
                                int note = ev.ID.Note;
                                int x = (int)(timesig * scalex + xoffset);
                                int y = -note * AppManager.editorConfig.PxTrackHeight + yoffset;
                                int lyric_width = (int)(length * scalex);
                                String lyric_jp = ev.ID.LyricHandle.L0.Phrase;
                                String lyric_en = ev.ID.LyricHandle.L0.getPhoneticSymbol();
                                String title = AppManager.trimString( lyric_jp + " [" + lyric_en + "]", SMALL_FONT, lyric_width );
                                int accent = ev.ID.DEMaccent;
                                int vibrato_start = x + lyric_width;
                                int vibrato_end = x;
                                int vibrato_delay = lyric_width * 2;
                                if ( ev.ID.VibratoHandle != null ) {
                                    double rate = (double)ev.ID.VibratoDelay / (double)length;
                                    vibrato_delay = _PX_ACCENT_HEADER + (int)((lyric_width - _PX_ACCENT_HEADER) * rate);
                                }
                                VibratoBPList rate_bp = null;
                                VibratoBPList depth_bp = null;
                                int rate_start = 0;
                                int depth_start = 0;
                                if ( ev.ID.VibratoHandle != null ) {
                                    rate_bp = ev.ID.VibratoHandle.RateBP;
                                    depth_bp = ev.ID.VibratoHandle.DepthBP;
                                    rate_start = ev.ID.VibratoHandle.StartRate;
                                    depth_start = ev.ID.VibratoHandle.StartDepth;
                                }
                                tmp.add( new DrawObject( new Rectangle( x, y, lyric_width, AppManager.editorConfig.PxTrackHeight ),
                                                         title,
                                                         accent,
                                                         ev.InternalID,
                                                         vibrato_delay,
                                                         false,
                                                         ev.ID.LyricHandle.L0.PhoneticSymbolProtected,
                                                         rate_bp,
                                                         depth_bp,
                                                         rate_start,
                                                         depth_start,
                                                         ev.ID.Note,
                                                         ev.UstEvent.Envelope,
                                                         ev.ID.Length ) );
                            }
                        }

                        // 重複部分があるかどうかを判定
                        for ( int i = 0; i < tmp.size() - 1; i++ ) {
                            boolean overwrapped = false;
                            for ( int j = i + 1; j < tmp.size(); j++ ) {
                                int startx = tmp.get( j ).pxRectangle.X;
                                int endx = tmp.get( j ).pxRectangle.X + tmp.get( j ).pxRectangle.Width;
                                if ( startx < tmp.get( i ).pxRectangle.X ) {
                                    if ( tmp.get( i ).pxRectangle.X < endx ) {
                                        overwrapped = true;
                                        tmp.get( j ).Overwrapped = true;
                                        // breakできない．2個以上の重複を検出する必要があるので．
                                    }
                                } else if ( tmp.get( i ).pxRectangle.X <= startx && startx < tmp.get( i ).pxRectangle.X + tmp.get( i ).pxRectangle.Width ) {
                                    overwrapped = true;
                                    tmp.get( j ).Overwrapped = true;
                                }
                            }
                            if ( overwrapped ) {
                                tmp.get( i ).Overwrapped = true;
                            }
                        }
                        m_draw_objects.add( tmp );
                    }
                }
            }
        }

        private void DrawTo( Graphics g, Size window_size, Point mouse_position ) {
            int start_draw_x = AppManager.startToDrawX;
            int start_draw_y = StartToDrawY;

            int width = window_size.Width;
            int height = window_size.Height;
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

            if ( AppManager.getSelectedEventCount() > 0 && m_input_textbox.Enabled ) {
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                int event_x = (int)(original.Clock * scalex + xoffset);
                int event_y = -original.ID.Note * track_height + yoffset;
                m_input_textbox.Left = event_x + 4;
                m_input_textbox.Top = event_y + 2;
            }

            Color black = AppManager.editorConfig.PianorollColorVocalo2Black.Color;
            Color white = AppManager.editorConfig.PianorollColorVocalo2White.Color;
            Color bar = AppManager.editorConfig.PianorollColorVocalo2Bar.Color;
            Color beat = AppManager.editorConfig.PianorollColorVocalo2Beat.Color;
            String renderer = "";
            if ( AppManager.getVsqFile() != null ) {
                renderer = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            }
            if ( renderer.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                black = AppManager.editorConfig.PianorollColorUtauBlack.Color;
                white = AppManager.editorConfig.PianorollColorUtauWhite.Color;
                bar = AppManager.editorConfig.PianorollColorUtauBar.Color;
                beat = AppManager.editorConfig.PianorollColorUtauBeat.Color;
            } else if ( renderer.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                black = AppManager.editorConfig.PianorollColorVocalo1Black.Color;
                white = AppManager.editorConfig.PianorollColorVocalo1White.Color;
                bar = AppManager.editorConfig.PianorollColorVocalo1Bar.Color;
                beat = AppManager.editorConfig.PianorollColorVocalo1Beat.Color;
            } else if ( renderer.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                black = AppManager.editorConfig.PianorollColorStraightBlack.Color;
                white = AppManager.editorConfig.PianorollColorStraightWhite.Color;
                bar = AppManager.editorConfig.PianorollColorStraightBar.Color;
                beat = AppManager.editorConfig.PianorollColorStraightBeat.Color;
            }

            TreeMap<Integer, Rectangle> property_relateditems = new TreeMap<Integer, Rectangle>();
            #region ピアノロール周りのスクロールバーなど
            // スクロール画面背景
                int calculated_height = height;
                if ( calculated_height > 0 ) {
                    g.FillRectangle( new SolidBrush( white ),
                                     new Rectangle( 3, 0, width, calculated_height ) );
                    g.FillRectangle( s_brs_240_240_240,
                                     new Rectangle( 3, 0, AppManager.KEY_LENGTH, calculated_height ) );
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
                if ( AppManager.getVsqFile() != null ) {
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
                        boolean note_is_whitekey = VsqNote.isNoteWhiteKey( i );

                        #region ピアノロール背景
                        SolidBrush b = new SolidBrush( Color.Black );
                        SolidBrush border;
                        boolean paint_required = true;
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
                                             new Rectangle( AppManager.KEY_LENGTH, y, width - AppManager.KEY_LENGTH, track_height + 1 ) );
                        }
                        if ( odd == 0 || odd == 5 ) {
                            g.DrawLine( new Pen( border ),
                                        new Point( AppManager.KEY_LENGTH, y + track_height ),
                                        new Point( width, y + track_height ) );
                        }
                        #endregion

                        #region プリメジャー部分のピアノロール背景
                        int premeasure_start_x = xoffset;
                        if ( premeasure_start_x < AppManager.KEY_LENGTH ) {
                            premeasure_start_x = AppManager.KEY_LENGTH;
                        }
                        int premeasure_end_x = (int)(AppManager.getVsqFile().getPreMeasureClocks() * scalex + xoffset);
                        if ( premeasure_start_x >= AppManager.KEY_LENGTH ) {
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
                int hilighted_note = -1;
                g.DrawLine( s_pen_212_212_212,
                            new Point( AppManager.KEY_LENGTH, 0 ),
                            new Point( AppManager.KEY_LENGTH, height - 15 ) );
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
                                new Point( AppManager.KEY_LENGTH, y ) );
                    boolean hilighted = false;
                    if ( AppManager.getEditMode() == EditMode.ADD_ENTRY ) {
                        if ( m_adding.ID.Note == i ) {
                            hilighted = true;
                            hilighted_note = i;
                        }
                    } else if ( AppManager.getEditMode() == EditMode.EDIT_LEFT_EDGE || AppManager.getEditMode() == EditMode.EDIT_RIGHT_EDGE ) {
#if DEBUG
                        //bocoree.debug.push_log( "(AppManager.LastSelectedEvent==null)=" + (AppManager.LastSelectedEvent == null) );
                        //bocoree.debug.push_log( "(AppManager.LastSelectedEvent.Original==null)=" + (AppManager.LastSelectedEvent.Original == null) );
#endif
                        if ( AppManager.getLastSelectedEvent().original.ID.Note == i ) { //TODO: ここでNullpointer exception
                            hilighted = true;
                            hilighted_note = i;
                        }
                    } else {
                        if ( 3 <= mouse_position.X && mouse_position.X <= width - 17 &&
                            0 <= mouse_position.Y && mouse_position.Y <= height - 1 ) {
                            if ( y <= mouse_position.Y && mouse_position.Y < y + track_height ) {
                                hilighted = true;
                                hilighted_note = i;
                            }
                        }
                    }
                    if ( hilighted ) {
                        g.FillRectangle( AppManager.getHilightBrush(),
                                         new Rectangle( 35, y, AppManager.KEY_LENGTH - 35, track_height ) );
                    }
                    if ( odd2 == 0 || hilighted ) {
                        g.DrawString( VsqNote.getNoteString( i ),
                                      AppManager.baseFont8,
                                      s_brs_072_077_098,
                                      new PointF( 42, y + half_track_height - AppManager.baseFont8OffsetHeight + 1 ) );
                    }
                    if ( !VsqNote.isNoteWhiteKey( i ) ) {
                        g.FillRectangle( s_brs_125_123_124, new Rectangle( 0, y, 34, track_height ) );
                    }
                    #endregion
                }
                g.ResetClip();

                g.SetClip( new Rectangle( AppManager.KEY_LENGTH, 0, width - AppManager.KEY_LENGTH, height ) );
                #region 小節ごとの線
                int dashed_line_step = AppManager.getPositionQuantizeClock();
                using ( Pen pen_bar = new Pen( bar ) )
                using ( Pen pen_beat = new Pen( beat ) ) {
                    for ( Iterator itr = AppManager.getVsqFile().getBarLineIterator( clockFromXCoord( width ) ); itr.hasNext(); ) {
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
                        if ( dashed_line_step > 1 && AppManager.isGridVisible() ) {
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
                    if ( AppManager.isOverlay() ) {
                        // まず、選択されていないトラックの簡易表示を行う
                        lock ( m_draw_objects ) {
                            int c = m_draw_objects.size();
                            for ( int i = 0; i < c; i++ ) {
                                if ( i == AppManager.getSelected() - 1 ) {
                                    continue;
                                }
                                Vector<DrawObject> target_list = m_draw_objects.get( i );
                                int j_start = m_draw_start_index[i];
                                boolean first = true;
                                int shift_center = AppManager.editorConfig.PxTrackHeight / 2;
                                int target_list_count = target_list.size();
                                for ( int j = j_start; j < target_list_count; j++ ) {
                                    DrawObject dobj = target_list.get( j );
                                    int x = dobj.pxRectangle.X - start_draw_x;
                                    y = dobj.pxRectangle.Y - start_draw_y;
                                    int lyric_width = dobj.pxRectangle.Width;
                                    if ( x + lyric_width < 0 ) {
                                        continue;
                                    } else if ( width < x ) {
                                        break;
                                    }
                                    if ( AppManager.isPlaying() && first ) {
                                        m_draw_start_index[i] = j;
                                        first = false;
                                    }
                                    if ( y + track_height < 0 || y > height ) {
                                        continue;
                                    }
                                    g.DrawLine( new Pen( AppManager.HILIGHT[i] ),
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
                    int selected = AppManager.getSelected();
                    boolean show_lyrics = AppManager.editorConfig.ShowLyric;
                    boolean show_exp_line = AppManager.editorConfig.ShowExpLine;
                    if ( selected >= 1 ) {
                        Region r = g.Clip.Clone();
                        g.Clip = new Region( new Rectangle( AppManager.KEY_LENGTH,
                                                            0,
                                                            pictPianoRoll.Width - AppManager.KEY_LENGTH,
                                                            pictPianoRoll.Height ) );
                        int j_start = m_draw_start_index[selected - 1];

                        boolean first = true;
                        lock ( m_draw_objects ) { //ここでロックを取得しないと、描画中にUpdateDrawObjectのサイズが0になる可能性がある
                            if ( selected - 1 < m_draw_objects.size() ) {
                                Vector<DrawObject> target_list = m_draw_objects.get( selected - 1 );
                                int c = target_list.size();
                                for ( int j = j_start; j < c; j++ ) {
                                    DrawObject dobj = target_list.get( j );
                                    int x = dobj.pxRectangle.X - start_draw_x;
                                    y = dobj.pxRectangle.Y - start_draw_y;
                                    int lyric_width = dobj.pxRectangle.Width;
                                    if ( x + lyric_width < 0 ) {
                                        continue;
                                    } else if ( width < x ) {
                                        break;
                                    }
                                    if ( AppManager.isPlaying() && first ) {
                                        m_draw_start_index[selected - 1] = j;
                                        first = false;
                                    }
                                    if ( y + 2 * track_height < 0 || y > height ) {
                                        continue;
                                    }
                                    Color id_fill;
                                    if ( AppManager.getSelectedEventCount() > 0 ) {
                                        boolean found = AppManager.isSelectedEventContains( AppManager.getSelected(), dobj.InternalID );
                                        if ( found ) {
                                            id_fill = AppManager.getHilightColor();
                                        } else {
                                            id_fill = s_note_fill;
                                        }
                                    } else {
                                        id_fill = s_note_fill;
                                    }
                                    g.FillRectangle(
                                        new SolidBrush( id_fill ),
                                        new Rectangle( x, y + 1, lyric_width, track_height - 1 ) );
                                    Font lyric_font = dobj.SymbolProtected ? AppManager.baseFont10Bold : AppManager.baseFont10;
                                    if ( dobj.Overwrapped ) {
                                        g.DrawRectangle( s_pen_125_123_124,
                                                         new Rectangle( x, y + 1, lyric_width, track_height - 1 ) );
                                        if ( show_lyrics ) {
                                            g.DrawString( dobj.Text,
                                                          lyric_font,
                                                          s_brs_147_147_147,
                                                          new PointF( x + 1, y + half_track_height - AppManager.baseFont10OffsetHeight + 1 ) );
                                        }
                                    } else {
                                        g.DrawRectangle( s_pen_125_123_124,
                                                         new Rectangle( x, y + 1, lyric_width, track_height - 1 ) );
                                        if ( show_lyrics ) {
#if DEBUG
                                            g.DrawString( dobj.Text + "(ID:" + dobj.InternalID + ")",
                                                          lyric_font,
                                                          Brushes.Black,
                                                          new PointF( x + 1, y + half_track_height - AppManager.baseFont10OffsetHeight + 1 ) );
#else
                                            g.DrawString( dobj.Text,
                                                          lyric_font,
                                                          Brushes.Black,
                                                          new PointF( x + 1, y + half_track_height - AppManager.baseFont10OffsetHeight + 1 ) );
#endif
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
                                        if ( AppManager.editorConfig.ViewAtcualPitch ) {
                                            if ( dobj.VibRate != null ) {
                                                int vibrato_delay = dobj.pxVibratoDelay;
                                                int vibrato_width = dobj.pxRectangle.Width - vibrato_delay;
                                                int vibrato_start = x + vibrato_delay;
                                                int vibrato_end = x + vibrato_delay + vibrato_width;
                                                int cl_sx = clockFromXCoord( vibrato_start );
                                                int cl_ex = clockFromXCoord( vibrato_end );
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
                    if ( AppManager.getEditMode() == EditMode.ADD_ENTRY ||
                         AppManager.getEditMode() == EditMode.ADD_FIXED_LENGTH_ENTRY ||
                         AppManager.getEditMode() == EditMode.REALTIME ) {
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
                    } else if ( AppManager.getEditMode() == EditMode.EDIT_VIBRATO_DELAY ) {
                        int x = (int)(m_adding.Clock * scalex + xoffset);
                        y = -m_adding.ID.Note * track_height + yoffset + 1;
                        int length = (int)(m_adding.ID.Length * scalex);
                        g.DrawRectangle( s_pen_a136_000_000_000,
                                         new Rectangle( x, y, length, track_height - 1 ) );
                    } else if ( (AppManager.getEditMode() == EditMode.MOVE_ENTRY ||
                           AppManager.getEditMode() == EditMode.EDIT_LEFT_EDGE ||
                           AppManager.getEditMode() == EditMode.EDIT_RIGHT_EDGE) && AppManager.getSelectedEventCount() > 0 ) {
                        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                            SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                            int x = (int)(ev.editing.Clock * scalex + xoffset);
                            y = -ev.editing.ID.Note * track_height + yoffset + 1;
                            if ( ev.editing.ID.Length == 0 ) {
                                g.DrawRectangle( s_pen_dashed_171_171_171,
                                                 new Rectangle( x, y, 10, track_height - 1 ) );
                            } else {
                                int length = (int)(ev.editing.ID.Length * scalex);
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
                if ( AppManager.getEditMode() == EditMode.ADD_ENTRY ) {
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
                } else if ( AppManager.getEditMode() == EditMode.MOVE_ENTRY || AppManager.getEditMode() == EditMode.MOVE_ENTRY_WAIT_MOVE ) {
                    #region EditMode.MoveEntry || EditMode.MoveEntryWaitMove
                    if ( AppManager.getSelectedEventCount() > 0 ) {
                        VsqEvent last = AppManager.getLastSelectedEvent().editing;
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
                                    new Point( AppManager.KEY_LENGTH, y + track_height / 2 - 2 ),
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
                                    new Point( AppManager.KEY_LENGTH, y + track_height / 2 - 1 ),
                                    new Point( x - 1, y + track_height / 2 - 1 ) );
                        g.DrawLine( s_pen_RD,
                                    new Point( x + length + 1, y + track_height / 2 - 1 ),
                                    new Point( width, y + track_height / 2 - 1 ) );
                    }
                    #endregion
                } else if ( AppManager.getEditMode() == EditMode.ADD_FIXED_LENGTH_ENTRY ) {
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
                                new Point( AppManager.KEY_LENGTH, y + track_height / 2 - 2 ),
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
                                new Point( AppManager.KEY_LENGTH, y + track_height / 2 - 1 ),
                                new Point( x - 1, y + track_height / 2 - 1 ) );
                    g.DrawLine( s_pen_RD,
                                new Point( x + length + 1, y + track_height / 2 - 1 ),
                                new Point( width, y + track_height / 2 - 1 ) );
                    #endregion
                } else if ( AppManager.getEditMode() == EditMode.EDIT_LEFT_EDGE ) {
                    #region EditMode.EditLeftEdge
                    VsqEvent last = AppManager.getLastSelectedEvent().editing;
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
                } else if ( AppManager.getEditMode() == EditMode.EDIT_RIGHT_EDGE ) {
                    #region EditMode.EditRightEdge
                    VsqEvent last = AppManager.getLastSelectedEvent().editing;
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
                } else if ( AppManager.getEditMode() == EditMode.EDIT_VIBRATO_DELAY ) {
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
                    String percent = rate + "%";
                    SizeF size = AppManager.measureString( percent, s_F9PT );
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

                // マウス位置での音階名
                if ( hilighted_note >= 0 ){   
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString( VsqNote.getNoteString( hilighted_note ), 
                                  AppManager.baseFont10Bold,
                                  Brushes.Black,
                                  new RectangleF( mouse_position.X - 110, mouse_position.Y - 50, 100, 100 ), 
                                  sf );
                }
                #endregion

                #region 外枠
                // 左(外側)
            g.DrawLine( s_pen_160_160_160,
                        new Point( 0, 0 ),
                        new Point( 0, height ) );
            // 左(内側)
            g.DrawLine( s_pen_105_105_105,
                        new Point( 1, 0 ),
                        new Point( 1, height ) );
            #endregion
        }
#endif

        /// <summary>
        /// アクセントを表す表情線を、指定された位置を基準点として描き込みます
        /// </summary>
        /// <param name="g"></param>
        /// <param name="accent"></param>
        private void DrawAccentLine( Graphics g, Point origin, int accent ) {
            int x0 = origin.X + 1;
            int y0 = origin.Y + 10;
            int height = 4 + accent * 4 / 100;
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Point[] ps = new Point[]{ new Point( x0, y0 ),
                                      new Point( x0 + 2, y0 ),
                                      new Point( x0 + 8, y0 - height ),
                                      new Point( x0 + 13, y0 ),
                                      new Point( x0 + 16, y0 ),
                                      new Point( x0 + 20, y0 - 4 ) };
            g.DrawLines( Pens.Black, ps );
            g.SmoothingMode = sm;
        }

        private void DrawVibratoLine( Graphics g, Point origin, int vibrato_length ) {
            int x0 = origin.X + 1;
            int y0 = origin.Y + 10;
            int clipx = origin.X + 1;
            int clip_length = vibrato_length;
            if ( clipx < AppManager.KEY_LENGTH ) {
                clipx = AppManager.KEY_LENGTH;
                clip_length = origin.X + 1 + vibrato_length - AppManager.KEY_LENGTH;
                if ( clip_length <= 0 ) {
                    return;
                }
            }
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            const int _UWID = 10;
            int count = vibrato_length / _UWID + 1;
            Point[] _BASE = new Point[]{ new Point( x0 - _UWID, y0 - 4 ),
                                         new Point( x0 + 2 - _UWID, y0 - 7 ),
                                         new Point( x0 + 4 - _UWID, y0 - 7 ),
                                         new Point( x0 + 7 - _UWID, y0 - 1 ),
                                         new Point( x0 + 9 - _UWID, y0 - 1 ),
                                         new Point( x0 + 10 - _UWID, y0 - 4 ) };
            Region old = g.Clip.Clone();
            g.Clip = new Region( new Rectangle( clipx, origin.Y + 10 - 8, clip_length, 10 ) );
            for ( int i = 0; i < count; i++ ) {
                for ( int j = 0; j < _BASE.Length; j++ ) {
                    _BASE[j].X += _UWID;
                }
                g.DrawLines( Pens.Black, _BASE );
            }
            g.SmoothingMode = sm;
            g.Clip = old;
        }

        /// <summary>
        /// クロック数から、画面に描くべきx座標の値を取得します。
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns></returns>
        public int xCoordFromClocks( double clocks ) {
            return (int)(AppManager.KEY_LENGTH + clocks * AppManager.scaleX - AppManager.startToDrawX) + 6;
        }

        /// <summary>
        /// 画面のx座標からクロック数を取得します
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int clockFromXCoord( int x ) {
            return (int)((x + AppManager.startToDrawX - 6 - AppManager.KEY_LENGTH) / AppManager.scaleX);
        }

        /// <summary>
        /// _editor_configのRecentFilesを元に，menuFileRecentのドロップダウンアイテムを更新します
        /// </summary>
        private void UpdateRecentFileMenu() {
            int added = 0;
            menuFileRecent.DropDownItems.Clear();
            if ( AppManager.editorConfig.RecentFiles != null ) {
                for ( int i = 0; i < AppManager.editorConfig.RecentFiles.size(); i++ ) {
                    String item = AppManager.editorConfig.RecentFiles.get( i );
                    if ( item == null ) {
                        continue;
                    }
                    if ( item != "" ) {
                        String short_name = Path.GetFileName( item );
                        boolean available = File.Exists( item );
                        ToolStripItem itm = (ToolStripItem)(new ToolStripMenuItem( short_name ));
                        if ( !available ) {
                            itm.ToolTipText = _( "[file not found]" ) + " ";
                        }
                        itm.ToolTipText += item;
                        itm.Tag = item;
                        itm.Enabled = available;
                        itm.Click += new EventHandler( itm_Click );
                        itm.MouseEnter += new EventHandler( itm_MouseEnter );
                        menuFileRecent.DropDownItems.Add( itm );
                        added++;
                    }
                }
            } else {
                AppManager.editorConfig.PushRecentFiles( "" );
            }
            if ( added == 0 ) {
                menuFileRecent.Enabled = false;
            } else {
                menuFileRecent.Enabled = true;
            }
        }

        /// <summary>
        /// 最後に保存したときから変更されているかどうかを取得または設定します
        /// </summary>
        public boolean Edited {
            get {
                return m_edited;
            }
            set {
                m_edited = value;
                String file = AppManager.getFileName();
                if ( file.Equals( "" ) ) {
                    file = "Untitled";
                } else {
                    file = Path.GetFileNameWithoutExtension( file );
                }
                if ( m_edited ) {
                    file += " *";
                }
                String title = file + " - " + _APP_NAME;
                if ( this.Text != title ) {
                    this.Text = title;
                }
                boolean redo = AppManager.isRedoAvailable();
                boolean undo = AppManager.isUndoAvailable();
                menuEditRedo.Enabled = redo;
                menuEditUndo.Enabled = undo;
                cMenuPianoRedo.Enabled = redo;
                cMenuPianoUndo.Enabled = undo;
                cMenuTrackSelectorRedo.Enabled = redo;
                cMenuTrackSelectorUndo.Enabled = undo;
                stripBtnUndo.Enabled = undo;
                stripBtnRedo.Enabled = redo;
                AppManager.setRenderRequired( AppManager.getSelected(), true );
                if ( AppManager.getVsqFile() != null ) {
                    int draft = AppManager.getVsqFile().TotalClocks;
                    if ( draft > hScroll.Maximum ) {
                        SetHScrollRange( draft );
                    }
                }
#if USE_DOBJ
                UpdateDrawObjectList();
#endif
                AppManager.propertyPanel.UpdateValue( AppManager.getSelected() );
            }
        }

        /// <summary>
        /// 入力用のテキストボックスを初期化します
        /// </summary>
        private void ShowInputTextBox( String phrase, String phonetic_symbol, Point position, boolean phonetic_symbol_edit_mode ) {
#if DEBUG
            AppManager.debugWriteLine( "InitializeInputTextBox" );
#endif
            HideInputTextBox();
            m_input_textbox.KeyUp += m_input_textbox_KeyUp;
            m_input_textbox.KeyDown += m_input_textbox_KeyDown;
            m_input_textbox.ImeModeChanged += m_input_textbox_ImeModeChanged;
            m_input_textbox.ImeMode = m_last_imemode;
            if ( phonetic_symbol_edit_mode ) {
                m_input_textbox.Tag = new TagLyricTextBox( phrase, true );
                m_input_textbox.Text = phonetic_symbol;
                m_input_textbox.BackColor = s_txtbox_backcolor;
            } else {
                m_input_textbox.Tag = new TagLyricTextBox( phonetic_symbol, false );
                m_input_textbox.Text = phrase;
                m_input_textbox.BackColor = Color.White;
            }
            m_input_textbox.Font = new Font( AppManager.editorConfig.BaseFontName, 9 );
            m_input_textbox.Location = new Point( position.X + 4, position.Y + 2 );
            m_input_textbox.Parent = pictPianoRoll;
            m_input_textbox.Enabled = true;
            m_input_textbox.Visible = true;
            m_input_textbox.Focus();
            m_input_textbox.SelectAll();

        }

        private void HideInputTextBox() {
            m_input_textbox.KeyUp -= m_input_textbox_KeyUp;
            m_input_textbox.KeyDown -= m_input_textbox_KeyDown;
            m_input_textbox.ImeModeChanged -= m_input_textbox_ImeModeChanged;
            if ( m_input_textbox.Tag != null && m_input_textbox.Tag is TagLyricTextBox ) {
                TagLyricTextBox tltb = (TagLyricTextBox)m_input_textbox.Tag;
                m_last_symbol_edit_mode = tltb.PhoneticSymbolEditMode;
            }
            m_input_textbox.Visible = false;
            m_input_textbox.Parent = null;
            m_input_textbox.Enabled = false;
            pictPianoRoll.Focus();
        }

        /// <summary>
        /// 歌詞入力用テキストボックスのモード（歌詞/発音記号）を切り替えます
        /// </summary>
        private void FlipInputTextBoxMode() {
            TagLyricTextBox kvp = (TagLyricTextBox)m_input_textbox.Tag;
            String new_value = m_input_textbox.Text;
            if ( !kvp.PhoneticSymbolEditMode ) {
                m_input_textbox.BackColor = s_txtbox_backcolor;
            } else {
                m_input_textbox.BackColor = Color.White;
            }
            m_input_textbox.Text = kvp.BufferText;
            m_input_textbox.Tag = new TagLyricTextBox( new_value, !kvp.PhoneticSymbolEditMode );
        }

        /// <summary>
        /// 音の高さを表すnoteから、画面に描くべきy座標を計算します
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public int yCoordFromNote( float note ) {
            return yCoordFromNote( note, StartToDrawY );
        }

        public int yCoordFromNote( float note, int start_to_draw_y ) {
            return (int)(-1 * (note - 127.0f) * AppManager.editorConfig.PxTrackHeight) - start_to_draw_y;
        }

        /// <summary>
        /// ピアノロール画面のy座標から、その位置における音の高さを取得します
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int noteFromYCoord( int y ) {
            return 127 - (int)((double)(StartToDrawY + y) / (double)AppManager.editorConfig.PxTrackHeight);
        }

        /// <summary>
        /// 「選択されている」と登録されているオブジェクトのうち、Undo, Redoなどによって存在しなくなったものを登録解除する
        /// </summary>
        public void cleanupDeadSelection() {
            Vector<ValuePair<int, int>> list = new Vector<ValuePair<int, int>>();
            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                SelectedEventEntry item = (SelectedEventEntry)itr.next();
                list.add( new ValuePair<int, int>( item.track, item.original.InternalID ) );
            }

            for ( Iterator itr = list.iterator(); itr.hasNext(); ){
                ValuePair<Integer, Integer> specif = (ValuePair<Integer, Integer>)itr.next();
                boolean found = false;
                for ( Iterator itr2 = AppManager.getVsqFile().Track.get( specif.Key ).getNoteEventIterator(); itr2.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr2.next();
                    if ( item.InternalID == specif.Value ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    AppManager.removeSelectedEvent( specif.Key );
                }
            }
        }

        /// <summary>
        /// アンドゥ処理を行います
        /// </summary>
        public void undo() {
            if ( AppManager.isUndoAvailable() ) {
                AppManager.undo();
                menuEditRedo.Enabled = AppManager.isRedoAvailable();
                menuEditUndo.Enabled = AppManager.isUndoAvailable();
                cMenuPianoRedo.Enabled = AppManager.isRedoAvailable();
                cMenuPianoUndo.Enabled = AppManager.isUndoAvailable();
                cMenuTrackSelectorRedo.Enabled = AppManager.isRedoAvailable();
                cMenuTrackSelectorUndo.Enabled = AppManager.isUndoAvailable();
                AppManager.mixerWindow.updateStatus();
                Edited = true;
                cleanupDeadSelection();
#if USE_DOBJ
                UpdateDrawObjectList();
#endif
                if ( AppManager.propertyPanel != null ) {
                    AppManager.propertyPanel.UpdateValue( AppManager.getSelected() );
                }
            }
        }

        /// <summary>
        /// リドゥ処理を行います
        /// </summary>
        public void redo() {
            if ( AppManager.isRedoAvailable() ) {
                AppManager.redo();
                menuEditRedo.Enabled = AppManager.isRedoAvailable();
                menuEditUndo.Enabled = AppManager.isUndoAvailable();
                cMenuPianoRedo.Enabled = AppManager.isRedoAvailable();
                cMenuPianoUndo.Enabled = AppManager.isUndoAvailable();
                cMenuTrackSelectorRedo.Enabled = AppManager.isRedoAvailable();
                cMenuTrackSelectorUndo.Enabled = AppManager.isUndoAvailable();
                AppManager.mixerWindow.updateStatus();
                Edited = true;
                cleanupDeadSelection();
#if USE_DOBJ
                UpdateDrawObjectList();
#endif
                if ( AppManager.propertyPanel != null ) {
                    AppManager.propertyPanel.UpdateValue( AppManager.getSelected() );
                }
            }
        }

        int StartToDrawY {
            get {
                return (int)((128 * AppManager.editorConfig.PxTrackHeight - vScroll.Height) * (float)vScroll.Value / ((float)vScroll.Maximum));
            }
        }

        /// <summary>
        /// pがrcの中にあるかどうかを判定します
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        static boolean IsInRect( Point p, Rectangle rc ) {
            if ( rc.X <= p.X ) {
                if ( p.X <= rc.X + rc.Width ) {
                    if ( rc.Y <= p.Y ) {
                        if ( p.Y <= rc.Y + rc.Height ) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// マウス位置におけるIDを返します。該当するIDが無ければnullを返します
        /// rectには、該当するIDがあればその画面上での形状を、該当するIDがなければ、
        /// 画面上で最も近かったIDの画面上での形状を返します
        /// </summary>
        /// <param name="mouse_position"></param>
        /// <returns></returns>
        VsqEvent GetItemAtClickedPosition( Point mouse_position, out Rectangle rect ) {
            rect = new Rectangle();
            if ( AppManager.KEY_LENGTH <= mouse_position.X && mouse_position.X <= pictPianoRoll.Width ) {
                if ( 0 <= mouse_position.Y && mouse_position.Y <= pictPianoRoll.Height ) {
                    int selected = AppManager.getSelected();
                    if ( selected >= 1 ) {
                        for ( int j = 0; j < AppManager.getVsqFile().Track.get( selected ).getEventCount(); j++ ) {
                            int timesig = AppManager.getVsqFile().Track.get( selected ).getEvent( j ).Clock;
                            int internal_id = AppManager.getVsqFile().Track.get( selected ).getEvent( j ).InternalID;
                            // イベントで指定されたIDがLyricであった場合
                            if ( AppManager.getVsqFile().Track.get( selected ).getEvent( j ).ID.type == VsqIDType.Anote &&
                                AppManager.getVsqFile().Track.get( selected ).getEvent( j ).ID.LyricHandle != null ) {
                                // 発音長を取得
                                int length = AppManager.getVsqFile().Track.get( selected ).getEvent( j ).ID.Length;
                                int note = AppManager.getVsqFile().Track.get( selected ).getEvent( j ).ID.Note;
                                int x = xCoordFromClocks( timesig );
                                int y = yCoordFromNote( note );
                                int lyric_width = (int)(length * AppManager.scaleX);
                                if ( x + lyric_width < 0 ) {
                                    continue;
                                } else if ( pictPianoRoll.Width < x ) {
                                    break;
                                }
                                if ( x <= mouse_position.X && mouse_position.X <= x + lyric_width ) {
                                    if ( y + 1 <= mouse_position.Y && mouse_position.Y <= y + AppManager.editorConfig.PxTrackHeight ) {
                                        rect = new Rectangle( x, y + 1, lyric_width, AppManager.editorConfig.PxTrackHeight );
                                        return AppManager.getVsqFile().Track.get( selected ).getEvent( j );
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void openVsqCor( String file ) {
            AppManager.readVsq( file );
            if ( AppManager.getVsqFile().Track.size() >= 2 ) {
                AppManager.setBaseTempo( AppManager.getVsqFile().getBaseTempo() );
                SetHScrollRange( AppManager.getVsqFile().TotalClocks );
            }
            AppManager.editorConfig.PushRecentFiles( file );
            UpdateRecentFileMenu();
            Edited = false;
            AppManager.clearCommandBuffer();
            AppManager.mixerWindow.updateStatus();
        }

        private void updateMenuFonts() {
            if ( AppManager.editorConfig.BaseFontName.Equals( "" ) ) {
                return;
            }
            Font font = AppManager.editorConfig.BaseFont;
            this.Font = font;
            foreach ( Control c in this.Controls ) {
                Misc.ApplyFontRecurse( c, font );
            }
            Misc.ApplyContextMenuFontRecurse( cMenuPiano, font );
            Misc.ApplyContextMenuFontRecurse( cMenuTrackSelector, font );
            if ( AppManager.mixerWindow != null ) {
                Misc.ApplyFontRecurse( AppManager.mixerWindow, font );
            }
            Misc.ApplyContextMenuFontRecurse( cMenuTrackTab, font );
            trackSelector.applyFont( font );
            Misc.ApplyToolStripFontRecurse( menuFile, font );
            Misc.ApplyToolStripFontRecurse( menuEdit, font );
            Misc.ApplyToolStripFontRecurse( menuVisual, font );
            Misc.ApplyToolStripFontRecurse( menuJob, font );
            Misc.ApplyToolStripFontRecurse( menuTrack, font );
            Misc.ApplyToolStripFontRecurse( menuLyric, font );
            Misc.ApplyToolStripFontRecurse( menuScript, font );
            Misc.ApplyToolStripFontRecurse( menuSetting, font );
            Misc.ApplyToolStripFontRecurse( menuHelp, font );
            foreach ( ToolStripItem tsi in toolStripFile.Items ) {
                Misc.ApplyToolStripFontRecurse( tsi, font );
            }
            foreach ( ToolStripItem tsi in toolStripMeasure.Items ) {
                Misc.ApplyToolStripFontRecurse( tsi, font );
            }
            foreach ( ToolStripItem tsi in toolStripPosition.Items ) {
                Misc.ApplyToolStripFontRecurse( tsi, font );
            }
            foreach ( ToolStripItem tsi in toolStripTool.Items ) {
                Misc.ApplyToolStripFontRecurse( tsi, font );
            }
            if ( m_preference_dlg != null ) {
                Misc.ApplyFontRecurse( m_preference_dlg, font );
            }

            AppManager.baseFont10Bold = new Font( AppManager.editorConfig.BaseFontName, 10, FontStyle.Bold );
            AppManager.baseFont8 = new Font( AppManager.editorConfig.BaseFontName, 8 );
            AppManager.baseFont10 = new Font( AppManager.editorConfig.BaseFontName, 10 );
            AppManager.baseFont9 = new Font( AppManager.editorConfig.BaseFontName, 9 );
            AppManager.baseFont10OffsetHeight = Misc.GetStringDrawOffset( AppManager.baseFont10 );
            AppManager.baseFont8OffsetHeight = Misc.GetStringDrawOffset( AppManager.baseFont8 );
            AppManager.baseFont9OffsetHeight = Misc.GetStringDrawOffset( AppManager.baseFont9 );
        }

        private void picturePositionIndicatorDrawTo( Graphics g ) {
            using ( Font SMALL_FONT = new Font( AppManager.editorConfig.ScreenFontName, 8 ) ) {
                int width = picturePositionIndicator.Width;
                int height = picturePositionIndicator.Height;

                #region 小節ごとの線
                int dashed_line_step = AppManager.getPositionQuantizeClock();
                for ( Iterator itr = AppManager.getVsqFile().getBarLineIterator( clockFromXCoord( width ) ); itr.hasNext(); ) {
                    VsqBarLineType blt = (VsqBarLineType)itr.next();
                    int local_clock_step = 480 * 4 / blt.getLocalDenominator();
                    int x = xCoordFromClocks( blt.clock() );
                    if ( blt.isSeparator() ) {
                        int current = blt.getBarCount() - AppManager.getVsqFile().getPreMeasure() + 1;
                        g.DrawLine( s_pen_105_105_105,
                                    new Point( x, 3 ),
                                    new Point( x, 46 ) );
                        // 小節の数字
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.DrawString( current.ToString(),
                                      SMALL_FONT,
                                      Brushes.Black,
                                      new PointF( x + 4, 6 ) );
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    } else {
                        g.DrawLine( s_pen_105_105_105,
                                    new Point( x, 11 ),
                                    new Point( x, 16 ) );
                        g.DrawLine( s_pen_105_105_105,
                                    new Point( x, 26 ),
                                    new Point( x, 31 ) );
                        g.DrawLine( s_pen_105_105_105,
                                    new Point( x, 41 ),
                                    new Point( x, 46 ) );
                    }
                    if ( dashed_line_step > 1 && AppManager.isGridVisible() ) {
                        int numDashedLine = local_clock_step / dashed_line_step;
                        for ( int i = 1; i < numDashedLine; i++ ) {
                            int x2 = xCoordFromClocks( blt.clock() + i * dashed_line_step );
                            g.DrawLine( s_pen_065_065_065,
                                        new Point( x2, 9 + 5 ),
                                        new Point( x2, 14 + 3 ) );
                            g.DrawLine( s_pen_065_065_065,
                                        new Point( x2, 24 + 5 ),
                                        new Point( x2, 29 + 3 ) );
                            g.DrawLine( s_pen_065_065_065,
                                        new Point( x2, 39 + 5 ),
                                        new Point( x2, 44 + 3 ) );
                        }
                    }
                }
                #endregion

                if ( AppManager.getVsqFile() != null ) {
                    #region 拍子の変更
                    for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                        int clock = AppManager.getVsqFile().TimesigTable.get( i ).Clock;
                        int barcount = AppManager.getVsqFile().TimesigTable.get( i ).BarCount;
                        int x = xCoordFromClocks( clock );
                        if ( width < x ) {
                            break;
                        }
                        String s = AppManager.getVsqFile().TimesigTable.get( i ).Numerator + "/" + AppManager.getVsqFile().TimesigTable.get( i ).Denominator;
                        if ( AppManager.isSelectedTimesigContains( barcount ) ) {
                            g.DrawString( s,
                                          SMALL_FONT,
                                          AppManager.getHilightBrush(),
                                          new PointF( x + 4, 36 ) );
                        } else {
                            g.DrawString( s,
                                          SMALL_FONT,
                                          Brushes.Black,
                                          new PointF( x + 4, 36 ) );
                        }

                        if ( m_timesig_dragging ) {
                            if ( AppManager.isSelectedTimesigContains( barcount ) ) {
                                int edit_clock_x = xCoordFromClocks( AppManager.getVsqFile().getClockFromBarCount( AppManager.getSelectedTimesig( barcount ).editing.BarCount ) );
                                g.DrawLine( s_pen_187_187_255,
                                            new Point( edit_clock_x - 1, 32 ),
                                            new Point( edit_clock_x - 1, picturePositionIndicator.Height - 1 ) );
                                g.DrawLine( s_pen_007_007_151,
                                            new Point( edit_clock_x, 32 ),
                                            new Point( edit_clock_x, picturePositionIndicator.Height - 1 ) );
                            }
                        }
                    }
                    #endregion

                    #region テンポの変更
                    for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                        int clock = AppManager.getVsqFile().TempoTable.get( i ).Clock;
                        int x = xCoordFromClocks( clock );
                        if ( width < x ) {
                            break;
                        }
                        String s = (60e6 / (float)AppManager.getVsqFile().TempoTable.get( i ).Tempo).ToString( "#.00" );
                        if ( AppManager.isSelectedTempoContains( clock ) ) {
                            g.DrawString( s,
                                          SMALL_FONT,
                                          AppManager.getHilightBrush(),
                                          new PointF( x + 4, 21 ) );
                        } else {
                            g.DrawString( s,
                                          SMALL_FONT,
                                          Brushes.Black,
                                          new PointF( x + 4, 21 ) );
                        }

                        if ( m_tempo_dragging ) {
                            if ( AppManager.isSelectedTempoContains( clock ) ) {
                                int edit_clock_x = xCoordFromClocks( AppManager.getSelectedTempo( clock ).editing.Clock );
                                g.DrawLine( s_pen_187_187_255,
                                            new Point( edit_clock_x - 1, 18 ),
                                            new Point( edit_clock_x - 1, 32 ) );
                                g.DrawLine( s_pen_007_007_151,
                                            new Point( edit_clock_x, 18 ),
                                            new Point( edit_clock_x, 32 ) );
                            }
                        }
                    }
                    #endregion
                }

                #region 外枠
                /* 左(外側) */
                g.DrawLine( new Pen( Color.FromArgb( 160, 160, 160 ) ),
                            new Point( 0, 0 ),
                            new Point( 0, height - 1 ) );
                /* 左(内側) */
                g.DrawLine( new Pen( Color.FromArgb( 105, 105, 105 ) ),
                            new Point( 1, 1 ),
                            new Point( 1, height - 2 ) );
                /* 中(上側) */
                g.DrawLine( new Pen( Color.FromArgb( 160, 160, 160 ) ),
                            new Point( 1, 47 ),
                            new Point( width - 2, 47 ) );
                /* 中(下側) */
                g.DrawLine( new Pen( Color.FromArgb( 105, 105, 105 ) ),
                            new Point( 2, 48 ),
                            new Point( width - 3, 48 ) );
                // 右(外側)
                g.DrawLine( new Pen( Color.White ),
                            new Point( width - 1, 0 ),
                            new Point( width - 1, height - 1 ) );
                // 右(内側)
                g.DrawLine( new Pen( Color.FromArgb( 241, 239, 226 ) ),
                            new Point( width - 2, 1 ),
                            new Point( width - 2, height - 1 ) );
                #endregion

                #region TEMPO & BEAT
                // TEMPO BEATの文字の部分。小節数が被っている可能性があるので、塗り潰す
                g.FillRectangle(
                    new SolidBrush( picturePositionIndicator.BackColor ),
                    new Rectangle( 2, 3, 65, 45 ) );
                // 横ライン上
                g.DrawLine( new Pen( Color.FromArgb( 104, 104, 104 ) ),
                            new Point( 2, 17 ),
                            new Point( width - 3, 17 ) );
                // 横ライン中央
                g.DrawLine( new Pen( Color.FromArgb( 104, 104, 104 ) ),
                            new Point( 2, 32 ),
                            new Point( width - 3, 32 ) );
                // 横ライン下
                g.DrawLine( new Pen( Color.FromArgb( 104, 104, 104 ) ),
                            new Point( 2, 47 ),
                            new Point( width - 3, 47 ) );
                // 縦ライン
                g.DrawLine( new Pen( Color.FromArgb( 104, 104, 104 ) ),
                            new Point( AppManager.KEY_LENGTH, 2 ),
                            new Point( AppManager.KEY_LENGTH, 46 ) );
                /* TEMPO&BEATとピアノロールの境界 */
                g.DrawLine( new Pen( Color.FromArgb( 104, 104, 104 ) ),
                            new Point( AppManager.KEY_LENGTH, 48 ),
                            new Point( width - 18, 48 ) );
                g.DrawString( "TEMPO",
                              SMALL_FONT,
                              Brushes.Black,
                              new PointF( 11, 20 ) );
                g.DrawString( "BEAT",
                              SMALL_FONT,
                              Brushes.Black,
                              new PointF( 11, 35 ) );
                g.DrawLine( new Pen( Color.FromArgb( 172, 168, 153 ) ),
                            new Point( 0, 0 ),
                            new Point( width, 0 ) );
                g.DrawLine( new Pen( Color.FromArgb( 113, 111, 100 ) ),
                            new Point( 1, 1 ),
                            new Point( width - 1, 1 ) );

                #endregion

                #region 現在のマーカー
                float xoffset = AppManager.KEY_LENGTH + 6 - AppManager.startToDrawX;
                int marker_x = (int)(AppManager.getCurrentClock() * AppManager.scaleX + xoffset);
                if ( AppManager.KEY_LENGTH <= marker_x && marker_x <= width ) {
                    g.DrawLine(
                        new Pen( Color.White, 2f ),
                        new Point( marker_x, 2 ),
                        new Point( marker_x, height ) );
                }
                if ( AppManager.startMarkerEnabled ) {
                    int x = xCoordFromClocks( AppManager.startMarker );
                    g.DrawImage(
                        Properties.Resources.start_marker, new Point( x, 3 ) );
                }
                if ( AppManager.endMarkerEnabled ) {
                    int x = xCoordFromClocks( AppManager.endMarker ) - 6;
                    g.DrawImage(
                        Properties.Resources.end_marker, new Point( x, 3 ) );
                }
                #endregion
            }
        }
    }

}
