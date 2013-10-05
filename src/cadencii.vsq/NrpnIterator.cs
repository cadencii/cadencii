/*
 * NrpnIterator.cs
 * Copyright © 2009-2013 kbinani
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
using System.Reflection;
using System.Collections.Generic;

namespace cadencii.vsq
{
    class NrpnIterator : IEnumerable<ValuePair<string, int>>
    {
        private List<ValuePair<string, int>> nrpns = new List<ValuePair<string, int>>();

        public NrpnIterator()
        {
            Type t = typeof(NRPN);
            foreach (FieldInfo fi in t.GetFields()) {
                if (fi.FieldType.Equals(typeof(int))) {
                    nrpns.Add(new ValuePair<string, int>(fi.Name, (int)fi.GetValue(t)));
                }
            }
        }

        public IEnumerator<ValuePair<string, int>> GetEnumerator()
        {
            return nrpns.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return nrpns.GetEnumerator();
        }
    }
}
