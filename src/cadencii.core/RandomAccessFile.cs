/*
 * RandomAccessFile.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii.java.io
{


    public class RandomAccessFile : InputStream
    {
        private System.IO.FileStream m_stream;

        public RandomAccessFile(string name, string mode)
        {
            if (mode == "r") {
                m_stream = new System.IO.FileStream(name, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            } else if (mode == "rw") {
                m_stream = new System.IO.FileStream(name, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
            } else {
                throw new ArgumentException("mode: \"" + mode + "\" is not supported", "mode");
            }
        }

        public void close()
        {
            m_stream.Close();
        }

        public long length()
        {
            return m_stream.Length;
        }

        public int read()
        {
            return m_stream.ReadByte();
        }

        public int read(byte[] b)
        {
            return m_stream.Read(b, 0, b.Length);
        }

        public int read(byte[] b, int off, int len)
        {
            return m_stream.Read(b, off, len);
        }

        public void seek(long pos)
        {
            m_stream.Seek(pos, System.IO.SeekOrigin.Begin);
        }

        public void write(byte[] b)
        {
            m_stream.Write(b, 0, b.Length);
        }

        public void write(byte[] b, int off, int len)
        {
            m_stream.Write(b, off, len);
        }

        public void write(int b)
        {
            m_stream.WriteByte((byte)b);
        }

        public void writeByte(int b)
        {
            write(b);
        }

        public long getFilePointer()
        {
            return m_stream.Position;
        }
    }

}
