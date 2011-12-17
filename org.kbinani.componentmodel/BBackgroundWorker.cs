/*
 * BBackgroundWorker.cs
 * Copyright © 2009-2011 kbinani
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
//INCLUDE ./BBackgroundWorker.java
#else
using com.github.cadencii;

namespace com.github.cadencii.componentmodel {
    public class BBackgroundWorker : System.ComponentModel.BackgroundWorker {
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
