#if ENABLE_VOCALOID
/*
 * MemoryManager.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {

    public class MemoryManager {
        private Vector<IntPtr> list = new Vector<IntPtr>();

        public IntPtr malloc( int bytes ) {
            IntPtr ret = Marshal.AllocHGlobal( bytes );
            list.add( ret );
            return ret;
        }

        public void free( IntPtr p ) {
            for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                IntPtr v = (IntPtr)itr.next();
                if ( v.Equals( p ) ) {
                    Marshal.FreeHGlobal( p );
                    itr.remove();
                    break;
                }
            }
        }

        public void dispose() {
            for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                IntPtr v = (IntPtr)itr.next();
                try {
                    Marshal.FreeHGlobal( v );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "MemoryManager#dispose; ex=" + ex );
                }
            }
            list.clear();
        }

        ~MemoryManager() {
            dispose();
        }
    }

}
#endif
