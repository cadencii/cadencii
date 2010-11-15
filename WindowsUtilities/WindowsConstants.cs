using System;

namespace WindowsUtilities
{

	public enum ColorConstants: uint 
	{
		CLR_NONE = 0xFFFFFFFFU,
		CLR_DEFAULT = 0xFF000000U
	}

	public enum InitWindowsCommonControlsConstants: int
	{
		ICC_LISTVIEW_CLASSES = 0x00000001, // listview, header
		ICC_TREEVIEW_CLASSES = 0x00000002, // treeview, tooltips
		ICC_BAR_CLASSES = 0x00000004, // toolbar, statusbar, trackbar, tooltips
		ICC_TAB_CLASSES = 0x00000008, // tab, tooltips
		ICC_UPDOWN_CLASS = 0x00000010, // updown
		ICC_PROGRESS_CLASS = 0x00000020, // progress
		ICC_HOTKEY_CLASS = 0x00000040, // hotkey
		ICC_ANIMATE_CLASS = 0x00000080, // animate
		ICC_WIN95_CLASSES = 0x000000FF,
		ICC_DATE_CLASSES = 0x00000100, // month picker, date picker, time picker, updown
		ICC_USEREX_CLASSES = 0x00000200, // comboex
		ICC_COOL_CLASSES = 0x00000400, // rebar (coolbar) control
		ICC_INTERNET_CLASSES = 0x00000800,
		ICC_PAGESCROLLER_CLASS = 0x00001000, // page scroller
		ICC_NATIVEFNTCTL_CLASS = 0x00002000, // native font control
		ICC_STANDARD_CLASSES = 0x00004000,
		ICC_LINK_CLASS = 0x00008000,
	}

	[Flags] public enum RebarBandStyleConstants: uint
	{
		RBBS_BREAK = 0x00000001, // break to new line
		RBBS_FIXEDSIZE = 0x00000002, // band can't be sized
		RBBS_CHILDEDGE = 0x00000004, // edge around top & bottom of child window
		RBBS_HIDDEN = 0x00000008, // don't show
		RBBS_NOVERT = 0x00000010, // don't show when vertical
		RBBS_FIXEDBMP = 0x00000020, // bitmap doesn't move during band resize
		RBBS_VARIABLEHEIGHT = 0x00000040, // allow autosizing of this child vertically
		RBBS_GRIPPERALWAYS = 0x00000080, // always show the gripper
		RBBS_NOGRIPPER = 0x00000100, // never show the gripper
		RBBS_USECHEVRON = 0x00000200, // display drop-down button for this band if it's sized smaller than ideal width
		RBBS_HIDETITLE = 0x00000400, // keep band title hidden
		RBBS_TOPALIGN = 0x00000800 // keep band title hidden
	}

	[Flags]public enum RebarBandInfoConstants: uint
	{
		RBBIM_STYLE        = 0x00000001,
		RBBIM_COLORS       = 0x00000002,
		RBBIM_TEXT         = 0x00000004,
		RBBIM_IMAGE        = 0x00000008,
		RBBIM_CHILD        = 0x00000010,
		RBBIM_CHILDSIZE    = 0x00000020,
		RBBIM_SIZE         = 0x00000040,
		RBBIM_BACKGROUND   = 0x00000080,
		RBBIM_ID           = 0x00000100,
		RBBIM_IDEALSIZE    = 0x00000200,
		RBBIM_LPARAM       = 0x00000400,
		RBBIM_HEADERSIZE   = 0x00000800  // control the size of the header
	}

	[Flags]public enum RebarImageListConstants: uint
	{
		RBIM_IMAGELIST = 0x00000001
	}

	[Flags]public enum RebarSizeToRectConstants: uint
	{
		RBSTR_CHANGERECT = 0x0001   // flags for RB_SIZETORECT
	}

	[Flags]
	public enum RedrawWindowConstants: uint
	{
		RDW_INVALIDATE         = 0x0001,
		RDW_INTERNALPAINT      = 0x0002,
		RDW_ERASE              = 0x0004,
		RDW_VALIDATE           = 0x0008,
		RDW_NOINTERNALPAINT    = 0x0010,
		RDW_NOERASE            = 0x0020,
		RDW_NOCHILDREN         = 0x0040,
		RDW_ALLCHILDREN        = 0x0080,
		RDW_UPDATENOW          = 0x0100,
		RDW_ERASENOW           = 0x0200,
		RDW_FRAME              = 0x0400,
		RDW_NOFRAME            = 0x0800
	}

