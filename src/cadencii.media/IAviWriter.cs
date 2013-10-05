/*
 * IAviWriter.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.media.
 *
 * cadencii.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Drawing;

namespace cadencii.media
{

    public interface IAviWriter
    {
        void AddFrame(Bitmap frame);
        void Close();
        bool Open(string file, uint scale, uint rate, int width, int height, IntPtr hwnd);
        Size Size
        {
            get;
        }
        uint Scale
        {
            get;
        }
        uint Rate
        {
            get;
        }
    }

}
