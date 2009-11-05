using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Boare.Lib.Vsq;
using bocoree.io;

namespace Boare.CadenciiScript {
    class UTAU_Plugin_Manager {
    }

    class TestPlugin {
        private void a() {
            Thread t = new Thread( new ParameterizedThreadStart( run ) );
            t.Start();
        }

        private void run( object arg ) {
            UstFile ust = new UstFile( "a" );
            UstTrack track;
            UstEvent itemj;
            BufferedWriter sw = new BufferedWriter( new OutputStreamWriter( new FileInputStream( "" ), "Shift_JIS" ) );
            //itemj.ToString
        }
    }

}
