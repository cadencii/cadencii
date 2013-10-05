/*
 * MemoryManager.cs
 * Copyright © 2009-2011 kbinani
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
using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// アンマネージドなメモリーの確保・解放を行うマネージャです。
    /// </summary>
    public class MemoryManager
    {
        /// <summary>
        /// 確保したメモリーへのポインターの一覧
        /// </summary>
        private List<IntPtr> mList = new List<IntPtr>();

        /// <summary>
        /// メモリを確保します
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public IntPtr malloc(int bytes)
        {
            IntPtr ret = Marshal.AllocHGlobal(bytes);
            mList.Add(ret);
            return ret;
        }

        /// <summary>
        /// メモリを開放します
        /// </summary>
        /// <param name="p"></param>
        public void free(IntPtr p)
        {
            var v = mList.FirstOrDefault((f) => f.Equals(p));
            if (v != IntPtr.Zero) {
                Marshal.FreeHGlobal(v);
                mList.Remove(v);
            }
        }

        /// <summary>
        /// このマネージャを使って確保されたメモリーのうち、未解放のものを全て解放します
        /// </summary>
        public void dispose()
        {
            foreach (var v in mList) {
                try {
                    Marshal.FreeHGlobal(v);
                } catch (Exception ex) {
                    serr.println("MemoryManager#dispose; ex=" + ex);
                }
            }
            mList.Clear();
        }

        /// <summary>
        /// デストラクタ。内部でdisposeメソッドを呼びます
        /// </summary>
        ~MemoryManager()
        {
            dispose();
        }
    }

}
