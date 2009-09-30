/*
 * KeyValuePair.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.corlib.
 *
 * com.boare.corlib is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.corlib is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.corlib;

public class KeyValuePair<K, V>{
    public K key;
    public V value;
    
    public KeyValuePair( K key_, V value_ ){
        key = key_;
        value = value_;
    }
}
