/*
 * TextStream.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Text;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{

    public class TextStream : ITextWriter, IDisposable
    {
        const int INIT_BUFLEN = 512;

        private char[] array = new char[INIT_BUFLEN];
        private int length = 0;
        private int position = -1;

        public int getPointer()
        {
            return position;
        }

        public void setPointer(int value)
        {
            position = value;
        }

        public char get()
        {
            position++;
            return array[position];
        }

        public string readLine()
        {
            StringBuilder sb = new StringBuilder();
            // '\n'が来るまで読み込み
            position++;
            for (; position < length; position++) {
                char c = array[position];
                if (c == '\n') {
                    break;
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        public bool ready()
        {
            if (0 <= position + 1 && position + 1 < length) {
                return true;
            } else {
                return false;
            }
        }

        private void ensureCapacity(int length)
        {
            if (length > array.Length) {
                int newLength = length;
                if (this.length <= 0) {
                    newLength = (length * 3) >> 1;
                } else {
                    int order = length / array.Length;
                    if (order <= 1) {
                        order = 2;
                    }
                    newLength = array.Length * order;
                }
                Array.Resize(ref array, newLength);
            }
        }

        public void write(string str)
        {
            int len = PortUtil.getStringLength(str);
            int newSize = length + len;
            int offset = length;
            ensureCapacity(newSize);
            for (int i = 0; i < len; i++) {
                array[offset + i] = str[i];
            }
            length = newSize;
        }

        public void writeLine(string str)
        {
            write(str);
            newLine();
        }

        public void newLine()
        {
            int new_size = length + 1;
            int offset = length;
            ensureCapacity(new_size);
            array[offset] = '\n';
            length = new_size;
        }

        public void close()
        {
            array = null;
            length = 0;
        }

        public void Dispose()
        {
            close();
        }
    }

}
