/*
 * io.cs
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
using System.Collections.Generic;

namespace cadencii.java.io
{


    public interface OutputStream
    {
    }

    public interface InputStream
    {
        void close();
        int read(byte[] b);
        int read(byte[] b, int off, int len);
    }

    public class ObjectInputStream : InputStream
    {
        private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter m_formatter;
        private System.IO.Stream m_stream;

        public ObjectInputStream(System.IO.Stream stream)
        {
            m_stream = stream;
            m_formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        }

        public object readObject()
        {
            return m_formatter.Deserialize(m_stream);
        }

        public int read(byte[] b)
        {
            return m_stream.Read(b, 0, b.Length);
        }

        public int read(byte[] b, int off, int len)
        {
            return m_stream.Read(b, off, len);
        }

        public void close()
        {
            m_stream.Close();
        }
    }

    public class ObjectOutputStream : OutputStream
    {
        private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter m_formatter;
        private System.IO.Stream m_stream;

        public ObjectOutputStream(System.IO.Stream stream)
        {
            m_formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            m_stream = stream;
        }

        public void writeObject(Object obj)
        {
            m_formatter.Serialize(m_stream, obj);
        }
    }
}
