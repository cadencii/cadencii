/*
 * BKeysUtility.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.windows.forms.
 *
 * org.kbinani.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package com.github.cadencii.windows.forms;

import java.awt.event.*;
import javax.swing.*;
import java.util.*;
#else
using com.github.cadencii.java.util;
using com.github.cadencii.java.awt.event_;
using com.github.cadencii.javax.swing;

namespace com.github.cadencii.windows.forms{
#endif

    public class BKeysUtility{
        private BKeysUtility(){
        }

        #region System.Windows.Forms.Keys and org.kbinani.windows.forms.BKeys
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
                } else if ( keys[i] == BKeys.Menu ){
                    modifier += InputEvent.META_MASK;
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
            if ( (modifier & InputEvent.META_MASK) == InputEvent.META_MASK ) {
                ret.add( BKeys.Menu );
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

    }

#if !JAVA
}
#endif
