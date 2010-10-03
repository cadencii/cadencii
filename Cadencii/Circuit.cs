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
    using Integer = System.Int32;
#endif

    /// <summary>
    /// シンセサイザ等の回路の接続と実行を行うクラス
    /// </summary>
#if JAVA
    public class Circuit implements Runnable {
#else
    public class Circuit : Runnable {
#endif
        /// <summary>
        /// 回路を構成するユニットの一覧
        /// </summary>
        public Vector<WaveUnit> mUnits;
        /// <summary>
        /// 回路の設定
        /// </summary>
        public CircuitConfig mConfig;
        /// <summary>
        /// 各装置の入力ポートの使用数
        /// </summary>
        public Vector<Integer> mNumPortsIn;
        /// <summary>
        /// 各装置の出力ポートの使用数
        /// </summary>
        public Vector<Integer> mNumPortsOut;
        private WaveGenerator mGenerator;
        private long mSamples;
        /// <summary>
        /// 装置同士が接続されているかどうかを表すマッピング。
        /// 行、列ともに大きさmUnits.size()の正方行列となっている。
        /// 第i番目の装置の出力ポートが、第j番目の入力ポートにつながっている時、mConnectionMap[i, j] = true。
        /// </summary>
        private boolean[,] mConnectionMap;

        private Circuit() {
        }

        /// <summary>
        /// 第i番目の装置と第j番目の装置が接続されているかどうかを取得します
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public boolean isConnected( int i, int j ) {
            return mConnectionMap[i, j];
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
            mConfig = config;

            // ユニットを格納する配列を作成
            int num = 0;
            if ( mConfig != null && mConfig.Units != null ) {
                num = mConfig.Units.size();
            }
            mUnits = new Vector<WaveUnit>();
            mNumPortsIn = new Vector<Integer>();
            mNumPortsOut = new Vector<Integer>();
            for ( int i = 0; i < num; i++ ) {
                mNumPortsIn.add( 0 );
                mNumPortsOut.add( 0 );
            }
            mConnectionMap = new boolean[num, num];
            for ( int i = 0; i < num; i++ ) {
                for ( int j = 0; j < num; j++ ) {
                    mConnectionMap[i, j] = false;
                }
            }

            // 型名と型のマッピング
            TreeMap<String, Type> map = new TreeMap<String, Type>();

            // ユニットのインスタンスを生成．
            for ( int i = 0; i < num; i++ ) {
                // 型名
                String classname = mConfig.Units.get( i );
                // 型名から型を取得
                Type unit_type = null;
                if ( map.containsKey( classname ) ) {
                    unit_type = map.get( classname );
                } else {
                    unit_type = Type.GetType( classname );
                    map.put( classname, unit_type );
                }

                // リフレクションによりインスタンスを生成
                WaveUnit instance = null;
                System.Reflection.ConstructorInfo ctor = unit_type.GetConstructor( Type.EmptyTypes );
                if ( ctor == null ) {
                    if ( unit_type == typeof( MonitorWaveReceiver ) ) {
                        instance = MonitorWaveReceiver.getInstance();
                    } else {
                        PortUtil.stderr.println( "Circuit#.ctor; cannot get default constructor for \"" + classname + "\"" );
                    }
                } else {
                    instance = (WaveUnit)ctor.Invoke( null );
                }

                // 初期化
                instance.setGlobalConfig( editor_config );
                instance.setConfig( mConfig.Arguments.get( i ) );

                // Generatorかどうか？
                if ( instance is WaveGenerator ) {
                    mGenerator = (WaveGenerator)instance;
                }

                // 格納
                mUnits.add( instance );
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
#if DEBUG
                    String arrow = entry.ConnectionKind == CircuitConnectionKind.RECEIVER ? "->" : "<-";
#endif
                    if ( entry.ConnectionKind == CircuitConnectionKind.RECEIVER ) {
                        // 相手をレシーバとして接続
                        if ( unit_col is WaveReceiver ) {
                            WaveReceiver receiver = (WaveReceiver)unit_col;
                            boolean add_port = false;
                            if ( unit_row is WaveReceiver ) {
                                WaveReceiver unit_row_as_receiver = (WaveReceiver)unit_row;
                                unit_row_as_receiver.setReceiver( receiver );
                                add_port = true;
#if DEBUG
                                PortUtil.println( "Circuit#.ctor; connection done; " + mConfig.Units.get( row ) + arrow + mConfig.Units.get( col ) );
#endif
                            } else if ( unit_row is WaveGenerator ) {
                                WaveGenerator unit_row_as_generator = (WaveGenerator)unit_row;
                                unit_row_as_generator.setReceiver( receiver );
                                add_port = true;
#if DEBUG
                                PortUtil.println( "Circuit#.ctor; connection done; " + mConfig.Units.get( row ) + arrow + mConfig.Units.get( col ) );
#endif
                            }

                            if ( add_port ) {
                                mNumPortsIn.set( col, mNumPortsIn.get( col ) + 1 );
                                mNumPortsOut.set( row, mNumPortsOut.get( row ) + 1 );
                                mConnectionMap[row, col] = true;
                            }
                        }
                    } else if ( entry.ConnectionKind == CircuitConnectionKind.SENDER ) {
                        // 相手をセンダーとして接続
                        if ( unit_col is WaveSender ) {
                            WaveSender sender = (WaveSender)unit_col;
                            boolean add_port = false;
                            if ( unit_row is WaveSender ) {
                                WaveSender unit_row_as_sender = (WaveSender)unit_row;
                                unit_row_as_sender.setSender( sender );
                                add_port = true;
#if DEBUG
                                PortUtil.println( "Circuit#.ctor; connection done; " + mConfig.Units.get( row ) + arrow + mConfig.Units.get( col ) );
#endif
                            }

                            if ( add_port ) {
                                mNumPortsIn.set( row, mNumPortsIn.get( row ) + 1 );
                                mNumPortsOut.set( col, mNumPortsOut.get( col ) + 1 );
                                mConnectionMap[col, row] = true;
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
