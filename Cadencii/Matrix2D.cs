/*
 * Matrix2D.cs
 * Copyright (C) 2010 kbinani
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
#if JAVA
package org.kbinani.cadencii;

import java.util.*;
import org.kbinani.media.*;
#else
using org.kbinani.java.util;
using org.kbinani.media;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 2次元の配列を格納します。XMLシリアライズ用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Matrix2D<T> {
        public T[] Data;
        public int Rows;
        public int Columns;

        public Matrix2D() {
            this.Rows = 0;
            this.Columns = 0;
            this.Data = new T[0];
        }

        public Matrix2D( int rows, int columns ) {
            this.Rows = rows;
            this.Columns = columns;
            this.Data = new T[rows * columns];
        }

        public void addRow() {
            T[,] buffer = new T[this.Rows, this.Columns];
            for ( int i = 0; i < this.Rows; i++ ) {
                for ( int j = 0; j < this.Columns; j++ ) {
                    buffer[i, j] = get( i, j );
                }
            }
            this.Data = new T[(this.Rows + 1) * this.Columns];
            for ( int i = 0; i < this.Rows; i++ ) {
                for ( int j = 0; j < this.Columns; j++ ) {
                    int indx = i * this.Columns + j;
                    this.Data[indx] = buffer[i, j];
                }
            }
            this.Rows++;
        }

        public void addColumn() {
            T[,] buffer = new T[this.Rows, this.Columns];
            for ( int i = 0; i < this.Rows; i++ ) {
                for ( int j = 0; j < this.Columns; j++ ) {
                    buffer[i, j] = get( i, j );
                }
            }
            this.Data = new T[this.Rows * (this.Columns + 1)];
            for ( int i = 0; i < this.Rows; i++ ) {
                for ( int j = 0; j < this.Columns; j++ ) {
                    int indx = i * this.Columns + j;
                    this.Data[indx] = buffer[i, j];
                }
            }
            this.Columns++;
        }

        public T get( int row_index, int column_index ) {
            int indx = row_index * this.Columns + column_index;
            return this.Data[indx];
        }

        public void set( int row_index, int column_index, T value ) {
            int indx = row_index * this.Columns + column_index;
            this.Data[indx] = value;
        }
    }

#if !JAVA
}
#endif
