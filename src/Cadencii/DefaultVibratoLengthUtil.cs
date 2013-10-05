/*
 * DefaultVibratoLengthUtil.cs
 * Copyright © 2009-2011 kbinani
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

    public class DefaultVibratoLengthUtil
    {
        public static string toString(DefaultVibratoLengthEnum value)
        {
            if (value == DefaultVibratoLengthEnum.L50) {
                return "50";
            } else if (value == DefaultVibratoLengthEnum.L66) {
                return "66";
            } else if (value == DefaultVibratoLengthEnum.L75) {
                return "75";
            } else {
                return "100";
            }
        }
    }

}
