/*
 * AppManager.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.editotoini.
 *
 * org.kbinani.editotoini is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.editotoini is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.editotoini;

import java.awt.*;
import java.io.*;
import org.kbinani.*;
import org.kbinani.xml.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.io;
using org.kbinani.xml;

namespace org.kbinani.editotoini {
#endif

    public class AppManager {
        private const String CONFIG_DIR_NAME = "Cadencii";
        private const String CONFIG_FILE_NAME = "config.xml";

        public static EditorConfig cadenciiConfig = null;
        private static Font baseFont = null;

        public static void loadConfig() {
            String config_file = fsys.combine( getApplicationDataPath(), CONFIG_FILE_NAME );
            if ( !PortUtil.isFileExists( config_file ) ) {
                cadenciiConfig = new EditorConfig();
                return;
            }
            XmlSerializer xs = null;
            FileInputStream ifs = null;
            try {
                xs = new XmlSerializer( typeof( EditorConfig ) );
                ifs = new FileInputStream( config_file );
                cadenciiConfig = (EditorConfig)xs.deserialize( ifs );
            } catch ( Exception ex ) {
                serr.println( "AppManager#loadConfig; ex=" + ex );
                cadenciiConfig = null;
            } finally {
                if ( ifs != null ) {
                    try {
                        ifs.close();
                    } catch ( Exception ex2 ) {
                        serr.println( "AppManager#loadConfig; ex2=" + ex2 );
                    }
                }
            }
            if ( cadenciiConfig == null ) {
                cadenciiConfig = new EditorConfig();
            }
        }

        /// <summary>
        /// アプリケーションデータの保存位置を取得します
        /// Gets the path for application data
        /// </summary>
        public static String getApplicationDataPath() {
#if JAVA
            String dir = fsys.combine( System.getenv( "APPDATA" ), "Boare" );
#else
            String dir = fsys.combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Boare" );
#endif
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                PortUtil.createDirectory( dir );
            }
            String dir2 = fsys.combine( dir, CONFIG_DIR_NAME );
            if ( !PortUtil.isDirectoryExists( dir2 ) ) {
                PortUtil.createDirectory( dir2 );
            }
            return dir2;
        }

        public static Font getBaseFont() {
            if ( cadenciiConfig == null ) {
                loadConfig();
            }
            if ( baseFont == null ) {
                baseFont = new Font( cadenciiConfig.BaseFontName, Font.PLAIN, (int)cadenciiConfig.BaseFontSize );
            }
            return baseFont;
        }

        public static String getPathResampler() {
            if ( cadenciiConfig == null ) {
                loadConfig();
            }
            return cadenciiConfig.PathResampler;
        }

        public static String getLanguage() {
            if ( cadenciiConfig == null ) {
                loadConfig();
            }
            String lang = cadenciiConfig.Language;
            if ( lang == null ) {
                lang = "ja";
            }
            return lang;
        }
    }

#if !JAVA
}
#endif
