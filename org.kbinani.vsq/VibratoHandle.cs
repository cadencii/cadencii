/*
 * VibratoHandle.cs
 * Copyright (C) 2009-2010 kbinani
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
    public class VibratoHandle implements Cloneable, Serializable {
#else
    [Serializable]
    public class VibratoHandle : IconParameter, ICloneable {
#endif
        /*public int StartDepth;
        public VibratoBPList DepthBP;
        public int StartRate;
        public VibratoBPList RateBP;*/
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;
        /*public String Caption = "";
        public int Length;*/

        public VibratoHandle() {
            startRate = 64;
            startDepth = 64;
            rateBP = new VibratoBPList();
            depthBP = new VibratoBPList();
        }

        public VibratoHandle( String aic_file, String ids, String icon_id, int index )
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
        public override string ToString() {
            return toString();
        }
#endif

        public VibratoBPList getRateBP() {
            return rateBP;
        }

        public void setRateBP( VibratoBPList value ) {
            rateBP = value;
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
        public String Caption{
            get{
                return getCaption();
            }
            set{
                setCaption( value );
            }
        }
#endif

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public VibratoBPList RateBP{
            get{
                return getRateBP();
            }
            set{
                setRateBP( value );
            }
        }
#endif

        public int getStartRate() {
            return startRate;
        }

        public void setStartRate( int value ) {
            startRate = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int StartRate{
            get{
                return getStartRate();
            }
            set{
                setStartRate( value );
            }
        }
#endif

        public VibratoBPList getDepthBP() {
            return depthBP;
        }

        public void setDepthBP( VibratoBPList value ) {
            depthBP = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public VibratoBPList DepthBP{
            get{
                return getDepthBP();
            }
            set{
                setDepthBP( value );
            }
        }
#endif

        public int getStartDepth() {
            return startDepth;
        }

        public void setStartDepth( int value ) {
            startDepth = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int StartDepth{
            get{
                return getStartDepth();
            }
            set{
                setStartDepth( value );
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
        public int Length{
            get{
                return getLength();
            }
            set{
                setLength( value );
            }
        }
#endif

        public String getDisplayString() {
            return caption;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public Object clone() {
            VibratoHandle result = new VibratoHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = this.IDS;
            result.Original = this.Original;
            result.setCaption( getCaption() );
            result.setLength( getLength() );
            result.setStartDepth( getStartDepth() );
            if ( depthBP != null ) {
                result.setDepthBP( (VibratoBPList)depthBP.clone() );
            }
            result.setStartRate( getStartRate() );
            if ( rateBP != null ) {
                result.setRateBP( (VibratoBPList)rateBP.clone() );
            }
            return result;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Vibrato;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = Caption;
            ret.setLength( Length );
            ret.StartDepth = StartDepth;
            ret.StartRate = StartRate;
            ret.DepthBP = (VibratoBPList)DepthBP.clone();
            ret.RateBP = (VibratoBPList)RateBP.clone();
            return ret;
        }
    }

#if !JAVA
}
#endif
