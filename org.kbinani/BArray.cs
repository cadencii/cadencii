using System;
using System.Collections.Generic;

namespace org.kbinani {

    public class BArray<T> {
        const int INIT_BUFLEN = 512;
        private T[] array;
        private int length = 0;

        public BArray() {
            ensureBufferLength( INIT_BUFLEN );
        }

        public void clear() {
            length = 0;
        }

        public int size() {
            return length;
        }

        public T get( int index ) {
            return array[index];
        }

        public void set( int index, T value ) {
            array[index] = value;
        }

        public void add( T value ) {
            length++;
            ensureBufferLength( length );
            array[length - 1] = value;
        }

        private void ensureBufferLength( int length ) {
            if ( array == null ) {
                array = new T[INIT_BUFLEN];
            }
            if ( length > array.Length ) {
                int newLength = length;
                if ( this.length <= 0 ) {
                    newLength = (int)(length * 1.2);
                } else {
                    int order = length / array.Length;
                    if ( order <= 1 ) {
                        order = 2;
                    }
                    newLength = array.Length * order;
                }
                Array.Resize( ref array, newLength );
            }
        }
    }

}
