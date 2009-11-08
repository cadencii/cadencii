/*
 * FormVibratoConfig.cs
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
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;

    class FormVibratoConfig : BForm {
        private VibratoHandle m_vibrato;
        private int m_note_length;
        private SynthesizerType m_synthesizer_type;

        /// <summary>
        /// コンストラクタ．引数vibrato_handleには，Cloneしたものを渡さなくてよい．
        /// </summary>
        /// <param name="vibrato_handle"></param>
        public FormVibratoConfig( VibratoHandle vibrato_handle, int note_length, DefaultVibratoLength default_vibrato_length, SynthesizerType type ) {
#if DEBUG
            AppManager.debugWriteLine( "FormVibratoConfig.ctor(Vsqhandle,int,DefaultVibratoLength)" );
            AppManager.debugWriteLine( "    (vibrato_handle==null)=" + (vibrato_handle == null) );
            PortUtil.println( "    type=" + type );
#endif
            m_synthesizer_type = type;
            if ( vibrato_handle != null ) {
                m_vibrato = (VibratoHandle)vibrato_handle.clone();
            }

            InitializeComponent();
            registerEventHandlers();
            setResources();
            ApplyLanguage();

            comboVibratoType.Items.Clear();
            VibratoConfig empty = new VibratoConfig();
            empty.contents.Caption = "[Non Vibrato]";
            empty.contents.IconID = "$04040000";
            comboVibratoType.Items.Add( empty );
            comboVibratoType.SelectedIndex = 0;
            int count = 0;
            for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( m_synthesizer_type ); itr.hasNext(); ) {
                VibratoConfig vconfig = (VibratoConfig)itr.next();
                comboVibratoType.Items.Add( vconfig );
                count++;
                if ( vibrato_handle != null ) {
                    if ( vibrato_handle.IconID.Equals( vconfig.contents.IconID ) ) {
                        comboVibratoType.SelectedIndex = count;
                    }
                }
            }

            txtVibratoLength.Enabled = vibrato_handle != null;
            if ( vibrato_handle != null ) {
                txtVibratoLength.Text = (int)((float)vibrato_handle.Length / (float)note_length * 100.0f) + "";
            } else {
                switch ( default_vibrato_length ) {
                    case DefaultVibratoLength.L100:
                        txtVibratoLength.Text = "100";
                        break;
                    case DefaultVibratoLength.L50:
                        txtVibratoLength.Text = "50";
                        break;
                    case DefaultVibratoLength.L66:
                        txtVibratoLength.Text = "66";
                        break;
                    case DefaultVibratoLength.L75:
                        txtVibratoLength.Text = "75";
                        break;
                }
            }

            this.comboVibratoType.SelectedIndexChanged += new System.EventHandler( this.comboVibratoType_SelectedIndexChanged );
            this.txtVibratoLength.TextChanged += new System.EventHandler( txtVibratoLength_TextChanged );

            m_note_length = note_length;
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            Text = _( "Vibrato property" );
            lblVibratoLength.Text = _( "Vibrato length" ) + "(&L)";
            lblVibratoType.Text = _( "Vibrato Type" ) + "(&T)";
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        /// <summary>
        /// 編集済みのビブラート設定．既にCloneされているので，改めてCloneしなくて良い
        /// </summary>
        public VibratoHandle VibratoHandle {
            get {
                return m_vibrato;
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
        }

        private void comboVibratoType_SelectedIndexChanged( object sender, EventArgs e ) {
            int index = comboVibratoType.SelectedIndex;
            if ( index >= 0 ) {
                String s = ((VibratoConfig)comboVibratoType.Items[index]).contents.IconID;
                if ( s.Equals( "$04040000" ) ) {
                    m_vibrato = null;
                    txtVibratoLength.Enabled = false;
                    return;
                } else {
                    txtVibratoLength.Enabled = true;
                    for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( m_synthesizer_type ); itr.hasNext(); ) {
                        VibratoConfig vconfig = (VibratoConfig)itr.next();
                        if ( s.Equals( vconfig.contents.IconID ) ) {
                            int percent;
                            try {
                                percent = int.Parse( txtVibratoLength.Text );
                            } catch {
                                return;
                            }
                            m_vibrato = (VibratoHandle)vconfig.contents.clone();
                            m_vibrato.Length = (int)(m_note_length * percent / 100.0f);
                            return;
                        }
                    }
                }
            }
        }

        private void txtVibratoLength_TextChanged( object sender, System.EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "txtVibratoLength_TextChanged" );
            AppManager.debugWriteLine( "    (m_vibrato==null)=" + (m_vibrato == null) );
#endif
            int percent = 0;
            try {
                percent = int.Parse( txtVibratoLength.Text );
                if ( percent < 0 ) {
                    percent = 0;
                } else if ( 100 < percent ) {
                    percent = 100;
                }
            } catch {
                return;
            }
            if ( percent == 0 ) {
                m_vibrato = null;
                txtVibratoLength.Enabled = false;
            } else {
                if ( m_vibrato != null ) {
                    int new_length = (int)(m_note_length * percent / 100.0f);
                    m_vibrato.Length = new_length;
                }
            }
        }

        private void registerEventHandlers() {
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
        }

        private void setResources() {
        }

#if JAVA
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
            this.lblVibratoLength = new System.Windows.Forms.Label();
            this.lblVibratoType = new System.Windows.Forms.Label();
            this.txtVibratoLength = new Boare.Cadencii.NumberTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboVibratoType = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
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
            this.txtVibratoLength.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
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

        private System.Windows.Forms.Label lblVibratoLength;
        private System.Windows.Forms.Label lblVibratoType;
        private NumberTextBox txtVibratoLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboVibratoType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        #endregion
#endif
    }

}
