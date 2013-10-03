/*
 * TextReaderUtil.cs
 * Copyright © 2013 kbinani
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
using System.Collections.Generic;

namespace cadencii
{
    public static class TextReaderUtil
    {
        public static IEnumerable<string> Lines(this TextReader reader)
        {
            return new TextReaderLineIterator(reader);
        }

        public static void EachLines(this TextReader reader, Action<string> action)
        {
            var iterator = new TextReaderLineIterator(reader);
            foreach (var line in iterator) {
                action(line);
            }
        }
    }

    class TextReaderLineIterator : IEnumerator<string>, IEnumerable<string>
    {
        private TextReader reader_;
        private string current_;

        public TextReaderLineIterator(TextReader reader) { reader_ = reader; }

        public string Current { get { return current_; } }

        object System.Collections.IEnumerator.Current { get { return current_; } }

        public void Reset() { }

        public bool MoveNext()
        {
            try {
                current_ = reader_.ReadLine();
                return current_ != null;
            } catch {
                return false;
            }
        }

        public void Dispose() { }

        public IEnumerator<string> GetEnumerator() { return this; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return this; }
    }
}
