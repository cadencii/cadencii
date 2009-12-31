/*
 * ValuePair.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani;
#else
using System;

namespace org.kbinani {
#endif

#if JAVA
    public class ValuePair<K extends Comparable<K>, V> implements Comparable<ValuePair<K, V>>{
#else
    public class ValuePair<K, V> : IComparable<ValuePair<K, V>> where K : IComparable {
#endif
        private K m_key;
        private V m_value;

        public ValuePair() {
        }

        public ValuePair( K key_, V value_ ) {
            m_key = key_;
            m_value = value_;
        }

        public int compareTo( ValuePair<K, V> item ) {
#if JAVA
            return m_key.compareTo( item.m_key );
#else
            return m_key.CompareTo( item.m_key );
#endif
        }

#if !JAVA
        public int CompareTo( ValuePair<K, V> item ){
            return compareTo( item );
        }
#endif

        public K getKey() {
            return m_key;
        }

        public void setKey( K value ) {
            m_key = value;
        }

        public V getValue() {
            return m_value;
        }

        public void setValue( V v ) {
            m_value = v;
        }

#if !JAVA
        public K Key {
            get {
                return getKey();
            }
            set {
                setKey( value );
            }
        }

        public V Value {
            get {
                return getValue();
            }
            set {
                setValue( value );
            }
        }
#endif
    }

#if !JAVA
}
#endif
