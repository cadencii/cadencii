/*
 * Rebar.cs
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
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using cadencii;

namespace cadencii.windows.forms
{

    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>
#if !MONO
    [ToolboxItem(true),
    DefaultProperty("Bands"),
    Designer(typeof(cadencii.windows.forms.RebarDesigner)),
    DesignTimeVisible(true)]
#endif
    public class Rebar : System.Windows.Forms.Control
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.ImageList _imageList;
        private NativeRebar _rebar = null;
        private System.ComponentModel.Container components = null;
        private bool _autoSize = true;
        private RebarBandCollection _bands;
        private bool _bandBorders = true;
        private Color _embossHighlight;
        private bool _embossPicture = false;
        private Color _embossShadow;
        private bool _fixedOrder = false;
        private Orientation _orientation = Orientation.Horizontal;
        private bool _showBackgroundImage = true;
        private bool _resizing = false;
        private bool _throwExceptions = true;
        private bool _variantHeight = true;
        /// <summary>
        /// ダブルクリックでBANDの最大化・最小化を行う場合true
        /// </summary>
        private bool _toggleDoubleClick = false;
        /*
                public event RebarEventHandler AddBand;
                public event RebarEventHandler RemoveBand;
        */

        /// <summary>
        /// width of chevron. This value will be updated in NotifyChevronPushed method.
        /// </summary>
        public static int CHEVRON_WIDTH = 0;

        public Rebar()
        {
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);

            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            //SetStyle(ControlStyles.ResizeRedraw, true);

            _embossHighlight = SystemColors.ControlLightLight;
            _embossShadow = SystemColors.ControlDarkDark;
            _bands = new RebarBandCollection(this);
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

        }

        [Category("Behavior"),
        DefaultValue(false),
        NotifyParentProperty(true)]
        public bool ToggleDoubleClick
        {
            get
            {
                return _toggleDoubleClick;
            }
            set
            {
                if (value != _toggleDoubleClick) {
                    _toggleDoubleClick = value;
                    UpdateStyle();
                }
            }
        }

        [Category("Behavior"),
        DefaultValue(true),
        NotifyParentProperty(true)]
        public bool AutoSize
        {
            get
            {
                return _autoSize;
            }
            set
            {
                if (value != _autoSize) {
                    _autoSize = value;
                    UpdateStyle();
                }
            }
        }

        [Category("Appearance"),
        DefaultValue(typeof(Color), "Control"),
        NotifyParentProperty(true)]
        public override System.Drawing.Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                UpdateColors();
            }
        }

        [Category("Appearance"),
        DefaultValue(null),
        NotifyParentProperty(true)]
        public new Bitmap BackgroundImage
        {
            get
            {
                return (Bitmap)base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
                foreach (RebarBand band in _bands) {
                    if (band.UseCoolbarPicture & band.FixedBackground) band.BackgroundImage = (Bitmap)base.BackgroundImage;

                }
            }
        }

#if !MONO
        [Category("Behavior"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(cadencii.windows.forms.BandCollectionEditor),
        typeof(System.Drawing.Design.UITypeEditor)),
        NotifyParentProperty(true)]
