/*
 * AttachedCurve.cs
 * Copyright © 2008-2011 kbinani
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
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii
{

    public class AttachedCurve : ICloneable
    {
        private List<BezierCurves> mCurves = new List<BezierCurves>();

        /// <summary>
        /// XML保存用
        /// </summary>
        public List<BezierCurves> Curves
        {
            get
            {
                return getCurves();
            }
            set
            {
                setCurves(value);
            }
        }

        public List<BezierCurves> getCurves()
        {
            return mCurves;
        }

        public void setCurves(List<BezierCurves> value)
        {
            mCurves = value;
        }

        public BezierCurves get(int index)
        {
            return mCurves[index];
        }

        public void set(int index, BezierCurves value)
        {
            mCurves[index] = value;
        }

        public void add(BezierCurves item)
        {
            mCurves.Add(item);
        }

        public void removeElementAt(int index)
        {
            mCurves.RemoveAt(index);
        }

        public void insertElementAt(int position, BezierCurves attached_curve)
        {
            mCurves.Insert(position, attached_curve);
        }

        public Object clone()
        {
            AttachedCurve ret = new AttachedCurve();
            ret.mCurves.Clear();
            int c = mCurves.Count;
            for (int i = 0; i < c; i++) {
                ret.mCurves.Add((BezierCurves)mCurves[i].clone());
            }
            return ret;
        }

        public Object Clone()
        {
            return clone();
        }
    }

}
