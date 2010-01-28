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

namespace org.kbinani.cadencii{
#endif

    public class Program {
        static FormSplash splash = null;
        static Thread splashThread = null;

#if JAVA
        public static void main( String[] args ){
            AppManager.init();
            AppManager.mainWindow = new FormMain();
            AppManager.mainWindow.setVisible( true );
        }
#else
        delegate void VoidDelegate();

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
#if !DEBUG
            try {
#endif
            splashThread = new Thread( new ThreadStart( showSplash ) );
            splashThread.TrySetApartmentState( ApartmentState.STA );
            splashThread.Start();

            AppManager.init();
            AppManager.mainWindow = new FormMain();
            AppManager.mainWindow.Load += mainWindow_Load;
            Application.Run( AppManager.mainWindow );
#if !DEBUG
            } catch ( Exception ex ) {
                org.kbinani.debug.push_log( ex.ToString() );
            }
#endif
        }

        static void showSplash() {
            splash = new FormSplash();
            splash.showDialog();
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
