#if !JAVA
/*
 * TreeMap.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
//#define DICTIONARY_TEST
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace bocoree {

    [Serializable]
#if DICTIONARY_TEST
    public class TreeMap<K, V> {
        public V remove( Object key ) {
            return default( V );
        }

        public Vector<K> keySet() {
            return null;
        }

        public V get( K key ) {
            return default( V );
        }

        public V put( K key, V value ) {
            return default( V );
        }

        public bool containsKey( Object key ) {
            return false;
        }

        public void clear() {
        }

        public int size() {
            return 0;
        }
    }
#else
    public class TreeMap<K, V> : Dictionary<K, V> {
        public TreeMap()
            : base() {
        }

        protected TreeMap( SerializationInfo info, StreamingContext context )
            : base( info, context ) {
        }

        public V remove( Object key ) {
            K k = (K)key;
            if ( base.ContainsKey( k ) ) {
                V old = base[k];
                base.Remove( k );
                return old;
            } else {
                base.Remove( k );
                return default( V );
            }
        }

        public Vector<K> keySet() {
            return new Vector<K>( base.Keys );
        }

        public V get( K key ){
            return base[key];
        }

        public int size(){
            return base.Count;
        }

        public bool containsKey( Object key ) {
            return base.ContainsKey( (K)key );
        }

        public void clear() {
            base.Clear();
        }

        public V put( K key, V value ) {
            if ( base.ContainsKey( key ) ){
                V old = base[key];
                base[key] = value;
                return old;
            } else {
                base.Add( key, value );
                return default( V );
            }
        }
    }
#endif

}
#endif
