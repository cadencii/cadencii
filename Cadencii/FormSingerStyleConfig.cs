/*
 * FormSingerStypeConfig.cs
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
using System.Drawing;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = Boolean;

    class FormSingerStyleConfig : BForm {
        boolean m_apply_current_track = false;

        public void ApplyLanguage() {
            lblTemplate.Text = _( "Template" ) + "(&T)";
            groupPitchControl.Text = _( "Pitch Control" );
            lblBendDepth.Text = _( "Bend Depth" ) + "(&B)";
            lblBendLength.Text = _( "Bend Length" ) + "(&L)";
            chkUpPortamento.Text = _( "Add portamento in rising movement" ) + "(&R)";
            chkDownPortamento.Text = _( "Add portamento in falling movement" ) + "(&F)";

            groupDynamicsControl.Text = _( "Dynamics Control" );
            lblDecay.Text = _( "Decay" ) + "(&D)";
            lblAccent.Text = _( "Accent" ) + "(&A)";

            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
            btnApply.Text = _( "Apply to current track" ) + "(&C)";

            lblTemplate.Left = comboTemplate.Left - lblTemplate.Width;
            this.Text = _( "Default Singer Style" );
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public int PMBendDepth {
            get {
                return trackBendDepth.Value;
            }
            set {
                trackBendDepth.Value = value;
                txtBendDepth.Text = value + "";
            }
        }

        public int PMBendLength {
            get {
                return trackBendLength.Value;
            }
            set {
                trackBendLength.Value = value;
                txtBendLength.Text = value + "";
            }
        }

        public int PMbPortamentoUse {
            get {
                int ret = 0;
                if ( chkUpPortamento.Checked ) {
                    ret += 1;
                }
                if ( chkDownPortamento.Checked ) {
                    ret += 2;
                }
                return ret;
            }
            set {
                if ( value % 2 == 1 ) {
                    chkUpPortamento.Checked = true;
                } else {
                    chkUpPortamento.Checked = false;
                }
                if ( value >= 2 ) {
                    chkDownPortamento.Checked = true;
                } else {
                    chkDownPortamento.Checked = false;
                }
            }
        }

        public int DEMdecGainRate {
            get {
                return trackDecay.Value;
            }
            set {
                trackDecay.Value = value;
                txtDecay.Text = value + "";
            }
        }
        
        public int DEMaccent {
            get {
                return trackAccent.Value;
            }
            set {
                trackAccent.Value = value;
                txtAccent.Text = value + "";
            }
        }

        public FormSingerStyleConfig() {
            InitializeComponent();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            ApplyLanguage();
            Size current_size = this.ClientSize;
        }

        private void trackBendDepth_Scroll( object sender, EventArgs e ) {
            txtBendDepth.Text = trackBendDepth.Value + "";
        }

        private void txtBendDepth_TextChanged( object sender, EventArgs e ) {
            try {
                int draft = int.Parse( txtBendDepth.Text );
                if ( draft != trackBendDepth.Value ) {
                    if ( draft < trackBendDepth.Minimum ) {
                        draft = trackBendDepth.Minimum;
                        txtBendDepth.Text = draft + "";
                    } else if ( trackBendDepth.Maximum < draft ) {
                        draft = trackBendDepth.Maximum;
                        txtBendDepth.Text = draft + "";
                    }
                    trackBendDepth.Value = draft;
                }
            } catch {
                //txtBendDepth.Text = trackBendDepth.Value + "";
            }
        }

        private void trackBendLength_Scroll( object sender, EventArgs e ) {
            txtBendLength.Text = trackBendLength.Value + "";
        }

        private void txtBendLength_TextChanged( object sender, EventArgs e ) {
            try {
                int draft = int.Parse( txtBendLength.Text );
                if ( draft != trackBendLength.Value ) {
                    if ( draft < trackBendLength.Minimum ) {
                        draft = trackBendLength.Minimum;
                        txtBendLength.Text = draft + "";
                    } else if ( trackBendLength.Maximum < draft ) {
                        draft = trackBendLength.Maximum;
                        txtBendLength.Text = draft + "";
                    }
                    trackBendLength.Value = draft;
                }
            } catch {
                //txtBendLength.Text = trackBendLength.Value + "";
            }
        }

        private void trackDecay_Scroll( object sender, EventArgs e ) {
            txtDecay.Text = trackDecay.Value + "";
        }

        private void txtDecay_TextChanged( object sender, EventArgs e ) {
            try {
                int draft = int.Parse( txtDecay.Text );
                if ( draft != trackDecay.Value ) {
                    if ( draft < trackDecay.Minimum ) {
                        draft = trackDecay.Minimum;
                        txtDecay.Text = draft + "";
                    } else if ( trackDecay.Maximum < draft ) {
                        draft = trackDecay.Maximum;
                        txtDecay.Text = draft + "";
                    }
                    trackDecay.Value = draft;
                }
            } catch {
                //txtDecay.Text = trackDecay.Value + "";
            }
        }

        private void trackAccent_Scroll( object sender, EventArgs e ) {
            txtAccent.Text = trackAccent.Value + "";
        }

        private void txtAccent_TextChanged( object sender, EventArgs e ) {
            try {
                int draft = int.Parse( txtAccent.Text );
                if ( draft != trackAccent.Value ) {
                    if ( draft < trackAccent.Minimum ) {
                        draft = trackAccent.Minimum;
                        txtAccent.Text = draft + "";
                    } else if ( trackAccent.Maximum < draft ) {
                        draft = trackAccent.Maximum;
                        txtAccent.Text = draft + "";
                    }
                    trackAccent.Value = draft;
                }
            } catch {
                //txtAccent.Text = trackAccent.Value + "";
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged( object sender, EventArgs e ) {
            switch ( comboTemplate.SelectedIndex ) {
                case 1:
                    PMBendDepth = 8;
                    PMBendLength = 0;
                    PMbPortamentoUse = 0;
                    DEMdecGainRate = 50;
                    DEMaccent = 50;
                    break;
                case 2:
                    PMBendDepth = 8;
                    PMBendLength = 0;
                    PMbPortamentoUse = 0;
                    DEMdecGainRate = 50;
                    DEMaccent = 68;
                    break;
                case 3:
                    PMBendDepth = 8;
                    PMBendLength = 0;
                    PMbPortamentoUse = 0;
                    DEMdecGainRate = 70;
                    DEMaccent = 80;
                    break;
                case 4:
                    PMBendDepth = 20;
                    PMBendLength = 0;
                    PMbPortamentoUse = 3;
                    DEMdecGainRate = 50;
                    DEMaccent = 42;
                    break;
                case 5:
                    PMBendDepth = 20;
                    PMBendLength = 0;
                    PMbPortamentoUse = 3;
                    DEMdecGainRate = 50;
                    DEMaccent = 25;
                    break;
            }
        }

        private void btnApply_Click( object sender, EventArgs e ) {
            if ( AppManager.showMessageBox( _( "Would you like to change singer style for all events?" ),
                                  FormMain._APP_NAME, 
                                  AppManager.MSGBOX_YES_NO_OPTION, 
                                  AppManager.MSGBOX_WARNING_MESSAGE ) == BDialogResult.YES ) {
                m_apply_current_track = true;
                DialogResult = DialogResult.OK;
            }
        }

        public boolean ApplyCurrentTrack {
            get {
                return m_apply_current_track;
            }
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
            this.groupPitchControl = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBendLength = new Boare.Cadencii.NumberTextBox();
            this.txtBendDepth = new Boare.Cadencii.NumberTextBox();
            this.trackBendLength = new System.Windows.Forms.TrackBar();
            this.trackBendDepth = new System.Windows.Forms.TrackBar();
            this.chkDownPortamento = new System.Windows.Forms.CheckBox();
            this.chkUpPortamento = new System.Windows.Forms.CheckBox();
            this.lblBendLength = new System.Windows.Forms.Label();
            this.lblBendDepth = new System.Windows.Forms.Label();
            this.groupDynamicsControl = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAccent = new Boare.Cadencii.NumberTextBox();
            this.txtDecay = new Boare.Cadencii.NumberTextBox();
            this.trackAccent = new System.Windows.Forms.TrackBar();
            this.trackDecay = new System.Windows.Forms.TrackBar();
            this.lblAccent = new System.Windows.Forms.Label();
            this.lblDecay = new System.Windows.Forms.Label();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.comboTemplate = new System.Windows.Forms.ComboBox();
            this.groupPitchControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendDepth)).BeginInit();
            this.groupDynamicsControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAccent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDecay)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPitchControl
            // 
            this.groupPitchControl.Controls.Add( this.label5 );
            this.groupPitchControl.Controls.Add( this.label4 );
            this.groupPitchControl.Controls.Add( this.txtBendLength );
            this.groupPitchControl.Controls.Add( this.txtBendDepth );
            this.groupPitchControl.Controls.Add( this.trackBendLength );
            this.groupPitchControl.Controls.Add( this.trackBendDepth );
            this.groupPitchControl.Controls.Add( this.chkDownPortamento );
            this.groupPitchControl.Controls.Add( this.chkUpPortamento );
            this.groupPitchControl.Controls.Add( this.lblBendLength );
            this.groupPitchControl.Controls.Add( this.lblBendDepth );
            this.groupPitchControl.Location = new System.Drawing.Point( 8, 38 );
            this.groupPitchControl.Name = "groupPitchControl";
            this.groupPitchControl.Size = new System.Drawing.Size( 367, 130 );
            this.groupPitchControl.TabIndex = 0;
            this.groupPitchControl.TabStop = false;
            this.groupPitchControl.Text = "Pitch Control";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 345, 42 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 11, 12 );
            this.label5.TabIndex = 9;
            this.label5.Text = "%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 345, 16 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 11, 12 );
            this.label4.TabIndex = 8;
            this.label4.Text = "%";
            // 
            // txtBendLength
            // 
            this.txtBendLength.BackColor = System.Drawing.SystemColors.Window;
            this.txtBendLength.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBendLength.Location = new System.Drawing.Point( 300, 39 );
            this.txtBendLength.Name = "txtBendLength";
            this.txtBendLength.Size = new System.Drawing.Size( 39, 19 );
            this.txtBendLength.TabIndex = 5;
            this.txtBendLength.Text = "0";
            this.txtBendLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBendLength.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            this.txtBendLength.TextChanged += new System.EventHandler( this.txtBendLength_TextChanged );
            // 
            // txtBendDepth
            // 
            this.txtBendDepth.BackColor = System.Drawing.SystemColors.Window;
            this.txtBendDepth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBendDepth.Location = new System.Drawing.Point( 300, 13 );
            this.txtBendDepth.Name = "txtBendDepth";
            this.txtBendDepth.Size = new System.Drawing.Size( 39, 19 );
            this.txtBendDepth.TabIndex = 2;
            this.txtBendDepth.Text = "8";
            this.txtBendDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBendDepth.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            this.txtBendDepth.TextChanged += new System.EventHandler( this.txtBendDepth_TextChanged );
            // 
            // trackBendLength
            // 
            this.trackBendLength.AutoSize = false;
            this.trackBendLength.Location = new System.Drawing.Point( 138, 40 );
            this.trackBendLength.Maximum = 100;
            this.trackBendLength.Name = "trackBendLength";
            this.trackBendLength.Size = new System.Drawing.Size( 156, 18 );
            this.trackBendLength.TabIndex = 4;
            this.trackBendLength.TickFrequency = 10;
            this.trackBendLength.Scroll += new System.EventHandler( this.trackBendLength_Scroll );
            // 
            // trackBendDepth
            // 
            this.trackBendDepth.AutoSize = false;
            this.trackBendDepth.Location = new System.Drawing.Point( 138, 14 );
            this.trackBendDepth.Maximum = 100;
            this.trackBendDepth.Name = "trackBendDepth";
            this.trackBendDepth.Size = new System.Drawing.Size( 156, 18 );
            this.trackBendDepth.TabIndex = 1;
            this.trackBendDepth.TickFrequency = 10;
            this.trackBendDepth.Value = 8;
            this.trackBendDepth.Scroll += new System.EventHandler( this.trackBendDepth_Scroll );
            // 
            // chkDownPortamento
            // 
            this.chkDownPortamento.AutoSize = true;
            this.chkDownPortamento.Location = new System.Drawing.Point( 20, 96 );
            this.chkDownPortamento.Name = "chkDownPortamento";
            this.chkDownPortamento.Size = new System.Drawing.Size( 224, 16 );
            this.chkDownPortamento.TabIndex = 7;
            this.chkDownPortamento.Text = "Add portamento in falling movement(&F)";
            this.chkDownPortamento.UseVisualStyleBackColor = true;
            // 
            // chkUpPortamento
            // 
            this.chkUpPortamento.AutoSize = true;
            this.chkUpPortamento.Location = new System.Drawing.Point( 20, 71 );
            this.chkUpPortamento.Name = "chkUpPortamento";
            this.chkUpPortamento.Size = new System.Drawing.Size( 222, 16 );
            this.chkUpPortamento.TabIndex = 6;
            this.chkUpPortamento.Text = "Add portamento in rising movement(&R)";
            this.chkUpPortamento.UseVisualStyleBackColor = true;
            // 
            // lblBendLength
            // 
            this.lblBendLength.AutoSize = true;
            this.lblBendLength.Location = new System.Drawing.Point( 20, 46 );
            this.lblBendLength.Name = "lblBendLength";
            this.lblBendLength.Size = new System.Drawing.Size( 83, 12 );
            this.lblBendLength.TabIndex = 3;
            this.lblBendLength.Text = "Bend Length(&L)";
            // 
            // lblBendDepth
            // 
            this.lblBendDepth.AutoSize = true;
            this.lblBendDepth.Location = new System.Drawing.Point( 20, 20 );
            this.lblBendDepth.Name = "lblBendDepth";
            this.lblBendDepth.Size = new System.Drawing.Size( 81, 12 );
            this.lblBendDepth.TabIndex = 0;
            this.lblBendDepth.Text = "Bend Depth(&B)";
            // 
            // groupDynamicsControl
            // 
            this.groupDynamicsControl.Controls.Add( this.label7 );
            this.groupDynamicsControl.Controls.Add( this.label6 );
            this.groupDynamicsControl.Controls.Add( this.txtAccent );
            this.groupDynamicsControl.Controls.Add( this.txtDecay );
            this.groupDynamicsControl.Controls.Add( this.trackAccent );
            this.groupDynamicsControl.Controls.Add( this.trackDecay );
            this.groupDynamicsControl.Controls.Add( this.lblAccent );
            this.groupDynamicsControl.Controls.Add( this.lblDecay );
            this.groupDynamicsControl.Location = new System.Drawing.Point( 8, 174 );
            this.groupDynamicsControl.Name = "groupDynamicsControl";
            this.groupDynamicsControl.Size = new System.Drawing.Size( 367, 74 );
            this.groupDynamicsControl.TabIndex = 1;
            this.groupDynamicsControl.TabStop = false;
            this.groupDynamicsControl.Text = "Dynamics Control";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 345, 46 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 11, 12 );
            this.label7.TabIndex = 11;
            this.label7.Text = "%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 345, 20 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 11, 12 );
            this.label6.TabIndex = 10;
            this.label6.Text = "%";
            // 
            // txtAccent
            // 
            this.txtAccent.BackColor = System.Drawing.SystemColors.Window;
            this.txtAccent.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtAccent.Location = new System.Drawing.Point( 300, 43 );
            this.txtAccent.Name = "txtAccent";
            this.txtAccent.Size = new System.Drawing.Size( 39, 19 );
            this.txtAccent.TabIndex = 13;
            this.txtAccent.Text = "50";
            this.txtAccent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAccent.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            this.txtAccent.TextChanged += new System.EventHandler( this.txtAccent_TextChanged );
            // 
            // txtDecay
            // 
            this.txtDecay.BackColor = System.Drawing.SystemColors.Window;
            this.txtDecay.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDecay.Location = new System.Drawing.Point( 300, 17 );
            this.txtDecay.Name = "txtDecay";
            this.txtDecay.Size = new System.Drawing.Size( 39, 19 );
            this.txtDecay.TabIndex = 10;
            this.txtDecay.Text = "50";
            this.txtDecay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDecay.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            this.txtDecay.TextChanged += new System.EventHandler( this.txtDecay_TextChanged );
            // 
            // trackAccent
            // 
            this.trackAccent.AutoSize = false;
            this.trackAccent.Location = new System.Drawing.Point( 138, 44 );
            this.trackAccent.Maximum = 100;
            this.trackAccent.Name = "trackAccent";
            this.trackAccent.Size = new System.Drawing.Size( 156, 18 );
            this.trackAccent.TabIndex = 12;
            this.trackAccent.TickFrequency = 10;
            this.trackAccent.Value = 50;
            this.trackAccent.Scroll += new System.EventHandler( this.trackAccent_Scroll );
            // 
            // trackDecay
            // 
            this.trackDecay.AutoSize = false;
            this.trackDecay.Location = new System.Drawing.Point( 138, 18 );
            this.trackDecay.Maximum = 100;
            this.trackDecay.Name = "trackDecay";
            this.trackDecay.Size = new System.Drawing.Size( 156, 18 );
            this.trackDecay.TabIndex = 9;
            this.trackDecay.TickFrequency = 10;
            this.trackDecay.Value = 50;
            this.trackDecay.Scroll += new System.EventHandler( this.trackDecay_Scroll );
            // 
            // lblAccent
            // 
            this.lblAccent.AutoSize = true;
            this.lblAccent.Location = new System.Drawing.Point( 18, 50 );
            this.lblAccent.Name = "lblAccent";
            this.lblAccent.Size = new System.Drawing.Size( 57, 12 );
            this.lblAccent.TabIndex = 11;
            this.lblAccent.Text = "Accent(&A)";
            // 
            // lblDecay
            // 
            this.lblDecay.AutoSize = true;
            this.lblDecay.Location = new System.Drawing.Point( 18, 24 );
            this.lblDecay.Name = "lblDecay";
            this.lblDecay.Size = new System.Drawing.Size( 53, 12 );
            this.lblDecay.TabIndex = 8;
            this.lblDecay.Text = "Decay(&D)";
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Location = new System.Drawing.Point( 163, 15 );
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size( 67, 12 );
            this.lblTemplate.TabIndex = 2;
            this.lblTemplate.Text = "Template(&T)";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 290, 259 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 78, 23 );
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 203, 259 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 78, 23 );
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point( 8, 259 );
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size( 153, 23 );
            this.btnApply.TabIndex = 14;
            this.btnApply.Text = "Apply to current track(&C)";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler( this.btnApply_Click );
            // 
            // comboTemplate
            // 
            this.comboTemplate.FormattingEnabled = true;
            this.comboTemplate.Items.AddRange( new object[] {
            "[Select a template]",
            "normal",
            "accent",
            "strong accent",
            "legato",
            "slow legato"} );
            this.comboTemplate.Location = new System.Drawing.Point( 253, 12 );
            this.comboTemplate.Name = "comboTemplate";
            this.comboTemplate.Size = new System.Drawing.Size( 121, 20 );
            this.comboTemplate.TabIndex = 0;
            this.comboTemplate.Text = "[Select a template]";
            this.comboTemplate.SelectedIndexChanged += new System.EventHandler( this.comboBox1_SelectedIndexChanged );
            // 
            // FormSingerStyleConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 384, 308 );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnApply );
            this.Controls.Add( this.lblTemplate );
            this.Controls.Add( this.groupDynamicsControl );
            this.Controls.Add( this.groupPitchControl );
            this.Controls.Add( this.comboTemplate );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size( 6000, 6000 );
            this.MinimizeBox = false;
            this.Name = "FormSingerStyleConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Default Singer Style";
            this.groupPitchControl.ResumeLayout( false );
            this.groupPitchControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendDepth)).EndInit();
            this.groupDynamicsControl.ResumeLayout( false );
            this.groupDynamicsControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAccent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDecay)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupPitchControl;
        private System.Windows.Forms.GroupBox groupDynamicsControl;
        private System.Windows.Forms.Label lblBendDepth;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.Label lblBendLength;
        private System.Windows.Forms.CheckBox chkDownPortamento;
        private System.Windows.Forms.CheckBox chkUpPortamento;
        private System.Windows.Forms.TrackBar trackBendDepth;
        private System.Windows.Forms.TrackBar trackBendLength;
        private System.Windows.Forms.TrackBar trackAccent;
        private System.Windows.Forms.TrackBar trackDecay;
        private System.Windows.Forms.Label lblAccent;
        private System.Windows.Forms.Label lblDecay;
        private NumberTextBox txtBendLength;
        private NumberTextBox txtBendDepth;
        private NumberTextBox txtAccent;
        private NumberTextBox txtDecay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox comboTemplate;
        #endregion
#endif
    }

}
