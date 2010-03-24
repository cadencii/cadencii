/*
 * Program.cs
 * Copyright (C) 2009-2010 kbinani
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

#else
using System;
using System.Threading;
using System.Windows.Forms;
using org.kbinani.apputil;

namespace org.kbinani.cadencii{
#endif

    public class Program {
        static FormSplash splash = null;
        static Thread splashThread = null;

#if JAVA
        public static void main( String[] args ){
            AppManager.init();
            AppManager.mainWindow = new FormMain( "" );
            AppManager.mainWindow.setVisible( true );
        }
#else
        delegate void VoidDelegate();

        [STAThread]
        public static void Main( String[] args ) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
#if !DEBUG
            try {
#endif

#if !MONO
            splashThread = new Thread( new ThreadStart( showSplash ) );
            splashThread.TrySetApartmentState( ApartmentState.STA );
            splashThread.Start();
#endif

            String file = "";
            if ( args.Length > 0 ) {
                file = args[0];
            }
            AppManager.init();
#if ENABLE_SCRIPT
            try {
                ScriptServer.reload();
                PaletteToolServer.init();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "Program#Main; ex=" + ex );
            }
#endif
            AppManager.mainWindow = new FormMain( file );
#if !MONO
            AppManager.mainWindow.Load += mainWindow_Load;
#endif
            Application.Run( AppManager.mainWindow );
#if !DEBUG
            } catch ( Exception ex ) {
                String str_ex = getExceptionText( ex, 0 );
                FormCompileResult dialog = new FormCompileResult(
                    _( "Failed to launch Cadencii. Please send the exception report to developer" ),
                    str_ex );
                dialog.setTitle( _( "Error" ) );
                dialog.showDialog();
                if ( splash != null ) {
                    VoidDelegate splash_close = new VoidDelegate( splash.close );
                    if ( splash != null ) {
                        splash.Invoke( splash_close );
                    }
                }
            }
#endif
        }

        /// <summary>
        /// 内部例外を含めた例外テキストを再帰的に取得します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="depth_count"></param>
        /// <returns></returns>
        private static String getExceptionText( Exception ex, int depth_count ) {
            String ret = ex.ToString();
            if ( ex.InnerException != null ) {
                ret += "\n" +
                       "-- InnerException; Depth Level " + depth_count + " -----------------------" +
                       getExceptionText( ex.InnerException, depth_count + 1 );
            }
            return ret;
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        static void showSplash() {
            splash = new FormSplash();
            splash.setModal( true );
            splash.setVisible( true );
        }

        static void closeSplash() {
            splash.close();
        }

        public static void mainWindow_Load( Object sender, EventArgs e ) {
            if ( splash != null ) {
                VoidDelegate deleg = new VoidDelegate( closeSplash );
                if ( deleg != null ) {
                    splash.Invoke( deleg );
                }
            }
            splash = null;
            AppManager.mainWindow.Load -= mainWindow_Load;
        }
#endif
    }

#if !JAVA
}
#endif
