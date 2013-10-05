/*
 * ClockResolutionUtility.cs
 * Copyright © 2010-2011 kbinani
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
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii
{

    public class ClockResolutionUtility
    {
        private ClockResolutionUtility()
        {
        }

        public static int getValue(ClockResolution value)
        {
            if (value == ClockResolution.Free) {
                return 1;
            } else if (value == ClockResolution.L1) {
                return 1;
            } else if (value == ClockResolution.L2) {
                return 2;
            } else if (value == ClockResolution.L4) {
                return 4;
            } else if (value == ClockResolution.L8) {
                return 8;
            } else if (value == ClockResolution.L16) {
                return 16;
            } else if (value == ClockResolution.L30) {
                return 30;
            } else if (value == ClockResolution.L60) {
                return 60;
            } else if (value == ClockResolution.L90) {
                return 90;
            } else if (value == ClockResolution.L120) {
                return 120;
            } else if (value == ClockResolution.L240) {
                return 240;
            } else if (value == ClockResolution.L480) {
                return 480;
            } else if (value == ClockResolution.Free) {
                return 1;
            }
            return 1;
        }

        public static string toString(ClockResolution value)
        {
            if (value == ClockResolution.Free) {
                return "Free";
            } else {
                return getValue(value) + "";
            }
        }

        public static IEnumerable<ClockResolution> iterator()
        {
            ClockResolution[] arr = new ClockResolution[] { ClockResolution.L1, 
                                                            ClockResolution.L2, 
                                                            ClockResolution.L4, 
                                                            ClockResolution.L8, 
                                                            ClockResolution.L16, 
                                                            ClockResolution.L30, 
                                                            ClockResolution.L60, 
                                                            ClockResolution.L90, 
                                                            ClockResolution.L120, 
                                                            ClockResolution.L240, 
                                                            ClockResolution.L480, 
                                                            ClockResolution.Free };
            return new List<ClockResolution>(arr);
        }
    }

}
