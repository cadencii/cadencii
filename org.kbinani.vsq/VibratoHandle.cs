/*
 * VibratoHandle.cs
 * Copyright © 2009-2010 kbinani
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
    public class VibratoHandle extends IconParameter implements Cloneable, Serializable {
#else
    [Serializable]
    public class VibratoHandle : IconParameter, ICloneable {
#endif
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;

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

        /// <summary>
        /// このビブラートの、位置x(0&lt;=x&lt;=1)におけるピッチベンド(ノートナンバー単位)を計算します
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="clock_length"></param>
        /// <param name="clock_start"></param>
        /// <param name="vsq"></param>
        /// <returns></returns>
        public double calculatePitchbend( int clock, int clock_start, int clock_length, VsqFile vsq ) {
            return calculatePitchbendCor( this.startRate, this.rateBP, 
                                          this.startDepth, this.depthBP,
                                          clock, clock_start, clock_length, vsq );
        }

        public static double calculatePitchbendCor( int start_rate, VibratoBPList rateBP,
                                                    int start_depth, VibratoBPList depthBP,
                                                    int clock, int clock_start, int clock_length, VsqFile vsq ) {
            if ( rateBP == null && depthBP == null ) {
                return 0.0;
            }
            if ( clock <= clock_start ) {
                return 0.0;
            }
            if ( clock_start + clock_length < clock ) {
                return 0.0;
            }
            int index_rate = -1;
            int index_depth = -1;

            double sec0 = vsq.getSecFromClock( clock_start );
            double sec1 = vsq.getSecFromClock( clock_start + clock_length );
            double fadewidth = (sec1 - sec0) * 0.2;

            double x = (clock - clock_start) / (double)clock_length;
            int rate = start_rate;
            int depth = start_depth;
            double phase = 0.0;
            double lastx = 0.0;
            double lastsec = sec0;
            double lastclock = clock_start;

            double amp = depth * 2.5 / 127.0 / 2.0;
            double period = Math.Exp( 5.24 - 1.07e-2 * rate ) * 2.0 / 1000.0;
            double omega = 2.0 * Math.PI / period;

            while ( true ) {
                double nextx_rate = 1.0;
                if ( rateBP != null && index_rate + 1 < rateBP.getCount() ) {
                    nextx_rate = rateBP.getElement( index_rate + 1 ).X;
                }
                double nextx_depth = 1.0;
                if ( depthBP != null && index_depth + 1 < depthBP.getCount() ) {
                    nextx_depth = depthBP.getElement( index_depth + 1 ).X;
                }
                // depth, rateの次のデータ点のうち、一番若いやつ
                double nextx = Math.Min( nextx_rate, nextx_depth );
                if ( nextx >= x ) {
                    // 次のデータ点のどちらも、目的のxよりもでかいか等しい場合、indexをインクリメントしないよ
                } else {
                    if ( nextx_depth == nextx_rate ) {
                        // 両方インクリメント
                        index_depth++;
                        index_rate++;
                    }else if ( nextx_depth < nextx_rate ) {
                        // depthだけの方をインクリメント
                        index_depth++;
                    } else {
                        // rateの方をインクリメント
                        index_rate++;
                    }
                }
                if ( nextx > x ) {
                    nextx = x;
                }
                double dx = nextx - lastx;
                double nextclock = clock_start + nextx * clock_length;
                double nextsec = vsq.getSecFromClock( nextclock );
                double dsec = nextsec - lastsec;
                phase += dsec * omega;

                if ( rateBP != null && 0 <= index_rate && index_rate < rateBP.getCount() ) {
                    rate = rateBP.getElement( index_rate ).Y;
                }
                if ( depthBP != null && 0 <= index_depth && index_depth < depthBP.getCount() ) {
                    depth = depthBP.getElement( index_depth ).Y;
                }
                amp = depth * 2.5 / 127.0 / 2.0;
                period = Math.Exp( 5.24 - 1.07e-2 * rate ) * 2.0 / 1000.0;
                omega = 2.0 * Math.PI / period;
                lastclock = nextclock;
                lastsec = nextsec;
                lastx = nextx;

                if ( nextx >= x ) {
                    break;
                }
            }
            double sec = vsq.getSecFromClock( clock );
            if ( sec0 <= sec && sec <= sec0 + fadewidth ) {
                amp *= (sec - sec0) / fadewidth;
            }
            if ( sec1 - fadewidth <= sec && sec <= sec1 ) {
                amp *= (sec1 - sec) / fadewidth;
            }
            return amp * Math.Sin( phase );
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
            result.setCaption( caption );
            result.setLength( getLength() );
            result.setStartDepth( startDepth );
            if ( depthBP != null ) {
                result.setDepthBP( (VibratoBPList)depthBP.clone() );
            }
            result.setStartRate( startRate );
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
            ret.Caption = caption;
            ret.setLength( getLength() );
            ret.StartDepth = startDepth;
            ret.StartRate = startRate;
            ret.DepthBP = (VibratoBPList)depthBP.clone();
            ret.RateBP = (VibratoBPList)rateBP.clone();
            return ret;
        }
    }

#if !JAVA
}
#endif
