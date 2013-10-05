/*
 * IconParameter.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Text;
using cadencii.java.io;

namespace cadencii.vsq
{

    /// <summary>
    /// アイコン設定ファイルである*.AICファイルを読み取ることで作成されるアイコン設定を表します。
    /// アイコン設定ファイルを使用するIconDynamicsHandle、NoteHeadHandle、およびVibratoHandleの基底クラスとなっています。
    /// <see cref="T:org.kbinani.vsq.IconDynamicsHandle"/>
    /// <see cref="T:org.kbinani.vsq.NoteHeadHandle"/>
    /// <see cref="T:org.kbinani.vsq.VibratoHandle"/>
    /// </summary>
    [Serializable]
    public class IconParameter
    {
        /// <summary>
        /// アイコン設定の種類を表します。
        /// </summary>
        public enum ArticulationType
        {
            /// <summary>
            /// ビブラート
            /// </summary>
            Vibrato,
            /// <summary>
            /// クレッシェンド、またはデクレッシェンド
            /// </summary>
            Crescendo,
            /// <summary>
            /// ピアノ、フォルテ等の強弱記号
            /// </summary>
            Dynaff,
            /// <summary>
            /// アタック
            /// </summary>
            NoteAttack,
            /// <summary>
            /// NoteTransition(詳細不明)
            /// </summary>
            NoteTransition,
        }

        /// <summary>
        /// アイコン設定の種類
        /// </summary>
        protected ArticulationType articulation;
        /// <summary>
        /// アイコンのボタンに使用される画像ファイルへの相対パス
        /// </summary>
        protected string button = "";
        /// <summary>
        /// キャプション
        /// </summary>
        protected string caption = "";

        /// <summary>
        /// ゲートタイム長さ
        /// </summary>
        protected int length;
        /// <summary>
        /// ビブラート深さの開始値
        /// </summary>
        protected int startDepth;
        /// <summary>
        /// ビブラート深さの終了値
        /// </summary>
        protected int endDepth;
        /// <summary>
        /// ビブラート速さの開始値
        /// </summary>
        protected int startRate;
        /// <summary>
        /// ビブラート速さの終了値
        /// </summary>
        protected int endRate;
        protected int startDyn;
        protected int endDyn;
        protected int duration;
        protected int depth;
        protected VibratoBPList dynBP;
        protected VibratoBPList depthBP;
        protected VibratoBPList rateBP;

        protected string buttonImageFullPath = "";

        protected IconParameter()
        {
        }

        protected IconParameter(string file)
        {
            if (file == null) {
                return;
            }
            if (file == "") {
                return;
            }
            StreamReader sr = null;
            try {
                sr = new StreamReader(file, Encoding.GetEncoding("Shift_JIS"));
                string line = "";
                string strDynBPNum = "";
                string strDynBPX = "";
                string strDynBPY = "";
                string strDepthBPNum = "";
                string strDepthBPX = "";
                string strDepthBPY = "";
                string strRateBPNum = "";
                string strRateBPX = "";
                string strRateBPY = "";
                while ((line = sr.ReadLine()) != null) {
                    // コメントを除去する
                    int indx_colon = line.IndexOf(';');
                    if (indx_colon >= 0) {
                        line = line.Substring(0, indx_colon);
                    }
                    // セクション名の指定行
                    if (line.StartsWith("[")) {
                        continue;
                    }
                    // イコールが含まれているかどうか
                    string[] spl = PortUtil.splitString(line, new char[] { '=' }, 2);
                    if (spl.Length != 2) {
                        continue;
                    }
                    string name = spl[0].Trim();// new char[]{ ' ', '\t' } );
                    string value = spl[1].Trim();// new char[]{ ' ', '\t' } );
                    if (name == "Articulation") {
                        if (value == "Vibrato") {
                            articulation = ArticulationType.Vibrato;
                        } else if (value == "Crescendo") {
                            articulation = ArticulationType.Crescendo;
                        } else if (value == "Dynaff") {
                            articulation = ArticulationType.Dynaff;
                        } else if (value == "NoteAttack") {
                            articulation = ArticulationType.NoteAttack;
                        } else if (value == "NoteTransition") {
                            articulation = ArticulationType.NoteTransition;
                        }
                    } else if (name == "Button") {
                        button = value;
                    } else if (name == "Caption") {
                        caption = value;
                    } else if (name == "Length") {
                        try {
                            length = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "StartDepth") {
                        try {
                            startDepth = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "EndDepth") {
                        try {
                            endDepth = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "StartRate") {
                        try {
                            startRate = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "EndRate") {
                        try {
                            endRate = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "StartDyn") {
                        try {
                            startDyn = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "EndDyn") {
                        try {
                            endDyn = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "Duration") {
                        try {
                            duration = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "Depth") {
                        try {
                            depth = int.Parse(value);
                        } catch (Exception ex) {
                            serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
                        }
                    } else if (name == "DynBPNum") {
                        strDynBPNum = value;
                    } else if (name == "DynBPX") {
                        strDynBPX = value;
                    } else if (name == "DynBPY") {
                        strDynBPY = value;
                    } else if (name == "DepthBPNum") {
                        strDepthBPNum = value;
                    } else if (name == "DepthBPX") {
                        strDepthBPX = value;
                    } else if (name == "DepthBPY") {
                        strDepthBPY = value;
                    } else if (name == "RateBPNum") {
                        strRateBPNum = value;
                    } else if (name == "RateBPX") {
                        strRateBPX = value;
                    } else if (name == "RateBPY") {
                        strRateBPY = value;
                    }
                }
                dynBP = getBPListFromText(strDynBPNum, strDynBPX, strDynBPY);
                rateBP = getBPListFromText(strRateBPNum, strRateBPX, strRateBPY);
                depthBP = getBPListFromText(strDepthBPNum, strDepthBPX, strDepthBPY);
            } catch (Exception ex) {
                serr.println("org.kbinani.vsq.IconParameter#.ctor; ex=" + ex);
            } finally {
                if (sr != null) {
                    try {
                        sr.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
        }

        /// <summary>
        /// テキストデータからVibratoBPListを構築する
        /// </summary>
        /// <param name="strNum"></param>
        /// <param name="strBPX"></param>
        /// <param name="strBPY"></param>
        /// <returns></returns>
        private static VibratoBPList getBPListFromText(string strNum, string strBPX, string strBPY)
        {
            VibratoBPList ret = null;
            if (strNum == null || (strNum != null && strNum == "")) {
                return ret;
            }
            int num = 0;
            try {
                num = int.Parse(strNum);
            } catch (Exception ex) {
                serr.println("org.kbinani.vsq.IconParameter.getBPListFromText; ex=" + ex);
                num = 0;
            }
            string[] sx = PortUtil.splitString(strBPX, ',');
            string[] sy = PortUtil.splitString(strBPY, ',');
            int actNum = Math.Min(num, Math.Min(sx.Length, sy.Length));
            if (actNum > 0) {
                float[] x = new float[actNum];
                int[] y = new int[actNum];
                for (int i = 0; i < actNum; i++) {
                    try {
                        x[i] = (float)double.Parse(sx[i]);
                        y[i] = int.Parse(sy[i]);
                    } catch (Exception ex) {
                        serr.println("org.kbinani.vsq.IconParameter.getBPListFromText; ex=" + ex);
                    }
                }
                ret = new VibratoBPList(x, y);
            }
            return ret;
        }

        public string getButton()
        {
            return button;
        }

        public string getButtonImageFullPath()
        {
            return buttonImageFullPath;
        }

        public void setButtonImageFullPath(string value)
        {
            buttonImageFullPath = value;
        }
    }

}
