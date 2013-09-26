#if !JAVA
/*
 * Vector.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
//#define VECTOR_TEST
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace cadencii.java.util {

    [Serializable]
#if VECTOR_TEST
    public class Vector<T> {
        public Vector( int capacity ) {
        }

        public Vector( Vector<T> array ) {
        }

        public Vector( T[] array ){
        }

        public Vector(){
        }

        public void addAll( T[] array ) {
        }

        public void addAll( Vector<T> array ) {
        }

        public void insertElementAt( T obj, int index ) {
        }

        public void removeElementAt( int index ) {
        }

        public int size() {
            return 0;
        }

        public T get( int index ) {
            return default( T );
        }

        public void set( int index, T value ) {
        }

        public Object[] toArray() {
            return null;
        }

        public T[] toArray( T[] array ) {
            return null;
        }

        public Iterator iterator() {
            return null;
        }

        public void add( T obj ) {
        }

        public void Add( T obj ) {
        }

        public bool contains( T obj ) {
            return false;
        }

        public void clear() {
        }
    }
#else
    public class Vector<T> : List<T> {
        public Vector( ICollection<T> list ) :
            base( list ) {
        }

        public Vector( int capacity )
            : base( capacity ) {
        }

        public Vector()
            : base() {
        }
    }
#endif

}
#endif
