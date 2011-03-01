/*
 * MidiInDevice.cs
 * Copyright © 2009-2011 kbinani
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

import org.kbinani.*;

#else

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using org.kbinani;

namespace org.kbinani.media
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class MidiInDevice implements javax.sound.midi.Receiver
#else
    public class MidiInDevice : IDisposable 
#endif
    {
#if JAVA
#else
        delegate void MidiInProcDelegate( uint hMidiIn, uint wMsg, int dwInstance, int dwParam1, int dwParam2 );

        private volatile MidiInProcDelegate m_delegate;
        private volatile IntPtr m_delegate_pointer;
        private uint m_hmidiin = 0;
#endif
        private int m_port_number;
        private boolean mReceiveSystemCommonMessage = false;
        private boolean mReceiveSystemRealtimeMessage = false;
        private boolean mIsActive = false;

#if JAVA
        public final BEvent<MidiReceivedEventHandler> midiReceivedEvent = new BEvent<MidiReceivedEventHandler>();
#else         
        public event MidiReceivedEventHandler MidiReceived;
#endif

        public MidiInDevice( int port_number ) {
            m_port_number = port_number;
#if JAVA
            javax.sound.midi.MidiDevice.Info[] info = javax.sound.midi.MidiSystem.getMidiDeviceInfo();
            if( port_number < 0 || info.length <= port_number ){
                System.err.println( "MidiInDevice#.ctor; invalid port-number" );
                return;
            }
            javax.sound.midi.Transmitter trans = null;
            javax.sound.midi.MidiDevice device = null;
            try{
                device = javax.sound.midi.MidiSystem.getMidiDevice( info[port_number] );
            }catch( Exception ex ){
                ex.printStackTrace();
                device = null;
            }
            if( device == null ) return;
            int max = device.getMaxTransmitters();
            if( max != -1 && max <= 0 ){
                System.err.println( "MidiInDevice#.ctor; cannot connect to device" );
                return;
            }
            if( !device.isOpen() ){
                try{
                    device.open();
                }catch( Exception ex ){
                    ex.printStackTrace();
                    return;
                }
            }
            try{
                trans = device.getTransmitter();
            }catch( Exception ex ){
                ex.printStackTrace();
            }
            trans.setReceiver( this );
#else
            m_delegate = new MidiInProcDelegate( MidiInProc );
            m_delegate_pointer = Marshal.GetFunctionPointerForDelegate( m_delegate );
            win32.midiInOpen( ref m_hmidiin, port_number, m_delegate_pointer, 0, win32.CALLBACK_FUNCTION );
#endif
        }

        public boolean isReceiveSystemRealtimeMessage() {
            return mReceiveSystemRealtimeMessage;
        }

        public void setReceiveSystemRealtimeMessage( boolean value ) {
            mReceiveSystemRealtimeMessage = value;
        }

        public boolean isReceiveSystemCommonMessage() {
            return mReceiveSystemCommonMessage;
        }

        public void setReceiveSystemCommonMessage( boolean value ) {
            mReceiveSystemCommonMessage = value;
        }

        public void start() {
            mIsActive = true;
#if !JAVA
            if ( m_hmidiin > 0 ) {
                try {
                    win32.midiInStart( m_hmidiin );
                } catch ( Exception ex ) {
                    debug.push_log( "MidiInDevice.Start" );
                    debug.push_log( "    ex=" + ex );
                }
            }
#endif
        }

        public void stop() {
            mIsActive = false;
#if !JAVA
            if ( m_hmidiin > 0 ) {
                try {
                    win32.midiInReset( m_hmidiin );
                } catch ( Exception ex ) {
                    debug.push_log( "MidiInDevice.Stop" );
                    debug.push_log( "    ex=" + ex );
                }
            }
#endif
        }

        public void close() {
            mIsActive = false;
#if !JAVA
            if ( m_hmidiin > 0 ) {
                try {
                    win32.midiInClose( m_hmidiin );
                } catch ( Exception ex ) {
                    debug.push_log( "MidiInDevice.Close" );
                    debug.push_log( "    ex=" + ex );
                }
            }
            m_hmidiin = 0;
#endif
        }

#if !JAVA
        public void Dispose() {
            close();
        }
#endif

/*
        public static int getNumDevs() {
#if JAVA
            return javax.sound.midi.MidiSystem.getMidiDeviceInfo().length;
#else
            try {
                int i = (int)win32.midiInGetNumDevs();
                return i;
            } catch ( Exception ex ) {
                debug.push_log( "MidiInDevice.GetNumDevs" );
                debug.push_log( "    ex=" + ex );
            }
            return 0;
#endif
        }
*/
        /*public static MIDIINCAPS[] GetMidiInDevices() {
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
                sout.println( "MidiInDevice#GetMidiDevices; #" + i + "; r=" + r + "; m=" + m );
#endif
                ret.Add( m );
            }
            return ret.ToArray();
        }*/

