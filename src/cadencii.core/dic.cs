/*
 * dic.cs
 * Copyright c 2011 kbinani
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

import java.util.*;
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
        class dic
#else
        public class dic
#endif
        {
            private dic()
            {
            }
#if __cplusplus
        public:
#endif

#if JAVA
            public static <K, V> int count( TreeMap<K, V> dict )
#elif __cplusplus
            template<typename K, typename V>
            static int count( map<K, V> &dict )
#else
            public static int count<K, V>( Dictionary<K, V> dict )
#endif
            {
#if JAVA
                return dict.size();
#elif __cplusplus
                return dict.count();
#else
                return dict.Count;
#endif
            }

#if JAVA
            public static <K, V> boolean containsKey( TreeMap<K, V> dict, K key )
#elif __cplusplus
            template<typename K, typename V>
            static bool containsKey( map<K, V> &dict, K key )
#else
            public static bool containsKey<K, V>( Dictionary<K, V> dict, K key )
#endif
            {
#if JAVA
                return dict.containsKey( key );
#elif __cplusplus
                return dict.count( key ) > 0;
#else
                return dict.ContainsKey( key );
#endif
            }

#if JAVA
            public static <K, V> void put( TreeMap<K, V> dict, K key, V value )
#elif __cplusplus
            template<typename K, typename V>
            static void put( map<K, V> &dict, K key, V value )
#else
            public static void put<K, V>( Dictionary<K, V> dict, K key, V value )
#endif
            {
#if JAVA
                dict.put( key, value );
#else
                dict[key] = value;
#endif
            }

#if JAVA
            public static <K, V> V get( TreeMap<K, V> dict, K key )
#elif __cplusplus
            template<typename K, typename V>
            static V get( map<K, V> &dict, K key )
#else
            public static V get<K, V>( Dictionary<K, V> dict, K key )
#endif
            {
#if JAVA
                return dict.get( key );
#else
                return dict[key];
#endif
            }
        };

#if !JAVA
    }
#endif
