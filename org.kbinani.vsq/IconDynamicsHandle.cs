/*
 * IconDynamicsHandle.cs
 * Copyright (C) 2009 kbinani
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

#else
using System;

namespace org.kbinani.vsq {
#endif

    public class IconDynamicsHandle : IconParameter, ICloneable {
        public String IconID = "";
        public String IDS = "";
        public int Original;

        public IconDynamicsHandle()
            : base() {
        }

        public IconDynamicsHandle( String aic_file, String ids, String icon_id, int index )
            : base( aic_file ) {
            IDS = ids;
            IconID = icon_id;
            Original = index;
        }

#if !JAVA
        public Object Clone() {
            return clone();
        }
#endif

        public Object clone() {
            IconDynamicsHandle ret = new IconDynamicsHandle();
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.setCaption( getCaption() );
            ret.setStartDyn( getStartDyn() );
            ret.setEndDyn( getEndDyn() );
            if ( dynBP != null ){
                ret.setDynBP( (VibratoBPList)dynBP.clone() );
            }
            ret.setLength( getLength() );
            return ret;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.DynamicsHandle;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = getCaption();
            ret.DynBP = getDynBP();
            ret.EndDyn = getEndDyn();
            ret.setLength( getLength() );
            ret.StartDyn = getStartDyn();
            return ret;
        }

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
        public String Caption {
            get {
                return getCaption();
            }
            set {
                setCaption( value );
            }
        }
#endif

        public int getLength() {
            return length;
        }

        public void setLength( int value ) {
            length = value;
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

        public int getStartDyn() {
            return startDyn;
        }

        public void setStartDyn( int value ) {
            startDyn = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int StartDyn {
            get {
                return getStartDyn();
            }
            set {
                setStartDyn( value );
            }
        }
#endif

        public int getEndDyn() {
            return endDyn;
        }

        public void setEndDyn( int value ) {
            endDyn = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int EndDyn {
            get {
                return getEndDyn();
            }
            set {
                setEndDyn( value );
            }
        }
#endif

        public VibratoBPList getDynBP() {
            return dynBP;
        }

        public void setDynBP( VibratoBPList value ) {
            dynBP = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public VibratoBPList DynBP {
            get {
                return getDynBP();
            }
            set {
                setDynBP( value );
            }
        }
#endif

    }

#if !JAVA
}
#endif
