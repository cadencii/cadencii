/*
 * InternalStreamWriter.cs
 * Copyright © 2011 kbinani
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

    /// <summary>
    /// 改行コードに0x0d 0x0aを用いるテキストライター
    /// </summary>
#if JAVA
    class InternalStreamWriter implements ITextWriter
#else
    class InternalStreamWriter : ITextWriter
#endif
    {
        private String mNL = "\n";
#if JAVA
        private BufferedWriter mStream;
#else
        private System.IO.StreamWriter mStream;
#endif

        public InternalStreamWriter( String path, String encoding )
#if JAVA
            throws java.io.FileNotFoundException,
                   java.io.UnsupportedEncodingException
#endif
        {
            mNL = new String( new char[]{ (char)0x0d, (char)0x0a } );
#if JAVA
            mStream = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( path ), encoding ) );
#else
            mStream = new System.IO.StreamWriter( path, false, System.Text.Encoding.GetEncoding( encoding ) );
#endif
        }

        public void write( String s )
#if JAVA
            throws java.io.IOException
#endif
        {
#if JAVA
            mStream.write( s );
#else
            mStream.Write( s );
#endif
        }
        
        public void writeLine( String s )
#if JAVA
            throws java.io.IOException
#endif
        {
            write( s );
            newLine();
        }
        
        public void newLine()
#if JAVA
            throws java.io.IOException
#endif
        {
            write( mNL );
        }
        
        public void close()
#if JAVA
            throws java.io.IOException
#endif
        {
#if JAVA
            mStream.close();
#else
            mStream.Close();
#endif
        }
    }

#if !JAVA
}
#endif
