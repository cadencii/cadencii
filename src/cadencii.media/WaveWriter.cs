/*
 * WaveWriter.cs
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
using System.IO;
using cadencii;
using cadencii.java.io;

namespace cadencii.media
{

    public class WaveWriter : IDisposable, IWaveReceiver
    {
        private int m_channel = 1;
        private int m_bit_per_sample;
        private int m_sample_rate;
        private long m_total_samples = 0;
        private Stream m_stream = null;
        private string m_path = "";
        /// <summary>
        /// dataチャンクの開始位置。第1番目のデータが、このアドレスに書き込まれることになる。
        /// </summary>
        private long m_pos_data_chunk;

        public WaveWriter(string path)
            : this(path, 2, 16, 44100)
        {
        }

        public WaveWriter(string path, int channel, int bit_per_sample, int sample_rate)
        {
            m_path = path;
#if DEBUG
            sout.println("WaveWriter#.ctor; m_path=" + m_path);
#endif
            m_stream = new FileStream(m_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            m_channel = channel;
            m_bit_per_sample = bit_per_sample;
            m_sample_rate = sample_rate;
            writeHeader();
            m_total_samples = (m_stream.Length - m_pos_data_chunk) / m_channel / (m_bit_per_sample / 8);
        }

        /// <summary>
        /// 第posサンプルからlengthサンプル分、指定した波形データで置き換えます
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="length"></param>
        /// <param name="L"></param>
        /// <param name="R"></param>
        public void replace(long pos, int length, double[] L, double[] R)
        {
            long lastPos = m_stream.Position;
            long posFile = pos * m_channel * m_bit_per_sample / 8 + m_pos_data_chunk;
            long streamLen = m_stream.Length;
            if (streamLen < posFile) {
                // ファイルの長さが足りていない場合、とりあえず0で埋める。
                m_stream.Seek(streamLen - 1, SeekOrigin.Begin);
                long remain = posFile - streamLen;
                int buflen = 1024;
                byte[] data = new byte[buflen];
                while (remain > 0) {
                    int delta = remain > buflen ? buflen : (int)remain;
                    m_stream.Write(data, 0, delta);
                    remain -= delta;
                }
                m_total_samples = pos;
            }
            m_stream.Seek(posFile, SeekOrigin.Begin);

            // 書き込み
            if (m_bit_per_sample == 8) {
                if (m_channel == 1) {
                    for (int i = 0; i < length; i++) {
                        m_stream.WriteByte((byte)((L[i] + R[i] + 2.0) * 63.75));
                    }
                } else {
                    for (int i = 0; i < length; i++) {
                        m_stream.WriteByte((byte)((L[i] + 1.0) * 127.5));
                        m_stream.WriteByte((byte)((R[i] + 1.0) * 127.5));
                    }
                }
            } else {
                byte[] buf;
                if (m_channel == 1) {
                    for (int i = 0; i < length; i++) {
                        buf = PortUtil.getbytes_int16_le((short)((L[i] + R[i]) * 16384.0));
                        writeByteArray(m_stream, buf, 2);
                    }
                } else {
                    for (int i = 0; i < length; i++) {
                        buf = PortUtil.getbytes_int16_le((short)(L[i] * 32768.0));
                        writeByteArray(m_stream, buf, 2);
                        buf = PortUtil.getbytes_int16_le((short)(R[i] * 32768.0));
                        writeByteArray(m_stream, buf, 2);
                    }
                }
            }
            m_total_samples = (m_total_samples < pos + length) ? (pos + length) : (m_total_samples);

            // 最後にファイルポインタを戻す
            m_stream.Seek(lastPos, SeekOrigin.Begin);
        }

        public void Dispose()
        {
            close();
        }

        /// <summary>
        /// Writes header of WAVE file
        /// </summary>
        private void writeHeader()
        {
            // RIFF
            m_stream.WriteByte(0x52); // loc=0x00
            m_stream.WriteByte(0x49);
            m_stream.WriteByte(0x46);
            m_stream.WriteByte(0x46);

            // ファイルサイズ - 8最後に記入
            m_stream.WriteByte(0x00); // loc=0x04
            m_stream.WriteByte(0x00);
            m_stream.WriteByte(0x00);
            m_stream.WriteByte(0x00);

            // WAVE
            m_stream.WriteByte(0x57); // loc=0x08
            m_stream.WriteByte(0x41);
            m_stream.WriteByte(0x56);
            m_stream.WriteByte(0x45);

            // fmt 
            m_stream.WriteByte(0x66); // loc=0x0c
            m_stream.WriteByte(0x6d);
            m_stream.WriteByte(0x74);
            m_stream.WriteByte(0x20);

            // fmt チャンクのサイズ
            m_stream.WriteByte(0x12); // loc=0x10
            m_stream.WriteByte(0x00);
            m_stream.WriteByte(0x00);
            m_stream.WriteByte(0x00);

            // format ID
            m_stream.WriteByte(0x01); // loc=0x14
            m_stream.WriteByte(0x00);

            // チャンネル数
            if (m_channel == 1) {
                m_stream.WriteByte(0x01); // loc=0x16
                m_stream.WriteByte(0x00);
            } else {
                m_stream.WriteByte(0x02); //loc=0x16
                m_stream.WriteByte(0x00);
            }

            // サンプリングレート
            byte[] buf = PortUtil.getbytes_uint32_le(m_sample_rate);
            writeByteArray(m_stream, buf, 4); // 0x18

            // データ速度
            int block_size = (int)(m_bit_per_sample / 8 * (int)m_channel);
            int data_rate = m_sample_rate * block_size;
            buf = PortUtil.getbytes_uint32_le(data_rate);
            writeByteArray(m_stream, buf, 4);//loc=0x1c

            // ブロックサイズ
            buf = PortUtil.getbytes_uint16_le(block_size);
            writeByteArray(m_stream, buf, 2); //0x20

            // サンプルあたりのビット数
            buf = PortUtil.getbytes_uint16_le(m_bit_per_sample);
            writeByteArray(m_stream, buf, 2); //loc=0x22

            // 拡張部分
            m_stream.WriteByte(0x00); //loc=0x24
            m_stream.WriteByte(0x00);

            // data
            m_stream.WriteByte(0x64); //loc=0x26
            m_stream.WriteByte(0x61);
            m_stream.WriteByte(0x74);
            m_stream.WriteByte(0x61);

            // size of data chunk
            long size = block_size * m_total_samples;
            buf = PortUtil.getbytes_uint32_le(size);
            writeByteArray(m_stream, buf, 4);
            m_pos_data_chunk = m_stream.Position;
        }

        public void close()
        {
#if DEBUG
            sout.println("WaveWriter#close; m_path=" + m_path);
#endif
            if (m_stream == null) {
                return;
            }
            try {
                // 最後にWAVEチャンクのサイズ
                int position = (int)m_stream.Position;
                m_stream.Seek(4, SeekOrigin.Begin);
                byte[] buf = PortUtil.getbytes_uint32_le(position - 8);
                writeByteArray(m_stream, buf, 4);

                // size of data chunk
                int block_size = (int)(m_bit_per_sample / 8 * (int)m_channel);
                long size = block_size * m_total_samples;
                m_stream.Seek(42, SeekOrigin.Begin);
                buf = PortUtil.getbytes_uint32_le(size);
                writeByteArray(m_stream, buf, 4);

                m_stream.Close();
            } catch (Exception ex) {
                serr.println("WaveWriter#close; ex=" + ex);
            }
        }

        public int getSampleRate()
        {
            return m_sample_rate;
        }

        public void append(float[] L)
        {
            append<float[]>(L, L, L.Length, (container, index) => container[index]);
        }

        public void append(double[] L)
        {
            append<double[]>(L, L, L.Length, (container, index) => container[index]);
        }

        public void append(float[] L, float[] R)
        {
            int length = Math.Min(L.Length, R.Length);
            append(L, R, length);
        }

        public void append(float[] L, float[] R, int length)
        {
            append<float[]>(L, R, Math.Min(L.Length, R.Length), (container, index) => container[index]);
        }

        public void append(double[] L, double[] R)
        {
            int length = Math.Min(L.Length, R.Length);
            append(L, R, length);
        }

        public void append(double[] left, double[] right, int length)
        {
            append<double[]>(left, right, length, (container, index) => container[index]);
        }

        private void append<ContainerT>(ContainerT L, ContainerT R, int length, Func<ContainerT, int, double> get_container_value)
        {
            try {
                if (m_bit_per_sample == 8) {
                    if (m_channel == 1) {
                        for (int i = 0; i < length; i++) {
                            m_stream.WriteByte((byte)((get_container_value(L, i) + get_container_value(R, i) + 2.0) * 63.75));
                        }
                    } else {
                        for (int i = 0; i < length; i++) {
                            m_stream.WriteByte((byte)((get_container_value(L, i) + 1.0) * 127.5));
                            m_stream.WriteByte((byte)((get_container_value(R, i) + 1.0) * 127.5));
                        }
                    }
                } else {
                    byte[] buf;
                    if (m_channel == 1) {
                        for (int i = 0; i < length; i++) {
                            buf = PortUtil.getbytes_int16_le((short)((get_container_value(L, i) + get_container_value(R, i)) * 16384.0));
                            writeByteArray(m_stream, buf, 2);
                        }
                    } else {
                        for (int i = 0; i < length; i++) {
                            buf = PortUtil.getbytes_int16_le((short)(get_container_value(L, i) * 32768.0));
                            writeByteArray(m_stream, buf, 2);
                            buf = PortUtil.getbytes_int16_le((short)(get_container_value(R, i) * 32768.0));
                            writeByteArray(m_stream, buf, 2);
                        }
                    }
                }
                m_total_samples += (int)length;
            } catch (Exception ex) {
                serr.println("WaveWriter#append(double[],double[],int); ex=" + ex);
            }
        }

        public void append(byte[] L, byte[] R)
        {
            int total = Math.Min(L.Length, R.Length);
            if (m_bit_per_sample == 8) {
                if (m_channel == 1) {
                    for (int i = 0; i < total; i++) {
                        m_stream.WriteByte((byte)(0xff & ((L[i] + R[i]) / 2)));
                    }
                } else {
                    for (int i = 0; i < total; i++) {
                        m_stream.WriteByte((byte)(0xff & L[i]));
                        m_stream.WriteByte((byte)(0xff & R[i]));
                    }
                }
            } else {
                byte[] buf;
                if (m_channel == 1) {
                    for (int i = 0; i < total; i++) {
                        buf = PortUtil.getbytes_int16_le((short)((L[i] + R[i]) * 128.5f - 32768f));
                        writeByteArray(m_stream, buf, 2);
                    }
                } else {
                    for (int i = 0; i < total; i++) {
                        buf = PortUtil.getbytes_int16_le((short)(L[i] * 257f - 32768f));
                        writeByteArray(m_stream, buf, 2);
                        buf = PortUtil.getbytes_int16_le((short)(R[i] * 257f - 32768f));
                        writeByteArray(m_stream, buf, 2);
                    }
                }
            }
            m_total_samples += (int)total;
        }

        public void append(short[] L, short[] R)
        {
            int total = Math.Min(L.Length, R.Length);
            if (m_bit_per_sample == 8) {
                if (m_channel == 1) {
                    for (int i = 0; i < total; i++) {
                        m_stream.WriteByte((byte)(((L[i] + R[i]) / 2f + 32768f) / 255f));
                    }
                } else {
                    for (int i = 0; i < total; i++) {
                        m_stream.WriteByte((byte)((L[i] + 32768f) / 255f));
                        m_stream.WriteByte((byte)((R[i] + 32768f) / 255f));
                    }
                }
            } else {
                byte[] buf;
                if (m_channel == 1) {
                    for (int i = 0; i < total; i++) {
                        buf = PortUtil.getbytes_int16_le((short)((L[i] + R[i]) / 2));
                        writeByteArray(m_stream, buf, 2);
                    }
                } else {
                    for (int i = 0; i < total; i++) {
                        buf = PortUtil.getbytes_int16_le(L[i]);
                        writeByteArray(m_stream, buf, 2);
                        buf = PortUtil.getbytes_int16_le(R[i]);
                        writeByteArray(m_stream, buf, 2);
                    }
                }
            }
            m_total_samples += (int)total;
        }

        private static void writeByteArray(Stream fs, byte[] dat, int limit)
        {
            fs.Write(dat, 0, (dat.Length > limit) ? limit : dat.Length);
            if (dat.Length < limit) {
                for (int i = 0; i < limit - dat.Length; i++) {
                    fs.WriteByte(0x00);
                }
            }
        }
    }

}
