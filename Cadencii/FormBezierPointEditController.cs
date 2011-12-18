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
package com.github.cadencii;

//INCLUDE-SECTION IMPORT ./ui/java/FormBezierPointEdit.java

import java.awt.*;
import java.util.*;
import com.github.cadencii.ui.*;
import com.github.cadencii.apputil.*;
import com.github.cadencii.windows.forms.*;
#else
using System;
using com.github.cadencii.apputil;
using com.github.cadencii;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.util;
using com.github.cadencii.windows.forms;

namespace com.github.cadencii
{
    using BEventArgs = System.EventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using BEventHandler = System.EventHandler;
    using BMouseEventHandler = System.Windows.Forms.MouseEventHandler;
    using boolean = System.Boolean;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
#endif

#if JAVA
    public class FormBezierPointEditController extends ControllerBase implements FormBezierPointEditUiListener
#else
    public class FormBezierPointEditController : ControllerBase, FormBezierPointEditUiListener
#endif
    {
        /// <summary>
        /// 右制御点を操作するためのボタンを表す
        /// </summary>
        public const int BUTTON_RIGHT = 1;

        /// <summary>
        /// 左制御点を操作するためのボタンを表す
        /// </summary>
        public const int BUTTON_LEFT = 2;

        /// <summary>
        /// データ点を操作するためのボタンを表す
        /// </summary>
        public const int BUTTON_POINT = 3;

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
            ui = (FormBezierPointEditUi)new FormBezierPointEditUiImpl( this );

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
        }


        #region FormBezierPointEditorUiListener の実装

        public void buttonOkClick()
        {
            try {
                int x, y;
                x = str.toi( this.ui.getDataPointClockText() );
                y = str.toi( this.ui.getDataPointValueText() );
                if( y < this.m_min || this.m_max < y ) {
                    AppManager.showMessageBox(
                        _( "Invalid value" ),
                        _( "Error" ),
                        com.github.cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                        com.github.cadencii.windows.forms.Utility.MSGBOX_ERROR_MESSAGE
                    );
                    return;
                }
                if( ui.isEnableSmoothSelected() ) {
                    x = str.toi( this.ui.getLeftClockText() );
                    y = str.toi( this.ui.getLeftValueText() );
                    x = str.toi( this.ui.getRightClockText() );
                    y = str.toi( this.ui.getRightValueText() );
                }
                this.ui.setDialogResult( BDialogResult.OK );
            } catch( Exception ex ) {
                AppManager.showMessageBox(
                    _( "Integer format error" ),
                    _( "Error" ),
                    com.github.cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    com.github.cadencii.windows.forms.Utility.MSGBOX_ERROR_MESSAGE
                );
                this.ui.setDialogResult( BDialogResult.CANCEL );
                Logger.write( typeof( FormBezierPointEditController ) + ".btnOK_Click; ex=" + ex + "\n" );
            }
        }

        public void buttonCancelClick()
        {
            this.ui.setDialogResult( BDialogResult.CANCEL );
        }

        public void buttonBackwardClick()
        {
            this.handleMoveButtonClick( true );
        }

        public void buttonForwardClick()
        {
            this.handleMoveButtonClick( false );
        }

        public void checkboxEnableSmoothCheckedChanged()
        {
            boolean value = this.ui.isEnableSmoothSelected();
            this.ui.setLeftClockEnabled( value );
            this.ui.setLeftValueEnabled( value );
            this.ui.setLeftButtonEnabled( value );
            this.ui.setRightClockEnabled( value );
            this.ui.setRightValueEnabled( value );
            this.ui.setRightButtonEnabled( value );

            boolean old =
                (m_point.getControlLeftType() != BezierControlType.None) ||
                (m_point.getControlRightType() != BezierControlType.None);
            if( value ) {
                m_point.setControlLeftType( BezierControlType.Normal );
                m_point.setControlRightType( BezierControlType.Normal );
            } else {
                m_point.setControlLeftType( BezierControlType.None );
                m_point.setControlRightType( BezierControlType.None );
            }
            this.ui.setLeftClockText( ((int)(m_point.getBase().getX() + m_point.controlLeft.getX())) + "" );
            this.ui.setLeftValueText( ((int)(m_point.getBase().getY() + m_point.controlLeft.getY())) + "" );
            this.ui.setRightClockText( ((int)(m_point.getBase().getX() + m_point.controlRight.getX())) + "" );
            this.ui.setRightValueText( ((int)(m_point.getBase().getY() + m_point.controlRight.getY())) + "" );
            m_parent.invalidate();
        }

