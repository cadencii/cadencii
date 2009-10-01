/*
 * TextMemoryStream.cs
 * Copyright (c) 2008-2009 kbinani
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
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using bocoree;

namespace Boare.Lib.Vsq {

    interface ITextWriter {
        void write( String value );
        void writeLine( String value );
        void close();
    }

    class WrappedStreamWriter : ITextWriter {
        StreamWriter m_writer;

        public WrappedStreamWriter( StreamWriter stream_writer ) {
            m_writer = stream_writer;
        }

        public void write( String value ) {
            m_writer.Write( value );
        }

        public void writeLine( String value ) {
            m_writer.WriteLine( value );
        }

        public void close() {
            m_writer.Close();
        }
    }

    public class TextMemoryStream : IDisposable, ITextWriter {
        private static readonly String NL = (char)0x0d + "" + (char)0x0a;

        private Vector<String> m_lines;
        private int m_index;

        public TextMemoryStream() {
            m_lines = new Vector<String>();
            m_lines.add( "" );
            m_index = 0;
        }

        public TextMemoryStream( String path, Encoding encoding ) {
            m_lines = new Vector<String>();
            m_index = 0;
            if ( PortUtil.isFileExists( path ) ) {
                using ( StreamReader sr = new StreamReader( path, encoding ) ) {
                    while ( sr.Peek() >= 0 ) {
                        String line = sr.ReadLine();
                        m_lines.add( line );
                        m_index++;
                    }
                }
            }
        }

        public void write( String value ) {
            appendString( value );
        }

        public void writeLine( String value ) {
            appendString( value + NL );
        }

        private static Vector<String> splitLine( String line ) {
            Vector<String> ret = new Vector<String>();
            String[] spl_0x0d_0x0a = PortUtil.splitString( line, new String[] { NL }, false );
            for ( int i = 0; i < spl_0x0d_0x0a.Length; i++ ) {
                String[] spl_0x0d = PortUtil.splitString( spl_0x0d_0x0a[i], (char)0x0d );
                for ( int j = 0; j < spl_0x0d.Length; j++ ) {
                    String[] spl_0x0a = PortUtil.splitString( spl_0x0d[j], (char)0x0a );
                    for ( int k = 0; k < spl_0x0a.Length; k++ ) {
                        ret.add( spl_0x0a[k] );
                    }
                }
            }
            return ret;
        }

        private void appendString( String value ) {
            Vector<String> lines2 = splitLine( value );
            int count = lines2.size();
            if ( count > 0 ) {
                m_lines.set( m_index, m_lines.get( m_index ) + lines2.get( 0 ) );
                for ( int i = 1; i < count; i++ ) {
                    m_lines.add( lines2.get( i ) );
                    m_index++;
                }
            }
        }

        public void rewind() {
            m_index = 0;
        }

        public String readLine() {
            m_index++;
            return m_lines.get( m_index - 1 );
        }

        public int peek() {
            if ( m_index < m_lines.size() ) {
                if ( m_lines.get( m_index ).Equals( "" ) ) {
                    return -1;
                } else {
                    return (int)m_lines.get( m_index )[0];
                }
            } else {
                return -1;
            }
        }

        public void close() {
            m_lines.clear();
        }

        public void Dispose() {
            close();
        }
    }

}
