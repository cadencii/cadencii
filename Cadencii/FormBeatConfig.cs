/*
 * FormBeatConfig.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Boare.Lib.AppUtil;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    partial class FormBeatConfig : Form {
        public void ApplyLanguage() {
            Text = _( "Beat Change" );
            groupPosition.Text = _( "Position" );
            groupBeat.Text = _( "Beat" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
            lblStart.Text = _( "From" ) + "(&F)";
            chkEnd.Text = _( "To" ) + "(&T)";
            lblBar1.Text = _( "Measure" );
            lblBar2.Text = _( "Measure" );
        }

        public static String _( String id ) {
            return Messaging.GetMessage( id );
        }
        
        public int Start {
            get {
                return (int)numStart.Value;
            }
        }
        
        public boolean EndSpecified {
            get {
                return chkEnd.Checked;
            }
        }

        public int End {
            get {
                return (int)numEnd.Value;
            }
        }
        
        public int Numerator {
            get {
                return (int)numNumerator.Value;
            }
        }
        
        public int Denominator {
            get {
                int ret = 1;
                for ( int i = 0; i < comboDenominator.SelectedIndex; i++ ) {
                    ret *= 2;
                }
                return ret;
            }
        }
        
        public FormBeatConfig( int bar_count, int numerator, int denominator, boolean num_enabled, int pre_measure ) {
            InitializeComponent();
            ApplyLanguage();
            //ClientSize = new Size( 278, 182 );

            numStart.Enabled = num_enabled;
            numEnd.Enabled = num_enabled;
            chkEnd.Enabled = num_enabled;
            numStart.Minimum = -pre_measure + 1;
            numStart.Maximum = decimal.MaxValue;
            numEnd.Minimum = -pre_measure + 1;
            numEnd.Maximum = decimal.MaxValue;

            // 拍子の分母
            comboDenominator.Items.Clear();
            comboDenominator.Items.Add( 1 );
            int count = 1;
            for ( int i = 1; i <= 5; i++ ) {
                count *= 2;
                comboDenominator.Items.Add( count + "" );
            }
            count = 0;
            while ( denominator > 1 ) {
                count++;
                denominator /= 2;
            }
            comboDenominator.SelectedIndex = count;

            // 拍子の分子
            if ( numerator < numNumerator.Minimum ) {
                numNumerator.Value = numNumerator.Minimum;
            } else if ( numNumerator.Maximum < numerator ) {
                numNumerator.Value = numNumerator.Maximum;
            } else {
                numNumerator.Value = numerator;
            }

            // 始点
            if ( bar_count < numStart.Minimum ) {
                numStart.Value = numStart.Minimum;
            } else if ( numStart.Maximum < bar_count ) {
                numStart.Value = numStart.Maximum;
            } else {
                numStart.Value = bar_count;
            }

            // 終点
            if ( bar_count < numEnd.Minimum ) {
                numEnd.Value = numEnd.Minimum;
            } else if ( numEnd.Maximum < bar_count ) {
                numEnd.Value = numEnd.Maximum;
            } else {
                numEnd.Value = bar_count;
            }
            Util.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }
        
        private void chkEnd_CheckedChanged( object sender, EventArgs e ) {
            numEnd.Enabled = chkEnd.Checked;
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
        }
    }

}
