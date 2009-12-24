/*
 * BEvent.cs
 * Copyright (C) 2009 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\BEvent.java
#else
using System;
using System.Reflection;
using System.Collections.Generic;

namespace org.kbinani {

    public class BEvent<T> where T : BEventHandler {
        private List<T> m_delegates;

        public BEvent(){
            m_delegates = new List<T>();
        }

        /*public BEvent<T> registerNative( object bind, string eventName, Delegate aDelegate ) {
            if ( bind == null || eventName == "" ) {
                return this;
            }
            Type t = bind.GetType();
            foreach ( EventInfo ei in t.GetEvents() ) {
                if ( ei.Name == eventName ) {
                    ei.AddEventHandler( bind, aDelegate );
                    break;
                }
            }
            return this;
        }*/

        public void add( T aDelegate ) {
            m_delegates.Add( aDelegate );
        }

        public void remove( T aDelegate ) {
            int count = m_delegates.Count;
            for ( int i = 0; i < count; i++ ) {
                T item = m_delegates[1];
                if ( aDelegate.equals( item ) ) {
                    m_delegates.RemoveAt( i );
                    break;
                }
            }
        }

        public void raise( params Object[] args ) {
            int count = m_delegates.Count;
            for ( int i = 0; i < count; i++ ) {
                m_delegates[i].invoke( args );
            }
        }
    }
}
#endif
