/*
 * FormCheckUnknownSingerAndResampler.cs
 * Copyright © 2010 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormCheckUnknownSingerAndResampler.java

import java.awt.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using BPaintEventArgs = System.Windows.Forms.PaintEventArgs;
    using BEventHandler = System.EventHandler;
    using BPaintEventHandler = System.Windows.Forms.PaintEventHandler;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormCheckUnknownSingerAndResampler extends BDialog
#else
    public class FormCheckUnknownSingerAndResampler : BDialog
#endif
    {
        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="resamplers"></param>
        /// <param name="singers"></param>
        public FormCheckUnknownSingerAndResampler( Vector<String> singers, Vector<String> resamplers )
        {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            
            // singers
            listSingers.Items.Clear();
            int size = vec.size( singers );
            for ( int i = 0; i < size; i++ ) {
                listSingers.Items.Add( vec.get( singers, i ), true );
            }

            // resampler
            listResampler.Items.Clear();
            size = vec.size( resamplers );
            for ( int i = 0; i < size; i++ ) {
                listResampler.Items.Add( vec.get( resamplers, i ), true );
            }

            registerEventHandlers();
        }

        #region public methods
        /// <summary>
        /// チェックボックスが入れられた原音のパスのリストを取得します
        /// </summary>
        /// <returns></returns>
        public Vector<String> getCheckedSingers()
        {
            Vector<String> ret = new Vector<String>();
#if !JAVA
            int size = listSingers.Items.Count;
            for ( int i = 0; i < size; i++ ) {
                if ( listSingers.GetItemChecked( i ) ) {
                    ret.add( (String)listSingers.Items[i] );
                }
            }
#endif
            return ret;
        }

        /// <summary>
        /// リサンプラーの項目にチェックが入れられたかどうか
        /// </summary>
        /// <returns></returns>
        public Vector<String> getCheckedResamplers()
        {
            Vector<String> ret = new Vector<String>();
#if !JAVA
            int size = listResampler.Items.Count;
            for ( int i = 0; i < size; i++ ) {
                if ( listResampler.GetItemChecked( i ) ) {
                    ret.add( (String)listResampler.Items[i] );
                }
            }
            //TODO:
#endif
            return ret;
        }
        #endregion

        #region helper methods
        /// <summary>
        /// イベントハンドラを登録します
        /// </summary>
        private void registerEventHandlers()
        {
        }

        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private void applyLanguage()
        {
            setTitle( _( "Unknown singers and resamplers" ) );
            labelMessage.setText( _( "These singers and resamplers are not registered to Cadencii.\nCheck the box if you want to register them." ) );
            labelSinger.setText( _( "Singer" ) );
            labelResampler.setText( _( "Resampler" ) );
        }
        #endregion

        private System.Windows.Forms.CheckedListBox listSingers;


#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/cadencii/FormCheckUnknownSingerAndResampler.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/cadencii/FormCheckUnknownSingerAndResampler.java
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
        protected override void Dispose( boolean disposing )
        {
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
        private void InitializeComponent()
        {
            this.buttonCancel = new org.kbinani.windows.forms.BButton();
            this.buttonOk = new org.kbinani.windows.forms.BButton();
            this.labelSinger = new org.kbinani.windows.forms.BLabel();
            this.listResampler = new System.Windows.Forms.CheckedListBox();
            this.labelResampler = new org.kbinani.windows.forms.BLabel();
            this.labelMessage = new org.kbinani.windows.forms.BLabel();
            this.listSingers = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point( 236, 347 );
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size( 75, 23 );
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point( 155, 347 );
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size( 75, 23 );
            this.buttonOk.TabIndex = 10;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // labelSinger
            // 
            this.labelSinger.AutoSize = true;
            this.labelSinger.Location = new System.Drawing.Point( 12, 86 );
            this.labelSinger.Name = "labelSinger";
            this.labelSinger.Size = new System.Drawing.Size( 37, 12 );
            this.labelSinger.TabIndex = 134;
            this.labelSinger.Text = "Singer";
            // 
            // listResampler
            // 
            this.listResampler.FormattingEnabled = true;
            this.listResampler.HorizontalScrollbar = true;
            this.listResampler.Location = new System.Drawing.Point( 14, 235 );
            this.listResampler.Name = "listResampler";
            this.listResampler.ScrollAlwaysVisible = true;
            this.listResampler.Size = new System.Drawing.Size( 291, 88 );
            this.listResampler.TabIndex = 145;
            // 
            // labelResampler
            // 
            this.labelResampler.AutoSize = true;
            this.labelResampler.Location = new System.Drawing.Point( 12, 216 );
            this.labelResampler.Name = "labelResampler";
            this.labelResampler.Size = new System.Drawing.Size( 59, 12 );
            this.labelResampler.TabIndex = 144;
            this.labelResampler.Text = "Resampler";
            // 
            // labelMessage
            // 
            this.labelMessage.Location = new System.Drawing.Point( 12, 12 );
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size( 293, 57 );
            this.labelMessage.TabIndex = 146;
            this.labelMessage.Text = "These singers and resamplers are not registered to Cadencii.\r\nCheck the box if yo" +
                "u want to register them.";
            // 
            // listSingers
            // 
            this.listSingers.FormattingEnabled = true;
            this.listSingers.HorizontalScrollbar = true;
            this.listSingers.Location = new System.Drawing.Point( 14, 105 );
            this.listSingers.Name = "listSingers";
            this.listSingers.ScrollAlwaysVisible = true;
            this.listSingers.Size = new System.Drawing.Size( 291, 88 );
            this.listSingers.TabIndex = 147;
            // 
            // FormCheckUnknownSingerAndResampler
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 96F, 96F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size( 323, 382 );
            this.Controls.Add( this.listSingers );
            this.Controls.Add( this.labelMessage );
            this.Controls.Add( this.listResampler );
            this.Controls.Add( this.labelResampler );
            this.Controls.Add( this.labelSinger );
            this.Controls.Add( this.buttonOk );
            this.Controls.Add( this.buttonCancel );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCheckUnknownSingerAndResampler";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Unknown singers and resamplers";
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion

        private BButton buttonCancel;
        private BButton buttonOk;
        private BLabel labelSinger;
        private System.Windows.Forms.CheckedListBox listResampler;
        private org.kbinani.windows.forms.BLabel labelResampler;
        private org.kbinani.windows.forms.BLabel labelMessage;

        #endregion
#endif
    }

#if !JAVA
}
#endif
