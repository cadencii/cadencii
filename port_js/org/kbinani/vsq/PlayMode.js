/*
 * PlayMode.js
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.PlayMode == undefined ){

    org.kbinani.vsq.PlayMode = new function(){
    };

    /**
     * トラックはミュートされる．(-1)
     */
    org.kbinani.vsq.PlayMode.Off = -1;
    /**
     * トラックは合成された後再生される(0)
     */
    org.kbinani.vsq.PlayMode.PlayAfterSynth = 0;
    /**
     * トラックは合成しながら再生される(1)
     */
    org.kbinani.vsq.PlayWithSynth = 1;

}
