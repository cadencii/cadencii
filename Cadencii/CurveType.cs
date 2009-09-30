/*
 * CurveType.cs
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

namespace Boare.Cadencii {

    using boolean = Boolean;

    /// <summary>
    /// vsqファイルで編集可能なカーブ・プロパティの種類
    /// </summary>
    [Serializable]
    public struct CurveType : IEquatable<CurveType> {
        private String m_type;
        private boolean m_is_scalar;
        private int m_minimum;
        private int m_maximum;
        private int m_default;
        private boolean m_is_attach_note;
        private int m_index;

        /// <summary>
        /// ベロシティ(index=-1)
        /// </summary>
        public static readonly CurveType VEL = new CurveType( "VEL", true, true, 0, 127, 64, -1 );
        private static readonly CurveType s_dyn = new CurveType( "DYN", false, false, 0, 127, 64, 0 );
        private static readonly CurveType s_bre = new CurveType( "BRE", false, false, 0, 127, 0, 1 );
        private static readonly CurveType s_bri = new CurveType( "BRI", false, false, 0, 127, 64, 2 );
        private static readonly CurveType s_cle = new CurveType( "CLE", false, false, 0, 127, 0, 3 );
        private static readonly CurveType s_ope = new CurveType( "OPE", false, false, 0, 127, 127, 4 );
        private static readonly CurveType s_gen = new CurveType( "GEN", false, false, 0, 127, 64, 5 );
        private static readonly CurveType s_por = new CurveType( "POR", false, false, 0, 127, 64, 6 );
        private static readonly CurveType s_pit = new CurveType( "PIT", false, false, -8192, 8191, 0, 7 );
        private static readonly CurveType s_pbs = new CurveType( "PBS", false, false, 0, 24, 2, 8 );
        private static readonly CurveType s_vibrato_rate  = new CurveType( "V-Rate", false, true, 0, 127, 64, 9 );
        private static readonly CurveType s_vibrato_depth = new CurveType( "V-Depth", false, true, 0, 127, 50, 10 );
        private static readonly CurveType s_accent        = new CurveType( "Accent", true, true, 0, 100, 50, -1 );
        private static readonly CurveType s_decay         = new CurveType( "Decay", true, true, 0, 100, 50, -1 );
        private static readonly CurveType s_harmonics     = new CurveType( "Harmonics", false, false, 0, 127, 0, 11 );
        private static readonly CurveType s_fx2depth      = new CurveType( "FX2Depth", false, false, 0, 127, 0, 12 );
        private static readonly CurveType s_reso1freq     = new CurveType( "reso1freq", false, false, 0, 127, 0, 13 );
        private static readonly CurveType s_reso1bw       = new CurveType( "reso1bw", false, false, 0, 127, 0, 14 );
        private static readonly CurveType s_reso1amp      = new CurveType( "reso1amp", false, false, 0, 127, 0, 15 );
        private static readonly CurveType s_reso2freq     = new CurveType( "reso2freq", false, false, 0, 127, 0, 16 );
        private static readonly CurveType s_reso2bw       = new CurveType( "reso2bw", false, false, 0, 127, 0, 17 );
        private static readonly CurveType s_reso2amp      = new CurveType( "reso2amp", false, false, 0, 127, 0, 18 );
        private static readonly CurveType s_reso3freq     = new CurveType( "reso3freq", false, false, 0, 127, 0, 19 );
        private static readonly CurveType s_reso3bw       = new CurveType( "reso3bw", false, false, 0, 127, 0, 20 );
        private static readonly CurveType s_reso3amp      = new CurveType( "reso3amp", false, false, 0, 127, 0, 21 );
        private static readonly CurveType s_reso4freq     = new CurveType( "reso4freq", false, false, 0, 127, 0, 22 );
        private static readonly CurveType s_reso4bw       = new CurveType( "reso4bw", false, false, 0, 127, 0, 23 );
        private static readonly CurveType s_reso4amp      = new CurveType( "reso4amp", false, false, 0, 127, 0, 24 );
        //private static readonly CurveType s_pitch = new CurveType( "Pitch", false, false, -240000, 240000, 0, 25 );
        private static readonly CurveType s_envelope      = new CurveType( "Env", true, true, 0, 200, 100, -1 );
        public static readonly CurveType Empty = new CurveType( "Empty", false, false, 0, 0, 0, -1 );

        private CurveType( String type, boolean is_scalar, boolean is_attach_note, int min, int max, int defalt_value, int index ) {
            m_type = type;
            m_is_scalar = is_scalar;
            m_minimum = min;
            m_maximum = max;
            m_default = defalt_value;
            m_is_attach_note = is_attach_note;
            m_index = index;
        }

        public override String ToString() {
            return toString();
        }

        public String toString() {
            return m_type;
        }

        public int Index {
            get {
                return m_index;
            }
        }

        [Obsolete]
        public String Name {
            get {
                return getName();
            }
        }

        public String getName() {
            return m_type;
        }

        public boolean IsAttachNote {
            get {
                return m_is_attach_note;
            }
        }

        public boolean IsScalar {
            get {
                return m_is_scalar;
            }
        }

        [Obsolete]
        public int Minimum {
            get {
                return getMinimum();
            }
        }

        [Obsolete]
        public int Maximum {
            get {
                return getMaximum();
            }
        }

        public int getMaximum() {
            return m_maximum;
        }

        public int getMinimum() {
            return m_minimum;
        }

        [Obsolete]
        public int Default {
            get {
                return getDefault();
            }
        }

        public int getDefault() {
            return m_default;
        }

        public boolean equals( CurveType other ) {
            return (m_type == other.m_type && m_is_scalar == other.m_is_scalar);
        }

        public boolean Equals( CurveType other ) {
            return equals( other );
        }

        public static CurveType Env {
            get {
                return s_envelope;
            }
        }

        /// <summary>
        /// ダイナミクス　64(index=0)
        /// </summary>
        public static CurveType DYN {
            get {
                return s_dyn;
            }
        }

        /// <summary>
        /// ブレシネス　0(index=1)
        /// </summary>
        public static CurveType BRE {
            get {
                return s_bre;
            }
        }

        /// <summary>
        /// ブライトネス　64(index=2)
        /// </summary>
        public static CurveType BRI {
            get {
                return s_bri;
            }
        }

        /// <summary>
        /// クリアネス　0(index=3)
        /// </summary>
        public static CurveType CLE {
            get {
                return s_cle;
            }
        }

        /// <summary>
        /// オープニング　127(index=4)
        /// </summary>
        public static CurveType OPE {
            get {
                return s_ope;
            }
        }

        /// <summary>
        /// ジェンダーファクター　64(index=5)
        /// </summary>
        public static CurveType GEN {
            get {
                return s_gen;
            }
        }

        /// <summary>
        /// ポルタメントタイミング　64(index=6)
        /// </summary>
        public static CurveType POR {
            get {
                return s_por;
            }
        }

        public static CurveType PIT {
            get {
                return s_pit;
            }
        }

        public static CurveType PBS {
            get {
                return s_pbs;
            }
        }

        /// <summary>
        /// ビブラートの振動の速さ(index=9)
        /// </summary>
        public static CurveType VibratoRate {
            get {
                return s_vibrato_rate;
            }
        }

        /// <summary>
        /// ビブラートの振幅の大きさ(index=10)
        /// </summary>
        public static CurveType VibratoDepth {
            get {
                return s_vibrato_depth;
            }
        }

        /// <summary>
        /// Accent(index=-1)
        /// </summary>
        public static CurveType Accent {
            get {
                return s_accent;
            }
        }

        /// <summary>
        /// Decay(index=-1)
        /// </summary>
        public static CurveType Decay {
            get {
                return s_decay;
            }
        }

        /// <summary>
        /// Harmonics(index=11)
        /// </summary>
        public static CurveType harmonics {
            get {
                return s_harmonics;
            }
        }

        /// <summary>
        /// FX2Depth(index=12)
        /// </summary>
        public static CurveType fx2depth {
            get {
                return s_fx2depth;
            }
        }

        /// <summary>
        /// reso1freq(index=13)
        /// </summary>
        public static CurveType reso1freq {
            get {
                return s_reso1freq;
            }
        }

        /// <summary>
        /// reso1bw(index=14)
        /// </summary>
        public static CurveType reso1bw {
            get {
                return s_reso1bw;
            }
        }

        /// <summary>
        /// reso1amp(index=15)
        /// </summary>
        public static CurveType reso1amp {
            get {
                return s_reso1amp;
            }
        }

        /// <summary>
        /// reso2freq(index=16)
        /// </summary>
        public static CurveType reso2freq {
            get {
                return s_reso2freq;
            }
        }

        /// <summary>
        /// reso2bw(index=17)
        /// </summary>
        public static CurveType reso2bw {
            get {
                return s_reso2bw;
            }
        }

        /// <summary>
        /// reso2amp(index=18)
        /// </summary>
        public static CurveType reso2amp {
            get {
                return s_reso2amp;
            }
        }

        /// <summary>
        /// reso3freq(index=19)
        /// </summary>
        public static CurveType reso3freq {
            get {
                return s_reso3freq;
            }
        }

        /// <summary>
        /// reso3bw(index=20)
        /// </summary>
        public static CurveType reso3bw {
            get {
                return s_reso3bw;
            }
        }

        /// <summary>
        /// reso3amp(index=21)
        /// </summary>
        public static CurveType reso3amp {
            get {
                return s_reso3amp;
            }
        }

        /// <summary>
        /// reso4freq(index=22)
        /// </summary>
        public static CurveType reso4freq {
            get {
                return s_reso4freq;
            }
        }

        /// <summary>
        /// reso4bw(index=23)
        /// </summary>
        public static CurveType reso4bw {
            get {
                return s_reso4bw;
            }
        }

        /// <summary>
        /// reso4amp(index=24)
        /// </summary>
        public static CurveType reso4amp {
            get {
                return s_reso4amp;
            }
        }

        /* /// <summary>
        /// Pitch(index=25)
        /// </summary>
        public static CurveType Pitch {
            get {
                return s_pitch;
            }
        }*/
    }

}
