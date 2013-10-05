/*
 * QuantizeModeUtil.cs
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



namespace cadencii
{

    public class QuantizeModeUtil
    {
        public static string getString(QuantizeMode quantize_mode)
        {
            if (quantize_mode == QuantizeMode.off) {
                return "Off";
            } else if (quantize_mode == QuantizeMode.p4) {
                return "1/4";
            } else if (quantize_mode == QuantizeMode.p8) {
                return "1/8";
            } else if (quantize_mode == QuantizeMode.p16) {
                return "1/16";
            } else if (quantize_mode == QuantizeMode.p32) {
                return "1/32";
            } else if (quantize_mode == QuantizeMode.p64) {
                return "1/64";
            } else if (quantize_mode == QuantizeMode.p128) {
                return "1/128";
            } else {
                return "";
            }
        }

        /// <summary>
        /// クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <param name="qm"></param>
        /// <param name="triplet"></param>
        /// <returns></returns>
        public static int getQuantizeClock(QuantizeMode qm, bool triplet)
        {
            int ret = 1;
            if (qm == QuantizeMode.p4) {
                ret = 480;
            } else if (qm == QuantizeMode.p8) {
                ret = 240;
            } else if (qm == QuantizeMode.p16) {
                ret = 120;
            } else if (qm == QuantizeMode.p32) {
                ret = 60;
            } else if (qm == QuantizeMode.p64) {
                ret = 30;
            } else if (qm == QuantizeMode.p128) {
                ret = 15;
            } else {
                return 1;
            }
            if (triplet) {
                ret = ret * 2 / 3;
            }
            return ret;
        }
    }

}
