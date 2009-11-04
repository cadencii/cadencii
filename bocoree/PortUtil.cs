/*
 * PortUtil.cs
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
#if JAVA
package org.kbinani;

import java.util.*;
import java.nio.*;
import java.nio.charset.*;
import java.io.*;
import java.text.*;
import java.awt.*;
import java.awt.event.*;
import java.awt.geom.*;
import java.security.*;
import java.nio.channels.*;
import javax.swing.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using bocoree.awt;
using bocoreex.swing;
using bocoree.windows.forms;
using bocoree.awt.event_;
using bocoree.util;

namespace bocoree {
    using boolean = System.Boolean;
    using Integer = System.Int32;
    using Long = System.Int64;
    using Float = System.Single;
#endif
    public class PortUtil {
        public static Color AliceBlue = new Color( 240, 248, 255 );
        public static Color AntiqueWhite = new Color( 250, 235, 215 );
        public static Color Aqua = new Color( 0, 255, 255 );
        public static Color Aquamarine = new Color( 127, 255, 212 );
        public static Color Azure = new Color( 240, 255, 255 );
        public static Color Beige = new Color( 245, 245, 220 );
        public static Color Bisque = new Color( 255, 228, 196 );
        public static Color Black = new Color( 0, 0, 0 );
        public static Color BlanchedAlmond = new Color( 255, 235, 205 );
        public static Color Blue = new Color( 0, 0, 255 );
        public static Color BlueViolet = new Color( 138, 43, 226 );
        public static Color Brown = new Color( 165, 42, 42 );
        public static Color BurlyWood = new Color( 222, 184, 135 );
        public static Color CadetBlue = new Color( 95, 158, 160 );
        public static Color Chartreuse = new Color( 127, 255, 0 );
        public static Color Chocolate = new Color( 210, 105, 30 );
        public static Color Coral = new Color( 255, 127, 80 );
        public static Color CornflowerBlue = new Color( 100, 149, 237 );
        public static Color Cornsilk = new Color( 255, 248, 220 );
        public static Color Crimson = new Color( 220, 20, 60 );
        public static Color Cyan = new Color( 0, 255, 255 );
        public static Color DarkBlue = new Color( 0, 0, 139 );
        public static Color DarkCyan = new Color( 0, 139, 139 );
        public static Color DarkGoldenrod = new Color( 184, 134, 11 );
        public static Color DarkGray = new Color( 169, 169, 169 );
        public static Color DarkGreen = new Color( 0, 100, 0 );
        public static Color DarkKhaki = new Color( 189, 183, 107 );
        public static Color DarkMagenta = new Color( 139, 0, 139 );
        public static Color DarkOliveGreen = new Color( 85, 107, 47 );
        public static Color DarkOrange = new Color( 255, 140, 0 );
        public static Color DarkOrchid = new Color( 153, 50, 204 );
        public static Color DarkRed = new Color( 139, 0, 0 );
        public static Color DarkSalmon = new Color( 233, 150, 122 );
        public static Color DarkSeaGreen = new Color( 143, 188, 139 );
        public static Color DarkSlateBlue = new Color( 72, 61, 139 );
        public static Color DarkSlateGray = new Color( 47, 79, 79 );
        public static Color DarkTurquoise = new Color( 0, 206, 209 );
        public static Color DarkViolet = new Color( 148, 0, 211 );
        public static Color DeepPink = new Color( 255, 20, 147 );
        public static Color DeepSkyBlue = new Color( 0, 191, 255 );
        public static Color DimGray = new Color( 105, 105, 105 );
        public static Color DodgerBlue = new Color( 30, 144, 255 );
        public static Color Firebrick = new Color( 178, 34, 34 );
        public static Color FloralWhite = new Color( 255, 250, 240 );
        public static Color ForestGreen = new Color( 34, 139, 34 );
        public static Color Fuchsia = new Color( 255, 0, 255 );
        public static Color Gainsboro = new Color( 220, 220, 220 );
        public static Color GhostWhite = new Color( 248, 248, 255 );
        public static Color Gold = new Color( 255, 215, 0 );
        public static Color Goldenrod = new Color( 218, 165, 32 );
        public static Color Gray = new Color( 128, 128, 128 );
        public static Color Green = new Color( 0, 128, 0 );
        public static Color GreenYellow = new Color( 173, 255, 47 );
        public static Color Honeydew = new Color( 240, 255, 240 );
        public static Color HotPink = new Color( 255, 105, 180 );
        public static Color IndianRed = new Color( 205, 92, 92 );
        public static Color Indigo = new Color( 75, 0, 130 );
        public static Color Ivory = new Color( 255, 255, 240 );
        public static Color Khaki = new Color( 240, 230, 140 );
        public static Color Lavender = new Color( 230, 230, 250 );
        public static Color LavenderBlush = new Color( 255, 240, 245 );
        public static Color LawnGreen = new Color( 124, 252, 0 );
        public static Color LemonChiffon = new Color( 255, 250, 205 );
        public static Color LightBlue = new Color( 173, 216, 230 );
        public static Color LightCoral = new Color( 240, 128, 128 );
        public static Color LightCyan = new Color( 224, 255, 255 );
        public static Color LightGoldenrodYellow = new Color( 250, 250, 210 );
        public static Color LightGreen = new Color( 144, 238, 144 );
        public static Color LightGray = new Color( 211, 211, 211 );
        public static Color LightPink = new Color( 255, 182, 193 );
        public static Color LightSalmon = new Color( 255, 160, 122 );
        public static Color LightSeaGreen = new Color( 32, 178, 170 );
        public static Color LightSkyBlue = new Color( 135, 206, 250 );
        public static Color LightSlateGray = new Color( 119, 136, 153 );
        public static Color LightSteelBlue = new Color( 176, 196, 222 );
        public static Color LightYellow = new Color( 255, 255, 224 );
        public static Color Lime = new Color( 0, 255, 0 );
        public static Color LimeGreen = new Color( 50, 205, 50 );
        public static Color Linen = new Color( 250, 240, 230 );
        public static Color Magenta = new Color( 255, 0, 255 );
        public static Color Maroon = new Color( 128, 0, 0 );
        public static Color MediumAquamarine = new Color( 102, 205, 170 );
        public static Color MediumBlue = new Color( 0, 0, 205 );
        public static Color MediumOrchid = new Color( 186, 85, 211 );
        public static Color MediumPurple = new Color( 147, 112, 219 );
        public static Color MediumSeaGreen = new Color( 60, 179, 113 );
        public static Color MediumSlateBlue = new Color( 123, 104, 238 );
        public static Color MediumSpringGreen = new Color( 0, 250, 154 );
        public static Color MediumTurquoise = new Color( 72, 209, 204 );
        public static Color MediumVioletRed = new Color( 199, 21, 133 );
        public static Color MidnightBlue = new Color( 25, 25, 112 );
        public static Color MintCream = new Color( 245, 255, 250 );
        public static Color MistyRose = new Color( 255, 228, 225 );
        public static Color Moccasin = new Color( 255, 228, 181 );
        public static Color NavajoWhite = new Color( 255, 222, 173 );
        public static Color Navy = new Color( 0, 0, 128 );
        public static Color OldLace = new Color( 253, 245, 230 );
        public static Color Olive = new Color( 128, 128, 0 );
        public static Color OliveDrab = new Color( 107, 142, 35 );
        public static Color Orange = new Color( 255, 165, 0 );
        public static Color OrangeRed = new Color( 255, 69, 0 );
        public static Color Orchid = new Color( 218, 112, 214 );
        public static Color PaleGoldenrod = new Color( 238, 232, 170 );
        public static Color PaleGreen = new Color( 152, 251, 152 );
        public static Color PaleTurquoise = new Color( 175, 238, 238 );
        public static Color PaleVioletRed = new Color( 219, 112, 147 );
        public static Color PapayaWhip = new Color( 255, 239, 213 );
        public static Color PeachPuff = new Color( 255, 218, 185 );
        public static Color Peru = new Color( 205, 133, 63 );
        public static Color Pink = new Color( 255, 192, 203 );
        public static Color Plum = new Color( 221, 160, 221 );
        public static Color PowderBlue = new Color( 176, 224, 230 );
        public static Color Purple = new Color( 128, 0, 128 );
        public static Color Red = new Color( 255, 0, 0 );
        public static Color RosyBrown = new Color( 188, 143, 143 );
        public static Color RoyalBlue = new Color( 65, 105, 225 );
        public static Color SaddleBrown = new Color( 139, 69, 19 );
        public static Color Salmon = new Color( 250, 128, 114 );
        public static Color SandyBrown = new Color( 244, 164, 96 );
        public static Color SeaGreen = new Color( 46, 139, 87 );
        public static Color SeaShell = new Color( 255, 245, 238 );
        public static Color Sienna = new Color( 160, 82, 45 );
        public static Color Silver = new Color( 192, 192, 192 );
        public static Color SkyBlue = new Color( 135, 206, 235 );
        public static Color SlateBlue = new Color( 106, 90, 205 );
        public static Color SlateGray = new Color( 112, 128, 144 );
        public static Color Snow = new Color( 255, 250, 250 );
        public static Color SpringGreen = new Color( 0, 255, 127 );
        public static Color SteelBlue = new Color( 70, 130, 180 );
        public static Color Tan = new Color( 210, 180, 140 );
        public static Color Teal = new Color( 0, 128, 128 );
        public static Color Thistle = new Color( 216, 191, 216 );
        public static Color Tomato = new Color( 255, 99, 71 );
        public static Color Turquoise = new Color( 64, 224, 208 );
        public static Color Violet = new Color( 238, 130, 238 );
        public static Color Wheat = new Color( 245, 222, 179 );
        public static Color White = new Color( 255, 255, 255 );
        public static Color WhiteSmoke = new Color( 245, 245, 245 );
        public static Color Yellow = new Color( 255, 255, 0 );
        public static Color YellowGreen = new Color( 154, 205, 50 );

        public static double getCurrentTime() {
#if JAVA
            return new Date().getTime();
#else
            return DateTime.Now.Ticks * 100.0 / 1e9;
#endif
        }

        #region BitConverter

        public static byte[] getbytes_int64_le( long data ) {
#if JAVA
            byte[] dat = new byte[8];
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[7] = (byte)(data & (byte)0xff);
            return dat;
#else
            byte[] dat = new byte[8];
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[7] = (byte)(data & (byte)0xff);
            return dat;
#endif
        }

        public static byte[] getbytes_uint32_le( long data ) {
#if JAVA
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[3] = (byte)(data & (byte)0xff);
            return dat;
#else
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            return dat;
#endif
        }

        public static byte[] getbytes_int32_le( int data ) {
            long v = data;
            if ( v < 0 ) {
                v += 4294967296L;
            }
            return getbytes_uint32_le( v );
        }

        public static byte[] getbytes_int64_be( long data ) {
#if JAVA
            byte[] dat = new byte[8];
            dat[7] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
#else
            byte[] dat = new byte[8];
            dat[7] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
#endif
        }

        public static byte[] getbytes_uint32_be( long data ) {
#if JAVA
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
#else
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[3] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[2] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[1] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
#endif
        }

        public static byte[] getbytes_int16_le( short data ) {
            int i = data;
            if ( i < 0 ) {
                i += 65536;
            }
            return getbytes_uint16_le( i );
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static byte[] getbytes_uint16_le( int data ) {
            byte[] dat = new byte[2];
#if JAVA
            dat[0] = (byte)(data & (byte)0xff);
            data = (byte)(data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
#else
            dat[0] = (byte)(data & (byte)0xff);
            data = (byte)(data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
#endif
            return dat;
        }

        public static byte[] getbytes_uint16_be( int data ) {
            byte[] dat = new byte[2];
#if JAVA
            dat[1] = (byte)(data & (byte)0xff);
            data = (byte)(data >>> 8);
            dat[0] = (byte)(data & (byte)0xff);
#else
            dat[1] = (byte)(data & (byte)0xff);
            data = (byte)(data >> 8);
            dat[0] = (byte)(data & (byte)0xff);
#endif
            return dat;
        }

        public static long make_int64_le( byte[] buf ) {
            return (long)((long)((long)((long)((long)((long)((long)((long)((long)(((buf[7] << 8) | buf[6]) << 8) | buf[5]) << 8) | buf[4]) << 8) | buf[3]) << 8 | buf[2]) << 8) | buf[1]) << 8 | buf[0];
        }

        public static long make_int64_be( byte[] buf ) {
            return (long)((long)((long)((long)((long)((long)((long)((long)((long)(((buf[0] << 8) | buf[1]) << 8) | buf[2]) << 8) | buf[3]) << 8) | buf[4]) << 8 | buf[5]) << 8) | buf[6]) << 8 | buf[7];
        }

        public static long make_uint32_le( byte[] buf ) {
            return (long)((long)((long)((long)((buf[3] << 8) | buf[2]) << 8) | buf[1]) << 8) | buf[0];
        }

        public static long make_uint32_be( byte[] buf ) {
            return (long)((long)((long)((long)((buf[0] << 8) | buf[1]) << 8) | buf[2]) << 8) | buf[3];
        }

        public static int make_int32_le( byte[] buf ) {
            long v = make_uint32_le( buf );
            if ( v >= 2147483647L ) {
                v -= 4294967296L;
            }
            return (int)v;
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static int make_uint16_le( byte[] buf ) {
            return (int)((int)(buf[1] << 8) | buf[0]);
        }

        public static int make_uint16_be( byte[] buf ) {
            return (int)((int)(buf[0] << 8) | buf[1]);
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static short make_int16_le( byte[] buf ) {
            int i = make_uint16_le( buf );
            if ( i >= 32768 ) {
                i = i - 65536;
            }
            return (short)i;
        }
        #endregion

        #region System.Windows.Forms.Keys and bocoree.windows.forms.BKeys
#if !JAVA
        public static int getModifierFromKeys( System.Windows.Forms.Keys keys ) {
            int ret = 0;
            if ( (keys & System.Windows.Forms.Keys.Control) == System.Windows.Forms.Keys.Control ) {
                ret += InputEvent.CTRL_MASK;
            }
            if ( (keys & System.Windows.Forms.Keys.Alt) == System.Windows.Forms.Keys.Alt ) {
                ret += InputEvent.ALT_MASK;
            }
            if ( (keys & System.Windows.Forms.Keys.Shift) == System.Windows.Forms.Keys.Shift ) {
                ret += InputEvent.SHIFT_MASK;
            }
            return ret;
        }
#endif

        public static KeyStroke getKeyStrokeFromBKeys( BKeys[] keys ) {
            int modifier = 0;
            int keycode = KeyEvent.VK_UNDEFINED;
            for ( int i = 0; i < keys.Length; i++ ) {
                if ( keys[i] == BKeys.Alt ) {
                    modifier += InputEvent.ALT_MASK;
                } else if ( keys[i] == BKeys.Control ) {
                    modifier += InputEvent.CTRL_MASK;
                } else if ( keys[i] == BKeys.Shift ) {
                    modifier += InputEvent.SHIFT_MASK;
                } else {
#if JAVA
                    keycode = keys[i].getValue();
#else
                    keycode = (int)keys[i];
#endif
                }
            }
            return KeyStroke.getKeyStroke( keycode, modifier );
        }

        public static BKeys[] getBKeysFromKeyStroke( KeyStroke stroke ) {
            Vector<BKeys> ret = new Vector<BKeys>();
            int keycodes = stroke.getKeyCode();
            int modifier = stroke.getModifiers();
            ret.add( getBKeysFromKeyCode( keycodes ) );
            if ( (modifier & InputEvent.ALT_MASK) == InputEvent.ALT_MASK ) {
                ret.add( BKeys.Alt );
            }
            if ( (modifier & InputEvent.CTRL_MASK) == InputEvent.CTRL_MASK ) {
                ret.add( BKeys.Control );
            }
            if ( (modifier & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                ret.add( BKeys.Shift );
            }
            return ret.toArray( new BKeys[] { } );
        }

        public static int getKeyCodeFromBKeys( BKeys keys ) {
#if JAVA
            return keys.getValue();
#else
            return (int)keys;
#endif
        }

        public static BKeys getBKeysFromKeyCode( int code ) {
            switch ( code ) {
                case KeyEvent.VK_UNDEFINED:
                    return BKeys.None;
                case KeyEvent.KEY_LOCATION_STANDARD:
                    return BKeys.LButton;
                case KeyEvent.KEY_LOCATION_LEFT:
                    return BKeys.RButton;
                case KeyEvent.VK_CANCEL:
                    return BKeys.Cancel;
                case KeyEvent.KEY_LOCATION_NUMPAD:
                    return BKeys.MButton;
                case KeyEvent.VK_BACK_SPACE:
                    return BKeys.Back;
                case KeyEvent.VK_TAB:
                    return BKeys.Tab;
                case KeyEvent.VK_ENTER:
                    return BKeys.LineFeed;
                case KeyEvent.VK_CLEAR:
                    return BKeys.Clear;
                case KeyEvent.VK_SHIFT:
                    return BKeys.ShiftKey;
                case KeyEvent.VK_CONTROL:
                    return BKeys.ControlKey;
                case KeyEvent.VK_ALT:
                    return BKeys.Menu;
                case KeyEvent.VK_PAUSE:
                    return BKeys.Pause;
                case KeyEvent.VK_CAPS_LOCK:
                    return BKeys.CapsLock;
                case KeyEvent.VK_KANA:
                    return BKeys.KanaMode;
                case KeyEvent.VK_FINAL:
                    return BKeys.FinalMode;
                case KeyEvent.VK_KANJI:
                    return BKeys.KanjiMode;
                case KeyEvent.VK_ESCAPE:
                    return BKeys.Escape;
                case KeyEvent.VK_CONVERT:
                    return BKeys.IMEConvert;
                case KeyEvent.VK_NONCONVERT:
                    return BKeys.IMENonconvert;
                case KeyEvent.VK_ACCEPT:
                    return BKeys.IMEAccept;
                case KeyEvent.VK_MODECHANGE:
                    return BKeys.IMEModeChange;
                case KeyEvent.VK_SPACE:
                    return BKeys.Space;
                case KeyEvent.VK_PAGE_UP:
                    return BKeys.PageUp;
                case KeyEvent.VK_PAGE_DOWN:
                    return BKeys.PageDown;
                case KeyEvent.VK_END:
                    return BKeys.End;
                case KeyEvent.VK_HOME:
                    return BKeys.Home;
                case KeyEvent.VK_LEFT:
                    return BKeys.Left;
                case KeyEvent.VK_UP:
                    return BKeys.Up;
                case KeyEvent.VK_RIGHT:
                    return BKeys.Right;
                case KeyEvent.VK_DOWN:
                    return BKeys.Down;
                case KeyEvent.VK_0:
                    return BKeys.D0;
                case KeyEvent.VK_1:
                    return BKeys.D1;
                case KeyEvent.VK_2:
                    return BKeys.D2;
                case KeyEvent.VK_3:
                    return BKeys.D3;
                case KeyEvent.VK_4:
                    return BKeys.D4;
                case KeyEvent.VK_5:
                    return BKeys.D5;
                case KeyEvent.VK_6:
                    return BKeys.D6;
                case KeyEvent.VK_7:
                    return BKeys.D7;
                case KeyEvent.VK_8:
                    return BKeys.D8;
                case KeyEvent.VK_9:
                    return BKeys.D9;
                case KeyEvent.VK_A:
                    return BKeys.A;
                case KeyEvent.VK_B:
                    return BKeys.B;
                case KeyEvent.VK_C:
                    return BKeys.C;
                case KeyEvent.VK_D:
                    return BKeys.D;
                case KeyEvent.VK_E:
                    return BKeys.E;
                case KeyEvent.VK_F:
                    return BKeys.F;
                case KeyEvent.VK_G:
                    return BKeys.G;
                case KeyEvent.VK_H:
                    return BKeys.H;
                case KeyEvent.VK_I:
                    return BKeys.I;
                case KeyEvent.VK_J:
                    return BKeys.J;
                case KeyEvent.VK_K:
                    return BKeys.K;
                case KeyEvent.VK_L:
                    return BKeys.L;
                case KeyEvent.VK_M:
                    return BKeys.M;
                case KeyEvent.VK_N:
                    return BKeys.N;
                case KeyEvent.VK_O:
                    return BKeys.O;
                case KeyEvent.VK_P:
                    return BKeys.P;
                case KeyEvent.VK_Q:
                    return BKeys.Q;
                case KeyEvent.VK_R:
                    return BKeys.R;
                case KeyEvent.VK_S:
                    return BKeys.S;
                case KeyEvent.VK_T:
                    return BKeys.T;
                case KeyEvent.VK_U:
                    return BKeys.U;
                case KeyEvent.VK_V:
                    return BKeys.V;
                case KeyEvent.VK_W:
                    return BKeys.W;
                case KeyEvent.VK_X:
                    return BKeys.X;
                case KeyEvent.VK_Y:
                    return BKeys.Y;
                case KeyEvent.VK_Z:
                    return BKeys.Z;
                case KeyEvent.VK_OPEN_BRACKET:
                    return BKeys.LWin;
                case KeyEvent.VK_BACK_SLASH:
                    return BKeys.RWin;
                case KeyEvent.VK_CLOSE_BRACKET:
                    return BKeys.Apps;
                case KeyEvent.VK_NUMPAD0:
                    return BKeys.NumPad0;
                case KeyEvent.VK_NUMPAD1:
                    return BKeys.NumPad1;
                case KeyEvent.VK_NUMPAD2:
                    return BKeys.NumPad2;
                case KeyEvent.VK_NUMPAD3:
                    return BKeys.NumPad3;
                case KeyEvent.VK_NUMPAD4:
                    return BKeys.NumPad4;
                case KeyEvent.VK_NUMPAD5:
                    return BKeys.NumPad5;
                case KeyEvent.VK_NUMPAD6:
                    return BKeys.NumPad6;
                case KeyEvent.VK_NUMPAD7:
                    return BKeys.NumPad7;
                case KeyEvent.VK_NUMPAD8:
                    return BKeys.NumPad8;
                case KeyEvent.VK_NUMPAD9:
                    return BKeys.NumPad9;
                case KeyEvent.VK_MULTIPLY:
                    return BKeys.Multiply;
                case KeyEvent.VK_ADD:
                    return BKeys.Add;
                case KeyEvent.VK_SEPARATOR:
                    return BKeys.Separator;
                case KeyEvent.VK_SUBTRACT:
                    return BKeys.Subtract;
                case KeyEvent.VK_DECIMAL:
                    return BKeys.Decimal;
                case KeyEvent.VK_DIVIDE:
                    return BKeys.Divide;
                case KeyEvent.VK_F1:
                    return BKeys.F1;
                case KeyEvent.VK_F2:
                    return BKeys.F2;
                case KeyEvent.VK_F3:
                    return BKeys.F3;
                case KeyEvent.VK_F4:
                    return BKeys.F4;
                case KeyEvent.VK_F5:
                    return BKeys.F5;
                case KeyEvent.VK_F6:
                    return BKeys.F6;
                case KeyEvent.VK_F7:
                    return BKeys.F7;
                case KeyEvent.VK_F8:
                    return BKeys.F8;
                case KeyEvent.VK_F9:
                    return BKeys.F9;
                case KeyEvent.VK_F10:
                    return BKeys.F10;
                case KeyEvent.VK_F11:
                    return BKeys.F11;
                case KeyEvent.VK_F12:
                    return BKeys.F12;
                case KeyEvent.VK_DELETE:
                    return BKeys.F16;
                case KeyEvent.VK_DEAD_GRAVE:
                    return BKeys.F17;
                case KeyEvent.VK_DEAD_ACUTE:
                    return BKeys.F18;
                case KeyEvent.VK_DEAD_CIRCUMFLEX:
                    return BKeys.F19;
                case KeyEvent.VK_DEAD_TILDE:
                    return BKeys.F20;
                case KeyEvent.VK_DEAD_MACRON:
                    return BKeys.F21;
                case KeyEvent.VK_DEAD_BREVE:
                    return BKeys.F22;
                case KeyEvent.VK_DEAD_ABOVEDOT:
                    return BKeys.F23;
                case KeyEvent.VK_DEAD_DIAERESIS:
                    return BKeys.F24;
                case KeyEvent.VK_NUM_LOCK:
                    return BKeys.NumLock;
                case KeyEvent.VK_SCROLL_LOCK:
                    return BKeys.Scroll;
                case KeyEvent.VK_GREATER:
                    return BKeys.LShiftKey;
                case KeyEvent.VK_BRACELEFT:
                    return BKeys.RShiftKey;
                case KeyEvent.VK_BRACERIGHT:
                    return BKeys.LControlKey;
                case KeyEvent.VK_BACK_QUOTE:
                    return BKeys.Oemtilde;
                case KeyEvent.VK_QUOTE:
                    return BKeys.OemQuotes;
            }
            return BKeys.None;
        }
        #endregion

        #region Graphics extension
        public static void drawBezier( Graphics2D g, float x1, float y1,
                           float ctrlx1, float ctrly1,
                           float ctrlx2, float ctrly2,
                           float x2, float y2 ) {
#if JAVA
            g.draw( new CubicCurve2D.Float( x1, y1, ctrlx1, ctrly1, ctrlx2, ctrly2, x2, y2 ) );
#else
            Stroke stroke = g.getStroke();
            System.Drawing.Pen pen = null;
            if ( stroke is BasicStroke ) {
                pen = ((BasicStroke)stroke).pen;
            } else {
                pen = new System.Drawing.Pen( System.Drawing.Color.Black );
            }
            g.nativeGraphics.DrawBezier( pen, new System.Drawing.PointF( x1, y1 ),
                                              new System.Drawing.PointF( ctrlx1, ctrly1 ),
                                              new System.Drawing.PointF( ctrlx2, ctrly2 ),
                                              new System.Drawing.PointF( x2, y2 ) );
#endif
        }

        public static void drawStringEx( Graphics2D g, String s, Font font, Rectangle rect, int align, int valign ) {
#if JAVA
            g.setFont( font );
            FontMetrics fm = g.getFontMetrics();
            Dimension ret = new Dimension( fm.stringWidth( s ), fm.getHeight() );
            float x = 0;
            float y = 0;
            if( align > 0 ){
                x = rect.x + rect.width - ret.width;
            }else if( align < 0 ){
                x = rect.x;
            }else{
                x = rect.x + rect.width / 2.0f - ret.width / 2.0f;
            }
            if( valign > 0 ){
                y = rect.y + rect.height - ret.height;
            }else if( valign < 0 ){
                y = rect.y;
            }else{
                y = rect.y + rect.height / 2.0f - ret.height / 2.0f;
            }
            g.drawString( s, x, y );
#else
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
            if ( align > 0 ) {
                sf.Alignment = System.Drawing.StringAlignment.Far;
            } else if ( align < 0 ) {
                sf.Alignment = System.Drawing.StringAlignment.Near;
            } else {
                sf.Alignment = System.Drawing.StringAlignment.Center;
            }
            if ( valign > 0 ) {
                sf.LineAlignment = System.Drawing.StringAlignment.Far;
            } else if ( valign < 0 ) {
                sf.LineAlignment = System.Drawing.StringAlignment.Near;
            } else {
                sf.LineAlignment = System.Drawing.StringAlignment.Center;
            }
            g.nativeGraphics.DrawString( s, font.font, g.brush, new System.Drawing.RectangleF( rect.x, rect.y, rect.width, rect.height ), sf );
#endif
        }
        #endregion

        #region System.IO
        public static long getFileLength( String fpath ) {
#if JAVA
            File f = new File( fpath );
            return f.length();
#else
            return new FileInfo( fpath ).Length;
#endif
        }

        public static String getExtension( String fpath ) {
#if JAVA
            String name = (new File( fpath )).getName();
            int index = name.lastIndexOf( '.' );
            if( index <= 0 ){
                return "";
            }else{
                return name.substring( index );
            }
#else
            return Path.GetExtension( fpath );
#endif
        }

        public static String getFileName( String path ) {
#if JAVA
            File f = new File( path );
            return f.getName();
#else
            return Path.GetFileName( path );
#endif
        }

        public static String getDirectoryName( String path ) {
#if JAVA
            File f = new File( path );
            return f.getParent();
#else
            return System.IO.Path.GetDirectoryName( path );
#endif
        }

        public static String getFileNameWithoutExtension( String path ) {
#if JAVA
            String file = getFileName( path );
            int index = file.lastIndexOf( file );
            if( index > 0 ){
                file = file.substring( 0, index );
            }
            return file;
#else
            return System.IO.Path.GetFileNameWithoutExtension( path );
#endif
        }

        public static String createTempFile() {
#if JAVA
            String ret = "";
            try{
                File.createTempFile( "tmp", "" ).getAbsolutePath();
            }catch( Exception ex ){
                System.out.println( "PortUtil#createTempFile; ex=" + ex );
            }
            return ret;
#else
            return System.IO.Path.GetTempFileName();
#endif
        }

        public static String[] listFiles( String directory, String extension ) {
#if JAVA
            File f = new File( directory );
            File[] list = f.listFiles();
            Vector<String> ret = new Vector<String>();
            for( int i = 0; i < list.length; i++ ){
                File t = list[i];
                if( !t.isDirectory() ){
                    String name = t.getName();
                    if( name.endsWith( extension ) ){
                        ret.add( name );
                    }
                }
            }
            return ret.toArray( new String[]{} );
#else
            return System.IO.Directory.GetFiles( directory, "*" + extension );
#endif
        }

        public static void deleteFile( String path ) {
#if JAVA
            new File( path ).delete();
#else
            System.IO.File.Delete( path );
#endif
        }

        public static boolean isDirectoryExists( String path ) {
#if JAVA
            File f = new File( path );
            if( f.exists() ){
                if( f.isFile() ){
                    return true;
                }else{
                    return false;
                }
            }else{
                return false;
            }
#else
            return Directory.Exists( path );
#endif
        }

        public static boolean isFileExists( String path ) {
#if JAVA
            return (new File( path )).exists();
#else
            return System.IO.File.Exists( path );
#endif
        }

        public static String combinePath( String path1, String path2 ) {
#if JAVA
            if( path1.endsWith( File.separator ) ){
                path1 = path1.substring( 0, path1.length() - 1 );
            }
            if( path2.startsWith( File.separator ) ){
                path2 = path2.substring( 1 );
            }
            return path1 + File.separator + path2;
#else
            return System.IO.Path.Combine( path1, path2 );
#endif
        }

        public static String getTempPath() {
#if JAVA
            return System.getProperty( "java.io.tmpdir" );
#else
            return Path.GetTempPath();
#endif
        }

        public static void createDirectory( String path ) {
#if JAVA
            File f = new File( path );
            f.mkdir();
#else
            Directory.CreateDirectory( path );
#endif
        }

        public static void deleteDirectory( String path, boolean recurse ) {
#if JAVA
            File f = new File( path );
            File[] list = f.listFiles();
            for( int i = 0; i < list.length; i++ ){
                File f0 = new File( combinePath( path, list[i].getName() ) );
                if( f0.isDirectory() ){
                    deleteDirectory( f0.getPath(), true );
                }else{
                    f0.delete();
                }
            }
#else
            Directory.Delete( path, recurse );
#endif
        }

        public static void deleteDirectory( String path ) {
#if JAVA
            (new File( path )).delete();
#else
            Directory.Delete( path );
#endif
        }

        public static void copyFile( String file1, String file2 )
#if JAVA
            throws FileNotFoundException, IOException
#endif
 {
#if JAVA
            FileChannel sourceChannel = new FileInputStream( new File( file1 ) ).getChannel();
            FileChannel destinationChannel = new FileOutputStream( new File( file2 ) ).getChannel();
            sourceChannel.transferTo( 0, sourceChannel.size(), destinationChannel );
            sourceChannel.close();
            destinationChannel.close();
#else
            File.Copy( file1, file2 );
#endif
        }
        #endregion

        #region Number Formatting
        public static boolean tryParseInt( String s, ByRef<Integer> value ) {
            try {
                value.value = parseInt( s );
            } catch ( Exception ex ) {
                return false;
            }
            return true;
        }

        public static boolean tryParseFloat( String s, ByRef<Float> value ) {
            try {
                value.value = parseFloat( s );
            } catch ( Exception ex ) {
                return false;
            }
            return true;
        }

        public static int parseInt( String value ) {
#if JAVA
            return Integer.parseInt( value );
#else
            return int.Parse( value );
#endif
        }

        public static float parseFloat( String value ) {
#if JAVA
            return Float.parseFloat( value );
#else
            return float.Parse( value );
#endif
        }

        public static double parseDouble( String value ) {
#if JAVA
            return Double.parseDouble( value );
#else
            return double.Parse( value );
#endif
        }

        public static String formatDecimal( String format, double value ) {
#if JAVA
            DecimalFormat df = new DecimalFormat( format );
            return df.format( value );
#else
            return value.ToString( format );
#endif
        }

        public static String formatDecimal( String format, long value ) {
#if JAVA
            DecimalFormat df = new DecimalFormat( format );
            return df.format( value );
#else
            return value.ToString( format );
#endif
        }

        public static String toHexString( long value, int digits ) {
            String ret = toHexString( value );
            int add = 4 - getStringLength( ret );
            for ( int i = 0; i < add; i++ ) {
                ret = "0" + ret;
            }
            return ret;
        }

        public static String toHexString( long value ) {
#if JAVA
            return Long.toHexString( value );
#else
            return Convert.ToString( value, 16 );
#endif
        }

        public static long fromHexString( String s ) {
#if JAVA
            return Long.parseLong( s, 16 );
#else
            return Convert.ToInt64( s, 16 );
#endif
        }
        #endregion

        #region String Utility
#if JAVA
        public static String[] splitString( String s, char... separator ) {
#else
        public static String[] splitString( String s, params char[] separator ) {
#endif
            return splitStringCorB( s, separator, int.MaxValue, false );
        }

        public static String[] splitString( String s, char[] separator, int count ) {
            return splitStringCorB( s, separator, count, false );
        }

        public static String[] splitString( String s, char[] separator, boolean ignore_empty_entries ) {
            return splitStringCorB( s, separator, int.MaxValue, ignore_empty_entries );
        }

        public static String[] splitString( String s, String[] separator, boolean ignore_empty_entries ) {
            return splitStringCorA( s, separator, int.MaxValue, ignore_empty_entries );
        }

        public static String[] splitString( String s, char[] separator, int count, boolean ignore_empty_entries ) {
            return splitStringCorB( s, separator, count, ignore_empty_entries );
        }

        public static String[] splitString( String s, String[] separator, int count, boolean ignore_empty_entries ) {
            return splitStringCorA( s, separator, count, ignore_empty_entries );
        }

        private static String[] splitStringCorB( String s, char[] separator, int count, boolean ignore_empty_entries ) {
#if JAVA
            int length = separator.length;
#else
            int length = separator.Length;
#endif
            String[] spl = new String[length];
            for ( int i = 0; i < length; i++ ) {
                spl[i] = separator[i] + "";
            }
            return splitStringCorA( s, spl, count, false );
        }

        private static String[] splitStringCorA( String s, String[] separator, int count, boolean ignore_empty_entries ) {
#if JAVA
            if( separator.length == 0 ){
                return new String[]{ s };
            }
            Vector<String> ret = new Vector<String>();
            String remain = s;
            int len = separator.length;
            int index = remain.indexOf( separator[0] );
            int i = 1;
            while( index < 0 && i < separator.length ){
                index = remain.indexOf( separator[i] );
            }
            int added_count = 0;
            while( index >= 0 ){
                if( !ignore_empty_entries || (ignore_empty_entries && index > 0) ){
                    if( added_count + 1 == count ){
                        break;
                    }else{
                        ret.add( remain.substring( 0, index ) );
                    }
                    added_count++;
                }
                remain = remain.substring( index + len );
                index = remain.indexOf( separator[0] );
                i = 1;
                while( index < 0 && i < separator.length ){
                    index = remain.indexOf( separator[i] );
                }
            }
            if( !ignore_empty_entries || (ignore_empty_entries && remain.length() > 0) ){
                ret.add( remain );
            }
            return ret.toArray( new String[]{} );
#else
            return s.Split( separator, count, (ignore_empty_entries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None) );
#endif
        }

        public static int getStringLength( String s ) {
            if ( s == null ) {
                return 0;
            } else {
#if JAVA
                return s.length();
#else
                return s.Length;
#endif
            }
        }

        public static int getEncodedByteCount( String encoding, String str ) {
            byte[] buf = getEncodedByte( encoding, str );
#if JAVA
            return buf.length;
#else
            return buf.Length;
#endif
        }

        public static byte[] getEncodedByte( String encoding, String str ) {
#if JAVA
            Charset enc = Charset.forName( encoding );
            ByteBuffer bb = enc.encode( str );
            byte[] dat = new byte[bb.limit()];
            bb.get( dat );
            return dat;
#else
            Encoding enc = Encoding.GetEncoding( encoding );
            return enc.GetBytes( str );
#endif
        }

        public static String getDecodedString( String encoding, byte[] data, int offset, int length ) {
#if JAVA
            Charset enc = Charset.forName( encoding );
            ByteBuffer bb = ByteBuffer.allocate( length );
            bb.put( data, offset, length );
            return enc.decode( bb ).toString();
#else
            Encoding enc = Encoding.GetEncoding( encoding );
            return enc.GetString( data, offset, length );
#endif
        }

        public static String getDecodedString( String encoding, byte[] data ) {
#if JAVA
            return getDecodedString( encoding, data, 0, data.length );
#else
            return getDecodedString( encoding, data, 0, data.Length );
#endif
        }

        #endregion

        public static Point getMousePosition() {
#if JAVA
            return MouseInfo.getPointerInfo().getLocation();
#else
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            return new Point( p.X, p.Y );
#endif
        }

#if JAVA
        public static Rectangle getWorkingArea( Window w ){
            return w.getGraphicsConfiguration().getBounds();
#else
        public static Rectangle getWorkingArea( System.Windows.Forms.Form w ) {
            System.Drawing.Rectangle r = System.Windows.Forms.Screen.GetWorkingArea( w );
            return new Rectangle( r.X, r.Y, r.Width, r.Height );
#endif
        }

        public static String getMD5( String file )
#if JAVA
            throws FileNotFoundException, IOException
#endif
        {
#if JAVA
            InputStream in = new FileInputStream( file );
            MessageDigest digest = null;
            try {
                digest = MessageDigest.getInstance("MD5");
                byte[] buff = new byte[4096];
                int len = 0;
                while ((len = in.read(buff, 0, buff.length)) >= 0) {
                    digest.update(buff, 0, len);
                }
            } catch (IOException e) {
                throw e;
            } catch( NoSuchAlgorithmException ex2 ){
                System.out.println( "PortUtil#getMD5; ex2=" + ex2 );
            } finally {
                if (in != null) {
                    try {
                        in.close();
                    } catch (IOException e) {
                    }
                }
            }
            byte[] dat = digest.digest();
            String ret = "";
            for( int i = 0; i < dat.length; i++ ){
                ret += String.format( "%02x", dat[i] );
            }
            return ret;
#else
            String ret = "";
            using ( FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read ) ) {
                ret = Misc.getmd5( fs );
            }
            return ret;
#endif
        }


#if JAVA
        class FileFilterImp implements FileFilter{
            private String m_extension;

            public FileFilterImp( String extension ){
                m_extension = extension;
            }

            public boolean accept( File f ){
                String file = f.getName();
                if( file.endsWith( m_extension ) ){
                    return true;
                }else{
                    return false;
                }
            }
        }
#endif

        #region Array conversion
        public static Integer[] convertIntArray( int[] arr ) {
#if JAVA
            Integer[] ret = new Integer[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
#else
            return arr;
#endif
        }

        public static Long[] convertLongArray( long[] arr ) {
#if JAVA
            Long[] ret = new Long[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
#else
            return arr;
#endif
        }

        public static Byte[] convertByteArray( byte[] arr ) {
#if JAVA
            Byte[] ret = new Byte[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
#else
            return arr;
#endif
        }

#if JAVA
        public static int[] convertIntArray( Integer[] arr ){
            int[] ret = new int[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
        }

        public static long[] convertLongArray( Long[] arr ){
            long[] ret = new long[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
        }

        public static byte[] convertByteArray( Byte[] arr ){
            byte[] ret = new byte[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
        }
#endif
        #endregion

        public static String getApplicationStartupPath() {
#if JAVA
            return System.getProperty( "user.dir" );
#else
            return System.Windows.Forms.Application.StartupPath;
#endif
        }

        public static void println( String s ) {
#if JAVA
            System.out.println( s );
#else
            Console.WriteLine( s );
#endif
        }
    }

#if !JAVA
}
#endif
