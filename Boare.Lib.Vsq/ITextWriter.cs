#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace Boare.Lib.Vsq
{
#endif

    public interface ITextWriter
    {
#if JAVA
        void write( String value ) throws IOException;
        void writeLine( String value ) throws IOException;
        void close() throws IOException;
#else
        void write( String value );
        void writeLine( String value );
        void close();
#endif
    }

#if !JAVA
}
#endif
