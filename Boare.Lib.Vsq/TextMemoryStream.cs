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
#if JAVA
package org.kbinani.vsq;

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using bocoree;
using bocoree.util;
using bocoree.io;

namespace Boare.Lib.Vsq {
#endif

#if JAVA
    public class TextMemoryStream implements ITextWriter {
#else
    public class TextMemoryStream : IDisposable, ITextWriter {
#endif
        private static readonly String NL = (char)0x0d + "" + (char)0x0a;

        private Vector<String> m_lines;
        private int m_index;

        public TextMemoryStream() {
            m_lines = new Vector<String>();
            m_lines.add( "" );
            m_index = 0;
        }

        public TextMemoryStream( String path, String encoding ) {
            m_lines = new Vector<String>();
            m_index = 0;
            if ( PortUtil.isFileExists( path ) ) {
                BufferedReader sr = null;
                try {
                    sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), encoding ) );
                    while ( sr.ready() ) {
                        String line = sr.readLine();
                        m_lines.add( line );
                        m_index++;
                    }
                } catch ( Exception ex ) {
                } finally {
                    if ( sr != null ) {
                        try {
                            sr.close();
                        } catch ( Exception ex2 ) {
                        }
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
#if JAVA
                    return (int)m_lines.get( m_index ).charAt( 0 );
#else
                    return (int)m_lines.get( m_index )[0];
#endif
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

#if !JAVA
}
#endif
