/*
 * FormRealtimeConfig.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using cadencii;
using cadencii.windows.forms;
using cadencii.apputil;

namespace cadencii
{

    public class FormRealtimeConfig : System.Windows.Forms.Form
    {
        private bool m_game_ctrl_enabled = false;
        private double m_last_event_processed;
        private System.Windows.Forms.Timer timer;

        public FormRealtimeConfig()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer(this.components);
            timer.Interval = 10;
            registerEventHandlers();
            setResources();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }

        #region public methods
        public float getSpeed()
        {
            return (float)numSpeed.Value;
        }
        #endregion

        #region event handlers
        public void FormRealtimeConfig_Load(Object sender, EventArgs e)
        {
            int num_joydev = winmmhelp.JoyGetNumJoyDev();
            m_game_ctrl_enabled = (num_joydev > 0);
            if (m_game_ctrl_enabled) {
                timer.Start();
            }
        }

        public void timer_Tick(Object sender, EventArgs e)
        {
            try {
                double now = PortUtil.getCurrentTime();
                double dt_ms = (now - m_last_event_processed) * 1000.0;
                //JoystickState state = m_game_ctrl.CurrentJoystickState;
                int len = winmmhelp.JoyGetNumButtons(0);
                byte[] buttons = new byte[len];
                int pov0;
                winmmhelp.JoyGetStatus(0, out buttons, out pov0);
                //int[] pov = state.GetPointOfView();
                //int pov0 = pov[0];
                bool btn_x = (buttons[AppManager.editorConfig.GameControlerCross] > 0x00);
                bool btn_o = (buttons[AppManager.editorConfig.GameControlerCircle] > 0x00);
                bool btn_tr = (buttons[AppManager.editorConfig.GameControlerTriangle] > 0x00);
                bool btn_re = (buttons[AppManager.editorConfig.GameControlerRectangle] > 0x00);
                bool pov_r = pov0 == 9000;  //(4500 <= pov0 && pov0 <= 13500);
                bool pov_l = pov0 == 27000; //(22500 <= pov[0] && pov[0] <= 31500);
                bool pov_u = pov0 == 0;     //(31500 <= pov[0] || (0 <= pov[0] && pov[0] <= 4500));
                bool pov_d = pov0 == 18000; //(13500 <= pov[0] && pov[0] <= 22500);
                bool L1 = (buttons[AppManager.editorConfig.GameControlL1] > 0x00);
                bool R1 = (buttons[AppManager.editorConfig.GameControlR1] > 0x00);
                bool L2 = (buttons[AppManager.editorConfig.GameControlL2] > 0x00);
                bool R2 = (buttons[AppManager.editorConfig.GameControlR2] > 0x00);
                bool SELECT = (buttons[AppManager.editorConfig.GameControlSelect] > 0x00);
                if (dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval) {
                    if (btnStart.Focused) {
                        if (btn_o) {
                            timer.Stop();
                            btnStart_Click(this, new EventArgs());
                            m_last_event_processed = now;
                        } else if (pov_r) {
                            btnCancel.Focus();
                            m_last_event_processed = now;
                        } else if (pov_d) {
                            numSpeed.Focus();
                            m_last_event_processed = now;
                        }
                    } else if (btnCancel.Focused) {
                        if (btn_o) {
                            timer.Stop();
                            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                            Close();
                        } else if (pov_l) {
                            btnStart.Focus();
                            m_last_event_processed = now;
                        } else if (pov_d || pov_r) {
                            numSpeed.Focus();
                            m_last_event_processed = now;
                        }
                    } else if (numSpeed.Focused) {
                        if (R1) {
                            if (numSpeed.Value + numSpeed.Increment <= numSpeed.Maximum) {
                                numSpeed.Value = numSpeed.Value + numSpeed.Increment;
                                m_last_event_processed = now;
                            }
                        } else if (L1) {
                            if (numSpeed.Value - numSpeed.Increment >= numSpeed.Minimum) {
                                numSpeed.Value = numSpeed.Value - numSpeed.Increment;
                                m_last_event_processed = now;
                            }
                        } else if (pov_l) {
                            btnCancel.Focus();
                            m_last_event_processed = now;
                        } else if (pov_u) {
                            btnStart.Focus();
                            m_last_event_processed = now;
                        }
                    }
                }
            } catch (Exception ex) {
            }
        }

        public void btnStart_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        #endregion

        #region helper methods
        private void registerEventHandlers()
        {
            this.Load += new EventHandler(FormRealtimeConfig_Load);
            timer.Tick += new EventHandler(timer_Tick);
            btnStart.Click += new EventHandler(btnStart_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region UI implementation
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRealTimeInput = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.numSpeed = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(25, 52);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 33);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(196, 52);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 33);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblRealTimeInput
            // 
            this.lblRealTimeInput.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRealTimeInput.Location = new System.Drawing.Point(23, 9);
            this.lblRealTimeInput.Name = "lblRealTimeInput";
            this.lblRealTimeInput.Size = new System.Drawing.Size(293, 28);
            this.lblRealTimeInput.TabIndex = 2;
            this.lblRealTimeInput.Text = "Realtime Input";
            this.lblRealTimeInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(49, 114);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(36, 12);
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "Speed";
            // 
            // numSpeed
            // 
            this.numSpeed.DecimalPlaces = 1;
            this.numSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numSpeed.Location = new System.Drawing.Point(107, 112);
            this.numSpeed.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            this.numSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.Size = new System.Drawing.Size(120, 19);
            this.numSpeed.TabIndex = 4;
            this.numSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FormRealtimeConfig
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(347, 161);
            this.Controls.Add(this.numSpeed);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblRealTimeInput);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRealtimeConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormRealtimeConfig";
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRealTimeInput;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.NumericUpDown numSpeed;
        #endregion
        #endregion
    }

}
