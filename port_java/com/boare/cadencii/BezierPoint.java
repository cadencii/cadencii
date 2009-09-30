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
package com.boare.cadencii;

import java.awt.*;


/// <summary>
/// ベジエ曲線を構成するデータ点。
/// </summary>
public class BezierPoint implements Comparable<BezierPoint>, Cloneable {
    PointD m_base;
    public PointD internalControlLeft;
    public PointD internalControlRight;
    BezierControlType m_type_left;
    BezierControlType m_type_right;
    ValueType m_type;
    int m_id;

    private enum ValueType {
        int_,
        float_,
        double_,
    }

    public int getID(){
        return m_id;
    }

    /**
     * IDのsetter。XmlSerializerによるシリアライズを回避するため、名前がsetIDとなっていない点に注意。
     */
    public void setIDInternal( int value ){
        m_id = value;
    }

    public String toString() {
        return "m_base=" + m_base.X + "," + m_base.Y + "\n" +
            "internalControlLeft=" + internalControlLeft.X + "," + internalControlLeft.Y + "\n" +
            "internalControlRight=" + internalControlRight.X + "," + internalControlRight.Y + "\n" +
            "m_type_left=" + m_type_left + "\n" +
            "m_type_right=" + m_type_right + "\n";
    }

    private BezierPoint() {
    }

    public BezierPoint( PointD p1 ){
        this( p1.X, p1.Y );
    }

    public BezierPoint( double x, double y ) {
        PointD p = new PointD( x, y );
        m_base = p;
        internalControlLeft = p;
        internalControlRight = p;
        m_type_left = BezierControlType.None;
        m_type_right = BezierControlType.None;
    }

    public BezierPoint( PointD p1, PointD left, PointD right ) {
        m_base = p1;
        internalControlLeft = new PointD( left.X - m_base.X, left.Y - m_base.Y );
        internalControlRight = new PointD( right.X - m_base.X, right.Y - m_base.Y );
        m_type_left = BezierControlType.None;
        m_type_right = BezierControlType.None;
    }

    public Object clone() {
        BezierPoint result = new BezierPoint( this.getBase(), this.getControlLeft(), this.getControlRight() );
        result.internalControlLeft = this.internalControlLeft;
        result.internalControlRight = this.internalControlRight;
        result.m_type_left = this.m_type_left;
        result.m_type_right = this.m_type_right;
        result.m_id = this.m_id;
        return result;
    }

    public int compareTo( BezierPoint item ) {
        double thisx = this.getBase().X;
        double itemx = item.getBase().X;
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

    public PointD getBase(){
        return m_base;
    }

    public void setBase( PointD value ){
        m_base = value;
    }

    public void setPosition( BezierPickedSide picked_side, PointD new_position ) {
        if ( picked_side == BezierPickedSide.Base ) {
            this.m_base = new_position;
        } else if ( picked_side == BezierPickedSide.Left ) {
            this.internalControlLeft = new PointD( new_position.X - this.m_base.X, new_position.Y - this.m_base.Y );
        } else {
            this.internalControlRight = new PointD( new_position.X - this.m_base.X, new_position.Y - this.m_base.Y );
        }
    }

    public PointD getPosition( BezierPickedSide picked_side ) {
        if ( picked_side == BezierPickedSide.Base ) {
            return this.m_base;
        } else if ( picked_side == BezierPickedSide.Left ) {
            return this.getControlLeft();
        } else {
            return this.getControlRight();
        }
    }

    public BezierControlType getControlType( BezierPickedSide picked_side ) {
        if ( picked_side == BezierPickedSide.Left ) {
            return this.getControlLeftType();
        } else if ( picked_side == BezierPickedSide.Right ) {
            return this.getControlRightType();
        } else {
            return BezierControlType.None;
        }
    }

    public PointD getControlLeft(){
        if ( m_type_left != BezierControlType.None ) {
            return new PointD( m_base.X + internalControlLeft.X, m_base.Y + internalControlLeft.Y );
        } else {
            return m_base;
        }
    }

    public void setControlLeft( PointD value ){
        internalControlLeft = new PointD( value.X - m_base.X, value.Y - m_base.Y );
    }

    public PointD getControlRight(){
        if ( m_type_right != BezierControlType.None ) {
            return new PointD( m_base.X + internalControlRight.X, m_base.Y + internalControlRight.Y );
        } else {
            return m_base;
        }
    }

    public void setControlRight( PointD value ){
        internalControlRight = new PointD( value.X - m_base.X, value.Y - m_base.Y );
    }

    public BezierControlType getControlLeftType(){
        return m_type_left;
    }

    public void setControlLeftType( BezierControlType value ){
        m_type_left = value;
        if ( m_type_left == BezierControlType.Master && m_type_right != BezierControlType.None ) {
            m_type_right = BezierControlType.Master;
        }
    }

    public BezierControlType getControlRightType(){
        return m_type_right;
    }

    public void setControlRightType( BezierControlType value ){
        m_type_right = value;
        if ( m_type_right == BezierControlType.Master && m_type_left != BezierControlType.None ) {
            m_type_left = BezierControlType.Master;
        }
    }
}
