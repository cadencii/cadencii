/*
 * ITextWriter.cs
 * Copyright © 2009-2011 kbinani
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
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace com.github.cadencii.vsq
{
#endif

    public interface ITextWriter
    {
#if JAVA
        void write( String value ) throws IOException;
        void writeLine( String value ) throws IOException;
        void close() throws IOException;
        void newLine() throws IOException;
#else
        void write( String value );
        void writeLine( String value );
        void close();
        void newLine();
#endif
    }

#if !JAVA
}
#endif
