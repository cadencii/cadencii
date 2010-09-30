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
#if !JAVA
        [XmlIgnore]
#endif
        public PointD controlLeft;
#if !JAVA
        [XmlIgnore]
#endif
        public PointD controlRight;

#if !JAVA
        [NonSerialized]
#endif
        private int mID;
        private PointD mCenter;
        private BezierControlType mTypeLeft;
        private BezierControlType mTypeRight;

        /// <summary>
        /// このクラスの指定した名前のプロパティを，XMLシリアライズ時に無視するかどうかを表す
        /// ブール値を返します．デフォルトの実装では戻り値は全てfalseです．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static boolean isXmlIgnored( String name ) {
            if ( name != null ) {
                if ( name.Equals( "ID" ) ) {
                    return true;
                } else if ( name.Equals( "controlLeft" ) ) {
                    return true;
                } else if ( name.Equals( "controlRight" ) ) {
                    return true;
                }
            }
            return false;
        }

        public int getID() {
            return mID;
        }

        public void setID( int value ) {
            mID = value;
        }

#if !JAVA
        public override string ToString() {
            return toString();
        }
#endif

        public String toString() {
            return "m_base=" + mCenter.getX() + "," + mCenter.getY() + "\n" +
                "m_control_left=" + controlLeft.getX() + "," + controlLeft.getY() + "\n" +
                "m_control_right=" + controlRight.getX() + "," + controlRight.getY() + "\n" +
                "m_type_left=" + mTypeLeft + "\n" +
                "m_type_right=" + mTypeRight + "\n";
        }

#if JAVA
        public BezierPoint(){
        }
#else
        private BezierPoint() {
        }
#endif

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
            mCenter = p;
            controlLeft = p;
            controlRight = p;
            mTypeLeft = BezierControlType.None;
            mTypeRight = BezierControlType.None;
        }

        public BezierPoint( PointD p1, PointD left, PointD right ) {
            mCenter = p1;
            controlLeft = new PointD( left.getX() - mCenter.getX(), left.getY() - mCenter.getY() );
            controlRight = new PointD( right.getX() - mCenter.getX(), right.getY() - mCenter.getY() );
            mTypeLeft = BezierControlType.None;
            mTypeRight = BezierControlType.None;
        }

        public Object clone() {
            BezierPoint result = new BezierPoint( this.getBase(), this.getControlLeft(), this.getControlRight() );
            result.controlLeft = this.controlLeft;
            result.controlRight = this.controlRight;
            result.mTypeLeft = this.mTypeLeft;
            result.mTypeRight = this.mTypeRight;
            result.mID = this.mID;
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
            return mCenter;
        }

        public void setBase( PointD value ) {
            mCenter = value;
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
            if ( mTypeLeft != BezierControlType.None ) {
                return new PointD( mCenter.getX() + controlLeft.getX(), mCenter.getY() + controlLeft.getY() );
            } else {
                return mCenter;
            }
        }

        public void setControlLeft( PointD value ) {
            controlLeft = new PointD( value.getX() - mCenter.getX(), value.getY() - mCenter.getY() );
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
            if ( mTypeRight != BezierControlType.None ) {
                return new PointD( mCenter.getX() + controlRight.getX(), mCenter.getY() + controlRight.getY() );
            } else {
                return mCenter;
            }
        }

        public void setControlRight( PointD value ) {
            controlRight = new PointD( value.getX() - mCenter.getX(), value.getY() - mCenter.getY() );
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
            return mTypeLeft;
        }

        public void setControlLeftType( BezierControlType value ) {
            mTypeLeft = value;
            if ( mTypeLeft == BezierControlType.Master && mTypeRight != BezierControlType.None ) {
                mTypeRight = BezierControlType.Master;
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
            return mTypeRight;
        }

        public void setControlRightType( BezierControlType value ) {
            mTypeRight = value;
            if ( mTypeRight == BezierControlType.Master && mTypeLeft != BezierControlType.None ) {
                mTypeLeft = BezierControlType.Master;
            }
        }
    }

#if !JAVA
}
#endif
