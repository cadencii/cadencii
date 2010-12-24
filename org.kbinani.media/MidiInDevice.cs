/*
 * MidiInDevice.cs
 * Copyright © 2009-2010 kbinani
 *
 * This file is part of org.kbinani.media.
 *
 * org.kbinani.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.media;

public class MidiInDevice{

}
#else
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using org.kbinani;

namespace org.kbinani.media {
    using boolean = System.Boolean;

    public delegate void MidiReceivedEventHandler( double time, byte[] data );

    public class MidiInDevice : IDisposable {
        delegate void MidiInProcDelegate( uint hMidiIn, uint wMsg, int dwInstance, int dwParam1, int dwParam2 );

        private volatile MidiInProcDelegate m_delegate;
        private volatile IntPtr m_delegate_pointer;
        private uint m_hmidiin = 0;
        private int m_port_number;
        private boolean receiveSystemCommonMessage = false;
        private boolean receiveSystemRealtimeMessage = false;
        
        public event MidiReceivedEventHandler MidiReceived;

        public MidiInDevice( int port_number ) {
            m_port_number = port_number;
            m_delegate = new MidiInProcDelegate( MidiInProc );
            m_delegate_pointer = Marshal.GetFunctionPointerForDelegate( m_delegate );
            win32.midiInOpen( ref m_hmidiin, port_number, m_delegate_pointer, 0, win32.CALLBACK_FUNCTION );
        }

        public boolean isReceiveSystemRealtimeMessage() {
            return receiveSystemRealtimeMessage;
        }

        public void setReceiveSystemRealtimeMessage( boolean value ) {
            receiveSystemRealtimeMessage = value;
        }

        public boolean isReceiveSystemCommonMessage() {
            return receiveSystemCommonMessage;
        }

        public void setReceiveSystemCommonMessage( boolean value ) {
            receiveSystemCommonMessage = value;
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
#if DEBUG
                PortUtil.println( "MidiInDevice#GetMidiDevices; #" + i + "; r=" + r + "; m=" + m );
#endif
                ret.Add( m );
            }
            return ret.ToArray();
        }

        public void MidiInProc( uint hMidiIn, uint wMsg, int dwInstance, int dwParam1, int dwParam2 ) {
            try {
                switch ( wMsg ) {
                    case win32.MM_MIM_OPEN: {
                        return;
                    }
                    case win32.MM_MIM_CLOSE: {
                        return;
                    }
                    case win32.MM_MIM_DATA: {
                        int receive = dwParam1;
                        double now = PortUtil.getCurrentTime();
                        switch ( receive & 0xF0 ) {
                            case 0x80:
                            case 0x90:
                            case 0xa0:
                            case 0xb0:
                            case 0xe0: {
                                if ( MidiReceived != null ) {
                                    MidiReceived.Invoke( now, new byte[] { (byte)(receive & 0xff),
                                                                    (byte)((receive & 0xffff) >> 8),
                                                                    (byte)((receive & ((2 << 24) - 1)) >> 16) } );
                                }
                                break;
                            }
                            case 0xc0:
                            case 0xd0: {
                                if ( MidiReceived != null ) {
                                    MidiReceived.Invoke( now, new byte[] { (byte)( receive & 0xff ),
                                                                    (byte)((receive & 0xffff) >> 8) } );
                                }
                                break;
                            }
                            case 0xf0: {
                                if ( receiveSystemCommonMessage ) {
                                    byte b0 = (byte)(receive & 0xff);
                                    byte b1 = (byte)((receive >> 8) & 0xff);
                                    byte b2 = (byte)((receive >> 16) & 0xff);
                                    byte b3 = (byte)((receive >> 24) & 0xff);
                                    if ( b0 == 0xf1 ) {
                                        // MTC quater frame message
                                        if ( MidiReceived != null ) {
                                            MidiReceived.Invoke( now, new byte[] { b0, b1, b2 } );
                                        }
                                    } else if ( b0 == 0xf2 ) {
                                        // song position pointer
#if DEBUG
                                        PortUtil.println( "MidiInDevice#MidiInProc; 0xf2; b0=" + PortUtil.toHexString( b0, 2 ) + "; b1=" + PortUtil.toHexString( b1, 2 ) + "; b2=" + PortUtil.toHexString( b2, 2 ) );
#endif
                                    }
                                }
                                if ( receiveSystemRealtimeMessage && MidiReceived != null ) {
                                    byte b0 = (byte)(receive & 0xff);
                                    byte b1 = (byte)((receive >> 8) & 0xff);
                                    byte b2 = (byte)((receive >> 16) & 0xff);
                                    byte b3 = (byte)((receive >> 24) & 0xff);
                                    if ( b0 == 0xfa ) {
                                        MidiReceived.Invoke( now, new byte[] { b0 } );
                                    } else if ( b0 == 0xfc ) {
                                        MidiReceived.Invoke( now, new byte[] { b0 } );
                                    }
                                }
                                break;
                            }
                        }
                        return;
                    }
                    case win32.MM_MIM_LONGDATA: {
                        return;
                    }
                    case win32.MM_MIM_ERROR: {
                        return;
                    }
                    case win32.MM_MIM_LONGERROR: {
                        return;
                    }
                }
            } catch ( Exception ex ) {
                debug.push_log( "MidiInDevice.MidiInProc" );
                debug.push_log( "    ex=" + ex );
            }
        }
    }

}
#endif
