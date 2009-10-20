/*
 * Program.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.cadencii.
 *
 * com.boare.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * com.boare.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

import javax.swing.*;
import java.util.*;
import java.lang.reflect.*;
import java.io.*;
import java.awt.*;

import com.boare.util.*;
import com.boare.vsq.*;
import com.boare.corlib.*;
import com.boare.media.*;

public class Program{
    private static Program s_instance = null;
    //public static Event<Integer> testEvent = new Event<Integer>();

    static{
        s_instance = new Program();
    }

    public static void main( String[] args ){
        Messaging.loadMessages();
        VsqCommon vc;
        Lyric lyric;
        KeyValuePair<Integer, Integer> k;
        VsqBPList list;
        VibratoBPList list2;
        VsqHandleType vht;
        VsqHandle vh;
        VsqID vsq_id;
        VsqMaster vsq_master;
        VsqMixer vsq_mixer;
        UstVibrato ust_vibrato;
        UstEnvelope ust_envelope;
        UstPortamentoPoint ust_portamento_point;
        UstPortamentoType ust_portamento_type;
        UstPortamento ust_portamento;
        UstEvent ust_event;
        VsqEvent vsq_event;
        VsqEventList vsq_event_list;
        VsqMetaText vsq_meta_text;
        TempoTableEntry tempo_table_entry;
        TimeSigTableEntry time_sig_table_entry;
        NrpnData nrpn_data;
        SingerConfig singer_config;
        VocaloSysUtil vocalo_sys_util;
        SymbolTable symbol_table;
        UstTrack ust_track;
        UstFile ust_file;
        VibratoTypeUtil vibrato_type_util;
        VsqBarLineType vsq_bar_line_type;
        VsqCommandType vsq_command_type;
        VsqTrack vsq_track;
        VsqNrpn vsq_nrpn;
        DefaultVibratoLength default_vibrato_length;
        AutoVibratoMinLength default_vibrato_min_length;
        ClockResolution clock_resolution;
        Platform platform;
        ToolStripLocation tool_stip_location;
        QuantizeMode quantize_mode;
        QuantizeModeUtil quantize_mode_util;
        MidiPortConfig midi_port_config;
        RgbColor rgb_color;
        Keys keys;
        KeysUtil keys_util;
        ValuePairOfStringArrayOfKeys value_pair_of_string_array_of_keys;
        com.boare.cadencii.Event ev;
        EditorConfig editor_config;
        PointD point_d;
        BezierControlType bezier_control_type;
        BezierPickedSide bezier_picked_side;
        BezierPoint bezier_point;
        BezierChain bezier_chain;
        CurveType curve_type;
        BezierCurves bezier_curves;
        Command command;
        CommandRunnable command_runnable;
        CadenciiCommandType cadencii_command_type;
        CadenciiCommand cadencii_command;
        AttachedCurve attached_curve;
        try{
            VsqFileEx vsq_file = new VsqFileEx( "kuziraNo12.vsq" );
            XmlSerializer xsvsq = new XmlSerializer( VsqFileEx.class );
            FileOutputStream fsout = new FileOutputStream( "kuziraNo12.xml" );
            xsvsq.serialize( fsout, vsq_file );
            fsout.close();
            vsq_file.write( "regen.vsq" );
        }catch( Exception ex ){
            System.out.println( "ex=" + ex );
        }
        EditMode edit_mode;
        SelectedEventEntry selected_event_entry;
        SelectedEventList selected_event_list;
        SelectedTempoEntry selected_tempo_entry;
        SelectedTimesigEntry selected_timesig_entry;
        EditTool edit_tool;
        SelectedBezierPoint selected_bezier_point;
        PlayPositionSpecifier play_position_specifier;
        SelectedRegion selected_region;
        ScreenStatus screen_status;
        BitConverter bit_converter;
        WaveWriter wave_writer;
        TempoInfo tempo_info;
        MIDI_EVENT midi_event;
        vstidrv _vstidrv;
        VstiRenderer vsti_renderer;
        WaveReader wave_reader;
        PlaySound play_sound;
        
        /*int size = 44100;
        PlaySound.init( size );
        double[] left = new double[size];
        double[] right = new double[size];
        
        int freq = 441;
        for( int i = 0; i < size; i++ ){
            double t = i / (double)size;
            double v = 0.2 * Math.sin( 2.0 * 3.14159 * freq * t );
            left[i] = v;
            right[i] = v;
        }
        for( int i = 0; i < 5; i++ ){
            PlaySound.append( left, right, size );
        }
        PlaySound.waitForExit();*/

        /*testEvent.add( new EventHandler<Integer>( Program.class, "hello" ) );
        testEvent.add( new EventHandler<Integer>( s_instance, "hello2" ) );
        try{
            testEvent.invoke( 1 );
            testEvent.invoke( 2 );
        }catch( Exception ex ){
        }*/
        try{
            Delegate delegate = new Delegate( Program.class, "hello", Void.TYPE, Integer.class );
            delegate.invoke( 1 );
            Delegate dlg2 = new Delegate( Program.class, "hello", Void.class, Integer.TYPE );
            dlg2.invoke( 2 );
            EventHandler eh = new EventHandler( Program.class, "hello2", Integer.TYPE );
            eh.invoke( 3 );
        }catch( Exception ex ){
            System.out.println( "ex=" + ex );
        }

        try{
            XmlSerializer xs = new XmlSerializer( Test.class );
            xs.setIndent( true );
            xs.setIndentWidth( 4 );
            FileOutputStream fs = new FileOutputStream( "foo.xml" );
            Test t = new Test();
            t.val = "value of val";
            System.out.println( "BEFORE" );
            System.out.println( "t=" + t.toString() );
            xs.serialize( fs, t );
            fs.close();
            FileInputStream fsin = new FileInputStream( "foo.xml" );
            Test t2 = (Test)xs.deserialize( fsin );
            fsin.close();
            System.out.println( "AFTER" );
            if( t2 == null ){
                System.out.println( "t2 is null" );
            }else{
                System.out.println( "t2=" + t2.toString() );
            }
        }catch( Exception ex ){
            System.out.println( "XmlSerializer test; ex=" + ex );
        }

        PropertyGrid property_grid = new PropertyGrid();
        Test t = new Test();
        t.foo = 100;
        t.bar = 200;
        property_grid.setSelectedObject( t );
        JFrame jf = new JFrame();
        jf.getContentPane().add( property_grid );
        jf.setVisible( true );

        System.out.println( "(new int[10]).getClass()=" + (new int[10]).getClass() );
        System.out.println( "(new Vector<Integer>()).getClass()=" + (new Vector<Integer>()).getClass() );
        TypeVariable[] type_variables = (new TreeMap<Integer, String>()).getClass().getTypeParameters();
        System.out.println( "(new TreeMap<Integer, String>()).getClass().getTypeParameters()=" );
        for( TypeVariable tv : type_variables ){
            System.out.println( "    getName()=" + tv.getName() );
            GenericDeclaration gd = tv.getGenericDeclaration();
            System.out.println( "    getGenericDeclaration()=" + gd );
            TypeVariable[] tp2 = gd.getTypeParameters();
            System.out.println( "    getGenericDeclaration().getTypeParameters()=" );
            for( TypeVariable tv2 : tp2 ){
                System.out.println( "        " + tv2 );
            }
        }
        
        try{
            KeyValuePair<Integer, String> kvp = new KeyValuePair<Integer, String>( 1, "" );
            Class c = kvp.getClass();
            System.out.println( "c=" + c );
            System.out.println( "fields=" );
            for( Field f : c.getDeclaredFields() ){
                System.out.println( "    f=" + f );
                System.out.println( "    f.get(kvp).getClass()=" + f.get( kvp ).getClass() );
            }
        }catch( Exception ex ){
        }

        if( VsqFileEx.class.getSuperclass() != null ){
            System.out.println( "VsqFileEx.class.getSuperclass()=" + VsqFileEx.class.getSuperclass() );
        }

        FormMain form_main = new FormMain();
        form_main.setDefaultCloseOperation( JFrame.EXIT_ON_CLOSE );
        form_main.setBounds( 10, 10, 300, 200 );
        form_main.setVisible( true );
    }

    public static void hello( Integer arg ){
        System.out.println( "hello, arg=" + arg );
    }

    public static void hello2( Object sender, Integer arg ){
        if( sender == null ){
            System.out.println( "sender is null" );
        }else{
            System.out.println( "sender.getClass()=" + sender.getClass() );
        }
        System.out.println( "good night, arg=" + arg );
    }
}
