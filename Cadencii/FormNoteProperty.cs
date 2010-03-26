/*
 * FormNoteProperty.cs
 * Copyright (C) 2009-2010 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormNoteProperty.java

import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class FormNoteProperty extends BForm{
#else
    public class FormNoteProperty : BForm {
#endif
        public FormNoteProperty() {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage() {
            setTitle( _( "Note Property" ) );
        }

        public BKeys[] getFormCloseShortcutKey() {
            return PortUtil.getBKeysFromKeyStroke( menuClose.getAccelerator() );
        }

        public void setFormCloseShortcutKey( BKeys[] value ) {
            menuClose.setAccelerator( PortUtil.getKeyStrokeFromBKeys( value ) );
        }
        #endregion

        #region helper methods
        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers() {
            menuClose.clickEvent.add( new BEventHandler( this, "menuClose_Click" ) );
        }

        private void setResources() {
        }
        #endregion

        #region event handlers
        public void menuClose_Click( Object sender, BEventArgs e ) {
            close();
        }
        #endregion

        #region UI implementation
#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormNoteProperty.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormNoteProperty.java
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
        #endregion

    }

#if !JAVA
}
#endif
