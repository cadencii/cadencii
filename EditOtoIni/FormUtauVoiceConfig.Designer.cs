/*
 * FormUtauVoiceConfig.Designer.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.EditOtoIni.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using Boare.Cadencii;

namespace Boare.EditOtoIni {

    using boolean = System.Boolean;
    
    partial class FormUtauVoiceConfig {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormUtauVoiceConfig ) );
            this.listFiles = new System.Windows.Forms.ListView();
            this.columnHeaderFilename = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAlias = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderOffset = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderConsonant = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderBlank = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPreUtterance = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderOverlap = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderFrq = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStf = new System.Windows.Forms.ColumnHeader();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditGenerateFRQ = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditGenerateSTF = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSearchNext = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSearchPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this.panelRight = new System.Windows.Forms.Panel();
            this.btnRefreshFrq = new System.Windows.Forms.Button();
            this.btnRefreshStf = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOverlap = new Boare.Cadencii.NumberTextBox();
            this.lblOverlap = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPreUtterance = new Boare.Cadencii.NumberTextBox();
            this.lblPreUtterance = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBlank = new Boare.Cadencii.NumberTextBox();
            this.lblBlank = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConsonant = new Boare.Cadencii.NumberTextBox();
            this.lblConsonant = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOffset = new Boare.Cadencii.NumberTextBox();
            this.lblOffset = new System.Windows.Forms.Label();
            this.txtAlias = new System.Windows.Forms.TextBox();
            this.lblAlias = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.pictWave = new Boare.Cadencii.BPictureBox();
            this.bgWorkRead = new System.ComponentModel.BackgroundWorker();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLblTootip = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerIn = new Boare.Lib.AppUtil.BSplitContainer();
            this.splitContainerOut = new Boare.Lib.AppUtil.BSplitContainer();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cmenuListFiles = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.generateSTRAIGHTFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.bgWorkScreen = new System.ComponentModel.BackgroundWorker();
            this.menuStrip.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictWave)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.cmenuListFiles.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // listFiles
            // 
            this.listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listFiles.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFilename,
            this.columnHeaderAlias,
            this.columnHeaderOffset,
            this.columnHeaderConsonant,
            this.columnHeaderBlank,
            this.columnHeaderPreUtterance,
            this.columnHeaderOverlap,
            this.columnHeaderFrq,
            this.columnHeaderStf} );
            this.listFiles.FullRowSelect = true;
            this.listFiles.HideSelection = false;
            this.listFiles.Location = new System.Drawing.Point( 0, 0 );
            this.listFiles.MultiSelect = false;
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size( 454, 242 );
            this.listFiles.TabIndex = 0;
            this.listFiles.UseCompatibleStateImageBehavior = false;
            this.listFiles.View = System.Windows.Forms.View.Details;
            this.listFiles.SelectedIndexChanged += new System.EventHandler( this.listFiles_SelectedIndexChanged );
            // 
            // columnHeaderFilename
            // 
            this.columnHeaderFilename.Text = "File Name";
            this.columnHeaderFilename.Width = 75;
            // 
            // columnHeaderAlias
            // 
            this.columnHeaderAlias.Text = "Alias";
            this.columnHeaderAlias.Width = 42;
            // 
            // columnHeaderOffset
            // 
            this.columnHeaderOffset.Text = "Offset";
            this.columnHeaderOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderOffset.Width = 50;
            // 
            // columnHeaderConsonant
            // 
            this.columnHeaderConsonant.Text = "Consonant";
            this.columnHeaderConsonant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderConsonant.Width = 72;
            // 
            // columnHeaderBlank
            // 
            this.columnHeaderBlank.Text = "Blank";
            this.columnHeaderBlank.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderBlank.Width = 51;
            // 
            // columnHeaderPreUtterance
            // 
            this.columnHeaderPreUtterance.Text = "Pre Utterance";
            this.columnHeaderPreUtterance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderPreUtterance.Width = 92;
            // 
            // columnHeaderOverlap
            // 
            this.columnHeaderOverlap.Text = "Overlap";
            this.columnHeaderOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderOverlap.Width = 61;
            // 
            // columnHeaderFrq
            // 
            this.columnHeaderFrq.Text = "FRQ";
            this.columnHeaderFrq.Width = 51;
            // 
            // columnHeaderStf
            // 
            this.columnHeaderStf.Text = "STF";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuView} );
            this.menuStrip.Location = new System.Drawing.Point( 0, 0 );
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size( 772, 24 );
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileSaveAs,
            this.toolStripMenuItem1,
            this.menuFileQuit} );
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size( 51, 20 );
            this.menuFile.Text = "File(&F)";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size( 129, 22 );
            this.menuFileOpen.Text = "Open(&O)";
            this.menuFileOpen.MouseEnter += new System.EventHandler( this.menuFileOpen_MouseEnter );
            this.menuFileOpen.Click += new System.EventHandler( this.menuFileOpen_Click );
            // 
            // menuFileSave
            // 
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.Size = new System.Drawing.Size( 129, 22 );
            this.menuFileSave.Text = "Save(&S)";
            this.menuFileSave.MouseEnter += new System.EventHandler( this.menuFileSave_MouseEnter );
            this.menuFileSave.Click += new System.EventHandler( this.menuFileSave_Click );
            // 
            // menuFileSaveAs
            // 
            this.menuFileSaveAs.Name = "menuFileSaveAs";
            this.menuFileSaveAs.Size = new System.Drawing.Size( 129, 22 );
            this.menuFileSaveAs.Text = "Save As(&A)";
            this.menuFileSaveAs.MouseEnter += new System.EventHandler( this.menuFileSaveAs_MouseEnter );
            this.menuFileSaveAs.Click += new System.EventHandler( this.menuFileSaveAs_Click );
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size( 126, 6 );
            // 
            // menuFileQuit
            // 
            this.menuFileQuit.Name = "menuFileQuit";
            this.menuFileQuit.Size = new System.Drawing.Size( 129, 22 );
            this.menuFileQuit.Text = "Close(&C)";
            this.menuFileQuit.MouseEnter += new System.EventHandler( this.menuFileQuit_MouseEnter );
            this.menuFileQuit.Click += new System.EventHandler( this.menuFileQuit_Click );
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuEditGenerateFRQ,
            this.menuEditGenerateSTF} );
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size( 52, 20 );
            this.menuEdit.Text = "Edit(&E)";
            // 
            // menuEditGenerateFRQ
            // 
            this.menuEditGenerateFRQ.Name = "menuEditGenerateFRQ";
            this.menuEditGenerateFRQ.Size = new System.Drawing.Size( 169, 22 );
            this.menuEditGenerateFRQ.Text = "Generate FRQ files";
            this.menuEditGenerateFRQ.Click += new System.EventHandler( this.menuEditGenerateFRQ_Click );
            // 
            // menuEditGenerateSTF
            // 
            this.menuEditGenerateSTF.Name = "menuEditGenerateSTF";
            this.menuEditGenerateSTF.Size = new System.Drawing.Size( 169, 22 );
            this.menuEditGenerateSTF.Text = "Generate STF files";
            this.menuEditGenerateSTF.Click += new System.EventHandler( this.menuEditGenerateSTF_Click );
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuViewSearchNext,
            this.menuViewSearchPrevious} );
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size( 58, 20 );
            this.menuView.Text = "View(&V)";
            // 
            // menuViewSearchNext
            // 
            this.menuViewSearchNext.Name = "menuViewSearchNext";
            this.menuViewSearchNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.menuViewSearchNext.Size = new System.Drawing.Size( 216, 22 );
            this.menuViewSearchNext.Text = "Search Next(&N)";
            this.menuViewSearchNext.Click += new System.EventHandler( this.menuViewSearchNext_Click );
            // 
            // menuViewSearchPrevious
            // 
            this.menuViewSearchPrevious.Name = "menuViewSearchPrevious";
            this.menuViewSearchPrevious.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
            this.menuViewSearchPrevious.Size = new System.Drawing.Size( 216, 22 );
            this.menuViewSearchPrevious.Text = "Search Previous(&P)";
            this.menuViewSearchPrevious.Click += new System.EventHandler( this.menuViewSearchPrevious_Click );
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add( this.btnRefreshFrq );
            this.panelRight.Controls.Add( this.btnRefreshStf );
            this.panelRight.Controls.Add( this.label9 );
            this.panelRight.Controls.Add( this.txtOverlap );
            this.panelRight.Controls.Add( this.lblOverlap );
            this.panelRight.Controls.Add( this.label7 );
            this.panelRight.Controls.Add( this.txtPreUtterance );
            this.panelRight.Controls.Add( this.lblPreUtterance );
            this.panelRight.Controls.Add( this.label5 );
            this.panelRight.Controls.Add( this.txtBlank );
            this.panelRight.Controls.Add( this.lblBlank );
            this.panelRight.Controls.Add( this.label3 );
            this.panelRight.Controls.Add( this.txtConsonant );
            this.panelRight.Controls.Add( this.lblConsonant );
            this.panelRight.Controls.Add( this.label2 );
            this.panelRight.Controls.Add( this.txtOffset );
            this.panelRight.Controls.Add( this.lblOffset );
            this.panelRight.Controls.Add( this.txtAlias );
            this.panelRight.Controls.Add( this.lblAlias );
            this.panelRight.Controls.Add( this.txtFileName );
            this.panelRight.Controls.Add( this.lblFileName );
            this.panelRight.Location = new System.Drawing.Point( 457, 30 );
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size( 210, 273 );
            this.panelRight.TabIndex = 2;
            // 
            // btnRefreshFrq
            // 
            this.btnRefreshFrq.Location = new System.Drawing.Point( 95, 189 );
            this.btnRefreshFrq.Name = "btnRefreshFrq";
            this.btnRefreshFrq.Size = new System.Drawing.Size( 100, 23 );
            this.btnRefreshFrq.TabIndex = 21;
            this.btnRefreshFrq.Text = "Refresh FRQ";
            this.btnRefreshFrq.UseVisualStyleBackColor = true;
            this.btnRefreshFrq.Click += new System.EventHandler( this.btnRefreshFrq_Click );
            // 
            // btnRefreshStf
            // 
            this.btnRefreshStf.Location = new System.Drawing.Point( 95, 218 );
            this.btnRefreshStf.Name = "btnRefreshStf";
            this.btnRefreshStf.Size = new System.Drawing.Size( 100, 23 );
            this.btnRefreshStf.TabIndex = 20;
            this.btnRefreshStf.Text = "Refresh STF";
            this.btnRefreshStf.UseVisualStyleBackColor = true;
            this.btnRefreshStf.Click += new System.EventHandler( this.btnRefreshStf_Click );
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point( 160, 167 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 20, 12 );
            this.label9.TabIndex = 19;
            this.label9.Text = "ms";
            // 
            // txtOverlap
            // 
            this.txtOverlap.BackColor = System.Drawing.SystemColors.Window;
            this.txtOverlap.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOverlap.Location = new System.Drawing.Point( 95, 164 );
            this.txtOverlap.Name = "txtOverlap";
            this.txtOverlap.Size = new System.Drawing.Size( 59, 19 );
            this.txtOverlap.TabIndex = 18;
            this.txtOverlap.Text = "0";
            this.txtOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtOverlap.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtOverlap.TextChanged += new System.EventHandler( this.txtOverlap_TextChanged );
            // 
            // lblOverlap
            // 
            this.lblOverlap.AutoSize = true;
            this.lblOverlap.Location = new System.Drawing.Point( 15, 167 );
            this.lblOverlap.Name = "lblOverlap";
            this.lblOverlap.Size = new System.Drawing.Size( 44, 12 );
            this.lblOverlap.TabIndex = 17;
            this.lblOverlap.Text = "Overlap";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 160, 142 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 20, 12 );
            this.label7.TabIndex = 16;
            this.label7.Text = "ms";
            // 
            // txtPreUtterance
            // 
            this.txtPreUtterance.BackColor = System.Drawing.SystemColors.Window;
            this.txtPreUtterance.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtPreUtterance.Location = new System.Drawing.Point( 95, 139 );
            this.txtPreUtterance.Name = "txtPreUtterance";
            this.txtPreUtterance.Size = new System.Drawing.Size( 59, 19 );
            this.txtPreUtterance.TabIndex = 15;
            this.txtPreUtterance.Text = "0";
            this.txtPreUtterance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPreUtterance.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtPreUtterance.TextChanged += new System.EventHandler( this.txtPreUtterance_TextChanged );
            // 
            // lblPreUtterance
            // 
            this.lblPreUtterance.AutoSize = true;
            this.lblPreUtterance.Location = new System.Drawing.Point( 15, 142 );
            this.lblPreUtterance.Name = "lblPreUtterance";
            this.lblPreUtterance.Size = new System.Drawing.Size( 76, 12 );
            this.lblPreUtterance.TabIndex = 14;
            this.lblPreUtterance.Text = "Pre Utterance";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 160, 117 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 20, 12 );
            this.label5.TabIndex = 13;
            this.label5.Text = "ms";
            // 
            // txtBlank
            // 
            this.txtBlank.BackColor = System.Drawing.SystemColors.Window;
            this.txtBlank.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBlank.Location = new System.Drawing.Point( 95, 114 );
            this.txtBlank.Name = "txtBlank";
            this.txtBlank.Size = new System.Drawing.Size( 59, 19 );
            this.txtBlank.TabIndex = 12;
            this.txtBlank.Text = "0";
            this.txtBlank.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBlank.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtBlank.TextChanged += new System.EventHandler( this.txtBlank_TextChanged );
            // 
            // lblBlank
            // 
            this.lblBlank.AutoSize = true;
            this.lblBlank.Location = new System.Drawing.Point( 15, 117 );
            this.lblBlank.Name = "lblBlank";
            this.lblBlank.Size = new System.Drawing.Size( 34, 12 );
            this.lblBlank.TabIndex = 11;
            this.lblBlank.Text = "Blank";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 160, 92 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 20, 12 );
            this.label3.TabIndex = 10;
            this.label3.Text = "ms";
            // 
            // txtConsonant
            // 
            this.txtConsonant.BackColor = System.Drawing.SystemColors.Window;
            this.txtConsonant.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtConsonant.Location = new System.Drawing.Point( 95, 89 );
            this.txtConsonant.Name = "txtConsonant";
            this.txtConsonant.Size = new System.Drawing.Size( 59, 19 );
            this.txtConsonant.TabIndex = 9;
            this.txtConsonant.Text = "0";
            this.txtConsonant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtConsonant.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtConsonant.TextChanged += new System.EventHandler( this.txtConsonant_TextChanged );
            // 
            // lblConsonant
            // 
            this.lblConsonant.AutoSize = true;
            this.lblConsonant.Location = new System.Drawing.Point( 15, 92 );
            this.lblConsonant.Name = "lblConsonant";
            this.lblConsonant.Size = new System.Drawing.Size( 59, 12 );
            this.lblConsonant.TabIndex = 8;
            this.lblConsonant.Text = "Consonant";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 160, 67 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 20, 12 );
            this.label2.TabIndex = 7;
            this.label2.Text = "ms";
            // 
            // txtOffset
            // 
            this.txtOffset.BackColor = System.Drawing.SystemColors.Window;
            this.txtOffset.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOffset.Location = new System.Drawing.Point( 95, 64 );
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size( 59, 19 );
            this.txtOffset.TabIndex = 6;
            this.txtOffset.Text = "0";
            this.txtOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtOffset.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtOffset.TextChanged += new System.EventHandler( this.txtOffset_TextChanged );
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point( 15, 67 );
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size( 37, 12 );
            this.lblOffset.TabIndex = 5;
            this.lblOffset.Text = "Offset";
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point( 95, 39 );
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size( 100, 19 );
            this.txtAlias.TabIndex = 4;
            this.txtAlias.TextChanged += new System.EventHandler( this.txtAlias_TextChanged );
            // 
            // lblAlias
            // 
            this.lblAlias.AutoSize = true;
            this.lblAlias.Location = new System.Drawing.Point( 15, 42 );
            this.lblAlias.Name = "lblAlias";
            this.lblAlias.Size = new System.Drawing.Size( 31, 12 );
            this.lblAlias.TabIndex = 3;
            this.lblAlias.Text = "Alias";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point( 95, 14 );
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size( 100, 19 );
            this.txtFileName.TabIndex = 2;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point( 15, 17 );
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size( 57, 12 );
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "File Name";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add( this.btnMinus );
            this.panelBottom.Controls.Add( this.btnPlus );
            this.panelBottom.Controls.Add( this.hScroll );
            this.panelBottom.Controls.Add( this.pictWave );
            this.panelBottom.Location = new System.Drawing.Point( 0, 306 );
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size( 667, 161 );
            this.panelBottom.TabIndex = 4;
            // 
            // btnMinus
            // 
            this.btnMinus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinus.Location = new System.Drawing.Point( 603, 145 );
            this.btnMinus.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size( 32, 16 );
            this.btnMinus.TabIndex = 4;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler( this.btnMinus_Click );
            // 
            // btnPlus
            // 
            this.btnPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlus.Location = new System.Drawing.Point( 635, 145 );
            this.btnPlus.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size( 32, 16 );
            this.btnPlus.TabIndex = 3;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler( this.btnPlus_Click );
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.Location = new System.Drawing.Point( 0, 145 );
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size( 603, 16 );
            this.hScroll.TabIndex = 2;
            this.hScroll.ValueChanged += new System.EventHandler( this.hScroll_ValueChanged );
            // 
            // pictWave
            // 
            this.pictWave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictWave.Location = new System.Drawing.Point( 0, 0 );
            this.pictWave.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictWave.Name = "pictWave";
            this.pictWave.Size = new System.Drawing.Size( 667, 145 );
            this.pictWave.TabIndex = 1;
            this.pictWave.TabStop = false;
            this.pictWave.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictWave_MouseMove );
            this.pictWave.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictWave_MouseDown );
            this.pictWave.Paint += new System.Windows.Forms.PaintEventHandler( this.pictWave_Paint );
            this.pictWave.MouseUp += new System.Windows.Forms.MouseEventHandler( this.pictWave_MouseUp );
            // 
            // bgWorkRead
            // 
            this.bgWorkRead.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWorkRead_DoWork );
            this.bgWorkRead.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWorkRead_RunWorkerCompleted );
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.statusLblTootip} );
            this.statusStrip.Location = new System.Drawing.Point( 0, 473 );
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size( 772, 22 );
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLblTootip
            // 
            this.statusLblTootip.Name = "statusLblTootip";
            this.statusLblTootip.Size = new System.Drawing.Size( 0, 17 );
            // 
            // splitContainerIn
            // 
            this.splitContainerIn.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitContainerIn.IsSplitterFixed = false;
            this.splitContainerIn.Location = new System.Drawing.Point( 673, 275 );
            this.splitContainerIn.Name = "splitContainerIn";
            this.splitContainerIn.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            this.splitContainerIn.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerIn.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerIn.Panel1.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerIn.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerIn.Panel1.Location = new System.Drawing.Point( 1, 1 );
            this.splitContainerIn.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainerIn.Panel1.Name = "m_panel1";
            this.splitContainerIn.Panel1.Padding = new System.Windows.Forms.Padding( 1 );
            this.splitContainerIn.Panel1.Size = new System.Drawing.Size( 101, 190 );
            this.splitContainerIn.Panel1.TabIndex = 0;
            this.splitContainerIn.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainerIn.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerIn.Panel2.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerIn.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerIn.Panel2.Location = new System.Drawing.Point( 108, 1 );
            this.splitContainerIn.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainerIn.Panel2.Name = "m_panel2";
            this.splitContainerIn.Panel2.Padding = new System.Windows.Forms.Padding( 1 );
            this.splitContainerIn.Panel2.Size = new System.Drawing.Size( 147, 190 );
            this.splitContainerIn.Panel2.TabIndex = 1;
            this.splitContainerIn.Panel2MinSize = 25;
            this.splitContainerIn.Size = new System.Drawing.Size( 256, 192 );
            this.splitContainerIn.SplitterDistance = 103;
            this.splitContainerIn.SplitterWidth = 4;
            this.splitContainerIn.TabIndex = 6;
            this.splitContainerIn.Text = "bSplitContainer1";
            // 
            // splitContainerOut
            // 
            this.splitContainerOut.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitContainerOut.IsSplitterFixed = false;
            this.splitContainerOut.Location = new System.Drawing.Point( 673, 30 );
            this.splitContainerOut.Name = "splitContainerOut";
            this.splitContainerOut.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // 
            // 
            this.splitContainerOut.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerOut.Panel1.BorderColor = System.Drawing.Color.Black;
            this.splitContainerOut.Panel1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainerOut.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainerOut.Panel1.Name = "m_panel1";
            this.splitContainerOut.Panel1.Size = new System.Drawing.Size( 441, 111 );
            this.splitContainerOut.Panel1.TabIndex = 0;
            this.splitContainerOut.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainerOut.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerOut.Panel2.BorderColor = System.Drawing.Color.Black;
            this.splitContainerOut.Panel2.Location = new System.Drawing.Point( 0, 115 );
            this.splitContainerOut.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainerOut.Panel2.Name = "m_panel2";
            this.splitContainerOut.Panel2.Size = new System.Drawing.Size( 441, 105 );
            this.splitContainerOut.Panel2.TabIndex = 1;
            this.splitContainerOut.Panel2MinSize = 25;
            this.splitContainerOut.Size = new System.Drawing.Size( 441, 220 );
            this.splitContainerOut.SplitterDistance = 111;
            this.splitContainerOut.SplitterWidth = 4;
            this.splitContainerOut.TabIndex = 2;
            this.splitContainerOut.Text = "bSplitContainer1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "oto.ini";
            // 
            // cmenuListFiles
            // 
            this.cmenuListFiles.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.generateSTRAIGHTFileToolStripMenuItem} );
            this.cmenuListFiles.Name = "cmenuListFiles";
            this.cmenuListFiles.Size = new System.Drawing.Size( 197, 26 );
            // 
            // generateSTRAIGHTFileToolStripMenuItem
            // 
            this.generateSTRAIGHTFileToolStripMenuItem.Name = "generateSTRAIGHTFileToolStripMenuItem";
            this.generateSTRAIGHTFileToolStripMenuItem.Size = new System.Drawing.Size( 196, 22 );
            this.generateSTRAIGHTFileToolStripMenuItem.Text = "Generate STRAIGHT file";
            // 
            // panelLeft
            // 
            this.panelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLeft.Controls.Add( this.buttonPrevious );
            this.panelLeft.Controls.Add( this.buttonNext );
            this.panelLeft.Controls.Add( this.lblSearch );
            this.panelLeft.Controls.Add( this.txtSearch );
            this.panelLeft.Controls.Add( this.listFiles );
            this.panelLeft.Location = new System.Drawing.Point( 0, 30 );
            this.panelLeft.Margin = new System.Windows.Forms.Padding( 0 );
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size( 454, 273 );
            this.panelLeft.TabIndex = 7;
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrevious.AutoSize = true;
            this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonPrevious.Image = global::EditOtoIni.Properties.Resources.arrow_090;
            this.buttonPrevious.Location = new System.Drawing.Point( 227, 245 );
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size( 75, 25 );
            this.buttonPrevious.TabIndex = 4;
            this.buttonPrevious.Text = "Previous";
            this.buttonPrevious.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler( this.buttonPrevious_Click );
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNext.AutoSize = true;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonNext.Image = global::EditOtoIni.Properties.Resources.arrow_270;
            this.buttonNext.Location = new System.Drawing.Point( 166, 245 );
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size( 55, 25 );
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = "Next";
            this.buttonNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler( this.buttonNext_Click );
            // 
            // lblSearch
            // 
            this.lblSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point( 12, 251 );
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size( 42, 12 );
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSearch.Location = new System.Drawing.Point( 60, 248 );
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size( 100, 19 );
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler( this.txtSearch_TextChanged );
            // 
            // bgWorkScreen
            // 
            this.bgWorkScreen.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWorkScreen_DoWork );
            // 
            // FormUtauVoiceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 772, 495 );
            this.Controls.Add( this.panelLeft );
            this.Controls.Add( this.splitContainerIn );
            this.Controls.Add( this.panelBottom );
            this.Controls.Add( this.splitContainerOut );
            this.Controls.Add( this.panelRight );
            this.Controls.Add( this.menuStrip );
            this.Controls.Add( this.statusStrip );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormUtauVoiceConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Voice DB Config. - Untitled";
            this.Load += new System.EventHandler( this.FormUtauVoiceConfig_Load );
            this.SizeChanged += new System.EventHandler( this.FormUtauVoiceConfig_SizeChanged );
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.FormUtauVoiceConfig_FormClosed );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormUtauVoiceConfig_FormClosing );
            this.menuStrip.ResumeLayout( false );
            this.menuStrip.PerformLayout();
            this.panelRight.ResumeLayout( false );
            this.panelRight.PerformLayout();
            this.panelBottom.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.pictWave)).EndInit();
            this.statusStrip.ResumeLayout( false );
            this.statusStrip.PerformLayout();
            this.cmenuListFiles.ResumeLayout( false );
            this.panelLeft.ResumeLayout( false );
            this.panelLeft.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderFilename;
        private System.Windows.Forms.ColumnHeader columnHeaderAlias;
        private System.Windows.Forms.ColumnHeader columnHeaderOffset;
        private System.Windows.Forms.ColumnHeader columnHeaderConsonant;
        private System.Windows.Forms.ColumnHeader columnHeaderBlank;
        private System.Windows.Forms.ColumnHeader columnHeaderPreUtterance;
        private System.Windows.Forms.ColumnHeader columnHeaderOverlap;
        private System.Windows.Forms.ColumnHeader columnHeaderFrq;
        private BPictureBox pictWave;
        private Boare.Lib.AppUtil.BSplitContainer splitContainerOut;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.ComponentModel.BackgroundWorker bgWorkRead;
        private System.Windows.Forms.StatusStrip statusStrip;
        private Boare.Lib.AppUtil.BSplitContainer splitContainerIn;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtAlias;
        private System.Windows.Forms.Label lblAlias;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label2;
        private NumberTextBox txtOffset;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.Label label9;
        private NumberTextBox txtOverlap;
        private System.Windows.Forms.Label lblOverlap;
        private System.Windows.Forms.Label label7;
        private NumberTextBox txtPreUtterance;
        private System.Windows.Forms.Label lblPreUtterance;
        private System.Windows.Forms.Label label5;
        private NumberTextBox txtBlank;
        private System.Windows.Forms.Label lblBlank;
        private System.Windows.Forms.Label label3;
        private NumberTextBox txtConsonant;
        private System.Windows.Forms.Label lblConsonant;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuFileQuit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem menuFileSaveAs;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel statusLblTootip;
        private System.Windows.Forms.ColumnHeader columnHeaderStf;
        private System.Windows.Forms.ContextMenuStrip cmenuListFiles;
        private System.Windows.Forms.ToolStripMenuItem generateSTRAIGHTFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuEditGenerateSTF;
        private System.Windows.Forms.Button btnRefreshStf;
        private System.Windows.Forms.Button btnRefreshFrq;
        private System.Windows.Forms.ToolStripMenuItem menuEditGenerateFRQ;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrevious;
        private System.ComponentModel.BackgroundWorker bgWorkScreen;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuViewSearchNext;
        private System.Windows.Forms.ToolStripMenuItem menuViewSearchPrevious;
    }
}
