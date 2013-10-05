/*
 * Util.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using cadencii;

namespace cadencii.apputil
{
    using java = cadencii.java;
    using WORD = System.UInt16;
    using DWORD = System.UInt32;
    using LONG = System.Int32;
    using BYTE = System.Byte;
    using HANDLE = System.IntPtr;
    using WCHAR = Char;
    using ULONG = System.UInt32;

    public static partial class Util
    {
        public static readonly string PANGRAM = "cozy lummox gives smart squid who asks for job pen. 01234567890 THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS.";
        /// <summary>
        /// このクラスのメソッド'applyFontRecurse', 'applyToolStripFontRecurse', 'applyContextMenuFontRecurse'の呼び出しを有効とするかどうか。
        /// デフォルトではtrue
        /// </summary>
        public static bool isApplyFontRecurseEnabled = true;

#if FOOOOOOOOOOOOOO
        [StructLayout( LayoutKind.Explicit )]
        struct REPARSE_DATA_BUFFER
        {
            [FieldOffset( 0 )]
            public DWORD ReparseTag;
            [FieldOffset( 4 )]
            public WORD ReparseDataLength;
            [FieldOffset( 6 )]
            public WORD Reserved;
            [FieldOffset( 8 )]
            public REPARSE_DATA_BUFFER_SymbolicLinkReparseBuffer SymbolicLinkReparseBuffer;
            [FieldOffset( 8 )]
            public REPARSE_DATA_BUFFER_MountPointReparseBuffer MountPointReparseBuffer;
            [FieldOffset( 8 )]
            public REPARSE_DATA_BUFFER_GenericReparseBuffer GenericReparseBuffer;
        }

        unsafe struct REPARSE_DATA_BUFFER_SymbolicLinkReparseBuffer
        {
            public WORD SubstituteNameOffset;
            public WORD SubstituteNameLength;
            public WORD PrintNameOffset;
            public WORD PrintNameLength;
            public ULONG Flags; /* 0=絶対パス, 1=相対パス */
            public WCHAR* PathBuffer;
        }

        unsafe struct REPARSE_DATA_BUFFER_MountPointReparseBuffer
        {
            public WORD SubstituteNameOffset;
            public WORD SubstituteNameLength;
            public WORD PrintNameOffset;
            public WORD PrintNameLength;
            public WCHAR* PathBuffer;
        }

        unsafe struct REPARSE_DATA_BUFFER_GenericReparseBuffer
        {
            public BYTE* DataBuffer;
        }

        [StructLayout( LayoutKind.Explicit )]
        unsafe struct REPARSE_DATA_BUFFER_Internal
        {
            [FieldOffset( 0 )]
            public REPARSE_DATA_BUFFER iobuf;
            [FieldOffset( 0 )]
            public fixed WCHAR dummy[16384/*MAXIMUM_REPARSE_DATA_BUFFER_SIZE*/];
        }

        public static bool createJunction( String mount_point_path, String target )
        {
            unsafe {
                /*union
                {
                    REPARSE_DATA_BUFFER iobuf;
                    TCHAR dummy[MAXIMUM_REPARSE_DATA_BUFFER_SIZE];
                } u;*/

                WCHAR* lpLinkName;
                WCHAR* lpTargetName;

                //lpLinkName = (char *)Marshal.StringToHGlobalUni( mount_point_path ).ToPointer();
                //lpTargetName = (char *)Marshal.StringToHGlobalUni( target ).ToPointer();

                HANDLE hFile;
                //WCHAR *namebuf = (WCHAR *)Marshal.AllocHGlobal( sizeof( WCHAR ) * MAX_PATH + 4 ).ToPointer();
                DWORD cb;
                DWORD attr;
                bool isDirectory;

                attr = win32.GetFileAttributesW( target );
                if ( win32.INVALID_FILE_ATTRIBUTES == attr ) {
                    return false;
                }
                isDirectory = (attr & win32.FILE_ATTRIBUTE_DIRECTORY) == win32.FILE_ATTRIBUTE_DIRECTORY;

                //
                // リンク先をフルパスにする
                //
                //IntPtr ptr_pre_namebuf = Marshal.StringToHGlobalUni( "\\??\\" ); //wcscpy( namebuf, L"\\?\?\\" );
                //WCHAR *namebuf = (WCHAR *)Marshal.ReAllocHGlobal( ptr_namebuf, new IntPtr( sizeof( WCHAR ) * (win32.MAX_PATH + 4) ) ).ToPointer();
                //int ptr_pre_namebuf_size = sizeof( WCHAR ) * (win32.MAX_PATH + 4);
                //IntPtr ptr_namebuf = Marshal.AllocHGlobal( ptr_namebuf_size );
                StringBuilder namebuf = new StringBuilder( win32.MAX_PATH + 4 );
                if ( win32.GetFullPathNameW( target, sizeof( WCHAR ) * (win32.MAX_PATH ), namebuf, IntPtr.Zero ) == 0 ) {
                    return false;
                }
                namebuf.Insert( 0, "\\??\\" );

                // わけがわからないよ！
                /*if ( !lstrcpyn( u.iobuf.MountPointReparseBuffer.PathBuffer, namebuf, MAXIMUM_REPARSE_DATA_BUFFER_SIZE ) ) {
                    return false;
                }*/
                // 最初の8は，REPARSE_DATA_BUFFERのunionの直前の部分のサイズ
                // 最後の2*3は，2バイトの間隙を3箇所設定するから（仮）
                IntPtr ptr_u = Marshal.AllocHGlobal( 8 + sizeof( WCHAR ) * (namebuf.Length + 1) + 2 * 3);
                REPARSE_DATA_BUFFER_Internal u = (REPARSE_DATA_BUFFER_Internal)Marshal.PtrToStructure( ptr_u, typeof( REPARSE_DATA_BUFFER_Internal ) );
                int imax = sizeof( WCHAR ) * (win32.MAX_PATH + 4);
                IntPtr ptr_namebuf = Marshal.StringToHGlobalUni( namebuf.ToString() );
                for ( int i = 0; i < imax; i++ ) {
                    // 8+8の意味は，先頭からMountPointReparseBufferまでに8バイト，PathBufferまでに8バイトあるから
                    Marshal.WriteByte( ptr_u, 8 + 8 + i, 0 );
                }
                for ( int i = 0; i < namebuf.Length; i++ ) {
                    Marshal.WriteInt16( ptr_u, 8 + 8 + i, namebuf[i] );
                }
                Marshal.WriteInt16( ptr_u, 8 + 8 + imax - 1, 0 );
                Marshal.FreeHGlobal( ptr_namebuf );

                //
                // リンクファイルを作成
                //
                if ( isDirectory ) {
                    if ( !win32.CreateDirectoryW( mount_point_path, IntPtr.Zero ) ) {
                        return false;
                    }
                    hFile =
                        win32.CreateFileW(
                            mount_point_path,
                            win32.GENERIC_WRITE,
                            win32.FILE_SHARE_READ | win32.FILE_SHARE_WRITE,
                            IntPtr.Zero,
                            win32.OPEN_EXISTING,
                            win32.FILE_FLAG_BACKUP_SEMANTICS,
                            IntPtr.Zero );
                } else {
                    hFile =
                        win32.CreateFileW(
                            mount_point_path,
                            win32.GENERIC_WRITE,
                            win32.FILE_SHARE_READ | win32.FILE_SHARE_WRITE,
                            IntPtr.Zero,
                            win32.CREATE_NEW,
                            0,
                            IntPtr.Zero );
                }
                if ( win32.INVALID_HANDLE_VALUE == hFile ) {
                    return false;
                }

                //
                // リパースデータを設定
                //
                u.iobuf.ReparseTag = win32.IO_REPARSE_TAG_MOUNT_POINT;
                u.iobuf.Reserved = 0;
                u.iobuf.MountPointReparseBuffer.SubstituteNameOffset = 0;
                u.iobuf.MountPointReparseBuffer.SubstituteNameLength = wcslen( u.iobuf.MountPointReparseBuffer.PathBuffer ) * 2;
                u.iobuf.MountPointReparseBuffer.PrintNameOffset = u.iobuf.MountPointReparseBuffer.SubstituteNameLength + 2;
                u.iobuf.MountPointReparseBuffer.PrintNameLength = 0;
                memset(
                    (char*)u.iobuf.MountPointReparseBuffer.PathBuffer + u.iobuf.MountPointReparseBuffer.SubstituteNameLength,
                    0,
                    4 );
                u.iobuf.ReparseDataLength =
                    8 +
                    u.iobuf.MountPointReparseBuffer.PrintNameOffset +
                    u.iobuf.MountPointReparseBuffer.PrintNameLength + 2;
                cb = 8 + u.iobuf.ReparseDataLength;
                if ( !win32.DeviceIoControl( hFile, win32.FSCTL_SET_REPARSE_POINT,
                                &u.iobuf, cb, NULL, 0, &cb, NULL ) ) {
                    win32.CloseHandle( hFile );
                    if ( isDirectory ) {
                        win32.RemoveDirectory( lpLinkName );
                    } else {
                        win32.DeleteFile( lpLinkName );
                    }
                    //deletefunc(lpLinkName);
                    return false;
                }

                win32.CloseHandle( hFile );
                return true;
            }
        }
