/*
 * FormWorkerUi.cs
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using cadencii.windows.forms;
using cadencii.apputil;



namespace cadencii
{
    public class FormWorkerUi : Form
    {
        private ProgressBar progressBar1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private IFormWorkerControl mControl;
        private Button buttonCancel;
        private Button buttonDetail;
        private bool mDetailVisible = true;
        private int mFullHeight = 1;

        private delegate void DelegateArgIntReturnVoid(int i0);
        private delegate void DelegateArgStringReturnVoid(string s0);

        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public FormWorkerUi(IFormWorkerControl control)
        {
            InitializeComponent();
            mControl = control;
            mFullHeight = this.Height;
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        public void applyLanguage()
        {
            buttonCancel.Text = _("Cancel");
            buttonDetail.Text = _("detail");
        }

        public void show(Object obj)
        {
            if (obj != null && obj is IWin32Window) {
                Show((IWin32Window)obj);
            } else {
                Show();
            }
        }

        /// <summary>
        /// フォームを閉じます．
        /// valueがtrueのときダイアログの結果をCancelに，それ以外の場合はOKとなるようにします．
        /// </summary>
        /// <param name="value"></param>
        public void close(bool value)
        {
            if (value) {
                this.DialogResult = DialogResult.Cancel;
            } else {
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        /// <summary>
        /// フォームを閉じます
        /// </summary>
        public void close()
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// 全体の進捗状況の表示を更新します．
        /// </summary>
        /// <param name="percentage"></param>
        public void setTotalProgress(int percentage)
        {
            if (this.InvokeRequired) {
                try {
                    DelegateArgIntReturnVoid deleg = new DelegateArgIntReturnVoid(setTotalProgressUnsafe);
                    this.Invoke(deleg, percentage);
                } catch (Exception ex) {
                    serr.println(typeof(FormWorkerUi) + ".setTotalProgress; ex=" + ex);
                }
            } else {
                setTotalProgressUnsafe(percentage);
            }
        }

        /// <summary>
        /// 追加されたプログレスバーをこのフォームから削除します
        /// </summary>
        /// <param name="ui"></param>
        public void removeProgressBar(ProgressBarWithLabelUi ui)
        {
            flowLayoutPanel1.Controls.Remove(ui);
        }

        /// <summary>
        /// プログレスバーをこのフォームに追加します．
        /// </summary>
        /// <param name="ui"></param>
        public void addProgressBar(ProgressBarWithLabelUi ui)
        {
            int draft_width = flowLayoutPanel1.Width - 10 - SystemInformation.VerticalScrollBarWidth;
            if (draft_width < 1) {
                draft_width = 1;
            }
            ui.Width = draft_width;
            //ui.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            //ui.Dock = DockStyle.Top;
            flowLayoutPanel1.Controls.Add(ui);
        }

        /// <summary>
        /// フォームのタイトルを設定します
        /// </summary>
        /// <param name="p"></param>
        public void setTitle(string p)
        {
            if (this.InvokeRequired) {
                try {
                    DelegateArgStringReturnVoid deleg = new DelegateArgStringReturnVoid(setTitleUnsafe);
                    this.Invoke(deleg, p);
                } catch (Exception ex) {
                    serr.println(typeof(FormWorkerUi) + ".setTitle; ex=" + ex);
                }
            } else {
                setTitleUnsafe(p);
            }
        }

        /// <summary>
        /// フォームのメッセージテキストを設定します
        /// </summary>
        /// <param name="p"></param>
        public void setText(string p)
        {
            if (this.InvokeRequired) {
                try {
                    DelegateArgStringReturnVoid deleg = new DelegateArgStringReturnVoid(setTextUnsafe);
                    this.Invoke(deleg, p);
                } catch (Exception ex) {
                    serr.println(typeof(FormWorkerUi) + ".setText; ex=" + ex);
                }
            } else {
                setTextUnsafe(p);
            }
        }

        private void setTitleUnsafe(string value)
        {
            this.Text = value;
        }

        private void setTextUnsafe(string value)
        {
            label1.Text = value;
        }

        private void setTotalProgressUnsafe(int percentage)
        {
            if (percentage < progressBar1.Minimum) percentage = progressBar1.Minimum;
            if (progressBar1.Maximum < percentage) percentage = progressBar1.Maximum;
            progressBar1.Value = percentage;
        }

        /// <summary>
        /// このフォームを指定したウィンドウに対してモーダルに表示します．
        /// フォームがキャンセルされた場合true，そうでない場合はfalseを返します
        /// </summary>
        /// <param name="main_window"></param>
        /// <returns></returns>
        public bool showDialogTo(FormMain main_window)
        {
            if (ShowDialog(main_window) == DialogResult.Cancel) {
                return true;
            } else {
                return false;
            }
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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDetail = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // progressBar1
            //
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 40);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(400, 23);
            this.progressBar1.TabIndex = 1;
            //
            // flowLayoutPanel1
            //
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 109);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(400, 126);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.WrapContents = false;
            //
            // label1
            //
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 28);
            this.label1.TabIndex = 3;
            //
            // buttonCancel
            //
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(332, 69);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            //
            // buttonDetail
            //
            this.buttonDetail.Location = new System.Drawing.Point(12, 69);
            this.buttonDetail.Name = "buttonDetail";
            this.buttonDetail.Size = new System.Drawing.Size(80, 23);
            this.buttonDetail.TabIndex = 5;
            this.buttonDetail.Text = "detail";
            this.buttonDetail.UseVisualStyleBackColor = true;
            this.buttonDetail.Click += new System.EventHandler(this.buttonDetail_Click);
            //
            // FormWorkerUi
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(424, 247);
            this.Controls.Add(this.buttonDetail);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.progressBar1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWorkerUi";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormWorker";
            this.SizeChanged += new System.EventHandler(this.FormWorkerUi_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWorkerUi_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            mControl.cancelJobSlot();
        }

        private void FormWorkerUi_FormClosing(object sender, FormClosingEventArgs e)
        {
            mControl.cancelJobSlot();
        }

        private void FormWorkerUi_SizeChanged(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Visible) {
                mFullHeight = this.Height;
                int draft_width = flowLayoutPanel1.Width - 10 - SystemInformation.VerticalScrollBarWidth;
                if (draft_width < 1) {
                    draft_width = 1;
                }
                foreach (Control c in flowLayoutPanel1.Controls) {
                    c.Width = draft_width;
                }
            }
        }

        private void buttonDetail_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = !flowLayoutPanel1.Visible;
            if (flowLayoutPanel1.Visible) {
                this.Height = mFullHeight;
            } else {
                int w = this.ClientSize.Width;
                int delta = flowLayoutPanel1.Top - buttonCancel.Bottom;
                int h = buttonCancel.Bottom + delta - 2;
                this.ClientSize = new Size(w, h);
            }
        }
    }
}