        public void buttonsMouseDown( int buttonType )
        {
            BezierPickedSide side = BezierPickedSide.BASE;
            if( buttonType == FormBezierPointEditController.BUTTON_LEFT ) {
                side = BezierPickedSide.LEFT;
            } else if( buttonType == FormBezierPointEditController.BUTTON_RIGHT ) {
                side = BezierPickedSide.RIGHT;
            }

            this.ui.setOpacity( m_min_opacity );

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

        public void buttonsMouseUp()
        {
            m_btn_datapoint_downed = false;

            this.ui.setOpacity( 1.0 );

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

        public void buttonsMouseMove()
        {
            if( m_btn_datapoint_downed ) {
                Point loc_on_screen = PortUtil.getMousePosition();

                if( loc_on_screen.x == mScreenMouseDownLocation.x &&
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

                this.ui.setDataPointClockText( ((int)ret.getBase().getX()) + "" );
                this.ui.setDataPointValueText( ((int)ret.getBase().getY()) + "" );
                this.ui.setLeftClockText( ((int)ret.getControlLeft().getX()) + "" );
                this.ui.setLeftValueText( ((int)ret.getControlLeft().getY()) + "" );
                this.ui.setRightClockText( ((int)ret.getControlRight().getX()) + "" );
                this.ui.setRightValueText( ((int)ret.getControlRight().getY()) + "" );

                m_parent.invalidate();
            }
        }

        #endregion


        #region public methods

        public void applyLanguage()
        {
            this.ui.setTitle( _( "Edit Bezier Data Point" ) );

            this.ui.setGroupDataPointTitle( _( "Data Poin" ) );
            this.ui.setLabelDataPointClockText( _( "Clock" ) );
            this.ui.setLabelDataPointValueText( _( "Value" ) );

            this.ui.setGroupLeftTitle( _( "Left Control Point" ) );
            this.ui.setLabelLeftClockText( _( "Clock" ) );
            this.ui.setLabelLeftValueText( _( "Value" ) );

            this.ui.setGroupRightTitle( _( "Right Control Point" ) );
            this.ui.setLabelRightClockText( _( "Clock" ) );
            this.ui.setLabelRightValueText( _( "Value" ) );

            this.ui.setCheckboxEnableSmoothText( _( "Smooth" ) );
        }

        public FormBezierPointEditUi getUi()
        {
            return this.ui;
        }

        #endregion

        #region helper methods
        private void updateStatus()
        {
            this.ui.setDataPointClockText( m_point.getBase().getX() + "" );
            this.ui.setDataPointValueText( m_point.getBase().getY() + "" );
            this.ui.setLeftClockText( ((int)(m_point.getBase().getX() + m_point.controlLeft.getX())) + "" );
            this.ui.setLeftValueText( ((int)(m_point.getBase().getY() + m_point.controlLeft.getY())) + "" );
            this.ui.setRightClockText( ((int)(m_point.getBase().getX() + m_point.controlRight.getX())) + "" );
            this.ui.setRightValueText( ((int)(m_point.getBase().getY() + m_point.controlRight.getY())) + "" );
            boolean smooth =
                (m_point.getControlLeftType() != BezierControlType.None) ||
                (m_point.getControlRightType() != BezierControlType.None);
            this.ui.setEnableSmoothSelected( smooth );
            this.ui.setLeftButtonEnabled( smooth );
            this.ui.setRightButtonEnabled( smooth );
            m_min = m_curve_type.getMinimum();
            m_max = m_curve_type.getMaximum();
        }

        private static String _( String message )
        {
            return Messaging.getMessage( message );
        }

        public void handleMoveButtonClick( bool backward )
        {
            // イベントの送り主によって動作を変える
            int delta = 1;
            if( backward ) {
                delta = -1;
            }

            // 選択中のデータ点を検索し，次に選択するデータ点を決める
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id );
            int index = -2;
            int size = target.size();
            for( int i = 0; i < size; i++ ) {
                if( target.points.get( i ).getID() == m_point_id ) {
                    index = i + delta;
                    break;
                }
            }

            // 次に選択するデータ点のインデックスが有効範囲なら，選択を実行
            if( 0 <= index && index < size ) {
                // 選択を実行
                m_point_id = target.points.get( index ).getID();
                m_point = target.points.get( index );
                updateStatus();
                m_parent.mEditingPointID = m_point_id;
                m_parent.invalidate();

                // スクリーン上でデータ点が見えるようにする
                FormMain main = m_parent.getMainForm();
                if( main != null ) {
                    main.ensureVisible( (int)m_point.getBase().getX() );
                }
            }
        }
        #endregion

        #region event handlers

        #endregion
    }

#if !JAVA
}
#endif