	[Flags]
	public enum SetWindowPosConstants: uint
	{
		SWP_NOSIZE         = 0x0001U,
		SWP_NOMOVE         = 0x0002U,
		SWP_NOZORDER       = 0x0004U,
		SWP_NOREDRAW       = 0x0008U,
		SWP_NOACTIVATE     = 0x0010U,
		SWP_FRAMECHANGED   = 0x0020U,  /* The frame changed: send WM_NCCALCSIZE */
		SWP_SHOWWINDOW     = 0x0040U,
		SWP_HIDEWINDOW     = 0x0080U,
		SWP_NOCOPYBITS     = 0x0100U,
		SWP_NOOWNERZORDER  = 0x0200U,  /* Don't do owner Z ordering */
		SWP_NOSENDCHANGING = 0x0400U,  /* Don't send WM_WINDOWPOSCHANGING */
		SWP_DRAWFRAME      = SWP_FRAMECHANGED,
		SWP_NOREPOSITION   = SWP_NOOWNERZORDER,
		SWP_DEFERERASE     = 0x2000U,
		SWP_ASYNCWINDOWPOS = 0x4000U,
		SWP_REDRAWONLY     = (SWP_NOSIZE | SWP_NOMOVE | 
			SWP_NOZORDER | SWP_NOACTIVATE | SWP_NOCOPYBITS | 
			SWP_NOOWNERZORDER | SWP_NOSENDCHANGING)
	}

	public enum WindowsHitTestConstants: int
	{
		HTERROR = (-2),
		HTTRANSPARENT = (-1),
		HTNOWHERE = 0,
		HTCLIENT = 1,
		HTCAPTION = 2,
		HTSYSMENU = 3,
		HTGROWBOX = 4,
		HTSIZE = HTGROWBOX,
		HTMENU = 5,
		HTHSCROLL = 6,
		HTVSCROLL = 7,
		HTMINBUTTON = 8,
		HTMAXBUTTON = 9,
		HTLEFT = 10,
		HTRIGHT = 11,
		HTTOP = 12,
		HTTOPLEFT = 13,
		HTTOPRIGHT = 14,
		HTBOTTOM = 15,
		HTBOTTOMLEFT = 16,
		HTBOTTOMRIGHT = 17,
		HTBORDER = 18,
		HTREDUCE = HTMINBUTTON,
		HTZOOM = HTMAXBUTTON,
		HTSIZEFIRST = HTLEFT,
		HTSIZELAST = HTBOTTOMRIGHT,
		HTOBJECT = 19,
		HTCLOSE = 20,
		HTHELP = 21,
	}

	public enum WindowLongConstants: int
	{
		GWL_WNDPROC        = (-4),
		GWL_HINSTANCE      = (-6),
		GWL_HWNDPARENT     = (-8),
		GWL_STYLE          = (-16),
		GWL_EXSTYLE        = (-20),
		GWL_USERDATA       = (-21),
		GWL_ID             = (-12),
		GWLP_WNDPROC       = (-4),
		GWLP_HINSTANCE     = (-6),
		GWLP_HWNDPARENT    = (-8),
		GWLP_USERDATA      = (-21),
		GWLP_ID            = (-12),
	}

	public enum WindowsNotifyConstants: int
	{
		NM_FIRST              = (0-  0),       // generic to all controls
		NM_LAST               = (0- 99),
		LVN_FIRST             = (0-100),       // listview
		LVN_LAST              = (0-199),
		HDN_FIRST             = (0-300),       // header
		HDN_LAST              = (0-399),
		TVN_FIRST             = (0-400),       // treeview
		TVN_LAST              = (0-499),
		TTN_FIRST             = (0-520),       // tooltips
		TTN_LAST              = (0-549),
		TCN_FIRST             = (0-550),       // tab control
		TCN_LAST              = (0-580),
		CDN_FIRST             = (0-601),       // common dialog (new)
		CDN_LAST              = (0-699),
		TBN_FIRST             = (0-700),       // toolbar
		TBN_LAST              = (0-720),
		UDN_FIRST             = (0-721),        // updown
		UDN_LAST              = (0-740),
		MCN_FIRST             = (0-750),       // monthcal
		MCN_LAST              = (0-759),
		DTN_FIRST             = (0-760),       // datetimepick
		DTN_LAST              = (0-799),
		CBEN_FIRST            = (0-800),       // combo box ex
		CBEN_LAST             = (0-830),
		RBN_FIRST             = (0-831),       // rebar
		RBN_LAST              = (0-859),
		IPN_FIRST             = (0-860),       // internet address
		IPN_LAST              = (0-879),       // internet address
		SBN_FIRST             = (0-880),       // status bar
		SBN_LAST              = (0-899),
		PGN_FIRST             = (0-900),       // Pager Control
		PGN_LAST              = (0-950),
		WMN_FIRST             = (0-1000),
		WMN_LAST              = (0-1200),
		BCN_FIRST             = (0-1250),
		BCN_LAST              = (0-1350),
		NM_OUTOFMEMORY         = (NM_FIRST-1),
		NM_CLICK               = (NM_FIRST-2),    // uses NMCLICK struct
		NM_DBLCLK              = (NM_FIRST-3),
		NM_RETURN              = (NM_FIRST-4),
		NM_RCLICK              = (NM_FIRST-5),    // uses NMCLICK struct
		NM_RDBLCLK             = (NM_FIRST-6),
		NM_SETFOCUS            = (NM_FIRST-7),
		NM_KILLFOCUS           = (NM_FIRST-8),
		NM_CUSTOMDRAW          = (NM_FIRST-12),
		NM_HOVER               = (NM_FIRST-13),
		NM_NCHITTEST           = (NM_FIRST-14),   // uses NMMOUSE struct
		NM_KEYDOWN             = (NM_FIRST-15),   // uses NMKEY struct
		NM_RELEASEDCAPTURE     = (NM_FIRST-16),
		NM_SETCURSOR           = (NM_FIRST-17),   // uses NMMOUSE struct
		NM_CHAR                = (NM_FIRST-18),   // uses NMCHAR struct
		NM_TOOLTIPSCREATED     = (NM_FIRST-19),   // notify of when the tooltips window is create
		NM_LDOWN               = (NM_FIRST-20),
		NM_RDOWN               = (NM_FIRST-21),
		NM_THEMECHANGED        = (NM_FIRST-22),
		RBN_HEIGHTCHANGE   = (RBN_FIRST - 0),
		RBN_GETOBJECT      = (RBN_FIRST - 1),
		RBN_LAYOUTCHANGED  = (RBN_FIRST - 2),
		RBN_AUTOSIZE       = (RBN_FIRST - 3),
		RBN_BEGINDRAG      = (RBN_FIRST - 4),
		RBN_ENDDRAG         = (RBN_FIRST - 5),
		RBN_DELETINGBAND   = (RBN_FIRST - 6),     // Uses NMREBAR
		RBN_DELETEDBAND    = (RBN_FIRST - 7),     // Uses NMREBAR
		RBN_CHILDSIZE      = (RBN_FIRST - 8),
		RBN_CHEVRONPUSHED  = (RBN_FIRST - 10),
		RBN_MINMAX         = (RBN_FIRST - 21),
		RBN_AUTOBREAK      = (RBN_FIRST - 22),
	}
	/*
		public enum blah: int
		{
			ODT_HEADER = 100,
			ODT_TAB = 101,
			ODT_LISTVIEW = 102,
		}
	*/

