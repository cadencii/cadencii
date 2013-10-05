/*
 * UstPortamento.cs
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
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{

    [Serializable]
    public class UstPortamento : ICloneable
    {
        public List<UstPortamentoPoint> Points = new List<UstPortamentoPoint>();
        public int Start;
        /// <summary>
        /// PBSの末尾のセミコロンの後ろについている整数
        /// </summary>
        private int mUnknownInt;
        /// <summary>
        /// mUnknownIntが設定されているかどうか
        /// </summary>
        private bool mIsUnknownIntSpecified = false;

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getXmlElementName(string name)
        {
            return name;
        }

        public void print(ITextWriter sw)
        {
            string pbw = "";
            string pby = "";
            string pbm = "";
            int count = Points.Count;
            for (int i = 0; i < count; i++) {
                string comma = (i == 0 ? "" : ",");
                pbw += comma + Points[i].Step;
                pby += Points[i].Value + ",";
                string type = "";
                UstPortamentoType ut = Points[i].Type;
                if (ut == UstPortamentoType.S) {
                    type = "";
                } else if (ut == UstPortamentoType.Linear) {
                    type = "s";
                } else if (ut == UstPortamentoType.R) {
                    type = "r";
                } else if (ut == UstPortamentoType.J) {
                    type = "j";
                }
                pbm += comma + type;
            }
            sw.write("PBW=" + pbw);
            sw.newLine();
            sw.write("PBS=" + Start + (mIsUnknownIntSpecified ? (";" + mUnknownInt) : ""));
            sw.newLine();
            if (Points.Count >= 2) {
                sw.write("PBY=" + pby);
                sw.newLine();
                sw.write("PBM=" + pbm);
                sw.newLine();
            }
        }

        public Object clone()
        {
            UstPortamento ret = new UstPortamento();
            int count = Points.Count;
            for (int i = 0; i < count; i++) {
                ret.Points.Add(Points[i]);
            }
            ret.Start = Start;
            ret.mIsUnknownIntSpecified = mIsUnknownIntSpecified;
            ret.mUnknownInt = mUnknownInt;
            return ret;
        }

        public object Clone()
        {
            return clone();
        }

        /*
        PBW=50,50,46,48,56,50,50,50,50
        PBS=-87
        PBY=-15.9,-20,-31.5,-26.6
        PBM=,s,r,j,s,s,s,s,s
        */
        public void parseLine(string line)
        {
            line = line.ToLower();
            string[] spl = PortUtil.splitString(line, '=');
            if (spl.Length == 0) {
                return;
            }
            string[] values = PortUtil.splitString(spl[1], ',');
            if (line.StartsWith("pbs=")) {
                string v = values[0];
                int indx = values[0].IndexOf(";", 0);
                if (indx >= 0) {
                    v = values[0].Substring(0, indx);
                    if (values[0].Length > indx + 1) {
                        string unknown = values[0].Substring(indx + 1);
                        mIsUnknownIntSpecified = true;
                        mUnknownInt = int.Parse(unknown);
                    }
                }
                Start = int.Parse(v);
            } else if (line.StartsWith("pbw=")) {
                for (int i = 0; i < values.Length; i++) {
                    if (i >= Points.Count) {
                        Points.Add(new UstPortamentoPoint());
                    }
                    UstPortamentoPoint up = Points[i];
                    up.Step = int.Parse(values[i]);
                    Points[i] = up;
                }
            } else if (line.StartsWith("pby=")) {
                for (int i = 0; i < values.Length; i++) {
                    if (values[i].Length <= 0) {
                        continue;
                    }
                    if (i >= Points.Count) {
                        Points.Add(new UstPortamentoPoint());
                    }
                    UstPortamentoPoint up = Points[i];
                    up.Value = (float)double.Parse(values[i]);
                    Points[i] = up;
                }
            } else if (line.StartsWith("pbm=")) {
                for (int i = 0; i < values.Length; i++) {
                    if (i >= Points.Count) {
                        Points.Add(new UstPortamentoPoint());
                    }
                    UstPortamentoPoint up = Points[i];
                    string search = values[i].ToLower();
                    if (search == "s") {
                        up.Type = UstPortamentoType.Linear;
                    } else if (search == "r") {
                        up.Type = UstPortamentoType.R;
                    } else if (search == "j") {
                        up.Type = UstPortamentoType.J;
                    } else {
                        up.Type = UstPortamentoType.S;
                    }
                    Points[i] = up;
                }
            } else if (line.StartsWith("pbs=")) {

            }
        }
    }

}
