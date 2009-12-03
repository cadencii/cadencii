/*
 * FormBezierPointEdit.cs
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
#if JAVA
package org.kbinani.Cadencii;

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\FormBezierPointEdit.java

import java.awt.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.java.awt;
using bocoree.java.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
#endif

#if JAVA
    public class FormBezierPointEdit extends BForm {
#else
    public class FormBezierPointEdit : BForm {
#endif
        private BezierPoint m_point;
        private int m_min;
        private int m_max;
        private Point m_last_mouse_global_location;
        private TrackSelector m_parent;
        private boolean m_btn_datapoint_downed = false;
        private double m_min_opacity = 0.4;
        private CurveType m_curve_type;
        private int m_track;
        private int m_chain_id = -1;
        private int m_point_id = -1;
        private BezierPickedSide m_picked_side = BezierPickedSide.BASE;

        public FormBezierPointEdit( TrackSelector parent,
                                    CurveType curve_type,
                                    int selected_chain_id,
                                    int selected_point_id ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            m_parent = parent;
            m_curve_type = curve_type;
            m_track = AppManager.getSelected();
            m_chain_id = selected_chain_id;
            m_point_id = selected_point_id;
            boolean found = false;
            Vector<BezierChain> chains = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).get( m_curve_type );
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
            for ( Iterator itr = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id ).points.iterator(); itr.hasNext(); ) {
                BezierPoint bp = (BezierPoint)itr.next();
                if ( bp.getID() == m_point_id ) {
                    m_point = bp;
                    smooth = bp.getControlLeftType() != BezierControlType.None || bp.getControlRightType() != BezierControlType.None;
                    break;
                }
            }
            UpdateStatus();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        private void UpdateStatus() {
            txtDataPointClock.setText( m_point.getBase().getX().ToString() );
            txtDataPointValue.setText( m_point.getBase().getY().ToString() );
            txtLeftClock.setText( ((int)(m_point.getBase().getX() + m_point.controlLeft.getX())).ToString() );
            txtLeftValue.setText( ((int)(m_point.getBase().getY() + m_point.controlLeft.getY())).ToString() );
            txtRightClock.setText( ((int)(m_point.getBase().getX() + m_point.controlRight.getX())).ToString() );
            txtRightValue.setText( ((int)(m_point.getBase().getY() + m_point.controlRight.getY())).ToString() );
            boolean smooth = m_point.getControlLeftType() != BezierControlType.None || m_point.getControlRightType() != BezierControlType.None;
            chkEnableSmooth.setSelected( smooth );
            btnLeft.setEnabled( smooth );
            btnRight.setEnabled( smooth );
            m_min = m_curve_type.getMinimum();
            m_max = m_curve_type.getMaximum();
        }

        private static String _( String message ) {
            return Messaging.getMessage( message );
        }

        public void ApplyLanguage() {
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

        private void btnOK_Click( Object sender, BEventArgs e ) {
            try {
                int x, y;
                x = PortUtil.parseInt( txtDataPointClock.getText() );
                y = PortUtil.parseInt( txtDataPointValue.getText() );
                if ( y < m_min || m_max < y ) {
                    AppManager.showMessageBox( _( "Invalid value" ), _( "Error" ), AppManager.MSGBOX_DEFAULT_OPTION, AppManager.MSGBOX_ERROR_MESSAGE );
                    return;
                }
                if ( chkEnableSmooth.isSelected() ) {
                    x = PortUtil.parseInt( txtLeftClock.getText() );
                    y = PortUtil.parseInt( txtLeftValue.getText() );
                    x = PortUtil.parseInt( txtRightClock.getText() );
                    y = PortUtil.parseInt( txtRightValue.getText() );
                }
                setDialogResult( BDialogResult.OK );
            } catch ( Exception ex ) {
                AppManager.showMessageBox( _( "Integer format error" ), _( "Error" ), AppManager.MSGBOX_DEFAULT_OPTION, AppManager.MSGBOX_ERROR_MESSAGE );
                setDialogResult( BDialogResult.CANCEL );
            }
        }

        private void chkEnableSmooth_CheckedChanged( Object sender, BEventArgs e ) {
            boolean value = chkEnableSmooth.isSelected();
            txtLeftClock.setEnabled( value );
            txtLeftValue.setEnabled( value );
            btnLeft.setEnabled( value );
            txtRightClock.setEnabled( value );
            txtRightValue.setEnabled( value );
            btnRight.setEnabled( value );

            boolean old = m_point.getControlLeftType() != BezierControlType.None || m_point.getControlRightType() != BezierControlType.None;
            if ( value ) {
                m_point.setControlLeftType( BezierControlType.Normal );
                m_point.setControlRightType( BezierControlType.Normal );
            } else {
                m_point.setControlLeftType( BezierControlType.None );
                m_point.setControlRightType( BezierControlType.None );
            }
            txtLeftClock.setText( ((int)(m_point.getBase().getX() + m_point.controlLeft.getX())).ToString() );
            txtLeftValue.setText( ((int)(m_point.getBase().getY() + m_point.controlLeft.getY())).ToString() );
            txtRightClock.setText( ((int)(m_point.getBase().getX() + m_point.controlRight.getX())).ToString() );
            txtRightValue.setText( ((int)(m_point.getBase().getY() + m_point.controlRight.getY())).ToString() );
            m_parent.Invalidate();
        }

        private void btnDataPoint_MouseDown( Object sender, BMouseEventArgs e ) {
            this.Opacity = m_min_opacity;
            m_last_mouse_global_location = PortUtil.getMousePosition();
            Point loc_on_trackselector = new Point( AppManager.xCoordFromClocks( (int)m_point.getBase().getX() ),
                                                    m_parent.yCoordFromValue( (int)m_point.getBase().getY() ) );
            Point loc_topleft = m_parent.getLocationOnScreen();
            Point loc_on_screen = new Point( loc_topleft.x + loc_on_trackselector.x, loc_topleft.y + loc_on_trackselector.y );
            PortUtil.setMousePosition( loc_on_screen );
            BMouseEventArgs event_arg = new BMouseEventArgs( BMouseButtons.Left, 0, loc_on_trackselector.x, loc_on_trackselector.y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.BASE;
            m_btn_datapoint_downed = true;
        }

        private void btnLeft_MouseDown( Object sender, BMouseEventArgs e ) {
#if JAVA
            setVisible( false );
#else
            this.Opacity = m_min_opacity;
#endif
            m_last_mouse_global_location = PortUtil.getMousePosition();
            Point loc_on_trackselector = new Point( AppManager.xCoordFromClocks( (int)m_point.getControlLeft().getX() ),
                                                    m_parent.yCoordFromValue( (int)m_point.getControlLeft().getY() ) );
            Point loc_topleft = m_parent.getLocationOnScreen();
            Point loc_on_screen = new Point( loc_on_trackselector.x + loc_topleft.x, loc_on_trackselector.y + loc_topleft.y );
            PortUtil.setMousePosition( loc_on_screen );
            BMouseEventArgs event_arg = new BMouseEventArgs( BMouseButtons.Left, 0, loc_on_trackselector.x, loc_on_trackselector.y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.LEFT;
            m_btn_datapoint_downed = true;
        }

        private void btnRight_MouseDown( Object sender, BMouseEventArgs e ) {
#if JAVA
            setVisible( false );
#else
            this.Opacity = m_min_opacity;
#endif
            m_last_mouse_global_location = PortUtil.getMousePosition();
            Point loc_on_trackselector = new Point( AppManager.xCoordFromClocks( (int)m_point.getControlRight().getX() ),
                                                    m_parent.yCoordFromValue( (int)m_point.getControlRight().getY() ) );
            Point loc_topleft = m_parent.getLocationOnScreen();
            Point loc_on_screen = new Point( loc_on_trackselector.x + loc_topleft.x, loc_on_trackselector.y + loc_topleft.y );
            PortUtil.setMousePosition( loc_on_screen );
            BMouseEventArgs event_arg = new BMouseEventArgs( BMouseButtons.Left, 0, loc_on_trackselector.x, loc_on_trackselector.y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.RIGHT;
            m_btn_datapoint_downed = true;
        }

        private void common_MouseUp( Object sender, BMouseEventArgs e ) {
            m_btn_datapoint_downed = false;
#if JAVA
            setVisible( true );
#else
            this.Opacity = 1.0;
#endif
            Point loc_on_screen = PortUtil.getMousePosition();
            Point loc_trackselector = m_parent.getLocationOnScreen();
            Point loc_on_trackselector = new Point( loc_on_screen.x - loc_trackselector.x, loc_on_screen.y - loc_trackselector.y );
            BMouseEventArgs event_arg = new BMouseEventArgs( BMouseButtons.Left, 0, loc_on_trackselector.x, loc_on_trackselector.y, 0 );
            m_parent.TrackSelector_MouseUp( this, event_arg );
            PortUtil.setMousePosition( m_last_mouse_global_location );
            m_parent.Invalidate();
        }

        private void common_MouseMove( Object sender, BMouseEventArgs e ) {
            if ( m_btn_datapoint_downed ) {
                Point loc_on_screen = PortUtil.getMousePosition();
                Point loc_trackselector = m_parent.getLocationOnScreen();
                Point loc_on_trackselector = new Point( loc_on_screen.x - loc_trackselector.x, loc_on_screen.y - loc_trackselector.y );
                BMouseEventArgs event_arg = new BMouseEventArgs( BMouseButtons.Left, 0, loc_on_trackselector.x, loc_on_trackselector.y, 0 );
                BezierPoint ret = m_parent.HandleMouseMoveForBezierMove( event_arg, m_picked_side );

                txtDataPointClock.setText( ((int)ret.getBase().getX()).ToString() );
                txtDataPointValue.setText( ((int)ret.getBase().getY()).ToString() );
                txtLeftClock.setText( ((int)ret.getControlLeft().getX()).ToString() );
                txtLeftValue.setText( ((int)ret.getControlLeft().getY()).ToString() );
                txtRightClock.setText( ((int)ret.getControlRight().getX()).ToString() );
                txtRightValue.setText( ((int)ret.getControlRight().getY()).ToString() );

                m_parent.Invalidate();
            }
        }

        private void btnBackward_Click( Object sender, BEventArgs e ) {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id );
            int index = -1;
            for ( int i = 0; i < target.size(); i++ ) {
                if ( target.points.get( i ).getID() == m_point_id ) {
                    index = i - 1;
                    break;
                }
            }
            if ( 0 <= index ) {
                m_point_id = target.points.get( index ).getID();
                m_point = target.points.get( index );
                UpdateStatus();
                m_parent.EditingPointID = m_point_id;
                m_parent.Invalidate();
            }
        }

        private void btnForward_Click( Object sender, BEventArgs e ) {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id );
            int index = -2;
            for ( int i = 0; i < target.size(); i++ ) {
                if ( target.points.get( i ).getID() == m_point_id ) {
                    index = i + 1;
                    break;
                }
            }
            if ( 0 <= index && index < target.size() ) {
                m_point_id = target.points.get( index ).getID();
                m_point = target.points.get( index );
                UpdateStatus();
                m_parent.EditingPointID = m_point_id;
                m_parent.Invalidate();
            }
        }

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        private void registerEventHandlers() {
#if JAVA
            this.btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
            this.chkEnableSmooth.checkedChangedEvent.add( new BEventHandler( this, "chkEnableSmooth_CheckedChanged" ) );
            this.btnLeft.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseDown );
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnDataPoint.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnDataPoint.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnDataPoint_MouseDown );
            this.btnDataPoint.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnRight.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseDown );
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnBackward.Click += new System.EventHandler( this.btnBackward_Click );
            this.btnForward.Click += new System.EventHandler( this.btnForward_Click );
#else
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            this.btnCancel.Click += new EventHandler( btnCancel_Click );
            this.chkEnableSmooth.CheckedChanged += new System.EventHandler( this.chkEnableSmooth_CheckedChanged );
            this.btnLeft.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseDown );
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnDataPoint.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnDataPoint.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnDataPoint_MouseDown );
            this.btnDataPoint.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnRight.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseDown );
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnBackward.Click += new System.EventHandler( this.btnBackward_Click );
            this.btnForward.Click += new System.EventHandler( this.btnForward_Click );
#endif
        }

        private void setResources() {
            this.btnLeft.setIcon( new ImageIcon( Resources.get_target__pencil() ) );
            this.btnDataPoint.setIcon( new ImageIcon( Resources.get_target__pencil() ) );
            this.btnRight.setIcon( new ImageIcon( Resources.get_target__pencil() ) );
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormBezierPointEdit.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormBezierPointEdit.java
        #endregion
#else
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.btnCancel = new BButton();
            this.btnOK = new BButton();
            this.chkEnableSmooth = new BCheckBox();
            this.lblLeftValue = new BLabel();
            this.lblLeftClock = new BLabel();
            this.groupLeft = new BGroupBox();
            this.btnLeft = new BButton();
            this.groupDataPoint = new BGroupBox();
            this.btnDataPoint = new BButton();
            this.lblDataPointValue = new BLabel();
            this.lblDataPointClock = new BLabel();
            this.groupRight = new BGroupBox();
            this.btnRight = new BButton();
            this.lblRightValue = new BLabel();
            this.lblRightClock = new BLabel();
            this.btnBackward = new BButton();
            this.btnForward = new BButton();
            this.txtRightClock = new Boare.Cadencii.NumberTextBox();
            this.txtRightValue = new Boare.Cadencii.NumberTextBox();
            this.txtDataPointClock = new Boare.Cadencii.NumberTextBox();
            this.txtDataPointValue = new Boare.Cadencii.NumberTextBox();
            this.txtLeftClock = new Boare.Cadencii.NumberTextBox();
            this.txtLeftValue = new Boare.Cadencii.NumberTextBox();
            this.groupLeft.SuspendLayout();
            this.groupDataPoint.SuspendLayout();
            this.groupRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 374, 163 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 293, 163 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // chkEnableSmooth
            // 
            this.chkEnableSmooth.AutoSize = true;
            this.chkEnableSmooth.Location = new System.Drawing.Point( 196, 12 );
            this.chkEnableSmooth.Name = "chkEnableSmooth";
            this.chkEnableSmooth.Size = new System.Drawing.Size( 62, 16 );
            this.chkEnableSmooth.TabIndex = 3;
            this.chkEnableSmooth.Text = "Smooth";
            this.chkEnableSmooth.UseVisualStyleBackColor = true;
            // 
            // lblLeftValue
            // 
            this.lblLeftValue.AutoSize = true;
            this.lblLeftValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblLeftValue.Name = "lblLeftValue";
            this.lblLeftValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblLeftValue.TabIndex = 16;
            this.lblLeftValue.Text = "Value";
            // 
            // lblLeftClock
            // 
            this.lblLeftClock.AutoSize = true;
            this.lblLeftClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblLeftClock.Name = "lblLeftClock";
            this.lblLeftClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblLeftClock.TabIndex = 15;
            this.lblLeftClock.Text = "Clock";
            // 
            // groupLeft
            // 
            this.groupLeft.Controls.Add( this.btnLeft );
            this.groupLeft.Controls.Add( this.lblLeftValue );
            this.groupLeft.Controls.Add( this.txtLeftClock );
            this.groupLeft.Controls.Add( this.lblLeftClock );
            this.groupLeft.Controls.Add( this.txtLeftValue );
            this.groupLeft.Location = new System.Drawing.Point( 14, 38 );
            this.groupLeft.Name = "groupLeft";
            this.groupLeft.Size = new System.Drawing.Size( 141, 113 );
            this.groupLeft.TabIndex = 17;
            this.groupLeft.TabStop = false;
            this.groupLeft.Text = "Left Control Point";
            // 
            // btnLeft
            // 
            this.btnLeft.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLeft.Location = new System.Drawing.Point( 18, 76 );
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size( 109, 27 );
            this.btnLeft.TabIndex = 18;
            this.btnLeft.UseVisualStyleBackColor = true;
            // 
            // groupDataPoint
            // 
            this.groupDataPoint.Controls.Add( this.btnDataPoint );
            this.groupDataPoint.Controls.Add( this.lblDataPointValue );
            this.groupDataPoint.Controls.Add( this.txtDataPointClock );
            this.groupDataPoint.Controls.Add( this.lblDataPointClock );
            this.groupDataPoint.Controls.Add( this.txtDataPointValue );
            this.groupDataPoint.Location = new System.Drawing.Point( 161, 38 );
            this.groupDataPoint.Name = "groupDataPoint";
            this.groupDataPoint.Size = new System.Drawing.Size( 141, 113 );
            this.groupDataPoint.TabIndex = 18;
            this.groupDataPoint.TabStop = false;
            this.groupDataPoint.Text = "Data Point";
            // 
            // btnDataPoint
            // 
            this.btnDataPoint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDataPoint.Location = new System.Drawing.Point( 18, 76 );
            this.btnDataPoint.Name = "btnDataPoint";
            this.btnDataPoint.Size = new System.Drawing.Size( 109, 27 );
            this.btnDataPoint.TabIndex = 17;
            this.btnDataPoint.UseVisualStyleBackColor = true;
            // 
            // lblDataPointValue
            // 
            this.lblDataPointValue.AutoSize = true;
            this.lblDataPointValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblDataPointValue.Name = "lblDataPointValue";
            this.lblDataPointValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointValue.TabIndex = 16;
            this.lblDataPointValue.Text = "Value";
            // 
            // lblDataPointClock
            // 
            this.lblDataPointClock.AutoSize = true;
            this.lblDataPointClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblDataPointClock.Name = "lblDataPointClock";
            this.lblDataPointClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointClock.TabIndex = 15;
            this.lblDataPointClock.Text = "Clock";
            // 
            // groupRight
            // 
            this.groupRight.Controls.Add( this.btnRight );
            this.groupRight.Controls.Add( this.lblRightValue );
            this.groupRight.Controls.Add( this.txtRightClock );
            this.groupRight.Controls.Add( this.lblRightClock );
            this.groupRight.Controls.Add( this.txtRightValue );
            this.groupRight.Location = new System.Drawing.Point( 308, 38 );
            this.groupRight.Name = "groupRight";
            this.groupRight.Size = new System.Drawing.Size( 141, 113 );
            this.groupRight.TabIndex = 19;
            this.groupRight.TabStop = false;
            this.groupRight.Text = "Right Control Point";
            // 
            // btnRight
            // 
            this.btnRight.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRight.Location = new System.Drawing.Point( 18, 76 );
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size( 109, 27 );
            this.btnRight.TabIndex = 18;
            this.btnRight.UseVisualStyleBackColor = true;
            // 
            // lblRightValue
            // 
            this.lblRightValue.AutoSize = true;
            this.lblRightValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblRightValue.Name = "lblRightValue";
            this.lblRightValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblRightValue.TabIndex = 16;
            this.lblRightValue.Text = "Value";
            // 
            // lblRightClock
            // 
            this.lblRightClock.AutoSize = true;
            this.lblRightClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblRightClock.Name = "lblRightClock";
            this.lblRightClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblRightClock.TabIndex = 15;
            this.lblRightClock.Text = "Clock";
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point( 99, 8 );
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size( 75, 23 );
            this.btnBackward.TabIndex = 20;
            this.btnBackward.Text = "<<";
            this.btnBackward.UseVisualStyleBackColor = true;
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point( 290, 9 );
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size( 75, 23 );
            this.btnForward.TabIndex = 21;
            this.btnForward.Text = ">>";
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // txtRightClock
            // 
            this.txtRightClock.Enabled = false;
            this.txtRightClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtRightClock.Name = "txtRightClock";
            this.txtRightClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtRightClock.TabIndex = 6;
            this.txtRightClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtRightValue
            // 
            this.txtRightValue.Enabled = false;
            this.txtRightValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtRightValue.Name = "txtRightValue";
            this.txtRightValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtRightValue.TabIndex = 7;
            this.txtRightValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDataPointClock
            // 
            this.txtDataPointClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtDataPointClock.Name = "txtDataPointClock";
            this.txtDataPointClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtDataPointClock.TabIndex = 1;
            this.txtDataPointClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDataPointValue
            // 
            this.txtDataPointValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtDataPointValue.Name = "txtDataPointValue";
            this.txtDataPointValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtDataPointValue.TabIndex = 2;
            this.txtDataPointValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtLeftClock
            // 
            this.txtLeftClock.BackColor = System.Drawing.SystemColors.Window;
            this.txtLeftClock.Enabled = false;
            this.txtLeftClock.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLeftClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtLeftClock.Name = "txtLeftClock";
            this.txtLeftClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtLeftClock.TabIndex = 4;
            this.txtLeftClock.Text = "0";
            this.txtLeftClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtLeftValue
            // 
            this.txtLeftValue.BackColor = System.Drawing.SystemColors.Window;
            this.txtLeftValue.Enabled = false;
            this.txtLeftValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLeftValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtLeftValue.Name = "txtLeftValue";
            this.txtLeftValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtLeftValue.TabIndex = 5;
            this.txtLeftValue.Text = "0";
            this.txtLeftValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // FormBezierPointEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 463, 201 );
            this.Controls.Add( this.btnForward );
            this.Controls.Add( this.btnBackward );
            this.Controls.Add( this.groupRight );
            this.Controls.Add( this.groupDataPoint );
            this.Controls.Add( this.groupLeft );
            this.Controls.Add( this.chkEnableSmooth );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBezierPointEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Bezier Data Point";
            this.groupLeft.ResumeLayout( false );
            this.groupLeft.PerformLayout();
            this.groupDataPoint.ResumeLayout( false );
            this.groupDataPoint.PerformLayout();
            this.groupRight.ResumeLayout( false );
            this.groupRight.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BButton btnCancel;
        private BButton btnOK;
        private BCheckBox chkEnableSmooth;
        private BLabel lblLeftValue;
        private BLabel lblLeftClock;
        private NumberTextBox txtLeftValue;
        private NumberTextBox txtLeftClock;
        private BGroupBox groupLeft;
        private BGroupBox groupDataPoint;
        private BLabel lblDataPointValue;
        private NumberTextBox txtDataPointClock;
        private BLabel lblDataPointClock;
        private NumberTextBox txtDataPointValue;
        private BGroupBox groupRight;
        private BLabel lblRightValue;
        private NumberTextBox txtRightClock;
        private BLabel lblRightClock;
        private NumberTextBox txtRightValue;
        private BButton btnDataPoint;
        private BButton btnLeft;
        private BButton btnRight;
        private BButton btnBackward;
        private BButton btnForward;
        #endregion
#endif
    }

#if !JAVA
}
#endif
