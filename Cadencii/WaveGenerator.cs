/*
 * WaveGenerator.cs
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
#else
using System;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 音声波形の生成器のためのインターフェース．
    /// このインターフェースを実装するクラスは，WaveUnitクラスを継承すること．
    /// このインターフェースのメソッドは全て同期的とすること
    /// </summary>
    public interface WaveGenerator {
        /// <summary>
        /// 音声波形の合成を開始します．
        /// このメソッドの前には，setGlobalConfig, setConfig, initメソッドをこの順で呼び出して
        /// 必要なパラメータを全て渡すようにしてください．
        /// (この順番に呼ばれることを前提とした実装をしなくてはならない)
        /// </summary>
        /// <param name="samples"></param>
        void begin( long samples );

        /// <summary>
        /// この音声波形器が生成した波形を受け取る装置を設定します．
        /// </summary>
        /// <param name="r"></param>
        void setReceiver( WaveReceiver r );

        /// <summary>
        /// 音声波形の合成に必要な引数を設定します．
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="start_clock"></param>
        /// <param name="end_clock"></param>
        void init( VsqFileEx vsq, int track, int start_clock, int end_clock );

        /// <summary>
        /// 停止を要求します．
        /// 停止が完了するまで処理は戻りません
        /// </summary>
        void stop();

        /// <summary>
        /// エディターの設定値を指定します
        /// </summary>
        /// <param name="config"></param>
        void setGlobalConfig( EditorConfig config );

        /// <summary>
        /// 合成処理の進捗状況を取得します．
        /// 戻り値は0から1までとなります
        /// </summary>
        /// <returns></returns>
        double getProgress();

        /// <summary>
        /// 合成処理の進捗状況を取得します．
        /// 単位はサンプル数です
        /// </summary>
        /// <returns></returns>
        long getPosition();

        /// <summary>
        /// beginメソッドで指定された，合成処理を行う合計のサンプル数を取得します
        /// </summary>
        /// <returns></returns>
        long getTotalSamples();

        /// <summary>
        /// 合成処理が実行中かどうかを取得します
        /// </summary>
        /// <returns></returns>
        boolean isRunning();
    }

#if !JAVA
}
#endif
