/*
 * ValuePairOfStringArrayOfKeys.cs
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
#if JAVA
package org.kbinani.cadencii;

import org.kbinani.windows.forms.*;
#else
using System;
using bocoree.windows.forms;

namespace org.kbinani.cadencii {
#endif

    public class ValuePairOfStringArrayOfKeys {
        public String Key;
        public BKeys[] Value;

        public ValuePairOfStringArrayOfKeys() {
        }

        public ValuePairOfStringArrayOfKeys( String key, BKeys[] value ) {
            Key = key;
            Value = value;
        }
    }

#if !JAVA
}
#endif
