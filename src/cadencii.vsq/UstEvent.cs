/*
 * UstEvent.cs
 * Copyright © 2009-2011 kbinani, HAL
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
#if JAVA
package cadencii.vsq;

import java.io.*;
import java.util.*;
import cadencii.*;
import cadencii.xml.*;

#else
using System;
using cadencii;
using cadencii.java.io;
using cadencii.java.util;

namespace cadencii.vsq
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class UstEvent implements Cloneable, Serializable
#else
    [Serializable]
    public class UstEvent : ICloneable
#endif
    {
        /// <summary>
        /// 音量の最大値
        /// </summary>
        public const int MAX_INTENSITY = 200;
        /// <summary>
        /// 音量の最小値
        /// </summary>
        public const int MIN_INTENSITY = -100;

        public String Tag;

        private String mLyric = "";
        private boolean mIsLyricSpec = false;

        private int mNote = -1;
        private boolean mIsNoteSpec = false;

        private int mIntensity = 100;
        private boolean mIsIntensitySpec = false;

        private int mPBType = -1;
        private boolean mIsPBTypeSpec = false;

        private float[] mPitches = null;
        private boolean mIsPitchesSpec = false;
        
        private float mTempo = -1;
        private boolean mIsTempoSpec = false;

        private UstVibrato mVibrato = null;
        private boolean mIsVibratoSpec = false;

        private UstPortamento mPortamento = null;
        private boolean mIsPortamentoSpec = false;

        private float mPreUtterance = 0;
        private boolean mIsPreUtteranceSpec = false;

        private float mVoiceOverlap = 0;
        private boolean mIsVoiceOverlapSpec = false;
        
        private UstEnvelope mEnvelope = null;
        private boolean mIsEnvelopeSpec = false;

        public String Flags = "";
        
        private int mModuration = 100;
        private boolean mIsModurationSpec = false;
        
        public int Index;

        private float mStartPoint;
        private boolean mIsStartPointSpec = false;
        
        private int mLength = 0;
        private boolean mIsLengthSpec = false;

#if JAVA
        @XmlGenericType( UstEventProperty.class )
#endif
        public Vector<UstEventProperty> Properties = new Vector<UstEventProperty>();

        public UstEvent()
        {
        }

        #region Lyric
        public String getLyric()
        {
            return mLyric;
        }

        public void setLyric( String value )
        {
            mLyric = value;
            mIsLyricSpec = true;
        }

        public boolean isLyricSpecified()
        {
            return mIsLyricSpec;
        }

#if !JAVA
        public String Lyric
        {
            get
            {
                return getLyric();
            }
            set
            {
                setLyric( value );
            }
        }
#endif
        #endregion

        #region Note
        public int getNote()
        {
            return mNote;
        }

        public void setNote( int value )
        {
            mNote = value;
            mIsNoteSpec = true;
        }

        public boolean isNoteSpecified()
        {
            return mIsNoteSpec;
        }

#if !JAVA
        public int Note
        {
            get
            {
                return getNote();
            }
            set
            {
                setNote( value );
            }
        }
#endif
        #endregion

        #region Intensity
        public int getIntensity()
        {
            return mIntensity;
        }

        public void setIntensity( int value )
        {
            mIntensity = value;
            mIsIntensitySpec = true;
        }

        public boolean isIntensitySpecified()
        {
            return mIsIntensitySpec;
        }

#if !JAVA
        public int Intensity
        {
            get
            {
                return getIntensity();
            }
            set
            {
                setIntensity( value );
            }
        }
#endif
        #endregion

        #region PBType
        public int getPBType()
        {
            return mPBType;
        }

        public void setPBType( int value )
        {
            mPBType = value;
            mIsPBTypeSpec = true;
        }

        public boolean isPBTypeSpecified()
        {
            return mIsPBTypeSpec;
        }

#if !JAVA
        public int PBType
        {
            get
            {
                return getPBType();
            }
            set
            {
                setPBType( value );
            }
        }
#endif
        #endregion

        #region Pitches
        public float[] getPitches()
        {
            return mPitches;
        }

        public void setPitches( float[] value )
        {
            mPitches = value;
            mIsPitchesSpec = true;
        }

        public boolean isPitchesSpecified()
        {
            return mIsPitchesSpec;
        }

#if !JAVA
        public float[] Pitches
        {
            get
            {
                return getPitches();
            }
            set
            {
                setPitches( value );
            }
        }
#endif
        #endregion

        #region Tempo
        public float getTempo()
        {
            return mTempo;
        }

        public void setTempo( float value )
        {
            mTempo = value;
            mIsTempoSpec = true;
        }

        public boolean isTempoSpecified()
        {
            return mIsTempoSpec;
        }

#if !JAVA
        public float Tempo
        {
            get
            {
                return getTempo();
            }
            set
            {
                setTempo( value );
            }
        }
#endif
        #endregion

        #region Vibrato
        public UstVibrato getVibrato()
        {
            return mVibrato;
        }

        public void setVibrato( UstVibrato value )
        {
            mVibrato = value;
            mIsVibratoSpec = true;
        }

        public boolean isVibratoSpecified()
        {
            return mIsVibratoSpec;
        }

#if !JAVA
        public UstVibrato Vibrato
        {
            get
            {
                return getVibrato();
            }
            set
            {
                setVibrato( value );
            }
        }
#endif
        #endregion

        #region Portamento
        public UstPortamento getPortamento()
        {
            return mPortamento;
        }

        public void setPortamento( UstPortamento value )
        {
            mPortamento = value;
            mIsPortamentoSpec = true;
        }

        public boolean isPortamentoSpecified()
        {
            return mIsPortamentoSpec;
        }

#if !JAVA
        public UstPortamento Portamento
        {
            get
            {
                return getPortamento();
            }
            set
            {
                setPortamento( value );
            }
        }
#endif
        #endregion

        #region PreUtterance
        public float getPreUtterance()
        {
            return mPreUtterance;
        }

        public void setPreUtterance( float value )
        {
            mPreUtterance = value;
            mIsPreUtteranceSpec = true;
        }

        public boolean isPreUtteranceSpecified()
        {
            return mIsPreUtteranceSpec;
        }

#if !JAVA
        public float PreUtterance
        {
            get
            {
                return getPreUtterance();
            }
            set
            {
                setPreUtterance( value );
            }
        }
#endif
        #endregion

        #region VoiceOverlap
        public float getVoiceOverlap()
        {
            return mVoiceOverlap;
        }

        public void setVoiceOverlap( float value )
        {
            mVoiceOverlap = value;
            mIsVoiceOverlapSpec = true;
        }

        public boolean isVoiceOverlapSpecified()
        {
            return mIsVoiceOverlapSpec;
        }

#if !JAVA
        public float VoiceOverlap
        {
            get
            {
                return getVoiceOverlap();
            }
            set
            {
                setVoiceOverlap( value );
            }
        }
#endif
        #endregion

        #region Envelope
        public UstEnvelope getEnvelope()
        {
            return mEnvelope;
        }

        public void setEnvelope( UstEnvelope value )
        {
            mEnvelope = value;
            mIsEnvelopeSpec = true;
        }

        public boolean isEnvelopeSpecified()
        {
            return mIsEnvelopeSpec;
        }

#if !JAVA
        public UstEnvelope Envelope
        {
            get
            {
                return getEnvelope();
            }
            set
            {
                setEnvelope( value );
            }
        }
#endif
        #endregion

        #region Moduration
        public int getModuration()
        {
            return mModuration;
        }

        public void setModuration( int value )
        {
            mModuration = value;
            mIsModurationSpec = true;
        }

        public boolean isModurationSpecified()
        {
            return mIsModurationSpec;
        }

#if !JAVA
        public int Moduration
        {
            get
            {
                return getModuration();
            }
            set
            {
                setModuration( value );
            }
        }
#endif
        #endregion

        #region StartPoint
        /// <summary>
        /// StartPointの値を取得します
        /// </summary>
        /// <returns></returns>
        public float getStartPoint()
        {
            return mStartPoint;
        }

        /// <summary>
        /// StartPoinの値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setStartPoint( float value )
        {
            mStartPoint = value;
            mIsStartPointSpec = true;
        }

        /// <summary>
        /// StartPointプロパティが設定されているかどうかを表す値を取得します．
        /// この値がfalseの場合，getStartPointで得られる値は不定です
        /// </summary>
        /// <returns></returns>
        public boolean isStartPointSpecified()
        {
            return mIsStartPointSpec;
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
        #endregion

        #region Length
        /// <summary>
        /// Lengthプロパティが設定されているかどうかを表す値を取得します．
        /// この値がfalseの場合，getLengthで得られる値は不定です
        /// </summary>
        /// <returns></returns>
        public boolean isLengthSpecified()
        {
            return mIsLengthSpec;
        }

        /// <summary>
        /// このイベントの長さを取得します
        /// </summary>
        /// <returns></returns>
        public int getLength()
        {
            return mLength;
        }

        /// <summary>
        /// このイベントの長さを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setLength( int value )
        {
            mLength = value;
            mIsLengthSpec = true;
        }

#if !JAVA
        /// <summary>
        /// XML用
        /// </summary>
        public int Length
        {
            get
            {
                return getLength();
            }
            set
            {
                setLength( value );
            }
        }
#endif
        #endregion

        public Object clone()
        {
            UstEvent ret = new UstEvent();
            ret.mLength = mLength;
            ret.mIsLengthSpec = mIsLengthSpec;
            ret.mLyric = mLyric;
            ret.mIsLyricSpec = mIsLyricSpec;
            ret.mNote = mNote;
            ret.mIsNoteSpec = mIsNoteSpec;
            ret.mIntensity = mIntensity;
            ret.mIsIntensitySpec = mIsIntensitySpec;
            ret.mPBType = mPBType;
            ret.mIsPBTypeSpec = mIsPBTypeSpec;
            if ( mPitches != null ) {
                ret.mPitches = new float[mPitches.Length];
                for ( int i = 0; i < mPitches.Length; i++ ) {
                    ret.mPitches[i] = mPitches[i];
                }
            }
            ret.mIsPitchesSpec = mIsPitchesSpec;
            ret.mTempo = mTempo;
            ret.mIsTempoSpec = mIsTempoSpec;
            if ( mVibrato != null ) {
                ret.mVibrato = (UstVibrato)mVibrato.clone();
            }
            ret.mIsVibratoSpec = mIsVibratoSpec;
            if ( mPortamento != null ) {
                ret.mPortamento = (UstPortamento)mPortamento.clone();
            }
            ret.mIsPortamentoSpec = mIsPortamentoSpec;
            if ( mEnvelope != null ) {
                ret.mEnvelope = (UstEnvelope)mEnvelope.clone();
            }
            ret.mIsEnvelopeSpec = mIsEnvelopeSpec;
            ret.mPreUtterance = mPreUtterance;
            ret.mIsPreUtteranceSpec = mIsPreUtteranceSpec;
            ret.mVoiceOverlap = mVoiceOverlap;
            ret.mIsVoiceOverlapSpec = mIsVoiceOverlapSpec;
            ret.Flags = Flags;
            ret.mModuration = mModuration;
            ret.mIsModurationSpec = mIsModurationSpec;
            ret.mStartPoint = mStartPoint;
            ret.mIsStartPointSpec = mIsStartPointSpec;
            ret.Tag = Tag;
            ret.Index = Index;
            return ret;
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif

        public void print( ITextWriter sw )
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
            if ( isLengthSpecified() ) {
                sw.write( "Length=" + mLength );
                sw.newLine();
            }
            if ( isLyricSpecified() ) {
                sw.write( "Lyric=" + getLyric() );
                sw.newLine();
            }
            if ( isNoteSpecified() ) {
                sw.write( "NoteNum=" + getNote() );
                sw.newLine();
            }
            if ( isIntensitySpecified() ) {
                sw.write( "Intensity=" + getIntensity() );
                sw.newLine();
            }
            if ( isPitchesSpecified() && mPitches != null ) {
                sw.write( "PBType=" + getPBType() );
                sw.newLine();
                sw.write( "Piches=" );
                for ( int i = 0; i < mPitches.Length; i++ ) {
                    if ( i == 0 ) {
                        sw.write( mPitches[i] + "" );
                    } else {
                        sw.write( "," + mPitches[i] );
                    }
                }
                sw.newLine();
            }
            if ( isTempoSpecified() ) {
                sw.write( "Tempo=" + getTempo() );
                sw.newLine();
            }
            if ( isVibratoSpecified() && mVibrato != null ) {
                sw.write( mVibrato.ToString() );
                sw.newLine();
            }
            if ( isPortamentoSpecified() && mPortamento != null ) {
                mPortamento.print( sw );
            }
            if ( isPreUtteranceSpecified() ) {
                sw.write( "PreUtterance=" + getPreUtterance() );
                sw.newLine();
            }
            if ( isVoiceOverlapSpecified() ) {
                sw.write( "VoiceOverlap=" + getVoiceOverlap() );
                sw.newLine();
            }
            if ( isEnvelopeSpecified() && mEnvelope != null ) {
                sw.write( mEnvelope.ToString() );
                sw.newLine();
            }
            if ( !str.compare( Flags, "" ) ) {
                sw.write( "Flags=" + Flags );
                sw.newLine();
            }
            if ( isModurationSpecified() ) {
                sw.write( "Moduration=" + getModuration() );
                sw.newLine();
            }
            if ( isStartPointSpecified() ) {
                sw.write( "StartPoint=" + getStartPoint() );
                sw.newLine();
            }
            if ( Properties != null ) {
                int size = vec.size( Properties );
                for ( int i = 0; i < size; i++ ) {
                    UstEventProperty itemi = vec.get( Properties, i );
                    sw.write( itemi.Name + "=" + itemi.Value );
                    sw.newLine();
                }
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
            if ( this.getModuration() != item.getModuration() ) ret = false;
            else if ( this.getPreUtterance() != item.getPreUtterance() ) ret = false;
            else if ( this.getStartPoint() != item.getStartPoint() ) ret = false;
            else if ( this.getVoiceOverlap() != item.getVoiceOverlap() ) ret = false;
            return ret;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getXmlElementName( String name )
        {
            return name;
        }
    }

#if !JAVA
}
#endif
