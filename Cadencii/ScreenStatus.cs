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
using System;
using System.Drawing;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    /// <summary>
    /// 画面描画が行われたときのステータスを表す
    /// </summary>
    public class ScreenStatus {
        public int StartToDrawX;
        public int StartToDrawY;
        public float ScaleX;
        public Size Size;

        public override boolean Equals( object obj ) {
            if ( obj is ScreenStatus ) {
                ScreenStatus ss = (ScreenStatus)obj;
                if ( ss.StartToDrawX == StartToDrawX &&
                     ss.StartToDrawY == StartToDrawY &&
                     ss.ScaleX == ScaleX &&
                     ss.Size == Size ) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return base.Equals( obj );
            }
        }
    }

}
