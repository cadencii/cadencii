using System;
using System.Runtime.InteropServices;

namespace WindowsUtilities
{
	/// <summary>
	/// Summary description for Kernel32Dll.
	/// </summary>
	public class Kernel32Dll
	{
		public Kernel32Dll()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[DllImport("Kernel32.Dll")]
		public static extern uint FormatMessage(
			uint dwFlags,      // source and processing options
			IntPtr lpSource,   // message source
			uint dwMessageId,  // message identifier
			uint dwLanguageId, // language identifier
			string lpBuffer,    // message buffer
			uint nSize,        // maximum size of message buffer
			IntPtr Arguments  // array of message inserts
			);


		[DllImport("Kernel32.Dll")]
		public static extern uint GetLastError();


	}
}
