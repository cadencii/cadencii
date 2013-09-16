/*
 * Separator.cs
 * Copyright © 2010-2011 kbinani
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
#if JAVA
package cadencii;

import java.util.*;
#else
using System;
using cadencii.java.util;

namespace cadencii
{
#endif

    /// <summary>
    /// 信号を分岐する装置
    /// </summary>
#if JAVA
    public class Separator extends WaveUnit implements WaveReceiver
#else
    public class Separator : WaveUnit, WaveReceiver
#endif
    {
        const int _BUFLEN = 1024;
        private Vector<WaveReceiver> mReceivers = new Vector<WaveReceiver>();
        private double[] mBufferL = new double[_BUFLEN];
        private double[] mBufferR = new double[_BUFLEN];
        private int mVersion = 0;

        public override void setConfig( String parameter )
        {
            // do nothing
        }

        public override int getVersion()
        {
            return mVersion;
        }

        public void setReceiver( WaveReceiver receiver )
        {
            if ( receiver == null ) {
                return;
            }
            if ( !mReceivers.contains( receiver ) ) {
                mReceivers.add( receiver );
            }
        }

        public void end()
        {
            foreach ( WaveReceiver r in mReceivers ) {
                r.end();
            }
        }

        public void addReceiver( WaveReceiver receiver )
        {
            if ( receiver == null ) {
                return;
            }
            if ( !mReceivers.contains( receiver ) ) {
                mReceivers.add( receiver );
            }
        }

        public void push( double[] l, double[] r, int length )
        {
            if ( mReceivers.size() <= 0 ) {
                return;
            }

            int remain = length;
            int offset = 0;
            while ( remain > 0 ) {
                int amount = remain > _BUFLEN ? _BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    mBufferL[i] = l[i + offset];
                    mBufferR[i] = r[i + offset];
                }
                foreach ( WaveReceiver rc in mReceivers ) {
                    rc.push( mBufferL, mBufferR, amount );
                }
                offset += amount;
                remain -= amount;
            }
        }
    }

#if !JAVA
}
#endif
