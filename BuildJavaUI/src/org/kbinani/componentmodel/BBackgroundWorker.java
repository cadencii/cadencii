package org.kbinani.componentmodel;

import org.kbinani.BEvent;

public class BBackgroundWorker{
    public BEvent<BDoWorkEventHandler> doWorkEvent = new BEvent<BDoWorkEventHandler>();
    public BEvent<BProgressChangedEventHandler> progressChangedEvent = new BEvent<BProgressChangedEventHandler>();
    public BEvent<BRunWorkerCompletedEventHandler> runWorkerCompletedEvent = new BEvent<BRunWorkerCompletedEventHandler>();
    private WorkerRunner m_runner = null;
    private Thread thread = null;

    class WorkerRunner implements Runnable{
        private BDoWorkEventArgs m_arg = null;
        private BEvent<BDoWorkEventHandler> m_delegate = null;
        private boolean isBusy = false;
        private BBackgroundWorker mParent = null;
        
        public WorkerRunner( BBackgroundWorker parent, BEvent<BDoWorkEventHandler> delegate, Object argument ){
            mParent = parent;
            m_delegate = delegate;
            m_arg = new BDoWorkEventArgs( argument );
        }

        public void run(){
            isBusy = true;
            try{
                m_delegate.raise( mParent, m_arg );
                BRunWorkerCompletedEventArgs e = new BRunWorkerCompletedEventArgs( null, null, false );
                runWorkerCompletedEvent.raise( mParent, e );
            }catch( Exception ex ){
                System.err.println( "BBackgroundWorker#WorkerRunner#run(void); ex=" + ex );
            }
            isBusy = false;
        }
    }

    public boolean isBusy(){
        if( m_runner == null ){
            return false;
        }else{
            return m_runner.isBusy;
        }
    }
    
    public void cancelAsync(){
        thread.interrupt();
        System.err.println( "info; BBackgroundWorker#cancelAsync" );
    }
    
    public BBackgroundWorker(){
    }
    
    public void runWorkerAsync(){
        runWorkerAsync( null );
    }

    public void runWorkerAsync( Object argument ){
        m_runner = new WorkerRunner( this, doWorkEvent, argument );
        thread = new Thread( m_runner );
        thread.start();
    }

    public void reportProgress( int percentProgress ){
        reportProgress( percentProgress, null );
    }
    
    public void reportProgress( int percentProgress, Object userState ){
        BProgressChangedEventArgs e = new BProgressChangedEventArgs( percentProgress, userState );
        try{
            progressChangedEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BBackgroundWorker#reportProgress(int,Object); ex=" + ex );
        }
    }
}
