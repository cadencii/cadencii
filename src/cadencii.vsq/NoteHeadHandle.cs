/*
 * NoteHeadHandle.cs
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

    [Serializable]
    public class NoteHeadHandle : IconParameter, ICloneable
    {
        public int Index;
        public string IconID = "";
        public string IDS = "";
        public int Original;

        public NoteHeadHandle()
        {
        }

        public NoteHeadHandle(string aic_file, string ids, string icon_id, int index)
            :
 base(aic_file)
        {
            IDS = ids;
            IconID = icon_id;
            Index = index;
        }

        public string toString()
        {
            return getDisplayString();
        }

        public override string ToString()
        {
            return toString();
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int Depth
        {
            get
            {
                return getDepth();
            }
            set
            {
                setDepth(value);
            }
        }

        public int getDepth()
        {
            return depth;
        }

        public void setDepth(int value)
        {
            depth = value;
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int Duration
        {
            get
            {
                return getDuration();
            }
            set
            {
                setDuration(value);
            }
        }

        public int getDuration()
        {
            return duration;
        }

        public void setDuration(int value)
        {
            duration = value;
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

        public int getLength()
        {
            return length;
        }

        public void setLength(int value)
        {
            length = value;
        }

        public string getDisplayString()
        {
            return IDS + caption;
        }

        public object Clone()
        {
            return clone();
        }

        public Object clone()
        {
            NoteHeadHandle result = new NoteHeadHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = IDS;
            result.Original = Original;
            result.setCaption(getCaption());
            result.setLength(getLength());
            result.setDuration(getDuration());
            result.setDepth(getDepth());
            return result;
        }

        public VsqHandle castToVsqHandle()
        {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.NoteHeadHandle;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = getCaption();
            ret.setLength(getLength());
            ret.Duration = getDuration();
            ret.Depth = getDepth();
            return ret;
        }
    }

}
