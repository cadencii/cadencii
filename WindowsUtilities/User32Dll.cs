using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsUtilities
{
	/// <summary>
	/// Summary description for User32Dll.
	/// </summary>
	public class User32Dll
	{
		public User32Dll()
		{

		}
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr CreateWindowEx(
			uint dwExStyle,      // extended window style
			string lpClassName,  // registered class name
			string lpWindowName, // window name
			uint dwStyle,        // window style
			int x,                // horizontal position of window
			int y,                // vertical position of window
			int nWidth,           // window width
			int nHeight,          // window height
			IntPtr hWndParent,      // handle to parent or owner window
			IntPtr hMenu,          // menu handle or child identifier
			IntPtr hInstance,  // handle to application instance
			int lpParam        // window-creation data
			);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool DestroyWindow(IntPtr hWnd);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool DispatchMessage(ref Message msg);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool GetMessage(ref Message msg, int hWnd, uint wFilterMin, uint wFilterMax);

		//[DllImport("User32.dll", CharSet=CharSet.Auto)] 
		[DllImport("User32.dll", CharSet=CharSet.Auto,
			 EntryPoint = "GetWindowLong", ExactSpelling=false)] //My Computer can't seem to find the entry point for GetWindowLongPtr
		public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool GetWindowRect(
			IntPtr hWnd,      // handle to window
			ref RECT lpRect   // window coordinates
			);
		
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern int MessageBox(int h, string m, string c, int type);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern int MoveWindow(
			IntPtr hWnd,      // handle to window
			int X,          // horizontal position
			int Y,          // vertical position
			int nWidth,     // width
			int nHeight,    // height
			bool bRepaint   // repaint option
			);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool PeekMessage(ref Message msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool RedrawWindow(IntPtr hWnd, ref RECT lprcUpdate,IntPtr hrgnUpdate, uint flags);[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
		
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
		
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref REBARINFO lParam);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref REBARBANDINFO lParam);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref COLORSCHEME lParam);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, COLORREF lParam);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref RECT lParam);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref MARGINS lParam);
		
		//[DllImport("User32.dll", CharSet=CharSet.Auto)]
		[DllImport("User32.dll", CharSet=CharSet.Auto, 
			 EntryPoint = "SetWindowLong", ExactSpelling=false)] //My Computer can't seem to find the entry point for SetWindowLongPtr
		public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool TranslateMessage(ref Message msg);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool WaitMessage();


	}
}
