/*
 * TextMemoryStream.cs
 * Copyright (C) 2008-2010 kbinani
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

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using System.Text;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

// old: Vector<String>
namespace org.kbinani.vsq {
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
                    PortUtil.stderr.println( "TextMemoryStream#.ctor; ex=" + ex );
                } finally {
                    if ( sr != null ) {
                        try {
                            sr.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "TextMemoryStream#.ctor; ex2=" + ex2 );
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

// implA: String[]
namespace org.kbinani.vsq.implA {
#endif

#if JAVA
    public class TextMemoryStream implements ITextWriter {
#else
    public class TextMemoryStream : IDisposable, ITextWriter {
#endif
        private static readonly String NL = (char)0x0d + "" + (char)0x0a;
        const int INIT_BUFLEN = 512;

        private String[] m_lines;
        private int m_index;
        private int length;

        public TextMemoryStream() {
            m_lines = new String[INIT_BUFLEN];
            m_lines[0] = "";
            m_index = 0;
        }

        public TextMemoryStream( String path, String encoding ) {
            m_lines = new String[INIT_BUFLEN];
            m_index = 0;
            if ( PortUtil.isFileExists( path ) ) {
                BufferedReader sr = null;
                try {
                    sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), encoding ) );
                    while ( sr.ready() ) {
                        String line = sr.readLine();
                        m_index++;
                        ensureBufferLength( m_index + 1 );
                        m_lines[m_index] = line;
                        length++;
                    }
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "TextMemoryStream#.ctor; ex=" + ex );
                } finally {
                    if ( sr != null ) {
                        try {
                            sr.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "TextMemoryStream#.ctor; ex2=" + ex2 );
                        }
                    }
                }
            }
        }

        private void ensureBufferLength( int length ) {
            if ( m_lines == null ) {
                m_lines = new String[INIT_BUFLEN];
            }
            if ( length > m_lines.Length ) {
                int newLength = length;
                if ( this.length <= 0 ) {
                    newLength = (int)(length * 1.2);
                } else {
                    int order = length / m_lines.Length;
                    if ( order <= 1 ) {
                        order = 2;
                    }
                    newLength = m_lines.Length * order;
                }
                Array.Resize( ref m_lines, newLength );
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
                m_lines[m_index] = m_lines[m_index] + lines2.get( 0 );
                for ( int i = 1; i < count; i++ ) {
                    m_index++;
                    ensureBufferLength( m_index + 1 );
                    m_lines[m_index] = lines2.get( i );
                    length++;
                }
            }
        }

        public void rewind() {
            m_index = 0;
        }

        public String readLine() {
            m_index++;
            return m_lines[m_index - 1];
        }

        public int peek() {
            if ( m_index < length ) {
                if ( m_lines[m_index].Equals( "" ) ) {
                    return -1;
                } else {
#if JAVA
                    return (int)m_lines.get( m_index ).charAt( 0 );
#else
                    return (int)m_lines[m_index][0];
#endif
                }
            } else {
                return -1;
            }
        }

        public void close() {
            length = 0;
        }

        public void Dispose() {
            close();
        }
    }

#if !JAVA
}

// implB: BArray<String>
namespace org.kbinani.vsq.implB {
#endif

#if JAVA
    public class TextMemoryStream implements ITextWriter {
#else
    public class TextMemoryStream : IDisposable, ITextWriter {
#endif
        private static readonly String NL = (char)0x0d + "" + (char)0x0a;

        private BArray<String> m_lines;
        private int m_index;

        public TextMemoryStream() {
            m_lines = new BArray<String>();
            m_lines.add( "" );
            m_index = 0;
        }

        public TextMemoryStream( String path, String encoding ) {
            m_lines = new BArray<String>();
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
                    PortUtil.stderr.println( "TextMemoryStream#.ctor; ex=" + ex );
                } finally {
                    if ( sr != null ) {
                        try {
                            sr.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "TextMemoryStream#.ctor; ex2=" + ex2 );
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

// implC: BArray<StringBuilder>
namespace org.kbinani.vsq.implC {
#endif

#if JAVA
    public class TextMemoryStream implements ITextWriter {
#else
    public class TextMemoryStream : IDisposable, ITextWriter {
#endif
        private static readonly String NL = (char)0x0d + "" + (char)0x0a;

        private BArray<StringBuilder> m_lines;
        private int m_index;

        public TextMemoryStream() {
            m_lines = new BArray<StringBuilder>();
            m_lines.add( new StringBuilder( "" ) );
            m_index = 0;
        }

        public TextMemoryStream( String path, String encoding ) {
            m_lines = new BArray<StringBuilder>();
            m_index = 0;
            if ( PortUtil.isFileExists( path ) ) {
                BufferedReader sr = null;
                try {
                    sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), encoding ) );
                    while ( sr.ready() ) {
                        String line = sr.readLine();
                        m_lines.add( new StringBuilder( line ) );
                        m_index++;
                    }
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "TextMemoryStream#.ctor; ex=" + ex );
                } finally {
                    if ( sr != null ) {
                        try {
                            sr.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "TextMemoryStream#.ctor; ex2=" + ex2 );
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
                m_lines.get( m_index ).Append( lines2.get( 0 ) );
                for ( int i = 1; i < count; i++ ) {
                    m_lines.add( new StringBuilder( lines2.get( i ) ) );
                    m_index++;
                }
            }
        }

        public void rewind() {
            m_index = 0;
        }

        public StringBuilder readLine() {
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
