/*
 * ColorBar.cs
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
using System.Drawing;

namespace cadencii.apputil
{

    public enum ColorBarType
    {
        apricot = 1,
        carnation = 2,
        ether = 3,
        grayscale_banded = 4,
        hot_metal = 5,
        hotmetal = 5,
        lava_waves = 6,
        lavawaves = 6,
        malachite = 7,
        morning_glory = 8,
        peanut_butter_and_jerry = 9,
        purple_haze = 10,
        rainbow = 11,
        rainbow_banded = 12,
        rainbow_striped = 13,
        rose = 14,
        saturn = 15,
        seismic = 16,
        space = 17,
        supernova = 18,
        cyclic = 19,
        rflow_rainbow = 20,
    }

    public static partial class Util
    {
        const double OPORT_OP_PI = 3.141592653589793238462643383279502884197169399;
        const double OPORT_OP_PI2 = 3.141592653589793238462643383279502884197169399 * 2.0;
        const double OPORT_OP_1P9 = 1.0 / 9.0;
        const double OPORT_OP_3P9 = 3.0 / 9.0;
        const double OPORT_OP_5P9 = 5.0 / 9.0;
        const double OPORT_OP_7P9 = 7.0 / 9.0;

        public static Color ColorBar(double x, double xmax, double xmin, ColorBarType bartype)
        {
            int red = 0;
            int green = 0;
            int blue = 0;
            double y, h, s, v, r, g, b;
            y = (x - xmin) / (xmax - xmin);
            switch (bartype) {
                case ColorBarType.apricot:
                red = oport_colorbar_apricot_red(y);
                green = oport_colorbar_apricot_green(y);
                blue = oport_colorbar_apricot_blue(y);
                break;
                case ColorBarType.carnation:
                red = oport_colorbar_carnation_red(y);
                green = oport_colorbar_carnation_green(y);
                blue = oport_colorbar_carnation_blue(y);
                break;
                case ColorBarType.ether:
                red = oport_colorbar_ether_red(y);
                green = oport_colorbar_ether_green(y);
                blue = oport_colorbar_ether_blue(y);
                break;
                case ColorBarType.grayscale_banded:
                red = oport_colorbar_grayscale_banded_rgb(y);
                green = red;
                blue = red;
                break;
                case ColorBarType.hot_metal:
                red = (int)oport_colorbar_hotmetal_red(y);
                green = (int)oport_colorbar_hotmetal_green(y);
                blue = (int)oport_colorbar_hotmetal_blue(y);
                break;
                case ColorBarType.lava_waves:
                red = oport_colorbar_lava_waves_red(y);
                green = oport_colorbar_lava_waves_green(y);
                blue = oport_colorbar_lava_waves_blue(y);
                break;
                case ColorBarType.malachite:
                red = oport_colorbar_malachite_red(y);
                green = oport_colorbar_malachite_green(y);
                blue = oport_colorbar_malachite_blue(y);
                break;
                case ColorBarType.morning_glory:
                red = oport_colorbar_morning_glory_red(y);
                green = oport_colorbar_morning_glory_green(y);
                blue = oport_colorbar_morning_glory_blue(y);
                break;
                case ColorBarType.peanut_butter_and_jerry:
                red = oport_colorbar_peanut_butter_and_jerry_red(y);
                green = oport_colorbar_peanut_butter_and_jerry_green(y);
                blue = oport_colorbar_peanut_butter_and_jerry_blue(y);
                break;
                case ColorBarType.purple_haze:
                red = oport_colorbar_purple_haze_red(y);
                green = oport_colorbar_purple_haze_green(y);
                blue = oport_colorbar_purple_haze_blue(y);
                break;
                case ColorBarType.rainbow:
                red = (int)oport_colorbar_rainbow_red(y);
                green = (int)oport_colorbar_rainbow_green(y);
                blue = (int)oport_colorbar_rainbow_blue(y);
                break;
                case ColorBarType.rainbow_banded:
                if (y < 0.0) {
                    red = 44;
                    green = 6;
                    blue = 65;
                } else if (y <= 1.0) {
                    h = oport_colorbar_rainbow_banded_h(y);
                    s = oport_colorbar_rainbow_banded_s(y);
                    v = oport_colorbar_rainbow_banded_v(y);
                    Util.HsvToRgb(h, s, v, out r, out g, out b);
                    red = (int)(r * 255.0);
                    green = (int)(g * 255.0);
                    blue = (int)(b * 255.0);
                } else {
                    red = 167;
                    green = 18;
                    blue = 15;
                }
                break;
                case ColorBarType.rainbow_striped:
                if (y < 0.0) {
                    red = 150;
                    green = 0;
                    blue = 144;
                } else if (y <= 1.0) {
                    h = oport_colorbar_rainbow_striped_h(y);
                    s = oport_colorbar_rainbow_striped_s(y);
                    v = oport_colorbar_rainbow_striped_v(y);
                    Util.HsvToRgb(h, s, v, out r, out g, out  b);
                    red = (int)(r * 255.0);
                    green = (int)(g * 255.0);
                    blue = (int)(b * 255.0);
                } else {
                    red = 255;
                    green = 0;
                    blue = 0;
                }
                break;
                case ColorBarType.rose:
                red = oport_colorbar_rose_red(y);
                green = oport_colorbar_rose_green(y);
                blue = oport_colorbar_rose_blue(y);
                break;
                case ColorBarType.saturn:
                red = oport_colorbar_saturn_red(y);
                green = oport_colorbar_saturn_green(y);
                blue = oport_colorbar_saturn_blue(y);
                break;
                case ColorBarType.seismic:
                red = oport_colorbar_seismic_red(y);
                green = oport_colorbar_seismic_green(y);
                blue = oport_colorbar_seismic_blue(y);
                break;
                case ColorBarType.space:
                red = oport_colorbar_space_red(y);
                green = oport_colorbar_space_green(y);
                blue = oport_colorbar_space_blue(y);
                break;
                case ColorBarType.supernova:
                red = oport_colorbar_supernova_red(y);
                green = oport_colorbar_supernova_green(y);
                blue = oport_colorbar_supernova_blue(y);
                break;
                case ColorBarType.cyclic:
                blue = (int)(100.0 * Math.Cos(OPORT_OP_PI2 * (y + 2.0 / 3.0))) + 100;
                green = (int)(100.0 * Math.Cos(OPORT_OP_PI2 * (y + 1.0 / 3.0))) + 100;
                red = (int)(100.0 * Math.Cos(OPORT_OP_PI2 * y)) + 100;
                break;
                case ColorBarType.rflow_rainbow:
                red = oport_colorbar_rflow_rainbow_red(y);
                green = oport_colorbar_rflow_rainbow_green(y);
                blue = oport_colorbar_rflow_rainbow_blue(y);
                break;
            }
            return Color.FromArgb(red, green, blue);
        }

        static int oport_colorbar_rgb2col(int red, int green, int blue)
        {
            int res = (blue * 256 + green) * 256 + red;
            return res;
        }

        /// APRICOT [No.1];
        private static int oport_colorbar_apricot_blue(double x)
        {
            double x255;
            int res;
            x255 = x * 256.0;
            if (x255 < 0.0) {
                res = 241;
            } else if (x255 < 66.82) {
                res = (int)(oport_colorbar_apricot_f(x255 - 32.0, -27.0));
                if (x255 > 32.0 && res > 225) {
                    res = res - 10;
                }
            } else if (x255 < 126.67) {
                res = (int)(oport_colorbar_apricot_f(x255 - 97.0, 30.0));
            } else if (x255 < 195.83) {
                res = (int)(oport_colorbar_apricot_f(x255 - 161.0, -27.0));
                if (x255 > 161.0 && res > 225) {
                    res = res - 10;
                }
            } else if (x255 < 256.0) {
                res = (int)(oport_colorbar_apricot_f(x255 - 226.0, 30.0));
            } else {
                res = 251;
            }
            if (res > 255) {
                res = 255;
            }
            return res;
        }
        private static int oport_colorbar_apricot_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x < 1.0) {
                res = (int)(109.0 * x + 25.0 * Math.Sin(9.92 * OPORT_OP_PI * x) * x);
            } else {
                res = 102;
            }
            return res;
        }
        private static int oport_colorbar_apricot_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x < 1.0) {
                res = oport_colorbar_apricot_f2(x);
                if (res > 255) {
                    res = 510 - res;
                }
            } else {
                res = 255;
            }
            return res;
        }
        private static double oport_colorbar_apricot_f(double x, double c)
        {
            double res;
            res = Math.Abs(-5.563d - 5 * (x * x * x * x) + 3.331d - 16 * (x * x * x) + 3.045d - 1 * (x * x) + 4.396d - 12 * x + c);
            return res;
        }
        private static int oport_colorbar_apricot_f2(double x)
        {
            int res;
            //    oport_colorbar_apricot_f2 = (int)(262.0 * x + 12.0 * x * Math.Sin(66.0 * OPORT_OP_PI * x - 8.0 * x**2 + x**3));
            res = (int)(262.0 * x + 12.0 * x * Math.Sin(((x - 8.0) * x + 66.0 * OPORT_OP_PI) * x));
            return res;
        }
        /// CARNATION [No.2];
        private static int oport_colorbar_carnation_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 11;
            } else if (x < 0.16531216481302) {
                res = (int)(-1635.0 * (x * x) + 1789.0 * x + 3.938);
            } else if (x < 0.50663669203696) {
                res = 255;
            } else if (x < 0.67502056695956) {
                res = (int)(1.28932e3 * (x * x * x) - 7.74147e2 * (x * x) - 9.47634e2 * x + 7.65071e2);
            } else if (x < 1.0) {
                res = oport_colorbar_carnation_f(x);
            } else {
                res = 251;
            }
            return res;
        }
        private static int oport_colorbar_carnation_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x < 0.33807590140751) {
                res = oport_colorbar_carnation_f(x);
            } else if (x < 0.50663669203696) {
                res = (int)(-5.83014e2 * (x * x * x) - 8.38523e2 * (x * x) + 2.03823e3 * x - 4.86592e2);
            } else if (x < 0.84702285244773) {
                res = 255;
            } else if (x < 1.0) {
                res = (int)(-5.03306e2 * (x * x * x) + 2.95545e3 * (x * x) - 4.19210e3 * x + 1.99128e3);
            } else {
                res = 251;
            }
            return res;
        }
        private static int oport_colorbar_carnation_red(double x)
        {
            int res;
            if (x < 0.16531216481302) {
                res = 255;
            } else if (x < 0.33807590140751) {
                res = (int)(-5.15164e3 * (x * x * x) + 5.30564e3 * (x * x) - 2.65098e3 * x + 5.70771e2);
            } else if (x < 0.67502056695956) {
                res = oport_colorbar_carnation_f(x);
            } else if (x < 0.84702285244773) {
                res = (int)(3.34136e3 * (x * x * x) - 9.01976e3 * (x * x) + 8.39740e3 * x - 2.41682e3);
            } else {
                res = 255;
            }
            return res;
        }
        private static int oport_colorbar_carnation_f(double x)
        {
            int res = (int)(-9.93427 * (x * x * x) + 1.56301e1 * (x * x) + 2.44663e2 * x);
            return res;
        }

        /// ETHER [No.3];
        private static int oport_colorbar_ether_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 246;
            } else if (x < 0.25) {
                res = oport_colorbar_ether_f(x - 32.0 / 256.0, 65.0);
            } else if (x < 130.0 / 256.0) {
                res = oport_colorbar_ether_f2(x - 97.0 / 256.0, 190.0);
            } else if (x < 193.0 / 256.0) {
                res = oport_colorbar_ether_f(x - 161.0 / 256.0, 65.0);
            } else if (x < 1.0) {
                res = oport_colorbar_ether_f2(x - 226.0 / 256.0, 190.0);
            } else {
                res = 18;
            }
            return res;
        }
        private static int oport_colorbar_ether_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x < 0.20615790927912) {
                res = (int)((((-3.81619e4 * x - 2.94574e3) * x + 2.61347e3) * x - 7.92183e1) * x);
            } else if (x < 0.54757171958025) {
                res = (int)(((((2.65271e5 * x - 4.14808e5) * x + 2.26118e5) * x - 5.16491e4) * x + 5.06893e3) * x - 1.80630e2);
            } else if (x < 0.71235558668792) {
                res = (int)((((1.77058e5 * x - 4.62571e5) * x + 4.39628e5) * x - 1.80143e5) * x + 2.68555e4);
            } else if (x < 1.0) {
                res = (int)((((1.70556e5 * x - 6.20429e5) * x + 8.28331e5) * x - 4.80913e5) * x + 1.02608e5);
                if (res > 255) {
                    res = 510 - res;
                }
            } else {
                res = 154;
            }
            return res;
        }
        private static int oport_colorbar_ether_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 2;
            } else if (x < 1.0) {
                res = (int)(2.83088e2 * x + 8.17847d - 1);
                if (res > 255) {
                    res = 510 - res;
                }
            } else {
                res = 226;
            }
            return res;
        }
        private static int oport_colorbar_ether_f(double x, double b)
        {
            int res = (int)(-1.89814e5 * (x * x * x * x) + 1.50967e4 * (x * x) + b);
            return res;
        }
        private static int oport_colorbar_ether_f2(double x, double b)
        {
            int res = (int)(1.88330e5 * (x * x * x * x) - 1.50839e4 * (x * x) + b);
            return res;
        }

        /// GRAYSCALE_BANDED [No.4];
        private static int oport_colorbar_grayscale_banded_rgb(double x)
        {
            int res;
            res = (int)(Math.Cos(133.0 * x) * 28.0 + 230.0 * x + 27.0);
            if (res > 255) {
                res = 255 - res;
            }
            return res;
        }

        /// HOT_METAL [No.5];
        private static double oport_colorbar_hotmetal_blue(double x)
        {
            int res;
            res = 0;
            return res;
        }
        private static double oport_colorbar_hotmetal_green(double x)
        {
            int res;
            if (x < 0.6) {
                res = 0;
            } else if (x <= 0.95) {
                res = (int)((x - 0.6) * 728.57);
            } else {
                res = 255;
            }
            return res;
        }
        private static double oport_colorbar_hotmetal_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x <= 0.57147) {
                res = (int)(446.22 * x);
            } else {
                res = 255;
            }
            return res;
        }

        /// LAVA_WAVES [No.6];
        private static int oport_colorbar_lava_waves_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 124;
            } else if (x <= 1.0) {
                res = (int)(128.0 * Math.Sin(6.25 * (x + 0.5)) + 128.0);
            } else {
                res = 134;
            }
            return res;
        }
        private static int oport_colorbar_lava_waves_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 121;
            } else if (x <= 1.0) {
                res = (int)(63.0 * Math.Sin(x * 99.72) + 97.0);
            } else {
                res = 52;
            }
            return res;
        }
        private static int oport_colorbar_lava_waves_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 131;
            } else if (x <= 1.0) {
                res = (int)(128.0 * Math.Sin(6.23 * x) + 128.0);
            } else {
                res = 121;
            }
            return res;
        }

        /// MALACHITE [No.7];
        private static int oport_colorbar_malachite_blue(double x)
        {
            int res;
            if (x < 248.25 / 1066.8) {
                res = 0;
            } else if (x < 384.25 / 1066.8) {
                res = (int)(1066.8 * x - 248.25);
            } else if (x < 0.5) {
                res = 136;
            } else if (x < 595.14 / 1037.9) {
                res = (int)(-1037.9 * x + 595.14);
            } else if (x < 666.68 / 913.22) {
                res = 0;
            } else if (x <= 1.0) {
                res = (int)(913.22 * x - 666.68);
            } else {
                res = 246;
            }
            return res;
        }
        private static int oport_colorbar_malachite_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 253;
            } else if (x < 248.25 / 1066.8) {
                res = (int)(-545.75 * x + 253.36);
            } else if (x < 384.25 / 1066.8) {
                res = (int)(426.18 * x + 19335217.0 / 711200.0);
            } else if (x < 0.5) {
                res = (int)(-385524981.0 / 298300.0 * x + 385524981.0 / 596600.0);
            } else if (x < 666.68 / 913.22) {
                res = (int)(3065810.0 / 3001.0 * x - 1532906.0 / 3001.0);
            } else {
                res = 0;
            }
            return res;
        }
        private static int oport_colorbar_malachite_red(double x)
        {
            int res;
            if (x < 384.25 / 1066.8) {
                res = 0;
            } else if (x < 0.5) {
                res = (int)(1092.0 * x - 99905.0 / 254.0);
            } else if (x < 259.3 / 454.5) {
                res = (int)(1091.9 * x - 478.18);
            } else if (x < 34188.3 / 51989.0) {
                res = (int)(819.2 * x - 322.6);
            } else if (x < 666.68 / 913.22) {
                res = (int)(299.31 * x + 19.283);
            } else {
                res = 0;
            }
            return res;
        }

        /// MORNING_GLORY [No.8];
        private static int oport_colorbar_morning_glory_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x <= 1.0) {
                res = (int)(270.9 * x + 0.7703);
                if (res > 255) {
                    res = 510 - res;
                }
            } else {
                res = 239;
            }
            return res;
        }
        private static int oport_colorbar_morning_glory_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 124;
            } else if (x <= 1.0) {
                res = (int)(180.0 * Math.Sin(x * 3.97 + 9.46) + 131.0);
                if (res < 0) {
                    res = Math.Abs(res);
                } else if (res > 255) {
                    res = 510 - res;
                }
            } else {
                res = 242;
            }
            return res;
        }
        private static int oport_colorbar_morning_glory_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 78;
            } else if (x <= 1.0) {
                res = (int)(95.0 * Math.Sin((x - 0.041) * 7.46) + 106.9);
            } else {
                res = 179;
            }
            return res;
        }

        /// PEANUT_BUTTER_AND_JERRY [No.9];
        private static int oport_colorbar_peanut_butter_and_jerry_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 1;
            } else if (x <= 1.0) {
                res = (int)(407.92 * x + 1.3181);
                if (res > 255) {
                    res = 510 - res;
                }
            } else {
                res = 100;
            }
            return res;
        }
        private static int oport_colorbar_peanut_butter_and_jerry_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x <= 1.0) {
                res = (int)(128.7 * x + 0.2089);
            } else {
                res = 128;
            }
            return res;
        }
        private static int oport_colorbar_peanut_butter_and_jerry_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 73;
            } else if (x <= 1.0) {
                res = (int)(63.0 * Math.Sin(x * 6.21 - 0.3) + 92.0);
            } else {
                res = 69;
            }
            return res;
        }

        /// PURPLE_HAZE [No.10];
        private static int oport_colorbar_purple_haze_red(double x)
        {
            int res;
            double e;
            e = Math.Exp(1.0);
            if (x < 0.0) {
                res = 13;
            } else if (x < e * 0.1) {
                res = (int)(706.48 * x + 13.06);
            } else if (x < e * 0.1 + 149.0 / 510.0) {
                res = (int)(166.35 * x + 28.3);
            } else if (x < e * 0.1 + 298.0 / 510.0) {
                res = (int)(313.65 * x - 47.179);
            } else if (x < e * 0.05 + 202.0 / 255.0) {
                res = (int)(557.93 * x - 310.05);
            } else if (x <= 1.0) {
                res = (int)(319.64 * x + 439093 / 34000 * e - 1030939 / 8500);
            } else {
                res = 249;
            }
            return res;
        }
        private static int oport_colorbar_purple_haze_green(double x)
        {
            int res;
            double e;
            e = Math.Exp(1.0);
            if (x < e * 0.1) {
                res = 0;
            } else if (x < e * 0.1 + 149.0 / 510.0) {
                res = (int)((3166.59 / 14.9 * e + 2098.7 / 74.5) * x - (316.659 / 14.9 * e + 209.87 / 74.5) * e);
            } else if (x < e * 0.1 + 298.0 / 510.0) {
                res = (int)(725.0 * x - 394.35);
            } else if (x <= 1.0) {
                res = (int)(-716.23 * x + 721.38);
            } else {
                res = 5;
            }
            return res;
        }
        private static int oport_colorbar_purple_haze_blue(double x)
        {
            int res;
            double e;
            e = Math.Exp(1.0);
            if (x < 0.0) {
                res = 16;
            } else if (x < e * 0.1) {
                res = (int)(878.72 * x + 16.389);
            } else if (x < e * 0.1 + 149.0 / 510.0) {
                res = (int)(-166.35 * x + 227.7);
            } else if (x < e * 0.1 + 298.0 / 510.0) {
                res = (int)(-317.2 * x + 305.21);
            } else if (x < 1.0) {
                res = (int)((1530.0 / (212.0 - 51.0 * e)) * x + (153.0 * e + 894.0) / (51.0 * e - 212.0));
            } else {
                res = 2;
            }
            return res;
        }

        /// RAINBOW [No.11];
        private static double oport_colorbar_rainbow_red(double x)
        {
            int res;
            if (x < 0) {
                res = 127;
            } else if (x <= OPORT_OP_1P9) {
                res = (int)(1147.5 * (OPORT_OP_1P9 - x));
            } else if (x <= OPORT_OP_5P9) {
                res = 0;
            } else if (x <= OPORT_OP_7P9) {
                res = (int)(1147.5 * (x - OPORT_OP_5P9));
            } else {
                res = 255;
            }
            return res;
        }
        private static double oport_colorbar_rainbow_green(double x)
        {
            int res;
            if (x <= OPORT_OP_1P9) {
                res = 0;
            } else if (x <= OPORT_OP_3P9) {
                res = (int)(1147.5 * (x - OPORT_OP_1P9));
            } else if (x <= OPORT_OP_7P9) {
                res = 255;
            } else if (x <= 1.0) {
                res = 255 - (int)(1147.5 * (x - OPORT_OP_7P9));
            } else {
                res = 0;
            }
            return res;
        }
        private static double oport_colorbar_rainbow_blue(double x)
        {
            int res;
            if (x <= OPORT_OP_3P9) {
                res = 255;
            } else if (x <= OPORT_OP_5P9) {
                res = 255 - (int)(1147.5 * (x - OPORT_OP_3P9));
            } else {
                res = 0;
            }
            return res;
        }

        /// RAINBOW_BANDED [No.12];
        private static double oport_colorbar_rainbow_banded_h(double x)
        {
            int res;
            res = (int)(-277.34 * x + 278.69);
            res = (int)(res / 360.0);
            return res;
        }
        private static double oport_colorbar_rainbow_banded_s(double x)
        {
            int res;
            res = (int)(0.908306127594742);
            return res;
        }
        private static double oport_colorbar_rainbow_banded_v(double x)
        {
            int res;
            res = (int)(0.293 * Math.Sin(x * 79.7 - 1.53) + 0.55);
            return res;
        }

        /// RAINBOW_STRIPED [No.13];
        private static double oport_colorbar_rainbow_striped_h(double x)
        {
            int res;
            if (x < 1303 / 3817) {
                res = (int)((171.99 * x - 241.35) * x + 302.44);
            } else if (x < 21321 / 25489) {
                res = (int)(-364.593271477158 * x + 364.553562528411);
            } else if (x < 44617 / 48806) {
                res = (int)((-1385.4 * x + 1948.5) * x - 600.94);
            } else {
                res = (int)(-262.518169057616 * x + 262.518169057616);
            }
            res = (int)(res / 360.0);
            return res;
        }
        private static double oport_colorbar_rainbow_striped_s(double x)
        {
            int res;
            if (x < 21321 / 25489) {
                res = (int)(1.0);
            } else if (x < 44617 / 48806) {
                res = (int)(-2.5489 * x + 3.1321);
            } else {
                res = (int)(2.3317 * x - 1.3296);
            }
            return res;
        }
        private static double oport_colorbar_rainbow_striped_v(double x)
        {
            int res;
            double xx;
            xx = x % 10.0 / 255.0;
            if (xx < 2 / 256) {
                if (x < 1303 / 3817) {
                    res = (int)(0.6067 * x + 0.5921);
                } else {
                    res = (int)(0.8);
                }
            } else {
                if (x < 1303 / 3817) {
                    res = (int)(0.7634 * x + 0.7394);
                } else {
                    res = (int)(1.0);
                }
            }
            return res;
        }

        /// ROSE [No.14];
        private static int oport_colorbar_rose_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 54;
            } else if (x < 20049 / 82979) {
                res = (int)(829.79 * x + 54.51);
            } else {
                res = 255;
            }
            return res;
        }
        private static int oport_colorbar_rose_green(double x)
        {
            int res;
            if (x < 20049 / 82979) {
                res = 0;
            } else if (x < 327013 / 810990) {
                res = (int)(8546482679670 / 10875673217 * x - 2064961390770 / 10875673217);
            } else if (x <= 1.0) {
                res = (int)(103806720 / 483977 * x + 19607415 / 483977);
            } else {
                res = 255;
            }
            return res;
        }
        private static int oport_colorbar_rose_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 54;
            } else if (x < 7249 / 82979) {
                res = (int)(829.79 * x + 54.51);
            } else if (x < 20049 / 82979) {
                res = 127;
            } else if (x < 327013 / 810990) {
                res = (int)(792.02249341361393720147485376583 * x - 64.364790735602331034989206222672);
            } else {
                res = 255;
            }
            return res;
        }

        /// SATURN [No.15];
        private static int oport_colorbar_saturn_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 255;
            } else if (x < 10873 / 94585) {
                res = (int)(oport_colorbar_saturn_II(x));
                if (res > 255) {
                    res = 510 - res;
                }
            } else if (x < 0.5) {
                res = 255;
            } else if (x < 146169 / 251000) {
                res = (int)(oport_colorbar_saturn_IV(x));
            } else if (x < 197169 / 251000) {
                res = (int)(oport_colorbar_saturn_V(x));
            } else {
                res = 0;
            }
            return res;
        }
        private static int oport_colorbar_saturn_green(double x)
        {
            int res;
            if (x < 10873 / 94585) {
                res = 255;
            } else if (x < 36373 / 94585) {
                res = (int)(oport_colorbar_saturn_II(x));
            } else if (x < 0.5) {
                res = (int)(oport_colorbar_saturn_I(x));
            } else if (x < 197169 / 251000) {
                res = 0;
            } else if (x <= 1.0) {
                res = (int)(Math.Abs(oport_colorbar_saturn_V(x)));
            } else {
                res = 0;
            }
            return res;
        }
        private static int oport_colorbar_saturn_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x < 36373 / 94585) {
                res = (int)(oport_colorbar_saturn_I(x));
            } else if (x < 146169 / 251000) {
                res = (int)(oport_colorbar_saturn_III(x));
            } else if (x <= 1.0) {
                res = (int)(oport_colorbar_saturn_IV(x));
            } else {
                res = 0;
            }
            return res;
        }
        private static double oport_colorbar_saturn_I(double x)
        {
            int res;
            res = (int)(-510.0 * x + 255.0);
            return res;
        }
        private static double oport_colorbar_saturn_II(double x)
        {
            int res;
            res = (int)((-1891.7 * x + 217.46) * x + 255.0);
            return res;
        }
        private static double oport_colorbar_saturn_III(double x)
        {
            int res;
            res = (int)(9.26643676359015e1 * Math.Sin((x - 4.83450094847127d - 1) * 9.93) + 1.35940451627965e2);
            return res;
        }
        private static double oport_colorbar_saturn_IV(double x)
        {
            int res;
            res = (int)(-510.0 * x + 510.0);
            return res;
        }
        private static double oport_colorbar_saturn_V(double x)
        {
            int res;
            double xx;
            xx = x - 197169 / 251000;
            res = (int)((2510.0 * xx - 538.31) * xx);
            return res;
        }

        /// SEISMIC [No.16];
        private static int oport_colorbar_seismic_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 3;
            } else if (x < 0.238) {
                res = (int)((-1810 * x + 414.49) * x + 3.87702);
            } else if (x < 51611 / 108060) {
                res = (int)(344441250 / 323659 * x - 23422005 / 92474);
            } else if (x < 25851 / 34402) {
                res = 255;
            } else if (x <= 1.0) {
                res = (int)(-688.04 * x + 772.02);
            } else {
                res = 83;
            }
            return res;
        }
        private static int oport_colorbar_seismic_green(double x)
        {
            int res;
            double xx;
            if (x < 0.0) {
                res = 0;
            } else if (x < 0.238) {
                res = 0;
            } else if (x < 51611 / 108060) {
                res = (int)(oport_colorbar_seismic_I(x));
            } else if (x < 0.739376978894039) {
                xx = x - 51611 / 108060;
                res = (int)((-914.74 * xx - 734.72) * xx + 255);
            } else {
                res = 0;
            }
            return res;
        }
        private static int oport_colorbar_seismic_blue(double x)
        {
            int res;
            double xx;
            if (x < 0.0) {
                res = 19;
            } else if (x < 0.238) {
                xx = x - 0.238;
                res = (int)(((1624.6 * xx + 1191.4) * xx + 1180.2) * xx + 255);
            } else if (x < 51611 / 108060) {
                res = 255;
            } else if (x < 174.5 / 256) {
                res = (int)(-951.67322673866 * x + 709.532730938451);
            } else if (x < 0.745745353439206) {
                res = (int)(-705.250074130877 * x + 559.620050530617);
            } else if (x <= 1.0) {
                res = (int)((-399.29 * x + 655.71) * x - 233.25);
            } else {
                res = 23;
            }
            return res;
        }
        private static double oport_colorbar_seismic_I(double x)
        {
            int res;
            res = (int)((-2010 * x + 36469683133498 / 14572746475) * x - 40117786837711 / 83272837000);
            return res;
        }

        /// SPACE [No.17];
        private static int oport_colorbar_space_red(double x)
        {
            int res;
            double xx;
            if (x < 37067 / 158860) {
                res = 0;
            } else if (x < 85181 / 230350) {
                xx = x - 37067 / 158860;
                res = (int)((780.25 * xx + 319.71) * xx);
            } else if (x < (Math.Sqrt(3196965649) + 83129) / 310480) {
                res = (int)((1035.33580904442 * x - 82.5380748768798) * x - 52.8985266363330);
            } else if (x < 231408 / 362695) {
                res = (int)(339.41 * x - 33.194);
            } else if (x < 152073 / 222340) {
                res = (int)(1064.8 * x - 496.01);
            } else if (x < 294791 / 397780) {
                res = (int)(397.78 * x - 39.791);
            } else if (x < 491189 / 550980) {
                res = 255;
            } else if (x < 1.0) {
                xx = x - 1.0;
                res = (int)((5509.8 * xx + 597.91) * xx);
            } else {
                res = 255;
            }
            return res;
        }
        private static int oport_colorbar_space_green(double x)
        {
            int res;
            double xx;
            if (x < 0.0) {
                res = 0;
            } else if (x < (-Math.Sqrt(166317494) + 39104) / 183830) {
                res = (int)((-1838.3 * x + 464.36) * x);
            } else if (x < 37067 / 158860) {
                res = (int)(-317.72 * x + 74.134);
            } else if (x < (3.0 * Math.Sqrt(220297369) + 58535) / 155240) {
                res = 0;
            } else if (x < 294791 / 397780) {
                xx = x - (3.0 * Math.Sqrt(220297369) + 58535) / 155240;
                res = (int)((-1945 * xx + 1430.2) * xx);
            } else if (x < 491189 / 550980) {
                res = (int)((-1770 * x + 3.92813840044638e3) * x - 1.84017494792245e3);
            } else {
                res = 255;
            }
            return res;
        }
        private static int oport_colorbar_space_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x < 51987 / 349730) {
                res = (int)(458.79 * x);
            } else if (x < 85181 / 230350) {
                res = (int)(109.06 * x + 51.987);
            } else if (x < (Math.Sqrt(3196965649) + 83129) / 310480) {
                res = (int)(339.41 * x - 33.194);
            } else if (x < (3.0 * Math.Sqrt(220297369) + 58535) / 155240) {
                res = (int)((-1552.4 * x + 1170.7) * x - 92.996);
            } else if (x < 27568 / 38629) {
                res = 0;
            } else if (x < 81692 / 96241) {
                res = (int)(386.29 * x - 275.68);
            } else if (x <= 1.0) {
                res = (int)(1348.7 * x - 1092.6);
            } else {
                res = 255;
            }
            return res;
        }

        /// SUPERNOVA [No.18];
        private static int oport_colorbar_supernova_red(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x < 0.136721748106749) {
                res = (int)(oport_colorbar_supernova_II(x));
            } else if (x < 0.23422409711017) {
                res = (int)(1789.6 * x - 226.52);
            } else if (x < 0.498842730309711) {
                res = (int)(oport_colorbar_supernova_I(x));
            } else if (x < 0.549121259378134) {
                res = (int)(-654.951781800243 * x + 562.838873112072);
            } else if (x < 1.0) {
                res = (int)((3.6897 * x + 11.125) * x + 223.15);
            } else {
                res = 237;
            }
            return res;
        }
        private static int oport_colorbar_supernova_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 154;
            } else if (x < 3.888853260731947d - 2) {
                res = (int)(oport_colorbar_supernova_I(x));
            } else if (x < 0.136721748106749) {
                res = (int)(-1455.86353067466 * x + 217.205447330541);
            } else if (x < 0.330799131955394) {
                res = (int)(oport_colorbar_supernova_II(x));
            } else if (x < 0.498842730309711) {
                res = (int)(1096.6 * x - 310.91);
            } else if (x < 0.549121259378134) {
                res = (int)(oport_colorbar_supernova_I(x));
            } else {
                res = 244;
            }
            return res;
        }
        private static int oport_colorbar_supernova_blue(double x)
        {
            int res;
            if (x < 0.0) {
                res = 93;
            } else if (x < 3.888853260731947d - 2) {
                res = (int)(1734.6 * x + 93.133);
            } else if (x < 0.234224097110170) {
                res = (int)(oport_colorbar_supernova_I(x));
            } else if (x < 0.330799131955394) {
                res = (int)(-1457.96598791534 * x + 534.138211325166);
            } else if (x < 0.549121259378134) {
                res = (int)(oport_colorbar_supernova_II(x));
            } else if (x < 1.0) {
                res = (int)((3.8931 * x + 176.32) * x + 3.1505);
            } else {
                res = 183;
            }
            return res;
        }
        private static double oport_colorbar_supernova_I(double x)
        {
            int res;
            res = (int)((0.3647 * x + 164.02) * x + 154.21);
            return res;
        }
        private static double oport_colorbar_supernova_II(double x)
        {
            int res;
            res = (int)((126.68 * x + 114.35) * x + 0.1551);
            return res;
        }

        /// rflow%rainbow [No.20];
        private static int oport_colorbar_rflow_rainbow_red(double x)
        {
            int res;
            if (x < 0.5) {
                res = 0;
            } else if (x < 0.75) {
                res = (int)(1020 * x - 510);
            } else {
                res = 255;
            }
            return res;
        }
        private static int oport_colorbar_rflow_rainbow_green(double x)
        {
            int res;
            if (x < 0.0) {
                res = 0;
            } else if (x < 0.25) {
                res = (int)(1020 * x);
            } else if (x < 0.75) {
                res = 255;
            } else if (x < 1.0) {
                res = (int)(-1020 * x + 1020);
            } else {
                res = 0;
            }
            return res;
        }
        private static int oport_colorbar_rflow_rainbow_blue(double x)
        {
            int res;
            if (x < 0.25) {
                res = 255;
            } else if (x < 0.5) {
                res = (int)(-1020 * x + 510);
            } else {
                res = 0;
            }
            return res;
        }
    }

}
