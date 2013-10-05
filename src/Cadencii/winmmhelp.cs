/*
 * winmmhelp.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;

using cadencii;

namespace cadencii
{
    public unsafe static class winmmhelp
    {
        static uint s_num_joydev = 0;
        static bool[] s_joy_attatched;
        static bool s_initialized = false;
        static int[] s_joy_available;
        static JOYCAPSW[] s_joycaps;
        static readonly uint[] _BTN = new uint[32] { 0x0001,
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

        public static int JoyInit()
        {
            if (s_initialized) {
                JoyReset();
            }
            s_initialized = true;
            int num_joydev = (int)win32.joyGetNumDevs();
#if DEBUG
            cadencii.debug.push_log("winmmhelp.JoyInit");
            cadencii.debug.push_log("    num_joydev=" + num_joydev);
#endif
            if (num_joydev <= 0) {
                num_joydev = 0;
                return num_joydev;
            }
            s_joy_attatched = new bool[num_joydev];
            s_joycaps = new JOYCAPSW[num_joydev];
            int count = 0;
            for (int k = 0; k < num_joydev; k++) {
                JOYINFO ji = new JOYINFO();
                if (win32.joyGetPos((uint)k, ref ji) == win32.JOYERR_NOERROR) {
                    s_joy_attatched[k] = true;
                    JOYCAPSW jc = new JOYCAPSW();
                    win32.joyGetDevCapsW((uint)k, ref jc, (uint)Marshal.SizeOf(jc));
                    s_joycaps[k] = jc;
                    count++;
                } else {
                    s_joy_attatched[k] = false;
                }
            }
            if (count > 0) {
                s_joy_available = new int[count];
                int c = -1;
                for (int i = 0; i < num_joydev; i++) {
                    if (s_joy_attatched[i]) {
                        c++;
                        if (c >= count) {
                            break; //ここに来るのはエラー
                        }
                        s_joy_available[c] = i;
                    }
                }
            }
            s_num_joydev = (uint)count;
            return (int)s_num_joydev;
        }

        public static bool IsJoyAttatched(int index)
        {
            if (!s_initialized) {
                JoyInit();
            }
            if (s_num_joydev == 0 || index < 0 || (int)s_num_joydev <= index) {
                return false;
            }
            return s_joy_attatched[index];
        }

        public static bool JoyGetStatus(int index_, out byte[] buttons, out int pov)
        {
            if (!s_initialized) {
                pov = -1;
                buttons = new byte[0];
                return false;
            }
            if (s_num_joydev == 0 || index_ < 0 || (int)s_num_joydev <= index_) {
                pov = -1;
                buttons = new byte[0];
                return false;
            }
            int index = s_joy_available[index_];
            int len = (int)s_joycaps[index].wNumButtons;
            buttons = new byte[len];
            pov = -1;
            JOYINFOEX ji_ex = new JOYINFOEX();
            JOYCAPSW jcs = s_joycaps[index];
            ji_ex.dwSize = (ushort)Marshal.SizeOf(ji_ex);
            if ((jcs.wCaps & win32.JOYCAPS_HASPOV) == win32.JOYCAPS_HASPOV) {
                ji_ex.dwFlags = win32.JOY_RETURNPOV | win32.JOY_RETURNBUTTONS;
            } else {
                ji_ex.dwFlags = win32.JOY_RETURNBUTTONS | win32.JOY_RETURNX | win32.JOY_RETURNY;
            }

            if (s_joy_attatched[index]) {
                uint ret_getpos = win32.joyGetPosEx((uint)index, ref ji_ex);
                if (ret_getpos == win32.JOYERR_NOERROR) {
                    if ((jcs.wCaps & win32.JOYCAPS_HASPOV) == win32.JOYCAPS_HASPOV) {
                        pov = (int)ji_ex.dwPOV;
                        if ((0xffff & ji_ex.dwPOV) == 0xffff) {
                            pov = -1;
                        }
                    } else {
                        int flag = 0;
                        if (ji_ex.dwXpos < jcs.wXmin + (jcs.wXmax - jcs.wXmin) / 3) flag = flag | 1;
                        if (ji_ex.dwYpos < jcs.wYmin + (jcs.wYmax - jcs.wYmin) / 3) flag = flag | 2;
                        if (ji_ex.dwXpos > jcs.wXmax - (jcs.wXmax - jcs.wXmin) / 3) flag = flag | 4;
                        if (ji_ex.dwYpos > jcs.wYmax - (jcs.wYmax - jcs.wYmin) / 3) flag = flag | 8;
                        if (flag == 1) pov = 27000;//左
                        if (flag == 2) pov = 0;//上
                        if (flag == 4) pov = 9000;//右
                        if (flag == 8) pov = 18000;//下
                        if (flag == 3) pov = 31500;//左上
                        if (flag == 6) pov = 4500;//右上
                        if (flag == 12) pov = 13500;//右下
                        if (flag == 9) pov = 22500;//左下
                    }
                    for (int i = 0; i < len && i < jcs.wNumButtons; i++) {
                        buttons[i] = (((uint)ji_ex.dwButtons & _BTN[i]) != 0x0) ? (byte)0x80 : (byte)0x00;
                    }
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        public static int JoyGetNumButtons(int index)
        {
            if (!s_initialized) {
                JoyInit();
            }
            if (s_num_joydev == 0 || index < 0 || (int)s_num_joydev <= index) {
                return 0;
            }
            return (int)s_joycaps[s_joy_available[index]].wNumButtons;
        }

        public static void JoyReset()
        {
            if (s_initialized) {
                s_initialized = false;
                s_num_joydev = 0;
            }
        }

        public static int JoyGetNumJoyDev()
        {
            if (!s_initialized) {
                return JoyInit();
            } else {
                return (int)s_num_joydev;
            }
        }
    }

}
