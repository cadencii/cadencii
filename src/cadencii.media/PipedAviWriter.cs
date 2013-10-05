/*
 * PipedAviWriter.cs
 * Copyright © 2008-2011 kbinani
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
#if !MONO
#define TEST
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
//using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;

using cadencii;

namespace cadencii.media
{

    public class PipedAviWriter
    {
        private const string _PIPE_NAME = "fifo";
        private uint m_scale = 1;
        private uint m_rate = 30;
#if TEST
        private FileStream m_stream = null;
#else
        private NamedPipeServerStream m_stream = null;
#endif
        private MainAVIHeader m_main_avi_header;
        private uint m_bitmapsize;
        private bool m_header_written = false;
        private ulong m_frames;
        private Thread m_ffmpeg = null;
        private PixelFormat m_pix_fmt = PixelFormat.Format24bppRgb;
        private int m_bit_count = 24;

        public bool AddFrame(Bitmap bmp)
        {
            if (bmp.PixelFormat != m_pix_fmt) {
                return false;
            }
            BitmapData bdat = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                            ImageLockMode.ReadOnly,
                                            bmp.PixelFormat);
            BinaryWriter bw = new BinaryWriter(new MemoryStream());
            if (!m_header_written) {
                m_bitmapsize = (uint)(bdat.Stride * bdat.Height);
                //m_stream.SetLength( (long)(0xdc + (m_bitmapsize + 0x8) * m_frames) );

                /*m_main_avi_header.dwWidth = (uint)bdat.Width;
                m_main_avi_header.dwHeight = (uint)bdat.Height;// bmp%infoHeader%Height
                m_main_avi_header.dwMaxBytesPerSec = (uint)((float)bdat.Stride * (float)bdat.Height * (float)m_rate / (float)m_scale);
                m_main_avi_header.dwStreams = 1;
                m_main_avi_header.dwSuggestedBufferSize = (uint)(bdat.Stride * bdat.Height);

                AVIStreamHeader stream_header = new AVIStreamHeader();
                stream_header.fccType = Util.mmioFOURCC( "vids" );
                stream_header.fccHandler = 0;
                stream_header.dwFlags = 0;
                stream_header.dwReserved1 = 0;
                stream_header.dwInitialFrames = 0;
                stream_header.dwScale = m_scale;
                stream_header.dwRate = m_rate;
                stream_header.dwStart = 0;
                stream_header.dwSuggestedBufferSize = m_main_avi_header.dwSuggestedBufferSize;
                stream_header.dwQuality = 0;
                stream_header.dwSampleSize = 0;

                BITMAPINFOHEADER bih = new BITMAPINFOHEADER(); //(BITMAPINFOHEADER)Marshal.PtrToStructure( Marshal.AllocHGlobal( sizeof( BITMAPINFOHEADER ) ), typeof( BITMAPINFOHEADER ) );
                bih.biSize = (uint)(Marshal.SizeOf( bih ));
                bih.biWidth = bdat.Width;
                bih.biHeight = bdat.Height;
                bih.biPlanes = 1;
                bih.biBitCount = m_pix_fmt == PixelFormat.Format24bppRgb ? (short)24 : (short)32;
                bih.biCompression = 0;//BI_RGB
                bih.biSizeImage = (uint)(bdat.Stride * bdat.Height);
                bih.biXPelsPerMeter = 0;
                bih.biYPelsPerMeter = 0;
                bih.biClrUsed = 0;
                bih.biClrImportant = 0;

                bw.Write( "RIFF".ToCharArray() );
                bw.Write( (uint)(0xdc + (m_bitmapsize + 0x8) * m_frames) );
                bw.Write( "AVI ".ToCharArray() );
                bw.Write( "LIST".ToCharArray() );
                bw.Write( (uint)0xc0 );
                bw.Write( "hdrl".ToCharArray() );
                bw.Write( "avih".ToCharArray() );
                bw.Write( (uint)0x38 );
                m_main_avi_header.Write( bw.BaseStream );
                bw.Write( "LIST".ToCharArray() );
                bw.Write( (uint)0x7C );
                bw.Write( "strl".ToCharArray() );
                bw.Write( "strh".ToCharArray() );
                bw.Write( (uint)0x38 );
                stream_header.Write( bw.BaseStream );
                bw.Write( (uint)0x0 );
                bw.Write( (uint)0x0 );
                bw.Write( "strf".ToCharArray() );
                bw.Write( (uint)0x28 );
                bih.Write( bw.BaseStream );
                bw.Write( "LIST".ToCharArray() );
                bw.Write( (uint)((m_bitmapsize + 0x8) * m_frames) );
                bw.Write( "movi".ToCharArray() );*/

                m_header_written = true;
            }

            //bw.Write( "00db" );
            //bw.Write( (uint)m_bitmapsize );
            int address = bdat.Scan0.ToInt32();
            byte[] bitmapData = new byte[bdat.Stride * bdat.Height];
            Marshal.Copy(new IntPtr(address), bitmapData, 0, bitmapData.Length);
            //bw.Write( (uint)m_main_avi_header.dwSuggestedBufferSize );
            bw.Write("BM".ToCharArray());
            bw.Write(m_bitmapsize);
            bw.Write((uint)0x0);
            bw.Write((uint)0x36);
            BITMAPINFOHEADER bih = new BITMAPINFOHEADER(); //(BITMAPINFOHEADER)Marshal.PtrToStructure( Marshal.AllocHGlobal( sizeof( BITMAPINFOHEADER ) ), typeof( BITMAPINFOHEADER ) );
            bih.biSize = (uint)(Marshal.SizeOf(bih));
            bih.biWidth = bdat.Width;
            bih.biHeight = bdat.Height;
            bih.biPlanes = 1;
            bih.biBitCount = m_pix_fmt == PixelFormat.Format24bppRgb ? (short)24 : (short)32;
            bih.biCompression = 0;//BI_RGB
            bih.biSizeImage = (uint)(bdat.Stride * bdat.Height);
            bih.biXPelsPerMeter = 0;
            bih.biYPelsPerMeter = 0;
            bih.biClrUsed = 0;
            bih.biClrImportant = 0;
            bih.Write(bw);
            bw.Write(bitmapData, 0, bitmapData.Length);
            //const int _BUF_LEN = 512;
            byte[] buf = new byte[m_bitmapsize + 6];
            bw.BaseStream.Seek(0, SeekOrigin.Begin);
            int len = bw.BaseStream.Read(buf, 0, (int)(m_bitmapsize + 6));
            if (len > 0) {
                m_stream.BeginWrite(buf, 0, len, null, null);
            }
            m_stream.Flush();
            bw.Close();

            bmp.UnlockBits(bdat);
            return true;
        }


        private void WriteFourCC(string value)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++) {
                b[i] = (byte)value[i];
            }
            m_stream.Write(b, 0, 4);
        }


        private void Write4Byte(uint value)
        {
            byte[] b;
            b = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) {
                Array.Reverse(b);
            }
            m_stream.Write(b, 0, 4);
        }


        public void Close()
        {
            if (m_stream != null) {
#if !TEST
                m_stream.Disconnect();
#endif
                m_stream.Close();
            }
            if (m_ffmpeg != null) {
                if (m_ffmpeg.IsAlive) {
                    m_ffmpeg.Abort();
                    while (m_ffmpeg.IsAlive) {
                    }
                }
            }
        }


        public void Open(uint scale, uint rate, ulong frames, PixelFormat pix_fmt)
        {
            if (m_stream != null) {
                m_stream.Close();
            }
#if TEST
            m_stream = new FileStream("test.out.avi", FileMode.Create);
#else
            m_stream = new NamedPipeServerStream( _PIPE_NAME, PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.None, 1, 1 );
#endif
            m_scale = scale;
            m_rate = rate;
            m_frames = frames;

            m_main_avi_header.dwMicroSecPerFrame = (uint)(1.0e6 * (double)scale / (double)rate);//  ! 1秒は10^6μ秒
            m_main_avi_header.dwReserved1 = 0;
            m_main_avi_header.dwFlags = 2064;
            m_main_avi_header.dwInitialFrames = 0;
            m_main_avi_header.dwStreams = 0;
            m_main_avi_header.dwScale = scale;
            m_main_avi_header.dwRate = rate;
            m_main_avi_header.dwStart = 0;
            m_main_avi_header.dwLength = 0;
            m_main_avi_header.dwTotalFrames = (uint)m_frames;

            m_pix_fmt = pix_fmt;
            if (m_pix_fmt != PixelFormat.Format24bppRgb && m_pix_fmt != PixelFormat.Format32bppArgb) {
                throw new ApplicationException("pixel format not supported");
            }
#if !TEST
            m_ffmpeg = new Thread( new ThreadStart( FFmpegEnc ) );
            m_ffmpeg.Start();
            m_stream.WaitForConnection();
#endif
        }


        private void FFmpegEnc()
        {
            Thread.Sleep(1000);
            Process client = new Process();
            client.StartInfo.FileName = "ffmpeg";

            // windowsの場合
            client.StartInfo.Arguments = @"-f image2pipe -vcodec bmp -i \\.\pipe\" + _PIPE_NAME + " -isync -an -f avi -y test.avi";

            // その他
            // client.StartInfo.Arguments = @"-i " + _PIPE_NAME + " -y test.mp3";

            client.StartInfo.RedirectStandardOutput = true;
            client.StartInfo.UseShellExecute = false;
            client.Start();
            StreamReader sr = client.StandardOutput;
            string line = "";
            while ((line = sr.ReadLine()) != null) {
                Console.WriteLine(line);
            }
        }
    }

}
#endif
