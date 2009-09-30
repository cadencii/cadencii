/*
 * MidiOutDevice.cs
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
using System.Windows.Forms;

using bocoree;

namespace Boare.Lib.Media {

    public unsafe class MidiOutDevice {
        private IntPtr m_handle;
        private uint m_device_id;

        public MidiOutDevice( uint device_id ) {
            m_device_id = device_id;
            windows.midiOutOpen( ref m_handle, m_device_id, null, 0, windows.CALLBACK_NULL );
        }

        public void Close() {
            if ( !m_handle.Equals( IntPtr.Zero ) ) {
                windows.midiOutClose( m_handle );
            }
        }

        public void ProgramChange( byte channel, byte program ) {
            SendShort( new byte[] { (byte)(0xc0 | (channel & 0x0f)) , program, 0x0 } );
        }

        public void Play( byte channel, byte note, byte velocity ) {
            SendShort( new byte[] { (byte)(0x90 | (channel & 0x0f)), note, velocity } );
        }

        public void SendData( byte[] data ) {
            if ( 0 < data.Length && data.Length <= 4 ) {
                SendShort( data );
            } else {
                SendLong( data );
            }
        }

        private void SendShort( byte[] data ) {
            uint message = 0;
            for ( int i = 0; i < data.Length; i++ ) {
                message |= ((uint)data[i]) << (i * 8);
            }
            windows.midiOutShortMsg( m_handle, message );
        }

        private void SendLong( byte[] data ) {
            MIDIHDR hdr = new MIDIHDR();
            GCHandle dataHandle = GCHandle.Alloc( data, GCHandleType.Pinned );
            uint size = (uint)sizeof( MIDIHDR );
            try {
                hdr.lpData = (byte*)dataHandle.AddrOfPinnedObject().ToPointer();
                hdr.dwBufferLength = (uint)data.Length;
                hdr.dwFlags = 0;
                windows.midiOutPrepareHeader( m_handle, ref hdr, size );
                while ( (hdr.dwFlags & windows.WHDR_PREPARED) != windows.WHDR_PREPARED ) {
                    Application.DoEvents();
                }
                windows.midiOutLongMsg( m_handle, ref hdr, size );
                while ( (hdr.dwFlags & windows.WHDR_DONE) != windows.WHDR_DONE ) {
                    Application.DoEvents();
                }
                windows.midiOutUnprepareHeader( m_handle, ref hdr, size );
            } finally {
                dataHandle.Free();
            }
        }
    }

}
