/*
 * MidiOutDevice.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.media.
 *
 * cadencii.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using cadencii;

namespace cadencii.media
{

    public unsafe class MidiOutDevice
    {
        private IntPtr m_handle;
        private uint m_device_id;

        public MidiOutDevice(uint device_id)
        {
            m_device_id = device_id;
            win32.midiOutOpen(ref m_handle, m_device_id, null, 0, win32.CALLBACK_NULL);
        }

        public void Close()
        {
            if (!m_handle.Equals(IntPtr.Zero)) {
                win32.midiOutClose(m_handle);
            }
        }

        public void ProgramChange(byte channel, byte program)
        {
            SendShort(new byte[] { (byte)(0xc0 | (channel & 0x0f)), program, 0x0 });
        }

        public void Play(byte channel, byte note, byte velocity)
        {
            SendShort(new byte[] { (byte)(0x90 | (channel & 0x0f)), note, velocity });
        }

        public void SendData(byte[] data)
        {
            if (0 < data.Length && data.Length <= 4) {
                SendShort(data);
            } else {
                SendLong(data);
            }
        }

        private void SendShort(byte[] data)
        {
            uint message = 0;
            for (int i = 0; i < data.Length; i++) {
                message |= ((uint)data[i]) << (i * 8);
            }
            win32.midiOutShortMsg(m_handle, message);
        }

#if !MONO
        /*private void SendLong_old( byte[] data ) {
            MIDIHDR hdr = new MIDIHDR();
            GCHandle dataHandle = GCHandle.Alloc( data, GCHandleType.Pinned ); // monoでコンパイルできない
            uint size = (uint)sizeof( MIDIHDR );
            try {
                hdr.lpData = (byte*)dataHandle.AddrOfPinnedObject().ToPointer();
                hdr.dwBufferLength = (uint)data.Length;
                hdr.dwFlags = 0;
                win32.midiOutPrepareHeader( m_handle, ref hdr, size );
                while ( (hdr.dwFlags & win32.WHDR_PREPARED) != win32.WHDR_PREPARED ) {
                    Application.DoEvents();
                }
                win32.midiOutLongMsg( m_handle, ref hdr, size );
                while ( (hdr.dwFlags & win32.WHDR_DONE) != win32.WHDR_DONE ) {
                    Application.DoEvents();
                }
                win32.midiOutUnprepareHeader( m_handle, ref hdr, size );
            } finally {
                dataHandle.Free();
            }
        }*/
#endif

        private void SendLong(byte[] data)
        {
#if !MONO
            IntPtr ptr = IntPtr.Zero;
            IntPtr ptrData = IntPtr.Zero;
            try {
                uint size = (uint)sizeof(MIDIHDR);
                ptr = Marshal.AllocHGlobal((int)size);
                MIDIHDR hdr = (MIDIHDR)Marshal.PtrToStructure(ptr, typeof(MIDIHDR));
                ptrData = Marshal.AllocHGlobal(data.Length);
                byte* pData = (byte*)ptrData.ToPointer();
                for (int i = 0; i < data.Length; i++) {
                    pData[i] = data[i];
                }
                hdr.lpData = pData;
                hdr.dwBufferLength = (uint)data.Length;
                hdr.dwFlags = 0;
                win32.midiOutPrepareHeader(m_handle, ref hdr, size);
                while ((hdr.dwFlags & win32.WHDR_PREPARED) != win32.WHDR_PREPARED) {
                    Application.DoEvents();
                }
                win32.midiOutLongMsg(m_handle, ref hdr, size);
                while ((hdr.dwFlags & win32.WHDR_DONE) != win32.WHDR_DONE) {
                    Application.DoEvents();
                }
                win32.midiOutUnprepareHeader(m_handle, ref hdr, size);
            } catch (Exception ex) {
                serr.println("MidiOutDevice#SendLong; ex=" + ex);
            } finally {
                if (ptrData != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptrData);
                }
                if (ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
#endif
        }
    }

}
