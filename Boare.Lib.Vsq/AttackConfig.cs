/*
 * AttackConfig.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package com.boare.vsq;
import java.io.*;
#else
using System;
using System.IO;

using bocoree;

namespace Boare.Lib.Vsq {
#endif

    public class AttackConfig {
        public int number;
        public String file;
        public String author;
        public String vendor;
        public NoteHeadHandle contents;

        public AttackConfig() {
            contents = new NoteHeadHandle();
        }

#if JAVA
        public String toString(){
#else
        public override string ToString() {
#endif
            if ( contents != null ) {
                return contents.Caption;
            } else {
#if JAVA
                return super.toString();
#else
                return base.ToString();
#endif
            }
        }

        public void parseAic( String aic_file ) {
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new FileReader( aic_file ) );
                String line;
                String current_entry = "";
                String articulation = "";
                while ( (line = sr.readLine()) != null ) {
                    if ( line.StartsWith( "[" ) ) {
                        current_entry = line;
                        continue;
                    } else if ( line.Equals( "" ) || line.StartsWith( ";" ) ) {
                        continue;
                    }

                    String[] spl = PortUtil.splitString( line, new char[] { '=' }, true );
                    if ( spl.Length < 2 ) {
                        continue;
                    }
                    spl[0] = spl[0].Trim();
                    spl[1] = spl[1].Trim();
                    if ( current_entry.Equals( "[Common]" ) ) {
                        if ( spl[0].Equals( "Articulation" ) ) {
                            articulation = spl[1];
                        }
                    } else if ( current_entry.Equals( "[Parameter]" ) ) {
                        if ( spl[0].Equals( "Length" ) ) {
                            try {
                                this.contents.Length = PortUtil.parseInt( spl[1] );
                            } catch { }
                        } else if ( spl[0].Equals( "Duration" ) ) {
                            try {
                                this.contents.Duration = PortUtil.parseInt( spl[1] );
                            } catch { }
                        } else if ( spl[0].Equals( "Depth" ) ) {
                            try {
                                this.contents.Depth = PortUtil.parseInt( spl[1] );
                            } catch { }
                        }
                    }
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }
    }
#if !JAVA
}
#endif
