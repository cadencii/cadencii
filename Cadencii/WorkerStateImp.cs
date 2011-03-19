#if JAVA

package org.kbinani.cadencii;

#elif __cplusplus

namespace org{
namespace kbinani{
namespace cadencii{

#else

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
#endif

    public class WorkerStateImp : WorkerState
    {
        private boolean mCancelRequested;

        public double getJobAmount()
        {
            return 0;
        }

        public double getProcessedAmount()
        {
            return 0;
        }

        public boolean isCancelRequested()
        {
            return mCancelRequested;
        }

        public void reportComplete()
        {
        }

        public void reportProgress( double prog )
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

#if JAVA
#elif __cplusplus
} } }
#else
}
#endif