#endif

        public static void applyContextMenuFontRecurse(ContextMenuStrip item, cadencii.java.awt.Font font)
        {
            if (!isApplyFontRecurseEnabled) {
                return;
            }
            item.Font = font.font;
            foreach (ToolStripItem tsi in item.Items) {
                applyToolStripFontRecurse(tsi, font);
            }
        }

        public static void applyToolStripFontRecurse(ToolStripItem item, cadencii.java.awt.Font font)
        {
            if (!isApplyFontRecurseEnabled) {
                return;
            }
            item.Font = font.font;
            if (item is ToolStripMenuItem) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                foreach (ToolStripItem tsi in tsmi.DropDownItems) {
                    applyToolStripFontRecurse(tsi, font);
                }
            } else if (item is ToolStripDropDownItem) {
                ToolStripDropDownItem tsdd = (ToolStripDropDownItem)item;
                foreach (ToolStripItem tsi in tsdd.DropDownItems) {
                    applyToolStripFontRecurse(tsi, font);
                }
            }
        }

        /// <summary>
        /// 指定したフォントを描画するとき、描画指定したy座標と、描かれる文字の中心線のズレを調べます
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static int getStringDrawOffset(java.awt.Font font)
        {
            int ret = 0;
            java.awt.Dimension size = measureString(PANGRAM, font);
            if (size.height <= 0) {
                return 0;
            }
            java.awt.Image b = null;
            java.awt.Graphics2D g = null;
            BitmapEx b2 = null;
            try {
                int string_desty = size.height * 2; // 文字列が書き込まれるy座標
                int w = size.width * 4;
                int h = size.height * 4;
                b = new java.awt.Image();
                b.image = new System.Drawing.Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                g = new java.awt.Graphics2D(System.Drawing.Graphics.FromImage(b.image));
                g.setColor(java.awt.Color.white);
                g.fillRect(0, 0, w, h);
                g.setFont(font);
                g.setColor(java.awt.Color.black);
                g.drawString(PANGRAM, size.width, string_desty);

                b2 = new BitmapEx(b.image);
                // 上端に最初に現れる色つきピクセルを探す
                int firsty = 0;
                bool found = false;
                for (int y = 0; y < h; y++) {
                    for (int x = 0; x < w; x++) {
                        java.awt.Color c = new cadencii.java.awt.Color(b2.GetPixel(x, y));
                        if (c.getRed() != 255 || c.getGreen() != 255 || c.getBlue() != 255) {
                            found = true;
                            firsty = y;
                            break;
                        }
                    }
                    if (found) {
                        break;
                    }
                }

                // 下端
                int endy = h - 1;
                found = false;
                for (int y = h - 1; y >= 0; y--) {
                    for (int x = 0; x < w; x++) {
                        java.awt.Color c = new cadencii.java.awt.Color(b2.GetPixel(x, y));
                        if (c.getRed() != 255 || c.getGreen() != 255 || c.getBlue() != 255) {
                            found = true;
                            endy = y;
                            break;
                        }
                    }
                    if (found) {
                        break;
                    }
                }

                int center = (firsty + endy) / 2;
                ret = center - string_desty;
            } catch (Exception ex) {
                serr.println("Util#getStringDrawOffset; ex=" + ex);
            } finally {
                if (b != null && b.image != null) {
                    b.image.Dispose();
                }
                if (g != null) {
                    g.nativeGraphics.Dispose();
                }
                if (b2 != null && b2 != null) {
                    b2.Dispose();
                }
            }
            return ret;
        }

        /// <summary>
        /// 指定した言語コードの表す言語が、右から左へ記述する言語かどうかを調べます
        /// </summary>
        /// <param name="language_code"></param>
        /// <returns></returns>
        public static bool isRightToLeftLanguage(string language_code)
        {
            language_code = language_code.ToLower();
            if (language_code.Equals("ar") ||
                 language_code.Equals("he") ||
                 language_code.Equals("iw") ||
                 language_code.Equals("fa") ||
                 language_code.Equals("ur")) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// 指定したディレクトリに作成可能な、一時ファイル名を取得します
        /// </summary>
        /// <param name="directory">ディレクトリ</param>
        /// <returns></returns>
        public static string GetTempFileNameIn(string directory)
        {
            for (uint i = uint.MinValue; i <= uint.MaxValue; i++) {
                string file = Path.Combine(directory, "temp" + i);
                if (!File.Exists(file)) {
                    return file;
                }
            }
            return "";
        }

        /// <summary>
        /// 指定したディレクトリに作成可能な、一時ファイル名を取得します
        /// </summary>
        /// <param name="directory">ディレクトリ</param>
        /// <param name="extention">拡張子（ex. ".txt"）</param>
        /// <returns></returns>
        public static string GetTempFileNameIn(string directory, string extention)
        {
            for (uint i = uint.MinValue; i <= uint.MaxValue; i++) {
                string file = Path.Combine(directory, "temp" + i + extention);
                if (!File.Exists(file)) {
                    return file;
                }
            }
            return "";
        }

        /// <summary>
        /// 指定した画像ファイルから新しいBitmapオブジェクトを作成します
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Bitmap BitmapFromStream(string file)
        {
            if (!File.Exists(file)) {
                return null;
            }
            FileStream fs = new FileStream(file, FileMode.Open);
            Bitmap ret = new Bitmap(fs);
            fs.Close();
            return ret;
        }

        /// <summary>
        /// 指定した画像ファイルから新しいImageオブジェクトを作成します
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Image ImageFromStream(string file)
        {
            if (!File.Exists(file)) {
                return null;
            }
            FileStream fs = new FileStream(file, FileMode.Open);
            Image ret = Image.FromStream(fs);
            fs.Close();
            return ret;
        }

        /// <summary>
        /// ImageFormatから，デフォルトの拡張子を取得します
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetExtensionFromImageFormat(ImageFormat format)
        {
            switch (format.ToString().ToLower()) {
                case "bmp":
                return ".bmp";
                case "emf":
                return ".emf";
                case "gif":
                return ".gif";
                case "jpeg":
                return ".jpg";
                case "png":
                return ".png";
                case "tiff":
                return ".tiff";
                case "wmf":
                return ".wmf";
                default:
                return "";

            }
        }

        /// <summary>
        /// System.Drawimg.Imaging.ImageFormatで使用可能なフォーマットの一覧を取得します
        /// </summary>
        /// <returns></returns>
        public static ImageFormat[] GetImageFormats()
        {
#if DEBUG
            Console.WriteLine("GetImageFormats()");
#endif
            PropertyInfo[] properties = typeof(System.Drawing.Imaging.ImageFormat).GetProperties();
            List<ImageFormat> ret = new List<ImageFormat>();
            foreach (PropertyInfo pi in properties) {
                if (pi.PropertyType.Equals(typeof(System.Drawing.Imaging.ImageFormat))) {
                    ImageFormat ifmt = (System.Drawing.Imaging.ImageFormat)pi.GetValue(null, null);
#if DEBUG
                    Console.WriteLine(ifmt.ToString());
#endif
                    ret.Add(ifmt);
                }
            }
            return ret.ToArray();
        }

        public static void RgbToHsv(int r, int g, int b, out double h, out double s, out double v)
        {
            RgbToHsv(r / 255.0, g / 255.0, b / 255.0, out h, out s, out v);
        }

        public static void RgbToHsv(double r, double g, double b, out double h, out double s, out double v)
        {
            double tmph, imax, imin;
            const double sqrt3 = 1.7320508075688772935274463415059;
            imax = Math.Max(r, Math.Max(g, b));
            imin = Math.Min(r, Math.Min(g, b));
            if (imax == 0.0) {
                h = 0;
                s = 0;
                v = 0;
                return;
            } else if (imax == imin) {
                tmph = 0;
            } else {
                if (r == imax) {
                    tmph = 60.0 * (g - b) / (imax - imin);
                } else if (g == imax) {
                    tmph = 60.0 * (b - r) / (imax - imin) + 120.0;
                } else {
                    tmph = 60.0 * (r - g) / (imax - imin) + 240.0;
                }
            }
            while (tmph < 0.0) {
                tmph = tmph + 360.0;
            }
            while (tmph >= 360.0) {
                tmph = tmph - 360.0;
            }
            h = tmph / 360.0;
            s = (imax - imin) / imax;
            v = imax;
        }

        public static Color HsvToColor(double h, double s, double v)
        {
            double dr, dg, db;
            HsvToRgb(h, s, v, out dr, out dg, out db);
            return Color.FromArgb((int)(dr * 255), (int)(dg * 255), (int)(db * 255));
        }

        public static void HsvToRgb(double h, double s, double v, out byte r, out byte g, out byte b)
        {
            double dr, dg, db;
            HsvToRgb(h, s, v, out dr, out dg, out db);
            r = (byte)(dr * 255);
            g = (byte)(dg * 255);
            b = (byte)(db * 255);
        }

        public static void HsvToRgb(double h, double s, double v, out double r, out double g, out double b)
        {
            double f, p, q, t, hh;
            int hi;
            r = g = b = 0.0;
            if (s == 0) {
                r = v;
                g = v;
                b = v;
            } else {
                hh = h * 360.0;
                hi = (int)(hh / 60.0) % 6;
                f = hh / 60.0 - (double)(hi);
                p = v * (1.0 - s);
                q = v * (1.0 - f * s);
                t = v * (1.0 - (1.0 - f) * s);
                switch (hi) {
                    case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                    case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                    case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                    case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                    case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                    case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
                }
            }
        }

        /// <summary>
        /// 指定された文字列を指定されたフォントで描画したときのサイズを計測します。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static java.awt.Dimension measureString(string text, java.awt.Font font)
        {
            using (Bitmap dumy = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(dumy)) {
                SizeF tmp = g.MeasureString(text, font.font);
                return new java.awt.Dimension((int)tmp.Width, (int)tmp.Height);
            }
        }

        public static java.awt.Dimension measureString(string text, Font font)
        {
            using (Bitmap dumy = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(dumy)) {
                SizeF tmp = g.MeasureString(text, font);
                return new java.awt.Dimension((int)tmp.Width, (int)tmp.Height);
            }
        }

        /// <summary>
        /// 指定したコントロールと、その子コントロールのフォントを再帰的に変更します
        /// </summary>
        /// <param name="c"></param>
        /// <param name="font"></param>
        public static void applyFontRecurse(Control c, Font font)
        {
            if (!isApplyFontRecurseEnabled) {
                return;
            }
            c.Font = font;
            for (int i = 0; i < c.Controls.Count; i++) {
                applyFontRecurse(c.Controls[i], font);
            }
        }

        [Obsolete]
        public static void applyFontRecurse(Control c, java.awt.Font font)
        {
            applyFontRecurse(c, font.font);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="start1"></param>
        /// <param name="end1"></param>
        /// <param name="start2"></param>
        /// <param name="end2"></param>
        /// <returns></returns>
        public static bool IsOverwrapped(double start1, double end1, double start2, double end2)
        {
            if (start2 <= start1 && start1 < end2) {
                return true;
            } else if (start2 < end1 && end1 < end2) {
                return true;
            } else {
                if (start1 <= start2 && start2 < end1) {
                    return true;
                } else if (start1 < end2 && end2 < end1) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    }

}
