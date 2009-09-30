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
package com.boare.cadencii;

import java.awt.*;

/// <summary>
/// 画面描画が行われたときのステータスを表す
/// </summary>
public class ScreenStatus {
    public int startToDrawX;
    public int startToDrawY;
    public float scaleX;
    public Dimension size;

    public boolean equals( Object obj ) {
        if ( obj instanceof ScreenStatus ) {
            ScreenStatus ss = (ScreenStatus)obj;
            if ( ss.startToDrawX == startToDrawX &&
                 ss.startToDrawY == startToDrawY &&
                 ss.scaleX == scaleX &&
                 ss.size == size ) {
                return true;
            } else {
                return false;
            }
        } else {
            return super.equals( obj );
        }
    }
}
