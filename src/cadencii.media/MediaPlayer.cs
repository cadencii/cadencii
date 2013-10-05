/*
 * MediaPlayer.cs
 * Copyright © 2007-2011 kbinani
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
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cadencii.media
{

    /// <summary>
    /// Sound player using mciSendSring command operation
    /// </summary>
    public class MediaPlayer : IDisposable
    {
        private string m_filename = "";
        const int FALSE = 0;
        const int TRUE = 1;
        bool pausing = false;
        int m_volume = 1000;
        bool mute = false;
        string m_alias = "";
        bool m_loaded = false;
        float m_speed = 1.0f;
        bool m_playing = false;
        /// <summary>
        /// the number of Load failure
        /// </summary>
        int m_load_failed = 0;

        ~MediaPlayer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (m_loaded) {
                Close();
            }
            m_playing = false;
        }

        /// <summary>
        /// Gets or Sets the speed
        /// </summary>
        public float Speed
        {
            get
            {
                return m_speed;
            }
            set
            {
                m_speed = value;
                SetSpeed(m_speed);
            }
        }

        /// <summary>
        /// Sets the speed
        /// </summary>
        /// <param name="speed">the value of speed to set</param>
        private void SetSpeed(float speed)
        {
            w_mciSendString("set " + m_alias + " speed " + (int)(speed * 1000.0f));
        }

        /// <summary>
        /// Gets or Sets the volume (0 &gt;= volume &gt;= 1000)
        /// </summary>
        public int Volume
        {
            get
            {
                if (mute) {
                    return 0;
                } else {
                    return m_volume;
                }
            }
            set
            {
                m_volume = value;
                SetVolume(m_volume);
            }
        }

        /// <summary>
        /// Sets the volume (0 &gt;= volume &gt;= 1000)
        /// </summary>
        /// <param name="value"></param>
        private void SetVolume(int value)
        {
            ReLoad();
            w_mciSendString("setaudio " + m_alias + " volume to " + value);
        }

        /// <summary>
        /// Gets the volume (0 &lt;= volume &lt;= 1000)
        /// </summary>
        /// <returns></returns>
        private int GetVolume()
        {
            ReLoad();
            string str;
            bool ret = w_mciSendString("status " + m_alias + " volume", out str);
            int v = m_volume;
            if (ret) {
                int v1;
                if (int.TryParse(str, out v1)) {
                    v = v1;
                }
            }
#if DEBUG
            Console.WriteLine("MediaPlayer+GetVolume()");
            Console.WriteLine("    str=" + str);
            Console.WriteLine("    volume=" + v);
#endif
            return v;
        }

        /// <summary>
        /// Gets or Sets whether sound is muted or not
        /// </summary>
        public bool IsMuted
        {
            get
            {
                return mute;
            }
            set
            {
                bool old = mute;
                mute = value;
                if (old != mute) {
                    if (mute) {
                        SetVolume(0);
                    } else {
                        SetVolume(m_volume);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the pass of the sound file
        /// </summary>
        public string SoundLocation
        {
            get
            {
                return m_filename;
            }
        }

        /// <summary>
        /// external declaration of mciSendString
        /// </summary>
        /// <param name="s1">Command String</param>
        /// <param name="s2">Return String</param>
        /// <param name="i1">Return String Size</param>
        /// <param name="i2">Callback Hwnd</param>
        /// <returns>true when successed, false if not</returns>
        [DllImport("winmm.dll")]
        extern static int mciSendString(string s1, StringBuilder s2, int i1, int i2);

        /// <summary>
        /// mciSendString wrapper with exception handling
        /// </summary>
        /// <param name="command">command sending to MCI</param>
        /// <param name="result">returned string of mciSendString</param>
        /// <returns>command successedd or not</returns>
        static bool w_mciSendString(string command, out string result)
        {
            StringBuilder sb = new StringBuilder(32);
            int io_status = 0;
            result = "";
            try {
                io_status = mciSendString(command, sb, sb.Capacity, 0);
                result = sb.ToString();
            } catch {
                return false;
            }
            if (io_status == 0) {
                return true;
            } else {
                return false;
            }
        }

        static bool w_mciSendString(string command)
        {
            string ret;
            return w_mciSendString(command, out ret);
        }

        /// <summary>
        /// Closes sound file temporary
        /// </summary>
        /// <returns></returns>
        public void UnLoad()
        {
            if (m_filename != "") {
                Stop();
                w_mciSendString("close " + m_alias);
                m_loaded = false;
            }
        }

        /// <summary>
        /// Opens sound file which was closed with "UnLoad" method
        /// </summary>
        /// <returns></returns>
        public void ReLoad()
        {
            if (m_filename != "" && !m_loaded && m_load_failed < 10) {
                if (Load(m_filename)) {
                    m_loaded = true;
                    if (mute) {
                        SetVolume(0);
                    } else {
                        SetVolume(m_volume);
                    }
                }
            }
        }

        /// <summary>
        /// Opens sound file
        /// </summary>
        /// <param name="filename">Path of sound file to open</param>
        /// <returns>successed opening the file or not</returns>
        public bool Load(string filename)
        {
#if DEBUG
            Console.WriteLine("MediaPlayer+Load(String)");
            Console.WriteLine("    filename=" + filename);
#endif
            if (m_filename != "") {
                Close();
            }
            this.m_filename = filename;
            m_alias = cadencii.Misc.getmd5(m_filename);
#if DEBUG
            Console.WriteLine("    m_alias=" + m_alias);
#endif
            bool ret = w_mciSendString("open \"" + filename + "\" type MPEGVIDEO2 alias " + m_alias);
            if (!ret) {
                ret = w_mciSendString("open \"" + filename + "\" type MPEGVIDEO alias " + m_alias);
                if (!ret) {
                    ret = w_mciSendString("open \"" + filename + "\" alias " + m_alias);
                }
            }
#if DEBUG
            Console.WriteLine("    w_mciSendString result=" + ret);
#endif
            if (ret) {
                m_loaded = true;
            } else {
                m_load_failed++;
            }
#if DEBUG
            m_volume = GetVolume();
            Console.WriteLine("    m_volume=" + m_volume);
#endif
            SetVolume(m_volume);
            return ret;
        }

        /// <summary>
        /// Plays sound from specified second
        /// </summary>
        /// <param name="time">Sound position start to play</param>
        /// <returns>true if play command successed</returns>
        public bool PlayFrom(double time)
        {
            if (m_filename == "") {
                return false;
            }
            long position = (long)(time * 1000);
            if (!m_loaded) {
                ReLoad();
            }
            /*if ( mute ) {
                EnterMute();
            } else {
                ExitMute();
            }*/
            SetSpeed(m_speed);
            m_playing = true;
            return w_mciSendString("play " + m_alias + " from " + position.ToString());
        }

        /// <summary>
        /// Closes sound file
        /// </summary>
        /// <returns>true if successed closing sound file</returns>
        public bool Close()
        {
            if (m_filename == "") {
                return false;
            }
            Stop();
            m_filename = "";
            m_loaded = false;
            return w_mciSendString("close " + m_alias);
        }

        /// <summary>
        /// Plays sound from time 0 second
        /// </summary>
        /// <returns>true if successed to play</returns>
        public bool Play()
        {
            if (m_filename == "") {
                return false;
            }
            if (!m_loaded) {
                ReLoad();
            }
            /*if ( mute ) {
                EnterMute();
            } else {
                ExitMute();
            }*/
            SetSpeed(m_speed);
            m_playing = true;
            if (pausing) {
                //return w_mciSendString( "resume \"" + m_filename + "\"", null, 0, 0 );
                return w_mciSendString("resume " + m_alias);
            } else {
                //return w_mciSendString( "play \"" + m_filename + "\"", null, 0, 0 );
                return w_mciSendString("play " + m_alias);
            }
        }

        /// <summary>
        /// Seeks to specified position
        /// </summary>
        /// <param name="pos_second">position to seek in second</param>
        /// <returns>true if successed to seek</returns>
        public bool Seek(double pos_second)
        {
            if (m_filename == "") {
                return false;
            }
            if (!m_loaded) {
                ReLoad();
            }
            long position = (long)(pos_second * 1000.0);
            bool ret = w_mciSendString("seek " + m_alias + " to " + position);
            return ret;
        }

        /// <summary>
        /// Pauses sound
        /// </summary>
        /// <returns>true if successed to pause</returns>
        public bool Pause()
        {
            if (m_filename == "") {
                return false;
            }
            if (!m_loaded) {
                ReLoad();
            }
            m_playing = false;
            pausing = true;
            //return w_mciSendString( "pause \"" + m_filename + "\"", null, 0, 0 );
            return w_mciSendString("pause " + m_alias);
        }

        /// <summary>
        /// Gets the current playing position in millisecond
        /// </summary>
        /// <returns>playing position in millisecond</returns>
        public int GetPosition()
        {
            if (this.SoundLocation == "") {
                return -1;
            }
            if (!m_loaded) {
                ReLoad();
            }
            string ret;
            w_mciSendString("status " + m_alias + " position", out ret);
            int pos;
            try {
                pos = int.Parse(ret);
            } catch {
                pos = -1;
            }
            return pos;
        }

        /// <summary>
        /// Gets the sound length in millisecond
        /// </summary>
        /// <returns>Sound length in millisecond</returns>
        public int GetLength()
        {
            if (this.SoundLocation == "") {
                return -1;
            }
            if (!m_loaded) {
                ReLoad();
            }
            string ret;
            w_mciSendString("status " + m_alias + " length", out ret);
            int length = -1;
            if (int.TryParse(ret, out length)) {
                return length;
            } else {
                return -1;
            }
        }

        /// <summary>
        /// Stops sound
        /// </summary>
        /// <returns>true if successed to stop</returns>
        public bool Stop()
        {
            m_playing = false;
            return w_mciSendString("stop " + m_alias);
        }

        public bool IsPlaying
        {
            get
            {
                return m_playing;
            }
        }
    }

}
