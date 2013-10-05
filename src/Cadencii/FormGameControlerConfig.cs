/*
 * FormGameControlerConfig.cs
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
using System.Collections.Generic;
using cadencii.apputil;
using cadencii;
using cadencii.java.util;
using cadencii.windows.forms;



namespace cadencii
{

    public class FormGameControlerConfig : System.Windows.Forms.Form
    {
        private List<int> m_list = new List<int>();
        private List<int> m_povs = new List<int>();
        private int index;
        private System.Windows.Forms.Timer timer;

        public FormGameControlerConfig()
        {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer(this.components);
            registerEventHandlers();
            setResources();
            for (int i = 0; i < 10; i++) {
                m_list.Add(-1);
            }
            for (int i = 0; i < 4; i++) {
                m_povs.Add(int.MinValue);
            }
            applyLanguage();
            int num_dev = winmmhelp.JoyGetNumJoyDev();
            if (num_dev > 0) {
                pictButton.Image = Properties.Resources.btn1;
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
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            int num_dev = winmmhelp.JoyGetNumJoyDev();
            if (num_dev > 0) {
                lblMessage.Text = _("Push buttons in turn as shown below");
            } else {
                lblMessage.Text = _("Game controler is not available");
            }
            this.Text = _("Game Controler Configuration");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            btnReset.Text = _("Reset And Exit");
            btnSkip.Text = _("Skip");
        }

        public int getRectangle()
        {
            return m_list[0];
        }

        public int getTriangle()
        {
            return m_list[1];
        }

        public int getCircle()
        {
            return m_list[2];
        }

        public int getCross()
        {
            return m_list[3];
        }

        public int getL1()
        {
            return m_list[4];
        }

        public int getL2()
        {
            return m_list[5];
        }

        public int getR1()
        {
            return m_list[6];
        }

        public int getR2()
        {
            return m_list[7];
        }

        public int getSelect()
        {
            return m_list[8];
        }

        public int getStart()
        {
            return m_list[9];
        }

        public int getPovDown()
        {
            return m_povs[0];
        }

        public int getPovLeft()
        {
            return m_povs[1];
        }

        public int getPovUp()
        {
            return m_povs[2];
        }

        public int getPovRight()
        {
            return m_povs[3];
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            timer.Tick += new EventHandler(timer_Tick);
            btnSkip.Click += new EventHandler(btnSkip_Click);
            btnReset.Click += new EventHandler(btnReset_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void timer_Tick(Object sender, EventArgs e)
        {
            //int num_btn = vstidrv.JoyGetNumButtons( 0 );
            byte[] btn;
            int pov;
            winmmhelp.JoyGetStatus(0, out btn, out pov);

#if DEBUG
            AppManager.debugWriteLine("FormGameControlerConfig+timer_Tick");
            AppManager.debugWriteLine("    pov=" + pov);
#endif
            bool added = false;
            if (index <= 4) {
                if (pov >= 0 && !m_povs.Contains(pov)) {
                    m_povs[index - 1] = pov;
                    added = true;
                }
            } else {
                for (int i = 0; i < btn.Length; i++) {
                    if (btn[i] > 0x0 && !m_list.Contains(i)) {
                        m_list[index - 5] = i;
                        added = true;
                        break;
                    }
                }
            }
            if (added) {
                if (index <= 8) {
                    progressCount.Value = index;
                } else if (index <= 12) {
                    progressCount.Value = index - 8;
                } else {
                    progressCount.Value = index - 12;
                }

                if (index == 8) {
                    pictButton.Image = Properties.Resources.btn2;
                    progressCount.Value = 0;
                    progressCount.Maximum = 4;
                } else if (index == 12) {
                    pictButton.Image = Properties.Resources.btn3;
                    progressCount.Value = 0;
                    progressCount.Maximum = 2;
                }
                if (index == 14) {
                    btnSkip.Enabled = false;
                    btnOK.Enabled = true;
                    timer.Stop();
                }
                index++;
            }
        }

        public void btnSkip_Click(Object sender, EventArgs e)
        {
            if (index <= 4) {
                m_povs[index - 1] = int.MinValue;
            } else {
                m_list[index - 5] = -1;
            }
            if (index <= 8) {
                progressCount.Value = index;
            } else if (index <= 12) {
                progressCount.Value = index - 8;
            } else {
                progressCount.Value = index - 12;
            }

            if (index == 8) {
                pictButton.Image = Properties.Resources.btn2;
                progressCount.Value = 0;
                progressCount.Maximum = 4;
            } else if (index == 12) {
                pictButton.Image = Properties.Resources.btn3;
                progressCount.Value = 0;
                progressCount.Maximum = 2;
            }
            if (index == 14) {
                btnSkip.Enabled = false;
                btnOK.Enabled = true;
                timer.Stop();
            }
            index++;
        }

        public void btnReset_Click(Object sender, EventArgs e)
        {
            m_list[0] = 3; // □
            m_list[1] = 0; // △
            m_list[2] = 1; // ○
            m_list[3] = 2; // ×
            m_list[4] = 4; // L1
            m_list[5] = 6; // L2
            m_list[6] = 5; // R1
            m_list[7] = 7; // R2
            m_list[8] = 8; // SELECT
            m_list[9] = 9; // START
            m_povs[0] = 18000; // down
            m_povs[1] = 27000; // left
            m_povs[2] = 0; // up
            m_povs[3] = 9000; // right
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region UI implementation
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

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pictButton = new System.Windows.Forms.PictureBox();
            this.progressCount = new System.Windows.Forms.ProgressBar();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictButton)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(15, 17);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(9, 12);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = " ";
            // 
            // timer
            // 
            // 
            // pictButton
            // 
            this.pictButton.Location = new System.Drawing.Point(12, 49);
            this.pictButton.Name = "pictButton";
            this.pictButton.Size = new System.Drawing.Size(316, 36);
            this.pictButton.TabIndex = 1;
            this.pictButton.TabStop = false;
            // 
            // progressCount
            // 
            this.progressCount.Location = new System.Drawing.Point(12, 101);
            this.progressCount.Maximum = 8;
            this.progressCount.Name = "progressCount";
            this.progressCount.Size = new System.Drawing.Size(316, 13);
            this.progressCount.TabIndex = 2;
            this.progressCount.Value = 1;
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(27, 127);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 23);
            this.btnSkip.TabIndex = 3;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(172, 159);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(253, 159);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReset.Location = new System.Drawing.Point(197, 127);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(131, 23);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset And Exit";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // FormGameControlerConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(340, 200);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.progressCount);
            this.Controls.Add(this.pictButton);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGameControlerConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Game Controler Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.pictButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.PictureBox pictButton;
        private System.Windows.Forms.ProgressBar progressCount;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;

        #endregion

    }

}
