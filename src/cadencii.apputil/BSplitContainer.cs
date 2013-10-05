/*
 * BSplitContainer.cs
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
using System.Drawing;
using System.Windows.Forms;

namespace cadencii.apputil
{

    [Serializable]
    public partial class BSplitContainer : ContainerControl
    {
        private Orientation m_orientation = Orientation.Horizontal;
        private int m_splitter_distance = 50;
        private int m_panel1_min = 25;
        private int m_panel2_min = 25;
        private int m_splitter_width = 4;
        private bool m_splitter_moving = false;
        private int m_splitter_distance_draft = 50;
        private BSplitterPanel m_panel1;
        private BSplitterPanel m_panel2;
        private System.ComponentModel.IContainer components = null;
        private bool m_splitter_fixed = false;
        private Pen m_panel1_border = null;
        private Pen m_panel2_border = null;
        private System.Windows.Forms.FixedPanel m_fixed_panel;
        private PictureBox m_lbl_splitter;
        private int m_panel2_distance = 1;
        private double m_distance_rate = 0.5;

        public event SplitterEventHandler SplitterMoved;
        [Browsable(false)]
        public event ControlEventHandler ControlAdded;

        public BSplitContainer()
        {
            InitializeComponent();
            if (m_orientation == Orientation.Horizontal) {
                m_lbl_splitter.Cursor = Cursors.VSplit;
            } else {
                m_lbl_splitter.Cursor = Cursors.HSplit;
            }
            if (m_orientation == Orientation.Horizontal) {
                m_panel2_distance = this.Width - m_splitter_distance;
            } else {
                m_panel2_distance = this.Height - m_splitter_distance;
            }
            m_distance_rate = m_splitter_distance / (double)(m_splitter_distance + m_panel2_distance);
        }

        public int getHeight()
        {
            return this.Height;
        }

        public int getWidth()
        {
            return this.Width;
        }

        public void setPanel2Hidden(bool value)
        {
            if (value) {
                if (m_orientation == Orientation.Horizontal) {
                    setDividerLocation(getWidth());
                } else {
                    setDividerLocation(getHeight());
                }
                setSplitterFixed(true);
            } else {
                setSplitterFixed(false);
            }
        }

        public void setPanel1Hidden(bool value)
        {
            if (value) {
                setDividerLocation(0);
                setSplitterFixed(true);
            } else {
                setSplitterFixed(false);
            }
        }

        public System.Windows.Forms.FixedPanel FixedPanel
        {
            get
            {
                return m_fixed_panel;
            }
            set
            {
                System.Windows.Forms.FixedPanel old = m_fixed_panel;
                m_fixed_panel = value;
                if (m_fixed_panel != FixedPanel.None && m_fixed_panel != old) {
                    if (m_fixed_panel == FixedPanel.Panel1) {
                        m_panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                        if (m_orientation == Orientation.Vertical) {
                            m_panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                        } else {
                            m_panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                        }
                    } else {
                        m_panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                        if (m_orientation == Orientation.Vertical) {
                            m_panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                        } else {
                            m_panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                        }
                    }
                }
            }
        }

        public bool IsSplitterFixed
        {
            get
            {
                return m_splitter_fixed;
            }
            set
            {
                m_splitter_fixed = value;
                if (m_splitter_fixed) {
                    m_lbl_splitter.Cursor = Cursors.Default;
                } else {
                    if (m_orientation == Orientation.Horizontal) {
                        m_lbl_splitter.Cursor = Cursors.VSplit;
                    } else {
                        m_lbl_splitter.Cursor = Cursors.HSplit;
                    }
                }
            }
        }

        public bool isSplitterFixed()
        {
            return this.IsSplitterFixed;
        }

        public void setSplitterFixed(bool value)
        {
            this.IsSplitterFixed = value;
        }

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.m_lbl_splitter = new System.Windows.Forms.PictureBox();
            this.m_panel2 = new cadencii.apputil.BSplitterPanel();
            this.m_panel1 = new cadencii.apputil.BSplitterPanel();
            ((System.ComponentModel.ISupportInitialize)(this.m_lbl_splitter)).BeginInit();
            this.SuspendLayout();
            // 
            // m_lbl_splitter
            // 
            this.m_lbl_splitter.BackColor = System.Drawing.Color.Transparent;
            this.m_lbl_splitter.Location = new System.Drawing.Point(0, 0);
            this.m_lbl_splitter.Name = "m_lbl_splitter";
            this.m_lbl_splitter.Size = new System.Drawing.Size(100, 50);
            this.m_lbl_splitter.TabIndex = 0;
            this.m_lbl_splitter.TabStop = false;
            this.m_lbl_splitter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_lbl_splitter_MouseMove);
            this.m_lbl_splitter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_lbl_splitter_MouseDown);
            this.m_lbl_splitter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_lbl_splitter_MouseUp);
            // 
            // m_panel2
            // 
            this.m_panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_panel2.BorderColor = System.Drawing.Color.Black;
            this.m_panel2.Location = new System.Drawing.Point(0, 103);
            this.m_panel2.Margin = new System.Windows.Forms.Padding(0);
            this.m_panel2.Name = "m_panel2";
            this.m_panel2.Size = new System.Drawing.Size(441, 245);
            this.m_panel2.TabIndex = 1;
            this.m_panel2.BorderStyleChanged += new System.EventHandler(this.m_panel2_BorderStyleChanged);
            this.m_panel2.SizeChanged += new System.EventHandler(this.m_panel2_SizeChanged);
            // 
            // m_panel1
            // 
            this.m_panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_panel1.BorderColor = System.Drawing.Color.Black;
            this.m_panel1.Location = new System.Drawing.Point(0, 0);
            this.m_panel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.m_panel1.Name = "m_panel1";
            this.m_panel1.Size = new System.Drawing.Size(441, 99);
            this.m_panel1.TabIndex = 0;
            this.m_panel1.BorderStyleChanged += new System.EventHandler(this.m_panel1_BorderStyleChanged);
            this.m_panel1.SizeChanged += new System.EventHandler(this.m_panel1_SizeChanged);
            // 
            // SplitContainerEx
            // 
            this.Controls.Add(this.m_panel2);
            this.Controls.Add(this.m_panel1);
            this.Controls.Add(this.m_lbl_splitter);
            this.Size = new System.Drawing.Size(441, 348);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitContainerEx_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.m_lbl_splitter)).EndInit();
            this.ResumeLayout(false);
        }

        /// <summary>
        /// コントロールがユーザーとの対話に応答できるかどうかを示す値を設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setEnabled(bool value)
        {
            base.Enabled = value;
        }

        /// <summary>
        /// コントロールがユーザーとの対話に応答できるかどうかを示す値を取得します。
        /// </summary>
        /// <returns></returns>
        public bool isEnabled()
        {
            return base.Enabled;
        }

        private void SplitContainerEx_Paint(object sender, PaintEventArgs e)
        {
            bool panel1_visible = true;
            if (Orientation == Orientation.Horizontal) {
                if (m_panel1.Width == 0) {
                    panel1_visible = false;
                }
            } else {
                if (m_panel1.Height == 0) {
                    panel1_visible = false;
                }
            }
            if (m_panel1.BorderStyle == BorderStyle.FixedSingle && panel1_visible) {
                if (m_panel1_border == null) {
                    m_panel1_border = new Pen(m_panel1.BorderColor);
                } else {
                    if (!m_panel1.BorderColor.Equals(m_panel1_border.Color)) {
                        m_panel1_border = new Pen(m_panel1.BorderColor);
                    }
                }
                e.Graphics.DrawRectangle(m_panel1_border,
                                          new Rectangle(m_panel1.Left - 1, m_panel1.Top - 1, m_panel1.Width + 1, m_panel1.Height + 1));
            }

            bool panel2_visible = true;
            if (Orientation == Orientation.Horizontal) {
                if (m_panel2.Width == 0) {
                    panel2_visible = false;
                }
            } else {
                if (m_panel2.Height == 0) {
                    panel2_visible = false;
                }
            }
            if (m_panel2.BorderStyle == BorderStyle.FixedSingle && panel2_visible) {
                if (m_panel2_border == null) {
                    m_panel2_border = new Pen(m_panel2.BorderColor);
                } else {
                    if (!m_panel2.BorderColor.Equals(m_panel2_border.Color)) {
                        m_panel2_border = new Pen(m_panel2.BorderColor);
                    }
                }
                e.Graphics.DrawRectangle(m_panel2_border,
                                          new Rectangle(m_panel2.Left - 1, m_panel2.Top - 1, m_panel2.Width + 1, m_panel2.Height + 1));
            }
        }

        private void m_panel2_BorderStyleChanged(object sender, EventArgs e)
        {
            UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, m_panel2_min, false);
        }

        private void m_panel1_BorderStyleChanged(object sender, EventArgs e)
        {
            UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, m_panel2_min, false);
        }

        private void m_panel2_SizeChanged(object sender, EventArgs e)
        {
            m_panel2.Invalidate(true);
        }

        private void m_panel1_SizeChanged(object sender, EventArgs e)
        {
            m_panel1.Invalidate(true);
        }

        public int getPanel1MinSize()
        {
            return m_panel1_min;
        }

        public void setPanel1MinSize(int value)
        {
            int min_splitter_distance = value;
            if (m_splitter_distance < min_splitter_distance && min_splitter_distance > 0) {
                m_splitter_distance = min_splitter_distance;
            }
            UpdateLayout(m_splitter_distance, m_splitter_width, value, m_panel2_min, false);
        }

        public int Panel1MinSize
        {
            get
            {
                return getPanel1MinSize();
            }
            set
            {
                setPanel1MinSize(value);
            }
        }

        public int getPanel2MinSize()
        {
            return m_panel2_min;
        }

        public void setPanel2MinSize(int value)
        {
            int max_splitter_distance = (m_orientation == Orientation.Horizontal) ?
                                        this.Width - m_splitter_width - value :
                                        this.Height - m_splitter_width - value;
            if (m_splitter_distance > max_splitter_distance && max_splitter_distance > 0) {
                m_splitter_distance = max_splitter_distance;
            }
            UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, value, false);
        }

        public int Panel2MinSize
        {
            get
            {
                return getPanel2MinSize();
            }
            set
            {
                setPanel2MinSize(value);
            }
        }

        public int SplitterWidth
        {
            get
            {
                return m_splitter_width;
            }
            set
            {
                if (value < 0) {
                    value = 0;
                }
                UpdateLayout(m_splitter_distance, value, m_panel1_min, m_panel2_min, false);
            }
        }

        public int getDividerSize()
        {
            return this.SplitterWidth;
        }

        public void setDividerSize(int value)
        {
            this.SplitterWidth = value;
        }

        private bool UpdateLayout(int splitter_distance, int splitter_width, int panel1_min, int panel2_min, bool check_only)
        {
            Point mouse = this.PointToClient(Control.MousePosition);
            int pad1 = (m_panel1.BorderStyle == BorderStyle.FixedSingle) ? 1 : 0;
            int pad2 = (m_panel2.BorderStyle == BorderStyle.FixedSingle) ? 1 : 0;
            if (m_orientation == Orientation.Horizontal) {
                int p1 = splitter_distance;
                if (p1 < 0) {
                    p1 = 0;
                } else if (this.Width < p1 + splitter_width) {
                    p1 = this.Width - splitter_width;
                }
                int p2 = this.Width - p1 - splitter_width;
                if (check_only) {
                    if (p1 < panel1_min || p2 < panel2_min) {
                        return false;
                    }
                } else {
                    if (p1 < panel1_min) {
                        p1 = panel1_min;
                    }
                    p2 = this.Width - p1 - splitter_width;
                    if (p2 < panel2_min) {
                        p2 = panel2_min;
                        //return false;
                    }
                }
                if (!check_only) {
                    m_panel1.Left = pad1;
                    m_panel1.Top = pad1;
                    m_panel1.Width = (p1 - 2 * pad1 >= 0) ? (p1 - 2 * pad1) : 0;
                    m_panel1.Height = (this.Height - 2 * pad1 >= 0) ? (this.Height - 2 * pad1) : 0;

                    m_panel2.Left = p1 + splitter_width + pad2;
                    m_panel2.Top = pad2;
                    m_panel2.Width = (p2 - 2 * pad2 >= 0) ? (p2 - 2 * pad2) : 0;
                    m_panel2.Height = (this.Height - 2 * pad2 >= 0) ? (this.Height - 2 * pad2) : 0;

                    m_splitter_distance = p1;
                    m_panel2_distance = this.Width - m_splitter_distance;
                    m_distance_rate = m_splitter_distance / (double)(m_splitter_distance + m_panel2_distance);
                    if (SplitterMoved != null) {
                        SplitterMoved(this, new SplitterEventArgs(mouse.X, mouse.Y, p1, 0));
                    }
                    m_splitter_width = splitter_width;
                    m_panel1_min = panel1_min;
                    m_panel2_min = panel2_min;

                    m_lbl_splitter.Left = p1;
                    m_lbl_splitter.Top = 0;
                    m_lbl_splitter.Width = splitter_width;
                    m_lbl_splitter.Height = this.Height;
                }
            } else {
                int p1 = splitter_distance;
                if (p1 < 0) {
                    p1 = 0;
                } else if (this.Height < p1 + splitter_width) {
                    p1 = this.Height - splitter_width;
                }
                int p2 = this.Height - p1 - splitter_width;
                if (check_only) {
                    if (p1 < panel1_min || p2 < panel2_min) {
                        return false;
                    }
                } else {
                    if (p1 < panel1_min) {
                        p1 = panel1_min;
                    }
                    p2 = this.Height - p1 - splitter_width;
                    if (p2 < panel2_min) {
                        p2 = panel2_min;
                        //return false;
                    }
                }
                if (!check_only) {
                    m_panel1.Left = pad1;
                    m_panel1.Top = pad1;
                    m_panel1.Width = (this.Width - 2 * pad1 >= 0) ? (this.Width - 2 * pad1) : 0;
                    m_panel1.Height = (p1 - 2 * pad1 >= 0) ? (p1 - 2 * pad1) : 0;

                    m_panel2.Left = pad2;
                    m_panel2.Top = p1 + splitter_width + pad2;
                    m_panel2.Width = (this.Width - 2 * pad2 >= 0) ? (this.Width - 2 * pad2) : 0;
                    m_panel2.Height = (p2 - 2 * pad2 >= 0) ? (p2 - 2 * pad2) : 0;

                    m_splitter_distance = p1;
                    m_panel2_distance = this.Height - m_splitter_distance;
                    m_distance_rate = m_splitter_distance / (double)(m_splitter_distance + m_panel2_distance);
                    if (SplitterMoved != null) {
                        SplitterMoved(this, new SplitterEventArgs(mouse.X, mouse.Y, 0, p1));
                    }
                    m_splitter_width = splitter_width;
                    m_panel1_min = panel1_min;
                    m_panel2_min = panel2_min;

                    m_lbl_splitter.Left = 0;
                    m_lbl_splitter.Top = p1;
                    m_lbl_splitter.Width = this.Width;
                    m_lbl_splitter.Height = splitter_width;
                }
            }
            return true;
        }

        public int SplitterDistance
        {
            get
            {
                return getDividerLocation();
            }
            set
            {
                setDividerLocation(value);
            }
        }

        public int getDividerLocation()
        {
            return m_splitter_distance;
        }

        public void setDividerLocation(int value)
        {
            UpdateLayout(value, m_splitter_width, m_panel1_min, m_panel2_min, false);
            if (m_orientation == Orientation.Horizontal) {
                m_panel2_distance = this.Width - m_splitter_distance;
            } else {
                m_panel2_distance = this.Height - m_splitter_distance;
            }
        }

        public Orientation Orientation
        {
            get
            {
                return m_orientation;
            }
            set
            {
                if (m_orientation != value) {
                    m_orientation = value;
                    UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, m_panel2_min, false);
                    if (m_orientation == Orientation.Horizontal) {
                        m_lbl_splitter.Cursor = Cursors.VSplit;
                    } else {
                        m_lbl_splitter.Cursor = Cursors.HSplit;
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BSplitterPanel Panel1
        {
            get
            {
                return m_panel1;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BSplitterPanel Panel2
        {
            get
            {
                return m_panel2;
            }
        }

        public void setTopComponent(Control comp)
        {
            setLeftComponent(comp);
        }

        public void setBottomComponent(Control comp)
        {
            setRightComponent(comp);
        }

        public void setRightComponent(Control comp)
        {
            m_panel2.Controls.Clear();
            m_panel2.Controls.Add(comp);
        }

        public void setLeftComponent(Control comp)
        {
            m_panel1.Controls.Clear();
            m_panel1.Controls.Add(comp);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
#if DEBUG
            //Console.WriteLine( "BSplitContainer+OnSizeChanged" );
            //Console.WriteLine( "    FixedPanel=" + FixedPanel );
            //Console.WriteLine( "    m_splitter_distance=" + m_splitter_distance );
            //Console.WriteLine( "    Width=" + Width );
            //Console.WriteLine( "    Height=" + Height );
            //Console.WriteLine( "    m_panel2_distance=" + m_panel2_distance );
#endif
            base.OnSizeChanged(e);
            if (Width <= 0 || Height <= 0) {
                return;
            }
            if (m_fixed_panel == FixedPanel.Panel2) {
                if (m_orientation == Orientation.Horizontal) {
                    m_splitter_distance = this.Width - m_panel2_distance;
                } else {
                    m_splitter_distance = this.Height - m_panel2_distance;
                }
            } else if (m_fixed_panel == FixedPanel.None) {
#if DEBUG
                //Console.WriteLine( "    m_distance_rate=" + m_distance_rate );
#endif
                if (m_orientation == Orientation.Horizontal) {
                    m_splitter_distance = (int)(this.Width * m_distance_rate);
                } else {
                    m_splitter_distance = (int)(this.Height * m_distance_rate);
                }
            }
            UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, m_panel2_min, false);
        }

        private void m_lbl_splitter_MouseDown(object sender, MouseEventArgs e)
        {
            if (!m_splitter_fixed) {
                m_splitter_moving = true;
                m_splitter_distance_draft = m_splitter_distance;
                this.Cursor = (m_orientation == Orientation.Horizontal) ? Cursors.VSplit : Cursors.HSplit;
                m_lbl_splitter.BackColor = SystemColors.ControlDark;
                m_lbl_splitter.BringToFront();
            }
        }

        private void m_lbl_splitter_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_splitter_moving) {
                m_splitter_moving = false;
                UpdateLayout(m_splitter_distance_draft, m_splitter_width, m_panel1_min, m_panel2_min, false);
                this.Cursor = Cursors.Default;
                m_lbl_splitter.BackColor = SystemColors.Control;
            }
        }

        private void m_lbl_splitter_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (m_splitter_fixed) {
                return;
            }
            Point mouse_local = this.PointToClient(Control.MousePosition);
            if (m_splitter_moving) {
                int new_distance = m_splitter_distance;
                if (m_orientation == Orientation.Horizontal) {
                    new_distance = mouse_local.X;
                } else {
                    new_distance = mouse_local.Y;
                }
                if (UpdateLayout(new_distance, m_splitter_width, m_panel1_min, m_panel2_min, true)) {
                    m_splitter_distance_draft = new_distance;
                    if (m_orientation == Orientation.Horizontal) {
                        m_lbl_splitter.Left = m_splitter_distance_draft;
                    } else {
                        m_lbl_splitter.Top = m_splitter_distance_draft;
                    }
                }
            }
        }
    }

}
