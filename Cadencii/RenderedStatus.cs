/*
 * RenderedStatus.cs
 * Copyright (C) 2010 kbinani
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
package org.kbinani.cadencii;

#else
using System;
using org.kbinani.java.util;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
#endif

    public class RenderedStatus {
        public VsqTrack track;
        public Vector<TempoTableEntry> tempo;

        /// <summary>
        /// コンストラクタ。trackはcloneされないが、tempoはcloneされる。
        /// </summary>
        /// <param name="track"></param>
        /// <param name="tempo"></param>
        public RenderedStatus( VsqTrack track, Vector<TempoTableEntry> tempo ) {
            this.track = track;
            this.tempo = new Vector<TempoTableEntry>();
            for ( Iterator itr = tempo.iterator(); itr.hasNext(); ) {
                this.tempo.add( (TempoTableEntry)itr.next() );
            }
        }
    }

#if !JAVA
}
#endif
