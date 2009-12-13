/*
 * AttachedCurve.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.util.*;
import org.kbinani.*;
#else
using System;
using bocoree;
using bocoree.java.util;
using bocoree.java.io;

namespace Boare.Cadencii {
#endif

#if JAVA
    public class AttachedCurve implements Cloneable{
#else
    public class AttachedCurve : ICloneable {
#endif
        private Vector<BezierCurves> m_curves = new Vector<BezierCurves>();

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

        public Vector<BezierCurves> getCurves() {
            return m_curves;
        }

        public void setCurves( Vector<BezierCurves> value ) {
            m_curves = value;
        }

        public BezierCurves get( int index ) {
            return m_curves.get( index );
        }

        public void set( int index, BezierCurves value ) {
            m_curves.set( index, value );
        }

        public void add( BezierCurves item ) {
            m_curves.add( item );
        }

        public void removeElementAt( int index ) {
            m_curves.removeElementAt( index );
        }

        public void insertElementAt( int position, BezierCurves attached_curve ) {
            m_curves.insertElementAt( attached_curve, position );
        }

        public Object clone() {
            AttachedCurve ret = new AttachedCurve();
            ret.m_curves.clear();
            int c = m_curves.size();
            for ( int i = 0; i < c; i++ ) {
                ret.m_curves.add( (BezierCurves)m_curves.get( i ).clone() );
            }
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
