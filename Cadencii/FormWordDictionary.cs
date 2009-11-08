/*
 * FormWordDictionary.cs
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
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;

    class FormWordDictionary : BForm {
        public FormWordDictionary() {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            Text = _( "User Dictionary Configuration" );
            lblAvailableDictionaries.Text = _( "Available Dictionaries" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
            btnUp.Text = _( "Up" );
            btnDown.Text = _( "Down" );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void FormWordDictionary_Load( object sender, EventArgs e ) {
            listDictionaries.Items.Clear();
            for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
                String name = SymbolTable.getSymbolTable( i ).getName();
                boolean enabled = SymbolTable.getSymbolTable( i ).isEnabled();
                listDictionaries.Items.Add( name, enabled );
            }
        }

        public Vector<ValuePair<String, Boolean>> Result {
            get {
                Vector<ValuePair<String, Boolean>> ret = new Vector<ValuePair<String, Boolean>>();
                for ( int i = 0; i < listDictionaries.Items.Count; i++ ) {
                    ret.add( new ValuePair<String, Boolean>( (String)listDictionaries.Items[i], listDictionaries.GetItemChecked( i ) ) );
                }
                return ret;
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.OK;
        }

        private void btnUp_Click( object sender, EventArgs e ) {
            int index = listDictionaries.SelectedIndex;
            if ( index >= 1 ) {
                listDictionaries.ClearSelected();
                String upper_name = (String)listDictionaries.Items[index - 1];
                boolean upper_enabled = listDictionaries.GetItemChecked( index - 1 );
                listDictionaries.Items[index - 1] = (String)listDictionaries.Items[index];
                listDictionaries.SetItemChecked( index - 1, listDictionaries.GetItemChecked( index ) );
                listDictionaries.Items[index] = upper_name;
                listDictionaries.SetItemChecked( index, upper_enabled );
                listDictionaries.SetSelected( index - 1, true );
            }
        }

        private void btnDown_Click( object sender, EventArgs e ) {
            int index = listDictionaries.SelectedIndex;
            if ( index + 1 < listDictionaries.Items.Count ) {
                listDictionaries.ClearSelected();
                String lower_name = (String)listDictionaries.Items[index + 1];
                boolean lower_enabled = listDictionaries.CheckedIndices.Contains( index + 1 );
                listDictionaries.Items[index + 1] = (String)listDictionaries.Items[index];
                listDictionaries.SetItemChecked( index + 1, listDictionaries.GetItemChecked( index ) );
                listDictionaries.Items[index] = lower_name;
                listDictionaries.SetItemChecked( index, lower_enabled );
                listDictionaries.SetSelected( index + 1, true );
            }
        }

        private void registerEventHandlers() {
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            this.btnUp.Click += new System.EventHandler( this.btnUp_Click );
            this.btnDown.Click += new System.EventHandler( this.btnDown_Click );
            this.Load += new System.EventHandler( this.FormWordDictionary_Load );
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
            this.listDictionaries = new System.Windows.Forms.CheckedListBox();
            this.lblAvailableDictionaries = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listDictionaries
            // 
            this.listDictionaries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listDictionaries.FormattingEnabled = true;
            this.listDictionaries.HorizontalScrollbar = true;
            this.listDictionaries.Items.AddRange( new object[] {
            "DEFAULT_JP"} );
            this.listDictionaries.Location = new System.Drawing.Point( 12, 33 );
            this.listDictionaries.Name = "listDictionaries";
            this.listDictionaries.Size = new System.Drawing.Size( 248, 186 );
            this.listDictionaries.TabIndex = 0;
            // 
            // lblAvailableDictionaries
            // 
            this.lblAvailableDictionaries.AutoSize = true;
            this.lblAvailableDictionaries.Location = new System.Drawing.Point( 12, 13 );
            this.lblAvailableDictionaries.Name = "lblAvailableDictionaries";
            this.lblAvailableDictionaries.Size = new System.Drawing.Size( 117, 12 );
            this.lblAvailableDictionaries.TabIndex = 1;
            this.lblAvailableDictionaries.Text = "Available Dictionaries";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 91, 277 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 185, 277 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point( 142, 229 );
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size( 56, 23 );
            this.btnUp.TabIndex = 5;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point( 204, 229 );
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size( 56, 23 );
            this.btnDown.TabIndex = 6;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // FormWordDictionary
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 272, 315 );
            this.Controls.Add( this.btnDown );
            this.Controls.Add( this.btnUp );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.lblAvailableDictionaries );
            this.Controls.Add( this.listDictionaries );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWordDictionary";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "User Dictionary Configuration";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox listDictionaries;
        private System.Windows.Forms.Label lblAvailableDictionaries;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        #endregion
#endif
    }

}
