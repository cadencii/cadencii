/*
 * WaveRateConvertAdapter.cs
 * Copyright (C) 2010 kbinani
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
        private IWaveReceiver receiver = null;
        private long bCount = 0; // 受け取ったデータの個数
        private long aCount = 0; // receiverに送ったデータの個数
        private int bRate = 44100;
        private int aRate = 44100;
        private double invBRate = 1.0 / 44100.0;
        private double invARate = 1.0 / 44100.0;
        private double[] bBufLeft;
        private double[] bBufRight;
        private long bBufBase;

        public WaveRateConvertAdapter( IWaveReceiver receiver, int sample_rate ) {
            this.receiver = receiver;
            bRate = sample_rate;
            aRate = receiver.getSampleRate();
            invBRate = 1.0 / (double)bRate;
            invARate = 1.0 / (double)aRate;
        }

        public void close() {
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

            double[] aLeft = new double[(int)(aEnd - aCount)];
            double[] aRight = new double[(int)(aEnd - aCount)];
            for ( long a = aCount; a < aEnd; a++ ) {
                double x = a * invARate;
                long bRequired = (long)(x * bRate);
                double x0 = bRequired * invBRate;
                double x1 = (bRequired + 1) * invBRate;
                int indx0 = (int)(bRequired - bCount);
                int indx1 = indx0 + 1;
                double y0 = 0.0;
                if ( 0 <= indx0 ) {
                    if ( indx0 < length ) {
                        y0 = left[indx0];
                    }
                } else {
                    int i = (int)(bRequired - bBufBase);
                    if ( 0 <= i && i < bBufLeft.Length ) {
                        y0 = bBufLeft[i];
                    }
                }
                double y1 = 0.0;
                if ( indx1 >= 0 ) {
                    if ( indx1 < length ) {
                        y1 = left[indx1];
                    }
                } else {
                    int i = (int)(bRequired + 1 - bBufBase);
                    if ( 0 <= i && i < bBufLeft.Length ) {
                        y1 = bBufLeft[i];
                    }
                }

                double s = (y1 - y0) / (x1 - x0);
                double y = y0 + s * (x - x0);
                aLeft[(int)(a - aCount)] = y;

                if ( indx0 >= 0 ) {
                    if ( indx0 < length ) {
                        y0 = right[indx0];
                    }
                } else {
                    int i = (int)(bRequired - bBufBase);
                    if ( 0 <= i && i < bBufRight.Length ) {
                        y0 = bBufRight[i];
                    }
                }
                if ( indx1 >= 0 ) {
                    if ( indx1 < length ) {
                        y1 = right[indx1];
                    }
                } else {
                    int i = (int)(bRequired + 1 - bBufBase);
                    if ( 0 <= i && i < bBufRight.Length ) {
                        y1 = bBufRight[i];
                    }
                }
                s = (y1 - y0) / (x1 - x0);
                y = y0 + s * (x - x0);
                aRight[(int)(a - aCount)] = y;
            }

            receiver.append( aLeft, aRight, aLeft.Length );

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
                    for ( int i = 0; i < num; i++ ) {
                        bBufLeft[i] = left[(int)(bBufBase + i - bCount)];
                        bBufRight[i] = right[(int)(bBufBase + i - bCount)];
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
