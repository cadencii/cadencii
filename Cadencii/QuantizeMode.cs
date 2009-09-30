/*
 * QuantizeMode.cs
 * Copyright (c) 2008-2009 kbinani
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
using System.Xml.Serialization;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public static class QuantizeModeUtil {
        public static String GetString( QuantizeMode quantize_mode ) {
            switch ( quantize_mode ) {
                case QuantizeMode.off:
                    return "Off";
                case QuantizeMode.p4:
                    return "1/4";
                case QuantizeMode.p8:
                    return "1/8";
                case QuantizeMode.p16:
                    return "1/16";
                case QuantizeMode.p32:
                    return "1/32";
                case QuantizeMode.p64:
                    return "1/64";
                case QuantizeMode.p128:
                    return "1/128";
                default:
                    return "";
            }
        }

        /// <summary>
        /// クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <param name="qm"></param>
        /// <param name="triplet"></param>
        /// <returns></returns>
        public static int GetQuantizeClock( QuantizeMode qm, boolean triplet ) {
            int ret = 1;
            switch ( qm ) {
                case QuantizeMode.p4:
                    ret = 480;
                    break;
                case QuantizeMode.p8:
                    ret = 240;
                    break;
                case QuantizeMode.p16:
                    ret = 120;
                    break;
                case QuantizeMode.p32:
                    ret = 60;
                    break;
                case QuantizeMode.p64:
                    ret = 30;
                    break;
                case QuantizeMode.p128:
                    ret = 15;
                    break;
                default:
                    return 1;
            }
            if ( triplet ) {
                ret = ret * 2 / 3;
            }
            return ret;
        }
    }

    /*public class Quantize {
        private int m_group_notes = 0;
        private int m_base_length = 5;
        private boolean m_quantize_enabled = true;

        public override String ToString() {
            if ( m_quantize_enabled ) {
                return "1/" + pow( 2, m_base_length );
            } else {
                return "Off";
            }
        }

        public int GroupNoteNum {
            get {
                return m_group_notes;
            }
            set {
                m_group_notes = value;
            }
        }

        public int BaseNote {
            get {
                return pow( 2, m_base_length );
            }
            set {
                int val = 1;
                for ( int i = 1; i < int.MaxValue; i++ ) {
                    val = val * 2;
                    if ( value <= val ) {
                        m_base_length = i;
#if DEBUG
                        AppManager.DebugWriteLine( "Quantize+set__BaseNote" );
                        AppManager.DebugWriteLine( "    value=" + value );
                        AppManager.DebugWriteLine( "    m_base_length=" + m_base_length );
#endif
                        break;
                    }
                }
            }
        }

        public boolean QuantizeEnabled {
            get {
                return m_quantize_enabled;
            }
            set {
                m_quantize_enabled = value;
            }
        }

        public int GetUnitClock() {
            if ( !m_quantize_enabled ) {
                return 1;
            } else {
                int length = pow( 2, m_base_length );
                int ret = 480 * 4 / length;
                if ( m_group_notes > 2 ) {
                    int val = 1;
                    for ( int i = 1; i <= int.MaxValue; i++ ) {
                        val = val * 2;
                        if ( val + 1 <= m_group_notes && m_group_notes <= val * 2 - 1 ) {
                            ret = ret * pow( 2, i ) / m_group_notes;
                            break;
                        }
                    }
                }
                return ret;
            }
        }

        private static int pow( int value, int power ) {
            int ret = 1;
            for ( int i = 0; i < power; i++ ) {
                ret = ret * value;
            }
            return ret;
        }
    }*/

    public enum QuantizeMode : int {
        p4 = 0,
        p8 = 1,
        p16 = 2,
        p32 = 3,
        p64 = 4,
        off = 5,
        p128 = 6,
    }

}
