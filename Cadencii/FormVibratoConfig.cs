/*
 * FormVibratoConfig.cs
 * Copyright © 2008-2011 kbinani
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
    using BEventHandler = System.EventHandler;
#endif

#if JAVA
    public class FormVibratoConfig extends BDialog{
#else
    public class FormVibratoConfig : BDialog
    {
#endif
        private VibratoHandle m_vibrato;
        private int m_note_length;

        /// <summary>
        /// コンストラクタ．引数vibrato_handleには，Cloneしたものを渡さなくてよい．
        /// </summary>
        /// <param name="vibrato_handle"></param>
        /// <param name="note_length"></param>
        /// <param name="default_vibrato_length"></param>
        /// <param name="type"></param>
        /// <param name="use_original"></param>
        public FormVibratoConfig( 
            VibratoHandle vibrato_handle, 
            int note_length, 
            DefaultVibratoLengthEnum default_vibrato_length, 
            SynthesizerType type, 
            boolean use_original )
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
            if ( use_original ) {
                radioUserDefined.Checked = true;
            } else {
                if ( type == SynthesizerType.VOCALOID1 ) {
                    radioVocaloid1.Checked = true;
                } else {
                    radioVocaloid2.Checked = true;
                }
            }
            if ( vibrato_handle != null ) {
                m_vibrato = (VibratoHandle)vibrato_handle.clone();
            }

            registerEventHandlers();
            setResources();
            applyLanguage();

            // 選択肢の状態を更新
            updateComboBoxStatus();
            // どれを選ぶか？
#if DEBUG
            PortUtil.println( "FormVibratoConfig#.ctor; vibrato_handle.IconID=" + vibrato_handle.IconID );
#endif
            for ( int i = 0; i < comboVibratoType.getItemCount(); i++ ) {
                VibratoHandle handle = (VibratoHandle)comboVibratoType.getItemAt( i );
#if DEBUG
                PortUtil.println( "FormVibratoConfig#.ctor; handle.IconID=" + handle.IconID );
#endif
                if ( vibrato_handle.IconID.Equals( handle.IconID ) ) {
                    comboVibratoType.setSelectedIndex( i );
                    break;
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

            comboVibratoType.SelectedIndexChanged += new BEventHandler( comboVibratoType_SelectedIndexChanged );
            txtVibratoLength.TextChanged += new BEventHandler( txtVibratoLength_TextChanged );

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
            groupSelect.setTitle( _( "Select from" ) );
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

        /// <summary>
        /// ビブラートの選択肢の状態を更新します
        /// </summary>
        private void updateComboBoxStatus()
        {
            // 選択位置
            int old = comboVibratoType.SelectedIndex;

            // 全部削除
            comboVibratoType.removeAllItems();

            // 「ビブラート無し」を表すアイテムを追加
            VibratoHandle empty = new VibratoHandle();
            empty.setCaption( "[Non Vibrato]" );
            empty.IconID = "$04040000";
            comboVibratoType.addItem( empty );

            // 選択元を元に，選択肢を追加する
            if ( radioUserDefined.Checked ) {
                // ユーザー定義のを使う場合
                int size = AppManager.editorConfig.AutoVibratoCustom.size();
                for ( int i = 0; i < size; i++ ) {
                    VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom.get( i );
                    comboVibratoType.addItem( handle );
                }
            } else {
                // VOCALOID1/VOCALOID2のシステム定義のを使う場合
                SynthesizerType type = radioVocaloid1.Checked ? SynthesizerType.VOCALOID1 : SynthesizerType.VOCALOID2;
                for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                    VibratoHandle vconfig = itr.next();
                    comboVibratoType.addItem( vconfig );
                }
            }

            // 選択位置を戻せるなら戻す
            int index = old;
            if ( index >= comboVibratoType.getItemCount() ) {
                index = comboVibratoType.getItemCount() - 1;
            }
            if ( 0 <= index ) {
                comboVibratoType.setSelectedIndex( index );
            }
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new BEventHandler( btnOK_Click );
            btnCancel.Click += new BEventHandler( btnCancel_Click );
            radioUserDefined.CheckedChanged += new BEventHandler( handleRadioCheckedChanged );
            comboVibratoType.SelectedIndexChanged += comboVibratoType_SelectedIndexChanged;
        }

        void handleRadioCheckedChanged( Object sender, EventArgs e )
        {
            comboVibratoType.SelectedIndexChanged -= comboVibratoType_SelectedIndexChanged;
            updateComboBoxStatus();
            comboVibratoType.SelectedIndexChanged += comboVibratoType_SelectedIndexChanged;
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
                    if ( radioUserDefined.Checked ) {
                        int size = AppManager.editorConfig.AutoVibratoCustom.size();
                        for ( int i = 0; i < size; i++ ) {
                            VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom.get( i );
                            if ( s.Equals( handle.IconID ) ) {
                                src = handle;
                                break;
                            }
                        }
                    } else {
                        SynthesizerType type = radioVocaloid1.Checked ? SynthesizerType.VOCALOID1 : SynthesizerType.VOCALOID2;
                        for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
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
            this.lblVibratoLength = new org.kbinani.windows.forms.BLabel();
            this.lblVibratoType = new org.kbinani.windows.forms.BLabel();
            this.txtVibratoLength = new org.kbinani.cadencii.NumberTextBox();
            this.label3 = new org.kbinani.windows.forms.BLabel();
            this.comboVibratoType = new org.kbinani.windows.forms.BComboBox();
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.groupSelect = new BGroupBox();
            this.radioUserDefined = new BRadioButton();
            this.radioVocaloid2 = new BRadioButton();
            this.radioVocaloid1 = new BRadioButton();
            this.groupSelect.SuspendLayout();
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
            this.lblVibratoType.Location = new System.Drawing.Point( 12, 39 );
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
            this.comboVibratoType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboVibratoType.FormattingEnabled = true;
            this.comboVibratoType.Location = new System.Drawing.Point( 143, 36 );
            this.comboVibratoType.Name = "comboVibratoType";
            this.comboVibratoType.Size = new System.Drawing.Size( 167, 20 );
            this.comboVibratoType.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 240, 129 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 159, 129 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // groupSelect
            // 
            this.groupSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSelect.Controls.Add( this.radioUserDefined );
            this.groupSelect.Controls.Add( this.radioVocaloid2 );
            this.groupSelect.Controls.Add( this.radioVocaloid1 );
            this.groupSelect.Location = new System.Drawing.Point( 14, 61 );
            this.groupSelect.Name = "groupSelect";
            this.groupSelect.Size = new System.Drawing.Size( 301, 55 );
            this.groupSelect.TabIndex = 9;
            this.groupSelect.TabStop = false;
            this.groupSelect.Text = "Select from";
            // 
            // radioUserDefined
            // 
            this.radioUserDefined.AutoSize = true;
            this.radioUserDefined.Location = new System.Drawing.Point( 198, 21 );
            this.radioUserDefined.Name = "radioUserDefined";
            this.radioUserDefined.Size = new System.Drawing.Size( 88, 16 );
            this.radioUserDefined.TabIndex = 11;
            this.radioUserDefined.TabStop = true;
            this.radioUserDefined.Text = "User defined";
            this.radioUserDefined.UseVisualStyleBackColor = true;
            // 
            // radioVocaloid2
            // 
            this.radioVocaloid2.AutoSize = true;
            this.radioVocaloid2.Checked = true;
            this.radioVocaloid2.Location = new System.Drawing.Point( 106, 21 );
            this.radioVocaloid2.Name = "radioVocaloid2";
            this.radioVocaloid2.Size = new System.Drawing.Size( 86, 16 );
            this.radioVocaloid2.TabIndex = 10;
            this.radioVocaloid2.TabStop = true;
            this.radioVocaloid2.Text = "VOCALOID2";
            this.radioVocaloid2.UseVisualStyleBackColor = true;
            // 
            // radioVocaloid1
            // 
            this.radioVocaloid1.AutoSize = true;
            this.radioVocaloid1.Location = new System.Drawing.Point( 14, 21 );
            this.radioVocaloid1.Name = "radioVocaloid1";
            this.radioVocaloid1.Size = new System.Drawing.Size( 86, 16 );
            this.radioVocaloid1.TabIndex = 9;
            this.radioVocaloid1.TabStop = true;
            this.radioVocaloid1.Text = "VOCALOID1";
            this.radioVocaloid1.UseVisualStyleBackColor = true;
            // 
            // FormVibratoConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 327, 164 );
            this.Controls.Add( this.groupSelect );
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
            this.groupSelect.ResumeLayout( false );
            this.groupSelect.PerformLayout();
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
        private BGroupBox groupSelect;
        private BRadioButton radioVocaloid2;
        private BRadioButton radioVocaloid1;
        private BRadioButton radioUserDefined;
        #endregion
#endif
    }

#if !JAVA
}
#endif
