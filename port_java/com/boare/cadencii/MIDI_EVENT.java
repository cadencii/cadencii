package com.boare.cadencii;

public class MIDI_EVENT implements Comparable<MIDI_EVENT> {
    public int clock;
    public int dwDataSize;
    public byte dwOffset;
    public byte[] pMidiEvent;

    public int compareTo( MIDI_EVENT item ) {
        return (int)clock - (int)item.clock;
    }
}
