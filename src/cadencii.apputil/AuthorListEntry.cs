/*
 * AuthorListEntry.cs
 * Copyright © 2007-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package cadencii.apputil;

import java.awt.Font;

#else
using System;
using cadencii.java.awt;

namespace cadencii.apputil {

#endif

    public class AuthorListEntry {
        string m_name = "";
        int m_style;
        string m_twtr_id = "";

        public AuthorListEntry( string name, string twitter_id, int style ) {
            m_name = name;
            m_twtr_id = twitter_id;
            m_style = style;
        }

        public AuthorListEntry( string name, int style )
#if JAVA
        {
#else
            :
#endif
            this( name, "", style )
#if JAVA
            ;
#else
        {
#endif
        }

        public AuthorListEntry( string name, string twitter_id )
#if JAVA
        {
#else
            :
#endif
            this( name, twitter_id, Font.PLAIN )
#if JAVA
            ;
#else
        {
#endif
        }

        public AuthorListEntry( string name )
#if JAVA
        {
#else
            :
#endif
            this( name, "", Font.PLAIN )
#if JAVA
            ;
#else
        {
#endif
        }

        public AuthorListEntry() {
            m_name = "";
            m_style = Font.PLAIN;
            m_twtr_id = "";
        }

        public string getName() {
            return m_name;
        }

        public int getStyle() {
            return m_style;
        }

        public string getTwitterID() {
            return m_twtr_id;
        }
    }

#if !JAVA
}
#endif
