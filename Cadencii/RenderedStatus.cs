﻿/*
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

import java.util.*;
import org.kbinani.vsq.*;
#else
using System;
using org.kbinani.java.util;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
#endif

    public class RenderedStatus {
        public VsqTrack track;
        public TempoVector tempo;

        /// <summary>
        /// コンストラクタ。trackはcloneされないが、tempoはcloneされる。
        /// </summary>
        /// <param name="track"></param>
        /// <param name="tempo"></param>
        public RenderedStatus( VsqTrack track, TempoVector tempo ) {
            this.track = track;
            this.tempo = new TempoVector();
            for ( Iterator<TempoTableEntry> itr = tempo.iterator(); itr.hasNext(); ) {
                this.tempo.add( itr.next() );
            }
        }

        public RenderedStatus() {
            track = new VsqTrack( 0, 0, 0 );
            tempo = new TempoVector();
        }

        public String getGenericTypeName( String name ) {
            if ( name != null ) {
                if ( name.Equals( "tempo" ) ) {
                    return "org.kbinani.vsq.TempoTableEntry";
                }
            }
            return "";
        }
    }

#if !JAVA
}
#endif
