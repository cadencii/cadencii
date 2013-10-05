/*
 * InternalStreamWriter.cs
 * Copyright © 2011 kbinani
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

namespace cadencii.vsq
{
    /// <summary>
    /// 改行コードに0x0d 0x0aを用いるテキストライター
    /// </summary>
    class InternalStreamWriter : ITextWriter
    {
        private string mNL = "\n";
        private System.IO.StreamWriter mStream;

        public InternalStreamWriter(string path, string encoding)
        {
            mNL = new string(new char[] { (char)0x0d, (char)0x0a });
            mStream = new System.IO.StreamWriter(path, false, System.Text.Encoding.GetEncoding(encoding));
        }

        public void write(string s)
        {
            mStream.Write(s);
        }

        public void writeLine(string s)
        {
            write(s);
            newLine();
        }

        public void newLine()
        {
            write(mNL);
        }

        public void close()
        {
            mStream.Close();
        }
    }

}
