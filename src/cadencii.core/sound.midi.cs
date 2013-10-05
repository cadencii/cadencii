/*
 * sound.midi.cs
 * Copyright © 2011 kbinani
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
using System.Collections.Generic;

namespace cadencii.javax.sound.midi
{


    // 本来ならinterface
    public class MidiDevice
    {
        public class Info
        {
            protected string mName;
            protected string mVendor;
            protected string mDescription;
            protected string mVersion;

            /// <summary>
            /// デバイス情報オブジェクトを構築します。
            /// </summary>
            protected Info(string name, string vendor, string description, string version)
            {
                mName = name;
                mVendor = vendor;
                mDescription = description;
                mVersion = version;
            }

            /// <summary>
            /// 2つのオブジェクトが等しいかどうかを報告します。
            /// </summary>
            public bool equals(Object obj)
            {
                return base.Equals(obj);
            }

            /// <summary>
            /// デバイスの説明を取得します。
            /// </summary>
            public string getDescription()
            {
                return mDescription;
            }

            /// <summary>
            /// デバイスの名前を取得します。
            /// </summary>          
            public string getName()
            {
                return mName;
            }

            /// <summary>
            /// デバイスの供給会社の名前を取得します。
            /// </summary> 
            public string getVendor()
            {
                return mVendor;
            }

            /// <summary>
            /// デバイスのバージョンを取得します。
            /// </summary>
            public string getVersion()
            {
                return mVersion;
            }

            /// <summary>
            /// デバイス情報の文字列表現を提供します。
            /// </summary>
            public string toString()
            {
                return mName;
            }
        }

        public int getMaxTransmitters()
        {
            //TODO:
            return 0;
        }
    }

    public class MidiMessage
    {
        protected byte[] data;
        protected int length;

        /// <summary>
        /// 本来はprotected
        /// </summary>
        /// <param name="data"></param>
        public MidiMessage(byte[] data)
        {
            if (data == null) {
                this.data = new byte[0];
                this.length = 0;
            } else {
                this.data = new byte[data.Length];
                for (int i = 0; i < data.Length; i++) {
                    this.data[i] = (byte)(0xff & data[i]);
                }
                this.length = data.Length;
            }
        }

        public int getLength()
        {
            return length;
        }

        public byte[] getMessage()
        {
            return data;
        }

        public int getStatus()
        {
            if (data != null && data.Length > 0) {
                return data[0];
            }
            return 0;
        }
    }

    public interface Receiver
    {
        /// <summary>
        /// アプリケーションによるレシーバの使用が終了し、レシーバが要求する限られたリソースを解放または使用可能にできることを示します。
        /// </summary>
        void close();

        /// <summary>
        /// MIDI メッセージおよび時刻表示をこのレシーバに送信します。
        /// </summary>          
        void send(MidiMessage message, long timeStamp);
    }

    public interface Transmitter
    {
        /// <summary>
        /// アプリケーションによるレシーバの使用が終了し、トランスミッタが要求する限られたリソースを解放または使用可能にできることを示します。
        /// </summary>
        void close();

        /// <summary>
        /// このトランスミッタで MIDI メッセージを配信する現在のレシーバを取得します。
        /// </summary>          
        Receiver getReceiver();

        /// <summary>
        /// このトランスミッタで MIDI メッセージを配信するレシーバを設定します。
        /// </summary>
        void setReceiver(Receiver receiver);
    }

    public class MidiSystem
    {
        private static List<MidiDeviceInfoImpl> mMidiIn = null;
        private static List<MidiDeviceInfoImpl> mMidiOut = null;

        private class MidiDeviceInfoImpl : MidiDevice.Info
        {
            private int mIndex;
            private bool mIsMidiIn;

            public MidiDeviceInfoImpl(string name, string vendor, string description, string version, bool is_midi_in, int index)
                : base(name, vendor, description, version)
            {
                mIndex = index;
                mIsMidiIn = is_midi_in;
            }

            public bool equals(Object obj)
            {
                if (obj == null) return false;
                if (!(obj is MidiDeviceInfoImpl)) return false;
                MidiDeviceInfoImpl info = (MidiDeviceInfoImpl)obj;
                if (info.mIndex == this.mIndex &&
                    info.mIsMidiIn == this.mIsMidiIn) {
                    return true;
                } else {
                    return false;
                }
            }

            public Object clone()
            {
                return new MidiDeviceInfoImpl(mName, mVendor, mDescription, mVersion, mIsMidiIn, mIndex);
            }
        }

        private class MidiDeviceTransmitterImpl : MidiDevice, Transmitter
        {
            private MidiDeviceInfoImpl mInfo;

            public MidiDeviceTransmitterImpl(MidiDeviceInfoImpl info)
            {
                mInfo = info;
            }

            public void setReceiver(Receiver rc)
            {
                //TODO:
            }

            public Receiver getReceiver()
            {
                //TODO:
                return null;
            }

            public void close()
            {
                //TODO:
            }
        }

        private class MidiDeviceReceiverImpl : MidiDevice, Receiver
        {
            private MidiDeviceInfoImpl mInfo;

            public MidiDeviceReceiverImpl(MidiDeviceInfoImpl info)
            {
                mInfo = info;
            }

            public void send(MidiMessage message, long time)
            {
                //TODO:
            }

            public void close()
            {
                //TODO:
            }
        }

        public static MidiDevice getMidiDevice(MidiDevice.Info info)
        {
            foreach (MidiDeviceInfoImpl i in getMidiIn()) {
                if (i.equals(info)) {
                    return new MidiDeviceTransmitterImpl(i);
                }
            }
            foreach (MidiDeviceInfoImpl i in getMidiOut()) {
                if (i.equals(info)) {
                    return new MidiDeviceReceiverImpl(i);
                }
            }
            // javaではIllegalArgumentException
            throw new ArgumentException();
        }

        private static List<MidiDeviceInfoImpl> getMidiIn()
        {
            if (mMidiIn == null) {
                mMidiIn = new List<MidiDeviceInfoImpl>();
                int num = 0;
                try {
                    num = (int)win32.midiInGetNumDevs();
                } catch {
                    num = 0;
                }
                for (int i = 0; i < num; i++) {
                    MIDIINCAPS m = new MIDIINCAPS();
                    uint r = win32.midiInGetDevCaps((uint)i, ref m, (uint)System.Runtime.InteropServices.Marshal.SizeOf(m));
                    MidiDeviceInfoImpl impl =
                        new MidiDeviceInfoImpl(m.szPname, "", "", m.vDriverVersion + "", true, i);
                    mMidiIn.Add(impl);
                }
            }
            return mMidiIn;
        }

        private static List<MidiDeviceInfoImpl> getMidiOut()
        {
            if (mMidiOut == null) {
                mMidiOut = new List<MidiDeviceInfoImpl>();
                int num = 0;
                try {
                    num = (int)win32.midiOutGetNumDevs();
                } catch {
                    num = 0;
                }
                for (int i = 0; i < num; i++) {
                    MIDIOUTCAPSA m = new MIDIOUTCAPSA();
                    uint r = win32.midiOutGetDevCapsA((uint)i, ref m, (uint)System.Runtime.InteropServices.Marshal.SizeOf(m));
                    MidiDeviceInfoImpl impl =
                        new MidiDeviceInfoImpl(m.szPname, "", "", m.vDriverVersion + "", false, i);
                    mMidiOut.Add(impl);
                }
            }
            return mMidiOut;
        }

        public static MidiDevice.Info[] getMidiDeviceInfo()
        {
            int num = getMidiIn().Count + getMidiOut().Count;
            MidiDevice.Info[] arr = new MidiDevice.Info[num];
            int i = 0;
            foreach (MidiDeviceInfoImpl info in getMidiIn()) {
                arr[i] = (MidiDevice.Info)(MidiDeviceInfoImpl)info.clone();
                i++;
            }
            foreach (MidiDeviceInfoImpl info in getMidiOut()) {
                arr[i] = (MidiDevice.Info)(MidiDeviceInfoImpl)info.clone();
            }
            return arr;
        }
    }

}
