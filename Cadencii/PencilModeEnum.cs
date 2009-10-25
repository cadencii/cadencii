/*
 * PencilModeEnum.cs
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
package org.kbinani.Cadencii;
#else
namespace Boare.Cadencii {
#endif

    public enum PencilModeEnum {
        Off,
        L1,
        L2,
        L4,
        L8,
        L16,
        L32,
        L64,
        L128,
    }

#if !JAVA
}
#endif
