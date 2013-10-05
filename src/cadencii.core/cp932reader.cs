/*
 * cp932reader.cs
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
using System.IO;
using System.Text;

namespace cadencii
{

    public class cp932reader : IDisposable
    {
        private Stream m_stream;


        private cp932reader()
        {
        }

        public cp932reader(Stream stream)
            : this()
        {
            m_stream = stream;
        }

        public cp932reader(string path)
            : this()
        {
            m_stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public string ReadLine()
        {
            byte[] line;
            if (get_line(out line)) {
                return cp932.convert(line);
            } else {
                return null;
            }
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

        public int Peek()
        {
            int ch = m_stream.ReadByte();
            if (ch < 0) {
                return ch;
            } else {
                m_stream.Seek(-1, SeekOrigin.Current);
                return ch;
            }
        }

        public string ReadToEnd()
        {
            StringBuilder sb = new StringBuilder();
            while (Peek() >= 0) {
                sb.Append(ReadLine() + Environment.NewLine);
            }
            return sb.ToString();
        }

        private bool get_line(out byte[] line)
        {
            List<byte> ret = new List<byte>();
            if (!m_stream.CanRead) {
                line = ret.ToArray();
                return false;
            }
            if (!m_stream.CanSeek) {
                line = ret.ToArray();
                return false;
            }
            int ch = m_stream.ReadByte();
            if (ch < 0) {
                line = ret.ToArray();
                return false;
            }
            while (ch >= 0) {
                if (ch == 0x0d) {
                    ch = m_stream.ReadByte();
                    if (ch < 0) {
                        break;
                    } else if (ch != 0x0a) {
                        m_stream.Seek(-1, SeekOrigin.Current);
                    }
                    break;
                } else if (ch == 0x0a) {
                    break;
                }
                ret.Add((byte)ch);
                ch = m_stream.ReadByte();
            }
            line = ret.ToArray();
            return true;
        }
    }

}
