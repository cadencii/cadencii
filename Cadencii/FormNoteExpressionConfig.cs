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
using Boare.Lib.Vsq;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;

    class FormNoteExpressionConfig : BForm {
        boolean m_apply_current_track = false;
        NoteHeadHandle m_note_head_handle = null;

        public NoteHeadHandle getEditedNoteHeadHandle() {
            return m_note_head_handle;
        }

        public void ApplyLanguage() {
            lblTemplate.Text = _( "Template" ) + "(&T)";
            groupPitchControl.Text = _( "Pitch Control" );
            lblBendDepth.Text = _( "Bend Depth" ) + "(&B)";
            lblBendLength.Text = _( "Bend Length" ) + "(&L)";
            chkUpPortamento.Text = _( "Add portamento in rising movement" ) + "(&R)";
            chkDownPortamento.Text = _( "Add portamento in falling movement" ) + "(&F)";

            groupAttack.Text = _( "Attack Control (VOCALOID1)" );
            groupDynamicsControl.Text = _( "Dynamics Control (VOCALOID2)" );
            lblDecay.Text = _( "Decay" ) + "(&D)";
            lblAccent.Text = _( "Accent" ) + "(&A)";

            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );

            lblTemplate.Left = comboTemplate.Left - lblTemplate.Width;
            this.Text = _( "Expression control property" );
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

        public FormNoteExpressionConfig( SynthesizerType type, NoteHeadHandle note_head_handle ) {
            if ( note_head_handle != null ) {
                m_note_head_handle = (NoteHeadHandle)note_head_handle.clone();
            }
            InitializeComponent();
            registerEventHandlers();
            setResources();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            ApplyLanguage();
            if ( type == SynthesizerType.VOCALOID1 ) {
                flowLayoutPanel.Controls.Remove( groupDynamicsControl );
                flowLayoutPanel.Controls.Remove( panelVocaloid2Template );
                flowLayoutPanel.Controls.Remove( groupPitchControl );
            } else {
                flowLayoutPanel.Controls.Remove( groupAttack );
            }

            //comboAttackTemplateを更新
            AttackConfig empty = new AttackConfig();
            comboAttackTemplate.Items.Clear();
            empty.contents.IconID = "$01010000";
            empty.contents.Caption = "[Non Attack]";
            comboAttackTemplate.Items.Add( empty );
            comboAttackTemplate.SelectedIndex = 0;
            String icon_id = "";
            if ( m_note_head_handle != null ) {
                icon_id = m_note_head_handle.IconID;
                txtDuration.Text = m_note_head_handle.Duration + "";
                txtDepth.Text = m_note_head_handle.Depth + "";
            } else {
                txtDuration.Enabled = false;
                txtDepth.Enabled = false;
                trackDuration.Enabled = false;
                trackDepth.Enabled = false;
            }
            for ( Iterator itr = VocaloSysUtil.attackConfigIterator( SynthesizerType.VOCALOID1 ); itr.hasNext(); ) {
                AttackConfig item = (AttackConfig)itr.next();
                comboAttackTemplate.Items.Add( item );
                if ( item.contents.IconID.Equals( icon_id ) ) {
                    comboAttackTemplate.SelectedIndex = comboAttackTemplate.Items.Count - 1;
                }
            }
            comboAttackTemplate.SelectedIndexChanged += new EventHandler( comboAttackTemplate_SelectedIndexChanged );

            Size current_size = this.ClientSize;
            this.ClientSize = new Size( current_size.Width, flowLayoutPanel.ClientSize.Height + flowLayoutPanel.Top * 2 );
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void comboAttackTemplate_SelectedIndexChanged( object sender, EventArgs e ) {
            int index = comboAttackTemplate.SelectedIndex;
            if ( index < 0 ) {
                return;
            }
            if ( index == 0 ) {
                m_note_head_handle = null;
                txtDuration.Enabled = false;
                trackDuration.Enabled = false;
                txtDepth.Enabled = false;
                trackDepth.Enabled = false;
                return;
            }
            txtDuration.Enabled = true;
            trackDuration.Enabled = true;
            txtDepth.Enabled = true;
            trackDepth.Enabled = true;
            AttackConfig aconfig = (AttackConfig)comboAttackTemplate.SelectedItem;
            if ( m_note_head_handle == null ) {
                txtDuration.Text = aconfig.contents.Duration + "";
                txtDepth.Text = aconfig.contents.Depth + "";
            }
            m_note_head_handle = (NoteHeadHandle)aconfig.contents.clone();
            m_note_head_handle.Duration = trackDuration.Value;
            m_note_head_handle.Depth = trackDepth.Value;
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

        private void btnApply_Click( Object sender, BEventArgs e ) {
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

        private void FormSingerStyleConfig_Load( Object sender, BEventArgs e ) {

        }

        private void trackDuration_Scroll( Object sender, BEventArgs e ) {
            txtDuration.Text = trackDuration.Value + "";
            if ( m_note_head_handle != null ) {
                m_note_head_handle.Duration = trackDuration.Value;
            }
        }

        private void trackDepth_Scroll( object sender, EventArgs e ) {
            txtDepth.Text = trackDepth.Value + "";
            if ( m_note_head_handle != null ) {
                m_note_head_handle.Depth = trackDepth.Value;
            }
        }

        private void txtDuration_TextChanged( object sender, EventArgs e ) {
            try {
                int draft = int.Parse( txtDuration.Text );
                if ( draft != trackDuration.Value ) {
                    if ( draft < trackDuration.Minimum ) {
                        draft = trackDuration.Minimum;
                    } else if ( trackDuration.Maximum < draft ) {
                        draft = trackDuration.Maximum;
                    }
                    txtDuration.Text = draft + "";
                    trackDuration.Value = draft;
                    if ( m_note_head_handle != null ) {
                        m_note_head_handle.Duration = draft;
                    }
                }
            } catch {
            }
        }

        private void txtDepth_TextChanged( object sender, EventArgs e ) {
            try {
                int draft = int.Parse( txtDepth.Text );
                if ( draft != trackDepth.Value ) {
                    if ( draft < trackDepth.Minimum ) {
                        draft = trackDepth.Minimum;
                    } else if ( trackDepth.Maximum < draft ) {
                        draft = trackDepth.Maximum;
                    }
                    txtDepth.Text = draft + "";
                    trackDepth.Value = draft;
                    if ( m_note_head_handle != null ) {
                        m_note_head_handle.Depth = trackDepth.Value;
                    }
                }
            } catch {
            }
        }

        private void registerEventHandlers() {
            this.txtBendLength.TextChanged += new System.EventHandler( this.txtBendLength_TextChanged );
            this.txtBendDepth.TextChanged += new System.EventHandler( this.txtBendDepth_TextChanged );
            this.trackBendLength.Scroll += new System.EventHandler( this.trackBendLength_Scroll );
            this.trackBendDepth.Scroll += new System.EventHandler( this.trackBendDepth_Scroll );
            this.txtAccent.TextChanged += new System.EventHandler( this.txtAccent_TextChanged );
            this.txtDecay.TextChanged += new System.EventHandler( this.txtDecay_TextChanged );
            this.trackAccent.Scroll += new System.EventHandler( this.trackAccent_Scroll );
            this.trackDecay.Scroll += new System.EventHandler( this.trackDecay_Scroll );
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            this.comboTemplate.SelectedIndexChanged += new System.EventHandler( this.comboBox1_SelectedIndexChanged );
            this.txtDepth.TextChanged += new System.EventHandler( this.txtDepth_TextChanged );
            this.txtDuration.TextChanged += new System.EventHandler( this.txtDuration_TextChanged );
            this.trackDepth.Scroll += new System.EventHandler( this.trackDepth_Scroll );
            this.trackDuration.Scroll += new System.EventHandler( this.trackDuration_Scroll );
            this.Load += new System.EventHandler( this.FormSingerStyleConfig_Load );
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
            this.comboTemplate = new System.Windows.Forms.ComboBox();
            this.groupAttack = new System.Windows.Forms.GroupBox();
            this.lblAttackTemplate = new System.Windows.Forms.Label();
            this.comboAttackTemplate = new System.Windows.Forms.ComboBox();
            this.txtDepth = new Boare.Cadencii.NumberTextBox();
            this.txtDuration = new Boare.Cadencii.NumberTextBox();
            this.trackDepth = new System.Windows.Forms.TrackBar();
            this.trackDuration = new System.Windows.Forms.TrackBar();
            this.lblDepth = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelVocaloid2Template = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.groupPitchControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendDepth)).BeginInit();
            this.groupDynamicsControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAccent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDecay)).BeginInit();
            this.groupAttack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDuration)).BeginInit();
            this.flowLayoutPanel.SuspendLayout();
            this.panelVocaloid2Template.SuspendLayout();
            this.panelButtons.SuspendLayout();
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
            this.groupPitchControl.Location = new System.Drawing.Point( 3, 38 );
            this.groupPitchControl.Name = "groupPitchControl";
            this.groupPitchControl.Size = new System.Drawing.Size( 367, 130 );
            this.groupPitchControl.TabIndex = 0;
            this.groupPitchControl.TabStop = false;
            this.groupPitchControl.Text = "Pitch Control (VOCALOID2)";
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
            this.groupDynamicsControl.Location = new System.Drawing.Point( 3, 174 );
            this.groupDynamicsControl.Name = "groupDynamicsControl";
            this.groupDynamicsControl.Size = new System.Drawing.Size( 367, 74 );
            this.groupDynamicsControl.TabIndex = 1;
            this.groupDynamicsControl.TabStop = false;
            this.groupDynamicsControl.Text = "Dynamics Control (VOCALOID2)";
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
            this.lblTemplate.Location = new System.Drawing.Point( 165, 6 );
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size( 67, 12 );
            this.lblTemplate.TabIndex = 2;
            this.lblTemplate.Text = "Template(&T)";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 285, 12 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 78, 23 );
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 198, 12 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 78, 23 );
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
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
            this.comboTemplate.Location = new System.Drawing.Point( 242, 3 );
            this.comboTemplate.Name = "comboTemplate";
            this.comboTemplate.Size = new System.Drawing.Size( 121, 20 );
            this.comboTemplate.TabIndex = 0;
            this.comboTemplate.Text = "[Select a template]";
            // 
            // groupAttack
            // 
            this.groupAttack.Controls.Add( this.lblAttackTemplate );
            this.groupAttack.Controls.Add( this.comboAttackTemplate );
            this.groupAttack.Controls.Add( this.txtDepth );
            this.groupAttack.Controls.Add( this.txtDuration );
            this.groupAttack.Controls.Add( this.trackDepth );
            this.groupAttack.Controls.Add( this.trackDuration );
            this.groupAttack.Controls.Add( this.lblDepth );
            this.groupAttack.Controls.Add( this.lblDuration );
            this.groupAttack.Location = new System.Drawing.Point( 3, 254 );
            this.groupAttack.Name = "groupAttack";
            this.groupAttack.Size = new System.Drawing.Size( 367, 107 );
            this.groupAttack.TabIndex = 17;
            this.groupAttack.TabStop = false;
            this.groupAttack.Text = "Attack (VOCALOID1)";
            // 
            // lblAttackTemplate
            // 
            this.lblAttackTemplate.AutoSize = true;
            this.lblAttackTemplate.Location = new System.Drawing.Point( 18, 23 );
            this.lblAttackTemplate.Name = "lblAttackTemplate";
            this.lblAttackTemplate.Size = new System.Drawing.Size( 105, 12 );
            this.lblAttackTemplate.TabIndex = 2;
            this.lblAttackTemplate.Text = "Attack Variation(&V)";
            // 
            // comboAttackTemplate
            // 
            this.comboAttackTemplate.FormattingEnabled = true;
            this.comboAttackTemplate.Location = new System.Drawing.Point( 143, 20 );
            this.comboAttackTemplate.Name = "comboAttackTemplate";
            this.comboAttackTemplate.Size = new System.Drawing.Size( 121, 20 );
            this.comboAttackTemplate.TabIndex = 0;
            this.comboAttackTemplate.Text = "[Non Attack]";
            // 
            // txtDepth
            // 
            this.txtDepth.BackColor = System.Drawing.SystemColors.Window;
            this.txtDepth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDepth.Location = new System.Drawing.Point( 300, 72 );
            this.txtDepth.Name = "txtDepth";
            this.txtDepth.Size = new System.Drawing.Size( 39, 19 );
            this.txtDepth.TabIndex = 13;
            this.txtDepth.Text = "64";
            this.txtDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDepth.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDuration
            // 
            this.txtDuration.BackColor = System.Drawing.SystemColors.Window;
            this.txtDuration.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDuration.Location = new System.Drawing.Point( 300, 46 );
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size( 39, 19 );
            this.txtDuration.TabIndex = 10;
            this.txtDuration.Text = "64";
            this.txtDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDuration.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // trackDepth
            // 
            this.trackDepth.AutoSize = false;
            this.trackDepth.Location = new System.Drawing.Point( 138, 69 );
            this.trackDepth.Maximum = 127;
            this.trackDepth.Name = "trackDepth";
            this.trackDepth.Size = new System.Drawing.Size( 156, 18 );
            this.trackDepth.TabIndex = 12;
            this.trackDepth.TickFrequency = 10;
            this.trackDepth.Value = 64;
            // 
            // trackDuration
            // 
            this.trackDuration.AutoSize = false;
            this.trackDuration.Location = new System.Drawing.Point( 138, 43 );
            this.trackDuration.Maximum = 127;
            this.trackDuration.Name = "trackDuration";
            this.trackDuration.Size = new System.Drawing.Size( 156, 18 );
            this.trackDuration.TabIndex = 9;
            this.trackDuration.TickFrequency = 10;
            this.trackDuration.Value = 64;
            // 
            // lblDepth
            // 
            this.lblDepth.AutoSize = true;
            this.lblDepth.Location = new System.Drawing.Point( 18, 75 );
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size( 51, 12 );
            this.lblDepth.TabIndex = 11;
            this.lblDepth.Text = "Depth(&D)";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point( 18, 49 );
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size( 64, 12 );
            this.lblDuration.TabIndex = 8;
            this.lblDuration.Text = "Duration(&D)";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.Controls.Add( this.panelVocaloid2Template );
            this.flowLayoutPanel.Controls.Add( this.groupPitchControl );
            this.flowLayoutPanel.Controls.Add( this.groupDynamicsControl );
            this.flowLayoutPanel.Controls.Add( this.groupAttack );
            this.flowLayoutPanel.Controls.Add( this.panelButtons );
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point( 9, 9 );
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding( 0 );
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size( 373, 418 );
            this.flowLayoutPanel.TabIndex = 18;
            // 
            // panelVocaloid2Template
            // 
            this.panelVocaloid2Template.Controls.Add( this.comboTemplate );
            this.panelVocaloid2Template.Controls.Add( this.lblTemplate );
            this.panelVocaloid2Template.Location = new System.Drawing.Point( 3, 3 );
            this.panelVocaloid2Template.Name = "panelVocaloid2Template";
            this.panelVocaloid2Template.Size = new System.Drawing.Size( 367, 29 );
            this.panelVocaloid2Template.TabIndex = 19;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add( this.btnCancel );
            this.panelButtons.Controls.Add( this.btnOK );
            this.panelButtons.Location = new System.Drawing.Point( 3, 367 );
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size( 367, 48 );
            this.panelButtons.TabIndex = 19;
            // 
            // FormNoteExpressionConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 390, 514 );
            this.Controls.Add( this.flowLayoutPanel );
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size( 6000, 6000 );
            this.MinimizeBox = false;
            this.Name = "FormNoteExpressionConfig";
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
            this.groupAttack.ResumeLayout( false );
            this.groupAttack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDuration)).EndInit();
            this.flowLayoutPanel.ResumeLayout( false );
            this.panelVocaloid2Template.ResumeLayout( false );
            this.panelVocaloid2Template.PerformLayout();
            this.panelButtons.ResumeLayout( false );
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
        private System.Windows.Forms.ComboBox comboTemplate;
        private System.Windows.Forms.GroupBox groupAttack;
        private NumberTextBox txtDepth;
        private NumberTextBox txtDuration;
        private System.Windows.Forms.TrackBar trackDepth;
        private System.Windows.Forms.TrackBar trackDuration;
        private System.Windows.Forms.Label lblDepth;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Panel panelVocaloid2Template;
        private System.Windows.Forms.ComboBox comboAttackTemplate;
        private System.Windows.Forms.Label lblAttackTemplate;
        #endregion
#endif
    }

}
