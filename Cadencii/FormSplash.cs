#if !JAVA
/*
 * FormSplash.cs
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.awt.*;
import javax.imageio.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.java.awt;
using org.kbinani.javax.imageio;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 起動時に表示されるスプラッシュウィンドウ
    /// </summary>
#if JAVA
    public class FormSplash extends BDialog {
#else
    public class FormSplash : BDialog {
#endif

#if !JAVA
        /// <summary>
        /// addIconメソッドを呼び出すときに使うデリゲート
        /// </summary>
        /// <param name="path_image"></param>
        /// <param name="singer_name"></param>
        private delegate void AddIconThreadSafeDelegate( String path_image, String singer_name );
#endif

        boolean mouseDowned = false;
        private FlowLayoutPanel panelIcon;
        private ToolTip toolTip;
        private System.ComponentModel.IContainer components;
        Point mouseDownedLocation = new Point( 0, 0 );

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSplash() {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
        }

        #region public methods
        /// <summary>
        /// アイコンパレードの末尾にアイコンを追加します。デリゲートを使用し、スレッド・セーフな処理を行います。
        /// </summary>
        /// <param name="path_image"></param>
        /// <param name="singer_name"></param>
        public void addIconThreadSafe( String path_image, String singer_name ) {
            Delegate deleg = (Delegate)new AddIconThreadSafeDelegate( addIcon );
            if ( deleg != null ) {
                this.Invoke( deleg, new String[] { path_image, singer_name } );
            }
        }

        /// <summary>
        /// アイコンパレードの末尾にアイコンを追加します
        /// </summary>
        /// <param name="path_image">イメージファイルへのパス</param>
        /// <param name="singer_name">歌手の名前</param>
        public void addIcon( String path_image, String singer_name ) {
#if JAVA
            //fixme: FormSplash#addIcon(String,String)
#else
            IconParader p = new IconParader();

            if ( PortUtil.isFileExists( path_image ) ) {
                System.IO.FileStream fs = null;
                try {
                    fs = new System.IO.FileStream( path_image, System.IO.FileMode.Open, System.IO.FileAccess.Read );
                    System.Drawing.Image img = System.Drawing.Image.FromStream( fs );
                    p.setImage( img );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "FormSplash#addIcon; ex=" + ex );
                } finally {
                    if ( fs != null ) {
                        try {
                            fs.Close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "FormSplash#addICon; ex2=" + ex2 );
                        }
                    }
                }
            } else {
                // 画像ファイルが無かった場合
#if JAVA
                p.setImage( null );
#else
                // 歌手名が描かれた画像をセットする
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap( IconParader.ICON_WIDTH, IconParader.ICON_HEIGHT );
                using ( System.Drawing.Graphics g = System.Drawing.Graphics.FromImage( bmp ) ) {
                    g.Clear( System.Drawing.Color.White );
                    System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
                    sf.Alignment = System.Drawing.StringAlignment.Near;
                    sf.LineAlignment = System.Drawing.StringAlignment.Near;
                    g.DrawString(
                        singer_name, SystemInformation.MenuFont, System.Drawing.Brushes.Black,
                        new System.Drawing.RectangleF( 1, 1, IconParader.ICON_WIDTH - 2, IconParader.ICON_HEIGHT - 2 ),
                        sf );
                }
                p.setImage( bmp );
#endif
            }

            panelIcon.BringToFront();
            panelIcon.Controls.Add( p );
#endif
        }
        #endregion

        #region helper methods
        private void setResources() {
#if !JAVA
            this.BackgroundImage = Resources.get_splash().image;
#endif
        }

        private void registerEventHandlers() {
            PortUtil.stderr.println( "foo" );
#if JAVA
            PortUtil.stdout.println( "//TODO: fixme: FormSplash#registerEventHandlers" );
#else
            MouseDown += new System.Windows.Forms.MouseEventHandler( FormSplash_MouseDown );
            MouseUp += new System.Windows.Forms.MouseEventHandler( FormSplash_MouseUp );
            MouseMove += new System.Windows.Forms.MouseEventHandler( FormSplash_MouseMove );
#endif
        }
        #endregion

        #region event handlers
        public void FormSplash_MouseDown( Object sender, MouseEventArgs e ) {
            mouseDowned = true;
            mouseDownedLocation = new Point( e.X, e.Y );
        }

        public void FormSplash_MouseUp( Object sender, MouseEventArgs e ) {
            mouseDowned = false;
        }

        public void FormSplash_MouseMove( Object sender, MouseEventArgs e ) {
            if ( !mouseDowned ) return;
            Point globalMouseLocation = PortUtil.getMousePosition();
            this.setLocation( globalMouseLocation.x - mouseDownedLocation.x, globalMouseLocation.y - mouseDownedLocation.y );
        }
        #endregion

        #region ui implementation
#if JAVA
        private void initialize(){
            setSize( 500, 335 );
        }
#else
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.panelIcon = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.SuspendLayout();
            // 
            // panelIcon
            // 
            this.panelIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelIcon.BackColor = System.Drawing.Color.Transparent;
            this.panelIcon.Location = new System.Drawing.Point( 12, 200 );
            this.panelIcon.Name = "panelIcon";
            this.panelIcon.Size = new System.Drawing.Size( 476, 123 );
            this.panelIcon.TabIndex = 1;
            // 
            // FormSplash
            // 
            this.ClientSize = new System.Drawing.Size( 500, 335 );
            this.Controls.Add( this.panelIcon );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSplash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ResumeLayout( false );

        }
#endif
        #endregion

    }

#if !JAVA
}
#endif
#endif
