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

import java.io.*;
import java.net.*;
import org.kbinani.apputil.*;
import org.kbinani.*;

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

#if JAVA
            public class ExceptionNotifyFormController extends ControllerBase implements ExceptionNotifyFormUiListener
#else
            public class ExceptionNotifyFormController : ControllerBase, ExceptionNotifyFormUiListener
#endif
            {
                protected ExceptionNotifyFormUi ui;
                protected string exceptionMessage = "";

                public ExceptionNotifyFormController()
                {
                    ui = (ExceptionNotifyFormUi)new ExceptionNotifyFormUiImpl( this );
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
#if DEBUG
					sout.println( "ExceptionNotifyFormController::sendButtonClick" );
#endif
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
                    try{
                        URL urlObj = new URL( url );
                        URLConnection connection = urlObj.openConnection();
                        connection.setDoOutput( true );
                        OutputStream stream = connection.getOutputStream();
                        String postData = "message=" + URLEncoder.encode( this.exceptionMessage, "UTF-8" );
                        PrintStream printStream = new PrintStream( stream );
                        printStream.print( postData );
                        printStream.close();
                        
                        InputStream inputStream = connection.getInputStream();
                        BufferedReader reader = new BufferedReader( new InputStreamReader( inputStream ) );
                        String str = "";
                        String s;
                        while( (s = reader.readLine()) != null ){
                        	str += s;
                        }
                        reader.close();
#if DEBUG
                        sout.println( "ExceptionNotifyFormController::sendButtonClick; str=" + str );
#endif
                    }catch( Exception ex ){
                    	ex.printStackTrace();
                    }
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
#if JAVA
                    String str = "[exception-" + count + "] " + ex.getMessage() + "\r\n";
                    StringWriter stream = new StringWriter();
                    ex.printStackTrace( new PrintWriter( stream ) );
                    str += stream.toString() + "\r\n";
                    Throwable t = ex.getCause();
                    if ( t != null && t instanceof Exception ) {
                        str += extractMessageString( (Exception)t, ++count );
                    }
#else
                    string str = "[exception-" + count + "] " + ex.Message + "\r\n";
                    str += ex.StackTrace + "\r\n";
                    if ( ex.InnerException != null ) {
                        str += extractMessageString( ex.InnerException, ++count );
                    }
#endif
                    return str;
                }

                /// <summary>
                /// システムの情報を取得する
                /// </summary>
                /// <returns></returns>
                protected string getSystemInfo()
                {
#if JAVA
					return "OSVersion=" + System.getProperty("os.name") + "\njavaVersion=" + System.getProperty("java.version");
#else
                    return "OSVersion=" + Environment.OSVersion.ToString() + "\ndotNetVersion=" + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
#endif
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
