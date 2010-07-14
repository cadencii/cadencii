/*
 * BBackgroundWorker.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ../BuildJavaUI/src/org/kbinani/componentmodel/BBackgroundWorker.java
#else
using org.kbinani;

namespace org.kbinani.componentmodel {
    public class BBackgroundWorker : System.ComponentModel.BackgroundWorker {
        // root impl of DoWork event
        #region impl of DoWork
        // root impl of DoWork is in BBackgroundWorker
        public BEvent<BDoWorkEventHandler> doWorkEvent = new BEvent<BDoWorkEventHandler>();
        protected override void OnDoWork( System.ComponentModel.DoWorkEventArgs e ) {
#if DEBUG
            PortUtil.println( "BBackgroundWorker#OnDoWork" );
#endif
            base.OnDoWork( e );
            doWorkEvent.raise( this, e );
        }
        #endregion

        // root impl of RunWorkerCompleted event
        #region impl of RunWorkerCompleted
        // root impl of RunWorkerCompleted is in BBackgroundWorker
        public BEvent<BRunWorkerCompletedEventHandler> runWorkerCompletedEvent = new BEvent<BRunWorkerCompletedEventHandler>();
        protected override void OnRunWorkerCompleted( System.ComponentModel.RunWorkerCompletedEventArgs e ) {
            base.OnRunWorkerCompleted( e );
            runWorkerCompletedEvent.raise( this, e );
        }
        #endregion

        public bool isWorkerReportsProgress() {
            return base.WorkerReportsProgress;
        }

        public void setWorkerReportsProgress( bool value ) {
            base.WorkerReportsProgress = value;
        }

        public bool isCancellationPending() {
            return base.CancellationPending;
        }

        public bool isBusy() {
            return base.IsBusy;
        }

        public void cancelAsync() {
            base.CancelAsync();
        }

        public void runWorkerAsync() {
            base.RunWorkerAsync();
        }

        public void runWorkerAsync( object argument ) {
            base.RunWorkerAsync( argument );
        }

        public void reportProgress( int percent_progress ) {
            reportProgress( percent_progress, null );
        }

        public void reportProgress( int percent_progress, object user_status ) {
            base.ReportProgress( percent_progress, user_status );
        }
    }
}
#endif
