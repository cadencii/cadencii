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
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    public class AttachedCurve : ICloneable {
        private Vector<BezierCurves> m_curves = new Vector<BezierCurves>();

        /// <summary>
        /// XML保存用
        /// </summary>
        public Vector<BezierCurves> Curves {
            get {
                return m_curves;
            }
            set {
                m_curves = value;
            }
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
            ret.Curves.clear();
            for ( int i = 0; i < Curves.size(); i++ ) {
                ret.Curves.add( (BezierCurves)Curves.get( i ).Clone() );
            }
            return ret;
        }

        public object Clone() {
            return clone();
        }
    }

}
