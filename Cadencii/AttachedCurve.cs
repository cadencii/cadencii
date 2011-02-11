/*
 * AttachedCurve.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package org.kbinani.cadencii;

import java.util.*;
import org.kbinani.*;
import org.kbinani.xml.*;

#else

using System;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.cadencii
{
#endif

#if JAVA
    public class AttachedCurve implements Cloneable{
#else
    public class AttachedCurve : ICloneable {
#endif
        private Vector<BezierCurves> mCurves = new Vector<BezierCurves>();

#if !JAVA
        /// <summary>
        /// XML保存用
        /// </summary>
        public Vector<BezierCurves> Curves {
            get {
                return getCurves();
            }
            set {
                setCurves( value );
            }
        }
#endif

#if JAVA
        @XmlGenericType( BezierCurves.class )
#endif
        public Vector<BezierCurves> getCurves() {
            return mCurves;
        }

        public void setCurves( Vector<BezierCurves> value ) {
            mCurves = value;
        }

        public BezierCurves get( int index ) {
            return mCurves.get( index );
        }

        public void set( int index, BezierCurves value ) {
            mCurves.set( index, value );
        }

        public void add( BezierCurves item ) {
            mCurves.add( item );
        }

        public void removeElementAt( int index ) {
            mCurves.removeElementAt( index );
        }

        public void insertElementAt( int position, BezierCurves attached_curve ) {
            mCurves.insertElementAt( attached_curve, position );
        }

        public Object clone() {
            AttachedCurve ret = new AttachedCurve();
            ret.mCurves.clear();
            int c = mCurves.size();
            for ( int i = 0; i < c; i++ ) {
                ret.mCurves.add( (BezierCurves)mCurves.get( i ).clone() );
            }
            return ret;
        }

#if !JAVA
        public Object Clone() {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
