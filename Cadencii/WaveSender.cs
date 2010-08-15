/*
 * WaveSender.cs
 * Copyright (C) 2010 kbinani
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
#endif

    /// <summary>
    /// 音声波形を出力するジェネレータ．
    /// </summary>
#if JAVA
    public interface WaveSender {
#else
    public interface WaveSender {
#endif
        void pull( double[] l, double[] r, int length );
        void setSender( WaveSender s );
        void end();
    }

#if !JAVA
}
#endif
