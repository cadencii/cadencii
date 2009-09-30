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
package com.boare.cadencii;

public class ValuePairOfStringArrayOfKeys {
    public String Key;
    public Keys[] Value;

    public ValuePairOfStringArrayOfKeys() {
    }

    public ValuePairOfStringArrayOfKeys( String key, Keys[] value ) {
        Key = key;
        Value = value;
    }
}
