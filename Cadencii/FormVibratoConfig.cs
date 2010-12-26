/*
 * FormVibratoConfig.cs
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoConfig.java

import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormVibratoConfig extends BDialog{
#else
    public class FormVibratoConfig : BDialog
    {
#endif
        private VibratoHandle m_vibrato;
        private int m_note_length;
        private SynthesizerType m_synthesizer_type;
        /// <summary>
        /// ユーザー定義のプリセットビブラートを使うかどうか
        /// </summary>
        private boolean mUseOriginal = false;

        /// <summary>
        /// コンストラクタ．引数vibrato_handleには，Cloneしたものを渡さなくてよい．
        /// </summary>
        /// <param name="vibrato_handle"></param>
        /// <param name="note_length"></param>
        /// <param name="default_vibrato_length"></param>
        /// <param name="type"></param>
        /// <param name="use_original"></param>
        public FormVibratoConfig( VibratoHandle vibrato_handle, int note_length, DefaultVibratoLengthEnum default_vibrato_length, SynthesizerType type, boolean use_original )
        {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif

#if DEBUG
            AppManager.debugWriteLine( "FormVibratoConfig.ctor(Vsqhandle,int,DefaultVibratoLength)" );
            AppManager.debugWriteLine( "    (vibrato_handle==null)=" + (vibrato_handle == null) );
            PortUtil.println( "    type=" + type );
#endif
            m_synthesizer_type = type;
            mUseOriginal = use_original;
            if ( vibrato_handle != null ) {
                m_vibrato = (VibratoHandle)vibrato_handle.clone();
            }

            registerEventHandlers();
            setResources();
            applyLanguage();

            comboVibratoType.removeAllItems();
            VibratoHandle empty = new VibratoHandle();
            empty.setCaption( "[Non Vibrato]" );
            empty.IconID = "$04040000";
            comboVibratoType.addItem( empty );
            comboVibratoType.setSelectedItem( empty );
            if ( use_original ) {
                int size = AppManager.editorConfig.AutoVibratoCustom.size();
                for ( int i = 0; i < size; i++ ) {
                    VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom.get( i );
                    comboVibratoType.addItem( handle );
                    if ( vibrato_handle.IconID.Equals( handle.IconID ) ) {
                        comboVibratoType.setSelectedItem( handle );
                    }
                }
            } else {
                for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( m_synthesizer_type ); itr.hasNext(); ) {
                    VibratoHandle vconfig = itr.next();
                    comboVibratoType.addItem( vconfig );
                    if ( vibrato_handle != null ) {
                        if ( vibrato_handle.IconID.Equals( vconfig.IconID ) ) {
                            comboVibratoType.setSelectedItem( vconfig );
                        }
                    }
                }
            }

            txtVibratoLength.setEnabled( vibrato_handle != null );
            if ( vibrato_handle != null ) {
                txtVibratoLength.setText( (int)((float)vibrato_handle.getLength() / (float)note_length * 100.0f) + "" );
            } else {
                String s = "";
                if ( default_vibrato_length == DefaultVibratoLengthEnum.L100 ) {
                    s = "100";
                } else if ( default_vibrato_length == DefaultVibratoLengthEnum.L50 ) {
                    s = "50";
                } else if ( default_vibrato_length == DefaultVibratoLengthEnum.L66 ) {
                    s = "66";
                } else if ( default_vibrato_length == DefaultVibratoLengthEnum.L75 ) {
                    s = "75";
                }
                txtVibratoLength.setText( s );
            }

            comboVibratoType.SelectedIndexChanged += new EventHandler( comboVibratoType_SelectedIndexChanged );
            txtVibratoLength.TextChanged += new EventHandler( txtVibratoLength_TextChanged );

            m_note_length = note_length;
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage()
        {
            setTitle( _( "Vibrato property" ) );
            lblVibratoLength.setText( _( "Vibrato length" ) + "(&L)" );
            lblVibratoType.setText( _( "Vibrato Type" ) + "(&T)" );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
        }

        /// <summary>
        /// 編集済みのビブラート設定．既にCloneされているので，改めてCloneしなくて良い
        /// </summary>
        public VibratoHandle getVibratoHandle()
        {
            return m_vibrato;
        }
        #endregion

        #region helper methods
        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new EventHandler( btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void btnOK_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.OK );
        }

        public void comboVibratoType_SelectedIndexChanged( Object sender, BEventArgs e )
        {
            int index = comboVibratoType.getSelectedIndex();
            if ( index >= 0 ) {
                String s = ((VibratoHandle)comboVibratoType.getItemAt( index )).IconID;
                if ( s.Equals( "$04040000" ) ) {
                    m_vibrato = null;
                    txtVibratoLength.setEnabled( false );
                    return;
                } else {
                    txtVibratoLength.setEnabled( true );
                    VibratoHandle src = null;
                    if ( mUseOriginal ) {
                        int size = AppManager.editorConfig.AutoVibratoCustom.size();
                        for ( int i = 0; i < size; i++ ) {
                            VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom.get( i );
                            if ( s.Equals( handle.IconID ) ) {
                                src = handle;
                                break;
                            }
                        }
                    } else {
                        for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( m_synthesizer_type ); itr.hasNext(); ) {
                            VibratoHandle vconfig = itr.next();
                            if ( s.Equals( vconfig.IconID ) ) {
                                src = vconfig;
                                break;
                            }
                        }
                    }
                    if ( src != null ) {
                        int percent;
                        try {
                            percent = PortUtil.parseInt( txtVibratoLength.getText() );
                        } catch ( Exception ex ) {
                            return;
                        }
                        m_vibrato = (VibratoHandle)src.clone();
                        m_vibrato.setLength( (int)(m_note_length * percent / 100.0f) );
                        return;
                    }
                }
            }
        }

        public void txtVibratoLength_TextChanged( Object sender, BEventArgs e )
        {
#if DEBUG
            AppManager.debugWriteLine( "txtVibratoLength_TextChanged" );
            AppManager.debugWriteLine( "    (m_vibrato==null)=" + (m_vibrato == null) );
#endif
            int percent = 0;
            try {
                percent = PortUtil.parseInt( txtVibratoLength.getText() );
                if ( percent < 0 ) {
                    percent = 0;
                } else if ( 100 < percent ) {
                    percent = 100;
                }
            } catch ( Exception ex ) {
                return;
            }
            if ( percent == 0 ) {
                m_vibrato = null;
                txtVibratoLength.setEnabled( false );
            } else {
                if ( m_vibrato != null ) {
                    int new_length = (int)(m_note_length * percent / 100.0f);
                    m_vibrato.setLength( new_length );
                }
            }
        }

        public void btnCancel_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.CANCEL );
        }
        #endregion

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoConfig.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoConfig.java
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
        protected override void Dispose( boolean disposing )
        {
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
        private void InitializeComponent()
        {
            this.lblVibratoLength = new BLabel();
            this.lblVibratoType = new BLabel();
            this.txtVibratoLength = new org.kbinani.cadencii.NumberTextBox();
            this.label3 = new BLabel();
            this.comboVibratoType = new BComboBox();
            this.btnCancel = new BButton();
            this.btnOK = new BButton();
            this.SuspendLayout();
            // 
            // lblVibratoLength
            // 
            this.lblVibratoLength.AutoSize = true;
            this.lblVibratoLength.Location = new System.Drawing.Point( 12, 15 );
            this.lblVibratoLength.Name = "lblVibratoLength";
            this.lblVibratoLength.Size = new System.Drawing.Size( 94, 12 );
            this.lblVibratoLength.TabIndex = 0;
            this.lblVibratoLength.Text = "Vibrato Length(&L)";
            // 
            // lblVibratoType
            // 
            this.lblVibratoType.AutoSize = true;
            this.lblVibratoType.Location = new System.Drawing.Point( 12, 38 );
            this.lblVibratoType.Name = "lblVibratoType";
            this.lblVibratoType.Size = new System.Drawing.Size( 86, 12 );
            this.lblVibratoType.TabIndex = 1;
            this.lblVibratoType.Text = "Vibrato Type(&T)";
            // 
            // txtVibratoLength
            // 
            this.txtVibratoLength.Enabled = false;
            this.txtVibratoLength.Location = new System.Drawing.Point( 143, 12 );
            this.txtVibratoLength.Name = "txtVibratoLength";
            this.txtVibratoLength.Size = new System.Drawing.Size( 61, 19 );
            this.txtVibratoLength.TabIndex = 2;
            this.txtVibratoLength.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 210, 15 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 49, 12 );
            this.label3.TabIndex = 3;
            this.label3.Text = "%(0-100)";
            // 
            // comboVibratoType
            // 
            this.comboVibratoType.FormattingEnabled = true;
            this.comboVibratoType.Location = new System.Drawing.Point( 143, 35 );
            this.comboVibratoType.Name = "comboVibratoType";
            this.comboVibratoType.Size = new System.Drawing.Size( 167, 20 );
            this.comboVibratoType.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 240, 71 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 159, 71 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // FormVibratoConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 327, 106 );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.comboVibratoType );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.txtVibratoLength );
            this.Controls.Add( this.lblVibratoType );
            this.Controls.Add( this.lblVibratoLength );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVibratoConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vibrato property";
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion

        private BLabel lblVibratoLength;
        private BLabel lblVibratoType;
        private NumberTextBox txtVibratoLength;
        private BLabel label3;
        private BComboBox comboVibratoType;
        private BButton btnCancel;
        private BButton btnOK;
        #endregion
#endif
    }

#if !JAVA
}
#endif
