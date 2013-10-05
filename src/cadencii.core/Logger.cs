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
using System;
using System.IO;

namespace cadencii
{

    public class Logger
    {
        private static StreamWriter log = null;
        private static string path = "";
        private static bool is_enabled = false;

        private Logger()
        {
        }

        public static bool isEnabled()
        {
            return is_enabled;
        }

        public static void setEnabled(bool value)
        {
            is_enabled = value;
        }

        public static void write(string s)
        {
            if (!is_enabled) {
                return;
            }

            if (log == null) {
                if (path == null || (path != null && path.Equals(""))) {
                    path = PortUtil.createTempFile();
                    //path = "C:\\log.txt";
                }
                try {
                    log = new StreamWriter(path);
                    log.AutoFlush = true;
                } catch (Exception ex) {
                    serr.println("Logger#write; ex=" + ex);
                }
            }

            if (log == null) {
                return;
            }
            try {
                log.Write(s);
            } catch (Exception ex) {
                serr.println("Logger#write; ex=" + ex);
            }
        }

        public static void writeLine(string s)
        {
            write(s + "\n");
        }

        public static string getPath()
        {
            return path;
        }

        public static void setPath(string file)
        {
            bool append = false;
            if (log != null && !path.Equals(file)) {
                try {
                    log.Close();
                } catch (Exception ex) {
                    serr.println("Logger#setPath; ex=" + ex);
                }
                log = null;
                if (File.Exists(file)) {
                    try {
                        PortUtil.deleteFile(file);
                    } catch (Exception ex) {
                        serr.println("Logger#setPath; ex=" + ex);
                    }
                }
                try {
                    PortUtil.moveFile(path, file);
                } catch (Exception ex) {
                    serr.println("Logger#setPath; ex=" + ex);
                }
                append = true;
            }
            path = file;

            if (is_enabled) {
                try {
                    log = new StreamWriter(path, append);
                    log.AutoFlush = true;
                } catch (Exception ex) {
                    serr.println("Logger#setPath; ex=" + ex);
                }
            }
        }
    }

}
