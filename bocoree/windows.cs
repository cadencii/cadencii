#if !JAVA
/*
 * windows.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;

namespace bocoree {

    public static partial class windows {
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

        [DllImport( "kernel32.dll" )]
        public static extern IntPtr LoadLibraryExW( [MarshalAs( UnmanagedType.LPWStr )]string lpFileName, IntPtr hFile, uint dwFlags );

        [DllImport( "kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true )]
        public static extern IntPtr GetProcAddress( IntPtr hModule, string lpProcName );

        [DllImport( "kernel32.dll" )]
        public static extern bool FreeLibrary( IntPtr hModule );
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

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        [DllImport( "shell32.dll" )]
        public static extern IntPtr SHGetFileInfo( string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags );

    }

    #region windef.h
    public struct FILETIME {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
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
