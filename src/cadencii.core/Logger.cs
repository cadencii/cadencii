/*
 * Logger.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package cadencii;

import java.io.BufferedWriter;
import java.io.FileOutputStream;
import java.io.OutputStreamWriter;
import java.io.FileWriter;

#else

using System;
using System.IO;

namespace cadencii {
    using boolean = System.Boolean;
#endif

    public class Logger {
#if JAVA
        private static BufferedWriter log = null;
#else
        private static StreamWriter log = null;
#endif
        private static String path = "";
        private static boolean is_enabled = false;

        private Logger() {
        }

        public static boolean isEnabled() {
            return is_enabled;
        }

        public static void setEnabled( boolean value ) {
            is_enabled = value;
        }

        public static void write( String s ) {
            if ( !is_enabled ) {
                return;
            }

            if ( log == null ) {
                if ( path == null || (path != null && path.Equals( "" )) ) {
                    path = PortUtil.createTempFile();
                    //path = "C:\\log.txt";
                }
                try {
#if JAVA
                    log = new BufferedWriter( new FileWriter( path ) );
#else
                    log = new StreamWriter( path );
                    log.AutoFlush = true;
#endif
                } catch ( Exception ex ) {
                    serr.println( "Logger#write; ex=" + ex );
                }
            }

            if ( log == null ) {
                return;
            }
            try {
#if JAVA
                log.write( s );
                log.flush();
#else
                log.Write( s );
#endif
            } catch ( Exception ex ) {
                serr.println( "Logger#write; ex=" + ex );
            }
        }

        public static void writeLine( String s )
        {
            write( s + "\n" );
        }

        public static String getPath() {
            return path;
        }

        public static void setPath( String file ) {
            boolean append = false;
            if ( log != null && !path.Equals( file ) ) {
                try {
#if JAVA
                    log.close();
#else
                    log.Close();
#endif
                } catch ( Exception ex ) {
                    serr.println( "Logger#setPath; ex=" + ex );
                }
                log = null;
                if( File.Exists( file ) ){
                    try{
                        PortUtil.deleteFile( file );
                    }catch( Exception ex ){
                        serr.println( "Logger#setPath; ex=" + ex );
                    }
                }
                try {
                    PortUtil.moveFile( path, file );
                } catch ( Exception ex ) {
                    serr.println( "Logger#setPath; ex=" + ex );
                }
                append = true;
            }
            path = file;

            if ( is_enabled ) {
                try {
#if JAVA
                log = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( path, append ), "UTF-8" ) );
#else
                    log = new StreamWriter( path, append );
                    log.AutoFlush = true;
#endif
                } catch ( Exception ex ) {
                    serr.println( "Logger#setPath; ex=" + ex );
                }
            }
        }
    }

#if !JAVA
}
#endif
