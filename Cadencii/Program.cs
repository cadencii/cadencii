/*
 * Program.cs
 * Copyright (C) 2009 kbinani
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
using System.Windows.Forms;

namespace org.kbinani.cadencii{
#endif

    public class Program {
#if JAVA
        public static void main( String[] args ){
            AppManager.init();
            AppManager.mainWindow = new FormMain();
            AppManager.mainWindow.setVisible( true );
        }
#else
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
#if !DEBUG
            try {
#endif
                AppManager.init();
                AppManager.mainWindow = new FormMain();
                Application.Run( AppManager.mainWindow );
#if !DEBUG
            } catch ( Exception ex ) {
                org.kbinani.debug.push_log( ex.ToString() );
            }
#endif
        }
#endif
    }

#if !JAVA
}
#endif