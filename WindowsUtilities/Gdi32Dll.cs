using System;
using System.Runtime.InteropServices;

namespace WindowsUtilities
{
	/// <summary>
	/// Summary description for Gdi32Dll.
	/// </summary>
	public class Gdi32Dll
	{
		public Gdi32Dll()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		[DllImport("GDI32.dll")]
		public static extern bool DeleteObject(
			IntPtr hObject   // handle to graphic object
			);
	}
}
