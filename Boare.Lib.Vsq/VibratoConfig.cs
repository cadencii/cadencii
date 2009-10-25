/*
 * VibratoConfig.cs
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
package org.kbinani.vsq;

import java.io.*;
import org.kbinani.*;
#else
using System;
using bocoree;
using bocoree.io;

namespace Boare.Lib.Vsq {
#endif

    public class VibratoConfig {
        public int number;
        public String file;
        public String author;
        public String vendor;
        public VibratoHandle contents;

        public override String ToString() {
            if ( contents == null ) {
                return base.ToString();
            } else {
                return contents.Caption;
            }
        }

        public VibratoConfig() {
            contents = new VibratoHandle();
        }

        public void parseAic( String aic_file ) {
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new FileReader( aic_file ) );
                String line;
                String current_entry = "";
                String articulation = "";
                String depth_bpx = "";
                String depth_bpy = "";
                String rate_bpx = "";
                String rate_bpy = "";
                int depth_bpnum = 0;
                int rate_bpnum = 0;
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
                                this.contents.setLength( PortUtil.parseInt( spl[1] ) );
                            } catch ( Exception ex0 ) {
                            }
                        } else if ( spl[0].Equals( "StartDepth" ) ) {
                            try {
                                this.contents.StartDepth = PortUtil.parseInt( spl[1] );
                            } catch ( Exception ex0 ) {
                            }
                        } else if ( spl[0].Equals( "DepthBPNum" ) ) {
                            try {
                                depth_bpnum = PortUtil.parseInt( spl[1] );
                            } catch ( Exception ex0 ) {
                            }
                        } else if ( spl[0].Equals( "DepthBPX" ) ) {
                            depth_bpx = spl[1];
                        } else if ( spl[0].Equals( "DepthBPY" ) ) {
                            depth_bpy = spl[1];
                        } else if ( spl[0].Equals( "StartRate" ) ) {
                            try {
                                this.contents.StartRate = PortUtil.parseInt( spl[1] );
                            } catch ( Exception ex0 ) {
                            }
                        } else if ( spl[0].Equals( "RateBPNum" ) ) {
                            try {
                                rate_bpnum = PortUtil.parseInt( spl[1] );
                            } catch ( Exception ex0 ) {
                            }
                        } else if ( spl[0].Equals( "RateBPX" ) ) {
                            rate_bpx = spl[1];
                        } else if ( spl[0].Equals( "RateBPY" ) ) {
                            rate_bpy = spl[1];
                        }
                    }
                }
                if ( !articulation.Equals( "Vibrato" ) ) {
                    return;
                }

                // depth bp
                if ( depth_bpnum > 0 && !depth_bpx.Equals( "" ) && !depth_bpy.Equals( "" ) ) {
                    String[] bpx = PortUtil.splitString( depth_bpx, ',' );
                    String[] bpy = PortUtil.splitString( depth_bpy, ',' );
                    if ( depth_bpnum == bpx.Length && depth_bpnum == bpy.Length ) {
                        float[] x = new float[depth_bpnum];
                        int[] y = new int[depth_bpnum];
                        try {
                            for ( int i = 0; i < depth_bpnum; i++ ) {
                                x[i] = PortUtil.parseFloat( bpx[i] );
                                y[i] = PortUtil.parseInt( bpy[i] );
                            }
                            this.contents.DepthBP = new VibratoBPList( x, y );
                        } catch ( Exception ex0 ) {
                        }
                    }
                }

                // rate bp
                if ( rate_bpnum > 0 && !rate_bpx.Equals( "" ) && !rate_bpy.Equals( "" ) ) {
                    String[] bpx = PortUtil.splitString( rate_bpx, ',' );
                    String[] bpy = PortUtil.splitString( rate_bpy, ',' );
                    if ( rate_bpnum == bpx.Length && rate_bpnum == bpy.Length ) {
                        float[] x = new float[rate_bpnum];
                        int[] y = new int[rate_bpnum];
                        try {
                            for ( int i = 0; i < rate_bpnum; i++ ) {
                                x[i] = PortUtil.parseFloat( bpx[i] );
                                y[i] = PortUtil.parseInt( bpy[i] );
                            }
                            this.contents.RateBP = new VibratoBPList( x, y );
                        } catch ( Exception ex0 ) {
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
