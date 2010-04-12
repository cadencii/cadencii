#if !JAVA
/*
 * windows.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace org.kbinani {
    using WORD = System.UInt16;
    using DWORD = System.UInt32;
    using LONG = System.Int32;
    using BYTE = System.Byte;
    using HANDLE = System.IntPtr;

    public static partial class win32 {
        #region winbase.h
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
        [DllImport( "Advapi32.dll" )]
        public static unsafe extern int RegOpenKeyExW(
            uint hKey,
            [MarshalAs( UnmanagedType.LPWStr )] string pSubKey,
            uint ulOptions,
            uint samDesired,
            uint* phkResult );

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
        [DllImport( "Advapi32.dll" )]
        public static unsafe extern int RegEnumKeyExW(
            uint hKey,
            uint dwIndex,
            [MarshalAs( UnmanagedType.LPWStr )] string pName,
            uint* pcbName,
            uint* pReserved,
            [MarshalAs( UnmanagedType.LPWStr )] string pClass,
            uint* pcbClass,
            FILETIME* pftLastWrite );

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
        [DllImport( "Advapi32.dll" )]
        public static unsafe extern int RegQueryValueExW(
            uint hKey,
            [MarshalAs( UnmanagedType.LPWStr )] string pValueName,
            uint* pReserved,
            uint* pType,
            byte* pData,
            uint* pcbData );


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hKey">キーのハンドル</param>
        /// <returns></returns>
        [DllImport( "Advapi32.dll" )]
        public static extern int RegCloseKey( uint hKey );
        #endregion

        #region winnt.h
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
        #endregion

        #region windef.h
        public const int MAX_PATH = 260;
        #endregion

        #region winuser.h
        public const int WM_MOUSEMOVE = 512;
        public const int WM_LBUTTONDOWN = 513;
        public const int WM_LBUTTONUP = 514;
        public const int WM_LBUTTONDBLCLK = 515;
        public const int WM_RBUTTONDOWN = 516;
        public const int WM_RBUTTONUP = 517;
        public const int WM_RBUTTONDBLCLK = 518;
        public const int WM_MBUTTONDOWN = 519;
        public const int WM_MBUTTONUP = 520;
        public const int WM_MBUTTONDBLCLK = 521;
        public const int WM_MOUSEWHEEL = 522;
        #endregion

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


        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        #region kernel32.dll
        [DllImport( "kernel32.dll" )]
        public static extern IntPtr LoadLibraryExW( [MarshalAs( UnmanagedType.LPWStr )]string lpFileName, IntPtr hFile, uint dwFlags );

        [DllImport( "kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true )]
        public static extern IntPtr GetProcAddress( IntPtr hModule, string lpProcName );

        [DllImport( "kernel32.dll" )]
        public static extern bool FreeLibrary( IntPtr hModule );

        [DllImport( "kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "WriteProfileStringA", ExactSpelling = true )]
        public static extern bool WriteProfileString( string section, string keyName, string value );

        [DllImport( "kernel32.dll" )]
        public static extern uint GetProfileString( string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpCriticalSection">元はLPCRITICAL_SECTION</param>
        [DllImport( "kernel32.dll" )]
        public static extern void InitializeCriticalSection( ref IntPtr lpCriticalSection );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpCriticalSection">元はLPCRITICAL_SECTION</param>
        [DllImport( "kernel32.dll" )]
        public static extern void DeleteCriticalSection( ref IntPtr lpCriticalSection );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpCriticalSection">もとはLPCRITICAL_SECTION</param>
        [DllImport( "kernel32.dll" )]
        public static extern void LeaveCriticalSection( ref IntPtr lpCriticalSection );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpCriticalSection">LPCRITICAL_SECTION</param>
        [DllImport( "kernel32.dll" )]
        public static extern void EnterCriticalSection( ref IntPtr lpCriticalSection );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hModule">モジュールのハンドル(HANDLE)</param>
        /// <param name="lpFilename">モジュールのファイル名(LPTSTR)</param>
        /// <param name="nSize">バッファのサイズ</param>
        /// <returns></returns>
        [DllImport( "kernel32.dll" )]
        public static extern DWORD GetModuleFileName(
          IntPtr hModule,    // 
          IntPtr lpFilename,  // 
          DWORD nSize         // 
        );
        #endregion

        #region shell32.dll
        [DllImport( "shell32.dll" )]
        public static extern IntPtr SHGetFileInfo( string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags );
        #endregion

        #region user32.dll
        [DllImport( "user32.dll" )]
        public static extern bool GetWindowRect( IntPtr hWnd, ref RECT lpRect );

        [DllImport( "user32.dll" )]
        public static extern bool GetClientRect( IntPtr hWnd, ref RECT lpRect );

        [DllImport( "user32.dll" )]
        public static extern bool EnumChildWindows( IntPtr hWndParent, [MarshalAs( UnmanagedType.FunctionPtr )]EnumChildProc lpEnumFunc, int lParam );


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="lpRect">長方形の座標(CONST RECT *lpRect)</param>
        /// <param name="bErase">消去するかどうかの状態</param>
        /// <returns></returns>
        [DllImport( "user32.dll" )]
        public static extern bool InvalidateRect( IntPtr hWnd, IntPtr lpRect, bool bErase );
        #endregion

        public static int MAKELONG( int a, int b ) {
            return 0xffff & a | ((0xffff & b) << 16);
        }
    }

    public delegate bool EnumChildProc( IntPtr hwnd, int lParam );

    #region windef.h
    public struct FILETIME {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [StructLayout( LayoutKind.Sequential, Pack = 1 )]
    public struct RECT {
        public int left;
        public int top;
	    public int right;
	    public int bottom;
    }
    #endregion

    #region winnt.h
    public struct IMAGE_IMPORT_DESCRIPTOR {
        DWORD union;

        public DWORD Characteristics {
            get {
                return union;
            }
            set {
                union = value;
            }
        }

        public DWORD OriginalFirstThunk {
            get {
                return union;
            }
            set {
                union = value;
            }
        }

        public DWORD TimeDateStamp;
        public DWORD ForwarderChain;
        public DWORD Name;
        public DWORD FirstThunk;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct LIST_ENTRY {
        public LIST_ENTRY* Flink;
        public LIST_ENTRY* Blink;
    }

    [StructLayout( LayoutKind.Sequential)]
    public struct IMAGE_FILE_HEADER {
        public WORD Machine;
        public WORD NumberOfSections;
        public DWORD TimeDateStamp;
        public DWORD PointerToSymbolTable;
        public DWORD NumberOfSymbols;
        public WORD SizeOfOptionalHeader;
        public WORD Characteristics;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DATA_DIRECTORY {
    	public DWORD VirtualAddress;
	    public DWORD Size;
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct IMAGE_EXPORT_DIRECTORY {
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

    [StructLayout( LayoutKind.Sequential)]
    public unsafe struct IMAGE_DOS_HEADER {
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

    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct IMAGE_OPTIONAL_HEADER {
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
        public IMAGE_DATA_DIRECTORY getDataDirectory( int index ) {
            switch ( index ) {
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
    public unsafe struct CRITICAL_SECTION_DEBUG {
        WORD Type;
        WORD CreatorBackTraceIndex;
        CRITICAL_SECTION* CriticalSection;
        LIST_ENTRY ProcessLocksList;
        DWORD EntryCount;
        DWORD ContentionCount;
        fixed DWORD Spare[2];
    }

    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct CRITICAL_SECTION {
        CRITICAL_SECTION_DEBUG* DebugInfo;
        LONG LockCount;
        LONG RecursionCount;
        HANDLE OwningThread;
        HANDLE LockSemaphore;
        DWORD SpinCount;
    }
    #endregion

    [StructLayout( LayoutKind.Sequential )]
    public struct SHFILEINFO {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
        public string szDisplayName;
        [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]
        public string szTypeName;
    }
}
#endif
