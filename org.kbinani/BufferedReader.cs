#if !JAVA
/*
 * BufferedReader.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Text;

namespace org.kbinani.java.io {

    public class FileReader{
        public StreamReader m_reader;

        public FileReader( String fileName ){
            m_reader = new StreamReader( fileName );
        }
    }

    public class FileInputStream : FileStream, InputStream {
        public FileInputStream( String fileName )
            : base( fileName, FileMode.Open, FileAccess.Read ) {
        }

        public void close() {
            base.Close();
        }

        public int read( byte[] b, int off, int len ) {
            return base.Read( b, off, len );
        }

        public int read( byte[] b ) {
            return read( b, 0, b.Length );
        }

        public int read() {
            return base.ReadByte();
        }
    }

    public class InputStreamReader {
        public StreamReader m_reader;

        public InputStreamReader( FileInputStream stream, String charsetName ) {
            m_reader = new StreamReader( stream, Encoding.GetEncoding( charsetName ) );
        }
    }

    public class BufferedReader {
        private StreamReader m_reader;

        public BufferedReader( FileReader reader ) {
            m_reader = reader.m_reader;
        }

        public BufferedReader( InputStreamReader reader ) {
            m_reader = reader.m_reader;
        }

        public void close() {
            m_reader.Close();
        }

        public int read() {
            return m_reader.Read();
        }

        public int read( char[] cbuf, int off, int len ) {
            return m_reader.Read( cbuf, off, len );
        }

        public String readLine() {
            return m_reader.ReadLine();
        }

        public bool ready() {
            return m_reader.Peek() >= 0;
        }
    }

}
#endif
