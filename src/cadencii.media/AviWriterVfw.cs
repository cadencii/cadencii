/*
 * AviWriterVfw.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.media.
 *
 * cadencii.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace cadencii.media
{

    public class AviWriterVfw : IAviWriter
    {
        private int m_file_handle = 0;
        private IntPtr m_video = new IntPtr(0);
        private IntPtr m_video_compressed = new IntPtr(0);
        private IntPtr m_audio = new IntPtr(0);
        private IntPtr m_audio_compressed = new IntPtr(0);
        private uint m_scale;
        private uint m_rate;
        private int m_count = 0;
        private UInt32 m_width = 0;
        private UInt32 m_stride = 0;
        private UInt32 m_height = 0;
        private static readonly UInt32 _STREAM_TYPE_VIDEO = (UInt32)mmioFOURCC('v', 'i', 'd', 's');
        private static readonly UInt32 _STREAM_TYPE_AUDIO = (UInt32)mmioFOURCC('a', 'u', 'd', 's');
        private uint m_strh_fcc = 0;
        private string m_file = "";

        private const int OF_READ = 0;
        private const int OF_READWRITE = 2;
        private const int OF_WRITE = 1;
        private const int OF_SHARE_COMPAT = 0;
        private const int OF_SHARE_DENY_NONE = 64;
        private const int OF_SHARE_DENY_READ = 48;
        private const int OF_SHARE_DENY_WRITE = 32;
        private const int OF_SHARE_EXCLUSIVE = 16;
        private const int OF_CREATE = 4096;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct AVISTREAMINFOW
        {
            public UInt32 fccType, fccHandler, dwFlags, dwCaps;

            public UInt16 wPriority, wLanguage;

            public UInt32 dwScale, dwRate,
                             dwStart, dwLength, dwInitialFrames, dwSuggestedBufferSize,
                             dwQuality, dwSampleSize, rect_left, rect_top,
                             rect_right, rect_bottom, dwEditCount, dwFormatChangeCount;
            public UInt16 szName0, szName1, szName2, szName3, szName4, szName5,
                             szName6, szName7, szName8, szName9, szName10, szName11,
                             szName12, szName13, szName14, szName15, szName16, szName17,
                             szName18, szName19, szName20, szName21, szName22, szName23,
                             szName24, szName25, szName26, szName27, szName28, szName29,
                             szName30, szName31, szName32, szName33, szName34, szName35,
                             szName36, szName37, szName38, szName39, szName40, szName41,
                             szName42, szName43, szName44, szName45, szName46, szName47,
                             szName48, szName49, szName50, szName51, szName52, szName53,
                             szName54, szName55, szName56, szName57, szName58, szName59,
                             szName60, szName61, szName62, szName63;
        }

        // vfw.h
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AVICOMPRESSOPTIONS
        {
            public UInt32 fccType;
            public UInt32 fccHandler;
            public UInt32 dwKeyFrameEvery;  // only used with AVICOMRPESSF_KEYFRAMES
            public UInt32 dwQuality;
            public UInt32 dwBytesPerSecond; // only used with AVICOMPRESSF_DATARATE
            public UInt32 dwFlags;
            public IntPtr lpFormat;
            public UInt32 cbFormat;
            public IntPtr lpParms;
            public UInt32 cbParms;
            public UInt32 dwInterleaveEvery;
            public override string ToString()
            {
                return "fccType=" + fccType + "\n" +
                    "fccHandler=" + fccHandler + "\n" +
                    "dwKeyFrameEvery=" + dwKeyFrameEvery + "\n" +
                    "dwQuality=" + dwQuality + "\n" +
                    "dwBytesPerSecond=" + dwBytesPerSecond + "\n" +
                    "dwFlags=" + dwFlags + "\n" +
                    "lpFormat=" + lpFormat + "\n" +
                    "cbFormat=" + cbFormat + "\n" +
                    "lpParms=" + lpParms + "\n" +
                    "cbParms=" + cbParms + "\n" +
                    "dwInterleaveEvery=" + dwInterleaveEvery;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPINFOHEADER
        {
            public UInt32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public Int16 biPlanes;
            public Int16 biBitCount;
            public UInt32 biCompression;
            public UInt32 biSizeImage;
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public UInt32 biClrUsed;
            public UInt32 biClrImportant;
        }

        public Size Size
        {
            get
            {
                return new Size((int)m_width, (int)m_height);
            }
        }

        public uint Scale
        {
            get
            {
                return m_scale;
            }
        }

        public uint Rate
        {
            get
            {
                return m_rate;
            }
        }

        public class AviException : ApplicationException
        {
            public AviException(string s)
                : base(s)
            {
            }
            public AviException(string s, Int32 hr)
                : base(s)
            {

                if (hr == AVIERR_BADPARAM) {
                    err_msg = "AVIERR_BADPARAM";
                } else {
                    err_msg = "unknown";
                }
            }

            public string ErrMsg()
            {
                return err_msg;
            }
            private const Int32 AVIERR_BADPARAM = -2147205018;
            private string err_msg;
        }

        public bool Open(string file_name, uint scale, uint rate, int width, int height, IntPtr hwnd)
        {
            m_file = file_name;
            this.m_scale = scale;
            this.m_rate = rate;
            this.m_width = (UInt32)width;
            this.m_height = (UInt32)height;
            using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb)) {
                BitmapData bmpDat = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                this.m_stride = (UInt32)bmpDat.Stride;
                bmp.UnlockBits(bmpDat);
            }
            AVIFileInit();
            int hr = AVIFileOpenW(ref m_file_handle, file_name, OF_WRITE | OF_CREATE, 0);
            if (hr != 0) {
                throw new AviException("error for AVIFileOpenW");
            }

            CreateStream();
            return SetOptions(hwnd);
        }

        public void AddFrame(Bitmap bmp)
        {
            BitmapData bmpDat = bmp.LockBits(new Rectangle(0, 0, (int)m_width, (int)m_height),
                                              ImageLockMode.ReadOnly,
                                              PixelFormat.Format24bppRgb);

            int hr = AVIStreamWrite(m_video_compressed, m_count, 1,
                                     bmpDat.Scan0,
                                     (Int32)(m_stride * m_height),
                                     0,
                                     0,
                                     0);

            if (hr != 0) {
                throw new AviException("AVIStreamWrite");
            }

            bmp.UnlockBits(bmpDat);

            m_count++;
        }

        unsafe public static AVICOMPRESSOPTIONS RequireVideoCompressOption(AVICOMPRESSOPTIONS current_option)
        {
            AviWriterVfw temp = new AviWriterVfw();
            temp.m_scale = 1000;
            temp.m_rate = 30 * temp.m_scale;
            int width = 10, height = 10;
            temp.m_width = (UInt32)width;
            temp.m_height = (UInt32)height;
            using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb)) {
                BitmapData bmpDat = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                temp.m_stride = (UInt32)bmpDat.Stride;
                bmp.UnlockBits(bmpDat);
            }
            AVIFileInit();
            string temp_file = Path.GetTempFileName() + ".avi";// .aviを付けないと、AVIFileOpenWが失敗する
            int hr = AVIFileOpenW(ref temp.m_file_handle, temp_file, OF_WRITE | OF_CREATE, 0);
            if (hr != 0) {
                throw new AviException("error for AVIFileOpenW");
            }

            temp.CreateStream();

            AVICOMPRESSOPTIONS opts = new AVICOMPRESSOPTIONS();
            opts.fccType = 0; //fccType_;
            opts.fccHandler = 0;//fccHandler_;
            opts.dwKeyFrameEvery = 0;
            opts.dwQuality = 0;  // 0 .. 10000
            opts.dwFlags = 0;  // AVICOMRPESSF_KEYFRAMES = 4
            opts.dwBytesPerSecond = 0;
            opts.lpFormat = new IntPtr(0);
            opts.cbFormat = 0;
            opts.lpParms = new IntPtr(0);
            opts.cbParms = 0;
            opts.dwInterleaveEvery = 0;

            opts = current_option;
            AVICOMPRESSOPTIONS* p = &opts;
            AVICOMPRESSOPTIONS** pp = &p;
            IntPtr x = temp.m_video;
            IntPtr* ptr_ps = &x;
            AVISaveOptions(IntPtr.Zero, 0, 1, ptr_ps, pp);
            //MessageBox.Show( "AVISaveOptions ok" );
            AVICOMPRESSOPTIONS copied = new AVICOMPRESSOPTIONS();
            copied = opts;
            AVIStreamRelease(temp.m_video);
            //MessageBox.Show( "AVIStreamRelease(temp.m_video) ok" );

            AVIFileRelease(temp.m_file_handle);
            //MessageBox.Show( "AVIFileRelease ok" );
            AVIFileExit();
            //MessageBox.Show( "AVIFileExit ok" );
            File.Delete(temp_file);
            //MessageBox.Show( "File.Delete(fileName) ok" );
            return copied;
        }

        /// <summary>
        /// オーディオ圧縮の設定ダイアログを表示し、オーディオ圧縮の設定を取得します
        /// </summary>
        /// <returns></returns>
        unsafe public static AVICOMPRESSOPTIONS RequireAudioCompressOption()
        {
            string temp_file = Path.GetTempFileName();
            byte[] buf = new byte[] {
                82, 73, 70, 70, 94, 0, 0, 0, 87, 65, 86, 69, 102, 109, 116, 32, 16, 0, 0, 0, 1,
                0, 1, 0, 64, 31, 0, 0, 64, 31, 0, 0, 1, 0, 8, 0, 100, 97, 116, 97, 58, 0, 0, 0,
                128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
                128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
                128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
                128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
            };
            using (FileStream fs = new FileStream(temp_file, FileMode.Create)) {
                fs.Write(buf, 0, buf.Length);
            }

            AVIFileInit();
            //MessageBox.Show( "AVIFileInit ok" );
            IntPtr audio;
            int hr = AVIStreamOpenFromFileW(out audio, temp_file, _STREAM_TYPE_AUDIO, 0, OF_READ, 0);
            //MessageBox.Show( "AVIStreamOpenFromFileW ok" );

            AVICOMPRESSOPTIONS opts = new AVICOMPRESSOPTIONS();
            opts.fccType = _STREAM_TYPE_AUDIO; //fccType_;
            opts.fccHandler = 0;//fccHandler_;
            opts.dwKeyFrameEvery = 0;
            opts.dwQuality = 0;  // 0 .. 10000
            opts.dwFlags = 0;  // AVICOMRPESSF_KEYFRAMES = 4
            opts.dwBytesPerSecond = 0;
            opts.lpFormat = new IntPtr(0);
            opts.cbFormat = 0;
            opts.lpParms = new IntPtr(0);
            opts.cbParms = 0;
            opts.dwInterleaveEvery = 0;
            AVICOMPRESSOPTIONS* p = &opts;
            AVICOMPRESSOPTIONS** pp = &p;
            IntPtr x = audio;
            IntPtr* ptr_ps = &x;
            AVISaveOptions(IntPtr.Zero, 0, 1, ptr_ps, pp);
            //MessageBox.Show( "AVISaveOptions ok" );
            AVICOMPRESSOPTIONS copied = new AVICOMPRESSOPTIONS();
            copied = opts;
            AVIStreamRelease(audio);
            //MessageBox.Show( "AVIStreamRelease(audio) ok" );

            AVIFileExit();
            //MessageBox.Show( "AVIFileExit ok" );
            File.Delete(temp_file);
            return copied;
        }

        public void Close()
        {
            AVIStreamRelease(m_video);
            AVIStreamRelease(m_video_compressed);
            AVIFileRelease(m_file_handle);
            AVIFileExit();
            using (FileStream fs = new FileStream(m_file, FileMode.Open)) {
                fs.Seek(0x70, SeekOrigin.Begin);
                byte ch3 = (byte)(m_strh_fcc >> 24);
                uint b = (uint)(m_strh_fcc - (ch3 << 24));
                byte ch2 = (byte)(b >> 16);
                b = (uint)(b - (ch2 << 16));
                byte ch1 = (byte)(b >> 8);
                byte ch0 = (byte)(b - (ch1 << 8));
                fs.Write(new byte[] { ch0, ch1, ch2, ch3 }, 0, 4);
            }
        }

        private void CreateStream()
        {
            // video stream
            AVISTREAMINFOW strhdr = new AVISTREAMINFOW();
            strhdr.fccType = _STREAM_TYPE_VIDEO;
            strhdr.fccHandler = 0;//            fccHandler_;
            strhdr.dwFlags = 0;
            strhdr.dwCaps = 0;
            strhdr.wPriority = 0;
            strhdr.wLanguage = 0;
            strhdr.dwScale = m_scale;
            strhdr.dwRate = m_rate;
            strhdr.dwStart = 0;
            strhdr.dwLength = 0;
            strhdr.dwInitialFrames = 0;
            strhdr.dwSuggestedBufferSize = m_height * m_stride;
            strhdr.dwQuality = 0xffffffff;
            strhdr.dwSampleSize = 0;
            strhdr.rect_top = 0;
            strhdr.rect_left = 0;
            strhdr.rect_bottom = m_height;
            strhdr.rect_right = m_width;
            strhdr.dwEditCount = 0;
            strhdr.dwFormatChangeCount = 0;
            strhdr.szName0 = 0;
            strhdr.szName1 = 0;
            int hr = AVIFileCreateStream(m_file_handle, out m_video, ref strhdr);
            if (hr != 0) {
                throw new AviException("AVIFileCreateStream; Video");
            }
#if DEBUG
            Console.WriteLine("AviWrierVfw+CreateStream");
            Console.WriteLine("    strhdr.fccHandler=" + strhdr.fccHandler);
#endif
        }

        internal static void CalcScaleAndRate(decimal fps, out uint scale, out uint rate)
        {
            scale = 100;
            rate = (uint)(fps * 1000m);
            int max = (int)(Math.Log10(uint.MaxValue));
            for (int i = 0; i <= max; i++) {
                scale = (uint)pow10(i);
                rate = (uint)(fps * scale);
                decimal t_fps = (decimal)rate / (decimal)scale;
                if (t_fps == fps) {
                    return;
                }
            }
        }

        private static int pow10(int x)
        {
            int result = 1;
            for (int i = 1; i <= x; i++) {
                result = result * 10;
            }
            return result;
        }

        unsafe private bool SetOptions(IntPtr hwnd)
        {
            // VIDEO
            AVICOMPRESSOPTIONS opts = new AVICOMPRESSOPTIONS();
            opts.fccType = 0;
            opts.fccHandler = 0;
            opts.dwKeyFrameEvery = 0;
            opts.dwQuality = 0;
            opts.dwFlags = 0;
            opts.dwBytesPerSecond = 0;
            opts.lpFormat = new IntPtr(0);
            opts.cbFormat = 0;
            opts.lpParms = new IntPtr(0);
            opts.cbParms = 0;
            opts.dwInterleaveEvery = 0;

            AVICOMPRESSOPTIONS* p = &opts;
            AVICOMPRESSOPTIONS** pp;
            pp = &p;
            IntPtr x = m_video;
            IntPtr* ptr_ps;
            ptr_ps = &x;
            if (AVISaveOptions(hwnd, 0, 1, ptr_ps, pp) == 0) {
                return false;
            }
            int hr = AVIMakeCompressedStream(out m_video_compressed, m_video, ref opts, 0);
            if (hr != 0) {
                throw new AviException("AVIMakeCompressedStream; Video");
            }
            m_strh_fcc = opts.fccHandler;
#if DEBUG
            Console.WriteLine("AviWriterVfw+SetOptions");
            Console.WriteLine("    opts.fccHandler=" + opts.fccHandler);
#endif

            // TODO: AVISaveOptionsFree(...)
            BITMAPINFO bi = new BITMAPINFO();
            bi.bmiHeader.biSize = 40;
            bi.bmiHeader.biWidth = (Int32)m_width;
            bi.bmiHeader.biHeight = (Int32)m_height;
            bi.bmiHeader.biPlanes = 1;
            bi.bmiHeader.biBitCount = 24;
            bi.bmiHeader.biCompression = 0;
            bi.bmiHeader.biSizeImage = m_stride * m_height;
            bi.bmiHeader.biXPelsPerMeter = 0;
            bi.bmiHeader.biYPelsPerMeter = 0;
            bi.bmiHeader.biClrUsed = 0;
            bi.bmiHeader.biClrImportant = 0;

            hr = AVIStreamSetFormat(m_video_compressed, 0, ref bi, sizeof(BITMAPINFO));
            if (hr != 0) {
                throw new AviException("AVIStreamSetFormat", hr);
            }
#if DEBUG
            Console.WriteLine("    bi.bmiHeader.biCompression=" + bi.bmiHeader.biCompression);
#endif
            return true;
        }

        [DllImport("avifil32.dll")]
        private static extern int AVIStreamOpenFromFileW(out IntPtr ppavi,
                                                          [MarshalAs(UnmanagedType.LPWStr)]string szfile,
                                                          uint fccType,
                                                          int lParam,
                                                          int mode,
                                                          int dumy);

        [DllImport("avifil32.dll")]
        private static extern void AVIFileInit();

        [DllImport("avifil32.dll")]
        private static extern int AVIFileOpenW(ref int ptr_pfile,
                                                [MarshalAs(UnmanagedType.LPWStr)]string fileName,
                                                int flags,
                                                int dummy);

        [DllImport("avifil32.dll")]
        private static extern int AVIFileCreateStream(
          int ptr_pfile, out IntPtr ptr_ptr_avi, ref AVISTREAMINFOW ptr_streaminfo);

        [DllImport("avifil32.dll")]
        private static extern int AVIMakeCompressedStream(
          out IntPtr ppsCompressed, IntPtr aviStream, ref AVICOMPRESSOPTIONS ao, int dummy);

        [DllImport("avifil32.dll")]
        private static extern int AVIStreamSetFormat(
          IntPtr aviStream, Int32 lPos, ref BITMAPINFO lpFormat, Int32 cbFormat);

        [DllImport("avifil32.dll")]
        unsafe private static extern int AVISaveOptions(
          IntPtr hwnd, UInt32 flags, int nStreams, IntPtr* ptr_ptr_avi, AVICOMPRESSOPTIONS** ao);

        [DllImport("avifil32.dll")]
        private static extern int AVIStreamWrite(IntPtr aviStream,
                                                  Int32 lStart,
                                                  Int32 lSamples,
                                                  IntPtr lpBuffer,
                                                  Int32 cbBuffer,
                                                  Int32 dwFlags,
                                                  Int32 dummy1,
                                                  Int32 dummy2);

        [DllImport("avifil32.dll")]
        private static extern int AVIStreamRelease(IntPtr aviStream);

        [DllImport("avifil32.dll")]
        private static extern int AVIFileRelease(int pfile);

        [DllImport("avifil32.dll")]
        private static extern void AVIFileExit();

        public static Int32 mmioFOURCC(char ch0, char ch1, char ch2, char ch3)
        {
            return ((Int32)(byte)(ch0) | ((byte)(ch1) << 8) | ((byte)(ch2) << 16) | ((byte)(ch3) << 24));
        }
    }

}
