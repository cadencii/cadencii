/*
 * FormGameControlerConfig.cs
 * Copyright (C) 2009 kbinani
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

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\FormGameControlerConfig.java

import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BEventArgs = EventArgs;
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class FormGameControlerConfig extends BForm {
#else
    public class FormGameControlerConfig : BForm {
#endif
        private Vector<Integer> m_list = new Vector<Integer>();
        private Vector<Integer> m_povs = new Vector<Integer>();
        private int index;
        private BTimer timer;

        public FormGameControlerConfig() {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif

#if JAVA
            timer = new BTimer();
#else
            timer = new BTimer( this.components );
#endif
            registerEventHandlers();
            setResources();
            for ( int i = 0; i < 10; i++ ) {
                m_list.add( -1 );
            }
            for ( int i = 0; i < 4; i++ ) {
                m_povs.add( int.MinValue );
            }
            ApplyLanguage();
#if JAVA
            int num_dev = 0;
#else
            int num_dev = winmmhelp.JoyGetNumJoyDev();
#endif
            if ( num_dev > 0 ) {
                pictButton.setImage( Resources.get_btn1() );
                progressCount.setMaximum( 8 );
                progressCount.setMinimum( 0 );
                progressCount.setValue( 0 );
                index = 1;
                btnSkip.setEnabled( true );
                btnReset.setEnabled( true );
                timer.start();
            } else {
                btnSkip.setEnabled( false );
                btnReset.setEnabled( false );
            }
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
#if JAVA
            int num_dev = 0;
#else
            int num_dev = winmmhelp.JoyGetNumJoyDev();
#endif
            if ( num_dev > 0 ) {
                lblMessage.setText( _( "Push buttons in turn as shown below" ) );
            } else {
                lblMessage.setText( _( "Game controler is not available" ) );
            }
            setTitle( _( "Game Controler Configuration" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            btnReset.setText( _( "Reset And Exit" ) );
            btnSkip.setText( _( "Skip" ) );
        }

        private void timer_Tick( Object sender, BEventArgs e ) {
            //int num_btn = vstidrv.JoyGetNumButtons( 0 );
            byte[] btn;
            int pov;
#if JAVA
            pov = -1;
            btn = new byte[]{};
#else
            winmmhelp.JoyGetStatus( 0, out btn, out pov );
#endif

#if DEBUG
            AppManager.debugWriteLine( "FormGameControlerConfig+timer_Tick" );
            AppManager.debugWriteLine( "    pov=" + pov );
#endif
            boolean added = false;
            if ( index <= 4 ) {
                if ( pov >= 0 && !m_povs.contains( pov ) ) {
                    m_povs.set( index - 1, pov );
                    added = true;
                }
            } else {
                for ( int i = 0; i < btn.Length; i++ ) {
                    if ( btn[i] > 0x0 && !m_list.contains( i ) ) {
                        m_list.set( index - 5, i );
                        added = true;
                        break;
                    }
                }
            }
            if ( added ) {
                if ( index <= 8 ) {
                    progressCount.setValue( index );
                } else if ( index <= 12 ) {
                    progressCount.setValue( index - 8 );
                } else {
                    progressCount.setValue( index - 12 );
                }

                if ( index == 8 ) {
                    pictButton.setImage( Resources.get_btn2() );
                    progressCount.setValue( 0 );
                    progressCount.setMaximum( 4 );
                } else if ( index == 12 ) {
                    pictButton.setImage( Resources.get_btn3() );
                    progressCount.setValue( 0 );
                    progressCount.setMaximum( 2 );
                }
                if ( index == 14 ) {
                    btnSkip.setEnabled( false );
                    btnOK.setEnabled( true );
                    timer.stop();
                }
                index++;
            }
        }

        public int getRectangle() {
            return m_list.get( 0 );
        }

        public int getTriangle() {
            return m_list.get( 1 );
        }

        public int getCircle() {
            return m_list.get( 2 );
        }

        public int getCross() {
            return m_list.get( 3 );
        }

        public int getL1() {
            return m_list.get( 4 );
        }

        public int getL2() {
            return m_list.get( 5 );
        }

        public int getR1() {
            return m_list.get( 6 );
        }

        public int getR2() {
            return m_list.get( 7 );
        }

        public int getSelect() {
            return m_list.get( 8 );
        }

        public int getStart() {
            return m_list.get( 9 );
        }

        public int getPovDown() {
            return m_povs.get( 0 );
        }

        public int getPovLeft() {
            return m_povs.get( 1 );
        }

        public int getPovUp() {
            return m_povs.get( 2 );
        }

        public int getPovRight() {
            return m_povs.get( 3 );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void btnSkip_Click( Object sender, BEventArgs e ) {
            if ( index <= 4 ) {
                m_povs.set( index - 1, int.MinValue );
            } else {
                m_list.set( index - 5, -1 );
            }
            if ( index <= 8 ) {
                progressCount.setValue( index );
            } else if ( index <= 12 ) {
                progressCount.setValue( index - 8 );
            } else {
                progressCount.setValue( index - 12 );
            }

            if ( index == 8 ) {
                pictButton.setImage( Resources.get_btn2() );
                progressCount.setValue( 0 );
                progressCount.setMaximum( 4 );
            } else if ( index == 12 ) {
                pictButton.setImage( Resources.get_btn3() );
                progressCount.setValue( 0 );
                progressCount.setMaximum( 2 );
            }
            if ( index == 14 ) {
                btnSkip.setEnabled( false );
                btnOK.setEnabled( true );
                timer.stop();
            }
            index++;
        }

        private void btnReset_Click( Object sender, BEventArgs e ) {
            m_list.set( 0, 3 ); // □
            m_list.set( 1, 0 ); // △
            m_list.set( 2, 1 ); // ○
            m_list.set( 3, 2 ); // ×
            m_list.set( 4, 4 ); // L1
            m_list.set( 5, 6 ); // L2
            m_list.set( 6, 5 ); // R1
            m_list.set( 7, 7 ); // R2
            m_list.set( 8, 8 ); // SELECT
            m_list.set( 9, 9 ); // START
            m_povs.set( 0, 18000 ); // down
            m_povs.set( 1, 27000 ); // left
            m_povs.set( 2, 0 ); // up
            m_povs.set( 3, 9000 ); // right
            setDialogResult( BDialogResult.OK );
        }

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        private void registerEventHandlers() {
            timer.tickEvent.add( new BEventHandler( this, "timer_Tick" ) );
            btnSkip.clickEvent.add( new BEventHandler( this, "btnSkip_Click" ) );
            btnReset.clickEvent.add( new BEventHandler( this, "btnReset_Click" ) );
            btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormGameControlerConfig.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormGameControlerConfig.java
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
            this.components = new System.ComponentModel.Container();
            this.lblMessage = new BLabel();
            this.pictButton = new BPictureBox();
            this.progressCount = new BProgressBar();
            this.btnSkip = new BButton();
            this.btnOK = new BButton();
            this.btnCancel = new BButton();
            this.btnReset = new BButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictButton)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point( 15, 17 );
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size( 9, 12 );
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = " ";
            // 
            // timer
            // 
            // 
            // pictButton
            // 
            this.pictButton.Location = new System.Drawing.Point( 12, 49 );
            this.pictButton.Name = "pictButton";
            this.pictButton.Size = new System.Drawing.Size( 316, 36 );
            this.pictButton.TabIndex = 1;
            this.pictButton.TabStop = false;
            // 
            // progressCount
            // 
            this.progressCount.Location = new System.Drawing.Point( 12, 101 );
            this.progressCount.Maximum = 8;
            this.progressCount.Name = "progressCount";
            this.progressCount.Size = new System.Drawing.Size( 316, 13 );
            this.progressCount.TabIndex = 2;
            this.progressCount.Value = 1;
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point( 27, 127 );
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size( 75, 23 );
            this.btnSkip.TabIndex = 3;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point( 172, 159 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 253, 159 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReset.Location = new System.Drawing.Point( 197, 127 );
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size( 131, 23 );
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset And Exit";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // FormGameControlerConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 340, 200 );
            this.Controls.Add( this.btnReset );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnSkip );
            this.Controls.Add( this.progressCount );
            this.Controls.Add( this.pictButton );
            this.Controls.Add( this.lblMessage );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGameControlerConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Game Controler Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.pictButton)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BLabel lblMessage;
        private BPictureBox pictButton;
        private BProgressBar progressCount;
        private BButton btnSkip;
        private BButton btnOK;
        private BButton btnCancel;
        private BButton btnReset;

        #endregion
#endif
    }

#if !JAVA
}
#endif
