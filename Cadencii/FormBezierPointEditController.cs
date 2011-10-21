/*
 * FormBezierPointEditController.cs
 * Copyright © 2008-2011 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormBezierPointEdit.java

import java.awt.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using BEventHandler = System.EventHandler;
    using BMouseEventHandler = System.Windows.Forms.MouseEventHandler;
    using boolean = System.Boolean;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
#endif

#if JAVA
    public class FormBezierPointEditController extends BDialog {
#else
    public class FormBezierPointEditController : ControllerBase, FormBezierPointEditUiListener
    {
#endif
        private BezierPoint m_point;
        private int m_min;
        private int m_max;
        /// <summary>
        /// 移動ボタンでデータ点または制御点を動かすためにマウスを強制的に動かす直前の，スクリーン上のマウス位置
        /// </summary>
        private Point m_last_mouse_global_location;
        private TrackSelector m_parent;
        private boolean m_btn_datapoint_downed = false;
        private double m_min_opacity = 0.4;
        private CurveType m_curve_type;
        private int m_track;
        private int m_chain_id = -1;
        private int m_point_id = -1;
        private BezierPickedSide m_picked_side = BezierPickedSide.BASE;
        /// <summary>
        /// 移動ボタンでデータ点または制御点を動かすためにマウスを強制的に動かした直後の，スクリーン上のマウス位置
        /// </summary>
        private Point mScreenMouseDownLocation;
        private FormBezierPointEditUi ui;

        public FormBezierPointEditController(
            TrackSelector parent,
            CurveType curve_type,
            int selected_chain_id,
            int selected_point_id )
        {
            ui = new FormBezierPointEditUiImpl( this );

            registerEventHandlers();
            setResources();
            applyLanguage();
            m_parent = parent;
            m_curve_type = curve_type;
            m_track = AppManager.getSelected();
            m_chain_id = selected_chain_id;
            m_point_id = selected_point_id;
            boolean found = false;
            VsqFileEx vsq = AppManager.getVsqFile();
            BezierCurves attached = vsq.AttachedCurves.get( m_track - 1 );
            Vector<BezierChain> chains = attached.get( m_curve_type );
            for ( int i = 0; i < chains.size(); i++ ) {
                if ( chains.get( i ).id == m_chain_id ) {
                    found = true;
                    break;
                }
            }
            if ( !found ) {
                return;
            }
            boolean smooth = false;
            for ( Iterator<BezierPoint> itr = attached.getBezierChain( m_curve_type, m_chain_id ).points.iterator(); itr.hasNext(); ) {
                BezierPoint bp = itr.next();
                if ( bp.getID() == m_point_id ) {
                    m_point = bp;
                    smooth = 
                        (bp.getControlLeftType() != BezierControlType.None) ||
                        (bp.getControlRightType() != BezierControlType.None);
                    break;
                }
            }
            updateStatus();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage()
        {
            setTitle( _( "Edit Bezier Data Point" ) );

            groupDataPoint.setTitle( _( "Data Poin" ) );
            lblDataPointClock.setText( _( "Clock" ) );
            lblDataPointValue.setText( _( "Value" ) );

            groupLeft.setTitle( _( "Left Control Point" ) );
            lblLeftClock.setText( _( "Clock" ) );
            lblLeftValue.setText( _( "Value" ) );

            groupRight.setTitle( _( "Right Control Point" ) );
            lblRightClock.setText( _( "Clock" ) );
            lblRightValue.setText( _( "Value" ) );

            chkEnableSmooth.setText( _( "Smooth" ) );
        }
        #endregion

        #region helper methods
        private void updateStatus()
        {
            txtDataPointClock.setText( m_point.getBase().getX() + "" );
            txtDataPointValue.setText( m_point.getBase().getY() + "" );
            txtLeftClock.setText( ((int)(m_point.getBase().getX() + m_point.controlLeft.getX())) + "" );
            txtLeftValue.setText( ((int)(m_point.getBase().getY() + m_point.controlLeft.getY())) + "" );
            txtRightClock.setText( ((int)(m_point.getBase().getX() + m_point.controlRight.getX())) + "" );
            txtRightValue.setText( ((int)(m_point.getBase().getY() + m_point.controlRight.getY())) + "" );
            boolean smooth = 
                (m_point.getControlLeftType() != BezierControlType.None) ||
                (m_point.getControlRightType() != BezierControlType.None);
            chkEnableSmooth.setSelected( smooth );
            btnLeft.setEnabled( smooth );
            btnRight.setEnabled( smooth );
            m_min = m_curve_type.getMinimum();
            m_max = m_curve_type.getMaximum();
        }

        private static String _( String message )
        {
            return Messaging.getMessage( message );
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new BEventHandler( btnOK_Click );
            btnCancel.Click += new BEventHandler( btnCancel_Click );
            chkEnableSmooth.CheckedChanged += new BEventHandler( chkEnableSmooth_CheckedChanged );
            btnLeft.MouseMove += new BMouseEventHandler( common_MouseMove );
            btnLeft.MouseDown += new BMouseEventHandler( handleOperationButtonMouseDown );
            btnLeft.MouseUp += new BMouseEventHandler( common_MouseUp );
            btnDataPoint.MouseMove += new BMouseEventHandler( common_MouseMove );
            btnDataPoint.MouseDown += new BMouseEventHandler( handleOperationButtonMouseDown );
            btnDataPoint.MouseUp += new BMouseEventHandler( common_MouseUp );
            btnRight.MouseMove += new BMouseEventHandler( common_MouseMove );
            btnRight.MouseDown += new BMouseEventHandler( handleOperationButtonMouseDown );
            btnRight.MouseUp += new BMouseEventHandler( common_MouseUp );
            btnBackward.Click += new BEventHandler( handleMoveButtonClick );
            btnForward.Click += new BEventHandler( handleMoveButtonClick );
        }

        private void setResources()
        {
            this.btnLeft.setIcon( new ImageIcon( Resources.get_target__pencil() ) );
            this.btnDataPoint.setIcon( new ImageIcon( Resources.get_target__pencil() ) );
            this.btnRight.setIcon( new ImageIcon( Resources.get_target__pencil() ) );
        }
        #endregion

        #region event handlers
        public void btnOK_Click( Object sender, BEventArgs e )
        {
            try {
                int x, y;
                x = str.toi( txtDataPointClock.getText() );
                y = str.toi( txtDataPointValue.getText() );
                if ( y < m_min || m_max < y ) {
                    AppManager.showMessageBox( 
                        _( "Invalid value" ), 
                        _( "Error" ), 
                        org.kbinani.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, 
                        org.kbinani.windows.forms.Utility.MSGBOX_ERROR_MESSAGE );
                    return;
                }
                if ( chkEnableSmooth.isSelected() ) {
                    x = str.toi( txtLeftClock.getText() );
                    y = str.toi( txtLeftValue.getText() );
                    x = str.toi( txtRightClock.getText() );
                    y = str.toi( txtRightValue.getText() );
                }
                setDialogResult( BDialogResult.OK );
            } catch ( Exception ex ) {
                AppManager.showMessageBox( _( "Integer format error" ), _( "Error" ), org.kbinani.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, org.kbinani.windows.forms.Utility.MSGBOX_ERROR_MESSAGE );
                setDialogResult( BDialogResult.CANCEL );
                Logger.write( typeof( FormBezierPointEdit ) + ".btnOK_Click; ex=" + ex + "\n" );
            }
        }

        public void chkEnableSmooth_CheckedChanged( Object sender, BEventArgs e )
        {
            boolean value = chkEnableSmooth.isSelected();
            txtLeftClock.setEnabled( value );
            txtLeftValue.setEnabled( value );
            btnLeft.setEnabled( value );
            txtRightClock.setEnabled( value );
            txtRightValue.setEnabled( value );
            btnRight.setEnabled( value );

            boolean old = 
                (m_point.getControlLeftType() != BezierControlType.None) ||
                (m_point.getControlRightType() != BezierControlType.None);
            if ( value ) {
                m_point.setControlLeftType( BezierControlType.Normal );
                m_point.setControlRightType( BezierControlType.Normal );
            } else {
                m_point.setControlLeftType( BezierControlType.None );
                m_point.setControlRightType( BezierControlType.None );
            }
            txtLeftClock.setText( ((int)(m_point.getBase().getX() + m_point.controlLeft.getX())) + "" );
            txtLeftValue.setText( ((int)(m_point.getBase().getY() + m_point.controlLeft.getY())) + "" );
            txtRightClock.setText( ((int)(m_point.getBase().getX() + m_point.controlRight.getX())) + "" );
            txtRightValue.setText( ((int)(m_point.getBase().getY() + m_point.controlRight.getY())) + "" );
            m_parent.invalidate();
        }

        public void handleOperationButtonMouseDown( Object sender, BMouseEventArgs e )
        {
#if DEBUG
            sout.println( "FormBezierPointEdit::handleOperationButtonMouseDown" );
#endif
            BezierPickedSide side = BezierPickedSide.BASE;
            if ( sender == btnLeft ) {
                side = BezierPickedSide.LEFT;
            } else if ( sender == btnRight ) {
                side = BezierPickedSide.RIGHT;
            }

#if !JAVA
            this.Opacity = m_min_opacity;
#endif
            m_last_mouse_global_location = PortUtil.getMousePosition();
            PointD pd = m_point.getPosition( side );
            Point loc_on_trackselector = 
                new Point( 
                    AppManager.xCoordFromClocks( (int)pd.getX() ),
                    m_parent.yCoordFromValue( (int)pd.getY() ) );
            Point loc_topleft = m_parent.getLocationOnScreen();
            mScreenMouseDownLocation =
                new Point(
                    loc_topleft.x + loc_on_trackselector.x,
                    loc_topleft.y + loc_on_trackselector.y );
            PortUtil.setMousePosition( mScreenMouseDownLocation );
            BMouseEventArgs event_arg =
                new BMouseEventArgs(
                    BMouseButtons.Left, 0,
                    loc_on_trackselector.x, loc_on_trackselector.y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = side;
            m_btn_datapoint_downed = true;
        }

        public void common_MouseUp( Object sender, BMouseEventArgs e )
        {
            m_btn_datapoint_downed = false;
#if JAVA
            setVisible( true );
#else
            this.Opacity = 1.0;
#endif
            Point loc_on_screen = PortUtil.getMousePosition();
            Point loc_trackselector = m_parent.getLocationOnScreen();
            Point loc_on_trackselector = 
                new Point( loc_on_screen.x - loc_trackselector.x, loc_on_screen.y - loc_trackselector.y );
            BMouseEventArgs event_arg = 
                new BMouseEventArgs( BMouseButtons.Left, 0, loc_on_trackselector.x, loc_on_trackselector.y, 0 );
            m_parent.TrackSelector_MouseUp( this, event_arg );
            PortUtil.setMousePosition( m_last_mouse_global_location );
            m_parent.invalidate();
        }

        public void common_MouseMove( Object sender, BMouseEventArgs e )
        {
            if ( m_btn_datapoint_downed ) {
                Point loc_on_screen = PortUtil.getMousePosition();

                if ( loc_on_screen.x == mScreenMouseDownLocation.x &&
                    loc_on_screen.y == mScreenMouseDownLocation.y ) {
                    // マウスが動いていないようならbailout
                    return;
                }

                Point loc_trackselector = m_parent.getLocationOnScreen();
                Point loc_on_trackselector = 
                    new Point( loc_on_screen.x - loc_trackselector.x, loc_on_screen.y - loc_trackselector.y );
                BMouseEventArgs event_arg = 
                    new BMouseEventArgs( BMouseButtons.Left, 0, loc_on_trackselector.x, loc_on_trackselector.y, 0 );
                BezierPoint ret = m_parent.HandleMouseMoveForBezierMove( event_arg, m_picked_side );

                txtDataPointClock.setText( ((int)ret.getBase().getX()) + "" );
                txtDataPointValue.setText( ((int)ret.getBase().getY()) + "" );
                txtLeftClock.setText( ((int)ret.getControlLeft().getX()) + "" );
                txtLeftValue.setText( ((int)ret.getControlLeft().getY()) + "" );
                txtRightClock.setText( ((int)ret.getControlRight().getX()) + "" );
                txtRightValue.setText( ((int)ret.getControlRight().getY()) + "" );

                m_parent.invalidate();
            }
        }

        public void handleMoveButtonClick( Object sender, BEventArgs e )
        {
            // イベントの送り主によって動作を変える
            int delta = 1;
            if ( sender == btnBackward ) {
                delta = -1;
            }

            // 選択中のデータ点を検索し，次に選択するデータ点を決める
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id );
            int index = -2;
            int size = target.size();
            for ( int i = 0; i < size; i++ ) {
                if ( target.points.get( i ).getID() == m_point_id ) {
                    index = i + delta;
                    break;
                }
            }

            // 次に選択するデータ点のインデックスが有効範囲なら，選択を実行
            if ( 0 <= index && index < size ) {
                // 選択を実行
                m_point_id = target.points.get( index ).getID();
                m_point = target.points.get( index );
                updateStatus();
                m_parent.mEditingPointID = m_point_id;
                m_parent.invalidate();

                // スクリーン上でデータ点が見えるようにする
                FormMain main = m_parent.getMainForm();
                if ( main != null ) {
                    main.ensureVisible( (int)m_point.getBase().getX() );
                }
            }
        }

        public void btnCancel_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.CANCEL );
        }
        #endregion
    }

#if !JAVA
}
#endif
