/*
 * EditTool.cs
 * Copyright (c) 2008-2009 kbinani
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
#if JAVA
package org.kbinani.cadencii;
#else
namespace org.kbinani.cadencii {
#endif

    public enum EditTool {
        ARROW,
        PENCIL,
        LINE,
        ERASER,
#if ENABLE_SCRIPT
        PALETTE_TOOL,
#endif
    }

#if !JAVA
}
#endif
