/*
 * VibratoConfig.cs
 * Copyright (C) 2009 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.io;

namespace org.kbinani.vsq {
#endif

    /*public class VibratoConfig : IconParameter {
        public int number;
        public String file;
        public String author;
        public String vendor;
        public String IconID = "";
        public String IDS = "";
        public int Original;

        public override String ToString() {
            return caption;
        }

        public VibratoConfig()
            : base( "" ) {
        }

        public VibratoConfig( String aic_file )
            : base( aic_file ) {
        }

        public VibratoHandle toHandle() {
            VibratoHandle ret = new VibratoHandle();
            ret.StartDepth = startDepth;
            ret.DepthBP = depthBP;
            ret.StartRate = startRate;
            ret.RateBP = rateBP;
            ret.Index = number;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = caption;
            ret.setLength( length );
            return ret;
        }

        public int getLength() {
            return length;
        }

        public void setLength( int value ) {
            length = value;
        }

        public String getCaption() {
            return caption;
        }

        public void setCaption( String value ) {
            caption = value;
        }

        public VibratoBPList getRateBP() {
            return rateBP;
        }

        public void setRateBP( VibratoBPList value ) {
            rateBP = value;
        }

        public int getStartRate() {
            return startRate;
        }

        public void setStartRate( int value ) {
            startRate = value;
        }

        public VibratoBPList getDepthBP() {
            return depthBP;
        }

        public void setDepthBP( VibratoBPList value ) {
            depthBP = value;
        }

        public int getStartDepth() {
            return startDepth;
        }

        public void setStartDepth( int value ) {
            startDepth = value;
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
    }*/

#if !JAVA
}
#endif
