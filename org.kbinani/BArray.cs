using System;
using System.Collections.Generic;

namespace org.kbinani {

    /// <summary>
    /// インデックスを使ってアクセスできる、型指定されたリストを表します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BArray<T> {
        const int INIT_BUFLEN = 512;
        private T[] array;
        private int length = 0;

        /// <summary>
        /// 要素数が0のリストの新しいインスタンスを初期化します。
        /// </summary>
        public BArray() {
            ensureBufferLength( INIT_BUFLEN );
        }

        /// <summary>
        /// このリストから全ての要素を削除します。
        /// </summary>
        public void clear() {
            length = 0;
        }

        /// <summary>
        /// このリストの要素数を取得します。
        /// </summary>
        /// <returns></returns>
        public int size() {
            return length;
        }

        /// <summary>
        /// このリストの、指定したインデックスにある要素を取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T get( int index ) {
            return array[index];
        }

        /// <summary>
        /// このリストの、指定したインデックスに要素を指定します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void set( int index, T value ) {
            array[index] = value;
        }

        /// <summary>
        /// このリストの末尾に、指定した要素を追加します。
        /// </summary>
        /// <param name="value"></param>
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
