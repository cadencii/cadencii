/*
 * BezierPoint.cs
 * Copyright (C) 2008-2010 kbinani
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
#else
using System;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// ベジエ曲線を構成するデータ点。
    /// </summary>
#if JAVA
    public class BezierPoint implements Comparable<BezierPoint>, Cloneable {
#else
    [Serializable]
    public class BezierPoint : IComparable<BezierPoint>, ICloneable {
#endif
        PointD center;
#if !JAVA
        [XmlIgnore]
#endif
        public PointD controlLeft;
#if !JAVA
        [XmlIgnore]
#endif
        public PointD controlRight;
        BezierControlType m_type_left;
        BezierControlType m_type_right;
#if !JAVA
        [NonSerialized]
#endif
        int m_id;

        public static boolean isXmlIgnored( String name ) {
            if ( name.Equals( "ID" ) ) {
                return true;
            }
            return false;
        }

        public int getID() {
            return m_id;
        }

        public void setID( int value ) {
            m_id = value;
        }

#if !JAVA
        public override string ToString() {
            return toString();
        }
#endif

        public String toString() {
            return "m_base=" + center.getX() + "," + center.getY() + "\n" +
                "m_control_left=" + controlLeft.getX() + "," + controlLeft.getY() + "\n" +
                "m_control_right=" + controlRight.getX() + "," + controlRight.getY() + "\n" +
                "m_type_left=" + m_type_left + "\n" +
                "m_type_right=" + m_type_right + "\n";
        }

        private BezierPoint() {
        }

#if JAVA
        public BezierPoint( PointD p1 ){
            this( p1.getX(), p1.getY() );
#else
        public BezierPoint( PointD p1 )
            : this( p1.getX(), p1.getY() ) {
#endif
        }

        public BezierPoint( double x, double y ) {
            PointD p = new PointD( x, y );
            center = p;
            controlLeft = p;
            controlRight = p;
            m_type_left = BezierControlType.None;
            m_type_right = BezierControlType.None;
        }

        public BezierPoint( PointD p1, PointD left, PointD right ) {
            center = p1;
            controlLeft = new PointD( left.getX() - center.getX(), left.getY() - center.getY() );
            controlRight = new PointD( right.getX() - center.getX(), right.getY() - center.getY() );
            m_type_left = BezierControlType.None;
            m_type_right = BezierControlType.None;
        }

        public Object clone() {
            BezierPoint result = new BezierPoint( this.getBase(), this.getControlLeft(), this.getControlRight() );
            result.controlLeft = this.controlLeft;
            result.controlRight = this.controlRight;
            result.m_type_left = this.m_type_left;
            result.m_type_right = this.m_type_right;
            result.m_id = this.m_id;
            return result;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

#if !JAVA
        public int CompareTo( BezierPoint item ) {
            return compareTo( item );
        }
#endif

        public int compareTo( BezierPoint item ) {
            double thisx = this.getBase().getX();
            double itemx = item.getBase().getX();
            if ( thisx > itemx ) {
                return 1;
            } else if ( thisx < itemx ) {
                return -1;
            } else {
                if ( this.getID() > item.getID() ) {
                    return 1;
                } else if ( this.getID() < item.getID() ) {
                    return -1;
                } else {
                    return 0;
                }
            }
        }

#if !JAVA
        /// <summary>
        /// XmlSerialize用
        /// </summary>
        public PointD Base {
            get {
                return getBase();
            }
            set {
                setBase( value );
            }
        }
#endif

        public PointD getBase() {
            return center;
        }

        public void setBase( PointD value ) {
            center = value;
        }

        public void setPosition( BezierPickedSide picked_side, PointD new_position ) {
            if ( picked_side == BezierPickedSide.BASE ) {
                this.setBase( new_position );
            } else if ( picked_side == BezierPickedSide.LEFT ) {
                this.controlLeft = new PointD( new_position.getX() - this.getBase().getX(), new_position.getY() - this.getBase().getY() );
            } else {
                this.controlRight = new PointD( new_position.getX() - this.getBase().getX(), new_position.getY() - this.getBase().getY() );
            }
        }

        public PointD getPosition( BezierPickedSide picked_side ) {
            if ( picked_side == BezierPickedSide.BASE ) {
                return this.getBase();
            } else if ( picked_side == BezierPickedSide.LEFT ) {
                return this.getControlLeft();
            } else {
                return this.getControlRight();
            }
        }

        public BezierControlType getControlType( BezierPickedSide picked_side ) {
            if ( picked_side == BezierPickedSide.LEFT ) {
                return this.getControlLeftType();
            } else if ( picked_side == BezierPickedSide.RIGHT ) {
                return this.getControlRightType();
            } else {
                return BezierControlType.None;
            }
        }

#if !JAVA
        /// <summary>
        /// XmlSerialize用
        /// </summary>
        public PointD ControlLeft{
            get {
                return getControlLeft();
            }
            set {
                setControlLeft( value );
            }
        }
#endif

        public PointD getControlLeft() {
            if ( m_type_left != BezierControlType.None ) {
                return new PointD( center.getX() + controlLeft.getX(), center.getY() + controlLeft.getY() );
            } else {
                return center;
            }
        }

        public void setControlLeft( PointD value ) {
            controlLeft = new PointD( value.getX() - center.getX(), value.getY() - center.getY() );
        }

#if !JAVA
        /// <summary>
        /// XmlSerialize用
        /// </summary>
        public PointD ControlRight {
            get {
                return getControlRight();
            }
            set {
                setControlRight( value );
            }
        }
#endif

        public PointD getControlRight() {
            if ( m_type_right != BezierControlType.None ) {
                return new PointD( center.getX() + controlRight.getX(), center.getY() + controlRight.getY() );
            } else {
                return center;
            }
        }

        public void setControlRight( PointD value ) {
            controlRight = new PointD( value.getX() - center.getX(), value.getY() - center.getY() );
        }

#if !JAVA
        /// <summary>
        /// XmlSerializer用
        /// </summary>
        public BezierControlType ControlLeftType {
            get {
                return getControlLeftType();
            }
            set {
                setControlLeftType( value );
            }
        }
#endif

        public BezierControlType getControlLeftType() {
            return m_type_left;
        }

        public void setControlLeftType( BezierControlType value ) {
            m_type_left = value;
            if ( m_type_left == BezierControlType.Master && m_type_right != BezierControlType.None ) {
                m_type_right = BezierControlType.Master;
            }
        }

#if !JAVA
        /// <summary>
        /// XmlSerializer用
        /// </summary>
        public BezierControlType ControlRightType {
            get {
                return getControlRightType();
            }
            set {
                setControlRightType( value );
            }
        }
#endif

        public BezierControlType getControlRightType() {
            return m_type_right;
        }

        public void setControlRightType( BezierControlType value ) {
            m_type_right = value;
            if ( m_type_right == BezierControlType.Master && m_type_left != BezierControlType.None ) {
                m_type_left = BezierControlType.Master;
            }
        }
    }

#if !JAVA
}
#endif
