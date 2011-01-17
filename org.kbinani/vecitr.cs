/*
 * vecitr.cs
 * Copyright c 2011 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani;

import java.util.*;
#else
#if !__cplusplus
using System;
using System.IO;
using System.Collections.Generic;

#endif
namespace org
{
    namespace kbinani
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
        template<typename T>
        class vecitr
#else
        public class vecitr<T>
#endif
        {
#if JAVA
            Vector<T> list;
#elif __cplusplus
#else
            List<T> list;
#endif
#if __cplusplus
            typename vector<T>::iterator itr;
            typename vector<T>::iterator itr_end;
#else
            int index = 0;
#endif

#if JAVA
            public vecitr( Vector<T> list )
#elif __cplusplus
            public vecitr( vector<T>& list )
#else
            public vecitr( List<T> list )
#endif
            {
#if __cplusplus
                itr = list.begin();
                itr_end = list.end();
#else
                this.list = list;
                index = 0;
#endif
            }

#if JAVA
            public boolean hasNext()
#else
            public bool hasNext()
#endif
            {
#if JAVA
                if ( index + 1 < list.size() ) {
                    return true;
                } else {
                    return false;
                }
#elif __cplusplus
                itr++;
                bool ret = false;
                if( itr != itr_end ){
                    ret = true;
                }
                itr--;
                return ret;
#else
                if ( index + 1 < list.Count ) {
                    return true;
                } else {
                    return false;
                }
#endif
            }

#if __cplusplus
            public T *next()
#else
            public T next()
#endif
            {
#if JAVA
                index++;
                return list.get( index );
#elif __cplusplus
                itr++;
                return &*itr;
#else
                index++;
                return list[index];
#endif

            }
        };

#if !JAVA
    }
}
#endif
