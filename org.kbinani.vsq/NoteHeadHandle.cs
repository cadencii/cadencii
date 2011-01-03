/*
 * NoteHeadHandle.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace org.kbinani.vsq {
#endif

#if JAVA
    public class NoteHeadHandle extends IconParameter implements Cloneable, Serializable {
#else
    [Serializable]
    public class NoteHeadHandle : IconParameter, ICloneable {
#endif
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;

        public NoteHeadHandle() {
        }

        public NoteHeadHandle( String aic_file, String ids, String icon_id, int index )
#if JAVA
        {
#else
            :
#endif
            base( aic_file )
#if JAVA
            ;
#else
        {
#endif
            IDS = ids;
            IconID = icon_id;
            Index = index;
        }

        public String toString() {
            return getDisplayString();
        }

#if !JAVA
        public override String ToString(){
            return toString();
        }
#endif

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int Depth{
            get{
                return getDepth();
            }
            set{
                setDepth( value );
            }
        }
#endif

        public int getDepth() {
            return depth;
        }

        public void setDepth( int value ) {
            depth = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int Duration {
            get {
                return getDuration();
            }
            set {
                setDuration( value );
            }
        }
#endif

        public int getDuration() {
            return duration;
        }

        public void setDuration( int value ) {
            duration = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String Caption {
            get {
                return getCaption();
            }
            set {
                setCaption( value );
            }
        }
#endif

        public String getCaption() {
            return caption;
        }

        public void setCaption( String value ) {
            caption = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int Length {
            get {
                return getLength();
            }
            set {
                setLength( value );
            }
        }
#endif

        public int getLength() {
            return length;
        }

        public void setLength( int value ) {
            length = value;
        }

        public String getDisplayString() {
            return IDS + caption;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public Object clone() {
            NoteHeadHandle result = new NoteHeadHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = IDS;
            result.Original = Original;
            result.setCaption( getCaption() );
            result.setLength( getLength() );
            result.setDuration( getDuration() );
            result.setDepth( getDepth() );
            return result;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.NoteHeadHandle;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = getCaption();
            ret.setLength( getLength() );
            ret.Duration = getDuration();
            ret.Depth = getDepth();
            return ret;
        }
    }

#if !JAVA
}
#endif
