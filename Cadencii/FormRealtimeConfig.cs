/*
 * FormRealtimeConfig.cs
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

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public partial class FormRealtimeConfig : Form {
        private boolean m_game_ctrl_enabled = false;
        private DateTime m_last_event_processed;

        public FormRealtimeConfig() {
            InitializeComponent();
            Boare.Lib.AppUtil.Util.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public float Speed {
            get {
                return (float)numSpeed.Value;
            }
        }

        private void FormRealtimeConfig_Load( object sender, EventArgs e ) {
            int num_joydev = winmmhelp.JoyGetNumJoyDev();
            m_game_ctrl_enabled = (num_joydev > 0);
            if ( m_game_ctrl_enabled ) {
                timer.Start();
            }
        }

        private unsafe void timer_Tick( object sender, EventArgs e ) {
            try {
                DateTime now = DateTime.Now;
                double dt_ms = now.Subtract( m_last_event_processed ).TotalMilliseconds;
                //JoystickState state = m_game_ctrl.CurrentJoystickState;
                int len = winmmhelp.JoyGetNumButtons( 0 );
                byte[] buttons = new byte[len];
                int pov0;
                    winmmhelp.JoyGetStatus( 0, out buttons, out pov0 );
                //int[] pov = state.GetPointOfView();
                //int pov0 = pov[0];
                boolean btn_x = (buttons[AppManager.editorConfig.GameControlerCross] > 0x00);
                boolean btn_o = (buttons[AppManager.editorConfig.GameControlerCircle] > 0x00);
                boolean btn_tr = (buttons[AppManager.editorConfig.GameControlerTriangle] > 0x00);
                boolean btn_re = (buttons[AppManager.editorConfig.GameControlerRectangle] > 0x00);
                boolean pov_r = pov0 == 9000;  //(4500 <= pov0 && pov0 <= 13500);
                boolean pov_l = pov0 == 27000; //(22500 <= pov[0] && pov[0] <= 31500);
                boolean pov_u = pov0 == 0;     //(31500 <= pov[0] || (0 <= pov[0] && pov[0] <= 4500));
                boolean pov_d = pov0 == 18000; //(13500 <= pov[0] && pov[0] <= 22500);
                boolean L1 = (buttons[AppManager.editorConfig.GameControlL1] > 0x00);
                boolean R1 = (buttons[AppManager.editorConfig.GameControlR1] > 0x00);
                boolean L2 = (buttons[AppManager.editorConfig.GameControlL2] > 0x00);
                boolean R2 = (buttons[AppManager.editorConfig.GameControlR2] > 0x00);
                boolean SELECT = (buttons[AppManager.editorConfig.GameControlSelect] > 0x00);
                if ( dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                    if ( btnStart.Focused ) {
                        if ( btn_o ) {
                            timer.Stop();
                            btnStart_Click( this, new EventArgs() );
                            m_last_event_processed = now;
                        } else if ( pov_r ) {
                            btnCancel.Focus();
                            m_last_event_processed = now;
                        } else if ( pov_d ) {
                            numSpeed.Focus();
                            m_last_event_processed = now;
                        }
                    } else if ( btnCancel.Focused ) {
                        if ( btn_o ) {
                            timer.Stop();
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                        } else if ( pov_l ) {
                            btnStart.Focus();
                            m_last_event_processed = now;
                        } else if ( pov_d || pov_r ) {
                            numSpeed.Focus();
                            m_last_event_processed = now;
                        }
                    } else if ( numSpeed.Focused ) {
                        if ( R1 ) {
                            if ( numSpeed.Value + numSpeed.Increment <= numSpeed.Maximum ) {
                                numSpeed.Value += numSpeed.Increment;
                                m_last_event_processed = now;
                            }
                        } else if ( L1 ) {
                            if ( numSpeed.Value - numSpeed.Increment >= numSpeed.Minimum ) {
                                numSpeed.Value -= numSpeed.Increment;
                                m_last_event_processed = now;
                            }
                        } else if ( pov_l ) {
                            btnCancel.Focus();
                            m_last_event_processed = now;
                        } else if ( pov_u ) {
                            btnStart.Focus();
                            m_last_event_processed = now;
                        }
                    }
                }
            } catch {
            }
        }

        private void btnStart_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

}
