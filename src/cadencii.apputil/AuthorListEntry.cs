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
using System;
using cadencii.java.awt;

namespace cadencii.apputil
{

    public class AuthorListEntry
    {
        string m_name = "";
        int m_style;
        string m_twtr_id = "";

        public AuthorListEntry(string name, string twitter_id, int style)
        {
            m_name = name;
            m_twtr_id = twitter_id;
            m_style = style;
        }

        public AuthorListEntry(string name, int style)
            : this(name, "", style)
        { }

        public AuthorListEntry(string name, string twitter_id)
            : this(name, twitter_id, Font.PLAIN)
        { }

        public AuthorListEntry(string name)
            : this(name, "", Font.PLAIN)
        { }

        public AuthorListEntry()
        {
            m_name = "";
            m_style = Font.PLAIN;
            m_twtr_id = "";
        }

        public string getName()
        {
            return m_name;
        }

        public int getStyle()
        {
            return m_style;
        }

        public string getTwitterID()
        {
            return m_twtr_id;
        }
    }

}
