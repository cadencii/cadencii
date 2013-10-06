/*
 * ExceptionNotifyFormController.cs
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

namespace cadencii
{
    using System;
    using cadencii.apputil;

    public class ExceptionNotifyFormController : ControllerBase, ExceptionNotifyFormUiListener
    {
        protected ExceptionNotifyFormUi ui;
        protected string exceptionMessage = "";

        public ExceptionNotifyFormController()
        {
            ui = (ExceptionNotifyFormUi)new ExceptionNotifyFormUiImpl(this);
            this.applyLanguage();
        }

        #region パブリックメソッド

        public void setReportTarget(Exception ex)
        {
            int count = 0;
            string message = "";
            message += "[version]\r\n" + Utility.getVersion().Replace("\n\n", "\n") + "\r\n";
            message += "[system]\r\n" + this.getSystemInfo() + "\r\n";
            message += this.extractMessageString(ex, count);
            this.exceptionMessage = message;
            this.ui.setExceptionMessage(this.exceptionMessage);
        }

        public ExceptionNotifyFormUi getUi()
        {
            return this.ui;
        }

        #endregion


        #region ExceptionNotifyFormUiListenerの実装

        public void sendButtonClick()
        {
#if DEBUG
            sout.println("ExceptionNotifyFormController::sendButtonClick");
#endif
            string url = "http://www.cadencii.info/error_report.php";
            try {
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
                string postData = "message=" + System.Web.HttpUtility.UrlEncode(this.exceptionMessage, enc);
                System.Net.WebClient client = new System.Net.WebClient();
                client.Encoding = enc;
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.UploadString(url, postData);
            } catch (Exception ex) {
            }
            ui.close();
        }

        public void cancelButtonClick()
        {
            ui.close();
        }

        #endregion


        /// <summary>
        /// 例外からその情報を再帰的に取り出す
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected string extractMessageString(Exception ex, int count)
        {
            string str = "[exception-" + count + "]\r\n" + ex.Message + "\r\n";
            str += ex.StackTrace + "\r\n";
            if (ex.InnerException != null) {
                str += extractMessageString(ex.InnerException, ++count);
            }
            return str;
        }

        /// <summary>
        /// システムの情報を取得する
        /// </summary>
        /// <returns></returns>
        protected string getSystemInfo()
        {
            return "OSVersion=" + Environment.OSVersion.ToString() + "\ndotNetVersion=" + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
        }

        protected void applyLanguage()
        {
            this.ui.setTitle(_("Problem Report for Cadencii"));
            this.ui.setDescription(_("Problem Details"));
            this.ui.setCancelButtonText(_("Cancel"));
            this.ui.setSendButtonText(_("Send to Developper"));
        }

        protected string _(string id)
        {
            return Messaging.getMessage(id);
        }
    }

}
