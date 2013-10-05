/*
 * VibratoHandle.cs
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

namespace cadencii.vsq
{

    /// <summary>
    /// ビブラートハンドル
    /// </summary>
    [Serializable]
    public class VibratoHandle : IconParameter, ICloneable
    {
        public int Index;
        public string IconID = "";
        public string IDS = "";
        public int Original;

        public VibratoHandle()
        {
            startRate = 64;
            startDepth = 64;
            rateBP = new VibratoBPList();
            depthBP = new VibratoBPList();
        }

        public VibratoHandle(string aic_file, string ids, string icon_id, int index)
            :
 base(aic_file)
        {
            IDS = ids;
            IconID = icon_id;
            Index = index;
        }

        /// <summary>
        /// このインスタンスと，指定したVibratoHandleのインスタンスが等しいかどうかを調べます
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool equals(VibratoHandle item)
        {
            if (item == null) {
                return false;
            }
            if (startRate != item.startRate) {
                return false;
            }
            if (startDepth != item.startDepth) {
                return false;
            }
            if (IconID != item.IconID) {
                return false;
            }
            if ((item.depthBP == null) != (this.depthBP == null)) {
                // どちらかがnullで，どちらかがnullでない場合，一致するのは考えられない
                return false;
            }
            if ((item.rateBP == null) != (this.rateBP == null)) {
                return false;
            }
            if (this.depthBP != null) {
                if (!this.depthBP.equals(item.depthBP)) {
                    return false;
                }
            }
            if (this.rateBP != null) {
                if (!this.rateBP.equals(item.rateBP)) {
                    return false;
                }
            }
            return true;
        }

        public string toString()
        {
            return getDisplayString();
        }

        public override string ToString()
        {
            return toString();
        }

        public VibratoBPList getRateBP()
        {
            return rateBP;
        }

        public void setRateBP(VibratoBPList value)
        {
            rateBP = value;
        }

        public string getCaption()
        {
            return caption;
        }

        public void setCaption(string value)
        {
            caption = value;
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public string Caption
        {
            get
            {
                return getCaption();
            }
            set
            {
                setCaption(value);
            }
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public VibratoBPList RateBP
        {
            get
            {
                return getRateBP();
            }
            set
            {
                setRateBP(value);
            }
        }

        public int getStartRate()
        {
            return startRate;
        }

        public void setStartRate(int value)
        {
            startRate = value;
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int StartRate
        {
            get
            {
                return getStartRate();
            }
            set
            {
                setStartRate(value);
            }
        }

        public VibratoBPList getDepthBP()
        {
            return depthBP;
        }

        public void setDepthBP(VibratoBPList value)
        {
            depthBP = value;
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public VibratoBPList DepthBP
        {
            get
            {
                return getDepthBP();
            }
            set
            {
                setDepthBP(value);
            }
        }

        public int getStartDepth()
        {
            return startDepth;
        }

        public void setStartDepth(int value)
        {
            startDepth = value;
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int StartDepth
        {
            get
            {
                return getStartDepth();
            }
            set
            {
                setStartDepth(value);
            }
        }

        public int getLength()
        {
            return length;
        }

        public void setLength(int value)
        {
            length = value;
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int Length
        {
            get
            {
                return getLength();
            }
            set
            {
                setLength(value);
            }
        }

        public string getDisplayString()
        {
            return caption;
        }

        public object Clone()
        {
            return clone();
        }

        public Object clone()
        {
            VibratoHandle result = new VibratoHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = this.IDS;
            result.Original = this.Original;
            result.setCaption(caption);
            result.setLength(getLength());
            result.setStartDepth(startDepth);
            if (depthBP != null) {
                result.setDepthBP((VibratoBPList)depthBP.clone());
            }
            result.setStartRate(startRate);
            if (rateBP != null) {
                result.setRateBP((VibratoBPList)rateBP.clone());
            }
            return result;
        }

        public VsqHandle castToVsqHandle()
        {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Vibrato;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = caption;
            ret.setLength(getLength());
            ret.StartDepth = startDepth;
            ret.StartRate = startRate;
            ret.DepthBP = (VibratoBPList)depthBP.clone();
            ret.RateBP = (VibratoBPList)rateBP.clone();
            return ret;
        }
    }

}
