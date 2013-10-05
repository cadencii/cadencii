/*
 * WrappedStreamWriter.cs
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
using System;
using cadencii;
using cadencii.java.io;

namespace cadencii.vsq
{

    public class WrappedStreamWriter : ITextWriter
    {
        BufferedWriter m_writer;

        public WrappedStreamWriter( BufferedWriter stream_writer )
        {
            m_writer = stream_writer;
        }

        public void newLine()
        {
            m_writer.newLine();
        }

        public void write( string value )
        {
            m_writer.write( value );
        }

        public void writeLine( string value )
        {
            m_writer.write( value );
            m_writer.newLine();
        }

        public void close()
        {
            m_writer.close();
        }
    }

}
