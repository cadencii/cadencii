/*
 * TextMemoryStream.cs
 * Copyright © 2008-2011 kbinani
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

namespace org.kbinani.vsq
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class TextStream implements ITextWriter {
#else
    public class TextStream : ITextWriter, IDisposable
    {
#endif
        const int INIT_BUFLEN = 512;

        private char[] array = new char[INIT_BUFLEN];
        private int length = 0;
        private int position = -1;

        public int getPointer()
        {
            return position;
        }

        public void setPointer( int value )
        {
            position = value;
        }

        public char get()
        {
            position++;
            return array[position];
        }

        public String readLine()
        {
            StringBuilder sb = new StringBuilder();
            // '\n'が来るまで読み込み
            position++;
            for ( ; position < length; position++ ) {
                char c = array[position];
                if ( c == '\n' ) {
                    break;
                }
#if JAVA
                sb.append( c );
#else
                sb.Append( c );
#endif
            }
            return sb.ToString();
        }

        public boolean ready()
        {
            if ( 0 <= position + 1 && position + 1 < length ) {
                return true;
            } else {
                return false;
            }
        }

        private void ensureCapacity( int length )
        {
            if ( length > array.Length ) {
                int newLength = length;
                if ( this.length <= 0 ) {
                    newLength = (length * 3) >> 1;
                } else {
                    int order = length / array.Length;
                    if ( order <= 1 ) {
                        order = 2;
                    }
                    newLength = array.Length * order;
                }
#if JAVA
#if JAVA_1_5
                char[] buf = new char[newLength];
                for( int i = 0; i < array.length; i++ ){
                    buf[i] = array[i];
                }
                array = buf;
#else
                array = Arrays.copyOf( array, newLength );
#endif
#else
                Array.Resize( ref array, newLength );
#endif
            }
        }

        public void write( String str )
        {
            int len = PortUtil.getStringLength( str );
            int newSize = length + len;
            int offset = length;
            ensureCapacity( newSize );
            for ( int i = 0; i < len; i++ ) {
#if JAVA
                array[offset + i] = str.charAt( i );
#else
                array[offset + i] = str[i];
#endif
            }
            length = newSize;
        }

        public void writeLine( String str )
        {
            int len = PortUtil.getStringLength( str );
            int newSize = length + len + 1;
            int offset = length;
            ensureCapacity( newSize );
            for ( int i = 0; i < len; i++ ) {
#if JAVA
                array[offset + i] = str.charAt( i );
#else
                array[offset + i] = str[i];
#endif
            }
            array[offset + len] = '\n';
            length = newSize;
        }

        public void close()
        {
            array = null;
            length = 0;
        }

#if !JAVA
        public void Dispose()
        {
            close();
        }
#endif
    }

#if !JAVA
}
#endif
