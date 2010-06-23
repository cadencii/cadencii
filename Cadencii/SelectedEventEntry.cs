/*
 * SelectedEventEntry.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import org.kbinani.vsq.*;
#else
using System;
using System.ComponentModel;
using org.kbinani.java.util;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
    using Integer = System.Int32;
#endif

    /// <summary>
    /// 選択されたアイテムを管理します。
    /// また、プロパティグリッドの登録アイテムとして編集されているオブジェクトと、
    /// VsqFileExに登録されているオブジェクトとの間を取り持つ処理を担います。
    /// </summary>
#if ENABLE_PROPERTY
    [TypeConverter( typeof( SelectedEventEntryTypeConverter ) )]
#endif
    public class SelectedEventEntry {
        /// <summary>
        /// 選択されたアイテムが存在しているトラック番号。
        /// </summary>
        public int track;
        /// <summary>
        /// 選択されたアイテム。
        /// </summary>
        public VsqEvent original;
        /// <summary>
        /// 選択されたアイテムの、編集後の値。
        /// </summary>
        public VsqEvent editing;
#if ENABLE_PROPERTY
        private static int lastVibratoLength = 66;
        private String m_clock;
        private BooleanEnum m_symbol_protected;
        private String m_length;
        private NoteNumberProperty m_note;
        private BooleanEnum m_portamento_up;
        private BooleanEnum m_portamento_down;
        private AttackVariation m_attack;
        private VibratoVariation m_vibrato;
        private String m_measure;
        private String m_beat;
        private String m_tick;
#if DEBUG
        private DEBUG_GatetimeProperty m_debug_clock = new DEBUG_GatetimeProperty();
#endif
#endif

        /// <summary>
        /// 指定されたパラメータを用いて、選択アイテムを表す新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="track_"></param>
        /// <param name="original_"></param>
        /// <param name="editing_"></param>
        public SelectedEventEntry( int track_, VsqEvent original_, VsqEvent editing_ ) {
            track = track_;
            original = original_;
            editing = editing_;

#if ENABLE_PROPERTY
            // clock
            m_clock = editing.Clock + "";

            // measure, beat, gate
            int measure, beat, gate;
            Timesig timesig = getPosition( out measure, out beat, out gate );
            m_measure = measure + "";
            m_beat = beat + "";
            m_tick = gate + "";

            // symbol_protected
            m_symbol_protected = BooleanEnum.Off;
            if ( editing.ID.LyricHandle != null && editing.ID.LyricHandle.L0 != null ) {
                m_symbol_protected = editing.ID.LyricHandle.L0.PhoneticSymbolProtected ? BooleanEnum.On : BooleanEnum.Off;
            }

            // length
            m_length = editing.ID.getLength() + "";

            // note
            m_note = new NoteNumberProperty();
            m_note.noteNumber = editing.ID.Note;

            // portamento
            m_portamento_up = BooleanEnum.Off;
            m_portamento_down = BooleanEnum.Off;
            if ( editing.ID.PMbPortamentoUse >= 2 ) {
                m_portamento_down = BooleanEnum.On;
            }
            if ( editing.ID.PMbPortamentoUse == 1 || editing.ID.PMbPortamentoUse == 3 ) {
                m_portamento_up = BooleanEnum.On;
            }

            // attack, vibrato
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq != null ) {
                SynthesizerType type = SynthesizerType.VOCALOID2;
                RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( track ) );
                if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
                    type = SynthesizerType.VOCALOID1;
                }

                if ( type == SynthesizerType.VOCALOID1 ) {
                    if ( editing.ID.NoteHeadHandle != null ) {
                        m_attack = new AttackVariation( editing.ID.NoteHeadHandle.getDisplayString() );
                    }
                }
                if ( editing.ID.VibratoHandle != null ) {
                    m_vibrato = new VibratoVariation( editing.ID.VibratoHandle.getDisplayString() );
                }
            }
            if ( m_attack == null ) {
                m_attack = new AttackVariation();
            }
            if ( m_vibrato == null ) {
                m_vibrato = new VibratoVariation( VibratoVariation.empty.description );
            }
#endif
        }

#if ENABLE_PROPERTY
        /// <summary>
        /// 小節数、拍数、ゲート数から、クロック値を計算します
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="beat"></param>
        /// <param name="gate"></param>
        /// <returns></returns>
        private int calculateClock( int measure, int beat, int gate ) {
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq == null ) {
                int premeasure = 2;
                return ((measure + premeasure - 1) * 4 + (beat - 1)) * 480 + gate;
            } else {
                int premeasure = vsq.getPreMeasure();
                int bartopclock = vsq.getClockFromBarCount( measure + premeasure - 1 );
                Timesig timesig = vsq.getTimesigAt( bartopclock );
                return bartopclock + (beat - 1) * 480 * 4 / timesig.denominator + gate;
            }
        }

        /// <summary>
        /// 現在のクロック値(m_clock)から、小節数、拍数、ゲート数(?)を計算します
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="beat"></param>
        /// <param name="gate"></param>
        /// <returns></returns>
        private Timesig getPosition( out int measure, out int beat, out int gate ) {
            VsqFileEx vsq = AppManager.getVsqFile();
            int clock = editing.Clock;
            if ( vsq == null ) {
                // 4/4拍子, プリメジャー2と仮定
                int i = clock / (480 * 4);
                int tpremeasure = 2;
                measure = i - tpremeasure + 1;
                int tdif = clock - i * 480 * 4;
                beat = tdif / 480 + 1;
                gate = tdif - (beat - 1) * 480;
                return new Timesig( 4, 4 );
            }

            int premeasure = vsq.getPreMeasure();
            measure = vsq.getBarCountFromClock( clock ) - premeasure + 1;
            int clock_bartop = vsq.getClockFromBarCount( measure + premeasure - 1 );
            Timesig timesig = vsq.getTimesigAt( clock );
            int den = timesig.denominator;
            int dif = clock - clock_bartop;
            int step = 480 * 4 / den;
            beat = dif / step + 1;
            gate = dif - (beat - 1) * step;
            return timesig;
        }

        /// <summary>
        /// プロパティに入力された文字列と、編集前の値を元に、入力された文字列を解釈することによって編集後の値がどうなるかを調べます
        /// </summary>
        /// <param name="old_value"></param>
        /// <param name="received_string"></param>
        /// <returns></returns>
        public static int evalReceivedString( int old_value, String received_string ) {
            int draft = old_value;
            if ( received_string.StartsWith( "+" ) || received_string.StartsWith( "-" ) || received_string.StartsWith( "*" ) || received_string.StartsWith( "/" ) ) {
                try {
                    string eq = "x" + received_string;

                    // 「+ 480)*1.1」みたいな書式を許容したいので。「+ 480)*1.1」=>「(x+ 480)*1.1」
                    int num_bla = 0; // "("の個数
                    int num_cket = 0; // ")"の個数
                    for ( int i = 0; i < eq.Length; i++ ) {
                        char c = eq[i];
                        if ( c == '(' ) {
                            num_bla++;
                        } else if ( c == ')' ) {
                            num_cket++;
                        }
                    }
                    int diff = num_cket - num_bla;
                    for ( int i = 0; i < diff; i++ ) {
                        eq = "(" + eq;
                    }
                    draft = (int)Utility.eval( draft, eq );
                } catch {
                    draft = old_value;
                }
            } else {
                try {
                    draft = (int)Utility.eval( old_value, received_string );
                } catch {
                    draft = old_value;
                }
            }
            return draft;
        }

        #region Lyric
        [Category( "Lyric" )]
        public String Phrase {
            get {
                if ( editing.ID.LyricHandle != null && editing.ID.LyricHandle.L0 != null ){
                    return editing.ID.LyricHandle.L0.Phrase;
                }
                return "";
            }
            set {
                if ( editing.ID.LyricHandle == null ){
                    return;
                }
                if ( editing.ID.LyricHandle.L0 == null ) {
                    return;
                }
                String old = editing.ID.LyricHandle.L0.Phrase;
                if ( !old.Equals( value ) ) {
                    // 歌詞
                    String phrase = value;
                    if ( AppManager.editorConfig.SelfDeRomanization ) {
                        phrase = KanaDeRomanization.Attach( value );
                    }
                    editing.ID.LyricHandle.L0.Phrase = phrase;

                    // 発音記号
                    String phonetic_symbol = "";
                    SymbolTableEntry entry = SymbolTable.attatch( phrase );
                    if ( entry == null ) {
                        phonetic_symbol = "a";
                    } else {
                        phonetic_symbol = entry.getSymbol();
                    }
                    editing.ID.LyricHandle.L0.setPhoneticSymbol( phonetic_symbol );

                    // consonant adjustment
                    String[] spl = PortUtil.splitString( phonetic_symbol, new char[] { ' ', ',' }, true );
                    String consonant_adjustment = "";
                    for ( int i = 0; i < spl.Length; i++ ) {
                        consonant_adjustment += (i == 0 ? "" : " ") + (VsqPhoneticSymbol.isConsonant( spl[i] ) ? 64 : 0);
                    }
                    editing.ID.LyricHandle.L0.setConsonantAdjustment( consonant_adjustment );

                    // overlap, preUtterancec
                    VsqFileEx vsq = AppManager.getVsqFile();
                    if ( vsq != null ) {
                        int selected = AppManager.getSelected();
                        VsqTrack vsq_track = vsq.Track.get( selected );
                        VsqEvent singer = vsq_track.getSingerEventAt( editing.Clock );
                        SingerConfig sc = AppManager.getSingerInfoUtau( singer.ID.IconHandle.Language, singer.ID.IconHandle.Program );
                        if ( sc != null && AppManager.utauVoiceDB.containsKey( sc.VOICEIDSTR ) ) {
                            UtauVoiceDB db = AppManager.utauVoiceDB.get( sc.VOICEIDSTR );
                            OtoArgs oa = db.attachFileNameFromLyric( phrase );
                            if ( editing.UstEvent == null ) {
                                editing.UstEvent = new UstEvent();
                            }
                            editing.UstEvent.VoiceOverlap = oa.msOverlap;
                            editing.UstEvent.PreUtterance = oa.msPreUtterance;
                        }
                    }
                }
            }
        }

        [Category( "Lyric" )]
        public String PhoneticSymbol {
            get {
                if ( editing.ID.LyricHandle != null && editing.ID.LyricHandle.L0 != null ) {
                    return editing.ID.LyricHandle.L0.getPhoneticSymbol();
                }
                return "";
            }
            set {
                if ( editing.ID.LyricHandle == null ) {
                    return;
                }
                if ( editing.ID.LyricHandle.L0 == null ) {
                    return;
                }
                editing.ID.LyricHandle.L0.setPhoneticSymbol( value );
            }
        }

        [Category( "Lyric" )]
        public String CosonantAdjustment {
            get {
                if ( editing.ID.LyricHandle != null && editing.ID.LyricHandle.L0 != null ) {
                    return editing.ID.LyricHandle.L0.getConsonantAdjustment();
                }
                return "";
            }
            set {
                if ( editing.ID.LyricHandle == null ) {
                    return;
                }
                if ( editing.ID.LyricHandle.L0 == null ) {
                    return;
                }
                String[] symbol = PortUtil.splitString( editing.ID.LyricHandle.L0.getPhoneticSymbol(), new char[] { ' ' }, true );
                String[] adjustment = PortUtil.splitString( value, new char[] { ' ', ',' }, true );
                if ( adjustment.Length < symbol.Length ) {
                    Array.Resize( ref adjustment, symbol.Length );
                }
                int[] iadj = new int[symbol.Length];
                for ( int i = 0; i < iadj.Length; i++ ) {
                    if ( VsqPhoneticSymbol.isConsonant( symbol[i] ) ) {
                        int v = 64;
                        try {
                            v = PortUtil.parseInt( adjustment[i] );
                        } catch ( Exception ex ) {
                        }
                        if ( v < 0 ) {
                            v = 0;
                        } else if ( 127 < v ) {
                            v = 127;
                        }
                        iadj[i] = v;
                    } else {
                        iadj[i] = 0;
                    }
                }
                String consonant_adjustment = "";
                for ( int i = 0; i < iadj.Length; i++ ) {
                    consonant_adjustment += (i == 0 ? "" : " ") + iadj[i];
                }
                editing.ID.LyricHandle.L0.setConsonantAdjustment( consonant_adjustment );
            }
        }

        [Category( "Lyric" )]
        public BooleanEnum Protect {
            get {
                return m_symbol_protected;
            }
            set {
                m_symbol_protected = value;
                if ( editing.ID.LyricHandle == null ) {
                    return;
                }
                if ( editing.ID.LyricHandle.L0 == null ) {
                    return;
                }
                editing.ID.LyricHandle.L0.PhoneticSymbolProtected = (value == BooleanEnum.On) ? true : false;
            }
        }
        #endregion

        #region Note Location
        [Category( "Note Location" )]
        public String Clock {
            get {
                return m_clock;
            }
            set {
                int oldvalue = editing.Clock;
                int draft = evalReceivedString( oldvalue, value );
                editing.Clock = draft;
                m_clock = draft + "";
            }
        }

        [Category( "Note Location" )]
        public String Measure {
            get {
                return m_measure;
            }
            set {
                int measure, beat, tick;
                Timesig timesig = getPosition( out measure, out beat, out tick );
                int draft = evalReceivedString( measure, value );
                int clock = calculateClock( draft, beat, tick );
                editing.Clock = clock;
                m_clock = clock + "";
            }
        }

        [Category( "Note Location" )]
        public String Beat {
            get {
                return m_beat;
            }
            set {
                int measure, beat, tick;
                Timesig timesig = getPosition( out measure, out beat, out tick );
                int draft = evalReceivedString( beat, value );
                int clock = calculateClock( measure, draft, tick );
                editing.Clock = clock;
                m_clock = clock + "";
            }
        }

        [Category( "Note Location" )]
        public String Tick {
            get {
                return m_tick;
            }
            set {
                int measure, beat, tick;
                Timesig timesig = getPosition( out measure, out beat, out tick );
                int draft = evalReceivedString( tick, value );
                int clock = calculateClock( measure, beat, draft );
                editing.Clock = clock;
                m_clock = clock + "";
            }
        }
        #endregion

        #region Note
        [Category( "Note" )]
        public String Length {
            get {
                return m_length;
            }
            set {
                int oldvalue = editing.ID.getLength();
                int draft = evalReceivedString( oldvalue, value );
                if ( draft < 0 ) {
                    draft = 0;
                } else {
                    VsqFileEx vsq = AppManager.getVsqFile();
                    if ( vsq != null ) {
                        int maxlength = vsq.getMaximumNoteLengthAt( editing.Clock );
                        if ( maxlength < draft ) {
                            draft = maxlength;
                        }
                    }
                }

                // ビブラートの長さを調節
                Utility.editLengthOfVsqEvent( editing, draft, AppManager.vibratoLengthEditingRule );
            }
        }

        [Category( "Note" )]
        public NoteNumberProperty Note {
            get {
                return m_note;
            }
            set {
                if ( value.noteNumber < 0 ) {
                    m_note.noteNumber = 0;
                } else if ( 127 < value.noteNumber ) {
                    m_note.noteNumber = 127;
                } else {
                    m_note = value;
                }
                editing.ID.Note = m_note.noteNumber;
            }
        }
        #endregion

        #region UTAU
        [Category( "UTAU" )]
        public float PreUtterance {
            get {
                if ( editing.UstEvent == null ) {
                    return 0;
                }
                return editing.UstEvent.PreUtterance;
            }
            set {
                if ( editing.UstEvent == null ) {
                    editing.UstEvent = new UstEvent();
                }
                editing.UstEvent.PreUtterance = value;
            }
        }

        [Category( "UTAU" )]
        public float Overlap {
            get {
                if ( editing.UstEvent == null ) {
                    return 0;
                }
                return editing.UstEvent.VoiceOverlap;
            }
            set {
                if ( editing.UstEvent == null ) {
                    editing.UstEvent = new UstEvent();
                }
                editing.UstEvent.VoiceOverlap = value;
            }
        }

        [Category( "UTAU" )]
        public int Moduration {
            get {
                if ( editing.UstEvent == null ) {
                    editing.UstEvent = new UstEvent();
                }
                return editing.UstEvent.Moduration;
            }
            set {
                if ( editing.UstEvent == null ) {
                    editing.UstEvent = new UstEvent();
                }
                editing.UstEvent.Moduration = value;
            }
        }

        [Category( "UTAU" )]
        public String Flags {
            get {
                if ( editing.UstEvent == null ) {
                    return "";
                }
                return editing.UstEvent.Flags;
            }
            set {
                if ( editing.UstEvent == null ) {
                    editing.UstEvent = new UstEvent();
                }
                editing.UstEvent.Flags = value;
            }
        }
        #endregion

        #region VOCALOID2
        [Category( "VOCALOID2" )]
        public int Accent {
            get {
                return editing.ID.DEMaccent;
            }
            set {
                int draft = value;
                if ( value < 0 ) {
                    draft = 0;
                } else if ( 100 < value ) {
                    draft = 100;
                }
                editing.ID.DEMaccent = draft;
            }
        }

        [Category( "VOCALOID2" )]
        public int Decay {
            get {
                return editing.ID.DEMdecGainRate;
            }
            set {
                int draft = value;
                if ( value < 0 ) {
                    draft = 0;
                } else if ( 100 < value ) {
                    draft = 100;
                }
                editing.ID.DEMdecGainRate = draft;
            }
        }

        [Category( "VOCALOID2" )]
        public BooleanEnum UpPortamento {
            get {
                return m_portamento_up;
            }
            set {
                m_portamento_up = value;
                editing.ID.PMbPortamentoUse = (m_portamento_up == BooleanEnum.On ? 1 : 0) + (m_portamento_down == BooleanEnum.On ? 2 : 0);
            }
        }

        [Category( "VOCALOID2" )]
        public BooleanEnum DownPortamento {
            get {
                return m_portamento_down;
            }
            set {
                m_portamento_down = value;
                editing.ID.PMbPortamentoUse = (m_portamento_up == BooleanEnum.On ? 1 : 0) + (m_portamento_down == BooleanEnum.On ? 2 : 0);
            }
        }

        [Category( "VOCALOID2" )]
        public int BendDepth {
            get {
                return editing.ID.PMBendDepth;
            }
            set {
                int draft = value;
                if ( value < 0 ) {
                    draft = 0;
                } else if ( 100 < value ) {
                    draft = 100;
                }
                editing.ID.PMBendDepth = draft;
            }
        }

        [Category( "VOCALOID2" )]
        public int BendLength {
            get {
                return editing.ID.PMBendLength;
            }
            set {
                int draft = value;
                if ( value < 0 ) {
                    draft = 0;
                } else if ( 100 < value ) {
                    draft = 100;
                }
                editing.ID.PMBendLength = draft;
            }
        }

        [Category( "VOCALOID2" )]
        public int Velocity {
            get {
                return editing.ID.Dynamics;
            }
            set {
                int draft = value;
                if ( value < 0 ) {
                    draft = 0;
                } else if ( 127 < value ) {
                    draft = 127;
                }
                editing.ID.Dynamics = draft;
            }
        }

        [Category( "VOCALOID2" )]
        public int pMeanOnsetFirstNote {
            get {
                return editing.ID.pMeanOnsetFirstNote;
            }
            set {
                int draft = value;
                if ( value < 0 ) {
                    draft = 0;
                } else if ( 0x32 < value ) {
                    draft = 0x32;
                }
                editing.ID.pMeanOnsetFirstNote = draft;
            }
        }

        [Category( "VOCALOID2" )]
        public int vMeanNoteTransition {
            get {
                return editing.ID.vMeanNoteTransition;
            }
            set {
                int draft = value;
                if ( value < 0x05 ) {
                    draft = 0x05;
                } else if ( 0x1e < value ) {
                    draft = 0x1e;
                }
                editing.ID.vMeanNoteTransition = draft;
            }
        }

        [Category( "VOCALOID2" )]
        public int d4mean {
            get {
                return editing.ID.d4mean;
            }
            set {
                int draft = value;
                if ( value < 0x0a ) {
                    draft = 0x0a;
                } else if ( 0x3c < value ) {
                    draft = 0x3c;
                }
                editing.ID.d4mean = draft;
            }
        }

        [Category( "VOCALOID2" )]
        public int pMeanEndingNote {
            get {
                return editing.ID.pMeanEndingNote;
            }
            set {
                int draft = value;
                if ( value < 0x05 ) {
                    draft = 0x05;
                } else if ( 0x1e < value ) {
                    draft = 0x1e;
                }
                editing.ID.pMeanEndingNote = draft;
            }
        }
        #endregion

        #region VOCALOID1
        [TypeConverter( typeof( AttackVariationConverter ) ), Category( "VOCALOID1" )]
        public AttackVariation Attack {
            get {
                return m_attack;
            }
            set {
                m_attack = value;
                VsqFileEx vsq = AppManager.getVsqFile();
                if ( vsq != null ) {
                    SynthesizerType type = SynthesizerType.VOCALOID2;
                    RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                    if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
                        type = SynthesizerType.VOCALOID1;
                    }

                    if ( m_attack.description.Equals( new AttackVariation().description ) ) {
                        editing.ID.NoteHeadHandle = null;
                    } else {
                        String description = m_attack.description;
                        int last_depth = 0;
                        int last_duration = 0;
                        if ( editing.ID.NoteHeadHandle != null ) {
                            last_depth = editing.ID.NoteHeadHandle.getDepth();
                            last_duration = editing.ID.NoteHeadHandle.getDuration();
                        }
                        for ( Iterator<NoteHeadHandle> itr = VocaloSysUtil.attackConfigIterator( type ); itr.hasNext(); ) {
                            NoteHeadHandle aconfig = itr.next();
                            if ( description.Equals( aconfig.getDisplayString() ) ) {
                                editing.ID.NoteHeadHandle = (NoteHeadHandle)aconfig.clone();
                                editing.ID.NoteHeadHandle.setDepth( last_depth );
                                editing.ID.NoteHeadHandle.setDuration( last_duration );
                                break;
                            }
                        }
                    }
                }
            }
        }

        [Category( "VOCALOID1" )]
        public int AttackDepth {
            get {
                if ( editing.ID.NoteHeadHandle == null ) {
                    return 0;
                }
                return editing.ID.NoteHeadHandle.getDepth();
            }
            set {
                if ( editing.ID.NoteHeadHandle == null ) {
                    return;
                }
                int draft = value;
                if ( draft < 0 ) {
                    draft = 0;
                } else if ( 127 < draft ) {
                    draft = 127;
                }
                editing.ID.NoteHeadHandle.setDepth( draft );
            }
        }

        [Category( "VOCALOID1" )]
        public int AttackDuration {
            get {
                if ( editing.ID.NoteHeadHandle == null ) {
                    return 0;
                }
                return editing.ID.NoteHeadHandle.getDuration();
            }
            set {
                if ( editing.ID.NoteHeadHandle == null ) {
                    return;
                }
                int draft = value;
                if ( draft < 0 ) {
                    draft = 0;
                } else if ( 127 < draft ) {
                    draft = 127;
                }
                editing.ID.NoteHeadHandle.setDuration( draft );
            }
        }
        #endregion

        #region Vibrato
        [Category( "Vibrato" )]
        public VibratoVariation Vibrato {
            get {
                return m_vibrato;
            }
            set {
                if ( value.description.Equals( VibratoVariation.empty.description ) ) {
                    editing.ID.VibratoHandle = null;
                } else {
                    int last_length = 0;
                    if ( editing.ID.VibratoHandle != null ) {
                        last_length = editing.ID.VibratoHandle.getLength();
                    }

                    VsqFileEx vsq = AppManager.getVsqFile();
                    if ( vsq != null ) {
                        SynthesizerType type = SynthesizerType.VOCALOID2;
                        RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                        if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
                            type = SynthesizerType.VOCALOID1;
                        }
                        String description = value.description;
                        for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                            VibratoHandle vconfig = itr.next();
                            if ( description.Equals( vconfig.getDisplayString() ) ) {
                                editing.ID.VibratoHandle = (VibratoHandle)vconfig.clone();
                                break;
                            }
                        }
                    }
                    if ( editing.ID.VibratoHandle != null ) {
                        if ( last_length <= 0 ) {
                            last_length = lastVibratoLength;
                            if ( last_length <= 0 ) {
                                last_length = 66;
                            }
                        }
                        editing.ID.VibratoHandle.setLength( last_length );
                    }
                }
                m_vibrato = value;
            }
        }

        [Category( "Vibrato" )]
        public int VibratoLength {
            get {
                if ( editing.ID.VibratoHandle == null ) {
                    return 0;
                }
                return editing.ID.VibratoHandle.getLength() * 100 / editing.ID.getLength();
            }
            set {
#if DEBUG
                PortUtil.println( "VsqEventItemProxy#set_VibratoLength; value=" + value );
#endif
                if ( value <= 0 ) {
                    m_vibrato = new VibratoVariation( VibratoVariation.empty.description );
                    editing.ID.VibratoHandle = null;
                    editing.ID.VibratoDelay = editing.ID.getLength();
                } else {
                    int draft = value;
                    if ( 100 < draft ) {
                        draft = 100;
                    }
                    if ( editing.ID.VibratoHandle == null ) {
                        VsqFileEx vsq = AppManager.getVsqFile();
                        if ( vsq != null ) {
                            String iconid = AppManager.editorConfig.AutoVibratoType2;
                            SynthesizerType type = SynthesizerType.VOCALOID2;
                            RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                            if ( kind == RendererKind.VOCALOID1_100 ||
                                kind == RendererKind.VOCALOID1_101 ) {
                                type = SynthesizerType.VOCALOID1;
                            }
                            editing.ID.VibratoHandle = AppManager.editorConfig.createAutoVibrato( type, 480 ); // 480はダミー
                        }
                        if ( editing.ID.VibratoHandle == null ) {
                            editing.ID.VibratoHandle = new VibratoHandle();
                        }
                    }
                    editing.ID.VibratoHandle.setLength( editing.ID.getLength() * draft / 100 );
                    editing.ID.VibratoDelay = editing.ID.getLength() - editing.ID.VibratoHandle.getLength();
                    lastVibratoLength = editing.ID.VibratoHandle.getLength() * 100 / editing.ID.getLength();
                }
            }
        }
        #endregion

        #region AquesTone
        [Category( "AquesTone" )]
        public int Release {
            get {
                VsqEvent e = new VsqEvent();
                e.Tag = editing.Tag;
                String v = VsqFileEx.getEventTag( e, VsqFileEx.TAG_VSQEVENT_AQUESTONE_RELEASE );
                int value = 64;
                if ( !v.Equals( "" ) ) {
                    try {
                        value = PortUtil.parseInt( v );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "VsqEventItemProxy#get_Release; ex=" + ex );
                        value = 64;
                    }
                }
                if ( 0 > value ) {
                    value = 0;
                } else if ( 127 < value ) {
                    value = 127;
                }
                VsqFileEx.setEventTag( e, VsqFileEx.TAG_VSQEVENT_AQUESTONE_RELEASE, value + "" );
                editing.Tag = e.Tag;
                return value;
            }
            set {
                if ( 0 > value ) {
                    value = 0;
                } else if ( 127 < value ) {
                    value = 127;
                }
                VsqEvent e = new VsqEvent();
                e.Tag = editing.Tag;
                VsqFileEx.setEventTag( e, VsqFileEx.TAG_VSQEVENT_AQUESTONE_RELEASE, value + "" );
                editing.Tag = e.Tag;
            }
        }
        #endregion

#endif
    }

#if !JAVA
#if DEBUG
    public class DEBUG_GatetimePropertyConverter : ExpandableObjectConverter {
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {
            if ( sourceType == typeof( string ) ) {
                return true;
            }
            return base.CanConvertFrom( context, sourceType );
        }

        // string -> DEBUG_GatetimeProperty
        public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value ) {
            if ( value is string ) {
                string s = (string)value;
                string[] spl = s.Split( ',' );
                if ( spl.Length >= 3 ) {
                    try {
                        int measure = int.Parse( spl[0].Trim() );
                        int beat = int.Parse( spl[1].Trim() );
                        int gate = int.Parse( spl[2].Trim() );
                        DEBUG_GatetimeProperty ret = new DEBUG_GatetimeProperty();
                        ret.Measure = measure + "";
                        ret.Beat = beat + "";
                        ret.Gate = gate + "";
                        return ret;
                    } catch {
                    }
                }
            }
            return base.ConvertFrom( context, culture, value );
        }

        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( DEBUG_GatetimeProperty ) ) {
                return true;
            }
            return base.CanConvertTo( context, destinationType );
        }

        // DEBUG_GatetimeProperty -> string
        public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType ) {
            if ( value is DEBUG_GatetimeProperty && destinationType == typeof( string ) ) {
                DEBUG_GatetimeProperty gp = (DEBUG_GatetimeProperty)value;
                return gp.Measure + ", " + gp.Beat + ", " + gp.Gate;
            }
            return base.ConvertTo( context, culture, value, destinationType );
        }
    }

    [TypeConverter( typeof( DEBUG_GatetimePropertyConverter) )]
    public class DEBUG_GatetimeProperty {
        string m = "1";
        string b = "2";
        string g = "3";

        [NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
        public string Measure {
            get {
                return m;
            }
            set {
                m = value;
            }
        }

        [NotifyParentProperty( true ), RefreshProperties( RefreshProperties.All )]
        public string Beat {
            get {
                return b;
            }
            set {
                b = value;
            }
        }

        [NotifyParentProperty( true ), RefreshProperties( RefreshProperties.All )]
        public string Gate {
            get {
                return g;
            }
            set {
                g = value;
            }
        }
    }
#endif
#endif

#if !JAVA
}
#endif
