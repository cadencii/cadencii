/*
 * VCM.cs
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
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

using cadencii;

namespace cadencii.media
{

    public class VCM
    {
        public const uint ICMODE_QUERY = 4;
        public const uint ICMF_CHOOSE_ALLCOMPRESSORS = 0x08;
        public static readonly uint ICTYPE_VIDEO = Util.mmioFOURCC('v', 'i', 'd', 'c');
        public static readonly uint ICTYPE_AUDIO = Util.mmioFOURCC('a', 'u', 'd', 'c');
        public const uint ICM_COMPRESS_GET_FORMAT = 0x4004;

        [DllImport("msvfw32.dll")]
        public static extern UInt32 ICOpen(UInt32 fccType, UInt32 fccHandler, uint wMode);

        [DllImport("msvfw32.dll")]
        public static unsafe extern int ICGetInfo(UInt32 hic, ICINFO* icinfo, int size);
        /*unsafe public static int ICGetInfo( UInt32 hic, ref ICINFO icinfo, int size ) {
            IntPtr pt = new IntPtr( 0 );
            int r = w_ICGetInfo( hic, &pt, size );
            ICINFO result = new ICINFO();
            result = (ICINFO)Marshal.PtrToStructure( pt, result.GetType() );
            icinfo = result;
            return r;
        }*/

        [DllImport("msvfw32.dll")]
        public static extern int ICClose(UInt32 hic);

        [DllImport("msvfw32.dll")]
        public static unsafe extern int ICCompressorChoose(
            IntPtr hwnd,
            UInt32 uiFlags,
            IntPtr pvIn,
            IntPtr lpData,
            COMPVARS* pc,
            string lpszTitle
        );

        [DllImport("msvfw32.dll")]
        public static unsafe extern int ICSeqCompressFrameStart(
            COMPVARS* pc,
            BITMAPINFO* lpbiIn
        );

        [DllImport("msvfw32.dll")]
        public static unsafe extern void ICSeqCompressFrameEnd(COMPVARS* pc);

        [DllImport("msvfw32.dll")]
        public static unsafe extern void ICCompressorFree(COMPVARS* pc);

        [DllImport("msvfw32.dll")]
        public static unsafe extern IntPtr ICSeqCompressFrame(
            COMPVARS* pc,
            UInt32 uiFlags,
            IntPtr lpBits,
            Int32* pfKey,
            Int32* plSize
        );

        [DllImport("msvfw32.dll")]
        public static unsafe extern UIntPtr ICSendMessage(UInt32 hic, UInt32 wMsg, IntPtr dw1, IntPtr dw2);

        public static unsafe uint ICCompressGetFormatSize(UInt32 hic, BITMAPINFO* lpbi)
        {
            return ICCompressGetFormat(hic, lpbi, (BITMAPINFO*)0);
        }


        public static unsafe uint ICCompressGetFormat(UInt32 hic, BITMAPINFO* lpbiInput, BITMAPINFO* lpbiOutput)
        {
            return (uint)ICSendMessage(hic, ICM_COMPRESS_GET_FORMAT, (IntPtr)lpbiInput, (IntPtr)lpbiOutput);
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ICINFO
    {
        public UInt32 dwSize;
        public UInt32 fccType;
        public UInt32 fccHandler;
        public UInt32 dwFlags;
        public UInt32 dwVersion;
        public UInt32 dwVersionICM;
        private sz16 szName;
        private sz128 szDescription;
        private sz128 szDriver;
        public string Name
        {
            get
            {
                string t = szName.Value;
                //return t.ToCharArray();
                return t;
            }
        }
        public string Description
        {
            get
            {
                string t = szDescription.Value;
                //return t.ToCharArray();
                return t;
            }
        }
        public string Driver
        {
            get
            {
                string t = szDriver.Value;
                //return t.ToCharArray();
                return t;
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct sz4
        {
            UInt16 UInt16_1;
            UInt16 UInt16_2;
            UInt16 UInt16_3;
            UInt16 UInt16_4;
            public string Value
            {
                get
                {
                    return "" + (char)UInt16_1 + (char)UInt16_2 + (char)UInt16_3 + (char)UInt16_4;
                }
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct sz16
        {
            sz4 sz4_1;
            sz4 sz4_2;
            sz4 sz4_3;
            sz4 sz4_4;
            public string Value
            {
                get
                {
                    return sz4_1.Value + sz4_2.Value + sz4_3.Value + sz4_4.Value;
                }
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct sz64
        {
            sz16 sz16_1;
            sz16 sz16_2;
            sz16 sz16_3;
            sz16 sz16_4;
            public string Value
            {
                get
                {
                    return sz16_1.Value + sz16_2.Value + sz16_3.Value + sz16_4.Value;
                }
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct sz128
        {
            sz64 sz64_1;
            sz64 sz64_2;
            public string Value
            {
                get
                {
                    return sz64_1.Value + sz64_2.Value;
                }
            }
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct COMPVARS
    {
        public Int32 cbSize;
        public UInt32 dwFlags;
        public UInt32 hic; // 元はHIC
        public UInt32 fccType;
        public UInt32 fccHandler;
        public BITMAPINFOHEADER* lpbiIn;
        public BITMAPINFOHEADER* lpbiOut;
        public void* lpBitsOut;
        public void* lpBitsPrev;
        public Int32 lFrame;
        public Int32 lKey;
        public Int32 lDataRate;
        public Int32 lQ;
        public Int32 lKeyCount;
        public void* lpState;
        public Int32 cbState;
        public override string ToString()
        {
            return "{cbSize=" + cbSize +
                   ",dwFlags=" + dwFlags +
                   ",hic=" + hic +
                   ",fccType=" + fccType +
                   ",fccHandler=" + fccHandler +
                   ",lpbiIn=" + ((int)lpbiIn).ToString() +
                   ",lpbiOut=" + ((int)lpbiOut).ToString() +
                   ",lpBitsOut=" + ((int)lpBitsOut).ToString() +
                   ",lpBitsPrev=" + ((int)lpBitsPrev).ToString() +
                   ",lFrame=" + lFrame +
                   ",lKey=" + lKey +
                   ",lDataRate=" + lDataRate +
                   ",lQ=" + lQ +
                   ",lKeyCount=" + lKeyCount +
                   ",lpState=" + ((int)lpState).ToString() +
                   ",cbState=" + cbState + "}";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        public RGBQUAD bmiColors;
        public unsafe void Write(BinaryWriter bw)
        {
            bmiHeader.Write(bw);
            int remain = (int)((bmiHeader.biSize - sizeof(BITMAPINFOHEADER)) / sizeof(RGBQUAD));
            fixed (RGBQUAD* rgbq = &bmiColors) {    // これ以外に，「落ちないやり方」が無かった．メモリーが連続していることを祈るのみ
                for (int i = 0; i < remain; i++) {
                    bw.Write(rgbq[i].rgbBlue);
                    bw.Write(rgbq[i].rgbGreen);
                    bw.Write(rgbq[i].rgbRed);
                    bw.Write(rgbq[i].rgbReserved);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }

}
