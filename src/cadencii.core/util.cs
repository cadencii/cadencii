#if !JAVA
/*
 * util.cs
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
//#define DICTIONARY_TEST
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace cadencii.java.util {
    using boolean = Boolean;

    public interface Entry<K, V> {
    }

    public interface Map<K, V> {
        /// <summary>
        /// マップからマッピングをすべて削除します (任意のオペレーション)。
        /// </summary>
        void clear();
        /// <summary>
        /// マップが指定のキーのマッピングを保持する場合に true を返します。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        boolean containsKey( Object key );
        /// <summary>
        /// マップが 1 つまたは複数のキーと指定された値をマッピングしている場合に true を返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        boolean containsValue( Object value );
        /// <summary>
        /// このマップに含まれるマップの Set ビューを返します。
        /// </summary>
        /// <returns></returns>
        Set<Entry<K, V>> entrySet();
        /// <summary>
        /// 指定されたオブジェクトがこのマップと等しいかどうかを比較します。
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        boolean equals( Object o );
        /// <summary>
        /// 指定されたキーがマップされている値を返します。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        V get( Object key );
        /// <summary>
        /// マップのハッシュコード値を返します。
        /// </summary>
        /// <returns></returns>
        int hashCode();
        /// <summary>
        /// マップがキーと値のマッピングを保持しない場合に true を返します。    }
        /// </summary>
        /// <returns></returns>
        boolean isEmpty();
    }

    public interface Set<E> {
        /// <summary>
        /// 指定された要素がセット内になかった場合、セットに追加します (任意のオペレーション)。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        boolean add( E e );
        /// <summary>
        /// 指定されたコレクションのすべての要素について、その要素がこのセット内にない場合、セットに追加します (任意のオペレーション)。
        /// </summary>
        /// <param name="extends"></param>
        /// <returns></returns>
        boolean addAll( Collection<E> c );
        /// <summary>
        /// セットからすべての要素を削除します (任意のオペレーション)。
        /// </summary>
        void clear();
        /// <summary>
        /// セットが、指定された要素を保持している場合に true を返します。
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        boolean contains( Object o );
        /// <summary>
        /// 指定されたコレクションのすべての要素がセット内にある場合に true を返します。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        boolean containsAll( Collection<E> c );
        /// <summary>
        /// 指定されたオブジェクトがセットと同じかどうかを比較します。
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        boolean equals( Object o );
        /// <summary>
        /// セットのハッシュコード値を返します。
        /// </summary>
        /// <returns></returns>
        int hashCode();
        /// <summary>
        /// セットが要素を 1 つも保持していない場合に true を返します。
        /// </summary>
        /// <returns></returns>
        boolean isEmpty();
        /// <summary>
        /// セット内の各要素についての反復子を返します。
        /// </summary>
        /// <returns></returns>
        Iterator<E> iterator();
        /// <summary>
        /// 指定された要素がセット内にあった場合、セットから削除します (任意のオペレーション)。
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        boolean remove( Object o );
        /// <summary>
        /// このセットから、指定されたコレクションに含まれる要素をすべて削除します (任意のオペレーション)。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        boolean removeAll( Collection<E> c );
        /// <summary>
        /// セット内の要素のうち、指定されたコレクション内にある要素だけを保持します (任意のオペレーション)。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        boolean retainAll( Collection<E> c );
        /// <summary>
        /// セット内の要素数 (そのカーディナリティ) を返します。
        /// </summary>
        /// <returns></returns>
        int size();
        /// <summary>
        /// セット内のすべての要素が格納されている配列を返します。
        /// </summary>
        /// <returns></returns>
        Object[] toArray();
        /// <summary>
        /// セット内のすべての要素を格納している配列を返します。
        /// </summary>
        /// <param name="a"></param>
        T[] toArray<T>( T[] a );
    }

    public interface Collection<E> {
    }

    /*class TreeMapItem<K, V> : Entry<K, V> {
        public K key;
        public V value;

        public TreeMapItem( K k, V v ) {
            key = k;
            value = v;
        }
    }

    class TreeMapItemSet<K, V> : Set<Entry<K, V>> {
        private TreeMap<K, V> m_dict;

        public TreeMapItemSet( TreeMap<K, V> item ) {
            m_dict = item;
        }

        public T[] toArray<T>( T[] arr ) {
            if ( typeof( T ) == typeof( Entry<K, V> ) ) {
                int c = size();
                TreeMapItem<K, V>[] items = new TreeMapItem<K, V>[c];
                int i = 0;
                foreach ( K key in m_dict.Keys ) {
                    items[i] = new TreeMapItem<K, V>( key, m_dict[key] );
                    i++;
                }
                return (T[])(object)items;
            } else {
                return null;
            }
        }

        public Object[] toArray() {
        }

        public int size() {
            return m_dict.Count;
        }

        public bool retainAll( Collection<Entry<K, V>> v ) {
        }

        public bool removeAll( Collection<Entry<K, V>> v ) {
        }

        public bool remove( Object obj ) {
        }

        public Iterator iterator() {
        }

        public bool isEmpty() {
            return m_dict.Count <= 0;
        }

        public int hashCode() {
            return m_dict.hashCode();
        }

        public bool equals( Object obj ) {
            return m_dict.equals( obj );
        }

        public bool contains( Object obj ) {
        }

        public void clear() {
            m_dict.clear();
        }

        public bool containsAll( Collection<Entry<K, V>> v ) {
        }

        public bool addAll( Collection<Entry<K, V>> v ) {
        }

        public bool add( Entry<K, V> v ) {
        }
    }*/

}
#endif
