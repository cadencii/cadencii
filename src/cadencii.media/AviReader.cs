/*
 * AviReader.cs
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

using cadencii;

namespace cadencii.media
{


    /// <summary>
    /// Extract bitmaps from AVI files
    /// </summary>
    public class AviReader
    {
        private int firstFrame = 0, countFrames = 0;
        private int aviFile = 0;
        private int getFrameObject;
        private IntPtr aviStream;
        private bool m_opened = false;
        private AVISTREAMINFOW streamInfo;
        private string m_fileName = "";

        public string FileName
        {
            get
            {
                return m_fileName;
            }
        }

        public int CountFrames
        {
            get
            {
                return countFrames;
            }
        }

        public uint dwRate
        {
            get
            {
                return streamInfo.dwRate;
            }
        }

        public uint dwScale
        {
            get
            {
                return streamInfo.dwScale;
            }
        }

        [Obsolete]
        public float FrameRate
        {
            get
            {
                return (float)streamInfo.dwRate / (float)streamInfo.dwScale;
            }
        }

        public Size BitmapSize
        {
            get
            {
                return new Size((int)streamInfo.rect_right, (int)streamInfo.rect_bottom);
            }
        }

        /// <summary>
        /// Opens an AVI file and creates a GetFrame object
        /// </summary>
        /// <param name="fileName">Name of the AVI file</param>
        public void Open(string fileName)
        {
            //Intitialize AVI library
            VFW.AVIFileInit();

            //Open the file
            int result = VFW.AVIFileOpenW(
                ref aviFile, fileName,
                (int)win32.OF_SHARE_DENY_WRITE, 0);

            if (result != 0) {
                throw new Exception("Exception in AVIFileOpen: " + result.ToString());
            }

            //Get the video stream
            result = VFW.AVIFileGetStream(
                aviFile,
                out aviStream,
                (int)VFW.streamtypeVIDEO, 0);

            if (result != 0) {
                throw new Exception("Exception in AVIFileGetStream: " + result.ToString());
            }

            firstFrame = VFW.AVIStreamStart(aviStream.ToInt32());
            countFrames = VFW.AVIStreamLength(aviStream.ToInt32());

            streamInfo = new AVISTREAMINFOW();
            result = VFW.AVIStreamInfo(aviStream.ToInt32(), ref streamInfo, Marshal.SizeOf(streamInfo));

            if (result != 0) {
                throw new Exception("Exception in AVIStreamInfo: " + result.ToString());
            }

            //Open frames

            BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
            bih.biBitCount = 24;
            bih.biClrImportant = 0;
            bih.biClrUsed = 0;
            bih.biCompression = 0; //BI_RGB;
            bih.biHeight = (Int32)streamInfo.rect_bottom;
            bih.biWidth = (Int32)streamInfo.rect_right;
            bih.biPlanes = 1;
            bih.biSize = (UInt32)Marshal.SizeOf(bih);
            bih.biXPelsPerMeter = 0;
            bih.biYPelsPerMeter = 0;

            getFrameObject = VFW.AVIStreamGetFrameOpen(aviStream, ref bih); //force function to return 24bit DIBS
            //getFrameObject = Avi.AVIStreamGetFrameOpen(aviStream, 0); //return any bitmaps
            if (getFrameObject == 0) {
                throw new Exception("Exception in AVIStreamGetFrameOpen!");
            }
            m_opened = true;
            m_fileName = fileName;
        }

        /// <summary>Closes all streams, files and libraries</summary>
        public void Close()
        {
            if (getFrameObject != 0) {
                VFW.AVIStreamGetFrameClose(getFrameObject);
                getFrameObject = 0;
            }
            if (aviStream != IntPtr.Zero) {
                VFW.AVIStreamRelease(aviStream);
                aviStream = IntPtr.Zero;
            }
            if (aviFile != 0) {
                VFW.AVIFileRelease(aviFile);
                aviFile = 0;
            }
            VFW.AVIFileExit();
            m_opened = false;
            m_fileName = "";
        }


        private void dispTime(string message, DateTime start)
        {
            DateTime current = DateTime.Now;
            TimeSpan ts = current.Subtract(start);
            System.Diagnostics.Debug.WriteLine(message + "; " + ts.TotalSeconds + "s");
        }


        public bool Opened
        {
            get
            {
                return m_opened;
            }
        }


        public void ExportToArray(int position, out byte[] bitmapData, out int width, out int height, out int bit_count)
        {
            if (position > countFrames) {
                throw new Exception("Invalid frame position");
            }
            //Decompress the frame and return a pointer to the DIB
            int pDib = VFW.AVIStreamGetFrame(getFrameObject, firstFrame + position);
            //Copy the bitmap header into a managed struct
            BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
            bih = (BITMAPINFOHEADER)Marshal.PtrToStructure(new IntPtr(pDib), bih.GetType());

            if (bih.biSizeImage < 1) {
                throw new Exception("Exception in AVIStreamGetFrame: Not bitmap decompressed.");
            }

            //Copy the image
            bitmapData = new byte[bih.biSizeImage];
            int address = pDib + Marshal.SizeOf(bih);
            Marshal.Copy(new IntPtr(address), bitmapData, 0, bitmapData.Length);
            width = bih.biWidth;
            height = bih.biHeight;
            bit_count = bih.biBitCount;
        }


        public Bitmap Export(int position)
        {
            if (position > countFrames) {
                throw new Exception("Invalid frame position");
            }
            //Decompress the frame and return a pointer to the DIB
            int pDib = VFW.AVIStreamGetFrame(getFrameObject, firstFrame + position);
            if (pDib == 0) {
                //throw new Exception( "AVIStreamGetFrame; failed" );
                return null;
            }
            //Copy the bitmap header into a managed struct
            BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
            bih = (BITMAPINFOHEADER)Marshal.PtrToStructure(new IntPtr(pDib), bih.GetType());
            if (bih.biSizeImage < 1) {
                throw new Exception("Exception in AVIStreamGetFrame: Not bitmap decompressed.");
            }
            Bitmap result = new Bitmap(bih.biWidth, bih.biHeight);
            BitmapData dat = result.LockBits(
                new Rectangle(0, 0, bih.biWidth, bih.biHeight),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            //Copy the image
            int address = pDib + Marshal.SizeOf(bih);
            int length = dat.Stride * dat.Height;
            byte[] target = new byte[length];
            Marshal.Copy(new IntPtr(address), target, 0, length);
            Marshal.Copy(target, 0, dat.Scan0, length);
            result.UnlockBits(dat);
            result.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return result;
        }


        public unsafe void ExportEx(Bitmap bmp, int position)
        {
            if (position > countFrames) {
                throw new Exception("Invalid frame position");
            }
            //Decompress the frame and return a pointer to the DIB
            int pDib = VFW.AVIStreamGetFrame(getFrameObject, firstFrame + position);
            if (pDib == 0) {
                //throw new Exception( "AVIStreamGetFrame; failed" );
                return;
            }
            //Copy the bitmap header into a managed struct
            BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
            bih = (BITMAPINFOHEADER)Marshal.PtrToStructure(new IntPtr(pDib), bih.GetType());
            if (bih.biSizeImage < 1) {
                throw new Exception("Exception in AVIStreamGetFrame: Not bitmap decompressed.");
            }
            BitmapData dat = bmp.LockBits(
                new Rectangle(0, 0, bih.biWidth, bih.biHeight),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            //Copy the image
            int address = pDib + Marshal.SizeOf(bih);
            int length = dat.Stride * dat.Height;
            CopyMemory(dat.Scan0, new IntPtr(address), length);
            //Marshal.Copy( new IntPtr( address ), target, 0, length );
            //Marshal.Copy( target, 0, dat.Scan0, length );
            bmp.UnlockBits(dat);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }


        [DllImport("kernel32.dll")]
        private static extern void CopyMemory(
            IntPtr destination, IntPtr source, int length);

        public unsafe void Export(Bitmap bmp, int position)
        {
            if (position > countFrames) {
                throw new Exception("Invalid frame position");
            }
            //Decompress the frame and return a pointer to the DIB
            int pDib = VFW.AVIStreamGetFrame(getFrameObject, firstFrame + position);
            if (pDib == 0) {
                //throw new Exception( "AVIStreamGetFrame; failed" );
                return;
            }
            //Copy the bitmap header into a managed struct
            BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
            bih = (BITMAPINFOHEADER)Marshal.PtrToStructure(new IntPtr(pDib), bih.GetType());
            if (bih.biSizeImage < 1) {
                throw new Exception("Exception in AVIStreamGetFrame: Not bitmap decompressed.");
            }
            BitmapData dat = bmp.LockBits(
                new Rectangle(0, 0, bih.biWidth, bih.biHeight),
                ImageLockMode.WriteOnly,
                bmp.PixelFormat);
            //Copy the image
            int address = pDib + Marshal.SizeOf(bih);
            int length = dat.Stride * dat.Height;
            byte[] target = new byte[length];
            Marshal.Copy(new IntPtr(address), target, 0, length);
            Marshal.Copy(target, 0, dat.Scan0, length);
            bmp.UnlockBits(dat);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }



        /// <summary>Exports a frame into a bitmap file</summary>
        /// <param name="position">Position of the frame</param>
        /// <param name="dstFileName">Name ofthe file to store the bitmap</param>
        public void ExportBitmap(int position, string dstFileName)
        {
            if (position > countFrames) {
                throw new Exception("Invalid frame position");
            }

            //Decompress the frame and return a pointer to the DIB
            int pDib = VFW.AVIStreamGetFrame(getFrameObject, firstFrame + position);
            //Copy the bitmap header into a managed struct
            BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
            bih = (BITMAPINFOHEADER)Marshal.PtrToStructure(new IntPtr(pDib), bih.GetType());

            /*if(bih.biBitCount < 24){
                throw new Exception("Not enough colors! DIB color depth is less than 24 bit.");
            }else */
            if (bih.biSizeImage < 1) {
                throw new Exception("Exception in AVIStreamGetFrame: Not bitmap decompressed.");
            }

            //Copy the image
            byte[] bitmapData = new byte[bih.biSizeImage];
            int address = pDib + Marshal.SizeOf(bih);
            for (int offset = 0; offset < bitmapData.Length; offset++) {
                bitmapData[offset] = Marshal.ReadByte(new IntPtr(address));
                address++;
            }

            //Copy bitmap info
            byte[] bitmapInfo = new byte[Marshal.SizeOf(bih)];
            IntPtr ptr;
            ptr = Marshal.AllocHGlobal(bitmapInfo.Length);
            Marshal.StructureToPtr(bih, ptr, false);
            address = ptr.ToInt32();
            for (int offset = 0; offset < bitmapInfo.Length; offset++) {
                bitmapInfo[offset] = Marshal.ReadByte(new IntPtr(address));
                address++;
            }

            //Create file header
            BITMAPFILEHEADER bfh = new BITMAPFILEHEADER();
            bfh.bfType = Util.BMP_MAGIC_COOKIE;
            bfh.bfSize = (Int32)(55 + bih.biSizeImage); //size of file as written to disk
            bfh.bfReserved1 = 0;
            bfh.bfReserved2 = 0;
            bfh.bfOffBits = Marshal.SizeOf(bih) + Marshal.SizeOf(bfh);

            //Create or overwrite the destination file
            FileStream fs = new FileStream(dstFileName, System.IO.FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            //Write header
            bw.Write(bfh.bfType);
            bw.Write(bfh.bfSize);
            bw.Write(bfh.bfReserved1);
            bw.Write(bfh.bfReserved2);
            bw.Write(bfh.bfOffBits);
            //Write bitmap info
            bw.Write(bitmapInfo);
            //Write bitmap data
            bw.Write(bitmapData);
            bw.Close();
            fs.Close();
        }
    }

}
