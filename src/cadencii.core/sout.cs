/*
 * sout.cs
 * Copyright c 2011 kbinani
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
//using System.IO;
//using System.Collections.Generic;

namespace cadencii
{

    public class sout
    {
        private sout()
        {
        }

        public static void println(string s)
        {
            Console.Out.WriteLine(s);
        }
    };

}