#if JAVA
        public void send( javax.sound.midi.MidiMessage message, long time )
        {
#if DEBUG
            sout.println( "MidiDevice#send; message.getStatus()=" + message.getStatus() );
#endif
            if( !mIsActive ) return;
            int status = message.getStatus();
            if( status >= 0xf8 ){
                if( !mReceiveSystemRealtimeMessage ){
                    return;
                }
            }
            if( status >= 0xf1 ){
                if( !mReceiveSystemCommonMessage ){
                    return;
                }
            }
#if DEBUG
            if( !mIsActive ){
                sout.println( "MidiInDevice#send; return because mIsActive==false" );
            }
#endif
            try{
                midiReceivedEvent.raise( this, message );
            }catch( Exception ex ){
                ex.printStackTrace();
            }
            sout.println( "MidiInDevice#send; message.getStatus()=0x" + PortUtil.toHexString( message.getStatus(), 2 ) );
        }
#endif

#if !JAVA
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
                                    javax.sound.midi.MidiMessage msg = 
                                        new org.kbinani.javax.sound.midi.MidiMessage( 
                                            new byte[] { (byte)(receive & 0xff),
                                                         (byte)((receive & 0xffff) >> 8),
                                                         (byte)((receive & ((2 << 24) - 1)) >> 16) } );
                                    MidiReceived.Invoke( this, msg );
                                }
                                break;
                            }
                            case 0xc0:
                            case 0xd0: {
                                if ( MidiReceived != null ) {
                                    javax.sound.midi.MidiMessage msg =
                                        new org.kbinani.javax.sound.midi.MidiMessage(
                                            new byte[] { (byte)( receive & 0xff ),
                                                         (byte)((receive & 0xffff) >> 8) } );
                                    MidiReceived.Invoke( this, msg );
                                }
                                break;
                            }
                            case 0xf0: {
                                if ( mReceiveSystemCommonMessage ) {
                                    byte b0 = (byte)(receive & 0xff);
                                    byte b1 = (byte)((receive >> 8) & 0xff);
                                    byte b2 = (byte)((receive >> 16) & 0xff);
                                    byte b3 = (byte)((receive >> 24) & 0xff);
                                    if ( b0 == 0xf1 ) {
                                        // MTC quater frame message
                                        if ( MidiReceived != null ) {
                                            javax.sound.midi.MidiMessage msg =
                                                new org.kbinani.javax.sound.midi.MidiMessage( new byte[] { b0, b1, b2 } );
                                            MidiReceived.Invoke( this, msg );
                                        }
                                    } else if ( b0 == 0xf2 ) {
                                        // song position pointer
#if DEBUG
                                        sout.println( "MidiInDevice#MidiInProc; 0xf2; b0=" + PortUtil.toHexString( b0, 2 ) + "; b1=" + PortUtil.toHexString( b1, 2 ) + "; b2=" + PortUtil.toHexString( b2, 2 ) );
#endif
                                    }
                                }
                                if ( mReceiveSystemRealtimeMessage && MidiReceived != null ) {
                                    byte b0 = (byte)(receive & 0xff);
                                    byte b1 = (byte)((receive >> 8) & 0xff);
                                    byte b2 = (byte)((receive >> 16) & 0xff);
                                    byte b3 = (byte)((receive >> 24) & 0xff);
                                    if ( b0 == 0xfa ) {
                                        MidiReceived.Invoke( this, new javax.sound.midi.MidiMessage( new byte[] { b0 } ) );
                                    } else if ( b0 == 0xfc ) {
                                        MidiReceived.Invoke( this, new javax.sound.midi.MidiMessage( new byte[] { b0 } ) );
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
#endif
    }

#if !JAVA
}
#endif
