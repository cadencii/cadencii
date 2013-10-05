/*
 * RebarBand.cs
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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using cadencii;

namespace cadencii.windows.forms
{
    /// <summary>
    /// Summary description for BandWrapper.
    /// </summary>
    public enum GripperSettings
    {
        Always,
        Auto,
        Never
    }

    [ToolboxItem(false)]
    public class RebarBand : Component, IDisposable
    {
        private RebarBandCollection _bands;
        private bool _allowVertical = true;
        private Color _backColor;
        private string _caption = "";
        private bool _created = false;
        private Control _child = null;
        protected bool _disposed = false;
        private bool _embossPicture = true;
        private bool _fixedBackground = true;
        private bool _fixedSize = false;
        private Color _foreColor;
        private GripperSettings _gripSettings = GripperSettings.Auto;
        private int _header = -1;
        private int _id = -1;
        private int _idealWidth = 0;
        private int _image = -1;
        private int _integral = 1;
        private string _key = "";
        private int _maxHeight = 40;
        private int _minHeight = 24;
        private int _minWidth = 24;
        private bool _newRow = true;
        private Bitmap _backgroundImage = null;
        private IntPtr _pictureHandle = IntPtr.Zero;
        private bool _showCaption = true;
        private bool _showIcon = false;
        private object _tag = null;
        private bool _throwExceptions = true;
        private bool _useCoolbarColors = true;
        private bool _useCoolbarPicture = true;
        private bool _visible = true;
        private int _bandSize = 0;
        private bool _useChevron = true;
        private bool _variantHeight = false;

        private const int SPACE_CHEVRON_MENU = 6;

        public event MouseEventHandler MouseDown; //Done
        public event MouseEventHandler MouseMove; //Done
        public event MouseEventHandler MouseUp; //Done
        public event MouseEventHandler MouseWheel; //Done
        public event EventHandler Move; //Fix
        public event EventHandler Resize; //Done
        public event EventHandler VisibleChanged; //Done

        public RebarBand()
        {
            _foreColor = SystemColors.ControlText;
            _backColor = SystemColors.Control;
        }

        ~RebarBand()
        {
            Dispose(false);
        }

        public void Show(Control control, Rectangle chevron_rect)
        {
            if (!(control is Rebar)) return;
            Rebar parent = (Rebar)control;
            // Bandの外形を調べる
            RECT rc_band = new RECT();
            if (win32.SendMessage(parent.RebarHwnd, win32.RB_GETRECT, this.BandIndex, ref rc_band) == 0) return;
            // chevronの分の幅を引く
            rc_band.right -= chevron_rect.Width;
            if (this._child == null) return;
            //TODO: このへんmanagedな処理に書き換える
            // ツールバーのボタンの数を調べる
            int num_buttons = (int)win32.SendMessage(this._child.Handle, (int)win32.TB_BUTTONCOUNT, 0, IntPtr.Zero);
            if (num_buttons <= 0) return;
            // ツールバーの各ボタンについて処理
            int hidden_start = num_buttons;
            // ツールバー
            if (!(this._child is ToolBar)) return;
            ToolBar toolbar = (ToolBar)this._child;
            for (int i = 0; i < num_buttons; i++) {
                // ボタンの外形を調べる
                RECT rc_button = new RECT();
                if (win32.SendMessage(this._child.Handle, win32.TB_GETITEMRECT, i, ref rc_button) == 0) return;
                rc_button.left += rc_band.left;
                rc_button.right += rc_band.left;
                rc_button.top += rc_band.top;
                rc_button.bottom += rc_band.top;
                RECT rc_intersect = new RECT();
                win32.IntersectRect(ref rc_intersect, ref rc_button, ref rc_band);
                if (win32.EqualRect(ref rc_intersect, ref rc_button)) {
                    // ボタンは隠れていないので続ける
                    continue;
                }
                hidden_start = i;
                break;
            }
            // 隠れているボタンが一つもない場合は何もしない
            if (hidden_start >= num_buttons) return;
            // pop-upメニューを作成する
            ContextMenu popup = new ContextMenu();
            for (int i = hidden_start; i < num_buttons; i++) {
                uint id = (uint)i;
                // ボタンの情報を調べながら，ポップアップに追加
                ToolBarButton button = toolbar.Buttons[i];
                if (button.Style == ToolBarButtonStyle.PushButton ||
                     button.Style == ToolBarButtonStyle.ToggleButton) {
                    MenuItem menu = new MenuItem();
                    menu.Text = button.Text;
                    menu.Tag = button;
                    menu.DrawItem += drawChevronMenuItem;
                    menu.MeasureItem += measureChevronMenuItem;
                    menu.OwnerDraw = true;
                    menu.Click += handleChevronMenuItemClick;
                    popup.MenuItems.Add(menu);
                } else if (button.Style == ToolBarButtonStyle.DropDownButton) {
                    if (button.DropDownMenu != null && button.DropDownMenu.MenuItems != null) {
                        MenuItem menu = new MenuItem();
                        cloneMenuItemRecursive(menu.MenuItems, button.DropDownMenu.MenuItems);
                        menu.Text = button.Text;
                        popup.MenuItems.Add(menu);
                    }
                }
            }
            // ポップアップメニューを表示
            popup.Show(control, new Point(chevron_rect.Left, chevron_rect.Bottom));
        }

        /// <summary>
        /// メニューアイテムの階層を再帰的にコピーします
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        private void cloneMenuItemRecursive(Menu.MenuItemCollection dest, Menu.MenuItemCollection src)
        {
            if (src.Count > 0) {
                foreach (MenuItem item in src) {
                    MenuItem clone = item.CloneMenu();
                    cloneMenuItemRecursive(clone.MenuItems, item.MenuItems);
                    dest.Add(clone);
                }
            }
        }

        private void handleChevronMenuItemClick(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (!(sender is MenuItem)) return;
            MenuItem menu = (MenuItem)sender;
            if (menu.Tag == null) return;
            if (!(menu.Tag is ToolBarButton)) return;
            ToolBarButton button = (ToolBarButton)menu.Tag;
            ToolBar parent = button.Parent;
            Rectangle rc = button.Rectangle;
            uint lparam = (uint)win32.MAKELONG(rc.Left + rc.Width / 2, rc.Top + rc.Height / 2);
            win32.SendMessage(
                parent.Handle,
                win32.WM_LBUTTONDOWN,
                win32.MK_LBUTTON,
                lparam);
            win32.SendMessage(
                parent.Handle,
                win32.WM_LBUTTONUP,
                win32.MK_LBUTTON,
                lparam);
        }

        void measureChevronMenuItem(object sender, MeasureItemEventArgs e)
        {
            if (!(sender is MenuItem)) return;
            MenuItem menu = (MenuItem)sender;
            if (menu.Tag == null) return;
            if (!(menu.Tag is ToolBarButton)) return;
            ToolBarButton button = (ToolBarButton)menu.Tag;
            SizeF text_size = e.Graphics.MeasureString(menu.Text, SystemInformation.MenuFont);
            int width = (int)text_size.Width;
            int height = (int)text_size.Height;
            height = Math.Max(height, button.Parent.Height);
            if (button.Parent != null && button.Parent.ImageList != null) {
                if (0 <= button.ImageIndex && button.ImageIndex < button.Parent.ImageList.Images.Count) {
                    Image img = button.Parent.ImageList.Images[button.ImageIndex];
                    if (img != null) {
                        width += img.Width;
                        height = Math.Max(height, img.Height);
                    }
                }
            }
            e.ItemHeight = height;
            e.ItemWidth = width;
        }

        private void drawChevronMenuItem(object sender, DrawItemEventArgs e)
        {
            Brush brush_back = ((e.State & DrawItemState.Selected) != 0) ?
                    SystemBrushes.Highlight :  // 選択時の背景色
                    SystemBrushes.Menu;       // 非選択時の背景色
            e.Graphics.FillRectangle(brush_back, e.Bounds);

            if (!(sender is MenuItem)) return;
            MenuItem menu = (MenuItem)sender;
            if (menu.Tag == null) return;
            if (!(menu.Tag is ToolBarButton)) return;
            ToolBarButton button = (ToolBarButton)menu.Tag;
            int x = 0;
            if (button.Parent != null && button.Parent.ImageList != null) {
                if (0 <= button.ImageIndex && button.ImageIndex < button.Parent.ImageList.Images.Count) {
                    Image img = button.Parent.ImageList.Images[button.ImageIndex];
                    if (img != null) {
                        int image_offset = (e.Bounds.Height - img.Height) / 2;
                        if (!button.Enabled) {
                            const float R = 0.298912f;
                            const float G = 0.586611f;
                            const float B = 0.114478f;

                            System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix(
                                new float[][]{
                                    new float[]{ R, R, R, 0, 0}, 
                                    new float[]{ G, G, G, 0, 0}, 
                                    new float[]{ B, B, B, 0, 0}, 
                                    new float[]{ 0, 0, 0, 1, 0}, 
                                    new float[]{ 0, 0, 0, 0, 1} });
                            System.Drawing.Imaging.ImageAttributes atr = new System.Drawing.Imaging.ImageAttributes();
                            atr.SetColorMatrix(cm);
                            e.Graphics.DrawImage(
                                img,
                                new Rectangle(
                                    e.Bounds.X + SPACE_CHEVRON_MENU,
                                    e.Bounds.Y + image_offset,
                                    img.Width,
                                    img.Height),
                                0, 0, img.Width, img.Height,
                                GraphicsUnit.Pixel,
                                atr);
                        } else {
                            button.Parent.ImageList.Draw(
                                e.Graphics,
                                e.Bounds.X + SPACE_CHEVRON_MENU,
                                e.Bounds.Y + image_offset,
                                button.ImageIndex);
                            x += button.Parent.ImageList.Images[button.ImageIndex].Width;
                        }
                    }
                }
            }
            SizeF text_size = e.Graphics.MeasureString(menu.Text, e.Font);
            int text_offset = (int)(e.Bounds.Height - text_size.Height) / 2;
            e.Graphics.DrawString(menu.Text, e.Font, Brushes.Black, e.Bounds.X + x + SPACE_CHEVRON_MENU, e.Bounds.Y + text_offset);
        }

        public bool UseChevron
        {
            get
            {
                return this._useChevron;
            }
            set
            {
                this._useChevron = value;
                if (this.Created) {
                    this.UpdateStyles();
                }
            }
        }

        [Browsable(false),
        System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int BandSize
        {
            get
            {
                if (this.Created) {
                    REBARBANDINFO info = new REBARBANDINFO();
                    info.fMask = (uint)win32.RBBIM_SIZE;

                    win32.SendMessage(
                        this._bands.Rebar.RebarHwnd,
                        (int)win32.RB_GETBANDINFO,
                        this.BandIndex,
                        ref info);

                    return (int)info.cx;
                } else {
                    return 0;
                }
            }
            set
            {
                this._bandSize = value;
                if (this._bandSize < 0) this._bandSize = 0;
                if (this.Created) {
                    REBARBANDINFO info = new REBARBANDINFO();
                    info.fMask = (uint)win32.RBBIM_SIZE;
                    info.cx = (uint)this._bandSize;

                    win32.SendMessage(
                        this._bands.Rebar.RebarHwnd,
                        (int)win32.RB_SETBANDINFOA,
                        this.BandIndex,
                        ref info);
                }
            }
        }

        [Browsable(true),
        DefaultValue(true),
        NotifyParentProperty(true)]
        public bool AllowVertical
        {
            get
            {
                return _allowVertical;
            }
            set
            {
                if (value != _allowVertical) {
                    //Code to set the style
                    _allowVertical = value;
                    UpdateStyles();
                }
            }
        }

        [Browsable(true),
        DefaultValue(typeof(Color), "Control"),
        NotifyParentProperty(true)]
        public Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                if (value != _backColor) {
                    //Code to set BackColor
                    _backColor = value;
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(null)]
        [NotifyParentProperty(true)]
        public Bitmap BackgroundImage
        {
            get
            {
                return _backgroundImage;
            }
            set
            {
                if (value != _backgroundImage) {
                    if (_pictureHandle != IntPtr.Zero) {
                        win32.DeleteObject(_pictureHandle);
                    }
                    _backgroundImage = value;
                    _pictureHandle = (value == null) ? IntPtr.Zero : _backgroundImage.GetHbitmap();
                    UpdatePicture();
                }
            }
        }

        [Browsable(false),
        System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int BandIndex
        {
            get
            {
                if (Created) {
                    return (int)win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_IDTOINDEX, (uint)_id, 0U);
                } else {
                    return -1;
                }
            }
        }

        [Browsable(false),
        System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Always)]
        public RebarBandCollection Bands
        {
            get
            {
                return _bands;
            }

            set
            {
                if (!Created) {
                    _bands = value;
                    _id = _bands.NextID();
                    if (_useCoolbarPicture)
                        BackgroundImage = _bands.Rebar.BackgroundImage;
                    CreateBand();
                }
            }
        }

        [Browsable(false),
        System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Always)]
        public Rectangle Bounds
        {
            get
            {
                if (Created) {
                    RECT rect = new RECT();
                    win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_GETRECT, BandIndex, ref rect);
                    return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
                } else {
                    return new Rectangle(0, 0, 0, 0);
                }
            }
        }

        [Browsable(true),
        DefaultValue(""),
        NotifyParentProperty(true)]
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                if (value != _caption) {
                    //Code to set Caption
                    _caption = value;
                    UpdateCaption();
                }
            }
        }

        [Browsable(true),
        DefaultValue(null),
        NotifyParentProperty(true)]
        public Control Child
        {
            get
            {
                return _child;
            }
            set
            {
                if (value != _child) {
                    if (_child != null && Created) {
                        _child.HandleCreated -= new EventHandler(OnChildHandleCreated);
                        _child.SizeChanged -= new EventHandler(OnChildSizeChanged);
                        _child.Move -= new EventHandler(OnChildMove);
                        _child.ParentChanged -= new EventHandler(OnChildParentChanged);
                        _child.Parent = _bands.Rebar.Parent;
                    }
                    //Code to set Child

                    _child = value;
                    if (_bands != null) {
                        _child.Parent = _bands.Rebar;
                        _child.HandleCreated += new EventHandler(OnChildHandleCreated);
                        _child.SizeChanged += new EventHandler(OnChildSizeChanged);
                        _child.Move += new EventHandler(OnChildMove);
                        _child.ParentChanged += new EventHandler(OnChildParentChanged);
                    }
                    UpdateChild();
                }

            }
        }

        [Browsable(false),
        System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Always)]
        public Rectangle ClientArea
        {
            get
            {
                if (Created) {
                    RECT rect = new RECT();
                    win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_GETBANDBORDERS, BandIndex, ref rect);
                    return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
                } else {
                    return new Rectangle(0, 0, 0, 0);
                }
            }
        }

        internal void CreateBand()
        {
            if (!Created && _bands != null && _bands.Rebar.NativeRebar != null) {
                if (_child != null) _child.Parent = _bands.Rebar;
                REBARBANDINFO rbBand = new REBARBANDINFO();
                rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
                rbBand.fMask = (uint)(win32.RBBIM_STYLE
                    | win32.RBBIM_ID | win32.RBBIM_TEXT
                    );//| RebarBandInfoConstants.RBBIM_HEADERSIZE);
                if (!_useCoolbarColors)
                    rbBand.fMask |= (uint)win32.RBBIM_COLORS;
                if (_child != null) //Add ChildSize stuff at some point
				{
                    rbBand.fMask |= (uint)win32.RBBIM_CHILD;
                }
                rbBand.fMask |= (uint)win32.RBBIM_CHILDSIZE;
                if (_image >= 0)
                    rbBand.fMask |= (uint)win32.RBBIM_IMAGE;
                if (_backgroundImage != null) {
                    rbBand.fMask |= (uint)win32.RBBIM_BACKGROUND;
                }
                rbBand.cx = (uint)_bandSize;
                rbBand.fMask |= (uint)win32.RBBIM_SIZE;
                rbBand.fMask |= (uint)win32.RBBIM_IDEALSIZE;
                rbBand.clrFore = new COLORREF(ForeColor);
                rbBand.clrBack = new COLORREF(BackColor);
                rbBand.fStyle = (uint)Style;
                if (_backgroundImage != null) {
                    rbBand.hbmBack = _pictureHandle;
                }
                rbBand.lpText = _caption;
                if (_child != null) {
                    rbBand.hwndChild = _child.Handle;
                    rbBand.cxMinChild = (uint)_minWidth;
                    rbBand.cyMinChild = (uint)_minHeight;
                    rbBand.cyIntegral = (uint)_integral;//0;
                    rbBand.cyChild = (uint)_minHeight;
                    rbBand.cyMaxChild = (uint)_maxHeight;
                    rbBand.cxIdeal = (uint)_idealWidth;
                }
                if (_showIcon) {
                    rbBand.iImage = _image;
                }
                rbBand.wID = (uint)_id;
                rbBand.cxHeader = (uint)_header;

                if (win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_INSERTBANDA, -1, ref rbBand) == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        Console.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Creating Band.", ex));
                    }

                } else {
                    _created = true;
                }

            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always)]
        public bool Created
        {
            get
            {
                return (_created);
            }
        }

        internal void DestroyBand()
        {
            if (Created) {
                win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_DELETEBAND, (uint)BandIndex, 0U);
                _bands = null;
                _created = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            DestroyBand();
            if (_pictureHandle != IntPtr.Zero) win32.DeleteObject(_pictureHandle);
            if (disposing) {

            }
            _disposed = true;
        }

        [Browsable(true),
        DefaultValue(true),
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
                    //Code to set Style
                    _embossPicture = value;
                    UpdateStyles();
                }
            }
        }

        [Browsable(true),
        DefaultValue(true),
        NotifyParentProperty(true)]
        public bool FixedBackground
        {
            get
            {
                return _fixedBackground;
            }
            set
            {
                if (value != _fixedBackground) {
                    //Code to set Style
                    _fixedBackground = value;
                    UpdateStyles();
                }
            }
        }

        [Browsable(true),
        DefaultValue(false),
        NotifyParentProperty(true)]
        public bool FixedSize
        {
            get
            {
                return _fixedSize;
            }
            set
            {
                if (value != _fixedSize) {
                    //Code to set Style
                    _fixedSize = value;
                    UpdateStyles();
                }
            }
        }

        [Browsable(true),
        DefaultValue(typeof(Color), "ControlText"),
        NotifyParentProperty(true)]
        public Color ForeColor
        {
            get
            {
                return _foreColor;
            }
            set
            {
                if (value != _foreColor) {
                    //Code to set ForeColor
                    _foreColor = value;
                    UpdateColors();
                }
            }
        }

        [Browsable(true),
        DefaultValue(GripperSettings.Auto),
        NotifyParentProperty(true)]
        public GripperSettings GripSettings
        {
            get
            {
                return _gripSettings;
            }
            set
            {
                if (value != _gripSettings) {
                    //Code to set Caption
                    _gripSettings = value;
                    UpdateStyles();
                }
            }
        }

        [Browsable(true),
        DefaultValue(0),
        NotifyParentProperty(true)]
        public int Header
        {
            get
            {
                return _header;
            }
            set
            {
                if (value != _header) {
                    //Set Band Header
                    _header = value;
                    UpdateMinimums();
                }
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int Height
        {
            get
            {
                return Bounds.Height;
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int ID
        {
            get
            {
                if (_bands != null)
                    return _id;
                else
                    return -1;
            }
        }

        [Browsable(true),
        DefaultValue(0),
        NotifyParentProperty(true)]
        public int IdealWidth
        {
            get
            {
                return _idealWidth;
            }
            set
            {
                if (value != _idealWidth) {
                    _idealWidth = value;
                    UpdateMinimums();
                }
            }
        }

        [Browsable(true),
        DefaultValue(-1),
        NotifyParentProperty(true)]
        public int Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != _image) {
                    //Set Image for band
                    _image = value;
                    UpdateIcon();
                }
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int Index
        {
            get
            {
                if (_bands != null)
                    return _bands.IndexOf(this);
                else
                    return -1;
            }
        }

        [Browsable(true),
        DefaultValue("1")]
        public int Integral
        {
            get
            {
                return _integral;
            }
            set
            {
                if (value != _integral) {
                    _integral = value;
                    UpdateMinimums();
                }
            }
        }

        [Browsable(true),
        DefaultValue("")]
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (value != _key) {
                    if (_bands != null & value != "") {
                        if (_bands[value] != null) {
                            if (_throwExceptions) throw (new ArgumentException("The key specified is not unique.", "Key"));
                            return;
                        }
                    }
                    _key = value;
                }
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int Left
        {
            get
            {
                return Bounds.Left;
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always)]
        public Point Location
        {
            get
            {
                return Bounds.Location;
            }
        }

        [Browsable(false),
        System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Always)]
        public MARGINS Margins
        {//RB_GETBANDMARGINS
            get
            {
                if (Created) {
                    if (OSFeature.Feature.GetVersionPresent(OSFeature.Themes) != null) {
                        MARGINS margins = new MARGINS();
                        win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_GETBANDMARGINS, 0, ref margins);
                        return margins;
                    }
                    return new MARGINS(0, 0, 0, 0);
                } else {
                    return new MARGINS(0, 0, 0, 0);
                }
            }
        }

        public void Maximize()
        {
            if (Created) {
                win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_MAXIMIZEBAND, (uint)BandIndex, (uint)_idealWidth);
            }
        }

        [Browsable(true),
        DefaultValue(24),
        NotifyParentProperty(true)]
        public int MaxHeight
        {
            get
            {
                return _maxHeight;
            }
            set
            {
                if (value != _maxHeight) {
                    //Set Band Height
                    _maxHeight = value;
                    UpdateMinimums();
                }
            }
        }

        public void Minimize()
        {
            if (Created) {
                win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_MINIMIZEBAND, (uint)BandIndex, (uint)_idealWidth);
            }
        }

        [Browsable(true),
        DefaultValue(24),
        NotifyParentProperty(true)]
        public int MinHeight
        {
            get
            {
                return _minHeight;
            }
            set
            {
                if (value != _minHeight) {
                    //Set Band Height
                    _minHeight = value;
                    UpdateMinimums();
                }
            }
        }

        [Browsable(true),
        DefaultValue(24),
        NotifyParentProperty(true)]
        public int MinWidth
        {
            get
            {
                return _minWidth;
            }
            set
            {
                if (value != _minWidth) {
                    //Set Band Width
                    _minWidth = value;
                    UpdateMinimums();
                }
            }
        }

        [Browsable(true),
        DefaultValue(true),
        NotifyParentProperty(true)]
        public bool NewRow
        {
            get
            {
                return _newRow;
            }
            set
            {
                if (value != _newRow) {
                    //Set Style
                    _newRow = value;
                    UpdateStyles();
                }
            }
        }

        protected void OnChildHandleCreated(object sender, EventArgs e)
        {
            //UpdateChild();
        }

        protected void OnChildMove(object sender, EventArgs e)
        {

        }

        protected void OnChildParentChanged(object sender, EventArgs e)
        {
            UpdateChild();
        }

        protected void OnChildSizeChanged(object sender, EventArgs e)
        {

        }

        internal void OnMouseDown(MouseEventArgs e)
        {
            if (MouseDown != null) {
                MouseDown(this, e);
            }
        }

        internal void OnMouseMove(MouseEventArgs e)
        {
            if (MouseMove != null) {
                MouseMove(this, e);
            }
        }

        internal void OnMouseUp(MouseEventArgs e)
        {
            if (MouseUp != null) {
                MouseUp(this, e);
            }
        }

        internal void OnMouseWheel(MouseEventArgs e)
        {
            if (MouseWheel != null) {
                MouseWheel(this, e);
            }
        }

        internal void OnMove(EventArgs e)
        {
            if (Move != null) {
                Move(this, e);
            }
        }

        internal void OnResize(EventArgs e)
        {
            if (Resize != null) {
                Resize(this, e);
            }
        }

        internal void OnVisibleChanged(EventArgs e)
        {
            if (VisibleChanged != null) {
                VisibleChanged(this, e);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int Position
        {
            get
            {
                if (Created) {
                    return BandIndex;
                } else if (_bands != null) {
                    return Index;
                } else {
                    return -1;
                }
            }
        }

        private bool ShouldSerializeForeColor()
        {
            return _foreColor != SystemColors.ControlText;
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool ShowCaption
        {
            get
            {
                return _showCaption;
            }
            set
            {
                if (value != _showCaption) {
                    //Set band style
                    _showCaption = value;
                    UpdateStyles();
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool ShowIcon
        {
            get
            {
                return _showIcon;
            }
            set
            {
                if (value != _showIcon) {
                    _showIcon = value;
                    UpdateIcon();
                }
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always)]
        public Size Size
        {
            get
            {
                return Bounds.Size;
            }
        }

        [Category("Appearance"),
        Browsable(true),
        DefaultValue(false),
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
                    UpdateStyles();
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        protected int Style
        {
            get
            {
                int style = 0;
                if (!_allowVertical)
                    style |= (int)win32.RBBS_NOVERT;
                if (_embossPicture)
                    style |= (int)win32.RBBS_CHILDEDGE;
                if (_fixedBackground)
                    style |= (int)win32.RBBS_FIXEDBMP;
                if (_fixedSize)
                    style |= (int)win32.RBBS_FIXEDSIZE;
                if (_newRow)
                    style |= (int)win32.RBBS_BREAK;
                if (!_showCaption)
                    style |= (int)win32.RBBS_HIDETITLE;
                if (!_visible)
                    style |= (int)win32.RBBS_HIDDEN;
                if (_gripSettings == GripperSettings.Always)
                    style |= (int)win32.RBBS_GRIPPERALWAYS;
                else if (_gripSettings == GripperSettings.Never)
                    style |= (int)win32.RBBS_NOGRIPPER;
                if (_useChevron)
                    style |= (int)win32.RBBS_USECHEVRON;
                if (_variantHeight) {
                    style |= win32.RBBS_VARIABLEHEIGHT;
                }
                return style;
            }
            set
            {
                _allowVertical = !((value & (int)win32.RBBS_NOVERT)
                    == (int)win32.RBBS_NOVERT);
                _embossPicture = (value & (int)win32.RBBS_CHILDEDGE)
                    == (int)win32.RBBS_CHILDEDGE;
                _fixedBackground = (value & (int)win32.RBBS_FIXEDBMP)
                    == (int)win32.RBBS_FIXEDBMP;
                _fixedSize = (value & (int)win32.RBBS_FIXEDSIZE)
                    == (int)win32.RBBS_FIXEDSIZE;
                _newRow = (value & (int)win32.RBBS_BREAK)
                    == (int)win32.RBBS_BREAK;
                _showCaption = !((value & (int)win32.RBBS_HIDETITLE)
                    == (int)win32.RBBS_HIDETITLE);
                _visible = !((value & (int)win32.RBBS_HIDDEN)
                    == (int)win32.RBBS_HIDDEN);
                _useChevron = !((value & (int)win32.RBBS_USECHEVRON)
                    == (int)win32.RBBS_USECHEVRON);
                if ((value & (int)win32.RBBS_GRIPPERALWAYS)
                    == (int)win32.RBBS_GRIPPERALWAYS) {
                    _gripSettings = GripperSettings.Always;
                } else if ((value & (int)win32.RBBS_NOGRIPPER)
                    == (int)win32.RBBS_NOGRIPPER) {
                    _gripSettings = GripperSettings.Never;
                } else {
                    _gripSettings = GripperSettings.Auto;
                }
                _variantHeight = ((value & win32.RBBS_VARIABLEHEIGHT) == win32.RBBS_VARIABLEHEIGHT);
                UpdateStyles();
            }
        }

        [Browsable(true),
        DefaultValue(null)]
        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
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

        protected void UpdateCaption()
        {
            if (Created) {
                REBARBANDINFO rbBand = new REBARBANDINFO();
                rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
                rbBand.fMask = (uint)win32.RBBIM_TEXT;
                rbBand.lpText = _caption;
                if (win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_SETBANDINFOA, BandIndex, ref rbBand) == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        Console.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Updating Caption.", ex));
                    }

                }
            }
        }

#if DEBUG
        public void setChildByHandle(IntPtr handle)
        {
        }
#endif

        private void UpdateChildByHandle(IntPtr handle)
        {
            if (Created) {
                REBARBANDINFO rbBand = new REBARBANDINFO();
                rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
                rbBand.fMask = (uint)win32.RBBIM_CHILD;
                if (handle.Equals(IntPtr.Zero)) {
                    rbBand.hwndChild = IntPtr.Zero;
                } else {
                    rbBand.hwndChild = handle;
                }

                if (win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_SETBANDINFOA, BandIndex, ref rbBand) == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        Console.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Updating Child.", ex));
                    }

                }
                UpdateMinimums();
            }
        }

        protected void UpdateChild()
        {
            UpdateChildByHandle(this._child == null ? IntPtr.Zero : this._child.Handle);
        }

        protected void UpdateColors()
        {
            if (Created) {
                REBARBANDINFO rbBand = new REBARBANDINFO();
                rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
                rbBand.fMask = (uint)win32.RBBIM_COLORS;
                if (_useCoolbarColors) {
                    rbBand.clrBack = new COLORREF();
                    rbBand.clrBack._ColorDWORD = (uint)win32.CLR_DEFAULT;
                    rbBand.clrFore = new COLORREF();
                    rbBand.clrFore._ColorDWORD = (uint)win32.CLR_DEFAULT;
                } else {
                    rbBand.clrBack = new COLORREF(_backColor);
                    rbBand.clrFore = new COLORREF(_foreColor);
                }

                if (win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_SETBANDINFOA, BandIndex, ref rbBand) == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        Console.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Updating Foreground and Background Colors.", ex));
                    }

                }
            }
        }

        protected void UpdateIcon()
        {
            if (Created) {
                REBARBANDINFO rbBand = new REBARBANDINFO();
                rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
                rbBand.fMask = (uint)win32.RBBIM_IMAGE;
                if (_showIcon) {
                    rbBand.iImage = _image;
                } else {
                    rbBand.iImage = -1;
                }

                if (win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_SETBANDINFOA, BandIndex, ref rbBand) == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        Console.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Updating Icon.", ex));
                    }
                }
            }
        }

        protected void UpdateMinimums()
        {
            //return;
            if (Created) {
                REBARBANDINFO rbBand = new REBARBANDINFO();
                rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
                rbBand.fMask = (uint)(win32.RBBIM_CHILDSIZE);
                if (_header != -1) rbBand.fMask |= (uint)win32.RBBIM_HEADERSIZE;
                rbBand.cxMinChild = (uint)_minWidth;
                rbBand.cyMinChild = (uint)_minHeight;
                rbBand.cyIntegral = (uint)_integral;//1;
                rbBand.cyChild = (uint)_minHeight;
                rbBand.cyMaxChild = (uint)_maxHeight;
                rbBand.cxIdeal = (uint)_idealWidth;
                rbBand.cxHeader = (uint)_header;
                if (win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_SETBANDINFOA, BandIndex, ref rbBand) == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        Console.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Updating Minimums.", ex));
                    }

                }

            }
        }

        protected void UpdatePicture()
        {
            if (Created) {
                REBARBANDINFO rbBand = new REBARBANDINFO();
                rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
                rbBand.fMask = (uint)win32.RBBIM_BACKGROUND;
                rbBand.hbmBack = _pictureHandle;

                if (win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_SETBANDINFOA, BandIndex, ref rbBand) == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        Console.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Updating Background.", ex));
                    }

                }
            }

        }

        protected void UpdateStyles()
        {
            if (Created) {
                REBARBANDINFO rbBand = new REBARBANDINFO();
                rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
                rbBand.fMask = (uint)win32.RBBIM_STYLE;
                rbBand.fStyle = (uint)Style;

                if (win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_SETBANDINFOA, BandIndex, ref rbBand) == 0) {
                    int LastErr = Marshal.GetHRForLastWin32Error();
                    try {
                        Marshal.ThrowExceptionForHR(LastErr);
                    } catch (Exception ex) {
                        Console.WriteLine(LastErr + " " + ex.Message);
                        if (_throwExceptions) throw (new Exception("Error Updating Styles.", ex));
                    }

                }
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool UseCoolbarColors
        {
            get
            {
                return _useCoolbarColors;
            }
            set
            {
                if (value != _useCoolbarColors) {
                    //Set the Colors
                    _useCoolbarColors = value;
                    UpdateColors();
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool UseCoolbarPicture
        {
            get
            {
                return _useCoolbarPicture;
            }
            set
            {
                if (value != _useCoolbarPicture) {
                    //Set the Picture
                    _useCoolbarPicture = value;
                    UpdatePicture();
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (value != _visible) {
                    //Set band style
                    _visible = value;
                    if (Created) {
                        win32.SendMessage(_bands.Rebar.RebarHwnd, (int)win32.RB_SHOWBAND, (uint)BandIndex, (_visible) ? 1U : 0U);
                        OnVisibleChanged(new System.EventArgs());
                    }
                }
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int Width
        {
            get
            {
                return Bounds.Width;
            }
        }
    }
}
