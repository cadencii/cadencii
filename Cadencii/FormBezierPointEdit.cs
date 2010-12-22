/*
 * FormBezierPointEdit.cs
 * Copyright © 2008-2010 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormBezierPointEdit.java

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
    using boolean = System.Boolean;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
#endif

#if JAVA
    public class FormBezierPointEdit extends BDialog {
#else
    public class FormBezierPointEdit : BDialog
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

        public FormBezierPointEdit( TrackSelector parent,
                                    CurveType curve_type,
                                    int selected_chain_id,
                                    int selected_point_id )
        {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
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
            btnOK.Click += new EventHandler( btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
            chkEnableSmooth.CheckedChanged += new EventHandler( chkEnableSmooth_CheckedChanged );
            btnLeft.MouseMove += new System.Windows.Forms.MouseEventHandler( common_MouseMove );
            btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler( handleOperationButtonMouseDown );
            btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler( common_MouseUp );
            btnDataPoint.MouseMove += new System.Windows.Forms.MouseEventHandler( common_MouseMove );
            btnDataPoint.MouseDown += new System.Windows.Forms.MouseEventHandler( handleOperationButtonMouseDown );
            btnDataPoint.MouseUp += new System.Windows.Forms.MouseEventHandler( common_MouseUp );
            btnRight.MouseMove += new System.Windows.Forms.MouseEventHandler( common_MouseMove );
            btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler( handleOperationButtonMouseDown );
            btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler( common_MouseUp );
            btnBackward.Click += new EventHandler( handleMoveButtonClick );
            btnForward.Click += new EventHandler( handleMoveButtonClick );
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
                x = PortUtil.parseInt( txtDataPointClock.getText() );
                y = PortUtil.parseInt( txtDataPointValue.getText() );
                if ( y < m_min || m_max < y ) {
                    AppManager.showMessageBox( 
                        _( "Invalid value" ), 
                        _( "Error" ), 
                        org.kbinani.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, 
                        org.kbinani.windows.forms.Utility.MSGBOX_ERROR_MESSAGE );
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

        public void handleOperationButtonMouseDown( object sender, BMouseEventArgs e )
        {
#if DEBUG
            PortUtil.println( "FormBezierPointEdit::handleOperationButtonMouseDown" );
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

        public void handleMoveButtonClick( object sender, BEventArgs e )
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

        #region UI implementation
#if JAVA
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormBezierPointEdit.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormBezierPointEdit.java
#else
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing )
        {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.chkEnableSmooth = new org.kbinani.windows.forms.BCheckBox();
            this.lblLeftValue = new org.kbinani.windows.forms.BLabel();
            this.lblLeftClock = new org.kbinani.windows.forms.BLabel();
            this.groupLeft = new org.kbinani.windows.forms.BGroupBox();
            this.btnLeft = new org.kbinani.windows.forms.BButton();
            this.txtLeftClock = new org.kbinani.cadencii.NumberTextBox();
            this.txtLeftValue = new org.kbinani.cadencii.NumberTextBox();
            this.groupDataPoint = new org.kbinani.windows.forms.BGroupBox();
            this.btnDataPoint = new org.kbinani.windows.forms.BButton();
            this.lblDataPointValue = new org.kbinani.windows.forms.BLabel();
            this.txtDataPointClock = new org.kbinani.cadencii.NumberTextBox();
            this.lblDataPointClock = new org.kbinani.windows.forms.BLabel();
            this.txtDataPointValue = new org.kbinani.cadencii.NumberTextBox();
            this.groupRight = new org.kbinani.windows.forms.BGroupBox();
            this.btnRight = new org.kbinani.windows.forms.BButton();
            this.lblRightValue = new org.kbinani.windows.forms.BLabel();
            this.txtRightClock = new org.kbinani.cadencii.NumberTextBox();
            this.lblRightClock = new org.kbinani.windows.forms.BLabel();
            this.txtRightValue = new org.kbinani.cadencii.NumberTextBox();
            this.btnBackward = new org.kbinani.windows.forms.BButton();
            this.btnForward = new org.kbinani.windows.forms.BButton();
            this.groupLeft.SuspendLayout();
            this.groupDataPoint.SuspendLayout();
            this.groupRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 374, 170 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSize = true;
            this.btnOK.Location = new System.Drawing.Point( 293, 170 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // chkEnableSmooth
            // 
            this.chkEnableSmooth.AutoSize = true;
            this.chkEnableSmooth.Location = new System.Drawing.Point( 196, 12 );
            this.chkEnableSmooth.Name = "chkEnableSmooth";
            this.chkEnableSmooth.Size = new System.Drawing.Size( 62, 16 );
            this.chkEnableSmooth.TabIndex = 2;
            this.chkEnableSmooth.Text = "Smooth";
            this.chkEnableSmooth.UseVisualStyleBackColor = true;
            // 
            // lblLeftValue
            // 
            this.lblLeftValue.AutoSize = true;
            this.lblLeftValue.Location = new System.Drawing.Point( 12, 54 );
            this.lblLeftValue.Name = "lblLeftValue";
            this.lblLeftValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblLeftValue.TabIndex = 16;
            this.lblLeftValue.Text = "Value";
            // 
            // lblLeftClock
            // 
            this.lblLeftClock.AutoSize = true;
            this.lblLeftClock.Location = new System.Drawing.Point( 12, 29 );
            this.lblLeftClock.Name = "lblLeftClock";
            this.lblLeftClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblLeftClock.TabIndex = 15;
            this.lblLeftClock.Text = "Clock";
            // 
            // groupLeft
            // 
            this.groupLeft.AutoSize = true;
            this.groupLeft.Controls.Add( this.btnLeft );
            this.groupLeft.Controls.Add( this.lblLeftValue );
            this.groupLeft.Controls.Add( this.txtLeftClock );
            this.groupLeft.Controls.Add( this.lblLeftClock );
            this.groupLeft.Controls.Add( this.txtLeftValue );
            this.groupLeft.Location = new System.Drawing.Point( 14, 38 );
            this.groupLeft.Name = "groupLeft";
            this.groupLeft.Size = new System.Drawing.Size( 141, 121 );
            this.groupLeft.TabIndex = 17;
            this.groupLeft.TabStop = false;
            this.groupLeft.Text = "Left Control Point";
            // 
            // btnLeft
            // 
            this.btnLeft.AutoSize = true;
            this.btnLeft.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLeft.Location = new System.Drawing.Point( 14, 76 );
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size( 113, 27 );
            this.btnLeft.TabIndex = 6;
            this.btnLeft.UseVisualStyleBackColor = true;
            // 
            // txtLeftClock
            // 
            this.txtLeftClock.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))) );
            this.txtLeftClock.Enabled = false;
            this.txtLeftClock.ForeColor = System.Drawing.Color.Black;
            this.txtLeftClock.Location = new System.Drawing.Point( 66, 26 );
            this.txtLeftClock.Name = "txtLeftClock";
            this.txtLeftClock.Size = new System.Drawing.Size( 61, 19 );
            this.txtLeftClock.TabIndex = 4;
            this.txtLeftClock.Text = "0";
            this.txtLeftClock.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtLeftValue
            // 
            this.txtLeftValue.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))) );
            this.txtLeftValue.Enabled = false;
            this.txtLeftValue.ForeColor = System.Drawing.Color.Black;
            this.txtLeftValue.Location = new System.Drawing.Point( 66, 51 );
            this.txtLeftValue.Name = "txtLeftValue";
            this.txtLeftValue.Size = new System.Drawing.Size( 61, 19 );
            this.txtLeftValue.TabIndex = 5;
            this.txtLeftValue.Text = "0";
            this.txtLeftValue.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // groupDataPoint
            // 
            this.groupDataPoint.AutoSize = true;
            this.groupDataPoint.Controls.Add( this.btnDataPoint );
            this.groupDataPoint.Controls.Add( this.lblDataPointValue );
            this.groupDataPoint.Controls.Add( this.txtDataPointClock );
            this.groupDataPoint.Controls.Add( this.lblDataPointClock );
            this.groupDataPoint.Controls.Add( this.txtDataPointValue );
            this.groupDataPoint.Location = new System.Drawing.Point( 161, 38 );
            this.groupDataPoint.Name = "groupDataPoint";
            this.groupDataPoint.Size = new System.Drawing.Size( 141, 121 );
            this.groupDataPoint.TabIndex = 18;
            this.groupDataPoint.TabStop = false;
            this.groupDataPoint.Text = "Data Point";
            // 
            // btnDataPoint
            // 
            this.btnDataPoint.AutoSize = true;
            this.btnDataPoint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDataPoint.Location = new System.Drawing.Point( 14, 76 );
            this.btnDataPoint.Name = "btnDataPoint";
            this.btnDataPoint.Size = new System.Drawing.Size( 113, 27 );
            this.btnDataPoint.TabIndex = 9;
            this.btnDataPoint.UseVisualStyleBackColor = true;
            // 
            // lblDataPointValue
            // 
            this.lblDataPointValue.AutoSize = true;
            this.lblDataPointValue.Location = new System.Drawing.Point( 12, 54 );
            this.lblDataPointValue.Name = "lblDataPointValue";
            this.lblDataPointValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointValue.TabIndex = 16;
            this.lblDataPointValue.Text = "Value";
            // 
            // txtDataPointClock
            // 
            this.txtDataPointClock.Location = new System.Drawing.Point( 66, 26 );
            this.txtDataPointClock.Name = "txtDataPointClock";
            this.txtDataPointClock.Size = new System.Drawing.Size( 61, 19 );
            this.txtDataPointClock.TabIndex = 7;
            this.txtDataPointClock.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // lblDataPointClock
            // 
            this.lblDataPointClock.AutoSize = true;
            this.lblDataPointClock.Location = new System.Drawing.Point( 12, 29 );
            this.lblDataPointClock.Name = "lblDataPointClock";
            this.lblDataPointClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointClock.TabIndex = 15;
            this.lblDataPointClock.Text = "Clock";
            // 
            // txtDataPointValue
            // 
            this.txtDataPointValue.Location = new System.Drawing.Point( 66, 51 );
            this.txtDataPointValue.Name = "txtDataPointValue";
            this.txtDataPointValue.Size = new System.Drawing.Size( 61, 19 );
            this.txtDataPointValue.TabIndex = 8;
            this.txtDataPointValue.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // groupRight
            // 
            this.groupRight.AutoSize = true;
            this.groupRight.Controls.Add( this.btnRight );
            this.groupRight.Controls.Add( this.lblRightValue );
            this.groupRight.Controls.Add( this.txtRightClock );
            this.groupRight.Controls.Add( this.lblRightClock );
            this.groupRight.Controls.Add( this.txtRightValue );
            this.groupRight.Location = new System.Drawing.Point( 308, 38 );
            this.groupRight.Name = "groupRight";
            this.groupRight.Size = new System.Drawing.Size( 141, 121 );
            this.groupRight.TabIndex = 19;
            this.groupRight.TabStop = false;
            this.groupRight.Text = "Right Control Point";
            // 
            // btnRight
            // 
            this.btnRight.AutoSize = true;
            this.btnRight.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRight.Location = new System.Drawing.Point( 14, 76 );
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size( 113, 27 );
            this.btnRight.TabIndex = 12;
            this.btnRight.UseVisualStyleBackColor = true;
            // 
            // lblRightValue
            // 
            this.lblRightValue.AutoSize = true;
            this.lblRightValue.Location = new System.Drawing.Point( 12, 54 );
            this.lblRightValue.Name = "lblRightValue";
            this.lblRightValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblRightValue.TabIndex = 16;
            this.lblRightValue.Text = "Value";
            // 
            // txtRightClock
            // 
            this.txtRightClock.Enabled = false;
            this.txtRightClock.Location = new System.Drawing.Point( 66, 26 );
            this.txtRightClock.Name = "txtRightClock";
            this.txtRightClock.Size = new System.Drawing.Size( 61, 19 );
            this.txtRightClock.TabIndex = 10;
            this.txtRightClock.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // lblRightClock
            // 
            this.lblRightClock.AutoSize = true;
            this.lblRightClock.Location = new System.Drawing.Point( 12, 29 );
            this.lblRightClock.Name = "lblRightClock";
            this.lblRightClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblRightClock.TabIndex = 15;
            this.lblRightClock.Text = "Clock";
            // 
            // txtRightValue
            // 
            this.txtRightValue.Enabled = false;
            this.txtRightValue.Location = new System.Drawing.Point( 66, 51 );
            this.txtRightValue.Name = "txtRightValue";
            this.txtRightValue.Size = new System.Drawing.Size( 61, 19 );
            this.txtRightValue.TabIndex = 11;
            this.txtRightValue.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // btnBackward
            // 
            this.btnBackward.AutoSize = true;
            this.btnBackward.Location = new System.Drawing.Point( 99, 8 );
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size( 75, 23 );
            this.btnBackward.TabIndex = 1;
            this.btnBackward.Text = "<<";
            this.btnBackward.UseVisualStyleBackColor = true;
            // 
            // btnForward
            // 
            this.btnForward.AutoSize = true;
            this.btnForward.Location = new System.Drawing.Point( 290, 9 );
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size( 75, 23 );
            this.btnForward.TabIndex = 3;
            this.btnForward.Text = ">>";
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // FormBezierPointEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 96F, 96F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 463, 208 );
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

#endif
        #endregion
    }

#if !JAVA
}
#endif
