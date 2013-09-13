/*
 * ValuePairOfStringArrayOfKeys.cs
 * Copyright © 2009-2011 kbinani
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
package cadencii;

import cadencii.windows.forms.*;
#else
using System;
using cadencii.windows.forms;

namespace cadencii {
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
