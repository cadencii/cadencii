/*
 * InputBox.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.windows.forms.
 *
 * org.kbinani.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.windows.forms;

import java.awt.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.windows.forms;

namespace org.kbinani.windows.forms {
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class InputBox extends BDialog {
#else
    public class InputBox : BDialog {
#endif
        private BLabel lblMessage;
        private BButton btnCancel;
        private BTextBox txtInput;
        private BButton btnOk;
#if JAVA
        public boolean closed = false;
        private BDialogResult m_result = BDialogResult.CANCEL;
#else
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
#endif

        public InputBox( String message ) {
#if JAVA
            initializeComponent();
#else
            InitializeComponent();
#endif
            lblMessage.setText( message );
        }

#if JAVA
        public class ShowDialogRunner implements Runnable{
            public void run(){
                show();
                while( !closed ){
                    try{
                        Thread.sleep( 100 );
                    }catch( Exception ex ){
                        break;
                    }
                }
                hide();
            }
        }

        public BDialogResult showDialog(){
            Thread t = new Thread( new ShowDialogRunner() );
            t.run();
            return m_result;
        }
#endif

        public String getResult(){
            return txtInput.getText();
        }
        
        public void setResult( String value ){
            txtInput.setText( value );
        }

        public void btnOk_Click( Object sender, BEventArgs e ) {
#if JAVA
            closed = true;
            m_result = BDialogResult.OK;
#else
            DialogResult = DialogResult.OK;
#endif
        }

#if JAVA
        private void initializeComponent(){
            txtInput = new BTextBox();
            btnOk = new BButton();
            lblMessage = new BLabel();
            btnCancel = new BButton();
            // 
            // txtInput
            // 
            // 
            // btnOk
            // 
            this.btnOk.setText( "OK" );
            this.btnOk.clickEvent.add( new BEventHandler( this, "btnOk_Click" ) );
            // 
            // lblMessage
            // 
            // 
            // btnCancel
            // 
            this.btnCancel.setText( "Cancel" );
            this.btnCancel.setVisible( false );
            // 
            // InputBox
            // 
            GridBagLayout gridbag = new GridBagLayout();
            GridBagConstraints c = new GridBagConstraints();
            setLayout( gridbag );
            // 1段目
            JPanel jp1_1 = new JPanel();
            gridbag.setConstraints( jp1_1, c );
            add( jp1_1 );

            c.gridwidth = 2;
            c.fill = GridBagConstraints.HORIZONTAL;
            gridbag.setConstraints( lblMessage, c );
            add( lblMessage );
            
            JPanel jp1_2 = new JPanel();
            c.gridwidth = GridBagConstraints.REMAINDER;
            c.fill = GridBagConstraints.NONE;
            gridbag.setConstraints( jp1_2, c );
            add( jp1_2 );

            // 2段目
            JPanel jp2_1 = new JPanel();
            c.gridwidth = 1;
            gridbag.setConstraints( jp2_1, c );
            add( jp2_1 );
            
            c.gridwidth = 2;
            c.fill = GridBagConstraints.HORIZONTAL;
            c.weightx = 1.0;
            gridbag.setConstraints( txtInput, c );
            add( txtInput );
            
            JPanel jp2_2 = new JPanel();
            c.gridwidth = GridBagConstraints.REMAINDER;
            c.fill = GridBagConstraints.NONE;
            c.weightx = 0.0;
            gridbag.setConstraints( jp2_2, c );
            add( jp2_2 );

            // 3段目
            JPanel jp3 = new JPanel();
            c.gridwidth = GridBagConstraints.REMAINDER;
            gridbag.setConstraints( jp3, c );
            add( jp3 );

            // 4段目
            JPanel jp4_1 = new JPanel();
            c.gridwidth = 2;
            gridbag.setConstraints( jp4_1, c );
            add( jp4_1 );
            
            c.gridwidth = 1;
            c.anchor = GridBagConstraints.EAST;
            gridbag.setConstraints( btnOk, c );
            add( btnOk );
            
            JPanel jp4_2 = new JPanel();
            c.gridwidth = GridBagConstraints.REMAINDER;
            c.anchor = GridBagConstraints.CENTER;
            gridbag.setConstraints( jp4_2, c );
            add( jp4_2 );

            // 5段目
            JPanel jp5 = new JPanel();
            c.gridwidth = GridBagConstraints.REMAINDER;
            c.gridheight = GridBagConstraints.REMAINDER;
            c.fill = GridBagConstraints.BOTH;
            gridbag.setConstraints( jp5, c );
            add( jp5 );

            this.formClosedEvent.add( new BEventHandler( this, "InputBox_FormClosed" ) );
            this.setTitle( "InputBox" );
            this.setSize( 339, 110 );
        }

        public void InputBox_FormClosed( Object sender, BEventArgs e ){
            closed = true;
        }
#else
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
            this.txtInput = new BTextBox();
            this.btnOk = new BButton();
            this.lblMessage = new BLabel();
            this.btnCancel = new BButton();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Location = new System.Drawing.Point( 12, 24 );
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size( 309, 19 );
            this.txtInput.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point( 246, 49 );
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size( 75, 23 );
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler( this.btnOk_Click );
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point( 12, 9 );
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size( 0, 12 );
            this.lblMessage.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( -100, 49 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // InputBox
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 333, 82 );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.lblMessage );
            this.Controls.Add( this.btnOk );
            this.Controls.Add( this.txtInput );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "InputBox";
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion
#endif
    }

#if !JAVA
}
#endif
