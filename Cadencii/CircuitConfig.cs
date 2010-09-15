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

import java.awt.*;
import java.util.*;
#else
using System;
using org.kbinani.java.awt;
using org.kbinani.java.util;

namespace org.kbinani.cadencii.draft {
#endif

    /// <summary>
    /// シンセサイザ等の回路の接続を表現するクラス
    /// </summary>
    public class CircuitConfig {
        public Vector<CircuitConfigEntry> Data;
        /// <summary>
        /// シンセサイザの型のフルネーム(ex. "org.kbinani.cadencii.VocaloidWaveGenerator")のリスト
        /// </summary>
        public Vector<String> Units;
        /// <summary>
        /// シンセサイザに渡す設定値を格納した文字列
        /// </summary>
        public Vector<String> Arguments;
        /// <summary>
        /// 画面にシンセサイザを描画するときの位置
        /// </summary>
        public Vector<Point> DrawPosition;

        public CircuitConfig() {
            this.Data = new Vector<CircuitConfigEntry>();
            this.Units = new Vector<String>();
            this.Arguments = new Vector<String>();
            this.DrawPosition = new Vector<Point>();
        }

        /// <summary>
        /// 指定したVSQの指定したトラックの歌声合成させるデフォルトの回路を生成するファクトリメソッド.
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        public static CircuitConfig createDefault( VsqFileEx vsq, int track ) {
            /*
             * 
             * *WaveGenerator--->Amplifier--->Mixer--->Separator--->MonitorWaveReceiver
             *                                   |          |
             * FileWaveSender<----Amplifier<-----|          -------->FileWaveReceiver
             *                                   |
             * FileWaveSender<----Amplifier<-----|
             *        .               .          |
             *        .               .          .
             *        .               .          .
             * 
             */
            throw new NotImplementedException();
        }

        public CircuitConfigEntry get( int row_index, int column_index ) {
            int indx = row_index * this.Units.size() + column_index;
            return this.Data.get( indx );
        }

        public void set( int row_index, int column_index, CircuitConfigEntry value ) {
            int indx = row_index * this.Units.size() + column_index;
            this.Data.set( indx, value );
        }

        /// <summary>
        /// デバイスをひとつ追加する
        /// </summary>
        /// <param name="device_name"></param>
        public void addUnit( String device_name ) {
#if DEBUG
            //PortUtil.println( "CircuitConfig#addUnit; before;" );
            //printDataAsMatrix( Data, Devices.size() );
#endif
            // バッファを持っておく
            Vector<CircuitConfigEntry> buf = new Vector<CircuitConfigEntry>();
            foreach ( CircuitConfigEntry s in this.Data ) {
                buf.add( s );
            }

            // 元に戻す
            this.Data.clear();
            int num = this.Units.size();
            int index = 0;
            for ( int row = 0; row < num; row++ ) {
                for ( int col = 0; col < num; col++ ) {
                    this.Data.add( buf.get( index ) );
                    index++;
                }
                this.Data.add( null );
            }
            for ( int newrow = 0; newrow < num + 1; newrow++ ) {
                this.Data.add( null );
            }

            // デバイス名の分を追加
            this.Units.add( device_name );
            this.Arguments.add( "" );
            this.DrawPosition.add( new Point() );
#if DEBUG
            //PortUtil.println( "CircuitConfig#addDevice; after;" );
            //printDataAsMatrix( Data, Devices.size() );
#endif
        }

#if DEBUG
        public void print() {
            if ( this.Units.size() > 0 ) {
                String s = "|";
                foreach ( String i in this.Units ) {
                    s += i + "|";
                }
                PortUtil.println( s );
            }
            printDataAsMatrix( this.Data, this.Units.size() );
        }
#endif

        /// <summary>
        /// 1次元のリストを、正方行列とみなして標準出力にプリントアウトする
        /// </summary>
        /// <param name="array"></param>
        /// <param name="num"></param>
        private static void printDataAsMatrix( Vector<CircuitConfigEntry> array, int num ) {
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
