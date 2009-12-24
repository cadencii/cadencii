#if !JAVA
/*
 * Vector.cs
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
//#define VECTOR_TEST
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace org.kbinani.java.util {

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
        internal Vector( ICollection<T> list ) :
            base( list ) {
        }

        public Vector( int capacity )
            : base( capacity ) {
        }

        public Vector( Vector<T> array )
            : base( array.toArray( new T[]{} ) ){
        }

        public void addAll( Vector<T> array ) {
            base.AddRange( array );
        }

        public int indexOf( T obj ) {
            return base.IndexOf( obj );
        }

        public void insertElementAt( T obj, int index ){
            base.Insert( index, obj );
        }

        public bool removeElement( T obj ) {
            return base.Remove( obj );
        }

        public void removeElementAt( int index ){
            base.RemoveAt( index );
        }

        public Vector()
            : base() {
        }

        public void clear(){
            base.Clear();
        }

        public bool contains( T obj ){
            return base.Contains( obj );
        }
    
        public void add( T obj ) {
            base.Add( obj );
        }

        public int size() {
            return base.Count;
        }

        public T get( int index ) {
            return base[index];
        }

        public void set( int index, T value ) {
            base[index] = value;
        }

        public Object[] toArray() {
            int c = size();
            Object[] ret = new Object[c];
            for( int i = 0; i < c; i++ ){
                ret[i] = base[i];
            }
            return ret;
        }

        public T[] toArray( T[] array ) {
            int c = size();
            T[] ret = new T[c];
            for( int i = 0; i < c; i++ ){
                ret[i] = base[i];
            }
            return ret;
        }

        public Iterator iterator() {
            return new ListIterator<T>( this );
        }
    }
#endif

}
#endif
