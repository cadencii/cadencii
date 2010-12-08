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
