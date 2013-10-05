/*
 * XmlStaticMemberSerializer.cs
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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

using Microsoft.CSharp;

namespace cadencii.xml
{


    /// <summary>
    /// クラスのstaticメンバーのxmlシリアライズ/デシリアライズを行うclass
    /// </summary>
    public class XmlStaticMemberSerializer
    {
        /// <summary>
        /// ターゲットとなるクラスから，シリアライズするメンバーを抽出する時に使用
        /// </summary>
        private class MemberEntry
        {
            /// <summary>
            /// プロパティ/フィールドの名前
            /// </summary>
            public string Name;
            /// <summary>
            /// プロパティ/フィールドの型
            /// </summary>
            public Type Type;
            /// <summary>
            /// プロパティ/フィールドのデフォルト値
            /// </summary>
            public object Default;

            public MemberEntry(string name, Type type, object default_)
            {
                Name = name;
                Type = type;
                Default = default_;
            }
        }

        /// <summary>
        /// シリアライズする対象の型．staticメンバーだけなので，インスタンスではなく型を保持
        /// </summary>
        private Type m_item;
        /// <summary>
        /// シリアライズ/デシリアライズするための内部型
        /// </summary>
        private Type m_config_type = null;
        /// <summary>
        /// m_config_typeで初期化されたシリアライザ
        /// </summary>
        private System.Xml.Serialization.XmlSerializer m_xs = null;

        private XmlStaticMemberSerializer()
        {
        }

        /// <summary>
        /// 指定された型をシリアライズするための初期化を行います
        /// </summary>
        /// <param name="item"></param>
        public XmlStaticMemberSerializer(Type item)
        {
            m_item = item;
        }

        /// <summary>
        /// シリアライズを行い，ストリームに書き込みます
        /// </summary>
        /// <param name="stream"></param>
        public void Serialize(Stream stream)
        {
            if (m_config_type == null) {
                GenerateConfigType();
            }
            ConstructorInfo ci = m_config_type.GetConstructor(new Type[] { });
            object config = ci.Invoke(new object[] { });
            foreach (FieldInfo target in m_config_type.GetFields()) {
                foreach (PropertyInfo pi in m_item.GetProperties(BindingFlags.Public | BindingFlags.Static)) {
                    if (target.Name == pi.Name && target.FieldType.Equals(pi.PropertyType) && pi.CanRead && pi.CanWrite) {
                        target.SetValue(config, pi.GetValue(m_item, new object[] { }));
                        break;
                    }
                }
                foreach (FieldInfo fi in m_item.GetFields(BindingFlags.Public | BindingFlags.Static)) {
                    if (target.Name == fi.Name && target.FieldType.Equals(fi.FieldType)) {
                        target.SetValue(config, fi.GetValue(m_item));
                        break;
                    }
                }
            }
            m_xs.Serialize(stream, config);
        }

        /// <summary>
        /// 指定したストリームを使って，デシリアライズを行います
        /// </summary>
        /// <param name="stream"></param>
        public void Deserialize(Stream stream)
        {
            if (m_config_type == null) {
                GenerateConfigType();
            }
            object config = m_xs.Deserialize(stream);
            if (config == null) {
                throw new ApplicationException("failed serializing internal config object");
            }
            foreach (FieldInfo target in m_config_type.GetFields()) {
                foreach (PropertyInfo pi in m_item.GetProperties(BindingFlags.Public | BindingFlags.Static)) {
                    if (target.Name == pi.Name && target.FieldType.Equals(pi.PropertyType) && pi.CanRead && pi.CanWrite) {
                        pi.SetValue(m_item, target.GetValue(config), new object[] { });
                        break;
                    }
                }
                foreach (FieldInfo fi in m_item.GetFields(BindingFlags.Public | BindingFlags.Static)) {
                    if (target.Name == fi.Name && target.FieldType.Equals(fi.FieldType)) {
                        fi.SetValue(m_item, target.GetValue(config));
                        break;
                    }
                }
            }
        }

        protected virtual Assembly Compile(string code)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.IncludeDebugInformation = true;
#if DEBUG
            sout.println( "XmlStaticMemberSerializer#Compile; code=" + code );
#endif
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            return results.CompiledAssembly;
        }

        /// <summary>
        /// シリアライズ用の内部型をコンパイルし，m_xsが使用できるようにします
        /// </summary>
        private void GenerateConfigType()
        {
            List<MemberEntry> config_names = CollectScriptConfigEntries(m_item);
            string code = GenerateClassCodeForXmlSerialization(config_names, m_item);
            Assembly asm = Compile(code);
            if (asm.GetTypes().Length <= 0) {
                m_config_type = null;
                m_xs = null;
                throw new ApplicationException("failed generating internal xml serizlizer");
            } else {
                m_config_type = (asm.GetTypes())[0];
                m_xs = new System.Xml.Serialization.XmlSerializer(m_config_type);
            }
        }

        /// <summary>
        /// 設定ファイルから読込むための型情報を蒐集
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static List<MemberEntry> CollectScriptConfigEntries(Type item)
        {
            List<MemberEntry> config_names = new List<MemberEntry>();
            foreach (PropertyInfo pi in item.GetProperties(BindingFlags.Static | BindingFlags.Public)) {
                object[] attrs = pi.GetCustomAttributes(true);
                foreach (object obj in attrs) {
                    if (obj.GetType().Equals(typeof(System.Xml.Serialization.XmlIgnoreAttribute))) {
                        continue;
                    }
                }
                if (pi.CanRead && pi.CanWrite) {
                    config_names.Add(new MemberEntry(pi.Name, pi.PropertyType, pi.GetValue(item, new object[] { })));
                }
            }
            foreach (FieldInfo fi in item.GetFields(BindingFlags.Static | BindingFlags.Public)) {
                object[] attrs = fi.GetCustomAttributes(true);
                foreach (object obj in attrs) {
                    if (obj.GetType().Equals(typeof(System.Xml.Serialization.XmlIgnoreAttribute))) {
                        continue;
                    }
                }
                config_names.Add(new MemberEntry(fi.Name, fi.FieldType, fi.GetValue(item)));
            }

            // 指定した型がorg.kbinani.xml.XmlSerializableを実装していたら
            return config_names;
        }

        /// <summary>
        /// 指定した型から、Reflectionを使ってxmlシリアライズ用のクラスをコンパイルするためのC#コードを作成します
        /// </summary>
        /// <param name="implemented"></param>
        /// <returns></returns>
        private static string GenerateClassCodeForXmlSerialization(List<MemberEntry> config_names, Type item)
        {
            // XmlSerialization用の型を作成
            string code = "";
            code += "using System;\n";
            code += "namespace org.kbinani.xml{\n";
            code += "    public class StaticMembersOf" + item.Name + "{\n";
            foreach (MemberEntry entry in config_names) {
                code += "        public " + actualTypeNameFrom(entry.Type.ToString()) + " " + entry.Name + ";\n";
            }
            code += "    }\n";
            code += "}\n";
            return code;
        }

        /// <summary>
        /// ソースコード上で利用可能な型名を調べます。
        /// System.Collections.Generic.List`1[System.String] =&gt; System.Collections.Generic.List&lt;System.String&gt;など。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string actualTypeNameFrom(string name)
        {
            int indx = name.IndexOf("`");
            if (indx >= 0) {
                int indx_bla = name.IndexOf("[", indx);
                int indx_cket = name.LastIndexOf("]");
                string contains = name.Substring(indx_bla + 1, indx_cket - indx_bla - 1);
                string str_num = name.Substring(indx + 1, indx_bla - indx - 1);
                string body = name.Substring(0, indx);

                int num = 0;
                if (int.TryParse(str_num, out num)) {
                    string[] spl = contains.Split(new char[] { ',' }, num);
                    string ret = body + "<";
                    for (int i = 0; i < num; i++) {
                        ret += (i > 0 ? "," : "") + actualTypeNameFrom(spl[i]);
                    }
                    ret += ">";
                    return ret;
                }
            }
            return name;
        }
    }

}
