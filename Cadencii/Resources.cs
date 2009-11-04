#if JAVA
package org.kbinani.Cadencii;

import java.awt.*;
import org.kbinani.*;
#else
using System;
using System.IO;
using System.Drawing;
using bocoree;

namespace Boare.Cadencii{
#endif
    public class Resources{
        private static readonly String s_stored_alarm_clock = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAutJREFUeNqUkstPE1EUxr/OTFumtIUi" +
            "g6Ao0LSUQEUEIkZAEgILRUPCooREo38Cblz42Bh15RrZsBEICTGE0JAYgxCCCwFDKK8ULFCgUkAeAaaPaWnHewchaNz4Jd887jm/c+6cO6reJwx4jSqDY1EHIBvH8h7F" +
            "0G9/Ha+kLzPPmJF/xUMR2c9JUWSq1InPr5Y7KtIvF5olKYyB/p4DteRtiGNzn2ZLrPBI1GYX19Q1GLXaBGysTi25hrtLpaj4ijsIyg/zb9XXJgk286p3BaOjYzhX2Mhb" +
            "7JW3NY8tSru8nx54ZkbQ19uDsrLrEM7bCsyl9fzXjx1rXDCCEplhMqddY5if9yKp+g2sdjuSdTpMT07iSlERTGlWWG9chKi3Y3jwKWy2bCSbdJmUZch38LHwrsrlcitw" +
            "rtUKI88rnSfGx3EiukZjNIfmUoayTCSKucGhKUkocsBsNkOn0YBRqRQoHA6fFqBrNGaxWCBcc4AylOVkGS2HOytVWfk3S2jiQSikmMrtdmNlext/K7ewCm7ni3nSpoWz" +
            "zqkWc+ewxb68EIsBLE3ISk1VEg8PD0+fqY7icfzY3UWiMTN294NqayEfi5x5jgSAiJphEDsD36mpQXV1NR40NsJgMCjW6/UoLi9HeVmZwlCWix4X9yeurQWQk2M86dbV" +
            "3Y37TU3o6Oo63YFMvB+JgPX5AoTzK7OhBYi/RZ3OTY5llW1SJaWkKDC9n1ifnAwtOY1wX9+mwtACETptoH+rvX0hwecTY78LnBQ5qziJJW1siL62tgXKUJaNHW9NNIRC" +
            "e/tDQ3nptbUpWkHQyLL8B8ySGcnLy4FJh2M6sL7+dhaY/ETXZ8jlM/GQIFzeFkWrsbNzB6LIGjIy1LzJxJFzliMeT3CltdU/0dzseh8IbL4zGkedweAyZekfoyU28zx/" +
            "j2xxyyJJazVAhRUoUAMZOJ6R/zswOwB88Wi1lxiGSQuFQk4SWqIFOGITsRr/JzrDvV8CDACGRzzydYP8EgAAAABJRU5ErkJggg==";
        private static Image s_alarm_clock = null;
        public static Image get_alarm_clock(){
            if( s_alarm_clock == null ){
                byte[] data = Base64.decode( s_stored_alarm_clock );
#if JAVA
                Frame frame = new Frame();
                s_alarm_clock = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_alarm_clock = Image.FromStream( ms );
                }
#endif
            }
            return s_alarm_clock;
        }

