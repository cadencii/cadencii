/*
 * PatchWorkQueue.cs
 * Copyright © 2010-2011 kbinani
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

import org.kbinani.*;
import org.kbinani.apputil.*;

#else

using System;
using com.github.cadencii.apputil;

namespace com.github.cadencii
{
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 合成の範囲やトラック番号を指示するためのクラス
    /// </summary>
    public class PatchWorkQueue
    {
        /// <summary>
        /// 合成対象のトラック番号
        /// </summary>
        public int track;
        /// <summary>
        /// 合成開始位置．単位はclock
        /// </summary>
        public int clockStart;
        /// <summary>
        /// 合成修了位置．単位はclock
        /// </summary>
        public int clockEnd;
        /// <summary>
        /// 合成結果を出力するファイル名
        /// </summary>
        public String file;
        /// <summary>
        /// トラック全体を合成する場合true，それ以外はfalse
        /// </summary>
        public boolean renderAll;
        /// <summary>
        /// シーケンスのインスタンス
        /// </summary>
        public VsqFileEx vsq;

        /// <summary>
        /// このキューの概要を記した文字列を取得します
        /// </summary>
        /// <returns></returns>
        public String getMessage()
        {
            String message = _( "track" ) + "#" + this.track + " ";
#if DEBUG
            sout.println( "PatchWorkQueue#getMessage; q.clockStart=" + this.clockStart + "; q.clockEnd=" + this.clockEnd );
#endif
            double start = this.vsq.getSecFromClock( this.clockStart );
            double cend = this.clockEnd;
            if ( this.clockEnd == int.MaxValue ) {
                cend = this.vsq.TotalClocks + 240;
            }
            double end = this.vsq.getSecFromClock( cend );
            int istart = (int)Math.Floor( start );
            int iend = (int)Math.Floor( end );
            message += istart + "." + str.format( (int)((start - istart) * 100), 2 ) + " " + _( "sec" );
            message += " - ";
            message += iend + "." + str.format( (int)((end - iend) * 100), 2 ) + " " + _( "sec" );

            return message;
        }

        public double getJobAmount()
        {
            double start = this.vsq.getSecFromClock( this.clockStart );
            double cend = this.clockEnd;
            if ( this.clockEnd == int.MaxValue ) {
                cend = this.vsq.TotalClocks + 240;
            }
            double end = this.vsq.getSecFromClock( cend );
            return (end - start) * vsq.config.SamplingRate;
        }

        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }
    }

#if !JAVA
}
#endif
