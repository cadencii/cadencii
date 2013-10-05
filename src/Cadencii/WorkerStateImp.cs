/*
 * WorkerStateImp.cs
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

    public class WorkerStateImp : WorkerState
    {
        private bool mCancelRequested;

        public double getJobAmount()
        {
            return 0;
        }

        public double getProcessedAmount()
        {
            return 0;
        }

        public bool isCancelRequested()
        {
            return mCancelRequested;
        }

        public void reportComplete()
        {
        }

        public void reportProgress(double prog)
        {
        }

        public void requestCancel()
        {
            mCancelRequested = true;
        }

        public void reset()
        {
            mCancelRequested = false;
        }
    }

}
