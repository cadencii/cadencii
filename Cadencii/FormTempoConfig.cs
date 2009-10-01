/*
 * FormTempoConfig.cs
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

namespace Boare.Cadencii {
    
    partial class FormTempoConfig : Form {
        public FormTempoConfig( int bar_count, int beat, int beat_max, int clock, int clock_max, decimal tempo, int pre_measure ) {
            InitializeComponent();
            ApplyLanguage();
            numBar.Minimum = -pre_measure + 1;
            numBar.Maximum = decimal.MaxValue;
            numBar.Value = bar_count;

            numBeat.Minimum = 1;
            numBeat.Maximum = beat_max;
            numBeat.Value = beat;
            numClock.Minimum = 0;
            numClock.Maximum = clock_max;
            numClock.Value = clock;
            numTempo.Value = tempo;
            Util.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        public void ApplyLanguage() {
            Text = _( "Global Tempo" );
            groupPosition.Text = _( "Position" );
            lblBar.Text = _( "Measure" ) + "(&M)";
            lblBeat.Text = _( "Beat" ) + "(&B)";
            lblClock.Text = _( "Clock" ) + "(&L)";
            groupTempo.Text = _( "Tempo" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
        }

        public int BeatCount {
            get {
                return (int)numBeat.Value;
            }
        }

        public int Clock {
            get {
                return (int)numClock.Value;
            }
        }

        public decimal Tempo {
            get {
                return numTempo.Value;
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
        }
    }

}
