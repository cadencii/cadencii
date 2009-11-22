/*
 * FormInsertBar.cs
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

import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormInsertBar extends BForm{
#else
    public class FormInsertBar : BForm {
#endif
        public FormInsertBar( int max_position ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            numPosition.Maximum = max_position;
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            setTitle( _( "Insert Bars" ) );
            String th_prefix = _( "_PREFIX_TH_" );
            if ( th_prefix.Equals( "_PREFIX_TH_" ) ) {
                lblPositionPrefix.setText( "" );
            } else {
                lblPositionPrefix.setText( th_prefix );
            }
            lblPosition.setText( _( "Position" ) );
            lblLength.setText( _( "Length" ) );
            lblThBar.setText( _( "th bar" ) );
            lblBar.setText( _( "bar" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            lblPositionPrefix.Left = numPosition.Left - lblPositionPrefix.Width;
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public int getLength() {
            return (int)numLength.Value;
        }

        public void setLength( int value ) {
            numLength.Value = value;
        }

        public int getPosition() {
            return (int)numPosition.Value;
        }

        public void setPosition( int value ) {
            numPosition.Value = value;
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        private void registerEventHandlers() {
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        private static final long serialVersionUID = 1L;
        private JPanel jContentPane = null;
        private BLabel lblPosition = null;
        private BNumericUpDown numPosition = null;
        private BLabel lblThBar = null;
        private BLabel lblLength = null;
        private BNumericUpDown numLength = null;
        private BLabel lblBar = null;
        private JPanel jPanel = null;
        private BButton btnOK = null;
        private BButton btnCancel = null;
        private JLabel lblPositionPrefix = null;

        /**
         * This method initializes this
         * 
         * @return void
         */
        private void initialize() {
            this.setSize(249, 158);
            this.setContentPane(getJContentPane());
            this.setTitle("Insert bar");
        }

        /**
         * This method initializes jContentPane
         * 
         * @return javax.swing.JPanel
         */
        private JPanel getJContentPane() {
            if (jContentPane == null) {
                GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
                gridBagConstraints11.gridx = 1;
                gridBagConstraints11.insets = new Insets(8, 0, 0, 0);
                gridBagConstraints11.gridy = 0;
                lblPositionPrefix = new JLabel();
                lblPositionPrefix.setText(" ");
                GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
                gridBagConstraints14.gridx = 0;
                gridBagConstraints14.anchor = GridBagConstraints.EAST;
                gridBagConstraints14.gridwidth = 4;
                gridBagConstraints14.weightx = 1.0D;
                gridBagConstraints14.insets = new Insets(16, 0, 8, 0);
                gridBagConstraints14.gridy = 2;
                GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
                gridBagConstraints13.gridx = 3;
                gridBagConstraints13.insets = new Insets(4, 8, 0, 16);
                gridBagConstraints13.anchor = GridBagConstraints.WEST;
                gridBagConstraints13.gridy = 1;
                lblBar = new BLabel();
                lblBar.setText("bar");
                GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
                gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
                gridBagConstraints12.gridy = 1;
                gridBagConstraints12.weightx = 1.0;
                gridBagConstraints12.insets = new Insets(4, 0, 0, 0);
                gridBagConstraints12.gridx = 2;
                GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
                gridBagConstraints3.gridx = 0;
                gridBagConstraints3.anchor = GridBagConstraints.WEST;
                gridBagConstraints3.insets = new Insets(4, 16, 0, 8);
                gridBagConstraints3.gridwidth = 2;
                gridBagConstraints3.gridy = 1;
                lblLength = new BLabel();
                lblLength.setText("Length");
                GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
                gridBagConstraints2.gridx = 3;
                gridBagConstraints2.insets = new Insets(8, 8, 0, 16);
                gridBagConstraints2.anchor = GridBagConstraints.WEST;
                gridBagConstraints2.gridy = 0;
                lblThBar = new BLabel();
                lblThBar.setText("th bar");
                GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
                gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
                gridBagConstraints1.gridy = 0;
                gridBagConstraints1.weightx = 1.0;
                gridBagConstraints1.insets = new Insets(8, 0, 0, 0);
                gridBagConstraints1.gridx = 2;
                GridBagConstraints gridBagConstraints = new GridBagConstraints();
                gridBagConstraints.gridx = 0;
                gridBagConstraints.anchor = GridBagConstraints.EAST;
                gridBagConstraints.insets = new Insets(8, 16, 0, 8);
                gridBagConstraints.gridy = 0;
                lblPosition = new BLabel();
                lblPosition.setText("Position");
                jContentPane = new JPanel();
                jContentPane.setLayout(new GridBagLayout());
                jContentPane.add(lblPosition, gridBagConstraints);
                jContentPane.add(getNumPosition(), gridBagConstraints1);
                jContentPane.add(lblThBar, gridBagConstraints2);
                jContentPane.add(lblLength, gridBagConstraints3);
                jContentPane.add(getNumLength(), gridBagConstraints12);
                jContentPane.add(lblBar, gridBagConstraints13);
                jContentPane.add(getJPanel(), gridBagConstraints14);
                jContentPane.add(lblPositionPrefix, gridBagConstraints11);
            }
            return jContentPane;
        }

        /**
         * This method initializes numPosition 
         *  
         * @return javax.swing.BTextBox 
         */
        private BNumericUpDown getNumPosition() {
            if (numPosition == null) {
                numPosition = new BNumericUpDown();
            }
            return numPosition;
        }

        /**
         * This method initializes numLength   
         *  
         * @return javax.swing.BTextBox 
         */
        private BNumericUpDown getNumLength() {
            if (numLength == null) {
                numLength = new BNumericUpDown();
            }
            return numLength;
        }

        /**
         * This method initializes jPanel   
         *  
         * @return javax.swing.JPanel   
         */
        private JPanel getJPanel() {
            if (jPanel == null) {
                GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
                gridBagConstraints5.gridx = 1;
                gridBagConstraints5.anchor = GridBagConstraints.WEST;
                gridBagConstraints5.insets = new Insets(0, 0, 0, 16);
                gridBagConstraints5.gridy = 0;
                GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
                gridBagConstraints4.gridx = 0;
                gridBagConstraints4.anchor = GridBagConstraints.WEST;
                gridBagConstraints4.insets = new Insets(0, 0, 0, 16);
                gridBagConstraints4.gridy = 0;
                jPanel = new JPanel();
                jPanel.setLayout(new GridBagLayout());
                jPanel.add(getBtnOK(), gridBagConstraints4);
                jPanel.add(getBtnCancel(), gridBagConstraints5);
            }
            return jPanel;
        }

        /**
         * This method initializes btnOK    
         *  
         * @return javax.swing.BButton  
         */
        private BButton getBtnOK() {
            if (btnOK == null) {
                btnOK = new BButton();
                btnOK.setText("OK");
            }
            return btnOK;
        }

        /**
         * This method initializes btnCancel    
         *  
         * @return javax.swing.BButton  
         */
        private BButton getBtnCancel() {
            if (btnCancel == null) {
                btnCancel = new BButton();
                btnCancel.setText("Cancel");
            }
            return btnCancel;
        }
        #endregion
