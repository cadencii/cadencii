namespace cadencii.vconnect
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openUtauDbDialog = new System.Windows.Forms.OpenFileDialog();
            this.textSourceDb = new System.Windows.Forms.TextBox();
            this.groupSourceDb = new System.Windows.Forms.GroupBox();
            this.groupDestinationDb = new System.Windows.Forms.GroupBox();
            this.buttonSelectSourceDb = new System.Windows.Forms.Button();
            this.buttonSelectDestinationDb = new System.Windows.Forms.Button();
            this.textDestinationDb = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.saveVConnectDbDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.textLineBuffer = new System.Windows.Forms.TextBox();
            this.groupSourceDb.SuspendLayout();
            this.groupDestinationDb.SuspendLayout();
            this.SuspendLayout();
            // 
            // openUtauDbDialog
            // 
            this.openUtauDbDialog.Title = "Select oto.ini";
            // 
            // textSourceDb
            // 
            this.textSourceDb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSourceDb.Location = new System.Drawing.Point(19, 29);
            this.textSourceDb.Name = "textSourceDb";
            this.textSourceDb.Size = new System.Drawing.Size(247, 19);
            this.textSourceDb.TabIndex = 0;
            // 
            // groupSourceDb
            // 
            this.groupSourceDb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSourceDb.Controls.Add(this.buttonSelectSourceDb);
            this.groupSourceDb.Controls.Add(this.textSourceDb);
            this.groupSourceDb.Location = new System.Drawing.Point(12, 12);
            this.groupSourceDb.Name = "groupSourceDb";
            this.groupSourceDb.Size = new System.Drawing.Size(366, 79);
            this.groupSourceDb.TabIndex = 1;
            this.groupSourceDb.TabStop = false;
            this.groupSourceDb.Text = "Select source UTAU database";
            // 
            // groupDestinationDb
            // 
            this.groupDestinationDb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupDestinationDb.Controls.Add(this.buttonSelectDestinationDb);
            this.groupDestinationDb.Controls.Add(this.textDestinationDb);
            this.groupDestinationDb.Location = new System.Drawing.Point(12, 97);
            this.groupDestinationDb.Name = "groupDestinationDb";
            this.groupDestinationDb.Size = new System.Drawing.Size(366, 79);
            this.groupDestinationDb.TabIndex = 2;
            this.groupDestinationDb.TabStop = false;
            this.groupDestinationDb.Text = "Select destination directory";
            // 
            // buttonSelectSourceDb
            // 
            this.buttonSelectSourceDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectSourceDb.Location = new System.Drawing.Point(272, 27);
            this.buttonSelectSourceDb.Name = "buttonSelectSourceDb";
            this.buttonSelectSourceDb.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectSourceDb.TabIndex = 1;
            this.buttonSelectSourceDb.Text = "Select";
            this.buttonSelectSourceDb.UseVisualStyleBackColor = true;
            this.buttonSelectSourceDb.Click += new System.EventHandler(this.buttonSelectSourceDb_Click);
            // 
            // buttonSelectDestinationDb
            // 
            this.buttonSelectDestinationDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectDestinationDb.Location = new System.Drawing.Point(272, 26);
            this.buttonSelectDestinationDb.Name = "buttonSelectDestinationDb";
            this.buttonSelectDestinationDb.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectDestinationDb.TabIndex = 3;
            this.buttonSelectDestinationDb.Text = "Select";
            this.buttonSelectDestinationDb.UseVisualStyleBackColor = true;
            this.buttonSelectDestinationDb.Click += new System.EventHandler(this.buttonSelectDestinationDb_Click);
            // 
            // textDestinationDb
            // 
            this.textDestinationDb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDestinationDb.Location = new System.Drawing.Point(19, 28);
            this.textDestinationDb.Name = "textDestinationDb";
            this.textDestinationDb.Size = new System.Drawing.Size(247, 19);
            this.textDestinationDb.TabIndex = 2;
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Enabled = false;
            this.buttonStart.Location = new System.Drawing.Point(258, 192);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(101, 23);
            this.buttonStart.TabIndex = 3;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBox1
            // 
            this.textLineBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLineBuffer.BackColor = System.Drawing.Color.DimGray;
            this.textLineBuffer.ForeColor = System.Drawing.Color.White;
            this.textLineBuffer.Location = new System.Drawing.Point(12, 231);
            this.textLineBuffer.Multiline = true;
            this.textLineBuffer.Name = "textBox1";
            this.textLineBuffer.ReadOnly = true;
            this.textLineBuffer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textLineBuffer.Size = new System.Drawing.Size(366, 191);
            this.textLineBuffer.TabIndex = 4;
            this.textLineBuffer.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 434);
            this.Controls.Add(this.textLineBuffer);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.groupDestinationDb);
            this.Controls.Add(this.groupSourceDb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "UTAU DB Converter for vConnect-STAND";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupSourceDb.ResumeLayout(false);
            this.groupSourceDb.PerformLayout();
            this.groupDestinationDb.ResumeLayout(false);
            this.groupDestinationDb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openUtauDbDialog;
        private System.Windows.Forms.TextBox textSourceDb;
        private System.Windows.Forms.GroupBox groupSourceDb;
        private System.Windows.Forms.GroupBox groupDestinationDb;
        private System.Windows.Forms.Button buttonSelectSourceDb;
        private System.Windows.Forms.Button buttonSelectDestinationDb;
        private System.Windows.Forms.TextBox textDestinationDb;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.FolderBrowserDialog saveVConnectDbDialog;
        private System.Windows.Forms.TextBox textLineBuffer;
    }
}

