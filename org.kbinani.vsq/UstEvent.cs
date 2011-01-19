/*
 * UstEvent.cs
 * Copyright © 2009-2011 kbinani, HAL
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
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.io;

namespace org.kbinani.vsq
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class UstEvent implements Cloneable, Serializable {
#else
    [Serializable]
    public class UstEvent : ICloneable
    {
#endif
        /// <summary>
        /// 音量の最大値
        /// </summary>
        public const int MAX_INTENSITY = 200;
        /// <summary>
        /// 音量の最小値
        /// </summary>
        public const int MIN_INTENSITY = -100;

        public String Tag;
        public int Length = 0;
        public String Lyric = "";
        public int Note = -1;
        public int Intensity = 100;
        public int PBType = -1;
        public float[] Pitches = null;
        public float Tempo = -1;
        public UstVibrato Vibrato = null;
        public UstPortamento Portamento = null;
        public float PreUtterance = 0;
        public float VoiceOverlap = 0;
        public UstEnvelope Envelope = null;
        public String Flags = "";
        public int Moduration = 100;
        public int Index;
        private float mStartPoint;
        private boolean mIsStartPointSpecified = false;

        public UstEvent()
        {
        }

        public float getStartPoint()
        {
            return mStartPoint;
        }

        public void setStartPoint( float value )
        {
            mStartPoint = value;
            mIsStartPointSpecified = true;
        }

        public boolean isStartPointSpecified()
        {
            return mIsStartPointSpecified;
        }

#if !JAVA
        public float StartPoint
        {
            get
            {
                return getStartPoint();
            }
            set
            {
                setStartPoint( value );
            }
        }
#endif

        public int getLength()
        {
            return Length;
        }

        public void setLength( int value )
        {
            Length = value;
        }

        public Object clone()
        {
            UstEvent ret = new UstEvent();
            ret.setLength( Length );
            ret.Lyric = Lyric;
            ret.Note = Note;
            ret.Intensity = Intensity;
            ret.PBType = PBType;
            if ( Pitches != null ) {
                ret.Pitches = new float[Pitches.Length];
                for ( int i = 0; i < Pitches.Length; i++ ) {
                    ret.Pitches[i] = Pitches[i];
                }
            }
            ret.Tempo = Tempo;
            if ( Vibrato != null ) {
                ret.Vibrato = (UstVibrato)Vibrato.clone();
            }
            if ( Portamento != null ) {
                ret.Portamento = (UstPortamento)Portamento.clone();
            }
            if ( Envelope != null ) {
                ret.Envelope = (UstEnvelope)Envelope.clone();
            }
            ret.PreUtterance = PreUtterance;
            ret.VoiceOverlap = VoiceOverlap;
            ret.Flags = Flags;
            ret.Moduration = Moduration;
            ret.mStartPoint = mStartPoint;
            ret.mIsStartPointSpecified = mIsStartPointSpecified;
            ret.Tag = Tag;
            return ret;
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif

        public void print( BufferedWriter sw )
#if JAVA
            throws IOException
#endif
        {
            if ( this.Index == UstFile.PREV_INDEX ) {
                sw.write( "[#PREV]" );
                sw.newLine();
            } else if ( this.Index == UstFile.NEXT_INDEX ) {
                sw.write( "[#NEXT]" );
                sw.newLine();
            } else {
                sw.write( "[#" + PortUtil.formatDecimal( "0000", Index ) + "]" );
                sw.newLine();
            }
            sw.write( "Length=" + Length );
            sw.newLine();
            sw.write( "Lyric=" + Lyric );
            sw.newLine();
            sw.write( "NoteNum=" + Note );
            sw.newLine();
            if ( Intensity >= 0 ) {
                sw.write( "Intensity=" + Intensity );
                sw.newLine();
            }
            if ( PBType >= 0 && Pitches != null ) {
                sw.write( "PBType=" + PBType );
                sw.newLine();
                sw.write( "Piches=" );
                for ( int i = 0; i < Pitches.Length; i++ ) {
                    if ( i == 0 ) {
                        sw.write( Pitches[i] + "" );
                    } else {
                        sw.write( "," + Pitches[i] );
                    }
                }
                sw.newLine();
            }
            if ( Tempo > 0 ) {
                sw.write( "Tempo=" + Tempo );
                sw.newLine();
            }
            if ( Vibrato != null ) {
                sw.write( Vibrato.ToString() );
                sw.newLine();
            }
            if ( Portamento != null ) {
                Portamento.print( sw );
            }
            if ( PreUtterance != 0 ) {
                sw.write( "PreUtterance=" + PreUtterance );
                sw.newLine();
            }
            if ( VoiceOverlap != 0 ) {
                sw.write( "VoiceOverlap=" + VoiceOverlap );
                sw.newLine();
            }
            if ( Envelope != null ) {
                sw.write( Envelope.ToString() );
                sw.newLine();
            }
            if ( Flags != "" ) {
                sw.write( "Flags=" + Flags );
                sw.newLine();
            }
            if ( Moduration >= 0 ) {
                sw.write( "Moduration=" + Moduration );
                sw.newLine();
            }
            if ( mIsStartPointSpecified ) {
                sw.write( "StartPoint=" + getStartPoint() );
                sw.newLine();
            }
        }

        /// <summary>
        /// このインスタンスと指定したアイテムが，歌声合成の観点から等しいかどうかを調べます
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public boolean equalsForSynth( UstEvent item )
        {
            if ( item == null ) {
                return false;
            }
            boolean ret = true;
            // モジュレーション・先行発声・スタート位置・オーバーラップのみチェック．
            // ほかに有効な値でかつ VsqEvent で比較できないものは何かあったか
            if ( this.Moduration != item.Moduration ) ret = false;
            else if ( this.PreUtterance != item.PreUtterance ) ret = false;
            else if ( this.StartPoint != item.StartPoint ) ret = false;
            else if ( this.VoiceOverlap != item.VoiceOverlap ) ret = false;
            return ret;
        }

        /*public VsqEvent convertToVsqEvent( int clock, int internal_id ) {
            VsqEvent ret = new VsqEvent();
            ret.Clock = clock;
            ret.InternalID = internal_id;
            ret.UstEvent = (UstEvent)this.clone();
            ret.ID.setLength( Length );
            ByRef<string> phonetic_symbol = new ByRef<string>( "" );
            SymbolTable.attatch( Lyric, phonetic_symbol );
            ret.ID.LyricHandle = new LyricHandle( Lyric, phonetic_symbol.value );
            ret.ID.Note = Note;
            ret.ID.Dynamics = Intensity;
            ret.ID.type = VsqIDType.Anote;
            return ret;
        }*/
    }

#if !JAVA
}
#endif
