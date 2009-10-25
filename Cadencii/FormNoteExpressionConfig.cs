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
using System.Windows.Forms;
using System.Drawing;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {

    using boolean = Boolean;

    partial class FormNoteExpressionConfig : BForm {
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

        private void btnApply_Click( object sender, EventArgs e ) {
            if ( AppManager.showMessageBox( _( "Would you like to change singer style for all events?" ),
                                  FormMain._APP_NAME, 
                                  MessageBoxButtons.YesNo, 
                                  MessageBoxIcon.Exclamation ) == BDialogResult.YES ) {
                m_apply_current_track = true;
                DialogResult = DialogResult.OK;
            }
        }

        public boolean ApplyCurrentTrack {
            get {
                return m_apply_current_track;
            }
        }

        private void FormSingerStyleConfig_Load( object sender, EventArgs e ) {

        }

        private void trackDuration_Scroll( object sender, EventArgs e ) {
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
    }

}
