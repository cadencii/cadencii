/*
 * ValuePair.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii
{

    public class ValuePair<K, V> : IComparable<ValuePair<K, V>> where K : IComparable
    {
        private K m_key;
        private V m_value;

        public ValuePair()
        {
        }

        public ValuePair(K key_, V value_)
        {
            m_key = key_;
            m_value = value_;
        }

        public int compareTo(ValuePair<K, V> item)
        {
            return m_key.CompareTo(item.m_key);
        }

        public int CompareTo(ValuePair<K, V> item)
        {
            return compareTo(item);
        }

        public K getKey()
        {
            return m_key;
        }

        public void setKey(K value)
        {
            m_key = value;
        }

        public V getValue()
        {
            return m_value;
        }

        public void setValue(V v)
        {
            m_value = v;
        }

        public K Key
        {
            get
            {
                return getKey();
            }
            set
            {
                setKey(value);
            }
        }

        public V Value
        {
            get
            {
                return getValue();
            }
            set
            {
                setValue(value);
            }
        }
    }

}
