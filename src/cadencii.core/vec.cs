/*
 * vec.cs
 * Copyright © 2011 kbinani
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
#if JAVA

package cadencii;

import java.util.Collections;
import java.util.Vector;

#else

#if !__cplusplus
using System;
using System.IO;
using System.Collections.Generic;

#endif
    namespace cadencii
    {
#if !__cplusplus
        using int8_t = System.SByte;
        using int16_t = System.Int16;
        using int32_t = System.Int32;
        using int64_t = System.Int64;
        using uint8_t = System.Byte;
        using uint16_t = System.UInt16;
        using uint32_t = System.UInt32;
        using uint64_t = System.UInt64;
#endif
#endif

#if __cplusplus
        class vec
#else
        public class vec
#endif
        {
            private vec()
            {
            }

#if __cplusplus
        public:
#endif

#if JAVA
            public static <T> void ensureCapacity( Vector<T> arr, int size )
#elif __cplusplus
            template<typename T>
            static void ensureCapacity( vector<T> &arr, int size )
#else
            public static void ensureCapacity<T>( List<T> arr, int size )
#endif
            {
#if JAVA
                arr.ensureCapacity( size );
#else
                int delta = size - vec.size( arr );
                if ( delta > 0 ) {
                    // 増やす
                    for ( int i = 0; i < delta; i++ ) {
#if __cplusplus
                        add( arr, (T)NULL );
#else
                        add( arr, default( T ) );
#endif
                    }
                }
#endif
            }

#if JAVA
            public static <T extends Comparable<? super T>> void sort( Vector<T> arr )
#elif __cplusplus
            template<class T>
            static void sort( vector<T> &arr )
#else
            public static void sort<T>( List<T> arr )
#endif
            {
#if JAVA
                Collections.sort( arr );
#elif __cplusplus
                std::stable_sort( arr.begin(), arr.end() );
#else
                arr.Sort();
#endif
            }

#if JAVA
            public static <T> T get( Vector<T> arr, int index )
#elif __cplusplus
            template<typename T>
            static T get( vector<T>& arr, int index )
#else
            public static T get<T>( List<T> arr, int index )
#endif
            {
#if JAVA
                return arr.get( index );
#else
                return arr[index];
#endif
            }

#if JAVA
            public static <T> void set( Vector<T> arr, int index, T item )
#elif __cplusplus
            template<typename T>
            static void set( vector<T>& arr, int index, T item )
#else
            public static void set<T>( List<T> arr, int index, T item )
#endif
            {
#if JAVA
                arr.set( index, item );
#else
                arr[index] = item;
#endif
            }

#if JAVA
            public static <T> int size( Vector<T> arr )
#elif __cplusplus
            template<typename T>
            static int size( vector<T>& arr )
#else
            public static int size<T>( List<T> arr )
#endif
            {
#if JAVA
                return arr.size();
#elif __cplusplus
                return arr.size();
#else
                return arr.Count;
#endif
            }

#if JAVA
            public static <T> void add( Vector<T> arr, T item )
#elif __cplusplus
            template<typename T>
            static void add( vector<T>& arr, T item )
#else
            public static void add<T>( List<T> arr, T item )
#endif
            {
#if JAVA
                arr.add( item );
#elif __cplusplus
                arr.push_back( item );
#else
                arr.Add( item );
#endif
            }

#if JAVA
            public static <T> boolean contains( Vector<T> arr, T item )
#elif __cplusplus
            template<typename T>
            static bool contains( vector<T>& arr, T item )
#else
            public static bool contains<T>( List<T> arr, T item )
#endif
            {
#if JAVA
                return arr.contains( item );
#elif __cplusplus
                if ( find( arr.begin(), arr.end(), item ) != arr.end() ) {
                    return true;
                } else {
                    return false;
                }
#else
                return arr.Contains( item );
#endif
            }

#if JAVA
            public static <T> void clear( Vector<T> arr )
#elif __cplusplus
            template<typename T>
            static void clear( vector<T>& arr )
#else
            public static void clear<T>( List<T> arr )
#endif
            {
#if JAVA
                arr.clear();
#elif __cplusplus
                arr.clear();
#else
                arr.Clear();
#endif
            }
        };

#if !JAVA
    }
#endif
