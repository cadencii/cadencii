/*
 * KeyValueType.cs
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
using System;

namespace Boare.Cadencii {

    public class ValuePair<K, V> : IComparable<ValuePair<K, V>> where K : IComparable {
        private K m_key;
        private V m_value;

        public ValuePair() {
        }

        public ValuePair( K key, V value ) {
            m_key = key;
            m_value = value;
        }

        public int CompareTo( ValuePair<K, V> item ) {
            return m_key.CompareTo( item.m_key );
        }

        public K Key {
            get {
                return m_key;
            }
            set {
                m_key = value;
            }
        }

        public V Value {
            get {
                return m_value;
            }
            set {
                m_value = value;
            }
        }
    }

}
