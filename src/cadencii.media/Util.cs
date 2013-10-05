/*
 * Util.cs
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
using System.IO;

using cadencii;

namespace cadencii.media
{

    internal static class Util
    {
        public const byte AVI_INDEX_OF_INDEXES = 0x00; //when each entry in aIndex
        // array points to an index chunk
        public const byte AVI_INDEX_OF_CHUNKS = 0x01;  // when each entry in aIndex
        // array points to a chunk in the file
        public const byte AVI_INDEX_IS_DATA = 0x80;    // when each entry is aIndex is
        // really the data
        public const byte AVI_INDEX_2FIELD = 0x01;     // when fields within frames
        // are also indexed

        public const int AVIF_HASINDEX = 16;           // Indicates the AVI file has an “idx1” chunk.
        public const int AVIF_MUSTUSEINDEX = 32;       // Indicates the index should be used to determine the order of presentation of the data.
        public const int AVIF_ISINTERLEAVED = 256;     // Indicates the AVI file is interleaved.
        public const int AVIF_WASCAPTUREFILE = 65536;  // Indicates the AVI file is a specially allocated file used for capturing real-time video.
        public const int AVIF_COPYRIGHTED = 131072;    // Indicates the AVI file contains copyrighted data.
        public const int AVIF_TRUSTCKTYPE = 2048;      // Use CKType to find key frames
        public const int BMP_MAGIC_COOKIE = 19778; //ascii string "BM"

        /*// <summary>
        /// 指定されたBITMAP型変数の情報ヘッダーをファイルに書き込みます．
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="fp"></param>
        public static void bmpWriteInfoHeader( BITMAPINFOHEADER infoHeader, BinaryWriter stream ) {
            //type(INFO_HEADER), intent(in) :: infoHeader
            //type(FILE), intent(inout) :: fp
            Util.WriteDWORD( (uint)infoHeader.biSize, stream );
            Util.WriteDWORD( (uint)infoHeader.biWidth, stream );
            Util.WriteDWORD( (uint)infoHeader.biHeight, stream );
            Util.WriteWORD( (ushort)infoHeader.biPlanes, stream );
            Util.WriteWORD( (ushort)(infoHeader.biBitCount), stream );
            Util.WriteDWORD( (uint)infoHeader.biCompression, stream );
            Util.WriteDWORD( (uint)infoHeader.biSizeImage, stream );
            Util.WriteDWORD( (uint)infoHeader.biXPelsPerMeter, stream );
            Util.WriteDWORD( (uint)infoHeader.biYPelsPerMeter, stream );
            Util.WriteDWORD( (uint)infoHeader.biClrUsed, stream );
            Util.WriteDWORD( (uint)infoHeader.biClrImportant, stream );
        }*/


        /// <summary>
        /// ファイルにAVIStreamHeader構造体の値を書き込みます
        /// </summary>
        public static void aviWriteStreamHeader(AVIStreamHeader streamHeader, MainAVIHeader mainHeader, BinaryWriter stream)
        {
            //type(AVI_CONTAINER), intent(inout) :: avi
            Util.fwrite("strh", stream);
            Util.WriteDWORD(56, stream);// call bmpQWordWrite( 56, avi%fp )    !// AVIStreamHeaderのサイズ
            //fwrite( streamHeader.fccType, fp );// i = fwrite( avi%streamHeader%fccType, 1, 4, avi%fp )
            Util.WriteDWORD((uint)streamHeader.fccType, stream);
            //fwrite( streamHeader.fccHandler, fp );//            i = fwrite( streamHeader.fccHandler, 1, 4, fp );
            Util.WriteDWORD((uint)streamHeader.fccHandler, stream);
            //WriteDWORD( 0, fp );
            Util.WriteDWORD(streamHeader.dwFlags, stream);
            //WriteDWORD( streamHeader.dwReserved1, fp );
            Util.WriteWORD(0, stream);//wPriority
            Util.WriteWORD(0, stream);//wLanghage
            Util.WriteDWORD(streamHeader.dwInitialFrames, stream);
            Util.WriteDWORD(streamHeader.dwScale, stream);
            Util.WriteDWORD(streamHeader.dwRate, stream);
            Util.WriteDWORD(streamHeader.dwStart, stream);
            Util.WriteDWORD(streamHeader.dwLength, stream);
            Util.WriteDWORD(streamHeader.dwSuggestedBufferSize, stream);
            Util.WriteDWORD(streamHeader.dwQuality, stream);
            Util.WriteDWORD(streamHeader.dwSampleSize, stream);
            Util.WriteWORD(0, stream);//left
            Util.WriteWORD(0, stream);//top
            Util.WriteWORD((ushort)mainHeader.dwWidth, stream);//right
            Util.WriteWORD((ushort)mainHeader.dwHeight, stream);//bottom
        }


        /// <summary>
        /// ファイルにMainAviHeader構造体の値を書き込みます
        /// </summary>
        public static void aviWriteMainHeader(MainAVIHeader mainHeader, BinaryWriter stream)
        {
            //type(AVI_CONTAINER), intent(inout) :: avi
            Util.fwrite("avih", stream);//    i = fwrite( 'avih', 1, 4, avi%fp )
            Util.WriteDWORD(56, stream);    // MainAVIHeaderのサイズ
            Util.WriteDWORD(mainHeader.dwMicroSecPerFrame, stream);
            Util.WriteDWORD(0/*this.mainHeader.dwMaxBytesPerSec*/, stream);
            Util.WriteDWORD(mainHeader.dwReserved1, stream);
            Util.WriteDWORD(mainHeader.dwFlags, stream);
            Util.WriteDWORD(mainHeader.dwTotalFrames, stream);
            Util.WriteDWORD(mainHeader.dwInitialFrames, stream);
            Util.WriteDWORD(mainHeader.dwStreams, stream);
            Util.WriteDWORD(0/*this.mainHeader.dwSuggestedBufferSize*/, stream);
            Util.WriteDWORD(mainHeader.dwWidth, stream);
            Util.WriteDWORD(mainHeader.dwHeight, stream);
            Util.WriteDWORD(mainHeader.dwScale, stream);
            Util.WriteDWORD(mainHeader.dwRate, stream);
            Util.WriteDWORD(mainHeader.dwStart, stream);
            Util.WriteDWORD(mainHeader.dwLength, stream);
        }//end subroutine


        public static void fwrite(string str, BinaryWriter fp)
        {
            int length = str.Length;
            if (length <= 0) {
                return;
            }
            foreach (char ch in str) {
                fp.Write((byte)ch);
            }
        }


        /// <summary>
        /// BYTE値を1byte分ファイルに書き込みます．
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fp"></param>
        public static void WriteBYTE(byte number, BinaryWriter fp)
        {
            fp.Write(number);
        }


        /// <summary>
        /// integer(2)のDWORD値を2byte分ファイルに書き込みます．
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fp"></param>
        public static void WriteWORD(ushort number, BinaryWriter fp)
        {
            byte k1, k2;
            k1 = (byte)(number >> 8);
            k2 = (byte)(number - (k1 << 8));
            fp.Write(k2);
            fp.Write(k1);
        }


        /// <summary>
        /// integer(4)のDWORD値を4byte分ファイルに書き込みます
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fp"></param>
        public static void WriteDWORD(uint number, BinaryWriter fp)
        {
            uint tmp;
            byte k1, k2, k3, k4;
            k1 = (byte)(number >> 24);
            number -= (uint)(k1 << 24);
            k2 = (byte)(number >> 16);
            number -= (uint)(k2 << 16);
            k3 = (byte)(number >> 8);
            k4 = (byte)(number - (k3 << 8));
            fp.Write(k4);
            fp.Write(k3);
            fp.Write(k2);
            fp.Write(k1);
        }


        /// <summary>
        /// integer(8)のQWORD値を8byte分ファイルに書き込みます
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fp"></param>
        public static void WriteQWORD(ulong number, BinaryWriter fp)
        {
            byte k1, k2, k3, k4, k5, k6, k7, k8;
            k1 = (byte)(number >> 56);
            number -= (ulong)k1 << 56;
            k2 = (byte)(number >> 48);
            number -= (ulong)k2 << 48;
            k3 = (byte)(number >> 40);
            number -= (ulong)k3 << 40;
            k4 = (byte)(number >> 32);
            number -= (ulong)k4 << 32;
            k5 = (byte)(number >> 24);
            number -= (ulong)k5 << 24;
            k6 = (byte)(number >> 16);
            number -= (ulong)k6 << 16;
            k7 = (byte)(number >> 8);
            k8 = (byte)(number - (ulong)(k7 << 8));
            fp.Write(k8);
            fp.Write(k7);
            fp.Write(k6);
            fp.Write(k5);
            fp.Write(k4);
            fp.Write(k3);
            fp.Write(k2);
            fp.Write(k1);
        }


        public static uint mmioFOURCC(string fcc)
        {
            char[] str = new char[4];
            for (int i = 0; i < 4; i++) {
                if (i < fcc.Length) {
                    str[i] = fcc[i];
                } else {
                    str[i] = ' ';
                }
            }
            return mmioFOURCC(str[0], str[1], str[2], str[3]);
        }


        public static uint mmioFOURCC(char ch0, char ch1, char ch2, char ch3)
        {
            return (uint)((byte)(ch0) | ((byte)(ch1) << 8) | ((byte)(ch2) << 16) | ((byte)(ch3) << 24));
        }
    }

}
