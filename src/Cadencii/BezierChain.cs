/*
 * BezierChain.cs
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
using System.Runtime.Serialization;
using System.Collections.Generic;
using cadencii.java.awt;
using cadencii.java.util;

namespace cadencii
{

    [Serializable]
    public class BezierChain : IDisposable, ICloneable
    {
        public List<BezierPoint> points;
        public double Default;
        public int id;
        private Color mColor;
        const double EPSILON = 1e-9;

        /// <summary>
        /// このベジエ曲線の開始位置を取得します。データ点が1つも無い場合はdouble.NaNを返します
        /// </summary>
        public double getStart()
        {
            if (points.Count <= 0) {
                return Double.NaN;
            } else {
                return points[0].getBase().getX();
            }
        }

        /// <summary>
        /// このベジエ曲線の終了位置を取得します。データ点が1つも無い場合はdouble.NaNを返します
        /// </summary>
        public double getEnd()
        {
            if (points.Count <= 0) {
                return Double.NaN;
            } else {
                return points[points.Count - 1].getBase().getX();
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
        public static PointD[] cutUnitBezier(PointD X0, PointD C0, PointD C1, PointD X1, double x)
        {
            if (X0.getX() >= x || x >= X1.getX()) {
                return null;
            }
            PointD[] ret = new PointD[7];
            for (int i = 0; i < 7; i++) {
                ret[i] = new PointD();
            }
            ret[0].setX(X0.getX());
            ret[0].setY(X0.getY());
            ret[6].setX(X1.getX());
            ret[6].setY(X1.getY());

            double x1 = X0.getX();
            double x2 = C0.getX();
            double x3 = C1.getX();
            double x4 = X1.getX();
            double a3 = x4 - 3.0 * x3 + 3.0 * x2 - x1;
            double a2 = 3.0 * x3 - 6.0 * x2 + 3.0 * x1;
            double a1 = 3.0 * (x2 - x1);
            double a0 = x1;
            double t = solveCubicEquation(a3, a2, a1, a0, x);
            x1 = X0.getY();
            x2 = C0.getY();
            x3 = C1.getY();
            x4 = X1.getY();
            a3 = x4 - 3 * x3 + 3 * x2 - x1;
            a2 = 3 * x3 - 6 * x2 + 3 * x1;
            a1 = 3 * (x2 - x1);
            a0 = x1;
            ret[3].setX(x);
            ret[3].setY(((a3 * t + a2) * t + a1) * t + a0);
            ret[1] = getMidPoint(X0, C0, t);
            ret[5] = getMidPoint(C1, X1, t);
            PointD m = getMidPoint(C0, C1, t);
            ret[2] = getMidPoint(ret[1], m, t);
            ret[4] = getMidPoint(m, ret[5], t);
            return ret;
        }

        /// <summary>
        /// 点p0, p1を結ぶ線分をt : 1 - tに分割する点の座標を計算します
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static PointD getMidPoint(PointD p0, PointD p1, double t)
        {
            double x = p0.getX() + (p1.getX() - p0.getX()) * t;
            double y = p0.getY() + (p1.getY() - p0.getY()) * t;
            return new PointD(x, y);
        }

        public BezierChain extractPartialBezier(double t_start, double t_end)
        {
            if (this.size() <= 1) {
                throw new Exception("chain must has two or more bezier points");
            }
            double start = this.points[0].getBase().getX();
            double end = this.points[this.size() - 1].getBase().getX();

            // [from, to]が、このベジエ曲線の範囲内にあるかどうかを検査
            if (start > t_start || t_end > end) {
                throw new Exception("no bezier point appeared in the range of \"from\" to \"to\"");
            }

            // t_start, t_endが既存のベジエデータ点位置を被っていないかどうか検査しながらコピー
            bool t_start_added = false; // 最初の区間が追加された直後だけ立つフラグ
            BezierChain edited = new BezierChain(mColor);
            int count = 0;
            for (int i = 0; i < this.points.Count - 1; i++) {
                if (this.points[i].getBase().getX() < t_start && t_start < this.points[i + 1].getBase().getX()) {
                    if (this.points[i].getBase().getX() < t_end && t_end < this.points[i + 1].getBase().getX()) {
#if DEBUG
                        AppManager.debugWriteLine("points[i].Base.X < t_start < t_end < points[i + 1].Base.X");
#endif
                        PointD x0 = this.points[i].getBase();
                        PointD x1 = this.points[i + 1].getBase();
                        PointD c0 = (this.points[i].getControlRightType() == BezierControlType.None) ?
                                                x0 : this.points[i].getControlRight();
                        PointD c1 = (this.points[i + 1].getControlLeftType() == BezierControlType.None) ?
                                                x1 : this.points[i + 1].getControlLeft();
                        PointD[] res = cutUnitBezier(x0, c0, c1, x1, t_start);

                        x0 = res[3];
                        c0 = res[4];
                        c1 = res[5];
                        x1 = res[6];
                        res = cutUnitBezier(x0, c0, c1, x1, t_end);

                        BezierPoint left = new BezierPoint(res[0]);
                        BezierPoint right = new BezierPoint(res[3]);
                        left.setControlRight(res[1]);
                        right.setControlLeft(res[2]);
                        left.setControlRightType(this.points[i].getControlRightType());
                        right.setControlLeftType(this.points[i + 1].getControlLeftType());
                        edited.add(left);
                        edited.add(right);
                        t_start_added = true;
                        break;
                    } else {
#if DEBUG
                        AppManager.debugWriteLine("points[i].Base.X < t_start < points[i + 1].Base.X");
#endif
                        PointD x0 = this.points[i].getBase();
                        PointD x1 = this.points[i + 1].getBase();
                        PointD c0 = (this.points[i].getControlRightType() == BezierControlType.None) ?
                                                x0 : this.points[i].getControlRight();
                        PointD c1 = (this.points[i + 1].getControlLeftType() == BezierControlType.None) ?
                                                x1 : this.points[i + 1].getControlLeft();
                        PointD[] res = cutUnitBezier(x0, c0, c1, x1, t_start);

                        BezierPoint left = new BezierPoint(res[3]);
                        BezierPoint right = new BezierPoint(res[6]);

                        left.setControlRight(res[4]);
                        left.setControlRightType(this.points[i].getControlRightType());

                        right.setControlLeft(res[5]);
                        right.setControlRight(this.points[i + 1].getControlRight());
                        right.setControlRightType(this.points[i + 1].getControlRightType());
                        right.setControlLeftType(this.points[i + 1].getControlLeftType());
                        edited.points.Add(left);
                        count++;
                        edited.points.Add(right);
                        count++;
                        t_start_added = true;
                    }
                }
                if (t_start <= this.points[i].getBase().getX() && this.points[i].getBase().getX() <= t_end) {
                    if (!t_start_added) {
                        edited.points.Add((BezierPoint)this.points[i].clone());
                        count++;
                    } else {
                        t_start_added = false;
                    }
                }
                if (this.points[i].getBase().getX() < t_end && t_end < this.points[i + 1].getBase().getX()) {
                    PointD x0 = this.points[i].getBase();
                    PointD x1 = this.points[i + 1].getBase();
                    PointD c0 = (this.points[i].getControlRightType() == BezierControlType.None) ?
                                            x0 : this.points[i].getControlRight();
                    PointD c1 = (this.points[i + 1].getControlLeftType() == BezierControlType.None) ?
                                            x1 : this.points[i + 1].getControlLeft();
                    PointD[] res = cutUnitBezier(x0, c0, c1, x1, t_end);

                    edited.points[count - 1].setControlRight(res[1]);

                    BezierPoint right = new BezierPoint(res[3]);
                    right.setControlLeft(res[2]);
                    right.setControlLeftType(this.points[i + 1].getControlLeftType());
                    edited.add(right);
                    count++;
                    break;
                }
            }

            if (this.points[this.points.Count - 1].getBase().getX() == t_end && !t_start_added) {
                edited.add((BezierPoint)this.points[this.points.Count - 1].clone());
                count++;
            }

            for (int i = 0; i < edited.size(); i++) {
                edited.points[i].setID(i);
            }
            return edited;
        }

        /// <summary>
        /// 登録されているデータ点を消去します
        /// </summary>
        public void clear()
        {
            points.Clear();
        }

        /// <summary>
        /// 与えられたBezierChainがx軸について陰かどうかを判定する
        /// </summary>
        /// <param name="chain"></param>
        /// <returns></returns>
        public static bool isBezierImplicit(BezierChain chain)
        {
            int size = chain.points.Count;
            if (size < 2) {
                return true;
            }
            BezierPoint last_point = chain.points[0];
            for (int i = 1; i < size; i++) {
                BezierPoint point = chain.points[i];
                double pt1 = last_point.getBase().getX();
                double pt2 = (last_point.getControlRightType() == BezierControlType.None) ? pt1 : last_point.getControlRight().getX();
                double pt4 = point.getBase().getX();
                double pt3 = (point.getControlLeftType() == BezierControlType.None) ? pt4 : point.getControlLeft().getX();
                if (!isUnitBezierImplicit(pt1, pt2, pt3, pt4)) {
                    return false;
                }
                last_point = point;
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
        private static bool isUnitBezierImplicit(double pt1, double pt2, double pt3, double pt4)
        {
            double a = pt4 - 3.0 * pt3 + 3.0 * pt2 - pt1;
            double b = 2.0 * pt3 - 4.0 * pt2 + 2.0 * pt1;
            double c = pt2 - pt1;
            if (a == 0.0) {
                if (c >= 0.0 && b + c >= 0.0) {
                    return true;
                } else {
                    return false;
                }
            } else if (a > 0.0) {
                if (-b / (2.0 * a) <= 0.0) {
                    if (c >= 0.0) {
                        return true;
                    } else {
                        return false;
                    }
                } else if (1.0 <= -b / (2.0 * a)) {
                    if (a + b + c >= 0.0) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    if (c - b * b / (4.0 * a) >= 0.0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            } else {
                if (-b / (2.0 * a) <= 0.5) {
                    if (a + b + c >= 0.0) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    if (c >= 0.0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }

        [OnDeserialized]
        private void onDeserialized(StreamingContext sc)
        {
            for (int i = 0; i < points.Count; i++) {
                points[i].setID(i);
            }
        }

        public void Dispose()
        {
            if (points != null) {
                points.Clear();
            }
        }

        public int getNextId()
        {
            if (points.Count > 0) {
                int max = points[0].getID();
                for (int i = 1; i < points.Count; i++) {
                    max = Math.Max(max, points[i].getID());
                }
                return max + 1;
            } else {
                return 0;
            }
        }

        public void getValueMinMax(ByRef<Double> min, ByRef<Double> max)
        {
            //todo: ベジエが有効なときに、曲線の描く最大値、最小値も考慮
            min.value = Default;
            max.value = Default;
            foreach (var bp in points) {
                min.value = Math.Min(min.value, bp.getBase().getY());
                max.value = Math.Max(max.value, bp.getBase().getY());
            }
        }

        public void getKeyMinMax(ByRef<Double> min, ByRef<Double> max)
        {
            min.value = Default;
            max.value = Default;
            foreach (var bp in points) {
                min.value = Math.Min(min.value, bp.getBase().getX());
                max.value = Math.Max(max.value, bp.getBase().getX());
            }
        }

        public Object clone()
        {
            BezierChain result = new BezierChain(this.mColor);
            foreach (var bp in points) {
                result.points.Add((BezierPoint)bp.clone());
            }
            result.Default = this.Default;
            result.id = id;
            return result;
        }

        public Object Clone()
        {
            return clone();
        }

        public BezierChain(Color curve)
        {
            points = new List<BezierPoint>();
            mColor = curve;
        }

        public BezierChain()
        {
            points = new List<BezierPoint>();
            mColor = Color.black;
        }

        public Color getColor()
        {
            return mColor;
        }

        public void setColor(Color value)
        {
            mColor = value;
        }

        public void add(BezierPoint bp)
        {
            if (points == null) {
                points = new List<BezierPoint>();
                mColor = Color.black;
            }
            points.Add(bp);
            points.Sort();
        }

        public void removeElementAt(int id_)
        {
            for (int i = 0; i < points.Count; i++) {
                if (points[i].getID() == id_) {
                    points.RemoveAt(i);
                    break;
                }
            }
        }

        public int size()
        {
            if (points == null) {
                return 0;
            }
            return points.Count;
        }

        public double getValue(double x)
        {
            int count = points.Count;
            for (int i = 0; i < count - 1; i++) {
                BezierPoint bpi = points[i];
                BezierPoint bpi1 = points[i + 1];
                if (bpi.getBase().getX() <= x && x <= bpi1.getBase().getX()) {
                    double x1 = bpi.getBase().getX();
                    double x4 = bpi1.getBase().getX();
                    if (x1 == x) {
                        return bpi.getBase().getY();
                    } else if (x4 == x) {
                        return bpi1.getBase().getY();
                    } else {
                        double x2 = bpi.getControlRight().getX();
                        double x3 = bpi1.getControlLeft().getX();
                        double a3 = x4 - 3 * x3 + 3 * x2 - x1;
                        double a2 = 3 * x3 - 6 * x2 + 3 * x1;
                        double a1 = 3 * (x2 - x1);
                        double a0 = x1;
                        double t = solveCubicEquation(a3, a2, a1, a0, x);
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
        private static double solveCubicEquation(double a3, double a2, double a1, double a0, double ans)
        {
            double suggested_t = 0.5;
            double a3_3 = a3 * 3.0;
            double a2_2 = a2 * 2.0;
            while ((a3_3 * suggested_t + a2_2) * suggested_t + a1 == 0.0) {
                suggested_t += 0.1;
            }
            double x = suggested_t;
            double new_x = suggested_t;
            for (int i = 0; i < 5000; i++) {
                new_x = x - (((a3 * x + a2) * x + a1) * x + a0 - ans) / ((a3_3 * x + a2_2) * x + a1);
                if (Math.Abs(new_x - x) < EPSILON * new_x) {
                    break;
                }
                x = new_x;
            }
            return new_x;
        }
    }

}
