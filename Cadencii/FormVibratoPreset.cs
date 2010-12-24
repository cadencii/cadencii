/*
 * FormVibratoPreset.cs
 * Copyright c 2010 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoConfig.java

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
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormVibratoPreset extends BDialog{
#else
    public class FormVibratoPreset : BDialog
    {
#endif
        private BListView listDictionaries;
        private org.kbinani.windows.forms.BButton btnRemove;
        private org.kbinani.windows.forms.BButton btnAdd;
        private org.kbinani.windows.forms.BButton btnUp;
        private org.kbinani.windows.forms.BButton btnDown;

        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="vibrato_handle"></param>
        public FormVibratoPreset()
        {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif

        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoConfig.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoConfig.java
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup( "Name", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup( "Start rate", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup( "Start depth", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem( "DEFAULT_JP" );
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.listDictionaries = new org.kbinani.windows.forms.BListView();
            this.btnRemove = new org.kbinani.windows.forms.BButton();
            this.btnAdd = new org.kbinani.windows.forms.BButton();
            this.btnUp = new org.kbinani.windows.forms.BButton();
            this.btnDown = new org.kbinani.windows.forms.BButton();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 260, 267 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 179, 267 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // listDictionaries
            // 
            this.listDictionaries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listDictionaries.CheckBoxes = true;
            this.listDictionaries.FullRowSelect = true;
            this.listDictionaries.GridLines = true;
            listViewGroup1.Header = "Name";
            listViewGroup1.Name = "listViewGroupName";
            listViewGroup2.Header = "Start rate";
            listViewGroup2.Name = "listViewGroupRate";
            listViewGroup3.Header = "Start depth";
            listViewGroup3.Name = "listViewGroupDepth";
            this.listDictionaries.Groups.AddRange( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3} );
            listViewItem1.Checked = true;
            listViewItem1.StateImageIndex = 1;
            this.listDictionaries.Items.AddRange( new System.Windows.Forms.ListViewItem[] {
            listViewItem1} );
            this.listDictionaries.Location = new System.Drawing.Point( 12, 12 );
            this.listDictionaries.Name = "listDictionaries";
            this.listDictionaries.Size = new System.Drawing.Size( 323, 204 );
            this.listDictionaries.TabIndex = 8;
            this.listDictionaries.UseCompatibleStateImageBehavior = false;
            this.listDictionaries.View = System.Windows.Forms.View.Details;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point( 93, 222 );
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size( 75, 23 );
            this.btnRemove.TabIndex = 126;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point( 12, 222 );
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size( 75, 23 );
            this.btnAdd.TabIndex = 125;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point( 179, 222 );
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size( 75, 23 );
            this.btnUp.TabIndex = 127;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point( 260, 222 );
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size( 75, 23 );
            this.btnDown.TabIndex = 128;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // FormVibratoPreset
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 347, 302 );
            this.Controls.Add( this.btnRemove );
            this.Controls.Add( this.btnAdd );
            this.Controls.Add( this.btnUp );
            this.Controls.Add( this.btnDown );
            this.Controls.Add( this.listDictionaries );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVibratoPreset";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vibrato preset";
            this.ResumeLayout( false );

        }
        #endregion

        private BButton btnCancel;
        private BButton btnOK;
        #endregion
#endif
    }

#if !JAVA
}
#endif
