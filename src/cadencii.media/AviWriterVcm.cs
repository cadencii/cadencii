/*
 * AviWriterVcm.cs
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
using System.IO;
using System.Runtime.InteropServices;

using cadencii;

namespace cadencii.media
{

    public unsafe class AviWriterVcm : IAviWriter
    {
        private const long _THRESHOLD = 1000000000L; //AVIXリストの最大サイズ(byte)
        private MainAVIHeader m_main_header;
        private AVIStreamHeader m_stream_header;
        private BinaryWriter m_stream;
        private int m_current_chunk = 0;
        private long m_position_in_chunk = 0L;
        private long m_split_sreshold = 160000000L;  //AVIリストの最大サイズ(byte)
        private AVISTDINDEX m_std_index;
        private AVISUPERINDEX m_super_index;
        private int m_linesize;//bitmapの1行分のデータサイズ(byte)
        private bool m_is_first = true;
        private long m_riff_position;//"RIFF*"の*が記入されているファイルの絶対位置。RIFF-AVI のときは必ず4。RIFF-AVIXの時は変化する
        private long m_movi_position;
        private long m_next_framedata_position;//次に00dbデータを記入するべき場所
        private long m_avix_position;
        private int m_junk_length;
        private uint m_scale;
        private uint m_rate;
        private bool m_compressed = false;
        private IntPtr m_hwnd = IntPtr.Zero;
        private uint m_stream_fcc_handler = 808810089;
        private string m_file;
        //private int m_addr_compvar;
        //private COMPVARS m_compvar;
        private COMPVARS* m_compvar;
        private BITMAPINFO* m_bitmapinfo_in;
        private BITMAPINFO* m_bitmapinfo_out;
        private IntPtr m_p_compvar;
        private IntPtr m_p_bitmapinfo_in;
        private IntPtr m_p_bitmapinfo_out;
        private uint m_bih_compression = 0;
        private long m_super_index_position;
        private bool m_closed = false;
        private bool m_opened = false;
        private bool m_is_transparent = false;
        /// <summary>
        /// 現在記入中のmoviチャンクのサイズ
        /// </summary>
        private long m_this_movi_size = 0;

        private static bool s_vfw_bug_compati = false;

        public Size Size
        {
            get
            {
                return new Size((int)m_main_header.dwWidth, (int)m_main_header.dwHeight);
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

        /// <summary>
        /// Video For Windows APIとバグコンパチブルな動作をするかどうかを表す値を取得または設定します
        /// </summary>
        public static bool VfwBugCompatible
        {
            get
            {
                return s_vfw_bug_compati;
            }
            set
            {
                s_vfw_bug_compati = value;
            }
        }

        internal float frameRate
        {
            get
            {
                return (float)m_rate / (float)m_scale;
            }
        }

        /// <summary>
        /// 指定したAVI_CONTAINER構造体にAVIファイルの情報を格納すると共に，
        /// ファイルにヘッダー情報を書き込みます．
        /// </summary>
        /// <param name="file">書き込み対象のファイル</param>
        /// <param name="scale"></param>
        /// <param name="rate"></param>
        /// <param name="compressed"></param>
        public unsafe bool Open(string file, uint scale, uint rate, int width, int height, bool compressed, bool transparent, IntPtr hWnd)
        {
#if DEBUG
            Console.WriteLine("AviWriterEx.Open(string,uint,uint,bool,IntPtr)");
#endif
            m_stream = new BinaryWriter(new FileStream(file, FileMode.Create, FileAccess.Write));
            float fps = (float)rate / (float)scale;
            m_main_header.dwMicroSecPerFrame = (uint)(1.0e6 / fps);//  ! 1秒は10^6μ秒
            m_main_header.dwReserved1 = 0;
            m_main_header.dwFlags = 2064;
            m_main_header.dwInitialFrames = 0;
            m_main_header.dwStreams = 0;
            m_main_header.dwScale = scale;
            m_main_header.dwRate = rate;
            m_main_header.dwStart = 0;
            m_main_header.dwLength = 0;
            m_rate = rate;
            m_scale = scale;
            Util.fwrite("RIFF", m_stream);
            Util.WriteDWORD(0, m_stream);
            Util.fwrite("AVI ", m_stream);
            Util.fwrite("LIST", m_stream);
            Util.WriteDWORD(0x9cc, m_stream);
            Util.fwrite("hdrl", m_stream);
            m_current_chunk = 0;
            m_position_in_chunk = 0L;
            m_std_index = new AVISTDINDEX(0L);
            m_super_index = new AVISUPERINDEX(0);
            m_riff_position = 0x4;
            m_compressed = compressed;
            m_is_transparent = transparent;
            m_stream_fcc_handler = 0;
            m_hwnd = hWnd;
            m_file = file;
            m_opened = true;
            if (m_is_first) {
                int stride = 0;
                using (Bitmap b = new Bitmap(width, height, m_is_transparent ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb)) {
                    BitmapData bd = b.LockBits(new Rectangle(0, 0, width, height),
                                                ImageLockMode.ReadOnly,
                                                b.PixelFormat);
                    stride = bd.Stride;
                    b.UnlockBits(bd);
                }
                m_is_first = false;
                m_main_header.dwWidth = (uint)width;
                m_main_header.dwHeight = (uint)height;
                m_main_header.dwMaxBytesPerSec = (uint)(stride * height * frameRate);
                m_main_header.dwStreams = 1;
                m_main_header.dwSuggestedBufferSize = (uint)(stride * height);
                m_linesize = stride;

                m_stream_header.fccType = Util.mmioFOURCC("vids");
                m_stream_header.fccHandler = 0;
                m_stream_header.dwFlags = 0;
                m_stream_header.dwReserved1 = 0;
                m_stream_header.dwInitialFrames = 0;
                m_stream_header.dwScale = m_scale;
                m_stream_header.dwRate = m_rate;
                m_stream_header.dwStart = 0;
                m_stream_header.dwSuggestedBufferSize = m_main_header.dwSuggestedBufferSize;
                m_stream_header.dwQuality = 0;
                m_stream_header.dwSampleSize = 0;

                Util.aviWriteMainHeader(m_main_header, m_stream);

                Util.fwrite("LIST", m_stream);
                Util.WriteDWORD(0x874, m_stream);
                Util.fwrite("strl", m_stream);

                Util.aviWriteStreamHeader(m_stream_header, m_main_header, m_stream);

                Util.fwrite("strf", m_stream);
                BITMAPINFOHEADER bih = new BITMAPINFOHEADER(); //(BITMAPINFOHEADER)Marshal.PtrToStructure( Marshal.AllocHGlobal( sizeof( BITMAPINFOHEADER ) ), typeof( BITMAPINFOHEADER ) );
                bih.biSize = (uint)(Marshal.SizeOf(bih));
                bih.biWidth = width;
                bih.biHeight = height;
                bih.biPlanes = 1;
                bih.biBitCount = m_is_transparent ? (short)32 : (short)24;
                bih.biCompression = 0;//BI_RGB
                bih.biSizeImage = (uint)(stride * height);
                bih.biXPelsPerMeter = 0;
                bih.biYPelsPerMeter = 0;
                bih.biClrUsed = 0;
                bih.biClrImportant = 0;

                if (m_compressed) {
                    m_p_compvar = Marshal.AllocHGlobal(sizeof(COMPVARS));
                    m_compvar = (COMPVARS*)m_p_compvar.ToPointer();
                    byte[] buf = new byte[sizeof(COMPVARS)];
                    for (int i = 0; i < buf.Length; i++) {
                        buf[i] = 0x0;
                    }
                    Marshal.Copy(buf, 0, m_p_compvar, buf.Length);
                    m_compvar->cbSize = sizeof(COMPVARS);
                    int ret = VCM.ICCompressorChoose(m_hwnd, 0, IntPtr.Zero, IntPtr.Zero, m_compvar, "Select Video Compressor");
                    if (ret == 0) {
                        m_opened = false;
                        Marshal.FreeHGlobal(m_p_compvar);
                        m_stream.Close();
                        return false;
                    }
                    if (m_compvar->hic != 0) {
                        m_p_bitmapinfo_in = Marshal.AllocHGlobal(sizeof(BITMAPINFO));
                        m_bitmapinfo_in = (BITMAPINFO*)m_p_bitmapinfo_in.ToPointer();
                        buf = new byte[sizeof(BITMAPINFO)];
                        for (int i = 0; i < buf.Length; i++) {
                            buf[i] = 0x0;
                        }
                        Marshal.Copy(buf, 0, m_p_bitmapinfo_in, buf.Length);
                        m_bitmapinfo_in->bmiHeader = bih;
                        uint dwSize = VCM.ICCompressGetFormatSize(m_compvar->hic, m_bitmapinfo_in);
#if DEBUG
                        Console.WriteLine("m_compvar->hic=" + m_compvar->hic);
                        Console.WriteLine("ICCompressGetFormatSize=" + dwSize);
#endif
                        m_p_bitmapinfo_out = Marshal.AllocHGlobal((int)dwSize);
                        m_bitmapinfo_out = (BITMAPINFO*)m_p_bitmapinfo_out.ToPointer();
                        buf = new byte[dwSize];
                        for (int i = 0; i < buf.Length; i++) {
                            buf[i] = 0x0;
                        }
                        Marshal.Copy(buf, 0, m_p_bitmapinfo_out, buf.Length);
                        VCM.ICCompressGetFormat(m_compvar->hic, m_bitmapinfo_in, m_bitmapinfo_out);
                        m_bih_compression = m_bitmapinfo_out->bmiHeader.biCompression;
#if DEBUG
                        Console.WriteLine("AddFrame(Bitmap)");
                        Console.WriteLine("    biout.biSize=" + m_bitmapinfo_out->bmiHeader.biSize);
#endif
                        VCM.ICSeqCompressFrameStart(m_compvar, m_bitmapinfo_in);
                        bih = m_bitmapinfo_out->bmiHeader;
                        Util.WriteDWORD(bih.biSize, m_stream);// infoHeaderのサイズ
                        m_bitmapinfo_out->Write(m_stream);
                    } else {
                        m_compressed = false;
                        Util.WriteDWORD(bih.biSize, m_stream);// infoHeaderのサイズ
                        bih.Write(m_stream);
                    }
                } else {
                    Util.WriteDWORD(bih.biSize, m_stream);// infoHeaderのサイズ
                    bih.Write(m_stream);
                }

                m_super_index_position = m_stream.BaseStream.Position;
                Util.fwrite("indx", m_stream);          //fcc
                Util.WriteDWORD(0x7f8, m_stream);       // cb
                Util.WriteWORD((byte)0x4, m_stream);    // wLongsPerEntry
                Util.WriteBYTE(0x0, m_stream);          // bIndexSubType
                Util.WriteBYTE(Util.AVI_INDEX_OF_INDEXES, m_stream);// bIndexType
                Util.WriteDWORD(0x0, m_stream);         // nEntriesInUse
                Util.fwrite("00db", m_stream);          // dwChunkId
                Util.WriteDWORD(0x0, m_stream);
                Util.WriteDWORD(0x0, m_stream);
                Util.WriteDWORD(0x0, m_stream);
                for (int ii = 1; ii <= 126; ii++) {
                    Util.WriteQWORD(0x0, m_stream);
                    Util.WriteDWORD(0x0, m_stream);
                    Util.WriteDWORD(0x0, m_stream);
                }

                Util.fwrite("LIST", m_stream);
                Util.WriteDWORD(0x104, m_stream);
                Util.fwrite("odml", m_stream);
                Util.fwrite("dmlh", m_stream);
                Util.WriteDWORD(0xf8, m_stream);
                Util.WriteDWORD(0x0, m_stream);//ここ後で更新するべき
                for (int ii = 1; ii <= 61; ii++) {
                    Util.WriteDWORD(0x0, m_stream);
                }

                Util.fwrite("JUNK", m_stream);
                Util.WriteDWORD(0x60c, m_stream);
                Util.WriteDWORD(0, m_stream);//"This"が将来登録されたらやばいので
                string msg = "This file was generated by AviWriter@Boare.Lib.Media;VfwBugCompatible=" + VfwBugCompatible;
                const int tlen = 1544;
                int remain = tlen - msg.Length;
                Util.fwrite(msg, m_stream);
                for (int i = 1; i <= remain; i++) {
                    m_stream.Write((byte)0);
                }
                m_junk_length = 0xff4;

                Util.fwrite("LIST", m_stream);
                m_movi_position = m_stream.BaseStream.Position;
                Util.WriteDWORD(0, m_stream);// call bmpQWordWrite( 0, avi%fp )     !// ******************ココの数字は一番最後に書き換える必要あり2040～2043あとdwTotalFrames（48～51）も
                Util.fwrite("movi", m_stream);
                m_next_framedata_position = m_stream.BaseStream.Position;

                m_std_index.SetBaseOffset((ulong)m_next_framedata_position);
                m_super_index.nEntriesInUse++;
            }
            return true;
        }

        public bool Open(string file, uint scale, uint rate, int width, int height, IntPtr hwnd)
        {
            return Open(file, scale, rate, width, height, false, false, hwnd);
        }

        //todo: AVIMainHeader.dwTotalFramesに、ファイル全体のフレーム数を入れる（仕様違反）
        //todo: AVIStreamHeader.dwLengthに、ファイル全体のフレーム数を入れる（仕様違反）
        /// <summary>
        /// 全てのインデックスを更新し、ファイルが（動画ファイルとして）使用できる状態にします
        /// この関数を読んだあとでも，さらにaviAddFrame関数を使うことでフレームを追加することが出来ます．
        /// </summary>
        public void UpdateIndex()
        {
            _avisuperindex_entry entry = m_super_index.aIndex[m_current_chunk];
            entry.qwOffset = (ulong)m_stream.BaseStream.Position;
            entry.dwSize = m_std_index.cb;
            m_super_index.aIndex[m_current_chunk] = entry;
            if (m_stream.BaseStream.Position != m_next_framedata_position) {
                m_stream.BaseStream.Seek(m_next_framedata_position, SeekOrigin.Begin);
            }
            m_std_index.Write(m_stream);
            int frames = (int)m_super_index.aIndex[m_current_chunk].dwDuration;
            m_avix_position = m_stream.BaseStream.Position;

            if (m_current_chunk == 0) {
                uint i, step, number;
                step = m_main_header.dwSuggestedBufferSize + 8;

                Util.fwrite("idx1", m_stream);
                Util.WriteDWORD((uint)(16 * frames), m_stream);
                for (i = 1; i <= frames; i++) {
                    Util.fwrite("00db", m_stream);
                    Util.WriteDWORD(Util.AVIF_HASINDEX, m_stream);
                    Util.WriteDWORD((uint)(4 + (i - 1) * step), m_stream);
                    Util.WriteDWORD(m_main_header.dwSuggestedBufferSize, m_stream);
                }
                m_avix_position = m_stream.BaseStream.Position;

                number = (uint)frames;
                m_stream.Seek(0x30, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);
                m_stream.Seek(0x8c, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);

                //odml dlmhのdwTotalFrames
                m_stream.Seek(0x8e8, SeekOrigin.Begin);
                Util.WriteDWORD((uint)frames, m_stream);

                // LIST****moviの****の数値を計算
                number = 4;//"movi"
                number += (uint)(m_this_movi_size + 8 * frames);//                (uint)(frames * (m_linesize * m_main_header.dwHeight + 8));//フレーム数*(フレームのサイズ+"00db"+00dbチャンクのサイズ)
                number += 4 + 4 + (uint)m_std_index.cb; //ix00のサイズ
                m_stream.BaseStream.Seek(m_movi_position, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);

                //number = 4096 + (this.mainHeader.dwSuggestedBufferSize + 24) * this.noOfFrame;
                //avi_writeIsolate( this.fp, number, 4 );     // RIFF****AVI  の ****部分
                number = (uint)m_junk_length/* 0xff4*/; //JUNKの終わりまでのサイズ。これは固定
                number += 4;//"movi"
                number += (uint)(m_this_movi_size + 8 * frames);//00db...の合計サイズ
                number += 4 + 4 + (uint)m_std_index.cb;
                number += (uint)(4 + 4 + 16 * frames);//idx1のサイズ
                m_stream.BaseStream.Seek(m_riff_position, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);
                UpdateIndexOfIndex();
            } else {
                // LIST****moviの****を更新
                uint number = 4;
                number += (uint)(m_this_movi_size + frames * 8);
                number += 8 + (uint)m_std_index.cb;
                m_stream.BaseStream.Seek(m_movi_position, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);

                // odml dlmhのdwTotalFrames
                uint frames2 = 0;
                for (int j = 0; j <= m_current_chunk; j++) {
                    frames2 += (uint)m_super_index.aIndex[j].dwDuration;
                }
                m_stream.BaseStream.Seek(0x8e8, SeekOrigin.Begin);
                Util.WriteDWORD(frames2, m_stream);

                m_stream.Seek(48, SeekOrigin.Begin);
                Util.WriteDWORD(frames2, m_stream);

                m_stream.Seek(140, SeekOrigin.Begin);
                Util.WriteDWORD(frames2, m_stream);

                //RIFF****AVIXの****を更新
                long num2 = m_junk_length + number;
                m_stream.BaseStream.Seek(m_riff_position, SeekOrigin.Begin);
                Util.WriteDWORD((uint)num2, m_stream);
                UpdateIndexOfIndex();
            }
        }

        /// <summary>
        /// aviファイルを閉じます
        /// </summary>
        public unsafe void Close()
        {
            UpdateIndex();
            m_stream.Close();
            if (m_compressed) {
                m_stream_fcc_handler = m_compvar->fccHandler;
                VCM.ICSeqCompressFrameEnd(m_compvar);
                VCM.ICCompressorFree(m_compvar);
                using (FileStream fs = new FileStream(m_file, FileMode.Open)) {
                    fs.Seek(0x70, SeekOrigin.Begin);
                    {
                        byte ch3 = (byte)(m_stream_fcc_handler >> 24);
                        uint b = (uint)(m_stream_fcc_handler - (ch3 << 24));
                        byte ch2 = (byte)(b >> 16);
                        b = (uint)(b - (ch2 << 16));
                        byte ch1 = (byte)(b >> 8);
                        byte ch0 = (byte)(b - (ch1 << 8));
                        fs.Write(new byte[] { ch0, ch1, ch2, ch3 }, 0, 4);
                    }
                    fs.Seek(0xbc, SeekOrigin.Begin);
                    {
                        byte ch3 = (byte)(m_bih_compression >> 24);
                        uint b = (uint)(m_bih_compression - (ch3 << 24));
                        byte ch2 = (byte)(b >> 16);
                        b = (uint)(b - (ch2 << 16));
                        byte ch1 = (byte)(b >> 8);
                        byte ch0 = (byte)(b - (ch1 << 8));
                        fs.Write(new byte[] { ch0, ch1, ch2, ch3 }, 0, 4);
                    }
                }
            }
            m_closed = true;
        }

        ~AviWriterVcm()
        {
            if (m_opened && !m_closed) {
                Close();
            }
        }

        /// <summary>
        /// 最初の[AVI :AVI[LIST:hdrl[LIST:strl]]]に書き込まれているsuper indexチャンク[indx]を更新します
        /// </summary>
        private void UpdateIndexOfIndex()
        {
            m_stream.Seek((int)m_super_index_position, SeekOrigin.Begin);
            m_super_index.Write(m_stream);
        }


        /// <summary>
        /// aviファイルにフレームを1つ追加します．
        /// </summary>
        /// <param name="bmp"></param>
        public unsafe void AddFrame(Bitmap bmp)
        {
            int width, height, lineSize;
            BitmapData bmpDat;
            if (m_is_transparent) {
                bmpDat = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                       ImageLockMode.ReadOnly,
                                       PixelFormat.Format32bppArgb);
            } else {
                bmpDat = bmp.LockBits(new Rectangle(0, 0, (int)bmp.Width, (int)bmp.Height),
                                       ImageLockMode.ReadOnly,
                                       PixelFormat.Format24bppRgb);
            }

            if (m_next_framedata_position != m_stream.BaseStream.Position) {
                m_stream.BaseStream.Seek(m_next_framedata_position, SeekOrigin.Begin);
            }

            long chunk_size = m_next_framedata_position - m_riff_position;
            if ((m_current_chunk == 0 && chunk_size > m_split_sreshold) ||
                 (m_current_chunk > 0 && chunk_size > _THRESHOLD)) {
                // AVIXリストへの書き込みに移行
                UpdateIndex();
                m_stream.BaseStream.Seek(m_avix_position, SeekOrigin.Begin);
                Util.fwrite("RIFF", m_stream);
                m_riff_position = m_stream.BaseStream.Position;
                Util.WriteDWORD(0, m_stream);
                Util.fwrite("AVIX", m_stream);
                long current = m_stream.BaseStream.Position;
                if ((current + 12) % 0x800 != 0) {
                    long additional = (current + 20) % 0x800;
                    additional = 0x800 - ((current + 20) % 0x800);
                    m_junk_length = (int)additional + 20;
                    Util.fwrite("JUNK", m_stream);
                    Util.WriteDWORD((uint)additional, m_stream);
                    for (long ii = 0; ii < additional; ii++) {
                        Util.WriteBYTE((byte)0, m_stream);
                    }
                } else {
                    m_junk_length = 0;
                }
                m_junk_length = 0;

                Util.fwrite("LIST", m_stream);
                m_movi_position = m_stream.BaseStream.Position;
                Util.WriteDWORD(0, m_stream);//後で更新するべき
                Util.fwrite("movi", m_stream);
                m_next_framedata_position = m_stream.BaseStream.Position;
                m_std_index.aIndex.Clear();
                m_std_index.SetBaseOffset((ulong)m_next_framedata_position);
                m_current_chunk++;
                m_super_index.nEntriesInUse++;
            }

            // フレームを書き込む処理
            width = (int)m_main_header.dwWidth;
            height = (int)m_main_header.dwHeight;
            if (width != bmpDat.Width) {
                return;
            }
            if (height != bmpDat.Height) {
                return;
            }
            lineSize = bmpDat.Stride;

            if (m_compressed) {
                int is_key_frame = 0;
                int size = bmpDat.Stride * bmpDat.Height;
                try {
                    IntPtr dat = VCM.ICSeqCompressFrame(m_compvar, 0, bmpDat.Scan0, &is_key_frame, &size);
                    if (!dat.Equals(IntPtr.Zero)) {
                        byte[] ndat = new byte[size];
                        Marshal.Copy(dat, ndat, 0, size);
                        m_std_index.AddIndex((uint)((ulong)m_stream.BaseStream.Position - m_std_index.qwBaseOffset) + 8, (uint)size);
                        Util.fwrite("00db", m_stream);
                        Util.WriteDWORD((uint)size, m_stream);
                        m_stream.Write(ndat, 0, size);
                        m_this_movi_size += size;
                    }
                } catch {
                }
            } else {
                m_std_index.AddIndex((uint)((ulong)m_stream.BaseStream.Position - m_std_index.qwBaseOffset) + 8, (uint)(lineSize * height));
                Util.fwrite("00db", m_stream);
                int address = bmpDat.Scan0.ToInt32();
                byte[] bitmapData = new byte[bmpDat.Stride * bmpDat.Height];
                Marshal.Copy(new IntPtr(address), bitmapData, 0, bitmapData.Length);
                Util.WriteDWORD(m_main_header.dwSuggestedBufferSize, m_stream);
                m_stream.Write(bitmapData);
                m_this_movi_size += bitmapData.Length;
            }
            m_next_framedata_position = m_stream.BaseStream.Position;
            _avisuperindex_entry entry = m_super_index.aIndex[m_current_chunk];
            entry.dwDuration++;
            m_super_index.aIndex[m_current_chunk] = entry;
            m_stream.Flush();
            bmp.UnlockBits(bmpDat);
        }
    }
}
