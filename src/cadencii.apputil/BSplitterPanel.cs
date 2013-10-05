/*
 * BSplitterPanel.cs
 * Copyright © 2008-2011 kbinani
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
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace cadencii.apputil
{

    public class BSplitterPanel : Panel
    {
        private BorderStyle m_border_style = BorderStyle.None;
        private Color m_border_color = Color.Black;

        public event EventHandler BorderStyleChanged;

        public BSplitterPanel()
            : base()
        {
            base.AutoScroll = false;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get
            {
                return m_border_color;
            }
            set
            {
                m_border_color = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new BorderStyle BorderStyle
        {
            get
            {
                return m_border_style;
            }
            set
            {
                BorderStyle old = m_border_style;
                m_border_style = value;
                if (m_border_style == BorderStyle.Fixed3D) {
                    base.BorderStyle = BorderStyle.Fixed3D;
                } else if (m_border_style == BorderStyle.FixedSingle) {
                    base.BorderStyle = BorderStyle.None;
                    base.Padding = new Padding(1);
                } else {
                    base.Padding = new Padding(0);
                    base.BorderStyle = BorderStyle.None;
                }
                if (old != m_border_style && BorderStyleChanged != null) {
                    BorderStyleChanged(this, new EventArgs());
                }
            }
        }

        [Browsable(false)]
        public new int Width
        {
            get
            {
                return base.Width;
            }
            internal set
            {
                base.Width = value;
            }
        }

        [Browsable(false)]
        public new int Height
        {
            get
            {
                return base.Height;
            }
            internal set
            {
                base.Height = value;
            }
        }

        [Browsable(false)]
        public new Rectangle Bounds
        {
            get
            {
                return base.Bounds;
            }
            internal set
            {
                base.Bounds = value;
            }
        }
    }

}
