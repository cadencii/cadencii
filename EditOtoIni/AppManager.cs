/*
 * AppManager.cs
 * Copyright (C) 2009-2010 kbinani
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
#else
using System;
using org.kbinani.java.awt;

namespace org.kbinani.editotoini {
#endif

    public class AppManager {
        //public static EditorConfig cadenciiConfig = new EditorConfig();
        private static Font baseFont = null;
        private static String BaseFontName = "MS UI Gothic";
        private static float BaseFontSize = 9.0f;
        private static String pathResampler = "";

        public static Font getBaseFont() {
            return new Font( BaseFontName, Font.PLAIN, (int)BaseFontSize );
        }

        public static String getPathResampler() {
            return pathResampler;
        }
    }

#if !JAVA
}
#endif
