/*
 * WaveRateConverter.cs
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

#else
namespace org.kbinani.media {
    using boolean = System.Boolean;
#endif

    public class WaveRateConverter {
        /// <summary>
        /// Waveデータの読み込み元
        /// </summary>
        private WaveReader reader = null;
        /// <summary>
        /// 変換後のサンプリングレート
        /// </summary>
        private int rate;
        /// <summary>
        /// 変換するデータ点ユニットのサイズ
        /// unit個のデータをunitAfter個に変更する
        /// </summary>
        private int unit = 1;
        /// <summary>
        /// 変換ユニットの，変換後のサイズ
        /// unit個のデータをunitAfter個に変更する
        /// </summary>
        private int unitAfter = 1;
        /// <summary>
        /// 変換後の第iインデクスにおける値を線形補間するとき，変換前の第unitIndex[i]番目のデータと第unitIndex[i] + 1番目のデータを利用すればいい
        /// </summary>
        private int[] unitIndex = null;
        private double[] unitX = null;
        /// <summary>
        /// 読み込み元のサンプルレート
        /// </summary>
        private int readerRate;

        const int MAX_BUFLEN = 1024;

        public WaveRateConverter( WaveReader wave_reader, int rate ) {
            reader = wave_reader;
            this.rate = rate;
            readerRate = reader.getSampleRate();
            int gcd = math.gcd( rate, readerRate );
            unit = readerRate / gcd;
            unitAfter = rate / gcd;

            unitIndex = new int[unitAfter];
            for ( int i = 0; i < unitAfter; i++ ) {
                unitIndex[i] = (int)(i * unit / (double)unitAfter);
            }
            unitX = new double[unit];
            for ( int i = 0; i < unit; i++ ) {
                unitX[i] = i / (double)unit;
            }
        }

        public void read( long index, int length, double[] left, double[] right ) {
            // 変換後換算で第index番目のデータは，何個目のunitブロックか
            int blockIndexStart = index / unitAfter;
            
            // 変換後換算で第index+length番目のデータは，何個目のunitブロックか
            int blockIndexEnd = (index + length) / unitAfter;
            
            // 第blockIndexStartブロックから第blockIndexEndブロックまでを読み込む
            int processedAfter = 0; // 処理済みのサンプル数(変換後換算)
            int offsetAfter = index - blockIndexStart * unitAfter; // 第blockIndexStartブロックの第offsetAfter番目のデータから計算を始める
            int numUnit = MAX_BUFLEN / unit;
            int buflen = numUnit * unit;
            double[] bufLeft = new double[buflen];
            double[] bufRight = new double[buflen];
            int imax = (blockIndexEnd - blockIndexStart + 1) / numUnit;
            double[] datUnitBefore = new double[unit];
            double[] datUnitAfter = new double[unitAfter];
            for ( int i = 0; i < imax; i++ ) {
                // このループ内では，第blockStartブロックから第blockStart + numUnit - 1ブロックまでを読み込む
                int blockStart = blockIndexStart + i * numUnit;
                reader.read( blockStart * unit, buflen, bufLeft, bufRight );
                
                for ( int block = blockStart; block < blockStart + numUnit; block++ ) {
                    // このループ内では，第blockブロックのデータを読み込む
                    
                    // 左チャンネルを変換
                    int offset = (block - blockStart) * unit;
                    for ( int j = 0; j < unit; j++ ) {
                        datUnitBefore[j] = bufLeft[offset + j];
                    }
                    for ( int j = 0; j < unitAfter; j++ ) {
                        int index = unitIndex[j];
                        double x0 = unitX[index];
                        double x1 = unitX[index + 1];
                        double y0 = datUnitBefore[index];
                        double y1 = datUnitBefore[index + 1];
                        double a = (y1 - y0) / (x1 - x0);
                        double x = j / (double)unitAfter;
                        double y = y0 + a * (x - x0);
                        datUnitAfter[j] = y;
                    }
                    // leftに転送
                    for ( int j = 0; j < unitAfter; j++ ) {
                        left[processedAfter + offsetAfter + j] = datUnitAfter[j];
                    }

                    // TODO：この辺から
                }
            }
        }

        public void close() {
            if ( reader == null ) {
                return;
            }
            reader.close();
            reader = null;
        }
    }

#if !JAVA
}
#endif