#else
        #region UI Impl for C#
        private System.ComponentModel.IContainer components = null;
        private NumericUpDownEx numPosition;
        private NumericUpDownEx numLength;
        private BLabel lblPosition;
        private BLabel lblLength;
        private BLabel lblThBar;
        private BLabel lblBar;
        private BButton btnCancel;
        private BButton btnOK;
        private BLabel lblPositionPrefix;

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
            this.numPosition = new NumericUpDownEx();
            this.numLength = new NumericUpDownEx();
            this.lblPosition = new BLabel();
            this.lblLength = new BLabel();
            this.lblThBar = new BLabel();
            this.lblBar = new BLabel();
            this.btnCancel = new BButton();
            this.btnOK = new BButton();
            this.lblPositionPrefix = new BLabel();
            ((System.ComponentModel.ISupportInitialize)(this.numPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            this.SuspendLayout();
            // 
            // numPosition
            // 
            this.numPosition.Location = new System.Drawing.Point( 78, 12 );
            this.numPosition.Name = "numPosition";
            this.numPosition.Size = new System.Drawing.Size( 52, 19 );
            this.numPosition.TabIndex = 0;
            // 
            // numLength
            // 
            this.numLength.Location = new System.Drawing.Point( 78, 37 );
            this.numLength.Maximum = new decimal( new int[] {
            32,
            0,
            0,
            0} );
            this.numLength.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size( 52, 19 );
            this.numLength.TabIndex = 1;
            this.numLength.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point( 12, 14 );
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size( 46, 12 );
            this.lblPosition.TabIndex = 2;
            this.lblPosition.Text = "Position";
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point( 12, 39 );
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size( 39, 12 );
            this.lblLength.TabIndex = 3;
            this.lblLength.Text = "Length";
            // 
            // lblThBar
            // 
            this.lblThBar.AutoSize = true;
            this.lblThBar.Location = new System.Drawing.Point( 136, 14 );
            this.lblThBar.Name = "lblThBar";
            this.lblThBar.Size = new System.Drawing.Size( 35, 12 );
            this.lblThBar.TabIndex = 4;
            this.lblThBar.Text = "th bar";
            // 
            // lblBar
            // 
            this.lblBar.AutoSize = true;
            this.lblBar.Location = new System.Drawing.Point( 136, 39 );
            this.lblBar.Name = "lblBar";
            this.lblBar.Size = new System.Drawing.Size( 21, 12 );
            this.lblBar.TabIndex = 5;
            this.lblBar.Text = "bar";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 134, 67 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 53, 67 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblPositionPrefix
            // 
            this.lblPositionPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPositionPrefix.AutoSize = true;
            this.lblPositionPrefix.Location = new System.Drawing.Point( 62, 14 );
            this.lblPositionPrefix.Name = "lblPositionPrefix";
            this.lblPositionPrefix.Size = new System.Drawing.Size( 0, 12 );
            this.lblPositionPrefix.TabIndex = 8;
            this.lblPositionPrefix.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormInsertBar
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 221, 102 );
            this.Controls.Add( this.lblPositionPrefix );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.lblBar );
            this.Controls.Add( this.lblThBar );
            this.Controls.Add( this.lblLength );
            this.Controls.Add( this.lblPosition );
            this.Controls.Add( this.numLength );
            this.Controls.Add( this.numPosition );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInsertBar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Insert bar";
            ((System.ComponentModel.ISupportInitialize)(this.numPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion
        #endregion
#endif

    }

#if !JAVA
}
#endif
