/*
 * SingerEventComparisonContext.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using cadencii.vsq;
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// 歌手変更イベントを比較するための，比較用コンテキスト．
    /// </summary>
    public class SingerEventComparisonContext : IComparisonContext
    {
        VsqTrack track1 = null;
        VsqTrack track2 = null;
        Iterator<int> it1 = null;
        Iterator<int> it2 = null;

        public SingerEventComparisonContext(VsqTrack track1, VsqTrack track2)
        {
            this.track1 = track1;
            this.track2 = track2;
            it1 = this.track1.indexIterator(IndexIteratorKind.SINGER);
            it2 = this.track2.indexIterator(IndexIteratorKind.SINGER);
        }

        public int getClockFrom(Object obj)
        {
            if (obj == null) {
                return 0;
            }
            if (obj is VsqEvent) {
                return ((VsqEvent)obj).Clock;
            }
            return 0;
        }

        public int getNextIndex1()
        {
            return it1.next();
        }

        public int getNextIndex2()
        {
            return it2.next();
        }

        public bool hasNext1()
        {
            return it1.hasNext();
        }

        public bool hasNext2()
        {
            return it2.hasNext();
        }

        public Object getElementAt1(int index)
        {
            return track1.getEvent(index);
        }

        public Object getElementAt2(int index)
        {
            return track2.getEvent(index);
        }

        public bool equals(Object obj1, Object obj2)
        {
            if (obj1 == null || obj2 == null) {
                return false;
            }
            if (!(obj1 is VsqEvent) || !(obj2 is VsqEvent)) {
                return false;
            }
            VsqEvent item1 = (VsqEvent)obj1;
            VsqEvent item2 = (VsqEvent)obj2;
            return (item1.ID.IconHandle.Program == item2.ID.IconHandle.Program);
        }
    }

}
