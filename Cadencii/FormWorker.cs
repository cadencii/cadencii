/*
 * FormWorker.cs
 * Copyright © 2011 kbinani
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
using System.Threading;
using System.ComponentModel;
using System.Reflection;
using org.kbinani.java.util;

namespace org.kbinani.cadencii
{
#endif

    class FormWorkerJobArgument
    {
        public Object invoker;
        public String name;
        public Object arguments;
        public int index;
        public FormWorkerJobState state;
    }

#if JAVA
#elif __cplusplus
#else
    class FormWorkerJobStateImp : FormWorkerJobState
    {
        private BackgroundWorker mWorker;
        private bool mIsCancelRequested;
        private double mJobAmount;
        private double mProcessed;

        public FormWorkerJobStateImp( BackgroundWorker worker, double job_amount )
        {
            mWorker = worker;
            mIsCancelRequested = false;
            mJobAmount = job_amount;
        }

        public void reportProgress( double processed_job )
        {
            mProcessed = processed_job;
            int percent = (int)(mProcessed / mJobAmount * 100.0);
            mWorker.ReportProgress( percent );
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
#endif

    /// <summary>
    /// 複数のジョブを順に実行し，その進捗状況を表示するダイアログを表示します
    /// </summary>
    public class FormWorker
    {
        private FormWorkerUi ptrUi = null;
        private Vector<ProgressBarWithLabel> mLabels;
        private mman mMemManager;
        private Vector<FormWorkerJobArgument> mArguments;
#if JAVA
#elif __cplusplus
#else
        private Vector<BackgroundWorker> mThreads;
#endif

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormWorker()
        {
#if __cplusplus
#else
            mLabels = new Vector<ProgressBarWithLabel>();
            mMemManager = new mman();
            mArguments = new Vector<FormWorkerJobArgument>();
#endif

#if JAVA
#elif __cplusplus
#else
            mThreads = new Vector<BackgroundWorker>();
#endif
        }

        /// <summary>
        /// 登録済みのジョブを開始します
        /// </summary>
        public void startJob()
        {
            int size = vec.size( mLabels );
            if ( size <= 0 ) return;
            startWorker( 0 );
        }

        /// <summary>
        /// ビューのセットアップを行います
        /// </summary>
        /// <param name="value"></param>
        public void setupUi( FormWorkerUi value )
        {
            ptrUi = value;
        }

        /// <summary>
        /// ジョブを追加します．objで指定したオブジェクトの，名前がnameであるメソッドを呼び出します．
        /// 当該メソッドは，戻り値は任意，引数は( FormWorkerProgressReceiver, Object )である必要があります．
        /// また，当該メソッドは第一引数で渡されたFormWorkerProgressReceiverのインスタンスのisCancelRequestedメソッドを
        /// 監視し，その戻り値がtrueの場合速やかに処理を中止しなければなりません．その際，処理の中止後にreportCompleteの呼び出しを
        /// 行ってはいけません．
        /// </summary>
        /// <param name="obj">メソッドの呼び出し元となるオブジェクト</param>
        /// <param name="method_name">メソッドの名前</param>
        /// <param name="job_description">ジョブの概要</param>
        /// <param name="job_amount">ジョブの処理量を表す，何らかの量．</param>
        /// <param name="argument">メソッドの第二引数</param>
        public void addJob( Object obj, String method_name, String job_description, double job_amount, Object argument )
        {
            // プログレスバーのUIを作成
            ProgressBarWithLabelUi ui = new ProgressBarWithLabelUi();
            ProgressBarWithLabel label = new ProgressBarWithLabel();
            label.setupUi( ui );
            label.setText( job_description );
            // フォームのビューにUIを追加
            ptrUi.addProgressBar( ui );
            
            // ラベルのリストに登録
            int index = vec.size( mLabels );
            vec.add( mLabels, label );
            mMemManager.add( label );

            // スレッドを作成して起動(platform依存)
            FormWorkerJobArgument arg = new FormWorkerJobArgument();
            arg.invoker = obj;
            arg.name = method_name;
            arg.arguments = argument;
            arg.index = index;
#if JAVA
            // TODO:
#elif __cplusplus
            // TODO:
#else
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler( worker_DoWork );
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler( worker_RunWorkerCompleted );
            worker.ProgressChanged += new ProgressChangedEventHandler( worker_ProgressChanged );
            vec.add( mThreads, worker );

            arg.state = new FormWorkerJobStateImp( worker, job_amount );
#endif
            vec.add( mArguments, arg );
        }

        /// <summary>
        /// ジョブをキャンセルします(非同期)
        /// </summary>
        public void cancelJobSlot()
        {
            int size = vec.size( mArguments );
            for ( int i = 0; i < size; i++ ) {
                FormWorkerJobArgument arg = vec.get( mArguments, i );
                arg.state.requestCancel();
            }
        }

        /// <summary>
        /// フォームを表示します
        /// </summary>
        public void show()
        {
            if ( ptrUi == null ) return;
            ptrUi.show();
        }

        /// <summary>
        /// ビューのインスタンスを取得します
        /// </summary>
        /// <returns></returns>
        public FormWorkerUi getUi()
        {
            return ptrUi;
        }

        private void workerProgressChanged( int index, int percentage )
        {
            ProgressBarWithLabel label = vec.get( mLabels, index );
            if ( label != null ) {
                label.setProgress( percentage );
            }
            int size = vec.size( mArguments );
            double total = 0.0;
            double processed = 0.0;
            for ( int i = 0; i < size; i++ ) {
                FormWorkerJobArgument arg = vec.get( mArguments, i );
                total += arg.state.getJobAmount();
                if ( i < index ) {
                    processed += arg.state.getJobAmount();
                } else if ( i == index ) {
                    processed += arg.state.getProcessedAmount();
                }
            }
            ptrUi.setTotalProgress( (int)(processed / total * 100.0) );
        }

        private void workerCompleted( int index )
        {
            ProgressBarWithLabel label = vec.get( mLabels, index );
            ptrUi.removeProgressBar( label.getUi() );
            mman.del( label );
            vec.set( mLabels, index, null );
            int size = vec.size( mLabels );
            index++;
            if ( index < size ) {
                startWorker( index );
            } else {
                ptrUi.close();
            }
        }

        private void startWorker( int index )
        {
            FormWorkerJobArgument arg = vec.get( mArguments, index );
#if JAVA
#elif __cplusplus
#else
            vec.get( mThreads, index ).RunWorkerAsync( arg );
#endif
        }

#if JAVA
#elif __cplusplus
#else
        private void worker_ProgressChanged( object sender, ProgressChangedEventArgs e )
        {
            int size = mThreads.size();
            for ( int i = 0; i < size; i++ ) {
                BackgroundWorker w = mThreads[i];
                if ( w == null ) continue;
                if ( w == sender ) {
                    workerProgressChanged( i, e.ProgressPercentage );
                    break;
                }
            }
        }

        private void worker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            int size = mThreads.size();
            for ( int i = 0; i < size; i++ ) {
                BackgroundWorker w = mThreads[i];
                if ( w == null ) continue;
                if ( w == sender ) {
                    workerCompleted( i );
                    break;
                }
            }
        }

        private void worker_DoWork( object sender, DoWorkEventArgs e )
        {
            FormWorkerJobArgument o = (FormWorkerJobArgument)e.Argument;
            MethodInfo mi = null;
            try {
                mi = o.invoker.GetType().GetMethod( o.name, new Type[] { typeof( FormWorkerJobState ), typeof( Object ) } );
            } catch ( Exception ex ) {
                serr.println( typeof( FormWorker ) + ".startWork; ex=" + ex );
            }
            if ( mi != null ) {
                try {
                    mi.Invoke( o.invoker, new object[] { o.state, o.arguments } );
                } catch ( Exception ex ) {
                    serr.println( typeof( FormWorker ) + ".startWork; ex=" + ex );
                }
            }
        }
#endif
    }

#if !JAVA
}
#endif
