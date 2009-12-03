/*
 * FormCurvePointEdit.cs
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

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\FormCurvePointEdit.java
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormCurvePointEdit extends BForm {
#else
    public class FormCurvePointEdit : BForm {
#endif
        long m_editing_id = -1;
        CurveType m_curve;
        boolean m_changed = false;

        public FormCurvePointEdit( long editing_id, CurveType curve ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            applyLanguage();
            m_editing_id = editing_id;
            m_curve = curve;

            VsqBPPairSearchContext context = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_curve.getName() ).findElement( m_editing_id );
            txtDataPointClock.setText( context.clock + "" );
            txtDataPointValue.setText( context.point.value + "" );
            txtDataPointValue.selectAll();

            btnUndo.setEnabled( AppManager.isUndoAvailable() );
            btnRedo.setEnabled( AppManager.isRedoAvailable() );
        }

        private String _( String id ) {
            return Messaging.getMessage( id );
        }

        public void applyLanguage() {
            setTitle( _( "Edit Value" ) );
            lblDataPointClock.setText( _( "Clock" ) );
            lblDataPointValue.setText( _( "Value" ) );
            btnApply.setText( _( "Apply" ) );
            btnExit.setText( _( "Exit" ) );
        }

        private void applyValue( boolean mode_clock ) {
            if ( !m_changed ) {
                return;
            }
            int value = m_curve.getDefault();
            try {
                value = PortUtil.parseInt( txtDataPointValue.getText() );
            } catch ( Exception ex ) {
                return;
            }
            if ( value < m_curve.getMinimum() ) {
                value = m_curve.getMinimum();
            } else if ( m_curve.getMaximum() < value ) {
                value = m_curve.getMaximum();
            }

            int clock = 0;
            try {
                clock = PortUtil.parseInt( txtDataPointClock.getText() );
            } catch ( Exception ex ) {
                return;
            }

            int track_num = AppManager.getSelected();
            VsqBPList list = (VsqBPList)AppManager.getVsqFile().Track.get( track_num ).getCurve( m_curve.getName() ).clone();

            VsqBPPairSearchContext context = list.findElement( m_editing_id );
            list.move( context.clock, clock, value );
            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandTrackCurveReplace( track_num,
                                                                                                    m_curve.getName(),
                                                                                                    list ) );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );

            txtDataPointClock.setText( clock + "" );
            txtDataPointValue.setText( value + "" );

            if ( AppManager.mainWindow != null ) {
                AppManager.mainWindow.setEdited( true );
                AppManager.mainWindow.ensureVisible( clock );
                AppManager.mainWindow.refreshScreen();
            }

            if ( mode_clock ) {
                txtDataPointClock.selectAll();
            } else {
                txtDataPointValue.selectAll();
            }

            btnUndo.setEnabled( AppManager.isUndoAvailable() );
            btnRedo.setEnabled( AppManager.isRedoAvailable() );
            m_changed = false;
        }

        private void commonTextBox_KeyUp( Object sender, BKeyEventArgs e ) {
#if JAVA
            if ( (e.KeyValue & KeyEvent.VK_ENTER) != KeyEvent.VK_ENTER ) {
#else
            if ( (e.KeyCode & System.Windows.Forms.Keys.Enter) != System.Windows.Forms.Keys.Enter ) {
#endif
                return;
            }
            applyValue( (sender == txtDataPointClock) );
        }

        private void commonButton_Click( Object sender, BEventArgs e ) {
            VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_curve.getName() );
            VsqBPPairSearchContext search = list.findElement( m_editing_id );
            int index = search.index;
            if ( sender == btnForward ) {
                index++;
            } else if ( sender == btnBackward ) {
                index--;
            } else if ( sender == btnBackward2 ) {
                index -= 5;
            } else if ( sender == btnForward2 ) {
                index += 5;
            } else if ( sender == btnForward3 ) {
                index += 10;
            } else if ( sender == btnBackward3 ) {
                index -= 10;
            }

            if ( index < 0 ) {
                index = 0;
            }

            if ( list.size() <= index ) {
                index = list.size() - 1;
            }

            VsqBPPair bp = list.getElementB( index );
            m_editing_id = bp.id;
            int clock = list.getKeyClock( index );
#if JAVA
            txtDataPointClock.textChangedEvent.remove( new BEventHandler( this, "commonTextBox_TextChanged" ) );
            txtDataPointValue.textChangedEvent.remove( new BEventHandler( this, "commonTextBox_TextChanged" ) );
#else
            txtDataPointClock.TextChanged -= commonTextBox_TextChanged;
            txtDataPointValue.TextChanged -= commonTextBox_TextChanged;
#endif
            txtDataPointClock.setText( clock + "" );
            txtDataPointValue.setText( bp.value + "" );
#if JAVA
            txtDataPointClock.textChangedEvent.add( new BEventHandler( this, "commonTextBox_TextChanged" ) );
            txtDataPointValue.textChangedEvent.add( new BEventHandler( this, "commonTextBox_TextChanged" ) );
#else
            txtDataPointClock.TextChanged += commonTextBox_TextChanged;
            txtDataPointValue.TextChanged += commonTextBox_TextChanged;
#endif

            txtDataPointValue.requestFocus();
            txtDataPointValue.selectAll();

            AppManager.clearSelectedPoint();
            AppManager.addSelectedPoint( m_curve, bp.id );
            if ( AppManager.mainWindow != null ) {
                AppManager.mainWindow.ensureVisible( clock );
                AppManager.mainWindow.refreshScreen();
            }
        }

        private void btnApply_Click( Object sender, BEventArgs e ) {
            applyValue( true );
        }

        private void commonTextBox_TextChanged( Object sender, BEventArgs e ) {
            m_changed = true;
        }

        private void handleUndoRedo_Click( Object sender, BEventArgs e ) {
            if ( sender == btnUndo ) {
                AppManager.undo();
            } else if ( sender == btnRedo ) {
                AppManager.redo();
            } else {
                return;
            }
            VsqFileEx vsq = AppManager.getVsqFile();
            boolean exists = false;
            if ( vsq != null ) {
                exists = vsq.Track.get( AppManager.getSelected() ).getCurve( m_curve.getName() ).findElement( m_editing_id ).index >= 0;
            }
#if DEBUG
            Console.WriteLine( "FormCurvePointEdit#handleUndoRedo_Click; exists=" + exists );
#endif
            txtDataPointClock.setEnabled( exists );
            txtDataPointValue.setEnabled( exists );
            btnApply.setEnabled( exists );
            btnBackward.setEnabled( exists );
            btnBackward2.setEnabled( exists );
            btnBackward3.setEnabled( exists );
            btnForward.setEnabled( exists );
            btnForward2.setEnabled( exists );
            btnForward3.setEnabled( exists );

            if ( exists ) {
                AppManager.clearSelectedPoint();
                AppManager.addSelectedPoint( m_curve, m_editing_id );
            }

            if ( AppManager.mainWindow != null ) {
                AppManager.mainWindow.updateDrawObjectList();
                AppManager.mainWindow.refreshScreen();
            }
            btnUndo.setEnabled( AppManager.isUndoAvailable() );
            btnRedo.setEnabled( AppManager.isRedoAvailable() );
        }

        private void btnExit_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        private void setResources() {
        }

        private void registerEventHandlers() {
#if JAVA
            this.btnForward.clickEvent.add( new BEventHandler( this, "commonButton_Click" ) );
            this.btnBackward.clickEvent.add( new BEventHandler( this, "commonButton_Click" ) );
            this.btnBackward2.clickEvent.add( new BEventHandler( this, "commonButton_Click" ) );
            this.btnForward2.clickEvent.add( new BEventHandler( this, "commonButton_Click" ) );
            this.btnApply.clickEvent.add( new BEventHandler( this, "btnApply_Click" ) );
            this.txtDataPointClock.textChangedEvent.add( new BEventHandler( this, "commonTextBox_TextChanged" ) );
            this.txtDataPointClock.keyUpEvent.add( new BKeyEventHandler( this, "commonTextBox_KeyUp" ) );
            this.txtDataPointValue.textChangedEvent.add( new BEventHandler( this, "commonTextBox_TextChanged" ) );
            this.txtDataPointValue.keyUpEvent.add( new BKeyEventHandler( this, "commonTextBox_KeyUp" ) );
            this.btnBackward3.clickEvent.add( new BEventHandler( this, "commonButton_Click" ) );
            this.btnForward3.clickEvent.add( new BEventHandler( this, "commonButton_Click" ) );
            this.btnUndo.clickEvent.add( new BEventHandler( this, "handleUndoRedo_Click" ) );
            this.btnRedo.clickEvent.add( new BEventHandler( this, "handleUndoRedo_Click" ) );
#else
            this.btnForward.Click += new System.EventHandler( this.commonButton_Click );
            this.btnBackward.Click += new System.EventHandler( this.commonButton_Click );
            this.btnBackward2.Click += new System.EventHandler( this.commonButton_Click );
            this.btnForward2.Click += new System.EventHandler( this.commonButton_Click );
            this.btnApply.Click += new System.EventHandler( this.btnApply_Click );
            this.txtDataPointClock.TextChanged += new System.EventHandler( this.commonTextBox_TextChanged );
            this.txtDataPointClock.KeyUp += new System.Windows.Forms.KeyEventHandler( this.commonTextBox_KeyUp );
            this.txtDataPointValue.TextChanged += new System.EventHandler( this.commonTextBox_TextChanged );
            this.txtDataPointValue.KeyUp += new System.Windows.Forms.KeyEventHandler( this.commonTextBox_KeyUp );
            this.btnBackward3.Click += new System.EventHandler( this.commonButton_Click );
            this.btnForward3.Click += new System.EventHandler( this.commonButton_Click );
            this.btnUndo.Click += new System.EventHandler( this.handleUndoRedo_Click );
            this.btnRedo.Click += new System.EventHandler( this.handleUndoRedo_Click );
            btnExit.Click += new EventHandler( btnExit_Click );
#endif
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormCurvePointEdit.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormCurvePointEdit.java
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
        protected override void Dispose( bool disposing ) {
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
            this.btnForward = new bocoree.windows.forms.BButton();
            this.btnBackward = new bocoree.windows.forms.BButton();
            this.lblDataPointValue = new bocoree.windows.forms.BLabel();
            this.lblDataPointClock = new bocoree.windows.forms.BLabel();
            this.btnExit = new bocoree.windows.forms.BButton();
            this.btnBackward2 = new bocoree.windows.forms.BButton();
            this.btnForward2 = new bocoree.windows.forms.BButton();
            this.btnApply = new bocoree.windows.forms.BButton();
            this.txtDataPointClock = new Boare.Cadencii.NumberTextBox();
            this.txtDataPointValue = new Boare.Cadencii.NumberTextBox();
            this.btnBackward3 = new bocoree.windows.forms.BButton();
            this.btnForward3 = new bocoree.windows.forms.BButton();
            this.btnUndo = new bocoree.windows.forms.BButton();
            this.btnRedo = new bocoree.windows.forms.BButton();
            this.SuspendLayout();
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point( 120, 12 );
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size( 35, 30 );
            this.btnForward.TabIndex = 6;
            this.btnForward.Text = ">";
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point( 84, 12 );
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size( 35, 30 );
            this.btnBackward.TabIndex = 5;
            this.btnBackward.Text = "<";
            this.btnBackward.UseVisualStyleBackColor = true;
            // 
            // lblDataPointValue
            // 
            this.lblDataPointValue.AutoSize = true;
            this.lblDataPointValue.Location = new System.Drawing.Point( 49, 55 );
            this.lblDataPointValue.Name = "lblDataPointValue";
            this.lblDataPointValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointValue.TabIndex = 16;
            this.lblDataPointValue.Text = "Value";
            // 
            // lblDataPointClock
            // 
            this.lblDataPointClock.AutoSize = true;
            this.lblDataPointClock.Location = new System.Drawing.Point( 49, 80 );
            this.lblDataPointClock.Name = "lblDataPointClock";
            this.lblDataPointClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointClock.TabIndex = 15;
            this.lblDataPointClock.Text = "Clock";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point( 152, 109 );
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size( 75, 23 );
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnBackward2
            // 
            this.btnBackward2.Location = new System.Drawing.Point( 48, 12 );
            this.btnBackward2.Name = "btnBackward2";
            this.btnBackward2.Size = new System.Drawing.Size( 35, 30 );
            this.btnBackward2.TabIndex = 4;
            this.btnBackward2.Text = "<5";
            this.btnBackward2.UseVisualStyleBackColor = true;
            // 
            // btnForward2
            // 
            this.btnForward2.Location = new System.Drawing.Point( 156, 12 );
            this.btnForward2.Name = "btnForward2";
            this.btnForward2.Size = new System.Drawing.Size( 35, 30 );
            this.btnForward2.TabIndex = 7;
            this.btnForward2.Text = "5>";
            this.btnForward2.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point( 71, 109 );
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size( 75, 23 );
            this.btnApply.TabIndex = 17;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // txtDataPointClock
            // 
            this.txtDataPointClock.Location = new System.Drawing.Point( 89, 77 );
            this.txtDataPointClock.Name = "txtDataPointClock";
            this.txtDataPointClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtDataPointClock.TabIndex = 2;
            this.txtDataPointClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDataPointValue
            // 
            this.txtDataPointValue.Location = new System.Drawing.Point( 89, 52 );
            this.txtDataPointValue.Name = "txtDataPointValue";
            this.txtDataPointValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtDataPointValue.TabIndex = 1;
            this.txtDataPointValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // btnBackward3
            // 
            this.btnBackward3.Location = new System.Drawing.Point( 12, 12 );
            this.btnBackward3.Name = "btnBackward3";
            this.btnBackward3.Size = new System.Drawing.Size( 35, 30 );
            this.btnBackward3.TabIndex = 18;
            this.btnBackward3.Text = "<10";
            this.btnBackward3.UseVisualStyleBackColor = true;
            // 
            // btnForward3
            // 
            this.btnForward3.Location = new System.Drawing.Point( 192, 12 );
            this.btnForward3.Name = "btnForward3";
            this.btnForward3.Size = new System.Drawing.Size( 35, 30 );
            this.btnForward3.TabIndex = 19;
            this.btnForward3.Text = "10>";
            this.btnForward3.UseVisualStyleBackColor = true;
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point( 178, 50 );
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size( 49, 23 );
            this.btnUndo.TabIndex = 20;
            this.btnUndo.Text = "undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnRedo
            // 
            this.btnRedo.Location = new System.Drawing.Point( 178, 75 );
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size( 49, 23 );
            this.btnRedo.TabIndex = 21;
            this.btnRedo.Text = "redo";
            this.btnRedo.UseVisualStyleBackColor = true;
            // 
            // FormCurvePointEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size( 239, 144 );
            this.Controls.Add( this.btnRedo );
            this.Controls.Add( this.btnUndo );
            this.Controls.Add( this.btnForward3 );
            this.Controls.Add( this.btnBackward3 );
            this.Controls.Add( this.btnApply );
            this.Controls.Add( this.btnForward2 );
            this.Controls.Add( this.btnBackward2 );
            this.Controls.Add( this.btnExit );
            this.Controls.Add( this.lblDataPointValue );
            this.Controls.Add( this.btnForward );
            this.Controls.Add( this.txtDataPointClock );
            this.Controls.Add( this.btnBackward );
            this.Controls.Add( this.lblDataPointClock );
            this.Controls.Add( this.txtDataPointValue );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCurvePointEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormCurvePointEdit";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BButton btnForward;
        private BButton btnBackward;
        private BLabel lblDataPointValue;
        private NumberTextBox txtDataPointClock;
        private BLabel lblDataPointClock;
        private NumberTextBox txtDataPointValue;
        private BButton btnExit;
        private BButton btnBackward2;
        private BButton btnForward2;
        private BButton btnApply;
        private BButton btnBackward3;
        private BButton btnForward3;
        private BButton btnUndo;
        private BButton btnRedo;
        #endregion
#endif
    }

#if !JAVA
}
#endif
