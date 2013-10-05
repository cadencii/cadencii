/*
 * math.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii
{

    public partial class math
    {
        private const double _PI2 = 2.0 * Math.PI;
        private const double _PI4 = 4.0 * Math.PI;
        private const double _PI6 = 6.0 * Math.PI;
        private const double _PI8 = 8.0 * Math.PI;

        public enum WindowFunctionType
        {
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

        /// <summary>
        /// 2つの整数の最大公約数を返します。
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long gcd(long m, long n)
        {
            if (n > m) {
                long a = n;
                n = m;
                m = a;
            }
            while (true) {
                if (n == 0) {
                    return m;
                }
                long quotient = m / n;
                long odd = m - n * quotient;
                if (odd == 0) {
                    return n;
                }
                m = n;
                n = odd;
            }
        }

        public static double window_func(WindowFunctionType type, double x)
        {
            if (type == WindowFunctionType.Akaike) {
                return wnd_akaike(x);
            } else if (type == WindowFunctionType.Bartlett) {
                return wnd_bartlett(x);
            } else if (type == WindowFunctionType.Blackman) {
                return wnd_blackman(x);
            } else if (type == WindowFunctionType.Blackman_Harris) {
                return wnd_blackman_harris(x);
            } else if (type == WindowFunctionType.Blackman_Nattall) {
                return wnd_blackman_nattall(x);
            } else if (type == WindowFunctionType.flap_top) {
                return wnd_flap_top(x);
            } else if (type == WindowFunctionType.Gauss) {
                throw new Exception("too few argument for Gauss window function");
            } else if (type == WindowFunctionType.Hamming) {
                return wnd_hamming(x);
            } else if (type == WindowFunctionType.Hann) {
                return wnd_hann(x);
            } else if (type == WindowFunctionType.Kaiser) {
                throw new Exception("too few argument for Kaiser window function");
            } else if (type == WindowFunctionType.Nuttall) {
                return wnd_nuttall(x);
            } else if (type == WindowFunctionType.Parzen) {
                return wnd_parzen(x);
            } else if (type == WindowFunctionType.rectangular) {
                return wnd_rectangular(x);
            } else if (type == WindowFunctionType.Welch) {
                return wnd_welch(x);
            }
            return 0.0;
        }

        public static double window_func(WindowFunctionType type, double x, params double[] param)
        {

            if (type == WindowFunctionType.Akaike) {
                return wnd_akaike(x);
            } else if (type == WindowFunctionType.Bartlett) {
                return wnd_bartlett(x);
            } else if (type == WindowFunctionType.Blackman) {
                return wnd_blackman(x);
            } else if (type == WindowFunctionType.Blackman_Harris) {
                return wnd_blackman_harris(x);
            } else if (type == WindowFunctionType.Blackman_Nattall) {
                return wnd_blackman_nattall(x);
            } else if (type == WindowFunctionType.flap_top) {
                return wnd_flap_top(x);
            } else if (type == WindowFunctionType.Gauss) {
                return wnd_gauss(x, param[0]);
            } else if (type == WindowFunctionType.Hamming) {
                return wnd_hamming(x);
            } else if (type == WindowFunctionType.Hann) {
                return wnd_hann(x);
            } else if (type == WindowFunctionType.Kaiser) {
                return wnd_kaiser(x, param[0]);
            } else if (type == WindowFunctionType.Nuttall) {
                return wnd_nuttall(x);
            } else if (type == WindowFunctionType.Parzen) {
                return wnd_parzen(x);
            } else if (type == WindowFunctionType.rectangular) {
                return wnd_rectangular(x);
            } else if (type == WindowFunctionType.Welch) {
                return wnd_welch(x);
            }
            return 0.0;
        }

        public static double wnd_kaiser(double x, double alpha)
        {
            if (0.0 <= x && x <= 1.0) {
                double t = 2.0 * x - 1.0;
                return besi0(Math.PI * alpha * Math.Sqrt(1.0 - t * t)) / besi0(Math.PI * alpha);
            } else {
                return 0.0;
            }
        }

        public static double wnd_welch(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 4.0 * x * (1.0 - x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_akaike(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 0.625 - 0.5 * Math.Cos(_PI2 * x)
                             - 0.125 * Math.Cos(_PI4 * x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_parzen(double x)
        {
            double x0 = Math.Abs(x);
            if (x0 <= 1.0) {
                return (0.75 * x0 - 1.5) * x0 * x0 + 1.0;
            } else {
                x0 = 2.0 - x0;
                return 0.25 * x0 * x0 * x0;
            }
        }

        public static double wnd_flap_top(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 1.0 - 1.93 * Math.Cos(_PI2 * x)
                           + 1.29 * Math.Cos(_PI4 * x)
                           - 0.388 * Math.Cos(_PI6 * x)
                           + 0.032 * Math.Cos(_PI8 * x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_blackman_nattall(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 0.3635819 - 0.4891775 * Math.Cos(_PI2 * x)
                                 + 0.1365995 * Math.Cos(_PI4 * x)
                                 - 0.0106411 * Math.Cos(_PI6 * x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_blackman_harris(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 0.35875 - 0.48829 * Math.Cos(_PI2 * x)
                               + 0.14128 * Math.Cos(_PI4 * x)
                               - 0.01168 * Math.Cos(_PI6 * x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_nuttall(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 0.355768 - 0.487396 * Math.Cos(_PI2 * x)
                                + 0.144232 * Math.Cos(_PI4 * x)
                                - 0.012604 * Math.Cos(_PI6 * x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_bartlett(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 1.0 - 2.0 * Math.Abs(x - 0.5);
            } else {
                return 0.0;
            }
        }

        public static double wnd_blackman(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 0.42 - 0.5 * Math.Cos(_PI2 * x) + 0.08 * Math.Cos(_PI4 * x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_hann(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 0.5 - 0.5 * Math.Cos(_PI2 * x);
            } else {
                return 0.0;
            }
        }

        public static double wnd_gauss(double x, double sigma)
        {
            return Math.Exp(-x * x / (sigma * sigma));
        }

        public static double wnd_rectangular(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 1.0;
            } else {
                return 0.0;
            }
        }

        public static double wnd_hamming(double x)
        {
            if (0.0 <= x && x <= 1.0) {
                return 0.54 - 0.46 * Math.Cos(_PI2 * x);
            } else {
                return 0.0;
            }
        }

        public static double besi0(double x)
        {
            int i;
            double w, wx375;
            double[] a = { 1.0,        3.5156229,  3.0899424,
                           1.2067492,  0.2659732,  0.0360768};
            double[] b = { 0.39894228,   0.013285917,  0.002253187,
                          -0.001575649,  0.009162808, -0.020577063,
                           0.026355372, -0.016476329};
            if (x < 0.0) {
                return 0.0;
            }
            if (x <= 3.75) {
                wx375 = x * x / 14.0625;
                w = 0.0045813;
                for (i = 5; i >= 0; i--) {
                    w = w * wx375 + a[i];
                }
                return w;
            }
            wx375 = 3.75 / x;
            w = 0.003923767;
            for (i = 7; i >= 0; i--) {
                w = w * wx375 + b[i];
            }
            return w / Math.Sqrt(x) * Math.Exp(x);
        }

        public static double erf(double x)
        {
            return 1.0 - erfc(x);
        }

        public static double erfc(double x)
        {
            double t, z, res;
            z = Math.Abs(x);
            t = 1.0 / (1.0 + 0.5 * z);
            res = t * Math.Exp(-z * z - 1.26551223 + t * (1.00002368 + t * (0.37409196
               + t * (0.09678418 + t * (-0.18628806 + t * (0.27886807
               + t * (-1.13520398 + t * (1.48851587 + t * (-0.82215223
               + t * 0.17087277)))))))));
            if (x < 0.0) {
                res = 2.0 - res;
            }
            return res;
        }

        // reference: http://www.kurims.kyoto-u.ac.jp/~ooura/index-j.html
        public static double erfcinv(double y)
        {
            double s, t, u, w, x, z;

            z = y;
            if (y > 1) {
                z = 2 - y;
            }
            w = 0.916461398268964 - Math.Log(z);
            u = Math.Sqrt(w);
            s = (Math.Log(u) + 0.488826640273108) / w;
            t = 1 / (u + 0.231729200323405);
            x = u * (1 - s * (s * 0.124610454613712 + 0.5)) -
                ((((-0.0728846765585675 * t + 0.269999308670029) * t +
                0.150689047360223) * t + 0.116065025341614) * t +
                0.499999303439796) * t;
            t = 3.97886080735226 / (x + 3.97886080735226);
            u = t - 0.5;
            s = (((((((((0.00112648096188977922 * u +
                1.05739299623423047e-4) * u - 0.00351287146129100025) * u -
                7.71708358954120939e-4) * u + 0.00685649426074558612) * u +
                0.00339721910367775861) * u - 0.011274916933250487) * u -
                0.0118598117047771104) * u + 0.0142961988697898018) * u +
                0.0346494207789099922) * u + 0.00220995927012179067;
            s = ((((((((((((s * u - 0.0743424357241784861) * u -
                0.105872177941595488) * u + 0.0147297938331485121) * u +
                0.316847638520135944) * u + 0.713657635868730364) * u +
                1.05375024970847138) * u + 1.21448730779995237) * u +
                1.16374581931560831) * u + 0.956464974744799006) * u +
                0.686265948274097816) * u + 0.434397492331430115) * u +
                0.244044510593190935) * t -
                z * Math.Exp(x * x - 0.120782237635245222);
            x += s * (x * s + 1);
            if (y > 1) {
                x = -x;
            }
            return x;
        }

        public static double erfinv(double y)
        {
            return -erfcinv(y + 1.0);
        }
    }

}
