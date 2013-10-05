/*
 * VolumeTracker.cs
 * Copyright © 2008-2011 kbinani
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
using System.Windows.Forms;
using cadencii;
using cadencii.windows.forms;
using cadencii.java.awt;
using cadencii.vsq;



namespace cadencii
{

    public class VolumeTracker : UserControl, IAmplifierView
    {
        private int mFeder = 0;
        private string m_number = "0";
        private string m_title = "";
        private Object m_tag = null;
        private bool mMuted = false;
        private int mPanpot = 0;
        private int mTrack = 0;

        #region Constants
        public const int WIDTH = 85;
        public const int HEIGHT = 284;
        private static readonly int[,] _KEY = {
            {55, 26}, 
            {51, 27},
            {47, 28},
            {42, 30},
            {38, 31},
            {35, 33},
            {31, 34},
            {28, 36},
            {24, 37},
            {21, 39},
            {18, 40},
            {15, 42},
            {12, 43},
            {10, 45},
            {7, 46},
            {5, 48},
            {2, 49},
            {0, 51},
            {-2, 52},
            {-5, 54},
            {-7, 55},
            {-10, 57},
            {-12, 58},
            {-15, 60},
            {-18, 61},
            {-21, 63},
            {-24, 64},
            {-28, 66},
            {-31, 67},
            {-35, 69},
            {-38, 70},
            {-42, 72},
            {-47, 73},
            {-51, 75},
            {-55, 76},
            {-60, 78},
            {-65, 79},
            {-70, 81},
            {-76, 82},
            {-81, 84},
            {-87, 85},
            {-93, 87},
            {-100, 88},
            {-107, 89},
            {-114, 91},
            {-121, 92},
            {-129, 94},
            {-137, 95},
            {-145, 97},
            {-154, 98},
            {-163, 100},
            {-173, 101},
            {-183, 103},
            {-193, 104},
            {-204, 106},
            {-215, 107},
            {-227, 109},
            {-240, 110},
            {-253, 112},
            {-266, 113},
            {-280, 115},
            {-295, 116},
            {-311, 118},
            {-327, 119},
            {-344, 121},
            {-362, 122},
            {-380, 124},
            {-399, 125},
            {-420, 127},
            {-441, 128},
            {-463, 130},
            {-486, 131},
            {-510, 133},
            {-535, 134},
            {-561, 136},
            {-589, 137},
            {-617, 139},
            {-647, 140},
            {-678, 142},
            {-711, 143},
            {-745, 145},
            {-781, 146},
            {-818, 148},
            {-857, 149},
            {-898, 151},
        };
        #endregion

        public event FederChangedEventHandler FederChanged;

        public event PanpotChangedEventHandler PanpotChanged;

        public event EventHandler MuteButtonClick;

        public event EventHandler SoloButtonClick;

        public VolumeTracker()
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            setMuted(false);
            setSolo(false);
        }

        public int getTrack()
        {
            return mTrack;
        }

        public void setTrack(int value)
        {
            mTrack = value;
        }

        public double getAmplifyL()
        {
            double ret = 0.0;
            if (!mMuted) {
                ret = VocaloSysUtil.getAmplifyCoeffFromFeder(mFeder) * VocaloSysUtil.getAmplifyCoeffFromPanLeft(mPanpot);
            }
            return ret;
        }

        public double getAmplifyR()
        {
            double ret = 0.0;
            if (!mMuted) {
                ret = VocaloSysUtil.getAmplifyCoeffFromFeder(mFeder) * VocaloSysUtil.getAmplifyCoeffFromPanRight(mPanpot);
            }
            return ret;
        }

        public void setLocation(int x, int y)
        {
            base.Location = new System.Drawing.Point(x, y);
        }

        public void setTag(Object value)
        {
            m_tag = value;
        }

        public Object getTag()
        {
            return m_tag;
        }

        public string getTitle()
        {
            return m_title;
        }

        public void setTitle(string value)
        {
            m_title = value;
            updateTitle();
        }

        private void updateTitle()
        {
            if (m_number == "") {
                lblTitle.Text = m_title;
            } else if (m_title == "") {
                lblTitle.Text = m_number;
            } else {
                lblTitle.Text = m_number + " " + m_title;
            }
        }

        public string getNumber()
        {
            return m_number;
        }

        public void setNumber(string value)
        {
            m_number = value;
            updateTitle();
        }

        public bool isMuted()
        {
            return chkMute.Checked;
        }

        public void setMuted(bool value)
        {
            bool old = chkMute.Checked;
            chkMute.Checked = value;
            chkMute.BackColor = value ? System.Drawing.Color.DimGray : System.Drawing.Color.White;
            mMuted = value;
        }

        public bool isSolo()
        {
            return chkSolo.Checked;
        }

        public void setSolo(bool value)
        {
            bool old = chkSolo.Checked;
            chkSolo.Checked = value;
            chkSolo.BackColor = value ? System.Drawing.Color.DarkCyan : System.Drawing.Color.White;
        }

        public int getPanpot()
        {
            return trackPanpot.Value;
        }

        public void setPanpot(int value)
        {
            trackPanpot.Value = Math.Min(trackPanpot.Maximum, Math.Max(trackPanpot.Minimum, value));
        }

        public bool isSoloButtonVisible()
        {
            return chkSolo.Visible;
        }

        public void setSoloButtonVisible(bool value)
        {
            chkSolo.Visible = value;
        }

        public int getFeder()
        {
            return mFeder;
        }

        public void setFeder(int value)
        {
            int old = mFeder;
            mFeder = value;
            if (old != mFeder) {
                try {
                    if (FederChanged != null) {
                        FederChanged.Invoke(mTrack, mFeder);
                    }
                } catch (Exception ex) {
                    serr.println("VolumeTracker#setFeder; ex=" + ex);
                }
            }
            int v = 177 - getYCoordFromFeder(mFeder);
            trackFeder.Value = v;
        }

        private static int getFederFromYCoord(int y)
        {
            int feder = _KEY[0, 0];
            int min_diff = Math.Abs(_KEY[0, 1] - y);
            int index = 0;
            int len = _KEY.GetUpperBound(0) + 1;
            for (int i = 1; i < len; i++) {
                int diff = Math.Abs(_KEY[i, 1] - y);
                if (diff < min_diff) {
                    index = i;
                    min_diff = diff;
                    feder = _KEY[i, 0];
                }
            }
            return feder;
        }

        private static int getYCoordFromFeder(int feder)
        {
            int y = _KEY[0, 1];
            int min_diff = Math.Abs(_KEY[0, 0] - feder);
            int index = 0;
            int len = _KEY.GetUpperBound(0) + 1;
            for (int i = 1; i < len; i++) {
                int diff = Math.Abs(_KEY[i, 0] - feder);
                if (diff < min_diff) {
                    index = i;
                    min_diff = diff;
                    y = _KEY[i, 1];
                }
            }
            return y;
        }

        #region event handlers
        private void txtPanpot_Enter(Object sender, EventArgs e)
        {
            txtPanpot.SelectAll();
        }

        private void txtFeder_Enter(Object sender, EventArgs e)
        {
            txtFeder.SelectAll();
        }

        public void VolumeTracker_Resize(Object sender, EventArgs e)
        {
            this.Width = WIDTH;
            this.Height = HEIGHT;
        }

        public void trackFeder_ValueChanged(Object sender, EventArgs e)
        {
            mFeder = getFederFromYCoord(151 - (trackFeder.Value - 26));
            txtFeder.Text = (mFeder / 10.0) + "";
            try {
                if (FederChanged != null) {
                    FederChanged.Invoke(mTrack, mFeder);
                }
            } catch (Exception ex) {
                serr.println("VolumeTracker#trackFeder_ValueChanged; ex=" + ex);
            }
        }

        public void trackPanpot_ValueChanged(Object sender, EventArgs e)
        {
            mPanpot = trackPanpot.Value;
            txtPanpot.Text = mPanpot + "";
            try {
                if (PanpotChanged != null) {
                    PanpotChanged.Invoke(mTrack, mPanpot);
                }
            } catch (Exception ex) {
                serr.println("VolumeTracker#trackPanpot_ValueChanged; ex=" + ex);
            }
        }

        public void txtFeder_KeyDown(Object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & Keys.Enter) != Keys.Enter) {
                return;
            }
            try {
                int feder = (int)((float)double.Parse(txtFeder.Text) * 10.0f);
                if (55 < feder) {
                    feder = 55;
                }
                if (feder < -898) {
                    feder = -898;
                }
                setFeder(feder);
                txtFeder.Text = getFeder() / 10.0f + "";
                txtFeder.Focus();
                txtFeder.SelectAll();
            } catch (Exception ex) {
                serr.println("VolumeTracker#txtFeder_KeyDown; ex=" + ex);
            }
        }

        public void txtPanpot_KeyDown(Object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & Keys.Enter) != Keys.Enter) {
                return;
            }
            try {
                int panpot = int.Parse(txtPanpot.Text);
                if (panpot < -64) {
                    panpot = -64;
                }
                if (64 < panpot) {
                    panpot = 64;
                }
                setPanpot(panpot);
                txtPanpot.Text = getPanpot() + "";
                txtPanpot.Focus();
                txtPanpot.SelectAll();
            } catch (Exception ex) {
                serr.println("VolumeTracker#txtPanpot_KeyDown; ex=" + ex);
            }
        }

        public void chkSolo_Click(Object sender, EventArgs e)
        {
            try {
                if (SoloButtonClick != null) {
                    SoloButtonClick.Invoke(this, e);
                }
            } catch (Exception ex) {
                serr.println("VolumeTracker#chkSolo_Click; ex=" + ex);
            }
        }

        public void chkMute_Click(Object sender, EventArgs e)
        {
            mMuted = chkMute.Checked;
            try {
                if (MuteButtonClick != null) {
                    MuteButtonClick.Invoke(this, e);
                }
            } catch (Exception ex) {
                serr.println("VolumeTracker#chkMute_Click; ex=" + ex);
            }
        }
        #endregion

        private void registerEventHandlers()
        {
            trackFeder.ValueChanged += new EventHandler(trackFeder_ValueChanged);
            trackPanpot.ValueChanged += new EventHandler(trackPanpot_ValueChanged);
            txtPanpot.KeyDown += new KeyEventHandler(txtPanpot_KeyDown);
            txtFeder.KeyDown += new KeyEventHandler(txtFeder_KeyDown);
            chkSolo.Click += new EventHandler(chkSolo_Click);
            chkMute.Click += new EventHandler(chkMute_Click);
            txtFeder.Enter += new EventHandler(txtFeder_Enter);
            txtPanpot.Enter += new EventHandler(txtPanpot_Enter);
        }

        private void setResources()
        {
        }

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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.trackFeder = new TrackBar();
            this.trackPanpot = new TrackBar();
            this.txtPanpot = new TextBox();
            this.lblTitle = new Label();
            this.txtFeder = new TextBox();
            this.chkMute = new CheckBox();
            this.chkSolo = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackFeder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPanpot)).BeginInit();
            this.SuspendLayout();
            // 
            // trackFeder
            // 
            this.trackFeder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackFeder.AutoSize = false;
            this.trackFeder.Location = new System.Drawing.Point(21, 58);
            this.trackFeder.Maximum = 151;
            this.trackFeder.Minimum = 26;
            this.trackFeder.Name = "trackFeder";
            this.trackFeder.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackFeder.Size = new System.Drawing.Size(45, 144);
            this.trackFeder.TabIndex = 0;
            this.trackFeder.TickFrequency = 10;
            this.trackFeder.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackFeder.Value = 100;
            // 
            // trackPanpot
            // 
            this.trackPanpot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPanpot.AutoSize = false;
            this.trackPanpot.Location = new System.Drawing.Point(3, 208);
            this.trackPanpot.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.trackPanpot.Maximum = 64;
            this.trackPanpot.Minimum = -64;
            this.trackPanpot.Name = "trackPanpot";
            this.trackPanpot.Size = new System.Drawing.Size(79, 21);
            this.trackPanpot.TabIndex = 2;
            this.trackPanpot.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // txtPanpot
            // 
            this.txtPanpot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPanpot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtPanpot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPanpot.Location = new System.Drawing.Point(10, 229);
            this.txtPanpot.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.txtPanpot.Name = "txtPanpot";
            this.txtPanpot.Size = new System.Drawing.Size(65, 19);
            this.txtPanpot.TabIndex = 3;
            this.txtPanpot.Text = "0";
            this.txtPanpot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitle.Location = new System.Drawing.Point(0, 261);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(85, 23);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "TITLE";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFeder
            // 
            this.txtFeder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFeder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtFeder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFeder.Location = new System.Drawing.Point(3, 33);
            this.txtFeder.Name = "txtFeder";
            this.txtFeder.Size = new System.Drawing.Size(79, 19);
            this.txtFeder.TabIndex = 5;
            this.txtFeder.Text = "0";
            this.txtFeder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkMute
            // 
            this.chkMute.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMute.Location = new System.Drawing.Point(4, 5);
            this.chkMute.Name = "chkMute";
            this.chkMute.Size = new System.Drawing.Size(22, 22);
            this.chkMute.TabIndex = 6;
            this.chkMute.Text = "M";
            this.chkMute.UseVisualStyleBackColor = true;
            // 
            // chkSolo
            // 
            this.chkSolo.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSolo.Location = new System.Drawing.Point(28, 5);
            this.chkSolo.Name = "chkSolo";
            this.chkSolo.Size = new System.Drawing.Size(22, 22);
            this.chkSolo.TabIndex = 7;
            this.chkSolo.Text = "S";
            this.chkSolo.UseVisualStyleBackColor = true;
            // 
            // VolumeTracker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Controls.Add(this.chkSolo);
            this.Controls.Add(this.chkMute);
            this.Controls.Add(this.txtFeder);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtPanpot);
            this.Controls.Add(this.trackPanpot);
            this.Controls.Add(this.trackFeder);
            this.DoubleBuffered = true;
            this.Name = "VolumeTracker";
            this.Size = new System.Drawing.Size(85, 284);
            ((System.ComponentModel.ISupportInitialize)(this.trackFeder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPanpot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar trackFeder;
        private TrackBar trackPanpot;
        private TextBox txtPanpot;
        private Label lblTitle;
        private TextBox txtFeder;
        private System.Windows.Forms.CheckBox chkMute;
        private System.Windows.Forms.CheckBox chkSolo;

        #endregion
    }

}
