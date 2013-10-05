/*
 * winmm.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;
using cadencii;

namespace cadencii
{

    using MMRESULT = System.UInt32;
    using HMIDIIN = System.Int32;
    using DWORD = System.UInt32;
    using UINT = System.UInt32;
    using WORD = System.UInt16;
    using BYTE = System.Byte;

    public delegate void delegateWaveOutProc(IntPtr hwo, uint uMsg, uint dwInstance, uint dwParam1, uint dwParam2);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct JOYINFO
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint wXpos;
        [MarshalAs(UnmanagedType.U4)]
        public uint wYpos;
        [MarshalAs(UnmanagedType.U4)]
        public uint wZpos;
        [MarshalAs(UnmanagedType.U4)]
        public uint wButtons;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct JOYCAPSW
    {
        const int MAXPNAMELEN = 32;
        const int MAX_JOYSTICKOEMVXDNAME = 260;
        [MarshalAs(UnmanagedType.U2)]
        public ushort wMid;
        [MarshalAs(UnmanagedType.U2)]
        public ushort wPid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
        public string szPname;
        [MarshalAs(UnmanagedType.U4)]
        public uint wXmin;
        [MarshalAs(UnmanagedType.U4)]
        public uint wXmax;
        [MarshalAs(UnmanagedType.U4)]
        public uint wYmin;
        [MarshalAs(UnmanagedType.U4)]
        public uint wYmax;
        [MarshalAs(UnmanagedType.U4)]
        public uint wZmin;
        [MarshalAs(UnmanagedType.U4)]
        public uint wZmax;
        [MarshalAs(UnmanagedType.U4)]
        public uint wNumButtons;
        [MarshalAs(UnmanagedType.U4)]
        public uint wPeriodMin;
        [MarshalAs(UnmanagedType.U4)]
        public uint wPeriodMax;
        [MarshalAs(UnmanagedType.U4)]
        public uint wRmin;
        [MarshalAs(UnmanagedType.U4)]
        public uint wRmax;
        [MarshalAs(UnmanagedType.U4)]
        public uint wUmin;
        [MarshalAs(UnmanagedType.U4)]
        public uint wUmax;
        [MarshalAs(UnmanagedType.U4)]
        public uint wVmin;
        [MarshalAs(UnmanagedType.U4)]
        public uint wVmax;
        [MarshalAs(UnmanagedType.U4)]
        public uint wCaps;
        [MarshalAs(UnmanagedType.U4)]
        public uint wMaxAxes;
        [MarshalAs(UnmanagedType.U4)]
        public uint wNumAxes;
        [MarshalAs(UnmanagedType.U4)]
        public uint wMaxButtons;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
        public string szRegKey;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_JOYSTICKOEMVXDNAME)]
        public string szOEMVxD;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct JOYINFOEX
    {
        public uint dwSize;
        public uint dwFlags;
        public uint dwXpos;
        public uint dwYpos;
        public uint dwZpos;
        public uint dwRpos;
        public uint dwUpos;
        public uint dwVpos;
        public uint dwButtons;
        public uint dwButtonNumber;
        public uint dwPOV;
        public uint dwReserved1;
        public uint dwReserved2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WAVEFORMATEX
    {
        public ushort wFormatTag;
        public ushort nChannels;
        public uint nSamplesPerSec;
        public uint nAvgBytesPerSec;
        public ushort nBlockAlign;
        public ushort wBitsPerSample;
        public ushort cbSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WAVEHDR
    {
        public IntPtr lpData; // pointer to locked data buffer
        public uint dwBufferLength; // length of data buffer
        public uint dwBytesRecorded; // used for input only
        public IntPtr dwUser; // for client's use
        public uint dwFlags; // assorted flags (see defines)
        public uint dwLoops; // loop control counter
        public IntPtr lpNext; // PWaveHdr, reserved for driver
        public uint reserved; // reserved for driver
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MMTIME
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MIDI
        {
            public DWORD songptrpos;   // song pointer position 
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct SMPTE
        {
            public BYTE hour;       // hours
            public BYTE min;        // minutes
            public BYTE sec;        // seconds
            public BYTE frame;      // frames 
            public BYTE fps;        // frames per second
            public BYTE dummy;      // pad
            public fixed BYTE pad[2];
        }

        [FieldOffset(0)]
        public uint wType;
        [FieldOffset(4)]
        public SMPTE smpte;
        [FieldOffset(4)]
        public MIDI midi;
        [FieldOffset(4)]
        public uint ms;
        [FieldOffset(4)]
        public uint sample;
        [FieldOffset(4)]
        public uint cb;
        [FieldOffset(4)]
        public uint ticks;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MIDIINCAPS
    {
        public ushort wMid;
        public ushort wPid;
        public uint vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = win32.MAXPNAMELEN)]
        public string szPname;
        public uint dwSupport;

        public override string ToString()
        {
            return szPname;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MIDIHDR
    {
        public byte* lpData;
        public DWORD dwBufferLength;
        public DWORD dwBytesRecorded;
        public DWORD dwUser;
        public DWORD dwFlags;
        public MIDIHDR* lpNext;
        public DWORD reserved;
        public DWORD dwOffset;
        public fixed DWORD dwReserved[8];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIOUTCAPSA
    {
        public WORD wMid;
        public WORD wPid;
        public uint vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = win32.MAXPNAMELEN)]
        public string szPname;
        public WORD wTechnology;
        public WORD wVoices;
        public WORD wNotes;
        public WORD wChannelMask;
        public DWORD dwSupport;
    }

    public static partial class win32
    {
        private enum DllStatus
        {
            Unknown,
            Found,
            NotFound,
        }

        public const uint JOYERR_NOERROR = 0;
        public const ushort JOY_RETURNX = 0x00000001;
        public const ushort JOY_RETURNY = 0x00000002;
        public const ushort JOY_RETURNZ = 0x00000004;
        public const ushort JOY_RETURNR = 0x00000008;
        public const ushort JOY_RETURNU = 0x00000010;
        public const ushort JOY_RETURNV = 0x00000020;
        public const ushort JOY_RETURNPOV = 0x00000040;
        public const ushort JOY_RETURNBUTTONS = 0x00000080;
        public const ushort JOY_RETURNRAWDATA = 0x00000100;
        public const ushort JOY_RETURNPOVCTS = 0x00000200;
        public const ushort JOY_RETURNCENTERED = 0x00000400;
        public const ushort JOY_USEDEADZONE = 0x00000800;
        public const ushort JOY_RETURNALL = (JOY_RETURNX | JOY_RETURNY | JOY_RETURNZ |
                                 JOY_RETURNR | JOY_RETURNU | JOY_RETURNV |
                                 JOY_RETURNPOV | JOY_RETURNBUTTONS);
        public const int JOYCAPS_HASZ = 0x0001;
        public const int JOYCAPS_HASR = 0x0002;
        public const int JOYCAPS_HASU = 0x0004;
        public const int JOYCAPS_HASV = 0x0008;
        public const int JOYCAPS_HASPOV = 0x0010;
        public const int JOYCAPS_POV4DIR = 0x0020;
        public const int JOYCAPS_POVCTS = 0x0040;

        public const ushort MM_MCINOTIFY = 0x3B9;          /* MCI */

        public const ushort MM_WOM_OPEN = 0x3BB;         /* waveform output */
        public const ushort MM_WOM_CLOSE = 0x3BC;
        public const ushort MM_WOM_DONE = 0x3BD;

        public const ushort MM_WIM_OPEN = 0x3BE;        /* waveform input */
        public const ushort MM_WIM_CLOSE = 0x3BF;
        public const ushort MM_WIM_DATA = 0x3C0;

        public const ushort MM_MIM_OPEN = 0x3C1;        /* MIDI input */
        public const ushort MM_MIM_CLOSE = 0x3C2;
        public const ushort MM_MIM_DATA = 0x3C3;
        public const ushort MM_MIM_LONGDATA = 0x3C4;
        public const ushort MM_MIM_ERROR = 0x3C5;
        public const ushort MM_MIM_LONGERROR = 0x3C6;

        public const ushort MM_MOM_OPEN = 0x3C7;         /* MIDI output */
        public const ushort MM_MOM_CLOSE = 0x3C8;
        public const ushort MM_MOM_DONE = 0x3C9;

        public const int WAVE_MAPPER = -1;

        public const int WAVE_FORMAT_PCM = 1;

        public const int CALLBACK_TYPEMASK = 0x00070000;    /* callback type mask */
        public const int CALLBACK_NULL = 0x00000000;    /* no callback */
        public const int CALLBACK_WINDOW = 0x00010000;    /* dwCallback is a HWND */
        public const int CALLBACK_TASK = 0x00020000;    /* dwCallback is a HTASK */
        public const int CALLBACK_FUNCTION = 0x00030000;    /* dwCallback is a FARPROC */
        public const int CALLBACK_THREAD = (CALLBACK_TASK);/* thread ID replaces 16 bit task */
        public const int CALLBACK_EVENT = 0x00050000;    /* dwCallback is an EVENT Handle */

        public const int WHDR_DONE = 0x00000001;  /* done bit */
        public const int WHDR_PREPARED = 0x00000002;  /* set if this header has been prepared */
        public const int WHDR_BEGINLOOP = 0x00000004;  /* loop start block */
        public const int WHDR_ENDLOOP = 0x00000008;  /* loop end block */
        public const int WHDR_INQUEUE = 0x00000010;  /* reserved for driver */

        public const int TIME_MS = 0x0001;  /* time in milliseconds */
        public const int TIME_SAMPLES = 0x0002;  /* number of wave samples */
        public const int TIME_BYTES = 0x0004;  /* current byte offset */
        public const int TIME_SMPTE = 0x0008;  /* SMPTE time */
        public const int TIME_MIDI = 0x0010;  /* MIDI time */
        public const int TIME_TICKS = 0x0020;  /* Ticks within MIDI stream */

        public const int MAXPNAMELEN = 32;
        public const int MAX_JOYSTICKOEMVXDNAME = 260;

        public const uint MMSYSERR_NOERROR = 0;
        public const uint MMSYSERR_ERROR = 1;
        public const uint MMSYSERR_BADDEVICEID = 2;
        public const uint MMSYSERR_NOTENABLED = 3;
        public const uint MMSYSERR_ALLOCATED = 4;
        public const uint MMSYSERR_INVALHANDLE = 5;
        public const uint MMSYSERR_NODRIVER = 6;
        public const uint MMSYSERR_NOMEM = 7;
        public const uint MMSYSERR_NOTSUPPORTED = 8;
        public const uint MMSYSERR_BADERRNUM = 9;
        public const uint MMSYSERR_INVALFLAG = 10;
        public const uint MMSYSERR_INVALPARAM = 11;
        public const uint MMSYSERR_HANDLEBUSY = 12;
        public const uint MMSYSERR_INVALIDALIAS = 13;
        public const uint MMSYSERR_BADDB = 14;
        public const uint MMSYSERR_KEYNOTFOUND = 15;
        public const uint MMSYSERR_READERROR = 16;
        public const uint MMSYSERR_WRITEERROR = 17;
        public const uint MMSYSERR_DELETEERROR = 18;
        public const uint MMSYSERR_VALNOTFOUND = 19;
        public const uint MMSYSERR_NODRIVERCB = 20;
        public const uint MMSYSERR_LASTERROR = 20;

        private static DllStatus status_winmm = DllStatus.Unknown;
        private static DllStatus status_winmm_so = DllStatus.Unknown;

        #region midi
        [DllImport("winmm", EntryPoint = "midiInGetDevCaps", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiInGetDevCaps([MarshalAs(UnmanagedType.U4)]uint uDeviceID, ref MIDIINCAPS lpMidiInCaps, [MarshalAs(UnmanagedType.U4)]uint cbMidiInCaps);

        [DllImport("winmm.dll.so", EntryPoint = "midiInGetDevCaps", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiInGetDevCaps([MarshalAs(UnmanagedType.U4)]uint uDeviceID, ref MIDIINCAPS lpMidiInCaps, [MarshalAs(UnmanagedType.U4)]uint cbMidiInCaps);

        public static uint midiInGetDevCaps(uint uDeviceID, ref MIDIINCAPS lpMidiInCaps, uint cbMidiInCaps)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiInGetDevCaps(uDeviceID, ref lpMidiInCaps, cbMidiInCaps);
                    status_winmm = DllStatus.Found;
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiInGetDevCaps(uDeviceID, ref lpMidiInCaps, cbMidiInCaps);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiInClose")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiInClose(uint hMidiIn);

        [DllImport("winmm.dll.so", EntryPoint = "midiInClose")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiInClose(uint hMidiIn);

        public static uint midiInClose(uint hMidiIn)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiInClose(hMidiIn);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiInClose(hMidiIn);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiInStart")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiInStart(uint hMidiIn);

        [DllImport("winmm.dll.so", EntryPoint = "midiInStart")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiInStart(uint hMidiIn);

        public static uint midiInStart(uint hMidiIn)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiInStart(hMidiIn);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiInStart(hMidiIn);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiInReset")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiInReset(uint hMidiIn);

        [DllImport("winmm.dll.so", EntryPoint = "midiInReset")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiInReset(uint hMidiIn);

        public static uint midiInReset(uint hMidiIn)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiInReset(hMidiIn);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiInReset(hMidiIn);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiInGetNumDevs")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiInGetNumDevs();

        [DllImport("winmm.dll.so", EntryPoint = "midiInGetNumDevs")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiInGetNumDevs();

        public static uint midiInGetNumDevs()
        {
            uint ret = 0;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiInGetNumDevs();
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiInGetNumDevs();
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiInOpen")]
        private static extern uint __midiInOpen(ref uint lphMidiIn,
                                                int uDeviceID,
                                                IntPtr dwCallback,
                                                int dwCallbackInstance,
                                                int dwFlags);

        [DllImport("winmm.dll.so", EntryPoint = "midiInOpen")]
        private static extern uint __so_midiInOpen(ref uint lphMidiIn,
                                                int uDeviceID,
                                                IntPtr dwCallback,
                                                int dwCallbackInstance,
                                                int dwFlags);

        public static uint midiInOpen(ref uint lphMidiIn,
                                      int uDeviceID,
                                      IntPtr dwCallback,
                                      int dwCallbackInstance,
                                      int dwFlags)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiInOpen(ref lphMidiIn, uDeviceID, dwCallback, dwCallbackInstance, dwFlags);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiInOpen(ref lphMidiIn, uDeviceID, dwCallback, dwCallbackInstance, dwFlags);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiOutGetNumDevs")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiOutGetNumDevs();

        [DllImport("winmm.dll.so", EntryPoint = "midiOutGetNumDevs")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiOutGetNumDevs();

        public static uint midiOutGetNumDevs()
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiOutGetNumDevs();
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiOutGetNumDevs();
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiOutGetDevCapsA", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiOutGetDevCapsA([MarshalAs(UnmanagedType.U4)] uint uDeviceID,
                                                      ref MIDIOUTCAPSA pMidiOutCaps,
                                                      [MarshalAs(UnmanagedType.U4)] uint cbMidiOutCaps);

        [DllImport("winmm.dll.so", EntryPoint = "midiOutGetDevCapsA", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiOutGetDevCapsA([MarshalAs(UnmanagedType.U4)] uint uDeviceID,
                                                      ref MIDIOUTCAPSA pMidiOutCaps,
                                                      [MarshalAs(UnmanagedType.U4)] uint cbMidiOutCaps);

        public static uint midiOutGetDevCapsA(uint uDeviceID, ref MIDIOUTCAPSA pMidiOutCaps, uint cbMidiOutCaps)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiOutGetDevCapsA(uDeviceID, ref pMidiOutCaps, cbMidiOutCaps);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiOutGetDevCapsA(uDeviceID, ref pMidiOutCaps, cbMidiOutCaps);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiOutOpen")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiOutOpen([MarshalAs(UnmanagedType.SysUInt)] ref IntPtr lphMidiOut,
                                               [MarshalAs(UnmanagedType.U4)] uint uDeviceID,
                                               [MarshalAs(UnmanagedType.FunctionPtr)] Delegate dwCallback,
                                               [MarshalAs(UnmanagedType.U4)] uint dwInstance,
                                               [MarshalAs(UnmanagedType.U4)] uint dwFlags);

        [DllImport("winmm.dll.so", EntryPoint = "midiOutOpen")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiOutOpen([MarshalAs(UnmanagedType.SysUInt)] ref IntPtr lphMidiOut,
                                               [MarshalAs(UnmanagedType.U4)] uint uDeviceID,
                                               [MarshalAs(UnmanagedType.FunctionPtr)] Delegate dwCallback,
                                               [MarshalAs(UnmanagedType.U4)] uint dwInstance,
                                               [MarshalAs(UnmanagedType.U4)] uint dwFlags);

        public static uint midiOutOpen(ref IntPtr lphMidiOut,
                                        uint uDeviceID,
                                        Delegate dwCallback,
                                        uint dwInstance,
                                        uint dwFlags)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiOutOpen(ref lphMidiOut, uDeviceID, dwCallback, dwInstance, dwFlags);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiOutOpen(ref lphMidiOut, uDeviceID, dwCallback, dwInstance, dwFlags);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiOutClose")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiOutClose([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut);

        [DllImport("winmm.dll.so", EntryPoint = "midiOutClose")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiOutClose([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut);

        public static uint midiOutClose(IntPtr hMidiOut)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiOutClose(hMidiOut);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiOutClose(hMidiOut);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiOutShortMsg")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiOutShortMsg([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut, [MarshalAs(UnmanagedType.U4)] uint dwMsg);

        [DllImport("winmm.dll.so", EntryPoint = "midiOutShortMsg")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiOutShortMsg([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut, [MarshalAs(UnmanagedType.U4)] uint dwMsg);

        public static uint midiOutShortMsg(IntPtr hMidiOut, uint dwMsg)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiOutShortMsg(hMidiOut, dwMsg);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiOutShortMsg(hMidiOut, dwMsg);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiOutLongMsg")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiOutLongMsg([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, [MarshalAs(UnmanagedType.U4)] uint uSize);

        [DllImport("winmm.dll.so", EntryPoint = "midiOutLongMsg")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiOutLongMsg([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, [MarshalAs(UnmanagedType.U4)] uint uSize);

        public static uint midiOutLongMsg(IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, uint uSize)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiOutLongMsg(hMidiOut, ref lpMidiOutHdr, uSize);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiOutLongMsg(hMidiOut, ref lpMidiOutHdr, uSize);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiOutPrepareHeader")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiOutPrepareHeader([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, [MarshalAs(UnmanagedType.U4)] uint uSize);

        [DllImport("winmm.dll.so", EntryPoint = "midiOutPrepareHeader")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiOutPrepareHeader([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, [MarshalAs(UnmanagedType.U4)] uint uSize);

        public static uint midiOutPrepareHeader(IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, uint uSize)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiOutPrepareHeader(hMidiOut, ref lpMidiOutHdr, uSize);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiOutPrepareHeader(hMidiOut, ref lpMidiOutHdr, uSize);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "midiOutUnprepareHeader")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __midiOutUnprepareHeader([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, [MarshalAs(UnmanagedType.U4)] uint uSize);

        [DllImport("winmm.dll.so", EntryPoint = "midiOutUnprepareHeader")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __so_midiOutUnprepareHeader([MarshalAs(UnmanagedType.SysUInt)] IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, [MarshalAs(UnmanagedType.U4)] uint uSize);

        public static uint midiOutUnprepareHeader(IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, uint uSize)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __midiOutUnprepareHeader(hMidiOut, ref lpMidiOutHdr, uSize);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            } else if (status_winmm_so != DllStatus.NotFound) {
                try {
                    ret = __so_midiOutUnprepareHeader(hMidiOut, ref lpMidiOutHdr, uSize);
                } catch (DllNotFoundException ex) {
                    status_winmm_so = DllStatus.NotFound;
                }
            }
            return ret;
        }
        #endregion

        #region joy
        [DllImport("winmm", EntryPoint = "joyGetNumDevs")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint __joyGetNumDevs();

        public static uint joyGetNumDevs()
        {
            uint ret = 0;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __joyGetNumDevs();
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "joyGetPos")]
        private static extern uint __joyGetPos(uint uJoyID, ref JOYINFO pji);

        public static uint joyGetPos(uint uJoyID, ref JOYINFO pji)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __joyGetPos(uJoyID, ref pji);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "joyGetDevCapsW")]
        private static extern uint __joyGetDevCapsW(uint uJoyID, ref JOYCAPSW pjc, uint cbjc);

        public static uint joyGetDevCapsW(uint uJoyID, ref JOYCAPSW pjc, uint cbjc)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __joyGetDevCapsW(uJoyID, ref pjc, cbjc);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            }
            return ret;
        }

        [DllImport("winmm", EntryPoint = "joyGetPosEx")]
        private static extern uint __joyGetPosEx(uint uJoyID, ref JOYINFOEX pji);

        public static uint joyGetPosEx(uint uJoyID, ref JOYINFOEX pji)
        {
            uint ret = MMSYSERR_ERROR;
            if (status_winmm != DllStatus.NotFound) {
                try {
                    ret = __joyGetPosEx(uJoyID, ref pji);
                } catch (DllNotFoundException ex) {
                    status_winmm = DllStatus.NotFound;
                }
            }
            return ret;
        }
        #endregion

        #region wave
        [DllImport("winmm.dll")]
        public static extern uint waveOutWrite(IntPtr hwo, ref WAVEHDR pwh, uint cbwh);
        [DllImport("winmm.dll")]
        //[return: MarshalAs(UnmanagedType.U4)]
        public static extern uint waveOutOpen(ref IntPtr hWaveOut,
                                               int uDeviceID,
                                               ref WAVEFORMATEX lpFormat,
                                               delegateWaveOutProc dwCallback,
                                               IntPtr dwInstance,
                                               uint dwFlags);
        //public static extern uint waveOutOpen( ref IntPtr phwo, UINT uDeviceID, ref WAVEFORMATEX pwfx, delegateWaveOutProc dwCallback, IntPtr dwInstance, uint fdwOpen );
        [DllImport("winmm.dll")]
        public static extern uint waveOutPrepareHeader(IntPtr hwo, ref WAVEHDR pwh, UINT cbwh);
        [DllImport("winmm.dll")]
        public static extern uint waveOutGetPosition(IntPtr hwo, ref MMTIME pmmt, UINT cbmmt);
        [DllImport("winmm.dll")]
        public static extern uint waveOutReset(IntPtr hwo);
        [DllImport("winmm.dll")]
        public static extern uint waveOutUnprepareHeader(IntPtr hwo, ref WAVEHDR pwh, UINT cbwh);
        [DllImport("winmm.dll")]
        public static extern uint waveOutClose(IntPtr hwo);
        #endregion

        #region mci
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern bool mciGetErrorStringA(uint mcierr, [MarshalAs(UnmanagedType.LPStr)] string pszText, UINT cchText);
        #endregion
    }

}
