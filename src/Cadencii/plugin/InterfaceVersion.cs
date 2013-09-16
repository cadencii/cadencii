/*
 * InterfaceVersion.cs
 * Copyright © 2013 kbinani
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
using System.Text;

namespace cadencii
{
    [Obsolete]
    public struct PlayPositionSpecifier
    {
        public int barCount;
        public int beat;
        public int clock;
        public int numerator;
        public int denominator;
        public int tempo;
    }

    public partial class AppManager
    {
        [Obsolete]
        public static PlayPositionSpecifier getPlayPosition()
        {
            var clock = getCurrentClock();
            var timesig = mVsq.TimesigTable.getTimesigAt( clock );

            var result = new PlayPositionSpecifier();
            int barCount = mVsq.TimesigTable.getBarCountFromClock( clock );

            int bar_top_clock = mVsq.TimesigTable.getClockFromBarCount( barCount );
            int clock_per_beat = 480 / 4 * timesig.denominator;
            int beat = (clock - bar_top_clock) / clock_per_beat;

            result.barCount = barCount - mVsq.getPreMeasure() + 1;
            result.beat = beat + 1;
            result.clock = clock - bar_top_clock - clock_per_beat * beat;
            result.denominator = timesig.denominator;
            result.numerator = timesig.numerator;
            result.tempo = mVsq.getTempoAt( clock );

            return result;
        }

        [Obsolete]
        public static int getSelectedEventCount()
        {
            return itemSelection.getEventCount();
        }

        [Obsolete]
        public static SelectedEventEntry getLastSelectedEvent()
        {
            return itemSelection.getLastEvent();
        }

        [Obsolete]
        public static cadencii.java.util.Iterator<SelectedEventEntry> getSelectedEventIterator()
        {
            return itemSelection.getEventIterator();
        }

        [Obsolete]
        public static bool isSelectedEventContains( int track, int id )
        {
            var i = itemSelection.getEventIterator();
            while ( i.hasNext() ) {
                var item = i.next();
                if ( item.original.InternalID == id && item.track == track ) {
                    return true;
                }
            }
            return false;
        }

        [Obsolete]
        public static void removeSelectedEvent( int id )
        {
            itemSelection.removeEvent( id );
        }

        [Obsolete]
        public static void addSelectedEvent( int id )
        {
            itemSelection.addEvent( id );
        }

        [Obsolete]
        public static FormMain mainWindow
        {
            get
            {
                return mMainWindow;
            }
        }
    }

    [Obsolete]
    public static class VSTiProxy
    {
        public const string RENDERER_DSB2 = VsqFileEx.RENDERER_DSB2;
        public const string RENDERER_DSB3 = VsqFileEx.RENDERER_DSB3;
        public const string RENDERER_UTU0 = VsqFileEx.RENDERER_UTU0;
        public const string RENDERER_STR0 = VsqFileEx.RENDERER_STR0;
        public const string RENDERER_AQT0 = VsqFileEx.RENDERER_AQT0;
        public const string RENDERER_NULL = VsqFileEx.RENDERER_NULL;
    }
}
