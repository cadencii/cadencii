/*
 * BezierPoint.cs
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
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Boare.Cadencii {

    /// <summary>
    /// ベジエ曲線を構成するデータ点。
    /// </summary>
    [Serializable]
    public class BezierPoint : IComparable<BezierPoint>, ICloneable {
        PointD m_base;
        [XmlIgnore]
        public PointD controlLeft;
        [XmlIgnore]
        public PointD controlRight;
        BezierControlType m_type_left;
        BezierControlType m_type_right;
        [NonSerialized]
        int m_id;

        [XmlIgnore]
        public int ID {
            get {
                return m_id;
            }
            internal set {
                m_id = value;
            }
        }

        public override string ToString() {
            return toString();
        }

        public String toString() {
            return "m_base=" + m_base.X + "," + m_base.Y + "\n" +
                "m_control_left=" + controlLeft.X + "," + controlLeft.Y + "\n" +
                "m_control_right=" + controlRight.X + "," + controlRight.Y + "\n" +
                "m_type_left=" + m_type_left + "\n" +
                "m_type_right=" + m_type_right + "\n";
        }

        private BezierPoint() {
        }

        public BezierPoint( PointD p1 ) : this( p1.X, p1.Y ) {
        }

        public BezierPoint( double x, double y ) {
            PointD p = new PointD( x, y );
            m_base = p;
            controlLeft = p;
            controlRight = p;
            m_type_left = BezierControlType.None;
            m_type_right = BezierControlType.None;
        }

        public BezierPoint( PointD p1, PointD left, PointD right ) {
            m_base = p1;
            controlLeft = new PointD( left.X - m_base.X, left.Y - m_base.Y );
            controlRight = new PointD( right.X - m_base.X, right.Y - m_base.Y );
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

        public object Clone() {
            return clone();
        }

        public int CompareTo( BezierPoint item ) {
            double thisx = this.getBase().X;
            double itemx = item.getBase().X;
            if ( thisx > itemx ) {
                return 1;
            } else if ( thisx < itemx ) {
                return -1;
            } else {
                if ( this.ID > item.ID ) {
                    return 1;
                } else if ( this.ID < item.ID ) {
                    return -1;
                } else {
                    return 0;
                }
            }
        }

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

        public PointD getBase() {
            return m_base;
        }

        public void setBase( PointD value ) {
            m_base = value;
        }

        public void setPosition( BezierPickedSide picked_side, PointD new_position ) {
            if ( picked_side == BezierPickedSide.BASE ) {
                this.setBase( new_position );
            } else if ( picked_side == BezierPickedSide.LEFT ) {
                this.controlLeft = new PointD( new_position.X - this.getBase().X, new_position.Y - this.getBase().Y );
            } else {
                this.controlRight = new PointD( new_position.X - this.getBase().X, new_position.Y - this.getBase().Y );
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

        public PointD getControlLeft() {
            if ( m_type_left != BezierControlType.None ) {
                return new PointD( m_base.X + controlLeft.X, m_base.Y + controlLeft.Y );
            } else {
                return m_base;
            }
        }

        public void setControlLeft( PointD value ) {
            controlLeft = new PointD( value.X - m_base.X, value.Y - m_base.Y );
        }

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

        public PointD getControlRight() {
            if ( m_type_right != BezierControlType.None ) {
                return new PointD( m_base.X + controlRight.X, m_base.Y + controlRight.Y );
            } else {
                return m_base;
            }
        }

        public void setControlRight( PointD value ) {
            controlRight = new PointD( value.X - m_base.X, value.Y - m_base.Y );
        }

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

        public BezierControlType getControlLeftType() {
            return m_type_left;
        }

        public void setControlLeftType( BezierControlType value ) {
            m_type_left = value;
            if ( m_type_left == BezierControlType.Master && m_type_right != BezierControlType.None ) {
                m_type_right = BezierControlType.Master;
            }
        }

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

}
