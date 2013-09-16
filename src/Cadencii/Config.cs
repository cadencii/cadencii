/*
 * Config.cs
 * Copyright © 2013 kbinani
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
#if JAVA

package cadencii;

import java.util.*;

#else

using System;
using cadencii.java.util;

namespace cadencii
{

#endif

    public class Config
    {
        private static TreeMap<String, Boolean> mDirectives = new TreeMap<String, Boolean>();

#if JAVA
        static
#else
        static Config()
#endif
        {
            mDirectives.put( "script", true );
            mDirectives.put( "vocaloid", true );
            mDirectives.put( "aquestone", true );
            mDirectives.put( "midi", true );
            mDirectives.put( "debug", false );
            mDirectives.put( "property", true );
        }

        public static String getWineVersion()
        {
            return "1.1.2";
        }

        public static TreeMap<String, Boolean> getDirectives()
        {
            TreeMap<String, Boolean> ret = new TreeMap<String, Boolean>();
            for( Iterator<String> itr = mDirectives.keySet().iterator(); itr.hasNext(); ){
                String key = itr.next();
                ret.put( key, mDirectives.get( key ) );
            }
            return ret;
        }

    }
#if !JAVA
}
#endif
