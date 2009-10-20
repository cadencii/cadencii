/*
 * MidiInDevice.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Media.
 *
 * Boare.Lib.Media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using bocoree;

namespace Boare.Lib.Media {

    public delegate void MidiReceivedEventHandler( DateTime time, byte[] data );

    public class MidiInDevice : IDisposable {
        delegate void MidiInProcDelegate( uint hMidiIn, uint wMsg, int dwInstance, int dwParam1, int dwParam2 );

        private volatile MidiInProcDelegate m_delegate;
        private IntPtr m_delegate_pointer;
        private uint m_hmidiin = 0;
        private int m_port_number;

        public event MidiReceivedEventHandler MidiReceived;

        public MidiInDevice( int port_number ) {
            m_port_number = port_number;
            m_delegate = new MidiInProcDelegate( MidiInProc );
            m_delegate_pointer = Marshal.GetFunctionPointerForDelegate( m_delegate );
            win32.midiInOpen( ref m_hmidiin, port_number, m_delegate_pointer, 0, win32.CALLBACK_FUNCTION );
        }

        public void Start() {
            if ( m_hmidiin > 0 ) {
                try {
                    win32.midiInStart( m_hmidiin );
                } catch ( Exception ex ) {
                    debug.push_log( "MidiInDevice.Start" );
                    debug.push_log( "    ex=" + ex );
                }
            }
        }

        public void Stop() {
            if ( m_hmidiin > 0 ) {
                try {
                    win32.midiInReset( m_hmidiin );
                } catch ( Exception ex ) {
                    debug.push_log( "MidiInDevice.Stop" );
                    debug.push_log( "    ex=" + ex );
                }
            }
        }

        public void Close() {
            if ( m_hmidiin > 0 ) {
                try {
                    win32.midiInClose( m_hmidiin );
                } catch ( Exception ex ) {
                    debug.push_log( "MidiInDevice.Close" );
                    debug.push_log( "    ex=" + ex );
                }
            }
            m_hmidiin = 0;
        }

        public void Dispose() {
            Close();
        }

        public static int GetNumDevs() {
            try {
                int i = (int)win32.midiInGetNumDevs();
                return i;
            } catch ( Exception ex ) {
                debug.push_log( "MidiInDevice.GetNumDevs" );
                debug.push_log( "    ex=" + ex );
            }
            return 0;
        }

        public static MIDIINCAPS[] GetMidiInDevices() {
            List<MIDIINCAPS> ret = new List<MIDIINCAPS>();
            uint num = 0;
            try {
                num = win32.midiInGetNumDevs();
            } catch {
                num = 0;
            }
            for ( uint i = 0; i < num; i++ ) {
                MIDIINCAPS m = new MIDIINCAPS();
                uint r = win32.midiInGetDevCaps( i, ref m, (uint)Marshal.SizeOf( m ) );
                ret.Add( m );
            }
            return ret.ToArray();
        }

        public void MidiInProc( uint hMidiIn, uint wMsg, int dwInstance, int dwParam1, int dwParam2 ) {
            try {
                switch ( wMsg ) {
                    case win32.MM_MIM_OPEN:
                        return;
                    case win32.MM_MIM_CLOSE:
                        return;
                    case win32.MM_MIM_DATA:
                        int receive = dwParam1;
                        DateTime now = DateTime.Now;
                        switch ( receive & 0xF0 ) {
                            case 0x80:
                            case 0x90:
                            case 0xa0:
                            case 0xb0:
                            case 0xe0:
                                if ( MidiReceived != null ) {
                                    MidiReceived( now, new byte[] { (byte)(receive & 0xff),
                                                                    (byte)((receive & 0xffff) >> 8),
                                                                    (byte)((receive & ((2 << 24) - 1)) >> 16) } );
                                }
                                break;
                            case 0xc0:
                            case 0xd0:
                                if ( MidiReceived != null ) {
                                    MidiReceived( now, new byte[] { (byte)( receive & 0xff ),
                                                                    (byte)((receive & 0xffff) >> 8) } );
                                }
                                break;
                        }
                        return;
                    case win32.MM_MIM_LONGDATA:
                        return;
                    case win32.MM_MIM_ERROR:
                        return;
                    case win32.MM_MIM_LONGERROR:
                        return;
                }
            } catch ( Exception ex ) {
                debug.push_log( "MidiInDevice.MidiInProc" );
                debug.push_log( "    ex=" + ex );
            }
        }
    }

}
