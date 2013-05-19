/*
 * AuthorListEntry.cs
 * Copyright © 2007-2011 kbinani
 *
 * This file is part of org.kbinani.apputil.
 *
 * org.kbinani.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.apputil is distributed in the hope that it will be useful,
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
        String m_name = "";
        int m_style;
        String m_twtr_id = "";

        public AuthorListEntry( String name, String twitter_id, int style ) {
            m_name = name;
            m_twtr_id = twitter_id;
            m_style = style;
        }

        public AuthorListEntry( String name, int style )
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

        public AuthorListEntry( String name, String twitter_id )
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

        public AuthorListEntry( String name )
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

        public String getName() {
            return m_name;
        }

        public int getStyle() {
            return m_style;
        }

        public String getTwitterID() {
            return m_twtr_id;
        }
    }

#if !JAVA
}
#endif