        private static readonly String s_stored_arrow_090 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAgxJREFUeNqkVL1rFEEU/83e7O7lUiQQ" +
            "UBMU0UoQG8dKTGHjKYKFghg5tLQStrWwErUVBMFCbeK/oWAq8QMCXk4iWETQ8+Mut3tu3J398r1J9o5cPC188ObNvDfvN+9rVxRFgf8hycvhW2/huC4kse04kFKiImWD" +
            "TB7xvSxNF9M0RaI10jiGJm7ePGoArDHADXLyFuq7FEs+j4tgJ0BRNOg17/KZObW2FmPh9JziM+v/DUCXkiTxGmf3qZWVPoJAo9Xq41J9r2L9n0CsoS+9rLV35dxBtbzc" +
            "QxynA242fVw8tV+xvRgBsUpnKpB39fwh1W5HmJmhYtoFfN9HGPaQZQGBfEL9+G6VjICYLmRaz3M7Hzx5+aY0nFB7VKezjtnZabx49WVL3zarEGKexOIAgJyv5XlepgKq" +
            "/OtfuYCWNiII0zZq6zFy3AzbsrbPQUl5liHa2DD7wHIRODmqwoWOIqOr1mqwKpWdg8SUJglCyrmkjpxAd4IGypZIKAImlpNTU2botgF0lx4j7AdwjlwYGLoE0Ku6FLpl" +
            "Uiip9/Q60nYTuF0MATpLjzZf+PwO7skbZu9XXPx06ALdoPYZXfz8LrJvrfGDxMbo2R0z87AEarZAQYXjM+tHnUeLyIkdyL+/n8TX1uqP+w+Hls6HVdIr2nEoH4nD0iS4" +
            "bVvtYbBpYucvX29GvM5A5W/gtwADAPaeC0Xi4UgBAAAAAElFTkSuQmCC";
        private static Image s_arrow_090 = null;
        public static Image get_arrow_090(){
            if( s_arrow_090 == null ){
                byte[] data = Base64.decode( s_stored_arrow_090 );
#if JAVA
                Frame frame = new Frame();
                s_arrow_090 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_090 = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_090;
        }

        private static readonly String s_stored_arrow_180 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAe9JREFUeNqkUz1oE2EYfu7uuyRKLCFt" +
            "6g+4VNQWWod+mQRRR1En0UFOHKoNCMKNju4SEQsOzsFNcRGl4CS42AzaKhKcsqhk0Etj7u+773y/6+USbOLSF5574b33eX+e906L4xh7MaYeC/c/IFcowMznEzDTBGPM" +
            "oldnqEFtkPy708mIqvHHe0s7BcaYJYSwRwPu9vbYRH1XJI4tEYb2jYtHOHko9LvdxE9cYZQcBoF9+9oJ7jgRQt+HFAJSyv9rkO6UkGvXF3mr9QelkpkUINsYR6T8Jrka" +
            "y8i+b9+5yfnmppMmSFw6e4yrIynBBsdS3jQ1PH/zeTiBIt+9dZpvbTlZh1+Oh/Z3F33XRUj7R1GUxA3DwMx0EYHnDUUMPe9Rfe1tc26uiL6M8aXno+UH6O7PIShPIapM" +
            "Qx6sQMxW4JbL+MkKCKhwNgGN2FD7Pnz82j63coF/aoc4ekDHtxfrzUniaZrW/FfEBomI9Scv7fnVq7zdBwIqajBWpeTd99d3vgBNCaQSzMOLyJ+6ApSPWxSzD61a/MfT" +
            "hupSjVuvxk2A3sazYYGBGbML0OcvW9rMyeRLFO8eVGXnKyacMiug5ikSplLs05dXzqNQWpbv6/URjpK+m6JH3GhQQI2QI+RTmBO0EwQ/RUBcqe31d/4rwAB0lPTXqN6H" +
            "zgAAAABJRU5ErkJggg==";
        private static Image s_arrow_180 = null;
        public static Image get_arrow_180(){
            if( s_arrow_180 == null ){
                byte[] data = Base64.decode( s_stored_arrow_180 );
#if JAVA
                Frame frame = new Frame();
                s_arrow_180 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_180 = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_180;
        }

        private static readonly String s_stored_arrow_270 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAhBJREFUeNqkk79v00AUx78X26GJkzQq" +
            "P4qKqLoQMWMhkBASLOyIAYQywVBGJv6OrFX5sUSoYoCpFagSVKpgac1SFmABofJDpE3t1Mn57LN5F+LEDoWFk55P9967z33P7x2L4xj/M3T1ufTwR8bJGFugyUq5bDpo" +
            "Pp2zdmt6BIikHAdb16/MWEEQQ9cZnqx+/beCMAz/CHiehOuGKJf1A+MZgNNqoVipZAKchwQQpCBGIEQm1nVd+s6OAJ7jIPA5zMrkMEkICd8PEQQaQt8fKXMdCO5nFaih" +
            "TunstXGoUBwAQjgEZsyAGAD8Xhdy7Dp6eqGC3NuHpulo0bV2dtrI56sIOIeUIaIoOvgfUIkWEP0um4wkgSR6EYPQDXCwRMFmsonybZrmR1UQYp2c1rWbF6xPP7sqAdte" +
            "gE7ehBQaaudrluq3qmng9YptU5+sJ7DcgNgkSGPp0Uu7OFXC1r6Gz/EEdgtlfEEB77mBuGxi7dkbW+Wp/AwggdCPbDxfXLbnTlaxN1Ea2uHpSWw8fmEHY5v7XavkVs7V" +
            "+/dmR2rA1Kk6+e7O3qlbH6jccybwfbGpZDew+7Eptp4i+PYOwzc0/phyx05Dv3ivbly9vzmzGsdqVmvlTw+1T1lfAdGVzyA7TnZUKcuduX0ZJ87ewPbGUvT2wavBvjaZ" +
            "ehg8OTgN0MhKZKqT2F9av0fWUYVLAL8EGAB/dRts9he48AAAAABJRU5ErkJggg==";
        private static Image s_arrow_270 = null;
        public static Image get_arrow_270(){
            if( s_arrow_270 == null ){
                byte[] data = Base64.decode( s_stored_arrow_270 );
#if JAVA
                Frame frame = new Frame();
                s_arrow_270 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_270 = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_270;
        }

        private static readonly String s_stored_arrow_skip_090 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAg5JREFUeNp8Uz1vE0EQfffpixNDiLCQ" +
            "USQ+BBINAmlpIyGKJChCShUUMKJM65KKgooSiSYRJangBwBpoIEqFjgoAtmtC8vEoEt8p/N9LTt79vkOnIz0bm539r2Zm9lTOOe48fw7dNOEbhgSmq4TqgBqAi+iMNwW" +
            "QBgECXwfjSfXQaZislUFoba+WGbkaX3MuYkCkvzwboW12wEeLFdOFMkLcF4VJdaqK/Os2XTgOD5aLQfry/OM9il+vACRRaZH9y6yvT0brhuk2N+3cX/pAqP4vyJqwk0y" +
            "P169wjodD3NzJixLQxxzFAoqZmcNdLse1pYuy0p4RkSnRxQECzSNzddf66PAyu2rzLZtKIqOnc+dejaroigLwm2nAoK8Ecfx8Es44ijaPTjoodf7A9M8jUCMTdW0W4KY" +
            "lK2Ov1zPKgsifM+T777Qi1RN+mAwkHumZZFQrompAF0U9+goDfQVA445BUc1ZQVk5IulElRx6XIC7uEh3H4/pxwWp1AQIr6ppRWQ2eK9ODOTF+h+3JIL6+baaL/+5e2n" +
            "bNPSJnrf3iBJdSeJUdNGzSGbXnwKpXyNupltO/ivn3B2nmWuDf+/iWSDdgP6qUtbgsQyp+thu7Ex8Wfg40wkdlbgvLH6arf8gfPSO87PvOec1rQvcI6GMeIRshVoQ4Hp" +
            "qPuj2Xu5OY78bjXFs0LTpQENvbS/AgwAny8I6CJJnqwAAAAASUVORK5CYII=";
        private static Image s_arrow_skip_090 = null;
        public static Image get_arrow_skip_090(){
            if( s_arrow_skip_090 == null ){
                byte[] data = Base64.decode( s_stored_arrow_skip_090 );
#if JAVA
                Frame frame = new Frame();
                s_arrow_skip_090 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_skip_090 = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_skip_090;
        }

        private static readonly String s_stored_arrow_135 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAfJJREFUeNqkU81rE0EU/20+JsaaVout" +
            "TfCDHkQKitQVepJij+KtHosHD3qtJ/+NXAVRxLRFoYIUP5AW1J4qHUSwBw+lBW2lKRgbs9nZ2cmu721Zk1irhwz8ePNm3++372PGCsMQnawEOlypeHP5QfkuGfs/8ZIy" +
            "vhU7b24cawoYY+xrYwXbmL0lCWHhyfymtCxrcd8MfM9DvW7gOI22gN7eNKaerzO5mBKiRPZfAj6qVRP5WmsMDAhMza1JcvnPJc910TAmQhgEdJRvCmiliBSgVnNRqVQo" +
            "EwerqyEunS/YC0tboNonKKaklQvPVQgajahlv6fA6jtOHetbZRwfPAwvmYJOpfHh8zZGhvtsIk+yyL5jZOUvVMKpM3nMvViRJ04XUBVdEeSGwtmRQdv39oq0ZKAgjuTw" +
            "anZJ0n5x4dl72XfyKL5ncxHkTgKF0WHKxGsTaWZAJbx79Fr6ShWpvtts5eN52dPfgx8HDkVY8TI4eGXMpm+ThprcJuBrLQlF/e1TyZEzYMv+2sNZ2dWdRU1kIXJZlKef" +
            "RnHq68ddIr8Fhjg3jmT/UFuD2BejdyYy4/eXu1+GIVv24zjmWfFjsnZvSJKQ5svXgkzi4s2ryF+4js3l6UDee0tnPMMN4m7/KRCL/A2JFvAt+klcx+r0Of8SYACYEhSM" +
            "5HNEuQAAAABJRU5ErkJggg==";
        private static Image s_arrow_135 = null;
        public static Image get_arrow_135(){
            if( s_arrow_135 == null ){
                byte[] data = Base64.decode( s_stored_arrow_135 );
#if JAVA
                Frame frame = new Frame();
                s_arrow_135 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_135 = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_135;
        }

        private static readonly String s_stored_arrow_circle_double = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAuZJREFUeNqkU9tLVGEQn3Pb21HX1V1d" +
            "V0hbL2mBUadwS7qR+FARJXSh3oOgP6GH3noK6rGX7GHtwaKCwCQiiMUoYgsltXDX9dKalz3pupdzvu/cmuOaGQU9dGC+73zfzPxm5jfzMZZlwf98vL0c61u6i5v0F30c" +
            "5SaKgJLHYLJpGFTXddA17ReAYRgxsCzpQk9IIqSUEc8x8OjVPJw9WtcgZ2kgnVHp7IKSNw2YQPW3nxFYe0G0KKIO9Q/OxhnLhEJOA0IMsCNp1GREgattCXoaTh4I7HLw" +
            "Vg8GC/8GoBESAkPrOtVZI2WWiuBy4p2qoTOFhy9TuXdji+V5hTSvympTzz7/HgaM85ZpVmI5GxlQ2tsYdDVnV4rA8xY8GJyMCw68JwTQKD6bzr54HptRVErD8lKheZvf" +
            "2YE+hwzkYZ0DSkhXtchXUaLBaEpO4vnt/SfjpRwRhGXZD0hgcDSRkdpDFfX+cp6gzRHUPiuVoKotHAseHetPppZXDV2/htFvo8SVfB5yKyt2loNTqWUH2tSjbRh99qNs" +
            "ZlBQdGtN0631f7OIpQhCFPsRxRLsFscKa2tRjufFgm5VCLBuJ262karq++UcqTAYzhnwizQ9lzmBkQcFpxM25sOWSDDorZRVEzjQiugzubULr2e/ZiZkEKZ82+sVZP8w" +
            "1rxXxUxQB8d7D0q4R/w7m5rQBubS8nc8D9u6EgClsekvM6OuSjGR5sSpcHfEXV7r60EuJEwVkkUWdp/rlhIKC6bogcTYdAJ9HtvTuA6Ada6q+eJAfGDoo8frTs67vAmz" +
            "M5Kru3imHMmDLOeAkRwHvioPjESfxmmhMIwlzOeHbpQ4yPVfArambYrsOK18uqe0e1vDZUK40UHqgpY9TKZDADdrwYIJ0H71sjR6p892+6wvjkcZ+zUyDGNf2KwGGLev" +
            "GgJtIpSFBIauGVDTcQWqW1r/eGby5IT55tb1rQA4e+BBcaMw/3jF9ovTbfp+CDAAIqquPExvnOoAAAAASUVORK5CYII=";
        private static Image s_arrow_circle_double = null;
        public static Image get_arrow_circle_double(){
            if( s_arrow_circle_double == null ){
                byte[] data = Base64.decode( s_stored_arrow_circle_double );
#if JAVA
                Frame frame = new Frame();
                s_arrow_circle_double = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_circle_double = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_circle_double;
        }

        private static readonly String s_stored_arrow_return = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAn1JREFUeNqEU89PE0EUfrMzsyt0aQQp" +
            "tEiUC1ISi9E2eibGwMG7wRiuHuUfMOHkgUQjR40HgxgPxniAg2cNJioQ/B1LBIz8CpEGbSmdnd0Z34xSGw72JV/e7OzOt9/73huSufEWGOdAGQOHUjgYDbGYzYSQO5he" +
            "RFE0GUoJBhGCQZ0QlQpw1zXL7F+YmNx/79QjUFEEgRD2j5cHU1nMI7h9Zf99rYIurfUw5hwiWUuukcQQFQoShgY7sw+mlkdAa6vEEuDBfk716Kl084mONr/Z49TDPfuN" +
            "UtrCPBeLEra2KnBp4Hj24dSiUfKDaaW6uKNG+88dPcMd14+kBgnqj/yawyZLqSAIlM0Sy7IlyCAYzmXauoliviKRWlzZWs8vbRd2y1j0gTibSWSTyTjcf7wwh125TSl9" +
            "xqSo5FKt8RYpI1j6urk+/371ESHOuMPYWtUDrUCF4WwsEYcn0x/tYeZ6thMsqIgkpY5n6v3wabUgRTDOXHfNqemCKJftevrpvD3MPbfaRlOCreu7JLAXSNv3crEIZt+E" +
            "GTAzBzhkc2aQCDovAwHObhkcx0ECITa/lEIxo5u89mPtLcWF/DU0dhw/tiVEYWiBBFf3FdkOKVVVMJtf3siVug6n3O50R1yqoV/5lQGNLtYaaEjgnylGjSVkOM8Tq6/f" +
            "nU8dSTSt+a0+Tfd1xk72dTaYSUK9DM3gmDlmF5E8BDBzc6LKRVHOTijEt/JmqSfOwGN+Iw+5y0JNQKDKvQighPiJegpoSwmFlF7NbsjnY3flm3uA3MARKZI63Ut6Ll4A" +
            "P9kLhCb+e0+285/Vy1vXcbViCMwd9hGN5tZC/TCXwBhiPNr5LcAA/9RJGqQ+yKYAAAAASUVORK5CYII=";
        private static Image s_arrow_return = null;
        public static Image get_arrow_return(){
            if( s_arrow_return == null ){
                byte[] data = Base64.decode( s_stored_arrow_return );
#if JAVA
                Frame frame = new Frame();
                s_arrow_return = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_return = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_return;
        }

        private static readonly String s_stored_arrow_skip = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAf1JREFUeNqkU79rFEEU/nZvd88LnKQ5" +
            "RAsJgYiFEGFa7SRgBAstFIyNzbVp/B8s06aI1YKKBCQBC7GzvQtiQoxKCNrkTIxs7nL7a2ZnfW9u9zyFQyEPPt68mTffvF9j5XmO04gz+2QTlmWBibTWsG3b2JONxtCJ" +
            "7GVS75RSvpISMkkM0jiGM475JAhQq9dLUxRg8Uf97HEE/FLY7RrNWJi/IEgvUqgL/0XAkimFpN834R4fZ3gwf1HINP2DxKbclwkt1rwxYhtkWdZigihS2NsLcf/mtCHJ" +
            "CxKHDsWje1fF0xfvh/k+vDMrwlCBGzRAjiBIEccZdncl7s7NiGdrm0wChyvZ7UpT0VJ6PYnOQYheGCGMIkiqAUViziqVCr7uu7hxbVq8fru96KTk4HkVsC7lSGX4bmuo" +
            "CQ+65lJ7iTTJ0E+1OZ9q1PBqvd2m9i5Z55prnLMgo00Nb2JQA/F3Qeu358RhpDF11kZn9Y257FarvkPOzdEBKu1SOE/qRuvIOYP6JPDNfzm47HlmHpx++7lxkvtbBu75" +
            "KwbDKbx0y+iaBQQrvrmMn1/8kw+rxt/616zbjctwrj9umWh+fFrSO+t+dvDx918ohqnGaRaYKPaN6MMdqM72Z8TBht5Y2aKtGW5UgYgjYLgEj1At1uMmlIsjCQkh5bV1" +
            "2u/8S4ABAAPWMTdAdgcEAAAAAElFTkSuQmCC";
        private static Image s_arrow_skip = null;
        public static Image get_arrow_skip(){
            if( s_arrow_skip == null ){
                byte[] data = Base64.decode( s_stored_arrow_skip );
#if JAVA
                Frame frame = new Frame();
                s_arrow_skip = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_skip = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_skip;
        }

        private static readonly String s_stored_arrow_skip_180 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAgtJREFUeNqkU71rFEEUfzs3u5cLnsGc" +
            "US5oFb+Cic1cJdhYKSIWKhauhYWugoT9E+y1kFSmsjlIo4UJIimsxEqvMQmoEI1I0HCKya2X/Zqb8b3J3uUiXpoM/PbNm3nf81tLaw27WXz0/ltw+vrAzucNuG0D59zF" +
            "uzMY3GsbrtXrQMmUUsAYA8uyjM7/E9SVUvrdB2EQ9KyAbdO0dmWa+jcuDAuUQNhoNIzs2UK3c5ok/u1rx0QQKEjjGJSUpuQdZ7Dpu+l81x0XS0shDAxwEwDXu+0F6hoK" +
            "D+UUSkE6N85x7N+7WRGLi43MUMGls0dE+4FIOg6D6Zn37Vjiyvkx8fTlAnBynrh1WszPr3UypWkL6r8k/AlD2EAkSQqlwQIkUdSxWVldNzpLo+jRw8lXtZGRPSZTs6Vh" +
            "BXtfZQrWCw7E+/ZCa6gEjf4iJBQsw+dEG2kdmpijfl1sxb/unRPVLxLKxRz8eDZX+3dg+PY1/HiYaQrtBentIVZxiPBk8rl/0rsqlpsACQ4xx3mFCNN58y0CeR1C0YVd" +
            "HoP8qcsAg0dNJYfvuOLb4ypVUNGfXmzN5vuCAdkTKJjVXWLuwCiwExdda/9xw0T5+kFF1T/syINcxsZ+REk3f5bV1ze/dWEoVFHgqI+zy3g+3IWDiCLCoc4RLaqAYGeH" +
            "+WzPeiQkWhKviWUJ7a3d/s5/BRgACwsSLXcLHWcAAAAASUVORK5CYII=";
        private static Image s_arrow_skip_180 = null;
        public static Image get_arrow_skip_180(){
            if( s_arrow_skip_180 == null ){
                byte[] data = Base64.decode( s_stored_arrow_skip_180 );
#if JAVA
                Frame frame = new Frame();
                s_arrow_skip_180 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_arrow_skip_180 = Image.FromStream( ms );
                }
#endif
            }
            return s_arrow_skip_180;
        }

        private static readonly String s_stored_author_list = "iVBORw0KGgoAAAANSUhEUgAAATIAAAQTCAYAAADnDqXxAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAA" +
            "OpgAABdwnLpRPAAAR6RJREFUeF7t3Y214zp2JtAJYBJwAk7ACXQEjsAZOAOn4BQcg5NwFk6m58Hz0MZCAzgHFCmJ1K61alXVJYmfTfK7B5R06//8H78IECBAgAABAgQI" +
            "ECBAgAABAtsC//bHEf/1x+///vP3f/7x51+2W/n7A2p7JzS11UTf76fGsTVoOxMgcFzgP/449K9//q43fP33Px9v9n+O/FSACLIXT5zDCdxJ4F+aAKsV2D/8WZ2VMCtV" +
            "2iu/PhVkr4zZsQQI3EygBFUJrL7yKmHWLy//8c+v1XAqx/5TN99/baqw9u/tbiU82zb6vuu2sl9d7o76atspVWX7S0V2swvRcAm8IrBTMbXP0MpxfcVWq7vy9bq9/r2O" +
            "sYRWu4ytf/9LM4n+2LpPWx3WdtqlcHnOV38JsleuCscSuJnATpC1UyuVWH9sre7+/c8dy599kNUwrMFV/iztlOqvDaFyXK2y2tCq+9S+yvF1LG3QCbKbXYiGS+AVgd0g" +
            "K8FRAqoGSTm+r4LKsrT8Kn+OAqWGW1tNjdpp57UbTLv7v2LoWAIEPiyw84ysLh1LkI0qsj48skGWCZ3MPq8E34dPg+4JEHhFYOdVyxomJcRWy726JKxv62irrX5pORr7" +
            "qEqcLWPLODKhult5vmLqWAIEPiCweh/ZX5rxtMvJdnlYd5k9yG+DrN+nBkz/jKw9prTfB9HoYX/7yuVuBfcBdl0SIHC2QHnFr9789eF7G2Klv1L51IqqBE95O0bZtz7c" +
            "L/u0b4mYvf2i/Xo5PnrrxCjI+r6iNlRkZ18x2iNAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAAB" +
            "AgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBA4HcF/nrS1M9q" +
            "ZzScK9venf5qLN80zt152Z8AgT8ErryJr2x79+QJsl0x+xO4icDVQXN1+1lmIZaVsh+BNwr0N2b5d/0dDaPdL9POKATar836zrRdxlr3G41/ta0eO5v3ap6tUd9HP7fe" +
            "s90/sradAIGFwOpmy1YffQCMgqcNmjqcbN87+832XY1xNt5+zFHARwE6Cj0XJwECJwiMKqKo2VVltbNtVZGMAilT0c3CIhuufWU3q6JGRkfmHlnbToBAQmB2862qj+iG" +
            "bZeIs0poFKD9cX3lFvW7s3QbLQPb/kcV5OxrowpzNfZVpZs4ZXYhQCBbsaxu2uxSbFW1rJaLRwPprONmwZRdao+Md+brKiVAYEMgurlmN+5qOboKuRqOmX1GYRIF6Gw+" +
            "s6pwFNbZNkahGY0vqug2Tp1dCRBYVRz98mqm1YbD6AaeLU1X4Tg6Jtt2v1ScVUWz5elqvG3bK482qFf9zKpiVyYBAj8scGTJ9wkuz8Y+oa5PAjcREGQ3OVGGSYDAXODb" +
            "g2y2bHVOCRAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBD4lMDRVxmvfh/W1e1/ylu/BAhcIHA0yC4YiiYJECBwTECQHXNzFAECXySQ/XD10f3KVGefGc18JvOLqAyF" +
            "AIFvFZj9hIk2gOrYsz+NImqzbW/V5reaGRcBAl8msPppEpkqrAZeO63scjXb/peRGQ4BAt8mMFv2RQGVrbr6pWX2OK9aftuVYjwEvlggGyx9sGWP26m6Vm1+MaGhESDw" +
            "aYFsIJ0RZP1PntgJuU876Z8AgS8WiMIkesUxWoK2S8tVGEbbvpjQ0AgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQ" +
            "IECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAEC" +
            "BAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAi8S+Bf/ujov5vf//nH3//xDZ3/0599" +
            "lv6iX6t9/70bfzuXsq382ukrGku//cq2d8difwI/KfDPf8z6r3/8rjf8X/4Mhf96g8a//tnXfyT6KvuWca72LYFc9vm3QXv1+DrPRJfpXTJjyzZWxleC0S8CBDYESmCV" +
            "Cubbf5UbvIRUCY3Zr7pPCeP+VwnA6PijBpmxZdoulWkZoyDLaNmHQCNQQqxWZKPl5D/8GXRlv3KjlT9L+LU3W6nqaiCWP9t2+m3l3/VXba+0Vfqp/659tPuWdqObvI6h" +
            "tNX/qseXIKz7lQqu/Ir6ruMaHVuOb8f2f//4dwnNMocy/mLRz2sUVPU8lDnWbyytXflapnIdTN2XCDxfoD7fKTdQ+V1v7jrzv/z59XIjlb/X5VtdetalaVnO1X3rM6/6" +
            "73ID1kBsl6z1WVbpq978ZTx1TG2l2O47OyurfWpQtGOp7Wf6rmHfBnsdR+23GNTAr2FaQ7MEXPGo2/s59O22dmXf6i7Mnn9PmuFBgXKT1KqiBlZtqtyc/ZKsDYy+Umq3" +
            "rZZKpVIp7c6exbXt1H1XS+BVe/3xo1Bt6dq++4Dpj61t128E5di2Iowq3tpv/01gVIFmwvzgJeAwAs8RqKHVBkZ0Q9Ubtd5k7c22uvH6G7c8Zyp91QqmfbDf7zsSrxXL" +
            "6GF+f3z9d5lv+bXTd32wX4+tbZXx1uBuq9r6ivCs4q1z6Z+zjewE2XPuNTO5UGC2bGqfTZVlX71py1Ci5dysiqqhWf6sYVn+Xiqc+mC+hET51e47m/7qgXt/fP13W4lm" +
            "+6791LCqbZWv1+qsfztJcS37raxWlW2Z86ztCy8HTRO4h0C9serD51pR1GqjBltbHfUh0/67BE9ps97I7cP88iyt3dbeuHUc5TnS6PlY9kH/7MWA/vhX+l611Qd7ffZW" +
            "XOq8oqV0MSi/+ldZa1vveH/fPa5eoyTwp0C5wepSroZJDbGyS9leq69avZT921cT601Xj+8fRrev2NUH7e0NX8KyLr9KG+2bW+uzptr26NXIejIzlWHfXjl2t+++n35s" +
            "7TeH/hXL1RuN23nXOdXwKm0W93I+/CJAYFMgs6TbbNLuBAgQeK9AZkn33hHpjQABApsCXiXbBLM7AQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIE" +
            "CBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQODHBP66mO9q26eZ" +
            "zhjbGW182kH/BG4rcOYNeGZb7wS967jfaaQvAl8tcOZNfGZb70K745jfZaMfArcQKDdx/d0OePb1ss/qmNmk+7B4pf3ReFfjGo2pbePMsdW+Zu3Xvnbn0B5X53qLC8wg" +
            "CbxLYHQj96HW3qDZbdn92v5XY+lv/uxxvWN/XLadaGwjo9GYZ/1F7asc33VH6OeWAqsbuf3uP7qRomP7m3t1M+62vwqkbGW4O7/Z+PuKaTfEd+d+ywvNoAlcJRBVY7s3" +
            "ehQgVwVZv6SLAmcUNO1yt/37qKKbVYe7gdQvNfsxzCrhq64H7RK4pcA7gmy3apuFzGyJGIXHrDpqQ7r/+2o5uqoyo+XhzhyE2C1vKYP+hEAUZEefH+0urUZB8krfmYps" +
            "VlWNAmQVlkfHefS4T1wn+iTw1QL9zVwDZfT11badJePusi0KuX5cq7G0+87azcw9CqHRHDPfNGbnY1VVfvUFZnAECNxfIArV+8/QDAgQeJRAVHE9arImQ4DAcwVmrzo+" +
            "d8ZmRoAAAQIECBAgQIAAAQIECBAgQIAAAQKXCGTeDHpFx2e91WDnPWllHkfnO3sv2G57u/tX+93jMvv3Lzas3rN3xTWgTQKnCnzirQBXB1n23fVtUKxu/uhNre0JiUJk" +
            "NrZs/3247bS3msepF5XGCLxb4GlBdnWQRF5X958NslEFKsjefXfp760C9QJfVTKzZc5sSbK64TNhUNtdQUTtjCqlnWpt5LJz/BX9C7K33ho6u5PA0SA7GlZnLNX6imO1" +
            "XI0Cbzb/s4LszP6jSmvl0ru334Rm35DudB0bK4H0g/Ds8iQTckcqwFmVc0aQtSEwm2e0dBxdStkgy/R/ZpC57Ak8TiB7g45uytEyMAqy2dJxVCXMsPs2ZsEYVR6j0Dor" +
            "yGZ9R+2vtu/MJxuOj7ugTeg3BY4G2ahCiqqGnaXlzjOyqN/MWKPlZNZp9kxx9PWzg3T2TSRbTf/mHWDWjxDI3qDZEIoqstmNHi3DZmE0W3JmgjDqMxMAs2rwzP5n56i3" +
            "XFWmUcA+4mI2id8VWN0k/VKmD43sMnEnvGZtZoKsVmZHb/xsCEdjObv/o+3NqsBoifq7d4OZEyBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAAB" +
            "AgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAr8hEP3v" +
            "0a8oXNn2K+OKjo3+l/HR/5Ddtjn7H7Rbj6iNvr3ZmEfGq/+ZvbYzO67dvvs/gUf/W3xk1M/xTKPof6vf8Y2uH9s/IHBl2FzZ9lVU0Zhn28/4+ipcsu1HAZUNsmyQZvfr" +
            "x5UZZ2af0n/dL2vUHnPE46prT7sHBaKb9mCzfzvs6vZfHd/o+GjMOzfL6IaZfS3aN3NTZyuiM+Ywqp52qppoPjtjXAVZtp1M0F5xvWnzoEAtsWffldrt0c1Vt/fHZNvu" +
            "v5vOLqZR+5m+oxt7NNdZX6vv3KtTEd2wq4qmPTZqJwrgqHKKrKLLbef4TLhk9umvs8goa52t2CIT2y8S6G+M1Y0y+y6XOSa6wLLjWIXq6jvobhhFgTGqPjLBkb0ZZyEZ" +
            "jSvavqqQqlH755EKNRPwo3MV9Z+1iwyy7fTfVC+6BTX7qsDqu9XOtlnAzcIr+i4ZBdLou+humzvVSCag2pt3pxLpj+tv5p2KIfPNYtZ+5Bd57Qbe6hzP+hoFXdTObF5R" +
            "aGbP56v3oONPEIjCanWyR+EVXWiZwIvGNKqE6g2cGW/2hou+q0f8O9/1s8uWyDcKspndqP/Z11ZfX7Ufbct4RaGVuQ6y1keCO7ombL9IYKfyWQVI+91rNdSrg2zWd1RR" +
            "7YZn5iLP3Jgrt8xNGwVXtjLcGWs2yLJ9R9dO5prJtrEb2Nm5XnR7ajYr0FcdqypkdGNFN9usvShAV8cdbTMTsKMb4kgI7t4wmSA5us+Vx+2YrsL/6BijIN85n5kxZO8r" +
            "+31AoC5XZiV3uz1biYyOmQXhLNRWbazGlOl7xFyPi8I5OnY2ttV39+gmylY3q3ba+WXmGI0pulT7/rLX2e55qOM8w2h2bne+HrnY/gMC2YvxByhMkQCBuwoIsrueOeMm" +
            "QOBvAoLMxUCAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAwH0EMm/YPONV0KNt7B432j8zx/aMXdVG6WPnzba7c7/PVWekBE4W2L3J++5XN1sUDpmp7Ixv9u70VTBl" +
            "Qmv1rvczxhcF3Mw8a59xtg+BWwvs3IijiWZvpux+maCMxvxKOL0aenX8n2rn1hejwRM4KhCFwqhaqJ/jW23r2139e2cJldlXkB29GhxH4KYCNZRGf46qizYk2kCbhVrU" +
            "xmpZ1W/LhNisIpoF6U7gRsHdG66W1lG/u/O46eVn2ATOEdityHYqqz70ZqGVCag+NKMl6M7SbucZ2FUBNPuGcmSe51wZWiFwI4Gzg2xWlbRfj27OvpIZBeJon1VV2Ybo" +
            "TnUWBUzWb7edWRBH7dzo0jNUAucJZG/EzFInU1lFS9BMyL1aFe0E2Ug6Ctbskjhq5+g8z7s6tETgJgJXBtnsRs3eoLOl6Kq6ioIwM99Vv32gR+2twj2aRxR0fVV6k0vO" +
            "MAmcLxDdiLMKarZUXH19duNlK7lRSGWDYlZRZirNVVUW+WXHd1Y7518hWiRAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQI3ERg9hm+6A2d" +
            "V01vZzzRm0lHY5x9ljG7b9nvaL/93No+M21+6pxcda61S+ASgd0bZXf/3UFn3vn/ajDV4z/djiDbvTrsT2AisBtMu/vvwp8ZZFFQCLLds2N/Al8qsLrZ67KoDv3Mz1zO" +
            "OM4KsrPaaefej/lIZWdp+aU3gmHdWyBTlbT79Puv/h3t+8ozqp1+V+Examf2zG6nnfpcrf9m0LcR9XV1BXzvq9foCfwpcHaQ7d7ssyon+vosyFbBUcPlyBijUI4CZzSu" +
            "aOk7Gq8LlwCBgcAsyPpKIVpiZW7U6Gaf3bizqqWvFFeVY18h7cyvH1dmPJmlsyBzSxI4SSBTkWWe64yCLqpiskvLozd8tv8oAEdBFo396JizfZ10+jVD4BkCmSBb3eg7" +
            "z6qOVGTRMXV7FBzf1s5q2ZgJ1mdcfWZB4CSBVQDMlouj8BrtO1rORcPOVlF9BbgKqtnYRpXmWYE4W8pmqltBFl0lthMgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAg" +
            "QIAAAQIECBAg8AmB6P1VZUzRe7ky4z7axuxjQ7M+M/Npj828t2v25uJ2bBkD+xAgcJHA7o3fDyN6Z33dP7tftv3VJxdWwbNqf/Xm31n4vTq/i06rZgn8lsAvBdmouhxV" +
            "ZDsfz4oC7reuJrMl8CGBTJDNPro0C4bZR6B2l3SrZe2RikyQfegi0y2BqwVmz6Bmz476r6+eMWXaWIVV3TYaY7QEzVZVmTFmnqNF87j6PGqfwE8L7FZk2YDob+x63KqS" +
            "Gp2IzPiiSms2lle/HoXpT19YJk/gnQKZoNipuvrqqc5l9Qrf6oWA7PhmlWXb/2hpm5mbiuydV6S+CBwQyAZFFAjR0ioTGEcrst05tGPNjEuQHbiwHELgnQK7IfDq0jKz" +
            "DBxVTrNl3KqamwVWXyXOQroev5rz6th3nkd9Efhpgd0ga2/uupzrg2f29VlARUvL1cP+V4KsD9WMxWipHI3hpy8wkydAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBA" +
            "gAABAgQIECBAgAABAgQIHBGI3qAZbT/S5xXHRG+UXfU5+3B69JnP1ZtvS3+jd/nPPl9a948+B3qFnTYJ3F5g9M76OqnVtjtNfHeOs2Br55x5Z3/mc5eZfe5kbawEPiKw" +
            "e5N/ZJAvdrozx2xlJ8hePCkOJ3CmQL0hZx92Xi2P+nFkl2KjymYWNrM2+3FnAigzx0w7o2VjW8VGf5+dv7ss48+8/rRF4BSBnSCbBcHsedDohu737QNstdTqt2WXZa/M" +
            "cbac3K3I+mdgo5M3ez4m4E651DXyZIFRGOwGxKpCmd2w2ZDLBsluJTWb4047o+CZjXdWvWb3f/I1aG4EXhbYDbL+5u1vxMwScafq+uYgi0IoU0llv2m8fKI1QODJAjtB" +
            "tlpajiqOK6uuV8dyRkW2mvNoCb1bnT75ujM3AqcKRIGwuuGPVFarZ2LRzZ/tbydgRsviUSUVOUXtHHmmduqJ1hiBJwtEN+ho++rhebS03Amruu+ozWjcq2Vf5tjVEnoU" +
            "WqPqc2bXtj1bmkf9P/maNDcCBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBNYCmY+9fMrwio/bfPN8P+F8pseZbX3CQp83Ffj2C293fLv7X33avm08" +
            "o/lmxpjZp7a9s+/V/tr/AYE7XHC7Y9zd/+rT/G3jEWRXn3Htv11g9vm/6CNBs4/j9B8zWrWTmexqWTn6SM7sYzptX6uP+KzG1M4t65YZT+lztd/qo0ftsW01tDo/s239" +
            "8b396FxGY8ucY/sQeFkgc1HPlgujC310M0V9ZMKj3rDZsRxps++jb6O/kaOQzS6zVj7RtugczMJtNvaov+gbQnbOL1+4GiCwuth2KqijN/LOUmt1w83aidpf3fzZAGxD" +
            "b1adZW7q1Vh32z0aQm2lOQrv0Tx2x+auI3CZQHQTtaG2e+Fmv+NHk1uFZbusWt2Mq5tztZyKjssEwE6oRv3tVKVHgrZdJrZ/H1Wqq32jyjY657YT2BLILPmO3Kw7lUE0" +
            "4FH/q1DdCY4onFfLqGxQ7IxnJ8iic5f5BtBXWtE3tshjZ3t03m0nkBaIbobVzbq6UWbtRjf1GcuaqI+dG3x1Y2bnvzOenYorOnf9+DIV8s43oJ190xekHQkcEZhVJP2y" +
            "Ilp+RTdVvUEz+83CY3TsaJz1azOPbDtRdbRqp59DJsxm416di92gHfXxyjyyYztybTqGQFogusHSDSV2nIVm4lC7vCDwznPcL1VfGLZDCeQF3nmR71Zj+VnYcyXwznPc" +
            "L42dGQJvE7j6Qo+Wem+b6I92dPX5XS1vf5TctAkQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECA" +
            "AAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAwCMFrvw/Eq9s+8qTceb/21nbOrPNK+eubQK3FLgybK5s+yrss8Y8C66z" +
            "2j97/t86rrPnqb0HClx98V7d/hWn5Kwxz9o5q/2z5/6t4zp7ntp7iEBbKfQXb7sMqtMdXeDt10bHlGMzbbf7jSqY2saquomOm5222VxXS8DIYmW2M46rXGq7/Ryz5/0h" +
            "t4Bp3F1gFECzm68NkXbefRuZbaNQa/udtTm64aLxtjfrKjwy4+6PX81j1t7qmvmEy5F53/26N/4HCayqiZ1ts4Drq7Dd/UYBNbvRo8potUw689hZP6v+dyrdbOhkrKMx" +
            "RdsfdCuYyp0ForBqlxezSmhU0fXH9Tdq1O+s6omOy4x3dL5eCbJ+yZet+EaBtDuOTCU8MlmF3CpU73ytG/uDBaJlTGYJtLqZjgbSWcdlb8ooIKNL4JVgiPyOVlavjKmv" +
            "pKP5207gowKjamp2849CbxWEs2Xl6CbJjmNWFWbazIRyZik7q+gyy7DIcOcbS6Ya23WJQvWjF6vOCUQ38ey7d7ssmVVJo6/3gTO7oVb77W6rfWT6XoVRFM6zY1/1aUM0" +
            "M4dskK1cIjPBJjsIHBBYVTWZiudAlw4hQIDAuQKC7FxPrREg8AEBQfYBdF0SIECAAAECBAgQIECAQFZg9V6xVRvtK7fR30ftzN5y0beVnYf9CBD4YYFZkI3CqWXKvs1h" +
            "9nwv83YOr8j+8IVp6vcRyN7kswDJfH21zyrEorajKmz1XrvS9pGKbNZnHavgu8+1b6QPEtgNsrr/keXg6phRu6u+VsERvVl0Vull5pTxEmYPukFM5R4CmRtzVBllbvpe" +
            "YFYBjaqjTIUTjSEKlKMVWfS8Ler3HleGURK4kcC7gizqZzfkRlVXtNSMgjVz2qJ5ZAI40499CBDYEFjd/NGNPwqfWddRALTLyFmltQq7UYBElVHfZ8Yimocg27j47Erg" +
            "LIHsjTla/l0RZKPwzIwxCqVoObha3vZL69kzttES/KzzpB0CBBYCmZCoN/msYslUIVE//VKx9jVrexYmoyXnqkqMAjAax24F6mIkQOACgShgopDKBkfUz2r7aok4W272" +
            "QTeqqqLlaBtykUO0DL/g1GmSAIHoxswGVLsk2wmcVf9tCM2Wa33IvBI62UDLLj939nMlEiBwgkBUKWVuymh5tmpjFX5twMyex80CLTPuWfurZ3+Z8e70fcIp1AQBAu8U" +
            "GIXA7Guzqixaws2e42UCaPQ8LHNcNKZ3GuuLAIGLBY6EwsVDuqT5X5nnJXgaJXAHgaff5E+f3x2uMWMkQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAg" +
            "QIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgcILAlf812ZVtnzD1aRP9" +
            "f8Lb7pj9X9dn/wlw32mmvUz/V3pom8DXC1wZNle2fRVsNOYzg6e2lf3f1cuco/Fd5aJdAl8rcPVNcXX7V8BGYxZkV6hrk8CmQLts6m/KdjlUm42qhdExo8oh2m+0nGsr" +
            "ltk4ouNmPLO5rpaVs4poFX6Z4IuMZ+ciCt3NS8PuBO4h0F74/Q07CrVZIGVurL6v2XOe1Tiibdk2o+dSq7Fmnk9lwmo1hpl9NO57XHVGSeBEgdV3/Z1tbZWUuTmjamN1" +
            "E2fDtQ/c3Qrp3UEWBdeRYDzxUtEUge8ViMKqXWrNKqFRRdcf11drUb+zMIyOy4x3dDZ2g/WKimw09kyYWkp+7/1lZG8S2Kl8MuES3VSZyi0Kq8w4dpdfu32eHWSZaiuz" +
            "z5suG90Q+C6BUTWVedZVl207y6FVdZEdx6wq7JeRO8vKs48dtTcz3fl6VDV+15VlNATeLNCGwyiY+vDI3HyjY7Jtt1XbLChXY8r0PVtennHsqirdDaOV2W5bb76sdEfg" +
            "dwV2Q+B3pcycAIGvFRBkX3tqDIwAgayAIMtK2Y8AAQIECBAgQIAAAQIECBAgQOAJAkfe4FnfHpH5c8coej/cqq1Xjt0Zo30JEPgygfa9Yv3Qznjgv2qj9JcJwn6fOs5X" +
            "jv2y02A4BAgcFVhVMGcG0Gh82SowGkfbdrbNo16OI0DgywTamz56Z3q0/cjUonfKj6qtnUCs1V5bvR0Zp2MIEPhygdWSrV++jZad2aVdZskaVV6ZZW5mPF9+SgyPAIFd" +
            "gVFVlgm3vtrZrXpmFV4URJlAzCw1d53sT4DAFwvMQisz5OgVwtp2djl4tCLbWaJm5mUfAgRuJhA9J1tNp321c/U8Kwq8Wt1F1dgqGGdV2Cvzu9mpNFwCvyuwWkZG1drR" +
            "VwhfeeEg0+cqEH/3TJs5gQcLZIIheh6WqaSi51aZNrJL1VkVFi1dH3yaTY3AswWyN/1OFZV5dXH3oX3dPzMOFdmzr1mzI/B3Au1N31Ze0XOtI2E1q+xWX88E3mqsnpG5" +
            "6Ak8XCBaqq2ekUUP+lcP/0esR5eWUYXWj/Php9T0CBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAg" +
            "QIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECJwoMPrfxE9sXlMECBC4XkCQXW+sBwKPE6jB" +
            "Uf7sQ6TdVide91vt224btVvaGrWzajszPiH4uMvThAjkBPqAWIXQLLzaYOoDr/336O/12My20b7CK3ee7UXg0QJROM0C5mgAjYKnD8+jfT76RJkcAQJjgStDJbNM7QNr" +
            "dEy73FxVj84xAQI/KrAKslWllgmglnRWce22M2vzR0+faRMgMHuudfRZ1SpkVkGW3RYtZZ1RAgR+VCDzimUfUKtXD3dCcNbOanmZDcsfPZ2mTeD3BLza93vn3IwJPE5A" +
            "kD3ulJoQgd8TEGS/d87NmAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgMDzBbwS+vxzbIYEHi8gyB5/ik2QwPMFBNnzz7EZEniEQObH6hz9PObouEegmQQBAt8rMPuA" +
            "dx1x9idbZH5k0PcqGBkBArcWiAJIkN369Bo8gecLRD9dtggIsudfB2ZI4LYCs4f6qwrt6LbbIhk4AQLfLSDIvvv8GB0BAoFA5lXL0cP+utRsj2+7mn3dCSFAgAABAgQI" +
            "ECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAAB" +
            "AgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBwL4HyP3fXX7O/32tGRkuAwM8JtOHVTn729XcBndX/We28a976IUDggMDoRv+Gm/+sMZzVzgFahxAg8A6BaFlZ" +
            "ttffo2qtPz7aNxuaq35HfZSxjY7ZbaeOr//zHedCHwQIHBRYBVkfOqPQGj1bq6HSbpsFw6paWvU/6mO2LN5tRwV38GJyGIFPCUQVWTYc+vFn280GWVTJzaq0KPAy2z91" +
            "bvRLgEBSIBs40Q3fLuH6UNnpY1Xhrfroq79VBZgda5LQbgQIfFpgdMPPqqTs8nAWXFEYzqq//rhV9dcHYbSsXPX56XOjfwIEEgKvPB/Lhs4sSKLnUFEArcKyDb5X20kw" +
            "2oUAgU8KREu+dgkWVS79cm22RKwhkwmyUQjNnoWt+s+2E4XeJ8+VvgkQ+DKBKMS+bLiGQ4AAgb8XEGSuCgIEbiuweovEbSdl4AQIECBAgAABAgQIECBAgAABAgQIECBA" +
            "gAABAgQIECBAgAABAgQIECBAgAABAr8jMPrsZvSh8tGPBPLJgN+5ZsyUwFcJRIFVB9v+qKD+Q+3RT7z4qgkbDAECzxNY/WSJzM9E85MpnndNmBGB2wn0P2ZnVF2tKi5B" +
            "drtTbsAEnieweq6lInve+TYjAo8UiCqqV7c/Es2kCBD4PoHZT5wtIxVk33e+jIgAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBA4VWD0zvzV/4o02380qJ22" +
            "d9o9FUBjBAg8U2D3p1Ds7H/GvkLvmdedWRE4TWAnaEqnO/ufte+snZ32TwPTEAEC3yWwGwQ7+5+5ryD7ruvGaAh8jcBO0NRKbPXRpHZiO233y8bZs7edr38NsoEQIHC+" +
            "wOphfPTAvx/N6DOVUftRG7Olq4rs/GtBiwQeI7AKrzrJVXUVbYu2zyCzIfmYE2EiBAgcE8gs/6J9jlZKu+1G+x8TcBQBArcWOCsYRu2c0faoIrs1uMETIHC+wBlhs/s8" +
            "a2cWgmxHy74EflTg1SDbfWEgyzxr99XxZvu3HwECNxMYvcK4eviefetFrdSyb6vYafdmxIZLgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgMCWwOrNrd7XtUVpZwIE" +
            "zhbIvgdLkJ0trz0CBA4LHK2edo5r9z36PrBXArbHybxn7TCoAwkQeL9ADYidN7yWUY4CaRRu7bvvd8Kv72MVZDvBdNXnPd9/5vRIgMDfBPqP+fTVU0s1C4w2DEe0Z3xE" +
            "6YyKLApDlwUBAjcViKqkTAWzCr9aWc14zgioPmyP9JUdx01Ps2ETeLaAIPv/51eQPfs6N7sHC/RLwt3qq9JklqNHqqRspZXdz9LywRezqf2ugCD733OvIvvd+8DMby4Q" +
            "PdsaLbkyVVtfqa1ezcwGyBn7qchufsEaPoGRQB9kUUjNgiD6+itLz9HydXU2o8CL5uhKIUDgZgLR0nL1QkCmmss+Q8uwRQG1E3ilrfZ3pn/7ECDwxQKzCiUTHDUMoumt" +
            "lpbRsbYTIECAAAECBAgQIECAAAECBAgQIECAAAECXyKQefGgDjV6i8ROW18yfcMgQOAJAtnwid6TViyybT3BzRwIEPgigUz4zN6bFlVoXzRNQyFA4MkCmSBrq63+zbqj" +
            "Sizb5pNdzY0AgTcK7IROZnn5xqHrigABAv9foP9Y0OxTAZnl5U4o8idAgMBpApnwGYXdKARHy8zTBqohAgQIzAQyQVaP3dmXOAECBN4msBNOq1cpd9p52+R0RIDAbwhk" +
            "Aqh/pXL0vGz0auZvCJolAQIfFzgSZO2zMO8l+/gpNAACBM4MMlWZ64kAgUsFZm+zWH29H9DszbBtG5dOQuMECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAA" +
            "AQIECBAgkBLY+blh/fvI+g5mP75ntt/O+9JSk7ETAQK/KZD5mWJFJvPxokyQZfb5zTNh1gQIHBaYfVQoGziZH6JYB9eHYeYjT4cn5kACBH5H4F1BlqnofkfdTAkQOFUg" +
            "U1GtKqdXjveM7NRTqTECvyvwShD1z85mP3ts5wWF3z0TZk6AwGGB2XOrM56RRW14Rnb4tDmQAIFW4Mog66U97HftESBwicDsIXwmdFb7ZJaTKrJLTqlGCfyeQCZwqkr/" +
            "cH5VcWVeIPCw//euNzMmQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIPE4jeerH7Pq/d/Svn0eMedjpMhwCBowKrn0qxGzC7+0dBdrS9oxaOI0DgAQL9j/SJ" +
            "PivZT3kneKI3w+609QB6UyBA4EyBGjClzT7Y2gpqFUSjbasxZj4OdeYctUWAwA8IzALslQosG2QqsR+4wEyRwFkCbdXUttlXZO2/zwiy3UpOsJ11xrVD4MECbfW1+4zs" +
            "rJA5q50HnyZTI0Agu6xrn4v1fx+1cUYARQ/9nT0CBAiEAqsH7dGrlq8E2WrJmgnRcGJ2IEDgdwRWYdVXS2c+I6thpSL7nWvNTAlcJvBqVfXKwFZ9vzKuV8bkWAIEbiaQ" +
            "fZvFbFqvho0gu9kFY7gEvk1g9GwsemvE7IH/keMsLb/tijAeAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBA" +
            "gAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIEPgfgVf/F3KMBAgQ+JvApwIl029mH6eSAAEC" +
            "H6uMMiGV2ccpJEDgxwVKUNTflaKGRxsio/3a5WHfRtvWaFsfULNxzI5dtTka/4+fZtMn8HyBWaj0wTb69ywER8/A+mDMtt+egdFY+9B8/hkzQwIE/k5gFQ4jrlkg9eE1" +
            "q9JGITcLq1Vfo/6cXgIEflBg9AxqtexbVWCzgFotX+sx7T6zpWE0Vs/TfvACNmUC0fIv2r4KlmzoREvF1bKyHZ8Qcz0T+GGBaFl5NGiyIXe0fcvKH75oTZ1ALxAtFUdL" +
            "v9pGJgSjZWXUfubVzahydNYJECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECDwbQL9x3Gi91pl3rV+1j7FajW+WT+7X//EOckYjcZ19Lja1qvHf8JK" +
            "nwRCgbMv7Gx72f3aCURveq3B1/65Ov6TN3d2/tn9whPd7HBFmzv925fA6QJnX9TZ9rL7RUE0C7dM6Amy0y8nDRL4jEAmUNp9VvuvPsIz+/hR//WVwqjvV4NstZTu5z0a" +
            "a91nNo9R+6OKcTaOmVsbwkfH9ZkrTq8ELhBob6D+76OKZSfIomVhNiBXldOrQbYKlVm/o4DLWM3mu6oeX9m28s18A7vgctMkgWsEMhd0NnB2Qm5n6RctAduqaBSe0Rx3" +
            "x5INiB2PWei/EmTRN5JrriitEviAQHST9xXLkZtzdMyqklot00ZE7wiyTLWaXTJG+x0Nyuxxo/4/cOnpksB5AlcH2az93SpodfNdHWRHq6L+LL17aakiO+8+0dKXCzwh" +
            "yDIhtzoNUaj2AZStfLIBeHX7swrwyy9NwyOQF+iXTO2/R8+mdpaWo7Znz7t2A3VV7awqkdnSdBVONQhGlV8mBEdL5dFxs/aj4My2v+uSv4rsSeAGApmQmQXUDab3c0Pc" +
            "OZ8/h2PCzxXYufB39n2u2HfPzDn67vNjdCcLjJYsmS7cKBmlz+zj3HzGXa8ECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAgd8QuOLVqCvanJ2NTF+ZfUr7qzf5ztrY/fon" +
            "rqrs/PuxHT2utvPq8Z+w0udNBa642K5oc8Sb7Se7X9tH9M77Gnztn6vjP3lzZ+ef3W/nUr+izZ3+7fsjAldcaFe0+ekgGwXW6KNBs2ATZD9yQ5nmZwSiyqNdbo0qjqOf" +
            "11sdl3lD7Wjc/dKwD49Mu6vAmVlFhm2b/RhGYXjEvF8aR9XlaByzsa3G31emo29i7/rG9pk7SK9fIdBeZNEN2e87O/aKNnus1c0xC7mdqihzQ55RkWXCow+S0TxW5y57" +
            "PrJt9FXnzhy+4qI3iOcJzG7GaCmXuegz34mzN9mrQbaqUKK5zirRzNejcUdGWZ+dYJ9ZZM7pboj2ofe8O8iMvkJgtowZLSd2bqr+u3R/84z6jSrCTBjtVlKzcc6CIarC" +
            "doJpdpO3Nlnzo0vLbPtHQ06QfcVt/vxBZC/k0XIi8509Cqed/qMgi8Ln1aVlazCb16tB9kpgvHo+jva9c46ff0eZ4UcEskHSVy7Ziz7aL9t/tERbfeePbrSdpeVOP5l2" +
            "z/KJ2pktCbP+2fZHPlG4f+TC1+mzBKKbvA2wMy76dgm0U+VFQdYvx7JjnZ3N1c2XrfxmQbYa2ys+rcGqel2d08it/4aWCS5B9qzMMJuTBdwgJ4Ne0JxzdAGqJp8l4Cb5" +
            "/vPpHH3/OTLCLxBwo3zBSZgMwbn53nNjZAQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAAB" +
            "AgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBA" +
            "gAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgMAHBf79j77/e/K7bCu//unP7f95wTivbPuC4WqSAIFvFviXPwb31z9+/9tgkP/657Ya" +
            "bGfOo7b9Hyc0WsZXgtEvAgR+VKCEQAmyvwzmX0KmbCuhc/av2u+rbZdqsYxRkJ19hrRH4EYC//XHWMsS8x8GYy7bapDV/UoFV36V/UuI1OVp2f7PTRt1Wwmq/tiyW227" +
            "BND//eN3Cc3SVmnjHwdtj4Kq7F/GV36Xv5df5fjaX/naGRXfgMaXCBD4JoEaRKMx1aAoYVCCq923Bk8JmPq8q4ZJaaseWyqv/ti6vexTlrTlzxJ8NUxrEJWA+0uzvR9j" +
            "327Zt4RaDa+6bBZm33TFGQuBkwVK5VNu/BIc/a+6rYZTDY3Rvm0wlb/3AdMfW9tuq6m2ImxDsOw7+1WDq74Y0VZ59ZhVUJ/MqTkCBD4hUCuW0cP8PiTqv+uLAuWYEhy1" +
            "emorof7Y+mC/Hlu3l2PqM666ZC0O5e/tsrHd1jr1z9lGoSXIPnFl6ZPAGwVWD9xL6LSvZtZ/l1CplU/5WqmY6osCJaDKr/7Y2k8NpLq9fL1WZ/1bPEqFVped7ZK15ekr" +
            "sD60Zm2/kVhXBAhcLTBaitU++23tv2tglGdYo+djq2NL+6sAqs/eSijWtqPlbBlH+dW/ylrbWi1PrzbWPgECFwtED/rbVzPbfevSr3ytfWNtfc5V9+3/Xacz216Cq3/F" +
            "slRqsyBq+65t1/AqfZQALIHoFwECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQ" +
            "IECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIEDgVwXK/xj+i79+dd6/eK7N+QcEjtzQO8fs7PtO7m8d1zsN9EXg9gLlRh79LhOrX59N" +
            "chYCo69fFRj92HdPyFXj2h2H/QkQOEFgdUOPAi3avx/SkcCIjqnb2/2i8D1jXCdwa4IAgSsEotDYDYC+vd32a0WYqQZf6evIuK7w1yYBAicI7CwTo5AZbT8SGKtlY1+F" +
            "7QZt3f/IuE7g1gQBAmcL7C4T3xlko7lmKrBMQI2Wpmfbao8AgTcJXBFkfdid2Ycge9OFoRsCdxIYPSxvl3WZqija56og210Sz5aUmQruTufUWAn8lEDmWdPoJt+tinaC" +
            "bCdEBdlPXa4mS2AskAmyzMP7KOyyQTarDtvRZ8c8O+dHw881RIDADQSiMOqnEAXKav/MEm/Vfq3aMn3MQjBaDt/glBkiAQKzGzla2vXBsPN8qW17VRnttFkrxqjqy7Y5" +
            "C0lXDAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAh8n8DOe8O+b/RGRIAAgT8EBJnLgACB2wsIstufQhMg8B6BGharzzdG28pIo89OtrNZfUyq" +
            "7Wv0UzhGY4nm8B5JvRAg8DGBPhiygTT6SRZ1EqMA2t22GtcoOHc/5P4xcB0TIHC+wCp0+t6OhFwfOkfbPOu48wW1SIDAxwWiIJv9RIujVVcNttHy8oqxfBzYAAgQuF7g" +
            "aCDd5bjrBfVAgMDHBbKBFD1LO7Ls3H0O1j5ny/b3cWADIEDgeoHscm70rCv7CuPs1cdVm7NXJ9tXKNtga6X6/q5X1AMBAo8QEB6POI0mQeC3BQTZb59/syfwCAFB9ojT" +
            "aBIECBAgQIAAAQIECBAgQIAAAQIECBAg8OsCHvj/+hVg/gRuLjB6A+vNp2T4BAj8ksDo3fa/NH9zJUDgQQKjpWX/0zNmH1eyLH3QhWAqBO4sEIVR5nOdd56/sRMg8AAB" +
            "QfaAk2gKBH5dYBVks2VnaxYF4a/7mj8BAm8QmAVR9uuC7A0nSRcECKwFsoFVW4memfEmQIDA2wW8avl2ch0SIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAEC" +
            "BAgQIECAAAECBAgcFTjj40RntHF0/I4jQODHBc76QYqC7McvJNMn8EkBQfZJfX0TIHCKwCrI2m1txTX6mf6zfcsgfaD8lFOlEQIERgJRwPSBNfp3bTfa1vZvGep6JEDg" +
            "NIFMkK0CqK/SMvsKsdNOn4YIEKgV1OjPtsrKhFNm+XjWszhnjgABAn8TyPzgxJ2KLbuvisxFSIDAKQJRmMyqp1VYRUE2qtpOmYxGCBD4TYGrgqxdpmZeXPhNfbMmQODW" +
            "AlGA3npyBk+AwG8ICLLfOM9mSeCRAqM30D5yoiZFgAABAgQIECBAgACB5wt88hnVqG9vkn3+NWeGBE4XEGSnk2qQAIF3C+wE2eojTDvt1DnO2nu3gf4IELi5QObzlW3w" +
            "zKZ7NMhG7R1p6+anwfAJEHhFIPMxoqNBFrWd+WznK3NzLAECPyCweti+Wym1bWX+XtoXZD9wkZkigasFrgqyNqSyodbO1dLy6jOvfQIPEThSDa0CJlpG9hXYTlsPITcN" +
            "AgTeJXA0YKIqalaZ9fOK2nmXg34IELipQBQiR0Nu9TxMkN30YjFsAt8qcFWQRe1Wj+x+3+pnXAQIfFggEyJH3hC70+6HCXRPgMBdBWo4Zca/s7TMtJvZJzMu+xAgQIAA" +
            "AQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIAAAQIECBAgQIBATiDz5tRcS/+71xVt7o7B/gQI/JCA0Pmhk22qBJ4qIMieembNi8APCdQgm300qP0sZc/SHhP9sMRR+0f7" +
            "bo/7oVNlqgQIzAT6gFn9jLBVWF2xrR1z375K0jVNgMDfBPpAOBJkpbGoIqsdnt2+U0mAAIG/+88+RpXPaHmZDcDsfqMw7H9E0CgMnUICBAiEQbZakq6WfrPQOVqRzfpy" +
            "CgkQILAVZEefg2UCb7U8jbY5jQQI/LjAbPnWVlTRK5pXPCOrba5e7fzxU2f6BAicLeBVxLNFtUeAwOUCq4f4l3euAwIECJwlsHqz7Fl9aIcAAQIECBAgQIAAAQIECBAg" +
            "QIAAAQIECBAgQIAAAQL/K7B6r1j0xlqOBAgQ+AqBWZCNvu4Nsl9xygyCAIFeYCecdvYlTYAAgbcJRD/RtR3I6J3/mc9NCsC3nU4dEfhNgT6IomdmVSn6OFMbkL8pa9YE" +
            "CLxNIAqk3eDq91eNve1U6ojA7wpkgizz4D+zz+8qmzkBApcKREGWfVVz1o6K7NLTp3ECBIrAKsiyz8tW7Qgy1xkBApcLzN702n999uqkVy0vP0U6IECAAAECBAgQIECA" +
            "AAECBAgQIECAAAECBAgQIPBggaNvjdg9bnf/B5ObGgECRwVmb6XIvMVi1Gc2mEbt1/ZW247O03EECDxYIBs8WYJseztvts22mR2j/QgQeJhA9DGkMt3ZZyVfqeaiiqxl" +
            "FmQPu+hMh8DZAkeDbDaObOhkK7Jse2e7aI8AgRsJ1KAYfRxp91lV21ZEIMgiIdsJEEgLnFmR7QbZ6rOclpbpU2hHAgTaIGuDpcqMvjZSywRiNpx223IWCRD4cYFRkLUP" +
            "+N8RZFFweU724xep6ROIBPrlYB9sfahlqrG2msu+KDDqN9NOND/bCRB4uMDomVb2a6OQmb1NYxV+teIbBWa77eGnwvQIEDgqkA2e2dLu6PHRctVS8ugZdRwBAv8jcDSc" +
            "omXgTjjt7Ou0ESBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBwBwGvAt7hLBkjAQJLgUyQ9R/gzh6zQ796P9rqza+ZseyMw74ECNxM4GgIZI6bfUZy5w20hXP18aR+" +
            "+834DZcAgVcERhVWJpxqn1G4jAJm9PGmUXv9vHzO8pUz7VgCPyCwE14tx5VBNgvZ7KcKfuC0mSIBApkqaKQ0e062u1SctT07M6uwPRrErgICBB4isBMCswrsrIfwR8Kq" +
            "XaruzOUhp880CBBYPSObPaSfVXKzMNsJp51923F4fuZaJvDjAtnwyAZbtN/sGdvshYE2bDPL0Z3+f/zUmz6B5wgcCbKdB+7Z9vsgGwVSJqQy+zzn7JkJAQL/I5ANmugV" +
            "yiMP+1ehM9u2Ow7PzVzoBH5EIFthHXkONgvLKPgylVU0biH2IxewaRLoH5rPwmomlQ2L/oWFTHu7z8dmLwA4ywQIECBAgAABAgQIECBAgAABAgQIECBAgAABAgQIECBA" +
            "gACBGwj0b68YvY1jtk99C0f03rIbMBgiAQJ3FsiEUOY9Z5k3wt7ZydgJEPhiAUH2xSfH0AgQyAmcFWSlt2ipmRuRvQgQILApkA2yzLO0Nsw2h2F3AgQIHBfIBFTmGVkd" +
            "wc6+x0ftSAIECDQCmeDJ7GNp6bIiQOBjApmQOrJP5piPTVrHBAg8SyATOEf2yRzzLEmzIUDgYwKZwPGG2I+dHh0TIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECA" +
            "AAECnxXIvLViNMLZj+TJfC6ztjf7APnRMX1WUu8ECJwi0AZANgyy+/UDvDLISl/9+9La/tttp8BphACB7xHY+cGFqw+C123RzEZhk63IjgboKFCz443mYzsBAl8i0Fdl" +
            "UfWyCpRRQKwqpRXBaBxnhdmX0BsGAQJnCfSBES03XwmT6NhZ6M2ejc2WjzuV5lmO2iFA4AMCs8orCoHsUnA2pUyFthrDaNtoTFEgf4BclwQIXCkQLRdr39n92v1Hz9ai" +
            "uewE2Sow+3FHy+ZoXLYTIPDFAtmAyu4XTXW3nSPVVb8cjSrNaMy2EyDw5QKzZ1BRZZRZJpap71RlZy0T2zm9uhz+8tNneAQI1KAZLQdbnagqqmHVi+6GSBRk2TOWfYEg" +
            "2579CBC4gcBquTcKu1lgvbKE2w29zPOxFX005xucNkMkQGBWcUUyO1XTTljMqr6jbayCbqfNyMN2AgS+RGD3xs4+H9t9RtaHa3Zcs6VttGT+En7DIECAAAECBAgQIECA" +
            "AAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAIEXBEaf5cy+/+yFbh1KgACB8wTO+mjTeSPSEgECBDYFBNkmmN0J/JpAu2zr5/7ObasfyyPIfu2qNF8CGwLRzyNrm1r9" +
            "mJ+ztu1+qNxzso2TbVcCTxVYBcFOyM0Cr3z9zJDb+SD7U8+ZeREg0AncKcicPAIECAwFBJkLgwCB2wvsLB/PWiKe8dzt9vAmQIDAuQLvfGWyPjMb/ZDEnVA9V0BrBAgQ" +
            "IECAAAECBAgQIECAAAECBAgQIECAAAECBAjcV8BHhO577oycAIE/BQSZS4EAgdsLrN77NZvc6n1rtwcxAQIE7icwCqXVj9w5Enz3UzFiAgS+XmD2EyjanyfWTmL0Lv5+" +
            "+9dP2gAJEHimwFkfKfKc7ZnXh1kRuIWAILvFaTJIAgRWD+z7baufjrGSVJG5zggQ+IjAKHwE2UdOhU4JEDgqEAXZTrsqsh0t+xIgcIrA6u0Vux0IsV0x+xMgcIrAbAmZ" +
            "CSX/ecgpp0AjBAicLRC9T+zs/rRHgAABAgQIECBAgAABAgQIECBAgAABAgQIECBAgMAvC2TeYvHLPuZOgMANBM4Msv49ZX7g4g0uAEMk8ASBM4Ns5HF1+084B+ZAgMAB" +
            "gVXltPrM5W4o7e5/YCoOIUDg1wWyQXPkp2Bk2/71c2D+BAi8KLATNu2PvY6efe20++IUHE6AwK8LjMJpZtJ+/nJUoUXLVeH261eb+RO4SGD04fA+cEbVV2ap6YPnF500" +
            "zRIg8L8Cqwpptm31QkBrq/pypREg8BaBV4Js9YxMiL3l9OmEAIFIYKci69sSZJGu7QQIXC4QBdGr2y+fgA4IEPhNgejtFLvPv0avYP6mrFkTIECAAAECBAgQIECAAAEC" +
            "BAgQIECAAAECBAgQIECAAIGfE8h8hvBTKNH7tz4xrsznKfu3YrwyzqsNsuc/Gke0/RUDxxIIBbIXctjQBTt8280RjWcWctFxK7pXjj3zlETjiLafORZtEfg7gW8Nsm+8" +
            "MaIxvfIxp9GlGfX3rss5Gke0/V3j1M/DBWbvYs+GWN2vrzhWFcho2047/b6jmyWaVz+/zLKwXAqjdmd91Utn52ZetdWOsW8zM98jTnXOs9sgOhfR9pnpw2870ztTYHQz" +
            "jG6+1Y0YBdgqEEdh0vY/O3YVhKMbL+qnNY3mOtt357goFKJzMDJfjWvlmJlPNLfoOoq2Z/3PvPa19WCB7AW/uvBmF+2sGphVLTsXfxteR/rJVEy77Z5xc7bVTH/ZZbat" +
            "Qj2a8+obUDS36NjVN5YH316mdpVAuxxZfZePviNngm3UV7Za62/IVajs9tNXgLO5XhVko5u6ncNsadcGWfY8RoGfqc5mgbpz7Myyvwavuu61+yCBVcWTCY7Vd/toaRTd" +
            "DLvVWPYGjeY8C41MmKwujUw47sw5qnhmVe4Rp51vYn2/0byjbw4Put1M5SqB7I2zcyFn24yCMmon2p6tDnZupJ0+R+dsdHy2Ih1VbbOwOtLm6nxE5z9yibZH39Suuv61" +
            "+yCBfklyZSVVb5bR8iGqlEY3Z7/86sMr008/ptVNuzP+2SWyGnM0lnY+s1DccczOJwqyTDvZcxX5P+jWMxUCBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECA" +
            "AAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQ" +
            "IECAAAECBAgQIJAV+Kc/dvzvP37/Z/aAi/crYym//SJAgEBa4F//2POvf/z+9/QR1+1YQrWM5b+u60LLBAg8UeA//gyPEmif/vVvXxSqn7bQP4FHC/zDH7Mry8C6BCvV" +
            "yz83M67bSjCVbWW/f2m2l7+Xr5VtdZ9SBZVqqP/VL/P+8uexdRla+q19lH1LKLa/6ljKceVX/feor7K9hmo73sGwfIkAgbsLlJu9hEYJg/p8q32mVP5el4ol9NowKoFS" +
            "ttXAqcExeyZVQqoNubp/aadvq4RP23Zx7oMwev5VQ/Hu58j4CRDYFGjDoQ+u+u/6zKlURCVs/vHPPmoYzYKs7l8Cs7ZVq7E+5PrgKn20z7v6sY2mWdrsq7pNDrsTIHAH" +
            "gfJQvtzwtXppq6AaTDVs6oP88uxpVCHVKmr2imV9ZlWCrP699DFqq/9a/yJCP7aRdQnUGrJ3OBfGSIDAAYFaBZVQKTd8u9QrzdWwqcFVQq8EXX3m1C/t6vF1/35I7XKx" +
            "f4tG31atwGoo1mquBl8dy6yvAxwOIUDgUwLRc6PVc6S67f/+MfjR87F+uTf7dwmoGiyzB/3Fp74douxTftdQKtv6Vzvrs7taUdVndeUFgdJX/ffsQX//wsCnzo9+CRBI" +
            "CLwSZPUVx9JGDYfy9/L8qfyqbff/rsMqIVKXpG0Q1v1Hw68BNFp+1vAq+5R226Cr4yv7lDCLHvT3Y09Q2oUAAQI5gRpkbUjljrQXAQIEvkCgPrD3SuIXnAxDIEBgX6Au" +
            "9b7lM5j7M3AEAQIECBCIBP4fUZljOWq1QqkAAAAASUVORK5CYII=";
        private static Image s_author_list = null;
        public static Image get_author_list(){
            if( s_author_list == null ){
                byte[] data = Base64.decode( s_stored_author_list );
#if JAVA
                Frame frame = new Frame();
                s_author_list = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_author_list = Image.FromStream( ms );
                }
#endif
            }
            return s_author_list;
        }

        private static readonly String s_stored_btn1 = "iVBORw0KGgoAAAANSUhEUgAAAOcAAAAkCAIAAAD96rptAAAABGdBTUEAALGPC/xhBQAABWdJREFUeF7tnL1u2zAQx9NH8Cv4FfwCHTwXWTxld9CpQBYDHTJ1MDx0y+Ap" +
            "8JLBSKcsQTwFyBCgBjKli+Mpg4EMBoIsmej+mXMpmdbHUZSoj1IgCjk+Ssfjz6e7I9VPm83mwB/eAvWyAKj1h7dAvSxwUC91vbbeAjI68FbwFqidBTy1tZsyr7D3tZ6B" +
            "GlqA62sHg0G3210ulxUfIzScTqcVV9KrZ2kBLrVAFrWR+Xxueb+iu3c6nVarVf1fV9F2sL++WK3E9bX49UucnsqGE3xcreyvbH+FRlFLPy0c7Xbb3jT2VxBrIYYWbS5M" +
            "dZi/vKCZ9tLkxd2dODnZHB5GNnwFActbWHZvDrWIYcKVchBsaRr77gLYwcBZG4g31aF9cYFm2issD7eqwSqOj9H0P0KsvKMh1CKWBbLD4ZDc7Xg8RpzQ7/fLM6y8M1Er" +
            "2ubutvvR0ZDa4cMDRo6GkwwDlyHB0RHRiRMxmWjxgBTAH8MyJQUMTaAWUSxCAoq5w/E3YtxyM7MttUDQ8JC8GlK7fn9vnZ8TtTjBR8N7bgIcEcW+vcV1x1fKH8suZRxN" +
            "oHY2mynTaVnj/0Nt7+YGvFKEgBN8NMIpAPHsjNNRQIy8Mi9UeH4RX74L/Bt3cXz1+Zu4+c36hTeB2rAhKlXrcOZrkYGRl6VsTJ1z+JORDLIrEwTpsgHojOQMyHb6G3AZ" +
            "CS4hSwIcnd1Ri2yJo5CljDNqETqv1+tkbZ1Rq/lX5XeZxqRkK8PjnoIK2T3tCHOpgZvwVdxVXVCLiBO5kZuHtTNqaVDh4GTfxG6opSQsHMsiqCV3O358TMNpI1MucrST" +
            "SaqwJiC7UF9GWhZJZwZkoUPh1MJ0SOrdOFotGzOdA1N5lCySh+aAWpWEaXUDhXLqoLB2wCdPp1YRj4swDo3RbMgWTm2v18O8IpdnjCgfEWe+ltRVA4yMFhxQmxAMUNjQ" +
            "v71NCWP+5VUZJ4B8LS+Hwy3CpKpYNiFLi9SqKF9L1ahcNsjDpfENakotHvS5KBkZLRRNrUq8Zs/P+yaaPj1x0jJKqpilgIgoyLy7AjchP0uecU9tbtTub9Iomlrypt2r" +
            "q7g5xlcQ6FxeJkDgnlooc/RDVgwqR632AOU7S0tJU1+by+3iVo8LpVathCUsKCxfX1PTMqq8ZiggkOm2ZQR2hKAhmw3conytooGyMWeLqy6pLTEbi0vC9n+EiGupwhD3" +
            "+8Rmrm02Fr8eFtsXXSiuxUV4h/KyWFCoaDZGA6EY11e+eNP64cDSVnSNKrK00huXlgWVLzZ5aiAB8YzKV9jLqjWwbOAW7mvVCI2SKv4Ea5LOfG2JqwwqCUO+xTEUqrYU" +
            "JyBgiJR3sMoQiSwpkwHcJGoT1vcruz3cGbUcXAqKa1OTsH3dkrsEK7qjEWdcJCMgTOEBY0U3HBjs3yLPFV2giSc7vRcQpgFFSjeOk29BJdl4alUSFuc4I42W6p7VNnBm" +
            "5TXYPYP944zj609ZNEjYHJPn7hnaqIqQVO1bxZKBs9SKYQ1dpNnUhrcj0kPftMWlZTuba9MKAgGy2IbLi2gxT/d/WDtjOJOeHteC0XAd3uVCF2cAZcW1HN22EUJLiK5h" +
            "a0fvr6UkzLIN7u+jo1ttVzheFNutKsidtfhjXXaFg1QCF343daMTZzqLk6mgr3X5Bo6lYaXH3X1jTL5+Q2877r6EI98eY3tZS632u6f7WuoDXmvxjm6lqJUpy1xkb+vc" +
            "HqlG3EiHuveimHpvTHJsXiMzUiBVmEttXf6fgbr8vw2pE1O6gFgsJL6j0dbX4gQfF4vSFYMCXGqroKvXwVuALOCp9STUzwKe2vrNmdfYU+sZqJ8FPLX1mzOv8V+w5d/b" +
            "GqglAgAAAABJRU5ErkJggg==";
        private static Image s_btn1 = null;
        public static Image get_btn1(){
            if( s_btn1 == null ){
                byte[] data = Base64.decode( s_stored_btn1 );
#if JAVA
                Frame frame = new Frame();
                s_btn1 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_btn1 = Image.FromStream( ms );
                }
#endif
            }
            return s_btn1;
        }

        private static readonly String s_stored_btn2 = "iVBORw0KGgoAAAANSUhEUgAAAK8AAAAkCAIAAABwh/05AAAABGdBTUEAALGPC/xhBQAAA3ZJREFUeF7tm9t1wjAMhukIrMAKXYEVWKErsAIrsAIrsAIrsAIjmMpVLCfE" +
            "1iVN0uKjnD5wiGP/lj/r4tCP5/O58cstgBYAGvxyC6AFnAYnIVvAaXAanAZnoGQB9w3OhfsGZ8B9gzPAW8AjhROiixRwGIF/put0ChOe6g+hH/dyCV9f4fOzGxEehM/H" +
            "Y3g8gklz15gGHn4I+z12KvYZTqcpJitO3qIhJEN0o2820Sg6zYPBmRnqV6XfyW63Eg3bbYZgvI5gH3HxXhtUaMgmvlz4PsNutxANjIaw3dLd8QcARW8HbuNbabheB9tU" +
            "L+KlpXJcbAa+4XbrFh5cAnkmuEXfa5WUBo5+hnY87LnrtdgbfA+Tz4uhHXLUboKGn0eik7zdsLtXzel7UdQ8NBQ3lTh2rYGSBvDfxYhwPnc+43Awuof6wOF8xpWOS16D" +
            "t2+FBSZf0xADWSmK5faHg1LOG9Mwe4wTdnaNlTW3gnKjkGmM7eehYVoO+Evf8F9oWHPyxtW1Rq4Gabjfu0gB+azSQ3bNmEhxv1cjxVo0BKWGpCe3h9xWdzVIAyWSkFfp" +
            "jJBaMTRA6MW8gU/RrXt3rO/3GoiGlPxCgqm0Q2s0QF5Jlaf51KGYz0NlQiiIZl2Ghlg16TVQWZEqT81JCeLSGg10EgUeQrkhcjP2vCHWmeI1Iw0lMSoNSEMqd/WPtEYD" +
            "hAa0obm27LZGOnwdrQSV8gIPS9Kg1QAogA/DuKauLVvzDYQCuAdxD5cbjNYy+2dlIjYjDRT+KUboNGQUxkcjkl0aiRSEApzESFOu3y/mDeRy93u55wVoGLh9SUNGQWpZ" +
            "nEsLNBAK5iLixSRFGqBgpXTseFwvUvRGglpRoyGjIGa7lWm8PQ2QIuAiwhs7ee/yLSo7O76bSLf+qsIUNeSgJiLLOEfGPtPc3rSn+jL0PVAFMeWN5XjmTK1PtTvUr3AK" +
            "VLv00u090MuzWEMPNeQKwvLGsmCA96UBUQDLmN9V2lciBm/K5iBwTOpB5bpYnooaEIWIiPpdZVW+hoZaHU7PsoW6+fcyyt7EZtjAcEk7m36+MHiTKeowKIj/+Mb/QqKg" +
            "QRSgNoQqb3AacEEH2RxlauJizEtDP6NEDaKAWWgwzcIbN2ABkydtYL4+Bc4CToPzkS3gNDgNToMzULKA+wbnwn2DM+C+wRngLfANn/WJQzjJ+FQAAAAASUVORK5CYII=" +
            "";
        private static Image s_btn2 = null;
        public static Image get_btn2(){
            if( s_btn2 == null ){
                byte[] data = Base64.decode( s_stored_btn2 );
#if JAVA
                Frame frame = new Frame();
                s_btn2 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_btn2 = Image.FromStream( ms );
                }
#endif
            }
            return s_btn2;
        }

        private static readonly String s_stored_btn3 = "iVBORw0KGgoAAAANSUhEUgAAANkAAAAkCAIAAACL0AjmAAAABGdBTUEAALGPC/xhBQAABKdJREFUeF7tWm1VZDEMZSVgAQtYwAIWsIAFLGABC1jAAhbGAtw9PZvttmly" +
            "kxke3UPm18ybNE1vbj7avl8fHx9X9SkEdkAAXKxPIbADAlc7GFE2FAK/63OhUAhsgkBxcRNHlBmVF4sD2yBQeXEbV/x4Q4qLP54C2wDgcPH5+fnu7q4/e8LPx8fH19dX" +
            "dQnMKVU/UORdQKKaReHLy8vDw8Pt7a1owHc86ZfAKB9kXINdARLbi9h2fX3d9OCLbdhquub30+mkuo830ph9ycX39/ebmxtjDnVVjE2HcRFss5eAf5+ennKn/S7VDIEQ" +
            "tgykdpwgGnsB/LQI4c3XD/dklf/DXAT9JZKQQoBdU4HncDCetElmvXyea2N5eV6yae4dAIPf3t7E2n4J6iqitoV4mcY2F8MYdX9/35MCPxku9jKwGUErSlZV8Uzc9Lwo" +
            "1re0MX9gHJL2tlwEWFKSehb2BiPAWuFeOSbKfpKRaWxzXISnBArJL0OpJTWjqWiqgFuUzQw4uifEaEYFuRJVFe9vXhITSWm2IxiSSJkHczGNbQ5nIRBWKgUND3OrZrzA" +
            "yOhkOJMiw/CoHbw8LynVWc3cfHTxM/I6IXkRtbwSCUvUAZSINhAP/xsuygLsPnfPGi3RHzX+zLgiGZnGNpEXZ/LJ7KvWxWA52PwNNbpvVLGTl72LCzcfr00VL89LyvEN" +
            "b/Zli4ONUhrbBBfnoiyzr5oTA2fpdMmduMuWMfhXA/oDOYkGLACtBtP59hu34XsC0561rmaetTZYl9Izz5LDNoHbvFnptzJ8BCKJChGNDjuaX1guQg4xJIsZGIBWTGWk" +
            "QRTVtby/ec28zu/iYg7bKBelbx4OceTyQk1vNs6rc5WobXoYuIkUMQELhqsLWAyazkUwygNe/iskv5GLbeoQtlF/rzhn7+0MLq5azDG9/VHhUiuQF1VdWIkky/nUlGdM" +
            "NJ/zmvudYxSOqL/P0T+PtbEN2WbXYuOgccZZqrOx+w7ZtgIt826E7M7ma0CeMV/HRbLFdmkUXYurkBEwsA35GztOpquBGJPVpMFljsnSuGW4COtX80Xt4OV5SalB9vWA" +
            "ywx+RldVSICZ15WxL+Jl+JzqVM1oxiSVzvRl2MwgoHOR3CuRKzHscDGVsbwkhghw7hHj8fcuaWz5vChXoEZVXV1NrXAWnRA49EwH8yGprM5u5NRq3lWFGGPk15m+Ic39" +
            "ffTqGrBdqUPtsn3J9uB2Dkhjy3PRcJAoWR00GjjLEHXbmssa/yxKBU4MGt7zg1+lG1MrYIgxX8dFaBZ/wKRhFejJ+n+P52JDKYotz0XxgnESLJubIRptD4r3jfcgoxz4" +
            "S2LVE72rRHX/ZdXDqsLDQxXT1cA52owphrX021J1FOqUUW7SmNp5MY0tyUV5GcLdZ8iOpH9Vwl21FPdVO+5qWAa/ARz8hDjoLwlgB54Y50xbcbEtra2i7+WxIuPV9PNr" +
            "DdOnJ7AluSj+cntldZPnMqnfx6i9r6shw0UG05IpBC6FQPJM51LTl55CwOkXC6BC4HgEKi8ej3nNqCNQXCxm7IJAcXEXT5QdxcXiwC4IFBd38UTZUVwsDuyCwCcAVoIE" +
            "Xz242gAAAABJRU5ErkJggg==";
        private static Image s_btn3 = null;
        public static Image get_btn3(){
            if( s_btn3 == null ){
                byte[] data = Base64.decode( s_stored_btn3 );
#if JAVA
                Frame frame = new Frame();
                s_btn3 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_btn3 = Image.FromStream( ms );
                }
#endif
            }
            return s_btn3;
        }

        private static readonly String s_stored_chevron_small_collapse = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAUJJREFUeNpi/P//PwMlgImBQkCxASww" +
            "RlLLKwYmJqZgRkbGbiBXEYva+0Dvlv7792/tvBoxTAO+fv0MororE6UUDdQ4MXRfuPVdsX3+M5DhaxkYsBjw5s0bEKWoq6zA8OfPHwwDdJXZQGqgLlPGDIN3796CMUjz" +
            "79+/GU5efMVgGr4LTIP4IHGYGqyB+OXLNzAGKTx+/iVDQtUxhqnVhmAaxAeJw9RgNeD/fxYwBtmW1nCBYftMRwZVWRYwDeKDxGFqsMYCOzs/mP779y/DuTVODJ8/f2b4" +
            "9YuBgZOVAcwHGQBTg9UFj0+WMXBzi9w/e/03w48fPxhYWFjgGMQHiYPkQeqwuuDz6zNiHx9vm9A2N6qIgZFZHsOq/38fAuUnAtVJAXnPYMKMsLwATEDcQIoPxCSQ+D4D" +
            "9XzGMGDoZiaAAAMAKfCtInFkoqkAAAAASUVORK5CYII=";
        private static Image s_chevron_small_collapse = null;
        public static Image get_chevron_small_collapse(){
            if( s_chevron_small_collapse == null ){
                byte[] data = Base64.decode( s_stored_chevron_small_collapse );
#if JAVA
                Frame frame = new Frame();
                s_chevron_small_collapse = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_chevron_small_collapse = Image.FromStream( ms );
                }
#endif
            }
            return s_chevron_small_collapse;
        }

        private static readonly String s_stored_clipboard_paste = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAldJREFUeNqMU0trE1EU/uaRycRHEwRj" +
            "i6UYpAERRIgWV+1CtJsiWLALlwXRleBe/A31gVUXogtd1Y2UgiBodaELXZXSoaXEFrWxwrTNTOeVeXjOzWuy84Qv99y59/vON+fekR6NQYQkoRPV3Cntx8HTE5QebT36" +
            "e2J/eb7krgTtPUnSHNUuDU8JFU6O1FbUM+OXihembw/w/OvzB1s/383dQx5ha+93wk1R+OFYR+Db5anpimEYKF5/Al3XkM8fEgt7ezY8L8D261sol8t4P/eCBc4JB1Hc" +
            "tRAEAer1OrZnJqFpmoAsy2g0GmItm82KMc1Ro6g7iWhSKpVgWZaYm/kh+FrThUxQdB3rJBoWBrPeyRFEYdTrgAW4mk4bOeIDeUzdmUFIz4M4hh+GWK9Wsby01OdWrg6T" +
            "mzWZHbTB5DSCoIGEiCHlCSEm+7Zt4+7j+SEiX0ySZFgOyUEHVKFNdl0XFm2W6XwV6gNDz2RgmiZeLSzgbH//7KfFxQk1TPWAiZ7nwXEc+L4PWyp2BNoxPjoKj/bJ1NCP" +
            "huGrYdgV4Kp8CjHZlogYU4NE8/iWpUTYiaQo8FxX7nHAVZmo0KJoKgmxAz5KUu0RUSl3XFdSGykBvp1yi9w+FSHYdpAS4X/XcSR1wwaO57oXPP2+PnWdPxF2INFazG74" +
            "lShn4R3TVNQ3v4Erx4ABrUlSUg4kq44bkyNIh0Q/Y632y+gbvK+q6hZ/TLm3f3D4WqFZKUMNovMVOF9IWsZ6x90Pm7XPu5svKbWEAKG4sY/V2Wdf8D+xk2CVOYTwnwAD" +
            "ALeQLeKXGiz0AAAAAElFTkSuQmCC";
        private static Image s_clipboard_paste = null;
        public static Image get_clipboard_paste(){
            if( s_clipboard_paste == null ){
                byte[] data = Base64.decode( s_stored_clipboard_paste );
#if JAVA
                Frame frame = new Frame();
                s_clipboard_paste = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_clipboard_paste = Image.FromStream( ms );
                }
#endif
            }
            return s_clipboard_paste;
        }

        private static readonly String s_stored_control = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAXhJREFUeNpi/P//PwMlgAWZ4zDvJQMj" +
            "I+NMIPPwnz9/lvz5/Zvh98+fYPzrxw+G6w2m+A34+/cviDKGYhBYQsgFTMgckI0gHOstZQykCxj+/48hyQCYc9+//8MQ7ydv/PvXL4KGYDXg+/c/DJcvf2KI9VMBG/If" +
            "jyEoBoACCoR///4HNOQHw/HjjxiCXIEu+fkTpyFYDXjz7gPD46dPgfQ7hsNH7zC428qCDSEYjb++fwfTr379Z/jAzg1m6ygJMqzddP4sMHonEDYAaDsIvGTlZnjJzcag" +
            "K8nBsG3lfrBmVnb2JYS9AEowQPyClYdBXFqQ4diiHWeBgTgB6P8lP59cIpwSQTEAFmRnYbg+cwnE2e9uL/lyaS3D7+dXgDIlGAYwIucFVu0ABiYNvzMg9v83Nyf8u7F5" +
            "yd9X1+Hy2PINigv+XNvIwSSgco3hx4dL/87NBVmpAcQ/oPg9EP/E6wKgk0EGCkEN/gcyEwn/AKr9g9cAcgBAgAEACsnYjGRDfAUAAAAASUVORK5CYII=";
        private static Image s_control = null;
        public static Image get_control(){
            if( s_control == null ){
                byte[] data = Base64.decode( s_stored_control );
#if JAVA
                Frame frame = new Frame();
                s_control = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_control = Image.FromStream( ms );
                }
#endif
            }
            return s_control;
        }

