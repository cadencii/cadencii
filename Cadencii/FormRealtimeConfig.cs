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
#if JAVA
package org.kbinani.Cadencii;

#else
using System;
using System.Windows.Forms;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormRealtimeConfig extends BForm {
#else
    public class FormRealtimeConfig : BForm {
#endif
        private boolean m_game_ctrl_enabled = false;
        private DateTime m_last_event_processed;

        public FormRealtimeConfig() {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            Boare.Lib.AppUtil.Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public float getSpeed() {
            return (float)numSpeed.getValue();
        }

        private void FormRealtimeConfig_Load( object sender, EventArgs e ) {
            int num_joydev = winmmhelp.JoyGetNumJoyDev();
            m_game_ctrl_enabled = (num_joydev > 0);
            if ( m_game_ctrl_enabled ) {
                timer.Start();
            }
        }

        private void timer_Tick( object sender, EventArgs e ) {
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
                    if ( btnStart.isFocusOwner() ) {
                        if ( btn_o ) {
                            timer.Stop();
                            btnStart_Click( this, new EventArgs() );
                            m_last_event_processed = now;
                        } else if ( pov_r ) {
                            btnCancel.requestFocus();
                            m_last_event_processed = now;
                        } else if ( pov_d ) {
                            numSpeed.requestFocus();
                            m_last_event_processed = now;
                        }
                    } else if ( btnCancel.isFocusOwner() ) {
                        if ( btn_o ) {
                            timer.Stop();
                            setDialogResult( BDialogResult.CANCEL );
                            close();
                        } else if ( pov_l ) {
                            btnStart.requestFocus();
                            m_last_event_processed = now;
                        } else if ( pov_d || pov_r ) {
                            numSpeed.requestFocus();
                            m_last_event_processed = now;
                        }
                    } else if ( numSpeed.isFocusOwner() ) {
                        if ( R1 ) {
                            if ( numSpeed.getValue() + numSpeed.getIncrement() <= numSpeed.getMaximum() ) {
                                numSpeed.setValue( numSpeed.getValue() + numSpeed.getIncrement() );
                                m_last_event_processed = now;
                            }
                        } else if ( L1 ) {
                            if ( numSpeed.getValue() - numSpeed.getIncrement() >= numSpeed.getMinimum() ) {
                                numSpeed.setValue( numSpeed.getValue() - numSpeed.getIncrement() );
                                m_last_event_processed = now;
                            }
                        } else if ( pov_l ) {
                            btnCancel.requestFocus();
                            m_last_event_processed = now;
                        } else if ( pov_u ) {
                            btnStart.requestFocus();
                            m_last_event_processed = now;
                        }
                    }
                }
            } catch {
            }
        }

        private void btnStart_Click( object sender, EventArgs e ) {
            setDialogResult( BDialogResult.OK );
            close();
        }

        private void registerEventHandlers() {
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            this.btnStart.Click += new System.EventHandler( this.btnStart_Click );
            this.Load += new System.EventHandler( this.FormRealtimeConfig_Load );
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
            this.components = new System.ComponentModel.Container();
            this.timer = new BTimer( this.components );
            this.btnStart = new BButton();
            this.btnCancel = new BButton();
            this.lblRealTimeInput = new BLabel();
            this.lblSpeed = new BLabel();
            this.numSpeed = new BNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 10;
            // 
            // btnStart
            // 
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point( 25, 52 );
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size( 120, 33 );
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point( 196, 52 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 120, 33 );
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblRealTimeInput
            // 
            this.lblRealTimeInput.Font = new System.Drawing.Font( "Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.lblRealTimeInput.Location = new System.Drawing.Point( 23, 9 );
            this.lblRealTimeInput.Name = "lblRealTimeInput";
            this.lblRealTimeInput.Size = new System.Drawing.Size( 293, 28 );
            this.lblRealTimeInput.TabIndex = 2;
            this.lblRealTimeInput.Text = "Realtime Input";
            this.lblRealTimeInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point( 49, 114 );
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size( 36, 12 );
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "Speed";
            // 
            // numSpeed
            // 
            this.numSpeed.DecimalPlaces = 1;
            this.numSpeed.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.numSpeed.Location = new System.Drawing.Point( 107, 112 );
            this.numSpeed.Maximum = new decimal( new int[] {
            30,
            0,
            0,
            65536} );
            this.numSpeed.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.Size = new System.Drawing.Size( 120, 19 );
            this.numSpeed.TabIndex = 4;
            this.numSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSpeed.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // FormRealtimeConfig
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 347, 161 );
            this.Controls.Add( this.numSpeed );
            this.Controls.Add( this.lblSpeed );
            this.Controls.Add( this.lblRealTimeInput );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnStart );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRealtimeConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormRealtimeConfig";
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BTimer timer;
        private BButton btnStart;
        private BButton btnCancel;
        private BLabel lblRealTimeInput;
        private BLabel lblSpeed;
        private BNumericUpDown numSpeed;
        #endregion
#endif
    }

#if !JAVA
}
#endif
