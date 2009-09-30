/*
 * UstEvent.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using bocoree;

namespace Boare.Lib.Vsq {

    [Serializable]
    public class UstEvent : ICloneable {
        public String Tag;
        public int Length = 0;
        public String Lyric = "";
        public int Note = -1;
        public int Intensity = -1;
        public int PBType = -1;
        public float[] Pitches = null;
        public float Tempo = -1;
        public UstVibrato Vibrato = null;
        public UstPortamento Portamento = null;
        public int PreUtterance = 0;
        public int VoiceOverlap = 0;
        public UstEnvelope Envelope = null;
        public String Flags = "";
        public int Moduration = 100;
        public int Index;

        public UstEvent(){
        }

        public object Clone() {
            UstEvent ret = new UstEvent();
            ret.Length = Length;
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
                ret.Vibrato = (UstVibrato)Vibrato.Clone();
            }
            if ( Portamento != null ) {
                ret.Portamento = (UstPortamento)Portamento.Clone();
            }
            if ( Envelope != null ) {
                ret.Envelope = (UstEnvelope)Envelope.Clone();
            }
            ret.PreUtterance = PreUtterance;
            ret.VoiceOverlap = VoiceOverlap;
            ret.Flags = Flags;
            ret.Moduration = Moduration;
            ret.Tag = Tag;
            return ret;
        }

        public void print( StreamWriter sw ) {
            if ( this.Index == int.MinValue ) {
                sw.WriteLine( "[#PREV]" );
            } else if ( this.Index == int.MaxValue ) {
                sw.WriteLine( "[#NEXT]" );
            } else {
                sw.WriteLine( String.Format( "[#{0:d4}]", Index ) );
            }
            sw.WriteLine( "Length=" + Length );
            sw.WriteLine( "Lyric=" + Lyric );
            sw.WriteLine( "NoteNum=" + Note );
            if ( Intensity >= 0 ) {
                sw.WriteLine( "Intensity=" + Intensity );
            }
            if ( PBType >= 0 && Pitches != null ) {
                sw.WriteLine( "PBType=" + PBType );
                sw.Write( "Piches=" );
                for ( int i = 0; i < Pitches.Length; i++ ) {
                    if ( i == 0 ) {
                        sw.Write( Pitches[i] );
                    } else {
                        sw.Write( "," + Pitches[i] );
                    }
                }
                sw.WriteLine();
            }
            if ( Tempo > 0 ) {
                sw.WriteLine( "Tempo=" + Tempo );
            }
            if ( Vibrato != null ) {
                sw.WriteLine( Vibrato.ToString() );
            }
            if ( Portamento != null ) {
                Portamento.print( sw );
            }
            if ( Envelope != null ) {
                if ( PreUtterance >= 0 ) {
                    sw.WriteLine( "PreUtterance=" + PreUtterance );
                }
                if ( VoiceOverlap != 0 ) {
                    sw.WriteLine( "VoiceOverlap=" + VoiceOverlap );
                }
                sw.WriteLine( Envelope.ToString() );
            }
            if ( Flags != "" ) {
                sw.WriteLine( "Flags=" + Flags );
            }
            if ( Moduration >= 0 ) {
                sw.WriteLine( "Moduration=" + Moduration );
            }
        }
    }

}
