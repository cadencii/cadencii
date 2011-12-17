package com.github.cadencii.media;

import com.github.cadencii.BEventHandler;

public class MidiReceivedEventHandler extends BEventHandler{
    public MidiReceivedEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, javax.sound.midi.MidiMessage.class );
    }

    public MidiReceivedEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, javax.sound.midi.MidiMessage.class );
    }
}
