/*
 * LyricHandle.cs
 * Copyright (C) 2008-2010 kbinani
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
import java.util.*;
#else
using System;
using org.kbinani.java.util;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class LyricHandle implements Cloneable, Serializable {
#else
    [Serializable]
    public class LyricHandle : ICloneable {
#endif
        public Lyric L0;
        public int Index;
        public Vector<Lyric> Trailing = new Vector<Lyric>();

        public LyricHandle() {
            L0 = new Lyric();
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティが総称型引数を用いる型である場合に，
        /// その型の限定名を返します．それ以外の場合は空文字を返します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getGenericTypeName( String name ) {
            if ( name != null ) {
                if ( name.Equals( "Trailing" ) ) {
                    return "org.kbinani.vsq.Lyric";
                }
            }
            return "";
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティを，XMLシリアライズ時に無視するかどうかを表す
        /// ブール値を返します．デフォルトの実装では戻り値は全てfalseです．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static boolean isXmlIgnored( String name ) {
            return false;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getXmlElementName( String name ) {
            return name;
        }

        public Lyric getLyricAt( int index ){
            if( index == 0 ){
                return L0;
            }else{
                return Trailing.get( index - 1 );
            }
        }

        public void setLyricAt( int index, Lyric value ){
            if( index == 0 ){
                L0 = value;
            }else{
                Trailing.set( index - 1, value );
            }
        }

        public int getCount(){
            return Trailing.size() + 1;
        }

        /// <summary>
        /// type = Lyric用のhandleのコンストラクタ
        /// </summary>
        /// <param name="phrase">歌詞</param>
        /// <param name="phonetic_symbol">発音記号</param>
        public LyricHandle( String phrase, String phonetic_symbol ) {
            L0 = new Lyric( phrase, phonetic_symbol );
        }

        public Object clone() {
            LyricHandle ret = new LyricHandle();
            ret.Index = Index;
            ret.L0 = (Lyric)L0.clone();
            int c = Trailing.size();
            for( int i = 0; i < c; i++ ){
                Lyric buf = (Lyric)Trailing.get( i ).clone();
                ret.Trailing.add( buf );
            }
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Lyric;
            ret.L0 = (Lyric)L0.clone();
            ret.Trailing = Trailing;
            ret.Index = Index;
            return ret;
        }
    }

#if !JAVA
}
#endif
