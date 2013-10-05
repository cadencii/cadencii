/*
 * WaveformZoomUiListener.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii
{
    using System;
    using cadencii.java.awt;

    public interface WaveformZoomUiListener
    {
        void receivePaintSignal(Graphics g);

        void receiveMouseDownSignal(int x, int y);

        void receiveMouseMoveSignal(int x, int y);

        void receiveMouseUpSignal(int x, int y);
    };


}
