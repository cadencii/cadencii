/*
 * VFW.cs
 * Copyright © 2007-2011 kbinani
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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using cadencii;

namespace cadencii.media
{

    public static class VFW
    {
        public static UInt32 streamtypeVIDEO = Util.mmioFOURCC('v', 'i', 'd', 's');
        public static UInt32 streamtypeAUDIO = Util.mmioFOURCC('a', 'u', 'd', 's');

        [DllImport("avifil32.dll")]
        public static extern int AVIStreamOpenFromFileW(
            out IntPtr ppavi,
            [MarshalAs(UnmanagedType.LPWStr)]string szfile,
            uint fccType,
            int lParam,
            int mode,
            int dumy
        );

        [DllImport("avifil32.dll")]
        public static extern void AVIFileInit();

        [DllImport("avifil32.dll")]
        public static extern int AVIFileOpenW(ref int ptr_pfile, [MarshalAs(UnmanagedType.LPWStr)]string fileName, int flags, int dummy);

        [DllImport("avifil32.dll")]
        public static extern int AVIFileCreateStream(
          int ptr_pfile, out IntPtr ptr_ptr_avi, ref AVISTREAMINFOW ptr_streaminfo);

        [DllImport("avifil32.dll")]
        public static extern int AVIMakeCompressedStream(
          out IntPtr ppsCompressed, IntPtr aviStream, ref AVICOMPRESSOPTIONS ao, int dummy);

        [DllImport("avifil32.dll")]
        public static extern int AVIStreamSetFormat(
          IntPtr aviStream, Int32 lPos, ref BITMAPINFOHEADER lpFormat, Int32 cbFormat);

        [DllImport("avifil32.dll")]
        public static unsafe extern int AVISaveOptions(
          IntPtr hwnd, UInt32 flags, int nStreams, IntPtr* ptr_ptr_avi, AVICOMPRESSOPTIONS** ao);

        [DllImport("avifil32.dll")]
        public static extern int AVIStreamWrite(
          IntPtr aviStream, Int32 lStart, Int32 lSamples, IntPtr lpBuffer,
          Int32 cbBuffer, Int32 dwFlags, Int32 dummy1, Int32 dummy2);

        [DllImport("avifil32.dll")]
        public static extern int AVIStreamRelease(IntPtr aviStream);

        [DllImport("avifil32.dll")]
        public static extern int AVIFileRelease(int pfile);

        [DllImport("avifil32.dll")]
        public static extern void AVIFileExit();

        //Get a pointer to a GETFRAME object (returns 0 on error)
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamGetFrameOpen(
            IntPtr pAVIStream,
            ref BITMAPINFOHEADER bih);

        //Get a stream from an open AVI file
        [DllImport("avifil32.dll")]
        public static extern int AVIFileGetStream(
            int pfile,
            out IntPtr ppavi,
            int fccType,
            int lParam);

        //Get the start position of a stream
        [DllImport("avifil32.dll", PreserveSig = true)]
        public static extern int AVIStreamStart(int pavi);

        //Get the length of a stream in frames
        [DllImport("avifil32.dll", PreserveSig = true)]
        public static extern int AVIStreamLength(int pavi);

        //Get information about an open stream
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamInfo(
            int pAVIStream,
            ref AVISTREAMINFOW psi,
            int lSize);

        //Get a pointer to a packed DIB (returns 0 on error)
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamGetFrame(
            int pGetFrameObj,
            int lPos);
        //Release the GETFRAME object
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamGetFrameClose(
            int pGetFrameObj);
    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AVISTREAMINFOW
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
        new public string ToString()
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
    public struct AVIStreamHeader
    {
        public uint fccType;//    character(len = 4) fccType
        public uint fccHandler;//character(len = 4) fccHandler
        public uint dwFlags;
        public uint dwReserved1;
        public uint dwInitialFrames;
        public uint dwScale;
        public uint dwRate;
        public uint dwStart;
        public uint dwLength;
        public uint dwSuggestedBufferSize;
        public uint dwQuality;
        public uint dwSampleSize;
        public void Write(Stream s)
        {
            bool bigendian = !BitConverter.IsLittleEndian;
            byte[] b;
            b = BitConverter.GetBytes(fccType);
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(fccHandler);
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwFlags);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwReserved1);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwInitialFrames);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwScale);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwRate);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwStart);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwLength);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwSuggestedBufferSize);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwQuality);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(dwSampleSize);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MainAVIHeader
    {
        public uint dwMicroSecPerFrame;     // specifies the period between video frames. This value indicates the overall timing for the file.
        public uint dwMaxBytesPerSec;       // specifies the approximate maximum data rate of the file.
        public uint dwReserved1;
        public uint dwFlags;                // AVIF_HASINDEX, AVIF_MUSTUSEINDEX, AVIF_ISINTERLEAVED, AVIF_WASCAPTUREFILE, AVIF_COPYRIGHTED
        public uint dwTotalFrames;          // specifies the total number of frames of data in file.
        public uint dwInitialFrames;        // used for interleaved files. If you are creating interleaved files, specify the number of frames in the file prior to the initial frame of the AVI sequence in this field.
        public uint dwStreams;              // specifies the number of streams in the file. For example, a file with audio and video has 2 streams.
        public uint dwSuggestedBufferSize;  // specifies the suggested buffer size for reading the file.
        public uint dwWidth;                // specify the width of the AVI file in pixels.
        public uint dwHeight;               // specify the height of the AVI file in pixels.
        public uint dwScale;                // used to specify the general time scale that the file will use.
        public uint dwRate;                 // used to specify the general time scale that the file will use.
        public uint dwStart;                // specify the starting time of the AVI file and the length of the file. The units are 
        public uint dwLength;               // defined by dwRate and dwScale. The dwStart field is usually set to zero.
        public void Write(Stream s)
        {
            byte[] b;
            bool bigendian = !BitConverter.IsLittleEndian;

            // dwMicroSecPerFrame
            b = BitConverter.GetBytes(dwMicroSecPerFrame);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwMaxBytesPerSec
            b = BitConverter.GetBytes(dwMaxBytesPerSec);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwReserved1
            b = BitConverter.GetBytes(dwReserved1);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwFlags
            b = BitConverter.GetBytes(dwFlags);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwtotalFrames
            b = BitConverter.GetBytes(dwTotalFrames);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwInitialFrames
            b = BitConverter.GetBytes(dwInitialFrames);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwStreams
            b = BitConverter.GetBytes(dwStreams);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwSuggestedBufferSize
            b = BitConverter.GetBytes(dwSuggestedBufferSize);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwWidth
            b = BitConverter.GetBytes(dwWidth);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwHeight
            b = BitConverter.GetBytes(dwHeight);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwScale
            b = BitConverter.GetBytes(dwScale);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwrate
            b = BitConverter.GetBytes(dwRate);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwStart
            b = BitConverter.GetBytes(dwStart);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            // dwLength
            b = BitConverter.GetBytes(dwLength);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPFILEHEADER
    {
        public Int16 bfType; //"magic cookie" - must be "BM"
        public Int32 bfSize;
        public Int16 bfReserved1;
        public Int16 bfReserved2;
        public Int32 bfOffBits;
    }
}
