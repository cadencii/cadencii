/*
 * ExpressionConfigSys.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using cadencii.java.io;
using cadencii.java.util;

namespace cadencii.vsq
{

    /// <summary>
    /// VOCALOID1またはVOCALOID2の、表情ライブラリの設定値を表します。
    /// </summary>
    public class ExpressionConfigSys
    {
        private const int MAX_VIBRATO = 0x400;
        private List<VibratoHandle> m_vibrato_configs;
        private List<NoteHeadHandle> m_attack_configs;
        private List<IconDynamicsHandle> m_dynamics_configs;

        /*private void printTo( String file ) {
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
        }*/

        /// <summary>
        /// VOCALOID1システムのデフォルトの表情ライブラリの設定値を取得します。
        /// </summary>
        /// <returns></returns>
        public static ExpressionConfigSys getVocaloid1Default()
        {
            ExpressionConfigSys ret = new ExpressionConfigSys();
            ret.m_vibrato_configs = new List<VibratoHandle>();
            ret.m_attack_configs = new List<NoteHeadHandle>();
            ret.m_dynamics_configs = new List<IconDynamicsHandle>();
            VibratoHandle v1 = new VibratoHandle();
            /*v1.author = "Taro";
            v1.file = "normal.aic";
            v1.number = 1;
            v1.vendor = "YAMAHA";*/
            v1.Index = 1;
            v1.IconID = "$04040001";
            v1.IDS = "normal";
            v1.Original = 0;
            v1.setCaption("Normal Vibrato");
            v1.setLength(480);
            v1.setStartDepth(64);
            v1.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v1.setStartRate(64);
            v1.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v1);

            VibratoHandle v2 = new VibratoHandle();
            /*v2.author = "Taro";
            v2.file = "subtle.aic";
            v2.number = 2;
            v2.vendor = "YAMAHA";*/
            v2.Index = 2;
            v2.IconID = "$04040002";
            v2.IDS = "normal";
            v2.Original = 0;
            v2.setCaption("Subtle Vibrato");
            v2.setLength(480);
            v2.setStartDepth(32);
            v2.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v2.setStartRate(56);
            v2.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v2);

            VibratoHandle v3 = new VibratoHandle();
            /*v3.author = "Taro";
            v3.file = "slight.aic";
            v3.number = 3;
            v3.vendor = "YAMAHA";*/
            v3.Index = 3;
            v3.IconID = "$04040003";
            v3.IDS = "slight";
            v3.Original = 0;
            v3.setCaption("Slight Vibrato");
            v3.setLength(480);
            v3.setStartDepth(32);
            v3.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v3.setStartRate(64);
            v3.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v3);

            VibratoHandle v4 = new VibratoHandle();
            /*v4.author = "Taro";
            v4.file = "deep.aic";
            v4.number = 4;
            v4.vendor = "YAMAHA";*/
            v4.Index = 4;
            v4.IconID = "$04040004";
            v4.IDS = "deep";
            v4.Original = 0;
            v4.setCaption("Deep Vibrato");
            v4.setLength(480);
            v4.setStartDepth(64);
            v4.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v4.setStartRate(64);
            v4.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v4);

            VibratoHandle v5 = new VibratoHandle();
            /*v5.author = "Taro";
            v5.file = "verydeep.aic";
            v5.number = 5;
            v5.vendor = "YAMAHA";*/
            v5.Index = 5;
            v5.IconID = "$04040005";
            v5.IDS = "extreme";
            v5.Original = 0;
            v5.setCaption("Very Deep Vibrato");
            v5.setLength(480);
            v5.setStartDepth(64);
            v5.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v5.setStartRate(120);
            v5.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v5);

            VibratoHandle v6 = new VibratoHandle();
            /*v6.author = "Taro";
            v6.file = "extreme.aic";
            v6.number = 6;
            v6.vendor = "YAMAHA";*/
            v6.Index = 6;
            v6.IconID = "$04040006";
            v6.IDS = "extreme";
            v6.Original = 0;
            v6.setCaption("Extreme Vibrato (like Japanese Enka)");
            v6.setLength(480);
            v6.setStartDepth(64);
            v6.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v6.setStartRate(64);
            v6.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v6);

            NoteHeadHandle a1 = new NoteHeadHandle();
            /*a1.author = "Taro";
            a1.file = "na_tenuto.aic";
            a1.number = 1;
            a1.vendor = "YAMAHA";*/
            a1.Index = 1;
            a1.IconID = "$01010001";
            a1.IDS = "tenuto";
            a1.Original = 0;
            a1.setCaption("Tenuto");
            a1.setLength(120);
            a1.setDuration(64);
            a1.setDepth(64);
            ret.m_attack_configs.Add(a1);

            NoteHeadHandle a2 = new NoteHeadHandle();
            /*a2.author = "Taro";
            a2.file = "na_accent.aic";
            a2.number = 2;
            a2.vendor = "YAMAHA";*/
            a2.Index = 2;
            a2.IconID = "$01010002";
            a2.IDS = "accent";
            a2.Original = 0;
            a2.setCaption("Accent");
            a2.setLength(120);
            a2.setDuration(64);
            a2.setDepth(64);
            ret.m_attack_configs.Add(a2);

            NoteHeadHandle a3 = new NoteHeadHandle();
            /*a3.author = "Taro";
            a3.file = "na_extreme_accent.aic";
            a3.number = 3;
            a3.vendor = "YAMAHA";*/
            a3.Index = 3;
            a3.IconID = "$01010003";
            a3.IDS = "accent_extreme";
            a3.Original = 0;
            a3.setCaption("Extreme Accent");
            a3.setLength(120);
            a3.setDuration(64);
            a3.setDepth(64);
            ret.m_attack_configs.Add(a3);

            NoteHeadHandle a4 = new NoteHeadHandle();
            /*a4.author = "Taro";
            a4.file = "na_legato.aic";
            a4.number = 4;
            a4.vendor = "YAMAHA";*/
            a4.Index = 4;
            a4.IconID = "$01010004";
            a4.IDS = "legato";
            a4.Original = 0;
            a4.setCaption("Legato");
            a4.setLength(120);
            a4.setDuration(64);
            a4.setDepth(64);
            ret.m_attack_configs.Add(a4);

            NoteHeadHandle a5 = new NoteHeadHandle();
            /*a5.author = "Taro";
            a5.file = "na_fast_bendup.aic";
            a5.number = 5;
            a5.vendor = "YAMAHA";*/
            a5.Index = 5;
            a5.IconID = "$01010005";
            a5.IDS = "bendup_fast";
            a5.Original = 0;
            a5.setCaption("Fast Bendu");
            a5.setLength(120);
            a5.setDuration(64);
            a5.setDepth(64);
            ret.m_attack_configs.Add(a5);

            NoteHeadHandle a6 = new NoteHeadHandle();
            /*a6.author = "Taro";
            a6.file = "na_slow_bendup.aic";
            a6.number = 6;
            a6.vendor = "YAMAHA";*/
            a6.Index = 6;
            a6.IconID = "$01010006";
            a6.IDS = "bendup_slow";
            a6.Original = 0;
            a6.setCaption("Slow Bendup");
            a6.setLength(120);
            a6.setDuration(64);
            a6.setDepth(64);
            ret.m_attack_configs.Add(a6);

            NoteHeadHandle a7 = new NoteHeadHandle();
            /*a7.author = "Taro";
            a7.file = "na_trill_semi.aic";
            a7.number = 7;
            a7.vendor = "YAMAHA";*/
            a7.Index = 7;
            a7.IconID = "$01010007";
            a7.IDS = "trill_semi";
            a7.Original = 0;
            a7.setCaption("Trill Semitone");
            a7.setLength(120);
            a7.setDuration(64);
            a7.setDepth(64);
            ret.m_attack_configs.Add(a7);

            NoteHeadHandle a8 = new NoteHeadHandle();
            /*a8.author = "Taro";
            a8.file = "na_trill_whole.aic";
            a8.number = 8;
            a8.vendor = "YAMAHA";*/
            a8.Index = 8;
            a8.IconID = "$01010008";
            a8.IDS = "trill_whole";
            a8.Original = 0;
            a8.setCaption("Trill Wholetone");
            a8.setLength(120);
            a8.setDuration(64);
            a8.setDepth(64);
            ret.m_attack_configs.Add(a8);

            NoteHeadHandle a9 = new NoteHeadHandle();
            /*a9.author = "Taro";
            a9.file = "na_mordent_semi.aic";
            a9.number = 9;
            a9.vendor = "YAMAHA";*/
            a9.Index = 9;
            a9.IconID = "$01010009";
            a9.IDS = "mordent_semi";
            a9.Original = 0;
            a9.setCaption("Mordent Semitone");
            a9.setLength(120);
            a9.setDuration(64);
            a9.setDepth(64);
            ret.m_attack_configs.Add(a9);

            NoteHeadHandle a10 = new NoteHeadHandle();
            /*a10.author = "Taro";
            a10.file = "na_mordent_whole.aic";
            a10.number = 10;
            a10.vendor = "YAMAHA";*/
            a10.Index = 10;
            a10.IconID = "$0101000a";
            a10.IDS = "mordent_whole";
            a10.Original = 0;
            a10.setCaption("Mordent Wholetone");
            a10.setLength(120);
            a10.setDuration(64);
            a10.setDepth(64);
            ret.m_attack_configs.Add(a10);

            IconDynamicsHandle d0 = new IconDynamicsHandle();
            d0.IDS = "Dynaff11";
            d0.IconID = "$05010000";
            d0.Original = 0;
            d0.setCaption("Fortississimo");
            d0.setStartDyn(120);
            d0.setEndDyn(120);
            d0.setLength(0);
            ret.m_dynamics_configs.Add(d0);

            IconDynamicsHandle d1 = new IconDynamicsHandle();
            d1.IDS = "Dynaff12";
            d1.IconID = "$05010001";
            d1.Original = 1;
            d1.setCaption("Fortissimo");
            d1.setStartDyn(104);
            d1.setEndDyn(104);
            d1.setLength(0);
            ret.m_dynamics_configs.Add(d1);

            IconDynamicsHandle d2 = new IconDynamicsHandle();
            d2.IDS = "Dynaff13";
            d2.IconID = "$05010002";
            d2.Original = 2;
            d2.setCaption("Forte");
            d2.setStartDyn(88);
            d2.setEndDyn(88);
            d2.setLength(0);
            ret.m_dynamics_configs.Add(d2);

            IconDynamicsHandle d3 = new IconDynamicsHandle();
            d3.IDS = "Dynaff21";
            d3.IconID = "$05010003";
            d3.Original = 3;
            d3.setCaption("MesoForte");
            d3.setStartDyn(72);
            d3.setEndDyn(72);
            d3.setLength(0);
            ret.m_dynamics_configs.Add(d3);

            IconDynamicsHandle d4 = new IconDynamicsHandle();
            d4.IDS = "Dynaff22";
            d4.IconID = "$05010004";
            d4.Original = 4;
            d4.setCaption("MesoPiano");
            d4.setStartDyn(56);
            d4.setEndDyn(56);
            d4.setLength(0);
            ret.m_dynamics_configs.Add(d4);

            IconDynamicsHandle d5 = new IconDynamicsHandle();
            d5.IDS = "Dynaff31";
            d5.IconID = "$05010005";
            d5.Original = 5;
            d5.setCaption("Piano");
            d5.setStartDyn(40);
            d5.setEndDyn(40);
            d5.setLength(0);
            ret.m_dynamics_configs.Add(d5);

            IconDynamicsHandle d6 = new IconDynamicsHandle();
            d6.IDS = "Dynaff32";
            d6.IconID = "$05010006";
            d6.Original = 6;
            d6.setCaption("Pianissimo");
            d6.setStartDyn(24);
            d6.setEndDyn(24);
            d6.setLength(0);
            ret.m_dynamics_configs.Add(d6);

            IconDynamicsHandle d7 = new IconDynamicsHandle();
            d7.IDS = "Dynaff33";
            d7.IconID = "$05010007";
            d7.Original = 7;
            d7.setCaption("Pianississimo");
            d7.setStartDyn(8);
            d7.setEndDyn(8);
            d7.setLength(0);
            ret.m_dynamics_configs.Add(d7);

            IconDynamicsHandle d8 = new IconDynamicsHandle();
            d8.IDS = "cresc_1";
            d8.IconID = "$05020000";
            d8.Original = 0;
            d8.setCaption("Zero Crescendo");
            d8.setStartDyn(0);
            d8.setEndDyn(38);
            d8.setLength(960);
            ret.m_dynamics_configs.Add(d8);

            IconDynamicsHandle d9 = new IconDynamicsHandle();
            d9.IDS = "cresc_2";
            d9.IconID = "$05020001";
            d9.Original = 1;
            d9.setCaption("Zero Crescendo");
            d9.setStartDyn(0);
            d9.setEndDyn(64);
            d9.setLength(960);
            ret.m_dynamics_configs.Add(d9);

            IconDynamicsHandle d10 = new IconDynamicsHandle();
            d10.IDS = "cresc_3";
            d10.IconID = "$05020002";
            d10.Original = 2;
            d10.setCaption("Zero Crescendo");
            d10.setStartDyn(0);
            d10.setEndDyn(127);
            d10.setLength(960);
            ret.m_dynamics_configs.Add(d10);

            IconDynamicsHandle d11 = new IconDynamicsHandle();
            d11.IDS = "cresc_4";
            d11.IconID = "$05020003";
            d11.Original = 3;
            d11.setCaption("Zero Crescendo Curve");
            d11.setStartDyn(0);
            d11.setEndDyn(38);
            d11.setLength(960);
            d11.setDynBP(new VibratoBPList(new float[] { 0.5f }, new int[] { 11 }));
            ret.m_dynamics_configs.Add(d11);

            IconDynamicsHandle d12 = new IconDynamicsHandle();
            d12.IDS = "cresc_5";
            d12.IconID = "$05020004";
            d12.Original = 4;
            d12.setCaption("Zero Crescendo Curve");
            d12.setStartDyn(0);
            d12.setEndDyn(102);
            d12.setLength(960);
            d12.setDynBP(new VibratoBPList(new float[] { 0.5f }, new int[] { 40 }));
            ret.m_dynamics_configs.Add(d12);

            IconDynamicsHandle d13 = new IconDynamicsHandle();
            d13.IDS = "dim_1";
            d13.IconID = "$05030000";
            d13.Original = 0;
            d13.setCaption("Zero Decrescendo");
            d13.setStartDyn(0);
            d13.setEndDyn(-38);
            d13.setLength(960);
            ret.m_dynamics_configs.Add(d13);

            IconDynamicsHandle d14 = new IconDynamicsHandle();
            d14.IDS = "dim_2";
            d14.IconID = "$05030001";
            d14.Original = 1;
            d14.setCaption("Zero Decrescendo");
            d14.setStartDyn(0);
            d14.setEndDyn(-64);
            d14.setLength(960);
            ret.m_dynamics_configs.Add(d14);

            IconDynamicsHandle d15 = new IconDynamicsHandle();
            d15.IDS = "dim_3";
            d15.IconID = "$05030002";
            d15.Original = 2;
            d15.setCaption("Zero Decrescendo");
            d15.setStartDyn(0);
            d15.setEndDyn(-127);
            d15.setLength(960);
            ret.m_dynamics_configs.Add(d15);

            IconDynamicsHandle d16 = new IconDynamicsHandle();
            d16.IDS = "dim_4";
            d16.IconID = "$05030003";
            d16.Original = 3;
            d16.setCaption("Zero Decrescendo Curve");
            d16.setStartDyn(0);
            d16.setEndDyn(-38);
            d16.setLength(960);
            d16.setDynBP(new VibratoBPList(new float[] { 0.5f }, new int[] { -11 }));
            ret.m_dynamics_configs.Add(d16);

            IconDynamicsHandle d17 = new IconDynamicsHandle();
            d17.IDS = "dim_5";
            d17.IconID = "$05030004";
            d17.Original = 4;
            d17.setCaption("Zero Decrescendo Curve");
            d17.setStartDyn(0);
            d17.setEndDyn(-102);
            d17.setLength(960);
            d17.setDynBP(new VibratoBPList(new float[] { 0.5f }, new int[] { -40 }));
            ret.m_dynamics_configs.Add(d17);

            return ret;
        }

        /// <summary>
        /// VOCALOID2システムのデフォルトの表情ライブラリの設定値を取得します。
        /// </summary>
        /// <returns></returns>
        public static ExpressionConfigSys getVocaloid2Default()
        {
            ExpressionConfigSys ret = new ExpressionConfigSys();
            ret.m_vibrato_configs = new List<VibratoHandle>();
            ret.m_attack_configs = new List<NoteHeadHandle>();
            ret.m_dynamics_configs = new List<IconDynamicsHandle>();
            VibratoHandle v1 = new VibratoHandle();
            /*v1.author = "Standard";
            v1.file = "normal2_type1.aic";
            v1.number = 1;
            v1.vendor = "YAMAHA";*/
            v1.Index = 1;
            v1.IconID = "$04040001";
            v1.IDS = "normal";
            v1.Original = 0;
            v1.setCaption("[Normal] Type 1");
            v1.setLength(480);
            v1.setStartDepth(64);
            v1.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v1.setStartRate(50);
            v1.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v1);

            VibratoHandle v2 = new VibratoHandle();
            /*v2.author = "Standard";
            v2.file = "normal2_type2.aic";
            v2.number = 2;
            v2.vendor = "YAMAHA";*/
            v2.Index = 2;
            v2.IconID = "$04040002";
            v2.IDS = "normal";
            v2.Original = 0;
            v2.setCaption("[Normal] Type 2");
            v2.setLength(480);
            v2.setStartDepth(40);
            v2.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v2.setStartRate(50);
            v2.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v2);

            VibratoHandle v3 = new VibratoHandle();
            /*v3.author = "Standard";
            v3.file = "normal2_type3.aic";
            v3.number = 3;
            v3.vendor = "YAMAHA";*/
            v3.Index = 3;
            v3.IconID = "$04040003";
            v3.IDS = "normal";
            v3.Original = 0;
            v3.setCaption("[Normal] Type 3");
            v3.setLength(480);
            v3.setStartDepth(127);
            v3.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v3.setStartRate(50);
            v3.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v3);

            VibratoHandle v4 = new VibratoHandle();
            /*v4.author = "Standard";
            v4.file = "normal2_type4.aic";
            v4.number = 4;
            v4.vendor = "YAMAHA";*/
            v4.Index = 4;
            v4.IconID = "$04040004";
            v4.IDS = "normal";
            v4.Original = 0;
            v4.setCaption("[Normal] Type 4");
            v4.setLength(480);
            v4.setStartDepth(64);
            v4.setDepthBP(new VibratoBPList(new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 }));
            v4.setStartRate(50);
            v4.setRateBP(new VibratoBPList(new float[] { 0.6f, 0.6125f, 0.6167f, 0.6208f, 0.6292f, 0.6333f, 0.6375f, 0.6417f, 0.6542f, 0.6583f, 0.6625f, 0.6667f, 0.675f, 0.6833f, 0.6875f, 0.6917f, 0.7f, 0.7042f, 0.7083f, 0.7125f, 0.725f, 0.7292f, 0.7333f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7708f, 0.775f, 0.7792f, 0.7833f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8167f, 0.8208f, 0.8292f, 0.8333f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8667f, 0.8708f, 0.875f, 0.8792f, 0.8875f, 0.8917f, 0.9f, 1f }, new int[] { 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 }));
            ret.m_vibrato_configs.Add(v4);

            VibratoHandle v5 = new VibratoHandle();
            /*v5.author = "Standard";
            v5.file = "extreme2_type1.aic";
            v5.number = 5;
            v5.vendor = "YAMAHA";*/
            v5.Index = 5;
            v5.IconID = "$04040005";
            v5.IDS = "extreme";
            v5.Original = 0;
            v5.setCaption("[Extreme] Type 1");
            v5.setLength(480);
            v5.setStartDepth(64);
            v5.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v5.setStartRate(64);
            v5.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v5);

            VibratoHandle v6 = new VibratoHandle();
            /*v6.author = "Standard";
            v6.file = "extreme2_type2.aic";
            v6.number = 6;
            v6.vendor = "YAMAHA";*/
            v6.Index = 6;
            v6.IconID = "$04040006";
            v6.IDS = "extreme";
            v6.Original = 0;
            v6.setCaption("[Extreme] Type 2");
            v6.setLength(480);
            v6.setStartDepth(32);
            v6.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v6.setStartRate(32);
            v6.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v6);

            VibratoHandle v7 = new VibratoHandle();
            /*v7.author = "Standard";
            v7.file = "extreme2_type3.aic";
            v7.number = 7;
            v7.vendor = "YAMAHA";*/
            v7.Index = 7;
            v7.IconID = "$04040007";
            v7.IDS = "extreme";
            v7.Original = 0;
            v7.setCaption("[Extreme] Type 3");
            v7.setLength(480);
            v7.setStartDepth(100);
            v7.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v7.setStartRate(50);
            v7.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v7);

            VibratoHandle v8 = new VibratoHandle();
            /*v8.author = "Standard";
            v8.file = "extreme2_type4.aic";
            v8.number = 8;
            v8.vendor = "YAMAHA";*/
            v8.Index = 8;
            v8.IconID = "$04040008";
            v8.IDS = "extreme";
            v8.Original = 0;
            v8.setCaption("[Extreme] Type 4");
            v8.setLength(480);
            v8.setStartDepth(64);
            v8.setDepthBP(new VibratoBPList(new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 }));
            v8.setStartRate(64);
            v8.setRateBP(new VibratoBPList(new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 }));
            ret.m_vibrato_configs.Add(v8);

            VibratoHandle v9 = new VibratoHandle();
            /*v9.author = "Standard";
            v9.file = "fast2_type1.aic";
            v9.number = 9;
            v9.vendor = "YAMAHA";*/
            v9.Index = 9;
            v9.IconID = "$04040009";
            v9.IDS = "fast";
            v9.Original = 0;
            v9.setCaption("[Fast] Type 1");
            v9.setLength(480);
            v9.setStartDepth(64);
            v9.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v9.setStartRate(64);
            v9.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v9);

            VibratoHandle v10 = new VibratoHandle();
            /*v10.author = "Standard";
            v10.file = "fast2_type2.aic";
            v10.number = 10;
            v10.vendor = "YAMAHA";*/
            v10.Index = 10;
            v10.IconID = "$0404000a";
            v10.IDS = "fast";
            v10.Original = 0;
            v10.setCaption("[Fast] Type 2");
            v10.setLength(480);
            v10.setStartDepth(40);
            v10.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v10.setStartRate(50);
            v10.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v10);

            VibratoHandle v11 = new VibratoHandle();
            /*v11.author = "Standard";
            v11.file = "fast2_type3.aic";
            v11.number = 11;
            v11.vendor = "YAMAHA";*/
            v11.Index = 11;
            v11.IconID = "$0404000b";
            v11.IDS = "fast";
            v11.Original = 0;
            v11.setCaption("[Fast] Type 3");
            v11.setLength(480);
            v11.setStartDepth(80);
            v11.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v11.setStartRate(70);
            v11.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v11);

            VibratoHandle v12 = new VibratoHandle();
            /*v12.author = "Standard";
            v12.file = "fast2_type4.aic";
            v12.number = 12;
            v12.vendor = "YAMAHA";*/
            v12.Index = 12;
            v12.IconID = "$0404000c";
            v12.IDS = "fast";
            v12.Original = 0;
            v12.setCaption("[Fast] Type 4");
            v12.setLength(480);
            v12.setStartDepth(64);
            v12.setDepthBP(new VibratoBPList(new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 }));
            v12.setStartRate(64);
            v12.setRateBP(new VibratoBPList(new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 }));
            ret.m_vibrato_configs.Add(v12);

            VibratoHandle v13 = new VibratoHandle();
            /*v13.author = "Standard";
            v13.file = "slight2_type1.aic";
            v13.number = 13;
            v13.vendor = "YAMAHA";*/
            v13.Index = 13;
            v13.IconID = "$0404000d";
            v13.IDS = "slight";
            v13.Original = 0;
            v13.setCaption("[Slight] Type 1");
            v13.setLength(480);
            v13.setStartDepth(64);
            v13.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v13.setStartRate(64);
            v13.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v13);

            VibratoHandle v14 = new VibratoHandle();
            /*v14.author = "Standard";
            v14.file = "slight2_type2.aic";
            v14.number = 14;
            v14.vendor = "YAMAHA";*/
            v14.Index = 14;
            v14.IconID = "$0404000e";
            v14.IDS = "slight";
            v14.Original = 0;
            v14.setCaption("[Slight] Type 2");
            v14.setLength(480);
            v14.setStartDepth(40);
            v14.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v14.setStartRate(64);
            v14.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v14);

            VibratoHandle v15 = new VibratoHandle();
            /*v15.author = "Standard";
            v15.file = "slight2_type3.aic";
            v15.number = 15;
            v15.vendor = "YAMAHA";*/
            v15.Index = 15;
            v15.IconID = "$0404000f";
            v15.IDS = "slight";
            v15.Original = 0;
            v15.setCaption("[Slight] Type 3");
            v15.setLength(480);
            v15.setStartDepth(72);
            v15.setDepthBP(new VibratoBPList(new float[] { }, new int[] { }));
            v15.setStartRate(64);
            v15.setRateBP(new VibratoBPList(new float[] { }, new int[] { }));
            ret.m_vibrato_configs.Add(v15);

            VibratoHandle v16 = new VibratoHandle();
            /*v16.author = "Standard";
            v16.file = "slight2_type4.aic";
            v16.number = 16;
            v16.vendor = "YAMAHA";*/
            v16.Index = 16;
            v16.IconID = "$04040010";
            v16.IDS = "slight";
            v16.Original = 0;
            v16.setCaption("[Slight] Type 4");
            v16.setLength(480);
            v16.setStartDepth(64);
            v16.setDepthBP(new VibratoBPList(new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 }));
            v16.setStartRate(64);
            v16.setRateBP(new VibratoBPList(new float[] { 0.6042f, 0.6125f, 0.6167f, 0.6208f, 0.625f, 0.6333f, 0.6375f, 0.6417f, 0.6458f, 0.6542f, 0.6583f, 0.6667f, 0.6708f, 0.675f, 0.6792f, 0.6833f, 0.6917f, 0.6958f, 0.7042f, 0.7083f, 0.7125f, 0.7167f, 0.7208f, 0.725f, 0.7292f, 0.7375f, 0.7458f, 0.75f, 0.7583f, 0.7625f, 0.7667f, 0.7708f, 0.775f, 0.7833f, 0.7917f, 0.7958f, 0.8f, 0.8042f, 0.8083f, 0.8125f, 0.8208f, 0.8292f, 0.8375f, 0.8417f, 0.8458f, 0.85f, 0.8542f, 0.8625f, 0.8667f, 0.875f, 0.8792f, 0.8833f, 0.8875f, 0.8917f, 0.8958f, 0.9f, 1f }, new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 }));
            ret.m_vibrato_configs.Add(v16);

            return ret;
        }

        /// <summary>
        /// 登録されているビブラート設定の個数を取得します。
        /// </summary>
        /// <returns></returns>
        public int getVibratoConfigCount()
        {
            return m_vibrato_configs.Count;
        }

        /// <summary>
        /// 登録されているアタック設定の個数を取得します。
        /// </summary>
        /// <returns></returns>
        public int getAttackConfigCount()
        {
            return m_attack_configs.Count;
        }

        /// <summary>
        /// 登録されている強弱記号設定の個数を取得します。
        /// </summary>
        /// <returns></returns>
        public int getDynamicsConfigCount()
        {
            return m_dynamics_configs.Count;
        }

        /// <summary>
        /// 登録されているビブラート設定を順に返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VibratoHandle> vibratoConfigIterator()
        {
            return m_vibrato_configs;
        }

        /// <summary>
        /// 登録されているアタック設定を順に返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NoteHeadHandle> attackConfigIterator()
        {
            return m_attack_configs;
        }

        /// <summary>
        /// 登録されている強弱記号設定を順に返す反復子を返します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IconDynamicsHandle> dynamicsConfigIterator()
        {
            return m_dynamics_configs;
        }

        private ExpressionConfigSys()
        {
        }

        /// <summary>
        /// VOCALOID(1/2)エディタの実行ファイルのパスと、表情ライブラリのディレクトリのパスを元に、新しい表情ライブラリ設定を構築します。
        /// </summary>
        /// <param name="path_editor"></param>
        /// <param name="path_expdb"></param>
        public ExpressionConfigSys(string path_editor, string path_expdb)
        {
            m_vibrato_configs = new List<VibratoHandle>();
            m_attack_configs = new List<NoteHeadHandle>();
            m_dynamics_configs = new List<IconDynamicsHandle>();

            string base_path = PortUtil.getDirectoryName(path_editor);
            string aiconDB_def = Path.Combine(base_path, "AiconDB.def");
            if (System.IO.File.Exists(aiconDB_def)) {
                string folder_name = "";
                SortedDictionary<string, List<string>> list = new SortedDictionary<string, List<string>>();
                StreamReader sr = null;
                try {
                    sr = new StreamReader(aiconDB_def, Encoding.GetEncoding("Shift_JIS"));
                    string line = "";
                    string current = "";
                    while ((line = sr.ReadLine()) != null) {
                        int index_semicollon = line.IndexOf(';');
                        if (index_semicollon >= 0) {
                            line = line.Substring(0, index_semicollon);
                        }
                        line = line.Trim();
                        if (line.StartsWith("[")) {
                            current = line;
                        } else {
                            int index_eq = line.IndexOf('=');
                            if (index_eq > 0) {
                                string[] spl = PortUtil.splitString(line, '=');
                                if (spl.Length != 2) {
                                    continue;
                                }
                                if (current.Equals("[Common]")) {
                                    if (spl[0].Equals("FolderName")) {
                                        folder_name = spl[1];
                                    }
                                } else {
                                    List<string> add = null;
                                    if (list.ContainsKey(current)) {
                                        add = list[current];
                                        list.Remove(current);
                                    } else {
                                        add = new List<string>();
                                    }
                                    add.Add(line);
                                    list[current] = add;
                                }
                            }
                        }
                    }
                } catch (Exception ex) {
                    serr.println("ExpressionConfigSys#.ctor; ex=" + ex);
                } finally {
                    if (sr != null) {
                        try {
                            sr.Close();
                        } catch (Exception ex2) {
                            serr.println("ExpressionConfigSys#.ctor; ex2=" + ex2);
                        }
                    }
                }

                if (!folder_name.Equals("")) {
                    string aiconDB_path = Path.Combine(base_path, folder_name);
                    if (Directory.Exists(aiconDB_path)) {
                        foreach (var key in list.Keys) {
                            string section_name = key.Replace("[", "").Replace("]", "");
                            string section_path = Path.Combine(aiconDB_path, section_name);
                            if (Directory.Exists(section_path)) {
                                foreach (var line in list[key]) {
                                    string[] spl = PortUtil.splitString(line, '=');
                                    if (spl.Length != 2) {
                                        continue;
                                    }
                                    string name = spl[0];
                                    string[] spl2 = PortUtil.splitString(spl[1], ',');
                                    string preset = "";
                                    if (name.Equals("Dynaff")) {
                                        preset = IconDynamicsHandle.ICONID_HEAD_DYNAFF;
                                    } else if (name.Equals("Crescendo")) {
                                        preset = IconDynamicsHandle.ICONID_HEAD_CRESCEND;
                                    } else if (name.Equals("Decrescendo")) {
                                        preset = IconDynamicsHandle.ICONID_HEAD_DECRESCEND;
                                    }
                                    for (int i = 0; i < spl2.Length; i++) {
                                        string aic_name = spl2[i];
                                        if (!aic_name.EndsWith(".aic")) {
                                            aic_name += ".aic";
                                        }
                                        string aic_path = Path.Combine(section_path, aic_name);
                                        string ids = spl2[i];
                                        string icon_id = preset + PortUtil.formatDecimal("0000", i);
                                        if (System.IO.File.Exists(aic_path)) {
                                            IconDynamicsHandle handle = new IconDynamicsHandle(aic_path, ids, icon_id, i);
                                            handle.setButtonImageFullPath(Path.Combine(section_path, handle.getButton()));
                                            m_dynamics_configs.Add(handle);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string expression = Path.Combine(path_expdb, "expression.map");
            if (!System.IO.File.Exists(expression)) {
                return;
            }
            FileStream fs = null;
            try {
                fs = new FileStream(expression, FileMode.Open, FileAccess.Read);
                byte[] dat = new byte[8];
                fs.Seek(0x20, SeekOrigin.Begin);
                for (int i = 0; i < MAX_VIBRATO; i++) {
                    fs.Read(dat, 0, 8);
                    long value = PortUtil.make_int64_le(dat);
                    if (value <= 0) {
                        continue;
                    }

                    string ved = Path.Combine(path_expdb, "vexp" + value + ".ved");
                    if (!System.IO.File.Exists(ved)) {
                        continue;
                    }
                    string vexp_dir = Path.Combine(path_expdb, "vexp" + value);
                    if (!Directory.Exists(vexp_dir)) {
                        continue;
                    }

                    string NL = (char)0x0D + "" + (char)0x0A;
                    FileStream fs_ved = null;
                    try {
                        fs_ved = new FileStream(ved, FileMode.Open, FileAccess.Read);
                        byte[] byte_ved = new byte[(int)fs_ved.Length];
                        fs_ved.Read(byte_ved, 0, byte_ved.Length);
                        TransCodeUtil.decodeBytes(byte_ved);
                        int[] int_ved = new int[byte_ved.Length];
                        for (int j = 0; j < byte_ved.Length; j++) {
                            int_ved[j] = 0xff & byte_ved[j];
                        }
                        string s = PortUtil.getDecodedString("ASCII", int_ved);
                        string[] spl = PortUtil.splitString(s, new string[] { NL }, true);
                        string current_entry = "";
                        for (int j = 0; j < spl.Length; j++) {
                            if (spl[j].StartsWith("[")) {
                                current_entry = spl[j];
                                continue;
                            } else if (spl[j].Equals("")) {
                                continue;
                            }
                            if (current_entry.Equals("[VIBRATO]")) {
                                string[] spl2 = PortUtil.splitString(spl[j], ',');
                                if (spl2.Length < 6) {
                                    continue;
                                }
                                // ex: 1,1,"normal","normal2_type1.aic","[Normal]:Type:1","Standard","YAMAHA",0
                                string file = spl2[3].Replace("\"", "");
                                string aic_file = Path.Combine(vexp_dir, file);
                                int index = int.Parse(spl2[0]);
                                string icon_id = "$0404" + PortUtil.toHexString(index, 4);
                                string ids = "";//spl2[2].Replace( "\"", "" );
                                string caption = spl2[4].Replace("\"", "").Replace(":", " ");
                                VibratoHandle item = new VibratoHandle(aic_file, ids, icon_id, index);
                                item.setCaption(caption);
                                m_vibrato_configs.Add(item);
                            } if (current_entry.Equals("[NOTEATTACK]")) {
                                string[] spl2 = PortUtil.splitString(spl[j], ',');
                                if (spl2.Length < 6) {
                                    continue;
                                }
                                // ex: 1,1,"normal","normal2_type1.aic","[Normal]:Type:1","Standard","YAMAHA",0
                                string file = spl2[3].Replace("\"", "");
                                string aic_path = Path.Combine(vexp_dir, file);
                                if (!System.IO.File.Exists(aic_path)) {
                                    continue;
                                }
                                string ids = "";// spl2[2].Replace( "\"", "" );
                                string caption = spl2[4].Replace("\"", "").Replace(":", " ");
                                int index = int.Parse(spl2[0]);
                                string icon_id = "$0101" + PortUtil.toHexString(index, 4);
                                NoteHeadHandle item = new NoteHeadHandle(aic_path, ids, icon_id, index);
                                item.setCaption(caption);
                                m_attack_configs.Add(item);
                            }
                        }
                    } catch (Exception ex) {
                        serr.println("ExpressionConfigSys#.ctor; ex=" + ex);
                    } finally {
                        if (fs_ved != null) {
                            try {
                                fs_ved.Close();
                            } catch (Exception ex2) {
                                serr.println("ExpressionConfigSys#.ctor; ex2=" + ex2);
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                serr.println("ExpressionConfigSys#.ctor; ex=" + ex);
            } finally {
                if (fs != null) {
                    try {
                        fs.Close();
                    } catch (Exception ex2) {
                        serr.println("ExpressionConfigSys#.ctor; ex2=" + ex2);
                    }
                }
            }
        }

    }

}
