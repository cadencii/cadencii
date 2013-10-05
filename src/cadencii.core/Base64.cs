/*
 * Base64.cs
 * Copyright © 2009-2011 kbinani
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
    public static class Base64
    {
        public static string encode(byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        public static byte[] decode(string value)
        {
            return Convert.FromBase64String(value);
        }
    }
}
