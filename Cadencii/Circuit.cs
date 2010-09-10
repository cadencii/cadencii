/*
 * Circuit.cs
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

namespace org.kbinani.cadencii.draft {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// シンセサイザ等の回路の接続と実行を行うクラス
    /// </summary>
#if JAVA
    public class Circuit implements Runnable {
#else
    public class Circuit : Runnable {
#endif
        private Vector<WaveUnit> mUnits;
        private WaveGenerator mGenerator;
        private long mSamples;

        private Circuit() {
        }

        /// <summary>
        /// 回路を実行する
        /// </summary>
        public void run() {
            if ( mGenerator == null ) {
                return;
            }
            mGenerator.begin( mSamples );
        }

        /// <summary>
        /// 実行時のパラメータを設定する
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="start_clock"></param>
        /// <param name="end_clock"></param>
        /// <param name="samples"></param>
        /// <returns></returns>
        public boolean init( VsqFileEx vsq, int track, int start_clock, int end_clock, long samples ) {
            if ( mGenerator == null ) {
                return false;
            }
            mGenerator.init( vsq, track, start_clock, end_clock );
            mSamples = samples;
            return true;
        }

        /// <summary>
        /// 回路の接続設定から，回路を構築する
        /// </summary>
        /// <param name="config"></param>
        public Circuit( CircuitConfig config, VsqFileEx vsq, EditorConfig editor_config ) {
            // ユニットを格納する配列を作成
            int num = 0;
            if ( config != null && config.Units != null ) {
                num = config.Units.size();
            }
            mUnits = new Vector<WaveUnit>( num );

            // 型名と型のマッピング
            TreeMap<String, Type> map = new TreeMap<String, Type>();

            // ユニットのインスタンスを生成．
            for ( int i = 0; i < num; i++ ) {
                // 型名
                String classname = config.Units.get( i );
                // 型名から型を取得
                Type unit_type = null;
                if ( map.containsKey( classname ) ) {
                    unit_type = map.get( classname );
                } else {
                    unit_type = Type.GetType( classname );
                    map.put( classname, unit_type );
                }
                // リフレクションによりインスタンスを生成
                WaveUnit instance = (WaveUnit)unit_type.GetConstructor( System.Type.EmptyTypes ).Invoke( null );

                // 初期化
                instance.setGlobalConfig( editor_config );
                instance.setConfig( config.Arguments.get( i ) );

                // Generatorかどうか？
                if ( instance is WaveGenerator ) {
                    mGenerator = (WaveGenerator)instance;
                }

                // 格納
                mUnits.set( i, instance );
            }

            // 接続設定に基づき，回路を組む
            for ( int row = 0; row < num; row++ ) {
                WaveUnit unit_row = mUnits.get( row );
                for ( int col = 0; col < num; col++ ) {
                    CircuitConfigEntry entry = config.get( row, col );
                    if( entry == null ){
                        continue;
                    }

                    //TODO: この辺がまだ
                    WaveUnit unit_col = mUnits.get( col );
                    if ( entry.ConnectionKind == CircuitConnectionKind.RECEIVER ) {
                        // 相手をレシーバとして接続
                        if ( unit_col is WaveReceiver ) {
                            WaveReceiver receiver = (WaveReceiver)unit_col;
                            if ( unit_row is WaveReceiver ) {
                                WaveReceiver unit_row_as_receiver = (WaveReceiver)unit_row;
                                unit_row_as_receiver.setReceiver( receiver );
                            } else if ( unit_row is WaveGenerator ) {
                                WaveGenerator unit_row_as_generator = (WaveGenerator)unit_row;
                                unit_row_as_generator.setReceiver( receiver );
                            }
                        }
                    } else if ( entry.ConnectionKind == CircuitConnectionKind.SENDER ) {
                        // 相手をセンダーとして接続
                        if ( unit_col is WaveSender ) {
                            WaveSender sender = (WaveSender)unit_col;
                            if ( unit_row is WaveSender ) {
                                WaveSender unit_row_as_sender = (WaveSender)unit_row;
                                unit_row_as_sender.setSender( sender );
                            }
                        }
                    }
                }
            }
        }
    }

#if !JAVA
}
#endif
