using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using WindowsUtilities;

namespace RebarDotNet
{
	/// <summary>
	/// Summary description for BandWrapper.
	/// </summary>
	public enum GripperSettings{Always, Auto, Never}; 

	[ToolboxItem(false)]
	public class BandWrapper: Component, IDisposable
	{
		
		private BandCollection _bands;
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
		private int _maxHeight = 0;
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

		public event MouseEventHandler MouseDown; //Done
		public event MouseEventHandler MouseMove; //Done
		public event MouseEventHandler MouseUp; //Done
		public event MouseEventHandler MouseWheel; //Done
		public event EventHandler Move; //Fix
		public event EventHandler Resize; //Done
		public event EventHandler VisibleChanged; //Done

		public BandWrapper()
		{
			_foreColor = SystemColors.ControlText;
			_backColor = SystemColors.Control;
		}

		~BandWrapper()
		{
			Dispose(false);
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
				if(value != _allowVertical)
				{
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
				if(value != _backColor)
				{
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
				if(value != _backgroundImage)
				{
					if(_pictureHandle != IntPtr.Zero) Gdi32Dll.DeleteObject(_pictureHandle);
					_backgroundImage = value;
					_pictureHandle = (value == null)?IntPtr.Zero:_backgroundImage.GetHbitmap();
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
				if(Created)
				{
					return (int)User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_IDTOINDEX, (uint)_id, 0U);
				}
				else
				{
					return -1;
				}
			}
		}

		[Browsable(false),
		System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Always)]
		public BandCollection Bands
		{
			get
			{
				return _bands;
			}

			set
			{
				if(!Created)
				{
					_bands = value;
					_id = _bands.NextID();
					if(_useCoolbarPicture)
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
				if(Created)
				{
					RECT rect = new RECT();
					User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_GETRECT, BandIndex ,ref rect);
					return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
				}
				else
				{
					return new Rectangle(0,0,0,0);
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
				if(value != _caption)
				{
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
				if(value != _child)
				{
					if(_child != null && Created)
					{
						_child.HandleCreated -= new EventHandler(OnChildHandleCreated);
						_child.SizeChanged -= new EventHandler(OnChildSizeChanged);
						_child.Move -= new EventHandler(OnChildMove);
						_child.ParentChanged -=new EventHandler(OnChildParentChanged);
						_child.Parent = _bands.Rebar.Parent;
					}
					//Code to set Child
					
					_child = value;
					if(_bands !=null)
					{
						_child.Parent = _bands.Rebar;
						_child.HandleCreated += new EventHandler(OnChildHandleCreated);
						_child.SizeChanged += new EventHandler(OnChildSizeChanged);
						_child.Move += new EventHandler(OnChildMove);
						_child.ParentChanged +=new EventHandler(OnChildParentChanged);
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
				if(Created)
				{
					RECT rect = new RECT();
					User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_GETBANDBORDERS, BandIndex ,ref rect);
					return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
				}
				else
				{
					return new Rectangle(0,0,0,0);
				}
			}
		}

		internal void CreateBand()
		{
			if(!Created && _bands != null && _bands.Rebar.Rebar != null)
			{
				if(_child != null) _child.Parent = _bands.Rebar;
				REBARBANDINFO rbBand = new REBARBANDINFO();
				rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
				rbBand.fMask = (uint)(RebarBandInfoConstants.RBBIM_STYLE
					| RebarBandInfoConstants.RBBIM_ID | RebarBandInfoConstants.RBBIM_TEXT
					);//| RebarBandInfoConstants.RBBIM_HEADERSIZE);
				if(!_useCoolbarColors)
					rbBand.fMask |= (uint) RebarBandInfoConstants.RBBIM_COLORS;
				if(_child != null) //Add ChildSize stuff at some point
				{
					rbBand.fMask |= (uint) RebarBandInfoConstants.RBBIM_CHILD;
				}
				rbBand.fMask |= (uint) RebarBandInfoConstants.RBBIM_CHILDSIZE;
				if(_image >= 0)
					rbBand.fMask |= (uint) RebarBandInfoConstants.RBBIM_IMAGE;
				if(_backgroundImage != null)
					rbBand.fMask |= (uint) RebarBandInfoConstants.RBBIM_BACKGROUND;
				rbBand.clrFore = new COLORREF(ForeColor);
				rbBand.clrBack = new COLORREF(BackColor);
				rbBand.fStyle = (uint)Style; 
				if(_backgroundImage != null)
				{
					rbBand.hbmBack = _pictureHandle;
				}
				rbBand.lpText = _caption;
				if(_child != null)
				{
					rbBand.hwndChild = _child.Handle;
					rbBand.cxMinChild = (uint)_minWidth;
					rbBand.cyMinChild = (uint)_minHeight;
					rbBand.cyIntegral = (uint)_integral;//0;
					rbBand.cyChild = (uint)_minHeight;
					rbBand.cyMaxChild = 40;
					rbBand.cxIdeal = (uint)_idealWidth;
				}
				if(_showIcon)
				{
					rbBand.iImage = _image;
				}
				rbBand.wID = (uint)_id;
				rbBand.cxHeader = (uint)_header;
				
				if(User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_INSERTBANDA, -1,ref rbBand) == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						Console.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Creating Band.", ex));
					}
						
				}
				else
				{
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
			if(Created)
			{
				User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_DELETEBAND, (uint)BandIndex, 0U);
				_bands = null;
				_created = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			DestroyBand();
			if(_pictureHandle != IntPtr.Zero) Gdi32Dll.DeleteObject(_pictureHandle);
			if(disposing)
			{
				
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
				if(value != _embossPicture)
				{
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
				if(value != _fixedBackground)
				{
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
				if(value != _fixedSize)
				{
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
				if(value != _foreColor)
				{
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
				if(value != _gripSettings)
				{
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
				if(value != _header)
				{
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
				if(_bands != null)
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
				if(value != _idealWidth)
				{
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
				if(value != _image)
				{
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
				if(_bands != null)
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
				if(value != _integral)
				{
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
				if(value != _key)
				{
					if(_bands != null & value != "")
					{
						if(_bands[value] != null)
						{
							if (_throwExceptions) throw(new ArgumentException("The key specified is not unique.", "Key"));
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
				if(Created)
				{
					if(OSFeature.Feature.GetVersionPresent(OSFeature.Themes) != null)
					{
						MARGINS margins = new MARGINS();
						User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_GETBANDMARGINS, 0,ref margins);
						return margins;
					}
					return new MARGINS(0,0,0,0);
				}
				else
				{
					return new MARGINS(0,0,0,0);
				}
			}
		}

		public void Maximize()
		{
			if(Created)
			{
				User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_MAXIMIZEBAND, (uint)BandIndex, (uint)_idealWidth);
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
				if(value != _maxHeight)
				{
					//Set Band Height
					_maxHeight = value;
					UpdateMinimums();
				}
			}
		}
		
		public void Minimize()
		{
			if(Created)
			{
				User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_MINIMIZEBAND, (uint)BandIndex, (uint)_idealWidth);
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
				if(value != _minHeight)
				{
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
				if(value != _minWidth)
				{
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
				if(value != _newRow)
				{
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
			if(MouseDown != null)
			{
				MouseDown(this, e);
			}
		}

		internal void OnMouseMove(MouseEventArgs e)
		{
			if(MouseMove != null)
			{
				MouseMove(this, e);
			}
		}

		internal void OnMouseUp(MouseEventArgs e)
		{
			if(MouseUp != null)
			{
				MouseUp(this, e);
			}
		}

		internal void OnMouseWheel(MouseEventArgs e)
		{
			if(MouseWheel != null)
			{
				MouseWheel(this, e);
			}
		}

		internal void OnMove(EventArgs e)
		{
			if(Move != null)
			{
				Move(this, e);
			}
		}

		internal void OnResize(EventArgs e)
		{
			if(Resize != null)
			{
				Resize(this, e);
			}
		}

		internal void OnVisibleChanged(EventArgs e)
		{
			if(VisibleChanged != null)
			{
				VisibleChanged(this, e);
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public int Position
		{
			get
			{
				if(Created)
				{
					return BandIndex;
				}
				else if(_bands != null)
				{
					return Index;
				}
				else
				{
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
				if(value != _showCaption)
				{
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
				if(value != _showIcon)
				{
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

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		protected int Style
		{
			get
			{
				int style = 0;
				if(!_allowVertical)
					style |= (int)RebarBandStyleConstants.RBBS_NOVERT;
				if(_embossPicture)
					style |= (int)RebarBandStyleConstants.RBBS_CHILDEDGE;
				if(_fixedBackground)
					style |= (int)RebarBandStyleConstants.RBBS_FIXEDBMP;
				if(_fixedSize)
					style |= (int)RebarBandStyleConstants.RBBS_FIXEDSIZE;
				if(_newRow)
					style |= (int)RebarBandStyleConstants.RBBS_BREAK;
				if(!_showCaption)
					style |= (int)RebarBandStyleConstants.RBBS_HIDETITLE;
				if(!_visible)
					style |= (int)RebarBandStyleConstants.RBBS_HIDDEN;
				if(_gripSettings == GripperSettings.Always)
					style |= (int)RebarBandStyleConstants.RBBS_GRIPPERALWAYS;
				else if(_gripSettings == GripperSettings.Never)
					style |= (int)RebarBandStyleConstants.RBBS_NOGRIPPER;
				return style;
			}
			set
			{
				_allowVertical = !((value & (int)RebarBandStyleConstants.RBBS_NOVERT) 
					== (int)RebarBandStyleConstants.RBBS_NOVERT);
				_embossPicture = (value & (int)RebarBandStyleConstants.RBBS_CHILDEDGE)
					== (int)RebarBandStyleConstants.RBBS_CHILDEDGE;
				_fixedBackground = (value & (int)RebarBandStyleConstants.RBBS_FIXEDBMP)
					== (int)RebarBandStyleConstants.RBBS_FIXEDBMP;
				_fixedSize = (value & (int)RebarBandStyleConstants.RBBS_FIXEDSIZE)
					== (int)RebarBandStyleConstants.RBBS_FIXEDSIZE;
				_newRow = (value & (int)RebarBandStyleConstants.RBBS_BREAK)
					== (int)RebarBandStyleConstants.RBBS_BREAK;
				_showCaption = !((value & (int)RebarBandStyleConstants.RBBS_HIDETITLE)
					== (int)RebarBandStyleConstants.RBBS_HIDETITLE);
				_visible = !((value & (int)RebarBandStyleConstants.RBBS_HIDDEN)
					== (int)RebarBandStyleConstants.RBBS_HIDDEN);
				if((value & (int)RebarBandStyleConstants.RBBS_GRIPPERALWAYS)
					== (int)RebarBandStyleConstants.RBBS_GRIPPERALWAYS)
					_gripSettings = GripperSettings.Always;
				else if ((value & (int)RebarBandStyleConstants.RBBS_NOGRIPPER)
					== (int)RebarBandStyleConstants.RBBS_NOGRIPPER)
					_gripSettings = GripperSettings.Never;
				else
					_gripSettings = GripperSettings.Auto;
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
			if(Created)
			{
				REBARBANDINFO rbBand = new REBARBANDINFO();
				rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
				rbBand.fMask = (uint)RebarBandInfoConstants.RBBIM_TEXT;
				rbBand.lpText = _caption;
				if(User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_SETBANDINFOA, BandIndex ,ref rbBand) == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						Console.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Updating Caption.", ex));
					}
						
				}
			}
		}

		protected void UpdateChild()
		{
			if(Created)
			{
				REBARBANDINFO rbBand = new REBARBANDINFO();
				rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
				rbBand.fMask = (uint)  RebarBandInfoConstants.RBBIM_CHILD;
				if(_child == null)
				{
					rbBand.hwndChild = IntPtr.Zero;
				}
				else
				{
					rbBand.hwndChild = _child.Handle;
				}
				
				if(User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_SETBANDINFOA, BandIndex ,ref rbBand) == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						Console.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Updating Child.", ex));
					}
						
				}
				UpdateMinimums();
			}
		}

		protected void UpdateColors()
		{
			if(Created)
			{
				REBARBANDINFO rbBand = new REBARBANDINFO();
				rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
				rbBand.fMask = (uint) RebarBandInfoConstants.RBBIM_COLORS;
				if(_useCoolbarColors)
				{
					rbBand.clrBack = new COLORREF();
					rbBand.clrBack._ColorDWORD = (uint) ColorConstants.CLR_DEFAULT;
					rbBand.clrFore = new COLORREF();
					rbBand.clrFore._ColorDWORD = (uint) ColorConstants.CLR_DEFAULT;
				}
				else
				{
					rbBand.clrBack = new COLORREF(_backColor);
					rbBand.clrFore = new COLORREF(_foreColor);
				}
				
				if(User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_SETBANDINFOA, BandIndex ,ref rbBand) == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						Console.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Updating Foreground and Background Colors.", ex));
					}
						
				}
			}
		}

		protected void UpdateIcon()
		{
			if(Created)
			{
				REBARBANDINFO rbBand = new REBARBANDINFO();
				rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
				rbBand.fMask = (uint)  RebarBandInfoConstants.RBBIM_IMAGE;
				if(_showIcon)
				{
					rbBand.iImage = _image;
				}
				else
				{
					rbBand.iImage = -1;
				}
				
				if(User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_SETBANDINFOA, BandIndex ,ref rbBand) == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						Console.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Updating Icon.", ex));
					}						
				}
			}
		}

		protected void UpdateMinimums()
		{
			//return;
			if(Created)
			{
				REBARBANDINFO rbBand = new REBARBANDINFO();
				rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
				rbBand.fMask = (uint)(RebarBandInfoConstants.RBBIM_CHILDSIZE);
				if (_header != -1) rbBand.fMask |= (uint)RebarBandInfoConstants.RBBIM_HEADERSIZE;
				rbBand.cxMinChild = (uint)_minWidth;
				rbBand.cyMinChild = (uint)_minHeight;
				rbBand.cyIntegral = (uint)_integral;//1;
				rbBand.cyChild = (uint)_minHeight;
				rbBand.cyMaxChild = 300;
				rbBand.cxIdeal = (uint)_idealWidth;
				rbBand.cxHeader = (uint)_header;
				if(User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_SETBANDINFOA, BandIndex ,ref rbBand) == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						Console.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Updating Minimums.", ex));
					}
						
				}

			}
		}

		protected void UpdatePicture()
		{
			if(Created)
			{
				REBARBANDINFO rbBand = new REBARBANDINFO();
				rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
				rbBand.fMask = (uint)  RebarBandInfoConstants.RBBIM_BACKGROUND;
				rbBand.hbmBack = _pictureHandle;
				
				if(User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_SETBANDINFOA, BandIndex ,ref rbBand) == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						Console.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Updating Background.", ex));
					}
						
				}
			}
			
		}

		protected void UpdateStyles()
		{
			if(Created)
			{
				REBARBANDINFO rbBand = new REBARBANDINFO();
				rbBand.cbSize = (uint)Marshal.SizeOf(rbBand);
				rbBand.fMask = (uint)  RebarBandInfoConstants.RBBIM_STYLE;
				rbBand.fStyle = (uint)Style; 
				
				if(User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int)WindowsMessages.RB_SETBANDINFOA, BandIndex ,ref rbBand) == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						Console.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Updating Styles.", ex));
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
				if(value != _useCoolbarColors)
				{
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
				if(value != _useCoolbarPicture)
				{
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
				if(value != _visible)
				{
					//Set band style
					_visible = value;
					if(Created)
					{
						User32Dll.SendMessage(_bands.Rebar.RebarHwnd, (int) WindowsMessages.RB_SHOWBAND, (uint)BandIndex, (_visible)?1U:0U);
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
