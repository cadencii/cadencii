/*
 * CubicSpline.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii.apputil
{

    public class CubicSpline : ICloneable, IDisposable
    {
        int m_num_key;
        public double[] m_key, m_sp3_a, m_sp3_b, m_sp3_c, m_sp3_d;
        double m_default;

        public void Dispose()
        {
            m_key = null;
            m_sp3_a = null;
            m_sp3_b = null;
            m_sp3_c = null;
            m_sp3_d = null;
            GC.Collect();
            m_num_key = 0;
        }

        public object Clone()
        {
            CubicSpline ret = new CubicSpline(m_key, m_sp3_d);
            ret.DefaultValue = m_default;
            return ret;
        }

        public double DefaultValue
        {
            get
            {
                return m_default;
            }
            set
            {
                m_default = value;
            }
        }

        public double GetValue(double x)
        {
            int status;

            double a, b, c, d, xx;
            int i, index;

            status = 0;
            if (this.m_num_key == -1) {
                status = -2;
                return 0.0;
            }

            if (x < this.m_key[0] || this.m_key[this.m_num_key] < x) {
                status = -1;
                return 0.0;
            }//end if

            index = -1;
            for (i = 0; i < this.m_num_key; i++) {//    do i = 0, sp3%noOfKey - 1
                if (this.m_key[i] <= x && x <= this.m_key[i + 1]) {// if(sp3%key(i) <= x .and. x <= sp3%key(i + 1))then
                    index = i;
                    break;// exit
                }//end if
            }//end do

            xx = x - this.m_key[index];// xx = x - sp3%key(index)
            a = this.m_sp3_a[index];// a = sp3%a(index)
            b = this.m_sp3_b[index];// b = sp3%b(index)
            c = this.m_sp3_c[index];// c = sp3%c(index)
            d = this.m_sp3_d[index];// d = sp3%d(index)
            return ((a * xx + b) * xx + c) * xx + d;// ans = ((a * xx + b) * xx + c) * xx + d
        }//end subroutine spline3_val

        /// <summary>配列の形で与えられるデータ点から，3次のスプライン補間で用いる各区域のxの多項式の係数を計算します</summary>
        /// <param name="x">データ点のx座標を格納した配列</param>
        /// <param name="y">データ点のy座標を格納した配列</param>
        public CubicSpline(double[] x, double[] y)
        {
            //integer, intent(in) :: n
            //real(8), intent(in) :: x(0:n), y(0:n)
            //integer, intent(out) :: status
            int n = x.Length - 1;
            int status;
            m_num_key = -1;
            m_default = 0.0;
            double[] h, v, a, b, c, u, tmp, xx, yy;//real(8), allocatable :: h(:), v(:), a(:), b(:), c(:), u(:), tmp(:), xx(:), yy(:)
            double buff1, buff2;//real(8) buff1, buff2
            int i, iostatus, nn, j;//integer i, iostatus, nn, j

            status = 0;
            nn = n;

            xx = new double[n + 1];
            yy = new double[n + 1];//allocate(xx(0:n), yy(0:n))

            xx = x;
            yy = y;//yy(0:n) = y(0:n)

            //nullify(sp3%a)
            //nullify(sp3%b)
            //nullify(sp3%c)
            //nullify(sp3%d)
            //nullify(sp3%key)

            while (true) {//do
                iostatus = 0;
                for (i = 1; i <= nn - 1; i++) {// do i = 1, nn - 1
                    if (xx[i + 1] - x[i] == 0.0) {// if(xx(i + 1) - xx(i) == 0.0d0)then
                        for (j = i; j <= nn - 2; j++) {// do j = i, nn - 2
                            xx[j] = xx[j + 1];// xx(j) = xx(j + 1)
                            yy[j] = yy[j + 1];// yy(j) = yy(j + 1)
                        }// end do
                        iostatus = -1;

                        // create the copy of xx
                        //if(allocated(tmp))then
                        //deallocate(tmp)
                        //end if
                        tmp = new double[nn];// allocate(tmp(0:nn - 1))
                        for (int k = 0; k < nn; k++) {
                            tmp[k] = xx[k];
                        }//          tmp(0:nn - 1) = xx(0:nn - 1)
                        // deallocate(xx)
                        xx = new double[nn];// allocate(xx(0:nn - 1))
                        for (int k = 0; k < nn; k++) {
                            xx[k] = tmp[k];
                        }//          xx(0:nn - 1) = tmp(0:nn - 1)

                        // create the copy of yy
                        for (int k = 0; k < nn; k++) {
                            tmp[k] = yy[k];
                        }//          tmp(0:nn - 1) = yy(0:nn - 1)
                        //deallocate(yy)
                        yy = new double[nn];//          allocate(yy(0:nn - 1))
                        for (int k = 0; k < nn; k++) {
                            yy[k] = tmp[k];
                        }//          yy(0:nn - 1) = tmp(0:nn - 1)
                        break;// exit
                    }//end if
                }//end do
                if (iostatus == 0) {// if(iostatus == 0)then
                    break;// exit
                } else {// else
                    nn = nn - 1;//        nn = nn - 1
                }// end if
            }// end do

            //allocate(h(0:nn - 1), v(1:nn - 1), a(1:nn - 1), b(1:nn - 1), c(1:nn - 1), u(1:nn - 1))
            h = new double[nn];
            v = new double[nn - 1];
            a = new double[nn - 1];
            b = new double[nn - 1];
            c = new double[nn - 1];
            u = new double[nn - 1];

            // executed in debug mode ******************************************************************************************************
            //if(spline_debug_flag == 1)then                                                                                               ! **
            //open(unit = 26, file = 'spline_debug.txt')                                                                                 ! **
            //end if                                                                                                                       ! **
            // *****************************************************************************************************************************

            // calculate h_j
            for (i = 0; i < nn; i++) {//    do i = 0, nn - 1
                h[i] = xx[i + 1] - xx[i];//      h(i) = xx(i + 1) - xx(i)
                if (h[i] <= 0.0) {// if(h(i) <= 0.0d0)then
                    this.m_key = new double[1];
                    this.m_sp3_a = new double[1];
                    this.m_sp3_b = new double[1];
                    this.m_sp3_c = new double[1];
                    this.m_sp3_d = new double[1];
                    return;
                }
            }

            // calculate v_j
            for (i = 1; i < nn; i++) {//    do i = 1, nn - 1
                buff1 = yy[i + 1] - yy[i];// buff1 = yy(i + 1) - yy(i)
                buff2 = yy[i] - y[i - 1];// buff2 = yy(i) - yy(i - 1)
                if (buff1 == 0.0) {// if(buff1 == 0.0d0)then
                    if (buff2 == 0.0) {// if(buff2 == 0.0d0)then
                        v[i - 1] = 0;// v(i) = 0.0d0
                    } else {// else
                        v[i - 1] = 6.0 * (-(yy[i] - yy[i - 1]) / h[i - 1]);// v(i) = 6.0d0 * (- (yy(i) - yy(i - 1)) / h(i - 1))
                    }// end if
                } else {// else
                    if (buff2 == 0.0) {// if(buff2 == 0.0d0)then
                        v[i - 1] = 6.0 * ((yy[i + 1] - yy[i]) / h[i]);// v(i) = 6.0d0 * ((yy(i + 1) - yy(i)) / h(i))
                    } else {// else
                        v[i - 1] = 6.0 * ((yy[i + 1] - yy[i]) / h[i] - (yy[i] - yy[i - 1]) / h[i - 1]);// v(i) = 6.0d0 * ((yy(i + 1) - yy(i)) / h(i) - (yy(i) - yy(i - 1)) / h(i - 1))
                    }// end if
                }// end if
            }//end do

            for (i = 1; i < nn; i++) {// do i = 1, nn - 1
                a[i - 1] = 2.0 * (h[i - 1] + h[i]);// a(i) = 2.0d0 * (h(i - 1) + h(i))
                b[i - 1] = h[i - 1];// b(i) = h(i - 1)
                c[i - 1] = h[i];// c(i) = h(i)
            }// end do

            LUDecTDM(nn - 1, a, b, c, v, out u);// call LUDecTDM(nn - 1, a, b, c, v, u)

            m_num_key = nn;// sp3%noOfKey = nn
            this.m_sp3_a = new double[nn];// allocate(sp3%a(0:nn - 1))
            this.m_sp3_b = new double[nn];// allocate(sp3%b(0:nn - 1))
            this.m_sp3_c = new double[nn];// allocate(sp3%c(0:nn - 1))
            this.m_sp3_d = new double[nn];// allocate(sp3%d(0:nn - 1))
            this.m_key = new double[nn + 1];// allocate(sp3%key(0:nn))

            this.m_sp3_a[0] = u[0] / (6.0 * h[0]);// sp3%a(0) = u(1) / (6.0d0 * h(0))
            this.m_sp3_b[0] = 0.0;// sp3%b(0) = 0.0d0
            this.m_sp3_c[0] = (yy[1] - yy[0]) / h[0] - h[0] * u[0] / 6.0;// sp3%c(0) = (yy(1) - yy(0)) / h(0) - h(0) * u(1) / 6.0d0
            this.m_sp3_a[nn - 1] = -u[nn - 2] / (6.0 * h[nn - 1]);// sp3%a(nn - 1) = -u(nn - 1) / (6.0d0 * h(nn - 1))
            this.m_sp3_b[nn - 1] = u[nn - 2] * 0.5;//  sp3%b(nn - 1) = u(nn - 1) * 0.5d0
            this.m_sp3_c[nn - 1] = (yy[nn] - yy[nn - 1]) / h[nn - 1] - h[nn - 1] * u[nn - 2] / 3.0;// sp3%c(nn - 1) = (yy(nn) - yy(nn - 1)) / h(nn - 1) - h(nn - 1) * u(nn - 1) / 3.0d0
            for (i = 1; i <= nn - 2; i++) {// do i = 1, nn - 2
                this.m_sp3_a[i] = (u[i] - u[i - 1]) / (6.0 * h[i]);// sp3%a(i) = (u(i + 1) - u(i)) / (6.0d0 * h(i))
                this.m_sp3_b[i] = u[i - 1] * 0.5;// sp3%b(i) = u(i) * 0.5d0
                this.m_sp3_c[i] = (yy[i + 1] - yy[i]) / h[i] - h[i] * (2.0 * u[i - 1] + u[i]) / 6.0;// sp3%c(i) = (yy(i + 1) - yy(i)) / h(i) - h(i) * (2.0d0 * u(i) + u(i + 1)) / 6.0d0
            }//end do
            this.m_key = xx;
            //    sp3%key(0:nn) = xx(0:nn)
            for (int k = 0; k < nn; k++) {
                this.m_sp3_d[k] = yy[k];
            }//    sp3%d(0:nn - 1) = yy(0:nn - 1)
        }

        /// <summary>
        /// This subroutine solves system of linear equation(only for
        /// tridiagonal matrix system) by LU decompression method.
        /// Meanings of variables are defined below. 
        ///           ( a1  c1                       0     ) ( x1 )   ( y1 )
        ///           ( b2  a2  c2                         ) ( x2 )   ( y2 )
        ///           (     b3  a3  c3                     ) ( x3 )   ( y3 )
        ///           (                 ...                ) ( ...) = ( ...)
        ///           (                     bN-1 aN-1 cN-1 ) (xN-1)   (yN-1)
        ///           (        0                  bN   aN  ) ( xN )   ( yN )
        /// </summary>
        /// <param name="N"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        private static void LUDecTDM(int N, double[] a, double[] b, double[] c, double[] y, out double[] x)
        {
            double[] d = new double[N];
            double[] z = new double[N];
            double[] l = new double[N];
            int i;
            x = new double[N];

            d[0] = a[0];//  d(1) = a(1)
            for (i = 1; i < N; i++) {//  do i = 2, N
                l[i] = b[i] / d[i - 1];//    l(i) = b(i) / d(i - 1)
                d[i] = a[i] - l[i] * c[i - 1];//    d(i) = a(i) - l(i) * c(i - 1)
            }//  end do

            z[0] = y[0];//  z(1) = y(1)
            for (i = 1; i < N; i++) {//  do i = 2, N
                z[i] = y[i] - l[i] * z[i - 1];//    z(i) = y(i) - l(i) * z(i - 1)
            }//  end do

            x[N - 1] = z[N - 1] / d[N - 1];//  x(N) = z(N) / d(N)
            for (i = N - 2; i >= 0; i--) {//  do i = N - 1, 1, -1
                x[i] = (z[i] - c[i] * x[i + 1]) / d[i];//    x(i) = (z(i) - c(i) * x(i + 1)) / d(i)
            }//  end do
        }
    }

}
