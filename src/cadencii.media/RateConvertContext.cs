/*
 * RateConvertContext.cs
 * Copyright © 2010-2011 kbinani
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

namespace cadencii.media
{

    /// <summary>
    /// サンプリング周波数を変換するコア機能を提供
    /// </summary>
    public class RateConvertContext
    {
        /// <summary>
        /// コンテキストの状態を表す列挙子
        /// </summary>
        enum Status
        {
            /// <summary>
            /// 初回のconvert呼び出し
            /// </summary>
            NORMAL,
            /// <summary>
            /// convertの戻り値がtrueだった場合の，2回目以降の呼び出し
            /// </summary>
            CONTINUE,
            /// <summary>
            /// 次のwave波形を送ってきてもOK
            /// </summary>
            COMPLETE,
        }

        private const int BUFLEN = 1024;
        /// <summary>
        /// 変換結果を格納するバッファ(右チャンネル)
        /// </summary>
        public double[] bufferRight = new double[BUFLEN];
        /// <summary>
        /// 変換結果を格納するバッファ(左チャンネル)
        /// </summary>
        public double[] bufferLeft = new double[BUFLEN];
        /// <summary>
        /// 変換結果を格納したバッファの長さ
        /// </summary>
        public int length;

        /// <summary>
        /// 受け取ったデータの個数
        /// </summary>
        private long bCount = 0;
        /// <summary>
        /// receiverに送ったデータの個数
        /// </summary>
        private long aCount = 0;
        /// <summary>
        /// 変換前のサンプリングレート
        /// </summary>
        private int bRate = 44100;
        /// <summary>
        /// 変換後のサンプリングレート
        /// </summary>
        private int aRate = 44100;
        /// <summary>
        /// bRateの逆数
        /// </summary>
        private double invBRate = 1.0 / 44100.0;
        /// <summary>
        /// aRateの逆数
        /// </summary>
        private double invARate = 1.0 / 44100.0;
        private double[] bBufLeft;
        private double[] bBufRight;
        /// <summary>
        /// bBufLeft[0]は，変換元データの先頭からbBufBase番目のデータであることを表す
        /// </summary>
        private long bBufBase;
        /// <summary>
        /// 変換コンテキストの現在の状態
        /// </summary>
        private Status mStatus = Status.NORMAL;

        private long aStart;
        private long aEnd;
        private long a;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sample_rate_from"></param>
        /// <param name="sample_rate_to"></param>
        public RateConvertContext(int sample_rate_from, int sample_rate_to)
        {
            if (sample_rate_from <= 0) {
                throw new ArgumentOutOfRangeException("sample_rate_from");
            }
            if (sample_rate_to <= 0) {
                throw new ArgumentOutOfRangeException("sample_rate_to");
            }
            bRate = sample_rate_from;
            aRate = sample_rate_to;
            invARate = 1.0 / aRate;
            invBRate = 1.0 / bRate;

            aEnd = -1;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~RateConvertContext()
        {
            dispose();
        }

        public int getSampleRateFrom()
        {
            return bRate;
        }

        public int getSampleRateTo()
        {
            return aRate;
        }

        /// <summary>
        /// デストラクタ，のようなもの
        /// </summary>
        public void dispose()
        {
            if (bBufLeft != null) {
                // C# m9ﾌﾟｷﾞｬｰ
                bBufLeft = null;
            }
            if (bBufRight != null) {
                // C#脳乙
                bBufRight = null;
            }
            if (this.bufferLeft != null) {
                // GC仕事しろ
                this.bufferLeft = null;
            }
            if (this.bufferRight != null) {
                // C++厨はちゃんとdelete[]すること。
                this.bufferRight = null;
            }
        }

        private static long calculateNextEnd(RateConvertContext context, int length)
        {
            double secStart = context.bCount * context.invBRate;
            double secEnd = (context.bCount + length - 1) * context.invBRate;

            // 送られてきたデータで、aStartからaEndまでのデータを作成できる
            long next_astart = context.aEnd + 1;// (long)(context.secStart * context.aRate);
            long next_aend = (long)(secEnd * context.aRate);

            double tx = next_aend * context.invARate;
            long btRequired = (long)(tx * context.bRate);
            int tindx1 = (int)(btRequired - context.bCount) + 1;
            if (tindx1 >= length) {
                next_aend--;
            }
            return next_aend;
        }

        /// <summary>
        /// 次にlengthサンプル分のデータを処理した時，結果として得られる変換データのサンプル数を計算します
        /// </summary>
        /// <param name="context"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int estimateResultSamples(RateConvertContext context, int length)
        {
            if (context.aRate == context.bRate) {
                // 変換を行わなくてもいい場合
                return length;
            } else {
                // 変換しなくちゃならん
                long astart = context.aEnd + 1;
                long aend = calculateNextEnd(context, length);
                return (int)(aend - astart + 1);
            }
        }

        /// <summary>
        /// 変換を実行する．変換結果は，context.aBufSendLeft, aBufSendRightに格納される．
        /// 変換後のデータの長さはaBufSendLengthに格納される．
        /// 戻り値がfalseの場合，もう一度convertを呼ぶ必要はない．
        /// 戻り値がtrueの場合，変換後のデータがaBufSendLeftなどに入りきらない場合なので，続けてもう一度convertを呼ばなくてはならない．
        /// </summary>
        /// <param name="context"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="length"></param>
        public static bool convert(RateConvertContext context, double[] left, double[] right, int length)
        {
            if (context.mStatus == Status.COMPLETE) {
                context.length = 0;
                context.mStatus = Status.NORMAL;
                return false;
            }

            // 変換前後で周波数が同じ，という指定の場合
            if (context.aRate == context.bRate) {
                if (context.mStatus == Status.NORMAL) {
                    context.a = context.aCount;
                    context.aStart = context.aCount;
                    context.aEnd = context.aCount + length;
                }
                int i = 0;
                int offset = (int)(context.a - context.aStart);
                for (; context.a < context.aEnd; context.a++) {
                    context.bufferLeft[i] = left[i + offset];
                    context.bufferRight[i] = right[i + offset];

                    i++;
                    if (i >= BUFLEN) {
                        context.length = BUFLEN;
                        context.mStatus = Status.CONTINUE;
                        return true;
                    }
                }

                context.length = i;
                context.aCount += length;
                context.bCount += length;
                context.mStatus = Status.COMPLETE;
                return true;
            } else {
                if (context.mStatus == Status.NORMAL) {
                    // 送られてきたデータで、aStartからaEndまでのデータを作成できる
                    context.aStart = context.aEnd + 1;
                    context.aEnd = calculateNextEnd(context, length);//
                    context.a = context.aStart;
                }

                int i = 0;
                for (; context.a <= context.aEnd; context.a++) {
                    double x = context.a * context.invARate;
                    long bRequired = (long)(x * context.bRate);
                    double x0 = bRequired * context.invBRate;
                    double x1 = (bRequired + 1) * context.invBRate;
                    int indx0 = (int)(bRequired - context.bCount);
                    int indx1 = indx0 + 1;

                    // 左チャンネル
                    double y0 = 0.0;
                    if (0 <= indx0) {
                        if (indx0 < length) {
                            y0 = left[indx0];
                        }
                    } else {
                        int j = (int)(bRequired - context.bBufBase);
                        if (0 <= j && j < context.bBufLeft.Length) {
                            y0 = context.bBufLeft[j];
                        }
                    }
                    double y1 = 0.0;
                    if (indx1 >= 0) {
                        if (indx1 < length) {
                            y1 = left[indx1];
                        }
                    } else {
                        int j = (int)(bRequired + 1 - context.bBufBase);
                        if (0 <= j && j < context.bBufLeft.Length) {
                            y1 = context.bBufLeft[j];
                        }
                    }
                    double s = (y1 - y0) / (x1 - x0);
                    double y = y0 + s * (x - x0);
                    context.bufferLeft[i] = y;

                    // 右チャンネル
                    if (indx0 >= 0) {
                        if (indx0 < length) {
                            y0 = right[indx0];
                        }
                    } else {
                        int j = (int)(bRequired - context.bBufBase);
                        if (0 <= j && j < context.bBufRight.Length) {
                            y0 = context.bBufRight[j];
                        }
                    }
                    if (indx1 >= 0) {
                        if (indx1 < length) {
                            y1 = right[indx1];
                        }
                    } else {
                        int j = (int)(bRequired + 1 - context.bBufBase);
                        if (0 <= j && j < context.bBufRight.Length) {
                            y1 = context.bBufRight[j];
                        }
                    }
                    s = (y1 - y0) / (x1 - x0);
                    y = y0 + s * (x - x0);
                    context.bufferRight[i] = y;

                    // 事後処理
                    i++;
                    if (i >= BUFLEN) {
                        // バッファがいっぱいだったら送信
                        context.length = BUFLEN;
                        context.mStatus = Status.CONTINUE;
                        context.a++;
                        return true;
                    }
                }

                // 未送信のバッファがあれば送信
                context.length = i;

                // 次回に繰り越すデータを確保
                // 次に送られてくるデータはbCount + length + 1から
                long aNext = (long)((context.bCount + length + 1) * context.invBRate * context.aRate) + 1;
                //long aNext = (long)((context.bCount + length + 1) * context.invBRate * context.aRate);
                if (context.aEnd + 1 < aNext) {
                    context.bBufBase = (long)((context.aEnd + 1) * context.invARate * context.bRate) - 2; // aEnd + 1番目のデータを作成するのに必要なデータ点のインデクス
                    int num = (int)(context.bCount + length - context.bBufBase);
                    if (num > 0) {
                        if (context.bBufLeft == null) {
                            context.bBufLeft = new double[num];
                        } else if (context.bBufLeft.Length < num) {
                            Array.Resize(ref context.bBufLeft, num);
                        }
                        if (context.bBufRight == null) {
                            context.bBufRight = new double[num];
                        } else if (context.bBufRight.Length < num) {
                            Array.Resize(ref context.bBufRight, num);
                        }
                        for (int j = 0; j < num; j++) {
                            int indx = (int)(context.bBufBase + j - context.bCount);
                            context.bBufLeft[j] = left[indx];
                            context.bBufRight[j] = right[indx];
                        }
                    }
                }

                context.bCount += length;
                context.aCount = context.aEnd;
                context.mStatus = Status.COMPLETE;
                return true;
            }
        }
    }

#if DEBUG
    /// <summary>
    /// RateConvertContextをテストするクラス
    /// </summary>
    public class TestRateConvertContext
    {
        public static void run()
        {
            string dir = System.Windows.Forms.Application.StartupPath;
            const int LEN = 100000;
            const int BUFLEN = 1000;
            // 元データのサンプルレート
            int rate_from = 48000;
            // 変換するサンプルレート
            int rate_to = 44100;
            // 何ヘルツの音のサイン波を作るか
            const double hz = 440.0;
            // 周期
            double period = 1.0 / hz;
            RateConvertContext c = new RateConvertContext(rate_from, rate_to);

            double[] left = new double[BUFLEN];
            double[] right = new double[BUFLEN];
            int j = 0;
            int total_s = 0;
            int total_d = 0;
            using (System.IO.StreamWriter sw_src = new System.IO.StreamWriter(System.IO.Path.Combine(dir, "TestRateConvertContext_src.txt")))
            using (System.IO.StreamWriter sw_dst = new System.IO.StreamWriter(System.IO.Path.Combine(dir, "TestRateConvertContext_dst.txt"))) {
                for (int i = 0; i < LEN; i++) {
                    double x = i / (double)rate_from;
                    double y = Math.Sin(2.0 * Math.PI * x / period);
                    left[j] = y;
                    right[j] = y;
                    j++;
                    if (j >= BUFLEN) {
                        // srcログに時系列データを書き込み
                        for (int k = 0; k < BUFLEN; k++) {
                            double sx = total_s / (double)rate_from;
                            sw_src.WriteLine(sx + "\t" + left[k]);
                            total_s++;
                        }
                        while (RateConvertContext.convert(c, left, right, j)) {
                            for (int k = 0; k < c.length; k++) {
                                double dx = total_d / (double)rate_to;
                                sw_dst.WriteLine(dx + "\t" + c.bufferRight[k]);
                                total_d++;
                            }
                        }
                        j = 0;
                    }
                }
                if (j > 0) {
                    // srcログに時系列データを書き込み
                    for (int k = 0; k < BUFLEN; k++) {
                        double sx = total_s / (double)rate_from;
                        sw_src.WriteLine(sx + "\t" + left[k]);
                        total_s++;
                    }
                    while (RateConvertContext.convert(c, left, right, j)) {
                        for (int k = 0; k < c.length; k++) {
                            double dx = total_d / (double)rate_to;
                            sw_dst.WriteLine(dx + "\t" + c.bufferRight[k]);
                            total_d++;
                        }
                    }
                }
            }
        }
    }
#endif

}
