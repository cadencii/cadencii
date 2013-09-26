/*
 * LyricHandle.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.vsq;

import java.io.*;
import java.util.*;
import cadencii.xml.*;

#elif __cplusplus
namespace org { namespace kbinani { namespace vsq {
#else
using System;
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii.vsq
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class LyricHandle implements Cloneable, Serializable
#elif __cplusplus
    class LyricHandle
#else
    [Serializable]
    public class LyricHandle : ICloneable
#endif
    {
        public Lyric L0;
        public int Index;
#if __cplusplus
        public: vector<Lyric> Trailing;
#else
#if JAVA
        @XmlGenericType( Lyric.class )
#endif
        public List<Lyric> Trailing = new List<Lyric>();
#endif

        public LyricHandle()
        {
#if !__cplusplus
            L0 = new Lyric();
#endif
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getXmlElementName( String name )
        {
            return name;
        }

        public Lyric getLyricAt( int index )
        {
            if ( index == 0 ) {
                return L0;
            } else {
#if JAVA
                return Trailing.get( index - 1 );
#else
                return Trailing[index - 1];
#endif
            }
        }

        public void setLyricAt( int index, Lyric value )
        {
            if ( index == 0 ) {
                L0 = value;
            } else {
#if JAVA
                Trailing.set( index - 1, value );
#else
                Trailing[index - 1] = value;
#endif
            }
        }

        public int getCount()
        {
            return Trailing.Count + 1;
        }

        /// <summary>
        /// type = Lyric用のhandleのコンストラクタ
        /// </summary>
        /// <param name="phrase">歌詞</param>
        /// <param name="phonetic_symbol">発音記号</param>
        public LyricHandle( String phrase, String phonetic_symbol )
        {
            L0 = new Lyric( phrase, phonetic_symbol );
        }

#if !__cplusplus
        public Object clone()
        {
            LyricHandle ret = new LyricHandle();
            ret.Index = Index;
            ret.L0 = (Lyric)L0.clone();
            int c = Trailing.Count;
            for ( int i = 0; i < c; i++ ) {
                Lyric buf = (Lyric)Trailing[ i ].clone();
                ret.Trailing.Add( buf );
            }
            return ret;
        }
#endif

#if JAVA
#elif __cplusplus
#else
        public object Clone()
        {
            return clone();
        }
#endif

#if __cplusplus
    };
#else
    }
#endif

#if JAVA
#elif __cplusplus
} } }
#else
}
#endif
