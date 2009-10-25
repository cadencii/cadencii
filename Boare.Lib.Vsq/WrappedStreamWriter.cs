/*
 * WrappedStreamWriter.cs
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
using bocoree;
using bocoree.io;

namespace Boare.Lib.Vsq {
#endif

#if JAVA
    public class WrappedStreamWriter implements ITextWriter {
#else
    public class WrappedStreamWriter : ITextWriter {
#endif
        BufferedWriter m_writer;

        public WrappedStreamWriter( BufferedWriter stream_writer ) {
            m_writer = stream_writer;
        }

        public void write( String value )
#if JAVA
            throws IOException
#endif
        {
            m_writer.write( value );
        }

        public void writeLine( String value )
#if JAVA
            throws IOException
#endif
        {
            m_writer.write( value );
            m_writer.newLine();
        }

        public void close()
#if JAVA
            throws IOException
#endif
        {
            m_writer.close();
        }
    }

#if !JAVA
}
#endif
