/*
 * BezierChain.cs
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
package org.kbinani.Cadencii;

import java.awt.*;
import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using bocoree;
using bocoree.util;
using bocoree.io;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class BezierChain implements Cloneable{
#else
    [Serializable]
    public class BezierChain : IDisposable, ICloneable {
#endif
        public Vector<BezierPoint> points;
        public double Default;
        public int id;
        private Color m_color;
        const double EPSILON = 1e-9;

        /// <summary>
        /// このベジエ曲線の開始位置を取得します。データ点が1つも無い場合はdouble.NaNを返します
        /// </summary>
        public double getStart() {
            if ( points.size() <= 0 ) {
                return Double.NaN;
            } else {
                return points.get( 0 ).getBase().getX();
            }
        }

        /// <summary>
        /// このベジエ曲線の終了位置を取得します。データ点が1つも無い場合はdouble.NaNを返します
        /// </summary>
        public double getEnd() {
            if ( points.size() <= 0 ) {
                return Double.NaN;
            } else {
                return points.get( points.size() - 1 ).getBase().getX();
            }
        }

        /// <summary>
        /// 4つの点X0, C0, C1, X1から構成されるベジエ曲線を、位置xで2つに分割することで出来る7個の新しい点の座標を計算します。
        /// X0, X1がデータ点、C0, C1が制御点となります。xがX0.X &lt; x &lt; X1.Xでない場合ArgumentOutOfRangeExceptionを投げます。
        /// </summary>
        /// <param name="X0"></param>
        /// <param name="C0"></param>
        /// <param name="C1"></param>
        /// <param name="X1"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static PointD[] cutUnitBezier( PointD X0, PointD C0, PointD C1, PointD X1, double x ) {
            if ( X0.getX() >= x || x >= X1.getX() ) {
                throw new ArgumentOutOfRangeException();
            }
            PointD[] ret = new PointD[7];
            for ( int i = 0; i < 7; i++ ) {
                ret[i] = new PointD();
            }
            ret[0].setX( X0.getX() );
            ret[0].setY( X0.getY() );
            ret[6].setX( X1.getX() );
            ret[6].setY( X1.getY() );

            double x1 = X0.getX();
            double x2 = C0.getX();
            double x3 = C1.getX();
            double x4 = X1.getX();
            double a3 = x4 - 3.0 * x3 + 3.0 * x2 - x1;
            double a2 = 3.0 * x3 - 6.0 * x2 + 3.0 * x1;
            double a1 = 3.0 * (x2 - x1);
            double a0 = x1;
            double t = solveCubicEquation( a3, a2, a1, a0, x );
            x1 = X0.getY();
            x2 = C0.getY();
            x3 = C1.getY();
            x4 = X1.getY();
            a3 = x4 - 3 * x3 + 3 * x2 - x1;
            a2 = 3 * x3 - 6 * x2 + 3 * x1;
            a1 = 3 * (x2 - x1);
            a0 = x1;
            ret[3].setX( x );
            ret[3].setY( ((a3 * t + a2) * t + a1) * t + a0 );
            ret[1] = getMidPoint( X0, C0, t );
            ret[5] = getMidPoint( C1, X1, t );
            PointD m = getMidPoint( C0, C1, t );
            ret[2] = getMidPoint( ret[1], m, t );
            ret[4] = getMidPoint( m, ret[5], t );
            return ret;
        }

        /// <summary>
        /// 点p0, p1を結ぶ線分をt : 1 - tに分割する点の座標を計算します
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static PointD getMidPoint( PointD p0, PointD p1, double t ) {
            double x = p0.getX() + (p1.getX() - p0.getX()) * t;
            double y = p0.getY() + (p1.getY() - p0.getY()) * t;
            return new PointD( x, y );
        }

        public BezierChain extractPartialBezier( double t_start, double t_end ) {
            if ( this.size() <= 1 ) {
                throw new ArgumentException( "chain must has two or more bezier points" );
            }
            double start = this.points.get( 0 ).getBase().getX();
            double end = this.points.get( this.size() - 1 ).getBase().getX();
            
            // [from, to]が、このベジエ曲線の範囲内にあるかどうかを検査
            if ( start > t_start || t_end > end ) {
                throw new ArgumentException( "no bezier point appeared in the range of \"from\" to \"to\"" );
            }

            // t_start, t_endが既存のベジエデータ点位置を被っていないかどうか検査しながらコピー
            boolean t_start_added = false; // 最初の区間が追加された直後だけ立つフラグ
            BezierChain edited = new BezierChain( m_color );
            int count = 0;
            for ( int i = 0; i < this.points.size() - 1; i++ ) {
                if ( this.points.get( i ).getBase().getX() < t_start && t_start < this.points.get( i + 1 ).getBase().getX() ) {
                    if ( this.points.get( i ).getBase().getX() < t_end && t_end < this.points.get( i + 1 ).getBase().getX() ) {
#if DEBUG
                        AppManager.debugWriteLine( "points[i].Base.X < t_start < t_end < points[i + 1].Base.X" );
#endif
                        PointD x0 = this.points.get( i ).getBase();
                        PointD x1 = this.points.get( i + 1 ).getBase();
                        PointD c0 = (this.points.get( i ).getControlRightType() == BezierControlType.None) ?
                                                x0 : this.points.get( i ).getControlRight();
                        PointD c1 = (this.points.get( i + 1 ).getControlLeftType() == BezierControlType.None) ?
                                                x1 : this.points.get( i + 1 ).getControlLeft();
                        PointD[] res = cutUnitBezier( x0, c0, c1, x1, t_start );
                        
                        x0 = res[3];
                        c0 = res[4];
                        c1 = res[5];
                        x1 = res[6];
                        res = cutUnitBezier( x0, c0, c1, x1, t_end );

                        BezierPoint left = new BezierPoint( res[0] );
                        BezierPoint right = new BezierPoint( res[3] );
                        left.setControlRight( res[1] );
                        right.setControlLeft( res[2] );
                        left.setControlRightType( this.points.get( i ).getControlRightType() );
                        right.setControlLeftType( this.points.get( i + 1 ).getControlLeftType() );
                        edited.add( left );
                        edited.add( right );
                        t_start_added = true;
                        break;
                    } else {
#if DEBUG
                        AppManager.debugWriteLine( "points[i].Base.X < t_start < points[i + 1].Base.X" );
#endif
                        PointD x0 = this.points.get( i ).getBase();
                        PointD x1 = this.points.get( i + 1 ).getBase();
                        PointD c0 = (this.points.get( i ).getControlRightType() == BezierControlType.None) ?
                                                x0 : this.points.get( i ).getControlRight();
                        PointD c1 = (this.points.get( i + 1 ).getControlLeftType() == BezierControlType.None) ?
                                                x1 : this.points.get( i + 1 ).getControlLeft();
                        PointD[] res = cutUnitBezier( x0, c0, c1, x1, t_start );

                        BezierPoint left = new BezierPoint( res[3] );
                        BezierPoint right = new BezierPoint( res[6] );

                        left.setControlRight( res[4] );
                        left.setControlRightType( this.points.get( i ).getControlRightType() );

                        right.setControlLeft( res[5] );
                        right.setControlRight( this.points.get( i + 1 ).getControlRight() );
                        right.setControlRightType( this.points.get( i + 1 ).getControlRightType() );
                        right.setControlLeftType( this.points.get( i + 1 ).getControlLeftType() );
                        edited.points.add( left );
                        count++;
                        edited.points.add( right );
                        count++;
                        t_start_added = true;
                    }
                }
                if ( t_start <= this.points.get( i ).getBase().getX() && this.points.get( i ).getBase().getX() <= t_end ) {
                    if ( !t_start_added ) {
                        edited.points.add( (BezierPoint)this.points.get( i ).Clone() );
                        count++;
                    } else {
                        t_start_added = false;
                    }
                }
                if ( this.points.get( i ).getBase().getX() < t_end && t_end < this.points.get( i + 1 ).getBase().getX() ) {
                    PointD x0 = this.points.get( i ).getBase();
                    PointD x1 = this.points.get( i + 1 ).getBase();
                    PointD c0 = (this.points.get( i ).getControlRightType() == BezierControlType.None) ?
                                            x0 : this.points.get( i ).getControlRight();
                    PointD c1 = (this.points.get( i + 1 ).getControlLeftType() == BezierControlType.None) ?
                                            x1 : this.points.get( i + 1 ).getControlLeft();
                    PointD[] res = cutUnitBezier( x0, c0, c1, x1, t_end );

                    edited.points.get( count - 1 ).setControlRight( res[1] );

                    BezierPoint right = new BezierPoint( res[3] );
                    right.setControlLeft( res[2] );
                    right.setControlLeftType( this.points.get( i + 1 ).getControlLeftType() );
                    edited.add( right );
                    count++;
                    break;
                }
            }

            if ( this.points.get( this.points.size() - 1 ).getBase().getX() == t_end && !t_start_added ) {
                edited.add( (BezierPoint)this.points.get( this.points.size() - 1 ).Clone() );
                count++;
            }

            for ( int i = 0; i < edited.size(); i++ ) {
                edited.points.get( i ).setID( i );
            }
            return edited;
        }

        /// <summary>
        /// 登録されているデータ点を消去します
        /// </summary>
        public void clear() {
            points.clear();
        }

        /// <summary>
        /// 与えられたBezierChainがx軸について陰かどうかを判定する
        /// </summary>
        /// <param name="chain"></param>
        /// <returns></returns>
        public static boolean isBezierImplicit( BezierChain chain ) {
            for ( int i = 0; i < chain.points.size() - 1; i++ ) {
                double pt1 = chain.points.get( i ).getBase().getX();
                double pt2 = (chain.points.get( i ).getControlRightType() == BezierControlType.None) ? pt1 : chain.points.get( i ).getControlRight().getX();
                double pt4 = chain.points.get( i + 1 ).getBase().getX();
                double pt3 = (chain.points.get( i + 1 ).getControlLeftType() == BezierControlType.None) ? pt4 : chain.points.get( i + 1 ).getControlLeft().getX();
                if ( !isUnitBezierImplicit( pt1, pt2, pt3, pt4 ) ) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 4つの制御点からなるベジエ曲線が、x軸について陰かどうかを判定する
        /// </summary>
        /// <param name="pt1">始点</param>
        /// <param name="pt2">制御点1</param>
        /// <param name="pt3">制御点2</param>
        /// <param name="pt4">終点</param>
        /// <returns></returns>
        private static boolean isUnitBezierImplicit( double pt1, double pt2, double pt3, double pt4 ) {
            double a = pt4 - 3.0 * pt3 + 3.0 * pt2 - pt1;
            double b = 2.0 * pt3 - 4.0 * pt2 + 2.0 * pt1;
            double c = pt2 - pt1;
            if ( a == 0.0f ) {
                if ( c >= 0.0f && b + c >= 0.0f ) {
                    return true;
                } else {
                    return false;
                }
            } else if ( a > 0.0f ) {
                if ( -b / (2.0f * a) <= 0.0f ) {
                    if ( c >= 0.0f ) {
                        return true;
                    } else {
                        return false;
                    }
                } else if ( 1.0f <= -b / (2.0f * a) ) {
                    if ( a + b + c >= 0.0f ) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    if ( c - b * b / (4.0f * a) >= 0.0f ) {
                        return true;
                    } else {
                        return false;
                    }
                }
            } else {
                if ( -b / (2.0f * a) <= 0.5f ) {
                    if ( a + b + c >= 0.0f ) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    if ( c >= 0.0f ) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }

#if !JAVA
        [OnDeserialized]
        private void onDeserialized( StreamingContext sc ) {
            for ( int i = 0; i < points.size(); i++ ) {
                points.get( i ).setID( i );
            }
        }
#endif

        public void Dispose() {
            if ( points != null ) {
                points.clear();
            }
        }

        public int getNextId() {
            if ( points.size() > 0 ) {
                int max = points.get( 0 ).getID();
                for ( int i = 1; i < points.size(); i++ ) {
                    max = Math.Max( max, points.get( i ).getID() );
                }
                return max + 1;
            } else {
                return 0;
            }
        }
        
        public void getValueMinMax( ByRef<Double> min, ByRef<Double> max ) {
            //todo: ベジエが有効なときに、曲線の描く最大値、最小値も考慮
            min.value = Default;
            max.value = Default;
            for ( Iterator itr = points.iterator(); itr.hasNext(); ){
                BezierPoint bp = (BezierPoint)itr.next();
                min.value = Math.Min( min.value, bp.getBase().getY() );
                max.value = Math.Max( max.value, bp.getBase().getY() );
            }
        }

        public void getKeyMinMax( ByRef<Double> min, ByRef<Double> max ) {
            min.value = Default;
            max.value = Default;
            for ( Iterator itr = points.iterator(); itr.hasNext(); ){
                BezierPoint bp = (BezierPoint)itr.next();
                min.value = Math.Min( min.value, bp.getBase().getX() );
                max.value = Math.Max( max.value, bp.getBase().getX() );
            }
        }
        
        public Object clone() {
            BezierChain result = new BezierChain( this.m_color );
            for ( Iterator itr = points.iterator(); itr.hasNext(); ){
                BezierPoint bp = (BezierPoint)itr.next();
                result.points.add( (BezierPoint)bp.clone() );
            }
            result.Default = this.Default;
            result.id = id;
            return result;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public BezierChain( Color curve ) {
            points = new Vector<BezierPoint>();
            m_color = curve;
        }

        public BezierChain() {
            points = new Vector<BezierPoint>();
            m_color = Color.Black;
        }

#if !JAVA
        /*public Color Color {
            get {
                return getColor();
            }
            set {
                setColor( value );
            }
        }*/
