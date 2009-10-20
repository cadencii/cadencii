/*
 * BMouseEventArgs.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.windows.forms;

import java.awt.*;
import org.kbinani.*;

public class BMouseEventArgs extends BEventArgs
{
    public BMouseButtons Button;
    public int Clicks;
    public int X;
    public int Y;
    public int Delta;

    public BMouseEventArgs( BMouseButtons button, int clicks, int x, int y, int delta )
    {
        Button = button;
        Clicks = clicks;
        X = x;
        Y = y;
        Delta = delta;
    }
}
#endif
