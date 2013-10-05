/*
 * VsqCommandType.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii.vsq
{

    public enum VsqCommandType
    {
        ROOT,
        CHANGE_PRE_MEASURE,
        EVENT_ADD,
        EVENT_DELETE,
        EVENT_CHANGE_CLOCK,
        EVENT_CHANGE_LYRIC,
        EVENT_CHANGE_NOTE,
        EVENT_CHANGE_CLOCK_AND_NOTE,
        TRACK_CURVE_EDIT,
        TRACK_CURVE_EDIT_RANGE,
        TRACK_CURVE_EDIT2,
        TRACK_CURVE_EDIT2_ALL,
        //TRACK_CURVE_REMOVE_POINTS,
        TRACK_CURVE_REPLACE,
        TRACK_CURVE_REPLACE_RANGE,
        EVENT_CHANGE_VELOCITY,
        EVENT_CHANGE_ACCENT,
        EVENT_CHANGE_DECAY,
        EVENT_CHANGE_LENGTH,
        EVENT_CHANGE_CLOCK_AND_LENGTH,
        EVENT_CHANGE_ID_CONTAINTS,
        EVENT_CHANGE_CLOCK_AND_ID_CONTAINTS,
        TRACK_CHANGE_NAME,
        TRACK_ADD,
        TRACK_DELETE,
        EVENT_CHANGE_CLOCK_AND_ID_CONTAINTS_RANGE,
        EVENT_DELETE_RANGE,
        EVENT_ADD_RANGE,
        UPDATE_TEMPO,
        UPDATE_TEMPO_RANGE,
        UPDATE_TIMESIG,
        UPDATE_TIMESIG_RANGE,
        EVENT_CHANGE_ID_CONTAINTS_RANGE,
        TRACK_REPLACE,
        REPLACE,
        TRACK_CHANGE_PLAY_MODE,
        EVENT_REPLACE,
        EVENT_REPLACE_RANGE,
    }

}
