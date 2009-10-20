/*
 * FormProjectProperty.cs
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using bocoree;
using Boare.Lib.AppUtil;

namespace Boare.Cadencii {

    public partial class FormTrackProperty : Form {
        private int m_master_tuning;

        public FormTrackProperty( int master_tuning_in_cent ) {
            InitializeComponent();
            ApplyLanguage();
            m_master_tuning = master_tuning_in_cent;
            txtMasterTuning.Text = master_tuning_in_cent.ToString();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            lblMasterTuning.Text = _( "Master Tuning in Cent" );
            this.Text = _( "Track Property" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
        }

        private String _( String id ) {
            return Messaging.getMessage( id );
        }

        public int MasterTuningInCent {
            get {
                return m_master_tuning;
            }
        }

        private void txtMasterTuning_TextChanged( object sender, EventArgs e ) {
            int v = m_master_tuning;
            try {
                v = PortUtil.parseInt( txtMasterTuning.Text );
                m_master_tuning = v;
            } catch ( Exception ex ) {
            }
        }
    }

}