#endif

        public Color getColor() {
            return m_color;
        }

        public void setColor( Color value ) {
            m_color = value;
        }

        public void add( BezierPoint bp ) {
            if ( points == null ) {
                points = new Vector<BezierPoint>();
                m_color = Color.Black;
            }
            points.add( bp );
            Collections.sort( points );
        }

        public void removeElementAt( int id_ ) {
            for ( int i = 0; i < points.size(); i++ ) {
                if ( points.get( i ).getID() == id_ ) {
                    points.removeElementAt( i );
                    break;
                }
            }
        }

        public int size() {
            if ( points == null ) {
                return 0;
            }
            return points.size();
        }

        public double getValue( double x ) {
            int count = points.size();
            for ( int i = 0; i < count - 1; i++ ) {
                BezierPoint bpi = points.get( i );
                BezierPoint bpi1 = points.get( i + 1 );
                if ( bpi.getBase().getX() <= x && x <= bpi1.getBase().getX() ) {
                    double x1 = bpi.getBase().getX();
                    double x4 = bpi1.getBase().getX();
                    if ( x1 == x ) {
                        return bpi.getBase().getY();
                    } else if ( x4 == x ) {
                        return bpi1.getBase().getY();
                    } else {
                        double x2 = bpi.getControlRight().getX();
                        double x3 = bpi1.getControlLeft().getX();
                        double a3 = x4 - 3 * x3 + 3 * x2 - x1;
                        double a2 = 3 * x3 - 6 * x2 + 3 * x1;
                        double a1 = 3 * (x2 - x1);
                        double a0 = x1;
                        double t = solveCubicEquation( a3, a2, a1, a0, x );
                        x1 = bpi.getBase().getY();
                        x2 = bpi.getControlRight().getY();
                        x3 = bpi1.getControlLeft().getY();
                        x4 = bpi1.getBase().getY();
                        a3 = x4 - 3 * x3 + 3 * x2 - x1;
                        a2 = 3 * x3 - 6 * x2 + 3 * x1;
                        a1 = 3 * (x2 - x1);
                        a0 = x1;
                        return ((a3 * t + a2) * t + a1) * t + a0;
                    }
                }
            }
            return Default;
        }

        /// <summary>
        /// 3次方程式a3*x^3 + a2*x^2 + a1*x + a0 = ansの解をニュートン法を使って計算します。ただし、単調増加である必要がある。
        /// </summary>
        /// <param name="a3"></param>
        /// <param name="a2"></param>
        /// <param name="a1"></param>
        /// <param name="a0"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        private static double solveCubicEquation( double a3, double a2, double a1, double a0, double ans ) {
            double suggested_t = 0.5;
            double a3_3 = a3 * 3.0;
            double a2_2 = a2 * 2.0;
            while ( (a3_3 * suggested_t + a2_2) * suggested_t + a1 == 0.0 ) {
                suggested_t += 0.1;
            }
            double x = suggested_t;
            double new_x = suggested_t;
            for ( int i = 0; i < 5000; i++ ) {
                new_x = x - (((a3 * x + a2) * x + a1) * x + a0 - ans) / ((a3_3 * x + a2_2) * x + a1);
                if ( Math.Abs( new_x - x ) < EPSILON * new_x ) {
                    break;
                }
                x = new_x;
            }
            return new_x;
        }
    }

#if !JAVA
}
#endif
