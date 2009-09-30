/*
 * PropertyPanelContainer.Designer.cs
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
    partial class PropertyPanelContainer {
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.panelMain = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnWindow = new System.Windows.Forms.Button();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Location = new System.Drawing.Point( 0, 29 );
            this.panelMain.Margin = new System.Windows.Forms.Padding( 0 );
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size( 159, 283 );
            this.panelMain.TabIndex = 0;
            this.panelMain.SizeChanged += new System.EventHandler( this.panelMain_SizeChanged );
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Image = global::Boare.Cadencii.Properties.Resources.cross_small;
            this.btnClose.Location = new System.Drawing.Point( 133, 3 );
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size( 23, 23 );
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler( this.btnClose_Click );
            // 
            // btnWindow
            // 
            this.btnWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindow.Image = global::Boare.Cadencii.Properties.Resources.chevron_small_collapse;
            this.btnWindow.Location = new System.Drawing.Point( 104, 3 );
            this.btnWindow.Name = "btnWindow";
            this.btnWindow.Size = new System.Drawing.Size( 23, 23 );
            this.btnWindow.TabIndex = 2;
            this.btnWindow.UseVisualStyleBackColor = true;
            this.btnWindow.Click += new System.EventHandler( this.btnWindow_Click );
            // 
            // panelTitle
            // 
            this.panelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTitle.Controls.Add( this.btnWindow );
            this.panelTitle.Controls.Add( this.btnClose );
            this.panelTitle.Location = new System.Drawing.Point( 0, 0 );
            this.panelTitle.Margin = new System.Windows.Forms.Padding( 0 );
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size( 159, 29 );
            this.panelTitle.TabIndex = 3;
            this.panelTitle.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.panelTitle_MouseDoubleClick );
            // 
            // PropertyPanelContainer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add( this.panelTitle );
            this.Controls.Add( this.panelMain );
            this.Name = "PropertyPanelContainer";
            this.Size = new System.Drawing.Size( 159, 312 );
            this.panelTitle.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnWindow;
        private System.Windows.Forms.Panel panelTitle;
    }
}
