/*
 * VersionInfo.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Windows.Forms;
using cadencii;
using cadencii.java.awt;

namespace cadencii.apputil
{
    using java = cadencii.java;
    using javax = cadencii.javax;
    using Graphics = cadencii.java.awt.Graphics2D;

    public partial class VersionInfo : System.Windows.Forms.Form
    {
        DateTime m_scroll_started;
        private AuthorListEntry[] m_credit;
        const float m_speed = 35f;
        string m_version;
        bool m_credit_mode = false;
        float m_last_t = 0f;
        float m_last_speed = 0f;
        float m_shift = 0f;
        int m_button_width_about = 75;
        int m_button_width_credit = 75;
        Image m_scroll;
        const int m_height = 380;
        readonly Color m_background = Color.white;
        private string m_app_name = "";
        private Color m_app_name_color = Color.black;
        private Color m_version_color = new Color(105, 105, 105);
        private bool m_shadow_enablde = true;

        public VersionInfo(string app_name, string version)
        {
            m_version = version;
            m_app_name = app_name;
            InitializeComponent();
            applyLanguage();

            this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);

            m_credit = new AuthorListEntry[] { };
            btnSaveAuthorList.Visible = false;
#if DEBUG
            GenerateAuthorList();
            btnSaveAuthorList.Visible = true;
            btnSaveAuthorList.Click += new EventHandler(btnSaveAuthorList_Click);
#endif
        }

        public bool SaveAuthorListVisible
        {
            set
            {
                btnSaveAuthorList.Visible = value;
            }
        }

        public static string _(string s)
        {
            return Messaging.getMessage(s);
        }

        /// <summary>
        /// バージョン番号表示の文字色を取得または設定します
        /// </summary>
        public Color VersionColor
        {
            get
            {
                return m_version_color;
            }
            set
            {
                m_version_color = value;
            }
        }

        /// <summary>
        /// アプリケーション名表示の文字色を取得または設定します
        /// </summary>
        public Color AppNameColor
        {
            get
            {
                return m_app_name_color;
            }
            set
            {
                m_app_name_color = value;
            }
        }

        public java.awt.Image Credit
        {
            set
            {
                m_scroll = value;
            }
        }

        public string AppName
        {
            get
            {
                return m_app_name;
            }
            set
            {
                m_app_name = value;
            }
        }

        public AuthorListEntry[] AuthorList
        {
            set
            {
                m_credit = value;
#if DEBUG
                GenerateAuthorList();
#endif
            }
        }

        private void GenerateAuthorList()
        {
            const float shadow_shift = 2f;
            const string font_name = "Arial";
            const int font_size = 10;
            Font font = new Font(font_name, java.awt.Font.PLAIN, font_size);
            Dimension size = cadencii.apputil.Util.measureString("Qjqp", font);
            float width = this.Width;
            float height = size.height;
            //StringFormat sf = new StringFormat();
            m_scroll = new Image();
            m_scroll.image = new System.Drawing.Bitmap((int)width, (int)(40f + m_credit.Length * height * 1.1f), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics2D g = new Graphics2D(System.Drawing.Graphics.FromImage(m_scroll.image));
            //sf.Alignment = StringAlignment.Center;
            g.setFont(new Font(font_name, java.awt.Font.BOLD, (int)(font_size * 1.1f)));
            if (m_shadow_enablde) {
                g.setColor(new Color(0, 0, 0, 40));
                g.drawString(m_app_name, shadow_shift, shadow_shift); //, width, height ), sf );
            }
            g.setColor(Color.black);
            g.drawString(m_app_name, 0f, 0f); //, width, height ), sf );
            for (int i = 0; i < m_credit.Length; i++) {
                g.setFont(new Font(font_name, m_credit[i].getStyle(), font_size));
                if (m_shadow_enablde) {
                    g.setColor(new Color(0, 0, 0, 40));
                    g.drawString(m_credit[i].getName(), 0f + shadow_shift, 40f + i * height * 1.1f + shadow_shift); //, width, height ), sf );
                }
                g.setColor(Color.black);
                g.drawString(m_credit[i].getName(), 0f, 40f + i * height * 1.1f);// , width, height ), sf );
            }
        }

        void btnSaveAuthorList_Click(object sender, EventArgs e)
        {
#if DEBUG
            using (var dlg = new System.Windows.Forms.SaveFileDialog()) {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    using (var stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write)) {
                        m_scroll.image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
#endif
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnFlip_Click(object sender, EventArgs e)
        {
            m_credit_mode = !m_credit_mode;
            if (m_credit_mode) {
                btnFlip.Width = m_button_width_about;
                btnFlip.Text = string.Format(_("About {0}"), m_app_name);
                m_scroll_started = DateTime.Now;
                m_last_speed = 0f;
                m_last_t = 0f;
                m_shift = 0f;

                timer.Enabled = true;
            } else {
                timer.Enabled = false;
                btnFlip.Width = m_button_width_credit;
                btnFlip.Text = _("Credit");
            }
            this.Invalidate();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void VersionInfoEx_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            try {
                paint(new Graphics2D(e.Graphics));
            } catch (Exception ex) {
#if DEBUG
                Console.WriteLine("VersionInfoEx_Paint");
                Console.WriteLine(ex.StackTrace);
#endif
            }
        }

        public void paint(Graphics g1)
        {
            Graphics2D g = (Graphics2D)g1;
            g.clipRect(0, 0, this.Width, m_height);
            g.clearRect(0, 0, this.Width, this.Height);
            if (m_credit_mode) {
                float times = (float)(((DateTime.Now).Subtract(m_scroll_started)).TotalSeconds) - 3f;
                float speed = (float)((2.0 - cadencii.math.erfc(times * 0.8)) / 2.0) * m_speed;
                float dt = times - m_last_t;
                m_shift += (speed + m_last_speed) * dt / 2f;
                m_last_t = times;
                m_last_speed = speed;
                float dx = (this.Width - m_scroll.getWidth(null)) * 0.5f;
                if (m_scroll != null) {
                    g.drawImage(m_scroll, (int)dx, (int)(90f - m_shift), null);
                    if (90f - m_shift + m_scroll.getHeight(null) < 0) {
                        m_shift = -m_height * 1.5f;
                    }
                }
                int grad_height = 60;
                Rectangle top = new Rectangle(0, 0, this.Width, grad_height);
                /*using ( LinearGradientBrush lgb = new LinearGradientBrush( top, Color.White, Color.Transparent, LinearGradientMode.Vertical ) ) {
                    g.FillRectangle( lgb, top );
                }*/
                Rectangle bottom = new Rectangle(0, m_height - grad_height, this.Width, grad_height);
                g.clipRect(0, m_height - grad_height + 1, this.Width, grad_height - 1);
                /*using ( LinearGradientBrush lgb = new LinearGradientBrush( bottom, Color.Transparent, Color.White, LinearGradientMode.Vertical ) ) {
                    g.FillRectangle( lgb, bottom );
                }*/
                g.setClip(null);
            } else {
                g.setFont(new Font("Century Gorhic", java.awt.Font.BOLD, 24));
                g.setColor(m_app_name_color);
                g.drawString(m_app_name, 20, 110);
                g.setFont(new Font("Arial", 0, 10));
                g.drawString("version " + m_version, 25, 150);
            }
        }

        private void VersionInfoEx_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode & System.Windows.Forms.Keys.Escape) == System.Windows.Forms.Keys.Escape) {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }

        private void VersionInfoEx_FontChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Controls.Count; i++) {
                Util.applyFontRecurse(this.Controls[i], new java.awt.Font(this.Font));
            }
        }
    }

}
