/*
 * IndexIteratorKind.cs
 * Copyright © 2010-2011 kbinani
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
namespace cadencii.vsq
{

    public class IndexIteratorKind
    {
        public const int SINGER = 1 << 0;
        public const int NOTE = 1 << 1;
        public const int CRESCEND = 1 << 2;
        public const int DECRESCEND = 1 << 3;
        public const int DYNAFF = 1 << 4;

        private IndexIteratorKind()
        {
        }
    }

}
