/*
 * StfQueueArgs.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.EditOtoIni.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.EditOtoIni;
#else
using System;

namespace Boare.EditOtoIni {
#endif

    public struct StfQueueArgs {
        public String waveName;
        public String offset;
        public String blank;
    }

#if !JAVA
}
#endif
