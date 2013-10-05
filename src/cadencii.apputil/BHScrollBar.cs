/*
 * BHScrollBar.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Windows.Forms;

namespace cadencii.apputil
{

    /// <summary>
    /// Valueの値が正しくMinimumからMaximumの間を動くスクロールバー
    /// </summary>
    public partial class OBSOLUTE_BHScrollBar : UserControl
    {
        int m_max = 100;
        int m_min = 0;

        public event EventHandler ValueChanged;

        public OBSOLUTE_BHScrollBar()
        {
            InitializeComponent();
        }

        public int Value
        {
            get
            {
                return hScroll.Value;
            }
            set
            {
                hScroll.Value = value;
            }
        }

        public int LargeChange
        {
            get
            {
                return hScroll.LargeChange;
            }
            set
            {
                hScroll.LargeChange = value;
                hScroll.Maximum = m_max + value;
            }
        }

        public int SmallChange
        {
            get
            {
                return hScroll.SmallChange;
            }
            set
            {
                hScroll.SmallChange = value;
            }
        }

        public int Maximum
        {
            get
            {
                return m_max;
            }
            set
            {
                m_max = value;
                hScroll.Maximum = m_max + hScroll.LargeChange;
            }
        }

        public int Minimum
        {
            get
            {
                return m_min;
            }
            set
            {
                m_min = value;
            }
        }

        private void hScroll_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null) {
                ValueChanged(this, e);
            }
        }
    }

}
