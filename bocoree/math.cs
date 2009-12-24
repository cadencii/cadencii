/*
 * math.cs
 * Copyright (C) 2008-2009 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani;

#else
using System;

namespace org.kbinani {
#endif

#if JAVA
    public class math{
#else
    public partial class math {
#endif
        private const double _PI2 = 2.0 * Math.PI;
        private const double _PI4 = 4.0 * Math.PI;
        private const double _PI6 = 6.0 * Math.PI;
        private const double _PI8 = 8.0 * Math.PI;

        public enum WindowFunctionType {
            Hamming,
            rectangular,
            Gauss,
            Hann,
            Blackman,
            Bartlett,
            Nuttall,
            Blackman_Harris,
            Blackman_Nattall,
            flap_top,
            Parzen,
            Akaike,
            Welch,
            Kaiser,
        }

        public static double window_func( WindowFunctionType type, double x )
#if JAVA
            throws Exception
#endif
        {
            if ( type == WindowFunctionType.Akaike ) {
                return wnd_akaike( x );
            } else if ( type == WindowFunctionType.Bartlett ) {
                return wnd_bartlett( x );
            } else if ( type == WindowFunctionType.Blackman ) {
                return wnd_blackman( x );
            } else if ( type == WindowFunctionType.Blackman_Harris ) {
                return wnd_blackman_harris( x );
            } else if ( type == WindowFunctionType.Blackman_Nattall ) {
                return wnd_blackman_nattall( x );
            } else if ( type == WindowFunctionType.flap_top ) {
                return wnd_flap_top( x );
            } else if ( type == WindowFunctionType.Gauss ) {
                throw new Exception( "too few argument for Gauss window function" );
            } else if ( type == WindowFunctionType.Hamming ) {
                return wnd_hamming( x );
            } else if ( type == WindowFunctionType.Hann ) {
                return wnd_hann( x );
            } else if ( type == WindowFunctionType.Kaiser ) {
                throw new Exception( "too few argument for Kaiser window function" );
            } else if ( type == WindowFunctionType.Nuttall ) {
                return wnd_nuttall( x );
            } else if ( type == WindowFunctionType.Parzen ) {
                return wnd_parzen( x );
            } else if ( type == WindowFunctionType.rectangular ) {
                return wnd_rectangular( x );
            } else if ( type == WindowFunctionType.Welch ) {
                return wnd_welch( x );
            }
            return 0.0;
        }

#if JAVA
        public static double window_func( WindowFunctionType type, double x, double... param ) 
#else
        public static double window_func( WindowFunctionType type, double x, params double[] param ) 
#endif
        {

            if ( type == WindowFunctionType.Akaike ) {
                return wnd_akaike( x );
            } else if ( type == WindowFunctionType.Bartlett ) {
                return wnd_bartlett( x );
            } else if ( type == WindowFunctionType.Blackman ) {
                return wnd_blackman( x );
            } else if ( type == WindowFunctionType.Blackman_Harris ) {
                return wnd_blackman_harris( x );
            } else if ( type == WindowFunctionType.Blackman_Nattall ) {
                return wnd_blackman_nattall( x );
            } else if ( type == WindowFunctionType.flap_top ) {
                return wnd_flap_top( x );
            } else if ( type == WindowFunctionType.Gauss ) {
                return wnd_gauss( x, param[0] );
            } else if ( type == WindowFunctionType.Hamming ) {
                return wnd_hamming( x );
            } else if ( type == WindowFunctionType.Hann ) {
                return wnd_hann( x );
            } else if ( type == WindowFunctionType.Kaiser ) {
                return wnd_kaiser( x, param[0] );
            } else if ( type == WindowFunctionType.Nuttall ) {
                return wnd_nuttall( x );
            } else if ( type == WindowFunctionType.Parzen ) {
                return wnd_parzen( x );
            } else if ( type == WindowFunctionType.rectangular ) {
                return wnd_rectangular( x );
            } else if ( type == WindowFunctionType.Welch ) {
                return wnd_welch( x );
            }
            return 0.0;
        }

        public static double wnd_kaiser( double x, double alpha ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                double t = 2.0 * x - 1.0;
                return besi0( Math.PI * alpha * Math.Sqrt( 1.0 - t * t ) ) / besi0( Math.PI * alpha );
            } else {
                return 0.0;
            }
        }

        public static double wnd_welch( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 4.0 * x * (1.0 - x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_akaike( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 0.625 - 0.5 * Math.Cos( _PI2 * x )
                             - 0.125 * Math.Cos( _PI4 * x );
            } else {
                return 0.0;
            }
        }

        public static double wnd_parzen( double x ) {
            double x0 = Math.Abs( x );
            if ( x0 <= 1.0 ) {
                return (0.75 * x0 - 1.5) * x0 * x0 + 1.0;
            } else {
                x0 = 2.0 - x0;
                return 0.25 * x0 * x0 * x0;
            }
        }

        public static double wnd_flap_top( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 1.0 - 1.93 * Math.Cos( _PI2 * x )
                           + 1.29 * Math.Cos( _PI4 * x )
                           - 0.388 * Math.Cos( _PI6 * x )
                           + 0.032 * Math.Cos( _PI8 * x );
            } else {
                return 0.0;
            }
        }

        public static double wnd_blackman_nattall( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 0.3635819 - 0.4891775 * Math.Cos( _PI2 * x )
                                 + 0.1365995 * Math.Cos( _PI4 * x )
                                 - 0.0106411 * Math.Cos( _PI6 * x );
            } else {
                return 0.0;
            }
        }

        public static double wnd_blackman_harris( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 0.35875 - 0.48829 * Math.Cos( _PI2 * x )
                               + 0.14128 * Math.Cos( _PI4 * x )
                               - 0.01168 * Math.Cos( _PI6 * x );
            } else {
                return 0.0;
            }
        }

        public static double wnd_nuttall( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 0.355768 - 0.487396 * Math.Cos( _PI2 * x )
                                + 0.144232 * Math.Cos( _PI4 * x )
                                - 0.012604 * Math.Cos( _PI6 * x );
            } else {
                return 0.0;
            }
        }

        public static double wnd_bartlett( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 1.0 - 2.0 * Math.Abs( x - 0.5 );
            } else {
                return 0.0;
            }
        }

        public static double wnd_blackman( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 0.42 - 0.5 * Math.Cos( _PI2 * x ) + 0.08 * Math.Cos( _PI4 * x );
            } else {
                return 0.0;
            }
        }

        public static double wnd_hann( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 0.5 - 0.5 * Math.Cos( _PI2 * x );
            } else {
                return 0.0;
            }
        }

        public static double wnd_gauss( double x, double sigma ) {
            return Math.Exp( -x * x / (sigma * sigma) );
        }

        public static double wnd_rectangular( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 1.0;
            } else {
                return 0.0;
            }
        }

        public static double wnd_hamming( double x ) {
            if ( 0.0 <= x && x <= 1.0 ) {
                return 0.54 - 0.46 * Math.Cos( _PI2 * x );
            } else {
                return 0.0;
            }
        }

        public static double besi0( double x ) {
            int i;
            double w, wx375;
            double[] a = { 1.0,        3.5156229,  3.0899424,
                           1.2067492,  0.2659732,  0.0360768};
            double[] b = { 0.39894228,   0.013285917,  0.002253187,
                          -0.001575649,  0.009162808, -0.020577063,
                           0.026355372, -0.016476329};
            if ( x < 0.0 ) {
                return 0.0;
            }
            if ( x <= 3.75 ) {
                wx375 = x * x / 14.0625;
                w = 0.0045813;
                for ( i = 5; i >= 0; i-- ) {
                    w = w * wx375 + a[i];
                }
                return w;
            }
            wx375 = 3.75 / x;
            w = 0.003923767;
            for ( i = 7; i >= 0; i-- ) {
                w = w * wx375 + b[i];
            }
            return w / Math.Sqrt( x ) * Math.Exp( x );
        }

        public static double erfcc( double x ) {
            double t, z, res;
            z = Math.Abs( x );
            t = 1.0 / (1.0 + 0.5 * z);
            res = t * Math.Exp( -z * z - 1.26551223 + t * (1.00002368 + t * (0.37409196
               + t * (0.09678418 + t * (-0.18628806 + t * (0.27886807
               + t * (-1.13520398 + t * (1.48851587 + t * (-0.82215223
               + t * 0.17087277)))))))) );
            if ( x < 0.0 ) {
                res = 2.0 - res;
            }
            return res;
        }
    }

#if !JAVA
}
#endif
