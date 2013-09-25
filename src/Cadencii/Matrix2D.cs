/*
 * Matrix2D.cs
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
import cadencii.media.*;
#else
using cadencii.java.util;
using cadencii.media;

namespace cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 2次元の配列を格納します。XMLシリアライズ用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Matrix2D<T> {
        public Vector<T> Data;
        public int Rows;
        public int Columns;

        public Matrix2D() {
            this.Rows = 0;
            this.Columns = 0;
            this.Data = newArray( 0 );
        }

        public Matrix2D( int rows, int columns ) {
            this.Rows = rows;
            this.Columns = columns;
            this.Data = newArray( rows * columns );
        }

        public void addRow() {
            Vector<Vector<T>> buffer = new Vector<Vector<T>>();

            for ( int i = 0; i < this.Rows; i++ ) {
                buffer.Add( newArray( this.Columns ) );
                for ( int j = 0; j < this.Columns; j++ ) {
                    buffer.get( i ).set( j, this.get( i, j ) );
                }
            }
            this.Data = newArray( (this.Rows + 1) * this.Columns );
            for ( int i = 0; i < this.Rows; i++ ) {
                for ( int j = 0; j < this.Columns; j++ ) {
                    int indx = i * this.Columns + j;
                    this.Data.set( indx, buffer.get( i ).get( j ) );
                }
            }
            this.Rows++;
        }

        public void addColumn() {
            Vector<Vector<T>> buffer = new Vector<Vector<T>>();
            for ( int i = 0; i < this.Rows; i++ ) {
                buffer.Add( newArray( this.Columns ) );
                for ( int j = 0; j < this.Columns; j++ ) {
                    buffer.get( i ).set( j, this.get( i, j ) );
                }
            }
            this.Data = newArray( this.Rows * (this.Columns + 1) );
            for ( int i = 0; i < this.Rows; i++ ) {
                for ( int j = 0; j < this.Columns; j++ ) {
                    int indx = i * this.Columns + j;
                    this.Data.set( indx, buffer.get( i ).get( j ) );
                }
            }
            this.Columns++;
        }

        private Vector<T> newArray( int size ) {
            Vector<T> ret = new Vector<T>();
            ret.setSize( size );
            return ret;
        }

        public T get( int row_index, int column_index ) {
            int indx = row_index * this.Columns + column_index;
            return this.Data.get( indx );
        }

        public void set( int row_index, int column_index, T value ) {
            int indx = row_index * this.Columns + column_index;
            this.Data.set( indx, value );
        }
    }

#if !JAVA
}
#endif
