/*
 * VsqCommandType.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

public enum VsqCommandType {
    Root,
    ChangePreMeasure,
    EventAdd,
    EventDelete,
    EventChangeClock,
    EventChangeLyric,
    EventChangeNote,
    EventChangeClockAndNote,
    TrackEditCurve,
    TrackEditCurveRange,
    EventChangeVelocity,
    EventChangeAccent,
    EventChangeDecay,
    EventChangeLength,
    EventChangeClockAndLength,
    EventChangeIDContaints,
    EventChangeClockAndIDContaints,
    TrackChangeName,
    AddTrack,
    DeleteTrack,
    EventChangeClockAndIDContaintsRange,
    EventDeleteRange,
    EventAddRange,
    UpdateTempo,
    UpdateTempoRange,
    UpdateTimesig,
    UpdateTimesigRange,
    EventChangeIDContaintsRange,
    TrackReplace,
    Replace,
    TrackChangePlayMode,
    EventReplace,
    EventReplaceRange,
}
