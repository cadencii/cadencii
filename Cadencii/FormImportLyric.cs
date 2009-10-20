/*
 * FormImportLyric.cs
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
import java.util.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
import org.kbinani.apputil.*;
#else
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii
{
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class FormImportLyric extends BForm
#else
    class FormImportLyric : BForm
#endif
    {
        private int m_max_notes = 1;

        public FormImportLyric( int max_notes )
        {
#if JAVA
            initialize();
#else
            InitializeComponent();
#endif
            ApplyLanguage();
            String notes = (max_notes > 1) ? " [notes]" : " [note]";
            lblNotes.Text = "Max : " + max_notes + notes;
            m_max_notes = max_notes;
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage()
        {
            Text = _( "Import lyrics" );
            btnCancel.Text = _( "Cancel" );
            btnOK.Text = _( "OK" );
        }

        public static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        public String[] GetLetters()
        {
            Vector<char> _SMALL = new Vector<char>( new char[] { 'ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ', 'ゃ', 'ゅ', 'ょ', 'ァ', 'ィ', 'ゥ', 'ェ', 'ォ', 'ャ', 'ュ', 'ョ' } );
            String tmp = "";
            for ( int i = 0; i < m_max_notes; i++ )
            {
                if ( i >= txtLyrics.Lines.Length )
                {
                    break;
                }
                tmp += txtLyrics.Lines[i] + " ";
            }
            String[] spl = PortUtil.splitString( tmp, new char[] { '\n', '\t', ' ', '　', '\r' }, true );
            Vector<String> ret = new Vector<String>();
            for ( int j = 0; j < spl.Length; j++ )
            {
                String s = spl[j];
                char[] list = s.ToCharArray();
                String t = "";
                int i = -1;
                while ( i + 1 < list.Length )
                {
                    i++;
                    if ( 0x41 <= list[i] && list[i] <= 0x176 )
                    {
                        t += list[i].ToString();
                    }
                    else
                    {
                        if ( t.Length > 0 )
                        {
                            ret.add( t );
                            t = "";
                        }
                        if ( i + 1 < list.Length )
                        {
                            if ( _SMALL.contains( list[i + 1] ) )
                            {
                                // 次の文字が拗音の場合
                                ret.add( list[i].ToString() + list[i + 1].ToString() );
                                i++;
                            }
                            else
                            {
                                ret.add( list[i].ToString() );
                            }
                        }
                        else
                        {
                            ret.add( list[i].ToString() );
                        }
                    }
                }
                if ( t.Length > 0 )
                {
                    ret.add( t );
                }
            }
            return ret.toArray( new String[] { } );
        }

        private void btnOK_Click( Object sender, BEventArgs e )
        {
            this.DialogResult = DialogResult.OK;
        }
#if JAVA
        #region UI Impl for Java
	    private JPanel jContentPane = null;
	    private JLabel lblNotes = null;
	    private JTextArea txtLyrics = null;
	    private JPanel jPanel = null;
	    private JButton btnOK = null;
	    private JButton btnCancel = null;

	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(456, 380);
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
			    GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			    gridBagConstraints4.gridx = 0;
			    gridBagConstraints4.anchor = GridBagConstraints.EAST;
			    gridBagConstraints4.insets = new Insets(0, 0, 16, 0);
			    gridBagConstraints4.gridy = 2;
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.fill = GridBagConstraints.BOTH;
			    gridBagConstraints1.gridy = 1;
			    gridBagConstraints1.weightx = 1.0;
			    gridBagConstraints1.weighty = 1.0;
			    gridBagConstraints1.insets = new Insets(0, 16, 16, 16);
			    gridBagConstraints1.gridx = 0;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.gridx = 0;
			    gridBagConstraints.anchor = GridBagConstraints.WEST;
			    gridBagConstraints.insets = new Insets(16, 16, 8, 0);
			    gridBagConstraints.gridy = 0;
			    lblNotes = new JLabel();
			    lblNotes.setText("Max : *[notes]");
			    jContentPane = new JPanel();
			    jContentPane.setLayout(new GridBagLayout());
			    jContentPane.add(lblNotes, gridBagConstraints);
			    jContentPane.add(getTxtLyrics(), gridBagConstraints1);
			    jContentPane.add(getJPanel(), gridBagConstraints4);
		    }
		    return jContentPane;
	    }

	    /**
	     * This method initializes txtLyrics	
	     * 	
	     * @return javax.swing.JTextArea	
	     */
	    private JTextArea getTxtLyrics() {
		    if (txtLyrics == null) {
			    txtLyrics = new JTextArea();
		    }
		    return txtLyrics;
	    }

	    /**
	     * This method initializes jPanel	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel() {
		    if (jPanel == null) {
			    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			    gridBagConstraints3.gridx = 1;
			    gridBagConstraints3.insets = new Insets(0, 0, 0, 16);
			    gridBagConstraints3.gridy = 0;
			    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			    gridBagConstraints2.gridx = 0;
			    gridBagConstraints2.insets = new Insets(0, 0, 0, 16);
			    gridBagConstraints2.gridy = 0;
			    jPanel = new JPanel();
			    jPanel.setLayout(new GridBagLayout());
			    jPanel.add(getBtnOK(), gridBagConstraints2);
			    jPanel.add(getBtnCancel(), gridBagConstraints3);
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
        protected override void Dispose( bool disposing )
        {
            if ( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLyrics = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblNotes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtLyrics
            // 
            this.txtLyrics.Location = new System.Drawing.Point( 12, 35 );
            this.txtLyrics.Multiline = true;
            this.txtLyrics.Name = "txtLyrics";
            this.txtLyrics.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLyrics.Size = new System.Drawing.Size( 426, 263 );
            this.txtLyrics.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 363, 317 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point( 269, 317 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point( 15, 16 );
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size( 78, 12 );
            this.lblNotes.TabIndex = 3;
            this.lblNotes.Text = "Max : *[notes]";
            // 
            // FormImportLyric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 450, 352 );
            this.Controls.Add( this.lblNotes );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.txtLyrics );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImportLyric";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Import lyrics";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLyrics;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblNotes;
        #endregion
#endif
    }

#if !JAVA
}
#endif
