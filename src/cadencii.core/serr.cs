/*
 * serr.cs
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
#if !__cplusplus
using System;
using System.IO;
using System.Collections.Generic;

#endif
    namespace cadencii
    {

#if __cplusplus
        class serr
#else
        public class serr
#endif
        {
            private serr()
            {
            }
#if __cplusplus
        public:
#endif

            public static void println( string s )
            {
                Console.Error.WriteLine( s );
            }
        };

    }
