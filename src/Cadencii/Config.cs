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
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii
{

#endif

    public class Config
    {
        private static SortedDictionary<string, Boolean> mDirectives = new SortedDictionary<string, Boolean>();

#if JAVA
        static
#else
        static Config()
#endif
        {
            mDirectives[ "script"] =  true ;
            mDirectives[ "vocaloid"] =  true ;
            mDirectives[ "aquestone"] =  true ;
            mDirectives[ "midi"] =  true ;
            mDirectives[ "debug"] =  false ;
            mDirectives[ "property"] =  true ;
        }

        public static string getWineVersion()
        {
            return "1.1.2";
        }

        public static SortedDictionary<string, Boolean> getDirectives()
        {
            SortedDictionary<string, Boolean> ret = new SortedDictionary<string, Boolean>();
            foreach (var key in mDirectives.Keys){
                ret[ key] =  mDirectives[ key ] ;
            }
            return ret;
        }

    }
#if !JAVA
}
#endif
