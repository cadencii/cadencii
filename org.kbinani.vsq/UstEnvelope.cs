/*
 * UstEnvelope.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;

namespace org.kbinani.vsq
{
#endif

    /// <summary>
    /// UTAUスクリプト形式で使用されるエンベロープのデータ点を表現します
    /// </summary>
#if JAVA
    public class UstEnvelope implements Cloneable, Serializable
#else
    [Serializable]
    public class UstEnvelope : ICloneable
#endif
    {
        public int p1 = 0;
        public int p2 = 5;
        public int p3 = 35;
        public int v1 = 0;
        public int v2 = 100;
        public int v3 = 100;
        public int v4 = 0;
        public int p4 = 0;
        public int p5 = 0;
        public int v5 = 100;

        /// <summary>
        /// コンストラクタ．デフォルトのエンベロープを構成します
        /// </summary>
        public UstEnvelope()
        {
        }

        /// <summary>
        /// UTAUスクリプト形式に記録されているエンベロープの表現に基づき，インスタンスを構成します
        /// </summary>
        /// <param name="line">ustに記録されるエンベロープの記述行</param>
        public UstEnvelope( String line )
        {
            if ( !str.startsWith( str.toLower( line ), "envelope=" ) ) {
                return;
            }
            String[] spl = PortUtil.splitString( line, '=' );
            if ( spl.Length < 2 ) {
                return;
            }
            spl = PortUtil.splitString( spl[1], ',' );
            if ( spl.Length < 7 ) {
                return;
            }
            try {
                p1 = (int)str.tof( spl[0] );
                p2 = (int)str.tof( spl[1] );
                p3 = (int)str.tof( spl[2] );
                v1 = (int)str.tof( spl[3] );
                v2 = (int)str.tof( spl[4] );
                v3 = (int)str.tof( spl[5] );
                v4 = (int)str.tof( spl[6] );
                if ( spl.Length == 11 ) {
                    p4 = (int)str.tof( spl[8] );
                    p5 = (int)str.tof( spl[9] );
                    v5 = (int)str.tof( spl[10] );
                }
            } catch ( Exception ex ) {
            }
        }

        /// <summary>
        /// このインスタンスのディープコピーを作成します
        /// </summary>
        /// <returns></returns>
        public Object clone()
        {
            return new UstEnvelope( toString() );
        }

#if !JAVA
        /// <summary>
        /// このインスタンスのディープコピーを作成します
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return clone();
        }
#endif

#if !JAVA
        /// <summary>
        /// このインスタンスの文字列表現を取得します
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return toString();
        }
#endif

        /// <summary>
        /// このインスタンスの文字列表現を取得します
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            String ret = "Envelope=" + p1 + "," + p2 + "," + p3 + "," + v1 + "," + v2 + "," + v3 + "," + v4;
            ret += ",%," + p4 + "," + p5 + "," + v5;
            return ret;
        }

        public int getCount()
        {
            return 5;
        }
    }

#if !JAVA
}
#endif
