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
#if JAVA
package cadencii;
#else
#if !__cplusplus
using System;
using System.IO;
using System.Collections.Generic;

#endif
    namespace cadencii
    {
#endif

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

#if JAVA
            public static void println( String s )
#elif __cplusplus
            static void println( string s )
#else
            public static void println( string s )
#endif
            {
#if JAVA
                System.err.println( s );
#elif __cplusplus
                cerr << s << endl;
#else
                Console.Error.WriteLine( s );
#endif
            }
        };

#if !JAVA
    }
#endif
