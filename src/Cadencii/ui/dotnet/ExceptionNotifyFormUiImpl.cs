/*
 * ExceptionNotifyFormUiImpl.cs
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

namespace cadencii
{
    public partial class ExceptionNotifyFormUiImpl : Form, ExceptionNotifyFormUi
    {
        protected ExceptionNotifyFormUiListener uiListener;

        public ExceptionNotifyFormUiImpl(ExceptionNotifyFormUiListener listener)
        {
            InitializeComponent();
            this.uiListener = listener;
        }

        #region ExceptionNotifyFormUiの実装

        public int showDialog(object parent_form)
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

        public void setTitle(string value)
        {
            this.Text = value;
        }

        public void setSendButtonText(string value)
        {
            buttonSend.Text = value;
        }

        public void setCancelButtonText(string value)
        {
            buttonCancel.Text = value;
        }

        public void setExceptionMessage(string value)
        {
            textMessage.Text = value;
        }

        public void setDescription(string value)
        {
            labelDescription.Text = value;
        }

        public void close()
        {
            this.Close();
        }

        #endregion

        private void buttonSend_Click(object sender, EventArgs e)
        {
            uiListener.sendButtonClick();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            uiListener.cancelButtonClick();
        }

    }
}
