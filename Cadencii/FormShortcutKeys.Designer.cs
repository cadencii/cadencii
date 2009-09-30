/*
 * FormShortcutKeys.Designer.cs
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
namespace Boare.Cadencii {
    using boolean = System.Boolean;
    partial class FormShortcutKeys {
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup( "File", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup( "Edit", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup( "View", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup( "Job", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup( "Track", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup( "Lyric", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup( "Script", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup( "Setting", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup( "Help", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup( "Other", System.Windows.Forms.HorizontalAlignment.Left );
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.list = new System.Windows.Forms.ListView();
            this.columnCommand = new System.Windows.Forms.ColumnHeader();
            this.columnShortcut = new System.Windows.Forms.ColumnHeader();
            this.btnLoadDefault = new System.Windows.Forms.Button();
            this.btnRevert = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 325, 403 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 244, 403 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // list
            // 
            this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.list.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnCommand,
            this.columnShortcut} );
            this.list.FullRowSelect = true;
            listViewGroup1.Header = "File";
            listViewGroup1.Name = "listGroupFile";
            listViewGroup2.Header = "Edit";
            listViewGroup2.Name = "listGroupEdit";
            listViewGroup3.Header = "View";
            listViewGroup3.Name = "listGroupVisual";
            listViewGroup4.Header = "Job";
            listViewGroup4.Name = "listGroupJob";
            listViewGroup5.Header = "Track";
            listViewGroup5.Name = "listGroupTrack";
            listViewGroup6.Header = "Lyric";
            listViewGroup6.Name = "listGroupLyric";
            listViewGroup7.Header = "Script";
            listViewGroup7.Name = "listGroupScript";
            listViewGroup8.Header = "Setting";
            listViewGroup8.Name = "listGroupSetting";
            listViewGroup9.Header = "Help";
            listViewGroup9.Name = "listGroupHelp";
            listViewGroup10.Header = "Other";
            listViewGroup10.Name = "listGroupOther";
            this.list.Groups.AddRange( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6,
            listViewGroup7,
            listViewGroup8,
            listViewGroup9,
            listViewGroup10} );
            this.list.Location = new System.Drawing.Point( 12, 12 );
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size( 388, 343 );
            this.list.TabIndex = 9;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            this.list.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.list_PreviewKeyDown );
            this.list.KeyDown += new System.Windows.Forms.KeyEventHandler( this.list_KeyDown );
            // 
            // columnCommand
            // 
            this.columnCommand.Text = "Command";
            this.columnCommand.Width = 240;
            // 
            // columnShortcut
            // 
            this.columnShortcut.Text = "Shortcut Key";
            this.columnShortcut.Width = 140;
            // 
            // btnLoadDefault
            // 
            this.btnLoadDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadDefault.Location = new System.Drawing.Point( 113, 361 );
            this.btnLoadDefault.Name = "btnLoadDefault";
            this.btnLoadDefault.Size = new System.Drawing.Size( 95, 23 );
            this.btnLoadDefault.TabIndex = 11;
            this.btnLoadDefault.Text = "Load Default";
            this.btnLoadDefault.UseVisualStyleBackColor = true;
            this.btnLoadDefault.Click += new System.EventHandler( this.btnLoadDefault_Click );
            // 
            // btnRevert
            // 
            this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRevert.Location = new System.Drawing.Point( 12, 361 );
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size( 95, 23 );
            this.btnRevert.TabIndex = 10;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler( this.btnRevert_Click );
            // 
            // FormShortcutKeys
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 412, 438 );
            this.Controls.Add( this.btnLoadDefault );
            this.Controls.Add( this.btnRevert );
            this.Controls.Add( this.list );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormShortcutKeys";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Shortcut Config";
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.ColumnHeader columnCommand;
        private System.Windows.Forms.ColumnHeader columnShortcut;
        private System.Windows.Forms.Button btnLoadDefault;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.ToolTip toolTip;
    }
}