/*
 * AppManager.cs
 * Copyright (C) 2009 kbinani
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

import org.kbinani.cadencii.*;
#else
using org.kbinani.cadencii;

namespace org.kbinani.editotoini {
#endif

    public class AppManager {
        public static EditorConfig cadenciiConfig = new EditorConfig();
    }

#if !JAVA
}
#endif
