/*
 * BBackgroundWorker.cs
 * Copyright (C) 2009 kbinani
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\componentModel\BBackgroundWorker.java
#else
using org.kbinani;

namespace org.kbinani.componentmodel {
    public class BBackgroundWorker : System.ComponentModel.BackgroundWorker {
        // root impl of DoWork event
        #region impl of DoWork
        // root impl of DoWork is in BBackgroundWorker
        public BEvent<BDoWorkEventHandler> doWorkEvent = new BEvent<BDoWorkEventHandler>();
        protected override void OnDoWork( System.ComponentModel.DoWorkEventArgs e ) {
            base.OnDoWork( e );
            doWorkEvent.raise( this, e );
        }
        #endregion

        public bool isBusy() {
            return base.IsBusy;
        }

        public void cancelAsync() {
            base.CancelAsync();
        }

        public void runWorkerAsync() {
            base.RunWorkerAsync();
        }
    }
}
#endif
