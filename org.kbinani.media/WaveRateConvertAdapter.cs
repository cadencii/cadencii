/*
 * WaveRateConvertAdapter.cs
 * Copyright © 2010 kbinani
 *
 * This file is part of org.kbinani.media.
 *
 * org.kbinani.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.media;

import java.util.*;
#else
using System;

namespace org.kbinani.media {
#endif

    /// <summary>
    /// 接頭辞b: 単位が変換前のサンプル数になっている変数
    /// 接頭辞a: 単位が変換後のサンプル数になっている変数
    /// </summary>
    public class WaveRateConvertAdapter {
        /// <summary>
        /// バッファの標準長さ
        /// </summary>
        private const int BUFLEN = 1024;

        private IWaveReceiver receiver = null;
        /// <summary>
        /// 受け取ったデータの個数
        /// </summary>
        private long bCount = 0;
        /// <summary>
        /// receiverに送ったデータの個数
        /// </summary>
        private long aCount = 0;
        private int bRate = 44100;
        private int aRate = 44100;
        private double invBRate = 1.0 / 44100.0;
        private double invARate = 1.0 / 44100.0;
        /// <summary>
        /// 次のappend呼び出しまでに残しておくデータのバッファ(左チャンネル)
        /// </summary>
        private double[] bBufLeft;
        /// <summary>
        /// 次のappend呼び出しまでに残しておくデータのバッファ(左チャンネル)
        /// </summary>
        private double[] bBufRight;
        /// <summary>
        /// レシーバに波形を送るときに使うバッファ(左チャンネル)
        /// </summary>
        private double[] aBufSendLeft = new double[BUFLEN];
        /// <summary>
        /// レシーバに波形を送るときに使うバッファ(右チャンネル)
        /// </summary>
        private double[] aBufSendRight = new double[BUFLEN];
        private long bBufBase;

        /// <summary>
        /// コンストラクタ．変換後のサンプリング周波数は，receiverのgetSampleRate()で自動的に取得される
        /// </summary>
        /// <param name="receiver">変換した波形を送る相手先</param>
        /// <param name="sample_rate">変換前のサンプリング周波数</param>
        public WaveRateConvertAdapter( IWaveReceiver receiver, int sample_rate ) {
            this.receiver = receiver;
            bRate = sample_rate;
            aRate = receiver.getSampleRate();
            invBRate = 1.0 / (double)bRate;
            invARate = 1.0 / (double)aRate;
        }

        public void close() {
#if DEBUG
            PortUtil.println( "WaveRateConvertAdapter#close" );
#endif
            receiver.close();
        }

        public void append( double[] left, double[] right, int length ) {
            if ( aRate == bRate ) {
                receiver.append( left, right, length );
                aCount += length;
                bCount += length;
                return;
            }

            double secStart = bCount * invBRate;
            double secEnd = (bCount + length) * invBRate;

            // 送られてきたデータで、aStartからaEndまでのデータを作成できる
            long aStart = (long)(secStart * aRate);
            long aEnd = (long)(secEnd * aRate) - 1;

            double tx = (aEnd - 1) * invARate;
            long btRequired = (long)(tx * bRate);
            int tindx1 = (int)(btRequired - bCount) + 1;
            if ( tindx1 >= length ) {
                aEnd--;
            }

            int i = 0;
            for ( long a = aCount; a < aEnd; a++ ) {
                double x = a * invARate;
                long bRequired = (long)(x * bRate);
                double x0 = bRequired * invBRate;
                double x1 = (bRequired + 1) * invBRate;
                int indx0 = (int)(bRequired - bCount);
                int indx1 = indx0 + 1;

                // 左チャンネル
                double y0 = 0.0;
                if ( 0 <= indx0 ) {
                    if ( indx0 < length ) {
                        y0 = left[indx0];
                    }
                } else {
                    int j = (int)(bRequired - bBufBase);
                    if ( 0 <= j && j < bBufLeft.Length ) {
                        y0 = bBufLeft[j];
                    }
                }
                double y1 = 0.0;
                if ( indx1 >= 0 ) {
                    if ( indx1 < length ) {
                        y1 = left[indx1];
                    }
                } else {
                    int j = (int)(bRequired + 1 - bBufBase);
                    if ( 0 <= j && j < bBufLeft.Length ) {
                        y1 = bBufLeft[j];
                    }
                }
                double s = (y1 - y0) / (x1 - x0);
                double y = y0 + s * (x - x0);
                aBufSendLeft[i] = y;

                // 右チャンネル
                if ( indx0 >= 0 ) {
                    if ( indx0 < length ) {
                        y0 = right[indx0];
                    }
                } else {
                    int j = (int)(bRequired - bBufBase);
                    if ( 0 <= j && j < bBufRight.Length ) {
                        y0 = bBufRight[j];
                    }
                }
                if ( indx1 >= 0 ) {
                    if ( indx1 < length ) {
                        y1 = right[indx1];
                    }
                } else {
                    int j = (int)(bRequired + 1 - bBufBase);
                    if ( 0 <= j && j < bBufRight.Length ) {
                        y1 = bBufRight[j];
                    }
                }
                s = (y1 - y0) / (x1 - x0);
                y = y0 + s * (x - x0);
                aBufSendRight[i] = y;

                // 事後処理
                i++;
                if ( i >= BUFLEN ) {
                    // バッファがいっぱいだったら送信
                    receiver.append( aBufSendLeft, aBufSendRight, BUFLEN );
                    i = 0;
                }
            }

            // 未送信のバッファがあれば送信
            if ( i > 0 ) {
                receiver.append( aBufSendLeft, aBufSendRight, i );
            }

            // 次回に繰り越すデータを確保
            // 次に送られてくるデータはbCount + length + 1から
            long aNext = (long)((bCount + length + 1) * invBRate * aRate) + 1;
            if ( aEnd + 1 < aNext ) {
                bBufBase = (long)((aEnd + 1) * invARate * bRate) - 2; // aEnd + 1番目のデータを作成するのに必要なデータ点のインデクス
                int num = (int)(bCount + length - bBufBase);
                if ( num > 0 ) {
                    if ( bBufLeft == null ) {
                        bBufLeft = new double[num];
                    } else if ( bBufLeft.Length < num ) {
#if JAVA
                        bBufLeft = Arrays.copyOf( bBufLeft, num );
#else
                        Array.Resize( ref bBufLeft, num );
#endif
                    }
                    if ( bBufRight == null ) {
                        bBufRight = new double[num];
                    } else if ( bBufRight.Length < num ) {
#if JAVA
                        bBufRight = Arrays.copyOf( bBufRight, num );
#else
                        Array.Resize( ref bBufRight, num );
#endif
                    }
                    for ( int j = 0; j < num; j++ ) {
                        int indx = (int)(bBufBase + j - bCount);
                        bBufLeft[j] = left[indx];
                        bBufRight[j] = right[indx];
                    }
                }
            }

            bCount += length;
            aCount = aEnd;
        }
    }

#if !JAVA
}
#endif
