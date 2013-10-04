#if DEBUG
/*
 * TestFormWorker.cs
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
#if JAVA

package cadencii;

import cadencii.*;

#else

using System;
using System.Threading;

namespace cadencii
{
#endif

    public class TestFormWorker
    {
        public static void run()
        {
            FormWorker fw = new FormWorker();
            fw.setupUi( new FormWorkerUi( fw ) );
            fw.getUi().show( null );
            TestFormWorker test_instance = new TestFormWorker();
            fw.addJob( test_instance, "testMethod", "job1", 5000, 5000 );
            fw.addJob( test_instance, "testMethod", "job2", 10000, 10000 );
            fw.addJob( test_instance, "testMethod", "job3", 15000, 15000 );
            fw.getUi().setTitle( "test title" );
            fw.getUi().setText( "test message" );
            fw.startJob();
            //Thread.Sleep( 10000 );
            //fw.cancelJob();
        }

        public void testMethod( WorkerState receiver, Object argument )
        {
            int wait_ms = 10;
            int amount = ((int)argument) / wait_ms;
            int proc = 0;
            for ( int i = 0; i < amount; i++ ) {
                // 中止処理後にreportCompleteを送ってはいけない
                if ( receiver.isCancelRequested() ) {
                    sout.println( "TestFormWorker#testMethod; cancel requested" );
                    return;
                }
                receiver.reportProgress( proc );
#if JAVA
                try{
                    Thread.sleep( wait_ms );
                }catch( Exception ex ){
                }
#else
                Thread.Sleep( wait_ms );
#endif
                proc += wait_ms;
            }
            receiver.reportComplete();
            sout.println( "TestFormWorker#testMethod; job done" );
        }
    }

#if !JAVA
}
#endif

#endif // DEBUG
