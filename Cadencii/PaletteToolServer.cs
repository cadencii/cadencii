#if ENABLE_SCRIPT
/*
 * PaletteToolServer.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using org.kbinani.java.util;
using org.kbinani.vsq;
using org.kbinani.xml;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    public static class PaletteToolServer {
        public static TreeMap<String, Object> loadedTools = new TreeMap<String, Object>();

        public static void init() {
            String path = Path.Combine( Application.StartupPath, "tool" );
            if ( !Directory.Exists( path ) ) {
                return;
            }

            FileInfo[] files = new DirectoryInfo( path ).GetFiles( "*.txt" );
            foreach ( FileInfo file in files ) {
                String code = "";
                using ( StreamReader sr = new StreamReader( file.FullName ) ){
                    code += sr.ReadToEnd();
                }
                CompilerResults results = AppManager.compileScript( code );
                if ( results == null ) {
                    continue;
                }
                Assembly asm = null;
                try {
                    asm = results.CompiledAssembly;
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "PaletteToolServer#init; ex=" + ex );
                    continue;
                }
                foreach ( Type t in asm.GetTypes() ) {
                    if ( t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterface( typeof( IPaletteTool ).FullName ) != null ) {
                        try {
#if DEBUG
                            AppManager.debugWriteLine( "t.FullName=" + t.FullName );
#endif
                            object instance = asm.CreateInstance( t.FullName );
                            String dir = Path.Combine( AppManager.getApplicationDataPath(), "tool" );
                            String cfg = Path.GetFileNameWithoutExtension( file.FullName ) + ".config";
                            String config = Path.Combine( dir, cfg );
                            if ( PortUtil.isFileExists( config ) ) {
                                XmlStaticMemberSerializer xsms = new XmlStaticMemberSerializer( instance.GetType() );
                                using ( FileStream fs = new FileStream( config, FileMode.Open ) ) {
                                    xsms.Deserialize( fs );
                                }
                            }
                            String id = Path.GetFileNameWithoutExtension( file.FullName );
                            loadedTools.put( id, instance );
#if DEBUG
                            AppManager.debugWriteLine( "PaletteToolServer#init; id=" + id );
#endif
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "PlaetteToolServer#init; ex=" + ex );
                        }
                    }
                }
            }
        }

        public static boolean invokePaletteTool( String id, int track, int[] vsq_event_intrenal_ids, MouseButtons button ) {
            if ( loadedTools.containsKey( id ) ) {
                VsqFileEx vsq = AppManager.getVsqFile();
                VsqTrack vsq_track = vsq.Track.get( track );
                VsqTrack item = (VsqTrack)vsq_track.clone();
                boolean edited = false;
                try {
                    edited = ((IPaletteTool)loadedTools.get( id )).edit( item, vsq_event_intrenal_ids, button );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "PaletteToolServer#InvokePaletteTool; ex=" + ex );
                    edited = false;
                }
                if ( edited ) {
                    CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( track, item, vsq.AttachedCurves.get( track - 1 ) );
                    AppManager.register( vsq.executeCommand( run ),
                                         track,
                                         AppManager.editedZone[track - 1].add( AppManager.detectTrackDifference( vsq_track, item ) ) );
                }
                return edited;
            } else {
                return false;
            }
        }
    }

}
#endif
