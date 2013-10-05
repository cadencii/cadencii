/*
 * FormWorker.cs
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
using System;
using System.Threading;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii
{

    class FormWorkerJobArgument
    {
        public Object invoker;
        public string name;
        public Object arguments;
        public int index;
        public WorkerState state;
    }

    class FormWorkerJobStateImp : WorkerState
    {
        private BackgroundWorker mWorker;
        private bool mIsCancelRequested;
        private double mJobAmount;
        private double mProcessed;

        public FormWorkerJobStateImp(BackgroundWorker worker, double job_amount)
        {
            mWorker = worker;
            mIsCancelRequested = false;
            mJobAmount = job_amount;
        }

        public void reportProgress(double processed_job)
        {
            mProcessed = processed_job;
            int percent = (int)(mProcessed / mJobAmount * 100.0);
            mWorker.ReportProgress(percent);
        }

        public double getProcessedAmount()
        {
            return mProcessed;
        }

        public double getJobAmount()
        {
            return mJobAmount;
        }

        public bool isCancelRequested()
        {
            return mIsCancelRequested;
        }

        public void requestCancel()
        {
            mIsCancelRequested = true;
        }

        public void reportComplete()
        {
            // do nothing
        }
    }

    /// <summary>
    /// 複数のジョブを順に実行し，その進捗状況を表示するダイアログを表示します
    /// </summary>
    public class FormWorker : IFormWorkerControl
    {
        private FormWorkerUi ptrUi = null;
        private List<ProgressBarWithLabel> mLabels;
        private mman mMemManager;
        private List<FormWorkerJobArgument> mArguments;
        private List<BackgroundWorker> mThreads;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormWorker()
        {
            mLabels = new List<ProgressBarWithLabel>();
            mMemManager = new mman();
            mArguments = new List<FormWorkerJobArgument>();

            mThreads = new List<BackgroundWorker>();
        }

        /// <summary>
        /// 登録済みのジョブを開始します
        /// </summary>
        public void startJob()
        {
            int size = mLabels.Count;
            if (size <= 0) return;
            startWorker(0);
        }

        /// <summary>
        /// ビューのセットアップを行います
        /// </summary>
        /// <param name="value"></param>
        public void setupUi(FormWorkerUi value)
        {
            ptrUi = value;
            ptrUi.applyLanguage();
        }

        /// <summary>
        /// ジョブを追加します．objで指定したオブジェクトの，名前がnameであるメソッドを呼び出します．
        /// 当該メソッドは，戻り値は任意，引数は( WorkerState, Object )である必要があります．
        /// また，当該メソッドは第一引数で渡されたWorkerStateのインスタンスのisCancelRequestedメソッドを
        /// 監視し，その戻り値がtrueの場合速やかに処理を中止しなければなりません．その際，処理の中止後にreportCompleteの呼び出しを
        /// 行ってはいけません．
        /// </summary>
        /// <param name="obj">メソッドの呼び出し元となるオブジェクト</param>
        /// <param name="method_name">メソッドの名前</param>
        /// <param name="job_description">ジョブの概要</param>
        /// <param name="job_amount">ジョブの処理量を表す，何らかの量．</param>
        /// <param name="argument">メソッドの第二引数</param>
        public void addJob(Object obj, string method_name, string job_description, double job_amount, Object argument)
        {
            // プログレスバーのUIを作成
            ProgressBarWithLabelUi ui = new ProgressBarWithLabelUi();
            ProgressBarWithLabel label = new ProgressBarWithLabel();
            label.setupUi(ui);
            label.setText(job_description);
            // フォームのビューにUIを追加
            ptrUi.addProgressBar(ui);

            // ラベルのリストに登録
            int index = mLabels.Count;
            mLabels.Add(label);
            mMemManager.add(label);

            // スレッドを作成して起動(platform依存)
            FormWorkerJobArgument arg = new FormWorkerJobArgument();
            arg.invoker = obj;
            arg.name = method_name;
            arg.arguments = argument;
            arg.index = index;
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            mThreads.Add(worker);

            arg.state = new FormWorkerJobStateImp(worker, job_amount);
            mArguments.Add(arg);
        }

        /// <summary>
        /// ジョブをキャンセルします(非同期)
        /// </summary>
        public void cancelJobSlot()
        {
            int size = mArguments.Count;
            for (int i = 0; i < size; i++) {
                FormWorkerJobArgument arg = mArguments[i];
                arg.state.requestCancel();
            }
        }

        /// <summary>
        /// ビューのインスタンスを取得します
        /// </summary>
        /// <returns></returns>
        public FormWorkerUi getUi()
        {
            return ptrUi;
        }

        public void workerProgressChanged(int index, int percentage)
        {
            ProgressBarWithLabel label = mLabels[index];
            if (label != null) {
                label.setProgress(percentage);
            }
            int size = mArguments.Count;
            double total = 0.0;
            double processed = 0.0;
            for (int i = 0; i < size; i++) {
                FormWorkerJobArgument arg = mArguments[i];
                total += arg.state.getJobAmount();
                if (i < index) {
                    processed += arg.state.getJobAmount();
                } else if (i == index) {
                    processed += arg.state.getProcessedAmount();
                }
            }
            ptrUi.setTotalProgress((int)(processed / total * 100.0));
            ptrUi.Refresh();
        }

        public void workerCompleted(int index)
        {
#if DEBUG
            sout.println("FormWorker#workerCompleted; index=" + index);
#endif
            ProgressBarWithLabel label = mLabels[index];
            ptrUi.removeProgressBar(label.getUi());
            mman.del(label);
            mLabels[index] = null;
            int size = mLabels.Count;
            index++;
            if (index < size) {
                startWorker(index);
            } else {
                ptrUi.close(false);//.setDialogResult( BDialogResult.OK );//.close();
            }
        }

        private void startWorker(int index)
        {
            FormWorkerJobArgument arg = mArguments[index];
            mThreads[index].RunWorkerAsync(arg);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int size = mThreads.Count;
            for (int i = 0; i < size; i++) {
                BackgroundWorker w = mThreads[i];
                if (w == null) continue;
                if (w == sender) {
                    workerProgressChanged(i, e.ProgressPercentage);
                    break;
                }
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int size = mThreads.Count;
            for (int i = 0; i < size; i++) {
                BackgroundWorker w = mThreads[i];
                if (w == null) continue;
                if (w == sender) {
                    WorkerState state = mArguments[i].state;
                    if (state.isCancelRequested() == false) {
                        workerCompleted(i);
                    }
                    break;
                }
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            FormWorkerJobArgument o = (FormWorkerJobArgument)e.Argument;
            MethodInfo mi = null;
            try {
                Type type = null;
                if (o.invoker is Type) {
                    type = (Type)o.invoker;
                } else {
                    type = o.invoker.GetType();
                }
                mi = type.GetMethod(o.name, new Type[] { typeof(WorkerState), typeof(Object) });
            } catch (Exception ex) {
                serr.println(typeof(FormWorker) + ".startWork; ex=" + ex);
            }
            if (mi != null) {
                try {
                    mi.Invoke(o.invoker, new object[] { o.state, o.arguments });
                } catch (Exception ex) {
                    serr.println(typeof(FormWorker) + ".startWork; ex=" + ex);
                }
            }
        }
    }

}
