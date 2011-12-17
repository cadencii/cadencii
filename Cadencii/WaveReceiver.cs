/*
 * WaveReceiver.cs
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
#else
using System;

namespace com.github.cadencii {
#endif

    /// <summary>
    /// 音声波形の受信器のためのインターフェース．
    /// </summary>
    public interface WaveReceiver {
        /// <summary>
        /// 波形を受信します
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="length"></param>
        void push( double[] left, double[] right, int length );

        /// <summary>
        /// 音声波形の受信器を設定します．
        /// </summary>
        /// <param name="r"></param>
        void setReceiver( WaveReceiver r );

        /// <summary>
        /// 波形の受信を終了します．
        /// </summary>
        void end();

        void setGlobalConfig( EditorConfig config );
    }

#if !JAVA
}
#endif
