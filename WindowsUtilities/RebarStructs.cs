using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WindowsUtilities
{
	[StructLayout(LayoutKind.Sequential)]
	public struct COLORREF
	{
		public uint _ColorDWORD;

		//Those goofs at MS store a COLORREF as XXBBGGRR and a Color as AARRGGBB
		//I'm sure that there's a good reason for it but it sure is a bummer
		//Lets do some bit shifting
		public COLORREF(Color color)
		{
			_ColorDWORD = ((uint)color.R) + 
				(uint)(color.G << 8) +
				(uint)(color.B << 16);
		}

		public Color GetColor()
		{
			return Color.FromArgb((int)(0x000000FFU | _ColorDWORD),
				(int)((0x0000FF00 | _ColorDWORD) >> 2),
				(int)((0x00FF0000 | _ColorDWORD) >> 4));
		}

		public void SetColor(Color color)
		{
			_ColorDWORD = ((uint)color.R) + 
				(uint)(color.G << 8) +
				(uint)(color.B << 16);
		}

		public override string ToString()
		{
			return _ColorDWORD.ToString();
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct COLORSCHEME
	{
		public uint dwSize;
		public COLORREF clrBtnHighlight;
		public COLORREF clrBtnShadow;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct INITCOMMONCONTROLSEX 
	{
		public uint dwSize;             // size of this structure
		public uint dwICC;              // flags indicating which classes to be initialized
	} 

	[StructLayout(LayoutKind.Sequential)]
	public struct MARGINS 
	{
		public MARGINS(int Left, int Right, int Top, int Bottom)
		{
			cxLeftWidth = Left;
			cxRightWidth = Right;
			cyTopHeight = Top;
			cyBottomHeight = Bottom;
		}

		public int  cxLeftWidth;
		public int  cxRightWidth;
		public int  cyTopHeight;
		public int  cyBottomHeight;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct NMHDR 
	{ 
		public IntPtr hwndFrom; 
		public uint idFrom; 
		public int code; 
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct NMMOUSE 
	{
		public NMHDR     hdr;
		public IntPtr dwItemSpec;
		public IntPtr dwItemData;
		public POINT     pt;
		public IntPtr    dwHitInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct NMREBARCHILDSIZE
	{
		public NMHDR hdr;
		public uint uBand;
		public uint wID;
		public RECT rcChild;
		public RECT rcBand;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT 
	{ 
		public int x; 
		public int y; 
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct REBARBANDINFO
	{
		public void REBARABANDINFO()
		{
			cbSize = (uint)Marshal.SizeOf(this);
			fMask = 0U;
			fStyle = 0U;
			clrFore = new COLORREF(SystemColors.ControlText);
			clrBack = new COLORREF(SystemColors.Control);
			lpText = "";
			cch = 0U;
			iImage = 0;
			hwndChild = IntPtr.Zero;
			cxMinChild = 0U;
			cyMinChild = 0U;
			cx = 0U;
			hbmBack = IntPtr.Zero;
			wID = 0U;
			cyChild = 0U; //Initial Child Height
			cyMaxChild = 0U;
			cyIntegral = 0U;
			cxIdeal = 0U;
			lParam = IntPtr.Zero;
			cxHeader = 0U;
		}

		public uint cbSize;
		public uint fMask;
		public uint fStyle;
		public COLORREF clrFore;
		public COLORREF clrBack;
		public string lpText;
		public uint cch; //Size of text buffer
		public int iImage;
		public IntPtr hwndChild;
		public uint cxMinChild;
		public uint cyMinChild;
		public uint cx;
		public IntPtr hbmBack;
		public uint wID;
		public uint cyChild;
		public uint cyMaxChild;
		public uint cyIntegral;
		public uint cxIdeal;
		public IntPtr lParam;
		public uint cxHeader;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct REBARINFO
	{
		public uint cbSize;
		public uint fMask;
		public IntPtr himl;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct TRACKMOUSEEVENT 
	{
		public int cbSize;
		public uint dwFlags;
		public IntPtr  hwndTrack;
		public int dwHoverTime;
	}

}
