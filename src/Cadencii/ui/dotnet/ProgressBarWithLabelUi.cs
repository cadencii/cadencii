/*
 * ProgressBarWithLabelUi.cs
 * Copyright © 2011 kbinani
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace cadencii
{

    public class ProgressBarWithLabelUi : UserControl
    {
        private ProgressBar progressBar1;
        private Label label1;
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ProgressBarWithLabelUi()
        {
            InitializeComponent();
        }

        public void setWidth(int value)
        {
            this.Width = value;
        }

        public void setText(string value)
        {
            label1.Text = value;
        }

        public string getText()
        {
            return label1.Text;
        }

        public void setProgress(int value)
        {
            if (value < progressBar1.Minimum) value = progressBar1.Minimum;
            if (progressBar1.Maximum < value) value = progressBar1.Maximum;
            progressBar1.Value = value;
        }

        public int getProgress()
        {
            return progressBar1.Value;
        }

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

        #region コンポーネント デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // progressBar1
            //
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(16, 11);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(290, 10);
            this.progressBar1.TabIndex = 0;
            //
            // label1
            //
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(14, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 12);
            this.label1.TabIndex = 1;
            //
            // ProgressBarWithLabelUi
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "ProgressBarWithLabelUi";
            this.Size = new System.Drawing.Size(322, 45);
            this.ResumeLayout(false);

        }

        #endregion
    }

}
