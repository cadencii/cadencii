/*
 * windows.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace cadencii
{
    using WORD = System.UInt16;
    using DWORD = System.UInt32;
    using LONG = System.Int32;
    using BYTE = System.Byte;
    using HANDLE = System.IntPtr;

    public static partial class win32
    {
        #region winbase.h
        public static readonly HANDLE INVALID_HANDLE_VALUE = new HANDLE(-1);

        public const DWORD CONSOLE_TEXTMODE_BUFFER = 1;
        public const DWORD CREATE_NEW = 1;
        public const DWORD CREATE_ALWAYS = 2;
        public const DWORD OPEN_EXISTING = 3;
        public const DWORD OPEN_ALWAYS = 4;
        public const DWORD TRUNCATE_EXISTING = 5;

        public const DWORD FILE_FLAG_WRITE_THROUGH = 0x80000000;
        public const DWORD FILE_FLAG_OVERLAPPED = 1073741824;
        public const DWORD FILE_FLAG_NO_BUFFERING = 536870912;
        public const DWORD FILE_FLAG_RANDOM_ACCESS = 268435456;
        public const DWORD FILE_FLAG_SEQUENTIAL_SCAN = 134217728;
        public const DWORD FILE_FLAG_DELETE_ON_CLOSE = 67108864;
        public const DWORD FILE_FLAG_BACKUP_SEMANTICS = 33554432;
        public const DWORD FILE_FLAG_POSIX_SEMANTICS = 16777216;
        public const DWORD FILE_FLAG_OPEN_REPARSE_POINT = 2097152;
        public const DWORD FILE_FLAG_OPEN_NO_RECALL = 1048576;
        public const DWORD FILE_FLAG_FIRST_PIPE_INSTANCE = 524288;

        public const int OF_READ = 0;
        public const int OF_READWRITE = 2;
        public const int OF_WRITE = 1;
        public const int OF_SHARE_COMPAT = 0;
        public const int OF_SHARE_DENY_NONE = 64;
        public const int OF_SHARE_DENY_READ = 48;
        public const int OF_SHARE_DENY_WRITE = 32;
        public const int OF_SHARE_EXCLUSIVE = 16;
        public const int OF_CREATE = 4096;

        public const int DONT_RESOLVE_DLL_REFERENCES = 0x00000001;
        public const int LOAD_LIBRARY_AS_DATAFILE = 0x00000002;
        public const int LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008;
        public const int LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010;
        public const int LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020;
        public const int LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040;

        #endregion

        #region winerror.h
        public const uint ERROR_SUCCESS = 0;
        #endregion

        #region winreg.h
        public const uint HKEY_CLASSES_ROOT = 0x80000000;
        public const uint HKEY_CURRENT_USER = 0x80000001;
        public const uint HKEY_LOCAL_MACHINE = 0x80000002;
        public const uint HKEY_USERS = 0x80000003;
        public const uint HKEY_PERFORMANCE_DATA = 0x80000004;
        public const uint HKEY_CURRENT_CONFIG = 0x80000005;
        public const uint HKEY_DYN_DATA = 0x80000006;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hKey">キーのハンドル</param>
        /// <param name="pSubKey">オープンするサブキーの名前</param>
        /// <param name="ulOptions">予約（0を指定）</param>
        /// <param name="samDesired">セキュリティアクセスマスク</param>
        /// <param name="phkResult">ハンドルを格納する変数のアドレス</param>
        /// <returns></returns>
        [DllImport("Advapi32.dll")]
        public static unsafe extern int RegOpenKeyExW(
            uint hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string pSubKey,
            uint ulOptions,
            uint samDesired,
            uint* phkResult);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hKey">キーのハンドル</param>
        /// <param name="dwIndex">サブキーのインデックス</param>
        /// <param name="pName">サブキー名を格納するバッファ</param>
        /// <param name="pcbName">pName のサイズを入れた変数</param>
        /// <param name="pReserved">予約（NULLを指定）</param>
        /// <param name="pClass">クラス名を格納するバッファ</param>
        /// <param name="pcbClass">pClass のサイズを入れた変数</param>
        /// <param name="pftLastWrite">最終書き込み時間</param>
        /// <returns></returns>
        [DllImport("Advapi32.dll")]
        public static unsafe extern int RegEnumKeyExW(
            uint hKey,
            uint dwIndex,
            [MarshalAs(UnmanagedType.LPWStr)] string pName,
            uint* pcbName,
            uint* pReserved,
            [MarshalAs(UnmanagedType.LPWStr)] string pClass,
            uint* pcbClass,
            FILETIME* pftLastWrite);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hKey">キーのハンドル</param>
        /// <param name="pValueName">値の名前</param>
        /// <param name="pReserved">予約（NULLを指定）</param>
        /// <param name="pType">データタイプを格納する変数</param>
        /// <param name="pData">データを格納するバッファ</param>
        /// <param name="pcbData">バッファサイズを入れた変数</param>
        /// <returns></returns>
        [DllImport("Advapi32.dll")]
        public static unsafe extern int RegQueryValueExW(
            uint hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string pValueName,
            uint* pReserved,
            uint* pType,
            byte* pData,
            uint* pcbData);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hKey">キーのハンドル</param>
        /// <returns></returns>
        [DllImport("Advapi32.dll")]
        public static extern int RegCloseKey(uint hKey);
        #endregion

        #region winnt.h
        public const DWORD MAXIMUM_REPARSE_DATA_BUFFER_SIZE = 16384;
        public const DWORD IO_REPARSE_TAG_RESERVED_ZERO = 0;
        public const DWORD IO_REPARSE_TAG_RESERVED_ONE = 1;
        public const DWORD IO_REPARSE_TAG_RESERVED_RANGE = IO_REPARSE_TAG_RESERVED_ONE;
        public const DWORD IO_REPARSE_TAG_VALID_VALUES = 0xE000FFFF;
        public const DWORD IO_REPARSE_TAG_SYMBOLIC_LINK = IO_REPARSE_TAG_RESERVED_ZERO;
        public const DWORD IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003;
        public const DWORD IO_REPARSE_TAG_SYMLINK = 0xA000000C;

        public const uint KEY_QUERY_VALUE = 1;
        public const uint KEY_SET_VALUE = 2;
        public const uint KEY_CREATE_SUB_KEY = 4;
        public const uint KEY_ENUMERATE_SUB_KEYS = 8;
        public const uint KEY_NOTIFY = 16;
        public const uint KEY_CREATE_LINK = 32;
        public const uint KEY_WRITE = 0x20006;
        public const uint KEY_EXECUTE = 0x20019;
        public const uint KEY_READ = 0x20019;
        public const uint KEY_ALL_ACCESS = 0xf003f;

        public const int REG_NONE = 0;
        public const int REG_SZ = 1;
        public const int REG_EXPAND_SZ = 2;
        public const int REG_BINARY = 3;
        public const int REG_DWORD = 4;
        public const int REG_DWORD_LITTLE_ENDIAN = 4;
        public const int REG_DWORD_BIG_ENDIAN = 5;
        public const int REG_LINK = 6;
        public const int REG_MULTI_SZ = 7;
        public const int REG_RESOURCE_LIST = 8;
        public const int REG_FULL_RESOURCE_DESCRIPTOR = 9;
        public const int REG_RESOURCE_REQUIREMENTS_LIST = 10;
        public const int REG_QWORD = 11;
        public const int REG_QWORD_LITTLE_ENDIAN = 11;

        public const int IMAGE_NUMBEROF_DIRECTORY_ENTRIES = 16;

        public const int IMAGE_DIRECTORY_ENTRY_EXPORT = 0;
        public const int IMAGE_DIRECTORY_ENTRY_IMPORT = 1;
        public const int IMAGE_DIRECTORY_ENTRY_RESOURCE = 2;
        public const int IMAGE_DIRECTORY_ENTRY_EXCEPTION = 3;
        public const int IMAGE_DIRECTORY_ENTRY_SECURITY = 4;
        public const int IMAGE_DIRECTORY_ENTRY_BASERELOC = 5;
        public const int IMAGE_DIRECTORY_ENTRY_DEBUG = 6;
        public const int IMAGE_DIRECTORY_ENTRY_COPYRIGHT = 7;
        public const int IMAGE_DIRECTORY_ENTRY_ARCHITECTURE = 7;
        public const int IMAGE_DIRECTORY_ENTRY_GLOBALPTR = 8;
        public const int IMAGE_DIRECTORY_ENTRY_TLS = 9;
        public const int IMAGE_DIRECTORY_ENTRY_LOAD_CONFIG = 10;
        public const int IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT = 11;
        public const int IMAGE_DIRECTORY_ENTRY_IAT = 12;
        public const int IMAGE_DIRECTORY_ENTRY_DELAY_IMPORT = 13;
        public const int IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR = 14;

        public const int DLL_PROCESS_DETACH = 0;
        public const int DLL_PROCESS_ATTACH = 1;
        public const int DLL_THREAD_ATTACH = 2;
        public const int DLL_THREAD_DETACH = 3;

        public const int IMAGE_FILE_RELOCS_STRIPPED = 1;
        public const int IMAGE_FILE_EXECUTABLE_IMAGE = 2;
        public const int IMAGE_FILE_LINE_NUMS_STRIPPED = 4;
        public const int IMAGE_FILE_LOCAL_SYMS_STRIPPED = 8;
        public const int IMAGE_FILE_AGGRESIVE_WS_TRIM = 16;
        public const int IMAGE_FILE_LARGE_ADDRESS_AWARE = 32;
        public const int IMAGE_FILE_BYTES_REVERSED_LO = 128;
        public const int IMAGE_FILE_32BIT_MACHINE = 256;
        public const int IMAGE_FILE_DEBUG_STRIPPED = 512;
        public const int IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP = 1024;
        public const int IMAGE_FILE_NET_RUN_FROM_SWAP = 2048;
        public const int IMAGE_FILE_SYSTEM = 4096;
        public const int IMAGE_FILE_DLL = 8192;
        public const int IMAGE_FILE_UP_SYSTEM_ONLY = 16384;
        public const int IMAGE_FILE_BYTES_REVERSED_HI = 32768;
        public const int IMAGE_FILE_MACHINE_UNKNOWN = 0x0000;
        public const int IMAGE_FILE_MACHINE_AM33 = 0x01d3;
        public const int IMAGE_FILE_MACHINE_AMD64 = 0x8664;
        public const int IMAGE_FILE_MACHINE_ARM = 0x01c0;
        public const int IMAGE_FILE_MACHINE_EBC = 0x0ebc;
        public const int IMAGE_FILE_MACHINE_I386 = 0x014c;
        public const int IMAGE_FILE_MACHINE_IA64 = 0x0200;
        public const int IMAGE_FILE_MACHINE_M32R = 0x9041;
        public const int IMAGE_FILE_MACHINE_MIPS16 = 0x0266;
        public const int IMAGE_FILE_MACHINE_MIPSFPU = 0x0366;
        public const int IMAGE_FILE_MACHINE_MIPSFPU16 = 0x0466;
        public const int IMAGE_FILE_MACHINE_POWERPC = 0x01f0;
        public const int IMAGE_FILE_MACHINE_POWERPCFP = 0x01f1;
        public const int IMAGE_FILE_MACHINE_R4000 = 0x0166;
        public const int IMAGE_FILE_MACHINE_SH3 = 0x01a2;
        public const int IMAGE_FILE_MACHINE_SH3DSP = 0x01a3;
        public const int IMAGE_FILE_MACHINE_SH4 = 0x01a6;
        public const int IMAGE_FILE_MACHINE_SH5 = 0x01a8;
        public const int IMAGE_FILE_MACHINE_THUMB = 0x01c2;
        public const int IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x0169;

        public const DWORD MAXIMUM_ALLOWED = 0x2000000;
        public const DWORD GENERIC_READ = 0x80000000;
        public const DWORD GENERIC_WRITE = 0x40000000;
        public const DWORD GENERIC_EXECUTE = 0x20000000;
        public const DWORD GENERIC_ALL = 0x10000000;

        public const DWORD INVALID_FILE_ATTRIBUTES = unchecked((DWORD)(-1));

        public const DWORD FILE_ATTRIBUTE_READONLY = 0x00000001;
        public const DWORD FILE_ATTRIBUTE_HIDDEN = 0x00000002;
        public const DWORD FILE_ATTRIBUTE_SYSTEM = 0x00000004;
        public const DWORD FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const DWORD FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
        public const DWORD FILE_ATTRIBUTE_DEVICE = 0x00000040;
        public const DWORD FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const DWORD FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
        public const DWORD FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
        public const DWORD FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
        public const DWORD FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
        public const DWORD FILE_ATTRIBUTE_OFFLINE = 0x00001000;
        public const DWORD FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
        public const DWORD FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
        public const DWORD FILE_ATTRIBUTE_VALID_FLAGS = 0x00007fb7;
        public const DWORD FILE_ATTRIBUTE_VALID_SET_FLAGS = 0x000031a7;

        public const DWORD FILE_SHARE_READ = 0x00000001;
        public const DWORD FILE_SHARE_WRITE = 0x00000002;
        public const DWORD FILE_SHARE_DELETE = 0x00000004;
        public const DWORD FILE_SHARE_VALID_FLAGS = 0x00000007;

        #endregion

        #region windef.h
        public const int MAX_PATH = 260;
        #endregion

        #region winuser.h
        public const int WM_NULL = 0x0000;
        public const int WM_CREATE = 0x0001;
        public const int WM_DESTROY = 0x0002;
        public const int WM_MOVE = 0x0003;
        public const int WM_SIZE = 0x0005;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_SETFOCUS = 0x0007;
        public const int WM_KILLFOCUS = 0x0008;
        public const int WM_ENABLE = 0x000A;
        public const int WM_SETREDRAW = 0x000B;
        public const int WM_SETTEXT = 0x000C;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
        public const int WM_PAINT = 0x000F;
        public const int WM_CLOSE = 0x0010;
        public const int WM_QUERYENDSESSION = 0x0011;
        public const int WM_QUERYOPEN = 0x0013;
        public const int WM_ENDSESSION = 0x0016;
        public const int WM_QUIT = 0x0012;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_SYSCOLORCHANGE = 0x0015;
        public const int WM_SHOWWINDOW = 0x0018;
        public const int WM_WININICHANGE = 0x001A;
        public const int WM_SETTINGCHANGE = 0x001A;
        public const int WM_DEVMODECHANGE = 0x001B;
        public const int WM_ACTIVATEAPP = 0x001C;
        public const int WM_FONTCHANGE = 0x001D;
        public const int WM_TIMECHANGE = 0x001E;
        public const int WM_CANCELMODE = 0x001F;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_MOUSEACTIVATE = 0x0021;
        public const int WM_CHILDACTIVATE = 0x0022;
        public const int WM_QUEUESYNC = 0x0023;
        public const int WM_GETMINMAXINFO = 0x0024;
        public const int WM_PAINTICON = 0x0026;
        public const int WM_ICONERASEBKGND = 0x0027;
        public const int WM_NEXTDLGCTL = 0x0028;
        public const int WM_SPOOLERSTATUS = 0x002A;
        public const int WM_DRAWITEM = 0x002B;
        public const int WM_MEASUREITEM = 0x002C;
        public const int WM_DELETEITEM = 0x002D;
        public const int WM_VKEYTOITEM = 0x002E;
        public const int WM_CHARTOITEM = 0x002F;
        public const int WM_SETFONT = 0x0030;
        public const int WM_GETFONT = 0x0031;
        public const int WM_SETHOTKEY = 0x0032;
        public const int WM_GETHOTKEY = 0x0033;
        public const int WM_QUERYDRAGICON = 0x0037;
        public const int WM_COMPAREITEM = 0x0039;
        public const int WM_GETOBJECT = 0x003D;
        public const int WM_COMPACTING = 0x0041;
        public const int WM_COMMNOTIFY = 0x0044;
        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_WINDOWPOSCHANGED = 0x0047;
        public const int WM_POWER = 0x0048;
        public const int WM_COPYDATA = 0x004A;
        public const int WM_CANCELJOURNAL = 0x004B;
        public const int WM_NOTIFY = 0x004E;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
        public const int WM_INPUTLANGCHANGE = 0x0051;
        public const int WM_TCARD = 0x0052;
        public const int WM_HELP = 0x0053;
        public const int WM_USERCHANGED = 0x0054;
        public const int WM_NOTIFYFORMAT = 0x0055;
        public const int WM_CONTEXTMENU = 0x007B;
        public const int WM_STYLECHANGING = 0x007C;
        public const int WM_STYLECHANGED = 0x007D;
        public const int WM_DISPLAYCHANGE = 0x007E;
        public const int WM_GETICON = 0x007F;
        public const int WM_SETICON = 0x0080;
        public const int WM_NCCREATE = 0x0081;
        public const int WM_NCDESTROY = 0x0082;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_GETDLGCODE = 0x0087;
        public const int WM_SYNCPAINT = 0x0088;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCRBUTTONDOWN = 0x00A4;
        public const int WM_NCRBUTTONUP = 0x00A5;
        public const int WM_NCRBUTTONDBLCLK = 0x00A6;
        public const int WM_NCMBUTTONDOWN = 0x00A7;
        public const int WM_NCMBUTTONUP = 0x00A8;
        public const int WM_NCMBUTTONDBLCLK = 0x00A9;
        public const int WM_NCXBUTTONDOWN = 0x00AB;
        public const int WM_NCXBUTTONUP = 0x00AC;
        public const int WM_NCXBUTTONDBLCLK = 0x00AD;
        public const int WM_INPUT = 0x00FF;
        public const int WM_KEYFIRST = 0x0100;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_DEADCHAR = 0x0103;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_SYSCHAR = 0x0106;
        public const int WM_SYSDEADCHAR = 0x0107;
        public const int WM_UNICHAR = 0x0109;
        public const int WM_KEYLAST_NT501 = 0x0109;
        public const int UNICODE_NOCHAR = 0xFFFF;
        public const int WM_KEYLAST_PRE501 = 0x0108;
        public const int WM_INITDIALOG = 0x0110;
        public const int WM_COMMAND = 0x0111;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_TIMER = 0x0113;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_INITMENU = 0x0116;
        public const int WM_INITMENUPOPUP = 0x0117;
        public const int WM_MENUSELECT = 0x011F;
        public const int WM_MENUCHAR = 0x0120;
        public const int WM_ENTERIDLE = 0x0121;
        public const int WM_MENURBUTTONUP = 0x0122;
        public const int WM_MENUDRAG = 0x0123;
        public const int WM_MENUGETOBJECT = 0x0124;
        public const int WM_UNINITMENUPOPUP = 0x0125;
        public const int WM_MENUCOMMAND = 0x0126;
        public const int WM_CHANGEUISTATE = 0x0127;
        public const int WM_UPDATEUISTATE = 0x0128;
        public const int WM_QUERYUISTATE = 0x0129;
        public const int WM_CTLCOLORMSGBOX = 0x0132;
        public const int WM_CTLCOLOREDIT = 0x0133;
        public const int WM_CTLCOLORLISTBOX = 0x0134;
        public const int CTLCOLORBTN = 0x0135;
        public const int WM_CTLCOLORDLG = 0x0136;
        public const int WM_CTLCOLORSCROLLBAR = 0x0137;
        public const int WM_CTLCOLORSTATIC = 0x0138;
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MBUTTONUP = 0x0208;
        public const int WM_MBUTTONDBLCLK = 0x0209;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_XBUTTONDOWN = 0x020B;
        public const int WM_XBUTTONUP = 0x020C;
        public const int WM_XBUTTONDBLCLK = 0x020D;
        public const int WM_MOUSELAST_5 = 0x020D;
        public const int WM_MOUSELAST_4 = 0x020A;
        public const int WM_MOUSELAST_PRE_4 = 0x0209;
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_ENTERMENULOOP = 0x0211;
        public const int WM_EXITMENULOOP = 0x0212;
        public const int WM_NEXTMENU = 0x0213;
        public const int WM_SIZING = 0x0214;
        public const int WM_CAPTURECHANGED = 0x0215;
        public const int WM_MOVING = 0x0216;
        public const int WM_POWERBROADCAST = 0x0218;
        public const int WM_DEVICECHANGE = 0x0219;
        public const int WM_MDICREATE = 0x0220;
        public const int WM_MDIDESTROY = 0x0221;
        public const int WM_MDIACTIVATE = 0x0222;
        public const int WM_MDIRESTORE = 0x0223;
        public const int WM_MDINEXT = 0x0224;
        public const int WM_MDIMAXIMIZE = 0x0225;
        public const int WM_MDITILE = 0x0226;
        public const int WM_MDICASCADE = 0x0227;
        public const int WM_MDIICONARRANGE = 0x0228;
        public const int WM_MDIGETACTIVE = 0x0229;
        public const int WM_MDISETMENU = 0x0230;
        public const int WM_ENTERSIZEMOVE = 0x0231;
        public const int WM_EXITSIZEMOVE = 0x0232;
        public const int WM_DROPFILES = 0x0233;
        public const int WM_MDIREFRESHMENU = 0x0234;
        public const int WM_MOUSEHOVER = 0x02A1;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_NCMOUSEHOVER = 0x02A0;
        public const int WM_NCMOUSELEAVE = 0x02A2;
        public const int WM_WTSSESSION_CHANGE = 0x02B1;
        public const int WM_TABLET_FIRST = 0x02c0;
        public const int WM_TABLET_LAST = 0x02df;
        public const int WM_CUT = 0x0300;
        public const int WM_COPY = 0x0301;
        public const int WM_PASTE = 0x0302;
        public const int WM_CLEAR = 0x0303;
        public const int WM_UNDO = 0x0304;
        public const int WM_RENDERFORMAT = 0x0305;
        public const int WM_RENDERALLFORMATS = 0x0306;
        public const int WM_DESTROYCLIPBOARD = 0x0307;
        public const int WM_DRAWCLIPBOARD = 0x0308;
        public const int WM_PAINTCLIPBOARD = 0x0309;
        public const int WM_VSCROLLCLIPBOARD = 0x030A;
        public const int WM_SIZECLIPBOARD = 0x030B;
        public const int WM_ASKCBFORMATNAME = 0x030C;
        public const int WM_CHANGECBCHAIN = 0x030D;
        public const int WM_HSCROLLCLIPBOARD = 0x030E;
        public const int WM_QUERYNEWPALETTE = 0x030F;
        public const int WM_PALETTEISCHANGING = 0x0310;
        public const int WM_PALETTECHANGED = 0x0311;
        public const int WM_HOTKEY = 0x0312;
        public const int WM_PRINT = 0x0317;
        public const int WM_PRINTCLIENT = 0x0318;
        public const int WM_APPCOMMAND = 0x0319;
        public const int WM_THEMECHANGED = 0x031A;
        public const int WM_HANDHELDFIRST = 0x0358;
        public const int WM_HANDHELDLAST = 0x035F;
        public const int WM_AFXFIRST = 0x0360;
        public const int WM_AFXLAST = 0x037F;
        public const int WM_PENWINFIRST = 0x0380;
        public const int WM_PENWINLAST = 0x038F;
        public const int WM_APP = 0x8000;
        public const int WM_USER = 0x0400;
        public const uint WS_OVERLAPPED = 0x00000000U;
        public const uint WS_POPUP = 0x80000000U;
        public const uint WS_CHILD = 0x40000000U;
        public const uint WS_MINIMIZE = 0x20000000U;
        public const uint WS_VISIBLE = 0x10000000U;
        public const uint WS_DISABLED = 0x08000000U;
        public const uint WS_CLIPSIBLINGS = 0x04000000U;
        public const uint WS_CLIPCHILDREN = 0x02000000U;
        public const uint WS_MAXIMIZE = 0x01000000U;
        public const uint WS_CAPTION = 0x00C00000U;    /* WS_BORDER | WS_DLGFRAME  */
        public const uint WS_BORDER = 0x00800000U;
        public const uint WS_DLGFRAME = 0x00400000U;
        public const uint WS_VSCROLL = 0x00200000U;
        public const uint WS_HSCROLL = 0x00100000U;
        public const uint WS_SYSMENU = 0x00080000U;
        public const uint WS_THICKFRAME = 0x00040000U;
        public const uint WS_GROUP = 0x00020000U;
        public const uint WS_TABSTOP = 0x00010000U;
        public const uint WS_MINIMIZEBOX = 0x00020000U;
        public const uint WS_MAXIMIZEBOX = 0x00010000U;
        public const uint WS_TILED = WS_OVERLAPPED;
        public const uint WS_ICONIC = WS_MINIMIZE;
        public const uint WS_SIZEBOX = WS_THICKFRAME;
        public const uint WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;
        public const uint WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED |
            WS_CAPTION |
            WS_SYSMENU |
            WS_THICKFRAME |
            WS_MINIMIZEBOX |
            WS_MAXIMIZEBOX);
        public const uint WS_POPUPWINDOW = (WS_POPUP |
            WS_BORDER |
            WS_SYSMENU);
        public const uint WS_CHILDWINDOW = (WS_CHILD);
        public const int EM_GETSEL = 0x00B0;
        public const int EM_SETSEL = 0x00B1;
        public const int EM_GETRECT = 0x00B2;
        public const int EM_SETRECT = 0x00B3;
        public const int EM_SETRECTNP = 0x00B4;
        public const int EM_SCROLL = 0x00B5;
        public const int EM_LINESCROLL = 0x00B6;
        public const int EM_SCROLLCARET = 0x00B7;
        public const int EM_GETMODIFY = 0x00B8;
        public const int EM_SETMODIFY = 0x00B9;
        public const int EM_GETLINECOUNT = 0x00BA;
        public const int EM_LINEINDEX = 0x00BB;
        public const int EM_SETHANDLE = 0x00BC;
        public const int EM_GETHANDLE = 0x00BD;
        public const int EM_GETTHUMB = 0x00BE;
        public const int EM_LINELENGTH = 0x00C1;
        public const int EM_REPLACESEL = 0x00C2;
        public const int EM_GETLINE = 0x00C4;
        public const int EM_LIMITTEXT = 0x00C5;
        public const int EM_CANUNDO = 0x00C6;
        public const int EM_UNDO = 0x00C7;
        public const int EM_FMTLINES = 0x00C8;
        public const int EM_LINEFROMCHAR = 0x00C9;
        public const int EM_SETTABSTOPS = 0x00CB;
        public const int EM_SETPASSWORDCHAR = 0x00CC;
        public const int EM_EMPTYUNDOBUFFER = 0x00CD;
        public const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        public const int EM_SETREADONLY = 0x00CF;
        public const int EM_SETWORDBREAKPROC = 0x00D0;
        public const int EM_GETWORDBREAKPROC = 0x00D1;
        public const int EM_GETPASSWORDCHAR = 0x00D2;
        public const int EM_SETMARGINS = 0x00D3;
        public const int EM_GETMARGINS = 0x00D4;
        public const int EM_SETLIMITTEXT = EM_LIMITTEXT;
        public const int EM_GETLIMITTEXT = 0x00D5;
        public const int EM_POSFROMCHAR = 0x00D6;
        public const int EM_CHARFROMPOS = 0x00D7;
        public const int EM_SETIMESTATUS = 0x00D8;
        public const int EM_GETIMESTATUS = 0x00D9;
        public const int BM_GETCHECK = 0x00F0;
        public const int BM_SETCHECK = 0x00F1;
        public const int BM_GETSTATE = 0x00F2;
        public const int BM_SETSTATE = 0x00F3;
        public const int BM_SETSTYLE = 0x00F4;
        public const int BM_CLICK = 0x00F5;
        public const int BM_GETIMAGE = 0x00F6;
        public const int BM_SETIMAGE = 0x00F7;
        public const int STM_SETICON = 0x0170;
        public const int STM_GETICON = 0x0171;
        public const int STM_SETIMAGE = 0x0172;
        public const int STM_GETIMAGE = 0x0173;
        public const int STM_MSGMAX = 0x0174;
        public const int DM_GETDEFID = (WM_USER + 0);
        public const int DM_SETDEFID = (WM_USER + 1);
        public const int DM_REPOSITION = (WM_USER + 2);
        public const int LB_ADDSTRING = 0x0180;
        public const int LB_INSERTSTRING = 0x0181;
        public const int LB_DELETESTRING = 0x0182;
        public const int LB_SELITEMRANGEEX = 0x0183;
        public const int LB_RESETCONTENT = 0x0184;
        public const int LB_SETSEL = 0x0185;
        public const int LB_SETCURSEL = 0x0186;
        public const int LB_GETSEL = 0x0187;
        public const int LB_GETCURSEL = 0x0188;
        public const int LB_GETTEXT = 0x0189;
        public const int LB_GETTEXTLEN = 0x018A;
        public const int LB_GETCOUNT = 0x018B;
        public const int LB_SELECTSTRING = 0x018C;
        public const int LB_DIR = 0x018D;
        public const int LB_GETTOPINDEX = 0x018E;
        public const int LB_FINDSTRING = 0x018F;
        public const int LB_GETSELCOUNT = 0x0190;
        public const int LB_GETSELITEMS = 0x0191;
        public const int LB_SETTABSTOPS = 0x0192;
        public const int LB_GETHORIZONTALEXTENT = 0x0193;
        public const int LB_SETHORIZONTALEXTENT = 0x0194;
        public const int LB_SETCOLUMNWIDTH = 0x0195;
        public const int LB_ADDFILE = 0x0196;
        public const int LB_SETTOPINDEX = 0x0197;
        public const int LB_GETITEMRECT = 0x0198;
        public const int LB_GETITEMDATA = 0x0199;
        public const int LB_SETITEMDATA = 0x019A;
        public const int LB_SELITEMRANGE = 0x019B;
        public const int LB_SETANCHORINDEX = 0x019C;
        public const int LB_GETANCHORINDEX = 0x019D;
        public const int LB_SETCARETINDEX = 0x019E;
        public const int LB_GETCARETINDEX = 0x019F;
        public const int LB_SETITEMHEIGHT = 0x01A0;
        public const int LB_GETITEMHEIGHT = 0x01A1;
        public const int LB_FINDSTRINGEXACT = 0x01A2;
        public const int LB_SETLOCALE = 0x01A5;
        public const int LB_GETLOCALE = 0x01A6;
        public const int LB_SETCOUNT = 0x01A7;
        public const int LB_INITSTORAGE = 0x01A8;
        public const int LB_ITEMFROMPOINT = 0x01A9;
        public const int LB_MULTIPLEADDSTRING = 0x01B1;
        public const int LB_GETLISTBOXINFO = 0x01B2;
        public const int LB_MSGMAX_501 = 0x01B3;
        public const int LB_MSGMAX_WCE4 = 0x01B1;
        public const int LB_MSGMAX_4 = 0x01B0;
        public const int LB_MSGMAX_PRE4 = 0x01A8;
        public const int CB_GETEDITSEL = 0x0140;
        public const int CB_LIMITTEXT = 0x0141;
        public const int CB_SETEDITSEL = 0x0142;
        public const int CB_ADDSTRING = 0x0143;
        public const int CB_DELETESTRING = 0x0144;
        public const int CB_DIR = 0x0145;
        public const int CB_GETCOUNT = 0x0146;
        public const int CB_GETCURSEL = 0x0147;
        public const int CB_GETLBTEXT = 0x0148;
        public const int CB_GETLBTEXTLEN = 0x0149;
        public const int CB_INSERTSTRING = 0x014A;
        public const int CB_RESETCONTENT = 0x014B;
        public const int CB_FINDSTRING = 0x014C;
        public const int CB_SELECTSTRING = 0x014D;
        public const int CB_SETCURSEL = 0x014E;
        public const int CB_SHOWDROPDOWN = 0x014F;
        public const int CB_GETITEMDATA = 0x0150;
        public const int CB_SETITEMDATA = 0x0151;
        public const int CB_GETDROPPEDCONTROLRECT = 0x0152;
        public const int CB_SETITEMHEIGHT = 0x0153;
        public const int CB_GETITEMHEIGHT = 0x0154;
        public const int CB_SETEXTENDEDUI = 0x0155;
        public const int CB_GETEXTENDEDUI = 0x0156;
        public const int CB_GETDROPPEDSTATE = 0x0157;
        public const int CB_FINDSTRINGEXACT = 0x0158;
        public const int CB_SETLOCALE = 0x0159;
        public const int CB_GETLOCALE = 0x015A;
        public const int CB_GETTOPINDEX = 0x015B;
        public const int CB_SETTOPINDEX = 0x015C;
        public const int CB_GETHORIZONTALEXTENT = 0x015d;
        public const int CB_SETHORIZONTALEXTENT = 0x015e;
        public const int CB_GETDROPPEDWIDTH = 0x015f;
        public const int CB_SETDROPPEDWIDTH = 0x0160;
        public const int CB_INITSTORAGE = 0x0161;
        public const int CB_MULTIPLEADDSTRING = 0x0163;
        public const int CB_GETCOMBOBOXINFO = 0x0164;
        public const int CB_MSGMAX_501 = 0x0165;
        public const int CB_MSGMAX_WCE400 = 0x0163;
        public const int CB_MSGMAX_400 = 0x0162;
        public const int CB_MSGMAX_PRE400 = 0x015B;
        public const int SBM_SETPOS = 0x00E0;
        public const int SBM_GETPOS = 0x00E1;
        public const int SBM_SETRANGE = 0x00E2;
        public const int SBM_SETRANGEREDRAW = 0x00E6;
        public const int SBM_GETRANGE = 0x00E3;
        public const int SBM_ENABLE_ARROWS = 0x00E4;
        public const int SBM_SETSCROLLINFO = 0x00E9;
        public const int SBM_GETSCROLLINFO = 0x00EA;
        public const int SBM_GETSCROLLBARINFO = 0x00EB;
        public const int GWL_EXSTYLE = (-20);
        public const int GWL_STYLE = (-16);
        public const int GWL_WNDPROC = (-4);
        public const int GWLP_WNDPROC = (-4);
        public const int GWL_HINSTANCE = (-6);
        public const int GWLP_HINSTANCE = (-6);
        public const int GWL_HWNDPARENT = (-8);
        public const int GWLP_HWNDPARENT = (-8);
        public const int GWL_ID = (-12);
        public const int GWLP_ID = (-12);
        public const int GWL_USERDATA = (-21);
        public const int GWLP_USERDATA = (-21);
        public const uint TPM_CENTERALIGN = 4U;
        public const uint TPM_LEFTALIGN = 0U;
        public const uint TPM_RIGHTALIGN = 8U;
        public const uint TPM_LEFTBUTTON = 0U;
        public const uint TPM_RIGHTBUTTON = 2U;
        public const uint TPM_HORIZONTAL = 0U;
        public const uint TPM_VERTICAL = 64U;
        public const uint TPM_TOPALIGN = 0U;
        public const uint TPM_VCENTERALIGN = 16U;
        public const uint TPM_BOTTOMALIGN = 32U;
        public const uint TPM_NONOTIFY = 128U;
        public const uint TPM_RETURNCMD = 256U;
        public const uint TPM_RECURSE = 1U;
        public const uint MIIM_STATE = 1U;
        public const uint MIIM_ID = 2U;
        public const uint MIIM_SUBMENU = 4U;
        public const uint MIIM_CHECKMARKS = 8U;
        public const uint MIIM_TYPE = 16U;
        public const uint MIIM_DATA = 32U;
        public const uint MIIM_STRING = 64U;
        public const uint MIIM_BITMAP = 128U;
        public const uint MIIM_FTYPE = 256U;
        public const uint MF_ENABLED = 0U;
        public const uint MF_GRAYED = 1U;
        public const uint MF_DISABLED = 2U;
        public const uint MF_BITMAP = 4U;
        public const uint MF_CHECKED = 8U;
        public const uint MF_MENUBARBREAK = 32U;
        public const uint MF_MENUBREAK = 64U;
        public const uint MF_OWNERDRAW = 256U;
        public const uint MF_POPUP = 16U;
        public const uint MF_SEPARATOR = 0x800U;
        public const uint MF_STRING = 0U;
        public const uint MF_UNCHECKED = 0U;
        public const uint MF_DEFAULT = 4096U;
        public const uint MF_SYSMENU = 0x2000U;
        public const uint MF_HELP = 0x4000U;
        public const uint MF_END = 128U;
        public const uint MF_RIGHTJUSTIFY = 0x4000U;
        public const uint MF_MOUSESELECT = 0x8000U;
        public const uint MF_INSERT = 0U;
        public const uint MF_CHANGE = 128U;
        public const uint MF_APPEND = 256U;
        public const uint MF_DELETE = 512U;
        public const uint MF_REMOVE = 4096U;
        public const uint MF_USECHECKBITMAPS = 512U;
        public const uint MF_UNHILITE = 0U;
        public const uint MF_HILITE = 128U;
        public const int MK_LBUTTON = 1;
        public const int MK_RBUTTON = 2;
        public const int MK_SHIFT = 4;
        public const int MK_CONTROL = 8;
        public const int MK_MBUTTON = 16;
        public const int MK_XBUTTON1 = 32;
        public const int MK_XBUTTON2 = 64;
        public const int WS_EX_ACCEPTFILES = 16;
        public const int WS_EX_APPWINDOW = 0x40000;
        public const int WS_EX_CLIENTEDGE = 512;
        public const int WS_EX_COMPOSITED = 0x2000000; /* XP */
        public const int WS_EX_CONTEXTHELP = 0x400;
        public const int WS_EX_CONTROLPARENT = 0x10000;
        public const int WS_EX_DLGMODALFRAME = 1;
        public const int WS_EX_LAYERED = 0x80000;   /* w2k */
        public const int WS_EX_LAYOUTRTL = 0x400000; /* w98, w2k */
        public const int WS_EX_LEFT = 0;
        public const int WS_EX_LEFTSCROLLBAR = 0x4000;
        public const int WS_EX_LTRREADING = 0;
        public const int WS_EX_MDICHILD = 64;
        public const int WS_EX_NOACTIVATE = 0x8000000; /* w2k */
        public const int WS_EX_NOINHERITLAYOUT = 0x100000; /* w2k */
        public const int WS_EX_NOPARENTNOTIFY = 4;
        public const int WS_EX_OVERLAPPEDWINDOW = 0x300;
        public const int WS_EX_PALETTEWINDOW = 0x188;
        public const int WS_EX_RIGHT = 0x1000;
        public const int WS_EX_RIGHTSCROLLBAR = 0;
        public const int WS_EX_RTLREADING = 0x2000;
        public const int WS_EX_STATICEDGE = 0x20000;
        public const int WS_EX_TOOLWINDOW = 128;
        public const int WS_EX_TOPMOST = 8;
        public const int WS_EX_TRANSPARENT = 32;
        public const int WS_EX_WINDOWEDGE = 256;
        #endregion // winuser.h

        #region imm.h
        public const int WM_CONVERTREQUESTEX = 0x108;
        public const int WM_IME_STARTCOMPOSITION = 0x10D;
        public const int WM_IME_ENDCOMPOSITION = 0x10E;
        public const int WM_IME_COMPOSITION = 0x10F;
        public const int WM_IME_KEYLAST = 0x10F;
        public const int WM_IME_SETCONTEXT = 0x281;
        public const int WM_IME_NOTIFY = 0x282;
        public const int WM_IME_CONTROL = 0x283;
        public const int WM_IME_COMPOSITIONFULL = 0x284;
        public const int WM_IME_SELECT = 0x285;
        public const int WM_IME_CHAR = 0x286;
        public const int WM_IME_KEYDOWN = 0x290;
        public const int WM_IME_KEYUP = 0x291;
        #endregion

        #region commctrl.h
        public const int NM_FIRST = (0 - 0);       // generic to all controls
        public const int NM_LAST = (0 - 99);
        public const int LVN_FIRST = (0 - 100);       // listview
        public const int LVN_LAST = (0 - 199);
        public const int HDN_FIRST = (0 - 300);       // header
        public const int HDN_LAST = (0 - 399);
        public const int TVN_FIRST = (0 - 400);       // treeview
        public const int TVN_LAST = (0 - 499);
        public const int TTN_FIRST = (0 - 520);       // tooltips
        public const int TTN_LAST = (0 - 549);
        public const int TCN_FIRST = (0 - 550);       // tab control
        public const int TCN_LAST = (0 - 580);
        public const int CDN_FIRST = (0 - 601);       // common dialog (new)
        public const int CDN_LAST = (0 - 699);
        public const int TBN_FIRST = (0 - 700);       // toolbar
        public const int TBN_LAST = (0 - 720);
        public const int UDN_FIRST = (0 - 721);        // updown
        public const int UDN_LAST = (0 - 740);
        public const int MCN_FIRST = (0 - 750);       // monthcal
        public const int MCN_LAST = (0 - 759);
        public const int DTN_FIRST = (0 - 760);       // datetimepick
        public const int DTN_LAST = (0 - 799);
        public const int CBEN_FIRST = (0 - 800);       // combo box ex
        public const int CBEN_LAST = (0 - 830);
        public const int RBN_FIRST = (0 - 831);       // rebar
        public const int RBN_LAST = (0 - 859);
        public const int IPN_FIRST = (0 - 860);       // internet address
        public const int IPN_LAST = (0 - 879);       // internet address
        public const int SBN_FIRST = (0 - 880);       // status bar
        public const int SBN_LAST = (0 - 899);
        public const int PGN_FIRST = (0 - 900);       // Pager Control
        public const int PGN_LAST = (0 - 950);
        public const int WMN_FIRST = (0 - 1000);
        public const int MN_LAST = (0 - 1200);
        public const int BCN_FIRST = (0 - 1250);
        public const int BCN_LAST = (0 - 1350);
        public const uint CCS_TOP = 0x00000001U;
        public const uint CCS_NOMOVEY = 0x00000002U;
        public const uint CCS_BOTTOM = 0x00000003U;
        public const uint CCS_NORESIZE = 0x00000004U;
        public const uint CCS_NOPARENTALIGN = 0x00000008U;
        public const uint CCS_ADJUSTABLE = 0x00000020U;
        public const uint CCS_NODIVIDER = 0x00000040U;
        public const uint CCS_VERT = 0x00000080U;
        public const uint CCS_LEFT = (CCS_VERT | CCS_TOP);
        public const uint CCS_RIGHT = (CCS_VERT | CCS_BOTTOM);
        public const uint CCS_NOMOVEX = (CCS_VERT | CCS_NOMOVEY);
        public const uint RBS_TOOLTIPS = 0x0100;
        public const uint RBS_VARHEIGHT = 0x0200;
        public const uint RBS_BANDBORDERS = 0x0400;
        public const uint RBS_FIXEDORDER = 0x0800;
        public const uint RBS_REGISTERDROP = 0x1000;
        public const uint RBS_AUTOSIZE = 0x2000;
        public const uint RBS_VERTICALGRIPPER = 0x4000; // this always has the vertical gripper (default for horizontal mode)
        public const uint RBS_DBLCLKTOGGLE = 0x8000;
        public const int LVM_FIRST = 0x1000;// ListView messages
        public const int TV_FIRST = 0x1100;// TreeView messages
        public const int HDM_FIRST = 0x1200;// Header messages
        public const int TCM_FIRST = 0x1300;
        public const int PGM_FIRST = 0x1400;
        public const int ECM_FIRST = 0x1500;// Edit control messages
        public const int BCM_FIRST = 0x1600;// Button control messages
        public const int CBM_FIRST = 0x1700;// Combobox control messages
        public const int CCM_FIRST = 0x2000;// Common control shared messages
        public const int CCM_LAST = (CCM_FIRST + 0x200);

        public const int CCM_SETBKCOLOR = (CCM_FIRST + 1);
        public const int CCM_SETCOLORSCHEME = (CCM_FIRST + 2);
        public const int CCM_GETCOLORSCHEME = (CCM_FIRST + 3);
        public const int CCM_GETDROPTARGET = (CCM_FIRST + 4);
        public const int CCM_SETUNICODEFORMAT = (CCM_FIRST + 5);
        public const int CCM_GETUNICODEFORMAT = (CCM_FIRST + 6);
        public const int CCM_SETVERSION = (CCM_FIRST + 0x7);
        public const int CCM_GETVERSION = (CCM_FIRST + 0x8);
        public const int CCM_SETNOTIFYWINDOW = (CCM_FIRST + 0x9);
        public const int CCM_SETWINDOWTHEME = (CCM_FIRST + 0xb);
        public const int CCM_DPISCALE = (CCM_FIRST + 0xc);
        public const int HDM_GETITEMCOUNT = (HDM_FIRST + 0);
        public const int HDM_INSERTITEMA = (HDM_FIRST + 1);
        public const int HDM_INSERTITEMW = (HDM_FIRST + 10);
        public const int HDM_DELETEITEM = (HDM_FIRST + 2);
        public const int HDM_GETITEMA = (HDM_FIRST + 3);
        public const int HDM_GETITEMW = (HDM_FIRST + 11);
        public const int HDM_SETITEMA = (HDM_FIRST + 4);
        public const int HDM_SETITEMW = (HDM_FIRST + 12);
        public const int HDM_LAYOUT = (HDM_FIRST + 5);
        public const int HDM_HITTEST = (HDM_FIRST + 6);
        public const int HDM_GETITEMRECT = (HDM_FIRST + 7);
        public const int HDM_SETIMAGELIST = (HDM_FIRST + 8);
        public const int HDM_GETIMAGELIST = (HDM_FIRST + 9);
        public const int HDM_ORDERTOINDEX = (HDM_FIRST + 15);
        public const int HDM_CREATEDRAGIMAGE = (HDM_FIRST + 16);
        public const int HDM_GETORDERARRAY = (HDM_FIRST + 17);
        public const int HDM_SETORDERARRAY = (HDM_FIRST + 18);
        public const int HDM_SETHOTDIVIDER = (HDM_FIRST + 19);
        public const int HDM_SETBITMAPMARGIN = (HDM_FIRST + 20);
        public const int HDM_GETBITMAPMARGIN = (HDM_FIRST + 21);
        public const int HDM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int HDM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int HDM_SETFILTERCHANGETIMEOUT = (HDM_FIRST + 22);
        public const int HDM_EDITFILTER = (HDM_FIRST + 23);
        public const int HDM_CLEARFILTER = (HDM_FIRST + 24);
        public const int TB_ENABLEBUTTON = (WM_USER + 1);
        public const int TB_CHECKBUTTON = (WM_USER + 2);
        public const int TB_PRESSBUTTON = (WM_USER + 3);
        public const int TB_HIDEBUTTON = (WM_USER + 4);
        public const int TB_INDETERMINATE = (WM_USER + 5);
        public const int TB_MARKBUTTON = (WM_USER + 6);
        public const int TB_ISBUTTONENABLED = (WM_USER + 9);
        public const int TB_ISBUTTONCHECKED = (WM_USER + 10);
        public const int TB_ISBUTTONPRESSED = (WM_USER + 11);
        public const int TB_ISBUTTONHIDDEN = (WM_USER + 12);
        public const int TB_ISBUTTONINDETERMINATE = (WM_USER + 13);
        public const int TB_ISBUTTONHIGHLIGHTED = (WM_USER + 14);
        public const int TB_SETSTATE = (WM_USER + 17);
        public const int TB_GETSTATE = (WM_USER + 18);
        public const int TB_ADDBITMAP = (WM_USER + 19);
        public const int TB_ADDBUTTONSA = (WM_USER + 20);
        public const int TB_INSERTBUTTONA = (WM_USER + 21);
        public const int TB_ADDBUTTONS = (WM_USER + 20);
        public const int TB_INSERTBUTTON = (WM_USER + 21);
        public const int TB_DELETEBUTTON = (WM_USER + 22);
        public const int TB_GETBUTTON = (WM_USER + 23);
        public const int TB_BUTTONCOUNT = (WM_USER + 24);
        public const int TB_COMMANDTOINDEX = (WM_USER + 25);
        public const int TB_SAVERESTOREA = (WM_USER + 26);
        public const int TB_SAVERESTOREW = (WM_USER + 76);
        public const int TB_CUSTOMIZE = (WM_USER + 27);
        public const int TB_ADDSTRINGA = (WM_USER + 28);
        public const int TB_ADDSTRINGW = (WM_USER + 77);

        public const int TB_GETITEMRECT = (WM_USER + 29);
        public const int TB_BUTTONSTRUCTSIZE = (WM_USER + 30);
        public const int TB_SETBUTTONSIZE = (WM_USER + 31);
        public const int TB_SETBITMAPSIZE = (WM_USER + 32);
        public const int TB_AUTOSIZE = (WM_USER + 33);
        public const int TB_GETTOOLTIPS = (WM_USER + 35);
        public const int TB_SETTOOLTIPS = (WM_USER + 36);
        public const int TB_SETPARENT = (WM_USER + 37);
        public const int TB_SETROWS = (WM_USER + 39);
        public const int TB_GETROWS = (WM_USER + 40);
        public const int TB_SETCMDID = (WM_USER + 42);
        public const int TB_CHANGEBITMAP = (WM_USER + 43);
        public const int TB_GETBITMAP = (WM_USER + 44);
        public const int TB_GETBUTTONTEXTA = (WM_USER + 45);
        public const int TB_GETBUTTONTEXTW = (WM_USER + 75);
        public const int TB_REPLACEBITMAP = (WM_USER + 46);
        public const int TB_SETINDENT = (WM_USER + 47);
        public const int TB_SETIMAGELIST = (WM_USER + 48);
        public const int TB_GETIMAGELIST = (WM_USER + 49);
        public const int TB_LOADIMAGES = (WM_USER + 50);
        public const int TB_GETRECT = (WM_USER + 51);
        public const int TB_SETHOTIMAGELIST = (WM_USER + 52);
        public const int TB_GETHOTIMAGELIST = (WM_USER + 53);
        public const int TB_SETDISABLEDIMAGELIST = (WM_USER + 54);
        public const int TB_GETDISABLEDIMAGELIST = (WM_USER + 55);
        public const int TB_SETSTYLE = (WM_USER + 56);
        public const int TB_GETSTYLE = (WM_USER + 57);
        public const int TB_GETBUTTONSIZE = (WM_USER + 58);
        public const int TB_SETBUTTONWIDTH = (WM_USER + 59);
        public const int TB_SETMAXTEXTROWS = (WM_USER + 60);
        public const int TB_GETTEXTROWS = (WM_USER + 61);
        public const int TB_GETOBJECT = (WM_USER + 62);
        public const int TB_GETHOTITEM = (WM_USER + 71);
        public const int TB_SETHOTITEM = (WM_USER + 72);
        public const int TB_SETANCHORHIGHLIGHT = (WM_USER + 73);
        public const int TB_GETANCHORHIGHLIGHT = (WM_USER + 74);
        public const int TB_MAPACCELERATORA = (WM_USER + 78);
        public const int TB_GETINSERTMARK = (WM_USER + 79);
        public const int TB_SETINSERTMARK = (WM_USER + 80);
        public const int TB_INSERTMARKHITTEST = (WM_USER + 81);
        public const int TB_MOVEBUTTON = (WM_USER + 82);
        public const int TB_GETMAXSIZE = (WM_USER + 83);
        public const int TB_SETEXTENDEDSTYLE = (WM_USER + 84);
        public const int TB_GETEXTENDEDSTYLE = (WM_USER + 85);
        public const int TB_GETPADDING = (WM_USER + 86);
        public const int TB_SETPADDING = (WM_USER + 87);
        public const int TB_SETINSERTMARKCOLOR = (WM_USER + 88);
        public const int TB_GETINSERTMARKCOLOR = (WM_USER + 89);
        public const int TB_SETCOLORSCHEME = CCM_SETCOLORSCHEME;
        public const int TB_GETCOLORSCHEME = CCM_GETCOLORSCHEME;
        public const int TB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int TB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int TB_MAPACCELERATORW = (WM_USER + 90);
        public const int TB_GETBITMAPFLAGS = (WM_USER + 41);
        public const int TB_GETBUTTONINFOW = (WM_USER + 63);
        public const int TB_SETBUTTONINFOW = (WM_USER + 64);
        public const int TB_GETBUTTONINFOA = (WM_USER + 65);
        public const int TB_SETBUTTONINFOA = (WM_USER + 66);
        public const int TB_INSERTBUTTONW = (WM_USER + 67);
        public const int TB_ADDBUTTONSW = (WM_USER + 68);

        public const int TB_HITTEST = (WM_USER + 69);
        public const int TB_SETDRAWTEXTFLAGS = (WM_USER + 70);
        public const int TB_GETSTRINGW = (WM_USER + 91);
        public const int TB_GETSTRINGA = (WM_USER + 92);
        public const int TB_GETMETRICS = (WM_USER + 101);
        public const int TB_SETMETRICS = (WM_USER + 102);
        public const int TB_SETWINDOWTHEME = CCM_SETWINDOWTHEME;
        public const int RB_INSERTBANDA = (WM_USER + 1);
        public const int RB_DELETEBAND = (WM_USER + 2);
        public const int RB_GETBARINFO = (WM_USER + 3);
        public const int RB_SETBARINFO = (WM_USER + 4);
        public const int RB_GETBANDINFO = (WM_USER + 5);
        public const int RB_SETBANDINFOA = (WM_USER + 6);
        public const int RB_SETPARENT = (WM_USER + 7);
        public const int RB_HITTEST = (WM_USER + 8);
        public const int RB_GETRECT = (WM_USER + 9);
        public const int RB_INSERTBANDW = (WM_USER + 10);
        public const int RB_SETBANDINFOW = (WM_USER + 11);
        public const int RB_GETBANDCOUNT = (WM_USER + 12);
        public const int RB_GETROWCOUNT = (WM_USER + 13);
        public const int RB_GETROWHEIGHT = (WM_USER + 14);
        public const int RB_IDTOINDEX = (WM_USER + 16);
        public const int RB_GETTOOLTIPS = (WM_USER + 17);
        public const int RB_SETTOOLTIPS = (WM_USER + 18);
        public const int RB_SETBKCOLOR = (WM_USER + 19);
        public const int RB_GETBKCOLOR = (WM_USER + 20);
        public const int RB_SETTEXTCOLOR = (WM_USER + 21);
        public const int RB_GETTEXTCOLOR = (WM_USER + 22);
        public const int RB_SIZETORECT = (WM_USER + 23);
        public const int RB_SETCOLORSCHEME = CCM_SETCOLORSCHEME;
        public const int RB_GETCOLORSCHEME = CCM_GETCOLORSCHEME;
        public const int RB_BEGINDRAG = (WM_USER + 24);
        public const int RB_ENDDRAG = (WM_USER + 25);
        public const int RB_DRAGMOVE = (WM_USER + 26);
        public const int RB_GETBARHEIGHT = (WM_USER + 27);
        public const int RB_GETBANDINFOW = (WM_USER + 28);

        public const int RB_GETBANDINFOA = (WM_USER + 29);
        public const int RB_MINIMIZEBAND = (WM_USER + 30);
        public const int RB_MAXIMIZEBAND = (WM_USER + 31);
        public const int RB_GETDROPTARGET = (CCM_GETDROPTARGET);
        public const int RB_GETBANDBORDERS = (WM_USER + 34);
        public const int RB_SHOWBAND = (WM_USER + 35);
        public const int RB_SETPALETTE = (WM_USER + 37);
        public const int RB_GETPALETTE = (WM_USER + 38);
        public const int RB_MOVEBAND = (WM_USER + 39);
        public const int RB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int RB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int RB_GETBANDMARGINS = (WM_USER + 40);
        public const int RB_SETWINDOWTHEME = CCM_SETWINDOWTHEME;
        public const int RB_PUSHCHEVRON = (WM_USER + 43);
        public const int TTM_ACTIVATE = (WM_USER + 1);
        public const int TTM_SETDELAYTIME = (WM_USER + 3);
        public const int TTM_ADDTOOLA = (WM_USER + 4);
        public const int TTM_ADDTOOLW = (WM_USER + 50);
        public const int TTM_DELTOOLA = (WM_USER + 5);
        public const int TTM_DELTOOLW = (WM_USER + 51);
        public const int TTM_NEWTOOLRECTA = (WM_USER + 6);
        public const int TTM_NEWTOOLRECTW = (WM_USER + 52);
        public const int TTM_RELAYEVENT = (WM_USER + 7);
        public const int TTM_GETTOOLINFOA = (WM_USER + 8);
        public const int TTM_GETTOOLINFOW = (WM_USER + 53);
        public const int TTM_SETTOOLINFOA = (WM_USER + 9);
        public const int TTM_SETTOOLINFOW = (WM_USER + 54);
        public const int TTM_HITTESTA = (WM_USER + 10);
        public const int TTM_HITTESTW = (WM_USER + 55);
        public const int TTM_GETTEXTA = (WM_USER + 11);
        public const int TTM_GETTEXTW = (WM_USER + 56);
        public const int TTM_UPDATETIPTEXTA = (WM_USER + 12);

        public const int TTM_UPDATETIPTEXTW = (WM_USER + 57);
        public const int TTM_GETTOOLCOUNT = (WM_USER + 13);
        public const int TTM_ENUMTOOLSA = (WM_USER + 14);
        public const int TTM_ENUMTOOLSW = (WM_USER + 58);
        public const int TTM_GETCURRENTTOOLA = (WM_USER + 15);
        public const int TTM_GETCURRENTTOOLW = (WM_USER + 59);
        public const int TTM_WINDOWFROMPOINT = (WM_USER + 16);
        public const int TTM_TRACKACTIVATE = (WM_USER + 17);
        public const int TTM_TRACKPOSITION = (WM_USER + 18);
        public const int TTM_SETTIPBKCOLOR = (WM_USER + 19);
        public const int TTM_SETTIPTEXTCOLOR = (WM_USER + 20);
        public const int TTM_GETDELAYTIME = (WM_USER + 21);
        public const int TTM_GETTIPBKCOLOR = (WM_USER + 22);
        public const int TTM_GETTIPTEXTCOLOR = (WM_USER + 23);
        public const int TTM_SETMAXTIPWIDTH = (WM_USER + 24);
        public const int TTM_GETMAXTIPWIDTH = (WM_USER + 25);
        public const int TTM_SETMARGIN = (WM_USER + 26);
        public const int TTM_GETMARGIN = (WM_USER + 27);
        public const int TTM_POP = (WM_USER + 28);
        public const int TTM_UPDATE = (WM_USER + 29);
        public const int TTM_GETBUBBLESIZE = (WM_USER + 30);
        public const int TTM_ADJUSTRECT = (WM_USER + 31);
        public const int TTM_SETTITLEA = (WM_USER + 32);
        public const int TTM_SETTITLEW = (WM_USER + 33);
        public const int TTM_POPUP = (WM_USER + 34);
        public const int TTM_GETTITLE = (WM_USER + 35);
        public const int TTM_SETWINDOWTHEME = CCM_SETWINDOWTHEME;
        public const int SB_SETTEXTA = (WM_USER + 1);
        public const int SB_SETTEXTW = (WM_USER + 11);
        public const int SB_GETTEXTA = (WM_USER + 2);
        public const int SB_GETTEXTW = (WM_USER + 13);
        public const int SB_GETTEXTLENGTHA = (WM_USER + 3);
        public const int SB_GETTEXTLENGTHW = (WM_USER + 12);
        public const int SB_SETPARTS = (WM_USER + 4);
        public const int SB_GETPARTS = (WM_USER + 6);
        public const int SB_GETBORDERS = (WM_USER + 7);
        public const int SB_SETMINHEIGHT = (WM_USER + 8);

        public const int SB_SIMPLE = (WM_USER + 9);
        public const int SB_GETRECT = (WM_USER + 10);
        public const int SB_ISSIMPLE = (WM_USER + 14);
        public const int SB_SETICON = (WM_USER + 15);
        public const int SB_SETTIPTEXTA = (WM_USER + 16);
        public const int SB_SETTIPTEXTW = (WM_USER + 17);
        public const int SB_GETTIPTEXTA = (WM_USER + 18);
        public const int SB_GETTIPTEXTW = (WM_USER + 19);
        public const int SB_GETICON = (WM_USER + 20);
        public const int SB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int SB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int SB_SETBKCOLOR = CCM_SETBKCOLOR;
        public const int SB_SIMPLEID = 0x00ff;
        public const int TBM_GETPOS = (WM_USER);
        public const int TBM_GETRANGEMIN = (WM_USER + 1);
        public const int TBM_GETRANGEMAX = (WM_USER + 2);
        public const int TBM_GETTIC = (WM_USER + 3);
        public const int TBM_SETTIC = (WM_USER + 4);
        public const int TBM_SETPOS = (WM_USER + 5);
        public const int TBM_SETRANGE = (WM_USER + 6);
        public const int TBM_SETRANGEMIN = (WM_USER + 7);
        public const int TBM_SETRANGEMAX = (WM_USER + 8);
        public const int TBM_CLEARTICS = (WM_USER + 9);
        public const int TBM_SETSEL = (WM_USER + 10);
        public const int TBM_SETSELSTART = (WM_USER + 11);
        public const int TBM_SETSELEND = (WM_USER + 12);
        public const int TBM_GETPTICS = (WM_USER + 14);
        public const int TBM_GETTICPOS = (WM_USER + 15);
        public const int TBM_GETNUMTICS = (WM_USER + 16);
        public const int TBM_GETSELSTART = (WM_USER + 17);
        public const int TBM_GETSELEND = (WM_USER + 18);
        public const int TBM_CLEARSEL = (WM_USER + 19);
        public const int TBM_SETTICFREQ = (WM_USER + 20);
        public const int TBM_SETPAGESIZE = (WM_USER + 21);
        public const int TBM_GETPAGESIZE = (WM_USER + 22);
        public const int TBM_SETLINESIZE = (WM_USER + 23);
        public const int TBM_GETLINESIZE = (WM_USER + 24);
        public const int TBM_GETTHUMBRECT = (WM_USER + 25);
        public const int TBM_GETCHANNELRECT = (WM_USER + 26);
        public const int TBM_SETTHUMBLENGTH = (WM_USER + 27);
        public const int TBM_GETTHUMBLENGTH = (WM_USER + 28);
        public const int TBM_SETTOOLTIPS = (WM_USER + 29);
        public const int TBM_GETTOOLTIPS = (WM_USER + 30);
        public const int TBM_SETTIPSIDE = (WM_USER + 31);
        public const int TBM_SETBUDDY = (WM_USER + 32);
        public const int TBM_GETBUDDY = (WM_USER + 33);
        public const int TBM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int TBM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int DL_BEGINDRAG = (WM_USER + 133);
        public const int DL_DRAGGING = (WM_USER + 134);
        public const int DL_DROPPED = (WM_USER + 135);
        public const int DL_CANCELDRAG = (WM_USER + 136);
        public const int UDM_SETRANGE = (WM_USER + 101);
        public const int UDM_GETRANGE = (WM_USER + 102);
        public const int UDM_SETPOS = (WM_USER + 103);
        public const int UDM_GETPOS = (WM_USER + 104);
        public const int UDM_SETBUDDY = (WM_USER + 105);
        public const int UDM_GETBUDDY = (WM_USER + 106);
        public const int UDM_SETACCEL = (WM_USER + 107);
        public const int UDM_GETACCEL = (WM_USER + 108);
        public const int UDM_SETBASE = (WM_USER + 109);
        public const int UDM_GETBASE = (WM_USER + 110);
        public const int UDM_SETRANGE32 = (WM_USER + 111);
        public const int UDM_GETRANGE32 = (WM_USER + 112);
        public const int UDM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int UDM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int UDM_SETPOS32 = (WM_USER + 113);
        public const int UDM_GETPOS32 = (WM_USER + 114);
        public const int PBM_SETRANGE = (WM_USER + 1);
        public const int PBM_SETPOS = (WM_USER + 2);
        public const int PBM_DELTAPOS = (WM_USER + 3);
        public const int PBM_SETSTEP = (WM_USER + 4);
        public const int PBM_STEPIT = (WM_USER + 5);
        public const int PBM_SETRANGE32 = (WM_USER + 6);
        public const int PBM_GETRANGE = (WM_USER + 7);
        public const int PBM_GETPOS = (WM_USER + 8);
        public const int PBM_SETBARCOLOR = (WM_USER + 9);
        public const int PBM_SETBKCOLOR = CCM_SETBKCOLOR;
        public const int HKM_SETHOTKEY = (WM_USER + 1);
        public const int HKM_GETHOTKEY = (WM_USER + 2);
        public const int HKM_SETRULES = (WM_USER + 3);
        public const int LVM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int LVM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int LVM_GETBKCOLOR = (LVM_FIRST + 0);
        public const int LVM_SETBKCOLOR = (LVM_FIRST + 1);
        public const int LVM_GETIMAGELIST = (LVM_FIRST + 2);
        public const int LVM_SETIMAGELIST = (LVM_FIRST + 3);
        public const int LVM_GETITEMCOUNT = (LVM_FIRST + 4);
        public const int LVM_GETITEMA = (LVM_FIRST + 5);
        public const int LVM_GETITEMW = (LVM_FIRST + 75);
        public const int LVM_SETITEMA = (LVM_FIRST + 6);
        public const int LVM_SETITEMW = (LVM_FIRST + 76);
        public const int LVM_INSERTITEMA = (LVM_FIRST + 7);
        public const int LVM_INSERTITEMW = (LVM_FIRST + 77);
        public const int LVM_DELETEITEM = (LVM_FIRST + 8);
        public const int LVM_DELETEALLITEMS = (LVM_FIRST + 9);
        public const int LVM_GETCALLBACKMASK = (LVM_FIRST + 10);
        public const int LVM_SETCALLBACKMASK = (LVM_FIRST + 11);
        public const int LVM_FINDITEMA = (LVM_FIRST + 13);
        public const int LVM_FINDITEMW = (LVM_FIRST + 83);
        public const int LVM_GETITEMRECT = (LVM_FIRST + 14);
        public const int LVM_SETITEMPOSITION = (LVM_FIRST + 15);
        public const int LVM_GETITEMPOSITION = (LVM_FIRST + 16);
        public const int LVM_GETSTRINGWIDTHA = (LVM_FIRST + 17);
        public const int LVM_GETSTRINGWIDTHW = (LVM_FIRST + 87);
        public const int LVM_HITTEST = (LVM_FIRST + 18);
        public const int LVM_ENSUREVISIBLE = (LVM_FIRST + 19);
        public const int LVM_SCROLL = (LVM_FIRST + 20);
        public const int LVM_REDRAWITEMS = (LVM_FIRST + 21);
        public const int LVM_ARRANGE = (LVM_FIRST + 22);
        public const int LVM_EDITLABELA = (LVM_FIRST + 23);
        public const int LVM_EDITLABELW = (LVM_FIRST + 118);
        public const int LVM_GETEDITCONTROL = (LVM_FIRST + 24);
        public const int LVM_GETCOLUMNA = (LVM_FIRST + 25);
        public const int LVM_GETCOLUMNW = (LVM_FIRST + 95);
        public const int LVM_SETCOLUMNA = (LVM_FIRST + 26);
        public const int LVM_SETCOLUMNW = (LVM_FIRST + 96);
        public const int LVM_INSERTCOLUMNA = (LVM_FIRST + 27);
        public const int LVM_INSERTCOLUMNW = (LVM_FIRST + 97);
        public const int LVM_DELETECOLUMN = (LVM_FIRST + 28);
        public const int LVM_GETCOLUMNWIDTH = (LVM_FIRST + 29);
        public const int LVM_SETCOLUMNWIDTH = (LVM_FIRST + 30);
        public const int LVM_CREATEDRAGIMAGE = (LVM_FIRST + 33);
        public const int LVM_GETVIEWRECT = (LVM_FIRST + 34);
        public const int LVM_GETTEXTCOLOR = (LVM_FIRST + 35);
        public const int LVM_SETTEXTCOLOR = (LVM_FIRST + 36);
        public const int LVM_GETTEXTBKCOLOR = (LVM_FIRST + 37);
        public const int LVM_SETTEXTBKCOLOR = (LVM_FIRST + 38);
        public const int LVM_GETTOPINDEX = (LVM_FIRST + 39);
        public const int LVM_GETCOUNTPERPAGE = (LVM_FIRST + 40);
        public const int LVM_GETORIGIN = (LVM_FIRST + 41);
        public const int LVM_UPDATE = (LVM_FIRST + 42);
        public const int LVM_SETITEMSTATE = (LVM_FIRST + 43);
        public const int LVM_GETITEMSTATE = (LVM_FIRST + 44);
        public const int LVM_GETITEMTEXTA = (LVM_FIRST + 45);
        public const int LVM_GETITEMTEXTW = (LVM_FIRST + 115);
        public const int LVM_SETITEMTEXTA = (LVM_FIRST + 46);
        public const int LVM_SETITEMTEXTW = (LVM_FIRST + 116);
        public const int LVM_SETITEMCOUNT = (LVM_FIRST + 47);
        public const int LVM_SORTITEMS = (LVM_FIRST + 48);
        public const int LVM_SETITEMPOSITION32 = (LVM_FIRST + 49);
        public const int LVM_GETSELECTEDCOUNT = (LVM_FIRST + 50);
        public const int LVM_GETITEMSPACING = (LVM_FIRST + 51);
        public const int LVM_GETISEARCHSTRINGA = (LVM_FIRST + 52);
        public const int LVM_GETISEARCHSTRINGW = (LVM_FIRST + 117);
        public const int LVM_SETICONSPACING = (LVM_FIRST + 53);
        public const int LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54);
        public const int LVM_GETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 55);
        public const int LVM_GETSUBITEMRECT = (LVM_FIRST + 56);
        public const int LVM_SUBITEMHITTEST = (LVM_FIRST + 57);
        public const int LVM_SETCOLUMNORDERARRAY = (LVM_FIRST + 58);
        public const int LVM_GETCOLUMNORDERARRAY = (LVM_FIRST + 59);
        public const int LVM_SETHOTITEM = (LVM_FIRST + 60);
        public const int LVM_GETHOTITEM = (LVM_FIRST + 61);
        public const int LVM_SETHOTCURSOR = (LVM_FIRST + 62);
        public const int LVM_GETHOTCURSOR = (LVM_FIRST + 63);
        public const int LVM_APPROXIMATEVIEWRECT = (LVM_FIRST + 64);
        public const int LVM_SETWORKAREAS = (LVM_FIRST + 65);
        public const int LVM_GETWORKAREAS = (LVM_FIRST + 70);
        public const int LVM_GETNUMBEROFWORKAREAS = (LVM_FIRST + 73);
        public const int LVM_GETSELECTIONMARK = (LVM_FIRST + 66);
        public const int LVM_SETSELECTIONMARK = (LVM_FIRST + 67);
        public const int LVM_SETHOVERTIME = (LVM_FIRST + 71);
        public const int LVM_GETHOVERTIME = (LVM_FIRST + 72);
        public const int LVM_SETTOOLTIPS = (LVM_FIRST + 74);
        public const int LVM_GETTOOLTIPS = (LVM_FIRST + 78);
        public const int LVM_SORTITEMSEX = (LVM_FIRST + 81);
        public const int LVM_SETBKIMAGEA = (LVM_FIRST + 68);
        public const int LVM_SETBKIMAGEW = (LVM_FIRST + 138);
        public const int LVM_GETBKIMAGEA = (LVM_FIRST + 69);
        public const int LVM_GETBKIMAGEW = (LVM_FIRST + 139);
        public const int LVM_SETSELECTEDCOLUMN = (LVM_FIRST + 140);
        public const int LVM_SETTILEWIDTH = (LVM_FIRST + 141);
        public const int LVM_SETVIEW = (LVM_FIRST + 142);
        public const int LVM_GETVIEW = (LVM_FIRST + 143);
        public const int LVM_INSERTGROUP = (LVM_FIRST + 145);
        public const int LVM_SETGROUPINFO = (LVM_FIRST + 147);
        public const int LVM_GETGROUPINFO = (LVM_FIRST + 149);
        public const int LVM_REMOVEGROUP = (LVM_FIRST + 150);
        public const int LVM_MOVEGROUP = (LVM_FIRST + 151);
        public const int LVM_MOVEITEMTOGROUP = (LVM_FIRST + 154);
        public const int LVM_SETGROUPMETRICS = (LVM_FIRST + 155);
        public const int LVM_GETGROUPMETRICS = (LVM_FIRST + 156);
        public const int LVM_ENABLEGROUPVIEW = (LVM_FIRST + 157);
        public const int LVM_SORTGROUPS = (LVM_FIRST + 158);
        public const int LVM_INSERTGROUPSORTED = (LVM_FIRST + 159);
        public const int LVM_REMOVEALLGROUPS = (LVM_FIRST + 160);
        public const int LVM_HASGROUP = (LVM_FIRST + 161);
        public const int LVM_SETTILEVIEWINFO = (LVM_FIRST + 162);
        public const int LVM_GETTILEVIEWINFO = (LVM_FIRST + 163);
        public const int LVM_SETTILEINFO = (LVM_FIRST + 164);
        public const int LVM_GETTILEINFO = (LVM_FIRST + 165);
        public const int LVM_SETINSERTMARK = (LVM_FIRST + 166);
        public const int LVM_GETINSERTMARK = (LVM_FIRST + 167);
        public const int LVM_INSERTMARKHITTEST = (LVM_FIRST + 168);
        public const int LVM_GETINSERTMARKRECT = (LVM_FIRST + 169);
        public const int LVM_SETINSERTMARKCOLOR = (LVM_FIRST + 170);
        public const int LVM_GETINSERTMARKCOLOR = (LVM_FIRST + 171);
        public const int LVM_SETINFOTIP = (LVM_FIRST + 173);
        public const int LVM_GETSELECTEDCOLUMN = (LVM_FIRST + 174);
        public const int LVM_ISGROUPVIEWENABLED = (LVM_FIRST + 175);
        public const int LVM_GETOUTLINECOLOR = (LVM_FIRST + 176);
        public const int LVM_SETOUTLINECOLOR = (LVM_FIRST + 177);
        public const int LVM_CANCELEDITLABEL = (LVM_FIRST + 179);
        public const int LVM_MAPINDEXTOID = (LVM_FIRST + 180);
        public const int LVM_MAPIDTOINDEX = (LVM_FIRST + 181);
        public const int TVM_INSERTITEMA = (TV_FIRST + 0);
        public const int TVM_INSERTITEMW = (TV_FIRST + 50);
        public const int TVM_DELETEITEM = (TV_FIRST + 1);
        public const int TVM_EXPAND = (TV_FIRST + 2);
        public const int TVM_GETITEMRECT = (TV_FIRST + 4);
        public const int TVM_GETCOUNT = (TV_FIRST + 5);
        public const int TVM_GETINDENT = (TV_FIRST + 6);
        public const int TVM_SETINDENT = (TV_FIRST + 7);
        public const int TVM_GETIMAGELIST = (TV_FIRST + 8);
        public const int TVM_SETIMAGELIST = (TV_FIRST + 9);
        public const int TVM_GETNEXTITEM = (TV_FIRST + 10);
        public const int TVM_SELECTITEM = (TV_FIRST + 11);
        public const int TVM_GETITEMA = (TV_FIRST + 12);
        public const int TVM_GETITEMW = (TV_FIRST + 62);
        public const int TVM_SETITEMA = (TV_FIRST + 13);
        public const int TVM_SETITEMW = (TV_FIRST + 63);
        public const int TVM_EDITLABELA = (TV_FIRST + 14);
        public const int TVM_EDITLABELW = (TV_FIRST + 65);
        public const int TVM_GETEDITCONTROL = (TV_FIRST + 15);
        public const int TVM_GETVISIBLECOUNT = (TV_FIRST + 16);
        public const int TVM_HITTEST = (TV_FIRST + 17);
        public const int TVM_CREATEDRAGIMAGE = (TV_FIRST + 18);
        public const int TVM_SORTCHILDREN = (TV_FIRST + 19);
        public const int TVM_ENSUREVISIBLE = (TV_FIRST + 20);
        public const int TVM_SORTCHILDRENCB = (TV_FIRST + 21);
        public const int TVM_ENDEDITLABELNOW = (TV_FIRST + 22);
        public const int TVM_GETISEARCHSTRINGA = (TV_FIRST + 23);
        public const int TVM_GETISEARCHSTRINGW = (TV_FIRST + 64);
        public const int TVM_SETTOOLTIPS = (TV_FIRST + 24);
        public const int TVM_GETTOOLTIPS = (TV_FIRST + 25);
        public const int TVM_SETINSERTMARK = (TV_FIRST + 26);
        public const int TVM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int TVM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int TVM_SETITEMHEIGHT = (TV_FIRST + 27);
        public const int TVM_GETITEMHEIGHT = (TV_FIRST + 28);
        public const int TVM_SETBKCOLOR = (TV_FIRST + 29);
        public const int TVM_SETTEXTCOLOR = (TV_FIRST + 30);
        public const int TVM_GETBKCOLOR = (TV_FIRST + 31);
        public const int TVM_GETTEXTCOLOR = (TV_FIRST + 32);
        public const int TVM_SETSCROLLTIME = (TV_FIRST + 33);
        public const int TVM_GETSCROLLTIME = (TV_FIRST + 34);
        public const int TVM_SETINSERTMARKCOLOR = (TV_FIRST + 37);
        public const int TVM_GETINSERTMARKCOLOR = (TV_FIRST + 38);
        public const int TVM_GETITEMSTATE = (TV_FIRST + 39);
        public const int TVM_SETLINECOLOR = (TV_FIRST + 40);
        public const int TVM_GETLINECOLOR = (TV_FIRST + 41);
        public const int TVM_MAPACCIDTOHTREEITEM = (TV_FIRST + 42);
        public const int TVM_MAPHTREEITEMTOACCID = (TV_FIRST + 43);
        public const int CBEM_INSERTITEMA = (WM_USER + 1);
        public const int CBEM_SETIMAGELIST = (WM_USER + 2);
        public const int CBEM_GETIMAGELIST = (WM_USER + 3);
        public const int CBEM_GETITEMA = (WM_USER + 4);
        public const int CBEM_SETITEMA = (WM_USER + 5);
        public const int CBEM_DELETEITEM = CB_DELETESTRING;
        public const int CBEM_GETCOMBOCONTROL = (WM_USER + 6);
        public const int CBEM_GETEDITCONTROL = (WM_USER + 7);
        public const int CBEM_SETEXTENDEDSTYLE = (WM_USER + 14);
        public const int CBEM_GETEXTENDEDSTYLE = (WM_USER + 9);
        public const int CBEM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int CBEM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int CBEM_SETEXSTYLE = (WM_USER + 8);
        public const int CBEM_GETEXSTYLE = (WM_USER + 9);
        public const int CBEM_HASEDITCHANGED = (WM_USER + 10);
        public const int CBEM_INSERTITEMW = (WM_USER + 11);
        public const int CBEM_SETITEMW = (WM_USER + 12);
        public const int CBEM_GETITEMW = (WM_USER + 13);
        public const int TCM_GETIMAGELIST = (TCM_FIRST + 2);
        public const int TCM_SETIMAGELIST = (TCM_FIRST + 3);
        public const int TCM_GETITEMCOUNT = (TCM_FIRST + 4);
        public const int TCM_GETITEMA = (TCM_FIRST + 5);
        public const int TCM_GETITEMW = (TCM_FIRST + 60);
        public const int TCM_SETITEMA = (TCM_FIRST + 6);
        public const int TCM_SETITEMW = (TCM_FIRST + 61);
        public const int TCM_INSERTITEMA = (TCM_FIRST + 7);
        public const int TCM_INSERTITEMW = (TCM_FIRST + 62);
        public const int TCM_DELETEITEM = (TCM_FIRST + 8);
        public const int TCM_DELETEALLITEMS = (TCM_FIRST + 9);
        public const int TCM_GETITEMRECT = (TCM_FIRST + 10);
        public const int TCM_GETCURSEL = (TCM_FIRST + 11);
        public const int TCM_SETCURSEL = (TCM_FIRST + 12);
        public const int TCM_HITTEST = (TCM_FIRST + 13);
        public const int TCM_SETITEMEXTRA = (TCM_FIRST + 14);
        public const int TCM_ADJUSTRECT = (TCM_FIRST + 40);
        public const int TCM_SETITEMSIZE = (TCM_FIRST + 41);
        public const int TCM_REMOVEIMAGE = (TCM_FIRST + 42);
        public const int TCM_SETPADDING = (TCM_FIRST + 43);
        public const int TCM_GETROWCOUNT = (TCM_FIRST + 44);
        public const int TCM_GETTOOLTIPS = (TCM_FIRST + 45);
        public const int TCM_SETTOOLTIPS = (TCM_FIRST + 46);
        public const int TCM_GETCURFOCUS = (TCM_FIRST + 47);
        public const int TCM_SETCURFOCUS = (TCM_FIRST + 48);
        public const int TCM_SETMINTABWIDTH = (TCM_FIRST + 49);
        public const int TCM_DESELECTALL = (TCM_FIRST + 50);
        public const int TCM_HIGHLIGHTITEM = (TCM_FIRST + 51);
        public const int TCM_SETEXTENDEDSTYLE = (TCM_FIRST + 52);
        public const int TCM_GETEXTENDEDSTYLE = (TCM_FIRST + 53);
        public const int TCM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int TCM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int ACM_OPENA = (WM_USER + 100);
        public const int ACM_OPENW = (WM_USER + 103);
        public const int ACM_PLAY = (WM_USER + 101);
        public const int ACM_STOP = (WM_USER + 102);
        public const int MCM_FIRST = 0x1000;
        public const int MCM_GETCURSEL = (MCM_FIRST + 1);
        public const int MCM_SETCURSEL = (MCM_FIRST + 2);
        public const int MCM_GETMAXSELCOUNT = (MCM_FIRST + 3);
        public const int MCM_SETMAXSELCOUNT = (MCM_FIRST + 4);
        public const int MCM_GETSELRANGE = (MCM_FIRST + 5);
        public const int MCM_SETSELRANGE = (MCM_FIRST + 6);
        public const int MCM_GETMONTHRANGE = (MCM_FIRST + 7);
        public const int MCM_SETDAYSTATE = (MCM_FIRST + 8);
        public const int MCM_GETMINREQRECT = (MCM_FIRST + 9);
        public const int MCM_SETCOLOR = (MCM_FIRST + 10);
        public const int MCM_GETCOLOR = (MCM_FIRST + 11);
        public const int MCM_SETTODAY = (MCM_FIRST + 12);
        public const int MCM_GETTODAY = (MCM_FIRST + 13);
        public const int MCM_HITTEST = (MCM_FIRST + 14);
        public const int MCM_SETFIRSTDAYOFWEEK = (MCM_FIRST + 15);
        public const int MCM_GETFIRSTDAYOFWEEK = (MCM_FIRST + 16);
        public const int MCM_GETRANGE = (MCM_FIRST + 17);
        public const int MCM_SETRANGE = (MCM_FIRST + 18);
        public const int MCM_GETMONTHDELTA = (MCM_FIRST + 19);
        public const int MCM_SETMONTHDELTA = (MCM_FIRST + 20);
        public const int MCM_GETMAXTODAYWIDTH = (MCM_FIRST + 21);
        public const int MCM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int MCM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int DTM_FIRST = 0x1000;
        public const int DTM_GETSYSTEMTIME = (DTM_FIRST + 1);
        public const int DTM_SETSYSTEMTIME = (DTM_FIRST + 2);
        public const int DTM_GETRANGE = (DTM_FIRST + 3);
        public const int DTM_SETRANGE = (DTM_FIRST + 4);
        public const int DTM_SETFORMATA = (DTM_FIRST + 5);
        public const int DTM_SETFORMATW = (DTM_FIRST + 50);
        public const int DTM_SETMCCOLOR = (DTM_FIRST + 6);
        public const int DTM_GETMCCOLOR = (DTM_FIRST + 7);
        public const int DTM_GETMONTHCAL = (DTM_FIRST + 8);
        public const int DTM_SETMCFONT = (DTM_FIRST + 9);
        public const int DTM_GETMCFONT = (DTM_FIRST + 10);
        public const int PGM_SETCHILD = (PGM_FIRST + 1);
        public const int PGM_RECALCSIZE = (PGM_FIRST + 2);
        public const int PGM_FORWARDMOUSE = (PGM_FIRST + 3);
        public const int PGM_SETBKCOLOR = (PGM_FIRST + 4);
        public const int PGM_GETBKCOLOR = (PGM_FIRST + 5);
        public const int PGM_SETBORDER = (PGM_FIRST + 6);
        public const int PGM_GETBORDER = (PGM_FIRST + 7);
        public const int PGM_SETPOS = (PGM_FIRST + 8);
        public const int PGM_GETPOS = (PGM_FIRST + 9);
        public const int PGM_SETBUTTONSIZE = (PGM_FIRST + 10);
        public const int PGM_GETBUTTONSIZE = (PGM_FIRST + 11);
        public const int PGM_GETBUTTONSTATE = (PGM_FIRST + 12);
        public const int PGM_GETDROPTARGET = CCM_GETDROPTARGET;
        public const int BCM_GETIDEALSIZE = (BCM_FIRST + 0x0001);
        public const int BCM_SETIMAGELIST = (BCM_FIRST + 0x0002);
        public const int BCM_GETIMAGELIST = (BCM_FIRST + 0x0003);
        public const int BCM_SETTEXTMARGIN = (BCM_FIRST + 0x0004);
        public const int BCM_GETTEXTMARGIN = (BCM_FIRST + 0x0005);
        public const int EM_SETCUEBANNER = (ECM_FIRST + 1);
        public const int EM_GETCUEBANNER = (ECM_FIRST + 2);
        public const int EM_SHOWBALLOONTIP = (ECM_FIRST + 3);
        public const int EM_HIDEBALLOONTIP = (ECM_FIRST + 4);
        public const int CB_SETMINVISIBLE = (CBM_FIRST + 1);
        public const int CB_GETMINVISIBLE = (CBM_FIRST + 2);
        public const int LM_HITTEST = (WM_USER + 0x300);
        public const int LM_GETIDEALHEIGHT = (WM_USER + 0x301);
        public const int LM_SETITEM = (WM_USER + 0x302);
        public const int LM_GETITEM = (WM_USER + 0x303);
        public const int RBBIM_STYLE = 1;
        public const int RBBIM_COLORS = 2;
        public const int RBBIM_TEXT = 4;
        public const int RBBIM_IMAGE = 8;
        public const int RBBIM_CHILD = 16;
        public const int RBBIM_CHILDSIZE = 32;
        public const int RBBIM_SIZE = 64;
        public const int RBBIM_BACKGROUND = 128;
        public const int RBBIM_ID = 256;
        public const int RBBIM_IDEALSIZE = 512;
        public const int RBBIM_LPARAM = 1024;
        public const int RBBIM_HEADERSIZE = 2048;
        public const int RBIM_IMAGELIST = 1;
        public const int RBBS_BREAK = 0x0001;
        public const int RBBS_FIXEDSIZE = 0x0002;
        public const int RBBS_CHILDEDGE = 0x0004;
        public const int RBBS_HIDDEN = 0x0008;
        public const int RBBS_NOVERT = 0x0010;
        public const int RBBS_FIXEDBMP = 0x0020;
        public const int RBBS_VARIABLEHEIGHT = 0x0040;
        public const int RBBS_GRIPPERALWAYS = 0x0080;
        public const int RBBS_NOGRIPPER = 0x0100;
        public const int RBBS_USECHEVRON = 0x0200;
        public const int RBBS_HIDETITLE = 0x0400;
        public const int RBBS_TOPALIGN = 0x0800;
        public const int ICC_LISTVIEW_CLASSES = 0x00000001; // listview; header
        public const int ICC_TREEVIEW_CLASSES = 0x00000002; // treeview; tooltips
        public const int ICC_BAR_CLASSES = 0x00000004; // toolbar; statusbar; trackbar; tooltips
        public const int ICC_TAB_CLASSES = 0x00000008; // tab; tooltips
        public const int ICC_UPDOWN_CLASS = 0x00000010; // updown
        public const int ICC_PROGRESS_CLASS = 0x00000020; // progress
        public const int ICC_HOTKEY_CLASS = 0x00000040; // hotkey
        public const int ICC_ANIMATE_CLASS = 0x00000080; // animate
        public const int ICC_WIN95_CLASSES = 0x000000FF;
        public const int ICC_DATE_CLASSES = 0x00000100; // month picker; date picker; time picker; updown
        public const int ICC_USEREX_CLASSES = 0x00000200; // comboex
        public const int ICC_COOL_CLASSES = 0x00000400; // rebar (coolbar) control
        public const int ICC_INTERNET_CLASSES = 0x00000800;
        public const int ICC_PAGESCROLLER_CLASS = 0x00001000; // page scroller
        public const int ICC_NATIVEFNTCTL_CLASS = 0x00002000; // native font control
        public const int ICC_STANDARD_CLASSES = 0x00004000;
        public const int ICC_LINK_CLASS = 0x00008000;
        public const int RBN_HEIGHTCHANGE = (RBN_FIRST - 0);
        public const int RBN_GETOBJECT = (RBN_FIRST - 1);
        public const int RBN_LAYOUTCHANGED = (RBN_FIRST - 2);
        public const int RBN_AUTOSIZE = (RBN_FIRST - 3);
        public const int RBN_BEGINDRAG = (RBN_FIRST - 4);
        public const int RBN_ENDDRAG = (RBN_FIRST - 5);
        public const int RBN_DELETINGBAND = (RBN_FIRST - 6);    // Uses NMREBAR
        public const int RBN_DELETEDBAND = (RBN_FIRST - 7);     // Uses NMREBAR
        public const int RBN_CHILDSIZE = (RBN_FIRST - 8);
        public const int RBN_CHEVRONPUSHED = (RBN_FIRST - 10);
        public const int RBN_MINMAX = (RBN_FIRST - 21);
        public const int RBN_AUTOBREAK = (RBN_FIRST - 22);
        public const DWORD TBIF_BYINDEX = 0x80000000U;
        public const DWORD TBIF_COMMAND = 32U;
        public const DWORD TBIF_IMAGE = 1U;
        public const DWORD TBIF_LPARAM = 16U;
        public const DWORD TBIF_SIZE = 64U;
        public const DWORD TBIF_STATE = 4U;
        public const DWORD TBIF_STYLE = 8U;
        public const DWORD TBIF_TEXT = 2U;
        #endregion // commctl.h

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon
        public const uint CLR_NONE = 0xFFFFFFFFU;
        public const uint CLR_DEFAULT = 0xFF000000U;

        #region kernel32.dll
        private const string kernel32 = "kernel32.dll";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpFileName">ファイル名(LPCTSTR)</param>
        /// <param name="dwDesiredAccess">アクセスモード(DWORD)</param>
        /// <param name="dwShareMode">共有モード(DWORD)</param>
        /// <param name="lpSecurityAttributes">セキュリティ記述子(LPSECURITY_ATTRIBUTES)</param>
        /// <param name="dwCreationDisposition">作成方法(DWORD)</param>
        /// <param name="dwFlagsAndAttributes">ファイル属性(DWORD)</param>
        /// <param name="hTemplateFile">テンプレートファイルのハンドル(HANDLE)</param>
        /// <returns></returns>
        [DllImport(kernel32)]
        public static extern HANDLE CreateFileW(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpFileName,
            DWORD dwDesiredAccess,
            DWORD dwShareMode,
            IntPtr lpSecurityAttributes,
            DWORD dwCreationDisposition,
            DWORD dwFlagsAndAttributes,
            HANDLE hTemplateFile
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpPathName">ディレクトリ名(LPCTSTR)</param>
        /// <param name="lpSecurityAttributes">セキュリティ識別子(LPSECURITY_ATTRIBUTES)</param>
        /// <returns></returns>
        [DllImport(kernel32)]
        public static extern bool CreateDirectoryW(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpPathName,
            IntPtr lpSecurityAttributes
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpFileName">ファイルまたはディレクトリの名前(LPCWSTR)</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern DWORD GetFileAttributesW(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpFileName
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpFileName">ファイル名(LPCTSTR)</param>
        /// <param name="nBufferLength">パス名を格納するバッファのサイズ(DWORD)</param>
        /// <param name="lpBuffer">パス名を格納するバッファ(LPTSTR)</param>
        /// <param name="lpFilePart">パス内のファイル名のアドレス(LPTSTR*)</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern DWORD GetFullPathNameW(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpFileName,
            DWORD nBufferLength,
            [MarshalAs(UnmanagedType.LPWStr)][Out]
            StringBuilder lpBuffer,
          [Out]
            IntPtr lpFilePart
      );

        [DllImport("kernel32.dll")]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            byte[] lpInBuffer,
            uint nInBufferSize,
            [Out] byte[] lpOutBuffer,
            uint nOutBufferSize,
            IntPtr lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetShortPathNameW(string longPath, StringBuilder shortPathBuffer, int bufferSize);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibraryExW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "WriteProfileStringA", ExactSpelling = true)]
        public static extern bool WriteProfileString(string section, string keyName, string value);

        [DllImport("kernel32.dll")]
        public static extern uint GetProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpCriticalSection">元はLPCRITICAL_SECTION</param>
        [DllImport("kernel32.dll")]
        public static extern void InitializeCriticalSection(ref IntPtr lpCriticalSection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpCriticalSection">元はLPCRITICAL_SECTION</param>
        [DllImport("kernel32.dll")]
        public static extern void DeleteCriticalSection(ref IntPtr lpCriticalSection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpCriticalSection">もとはLPCRITICAL_SECTION</param>
        [DllImport("kernel32.dll")]
        public static extern void LeaveCriticalSection(ref IntPtr lpCriticalSection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpCriticalSection">LPCRITICAL_SECTION</param>
        [DllImport("kernel32.dll")]
        public static extern void EnterCriticalSection(ref IntPtr lpCriticalSection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hModule">モジュールのハンドル(HANDLE)</param>
        /// <param name="lpFilename">モジュールのファイル名(LPTSTR)</param>
        /// <param name="nSize">バッファのサイズ</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern DWORD GetModuleFileName(
          IntPtr hModule,    // 
          IntPtr lpFilename,  // 
          DWORD nSize         // 
        );
        #endregion

        #region shell32.dll
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        #endregion

        #region user32.dll
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, [MarshalAs(UnmanagedType.FunctionPtr)]EnumChildProc lpEnumFunc, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="lpRect">長方形の座標(CONST RECT *lpRect)</param>
        /// <param name="bErase">消去するかどうかの状態</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
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

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool DispatchMessage(ref Message msg);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMessage(ref Message msg, int hWnd, uint wFilterMin, uint wFilterMax);

        //[DllImport("User32.dll", CharSet=CharSet.Auto)] 
        [DllImport("User32.dll", CharSet = CharSet.Auto,
             EntryPoint = "GetWindowLong", ExactSpelling = false)] //My Computer can't seem to find the entry point for GetWindowLongPtr
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int MessageBox(int h, string m, string c, int type);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(
            IntPtr hWnd,      // handle to window
            int X,          // horizontal position
            int Y,          // vertical position
            int nWidth,     // width
            int nHeight,    // height
            bool bRepaint   // repaint option
            );

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(ref Message msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool RedrawWindow(IntPtr hWnd, ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref REBARINFO lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref REBARBANDINFO lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref COLORSCHEME lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, COLORREF lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref RECT lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref MARGINS lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, int wParam, ref TBBUTTONINFO lParam);

        //[DllImport("User32.dll", CharSet=CharSet.Auto)]
        [DllImport("User32.dll", CharSet = CharSet.Auto,
             EntryPoint = "SetWindowLong", ExactSpelling = false)] //My Computer can't seem to find the entry point for SetWindowLongPtr
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool TranslateMessage(ref Message msg);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool WaitMessage();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IntersectRect(ref RECT lprcDst, ref RECT lprcSrc1, ref RECT lprcSrc2);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EqualRect(ref RECT lprc1, ref RECT lprc2);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, ref RECT prcRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AppendMenu(IntPtr hMenu, uint uFlags, uint uIDNewItem, string lpNewItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="lpPoint">クライアント座標</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateMenu();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="hMenu">メニューのハンドル</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetMenu(IntPtr hWnd, IntPtr hMenu);
        #endregion

        #region gdi32.dll
        [DllImport("GDI32.dll")]
        public static extern bool DeleteObject(
            IntPtr hObject   // handle to graphic object
            );
        #endregion

        #region comctl32.dll
        [DllImport("ComCtl32.dll")]
        public static extern IntPtr ImageList_Create(int cx, int cy, uint flags, int cInitial, int cGrow);

        [DllImport("ComCtl32.dll")]
        public static extern bool ImageList_Destroy(IntPtr himl);

        [DllImport("ComCtl32.dll")]
        public static extern int ImageList_GetImageCount(IntPtr himl);

        [DllImport("ComCtl32.dll")]
        public static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX ComCtls);
        #endregion

        public static int MAKELONG(int a, int b)
        {
            return 0xffff & a | ((0xffff & b) << 16);
        }
    }

    public delegate bool EnumChildProc(IntPtr hwnd, int lParam);

    #region windef.h
    public struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override string ToString()
        {
            return "{left=" + left + ", top=" + top + ", right=" + right + ", bottom=" + bottom + "}";
        }
    }

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
    public struct POINT
    {
        public int x;
        public int y;
    }
    #endregion

    #region winnt.h
    public struct IMAGE_IMPORT_DESCRIPTOR
    {
        DWORD union;

        public DWORD Characteristics
        {
            get
            {
                return union;
            }
            set
            {
                union = value;
            }
        }

        public DWORD OriginalFirstThunk
        {
            get
            {
                return union;
            }
            set
            {
                union = value;
            }
        }

        public DWORD TimeDateStamp;
        public DWORD ForwarderChain;
        public DWORD Name;
        public DWORD FirstThunk;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct LIST_ENTRY
    {
        public LIST_ENTRY* Flink;
        public LIST_ENTRY* Blink;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_FILE_HEADER
    {
        public WORD Machine;
        public WORD NumberOfSections;
        public DWORD TimeDateStamp;
        public DWORD PointerToSymbolTable;
        public DWORD NumberOfSymbols;
        public WORD SizeOfOptionalHeader;
        public WORD Characteristics;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DATA_DIRECTORY
    {
        public DWORD VirtualAddress;
        public DWORD Size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_EXPORT_DIRECTORY
    {
        public DWORD Characteristics;
        public DWORD TimeDateStamp;
        public WORD MajorVersion;
        public WORD MinorVersion;
        public DWORD Name;
        public DWORD Base;
        public DWORD NumberOfFunctions;
        public DWORD NumberOfNames;
        public DWORD AddressOfFunctions;
        public DWORD AddressOfNames;
        public DWORD AddressOfNameOrdinals;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_DOS_HEADER
    {
        public WORD e_magic;
        public WORD e_cblp;
        public WORD e_cp;
        public WORD e_crlc;
        public WORD e_cparhdr;
        public WORD e_minalloc;
        public WORD e_maxalloc;
        public WORD e_ss;
        public WORD e_sp;
        public WORD e_csum;
        public WORD e_ip;
        public WORD e_cs;
        public WORD e_lfarlc;
        public WORD e_ovno;
        public fixed WORD e_res[4];
        public WORD e_oemid;
        public WORD e_oeminfo;
        public fixed WORD e_res2[10];
        public LONG e_lfanew;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_OPTIONAL_HEADER
    {
        public WORD Magic;
        public BYTE MajorLinkerVersion;
        public BYTE MinorLinkerVersion;
        public DWORD SizeOfCode;
        public DWORD SizeOfInitializedData;
        public DWORD SizeOfUninitializedData;
        public DWORD AddressOfEntryPoint;
        public DWORD BaseOfCode;
        public DWORD BaseOfData;
        public DWORD ImageBase;
        public DWORD SectionAlignment;
        public DWORD FileAlignment;
        public WORD MajorOperatingSystemVersion;
        public WORD MinorOperatingSystemVersion;
        public WORD MajorImageVersion;
        public WORD MinorImageVersion;
        public WORD MajorSubsystemVersion;
        public WORD MinorSubsystemVersion;
        public DWORD Win32VersionValue;
        public DWORD SizeOfImage;
        public DWORD SizeOfHeaders;
        public DWORD CheckSum;
        public WORD Subsystem;
        public WORD DllCharacteristics;
        public DWORD SizeOfStackReserve;
        public DWORD SizeOfStackCommit;
        public DWORD SizeOfHeapReserve;
        public DWORD SizeOfHeapCommit;
        public DWORD LoaderFlags;
        public DWORD NumberOfRvaAndSizes;
        private IMAGE_DATA_DIRECTORY dataDirectory00;
        private IMAGE_DATA_DIRECTORY dataDirectory01;
        private IMAGE_DATA_DIRECTORY dataDirectory02;
        private IMAGE_DATA_DIRECTORY dataDirectory03;
        private IMAGE_DATA_DIRECTORY dataDirectory04;
        private IMAGE_DATA_DIRECTORY dataDirectory05;
        private IMAGE_DATA_DIRECTORY dataDirectory06;
        private IMAGE_DATA_DIRECTORY dataDirectory07;
        private IMAGE_DATA_DIRECTORY dataDirectory08;
        private IMAGE_DATA_DIRECTORY dataDirectory09;
        private IMAGE_DATA_DIRECTORY dataDirectory10;
        private IMAGE_DATA_DIRECTORY dataDirectory11;
        private IMAGE_DATA_DIRECTORY dataDirectory12;
        private IMAGE_DATA_DIRECTORY dataDirectory13;
        private IMAGE_DATA_DIRECTORY dataDirectory14;
        private IMAGE_DATA_DIRECTORY dataDirectory15;
        public IMAGE_DATA_DIRECTORY getDataDirectory(int index)
        {
            switch (index) {
                case 0:
                return dataDirectory00;
                case 1:
                return dataDirectory01;
                case 2:
                return dataDirectory02;
                case 3:
                return dataDirectory03;
                case 4:
                return dataDirectory04;
                case 5:
                return dataDirectory05;
                case 6:
                return dataDirectory06;
                case 7:
                return dataDirectory07;
                case 8:
                return dataDirectory08;
                case 9:
                return dataDirectory09;
                case 10:
                return dataDirectory10;
                case 11:
                return dataDirectory11;
                case 12:
                return dataDirectory12;
                case 13:
                return dataDirectory13;
                case 14:
                return dataDirectory14;
                case 15:
                return dataDirectory15;
                default:
                return new IMAGE_DATA_DIRECTORY();
            }
        }
    }
    #endregion

    #region winbase
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CRITICAL_SECTION_DEBUG
    {
        WORD Type;
        WORD CreatorBackTraceIndex;
        CRITICAL_SECTION* CriticalSection;
        LIST_ENTRY ProcessLocksList;
        DWORD EntryCount;
        DWORD ContentionCount;
        fixed DWORD Spare[2];
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CRITICAL_SECTION
    {
        CRITICAL_SECTION_DEBUG* DebugInfo;
        LONG LockCount;
        LONG RecursionCount;
        HANDLE OwningThread;
        HANDLE LockSemaphore;
        DWORD SpinCount;
    }
    #endregion

    #region uxtheme.h
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

        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    #endregion

    #region commctrl.h
    [StructLayout(LayoutKind.Sequential)]
    public struct REBARINFO
    {
        public uint cbSize;
        public uint fMask;
        public IntPtr himl;
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
    public struct NMREBARCHEVRON
    {
        public NMHDR hdr;
        public int uBand;
        public int wID;
        public int lParam;
        public RECT rc;
        public int lParamNM;
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
    public struct NMMOUSE
    {
        public NMHDR hdr;
        public IntPtr dwItemSpec;
        public IntPtr dwItemData;
        public POINT pt;
        public IntPtr dwHitInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TBBUTTONINFO
    {
        public uint cbSize;
        public DWORD dwMask;
        public int idCommand;
        public int iImage;
        public BYTE fsState;
        public BYTE fsStyle;
        public WORD cx;
        /// <summary>
        /// DWORD_PTR
        /// </summary>
        public IntPtr lParam;
        public string pszText;
        public int cchText;
    }
    #endregion

    #region winuser.h
    [StructLayout(LayoutKind.Sequential)]
    public struct TRACKMOUSEEVENT
    {
        public int cbSize;
        public uint dwFlags;
        public IntPtr hwndTrack;
        public int dwHoverTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public uint idFrom;
        public int code;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MENUITEMINFO
    {
        public uint cbSize;
        public uint fMask;
        public uint fType;
        public uint fState;
        public uint wID;
        /// <summary>
        /// HMENU
        /// </summary>
        public IntPtr hSubMenu;
        /// <summary>
        /// HBITMAP
        /// </summary>
        public IntPtr hbmpChecked;
        /// <summary>
        /// HBITMAP
        /// </summary>
        public IntPtr hbmpUnchecked;
        /// <summary>
        /// ULONG_PTR
        /// </summary>
        public DWORD dwItemData;
        public string dwTypeData;
        public uint cch;
        /// <summary>
        /// HBITMAP
        /// </summary>
        public IntPtr hbmpItem;
    }
    #endregion

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }
}
