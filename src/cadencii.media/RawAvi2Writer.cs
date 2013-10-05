/*
 * RawAvi2Writer.cs
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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
#if DEBUG
using System.Windows.Forms;
#endif

using cadencii;

namespace cadencii.media
{

    public struct _avistdindex_entry
    {
        public UInt32 dwOffset;
        public UInt32 dwSize;
    }


    public struct AVISTDINDEX
    {
        public readonly string fcc;
        //private UInt32 m_cb;
        public UInt32 cb
        {
            get
            {
                if (aIndex == null) {
                    return 24;
                } else {
                    return (uint)(24 + 8 * aIndex.Count);
                }
            }
        }
        public readonly UInt16 wLongsPerEntry;
        public readonly byte bIndexSubType;
        public readonly byte bIndexType;
        //private UInt32 m_nEntriesInUse;
        public UInt32 nEntriesInUse
        {
            get
            {
                if (aIndex == null) {
                    return 0;
                } else {
                    return (uint)aIndex.Count;
                }
            }
        }
        public readonly string dwChunkId;
        public UInt64 qwBaseOffset;
        public readonly UInt32 dwReserved3;
        public List<_avistdindex_entry> aIndex;
        public AVISTDINDEX(ulong BaseOffset)
        {
            fcc = "ix00";
            wLongsPerEntry = 2;
            bIndexSubType = 0;
            bIndexType = Util.AVI_INDEX_OF_CHUNKS;
            dwChunkId = "00db";
            qwBaseOffset = BaseOffset;
            dwReserved3 = 0;
            //m_nEntriesInUse = 0;
            aIndex = new List<_avistdindex_entry>();
        }
        public void SetBaseOffset(UInt64 base_offset)
        {
            this.qwBaseOffset = base_offset;
        }
        public void AddIndex(uint dwOffset, uint dwSize)
        {
            _avistdindex_entry entry = new _avistdindex_entry();
            entry.dwOffset = dwOffset;
            entry.dwSize = dwSize;
            this.aIndex.Add(entry);
        }
        public void Write(BinaryWriter fp)
        {
            Util.fwrite(fcc, fp);
            Util.WriteDWORD((uint)cb, fp);
            Util.WriteWORD(wLongsPerEntry, fp);
            Util.WriteBYTE(bIndexSubType, fp);
            Util.WriteBYTE(bIndexType, fp);
            Util.WriteDWORD((uint)nEntriesInUse, fp);
            Util.fwrite(dwChunkId, fp);
            Util.WriteQWORD((ulong)qwBaseOffset, fp);
            Util.WriteDWORD((uint)dwReserved3, fp);
            foreach (_avistdindex_entry entry in aIndex) {
                Util.WriteDWORD((uint)entry.dwOffset, fp);
                Util.WriteDWORD((uint)entry.dwSize, fp);
            }
        }
    }


    public struct _avisuperindex_entry
    {
        public UInt64 qwOffset;
        public UInt32 dwSize;
        public UInt32 dwDuration;
    }


    public struct AVISUPERINDEX
    {
        public readonly string fcc;
        //public UInt32 cb;
        public UInt32 cb
        {
            get
            {
                if (aIndex == null) {
                    return 24;
                } else {
                    return (uint)(24 + 16 * aIndex.Count);
                }
            }
        }
        public readonly UInt16 wLongsPerEntry;
        public byte bIndexSubType;
        public byte bIndexType;
        public UInt32 nEntriesInUse;
        public readonly string dwChunkId;
        public UInt32 dwReserved1;
        public UInt32 dwReserved2;
        public UInt32 dwReserved3;
        public List<_avisuperindex_entry> aIndex;
        public AVISUPERINDEX(int dumy)
        {
            this.fcc = "indx";
            this.wLongsPerEntry = 4;
            this.bIndexSubType = 0;
            this.bIndexType = Util.AVI_INDEX_OF_INDEXES;
            this.nEntriesInUse = 0;
            this.dwChunkId = "00db";
            this.dwReserved1 = 0;
            this.dwReserved2 = 0;
            this.dwReserved3 = 0;
            this.aIndex = new List<_avisuperindex_entry>();
            _avisuperindex_entry entry = new _avisuperindex_entry();
            entry.qwOffset = 0;
            entry.dwSize = 0;
            entry.dwDuration = 0;
            for (int i = 0; i < 126; i++) {
                this.aIndex.Add(entry);
            }
        }
        public void Write(BinaryWriter fp)
        {
            Util.fwrite(fcc, fp);
            Util.WriteDWORD((uint)cb, fp);//ここほんとは(int)cb
            Util.WriteWORD(wLongsPerEntry, fp);
            Util.WriteBYTE(bIndexSubType, fp);
            Util.WriteBYTE(bIndexType, fp);
            Util.WriteDWORD((uint)nEntriesInUse, fp);
            Util.fwrite(dwChunkId, fp);
            Util.WriteDWORD((uint)dwReserved1, fp);
            Util.WriteDWORD((uint)dwReserved2, fp);
            Util.WriteDWORD((uint)dwReserved3, fp);
            foreach (_avisuperindex_entry entry in aIndex) {
                Util.WriteQWORD((ulong)entry.qwOffset, fp);
                Util.WriteDWORD((uint)entry.dwSize, fp);
                Util.WriteDWORD((uint)entry.dwDuration, fp);
            }
        }
    }

    public class RawAvi2Writer : IAviWriter
    {
        public MainAVIHeader m_main_header;
        public AVIStreamHeader m_stream_header;
        //long currentIndex;
        BinaryWriter m_stream;
        //int frameRate;
        //int noOfFrame;
        int m_current_chunk = 0;
        long m_position_in_chunk = 0L;
        const long SRESHOLD = 1000000000L; //AVIXリストの最大サイズ(byte)
        long m_split_sreshold = 160000000L;  //AVIリストの最大サイズ(byte)
        AVISTDINDEX m_std_index;
        AVISUPERINDEX m_super_index;
        int m_linesize;//bitmapの1行分のデータサイズ(byte)
        bool m_is_first = true;
        long m_riff_position;//"RIFF*"の*が記入されているファイルの絶対位置。RIFF-AVI のときは必ず4。RIFF-AVIXの時は変化する
        long m_movi_position;
        long m_next_framedata_position;//次に00dbデータを記入するべき場所
        long m_avix_position;
        int m_junk_length;
        uint m_scale;
        uint m_rate;
        private int m_width;
        private int m_height;

        public Size Size
        {
            get
            {
                return new Size(m_width, m_height);
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
        /// <param name="frameRate">AVIファイルのフレームレート</param>
        // string file, uint scale, uint rate, int width, int height, IntPtr hwnd
        public bool Open(string file, uint scale, uint rate, int width, int height, IntPtr hwnd)
        {
            m_width = width;
            m_height = height;
            this.m_stream = new BinaryWriter(new FileStream(file, FileMode.Create, FileAccess.Write));
            float fps = (float)rate / (float)scale;
            this.m_main_header.dwMicroSecPerFrame = (uint)(1.0e6 / fps);//  ! 1秒は10^6μ秒
            this.m_main_header.dwReserved1 = 0;
            this.m_main_header.dwFlags = 2064;
            this.m_main_header.dwInitialFrames = 0;
            this.m_main_header.dwStreams = 0;
            this.m_main_header.dwScale = scale;
            this.m_main_header.dwRate = rate;
            this.m_main_header.dwStart = 0;
            this.m_main_header.dwLength = 0;
            this.m_rate = rate;
            this.m_scale = scale;
            //this.noOfFrame = 0;
            Util.fwrite("RIFF", this.m_stream);
            Util.WriteDWORD(0, this.m_stream);
            Util.fwrite("AVI ", this.m_stream);
            Util.fwrite("LIST", this.m_stream);
            Util.WriteDWORD(0x9cc, this.m_stream);
            Util.fwrite("hdrl", this.m_stream);
            m_current_chunk = 0;
            m_position_in_chunk = 0L;
            m_std_index = new AVISTDINDEX(0L);
            m_super_index = new AVISUPERINDEX(0);
            m_riff_position = 0x4;
            return true;
        }



        /*[Obsolete]
        public void Open( string file, float frameRate ) {
            uint scale, rate;
            AviWriter.CalcScaleAndRate( (decimal)frameRate, out scale, out rate );
            Open( file, scale, rate );
        }*/


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
                step = this.m_main_header.dwSuggestedBufferSize + 8;

                Util.fwrite("idx1", this.m_stream);
                Util.WriteDWORD((uint)(16 * frames), this.m_stream);
                for (i = 1; i <= frames; i++) {
                    Util.fwrite("00db", this.m_stream);
                    Util.WriteDWORD(Util.AVIF_HASINDEX, this.m_stream);
                    Util.WriteDWORD((uint)(4 + (i - 1) * step), this.m_stream);
                    Util.WriteDWORD(this.m_main_header.dwSuggestedBufferSize, this.m_stream);
                }//    end do
                m_avix_position = m_stream.BaseStream.Position;

                number = (uint)frames;
                //avi_writeIsolate( this.fp, number, 48 );    // AVIMainHeader.dwTotalFrames
                m_stream.Seek(48, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);
                //avi_writeIsolate( this.fp, number, 140 );   // AVIStreamHeader.dwLength
                m_stream.Seek(140, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);

                //odml dlmhのdwTotalFrames
                m_stream.Seek(0x8e8, SeekOrigin.Begin);
                Util.WriteDWORD((uint)frames, m_stream);

                // LIST****moviの****の数値を計算
                number = 4;//"movi"
                number += (uint)(frames * (m_linesize * m_main_header.dwHeight + 8));//フレーム数*(フレームのサイズ+"00db"+00dbチャンクのサイズ)
                number += 4 + 4 + (uint)m_std_index.cb; //ix00のサイズ
                //number += 4 + 4 + 16 * frames;//idx1のサイズ
                //avi_writeIsolate( this.fp, number, 2040 );  // LIST****movi の ****部分
                m_stream.BaseStream.Seek(m_movi_position, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);

                //number = 4096 + (this.mainHeader.dwSuggestedBufferSize + 24) * this.noOfFrame;
                //avi_writeIsolate( this.fp, number, 4 );     // RIFF****AVI  の ****部分
                number = (uint)m_junk_length/* 0xff4*/; //JUNKの終わりまでのサイズ。これは固定
                number += 4;//"movi"
                number += (uint)(frames * (m_linesize * m_main_header.dwHeight + 8));//00db...の合計サイズ
                number += 4 + 4 + (uint)m_std_index.cb;
                number += (uint)(4 + 4 + 16 * frames);//idx1のサイズ
                m_stream.BaseStream.Seek(m_riff_position, SeekOrigin.Begin);
                Util.WriteDWORD(number, m_stream);
                UpdateIndexOfIndex();
            } else {

                // LIST****moviの****を更新
                int number = 4;
                number += (int)(frames * (m_linesize * m_main_header.dwHeight + 8));
                number += 8 + (int)m_std_index.cb;
                m_stream.BaseStream.Seek(m_movi_position, SeekOrigin.Begin);
                Util.WriteDWORD((uint)number, m_stream);

                // odml dlmhのdwTotalFrames
                uint frames2 = 0;
                for (int j = 0; j <= m_current_chunk; j++) {
                    frames2 += (uint)m_super_index.aIndex[j].dwDuration;
                }
                m_stream.BaseStream.Seek(0x8e8, SeekOrigin.Begin);
                Util.WriteDWORD(frames2, m_stream);

                //avi_writeIsolate( this.fp, number, 48 );    // AVIMainHeader.dwTotalFrames
                m_stream.Seek(48, SeekOrigin.Begin);
                Util.WriteDWORD(frames2, m_stream);

                //avi_writeIsolate( this.fp, number, 140 );   // AVIStreamHeader.dwLength
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
        public void Close()
        {
            UpdateIndex();
            this.m_stream.Close();
        }


        /// <summary>
        /// 最初の[AVI :AVI[LIST:hdrl[LIST:strl]]]に書き込まれているsuper indexチャンク[indx]を更新します
        /// </summary>
        private void UpdateIndexOfIndex()
        {
            m_stream.Seek(0xd4, SeekOrigin.Begin);
            m_super_index.Write(m_stream);
        }


        /// <summary>
        /// aviファイルにフレームを1つ追加します．
        /// </summary>
        /// <param name="bmp"></param>
        public void AddFrame(Bitmap bmp)
        {
            int i, width, height, lineSize;

            if (bmp.Width != m_width || bmp.Height != m_height) {
                throw new Exception("bitmap size mismatch");
            }

            // BitmapDataからビットマップデータと、BITMPAINFOHEADERを取り出す
            BitmapData bmpDat = bmp.LockBits(
                    new Rectangle(0, 0, (int)bmp.Width, (int)bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int address = bmpDat.Scan0.ToInt32();
            byte[] bitmapData = new byte[bmpDat.Stride * bmpDat.Height];
            Marshal.Copy(new IntPtr(address), bitmapData, 0, bitmapData.Length);

            if (m_is_first) {//then
                m_is_first = false;
                this.m_main_header.dwWidth = (uint)m_width;
                this.m_main_header.dwHeight = (uint)m_height;
                this.m_main_header.dwMaxBytesPerSec = (uint)(bmpDat.Stride * bmpDat.Height * this.frameRate);// bmp%infoHeader%SizeImage * avi%frameRate
                this.m_main_header.dwStreams = 1;
                this.m_main_header.dwSuggestedBufferSize = (uint)(bmpDat.Stride * bmpDat.Height);// bmp.infoHeader%SizeImage
                m_linesize = bmpDat.Stride;

                this.m_stream_header.fccType = Util.mmioFOURCC("vids");
                this.m_stream_header.fccHandler = 0;
                this.m_stream_header.dwFlags = 0;
                this.m_stream_header.dwReserved1 = 0;
                this.m_stream_header.dwInitialFrames = 0;
                this.m_stream_header.dwScale = m_scale;
                this.m_stream_header.dwRate = m_rate;
                this.m_stream_header.dwStart = 0;
                this.m_stream_header.dwSuggestedBufferSize = this.m_main_header.dwSuggestedBufferSize;
                this.m_stream_header.dwQuality = 0;
                this.m_stream_header.dwSampleSize = 0;

                Util.aviWriteMainHeader(m_main_header, m_stream);

                Util.fwrite("LIST", this.m_stream);// i = fwrite( 'LIST', 1, 4, avi%fp )
                Util.WriteDWORD(0x874, this.m_stream);// call bmpQWordWrite( 130, avi%fp )
                Util.fwrite("strl", this.m_stream);// i = fwrite( 'strl', 1, 4, avi%fp )

                Util.aviWriteStreamHeader(m_stream_header, m_main_header, m_stream);// avi )

                Util.fwrite("strf", this.m_stream);// i = fwrite( 'strf', 1, 4, avi%fp )
                Util.WriteDWORD(0x28, this.m_stream); //call bmpQWordWrite( 40, avi%fp )    !// infoHeaderのサイズ
                BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
                bih.biSize = (uint)(Marshal.SizeOf(bih));
                bih.biWidth = bmpDat.Width;
                bih.biHeight = bmpDat.Height;
                bih.biPlanes = 1;
                bih.biBitCount = 24;
                bih.biCompression = 0;//BI_RGB
                bih.biSizeImage = (uint)(bmpDat.Stride * bmpDat.Height);
                bih.biXPelsPerMeter = 0;
                bih.biYPelsPerMeter = 0;
                bih.biClrUsed = 0;
                bih.biClrImportant = 0;
                bih.Write(m_stream);

                /*fwrite( "strn", this.fp );
                WriteDWORD( 6, this.fp );
                fwrite( "VIDEO", this.fp );
                WriteBYTE( 0, this.fp );*/
                Util.fwrite("indx", this.m_stream);          //fcc
                Util.WriteDWORD(0x7f8, this.m_stream);       // cb
                Util.WriteWORD((byte)0x4, this.m_stream);    // wLongsPerEntry
                Util.WriteBYTE(0x0, this.m_stream);          // bIndexSubType
                Util.WriteBYTE(Util.AVI_INDEX_OF_INDEXES, this.m_stream);// bIndexType
                Util.WriteDWORD(0x0, this.m_stream);         // nEntriesInUse
                Util.fwrite("00db", this.m_stream);          // dwChunkId
                Util.WriteDWORD(0x0, this.m_stream);
                Util.WriteDWORD(0x0, this.m_stream);
                Util.WriteDWORD(0x0, this.m_stream);
                for (int ii = 1; ii <= 126; ii++) {
                    Util.WriteQWORD(0x0, this.m_stream);
                    Util.WriteDWORD(0x0, this.m_stream);
                    Util.WriteDWORD(0x0, this.m_stream);
                }

                Util.fwrite("LIST", this.m_stream);
                Util.WriteDWORD(0x104, m_stream);
                Util.fwrite("odml", this.m_stream);
                Util.fwrite("dmlh", m_stream);
                Util.WriteDWORD(0xf8, m_stream);
                Util.WriteDWORD(0x0, m_stream);//ここ後で更新するべき
                for (int ii = 1; ii <= 61; ii++) {
                    Util.WriteDWORD(0x0, m_stream);
                }

                Util.fwrite("JUNK", this.m_stream);// i = fwrite( 'JUNK', 1, 4, avi%fp )
                Util.WriteDWORD(0x60c, m_stream);
                Util.WriteDWORD(0, m_stream);//"This"が将来登録されたらやばいので
                Util.fwrite("This file was generated by RawAvi@LipSync", this.m_stream);
                //WriteDWORD( 1503, this.fp );// call bmpQWordWrite( 1802, avi%fp )
                for (i = 1; i <= 1503; i++) {//do i = 1, 1802
                    this.m_stream.Write((byte)0);// call fputc( 0, avi%fp )
                }//end do
                m_junk_length = 0xff4;

                Util.fwrite("LIST", this.m_stream);//      i = fwrite( 'LIST', 1, 4, avi%fp )
                m_movi_position = m_stream.BaseStream.Position;
                Util.WriteDWORD(0, this.m_stream);// call bmpQWordWrite( 0, avi%fp )     !// ******************ココの数字は一番最後に書き換える必要あり2040～2043あとdwTotalFrames（48～51）も
                Util.fwrite("movi", this.m_stream);// i = fwrite( 'movi', 1, 4, avi%fp )
                m_next_framedata_position = m_stream.BaseStream.Position;

                m_std_index.SetBaseOffset((ulong)m_next_framedata_position);
                m_super_index.nEntriesInUse++;
            }//end if

            if (m_next_framedata_position != m_stream.BaseStream.Position) {
                m_stream.BaseStream.Seek(m_next_framedata_position, SeekOrigin.Begin);
            }

            long chunk_size = m_next_framedata_position - m_riff_position;
#if DEBUG
            //            MessageBox.Show( "chunk_size=" + chunk_size );
#endif
            if ((m_current_chunk == 0 && chunk_size > m_split_sreshold) ||
                 (m_current_chunk > 0 && chunk_size > SRESHOLD)) {
                // AVIXリストへの書き込みに移行
                UpdateIndex();
                m_stream.BaseStream.Seek(m_avix_position, SeekOrigin.Begin);
                Util.fwrite("RIFF", m_stream);
                m_riff_position = m_stream.BaseStream.Position;
#if DEBUG
                //                fp.Flush();
                //                MessageBox.Show( "m_riff_position=" + m_riff_position );
#endif
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
            width = (int)this.m_main_header.dwWidth;
            height = (int)this.m_main_header.dwHeight;
            if (width != bmpDat.Width) {//then
                //aviAddFrame = -1
                return;
            }//end if
            if (height != bmpDat.Height) {//then
                //aviAddframe = -1
                return;
            }//end if
            lineSize = bmpDat.Stride;// int( (width * 24 + 31) / 32 ) * 4

            m_std_index.AddIndex((uint)((ulong)m_stream.BaseStream.Position - m_std_index.qwBaseOffset) + 8, (uint)(lineSize * height));
            Util.fwrite("00db", this.m_stream);//    i = fwrite( '00db', 1, 4, avi%fp )
            Util.WriteDWORD(m_main_header.dwSuggestedBufferSize, m_stream);// call bmpQWordWrite( avi%mainHeader%dwSuggestedBufferSize, avi%fp )
            m_stream.Write(bitmapData);
            m_next_framedata_position = m_stream.BaseStream.Position;
            _avisuperindex_entry entry = m_super_index.aIndex[m_current_chunk];
            entry.dwDuration++;
            m_super_index.aIndex[m_current_chunk] = entry;//    avi%noOfFrame = avi%noOfFrame + 1
            this.m_stream.Flush();// aviAddFrame = fflush( avi%fp )
            bmp.UnlockBits(bmpDat);
        }//  end function



    }
}
