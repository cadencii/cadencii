/*
 * VsqEventList.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;

namespace cadencii.vsq
{

    /// <summary>
    /// 固有ID付きのVsqEventのリストを取り扱う
    /// </summary>
    [Serializable]
    public class VsqEventList
    {
        public List<VsqEvent> Events;
        private List<int> m_ids;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VsqEventList()
        {
            Events = new List<VsqEvent>();
            m_ids = new List<int>();
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getXmlElementName(string name)
        {
            return name;
        }

        public int findIndexFromID(int internal_id)
        {
            int c = Events.Count;
            for (int i = 0; i < c; i++) {
                VsqEvent item = Events[i];
                if (item.InternalID == internal_id) {
                    return i;
                }
            }
            return -1;
        }

        public VsqEvent findFromID(int internal_id)
        {
            int index = findIndexFromID(internal_id);
            if (0 <= index && index < Events.Count) {
                return Events[index];
            } else {
                return null;
            }
        }

        public void setForID(int internal_id, VsqEvent value)
        {
            int c = Events.Count;
            for (int i = 0; i < c; i++) {
                if (Events[i].InternalID == internal_id) {
                    Events[i] = value;
                    break;
                }
            }
        }

        public void sort()
        {
            lock (this) {
                Events.Sort();
                updateIDList();
            }
        }

        public void clear()
        {
            Events.Clear();
            m_ids.Clear();
        }

        public IEnumerable<VsqEvent> iterator()
        {
            updateIDList();
            return Events;
        }

        public int add(VsqEvent item)
        {
            int id = getNextId(0);
            add(item, id);
            Events.Sort();
            int count = Events.Count;
            for (int i = 0; i < count; i++) {
                m_ids[i] = Events[i].InternalID;
            }
            return id;
        }

        public void add(VsqEvent item, int internal_id)
        {
            updateIDList();
            item.InternalID = internal_id;
            Events.Add(item);
            m_ids.Add(internal_id);
        }

        public void removeAt(int index)
        {
            updateIDList();
            Events.RemoveAt(index);
            m_ids.RemoveAt(index);
        }

        private int getNextId(int next)
        {
            updateIDList();
            int index = -1;
            List<int> current = new List<int>(m_ids);
            int nfound = 0;
            while (true) {
                index++;
                if (!current.Contains(index)) {
                    nfound++;
                    if (nfound == next + 1) {
                        return index;
                    } else {
                        current.Add(index);
                    }
                }
            }
        }

        public int getCount()
        {
            return Events.Count;
        }

        public VsqEvent getElement(int index)
        {
            return Events[index];
        }

        public void setElement(int index, VsqEvent value)
        {
            value.InternalID = Events[index].InternalID;
            Events[index] = value;
        }

        public void updateIDList()
        {
            if (m_ids.Count != Events.Count) {
                m_ids.Clear();
                int count = Events.Count;
                for (int i = 0; i < count; i++) {
                    m_ids.Add(Events[i].InternalID);
                }
            } else {
                int count = Events.Count;
                for (int i = 0; i < count; i++) {
                    m_ids[i] = Events[i].InternalID;
                }
            }
        }
    }

}
