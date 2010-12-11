/*
 * UstEnvelope.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
import org.kbinani.*;
#else

#if !__cplusplus
using System;
using System.Collections.Generic;
#endif

namespace org
{
    namespace kbinani
    {
        namespace vsq
        {
#endif

#if JAVA
            public class UstEnvelope implements Cloneable, Serializable
#elif __cplusplus
            class UstEnvelope
#else
            [Serializable]
            public class UstEnvelope : ICloneable
#endif
            {
                public int p1;
                public int p2;
                public int p3;
                public int v1;
                public int v2;
                public int v3;
                public int v4;
                public int p4;
                public int p5;
                public int v5;

                public UstEnvelope()
                {
                    init();
                }

                private void init()
                {
                    p1 = 0;
                    p2 = 5;
                    p3 = 35;
                    v1 = 0;
                    v2 = 100;
                    v3 = 100;
                    v4 = 0;
                    p4 = 0;
                    p5 = 0;
                    v5 = 100;
                }

                public UstEnvelope( String line )
                {
                    init();
                    if ( line.ToLower().StartsWith( "envelope=" ) ) {
#if JAVA
                        Vector<String> spl = new Vector<String>();
#elif __cplusplus
                        vector<string> spl;
#else
                        List<string> spl = new List<string>();
#endif
                        str.split( line, spl, "=", false );
                        int size = str.split( vec.get( spl, 1 ), spl, ",", false );
                        if ( size < 7 ) {
                            return;
                        }
                        p1 = str.toi( vec.get( spl, 0 ) );
                        p2 = str.toi( vec.get( spl, 1 ) );
                        p3 = str.toi( vec.get( spl, 2 ) );
                        v1 = str.toi( vec.get( spl, 3 ) );
                        v2 = str.toi( vec.get( spl, 4 ) );
                        v3 = str.toi( vec.get( spl, 5 ) );
                        v4 = str.toi( vec.get( spl, 6 ) );
                        if ( size == 11 ) {
                            p4 = str.toi( vec.get( spl, 8 ) );
                            p5 = str.toi( vec.get( spl, 9 ) );
                            v5 = str.toi( vec.get( spl, 10 ) );
                        }
                    }
                }

#if __cpusplus
        public UstEnvelope clone()
#else
                public Object clone()
#endif
                {
#if __cplusplus
            return this;
#else
                    return new UstEnvelope( toString() );
#endif
                }

#if JAVA
#elif __cplusplus
#else
                public object Clone()
                {
                    return clone();
                }
#endif

#if JAVA
#elif __cplusplus
#else
                public override string ToString()
                {
                    return toString();
                }
#endif

                public String toString()
                {
                    String ret = "Envelope=" + p1 + "," + p2 + "," + p3 + "," + v1 + "," + v2 + "," + v3 + "," + v4;
                    ret += ",%," + p4 + "," + p5 + "," + v5;
                    return ret;
                }

                public int getCount()
                {
                    return 5;
                }
            };

#if !JAVA
        }
    }
}
#endif
