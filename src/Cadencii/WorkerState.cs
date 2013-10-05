/*
 * WorkerState.cs
 * Copyright © 2011 kbinani
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
namespace cadencii
{

    /// <summary>
    /// FormWorkerに登録されている1個のジョブについての状態を表現します
    /// </summary>
    public interface WorkerState
    {
        /// <summary>
        /// workerスレッドから呼び出し元に進捗を通知します
        /// </summary>
        /// <param name="processed_job">ジョブの処理済み量</param>
        void reportProgress(double processed_job);

        /// <summary>
        /// workerスレッドから呼び出し元に，workerスレッドの処理が完了したことを通知します
        /// </summary>
        void reportComplete();

        /// <summary>
        /// workerスレッドが，キャンセル要求の有無を呼び出し元に問い合せるためのメソッド
        /// </summary>
        /// <returns></returns>
        bool isCancelRequested();

        /// <summary>
        /// 呼び出し元が，workerスレッドにキャンセル要求を出すためのメソッド
        /// </summary>
        void requestCancel();

        /// <summary>
        /// ジョブの現在の処理量を取得します
        /// </summary>
        /// <returns></returns>
        double getProcessedAmount();

        /// <summary>
        /// ジョブの総処理量を取得します
        /// </summary>
        /// <returns></returns>
        double getJobAmount();
    }

}
