/*
 * winmmhelp.cs
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
using System.Runtime.InteropServices;

using bocoree;

namespace Boare.Cadencii {

    using boolean = Boolean;

    public unsafe static class winmmhelp {
        static uint s_num_joydev = 0;
        static boolean[] s_joy_attatched;
        static int[] s_button_num;
        static boolean s_initialized = false;
        static int[] s_joy_available;

        public static int JoyInit() {
            if ( s_initialized ) {
                JoyReset();
            }
            s_initialized = true;
            int num_joydev = (int)windows.joyGetNumDevs();
#if DEBUG
            bocoree.debug.push_log( "winmmhelp.JoyInit" );
            bocoree.debug.push_log( "    num_joydev=" + num_joydev );
#endif
            if ( num_joydev <= 0 ) {
                num_joydev = 0;
                return num_joydev;
            }
            s_joy_attatched = new boolean[num_joydev];
            s_button_num = new int[num_joydev];
            int count = 0;
            for ( int k = 0; k < num_joydev; k++ ) {
                JOYINFO ji = new JOYINFO();
                if ( windows.joyGetPos( (uint)k, ref ji ) == windows.JOYERR_NOERROR ) {
                    s_joy_attatched[k] = true;
                    JOYCAPSW jc = new JOYCAPSW();
                    windows.joyGetDevCapsW( (uint)k, ref jc, (uint)Marshal.SizeOf( jc ) );
                    s_button_num[k] = (int)jc.wNumButtons;
                    count++;
                } else {
                    s_joy_attatched[k] = false;
                    s_button_num[k] = 0;
                }
            }
            if ( count > 0 ) {
                s_joy_available = new int[count];
                int c = -1;
                for ( int i = 0; i < num_joydev; i++ ) {
                    if ( s_joy_attatched[i] ) {
                        c++;
                        if ( c >= count ) {
                            break; //ここに来るのはエラー
                        }
                        s_joy_available[c] = i;
                    }
                }
            }
            s_num_joydev = (uint)count;
            return (int)s_num_joydev;
        }

        public static boolean IsJoyAttatched( int index ) {
            if ( !s_initialized ) {
                JoyInit();
            }
            if ( s_num_joydev == 0 || index < 0 || (int)s_num_joydev <= index ) {
                return false;
            }
            return s_joy_attatched[index];
        }

        public static boolean JoyGetStatus( int index_, out byte[] buttons, out int pov ) {
            uint[] _BTN = new uint[32] { 0x0001,
                               0x0002,
                               0x0004,
                               0x0008,
                               0x00000010,
                               0x00000020,
                               0x00000040,
                               0x00000080,
                               0x00000100,
                               0x00000200,
                               0x00000400,
                               0x00000800,
                               0x00001000,
                               0x00002000,
                               0x00004000,
                               0x00008000,
                               0x00010000,
                               0x00020000,
                               0x00040000,
                               0x00080000,
                               0x00100000,
                               0x00200000,
                               0x00400000,
                               0x00800000,
                               0x01000000,
                               0x02000000,
                               0x04000000,
                               0x08000000,
                               0x10000000,
                               0x20000000,
                               0x40000000,
                               0x80000000 };
            if ( !s_initialized ) {
                pov = -1;
                buttons = new byte[0];
                return false;
            }
            if ( s_num_joydev == 0 || index_ < 0 || (int)s_num_joydev <= index_ ) {
                pov = -1;
                buttons = new byte[0];
                return false;
            }
            int len = s_button_num[index_];
            buttons = new byte[len];
            pov = -1;
            int index = s_joy_available[index_];
            JOYINFOEX ji_ex = new JOYINFOEX();
            ji_ex.dwSize = (ushort)Marshal.SizeOf( ji_ex );
            ji_ex.dwFlags = windows.JOY_RETURNPOV | windows.JOY_RETURNBUTTONS;

            if ( s_joy_attatched[index] ) {
                windows.joyGetPosEx( (uint)index, ref ji_ex );
                pov = (int)ji_ex.dwPOV;
                if ( pov == 0xffff ) {
                    pov = -1;
                }
                for ( int i = 0; i < len && i < s_button_num[index]; i++ ) {
                    buttons[i] = (((uint)ji_ex.dwButtons & _BTN[i]) != 0x0) ? (byte)0x80 : (byte)0x00;
                }
                return true;
            } else {
                return false;
            }
        }

        public static int JoyGetNumButtons( int index ) {
            if ( !s_initialized ) {
                JoyInit();
            }
            if ( s_num_joydev == 0 || index < 0 || (int)s_num_joydev <= index ) {
                return 0;
            }
            return s_button_num[s_joy_available[index]];
        }

        public static void JoyReset() {
            if ( s_initialized ) {
                s_initialized = false;
                s_num_joydev = 0;
            }
        }

        public static int JoyGetNumJoyDev() {
            if ( !s_initialized ) {
                return JoyInit();
            } else {
                return (int)s_num_joydev;
            }
        }
    }

}
