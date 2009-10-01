#if !JAVA
/*
 * BufferedWriter.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Text;

namespace bocoree {

    public class FileWriter {
        public StreamWriter m_writer;

        public FileWriter( String fileName ) {
            m_writer = new StreamWriter( fileName );
        }
    }

    public class FileOutputStream : FileStream {
        public FileOutputStream( String fileName )
            : base( fileName, FileMode.Open, FileAccess.Write ) {
        }
    }

    public class OutputStreamWriter {
        public StreamWriter m_writer;

        public OutputStreamWriter( FileOutputStream stream, String charsetName ) {
            m_writer = new StreamWriter( stream, Encoding.GetEncoding( charsetName ) );
        }
    }

    public class BufferedWriter {
        private StreamWriter m_writer;

        public BufferedWriter( FileWriter writer ) {
            m_writer = writer.m_writer;
        }

        public BufferedWriter( OutputStreamWriter writer ) {
            m_writer = writer.m_writer;
        }

        public void close() {
            m_writer.Close();
        }

        public void flush() {
            m_writer.Flush();
        }

        public void newLine() {
            m_writer.WriteLine();
        }

        public void write( char[] cbuf, int off, int len ) {
            m_writer.Write( cbuf, off, len );
        }

        public void write( int c ) {
            m_writer.Write( (char)c );
        }

        public void write( String s, int off, int len ) {
            m_writer.Write( s.ToCharArray(), off, len );
        }

        public void write( String str ) {
            m_writer.Write( str );
        }

        public void write( char[] cbuf ) {
            m_writer.Write( cbuf );
        }

    }

}
#endif
