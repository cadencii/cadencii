/*
 * cp932writer.cs
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
using System.IO;
using System.Text;

namespace cadencii
{

    public class cp932writer : IDisposable
    {
        Stream m_stream;
        byte[] m_newline;

        private cp932writer()
        {
            m_newline = Encoding.ASCII.GetBytes(Environment.NewLine);
        }

        public cp932writer(Stream stream)
            : this()
        {
            m_stream = stream;
        }

        public cp932writer(string file)
            : this()
        {
            m_stream = new FileStream(file, FileMode.Create);
        }

        public void WriteLine(string line)
        {
            byte[] bytes = cp932.convert(line);
            m_stream.Write(bytes, 0, bytes.Length);
            m_stream.Write(m_newline, 0, m_newline.Length);
        }

        public void Write(string line)
        {
            byte[] bytes = cp932.convert(line);
            m_stream.Write(bytes, 0, bytes.Length);
        }

        public void Close()
        {
            if (m_stream != null) {
                m_stream.Close();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }

}
