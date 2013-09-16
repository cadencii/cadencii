/*
 * RenderQueue.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii;

import java.util.*;
#else
using System;
using cadencii.java.util;

namespace cadencii {
    using boolean = System.Boolean;
#endif

    public class RenderQueue {
        private Vector<String> _resampler_arg = new Vector<String>();
        public Vector<String> WavtoolArgPrefix = new Vector<String>();
        public Vector<String> WavtoolArgSuffix = new Vector<String>();
        //public String WavtoolArgPrefix;
        //public String WavtoolArgSuffix;
        public OtoArgs Oto;
        //public double secEnd;
        public double secStart;
        public String FileName;
        public boolean ResamplerFinished;
        /// <summary>
        /// MD5ハッシュによるファイル名の生成元となる文字列
        /// </summary>
        public String hashSource;

        /// <summary>
        /// このキューの引数リストに、引数を1つ追加します
        /// </summary>
        /// <param name="value"></param>
        public void appendArg( String value ) {
            _resampler_arg.add( value );
        }

        /// <summary>
        /// このキューの引数リストに、指定された引数をすべて追加します
        /// </summary>
        /// <param name="args"></param>
        public void appendArgRange( String[] args ) {
            foreach ( String s in args ) {
                _resampler_arg.add( s );
            }
        }

        /// <summary>
        /// このキューの引数リストを、文字列配列の形式で取得します
        /// </summary>
        /// <returns></returns>
        public String[] getResamplerArg() {
            return _resampler_arg.toArray( new String[0] );
        }

        /// <summary>
        /// このキューの引数リストを、スペースで繋げた文字列形式で取得します
        /// </summary>
        /// <returns></returns>
        public String getResamplerArgString() {
            String ret = "";
            int c = _resampler_arg.size();
            for ( int i = 0; i < c; i++ ) {
                ret += _resampler_arg.get( i ) + ((i < c - 1) ? " " : "");
            }
            return ret;
        }
    }

#if !JAVA
}
#endif
