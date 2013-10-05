/*
 * RebarBandCollection.cs
 * Copyright © Anthony Baraff
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.windows.forms.
 *
 * cadencii.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace cadencii.windows.forms
{

    /// <summary>
    /// Summary description for BandCollection.
    /// </summary>
#if !MONO
    [Editor(typeof(cadencii.windows.forms.BandCollectionEditor),
        typeof(System.Drawing.Design.UITypeEditor))]
#endif
    public class RebarBandCollection : CollectionBase, IEnumerable
    {
        private Rebar _rebar;
        private int _idCounter = 0;

        public RebarBandCollection(Rebar rebar)
        {
            _rebar = rebar;
        }

        public RebarBand this[int Index]
        {
            get
            {
                return (RebarBand)List[Index];
            }
        }

        public RebarBand this[string Key]
        {
            get
            {
                if (Key == null) return null;
                foreach (RebarBand band in this) {
                    if (band.Key == Key) return band;
                }
                return null;
            }
        }

        public RebarBand Add(RebarBand band)
        {
            List.Add(band);
            band.Bands = this;
            return band;
        }

        public RebarBand BandFromID(int ID)
        {
            foreach (RebarBand band in this) {
                if (band.ID == ID)
                    return band;
            }
            return null;
        }

        public new void Clear()
        {
            for (; List.Count > 0; ) {
                Remove(0);
            }
            base.Clear();
        }

        public new BandEnumerator GetEnumerator()
        {
            return new BandEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(RebarBand band)
        {
            return List.IndexOf(band);
        }

        internal int NextID()
        {
            return _idCounter++;
        }

        public Rebar Rebar
        {
            get
            {
                return _rebar;
            }
        }

        public void Remove(int Index)
        {
            Remove(this[Index]);
        }

        public void Remove(string Key)
        {
            Remove(this[Key]);
        }

        public void Remove(RebarBand band)
        {
            band.DestroyBand();
            List.Remove(band);
            band.Dispose();
        }

        public class BandEnumerator : IEnumerator
        {
            private int Index;
            private RebarBandCollection Collection;

            public BandEnumerator(RebarBandCollection Bands)
            {
                Collection = Bands;
                Index = -1;
            }

            public bool MoveNext()
            {
                Index++;
                return (Index < Collection.Count);
            }

            public void Reset()
            {
                Index = -1;
            }

            public RebarBand Current
            {
                get
                {
                    return Collection[Index];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }
        }
    }
}
