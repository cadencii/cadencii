/*
 * FormNoteProperty.cs
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
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormNoteProperty extends BForm{
#else
    public class FormNoteProperty : BForm {
#endif
        public FormNoteProperty() {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            this.Text = _( "Note Property" );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public BKeys[] getFormCloseShortcutKey() {
            return PortUtil.getBKeysFromKeyStroke( menuClose.getAccelerator() );
        }

        public void setFormCloseShortcutKey( BKeys[] value ) {
            menuClose.setAccelerator( PortUtil.getKeyStrokeFromBKeys( value ) );
        }

        private void menuClose_Click( object sender, EventArgs e ) {
            this.Close();
        }

        private void registerEventHandlers() {
            this.menuClose.Click += new System.EventHandler( this.menuClose_Click );
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
            this.menuStrip = new BMenuBar();
            this.menuWindow = new BMenuItem();
            this.menuClose = new BMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuWindow} );
            this.menuStrip.Location = new System.Drawing.Point( 0, 0 );
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size( 188, 24 );
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.Visible = false;
            // 
            // menuWindow
            // 
            this.menuWindow.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuClose} );
            this.menuWindow.Name = "menuWindow";
            this.menuWindow.Size = new System.Drawing.Size( 72, 20 );
            this.menuWindow.Text = "Window(&W)";
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size( 115, 22 );
            this.menuClose.Text = "Close(&C)";
            // 
            // FormNoteProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size( 188, 291 );
            this.Controls.Add( this.menuStrip );
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "FormNoteProperty";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Note Property";
            this.TopMost = true;
            this.menuStrip.ResumeLayout( false );
            this.menuStrip.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BMenuBar menuStrip;
        private BMenuItem menuWindow;
        private BMenuItem menuClose;
        #endregion
#endif
    }

#if !JAVA
}
#endif
