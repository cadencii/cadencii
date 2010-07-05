/*
 * RenderQueue.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.util.*;
#else
using System;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class RenderQueue {
        private Vector<String> _resampler_arg = new Vector<String>();
        public String WavtoolArgPrefix;
        public String WavtoolArgSuffix;
        public OtoArgs Oto;
        public double secEnd;
        public String FileName;
        public boolean ResamplerFinished;

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
