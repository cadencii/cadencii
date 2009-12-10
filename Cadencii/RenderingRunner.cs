/*
 * RenderingRunner.cs
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
package org.kbinani.Cadencii;
#else
using System;

namespace Boare.Cadencii {
    using boolean = Boolean;
#endif

#if JAVA
    public interface RenderingRunner extends Runnable {
#else
    public interface RenderingRunner : Runnable {
#endif
        //void run();
        double getProgress();
        void abortRendering();
        boolean isRendering();
        double getElapsedSeconds();
        double computeRemainingSeconds();
    }

#if !JAVA
}
#endif
