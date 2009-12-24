/*
 * ExpressionConfigSys.cs
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

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using System.Text;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.vsq {
#endif

    public class ExpressionConfigSys {
#if JAVA
        private final int MAX_VIBRATO = 0x400;
#else
        private const int MAX_VIBRATO = 0x400;
#endif
        private Vector<VibratoConfig> m_vibrato_configs;
        private Vector<AttackConfig> m_attack_configs;

        private void printTo( String file ) {
            BufferedWriter sw = null;
            try {
                sw = new BufferedWriter( new FileWriter( file ) );
                int count = 0;
                for ( Iterator itr = m_vibrato_configs.iterator(); itr.hasNext(); ) {
                    count++;
                    VibratoConfig vconfig = (VibratoConfig)itr.next();
                    String name = "v" + count;
                    sw.write( "VibratoConfig " + name + " = new VibratoConfig();" );
                    sw.newLine();
                    sw.write( name + ".author = \"" + vconfig.author + "\";" );
                    sw.newLine();
                    sw.write( name + ".file = \"" + vconfig.file + "\";" );
                    sw.newLine();
                    sw.write( name + ".number = " + vconfig.number + ";" );
                    sw.newLine();
                    sw.write( name + ".vendor = \"" + vconfig.vendor + "\";" );
                    sw.newLine();
                    sw.write( name + ".contents.IconID = \"" + vconfig.contents.IconID + "\";" );
                    sw.newLine();
                    sw.write( name + ".contents.IDS = \"" + vconfig.contents.IDS + "\";" );
                    sw.newLine();
                    sw.write( name + ".contents.Original = " + vconfig.contents.Original + ";" );
                    sw.newLine();
                    sw.write( name + ".contents.Caption = \"" + vconfig.contents.Caption + "\";" );
                    sw.newLine();
                    sw.write( name + ".contents.Length = " + vconfig.contents.getLength() + ";" );
                    sw.newLine();
                    sw.write( name + ".contents.StartDepth = " + vconfig.contents.StartDepth + ";" );
                    sw.write( name + ".contents.DepthBP = new VibratoBPList( new float[]{ " );
                    for ( int i = 0; i < vconfig.contents.DepthBP.getCount(); i++ ) {
                        sw.write( ((i > 0) ? ", " : "") + vconfig.contents.DepthBP.getElement( i ).X + "f" );
                    }
                    sw.write( " }, new int[]{ " );
                    for ( int i = 0; i < vconfig.contents.DepthBP.getCount(); i++ ) {
                        sw.write( ((i > 0) ? ", " : "") + vconfig.contents.DepthBP.getElement( i ).Y );
                    }
                    sw.write( " } );" );
                    sw.newLine();
                    sw.write( name + ".contents.StartRate = " + vconfig.contents.StartRate + ";" );
                    sw.newLine();
                    sw.write( name + ".contents.RateBP = new VibratoBPList( new float[]{ " );
                    for ( int i = 0; i < vconfig.contents.RateBP.getCount(); i++ ) {
                        sw.write( ((i > 0) ? ", " : "") + vconfig.contents.RateBP.getElement( i ).X + "f" );
                    }
                    sw.write( " }, new int[]{ " );
                    for ( int i = 0; i < vconfig.contents.RateBP.getCount(); i++ ) {
                        sw.write( ((i > 0) ? ", " : "") + vconfig.contents.RateBP.getElement( i ).Y );
                    }
                    sw.write( " } );" );
                    sw.newLine();
                    sw.write( "ret.m_vibrato_configs.add( " + name + " );" );
                    sw.newLine();
                    sw.newLine();
                }
                count = 0;

                for ( Iterator itr = m_attack_configs.iterator(); itr.hasNext(); ) {
                    count++;
                    AttackConfig aconfig = (AttackConfig)itr.next();
                    String name = "a" + count;
                    sw.write( "AttackConfig " + name + " = new AttackConfig();" );
                    sw.newLine();
                    sw.write( name + ".author = \"" + aconfig.author + "\";" );
                    sw.newLine();
                    sw.write( name + ".file = \"" + aconfig.file + "\";" );
                    sw.newLine();
                    sw.write( name + ".number = " + aconfig.number + ";" );
                    sw.newLine();
                    sw.write( name + ".vendor = \"" + aconfig.vendor + "\";" );
                    sw.newLine();
                    sw.write( name + ".contents.IconID = \"" + aconfig.contents.IconID + "\";" );
                    sw.newLine();
                    sw.write( name + ".contents.IDS = \"" + aconfig.contents.IDS + "\";" );
                    sw.newLine();
                    sw.write( name + ".contents.Original = " + aconfig.contents.Original + ";" );
                    sw.newLine();
                    sw.write( name + ".contents.Caption = \"" + aconfig.contents.Caption + "\";" );
                    sw.newLine();
                    sw.write( name + ".contents.Length = " + aconfig.contents.getLength() + ";" );
                    sw.newLine();
                    sw.write( name + ".contents.Duration = " + aconfig.contents.Duration + ";" );
                    sw.newLine();
                    sw.write( name + ".contents.Depth = " + aconfig.contents.Depth + ";" );
                    sw.newLine();
                    sw.write( "ret.m_attack_configs.add( " + name + " );" );
                    sw.newLine();
                    sw.newLine();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        public static ExpressionConfigSys getVocaloid1Default() {
            ExpressionConfigSys ret = new ExpressionConfigSys();
            ret.m_vibrato_configs = new Vector<VibratoConfig>();
            ret.m_attack_configs = new Vector<AttackConfig>();
            VibratoConfig v1 = new VibratoConfig();
            v1.author = "Taro";
            v1.file = "normal.aic";
            v1.number = 1;
            v1.vendor = "YAMAHA";
            v1.contents.IconID = "$04040001";
            v1.contents.IDS = "normal";
            v1.contents.Original = 0;
            v1.contents.Caption = "Normal Vibrato";
            v1.contents.setLength( 480 );
            v1.contents.StartDepth = 64;
            v1.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v1.contents.StartRate = 64;
            v1.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v1 );

            VibratoConfig v2 = new VibratoConfig();
            v2.author = "Taro";
            v2.file = "subtle.aic";
            v2.number = 2;
            v2.vendor = "YAMAHA";
            v2.contents.IconID = "$04040002";
            v2.contents.IDS = "normal";
            v2.contents.Original = 0;
            v2.contents.Caption = "Subtle Vibrato";
            v2.contents.setLength( 480 );
            v2.contents.StartDepth = 32;
            v2.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v2.contents.StartRate = 56;
            v2.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v2 );

            VibratoConfig v3 = new VibratoConfig();
            v3.author = "Taro";
            v3.file = "slight.aic";
            v3.number = 3;
            v3.vendor = "YAMAHA";
            v3.contents.IconID = "$04040003";
            v3.contents.IDS = "slight";
            v3.contents.Original = 0;
            v3.contents.Caption = "Slight Vibrato";
            v3.contents.setLength( 480 );
            v3.contents.StartDepth = 32;
            v3.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v3.contents.StartRate = 64;
            v3.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v3 );

            VibratoConfig v4 = new VibratoConfig();
            v4.author = "Taro";
            v4.file = "deep.aic";
            v4.number = 4;
            v4.vendor = "YAMAHA";
            v4.contents.IconID = "$04040004";
            v4.contents.IDS = "deep";
            v4.contents.Original = 0;
            v4.contents.Caption = "Deep Vibrato";
            v4.contents.setLength( 480 );
            v4.contents.StartDepth = 64;
            v4.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v4.contents.StartRate = 64;
            v4.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v4 );

            VibratoConfig v5 = new VibratoConfig();
            v5.author = "Taro";
            v5.file = "verydeep.aic";
            v5.number = 5;
            v5.vendor = "YAMAHA";
            v5.contents.IconID = "$04040005";
            v5.contents.IDS = "extreme";
            v5.contents.Original = 0;
            v5.contents.Caption = "Very Deep Vibrato";
            v5.contents.setLength( 480 );
            v5.contents.StartDepth = 64;
            v5.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v5.contents.StartRate = 120;
            v5.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v5 );

            VibratoConfig v6 = new VibratoConfig();
            v6.author = "Taro";
            v6.file = "extreme.aic";
            v6.number = 6;
            v6.vendor = "YAMAHA";
            v6.contents.IconID = "$04040006";
            v6.contents.IDS = "extreme";
            v6.contents.Original = 0;
            v6.contents.Caption = "Extreme Vibrato (like Japanese Enka)";
            v6.contents.setLength( 480 );
            v6.contents.StartDepth = 64;
            v6.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v6.contents.StartRate = 64;
            v6.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v6 );

            AttackConfig a1 = new AttackConfig();
            a1.author = "Taro";
            a1.file = "na_tenuto.aic";
            a1.number = 1;
            a1.vendor = "YAMAHA";
            a1.contents.IconID = "$01010001";
            a1.contents.IDS = "tenuto";
            a1.contents.Original = 0;
            a1.contents.Caption = "Tenuto";
            a1.contents.setLength( 120 );
            a1.contents.Duration = 64;
            a1.contents.Depth = 64;
            ret.m_attack_configs.add( a1 );

            AttackConfig a2 = new AttackConfig();
            a2.author = "Taro";
            a2.file = "na_accent.aic";
            a2.number = 2;
            a2.vendor = "YAMAHA";
            a2.contents.IconID = "$01010002";
            a2.contents.IDS = "accent";
            a2.contents.Original = 0;
            a2.contents.Caption = "Accent";
            a2.contents.setLength( 120 );
            a2.contents.Duration = 64;
            a2.contents.Depth = 64;
            ret.m_attack_configs.add( a2 );

            AttackConfig a3 = new AttackConfig();
            a3.author = "Taro";
            a3.file = "na_extreme_accent.aic";
            a3.number = 3;
            a3.vendor = "YAMAHA";
            a3.contents.IconID = "$01010003";
            a3.contents.IDS = "accent_extreme";
            a3.contents.Original = 0;
            a3.contents.Caption = "Extreme Accent";
            a3.contents.setLength( 120 );
            a3.contents.Duration = 64;
            a3.contents.Depth = 64;
            ret.m_attack_configs.add( a3 );

            AttackConfig a4 = new AttackConfig();
            a4.author = "Taro";
            a4.file = "na_legato.aic";
            a4.number = 4;
            a4.vendor = "YAMAHA";
            a4.contents.IconID = "$01010004";
            a4.contents.IDS = "legato";
            a4.contents.Original = 0;
            a4.contents.Caption = "Legato";
            a4.contents.setLength( 120 );
            a4.contents.Duration = 64;
            a4.contents.Depth = 64;
            ret.m_attack_configs.add( a4 );

            AttackConfig a5 = new AttackConfig();
            a5.author = "Taro";
            a5.file = "na_fast_bendup.aic";
            a5.number = 5;
            a5.vendor = "YAMAHA";
            a5.contents.IconID = "$01010005";
            a5.contents.IDS = "bendup_fast";
            a5.contents.Original = 0;
            a5.contents.Caption = "Fast Bendu";
            a5.contents.setLength( 120 );
            a5.contents.Duration = 64;
            a5.contents.Depth = 64;
            ret.m_attack_configs.add( a5 );

            AttackConfig a6 = new AttackConfig();
            a6.author = "Taro";
            a6.file = "na_slow_bendup.aic";
            a6.number = 6;
            a6.vendor = "YAMAHA";
            a6.contents.IconID = "$01010006";
            a6.contents.IDS = "bendup_slow";
            a6.contents.Original = 0;
            a6.contents.Caption = "Slow Bendup";
            a6.contents.setLength( 120 );
            a6.contents.Duration = 64;
            a6.contents.Depth = 64;
            ret.m_attack_configs.add( a6 );

            AttackConfig a7 = new AttackConfig();
            a7.author = "Taro";
            a7.file = "na_trill_semi.aic";
            a7.number = 7;
            a7.vendor = "YAMAHA";
            a7.contents.IconID = "$01010007";
            a7.contents.IDS = "trill_semi";
            a7.contents.Original = 0;
            a7.contents.Caption = "Trill Semitone";
            a7.contents.setLength( 120 );
            a7.contents.Duration = 64;
            a7.contents.Depth = 64;
            ret.m_attack_configs.add( a7 );

            AttackConfig a8 = new AttackConfig();
            a8.author = "Taro";
            a8.file = "na_trill_whole.aic";
            a8.number = 8;
            a8.vendor = "YAMAHA";
            a8.contents.IconID = "$01010008";
            a8.contents.IDS = "trill_whole";
            a8.contents.Original = 0;
            a8.contents.Caption = "Trill Wholetone";
            a8.contents.setLength( 120 );
            a8.contents.Duration = 64;
            a8.contents.Depth = 64;
            ret.m_attack_configs.add( a8 );

            AttackConfig a9 = new AttackConfig();
            a9.author = "Taro";
            a9.file = "na_mordent_semi.aic";
            a9.number = 9;
            a9.vendor = "YAMAHA";
            a9.contents.IconID = "$01010009";
            a9.contents.IDS = "mordent_semi";
            a9.contents.Original = 0;
            a9.contents.Caption = "Mordent Semitone";
            a9.contents.setLength( 120 );
            a9.contents.Duration = 64;
            a9.contents.Depth = 64;
            ret.m_attack_configs.add( a9 );

            AttackConfig a10 = new AttackConfig();
            a10.author = "Taro";
            a10.file = "na_mordent_whole.aic";
            a10.number = 10;
            a10.vendor = "YAMAHA";
            a10.contents.IconID = "$0101000a";
            a10.contents.IDS = "mordent_whole";
            a10.contents.Original = 0;
            a10.contents.Caption = "Mordent Wholetone";
            a10.contents.setLength( 120 );
            a10.contents.Duration = 64;
            a10.contents.Depth = 64;
            ret.m_attack_configs.add( a10 );

            return ret;
        }

        public static ExpressionConfigSys getVocaloid2Default() {
            ExpressionConfigSys ret = new ExpressionConfigSys();
            ret.m_vibrato_configs = new Vector<VibratoConfig>();
            ret.m_attack_configs = new Vector<AttackConfig>();
            VibratoConfig v1 = new VibratoConfig();
            v1.author = "Standard";
            v1.file = "normal2_type1.aic";
            v1.number = 1;
            v1.vendor = "YAMAHA";
            v1.contents.IconID = "$04040001";
            v1.contents.IDS = "normal";
            v1.contents.Original = 0;
            v1.contents.Caption = "[Normal] Type 1";
            v1.contents.setLength( 480 );
            v1.contents.StartDepth = 64;
            v1.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v1.contents.StartRate = 50;
            v1.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v1 );

            VibratoConfig v2 = new VibratoConfig();
            v2.author = "Standard";
            v2.file = "normal2_type2.aic";
            v2.number = 2;
            v2.vendor = "YAMAHA";
            v2.contents.IconID = "$04040002";
            v2.contents.IDS = "normal";
            v2.contents.Original = 0;
            v2.contents.Caption = "[Normal] Type 2";
            v2.contents.setLength( 480 );
            v2.contents.StartDepth = 40;
            v2.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v2.contents.StartRate = 50;
            v2.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v2 );

            VibratoConfig v3 = new VibratoConfig();
            v3.author = "Standard";
            v3.file = "normal2_type3.aic";
            v3.number = 3;
            v3.vendor = "YAMAHA";
            v3.contents.IconID = "$04040003";
            v3.contents.IDS = "normal";
            v3.contents.Original = 0;
            v3.contents.Caption = "[Normal] Type 3";
            v3.contents.setLength( 480 );
            v3.contents.StartDepth = 127;
            v3.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v3.contents.StartRate = 50;
            v3.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v3 );

            VibratoConfig v4 = new VibratoConfig();
            v4.author = "Standard";
            v4.file = "normal2_type4.aic";
            v4.number = 4;
            v4.vendor = "YAMAHA";
            v4.contents.IconID = "$04040004";
            v4.contents.IDS = "normal";
            v4.contents.Original = 0;
            v4.contents.Caption = "[Normal] Type 4";
            v4.contents.setLength( 480 );
            v4.contents.StartDepth = 64;
            v4.contents.DepthBP = new VibratoBPList( new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
            v4.contents.StartRate = 50;
            v4.contents.RateBP = new VibratoBPList( new float[] { 0.6f, 0.6125f, 0.6167f, 0.6208f, 0.6292f, 0.6333f, 0.6375f, 0.6417f, 0.6542f, 0.6583f, 0.6625f, 0.6667f, 0.675f, 0.6833f, 0.6875f, 0.6917f, 0.7f, 0.7042f, 0.7083f, 0.7125f, 0.725f, 0.7292f, 0.7333f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7708f, 0.775f, 0.7792f, 0.7833f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8167f, 0.8208f, 0.8292f, 0.8333f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8667f, 0.8708f, 0.875f, 0.8792f, 0.8875f, 0.8917f, 0.9f, 1f }, new int[] { 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
            ret.m_vibrato_configs.add( v4 );

            VibratoConfig v5 = new VibratoConfig();
            v5.author = "Standard";
            v5.file = "extreme2_type1.aic";
            v5.number = 5;
            v5.vendor = "YAMAHA";
            v5.contents.IconID = "$04040005";
            v5.contents.IDS = "extreme";
            v5.contents.Original = 0;
            v5.contents.Caption = "[Extreme] Type 1";
            v5.contents.setLength( 480 );
            v5.contents.StartDepth = 64;
            v5.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v5.contents.StartRate = 64;
            v5.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v5 );

            VibratoConfig v6 = new VibratoConfig();
            v6.author = "Standard";
            v6.file = "extreme2_type2.aic";
            v6.number = 6;
            v6.vendor = "YAMAHA";
            v6.contents.IconID = "$04040006";
            v6.contents.IDS = "extreme";
            v6.contents.Original = 0;
            v6.contents.Caption = "[Extreme] Type 2";
            v6.contents.setLength( 480 );
            v6.contents.StartDepth = 32;
            v6.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v6.contents.StartRate = 32;
            v6.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v6 );

            VibratoConfig v7 = new VibratoConfig();
            v7.author = "Standard";
            v7.file = "extreme2_type3.aic";
            v7.number = 7;
            v7.vendor = "YAMAHA";
            v7.contents.IconID = "$04040007";
            v7.contents.IDS = "extreme";
            v7.contents.Original = 0;
            v7.contents.Caption = "[Extreme] Type 3";
            v7.contents.setLength( 480 );
            v7.contents.StartDepth = 100;
            v7.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v7.contents.StartRate = 50;
            v7.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v7 );

            VibratoConfig v8 = new VibratoConfig();
            v8.author = "Standard";
            v8.file = "extreme2_type4.aic";
            v8.number = 8;
            v8.vendor = "YAMAHA";
            v8.contents.IconID = "$04040008";
            v8.contents.IDS = "extreme";
            v8.contents.Original = 0;
            v8.contents.Caption = "[Extreme] Type 4";
            v8.contents.setLength( 480 );
            v8.contents.StartDepth = 64;
            v8.contents.DepthBP = new VibratoBPList( new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
            v8.contents.StartRate = 64;
            v8.contents.RateBP = new VibratoBPList( new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
            ret.m_vibrato_configs.add( v8 );

            VibratoConfig v9 = new VibratoConfig();
            v9.author = "Standard";
            v9.file = "fast2_type1.aic";
            v9.number = 9;
            v9.vendor = "YAMAHA";
            v9.contents.IconID = "$04040009";
            v9.contents.IDS = "fast";
            v9.contents.Original = 0;
            v9.contents.Caption = "[Fast] Type 1";
            v9.contents.setLength( 480 );
            v9.contents.StartDepth = 64;
            v9.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v9.contents.StartRate = 64;
            v9.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v9 );

            VibratoConfig v10 = new VibratoConfig();
            v10.author = "Standard";
            v10.file = "fast2_type2.aic";
            v10.number = 10;
            v10.vendor = "YAMAHA";
            v10.contents.IconID = "$0404000a";
            v10.contents.IDS = "fast";
            v10.contents.Original = 0;
            v10.contents.Caption = "[Fast] Type 2";
            v10.contents.setLength( 480 );
            v10.contents.StartDepth = 40;
            v10.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v10.contents.StartRate = 50;
            v10.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v10 );

            VibratoConfig v11 = new VibratoConfig();
            v11.author = "Standard";
            v11.file = "fast2_type3.aic";
            v11.number = 11;
            v11.vendor = "YAMAHA";
            v11.contents.IconID = "$0404000b";
            v11.contents.IDS = "fast";
            v11.contents.Original = 0;
            v11.contents.Caption = "[Fast] Type 3";
            v11.contents.setLength( 480 );
            v11.contents.StartDepth = 80;
            v11.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v11.contents.StartRate = 70;
            v11.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v11 );

            VibratoConfig v12 = new VibratoConfig();
            v12.author = "Standard";
            v12.file = "fast2_type4.aic";
            v12.number = 12;
            v12.vendor = "YAMAHA";
            v12.contents.IconID = "$0404000c";
            v12.contents.IDS = "fast";
            v12.contents.Original = 0;
            v12.contents.Caption = "[Fast] Type 4";
            v12.contents.setLength( 480 );
            v12.contents.StartDepth = 64;
            v12.contents.DepthBP = new VibratoBPList( new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
            v12.contents.StartRate = 64;
            v12.contents.RateBP = new VibratoBPList( new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
            ret.m_vibrato_configs.add( v12 );

            VibratoConfig v13 = new VibratoConfig();
            v13.author = "Standard";
            v13.file = "slight2_type1.aic";
            v13.number = 13;
            v13.vendor = "YAMAHA";
            v13.contents.IconID = "$0404000d";
            v13.contents.IDS = "slight";
            v13.contents.Original = 0;
            v13.contents.Caption = "[Slight] Type 1";
            v13.contents.setLength( 480 );
            v13.contents.StartDepth = 64;
            v13.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v13.contents.StartRate = 64;
            v13.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v13 );

            VibratoConfig v14 = new VibratoConfig();
            v14.author = "Standard";
            v14.file = "slight2_type2.aic";
            v14.number = 14;
            v14.vendor = "YAMAHA";
            v14.contents.IconID = "$0404000e";
            v14.contents.IDS = "slight";
            v14.contents.Original = 0;
            v14.contents.Caption = "[Slight] Type 2";
            v14.contents.setLength( 480 );
            v14.contents.StartDepth = 40;
            v14.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v14.contents.StartRate = 64;
            v14.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v14 );

            VibratoConfig v15 = new VibratoConfig();
            v15.author = "Standard";
            v15.file = "slight2_type3.aic";
            v15.number = 15;
            v15.vendor = "YAMAHA";
            v15.contents.IconID = "$0404000f";
            v15.contents.IDS = "slight";
            v15.contents.Original = 0;
            v15.contents.Caption = "[Slight] Type 3";
            v15.contents.setLength( 480 );
            v15.contents.StartDepth = 72;
            v15.contents.DepthBP = new VibratoBPList( new float[] { }, new int[] { } );
            v15.contents.StartRate = 64;
            v15.contents.RateBP = new VibratoBPList( new float[] { }, new int[] { } );
            ret.m_vibrato_configs.add( v15 );

            VibratoConfig v16 = new VibratoConfig();
            v16.author = "Standard";
            v16.file = "slight2_type4.aic";
            v16.number = 16;
            v16.vendor = "YAMAHA";
            v16.contents.IconID = "$04040010";
            v16.contents.IDS = "slight";
            v16.contents.Original = 0;
            v16.contents.Caption = "[Slight] Type 4";
            v16.contents.setLength( 480 );
            v16.contents.StartDepth = 64;
            v16.contents.DepthBP = new VibratoBPList( new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
            v16.contents.StartRate = 64;
            v16.contents.RateBP = new VibratoBPList( new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
            ret.m_vibrato_configs.add( v16 );

            return ret;
        }

        public int getVibratoConfigCount() {
            return m_vibrato_configs.size();
        }

        public int getAttackConfigCount() {
            return m_attack_configs.size();
        }

        public Iterator vibratoConfigIterator() {
#if JAVA
            return m_vibrato_configs.iterator();
#else
            return new ListIterator<VibratoConfig>( m_vibrato_configs );
#endif
        }

        public Iterator attackConfigIterator() {
#if JAVA
            return m_attack_configs.iterator();
#else
            return new ListIterator<AttackConfig>( m_attack_configs );
#endif
        }

        private ExpressionConfigSys() {
        }

        public ExpressionConfigSys( String path_expdb ) {
            m_vibrato_configs = new Vector<VibratoConfig>();
            m_attack_configs = new Vector<AttackConfig>();
            String expression = PortUtil.combinePath( path_expdb, "expression.map" );
            if ( !PortUtil.isFileExists( expression ) ) {
#if DEBUG
                PortUtil.println( "ExpressionConfigSys#.ctor; expression.map does not exist" );
#endif
                return;
            }

            RandomAccessFile fs = null;
            try {
                fs = new RandomAccessFile( expression, "r" );
                byte[] dat = new byte[8];
                fs.seek( 0x20 );
                for ( int i = 0; i < MAX_VIBRATO; i++ ) {
                    fs.read( dat, 0, 8 );
                    long value = VocaloSysUtil.makelong_le( dat );
                    if ( value <= 0 ) {
                        continue;
                    }

                    String ved = PortUtil.combinePath( path_expdb, "vexp" + value + ".ved" );
                    if ( !PortUtil.isFileExists( ved ) ) {
                        continue;
                    }
                    String vexp_dir = PortUtil.combinePath( path_expdb, "vexp" + value );
                    if ( !PortUtil.isFileExists( vexp_dir ) ) {
                        continue;
                    }

#if DEBUG
                    PortUtil.println( "ExpresionConfigSys#.ctor; ved=" + ved + "; vexp_dir=" + vexp_dir );
#endif
                    String NL = (char)0x0D + "" + (char)0x0A;
                    RandomAccessFile fs_ved = null;
                    try {
                        fs_ved = new RandomAccessFile( ved, "r" );
                        byte[] byte_ved = new byte[(int)fs_ved.length()];
                        fs_ved.read( byte_ved, 0, byte_ved.Length );
                        TransCodeUtil.decodeBytes( byte_ved );
                        String str = PortUtil.getDecodedString( "ASCII", byte_ved );
#if DEBUG
                        String txt_file = PortUtil.combinePath( path_expdb, "vexp" + value + ".txt" );
                        using ( System.IO.StreamWriter sw = new System.IO.StreamWriter( txt_file ) ) {
                            sw.Write( str );
                        }
#endif
                        String[] spl = PortUtil.splitString( str, new String[] { NL }, true );
                        String current_entry = "";
                        for ( int j = 0; j < spl.Length; j++ ) {
#if DEBUG
                            //PortUtil.println( "ExpressionConfigSys#.ctor; line=" + spl[j] );
#endif
                            if ( spl[j].StartsWith( "[" ) ) {
                                current_entry = spl[j];
                                continue;
                            } else if ( spl[j].Equals( "" ) ) {
                                continue;
                            }
                            if ( current_entry.Equals( "[VIBRATO]" ) ) {
                                String[] spl2 = PortUtil.splitString( spl[j], ',' );
                                if ( spl2.Length < 6 ) {
                                    continue;
                                }
                                // ex: 1,1,"normal","normal2_type1.aic","[Normal]:Type:1","Standard","YAMAHA",0
                                VibratoConfig item = new VibratoConfig();
                                item.number = PortUtil.parseInt( spl2[0] );
                                item.contents.IDS = spl2[2].Replace( "\"", "" );
                                item.file = spl2[3].Replace( "\"", "" );
                                item.contents.Caption = spl2[4].Replace( ":", " " ).Replace( "\"", "" );
                                item.author = spl2[5].Replace( "\"", "" );
                                item.vendor = spl2[6].Replace( "\"", "" );
                                item.contents.IconID = "$0404" + PortUtil.toHexString( item.number, 4 );
                                String aic_file = PortUtil.combinePath( vexp_dir, item.file );
                                if ( !PortUtil.isFileExists( aic_file ) ) {
                                    continue;
                                }
                                item.parseAic( aic_file );
                                m_vibrato_configs.add( item );
                            } if ( current_entry.Equals( "[NOTEATTACK]" ) ) {
                                String[] spl2 = PortUtil.splitString( spl[j], ',' );
                                if ( spl2.Length < 6 ) {
                                    continue;
                                }
                                // ex: 1,1,"normal","normal2_type1.aic","[Normal]:Type:1","Standard","YAMAHA",0
                                AttackConfig item = new AttackConfig();
                                item.number = PortUtil.parseInt( spl2[0] );
                                item.contents.IDS = spl2[2].Replace( "\"", "" );
                                item.file = spl2[3].Replace( "\"", "" );
                                item.contents.Caption = spl2[4].Replace( ":", " " ).Replace( "\"", "" );
                                item.author = spl2[5].Replace( "\"", "" );
                                item.vendor = spl2[6].Replace( "\"", "" );
                                item.contents.IconID = "$0101" + PortUtil.toHexString( item.number, 4 );
                                String aic_file = PortUtil.combinePath( vexp_dir, item.file );
                                if ( !PortUtil.isFileExists( aic_file ) ) {
                                    continue;
                                }
                                item.parseAic( aic_file );
                                m_attack_configs.add( item );
                            }
                        }
                    } catch ( Exception ex ) {
#if DEBUG
                        PortUtil.println( "ExpressionConfigSys#.ctor; ex=" + ex );
#endif
                    } finally {
                        if ( fs_ved != null ) {
                            try {
                                fs_ved.close();
                            } catch ( Exception ex2 ) {
#if DEBUG
                                PortUtil.println( "ExpressionConfigSys#.ctor; ex2=" + ex2 );
#endif
                            }
                        }
                    }
                }
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "ExpressionConfigSys#.ctor; ex=" + ex );
#endif
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
#if DEBUG
                        PortUtil.println( "ExpressionConfigSys#.ctor; ex2=" + ex2 );
#endif
                    }
                }
            }
        }

    }

#if !JAVA
}
#endif