        private static readonly String s_stored_control_double = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAdZJREFUeNrEk01LG1EUht/R6CgRW8E2" +
            "Uj+w1UXcCQPSVbtxI7qy7mpd6B/IzrW/wIArEaXCgAtXgitREKSL0gRCCBpFBRdqo5RgNJm5M3MzPecmfia7LLzwzsfDPe85Z+Zczfd91LLqUOMK8OXrSgaapi3S457n" +
            "uqYrBJqCQbXhJXdsG8KyUJQS6bmhkoH0PL4ZSqWWTFEoINDYWMGrtsDOrKmxTsN1nAhtnuQs9/zH6COvasBlsW5uJGa+fTIcISL0cSftfF7xbNbD99HeB15hwNlYtu0h" +
            "lbrF9ETYoMAIBahKTk7OsbmZwPBQu+JPTUoG1K+SkLjN29jaPcX4SL/hWFaE+R0Zu1odYskLfB58pzi3VlHBX8tDPGejeyCEn+bvOGWLMr8MBJFpaUPzx05s7aQVd54Z" +
            "lCtISh394Q6sL+/EKUuUsqi/caa/Qf2HEP5s/FKcWjCfzQFn4aW36NheWIvTv49q2WOz+LavtElvxr/VR55LrEOcJ4F5HxqPciA0gIYvszHeXLw+jMr0hikzB6h/H67K" +
            "75eKVYN0le7CZWofIpeQsaWD8vAwB/Ej4jHibNRDypMKXDj7qgqotFZ6aSir2vkoknhc3SfyKNbXXv00/hdgAJGwSvp/yedkAAAAAElFTkSuQmCC";
        private static Image s_control_double = null;
        public static Image get_control_double(){
            if( s_control_double == null ){
                byte[] data = Base64.decode( s_stored_control_double );
#if JAVA
                Frame frame = new Frame();
                s_control_double = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_control_double = Image.FromStream( ms );
                }
#endif
            }
            return s_control_double;
        }

        private static readonly String s_stored_control_double_180 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAclJREFUeNpi/P//PwMlgImBQsACY2g1" +
            "nWVg5+RkYOPgYGBlZ2dgYWWNAQrbAl2YDpL/8fUrhvjBJHGEASjg//+YP79/F8C4f379wiqO3QtARb9//SqI85E2/v3zJwMI//z+HS4e5YEQxzAA6KyYXz9/FiQGKhm/" +
            "ffub4dePH2Cnw8QjPeXh4iCMEgZgRT9+FGTHGxlfvPiBQVCQBWwzSJyRkbHA31neeNu2iwzi4vwQF6EH4q/v3wvyUq2Mz5x7w/Dmw0eGb79YGX5++waSKvB2VTM+ce4J" +
            "AwMjE8OXn39g4qheANo+oXvS3rMCstwMdxlYGV4wcYBtAomv23TxLJ+yDMNLHkGGZ6w8YHGYK5iQvLAE6IoJS2buOquoLsnwiJ0fbBMwsMDix9YfOftPQpzhEacQWBzm" +
            "CrgBb9aWMvx6dnkJyMbD09aeZeCEuODb589w8WdL15/9wcGJ4gJGWFIGBhSYZhbXZGDW8I9hElUHx/fvQ50mf1/dwCr+5+V1lITECsScf19e5wbi48wmqSIM7HxmQM3G" +
            "IEmgGAhfZzZOWc7AwW8AFFcACj9AdgEzNFbYoIaxIMcScnIBmQcKd6DeD4wDnhsBAgwAzWYcBzaIFnUAAAAASUVORK5CYII=";
        private static Image s_control_double_180 = null;
        public static Image get_control_double_180(){
            if( s_control_double_180 == null ){
                byte[] data = Base64.decode( s_stored_control_double_180 );
#if JAVA
                Frame frame = new Frame();
                s_control_double_180 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_control_double_180 = Image.FromStream( ms );
                }
#endif
            }
            return s_control_double_180;
        }

        private static readonly String s_stored_control_pause = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAU1JREFUeNrEk79OwzAQxs/ELQkRUiUG" +
            "hu5dGDMysMErwAoDDwCPAQ/AAG/AMyAGRhYWpM6obcjfpjRpnETmnDRWraRl6MAnnc73i/PpcrEJ5xy20Q5sKVovTp5sIIQ84NJaonfs7rqNvV4eNg2KPBfJujjrW4xx" +
            "eH4Z1Y+s89O+lWUKaxpkaVrm+TyHIMhlLTSb5RBFKmsYsMWizHEsNjNZC7nuFMbjH4U1DNIkqYyY6CCUtZDnTcFxAmQbhpjGcZknfggjP5C1UFL0INMosujvDjzeAd/Y" +
            "x9qWm3zNQEYUtrYDm+6BY3YgX+lg0jXBNXWFre3AQQNf57C7MoNvakKog8IaJ1H8IhGFpoFpUFmLYPj9Wrdi6deHYkDqu4AnjmrHN49wMDiqhjEcFm/3t8jukA2W7BPZ" +
            "Fb7DWg0w9TC6G45+gRG0GvzbbfwVYACMcsm3y4uD/AAAAABJRU5ErkJggg==";
        private static Image s_control_pause = null;
        public static Image get_control_pause(){
            if( s_control_pause == null ){
                byte[] data = Base64.decode( s_stored_control_pause );
#if JAVA
                Frame frame = new Frame();
                s_control_pause = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_control_pause = Image.FromStream( ms );
                }
#endif
            }
            return s_control_pause;
        }

        private static readonly String s_stored_control_skip = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAd5JREFUeNrEk8tLG1EUxr/RSSqapNBF" +
            "VXziQrKfLty0XUg36kJw4aJB1I3b7Pwb3GUj+EYxm0KhdSEUhIq6iIgjCsVoQF34IobERDEzd+5M4rnTTGSoiuCiF77h8pv5zpxzz7lSsVjEa1YFXrnkz3NJSJI0SfsN" +
            "k/OogJWy/BxTSCplPrI2XAvZMk2UoAIqhxtGtBT8Udb/pUH5tnL+UAJnDEIDPQ0KfRgmQ4jl8zYLdf3LNM2C47EDGLoOoUyGY7C3TTEYC5MpJFg6zfG1u9XFdN2C47ED" +
            "ME2DLVbA/v4NhvqCCr0MC5ZKZbG6mkD3x/oyOzo6g+P5G4BSEzIMSo1b2N1Lo7+nXREsk73B7V0em1sn6Oyot5nGOByP3QUnUqFQRCrP0NJcjemFmCpYVvIiV+VDsOUt" +
            "fizv2ey64k35764MjrUCapoCWJz9rRqaFhHs0uPDu7Y6/PoeK7MLr9+VQfkMbn3V+Dm+pFKtEdE2wUy/H7H5ZRdLVgVcZyBVvg/C82l02y4jdRixDpaiovdPMXl4UDHn" +
            "5lW+PvbBTMZpkK4OGnH5Jw49t2upM/HSAIFYAnp2x1JnXYxPTAHpRJx8TUROJXoESF6SR2T0gvEXt0+Mr0HjnJP++228F2AASZRvd11TISIAAAAASUVORK5CYII=";
        private static Image s_control_skip = null;
        public static Image get_control_skip(){
            if( s_control_skip == null ){
                byte[] data = Base64.decode( s_stored_control_skip );
#if JAVA
                Frame frame = new Frame();
                s_control_skip = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_control_skip = Image.FromStream( ms );
                }
#endif
            }
            return s_control_skip;
        }

        private static readonly String s_stored_control_stop = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAeNJREFUeNqkUztLA0EQnou5i29sVPAV" +
            "RBAtrK4XLUWxULBKGhvbVP6OVIrgowmiIFZiWkHExgODGBTLENSLUSImub1H1pnl9vSQWOjAx+7Ot/PtzOyuwjmH/1hUTmZ2n0FRlC2cnruum3EdByKKAmosJnif0xEG" +
            "Hrp2ttov/BEp4HkeYKCOSOEyQT5mWcIvueXZPuJ1WksLBOhEQnJ+QMcxBZwLEataDbhazQvmPwQcxgRKJRuSCyO6Y9tCxMYsJGdZbjBvKsCYBzc3FUgujgkRrDfBajVf" +
            "wGsuQCfZomYOjQaHXK4MK3MowlgKhUQmxeJjsO/HLUgnYw6YpglVrL1Q4LAwG9ePs/cpkaXjhYLDGdTrAsW3dzCZC9VoDManRuDg+NpwLCtN3AeeJ/c1LeElEoNyezfE" +
            "Jwfh6PDSwBLSWEJGcGr7LyX4jSlpHRAf7YLs3omBjyetalqGmkhmImd/a2BIQHY22tkGFxv7IhheHzL15zy0TCwK7lXtCN2AMPoLhEjvBGhLO1cEdXo90dI3+VWnz3Wd" +
            "ck4jrWVckEGjdDfkPuXzwCq5hrF9i64xxBu9cuR6kLt3N/E7lB/yuB5Gf0H8EfkbMeVuHFSE5mtSrtRu2tDqQ6FqCRhXCQn81T4FGAAo72Xu5YvuhAAAAABJRU5ErkJg" +
            "gg==";
        private static Image s_control_stop = null;
        public static Image get_control_stop(){
            if( s_control_stop == null ){
                byte[] data = Base64.decode( s_stored_control_stop );
#if JAVA
                Frame frame = new Frame();
                s_control_stop = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_control_stop = Image.FromStream( ms );
                }
#endif
            }
            return s_control_stop;
        }

        private static readonly String s_stored_control_stop_180 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAfFJREFUeNqkU0tLHEEQrhlnZnXVVRAf" +
            "iwQCimTxIvRZCB68RAVBiIeJhxDIdS75CZ68rDe9iIchERT9AwoRESQwycXHoh68iHHdJWGJO4+e2bGq3WmQwRySho+qrq/q666aHiWOY/ifpSXO67VbUBRlFV2GcFD4" +
            "I8W570MDD9F0HTRNMzE0TtzX9/2iTk0EoiiCMAzZ3EQfI0t7gu95SYqJcSvhkiUFQs4FXLchfe/+/pGMYxP31vxkniVcqgW6Ki3fD6VPVjcMkweB9W5mmFUqgeT+IhA9" +
            "9o3XVFRVFC/MFlipVIPubuN5gaDZaxTFwsdBmThU6+3UCNvdPRHzGBjIybxnBXyfJ74196bADg8vZTLnjZSAHGLgugLXv2rCcs8rft767gyNvoC63irwB1pkXloAlQkV" +
            "NSMs9m5jv8WdzSOndygP1WwOKnpW5qUFcDiEO6NdWK9epznYKFTc/7LndOZ7oJzpkHkpAZouoaq1Sb/2zQZ+c2yHKHK2vu0E2XbJpYbo7i2CNv4JYlWl60N4sASNu5Lg" +
            "WvoKtvpqGsor65Z4dMjhw3wqgMmD4c/Tk5tl/B2qF+e4L2C4ivCi8lkH4ofKPmxApmsMuZcYv6I6Jfkb8Zvn6DCE0bT0Xut0YDPW1jyQ9gHW/X4i8K/rQYABAObvVqib" +
            "xFxaAAAAAElFTkSuQmCC";
        private static Image s_control_stop_180 = null;
        public static Image get_control_stop_180(){
            if( s_control_stop_180 == null ){
                byte[] data = Base64.decode( s_stored_control_stop_180 );
#if JAVA
                Frame frame = new Frame();
                s_control_stop_180 = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_control_stop_180 = Image.FromStream( ms );
                }
#endif
            }
            return s_control_stop_180;
        }

        private static readonly String s_stored_cross_small = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAPpJREFUeNpi/P//PwMlgImBQjAMDGBB" +
            "5qxkZGRgZGDwADJbgLjmDwPDDhYkPjC4d4SjBTqKC4AaGH4DFYeWlBiDaKDSamT+H2xOAEUjDM8H8ucBbZzLwHDmR3Lyf2R6IQOD8SI09SCM4oVvEGoHEBvPnTuXIdrb" +
            "G0SfBbpk6Q8GhrP/CAXiV4ghHt8ZGAKDDQ2N52zdehZEAzVH/4eEBX4vKAH5zUDnPpSV/Q+ikxkYCpH5Sli8gMphYJCzZmBIygQqlmVg8Afype0ZGKJAfCsGhhQgXxHd" +
            "AEbkvMDIyMgPpDigXvsFiRQwmx1K/wCqf4/sA8ahn5kAAgwAg/CW/Vmop9QAAAAASUVORK5CYII=";
        private static Image s_cross_small = null;
        public static Image get_cross_small(){
            if( s_cross_small == null ){
                byte[] data = Base64.decode( s_stored_cross_small );
#if JAVA
                Frame frame = new Frame();
                s_cross_small = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_cross_small = Image.FromStream( ms );
                }
#endif
            }
            return s_cross_small;
        }

        private static readonly String s_stored_disk = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAZxJREFUeNqkk79Lw1AQxy8vL5IORTeL" +
            "QxBLnV3qopC/wsHBQbro4uCkm6NTFXEQQRxEBGc3p6KO0kkQbRelxkGkYK3lNcl595rWFLWtGLi8/Ljv533vLjEQEf5zyJzcpsVYolPaORm7H0T0MP+UoaUMgHuytT9m" +
            "1o5yq5NzSejnx6C4899gc+Egrx0ghLwKpRT4VE4QBD0BpmkC55JORAC9pwjDEBSJm81mT4BFwbmkE3EHpu/7GtDoA0AhgHNJZ2pAiNqyydYVkfsBhGXpMkkXASIH/PDd" +
            "UzA0avcEcI4GtB3ESyidPg40+0QiESsBvkooFovw24dlGEbn2nVd2ra7BMkOeEQzK+lvk7Co7qudMjiOA5VKRTeRdDLuQLIDKSXUag0o5G+0cHp5omOZ3/Fh23bUg6AF" +
            "CMDXmwgaDyfV6wpSqZRO5ms9OjS7APwdkM7SU7nAY3jG0u7W+v75i/daVQp1MgdiK9rP4gCFH0nWcmccipE0ZLPjMJUryMPLn5ro+ouz8fsqeLfXcLbBgGEG8yT++Cdz" +
            "8xqfAgwAOI7a+1YYTicAAAAASUVORK5CYII=";
        private static Image s_disk = null;
        public static Image get_disk(){
            if( s_disk == null ){
                byte[] data = Base64.decode( s_stored_disk );
#if JAVA
                Frame frame = new Frame();
                s_disk = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_disk = Image.FromStream( ms );
                }
#endif
            }
            return s_disk;
        }

        private static readonly String s_stored_disk__plus = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAiVJREFUeNqUkk1oE0EYht/dndQEXCKG" +
            "kiBlW4w/RawopR6sEjyI1aNSEG+2BysWQU8WL8GDFgSL1YP2oKBUS4WcBKGCUrU3wxpEEawXNcbaWFKoNUx2Z/xm0tSUxKIffMzf9z7zzo/RYw0BMKDCebABq0VSnr5F" +
            "TTtluuf4hZO3Ry6CLa31UcYpP6wGWFuyDx47fLR57G6qiYYu5U0m9ZLcfP5e77kt3TZkHWHf1Fmkf2SwdU0z5v15tISbouOD15JUvIsAQtWYnHN4UsL3/RrAq68uwvsi" +
            "up/38mjsiqARkeiz1NN2Aug9TSEEOIlLpVINYIe9Ha8nMrrv7TcgUj4+zX75jvUiXXFgeZ6nAcU6gMt7Lum2a+wQ5ugI4aKNI4P9DzMHnvczIbVlS1nn5KIeoBLbGlrx" +
            "7v4bbOKtIJ3VNtEJJpYcKMDPHEdDNPhXwJXuYfAZDzOPC7iOUUvNrTjC9Phn/EuEQiG6OVEGCPw5guu6kFLWFRmGsdxPJBK0rV8BaAdMObAsC51n4jUvEQgEMDX8EY7j" +
            "IJvNQtWSjlU7YMoBYwwLC0VMXn2rhbtPbVy2rNZUBINB/VdIVwb48PQmpmnqosVFjlgspotVX/9Taa0AqD9DuoD+QC/kKL7J6RtDAyNP8rm5AudSF6uUspyVuWoAl79s" +
            "pVU341Cui6OjowU7eyfZnZf1LjHhndhbPS4g9z6NR0kFCCuwegn8X6jLK/4WYABx/vhNCZSgLgAAAABJRU5ErkJggg==";
        private static Image s_disk__plus = null;
        public static Image get_disk__plus(){
            if( s_disk__plus == null ){
                byte[] data = Base64.decode( s_stored_disk__plus );
#if JAVA
                Frame frame = new Frame();
                s_disk__plus = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_disk__plus = Image.FromStream( ms );
                }
#endif
            }
            return s_disk__plus;
        }

        private static readonly String s_stored_documents = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAfJJREFUeNp8k79Lw0AUx1+Sq0U7+IMu" +
            "1U1wEVwLDjroLI6C+Ac4iou4COIfIA5C/wCFjuLg4OAmIqJDN6VaI1qlSKWSkDRJr77vJWljlR68JNy9+7zPvSTaTrFImqapkFISBj8v8S1H/4+LVhCU/SCgdrtNIrmi" +
            "MyTgBR7j2ysrhSY/ewz1Wy2SnGyaJp2XSutRelnt+cVmgG4YpOi8UfJdeh55rku2bZNlWbS3ulrg9UXOnsKW2GCJdXKRPhmGMVd6fqaAK4+PjdFQOq2S6vU6HZ+d0Uw2" +
            "WzgoFjd5al9ITuqn/Pb5SRPZrILM5vOEnEwmQ9em6aojQLef8vz0NL3UamQ7Dg2mUpQWgnRdp2azqXGQ8H0/Mmd1jgHR7WusjFGpVGh5YSFsNgM8Bqge+FwRALwBLPC7" +
            "7ECSyvePj50C3CNloADQUAC+IIF6IHFFHDEugDzXdUOAFwG0hEEvBPNodlwAxf4aAJAwSEKgLBMGyHMcJwSwSvQJRUcIS/6CYB4GgkFtzHG8V6vi9eqqaxD/D0b34B0I" +
            "lHcPD0/XNjYu42VpWd9WrTYpPp6eaDiXU7Whht+pF6KH3b9t3Nwc9fxYjn53cjICnfgzxjkR0EWkoh7UGw0bGziqifjCxuG3h4fvkXx+i/oMKPNtFJuS8z8CDAAJaBuW" +
            "dfNkLgAAAABJRU5ErkJggg==";
        private static Image s_documents = null;
        public static Image get_documents(){
            if( s_documents == null ){
                byte[] data = Base64.decode( s_stored_documents );
#if JAVA
                Frame frame = new Frame();
                s_documents = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_documents = Image.FromStream( ms );
                }
#endif
            }
            return s_documents;
        }

        private static readonly String s_stored_drive = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAVRJREFUeNrEk89Kw0AQxifJBnMoluYP" +
            "oRASoZcWc6tv4BuYe9/Au2/QPkAfx6s3c/DQUsFKLkKFiuRQ0mR3E3c2RLAg2ObgwMduYL7fzCwTpaoqaBMqtIzWABJFEWiadiXu4yO9Mef8kYg3sB3HuZ5Op7P9fg9F" +
            "kUOe16K0AMZKkasIaVhPStcNmM9nd9vte0IYY85wOByJE9I0lQAE5TnCDgG6BPR6NgTBYLTZvDmqMLr9ft8/dnbTdHz0EkqpGwQBfkAzwu8dcNkB5rqu56OXiCTb87yB" +
            "ZVnQ7Z5LE86Pp0gAzhuA+uMdGOMD9CLAEAHr9Ys0oLERpQzK8hBQQ0zTxhwDAVqWZbBaPcNyuZDtNcLq9aKq35VRn7MEJg+3CNDIbrfT4zhOwjC8CMPLP3WQ3VNYvD4l" +
            "6FU6nc4NIWR8yhaKLmNEnwlZJ27yh/Lvf+OXAAMAVqHijIDHTKcAAAAASUVORK5CYII=";
        private static Image s_drive = null;
        public static Image get_drive(){
            if( s_drive == null ){
                byte[] data = Base64.decode( s_stored_drive );
#if JAVA
                Frame frame = new Frame();
                s_drive = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_drive = Image.FromStream( ms );
                }
#endif
            }
            return s_drive;
        }

        private static readonly String s_stored_edit_list_order = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAQpJREFUeNpi/P//PwMlgImBQsAIBCC6" +
            "EYi1gDiUXANAYA0QhyDJtQCxLtSVjFjoy0BczIJsGJrheklJSb4fP35k+Pv3LxgzM3My8POLM3BwcDHMnNnJClLEgic8rs6bN48Fi+0wfAnZC+uRFPmRGwZkARZyNCGn" +
            "HZgBnaBAA+JnQJxMrhe4gHgjELtC+ROB2AhLADIDXcAGpC8CcQzMBTJAXA3Ea5EMNwZGozUoGt+/f8/AwsLGwMMjxMDLKwyOxhkzOoSQXbAKiGcC8V6o/6SA4u1AWh3N" +
            "VnYg5gBiUBq4BsRJIN1CQMXrYE4EKgwD0p+BmJeIIPgGMoATqhjmnc9QTFwgUpqdAQIMAF6eQybUK9N3AAAAAElFTkSuQmCC";
        private static Image s_edit_list_order = null;
        public static Image get_edit_list_order(){
            if( s_edit_list_order == null ){
                byte[] data = Base64.decode( s_stored_edit_list_order );
#if JAVA
                Frame frame = new Frame();
                s_edit_list_order = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_edit_list_order = Image.FromStream( ms );
                }
#endif
            }
            return s_edit_list_order;
        }

        private static readonly String s_stored_end_marker = "iVBORw0KGgoAAAANSUhEUgAAAAYAAAAOCAYAAAAMn20lAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAKUlEQVR4nGNgwAL+////H6sghsR/JIBVEC6BLvj/////WAWJt3zw" +
            "SwAA7CiTbU98WiAAAAAASUVORK5CYII=";
        private static Image s_end_marker = null;
        public static Image get_end_marker(){
            if( s_end_marker == null ){
                byte[] data = Base64.decode( s_stored_end_marker );
#if JAVA
                Frame frame = new Frame();
                s_end_marker = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_end_marker = Image.FromStream( ms );
                }
#endif
            }
            return s_end_marker;
        }

        private static readonly String s_stored_eraser = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAkBJREFUeNqEkj2IE0EUx99sPo5skY/C" +
            "JhAhEktJI6wiJhhykly4whxWdloEBCGQIhySw+PgSJNCBI8VQQQVK+FuT69RU+o1B+k0RT5KIdmYIskm2d3xzbgb1ruoD/7MsG9///fmzRDAiEQikEwmIRaLgdfrBY/H" +
            "AxfLz0DAHEEJQCQKkAagBNePOsBXAyjoKHc+n4doNMrBv8QVE2h67cOrHUIIKNk7tu8XlhTC4TD8I66aADezysudQb8PqqrC+tHrbTTMYO4aN/gfvLb/YltFmFJs3jS5" +
            "ya2jN1sGQA7/ub7UAH+Of9u9dyn77vkjtdfjMEXYNmmdnMDq3u4mmqwLy+DpdCoVi0VZX73MQbBgtu+327/3lA/RJZyGNU2TSqWSPJvNwDAMEG+nFyY9Cw4EAnBwf/Mx" +
            "fj0UnPBkMpHK5bKMHXDY1kr+BvxotdDD5LDy4OETvMYD7KMuOCtXKhUZ1z9gpjZWNjdSHD4sbj3Fs+8j9pmxbrtytVqVx+PxmYF2u10QBAH8fj9svN0r3CW0TSj5ZOfd" +
            "rHKtVpNHo9EZuNPpLOBcLlfQdf0Y76HBXtF30KBJNXDbIGvVGaxtG06lUhwWRbHRRPA9/clhFiSTycTRRFIURR4Oh/xjCwfGnm0oFIJEIlFA82OXy9Vgw2UzcgYJBoOs" +
            "ShyrSfV6XR4MBoukJEkcxm2D3cCyYMcJoc75fL4LWPX8fD7nx7EeS5M1hJqjZtbKpLNTo0xm4EOJqBV2o5ZMhwzHapzK0V8CDABOsVf4lznYswAAAABJRU5ErkJggg==" +
            "";
        private static Image s_eraser = null;
        public static Image get_eraser(){
            if( s_eraser == null ){
                byte[] data = Base64.decode( s_stored_eraser );
#if JAVA
                Frame frame = new Frame();
                s_eraser = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_eraser = Image.FromStream( ms );
                }
#endif
            }
            return s_eraser;
        }

        private static readonly String s_stored_folder__plus = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAfRJREFUeNqMUr9Pk1EUPe99r6a1qKH8" +
            "KBJiOxgHF5oQFlmQARYHdTMaJuNMQzQmDk7udhBCbGIYWHEhDSQY/gAlgUQwNkaLMWoUmgC1tl/fD+/72vpBWlpecvN+nvPOPfeybIrDDsZwl6ZLON34agwW7EKY2gkd" +
            "xK7cf/0MSrWGOg6y6VtP6luhzf8rDlmBqVRa4lkgAMJwn0D7BJp+N20UMM5BGJ9A+QSOOQUBiIAwTjMCbsFayTZ4j6C5Aq0qULKNB0J4CureCXmUQJICebICETqL37kd" +
            "yGoKCarcBrcKakEeSC+FZsGpfL++7JBCheHH8hG9v2dViHQ4iA7i2w4FxvNbsxgMRvH84njD75pyLx4eYuByHEXXBfF4RopX54M4Q8uIQfTGyATermWgemQT8xzqsWqK" +
            "hVKpnga5aTDnKryDq9m+2gdoM/ZhHsncMqypfkhIWU3nT7lsG9axFRco66Hrt8eGLNuu3EVk9Kr34+bKe+h+vydsiVWtTx7kevBQE9amgJJeX1tYRZ8y8e47iS6e+eEB" +
            "Bjt6jzWV0Qolkm7nvb0i/rosuP3NEkwfTNH9OfM0/DGv8ugtG7y5NtnoAeMoFArenFy6sLicNYsvV3WI0V2Eor/zpngR6GOJGHAw6eqtVs306Sc+pzJqhpbf/wkwAGyi" +
            "E72nxm3yAAAAAElFTkSuQmCC";
        private static Image s_folder__plus = null;
        public static Image get_folder__plus(){
            if( s_folder__plus == null ){
                byte[] data = Base64.decode( s_stored_folder__plus );
#if JAVA
                Frame frame = new Frame();
                s_folder__plus = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_folder__plus = Image.FromStream( ms );
                }
#endif
            }
            return s_folder__plus;
        }

        private static readonly String s_stored_folder = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAYNJREFUeNqMUz1LA0EQnf04MCIIgiIi" +
            "KhKsbfwTdtZaioWN2Kgglv6ANFaWClaxEfFPiKmDxMRCJJLGnGeSm911Zi/xUtwlWRh2b2fe2/dm90S1JIGHELBL0wpMNt6dg1teaNffoY3Vjf37SzBmNFQpqF7vnA8+" +
            "tXX/KQkYg4vjkXgRBEAYmRLYlMDS6W6MAiElECYlMCmBchMQABEQRmURSAZbg2PwniBbgTUxGBzTA629gkHvNA4TICnAfAW6MA1f9QZgYmGTbq4iWUE/qAfoLWSFpOtr" +
            "vjVIoYGtMzyh+j1WodEMK4jB5liw5D1qt2G5uAZRrweEUxkWkE7AnOYpemOJxbDTGdigJg4r8HJzFBgFiImdn26XH6wnkC91YDYOnVxjdnDO9OeD+ryvZ5w8vbHwXBPw" +
            "2xNTziYFmUG5DknnudWKfD3j+EEULu7MwncEM1JICHSQGZwLw9DPxw+z5aeKKxOuIIhgjmLpaFsdFhdhfZJ/+fUTaqVHc0XLjz8BBgAe8hnM7R7yAAAAAABJRU5ErkJg" +
            "gg==";
        private static Image s_folder = null;
        public static Image get_folder(){
            if( s_folder == null ){
                byte[] data = Base64.decode( s_stored_folder );
#if JAVA
                Frame frame = new Frame();
                s_folder = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_folder = Image.FromStream( ms );
                }
#endif
            }
            return s_folder;
        }

        private static readonly String s_stored_folder_horizontal_open = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAXJJREFUeNqkUz1LA0EQfXu7OUIgEgzY" +
            "SRpB1MrSj87eylKs0voHBC3UUlC01c7OVCKIIIgi2KhpjGLMBTUiRghJOLxc7tPZQ1NIipUMPG6WmXn7ZvaGhWGIXkwUt3nXgMYwRp/ZLqHDIETh915BmCdk/mZREoYW" +
            "vfVmq4W6ZSFbTqNWs3Dw1KdReJywL/PY/aa2PJLNrYa+rySZcY6H3bkVctciBV4AHjgOJFRM03XIms4M6GIeeC4k1BgYZE2HIFLgefCVCbSOgnhMEvikwnUgoTYEBlmT" +
            "SgCcxilcOgRtC2bpVE0AF0jomKTiJXqo10iBXa8g1p9BenRGiWRgYmGqYVyIt7OdjUiBWS0hPTwNp1lXUyEE3vMnBdtBXrgei7Udl1qLwzabSgRt87NVfSk+UxuGaFhI" +
            "6slBePYXoLIWDCjenluXj+wmdxVAXJdRaeT2jv+zQKUPGFtH/h25KeKDLp2fvVA1+d/bshvW6zp/CzAAGwSgJsFLrssAAAAASUVORK5CYII=";
        private static Image s_folder_horizontal_open = null;
        public static Image get_folder_horizontal_open(){
            if( s_folder_horizontal_open == null ){
                byte[] data = Base64.decode( s_stored_folder_horizontal_open );
#if JAVA
                Frame frame = new Frame();
                s_folder_horizontal_open = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_folder_horizontal_open = Image.FromStream( ms );
                }
#endif
            }
            return s_folder_horizontal_open;
        }

        private static readonly String s_stored_layer_shape_curve = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAa5JREFUeNqckk1IAlEQx2c3NUsQkQxX" +
            "hewUmBJ9oB3qJggRdQnKW4fO0aU6d+vYPbpWly7RqQgKiSyDJI26SWgRJVSkhfjVf7ZdWSVBGvgxM29n/m/fvCcMrx75iYi5VKizQqFA1WpVjr8ek5RL31BZGiGh3UzZ" +
            "3QUS8TFwtR6cZ8+FWorFYp1Yp9NLtkCYrPYeMhgM8poOhbGhlUMBcUxbXC6XqVKp0F+m1+vJYrH8CoAoRNzw92oBNzZrbjSRCyHwDhwc885MqyYq570DXmBqdedGgQc0" +
            "PsOHsOYCbWAMLCu+qek0O54Bn9JggpgtuTE54106YLE+kFfmlOAZ1wS0gwdxNMY5gR/3LO6LLCwIwjlyCfEosCA+qQm8XuyQyeWT77jBIiiMoFnN08jfwDTiXviULPAS" +
            "3Za/sgijvrou/1xNiV+gUfLw1eaQJsEASNUdIZ9JyKjGf6a1Dkc/OafWOMzw01dnJ7Z6Xd9Pt7IonvdgqVRy460EVQFBuTY9MPJpgJmHBax8GtAN7B/XexIEJlKbsyH2" +
            "jbfQkhU+s6e28JYRwz3mXFCH9l/7EWAAROLnQcP+5zAAAAAASUVORK5CYII=";
        private static Image s_layer_shape_curve = null;
        public static Image get_layer_shape_curve(){
            if( s_layer_shape_curve == null ){
                byte[] data = Base64.decode( s_stored_layer_shape_curve );
#if JAVA
                Frame frame = new Frame();
                s_layer_shape_curve = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_layer_shape_curve = Image.FromStream( ms );
                }
#endif
            }
            return s_layer_shape_curve;
        }

        private static readonly String s_stored_layer_shape_line = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAWdJREFUeNpiNC7fbcbAwADCp6AYA3x4" +
            "+5Lh7pwYDPH///8zMAEJ8zMdLgkgGiSADfMLiTGoRfZgM5uB0bB0pwXUBX+AeC1Qw8t///6BNSLTMPbb0yvhBr8/u5qBBajpBBRLAAXdgfQuIH7BgAMIGofCDQUBJiQ5" +
            "kKZdUEMkGIgETGj8F0ADdgJpTyCWJMcAmCE7gJgoQ5hwiD8HGrAdSHsTMoQRxuCR1WOQsIph4JLWQY4BKSD2AeItQP4z9Ji5PzscYQCyQWIWUQycUtowhSBD/IB4ExA/" +
            "I2gADHDL6DKImkcycEhqgRRLA4X8gRo3AtlPiTIABkBeEjYNBxkkDdQYCMTrgfgp0QbAAMhLwEQkwyaukQLksgIN2f9gTsQeJmhAMoMEgZgD5Hog5gNiASAWAmIRIBb7" +
            "/uyqxLPNDX9//frFB7TZ/ffv314gg1kYSAQ/P785KBo5l4ORkXEvOBpBAUIJAAgwALab9b1aqRGNAAAAAElFTkSuQmCC";
        private static Image s_layer_shape_line = null;
        public static Image get_layer_shape_line(){
            if( s_layer_shape_line == null ){
                byte[] data = Base64.decode( s_stored_layer_shape_line );
#if JAVA
                Frame frame = new Frame();
                s_layer_shape_line = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_layer_shape_line = Image.FromStream( ms );
                }
#endif
            }
            return s_layer_shape_line;
        }

        private static readonly String s_stored_pencil = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAZNJREFUeNpiZMABdNQYGSJ8QDSEv3Bn" +
            "AJiesmEDiBJ7w8Dw6tH//wwshDRiATZ/GBiK/jEw3ASyK1lI0Mjw//9/tRgHB+O9JiaBjJcuvbq/c+deFi15BoYgOwYGE2sGvACk+ePHj5579uyZsG79+mv/urvf/2Zg" +
            "2MO4tAaigFuGEY9mBoefPxkq9M0C3J8/e/TBO/bazOIfPypUgBJMDAyEbIZoDggvd//39/+rC2fOXpST+F1xjRFiIROxmu/eufFq59YN1//+ZXBQkP4HV8NEqmagOApg" +
            "okTzDSBmIUczMEYY1gP5/9ENIKT53mMGht3HGBjuPkaYxkKMZoRGTO+yfP8JJJgZ5N495rENTI5037d7z8sb187cAGm++wi3RrgB334yMP79x5hS0LS/tjjDdOH3Xwy3" +
            "vv9nbDt87j9ejXDQlsrY8+zqsv85QYzLHY0YY4FC4kAsCsTCQCwIxPxAzAvEXEDMAcRsUK8zAzE4Of3P8Gdcvekow/Jnb/7fB/L/QjEw0zH8htLI7N9Iav4CBBgAVaEE" +
            "gahXNNEAAAAASUVORK5CYII=";
        private static Image s_pencil = null;
        public static Image get_pencil(){
            if( s_pencil == null ){
                byte[] data = Base64.decode( s_stored_pencil );
#if JAVA
                Frame frame = new Frame();
                s_pencil = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_pencil = Image.FromStream( ms );
                }
#endif
            }
            return s_pencil;
        }

        private static readonly String s_stored_piano = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAARZJREFUeNrEUzuOwjAUfDbOp+UGHGGV" +
            "1R6CljZlJM7ADai4AFLusC1Stk61hYWUMhUHoMyPGI/hRdlVCliKfdJorHHmefySCGMMvVKSXixhsbd4/4P326ZfK7v4SJLkres6klIOEEIMT+Kafd87HFYHWn4uKU1T" +
            "lx4NpNaaPM+jIAgoDEPHSinXCKa2bamua4fFdkG60T+ufyzL0kRRZPI8HzjLMjOlM8OHZOgyw2lN09CYGb91Zvg4xgzxq6pyG8zQpnRmboCV8n3fbYwZQ53Sme/eWwII" +
            "GBBOZIY2pTNzAtTJdjVFUZhnGD4eYoiT+M6PAj5O8BXH8c6+880zDB8S4HPDRObjOz1QF4uzbVCLf/8brwIMAH1o5N2pLYrnAAAAAElFTkSuQmCC";
        private static Image s_piano = null;
        public static Image get_piano(){
            if( s_piano == null ){
                byte[] data = Base64.decode( s_stored_piano );
#if JAVA
                Frame frame = new Frame();
                s_piano = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_piano = Image.FromStream( ms );
                }
#endif
            }
            return s_piano;
        }

        private static readonly String s_stored_pin__arrow = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAApNJREFUeNqkU19IU3EUPnf3TnNmRWCR" +
            "lmYESmwhXJ8sKqmlW6wwpYeEIHqKHrQCIcgHYUJv6iAihMDVNA2rl9qFuVIWJuTQVi6JhU3ZaJW76W3O3b+dO5fbitFDFz5+/875znfOPYdQFAX+56PCGs1flyolkX11" +
            "GNEqAdhYAM8XPCxiYB5XTS5mKfvYWjI42JwAuI7kRzMfchLI2XsNhEJQYbOdiQPcQJLj/yTIlC+gHc9xEBodheKmpvplRelEknr1kQgRG9l2Ig4gEhgxIgLM5AHMJ+X3" +
            "9TUvjY9DJByGrQUFUKjTweDwsBdJaqiP6b9w6tjQEK0sLooiy3Ir09Mh1uNZ2N/dbVad1yQJZtzu15gCicUj0etmUoE7LbUBI1tP9PfTks8HCUEAEaMJGFV1fjYwMIWq" +
            "OigARszIb4NAWa98A8Jq7O2llyYngWNZkPPzYdbpnIzwfOfXylrmRfvjpH3VyC0Q+ASQagXephFACFqG2V1WV1eyFAiAFI8DodWKK9Ho3KdD595EKmvvoum2cEWNL1h6" +
            "ECh7uoiAXVmES95Fl4seMxq91RYLTcZioCsvL/4QCNQG91Tf5tfWaLShUx38gCDSBCTirN/vt+v1emujokRPy/Il/9V7dGBzKXz7GY+xO/bNqYaXLxjoO/Z3XkWWe6hk" +
            "IdZJjjgcjnaDwdCK50coh8Wizo9x1Mj5ayd1cytQyImgRoeJ7wDGRgP9tP9VW9KTJEmDyWRqYxjmiSzLTkSyk6sQnLnj4fYtuyyCdtPqj6KdQfV+b4uZ/ux47hXC73tU" +
            "gjKKoq6g0wRiITUG4h8Qfq9FLY6XKFmRI7O9MVfXfTWFZVEUe1IDqKTGIBekxOrqFMiSh3d1OVT7XwIMAFEjN/TZelQfAAAAAElFTkSuQmCC";
        private static Image s_pin__arrow = null;
        public static Image get_pin__arrow(){
            if( s_pin__arrow == null ){
                byte[] data = Base64.decode( s_stored_pin__arrow );
#if JAVA
                Frame frame = new Frame();
                s_pin__arrow = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_pin__arrow = Image.FromStream( ms );
                }
#endif
            }
            return s_pin__arrow;
        }

        private static readonly String s_stored_pin__arrow_inv = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7DAAAOwwHHb6hkAAACh0lEQVR4nKXPS0jUURTH8e/5//9IIENFRFbQoqIicKBNjVFJTdEg" +
            "Y1MwRdlMUkSRPShcVEhQixgEXZivxDF6mUr02BiKiUEvokvqhFgYTaVDRlIESvSg06L/mG1q0YUf93LOuR84qCr/E2qAONAuQq8Ib0RWpkSupkRWpERIZ2jCe2L+AHpE" +
            "cgdEbmpzs7oIqV/ov4E2Ef9DkbbvlZWq5eU6KHItPfTqX0A9rG+Bey/CYX0dCumXkyf1pciN9Bp/BaqBGjAft2zRoWBQ+/x+HYlGVePx9BorkiLRQZHylMhlt3bqD6Aa" +
            "AjVg4vCoER4MFRSMIwMeT+tIXt7TryUlH36UlX3TlhbtAtMFdME4QDVQ52J18DiNfDp6VMcOHdLvxcWqFy5oB5hOCHQCnQC71u5jb95hchuGyW0Y5tTC5dRC4HpGxsPn" +
            "oZAOBAI6vG2bfquo0Ftg2iFw2/3cCeCNJfBVJVkVfxtZFX9bd2BDMQ2w/878+cnenBztXrpU3x08qDfAFMDOCLADKHSDN5ZgWeXLiK8qaXxVSZO//xK10Djg94++WLNG" +
            "n+fnawuYjo4OBfYCHhEhHbJP90S8sYSpTal6YwnjjSVM7pGbo+ET7XrsyDm9CCZsWfts2y7p7+//LCKbRcQeB+Ydv2uKn6lGe3+nqE/1zKCqb/fFsfMQCFsWjuNMdRxn" +
            "T1NT02PLslZblvULmLaxNJJV1Gp891WzilpNVlGrWRQ99z57a83Y7LwTzYv4fSzLsh3HCQaDwQbbtrPH65nrSgo9kStPPNsbu4EpwHRgJjAHmAssABYDXmCJZVkhx3FK" +
            "3T4C2BmhszsyNtXXA5OATMADTAamAtNcdIYLz3LvyT8By6VlxFLsqNAAAAAASUVORK5CYII=";
        private static Image s_pin__arrow_inv = null;
        public static Image get_pin__arrow_inv(){
            if( s_pin__arrow_inv == null ){
                byte[] data = Base64.decode( s_stored_pin__arrow_inv );
#if JAVA
                Frame frame = new Frame();
                s_pin__arrow_inv = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_pin__arrow_inv = Image.FromStream( ms );
                }
#endif
            }
            return s_pin__arrow_inv;
        }

        private static readonly String s_stored_ruler_crop = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAoFJREFUeNp8U0toE1EUPW8yUZPOJClJ" +
            "G6MVMcUiQiN2I66iNbpwK4qCK6tQFBQEQSoudCP4W9QWipuKC3EhIojgotIulLqpm0K1YtJqSNJIDBFrks7Mm+d9k4/pIn1wuMOdueeee94dNnNOhQDAgGEKvaidJOUm" +
            "GNofUY8qasWwBXbHrz+9yk0T7x8MPVQ2qm45ksDpTIxxbhjIpFJYWRWJbTq7t0FdkjDhEDQ6f34zCdu2EQqHETs4GOs7djbWrloqbDwrRKDIzpxzCMLPbBa5YgVmuYxM" +
            "MrkuNiBrCDI6ClIz98+PWrY43Bs/2d8ZDML8Pjv/YfzjdPq37dnhVyoyStcGh25eCHV3OwStHjziNmByPLY57y/k81jMibk9EXZFUFED2/1sbGtPD4xKBVwIV3MEgwMV" +
            "S1ASLtuy4A8EsLOL2TKnbWLOdRkmsFQQnuzyMjh9Qw1dsqmEWjEbN0oE5MGvQgFf0sItE74O4GtaYF/UuVPeFYnUFBCBTMjmCs2OOlSpwKfr2BWBonmAgAbnXaYk8G1F" +
            "bM6l0zUFpLZMqqqkUrXs/zshFZSKRSwsC2nSs04fU2h+W/dACWrsQBcZuEYKsgVxyO/Dc/pmQbV4k8At90AjBZdu3T5ToFFCoRD21+P0qxfIZzLQfT7sHYhFY4nT0dfj" +
            "Iy/VP0bTA10QgSQp0117vd51MdBhIUhEVVKwRq6atDtUqzvuJMikI1GMkdyBdttX/Yu+UxcvBzVNw9vJO8ViWSwyhk9yD9hUSrimUrgmx6jvhoxK/T9zzuhx9iQcDh81" +
            "6WczuJgbeSdOUNpSWxZqC8FL8NThbiWYzyO3evfGrHxeKuGHHFkK+yfAAKZHVlxnqygZAAAAAElFTkSuQmCC";
        private static Image s_ruler_crop = null;
        public static Image get_ruler_crop(){
            if( s_ruler_crop == null ){
                byte[] data = Base64.decode( s_stored_ruler_crop );
#if JAVA
                Frame frame = new Frame();
                s_ruler_crop = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_ruler_crop = Image.FromStream( ms );
                }
#endif
            }
            return s_ruler_crop;
        }

        private static readonly String s_stored_scissors = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAlRJREFUeNqkU09okmEcfj6ThNRhjUEJ" +
            "DhxDZ1t4sI3lDrKDhESHpC6x6yBoh52GfcdBJDvtElEUdKhOSq1JjF0SabSton9OmIeRxVKIrUgdU5xvv8e+hXXYpRcefv+e5/mh7/tpSin8z9Gi0Sg0TTsv+edarfa+" +
            "Wq2iUqmgXC6DudVqhd1uh81ma+UWi8Uv3G5ZPJ9MJmGq1+twOBynBOek6T9oG+fkkU8djymVSiGTyWBiYuL6QSb7YvLIp679+D0ej57NZpX8JD0QCPj7+vrgcrnAyJp9" +
            "zskj3zD928Tr9er5fF5FIhFdiH4aMLJmn/N98R+Dq5qGSUFQwKFs1AuFggqFQrrT6bzIyJp9zoMGn7qWQU6ST4JNQeK3kd/n8+nFYlGFw+HFdDqtWLOfMHjk5wwDjckR" +
            "wGYGeJVnBMdXgaNrbveJKysr/etu91pHtVo8BnyXWUnwsgHM7wAVX7MJ0cEmjWvW4eGzjpGRXnNnZ8cFeRi9pRI+dnXtjMbj/cLp57rG1tbPH0tLwd3l5QHp3RBU8E7T" +
            "xr4MDb1V8bh60tOzfhN4vTc9rRYkCm4tGDX7nJNHPnWt/+CFpt1rF9emptScxKfA2DNZwThn9NtNWjoxMH1Tqru+va02NzbK47FY4PHMzJtdYPYD8OChGDCyZp9z8sin" +
            "rnWXt4E7q4ODRbrelw0x2Xjyn1fImn3OySOfutYt+IDRSeCycAZeAYm7wKLkshR87A1+cILDAss4EDkNXJI8Ows8yin1nENn+5MXNA3hnpHzHBKYjWhqe4lffwkwAMRc" +
            "PMqRQZ4vAAAAAElFTkSuQmCC";
        private static Image s_scissors = null;
        public static Image get_scissors(){
            if( s_scissors == null ){
                byte[] data = Base64.decode( s_stored_scissors );
#if JAVA
                Frame frame = new Frame();
                s_scissors = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_scissors = Image.FromStream( ms );
                }
#endif
            }
            return s_scissors;
        }

        private static readonly String s_stored_slash = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAApJJREFUeNqkk09ME0EYxae7paWbUksq" +
            "G4IIYuufgIBUPaAx9kIkEoTUeMMLQYkxXnr24MGrXAzx4LE1hhCIiUowxoAH8WKFsKYxWERr25DSpoJs6e72j++Dlqy9eHCT3+7OzL73zcy+MRSLRfY/l5FufoOB8XgS" +
            "BsYagBev50FT6bsIWECpaRDPo0HcQHFjhWF/lcXiO9rR4TrS3i6aBMFEnYosn/suSRfXJOmaurMzhq4Xf82gLLbY7fevjo66s9ksezo5ubKRSmVooM7hEDrb2sSe4WHP" +
            "20DAJm9usrIJRzdMq4Gvrvb1jYy4c7kcezY1tRKNx/2KongIvN969QbX3Fys2+t1c2azjzT7BliPt6m11ZVOp5l/YmLl+sDAcXQPgm5A5d6B28uh0MKSJP0SnU5Xfm+f" +
            "9gxyjF1wNDaKVDkSjfofjo8Hbw4NncHQA9BbWmIKjC9LUgJikTR6g8O5YtH0e3ub1vwI3HsSCAS9Hk+lyZKiaRlNlk2k0RswTdOYLhOzZDI9Px+80tVVacJ4Vd3V7Bto" +
            "jP3MJpOqleMENE/rTWYWF4OXnc6yyV0rzwtMVVXS6GfwPhqJJFxWq4jmHeDQm7xeXQ321teTyWCL2SwmM5kEafQG099SqXBzTY39hCBQAh+DS+AA+ACez66vB/tqa92H" +
            "qqrsMUUJk2Y/SAXEU87nx2bicVs/Pqrj+Z6v2WxnOp/fDVItpn0MlS1I+sutrU87hcIYhHEaM9DGGXAWmtE4izS2cJyv1WRydUBg47jdKG8VCuqyoiRCqhpeg/gjUviD" +
            "AkjakoGxtO6D9fg9JzF9bMYpqOvIQGVsI8HY5y8I1DpjMXRtgCS0ubIB7YVQwviPE0wHUQYZaAt/BBgA1XccJAJP038AAAAASUVORK5CYII=";
        private static Image s_slash = null;
        public static Image get_slash(){
            if( s_slash == null ){
                byte[] data = Base64.decode( s_stored_slash );
#if JAVA
                Frame frame = new Frame();
                s_slash = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_slash = Image.FromStream( ms );
                }
#endif
            }
            return s_slash;
        }

        private static readonly String s_stored_start_marker = "iVBORw0KGgoAAAANSUhEUgAAAAYAAAAOCAYAAAAMn20lAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAKUlEQVR4nGP4////fwZs4D8U4JTAkPyPBnBKwCWxSfz///8/YcuH" +
            "hgQAFmGTbRk2RiQAAAAASUVORK5CYII=";
        private static Image s_start_marker = null;
        public static Image get_start_marker(){
            if( s_start_marker == null ){
                byte[] data = Base64.decode( s_stored_start_marker );
#if JAVA
                Frame frame = new Frame();
                s_start_marker = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_start_marker = Image.FromStream( ms );
                }
#endif
            }
            return s_start_marker;
        }

        private static readonly String s_stored_target__pencil = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAtlJREFUeNqkU11Ik1EYfr7t82fappsW" +
            "m5qmTl1qGYjCFJ0zipC6EKtLiRTEy4SupSgI6srYxQpvgsBuIksMlKYkeCGUyaRtKbp+pvNn0/btx23fT+d828iL7vrg+XjP877vc95zzvsykiThfz6W/n4pFFD85U4R" +
            "XCK4QGBIc9sEXwhmCXYpIR4XOPZ1KTSagfzu7nOqpqZypVqtpqTAcVxsZeVHxOG4IoZC44Saxz8Euli9/o5uaKiTNZkK0doKlJSkgra2tOqlJS0RrQja7Rre70dGRBaQ" +
            "aNlq9aB2YMDCWq0FqKuDOD0NbG6mpCsroejpAWswFGp53rI3NsZJHPeV5O3KR+eBy6r29vNKk6kAtbUQbDZIJJlpbJRBbcpRH42hsTRHILmyQAJozq2pOS22tCA5OQlR" +
            "IK7hYQjV1TKoTTnZR2JoLM3hMgJxoEyRna2OFxdDXFuDQHZKeL0QxsdlyDbhqI/GJAMBjRK4GaGXTgWOAKUYjyN+dASR58Enk0iQNbUpqE05av/0+bAUDjPG0dGSPeAe" +
            "mxbwx/b3w7zXW5AoLQXrdCJGXiHR1yffoaDTQU24gD6E5bk59E5MMHcNBmeDJD2UKwhL0iefy7XDLiwgZLFAIDvl2e1gPB4Z1D5kN7B+Zg29N/wYbW4OFEWjL3KAGbmC" +
            "38B0xOW6rltcLM0yGPIP+/uRNzuLrNVVuYKDsig2jAcwd3Xi7ZPnojESdJ9IiC8VmT4gN7rNxeO2OYfjZCfP16vMZlWsrQ0cOUIssowd91OYrVZMPn4mBqcOPcrv4qNE" +
            "qr3B0GFiGAa1ZNEDXGvMyRmprqoyVpSX67KKmNyN+hVF6+1bmHgwFo/MxJzr6+J90mLvvqXbNyNAKykiKD4L6C8CHUagIbsDLcPvPRW2EdOe+6P06oMbb1ypYSIPgH3a" +
            "gxkBepy8NI7Ph2/wKjMVDMP+el76nOZoA5IWQJQO5R8BBgA5/EXgQmqRywAAAABJRU5ErkJggg==";
        private static Image s_target__pencil = null;
        public static Image get_target__pencil(){
            if( s_target__pencil == null ){
                byte[] data = Base64.decode( s_stored_target__pencil );
#if JAVA
                Frame frame = new Frame();
                s_target__pencil = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_target__pencil = Image.FromStream( ms );
                }
#endif
            }
            return s_target__pencil;
        }

        private static readonly String s_stored_VSTonWht = "iVBORw0KGgoAAAANSUhEUgAAAF8AAAA3CAIAAADliQGVAAAAAXNSR0IArs4c6QAAAAlwSFlzAAAuIwAALiMBeKU/dgAAAAd0SU1FB9kEHBEyDgGGGZEAABEiSURBVGje" +
            "5Vt5cFTVmj/fuff23p3O1mQlGwkhiSQCYZGEdcBQMCKLOKLzBhVm/hBrrGEceY7lvOeMFjNC8cbyuVcpmmfBgOOThws4KpsPEshOSISQkChJJx3SSbo7vd17vvnjPK9t" +
            "N3RYkrzyeSrVde7tu5zzO7/v9y2nA4g4MDBQWVnZ09PDGENE8nNtAEApjYuL27BhQ0ZGBiEEHA7Hzp078/LyWltb+/v7yc+4IaLZbC4sLOzp6Vm9enVJSYmQm5ubmJi4" +
            "Y8eOEydOJCcna7XanzNAgUCgpaUlPz//nXfeKSsrE3t6evx+f3d3d1VV1bRp0yilP3P6IOJzzz2XkZGxe/du6vf7vV7vjBkzflrQ8GmMk/QwxiwWS21trcjPmkymPxc0" +
            "jLGenh6Hw+HxeAAAETUajSiKkiTFx8cnJSUBQORdTqezsbGREFJaWmo0GsccI0QMBoPin2XZGWNtbW11dXVdXV0Oh0OSJIPBQCkFAL56vO/1ehljiYmJcXFxcXFxycnJ" +
            "kydPNplMgUDg0KFDjDGv11tQUDDm6KhNnHimNDc3f/zxxyMjI1qtFgDi4+O9Xm9CQkJ6enp8fLzBYJBl2e/3u93u3t7enp4eAHC5XG63u7Oz84svvhBFkTFmtVq1Wi1j" +
            "bFxHO6Ho+Hy+ffv2dXR06PV6s9lMKfX5fFOnTl2yZInZbOaUVu2Iy0ogEKirqzt//jwiGgwGrgAqxf5y0BkcHHz77bcVRYmJieG24/P51qxZk5+ff03J4zBptdrZs2fP" +
            "nj27oaGhrq4uLi7uLxAdRVHeffddURS5NfG5JSQkFBQUjHovx66kpCQrK+vw4cNms1kURY6RoijjOmw6MTL82WefaTQanU6n1Wp1Op1er9dqtQkJCTfulQEgJiZm7dq1" +
            "sizzR/GnTRB3EPHy5ctXr14NPRNJ9dA+Yyw/P59Lhtrcbndra2uosSDimTNn9Ho9X3Cz2RwTEwMAQ0NDN47v2bNnr1y5AgCTJk2y2+1paWkAIMvy0aNHrVYrpxh/qTo2" +
            "HhOxkMYPFUVR+7zl5OTMmzdvFHTOnTtXVVWFiKIoqqwOdbG8w32KIAh8/cPQuXr16qlTp2RZFgQhJiZGEARKaX5+PgAEAoGRkZG8vDyPx6MoisfjuXjxYm5u7jUjmkj7" +
            "SktL48PIzs7u7e1NTk4OBoOSJOn1ekmSBgcHMzIyEJFPPvSzr6/ParXyuNfj8VgsFkpp2GWjcAcAVq1atWrVqsbGxkOHDsH3LRQaAPD5fAsWLJgxY4ZOp7vmEzMyMrZu" +
            "3UoIGRoaampq6ujoMBgMADAyMhIXFzd58uRAICAIAp9VY2Oj3W6fOXMmvyZ6CCtJEu/4/X6PxyOKoiAIc+fOTUpKIoS0tbUNDg6q6PA5835+fn5eXh5/lCzLLS0ttbW1" +
            "qq7zNjo6vDN9+vSUlJQ33nhDr9eHAsTRkWU5NzdXr9dH1whCiNVqLS4uvnTpklardblcBQUFfX19gUCAUqrT6fjT9Hq9IAhHjhyJiYmZOnVqSkpKpBWreYMkSYh49epV" +
            "o9G4bNkyPkOTycQvBgC32x3JHUVRQgMFSZKmT59eVFR07Nix+vp6jUbDGAsGgzfqswAgLi5uxYoVx48f1+l0YfTR6/UnTpxYu3btjZhDU1NTSkrK0NDQ9OnTe3t7OSiR" +
            "1pqZmQkA7e3t586dM5vN2dnZkyZNCnu+oig+n8/lchkMhuLiYgAoLy8Pe52Kjsqa61kNpXTBggU+n6++vh4RR0ZGbsKjC4JQUlLS3Nwsy3IYdyilw8PDHR0dWVlZ0QHi" +
            "qf+kSZMIIXa7XVXl0KeFHqp8bG9vb2hoMJlMmZmZqampfMEURXG73f39/ffee+/1lDuMO9E1RRCEpUuXnj59OhAIyLJ8c/EOpXT58uUffvih1WoNW3C9Xl9dXZ2dnR2d" +
            "ONXV1WlpaQ6Ho7y83O/319fXJycnh4KiEp4bjizLfD5arVYURUVR6uvrq6qqYmNj09PTg8EgV64ob3S73dwZKSEtSsQoimJeXl5VVdXoqhzZbDZbTk6O2+0WBCF0kTnw" +
            "NTU1M2fOjBIZDw8Pp6SkxMfHc4+bk5Nz5cqV5ORkAFDnELbIYZ886nO73WfPnu3u7nY6natWrVIUhSMbKQgcnTBVjhIxUkrvuusuj8czefLkm0YHABYtWrR3716bzRYp" +
            "Fp2dnYWFhdf0XIjY0NCQlZXV1dW1ePFiPpOcnByLxcKzAZ5JXg+dax7yTP306dNNTU133nlnSUlJGEaI6HK5QqObUblDCMnKytqyZcstZhKSJJWWll66dMlisYSiAwDp" +
            "6elnzpwpKyuL9C92u10QBFmWbTabRqNRF8pmsy1fvryxsbG9vd1gMEiSFB2RSEIRQlwu15EjR44fP75+/XoeE6qv9ng83ELD7h01Cr/1PCsvL+/y5cvceYXRx+129/X1" +
            "cd0Nc1WJiYnffffd3XffHRZhA0BxcXFRUdE333zT0dHR29trMpnUEFaVjEhcQjuKooyMjLz00ktr1qyZN2+e+opQn3WD3LndLJRSWlpaWlNTwzVVhQYRk5OTm5qaEhIS" +
            "BEFQyzeXLl1CRIfDkZGRcb3kWxTFwsLCgoICj8fT1NR08eLF/v5+g8EQ3cQiafXee+85HI7Vq1eHqnIoiFEivTHL0ePj42NjYwVBEASBvzsYDPKOIAhtbW1Tp05VL66t" +
            "rY2NjR0eHl62bFl0l89juXnz5s2dO7e/v7+mpubChQsOh0Or1UYBKOz8vn37CgsLp0yZwotk14yVx72CUVJScvjw4ZSUlDCTBoD6+vrs7GxRFBGxrq6OENLT07No0aLQ" +
            "MJf7KZVikTAlJiZWVFRUVFT09fVVV1fX1dU5HI5I8Q61PhWF/fv3b9++PdSyJhodURQLCgpaW1uNRmPY4CRJOn36dHl5uSzLZ86cMZlMRqMxPT1dJc5bb70VCASKiopU" +
            "yKKHEStXrly5cmV3d/fJkyerqqoGBgZ4rBy2MOqZr776atu2bTwaDMvIo4vumKEDAJmZmS0tLdxrhnG+oaGhpKSkuroaAOx2+6ZNm1TFAQCn06koyokTJ8rLy69Hn0gn" +
            "kpqaumHDhvXr1589e/aDDz5obm7muhYpQ4FAwOl08iq9qv1cB9QAbdxrgzy12bt3r9FoDBslpfTAgQO9vb0ajWbKlCnx8fGhN3q9Xr/fHwwGq6qqQl3MjbxREITS0tJZ" +
            "s2YdOHDg5Zdf5iRSWcOtRlEUbtcajYb+uHGAJggdi8VSUFDQ0NDAdSSURE6nk28zRYoxLzgoirJ///4opY/oldP77ruvtrb2888/Z4ypsYUkSYIgWK3WmJiYgYEBnsqp" +
            "oKj9iauczp8/nzHmjmgjIyMul6usrCyyuOH1evk1/f39e/bsubViMAAsXLhQFEVeeDUYDAaDwWg06vX6DRs2cAj4GaPRyDv8mujFljGuuvMi2euvv84TxVAT0+v1S5Ys" +
            "iQydfT6f2+3mLDt48GBaWlpFRcUtLClPgMOoYbPZ7r//fs4mg8EQ+hW/ciJUObRlZmYWFhbW1NSE2r/f79+4caMoimGj4eVEjg4HcefOnW63e926dWp99gZ3zb/88ku+" +
            "ZapCoNFonn32Wc6OQCDA0eFqFYpRIBCYOHQAYN26dadOneI7uZwUmZmZpaWl11yoMHQURdmxY8fRo0effPJJXveKvrw8z3j11VcvXLhgNBrVOWs0mieeeII/ARH7+/v5" +
            "t5HohG4lTMR+lk6ne+CBB3bv3s3rUsFg8KGHHrreJDk6avTB1bSuru7BBx9csWLF6tWri4qKBEG4ZniCiD09PXv27GloaDAajaoPSk1Nffzxx202G4fG7/cfO3aM12Fp" +
            "RKupqZkzZw7fSuOh6fiiQymdP3/+J598UltbqyhKWVlZYWFhlOvVQSckJPCNGn7m5MmTJ0+etFgsd9xxR1ZWVmxsrNFolCTJ4/EMDQ05nc7Ozs7Ozk6tVsvrx4qixMfH" +
            "L1u2bPHixYqivP/++7Isu1yurq4uURTD7E7VHUTctWtXamqq0WjUaDRz5szhhfpx3AullD722GMbN25kjG3evDnKlVqt1mAwZGZmvvjii4mJibIst7W1NTc3X7hwob29" +
            "3W63M8bOnz/f0tISNjfeJEnSarWZmZk5OTlFRUXTpk1Tdxf8fr8oilarNTY2Fn7c1LAw7JAxdr066RjvFKelpX300UeIGBsbG0U1Nm3a5PF4ZsyYkZiYCACSJE2bNk2d" +
            "ZCAQ6OzsHBwc9Pv9fMtJzV1MJpPZbE5PT7darZHypNfrH3300Vvbm52gffQouKitoqIiisFrNJrc3Fx1xGHlvttMlG623DXRvzJQhzJqNeN2pvET+w0GEgQkjAAQGRAI" +
            "XjcoR/ieNZQSQhAZAwDCkERCA0gIEEIZIZQAgZ8qOoxhEAl+fMi39Z8VAgK9bpFFoUQIMHzu6bhHHiGEBFBZ8Ltf+AIjjEbsOiAIDBTwry9YsX3WP4iiRH+i6BCmBF9+" +
            "BZ/+lWQ0abZupmbrdW0Kibfh7Esv/Ne/PfwwACAofub9RckaHRUj+AhIwC2PvN34v+3O7v9esj1Ga/kpocOQIGHM6x3+l381vPa6yCh4B8iv/pOEWEGQgovKV4iGEYUi" +
            "UYBShJ7kBIYISARCA8z/QvUryp+4g9/fi4QAcIQI+X3X/333B/uby/8905QK40YhcdRA/uY0GNF/tdf/t39v/vwLwB8k5UeRIbIjkubbp56anJ0hEPzO6yBKcJpO3+Wx" +
            "E0IQZR8hCAgIP773+98TAgcMaq6eu+f3j71993/MtBXwaGZsFBNR/RQtFovJZGpsbFTrJrcqw0RBlFtb/Rv+Tt/aRBkAoeyHSYYgSFAjk1llc5YuWSoT5eGPnkpNTFNQ" +
            "ea1+HyGky3mlP+ikhN7AOKDT173u4D/uXrz93uy/IoC3r9OI6PV6KaUdHR2CIIj5+fnd3d1JSUmffvppXFzcbaCjZLR/a/6nXxocDopICEXuYyLnREAGAAKEoEDAjgNn" +
            "L7QSEsQ/mbkisCBBDYHRq+UCwjC6N3/x60u9bXeJxQLS24TG4/F8/fXXGRkZlZWVS5cuFVesWLFjx47S0lK73d7Z2XnLP6+3fHsl66XXxIA30pRGoVwQHX4HRapQ5CKN" +
            "IAIoN/OQ4PNNlcvctYv0JbfJHUmSUlJSKisru7u7t2zZIkqStG3btmeeeebgwYPqru4tNAFpsyA/DUBRUQCEGwAZCEECjAKjwAj+EAARuDFogCAQQhgwodlVc+xYc/Dr" +
            "20QnGAw6HA6NRvPmm2/OnDlT5AXHXbt2bd++vba29pYLRQQRCXV2XrI+84Le477xuygSgaEKDhINo4yAHClYkaEAAyIgkWrcW/MeKv51AdIxEObY2NhZs2bx4reoxuw2" +
            "m62iouJ2gmMkDBmMTCv0PfiIrn8ACGGAAoscMSMUABWCCIQ8U/Dwe++9qwAIhBJCkJJjSd8MSnIUcBggRUoIoahIf3T/du3za++5h1EqjIUq/yj7Gdv/4wkSQhQm19b6" +
            "1m00XOmkKERSAIENUolkpZsNRgQiAwOiITufNy1dTAhRmDJv3wMXhzujTgI0KPupYDke2LP5NwsXLaDjk5GNMToEiQIICMG2i541f2M634JAKANKWCg6CApBkYuLAoQS" +
            "ZffChU99+TkhREFlzmjoCIwQGZNO0spfvl5cWCSIAhmfNtZhJhCBAAUiZU/RH/lw5K75BAijhIGg/iGRCOoIEQkRCBEQCKLgdPR/z20iMkKRUURABMTIjuDF7FO6T16o" +
            "nFFcPH7QjGeOTol2Uhr84XcNv3nl4IEPFWTR7IQI/Vmp6tHDSX994I//w65vLAlo+e3u3bG2+PHO0f8f5y8XKljNDxUAAAAASUVORK5CYII=";
        private static Image s_VSTonWht = null;
        public static Image get_VSTonWht(){
            if( s_VSTonWht == null ){
                byte[] data = Base64.decode( s_stored_VSTonWht );
#if JAVA
                Frame frame = new Frame();
                s_VSTonWht = frame.getToolKit().createImage( data );
#else
                using( MemoryStream ms = new MemoryStream( data ) ){
                    s_VSTonWht = Image.FromStream( ms );
                }
#endif
            }
            return s_VSTonWht;
        }

    }
#if !JAVA
}
#endif
