/*
 * FileDialog.Designer.cs
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
    partial class FileDialog {
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
            this.lblLocaion = new System.Windows.Forms.Label();
            this.comboLocation = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnView = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnSmallIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLargeIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.btnList = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.chkDesktop = new System.Windows.Forms.CheckBox();
            this.chkPersonal = new System.Windows.Forms.CheckBox();
            this.chkMyComputer = new System.Windows.Forms.CheckBox();
            this.listFiles = new System.Windows.Forms.ListView();
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblFileType = new System.Windows.Forms.Label();
            this.comboFileName = new System.Windows.Forms.ComboBox();
            this.comboFileType = new System.Windows.Forms.ComboBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLocaion
            // 
            this.lblLocaion.Location = new System.Drawing.Point( 12, 13 );
            this.lblLocaion.Name = "lblLocaion";
            this.lblLocaion.Size = new System.Drawing.Size( 95, 18 );
            this.lblLocaion.TabIndex = 0;
            this.lblLocaion.Text = "Look in(&I):";
            this.lblLocaion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboLocation
            // 
            this.comboLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboLocation.FormattingEnabled = true;
            this.comboLocation.Location = new System.Drawing.Point( 112, 13 );
            this.comboLocation.Name = "comboLocation";
            this.comboLocation.Size = new System.Drawing.Size( 255, 20 );
            this.comboLocation.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.btnPrev,
            this.btnUp,
            this.btnNew,
            this.btnView} );
            this.toolStrip1.Location = new System.Drawing.Point( 412, 9 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 101, 25 );
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPrev
            // 
            this.btnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrev.Image = global::Boare.Cadencii.Properties.Resources.arrow_180;
            this.btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size( 23, 22 );
            this.btnPrev.Text = "toolStripButton1";
            this.btnPrev.Click += new System.EventHandler( this.btnPrev_Click );
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUp.Image = global::Boare.Cadencii.Properties.Resources.arrow_skip_090;
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size( 23, 22 );
            this.btnUp.Text = "toolStripButton2";
            this.btnUp.Click += new System.EventHandler( this.btnUp_Click );
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = global::Boare.Cadencii.Properties.Resources.folder__plus;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size( 23, 22 );
            this.btnNew.Text = "toolStripButton3";
            // 
            // btnView
            // 
            this.btnView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnView.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.btnSmallIcon,
            this.btnTiles,
            this.btnLargeIcon,
            this.btnList,
            this.btnDetails} );
            this.btnView.Image = global::Boare.Cadencii.Properties.Resources.edit_list_order;
            this.btnView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size( 29, 22 );
            this.btnView.Text = "toolStripDropDownButton1";
            // 
            // btnSmallIcon
            // 
            this.btnSmallIcon.Name = "btnSmallIcon";
            this.btnSmallIcon.Size = new System.Drawing.Size( 123, 22 );
            this.btnSmallIcon.Text = "Small Icon";
            // 
            // btnTiles
            // 
            this.btnTiles.Name = "btnTiles";
            this.btnTiles.Size = new System.Drawing.Size( 123, 22 );
            this.btnTiles.Text = "Tiles";
            // 
            // btnLargeIcon
            // 
            this.btnLargeIcon.Name = "btnLargeIcon";
            this.btnLargeIcon.Size = new System.Drawing.Size( 123, 22 );
            this.btnLargeIcon.Text = "Large Icon";
            // 
            // btnList
            // 
            this.btnList.Checked = true;
            this.btnList.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size( 123, 22 );
            this.btnList.Text = "List";
            // 
            // btnDetails
            // 
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size( 123, 22 );
            this.btnDetails.Text = "Details";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.flowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel.Controls.Add( this.chkDesktop );
            this.flowLayoutPanel.Controls.Add( this.chkPersonal );
            this.flowLayoutPanel.Controls.Add( this.chkMyComputer );
            this.flowLayoutPanel.Location = new System.Drawing.Point( 11, 40 );
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding( 0 );
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size( 95, 292 );
            this.flowLayoutPanel.TabIndex = 6;
            // 
            // chkDesktop
            // 
            this.chkDesktop.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkDesktop.ForeColor = System.Drawing.Color.White;
            this.chkDesktop.Location = new System.Drawing.Point( 0, 0 );
            this.chkDesktop.Margin = new System.Windows.Forms.Padding( 0 );
            this.chkDesktop.Name = "chkDesktop";
            this.chkDesktop.Size = new System.Drawing.Size( 90, 45 );
            this.chkDesktop.TabIndex = 11;
            this.chkDesktop.Text = "Desktop";
            this.chkDesktop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDesktop.UseVisualStyleBackColor = true;
            this.chkDesktop.Click += new System.EventHandler( this.radioDesktop_Click );
            // 
            // chkPersonal
            // 
            this.chkPersonal.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPersonal.ForeColor = System.Drawing.Color.White;
            this.chkPersonal.Location = new System.Drawing.Point( 0, 45 );
            this.chkPersonal.Margin = new System.Windows.Forms.Padding( 0 );
            this.chkPersonal.Name = "chkPersonal";
            this.chkPersonal.Size = new System.Drawing.Size( 90, 45 );
            this.chkPersonal.TabIndex = 12;
            this.chkPersonal.Text = "Personal";
            this.chkPersonal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPersonal.UseVisualStyleBackColor = true;
            this.chkPersonal.Click += new System.EventHandler( this.radioPersonal_Click );
            // 
            // chkMyComputer
            // 
            this.chkMyComputer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMyComputer.ForeColor = System.Drawing.Color.White;
            this.chkMyComputer.Location = new System.Drawing.Point( 0, 90 );
            this.chkMyComputer.Margin = new System.Windows.Forms.Padding( 0 );
            this.chkMyComputer.Name = "chkMyComputer";
            this.chkMyComputer.Size = new System.Drawing.Size( 90, 45 );
            this.chkMyComputer.TabIndex = 13;
            this.chkMyComputer.Text = "My Computer";
            this.chkMyComputer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMyComputer.UseVisualStyleBackColor = true;
            this.chkMyComputer.Click += new System.EventHandler( this.radioMyComputer_Click );
            // 
            // listFiles
            // 
            this.listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listFiles.Location = new System.Drawing.Point( 112, 40 );
            this.listFiles.MultiSelect = false;
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size( 476, 237 );
            this.listFiles.TabIndex = 7;
            this.listFiles.UseCompatibleStateImageBehavior = false;
            this.listFiles.View = System.Windows.Forms.View.List;
            this.listFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.listFiles_MouseDoubleClick );
            this.listFiles.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.listFiles_PreviewKeyDown );
            this.listFiles.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler( this.listFiles_ItemSelectionChanged );
            // 
            // lblFileName
            // 
            this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point( 115, 289 );
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size( 73, 12 );
            this.lblFileName.TabIndex = 8;
            this.lblFileName.Text = "File name(&N):";
            // 
            // lblFileType
            // 
            this.lblFileType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFileType.AutoSize = true;
            this.lblFileType.Location = new System.Drawing.Point( 115, 315 );
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size( 87, 12 );
            this.lblFileType.TabIndex = 9;
            this.lblFileType.Text = "Files of type(&T):";
            // 
            // comboFileName
            // 
            this.comboFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFileName.FormattingEnabled = true;
            this.comboFileName.Location = new System.Drawing.Point( 206, 286 );
            this.comboFileName.Name = "comboFileName";
            this.comboFileName.Size = new System.Drawing.Size( 256, 20 );
            this.comboFileName.TabIndex = 10;
            // 
            // comboFileType
            // 
            this.comboFileType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFileType.FormattingEnabled = true;
            this.comboFileType.Location = new System.Drawing.Point( 206, 312 );
            this.comboFileType.Name = "comboFileType";
            this.comboFileType.Size = new System.Drawing.Size( 256, 20 );
            this.comboFileType.TabIndex = 11;
            this.comboFileType.SelectedIndexChanged += new System.EventHandler( this.comboFileType_SelectedIndexChanged );
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.Location = new System.Drawing.Point( 495, 284 );
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size( 93, 23 );
            this.btnAccept.TabIndex = 12;
            this.btnAccept.Text = "Open(&O)";
            this.btnAccept.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 495, 310 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 93, 23 );
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FileDialog
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 600, 343 );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnAccept );
            this.Controls.Add( this.comboFileType );
            this.Controls.Add( this.comboFileName );
            this.Controls.Add( this.lblFileType );
            this.Controls.Add( this.lblFileName );
            this.Controls.Add( this.listFiles );
            this.Controls.Add( this.flowLayoutPanel );
            this.Controls.Add( this.toolStrip1 );
            this.Controls.Add( this.comboLocation );
            this.Controls.Add( this.lblLocaion );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FileDialog";
            this.Load += new System.EventHandler( this.FileDialog_Load );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.flowLayoutPanel.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLocaion;
        private System.Windows.Forms.ComboBox comboLocation;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPrev;
        private System.Windows.Forms.ToolStripButton btnUp;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripDropDownButton btnView;
        private System.Windows.Forms.ToolStripMenuItem btnSmallIcon;
        private System.Windows.Forms.ToolStripMenuItem btnTiles;
        private System.Windows.Forms.ToolStripMenuItem btnLargeIcon;
        private System.Windows.Forms.ToolStripMenuItem btnList;
        private System.Windows.Forms.ToolStripMenuItem btnDetails;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.ListView listFiles;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblFileType;
        private System.Windows.Forms.ComboBox comboFileName;
        private System.Windows.Forms.ComboBox comboFileType;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkDesktop;
        private System.Windows.Forms.CheckBox chkPersonal;
        private System.Windows.Forms.CheckBox chkMyComputer;
    }
}