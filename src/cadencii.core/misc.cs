/*
 * misc.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace cadencii
{

    public static class Misc
    {
        /// <summary>
        /// Reflectionを利用して、インスタンスのディープなクローニングを行います。
        /// クローン操作の対象はインスタンスのフィールドであり、値型のものは単なる代入を、
        /// 参照型の物であってかつClone(void)メソッドが実装されているものはCloneしたものを、
        /// それ以外は単に参照が代入されます
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object obj_clone(object obj)
        {
            Type t = obj.GetType();
            object ret;
            if (t.IsValueType) {
                return obj; //値型の場合
            } else {
                System.Reflection.MethodInfo mi = t.GetMethod("Clone", new Type[] { });
                if (mi != null && (mi.ReturnType == typeof(object) || mi.ReturnType == t)) {
                    // return値がobject型またはt型のCloneメソッドを持っている場合
                    ret = mi.Invoke(obj, new object[] { });
                } else {
                    // Cloneメソッドが無い場合。型のフィールドを列挙してobj_cloneをRecursiveに呼び出す
                    // 最初にコンストラクタを取得
                    System.Reflection.ConstructorInfo ctor = t.GetConstructor(new Type[] { });
                    if (ctor == null) {
                        // 引数無しのコンストラクタが必要。無い場合はどうしようもないので例外を投げる
                        throw new ApplicationException("obj_clone requires zero-argument constructor");
                    }
                    // インスタンスを作成
                    ret = ctor.Invoke(new object[] { });
                    // 全てのフィールドに対してobj_cloneを呼び、値をセットする
                    foreach (System.Reflection.FieldInfo fi in t.GetFields()) {
                        Type fieldtype = fi.FieldType;
                        fi.SetValue(ret, obj_clone(obj));
                    }
                }
                return ret;
            }
        }

        public static string getmd5(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] data = Encoding.Unicode.GetBytes(s);
            byte[] hash = md5.ComputeHash(data);
            return BitConverter.ToString(hash).ToLower().Replace("-", "_");
        }

        public static string getmd5(FileStream file_stream)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(file_stream);
            return BitConverter.ToString(hash).ToLower().Replace("-", "_");
        }

        /// <summary>
        /// 現在の実行アセンブリで使用されている型のリストを取得します
        /// </summary>
        /// <returns></returns>
        public static Type[] get_executing_types()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            List<Type> types = new List<Type>(asm.GetTypes());
            foreach (System.Reflection.AssemblyName asmname in asm.GetReferencedAssemblies()) {
                System.Reflection.Assembly asmref = System.Reflection.Assembly.Load(asmname);
                types.AddRange(asmref.GetTypes());
            }

            asm = System.Reflection.Assembly.GetCallingAssembly();
            types.AddRange(asm.GetTypes());
            foreach (System.Reflection.AssemblyName asmname in asm.GetReferencedAssemblies()) {
                System.Reflection.Assembly asmref = System.Reflection.Assembly.Load(asmname);
                types.AddRange(asmref.GetTypes());
            }

            asm = System.Reflection.Assembly.GetEntryAssembly();
            types.AddRange(asm.GetTypes());
            foreach (System.Reflection.AssemblyName asmname in asm.GetReferencedAssemblies()) {
                System.Reflection.Assembly asmref = System.Reflection.Assembly.Load(asmname);
                types.AddRange(asmref.GetTypes());
            }

            List<Type> ret = new List<Type>();
            foreach (Type t in types) {
                if (t.IsPublic && !ret.Contains(t)) {
                    ret.Add(t);
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// 現在の実行アセンブリで使用されている名前空間のリストを取得します
        /// </summary>
        /// <returns></returns>
        public static string[] get_executing_namespaces()
        {
            Type[] types = get_executing_types();
            List<string> list = new List<string>();
            foreach (Type t in types) {
                if (!list.Contains(t.Namespace)) {
                    list.Add(t.Namespace);
                }
            }
            list.Sort();
            return list.ToArray();
        }
    }

    public static class debug
    {
        private static StreamWriter s_debug_out = null;
        private static string s_path = "";

        public static void force_logfile_path(string path)
        {
            try {
                if (s_debug_out != null) {
                    s_debug_out.Close();
                    s_debug_out = new StreamWriter(path);
                }
            } catch {
            }
            s_path = path;
        }

        public static void push_log(string s)
        {
            try {
                if (s_debug_out == null) {
                    if (s_path == "") {
                        s_debug_out = new StreamWriter(Path.Combine(System.Windows.Forms.Application.StartupPath, "run.log"));
                    } else {
                        s_debug_out = new StreamWriter(s_path);
                    }
                    s_debug_out.AutoFlush = true;
                    s_debug_out.WriteLine("************************************************************************");
                    s_debug_out.WriteLine("  Date: " + DateTime.Now.ToString());
                    s_debug_out.WriteLine("------------------------------------------------------------------------");
                }
                s_debug_out.WriteLine(s);
            } catch (Exception ex) {
                Console.WriteLine("org.kbinani.debug.push_log; log file I/O Exception");
            }
            Console.WriteLine(s);
        }

        public static void close()
        {
            if (s_debug_out != null) {
                s_debug_out.Close();
                s_debug_out = null;
                s_path = "";
            }
        }
    }

}
