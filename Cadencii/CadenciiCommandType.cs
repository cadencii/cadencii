/*
 * CadenciiCommandType.cs
 * Copyright (c) 2008-2009 kbinani
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
namespace org.kbinani.cadencii {
#endif

    public enum CadenciiCommandType {
        VSQ_COMMAND,
        BEZIER_CHAIN_ADD,
        BEZIER_CHAIN_DELETE,
        BEZIER_CHAIN_REPLACE,
        REPLACE,
        ATTACHED_CURVE_REPLACE_RANGE,
        TRACK_ADD,
        TRACK_DELETE,
        TRACK_EDIT_CURVE,
        TRACK_REPLACE,
        BGM_UPDATE,
    }

#if !JAVA
}
#endif
