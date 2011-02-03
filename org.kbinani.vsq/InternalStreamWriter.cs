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
namespace org.kbinani.vsq
{
#endif

#if JAVA
    class InternalStreamWriter implements ITextWriter
#else
    class InternalStreamWriter : ITextWriter
#endif
    {
        private String mNL = "\n";
        private BufferedWriter mStream;

        public InternalStreamWriter( String path, String encoding )
#if JAVA
            throws java.io.FileNotFoundException,
                   java.io.UnsupportedEncodingException
#endif
        {
            mNL = new String( new char[]{ 0x0d, 0x0a } );
            mStream = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( path ), encoding ) );
        }

        public void write( String s )
#if JAVA
            throws java.io.IOException
#endif
        {
            mStream.write( s );
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
            mStream.write( mNL );
        }
        
        public void close()
#if JAVA
            throws java.io.IOException
#endif
        {
            mStream.close();
        }
    }

#if !JAVA
}
#endif
