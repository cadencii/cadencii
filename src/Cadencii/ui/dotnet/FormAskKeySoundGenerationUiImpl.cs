/*
 * FormAskKeySoundGenerationUiImpl.cs
 * Copyright © 2010-2011 kbinani
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



namespace cadencii
{

    public class FormAskKeySoundGenerationUiImpl : Form, FormAskKeySoundGenerationUi
    {
        private FormAskKeySoundGenerationUiListener mListener;

        public FormAskKeySoundGenerationUiImpl(FormAskKeySoundGenerationUiListener controller)
        {
            InitializeComponent();
            mListener = controller;
            registerEventHandlers();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }


        #region FormAskKeySoundGenerationUiインターフェースの実装

        /// <summary>
        /// メッセージの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        public void setMessageLabelText(string value)
        {
            lblMessage.Text = value;
        }

        public void setAlwaysPerformThisCheckCheckboxText(string value)
        {
            chkAlwaysPerformThisCheck.Text = value;
        }

        public void setYesButtonText(string value)
        {
            btnYes.Text = value;
        }

        public void setNoButtonText(string value)
        {
            btnNo.Text = value;
        }

        public void setAlwaysPerformThisCheck(bool value)
        {
            chkAlwaysPerformThisCheck.Checked = value;
        }

        public bool isAlwaysPerformThisCheck()
        {
            return chkAlwaysPerformThisCheck.Checked;
        }

        #endregion



        #region UiBaseインターフェースの実装

        public int showDialog(Object parent_form)
        {
            DialogResult ret;
            if (parent_form == null || (parent_form != null && !(parent_form is Form))) {
                ret = base.ShowDialog();
            } else {
                Form form = (Form)parent_form;
                ret = base.ShowDialog(form);
            }
            if (ret == DialogResult.OK || ret == DialogResult.Yes) {
                return 1;
            } else {
                return 0;
            }
        }

        #endregion



        #region public methods
        /// <summary>
        /// フォームを閉じます．
        /// valueがtrueのときダイアログの結果をCancelに，それ以外の場合はOKとなるようにします．
        /// </summary>
        public void close(bool value)
        {
            if (value) {
                this.DialogResult = DialogResult.Cancel;
            } else {
                this.DialogResult = DialogResult.OK;
            }
        }
        #endregion



        #region helper methods

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            btnYes.Click += new EventHandler(btnYes_Click);
            btnNo.Click += new EventHandler(btnNo_Click);
        }

        #endregion



        #region event handlers

        private void btnYes_Click(Object sender, EventArgs e)
        {
            mListener.buttonOkClickedSlot();
        }

        private void btnNo_Click(Object sender, EventArgs e)
        {
            mListener.buttonCancelClickedSlot();
        }
        #endregion



        #region UI implementation

        private void InitializeComponent()
        {
            this.btnNo = new Button();
            this.btnYes = new Button();
            this.chkAlwaysPerformThisCheck = new CheckBox();
            this.lblMessage = new Label();
            this.SuspendLayout();
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.Location = new System.Drawing.Point(183, 112);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 5;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnYes.Location = new System.Drawing.Point(63, 112);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 4;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysPerformThisCheck
            // 
            this.chkAlwaysPerformThisCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysPerformThisCheck.AutoSize = true;
            this.chkAlwaysPerformThisCheck.Checked = true;
            this.chkAlwaysPerformThisCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlwaysPerformThisCheck.Location = new System.Drawing.Point(14, 77);
            this.chkAlwaysPerformThisCheck.Name = "chkAlwaysPerformThisCheck";
            this.chkAlwaysPerformThisCheck.Size = new System.Drawing.Size(284, 16);
            this.chkAlwaysPerformThisCheck.TabIndex = 6;
            this.chkAlwaysPerformThisCheck.Text = "Always perform this check when starting Cadencii.";
            this.chkAlwaysPerformThisCheck.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 21);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(302, 53);
            this.lblMessage.TabIndex = 7;
            this.lblMessage.Text = "It seems some key-board sounds are missing. Do you want to re-generate them now?";
            // 
            // FormAskKeySoundGeneration
            // 
            this.CancelButton = this.btnNo;
            this.ClientSize = new System.Drawing.Size(326, 147);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.chkAlwaysPerformThisCheck);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAskKeySoundGeneration";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button btnNo;
        private CheckBox chkAlwaysPerformThisCheck;
        private Label lblMessage;
        private Button btnYes;

        #endregion
    }


}
