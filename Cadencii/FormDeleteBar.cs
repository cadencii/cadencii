/*
 * FormDeleteBar.cs
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

import javax.swing.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class FormDeleteBar extends BForm{
#else
    class FormDeleteBar : BForm {
#endif
        public FormDeleteBar( int max_barcount ) {
#if JAVA
            initialize();
#else
            InitializeComponent();
#endif
            ApplyLanguage();
            numStart.Maximum = max_barcount;
            numEnd.Maximum = max_barcount;
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            setTitle( _( "Delete Bars" ) );
            lblStart.setText( _( "Start" ) );
            lblEnd.setText( _( "End" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public int getStart() {
            return (int)numStart.Value;
        }

        public void setStart( int value ) {
            numStart.Value = value;
        }

        public int getEnd() {
            return (int)numEnd.Value;
        }

        public void setEnd( int value ) {
            numEnd.Value = value;
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }
#if JAVA
        #region UI Impl for Java
	    private JPanel jContentPane = null;
	    private JLabel lblStart = null;
	    private JTextField numStart = null;
	    private JLabel label3 = null;
	    private JLabel lblEnd = null;
	    private JTextField numEnd = null;
	    private JLabel label4 = null;
	    private JPanel jPanel = null;
	    private JButton btnOK = null;
	    private JButton btnCancel = null;

	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(210, 149);
		    this.setContentPane(getJContentPane());
		    this.setTitle("JFrame");
	    }

	    /**
	     * This method initializes jContentPane
	     * 
	     * @return javax.swing.JPanel
	     */
	    private JPanel getJContentPane() {
		    if (jContentPane == null) {
			    GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			    gridBagConstraints14.gridx = 0;
			    gridBagConstraints14.anchor = GridBagConstraints.EAST;
			    gridBagConstraints14.gridwidth = 3;
			    gridBagConstraints14.weightx = 1.0D;
			    gridBagConstraints14.insets = new Insets(16, 0, 8, 0);
			    gridBagConstraints14.gridy = 2;
			    GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			    gridBagConstraints13.gridx = 2;
			    gridBagConstraints13.insets = new Insets(4, 8, 0, 16);
			    gridBagConstraints13.gridy = 1;
			    label4 = new JLabel();
			    label4.setText(":0:000");
			    GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			    gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints12.gridy = 1;
			    gridBagConstraints12.weightx = 1.0;
			    gridBagConstraints12.insets = new Insets(4, 0, 0, 0);
			    gridBagConstraints12.gridx = 1;
			    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			    gridBagConstraints3.gridx = 0;
			    gridBagConstraints3.anchor = GridBagConstraints.EAST;
			    gridBagConstraints3.insets = new Insets(4, 16, 0, 8);
			    gridBagConstraints3.gridy = 1;
			    lblEnd = new JLabel();
			    lblEnd.setText("End");
			    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			    gridBagConstraints2.gridx = 2;
			    gridBagConstraints2.insets = new Insets(8, 8, 0, 16);
			    gridBagConstraints2.gridy = 0;
			    label3 = new JLabel();
			    label3.setText(":0:000");
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints1.gridy = 0;
			    gridBagConstraints1.weightx = 1.0;
			    gridBagConstraints1.insets = new Insets(8, 0, 0, 0);
			    gridBagConstraints1.gridx = 1;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.gridx = 0;
			    gridBagConstraints.anchor = GridBagConstraints.EAST;
			    gridBagConstraints.insets = new Insets(8, 16, 0, 8);
			    gridBagConstraints.gridy = 0;
			    lblStart = new JLabel();
			    lblStart.setText("Start");
			    jContentPane = new JPanel();
			    jContentPane.setLayout(new GridBagLayout());
			    jContentPane.add(lblStart, gridBagConstraints);
			    jContentPane.add(getNumStart(), gridBagConstraints1);
			    jContentPane.add(label3, gridBagConstraints2);
			    jContentPane.add(lblEnd, gridBagConstraints3);
			    jContentPane.add(getNumEnd(), gridBagConstraints12);
			    jContentPane.add(label4, gridBagConstraints13);
			    jContentPane.add(getJPanel(), gridBagConstraints14);
		    }
		    return jContentPane;
	    }

	    /**
	     * This method initializes numStart	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getNumStart() {
		    if (numStart == null) {
			    numStart = new JTextField();
		    }
		    return numStart;
	    }

	    /**
	     * This method initializes numEnd	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getNumEnd() {
		    if (numEnd == null) {
			    numEnd = new JTextField();
		    }
		    return numEnd;
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
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnOK() {
		    if (btnOK == null) {
			    btnOK = new JButton();
			    btnOK.setText("OK");
		    }
		    return btnOK;
	    }

	    /**
	     * This method initializes btnCancel	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnCancel() {
		    if (btnCancel == null) {
			    btnCancel = new JButton();
			    btnCancel.setText("Cancel");
		    }
		    return btnCancel;
	    }
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
            this.btnOK = new BButton();
            this.btnCancel = new BButton();
            this.label4 = new BLabel();
            this.label3 = new BLabel();
            this.lblEnd = new BLabel();
            this.lblStart = new BLabel();
            this.numEnd = new NumericUpDownEx();
            this.numStart = new NumericUpDownEx();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point( 37, 66 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 118, 66 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 125, 38 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 33, 12 );
            this.label4.TabIndex = 13;
            this.label4.Text = ":0:000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 125, 13 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 33, 12 );
            this.label3.TabIndex = 12;
            this.label3.Text = ":0:000";
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point( 12, 38 );
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size( 49, 12 );
            this.lblEnd.TabIndex = 11;
            this.lblEnd.Text = "End";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point( 12, 13 );
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size( 49, 12 );
            this.lblStart.TabIndex = 10;
            this.lblStart.Text = "Start";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numEnd
            // 
            this.numEnd.Location = new System.Drawing.Point( 67, 36 );
            this.numEnd.Maximum = new decimal( new int[] {
            32,
            0,
            0,
            0} );
            this.numEnd.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numEnd.Name = "numEnd";
            this.numEnd.Size = new System.Drawing.Size( 52, 19 );
            this.numEnd.TabIndex = 9;
            this.numEnd.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point( 67, 11 );
            this.numStart.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size( 52, 19 );
            this.numStart.TabIndex = 8;
            this.numStart.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // FormDeleteBar
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 204, 100 );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.lblEnd );
            this.Controls.Add( this.lblStart );
            this.Controls.Add( this.numEnd );
            this.Controls.Add( this.numStart );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDeleteBar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Delete Bars";
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BButton btnOK;
        private BButton btnCancel;
        private BLabel label4;
        private BLabel label3;
        private BLabel lblEnd;
        private BLabel lblStart;
        private NumericUpDownEx numEnd;
        private NumericUpDownEx numStart;
        #endregion
#endif
    }

#if !JAVA
}
#endif
