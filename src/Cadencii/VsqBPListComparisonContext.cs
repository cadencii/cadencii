/*
 * VsqBPListComparisonContext.cs
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

namespace cadencii
{

    /// <summary>
    /// VsqBPListを比較するための，比較用コンテキスト
    /// </summary>
    public class VsqBPListComparisonContext : IComparisonContext
    {
        public class WrappedVsqBPPair
        {
            public VsqBPPair body;
            public int clock;

            public WrappedVsqBPPair(VsqBPPair body, int clock)
            {
                this.body = body;
                this.clock = clock;
            }
        }

        VsqBPList list1 = null;
        VsqBPList list2 = null;
        int pos1 = -1;
        int pos2 = -1;

        public VsqBPListComparisonContext(VsqBPList list1, VsqBPList list2)
        {
            // インデクス0の位置に，デフォルト値を持つダミーがあるように振舞わせる
            this.list1 = list1;
            this.list2 = list2;
        }

        public int getNextIndex1()
        {
            pos1++;
            return pos1;
        }

        public int getNextIndex2()
        {
            pos2++;
            return pos2;
        }

        public bool hasNext1()
        {
            return (pos1 < list1.size());
        }

        public bool hasNext2()
        {
            return (pos2 < list2.size());
        }

        public Object getElementAt1(int index)
        {
            if (index <= 0) {
                return new WrappedVsqBPPair(new VsqBPPair(list1.getDefault(), -1), 0);
            } else {
                return new WrappedVsqBPPair(list1.getElementB(index - 1), list1.getKeyClock(index - 1));
            }
        }

        public Object getElementAt2(int index)
        {
            if (index <= 0) {
                return new WrappedVsqBPPair(new VsqBPPair(list2.getDefault(), -1), 0);
            } else {
                return new WrappedVsqBPPair(list2.getElementB(index - 1), list2.getKeyClock(index - 1));
            }
        }

        public int getClockFrom(Object obj)
        {
            if (obj == null) {
                return 0;
            }
            if (!(obj is WrappedVsqBPPair)) {
                return 0;
            }
            return ((WrappedVsqBPPair)obj).clock;
        }

        public bool equals(Object obj1, Object obj2)
        {
            if (obj1 == null || obj2 == null) {
                return false;
            }
            if (!(obj1 is WrappedVsqBPPair) || !(obj2 is WrappedVsqBPPair)) {
                return false;
            }
            WrappedVsqBPPair item1 = (WrappedVsqBPPair)obj1;
            WrappedVsqBPPair item2 = (WrappedVsqBPPair)obj2;
            return (item1.body.value == item2.body.value);
        }
    }

}
