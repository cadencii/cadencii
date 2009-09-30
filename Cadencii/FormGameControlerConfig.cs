/*
 * FormGameControlerConfig.cs
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
using System.Windows.Forms;
using System.Collections.Generic;

using Boare.Lib.AppUtil;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;
    using Integer = Int32;

    public partial class FormGameControlerConfig : Form {
        private Vector<Integer> m_list = new Vector<Integer>();
        private Vector<Integer> m_povs = new Vector<Integer>();
        private int index;

        public FormGameControlerConfig() {
            InitializeComponent();
            for ( int i = 0; i < 10; i++ ) {
                m_list.add( -1 );
            }
            for ( int i = 0; i < 4; i++ ) {
                m_povs.add( int.MinValue );
            }
            ApplyLanguage();
            int num_dev = winmmhelp.JoyGetNumJoyDev();
            if ( num_dev > 0 ) {
                pictButton.Image = Boare.Cadencii.Properties.Resources.btn1;
                progressCount.Maximum = 8;
                progressCount.Minimum = 0;
                progressCount.Value = 0;
                index = 1;
                btnSkip.Enabled = true;
                btnReset.Enabled = true;
                timer.Start();                
            } else {
                btnSkip.Enabled = false;
                btnReset.Enabled = false;
            }
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public void ApplyLanguage() {
            int num_dev = winmmhelp.JoyGetNumJoyDev();
            if ( num_dev > 0 ) {
                lblMessage.Text = _( "Push buttons in turn as shown below" );
            } else {
                lblMessage.Text = _( "Game controler is not available" );
            }
            this.Text = _( "Game Controler Configuration" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
            btnReset.Text = _( "Reset And Exit" );
            btnSkip.Text = _( "Skip" );
        }

        private void timer_Tick( object sender, EventArgs e ) {
            //int num_btn = vstidrv.JoyGetNumButtons( 0 );
            byte[] btn;
            int pov;
            winmmhelp.JoyGetStatus( 0, out btn, out pov );
#if DEBUG
            AppManager.debugWriteLine( "FormGameControlerConfig+timer_Tick" );
            AppManager.debugWriteLine( "    pov=" + pov );
#endif
            boolean added = false;
            if ( index <= 4 ) {
                if ( pov >= 0 && !m_povs.contains( pov ) ) {
                    m_povs.set( index - 1, pov );
                    added = true;
                }
            } else {
                for ( int i = 0; i < btn.Length; i++ ) {
                    if ( btn[i] > 0x0 && !m_list.contains( i ) ) {
                        m_list.set( index - 5, i );
                        added = true;
                        break;
                    }
                }
            }
            if ( added ) {
                if ( index <= 8 ) {
                    progressCount.Value = index;
                } else if ( index <= 12 ) {
                    progressCount.Value = index - 8;
                } else {
                    progressCount.Value = index - 12;
                }

                if ( index == 8 ) {
                    pictButton.Image = Boare.Cadencii.Properties.Resources.btn2;
                    progressCount.Value = 0;
                    progressCount.Maximum = 4;
                } else if ( index == 12 ) {
                    pictButton.Image = Boare.Cadencii.Properties.Resources.btn3;
                    progressCount.Value = 0;
                    progressCount.Maximum = 2;
                }
                if ( index == 14 ) {
                    btnSkip.Enabled = false;
                    btnOK.Enabled = true;
                    timer.Stop();
                }
                index++;
            }
        }

        public int Rectangle {
            get {
                return m_list.get( 0 );
            }
        }

        public int Triangle {
            get {
                return m_list.get( 1 );
            }
        }

        public int Circle {
            get {
                return m_list.get( 2 );
            }
        }

        public int Cross {
            get {
                return m_list.get( 3 );
            }
        }

        public int L1 {
            get {
                return m_list.get( 4 );
            }
        }

        public int L2 {
            get {
                return m_list.get( 5 );
            }
        }

        public int R1 {
            get {
                return m_list.get( 6 );
            }
        }

        public int R2 {
            get {
                return m_list.get( 7 );
            }
        }

        public int Select {
            get {
                return m_list.get( 8 );
            }
        }

        public int Start {
            get {
                return m_list.get( 9 );
            }
        }

        public int PovDown {
            get {
                return m_povs.get( 0 );
            }
        }

        public int PovLeft {
            get {
                return m_povs.get( 1 );
            }
        }

        public int PovUp {
            get {
                return m_povs.get( 2 );
            }
        }

        public int PovRight {
            get {
                return m_povs.get( 3 );
            }
        }

        private static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        private void btnSkip_Click( object sender, EventArgs e ) {
            if ( index <= 4 ) {
                m_povs.set( index - 1, int.MinValue );
            } else {
                m_list.set( index - 5, -1 );
            }
            if ( index <= 8 ) {
                progressCount.Value = index;
            } else if ( index <= 12 ) {
                progressCount.Value = index - 8;
            } else {
                progressCount.Value = index - 12;
            }

            if ( index == 8 ) {
                pictButton.Image = Boare.Cadencii.Properties.Resources.btn2;
                progressCount.Value = 0;
                progressCount.Maximum = 4;
            } else if ( index == 12 ) {
                pictButton.Image = Boare.Cadencii.Properties.Resources.btn3;
                progressCount.Value = 0;
                progressCount.Maximum = 2;
            }
            if ( index == 14 ){
                btnSkip.Enabled = false;
                btnOK.Enabled = true;
                timer.Stop();
            }
            index++;
        }

        private void btnReset_Click( object sender, EventArgs e ) {
            m_list.set( 0, 3 ); // □
            m_list.set( 1, 0 ); // △
            m_list.set( 2, 1 ); // ○
            m_list.set( 3, 2 ); // ×
            m_list.set( 4, 4 ); // L1
            m_list.set( 5, 6 ); // L2
            m_list.set( 6, 5 ); // R1
            m_list.set( 7, 7 ); // R2
            m_list.set( 8, 8 ); // SELECT
            m_list.set( 9, 9 ); // START
            m_povs.set( 0, 18000 ); // down
            m_povs.set( 1, 27000 ); // left
            m_povs.set( 2, 0 ); // up
            m_povs.set( 3, 9000 ); // right
        }
    }

}
