/*
 * XmlSerializer.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ../BuildJavaUI/src/org/kbinani/xml/XmlSerializer.java
#else
using System;
using System.IO;
using org.kbinani.xml;

namespace org.kbinani.xml {

    public class XmlSerializer {
        private bool m_serialize_static_mode = false;
        System.Xml.Serialization.XmlSerializer m_serializer;
        XmlStaticMemberSerializer m_static_serializer;

        public XmlSerializer( Type cls )
            : this( cls, false ) {
        }

        public XmlSerializer( Type cls, bool serialize_static_mode ) {
            m_serialize_static_mode = serialize_static_mode;
            if ( serialize_static_mode ) {
                m_static_serializer = new XmlStaticMemberSerializer( cls );
            } else {
                m_serializer = new System.Xml.Serialization.XmlSerializer( cls );
            }
        }

        public object deserialize( Stream stream ) {
            if ( m_serialize_static_mode ) {
                m_static_serializer.Deserialize( stream );
                return null;
            } else {
                return m_serializer.Deserialize( stream );
            }
        }

        public void serialize( Stream stream, object obj ) {
            if ( m_serialize_static_mode ) {
                m_static_serializer.Serialize( stream );
            } else {
                System.Xml.XmlTextWriter xw = null;
                try {
                    xw = new System.Xml.XmlTextWriter( stream, null );
                    xw.Formatting = System.Xml.Formatting.None;
                    m_serializer.Serialize( xw, obj );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "XmlSerializer#serialize; ex=" + ex );
                }
            }
        }
    }

}
#endif
