/*
 * XmlSerializer.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.xml.
 *
 * cadencii.xml is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.xml is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using cadencii.xml;

namespace cadencii.xml
{

    public class XmlSerializer
    {
        private bool m_serialize_static_mode = false;
        System.Xml.Serialization.XmlSerializer m_serializer;
        XmlStaticMemberSerializer m_static_serializer;

        public XmlSerializer(Type cls)
            : this(cls, false)
        {
        }

        public XmlSerializer(Type cls, bool serialize_static_mode)
        {
            m_serialize_static_mode = serialize_static_mode;
            if (serialize_static_mode) {
                m_static_serializer = new XmlStaticMemberSerializer(cls);
            } else {
                m_serializer = new System.Xml.Serialization.XmlSerializer(cls);
            }
        }

        public object deserialize(Stream stream)
        {
            if (m_serialize_static_mode) {
                m_static_serializer.Deserialize(stream);
                return null;
            } else {
                return m_serializer.Deserialize(stream);
            }
        }

        public void serialize(Stream stream, object obj)
        {
            if (m_serialize_static_mode) {
                m_static_serializer.Serialize(stream);
            } else {
                System.Xml.XmlTextWriter xw = null;
                try {
                    xw = new System.Xml.XmlTextWriter(stream, null);
                    xw.Formatting = System.Xml.Formatting.None;
                    m_serializer.Serialize(xw, obj);
                } catch (Exception ex) {
                    serr.println("XmlSerializer#serialize; ex=" + ex);
                }
            }
        }
    }

}
