/*
 * XmlSerializeWithDescription.cs
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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;

namespace cadencii.xml
{


    /// <summary>
    /// フィールド、またはプロパティの概要を格納するattribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class XmlItemDescription : Attribute
    {
        private string m_value = "";
        private string m_attribute_name = "description";

        public XmlItemDescription(string Value)
        {
            m_value = Value;
        }

        public XmlItemDescription(string AttributeName, string Value)
        {
            m_value = Value;
            m_attribute_name = AttributeName;
        }

        public string AttributeName
        {
            get
            {
                return m_attribute_name;
            }
        }

        public string Value
        {
            get
            {
                return m_value;
            }
        }
    }

    /// <summary>
    /// フィールドおよびプロパティを、XmlItemDescription属性の文字列を付加しながらXmlシリアライズする
    /// </summary>
    public class XmlSerializeWithDescription
    {
        private XmlTextWriter m_writer;
        private Type m_type;

        public XmlSerializeWithDescription()
        {
        }

        public void Serialize(Stream stream, object obj)
        {
            m_writer = new XmlTextWriter(stream, null);
            m_writer.Formatting = Formatting.Indented;
            m_writer.Indentation = 4;
            m_writer.IndentChar = ' ';
            m_writer.WriteStartDocument();
            m_writer.WriteStartElement(obj.GetType().Name);
            PrintItemRecurse(obj);
            m_writer.WriteEndElement();
            m_writer.WriteEndDocument();
            m_writer.Flush();
        }

        private void PrintItemRecurse(object obj)
        {
            Type t = obj.GetType();
            if (!TryWriteValueType(obj)) {
                if (t.IsGenericType) {
                    List<int> f = new List<int>();
                    Type list_type = f.GetType().GetGenericTypeDefinition();
                    if (t.GetGenericTypeDefinition().Equals(list_type)) {
                        Type[] gen = t.GetGenericArguments();
                        if (gen.Length == 1) {
                            PropertyInfo count_property = t.GetProperty("Count", typeof(int));
                            int count = (int)count_property.GetValue(obj, new object[] { });
                            Type returntype = gen[0];
                            MethodInfo indexer = t.GetMethod("get_Item", new Type[] { typeof(int) });
                            string name = "";
                            if (returntype.Equals(typeof(Boolean))) {
                                name = "boolean";
                            } else if (returntype.Equals(typeof(DateTime))) {
                                name = "dateTime";
                            } else if (returntype.Equals(typeof(Decimal))) {
                                name = "decimal";
                            } else if (returntype.Equals(typeof(Double))) {
                                name = "double";
                            } else if (returntype.Equals(typeof(Int32))) {
                                name = "int";
                            } else if (returntype.Equals(typeof(Int64))) {
                                name = "long";
                            } else if (returntype.Equals(typeof(Single))) {
                                name = "float";
                            } else if (returntype.Equals(typeof(string))) {
                                name = "string";
                            } else if (returntype.IsEnum) {
                                name = returntype.Name;
                            }
                            if (indexer != null && name != "") {
                                for (int i = 0; i < count; i++) {
                                    object value = indexer.Invoke(obj, new object[] { i });
                                    m_writer.WriteStartElement(name);
                                    TryWriteValueType(value);
                                    m_writer.WriteEndElement();
                                }
                            }
                        }
                    }
                } else {
                    foreach (FieldInfo fi in t.GetFields()) {
                        if (fi.IsPrivate || fi.IsStatic) {
                            continue;
                        }
                        object[] attr = fi.GetCustomAttributes(typeof(XmlItemDescription), false);
                        XmlItemDescription xid = null;
                        if (attr.Length > 0) {
                            xid = (XmlItemDescription)attr[0];
                        }
                        WriteContents(fi.Name, fi.GetValue(obj), xid);
                    }
                    foreach (PropertyInfo pi in t.GetProperties()) {
                        if (!pi.CanRead | !pi.CanWrite) {
                            continue;
                        }
                        if (!pi.GetSetMethod().IsPublic | !pi.GetGetMethod().IsPublic) {
                            continue;
                        }
                        if (pi.GetSetMethod().IsStatic | pi.GetGetMethod().IsStatic) {
                            continue;
                        }
                        object[] attr = pi.GetCustomAttributes(typeof(XmlItemDescription), false);
                        XmlItemDescription xid = null;
                        if (attr.Length > 0) {
                            xid = (XmlItemDescription)attr[0];
                        }
                        WriteContents(pi.Name, pi.GetValue(obj, new object[] { }), xid);
                    }
                }
            }
        }

        private bool TryWriteValueType(object obj)
        {
            Type t = obj.GetType();
            if (t.Equals(typeof(Boolean))) {
                m_writer.WriteValue((Boolean)obj);
                return true;
            } else if (t.Equals(typeof(DateTime))) {
                m_writer.WriteValue((DateTime)obj);
                return true;
            } else if (t.Equals(typeof(Decimal))) {
                m_writer.WriteValue((Decimal)obj);
                return true;
            } else if (t.Equals(typeof(Double))) {
                m_writer.WriteValue((Double)obj);
                return true;
            } else if (t.Equals(typeof(Int32))) {
                m_writer.WriteValue((Int32)obj);
                return true;
            } else if (t.Equals(typeof(Int64))) {
                m_writer.WriteValue((Int64)obj);
                return true;
            } else if (t.Equals(typeof(Single))) {
                m_writer.WriteValue((Single)obj);
                return true;
            } else if (t.Equals(typeof(string))) {
                m_writer.WriteString((string)obj);
                return true;
            } else if (t.IsEnum) {
                string val = Enum.GetName(t, obj);
                m_writer.WriteString(val);
                return true;
            } else {
                return false;
            }
        }

        private void WriteContents(string name, object next_obj, XmlItemDescription xid)
        {
            m_writer.WriteStartElement(name);
            if (xid != null) {
                m_writer.WriteAttributeString(xid.AttributeName, xid.Value);
            }
            PrintItemRecurse(next_obj);
            m_writer.WriteEndElement();
        }

        private void test()
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(int));
        }

    }

}
