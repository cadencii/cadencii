/*
 * ITextWriter.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.vsq;

import java.io.*;
#else
using System;

namespace cadencii.vsq
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
        void write( string value );
        void writeLine( string value );
        void close();
        void newLine();
#endif
    }

#if !JAVA
}
#endif
