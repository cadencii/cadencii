/*
 * ITextWriter.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace Boare.Lib.Vsq {
#endif

    public interface ITextWriter {
#if JAVA
        void write( String value ) throws IOException;
        void writeLine( String value ) throws IOException;
        void close() throws IOException;
#else
        void write( String value );
        void writeLine( String value );
        void close();
#endif
    }

#if !JAVA
}
#endif