	[Flags]public enum WindowsStyleConstants: uint
	{	
		CCS_TOP                = 0x00000001U,
		CCS_NOMOVEY            = 0x00000002U,
		CCS_BOTTOM             = 0x00000003U,
		CCS_NORESIZE           = 0x00000004U,
		CCS_NOPARENTALIGN      = 0x00000008U,
		CCS_ADJUSTABLE         = 0x00000020U,
		CCS_NODIVIDER          = 0x00000040U,
		CCS_VERT               = 0x00000080U,
		CCS_LEFT               = (CCS_VERT | CCS_TOP),
		CCS_RIGHT              = (CCS_VERT | CCS_BOTTOM),
		CCS_NOMOVEX            = (CCS_VERT | CCS_NOMOVEY),
		RBS_TOOLTIPS = 0x0100,
		RBS_VARHEIGHT = 0x0200,
		RBS_BANDBORDERS = 0x0400,
		RBS_FIXEDORDER = 0x0800,
		RBS_REGISTERDROP = 0x1000,
		RBS_AUTOSIZE = 0x2000,
		RBS_VERTICALGRIPPER = 0x4000, // this always has the vertical gripper (default for horizontal mode)
		RBS_DBLCLKTOGGLE = 0x8000,
		WS_OVERLAPPED      = 0x00000000U,
		WS_POPUP           = 0x80000000U,
		WS_CHILD           = 0x40000000U,
		WS_MINIMIZE        = 0x20000000U,
		WS_VISIBLE         = 0x10000000U,
		WS_DISABLED        = 0x08000000U,
		WS_CLIPSIBLINGS    = 0x04000000U,
		WS_CLIPCHILDREN    = 0x02000000U,
		WS_MAXIMIZE        = 0x01000000U,
		WS_CAPTION         = 0x00C00000U,    /* WS_BORDER | WS_DLGFRAME  */
		WS_BORDER          = 0x00800000U,
		WS_DLGFRAME        = 0x00400000U,
		WS_VSCROLL         = 0x00200000U,
		WS_HSCROLL         = 0x00100000U,
		WS_SYSMENU         = 0x00080000U,
		WS_THICKFRAME      = 0x00040000U,
		WS_GROUP           = 0x00020000U,
		WS_TABSTOP         = 0x00010000U,
		WS_MINIMIZEBOX     = 0x00020000U,
		WS_MAXIMIZEBOX     = 0x00010000U,
		WS_TILED           = WS_OVERLAPPED,
		WS_ICONIC          = WS_MINIMIZE,
		WS_SIZEBOX         = WS_THICKFRAME,
		WS_TILEDWINDOW     = WS_OVERLAPPEDWINDOW,
		WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED     | 
			WS_CAPTION        | 
			WS_SYSMENU        | 
			WS_THICKFRAME     | 
			WS_MINIMIZEBOX    | 
			WS_MAXIMIZEBOX),
		WS_POPUPWINDOW     = (WS_POPUP          | 
			WS_BORDER         | 
			WS_SYSMENU),
		WS_CHILDWINDOW     = (WS_CHILD)
	}

	public enum WindowZOrderConstants: int
	{
		HWND_TOP       = 0,
		HWND_BOTTOM    = 1,
		HWND_TOPMOST   = -1,
		HWND_NOTOPMOST = -2
	}
}