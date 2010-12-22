/*
 * WrappedStreamWriter.cs
 * Copyright (C) 2009-2010 kbinani
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

namespace org.kbinani.vsq {
#endif

#if JAVA
    public class WrappedStreamWriter implements ITextWriter {
#else
    public class WrappedStreamWriter : ITextWriter {
#endif

#if __cplusplus
        BufferedWriter& m_writer;
#else
        BufferedWriter m_writer;
#endif

#if __cplusplus
        public WrappedStreamWriter( BufferedWriter& stream_writer ) 
#else
        public WrappedStreamWriter( BufferedWriter stream_writer ) 
#endif
        {
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
