/*
 * PatchWorkQueue.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using cadencii.apputil;



namespace cadencii
{

    /// <summary>
    /// 合成の範囲やトラック番号を指示するためのクラス
    /// </summary>
    public class PatchWorkQueue
    {
        /// <summary>
        /// 合成対象のトラック番号
        /// </summary>
        public int track;
        /// <summary>
        /// 合成開始位置．単位はclock
        /// </summary>
        public int clockStart;
        /// <summary>
        /// 合成修了位置．単位はclock
        /// </summary>
        public int clockEnd;
        /// <summary>
        /// 合成結果を出力するファイル名
        /// </summary>
        public string file;
        /// <summary>
        /// トラック全体を合成する場合true，それ以外はfalse
        /// </summary>
        public bool renderAll;
        /// <summary>
        /// シーケンスのインスタンス
        /// </summary>
        public VsqFileEx vsq;

        /// <summary>
        /// このキューの概要を記した文字列を取得します
        /// </summary>
        /// <returns></returns>
        public string getMessage()
        {
            string message = _("track") + "#" + this.track + " ";
#if DEBUG
            sout.println("PatchWorkQueue#getMessage; q.clockStart=" + this.clockStart + "; q.clockEnd=" + this.clockEnd);
#endif
            double start = this.vsq.getSecFromClock(this.clockStart);
            double cend = this.clockEnd;
            if (this.clockEnd == int.MaxValue) {
                cend = this.vsq.TotalClocks + 240;
            }
            double end = this.vsq.getSecFromClock(cend);
            int istart = (int)Math.Floor(start);
            int iend = (int)Math.Floor(end);
            message += istart + "." + ((int)((start - istart) * 100)).ToString("D2") + " " + _("sec");
            message += " - ";
            message += iend + "." + ((int)((end - iend) * 100)).ToString("D2") + " " + _("sec");

            return message;
        }

        public double getJobAmount()
        {
            double start = this.vsq.getSecFromClock(this.clockStart);
            double cend = this.clockEnd;
            if (this.clockEnd == int.MaxValue) {
                cend = this.vsq.TotalClocks + 240;
            }
            double end = this.vsq.getSecFromClock(cend);
            return (end - start) * vsq.config.SamplingRate;
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }
    }

}
