/*
 * Program.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Windows.Forms;

namespace Boare.Cadencii{

    public class Program {
        [STAThread]
        static void Main() {
#if DEBUG
            //test.run();
#endif
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
                bocoree.debug.push_log( ex.ToString() );
            }
#endif
        }
    }

}
