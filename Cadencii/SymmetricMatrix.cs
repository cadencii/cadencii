/*
 * SymmetricMatrix.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;

using bocoree;

namespace Boare.Cadencii {

    /// <summary>
    /// 対称行列．格納できるのは値型のみ．
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SymmetricMatrix<T> where T : struct {
        public Vector<Vector<T>> Data = new Vector<Vector<T>>();
        /// <summary>
        /// 型Tのデフォルト値
        /// </summary>
        private T m_default;

        public SymmetricMatrix(){
            Type t = typeof( T );
            m_default = (T)t.Assembly.CreateInstance( t.FullName );
        }

        public int Count{
            get{
                return Data.size();
            }
        }

        public T this[int row, int column ]{
            get {
                if ( Count > 0 ) {
                    if ( column > row ) {
                        int b = column;
                        column = row;
                        row = b;
                    }
                    return Data.get( row ).get( column );
                } else {
                    return m_default;
                }
            }
            set {
                int newcount = Math.Max( row, column ) + 1;
                if ( this.Count < newcount ) {
                    setCount( newcount );
                }
                if ( column > row ) {
                    int b = column;
                    column = row;
                    row = b;
                }
                Data.get( row ).set( column, value );
            }
        }

        private void setCount( int value ) {
            int count = value;
            if ( count > 0 ){
                int current_count = Data.size();
                for( int i = current_count; i < count; i++ ){
                    Vector<T> add = new Vector<T>();
                    for ( int j = 0; j <= i; j++ ) {
                        add.add( m_default );
                    }
                    Data.add( add );
                }
            }
        }
    }

}
