/*
 * MidiDeviceImp.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using cadencii.media;

namespace cadencii
{
    public class MidiDeviceImp
    {
        const int CHANNEL = 16;
        private MidiOutDevice s_device;
        private int[] s_last_program = new int[CHANNEL];
        private bool s_initialized = false;

        public bool Initialized
        {
            get
            {
                return s_initialized;
            }
        }

        public MidiDeviceImp(uint device_id)
        {
            s_device = new MidiOutDevice(device_id);
            s_initialized = true;
            for (int i = 0; i < CHANNEL; i++) {
                s_last_program[i] = -1;
            }
        }

        public void Play(byte channel, byte program, byte note, byte velocity)
        {
            if (CHANNEL < channel) {
                return;
            }
            if (s_last_program[channel] != program) {
                s_device.ProgramChange(channel, program);
                s_last_program[channel] = program;
            }
            s_device.Play(channel, note, velocity);
        }

        public void Terminate()
        {
            if (s_initialized && s_device != null) {
                s_device.Close();
            }
        }
    }

}
