/*
 * FormPlayBack.cs
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
using System.Windows.Forms;

namespace Boare.Cadencii {

    partial class FormPlayBack : Form {
        private int[,] c;
        private dot[] dots;
        private int n;

        public FormPlayBack() {
            InitializeComponent();
            n = 4;
            c = new int[,]{
                {0, 1, 0, 0},
                {1, 0, 1, 0},
                {0, 1, 0, 0},
                {0, 1, 0, 0},
            };
#if DEBUG
            for ( int i = 0; i < n; i++ ) {
                for ( int j = 0; j < n; j++ ) {
                    Console.Write( "  " + c[i, j] );
                }
                Console.WriteLine();
            }
#endif
            dots = new dot[n];
            for ( int i = 0; i < n; i++ ) {
                dots[i] = new dot();
                dots[i].x = i;
                dots[i].y = i;
            }
            CalculateStaticPosition( n, ref dots, c, 0 );
        }

        public static void CalculateStaticPosition( int n, ref dot[] dots, int[,] c, int max_step ) {
#if DEBUG
            Console.WriteLine( "CalculateStaticPosition" );
#endif
            const double _M = 20.0;
            const double _DT2 = 0.001; // dt^2 / 2
            const double _G = 1.0;
            double _K = _G / (1.0 + Math.Sqrt( 3.0 ) / 3);
            double[,] t = new double[n, n];
            double[,] theta = new double[n, n];
            double[,] dist = new double[n, n];
            dot[] old = new dot[n];
            for ( int i = 0; i < n; i++ ) {
                old[i] = new dot();
            }
            for( int m = 0; m < max_step; m++ ){
                for ( int i = 0; i < n; i++ ) {
                    old[i].x = dots[i].x;
                    old[i].y = dots[i].y;
                }

                // 距離をあらかじめ計算
                for ( int i = 0; i < n; i++ ) {
                    for ( int j = i + 1; j < n; j++ ) {
                        double dx = dots[i].x - dots[j].x;
                        double dy = dots[i].y - dots[j].y;
                        dist[i, j] = dx * dx + dy * dy;
                        dist[j, i] = dist[i, j];
                    }
                }

                // まず仮想張力を計算
                for ( int i = 0; i < n; i++ ) {
                    for ( int j = 0; j < n; j++ ) {
                        if ( c[i, j] == 1 ) {
                            t[i, j] = _M * (Math.Sqrt( dist[i, j] ) - 1.0);
                        } else {
                            t[i, j] = 0.0;
                        }
                    }
                }

                // 角度をあらかじめ計算
                for ( int i = 0; i < n; i++ ) {
                    for ( int j = 0; j < n; j++ ) {
                        theta[i, j] = Math.Atan2( dots[j].y - dots[i].y, dots[j].x - dots[i].x );
                    }
                }

                for ( int i = 0; i < n; i++ ) {
                    // 仮想張力
                    double fx = 0.0;
                    double fy = 0.0;
                    for ( int j = 0; j < n; j++ ) {
                        if ( c[i, j] == 1 && i != j ) {
                            if ( t[i, j] > 0 ) {
                                fx += t[i, j] * Math.Cos( theta[i, j] );
                                fy += t[i, j] * Math.Sin( theta[i, j] );
                            } else {
                                fx += t[i, j] * Math.Cos( -theta[i, j] );
                                fy += t[i, j] * Math.Sin( -theta[i, j] );
                            }
                        }
                    }
                    // 反発力
                    for ( int j = 0; j < n; j++ ) {
                        if ( j != i ) {
                            double force = (dist[i, j] == 0.0) ? _K : _K / dist[i, j];
                            fx += force * Math.Cos( -theta[i, j] );
                            fy += force * Math.Sin( -theta[i, j] );
                        }
                    }
                    fy += _G;
                    double dx = fx * _DT2;
                    double dy = fy * _DT2;
                    if ( i == 0 ) {
                        dx = 0;
                        dy = 0;
                    }
                    dots[i].x = old[i].x + dx;
                    dots[i].y = old[i].y + dy;
#if DEBUG
                    Console.WriteLine( "(i,x,y)=" + i + "," + dots[i].x + "," + dots[i].y );
#endif
                }

            }
        }

        private void FormPlayBack_Paint( object sender, PaintEventArgs e ) {
            double _ORDER = 20.0;
            int _RADIUS = 10;
            int centre = (int)(this.ClientSize.Width / 2);
            for ( int i = 0; i < n; i++ ) {
                int x = (int)(dots[i].x * _ORDER);
                int y = (int)(dots[i].y * _ORDER);
                e.Graphics.DrawEllipse(
                    Pens.Black,
                    new Rectangle( x - _RADIUS / 2 + centre, y - _RADIUS / 2, _RADIUS, _RADIUS ) );
                e.Graphics.DrawString(
                    i.ToString(),
                    new Font( FontFamily.GenericSansSerif, 9 ),
                    Brushes.Black,
                    new PointF( x + centre, y ) );
            }
        }

        private void btnRecalculation_Click( object sender, EventArgs e ) {
            try {
                Console.WriteLine( "btnRecalculation_Click" );
                int max = int.Parse( txtCalcNum.Text );
                Console.WriteLine( "    max=" + max );
                CalculateStaticPosition( n, ref dots, c, max );
                this.Refresh();
            } catch {
            }
        }
    }

    public struct dot {
        public double x;
        public double y;
        public int id;
    }

}
