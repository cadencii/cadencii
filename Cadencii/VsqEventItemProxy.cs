/*
 * VsqEventItemProxy.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using System.Reflection;
using Boare.Lib.Vsq;
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.util;

namespace Boare.Cadencii {

    [TypeConverter( typeof( VsqEventItemProxyTypeConverter ) )]
    class VsqEventItemProxy {
        private ClockProperty m_clock;
        private CalculatableString m_length;
        private NoteNumberProperty m_note;
        private int m_velocity;
        private int m_bend_depth;
        private int m_bend_length;
        private int m_decay;
        private int m_accent;
        private BooleanEnum m_portamento_up;
        private BooleanEnum m_portamento_down;
        private int m_vibrato_percent;
        private String m_phrase;
        private String m_phonetic_symbol;
        private int m_pMeanOnsetFirstNote = 0x0a;
        private int m_vMeanNoteTransition = 0x0c;
        private int m_d4mean = 0x18;
        private int m_pMeanEndingNote = 0x0c;
        private UstEvent m_ust_event;
        private AttackVariation m_attack;
        private VibratoVariation m_vibrato;
        private int m_attack_depth = 64;
        private int m_attack_duration = 64;

        private static int s_vibrato_percent_last = -1;

        public VsqEvent original;

        public VsqEventItemProxy( VsqEvent item ) {
            original = (VsqEvent)item.clone();
            m_clock = new ClockProperty();
            m_clock.setClock( new CalculatableString( item.Clock ) );
            m_length = new CalculatableString( item.ID.Length );
            m_note = new NoteNumberProperty();
            m_note.noteNumber = item.ID.Note;
            m_velocity = item.ID.Dynamics;
            m_bend_depth = item.ID.PMBendDepth;
            m_bend_length = item.ID.PMBendLength;
            m_decay = item.ID.DEMdecGainRate;
            m_accent = item.ID.DEMaccent;
            if ( item.ID.VibratoHandle == null || item.ID.VibratoDelay >= item.ID.Length ) {
                m_vibrato_percent = 0;
            } else {
                m_vibrato_percent = (int)(100 - item.ID.VibratoDelay / (double)item.ID.Length * 100.0);
            }
            if ( m_vibrato_percent > 0 ) {
                lastVibratoLength = m_vibrato_percent;
            }
            m_portamento_up = BooleanEnum.Off;
            m_portamento_down = BooleanEnum.Off;
            if ( item.ID.PMbPortamentoUse >= 2 ) {
                m_portamento_down = BooleanEnum.On;
            }
            if ( item.ID.PMbPortamentoUse == 1 || item.ID.PMbPortamentoUse == 3 ) {
                m_portamento_up = BooleanEnum.On;
            }
            if ( item.ID.LyricHandle != null ) {
                m_phrase = item.ID.LyricHandle.L0.Phrase;
                m_phonetic_symbol = item.ID.LyricHandle.L0.getPhoneticSymbol();
            }
            m_ust_event = item.UstEvent;
            m_pMeanOnsetFirstNote = item.ID.pMeanOnsetFirstNote;
            m_vMeanNoteTransition = item.ID.vMeanNoteTransition;
            m_d4mean = item.ID.d4mean;
            m_pMeanEndingNote = item.ID.pMeanEndingNote;

            if ( item.ID.NoteHeadHandle != null ) {
                m_attack_depth = item.ID.NoteHeadHandle.Depth;
                m_attack_duration = item.ID.NoteHeadHandle.Duration;
            }

            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq != null ) {
                SynthesizerType type = SynthesizerType.VOCALOID2;
                if ( vsq.Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                    type = SynthesizerType.VOCALOID1;
                }

                if ( type == SynthesizerType.VOCALOID1 ) {
                    if ( item.ID.NoteHeadHandle != null ) {
                        m_attack = new AttackVariation( item.ID.NoteHeadHandle.getDisplayString() );
                    }
                }
                if ( item.ID.VibratoHandle != null ) {
                    m_vibrato = new VibratoVariation( item.ID.VibratoHandle.getDisplayString() );
                }
            }
            if ( m_attack == null ) {
                m_attack = new AttackVariation();
            }
            if ( m_vibrato == null ) {
                m_vibrato = new VibratoVariation( VibratoVariation.empty.description );
            }
        }

        private static int lastVibratoLength {
            get {
#if DEBUG
                PortUtil.println( "VsqEventItemProxy#get_lastVibratoLength; m_vibrato_percent_last=" + s_vibrato_percent_last );
#endif
                return s_vibrato_percent_last;
            }
            set {
                s_vibrato_percent_last = value;
#if DEBUG
                PortUtil.println( "VsqEventItemProxy#set_lastVibratoLength; m_vibrato_percent_last=" + s_vibrato_percent_last );
#endif
            }
        }

        private static int getVibatoPercent( int vibrato_delay, int note_length ) {
            return (int)(100 - vibrato_delay / (double)note_length * 100.0);
        }

        public static int GetVibratoDelay( int percent, int note_length ) {
            if ( percent <= 0 ) {
                return note_length;
            } else {
                int i = (int)((double)note_length * (percent + 0.5) / 100.0);
                return note_length - i;
            }
        }

        public int GetPortamentoUsage() {
            return (m_portamento_up == BooleanEnum.On ? 1 : 0) + (m_portamento_down == BooleanEnum.On ? 2 : 0);
        }

        public VsqEvent GetItemDifference() {
            VsqEvent item = original;
            VsqEvent ret = new VsqEvent();
            ret.Clock = m_clock.getClock().getIntValue();
            ret.ID = new VsqID();
            ret.ID.DEMaccent = m_accent;
            ret.ID.DEMdecGainRate = m_decay;
            ret.ID.Dynamics = m_velocity;
            if ( item.ID.IconHandle != null ) {
                ret.ID.IconHandle = (IconHandle)item.ID.IconHandle.clone();
            }
            int note_length = m_length.getIntValue();
            ret.ID.Length = note_length;
            ret.ID.Note = m_note.noteNumber;

            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq != null ) {
                SynthesizerType type = SynthesizerType.VOCALOID2;
                if ( vsq.Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                    type = SynthesizerType.VOCALOID1;
                }

                if ( m_attack.description.Equals( new AttackVariation().description ) ) {
                    ret.ID.NoteHeadHandle = null;
                } else {
                    String description = m_attack.description;
                    for ( Iterator itr = VocaloSysUtil.attackConfigIterator( type ); itr.hasNext(); ) {
                        AttackConfig aconfig = (AttackConfig)itr.next();
                        if ( description.Equals( aconfig.contents.getDisplayString() ) ) {
                            ret.ID.NoteHeadHandle = (NoteHeadHandle)aconfig.contents.clone();
                            ret.ID.NoteHeadHandle.Depth = m_attack_depth;
                            ret.ID.NoteHeadHandle.Duration = m_attack_duration;
                            break;
                        }
                    }
                }

                if ( m_vibrato.description.Equals( VibratoVariation.empty.description ) ) {
                    ret.ID.VibratoHandle = null;
                } else {
                    String description = m_vibrato.description;
                    for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( type ); itr.hasNext(); ) {
                        VibratoConfig vconfig = (VibratoConfig)itr.next();
                        if ( description.Equals( vconfig.contents.getDisplayString() ) ) {
                            ret.ID.VibratoHandle = (VibratoHandle)vconfig.contents.clone();
                            break;
                        }
                    }
                }
            }
    
            ret.ID.PMBendDepth = m_bend_depth;
            ret.ID.PMBendLength = m_bend_length;
            ret.ID.PMbPortamentoUse = GetPortamentoUsage();
            ret.ID.type = VsqIDType.Anote;
            if ( m_vibrato_percent <= 0 ) {
                ret.ID.VibratoDelay = GetVibratoDelay( m_vibrato_percent, note_length );
            } else {
                ret.ID.VibratoDelay = GetVibratoDelay( m_vibrato_percent, note_length );
#if DEBUG
                PortUtil.println( "VsqEventItemProxy#GetItemDifference" );
                PortUtil.println( "    m_vibrato_percent=" + m_vibrato_percent );
                PortUtil.println( "    ret.ID.VibratoDelay=" + ret.ID.VibratoDelay );
                PortUtil.println( "    ret.ID.Length=" + ret.ID.Length );
                PortUtil.println( "    percent=" + getVibatoPercent( ret.ID.VibratoDelay, ret.ID.Length ) );
#endif
                if ( ret.ID.VibratoHandle != null ) {
                    ret.ID.VibratoHandle.Length = ret.ID.Length - ret.ID.VibratoDelay;
                }
            }
            ret.ID.LyricHandle = new LyricHandle( m_phrase, m_phonetic_symbol );
            ret.InternalID = original.InternalID;
            ret.UstEvent = (UstEvent)m_ust_event.clone();

            ret.ID.pMeanOnsetFirstNote = m_pMeanOnsetFirstNote;
            ret.ID.vMeanNoteTransition = m_vMeanNoteTransition;
            ret.ID.d4mean = m_d4mean;
            ret.ID.pMeanEndingNote = m_pMeanEndingNote;
            return ret;
        }

        #region Lyric
        [Category( "Lyric" )]
        public String Phrase {
            get {
                return m_phrase;
            }
            set {
                String old = m_phrase;
                m_phrase = value;
                if ( !old.Equals( m_phrase ) ) {
                    if ( AppManager.editorConfig.SelfDeRomanization ) {
                        m_phrase = KanaDeRomanization.Attach( m_phrase );
                        ByRef<String> phonetic_symbol = new ByRef<String>( "" );
                        SymbolTable.attatch( m_phrase, phonetic_symbol );
                        m_phonetic_symbol = phonetic_symbol.value;
                    }
                }
            }
        }

        [Category( "Lyric" )]
        public String PhoneticSymbol {
            get {
                return m_phonetic_symbol;
            }
            set {
                m_phonetic_symbol = value;
            }
        }
        #endregion

        #region Note
        [Category( "Note" )]
        public ClockProperty Clock {
            get {
                return m_clock;
            }
            set {
                m_clock = value;
            }
        }

        [Category( "Note" )]
        public CalculatableString Length {
            get {
                return m_length;
            }
            set {
                int draft = value.getIntValue();
                if ( draft <= 0 ) {
                    m_length = new CalculatableString( 0 );
                } else {
                    VsqFileEx vsq = AppManager.getVsqFile();
                    int clock = m_clock.getClock().getIntValue();
                    if ( vsq != null ) {
                        double ms_clock = vsq.getSecFromClock( clock ) * 1000.0;
                        double ms_end = vsq.getSecFromClock( clock + draft ) * 1000.0;
                        if ( (int)(ms_end - ms_clock) > VsqID.MAX_NOTE_LENGTH ) {
                            double ms_max = ms_clock + VsqID.MAX_NOTE_LENGTH;
                            int draft2 = (int)vsq.getClockFromSec( ms_max / 1000.0 ) - clock;
                            if ( draft2 < 0 ) {
                                draft2 = 0;
                            } else {
                                ms_end = vsq.getSecFromClock( clock + draft2 );
                                while ( (int)(ms_end - ms_clock) <= VsqID.MAX_NOTE_LENGTH ) {
                                    draft2++;
                                    ms_end = vsq.getSecFromClock( clock + draft2 ) * 1000.0;
                                }
                                draft2--;
                            }
                            m_length = new CalculatableString( draft2 );
                        } else {
                            m_length = value;
                        }
                    } else {
                        m_length = value;
                    }
                }
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
            }
        }
        #endregion

        #region UTAU
        [Category( "UTAU" )]
        public int PreUtterance {
            get {
                return m_ust_event.PreUtterance;
            }
            set {
                m_ust_event.PreUtterance = value;
            }
        }

        [Category( "UTAU" )]
        public int Overlap {
            get {
                return m_ust_event.VoiceOverlap;
            }
            set {
                m_ust_event.VoiceOverlap = value;
            }
        }

        [Category( "UTAU" )]
        public int Moduration {
            get {
                return m_ust_event.Moduration;
            }
            set {
                m_ust_event.Moduration = value;
            }
        }
        #endregion

        #region VOCALOID2
        [Category( "VOCALOID2" )]
        public int Accent {
            get {
                return m_accent;
            }
            set {
                if ( value < 0 ) {
                    m_accent = 0;
                } else if ( 100 < value ) {
                    m_accent = 100;
                } else {
                    m_accent = value;
                }
            }
        }

        [Category( "VOCALOID2" )]
        public int Decay {
            get {
                return m_decay;
            }
            set {
                if ( value < 0 ) {
                    m_decay = 0;
                } else if ( 100 < value ) {
                    m_decay = 100;
                } else {
                    m_decay = value;
                }
            }
        }

        [Category( "VOCALOID2" )]
        public BooleanEnum UpPortamento {
            get {
                return m_portamento_up;
            }
            set {
                m_portamento_up = value;
            }
        }

        [Category( "VOCALOID2" )]
        public BooleanEnum DownPortamento {
            get {
                return m_portamento_down;
            }
            set {
                m_portamento_down = value;
            }
        }

        [Category( "VOCALOID2" )]
        public int BendDepth {
            get {
                return m_bend_depth;
            }
            set {
                if ( value < 0 ) {
                    m_bend_depth = 0;
                } else if ( 100 < value ) {
                    m_bend_depth = 100;
                } else {
                    m_bend_depth = value;
                }
            }
        }

        [Category( "VOCALOID2" )]
        public int BendLength {
            get {
                return m_bend_length;
            }
            set {
                if ( value < 0 ) {
                    m_bend_length = 0;
                } else if ( 100 < value ) {
                    m_bend_length = 100;
                } else {
                    m_bend_length = value;
                }
            }
        }

        [Category( "VOCALOID2" )]
        public int Velocity {
            get {
                return m_velocity;
            }
            set {
                if ( value < 0 ) {
                    m_velocity = 0;
                } else if ( 127 < value ) {
                    m_velocity = 127;
                } else {
                    m_velocity = value;
                }
            }
        }

        [Category( "VOCALOID2" )]
        public int pMeanOnsetFirstNote{
            get{
                return m_pMeanOnsetFirstNote;
            }
            set{
                if( value < 0 ){
                    m_pMeanOnsetFirstNote = 0;
                }else if( 0x32 < value ){
                    m_pMeanOnsetFirstNote = 0x32;
                }else{
                    m_pMeanOnsetFirstNote = value;
                }
            }
        }

        [Category( "VOCALOID2" )]
        public int vMeanNoteTransition{
            get{
                return m_vMeanNoteTransition;
            }
            set{
                if( value < 0x05 ){
                    m_vMeanNoteTransition = 0x05;
                }else if( 0x1e < value ){
                    m_vMeanNoteTransition = 0x1e;
                }else{
                    m_vMeanNoteTransition = value;
                }
            }
        }

        [Category( "VOCALOID2" )]
        public int d4mean{
            get{
                return m_d4mean;
            }
            set{
                if( value < 0x0a ){
                    m_d4mean = 0x0a;
                }else if( 0x3c < value ){
                    m_d4mean = 0x3c;
                }else{
                    m_d4mean = value;
                }
            }
        }

        [Category( "VOCALOID2" )]
        public int pMeanEndingNote{
            get{
                return m_pMeanEndingNote;
            }
            set{
                if( value < 0x05 ){
                    m_pMeanEndingNote = 0x05;
                }else if( 0x1e < value ){
                    m_pMeanEndingNote = 0x1e;
                }else{
                    pMeanEndingNote = value;
                }
            }
        }
        #endregion

        #region VOCALOID1
        [TypeConverter( typeof( AttackVariationConverter ) ), Category( "VOALOID1" )]
        public AttackVariation Attack {
            get {
                return m_attack;
            }
            set {
                m_attack = value;
            }
        }

        [Category( "VOALOID1" )]
        public int AttackDepth {
            get {
                return m_attack_depth;
            }
            set {
                int draft = value;
                if ( draft < 0 ) {
                    draft = 0;
                } else if ( 127 < draft ) {
                    draft = 127;
                }
                m_attack_depth = draft;
            }
        }

        [Category( "VOALOID1" )]
        public int AttackDuration {
            get {
                return m_attack_duration;
            }
            set {
                int draft = value;
                if ( draft < 0 ) {
                    draft = 0;
                } else if ( 127 < draft ) {
                    draft = 127;
                }
                m_attack_duration = draft;
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
                    if ( m_vibrato_percent > 0 ) {
                        lastVibratoLength = m_vibrato_percent;
                    }
                } else if ( m_vibrato.description.Equals( VibratoVariation.empty.description ) ) {
                    if ( lastVibratoLength > 0 ) {
                        m_vibrato_percent = lastVibratoLength;
                    }
                }
                m_vibrato = value;
            }
        }

        [Category( "Vibrato" )]
        public int VibratoLength {
            get {
#if DEBUG
                PortUtil.println( "VsqEventItemProxy#get_VibratoLength; m_vibrato.description=" + m_vibrato.description );
#endif
                //if ( m_vibrato.description.Equals( VibratoVariation.empty.description ) ) {
                //    return 0;
                //} else {
                    return m_vibrato_percent;
                //}
            }
            set {
#if DEBUG
                PortUtil.println( "VsqEventItemProxy#set_VibratoLength; value=" + value );
#endif
                if ( value <= 0 ) {
                    m_vibrato = new VibratoVariation( VibratoVariation.empty.description );
                    if ( m_vibrato_percent > 0 ) {
                        lastVibratoLength = m_vibrato_percent;
                    }
                    m_vibrato_percent = 0;
                } else {
                    int draft = value;
                    if( 100 < draft ){
                        draft = 100;
                    }
                    lastVibratoLength = draft;
                    m_vibrato_percent = draft;
                }
            }
        }
        #endregion
    }

}
