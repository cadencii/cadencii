/*
 * ScreenStatus.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.awt.*;
#else
using System;
using System.Drawing;
using bocoree.java.awt;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 画面描画が行われたときのステータスを表す
    /// </summary>
    public class ScreenStatus {
        public int StartToDrawX;
        public int StartToDrawY;
        public float ScaleX;
        public Dimension Size;

#if !JAVA
        public override bool Equals( object obj ) {
            return equals( obj );
        }
#endif

        public boolean equals( Object obj ) {
            if ( obj is ScreenStatus ) {
                ScreenStatus ss = (ScreenStatus)obj;
                if ( ss.StartToDrawX == StartToDrawX &&
                     ss.StartToDrawY == StartToDrawY &&
                     ss.ScaleX == ScaleX &&
                     ss.Size.width == Size.width &&
                     ss.Size.height == Size.height) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return base.Equals( obj );
            }
        }
    }

#if !JAVA
}
#endif
