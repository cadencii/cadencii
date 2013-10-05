/*
 * FormTrackProperty.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Windows.Forms;
using cadencii.apputil;
using cadencii;
using cadencii.windows.forms;



namespace cadencii
{

    public class FormTrackProperty : Form
    {
        private int m_master_tuning;

        public FormTrackProperty(int master_tuning_in_cent)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            applyLanguage();
            m_master_tuning = master_tuning_in_cent;
            txtMasterTuning.Text = master_tuning_in_cent + "";
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            lblMasterTuning.Text = _("Master Tuning in Cent");
            this.Text = _("Track Property");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
        }

        public int getMasterTuningInCent()
        {
            return m_master_tuning;
        }
        #endregion

        #region helper methods
        private string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            txtMasterTuning.TextChanged += new EventHandler(txtMasterTuning_TextChanged);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void txtMasterTuning_TextChanged(Object sender, EventArgs e)
        {
            int v = m_master_tuning;
            try {
                v = int.Parse(txtMasterTuning.Text);
                m_master_tuning = v;
            } catch (Exception ex) {
            }
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region UI implementation
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.lblMasterTuning = new Label();
            this.txtMasterTuning = new TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(92, 62);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(186, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblMasterTuning
            // 
            this.lblMasterTuning.AutoSize = true;
            this.lblMasterTuning.Location = new System.Drawing.Point(15, 14);
            this.lblMasterTuning.Name = "lblMasterTuning";
            this.lblMasterTuning.Size = new System.Drawing.Size(119, 12);
            this.lblMasterTuning.TabIndex = 28;
            this.lblMasterTuning.Text = "Master Tuning in Cent";
            // 
            // txtMasterTuning
            // 
            this.txtMasterTuning.Location = new System.Drawing.Point(46, 29);
            this.txtMasterTuning.Name = "txtMasterTuning";
            this.txtMasterTuning.Size = new System.Drawing.Size(187, 19);
            this.txtMasterTuning.TabIndex = 29;
            // 
            // FormTrackProperty
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(286, 97);
            this.Controls.Add(this.txtMasterTuning);
            this.Controls.Add(this.lblMasterTuning);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTrackProperty";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Project Property";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnOK;
        private Button btnCancel;
        private Label lblMasterTuning;
        private TextBox txtMasterTuning;
        #endregion
        #endregion

    }

}
