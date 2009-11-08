/*
 * FormCompileResult.cs
 * Copyright (c) 2009 kbinani
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
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
#endif

#if JAVA
    public class FormCompileResult extends BForm {
#else
    public class FormCompileResult : BForm {
#endif
        public FormCompileResult( String message, String errors ) {
#if JAVA
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            label1.setText( message );
            textBox1.setText( errors );
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            this.textBox1.setText( _( "Script Compilation Result" ) );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void setResources() {
        }

        private void registerEventHandlers() {
        }
#if JAVA
        #region UI Impl for Java
	    private JPanel jContentPane = null;
	    private JLabel label1 = null;
	    private JTextArea textBox1 = null;
	    private JButton btnOK = null;
	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(386, 309);
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
			    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			    gridBagConstraints2.gridx = 0;
			    gridBagConstraints2.anchor = GridBagConstraints.EAST;
			    gridBagConstraints2.insets = new Insets(0, 16, 16, 16);
			    gridBagConstraints2.gridy = 2;
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.fill = GridBagConstraints.BOTH;
			    gridBagConstraints1.gridy = 1;
			    gridBagConstraints1.weightx = 1.0;
			    gridBagConstraints1.weighty = 1.0;
			    gridBagConstraints1.insets = new Insets(10, 16, 16, 16);
			    gridBagConstraints1.gridx = 0;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.gridx = 0;
			    gridBagConstraints.anchor = GridBagConstraints.WEST;
			    gridBagConstraints.insets = new Insets(23, 16, 0, 0);
			    gridBagConstraints.gridy = 0;
			    label1 = new JLabel();
			    label1.setText("JLabel");
			    jContentPane = new JPanel();
			    jContentPane.setLayout(new GridBagLayout());
			    jContentPane.add(label1, gridBagConstraints);
			    jContentPane.add(getTextBox1(), gridBagConstraints1);
			    jContentPane.add(getBtnOK(), gridBagConstraints2);
		    }
		    return jContentPane;
	    }

	    /**
	     * This method initializes textBox1	
	     * 	
	     * @return javax.swing.JTextArea	
	     */
	    private JTextArea getTextBox1() {
		    if (textBox1 == null) {
			    textBox1 = new JTextArea();
			    textBox1.setLineWrap(true);
		    }
		    return textBox1;
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
            this.label1 = new BLabel();
            this.textBox1 = new BTextBox();
            this.btnOK = new BButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 23, 23 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 35, 12 );
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point( 12, 53 );
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size( 354, 174 );
            this.textBox1.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 291, 240 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // FormCompileResult
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 378, 275 );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.textBox1 );
            this.Controls.Add( this.label1 );
            this.Name = "FormCompileResult";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Script Compilation Result";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BLabel label1;
        private BTextBox textBox1;
        private BButton btnOK;
        #endregion
#endif
    }

#if !JAVA
}
#endif
