/*
 * CircuitConfig.cs
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
#else
using System;
using org.kbinani.java.util;

namespace org.kbinani.cadencii{
#endif

    public class CircuitConfig {
        public Vector<String> Data;
        public Vector<String> Devices;

        public CircuitConfig() {
            this.Data = new Vector<String>();
            this.Devices = new Vector<String>();
        }

        public String get( int row_index, int column_index ) {
            int indx = row_index * this.Devices.size() + column_index;
            return this.Data.get( indx );
        }

        public void set( int row_index, int column_index, String value ) {
            int indx = row_index * this.Devices.size() + column_index;
            this.Data.set( indx, value );
        }

        /// <summary>
        /// デバイスをひとつ追加する
        /// </summary>
        /// <param name="device_name"></param>
        public void addDevice( String device_name ) {
#if DEBUG
            //PortUtil.println( "CircuitConfig#addDevice; before;" );
            //printDataAsMatrix( Data, Devices.size() );
#endif
            // バッファを持っておく
            Vector<String> buf = new Vector<String>();
            foreach ( String s in this.Data ) {
                buf.add( s );
            }

            // 元に戻す
            this.Data.clear();
            int num = this.Devices.size();
            int index = 0;
            for ( int row = 0; row < num; row++ ) {
                for ( int col = 0; col < num; col++ ) {
                    this.Data.add( buf.get( index ) );
                    index++;
                }
                this.Data.add( "" );
            }
            for ( int newrow = 0; newrow < num + 1; newrow++ ) {
                this.Data.add( "" );
            }

            // デバイス名の分を追加
            this.Devices.add( device_name );
#if DEBUG
            //PortUtil.println( "CircuitConfig#addDevice; after;" );
            //printDataAsMatrix( Data, Devices.size() );
#endif
        }

#if DEBUG
        public void print() {
            if ( this.Devices.size() > 0 ) {
                String s = "|";
                foreach ( String i in this.Devices ) {
                    s += i + "|";
                }
                PortUtil.println( s );
            }
            printDataAsMatrix( this.Data, this.Devices.size() );
        }
#endif

        /// <summary>
        /// 1次元のリストを、正方行列とみなして標準出力にプリントアウトする
        /// </summary>
        /// <param name="array"></param>
        /// <param name="num"></param>
        private static void printDataAsMatrix( Vector<String> array, int num ) {
            int index = 0;
            for ( int row = 0; row < num; row++ ) {
                String s = "|";
                for ( int col = 0; col < num; col++ ) {
                    s += array.get( index ) + "|";
                    index++;
                }
                PortUtil.println( s );
            }
        }
    }

#if !JAVA
}
#endif
