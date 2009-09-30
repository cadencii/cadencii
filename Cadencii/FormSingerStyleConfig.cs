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

namespace Boare.Cadencii {

    using boolean = Boolean;

    partial class FormSingerStyleConfig : Form {
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
            this.Text = _( "Default Singer Style" );
        }

        public static String _( String id ) {
            return Messaging.GetMessage( id );
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
            ApplyLanguage();

            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
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
            if ( MessageBox.Show( _( "Would you like to change singer style for all events?" ),
                                  FormMain._APP_NAME, 
                                  MessageBoxButtons.YesNo, 
                                  MessageBoxIcon.Exclamation ) == DialogResult.Yes ) {
                m_apply_current_track = true;
                DialogResult = DialogResult.OK;
            }
        }

        public boolean ApplyCurrentTrack {
            get {
                return m_apply_current_track;
            }
        }
    }

}
