/*
 * ExceptionNotifyFormController.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package org.kbinani.cadencii;

import org.kbinani.apputil.*;

#else

namespace org
{
    namespace kbinani
    {
        namespace cadencii
        {
#if CSHARP
            using System;
            using org.kbinani.apputil;
#endif

#endif

            public class ExceptionNotifyFormController : ControllerBase, ExceptionNotifyFormUiListener
            {
                protected ExceptionNotifyFormUi ui;
                protected string exceptionMessage = "";

                public ExceptionNotifyFormController()
                {
                    ui = new ExceptionNotifyFormUiImpl( this );
                    this.applyLanguage();
                }

                #region パブリックメソッド

                public void setReportTarget( Exception ex )
                {
                    int count = 0;
                    string message = "";
                    message += "[version]" + Utility.getVersion() + "\r\n";
                    message += "[system]" + this.getSystemInfo() + "\r\n";
                    message += this.extractMessageString( ex, count );
                    this.exceptionMessage = message;
                    this.ui.setExceptionMessage( this.exceptionMessage );
                }

                public ExceptionNotifyFormUi getUi()
                {
                    return this.ui;
                }

                #endregion


                #region ExceptionNotifyFormUiListenerの実装

                public void sendButtonClick()
                {
                    string url = "http://www.kbinani.info/cadenciiProblemReport.php";
#if CSHARP
                    try {
                        System.Text.Encoding enc = System.Text.Encoding.GetEncoding( "UTF-8" );
                        string postData = "message=" + System.Web.HttpUtility.UrlEncode( this.exceptionMessage, enc );
                        System.Net.WebClient client = new System.Net.WebClient();
                        client.Encoding = enc;
                        client.Headers.Add( "Content-Type", "application/x-www-form-urlencoded" );
                        client.UploadString( url, postData );
                    } catch ( Exception ex ) {
                    }
#elif JAVA
#endif
                    
                }

                #endregion


                /// <summary>
                /// 例外からその情報を再帰的に取り出す
                /// </summary>
                /// <param name="ex"></param>
                /// <returns></returns>
                protected string extractMessageString( Exception ex, int count )
                {
                    string str = "[exception-" + count + "] " + ex.Message + "\r\n";
                    str += ex.StackTrace + "\r\n";
                    if ( ex.InnerException != null ) {
                        str += extractMessageString( ex.InnerException, ++count );
                    }
                    return str;
                }

                /// <summary>
                /// システムの情報を取得する
                /// </summary>
                /// <returns></returns>
                protected string getSystemInfo()
                {
                    return Environment.OSVersion.ToString();
                }

                protected void applyLanguage()
                {
                    this.ui.setTitle( _( "Problem Report for Cadencii" ) );
                    this.ui.setDescription( _( "Problem Details" ) );
                    this.ui.setCancelButtonText( _( "Cancel" ) );
                    this.ui.setSendButtonText( _( "Send to Developper" ) );
                }

                protected string _( string id )
                {
                    return Messaging.getMessage( id );
                }
            }

#if !JAVA
        }
    }
}
#endif
