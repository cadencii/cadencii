/*
 * VersionInfo.cs
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
#if JAVA
package org.kbinani.Cadencii;

import java.awt.*;
import java.awt.image.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
import org.kbinani.apputil.*;
#else
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.awt;
using bocoree.awt.image;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
    using Graphics = Graphics2D;
    using java = bocoree;
    using javax = bocoreex;
    using BEventArgs = System.EventArgs;
    using BPaintEventArgs = System.Windows.Forms.PaintEventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
#endif

#if JAVA
    public class VersionInfo extends BForm {
#else
    public class VersionInfo : BForm {
#endif
        double m_scroll_started;
        private AuthorListEntry[] m_credit;
        const float m_speed = 35f;
        String m_version;
        boolean m_credit_mode = false;
        float m_last_t = 0f;
        float m_last_speed = 0f;
        float m_shift = 0f;
        int m_button_width_about = 75;
        int m_button_width_credit = 75;
        BufferedImage m_scroll;
        const int m_height = 380;
        readonly Color m_background = Color.white;
        private String m_app_name = "";
        private Color m_app_name_color = Color.black;
        private Color m_version_color = new Color( 105, 105, 105 );
        private boolean m_shadow_enablde = false;

        public VersionInfo( String app_name, String version ) {
            m_version = version;
            m_app_name = app_name;
            InitializeComponent();
            registerEventHandlers();
            setResources();
            ApplyLanguage();

            this.SetStyle( ControlStyles.DoubleBuffer, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );

            m_credit = new AuthorListEntry[] { };
            btnSaveAuthorList.Visible = false;
            lblVstLogo.ForeColor = m_version_color.color;
            lblStraightAcknowledgement.ForeColor = m_version_color.color;
#if DEBUG
            GenerateAuthorList();
            btnSaveAuthorList.Visible = true;
            btnSaveAuthorList.Click += new EventHandler( btnSaveAuthorList_Click );
#endif
        }

        public void ApplyLanguage() {
            String about = String.Format( _( "About {0}" ), m_app_name );
            String credit = _( "Credit" );
            Dimension size1 = Util.measureString( about, new java.awt.Font( btnFlip.Font ) );
            Dimension size2 = Util.measureString( credit, new java.awt.Font( btnFlip.Font ) );
            m_button_width_about = Math.Max( 75, (int)(size1.width * 1.3) );
            m_button_width_credit = Math.Max( 75, (int)(size2.width * 1.3) );
            if ( m_credit_mode ) {
                btnFlip.Width = m_button_width_about;
                btnFlip.Text = about;
            } else {
                btnFlip.Width = m_button_width_credit;
                btnFlip.Text = credit;
            }
            this.Text = about;
        }

        public void setSaveAuthorListVisible( boolean value ) {
            btnSaveAuthorList.Visible = value;
        }

        public static String _( String s ) {
            return Messaging.getMessage( s );
        }

        /// <summary>
        /// バージョン番号表示の文字色を取得または設定します
        /// </summary>
        public Color getVersionColor() {
            return m_version_color;
        }

        public void setVersionColor( Color value ) {
            m_version_color = value;
            lblVstLogo.ForeColor = value.color;
            lblStraightAcknowledgement.ForeColor = value.color;
        }

        /// <summary>
        /// アプリケーション名表示の文字色を取得または設定します
        /// </summary>
        public Color getAppNameColor() {
            return m_app_name_color;
        }

        public void setAppNameColor( Color value ) {
            m_app_name_color = value;
        }

#if !JAVA
        public void setCredit( BufferedImage value ) {
            m_scroll = value;
        }
#endif

        public String getAppName() {
            return m_app_name;
        }

        public void setAppName( String value ) {
            m_app_name = value;
        }

        public void setAuthorList( AuthorListEntry[] value ) {
            m_credit = value;
#if JAVA
            GenerateAuthorList();
#else
            GenerateAuthorList();
#endif
        }

        private void GenerateAuthorList() {
            int shadow_shift = 2;
            string font_name = "Arial";
            int font_size = 10;
            Font font = new Font( font_name, java.awt.Font.PLAIN, font_size );
            Dimension size = Boare.Lib.AppUtil.Util.measureString( "the quick brown fox jumped over the lazy dogs. THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS. 0123456789", font );
            int width = this.Width;
            int height = size.height;
            //StringFormat sf = new StringFormat();
            m_scroll = new BufferedImage( (int)width, (int)(40f + m_credit.Length * height * 1.1f), BufferedImage.TYPE_INT_BGR );
            Graphics2D g = m_scroll.createGraphics();
            g.setColor( Color.white );
            g.fillRect( 0, 0, m_scroll.getWidth( null ), m_scroll.getHeight( null ) );
            int align = 0;
            int valign = 0;
            //sf.Alignment = StringAlignment.Center;
            Font f = new Font( font_name, java.awt.Font.BOLD, (int)(font_size * 1.1f) );
            if ( m_shadow_enablde ) {
                g.setColor( new Color( 0, 0, 0, 40 ) );
                PortUtil.drawStringEx( g, m_app_name,
                                       f,
                                       new Rectangle( shadow_shift, shadow_shift, width, height ),
                                       align,
                                       valign );
            }
            g.setColor( Color.black );
            PortUtil.drawStringEx( g,
                                   m_app_name,
                                   f,
                                   new Rectangle( 0, 0, width, height ),
                                   align,
                                   valign );
            for ( int i = 0; i < m_credit.Length; i++ ) {
                Font f2 = new Font( font_name, m_credit[i].getStyle(), font_size );
                if ( m_shadow_enablde ) {
                    g.setColor( new Color( 0, 0, 0, 40 ) );
                    PortUtil.drawStringEx( g,
                                           m_credit[i].getName(),
                                           font,
                                           new Rectangle( 0 + shadow_shift, 40 + (int)(i * height * 1.1) + shadow_shift, width, height ),
                                           align,
                                           valign );
                }
                g.setColor( Color.black );
                PortUtil.drawStringEx( g,
                                       m_credit[i].getName(),
                                       f2,
                                       new Rectangle( 0, 40 + (int)(i * height * 1.1), width, height ),
                                       align,
                                       valign );
            }
        }

        void btnSaveAuthorList_Click( Object sender, BEventArgs e ) {
#if DEBUG
            using ( SaveFileDialog dlg = new SaveFileDialog() ) {
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    javax.imageio.ImageIO.write( m_scroll, "png", new java.io.File( dlg.FileName ) );
                }
            }
#endif
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnFlip_Click( Object sender, BEventArgs e ) {
            m_credit_mode = !m_credit_mode;
            if ( m_credit_mode ) {
                btnFlip.Width = m_button_width_about;
                btnFlip.Text = String.Format( _( "About {0}" ), m_app_name );
                m_scroll_started = PortUtil.getCurrentTime();
                m_last_speed = 0f;
                m_last_t = 0f;
                m_shift = 0f;
                pictVstLogo.Visible = false;
                lblVstLogo.Visible = false;
                lblStraightAcknowledgement.Visible = false;
                timer.Enabled = true;
            } else {
                timer.Enabled = false;
                btnFlip.Width = m_button_width_credit;
                btnFlip.Text = _( "Credit" );
                pictVstLogo.Visible = true;
                lblVstLogo.Visible = true;
                lblStraightAcknowledgement.Visible = true;
            }
            this.Invalidate();
        }

        private void timer_Tick( Object sender, BEventArgs e ) {
            this.Invalidate();
        }

        private void VersionInfoEx_Paint( Object sender, BPaintEventArgs e ) {
            try {
                paint( new Graphics2D( e.Graphics ) );
            } catch ( Exception ex ) {
#if DEBUG
                Console.WriteLine( "VersionInfoEx_Paint" );
                Console.WriteLine( ex.StackTrace );
#endif
            }
        }

        public void paint( Graphics g1 ) {
            Graphics2D g = (Graphics2D)g1;
            g.clipRect( 0, 0, this.Width, m_height );
            g.clearRect( 0, 0, this.Width, this.Height );
            if ( m_credit_mode ) {
                float times = (float)(PortUtil.getCurrentTime() - m_scroll_started) - 3f;
                float speed = (float)((2.0 - bocoree.math.erfcc( times * 0.8 )) / 2.0) * m_speed;
                float dt = times - m_last_t;
                m_shift += (speed + m_last_speed) * dt / 2f;
                m_last_t = times;
                m_last_speed = speed;
                float dx = (this.Width - m_scroll.getWidth( null )) * 0.5f;
                if ( m_scroll != null ) {
                    g.drawImage( m_scroll, (int)dx, (int)(90f - m_shift), null );
                    if ( 90f - m_shift + m_scroll.getHeight( null ) < 0 ) {
                        m_shift = -m_height * 1.5f;
                    }
                }
                int grad_height = 60;
                Rectangle top = new Rectangle( 0, 0, this.Width, grad_height );
                /*using ( LinearGradientBrush lgb = new LinearGradientBrush( top, Color.White, Color.Transparent, LinearGradientMode.Vertical ) ) {
                    g.FillRectangle( lgb, top );
                }*/
                Rectangle bottom = new Rectangle( 0, m_height - grad_height, this.Width, grad_height );
                g.clipRect( 0, m_height - grad_height + 1, this.Width, grad_height - 1 );
                /*using ( LinearGradientBrush lgb = new LinearGradientBrush( bottom, Color.Transparent, Color.White, LinearGradientMode.Vertical ) ) {
                    g.FillRectangle( lgb, bottom );
                }*/
                g.setClip( null );
            } else {
                g.setFont( new Font( "Century Gorhic", java.awt.Font.BOLD, 24 ) );
                g.setColor( m_app_name_color );
                g.drawString( m_app_name, 20, 60 );
                g.setFont( new Font( "Arial", 0, 10 ) );
                g.drawString( "version " + m_version, 25, 100 );
            }
        }

        private void VersionInfoEx_KeyDown( Object sender, BKeyEventArgs e ) {
            if ( (e.KeyCode & Keys.Escape) == Keys.Escape ) {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void VersionInfoEx_FontChanged( Object sender, BEventArgs e ) {
            for ( int i = 0; i < this.Controls.Count; i++ ) {
                Util.applyFontRecurse( this.Controls[i], new java.awt.Font( this.Font ) );
            }
        }

        private void registerEventHandlers() {
            this.btnFlip.Click += new System.EventHandler( this.btnFlip_Click );
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            this.Paint += new System.Windows.Forms.PaintEventHandler( this.VersionInfoEx_Paint );
            this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.VersionInfoEx_KeyDown );
            this.FontChanged += new System.EventHandler( this.VersionInfoEx_FontChanged );
        }

        private void setResources() {
            this.pictVstLogo.Image = Resources.get_VSTonWht();
        }

#if JAVA
        #region UI Impl for Java
        #endregion
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
            this.btnFlip = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.btnSaveAuthorList = new System.Windows.Forms.Button();
            this.lblVstLogo = new System.Windows.Forms.Label();
            this.pictVstLogo = new System.Windows.Forms.PictureBox();
            this.lblStraightAcknowledgement = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictVstLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFlip
            // 
            this.btnFlip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlip.Location = new System.Drawing.Point( 13, 391 );
            this.btnFlip.Name = "btnFlip";
            this.btnFlip.Size = new System.Drawing.Size( 75, 21 );
            this.btnFlip.TabIndex = 2;
            this.btnFlip.Text = "クレジット";
            this.btnFlip.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 211, 391 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 21 );
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Interval = 30;
            // 
            // btnSaveAuthorList
            // 
            this.btnSaveAuthorList.Location = new System.Drawing.Point( 123, 391 );
            this.btnSaveAuthorList.Name = "btnSaveAuthorList";
            this.btnSaveAuthorList.Size = new System.Drawing.Size( 43, 21 );
            this.btnSaveAuthorList.TabIndex = 3;
            this.btnSaveAuthorList.Text = "button1";
            this.btnSaveAuthorList.UseVisualStyleBackColor = true;
            this.btnSaveAuthorList.Visible = false;
            // 
            // lblVstLogo
            // 
            this.lblVstLogo.BackColor = System.Drawing.Color.White;
            this.lblVstLogo.Location = new System.Drawing.Point( 25, 238 );
            this.lblVstLogo.Name = "lblVstLogo";
            this.lblVstLogo.Size = new System.Drawing.Size( 263, 32 );
            this.lblVstLogo.TabIndex = 5;
            this.lblVstLogo.Text = "VST PlugIn Technology by Steinberg Media Technologies GmbH";
            // 
            // pictVstLogo
            // 
            this.pictVstLogo.BackColor = System.Drawing.Color.White;
            this.pictVstLogo.Location = new System.Drawing.Point( 27, 265 );
            this.pictVstLogo.Name = "pictVstLogo";
            this.pictVstLogo.Size = new System.Drawing.Size( 88, 60 );
            this.pictVstLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictVstLogo.TabIndex = 4;
            this.pictVstLogo.TabStop = false;
            // 
            // lblStraightAcknowledgement
            // 
            this.lblStraightAcknowledgement.BackColor = System.Drawing.Color.White;
            this.lblStraightAcknowledgement.Location = new System.Drawing.Point( 25, 328 );
            this.lblStraightAcknowledgement.Name = "lblStraightAcknowledgement";
            this.lblStraightAcknowledgement.Size = new System.Drawing.Size( 263, 40 );
            this.lblStraightAcknowledgement.TabIndex = 6;
            this.lblStraightAcknowledgement.Text = "Components of Cadencii, \"vConnect.exe\" and \"straightVoiceDB.exe\", are powererd by" +
                " STRAIGHT LIBRARY.";
            // 
            // VersionInfo
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size( 300, 423 );
            this.Controls.Add( this.pictVstLogo );
            this.Controls.Add( this.lblStraightAcknowledgement );
            this.Controls.Add( this.lblVstLogo );
            this.Controls.Add( this.btnSaveAuthorList );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnFlip );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size( 306, 451 );
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size( 306, 451 );
            this.Name = "VersionInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VersionInfoEx";
            ((System.ComponentModel.ISupportInitialize)(this.pictVstLogo)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button btnFlip;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnSaveAuthorList;
        private System.Windows.Forms.PictureBox pictVstLogo;
        private System.Windows.Forms.Label lblVstLogo;
        private System.Windows.Forms.Label lblStraightAcknowledgement;
        #endregion
#endif
    }

#if !JAVA
}
#endif