#endif
        public RebarBandCollection Bands
        {
            get
            {
                return _bands;
            }
        }

        [Category("Appearance"),
        NotifyParentProperty(true),
        DefaultValue(true)]
        public bool BandBorders
        {
            get
            {
                return _bandBorders;
            }
            set
            {
                if (value != _bandBorders) {
                    _bandBorders = value;
                    UpdateStyle();
                }
            }
        }

        public RebarBand BandHitTest(Point pt)
        {
            foreach (RebarBand band in _bands) {
                if (band.Bounds.Contains(pt))
                    return band;
            }
            return null;
        }

        [Category("Appearance"),
        DefaultValue(typeof(Color), "ControlLightLight"),
        NotifyParentProperty(true)]
        public Color EmbossHighlight
        {
            get
            {
                return _embossHighlight;
            }
            set
            {
                if (value != _embossHighlight) {
                    //Code to set EmbossHighlight
                    _embossHighlight = value;
                    UpdateColors();
                }
            }
        }

        [Category("Appearance"),
        DefaultValue(false),
        NotifyParentProperty(true)]
        public bool EmbossPicture
        {
            get
            {
                return _embossPicture;
            }
            set
            {
                if (value != _embossPicture) {
                    _embossPicture = value;
                    UpdateStyle();
                }
            }
        }

        [Category("Appearance"),
        DefaultValue(typeof(Color), "ControlDarkDark"),
        NotifyParentProperty(true)]
        public Color EmbossShadow
        {
            get
            {
                return _embossShadow;
            }
            set
            {
                if (value != _embossShadow) {
                    //Code to set EmbossShadow
                    _embossShadow = value;
                    UpdateColors();
                }
            }
        }

        [Category("Appearance"),
        DefaultValue(false),
        NotifyParentProperty(true)]
        public bool FixedOrder
        {
            get
            {
                return _fixedOrder;
            }
            set
            {
                if (value != _fixedOrder) {
                    _fixedOrder = value;
                    UpdateStyle();
                }
            }
        }

        [Category("Appearance"),
        DefaultValue(typeof(Color), "ControlText"),
        NotifyParentProperty(true)]
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                UpdateColors();
            }
        }

        [Browsable(false),
        System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public RebarBand HitTest(Point pt)
        {//RB_HITTEST
            return null;
        }

        [Category("Appearance"),
        NotifyParentProperty(true),
        DefaultValue(null)]
        public ImageList ImageList
        {
            get
            {
                return _imageList;
            }
            set
            {
                if (value != _imageList) {
                    _imageList = value;
                    UpdateImageList();
                }
            }
        }

        [Category("Behavior"),
        NotifyParentProperty(true),
        DefaultValue(Orientation.Horizontal)]
        public Orientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                if (value != _orientation) {
                    _orientation = value;
                    _resizing = true;
                    UpdateStyle();
                    _resizing = false;
                }
            }
        }

        internal NativeRebar NativeRebar
        {
            get
            {
                return _rebar;
            }
        }

        internal IntPtr RebarHwnd
        {
            get
            {
                if (_rebar == null) {
                    return IntPtr.Zero;
                } else {
                    return _rebar.Handle;
                }
            }
        }

        [Browsable(false),
        System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public int RowCount
        {//RB_GETROWCOUNT
            get
            {
                return 0;
            }
        }

        [Category("Appearance"),
        Browsable(true),
        DefaultValue(true)]
        public bool ShowBackgroundImage
        {
            get
            {
                return _showBackgroundImage;
            }
            set
            {
                if (value != _showBackgroundImage) {
                    _showBackgroundImage = value;
                    foreach (RebarBand band in _bands) {
                        if (band.UseCoolbarPicture & band.FixedBackground) band.BackgroundImage = (_showBackgroundImage) ? BackgroundImage : null;
                    }
                }
            }
        }

        protected int Style
        {
            get
            {
                int style = (int)(win32.WS_BORDER |
                    win32.WS_CHILD |
                    win32.WS_CLIPCHILDREN |
                    win32.WS_CLIPSIBLINGS |
                    win32.CCS_NODIVIDER |
                    win32.CCS_NOPARENTALIGN
                    //|WindowsStyleConstants.CCS_NORESIZE
                    );
                if (_autoSize) {
                    style |= (int)win32.RBS_AUTOSIZE;
                }
                if (_bandBorders) {
                    style |= (int)win32.RBS_BANDBORDERS;
                }
                if (_orientation == Orientation.Vertical) {
                    style |= (int)win32.CCS_LEFT;
                } else {
                    style |= (int)win32.CCS_TOP;
                }
                if (this._variantHeight) {
                    style |= (int)win32.RBS_VARHEIGHT;
                }
                if (this.Visible) {
                    style |= (int)win32.WS_VISIBLE;
                }
                if (this._toggleDoubleClick) {
                    style |= (int)win32.RBS_DBLCLKTOGGLE;
                }

                return style;
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never)]
        private new string Text
        {
            get
            {
                return base.Text;
            }
        }

        [Category("Behavior"),
        Browsable(true),
        DefaultValue(true)]
        public bool ThrowExceptions
        {
            get
            {
                return _throwExceptions;
            }
            set
            {
                _throwExceptions = value;
            }
        }

        protected void UpdateColors()
        {
            if (Created && _rebar != null) {
                COLORSCHEME CSInfo = new COLORSCHEME();
                CSInfo.dwSize = (uint)Marshal.SizeOf(CSInfo);
                CSInfo.clrBtnHighlight = new COLORREF(_embossHighlight);
                CSInfo.clrBtnShadow = new COLORREF(_embossShadow);
                win32.SendMessage(_rebar.Handle, (int)win32.RB_SETCOLORSCHEME, 0, ref CSInfo);

                COLORREF color = new COLORREF(this.ForeColor);
                win32.SendMessage(_rebar.Handle, (int)win32.RB_SETTEXTCOLOR, 0, color);
                color = new COLORREF(this.BackColor);
                win32.SendMessage(_rebar.Handle, (int)win32.RB_SETBKCOLOR, 0, color);
            }
        }

        protected void UpdateStyle()
        {
            if (_rebar != null) {
                if (win32.SetWindowLongPtr(_rebar.Handle, (int)win32.GWL_STYLE, (IntPtr)Style).ToInt32() == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Updating Styles.", ex));
                    }
                } else {
                    this.Invalidate();
                    this.Refresh();
                }
            }
        }

        protected void UpdateImageList()
        {
            if (_rebar != null) {
                REBARINFO RBInfo = new REBARINFO();
                RBInfo.cbSize = (uint)Marshal.SizeOf(RBInfo);
                RBInfo.fMask = (uint)win32.RBIM_IMAGELIST;
                if (_imageList == null) {
                    RBInfo.himl = IntPtr.Zero;
                } else {
                    RBInfo.himl = _imageList.Handle;
                }
                if (win32.SendMessage(_rebar.Handle,
                    (int)win32.RB_SETBARINFO,
                    0, ref RBInfo) == 0) {

                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Setting Imagelist.", ex));
                    }
                }
            }
        }

        [Category("Appearance"),
        Browsable(true),
        DefaultValue(true),
        NotifyParentProperty(true)]
        public bool VariantHeight
        {
            get
            {
                return _variantHeight;
            }
            set
            {
                if (value != _variantHeight) {
                    //Code to set Style
                    _variantHeight = value;
                    UpdateStyle();
                }
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_rebar != null) {
                _rebar.DestroyHandle();
            }
            if (_imageList != null)
                _imageList.Dispose();
            if (disposing) {
                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ReBarWrapper
            // 
            this.Name = "ReBarWrapper";
            this.Size = new System.Drawing.Size(464, 64);
        }
        #endregion


        protected override void OnBackColorChanged(System.EventArgs e)
        {
            base.OnBackColorChanged(e);
            UpdateColors();
        }

        protected override void OnClick(System.EventArgs e)
        {
            base.OnClick(e);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            INITCOMMONCONTROLSEX ComCtls = new INITCOMMONCONTROLSEX();
            ComCtls.dwICC = (uint)win32.ICC_COOL_CLASSES;
            ComCtls.dwSize = (uint)Marshal.SizeOf(ComCtls);
            bool Result = win32.InitCommonControlsEx(ref ComCtls);
            if (!Result) {
                MessageBox.Show("There was a tragic error.  InitCommControlsEx Failed!");
            } else {

                _rebar = new NativeRebar(win32.CreateWindowEx(
                    0U,
                    "ReBarWindow32",
                    null,
                    (uint)Style,
                    0, 0, this.Width, this.Height,
                    this.Handle,
                    (IntPtr)1,//This will always be the only child window
                    IntPtr.Zero,
                    0
                    ));
                win32.SetWindowLongPtr(_rebar.Handle, (int)win32.GWL_EXSTYLE, (IntPtr)win32.WS_EX_TOOLWINDOW);
                _rebar.WindowPosChanging += new NativeRebarEventHandler(OnWindowPosChanging);
                _rebar.WindowsMessageRecieved += new NativeRebarEventHandler(OnWindowsMessageRecieved);
                UpdateImageList();
                UpdateColors();
                foreach (RebarBand band in _bands) {
                    band.CreateBand();
                }
            }
        }

        protected override void OnDoubleClick(System.EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            UpdateColors();
        }

        protected override void OnHandleCreated(System.EventArgs e)
        {
            base.OnHandleCreated(e);

        }

        protected override void OnLayout(System.Windows.Forms.LayoutEventArgs levent)
        {
            base.OnLayout(levent);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            RebarBand band = BandHitTest(new Point(e.X, e.Y));
            if (band != null) {
                band.OnMouseDown(e);
            }
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseHover(System.EventArgs e)
        {
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            RebarBand band = BandHitTest(new Point(e.X, e.Y));
            if (band != null) {
                band.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            RebarBand band = BandHitTest(new Point(e.X, e.Y));
            if (band != null) {
                band.OnMouseUp(e);
            }
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            RebarBand band = BandHitTest(new Point(e.X, e.Y));
            if (band != null) {
                band.OnMouseWheel(e);
            }
        }

        protected override void OnMove(System.EventArgs e)
        {
            base.OnMove(e);
        }

        protected override void OnNotifyMessage(System.Windows.Forms.Message m)
        {
            System.Diagnostics.Debug.WriteLine("Notify Message");
            base.OnNotifyMessage(m);
        }

        protected override void OnResize(System.EventArgs e)
        {
            bool ResizeSetting = _resizing;
            _resizing = true;
            if (_rebar != null) {
                if (_orientation == Orientation.Horizontal) {
                    int height = _rebar.BarHeight;
                    if (Height != height) {
                        Height = height;
                    }
                    if (this.Width != _rebar.BarWidth) {
                        if (win32.MoveWindow(_rebar.Handle, 0, 0, this.Width, height, true) == 0) {
                            System.Diagnostics.Debug.WriteLine(Marshal.GetLastWin32Error());
                            try {
                                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
                            } catch (Exception ex) {
                                System.Diagnostics.Debug.WriteLine("Something went wrong resizing the window: " + ex.Message);
                                if (_throwExceptions) throw (new Exception("Error Resizing.", ex));
                            }
                        } else {
                            Refresh();
                        }
                    }
                } else {//Vertical
                    int width = _rebar.BarWidth;
                    if (Width != width) {
                        Width = width;
                    }
                    if (Height != _rebar.BarHeight) {
                        if (win32.MoveWindow(_rebar.Handle, 0, 0, width, this.Height, true) == 0) {
                            try {
                                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
                            } catch (Exception ex) {
                                System.Diagnostics.Debug.WriteLine("Something went wrong resizing the window: " + ex.Message);
                                if (_throwExceptions) throw (new Exception("Error Updating Styles.", ex));

                            }
                        } else {
                            Refresh();
                        }
                    }
                }
            }
            base.OnResize(e);
            _resizing = ResizeSetting;
        }

        internal void OnWindowPosChanging(object sender, NativeRebarEventArgs e)
        {
            if (_orientation == Orientation.Horizontal) {
                if (Height != _rebar.BarHeight) {
                    this.OnResize(new EventArgs());
                }
            } else { //Vertical
                if (Width != _rebar.BarWidth) {
                    this.OnResize(new EventArgs());
                }
            }
        }

        internal void OnWindowsMessageRecieved(object sender, NativeRebarEventArgs e)
        {
            //Send mouse messages to the parent window
            if (e.m.Msg == (int)win32.WM_LBUTTONDBLCLK |
                e.m.Msg == (int)win32.WM_LBUTTONDOWN |
                e.m.Msg == (int)win32.WM_LBUTTONUP |
                e.m.Msg == (int)win32.WM_MBUTTONDBLCLK |
                e.m.Msg == (int)win32.WM_MBUTTONDOWN |
                e.m.Msg == (int)win32.WM_MBUTTONUP |
                e.m.Msg == (int)win32.WM_RBUTTONDBLCLK |
                e.m.Msg == (int)win32.WM_RBUTTONDOWN |
                e.m.Msg == (int)win32.WM_RBUTTONUP |
                e.m.Msg == (int)win32.WM_MOUSEHOVER |
                e.m.Msg == (int)win32.WM_MOUSEMOVE |
                e.m.Msg == (int)win32.WM_MOUSEWHEEL) {
                win32.SendMessage(this.Handle, e.m.Msg, e.m.WParam, e.m.LParam);
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (_rebar != null && m.Msg == (int)win32.WM_NOTIFY) {
                NMHDR Notify = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NMHDR));
                if (Notify.idFrom == 1) {
                    switch (Notify.code) {
                        case win32.RBN_LAYOUTCHANGED: {
                            //System.Diagnostics.Debug.WriteLine("Layout Changed");
                            break;
                        }
                        case win32.RBN_AUTOSIZE: {
                            //System.Diagnostics.Debug.WriteLine("Autosized");
                            break;
                        }
                        case win32.RBN_BEGINDRAG: {
                            //System.Diagnostics.Debug.WriteLine("Begin Drag");
                            break;
                        }
                        case win32.RBN_ENDDRAG: {
                            //System.Diagnostics.Debug.WriteLine("End Drag");
                            break;
                        }
                        case win32.RBN_DELETEDBAND: {
                            //System.Diagnostics.Debug.WriteLine("Delete band");
                            break;
                        }
                        case win32.RBN_DELETINGBAND: {
                            //System.Diagnostics.Debug.WriteLine("Deleting Band");
                            break;
                        }
                        case win32.RBN_CHILDSIZE: {
                            NMREBARCHILDSIZE ChildSize = (NMREBARCHILDSIZE)Marshal.PtrToStructure(m.LParam, typeof(NMREBARCHILDSIZE));
                            Form form = this.FindForm();
                            if (form != null && !form.IsDisposed && form.WindowState == FormWindowState.Minimized) {
                                // 親フォームが最小化された状態の場合，Resizeを送信しない
                                break;
                            }
                            foreach (RebarBand band in _bands) {
                                if (band.ID == ChildSize.wID) {
                                    band.OnResize(new EventArgs());
                                    continue;
                                }
                            }
                            //System.Diagnostics.Debug.WriteLine("Child Sized");
                            break;
                        }
                        case win32.RBN_CHEVRONPUSHED: {
                            this.NotifyChevronPushed(ref m);
                            break;
                        }
                        default: {
                            //System.Diagnostics.Debug.WriteLine("Other Notify code recieved " + Notify.code);
                            break;
                        }
                    }
                }
                //System.Diagnostics.Debug.WriteLine("Control WndProc Notified");
            }
            base.WndProc(ref m);
        }

        private void NotifyChevronPushed(ref Message message)
        {
            NMREBARCHEVRON nrch = (NMREBARCHEVRON)message.GetLParam(
                typeof(NMREBARCHEVRON));
            int index = nrch.wID;
            if ((index >= 0) && (index < this._bands.Count) &&
                (this._bands[index] != null)) {
                Rectangle chevron_rc = new Rectangle(
                    nrch.rc.left, nrch.rc.top,
                    nrch.rc.right - nrch.rc.left, nrch.rc.bottom - nrch.rc.top);
                int chevron_width = chevron_rc.Width;
                if (CHEVRON_WIDTH != chevron_width) CHEVRON_WIDTH = chevron_width;
                this._bands[index].Show(this, chevron_rc);
            }
        }

    }

    internal class NativeRebar : NativeWindow
    {
        public event NativeRebarEventHandler WindowPosChanging;
        public event NativeRebarEventHandler WindowsMessageRecieved;

        internal NativeRebar(IntPtr handle)
        {
            base.AssignHandle(handle);
        }

        internal int BarHeight
        {
            get
            {
                RECT rect = new RECT();
                win32.GetWindowRect(Handle, ref rect);
                return rect.bottom - rect.top;
                //return (int)win32.SendMessage( this.Handle, (int)win32.RB_GETBARHEIGHT, 0U, 0U );
            }
        }

        internal int BarWidth
        {
            get
            {
                RECT rect = new RECT();
                win32.GetWindowRect(Handle, ref rect);
                return rect.right - rect.left;
            }
        }

        internal bool WidthHeightMatch(int Width, int Height)
        {
            return (Height == BarHeight && Width == BarWidth);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == (int)win32.WM_WINDOWPOSCHANGING) {
                OnWindowPosChanging(new NativeRebarEventArgs(m));
            }
            //System.Diagnostics.Debug.WriteLine("Message: " + m.Msg);
            OnWindowsMessageRecieved(new NativeRebarEventArgs(m));
            this.DefWndProc(ref m);

        }

        protected virtual void OnWindowPosChanging(NativeRebarEventArgs e)
        {
            if (WindowPosChanging != null) {
                WindowPosChanging(this, e);
            }
        }

        protected virtual void OnWindowsMessageRecieved(NativeRebarEventArgs e)
        {
            if (WindowsMessageRecieved != null) {
                WindowsMessageRecieved(this, e);
            }
        }
    }

    internal class NativeRebarEventArgs : EventArgs
    {
        Message _m;

        public NativeRebarEventArgs(Message m)
        {
            _m = m;
        }

        public Message m
        {
            get
            {
                return _m;
            }
        }
    }

    internal delegate void NativeRebarEventHandler(object sender, NativeRebarEventArgs e);
}
