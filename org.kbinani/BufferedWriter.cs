#if !JAVA
/*
 * BufferedWriter.cs
 * Copyright © 2009-2010 kbinani
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

    public class FileWriter {
        public StreamWriter m_writer;

        public FileWriter( String fileName ) {
            m_writer = new StreamWriter( fileName );
        }
    }

    public class FileOutputStream : FileStream, OutputStream {
        public FileOutputStream( String fileName, bool append )
            : base( fileName, FileMode.Create, FileAccess.Write ) {
            if( append ){
                base.Seek( base.Length, SeekOrigin.Begin );
            }
        }

        public FileOutputStream( String fileName )
            : this( fileName, false ) {
        }

        public void close() {
            base.Close();
        }

        /// <summary>
        /// 出力ストリームをフラッシュして、バッファリングされていたすべての出力バイトを強制的にストリームに書き込みます。
        /// </summary>
        public void flush() {
            base.Flush();
        }

        /// <summary>
        /// b.length バイトのデータを出力ストリームに書き込みます。
        /// </summary>
        /// <param name="b"></param>
        public void write( byte[] b ) {
            base.Write( b, 0, b.Length );
        }

        /// <summary>
        /// 指定された byte 配列の、オフセット位置 off から始まる len バイトを出力ストリームに書き込みます。
        /// </summary>
        /// <param name="b"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        public void write( byte[] b, int off, int len ) {
            base.Write( b, off, len );
        }

        /// <summary>
        /// 指定された byte を出力ストリームに書き込みます。
        /// </summary>
        /// <param name="b"></param>
        public void write( int b ) {
            base.WriteByte( (byte)b );
        }
    }
          
    public class OutputStreamWriter {
        public StreamWriter m_writer;

        public OutputStreamWriter( FileOutputStream stream, String charsetName ) {
            Encoding enc = Encoding.GetEncoding( charsetName );
            if ( charsetName.ToLower().Equals( "utf-8" ) ) {
                enc = new System.Text.UTF8Encoding( false );
            }
            m_writer = new StreamWriter( stream, enc );
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
