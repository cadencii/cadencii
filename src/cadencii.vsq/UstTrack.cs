/*
 * UstTrack.cs
 * Copyright © 2009-2011 kbinani
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

    public class UstTrack : ICloneable
    {
        public Object Tag;
        private List<UstEvent> m_events;

        public UstTrack()
        {
            m_events = new List<UstEvent>();
        }

        /// <summary>
        /// 指定したindex値を持つイベントを検索します
        /// </summary>
        /// <param name="index">検索するindex値</param>
        /// <returns>見つからなかったらnullを返す</returns>
        public UstEvent findEventFromIndex(int index)
        {
            foreach (UstEvent ue in m_events) {
                if (ue.Index == index) {
                    return ue;
                }
            }
            return null;
        }

        public UstEvent getEvent(int index)
        {
            return m_events[index];
        }

        public void setEvent(int index, UstEvent item)
        {
            m_events[index] = item;
        }

        public void addEvent(UstEvent item)
        {
            m_events.Add(item);
        }

        public void removeEventAt(int index)
        {
            m_events.RemoveAt(index);
        }

        public int getEventCount()
        {
            return m_events.Count;
        }

        public IEnumerable<UstEvent> getNoteEventIterator()
        {
            return m_events;
        }

        public Object clone()
        {
            UstTrack ret = new UstTrack();
            int c = m_events.Count;
            for (int i = 0; i < c; i++) {
                ret.m_events.Add((UstEvent)m_events[i].clone());
            }
            return ret;
        }

        public object Clone()
        {
            return clone();
        }
    }

}
