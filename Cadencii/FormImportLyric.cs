/*
 * FormImportLyric.cs
 * Copyright (C) 2008-2010 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormImportLyric.java

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
    using BEventArgs = System.EventArgs;
    using Character = System.Char;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormImportLyric extends BDialog {
#else
    class FormImportLyric : BDialog {
#endif
        private int m_max_notes = 1;

        public FormImportLyric( int max_notes ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            applyLanguage();
            setMaxNotes( max_notes );
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
#if JAVA
        public void setVisible( boolean value ){
            super.setVisible( value );
#else
        public override void setVisible( boolean value ) {
            base.setVisible( value );
#endif
#if JAVA
            //TODO: FormImportLyric#setVisible
#else
            this.txtLyrics.HideSelection = false;
            this.txtLyrics.SelectAll();
            this.txtLyrics.Focus();
#endif
        }

        public void applyLanguage() {
            setTitle( _( "Import lyrics" ) );
            btnCancel.setText( _( "Cancel" ) );
            btnOK.setText( _( "OK" ) );
        }

        /// <summary>
        /// このダイアログに入力できる最大の文字数を設定します．
        /// </summary>
        /// <param name="max_notes"></param>
        public void setMaxNotes( int max_notes ) {
            String notes = (max_notes > 1) ? " [notes]" : " [note]";
            this.lblNotes.setText( "Max : " + max_notes + notes );
            this.m_max_notes = max_notes;
        }

        public String[] getLetters() {
            Vector<Character> _SMALL = new Vector<Character>( Arrays.asList( new Character[] { 'ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ',
                                                                                               'ゃ', 'ゅ', 'ょ',
                                                                                               'ァ', 'ィ', 'ゥ', 'ェ', 'ォ',
                                                                                               'ャ', 'ュ', 'ョ' } ) );
            String tmp = "";
            for ( int i = 0; i < m_max_notes; i++ ) {
                if ( i >= txtLyrics.getLineCount() ) {
                    break;
                }
                try {
                    int start = txtLyrics.getLineStartOffset( i );
                    int end = txtLyrics.getLineEndOffset( i );
                    tmp += txtLyrics.getText( start, end - start ) + " ";
                } catch ( Exception ex ) {
                    Logger.write( typeof( FormImportLyric ) + ".getLetters; ex=" + ex + "\n" );
                }
            }
            String[] spl = PortUtil.splitString( tmp, new char[] { '\n', '\t', ' ', '　', '\r' }, true );
            Vector<String> ret = new Vector<String>();
            for ( int j = 0; j < spl.Length; j++ ) {
                String s = spl[j];
                char[] list = s.ToCharArray();
                String t = "";
                int i = -1;
                while ( i + 1 < list.Length ) {
                    i++;
                    if ( 0x41 <= list[i] && list[i] <= 0x176 ) {
                        t += list[i] + "";
                    } else {
                        if ( PortUtil.getStringLength( t ) > 0 ) {
                            ret.add( t );
                            t = "";
                        }
                        if ( i + 1 < list.Length ) {
                            if ( _SMALL.contains( list[i + 1] ) ) {
                                // 次の文字が拗音の場合
                                ret.add( list[i] + "" + list[i + 1] + "" );
                                i++;
                            } else {
                                ret.add( list[i] + "" );
                            }
                        } else {
                            ret.add( list[i] + "" );
                        }
                    }
                }
                if ( PortUtil.getStringLength( t ) > 0 ) {
                    ret.add( t );
                }
            }
            return ret.toArray( new String[] { } );
        }
        #endregion

        #region helper methods
        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers() {
            btnOK.Click += new EventHandler( btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
        }

        private void setResources() {
        }
        #endregion

        #region event handlers
        public void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        public void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }
        #endregion

        #region UI implementation
#if JAVA
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormImportLyric.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormImportLyric.java
#else
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

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.txtLyrics = new org.kbinani.windows.forms.BTextArea();
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.lblNotes = new org.kbinani.windows.forms.BLabel();
            this.SuspendLayout();
            // 
            // txtLyrics
            // 
            this.txtLyrics.AcceptsReturn = true;
            this.txtLyrics.AcceptsTab = true;
            this.txtLyrics.Location = new System.Drawing.Point( 12, 35 );
            this.txtLyrics.Multiline = true;
            this.txtLyrics.Name = "txtLyrics";
            this.txtLyrics.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLyrics.Size = new System.Drawing.Size( 426, 263 );
            this.txtLyrics.TabIndex = 0;
            this.txtLyrics.WordWrap = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 363, 317 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point( 269, 317 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
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
            this.AcceptButton = this.btnOK;
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

        private BTextArea txtLyrics;
        private BButton btnCancel;
        private BButton btnOK;
        private BLabel lblNotes;
#endif
        #endregion

    }

#if !JAVA
}
#endif
