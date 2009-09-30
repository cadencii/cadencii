namespace Boare.PitchEdit {
    partial class FormMain {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( bool disposing ) {
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.pictMain = new System.Windows.Forms.PictureBox();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.vScroll = new System.Windows.Forms.VScrollBar();
            this.btnZoomX = new System.Windows.Forms.Button();
            this.btnMoozX = new System.Windows.Forms.Button();
            this.btnMoozY = new System.Windows.Forms.Button();
            this.btnZoomY = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictMain)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit} );
            this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size( 631, 24 );
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size( 51, 20 );
            this.menuFile.Text = "File(&F)";
            // 
            // menuEdit
            // 
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size( 52, 20 );
            this.menuEdit.Text = "Edit(&E)";
            // 
            // toolStrip
            // 
            this.toolStrip.Location = new System.Drawing.Point( 0, 24 );
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size( 631, 25 );
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // pictMain
            // 
            this.pictMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictMain.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))) );
            this.pictMain.Location = new System.Drawing.Point( 0, 51 );
            this.pictMain.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictMain.Name = "pictMain";
            this.pictMain.Size = new System.Drawing.Size( 614, 236 );
            this.pictMain.TabIndex = 2;
            this.pictMain.TabStop = false;
            this.pictMain.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictMain_MouseMove );
            this.pictMain.Paint += new System.Windows.Forms.PaintEventHandler( this.pictMain_Paint );
            this.pictMain.SizeChanged += new System.EventHandler( this.pictMain_SizeChanged );
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.Location = new System.Drawing.Point( 0, 288 );
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size( 538, 17 );
            this.hScroll.TabIndex = 3;
            // 
            // vScroll
            // 
            this.vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScroll.Location = new System.Drawing.Point( 614, 51 );
            this.vScroll.Maximum = 12700;
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size( 17, 158 );
            this.vScroll.TabIndex = 4;
            this.vScroll.ValueChanged += new System.EventHandler( this.vScroll_ValueChanged );
            // 
            // btnZoomX
            // 
            this.btnZoomX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomX.Location = new System.Drawing.Point( 576, 288 );
            this.btnZoomX.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnZoomX.Name = "btnZoomX";
            this.btnZoomX.Size = new System.Drawing.Size( 38, 17 );
            this.btnZoomX.TabIndex = 5;
            this.btnZoomX.Text = "+";
            this.btnZoomX.UseVisualStyleBackColor = true;
            this.btnZoomX.Click += new System.EventHandler( this.btnZoomX_Click );
            // 
            // btnMoozX
            // 
            this.btnMoozX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoozX.Location = new System.Drawing.Point( 538, 288 );
            this.btnMoozX.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnMoozX.Name = "btnMoozX";
            this.btnMoozX.Size = new System.Drawing.Size( 38, 17 );
            this.btnMoozX.TabIndex = 6;
            this.btnMoozX.Text = "-";
            this.btnMoozX.UseVisualStyleBackColor = true;
            this.btnMoozX.Click += new System.EventHandler( this.btnMoozX_Click );
            // 
            // btnMoozY
            // 
            this.btnMoozY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoozY.Location = new System.Drawing.Point( 614, 247 );
            this.btnMoozY.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnMoozY.Name = "btnMoozY";
            this.btnMoozY.Size = new System.Drawing.Size( 17, 38 );
            this.btnMoozY.TabIndex = 7;
            this.btnMoozY.Text = "-";
            this.btnMoozY.UseVisualStyleBackColor = true;
            this.btnMoozY.Click += new System.EventHandler( this.btnMoozY_Click );
            // 
            // btnZoomY
            // 
            this.btnZoomY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomY.Location = new System.Drawing.Point( 614, 209 );
            this.btnZoomY.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnZoomY.Name = "btnZoomY";
            this.btnZoomY.Size = new System.Drawing.Size( 17, 38 );
            this.btnZoomY.TabIndex = 8;
            this.btnZoomY.Text = "+";
            this.btnZoomY.UseVisualStyleBackColor = true;
            this.btnZoomY.Click += new System.EventHandler( this.btnZoomY_Click );
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 631, 306 );
            this.Controls.Add( this.btnZoomY );
            this.Controls.Add( this.btnMoozY );
            this.Controls.Add( this.btnMoozX );
            this.Controls.Add( this.btnZoomX );
            this.Controls.Add( this.vScroll );
            this.Controls.Add( this.hScroll );
            this.Controls.Add( this.pictMain );
            this.Controls.Add( this.toolStrip );
            this.Controls.Add( this.menuStrip1 );
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormMain";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.FormMain_FormClosed );
            this.menuStrip1.ResumeLayout( false );
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictMain)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.PictureBox pictMain;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.Windows.Forms.VScrollBar vScroll;
        private System.Windows.Forms.Button btnZoomX;
        private System.Windows.Forms.Button btnMoozX;
        private System.Windows.Forms.Button btnMoozY;
        private System.Windows.Forms.Button btnZoomY;
    }
}