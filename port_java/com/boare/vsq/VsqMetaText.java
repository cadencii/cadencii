/*
 * VsqMetaText.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.baore.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

import java.awt.*;
import java.io.*;
import java.util.*;
import java.text.*;
import com.boare.corlib.*;

/// <summary>
/// vsqのメタテキストの中身を処理するためのクラス
/// </summary>
public class VsqMetaText implements Cloneable {
    public VsqCommon common;
    public VsqMaster master;
    public VsqMixer mixer;
    public VsqEventList events;
    /// <summary>
    /// PIT。ピッチベンド(pitchBendBPList)。default=0
    /// </summary>
    public VsqBPList pit;
    /// <summary>
    /// PBS。ピッチベンドセンシティビティ(pitchBendSensBPList)。dfault=2
    /// </summary>
    public VsqBPList pbs;
    /// <summary>
    /// DYN。ダイナミクス(dynamicsBPList)。default=64
    /// </summary>
    public VsqBPList dyn;
    /// <summary>
    /// BRE。ブレシネス(epRResidualBPList)。default=0
    /// </summary>
    public VsqBPList bre;
    /// <summary>
    /// BRI。ブライトネス(epRESlopeBPList)。default=64
    /// </summary>
    public VsqBPList bri;
    /// <summary>
    /// CLE。クリアネス(epRESlopeDepthBPList)。default=0
    /// </summary>
    public VsqBPList cle;
    public VsqBPList reso1FreqBPList;
    public VsqBPList reso2FreqBPList;
    public VsqBPList reso3FreqBPList;
    public VsqBPList reso4FreqBPList;
    public VsqBPList reso1BWBPList;
    public VsqBPList reso2BWBPList;
    public VsqBPList reso3BWBPList;
    public VsqBPList reso4BWBPList;
    public VsqBPList reso1AmpBPList;
    public VsqBPList reso2AmpBPList;
    public VsqBPList reso3AmpBPList;
    public VsqBPList reso4AmpBPList;
    /// <summary>
    /// Harmonics。(EpRSineBPList)default = 64
    /// </summary>
    public VsqBPList harmonics;
    /// <summary>
    /// Effect2 Depth。
    /// </summary>
    public VsqBPList fx2depth;
    /// <summary>
    /// GEN。ジェンダーファクター(genderFactorBPList)。default=64
    /// </summary>
    public VsqBPList gen;
    /// <summary>
    /// POR。ポルタメントタイミング(portamentoTimingBPList)。default=64
    /// </summary>
    public VsqBPList por;
    /// <summary>
    /// OPE。オープニング(openingBPList)。default=127
    /// </summary>
    public VsqBPList ope;

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "Singer" ) ){
            return true;
        }else if( name.equals( "master" ) ){
            return true;
        }else if( name.equals( "mixer" ) ){
            return true;
        }
        return false;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "common" ) ){
            return "Common";
        }else if( name.equals( "events" ) ){
            return "Events";
        }else if( name.equals( "pit" ) ){
            return "PIT";
        }else if( name.equals( "pbs" ) ){
            return "PBS";
        }else if( name.equals( "dyn" ) ){
            return "DYN";
        }else if( name.equals( "bre" ) ){
            return "BRE";
        }else if( name.equals( "bri" ) ){
            return "BRI";
        }else if( name.equals( "cle" ) ){
            return "CLE";
        }else if( name.equals( "gen" ) ){
            return "GEN";
        }else if( name.equals( "por" ) ){
            return "POR";
        }else if( name.equals( "ope" ) ){
            return "OPE";
        }
        return name;
    }

    public Object clone() {
        VsqMetaText res = new VsqMetaText();
        if ( common != null ) {
            res.common = (VsqCommon)common.clone();
        }
        if ( master != null ) {
            res.master = (VsqMaster)master.clone();
        }
        if ( mixer != null ) {
            res.mixer = (VsqMixer)mixer.clone();
        }
        if ( events != null ) {
            res.events = new VsqEventList();
            for ( Iterator itr = events.iterator(); itr.hasNext(); ) {
                res.events.add( (VsqEvent)((VsqEvent)itr.next()).clone() );
            }
        }
        if ( pit != null ) {
            res.pit = (VsqBPList)pit.clone();
        }
        if ( pbs != null ) {
            res.pbs = (VsqBPList)pbs.clone();
        }
        if ( dyn != null ) {
            res.dyn = (VsqBPList)dyn.clone();
        }
        if ( bre != null ) {
            res.bre = (VsqBPList)bre.clone();
        }
        if ( bri != null ) {
            res.bri = (VsqBPList)bri.clone();
        }
        if ( cle != null ) {
            res.cle = (VsqBPList)cle.clone();
        }
        if ( reso1FreqBPList != null ) {
            res.reso1FreqBPList = (VsqBPList)reso1FreqBPList.clone();
        }
        if ( reso2FreqBPList != null ) {
            res.reso2FreqBPList = (VsqBPList)reso2FreqBPList.clone();
        }
        if ( reso3FreqBPList != null ) {
            res.reso3FreqBPList = (VsqBPList)reso3FreqBPList.clone();
        }
        if ( reso4FreqBPList != null ) {
            res.reso4FreqBPList = (VsqBPList)reso4FreqBPList.clone();
        }
        if ( reso1BWBPList != null ) {
            res.reso1BWBPList = (VsqBPList)reso1BWBPList.clone();
        }
        if ( reso2BWBPList != null ) {
            res.reso2BWBPList = (VsqBPList)reso2BWBPList.clone();
        }
        if ( reso3BWBPList != null ) {
            res.reso3BWBPList = (VsqBPList)reso3BWBPList.clone();
        }
        if ( reso4BWBPList != null ) {
            res.reso4BWBPList = (VsqBPList)reso4BWBPList.clone();
        }
        if ( reso1AmpBPList != null ) {
            res.reso1AmpBPList = (VsqBPList)reso1AmpBPList.clone();
        }
        if ( reso2AmpBPList != null ) {
            res.reso2AmpBPList = (VsqBPList)reso2AmpBPList.clone();
        }
        if ( reso3AmpBPList != null ) {
            res.reso3AmpBPList = (VsqBPList)reso3AmpBPList.clone();
        }
        if ( reso4AmpBPList != null ) {
            res.reso4AmpBPList = (VsqBPList)reso4AmpBPList.clone();
        }
        if ( harmonics != null ) {
            res.harmonics = (VsqBPList)harmonics.clone();
        }
        if ( fx2depth != null ) {
            res.fx2depth = (VsqBPList)fx2depth.clone();
        }
        if ( gen != null ) {
            res.gen = (VsqBPList)gen.clone();
        }
        if ( por != null ) {
            res.por = (VsqBPList)por.clone();
        }
        if ( ope != null ) {
            res.ope = (VsqBPList)ope.clone();
        }
        return res;
    }

    public VsqEventList getEventList() {
        return events;
    }

    public VsqBPList getElement( String curve ) {
        String s = curve.trim().toLowerCase();
        if( s.equals( "bre" ) ){
            return this.bre;
        }else if( s.equals( "bri" ) ){
            return this.bri;
        }else if( s.equals( "cle" ) ){
            return this.cle;
        }else if( s.equals( "dyn" ) ){
            return this.dyn;
        }else if( s.equals( "gen" ) ){
            return this.gen;
        }else if( s.equals( "ope" ) ){
            return this.ope;
        }else if( s.equals( "pbs" ) ){
            return this.pbs;
        }else if( s.equals( "pit" ) ){
            return this.pit;
        }else if( s.equals( "por" ) ){
            return this.por;
        }else if( s.equals( "harmonics" ) ){
            return this.harmonics;
        }else if( s.equals( "fx2depth" ) ){
            return this.fx2depth;
        }else if( s.equals( "reso1amp" ) ){
            return this.reso1AmpBPList;
        }else if( s.equals( "reso1bw" ) ){
            return this.reso1BWBPList;
        }else if( s.equals( "reso1freq" ) ){
            return this.reso1FreqBPList;
        }else if( s .equals( "reso2amp" ) ){
            return this.reso2AmpBPList;
        }else if( s.equals( "reso2bw" ) ){
            return this.reso2BWBPList;
        }else if( s.equals( "reso2freq" ) ){
            return this.reso2FreqBPList;
        }else if( s.equals( "reso3amp" ) ){
            return this.reso2AmpBPList;
        }else if( s.equals( "reso3bw" ) ){
            return this.reso3BWBPList;
        }else if( s.equals( "reso3freq" ) ){
            return this.reso3FreqBPList;
        }else if( s.equals( "reso4amp" ) ){
            return this.reso4AmpBPList;
        }else if( s.equals( "reso4bw" ) ){
            return this.reso4BWBPList;
        }else if( s.equals( "reso4freq" ) ){
            return this.reso4FreqBPList;
        }else{
            return null;
        }
    }

    public void setElement( String curve, VsqBPList value ) {
        String s = curve.trim().toLowerCase();
        if( s.equals( "bre" ) ){
            this.bre = value;
        }else if( s.equals( "bri" ) ){
            this.bri = value;
        }else if( s.equals( "cle" ) ){
            this.cle = value;
        }else if( s.equals( "dyn" ) ){
            this.dyn = value;
        }else if( s.equals( "gen" ) ){
            this.gen = value;
        }else if( s.equals( "ope" ) ){
            this.ope = value;
        }else if( s.equals( "pbs" ) ){
            this.pbs = value;
        }else if( s.equals( "pit" ) ){
            this.pit = value;
        }else if( s.equals( "por" ) ){
            this.por = value;
        }
    }

    /// <summary>
    /// Editor画面上で上からindex番目のカーブを表すBPListを求めます
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public VsqBPList getCurve( int index ) {
        switch ( index ) {
            case 1:
                return dyn;
            case 2:
                return bre;
            case 3:
                return bri;
            case 4:
                return cle;
            case 5:
                return ope;
            case 6:
                return gen;
            case 7:
                return por;
            case 8:
                return pit;
            case 9:
                return pbs;
            default:
                return null;
        }
    }


    /// <summary>
    /// Editor画面上で上からindex番目のカーブの名前を調べます
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static String getCurveName( int index ) {
        switch ( index ) {
            case 0:
                return "VEL";
            case 1:
                return "DYN";
            case 2:
                return "BRE";
            case 3:
                return "BRI";
            case 4:
                return "CLE";
            case 5:
                return "OPE";
            case 6:
                return "GEN";
            case 7:
                return "POR";
            case 8:
                return "PIT";
            case 9:
                return "PBS";
            default:
                return "";
        }
    }

    /// <summary>
    /// Singerプロパティに指定されている
    /// </summary>
    public String getSinger() {
        for ( Iterator itr = events.iterator(); itr.hasNext(); ) {
            VsqEvent item = (VsqEvent)itr.next();
            if ( item.id.type == VsqIDType.Singer ) {
                return item.id.iconHandle.IDS;
            }
        }
        return "";
    }

    public void setSinger( String value ) {
        for ( Iterator itr = events.iterator(); itr.hasNext(); ) {
            VsqEvent item = (VsqEvent)itr.next();
            if ( item.id.type == VsqIDType.Singer ) {
                item.id.iconHandle.IDS = value;
                break;
            }
        }
    }

    /// <summary>
    /// EOSイベントが記録されているクロックを取得します。
    /// </summary>
    /// <returns></returns>
    public int getIndexOfEOS() {
        int result;
        if ( events.getCount() > 0 ) {
            int ilast = events.getCount() - 1;
            result = events.getElement( ilast ).clock;
        } else {
            result = -1;
        }
        return result;
    }

    /// <summary>
    /// このインスタンスから、IDとHandleのリストを構築します
    /// </summary>
    /// <param name="id"></param>
    /// <param name="handle"></param>
    void buildIDHandleList( Vector<VsqID> id,  Vector<VsqHandle> handle ) {
        id.clear();
        handle.clear();
        int current_id = -1;
        int current_handle = -1;
        Vector<VsqEvent> items = new Vector<VsqEvent>();
        for ( Iterator itr = events.iterator(); itr.hasNext(); ) {
            items.add( (VsqEvent)itr.next() );
        }
        Collections.sort( items );
        int c = items.size();
        for ( int i = 0; i < c; i++ ) {
            VsqEvent item = items.get( i );
            VsqID id_item = (VsqID)item.id.clone();
            current_id++;
            item.id.value = current_id;
            id_item.value = current_id;
            // IconHandle
            if ( item.id.iconHandle != null ) {
                current_handle++;
                VsqHandle handle_item = item.id.iconHandle.castToVsqHandle();
                handle_item.index = current_handle;
                handle.add( handle_item );
                id_item.iconHandleIndex = current_handle;
            }
            // LyricHandle
            if ( item.id.lyricHandle != null ) {
                current_handle++;
                VsqHandle handle_item = item.id.lyricHandle.castToVsqHandle();
                handle_item.index = current_handle;
                handle.add( handle_item );
                id_item.lyricHandleIndex = current_handle;
            }
            // VibratoHandle
            if ( item.id.vibratoHandle != null ) {
                current_handle++;
                VsqHandle handle_item = item.id.vibratoHandle.castToVsqHandle();
                handle_item.index = current_handle;
                handle.add( handle_item );
                id_item.vibratoHandleIndex = current_handle;
            }
            // NoteHeadHandle
            if ( item.id.noteHeadHandle != null ) {
                current_handle++;
                VsqHandle handle_item = item.id.noteHeadHandle.castToVsqHandle();
                handle_item.index = current_handle;
                handle.add( handle_item );
                id_item.noteHeadHandleIndex = current_handle;
            }
            id.add( id_item );
        }
    }

    /// <summary>
    /// このインスタンスの内容を指定されたファイルに出力します。
    /// </summary>
    /// <param name="sw"></param>
    /// <param name="encode"></param>
    public void print( TextMemoryStream sw, boolean encode, int eos, int start ) throws IOException{
        if ( common != null ) {
            common.write( sw );
        }
        if ( master != null ) {
            master.write( sw );
        }
        if ( mixer != null ) {
            mixer.write( sw );
        }
        Vector<VsqID> id = new Vector<VsqID>();
        Vector<VsqHandle> handle = new Vector<VsqHandle>();
        buildIDHandleList( id, handle );
        writeEventList( sw, eos );
        int c = id.size();
        for ( int i = 0; i < c; i++ ) {
            id.get( i ).write( sw );
        }
        c = handle.size();
        for ( int i = 0; i < c; i++ ) {
            handle.get( i ).write( sw, encode );
        }
        String version = common.version;
        if ( pit.getCount() > 0 ) {
            pit.print( sw, start, "[PitchBendBPList]" );
        }
        if ( pbs.getCount() > 0 ) {
            pbs.print( sw, start, "[PitchBendSensBPList]" );
        }
        if ( dyn.getCount() > 0 ) {
            dyn.print( sw, start, "[DynamicsBPList]" );
        }
        if ( bre.getCount() > 0 ) {
            bre.print( sw, start, "[EpRResidualBPList]" );
        }
        if ( bri.getCount() > 0 ) {
            bri.print( sw, start, "[EpRESlopeBPList]" );
        }
        if ( cle.getCount() > 0 ) {
            cle.print( sw, start, "[EpRESlopeDepthBPList]" );
        }
        if ( version.startsWith( "DSB2" ) ) {
            if ( harmonics.getCount() > 0 ) {
                harmonics.print( sw, start, "[EpRSineBPList]" );
            }
            if ( fx2depth.getCount() > 0 ) {
                fx2depth.print( sw, start, "[VibTremDepthBPList]" );
            }

            if ( reso1FreqBPList.getCount() > 0 ) {
                reso1FreqBPList.print( sw, start, "[Reso1FreqBPList]" );
            }
            if ( reso2FreqBPList.getCount() > 0 ) {
                reso2FreqBPList.print( sw, start, "[Reso2FreqBPList]" );
            }
            if ( reso3FreqBPList.getCount() > 0 ) {
                reso3FreqBPList.print( sw, start, "[Reso3FreqBPList]" );
            }
            if ( reso4FreqBPList.getCount() > 0 ) {
                reso4FreqBPList.print( sw, start, "[Reso4FreqBPList]" );
            }

            if ( reso1BWBPList.getCount() > 0 ) {
                reso1BWBPList.print( sw, start, "[Reso1BWBPList]" );
            }
            if ( reso2BWBPList.getCount() > 0 ) {
                reso2BWBPList.print( sw, start, "[Reso2BWBPList]" );
            }
            if ( reso3BWBPList.getCount() > 0 ) {
                reso3BWBPList.print( sw, start, "[Reso3BWBPList]" );
            }
            if ( reso4BWBPList.getCount() > 0 ) {
                reso4BWBPList.print( sw, start, "[Reso4BWBPList]" );
            }

            if ( reso1AmpBPList.getCount() > 0 ) {
                reso1AmpBPList.print( sw, start, "[Reso1AmpBPList]" );
            }
            if ( reso2AmpBPList.getCount() > 0 ) {
                reso2AmpBPList.print( sw, start, "[Reso2AmpBPList]" );
            }
            if ( reso3AmpBPList.getCount() > 0 ) {
                reso3AmpBPList.print( sw, start, "[Reso3AmpBPList]" );
            }
            if ( reso4AmpBPList.getCount() > 0 ) {
                reso4AmpBPList.print( sw, start, "[Reso4AmpBPList]" );
            }
        }

        if ( gen.getCount() > 0 ) {
            gen.print( sw, start, "[GenderFactorBPList]" );
        }
        if ( por.getCount() > 0 ) {
            por.print( sw, start, "[PortamentoTimingBPList]" );
        }
        if ( version.startsWith( "DSB3" ) ) {
            if ( ope.getCount() > 0 ) {
                ope.print( sw, start, "[OpeningBPList]" );
            }
        }
    }

    private void writeEventList( TextMemoryStream sw, int eos ) throws IOException{
        sw.writeLine( "[EventList]" );
        Vector<VsqEvent> temp = new Vector<VsqEvent>();
        for ( Iterator itr = events.iterator(); itr.hasNext(); ) {
            temp.add( (VsqEvent)itr.next() );
        }
        Collections.sort( temp );
        int i = 0;
        int count = temp.size();
        while ( i < count ) {
            VsqEvent item = temp.get( i );
            if ( !item.id.equals( VsqID.EOS ) ) {
                String ids = "ID#" + (new DecimalFormat( "0000" )).format( i );
                int clock = temp.get( i ).clock;
                while ( i + 1 < count && clock == temp.get( i + 1 ).clock ) {
                    i++;
                    ids += ",ID#" + (new DecimalFormat( "0000" )).format( i );
                }
                sw.writeLine( clock + "=" + ids );
            }
            i++;
        }
        sw.writeLine( eos + "=EOS" );
    }

    /// <summary>
    /// 何も無いVsqMetaTextを構築する。これは、Master Track用のMetaTextとしてのみ使用されるべき
    /// </summary>
    public VsqMetaText() {
    }

    /// <summary>
    /// 最初のトラック以外の一般のメタテキストを構築。(Masterが作られない)
    /// </summary>
    public VsqMetaText( String name, String singer ){
        this( name, 0, singer, false );
    }

    /// <summary>
    /// 最初のトラックのメタテキストを構築。(Masterが作られる)
    /// </summary>
    /// <param name="pre_measure"></param>
    public VsqMetaText( String name, String singer, int pre_measure ){
        this( name, pre_measure, singer, true );
    }

    private VsqMetaText( String name, int pre_measure, String singer, boolean is_first_track ) {
        common = new VsqCommon( name, new Color( 179, 181, 123 ), 1, 1 );
        pit = new VsqBPList( 0, -8192, 8191 );
        //PIT.add( 0, PIT.getDefault() );

        pbs = new VsqBPList( 2, 0, 24 );
        //PBS.add( 0, PBS.getDefault() );

        dyn = new VsqBPList( 64, 0, 127 );
        //DYN.add( 0, DYN.getDefault() );

        bre = new VsqBPList( 0, 0, 127 );
        //BRE.add( 0, BRE.getDefault() );

        bri = new VsqBPList( 64, 0, 127 );
        //BRI.add( 0, BRI.getDefault() );

        cle = new VsqBPList( 0, 0, 127 );
        //CLE.add( 0, CLE.getDefault() );

        reso1FreqBPList = new VsqBPList( 64, 0, 127 );
        //reso1FreqBPList.add( 0, reso1FreqBPList.getDefault() );

        reso2FreqBPList = new VsqBPList( 64, 0, 127 );
        //reso2FreqBPList.add( 0, reso2FreqBPList.getDefault() );

        reso3FreqBPList = new VsqBPList( 64, 0, 127 );
        //reso3FreqBPList.add( 0, reso3FreqBPList.getDefault() );

        reso4FreqBPList = new VsqBPList( 64, 0, 127 );
        //reso4FreqBPList.add( 0, reso4FreqBPList.getDefault() );

        reso1BWBPList = new VsqBPList( 64, 0, 127 );
        //reso1BWBPList.add( 0, reso1BWBPList.getDefault() );

        reso2BWBPList = new VsqBPList( 64, 0, 127 );
        //reso2BWBPList.add( 0, reso2BWBPList.getDefault() );

        reso3BWBPList = new VsqBPList( 64, 0, 127 );
        //reso3BWBPList.add( 0, reso3BWBPList.getDefault() );

        reso4BWBPList = new VsqBPList( 64, 0, 127 );
        //reso4BWBPList.add( 0, reso4BWBPList.getDefault() );

        reso1AmpBPList = new VsqBPList( 64, 0, 127 );
        //reso1AmpBPList.add( 0, reso1AmpBPList.getDefault() );

        reso2AmpBPList = new VsqBPList( 64, 0, 127 );
        //reso2AmpBPList.add( 0, reso2AmpBPList.getDefault() );

        reso3AmpBPList = new VsqBPList( 64, 0, 127 );
        //reso3AmpBPList.add( 0, reso3AmpBPList.getDefault() );

        reso4AmpBPList = new VsqBPList( 64, 0, 127 );
        //reso4AmpBPList.add( 0, reso4AmpBPList.getDefault() );

        harmonics = new VsqBPList( 64, 0, 127 );
        //harmonics.add( 0, harmonics.getDefault() );

        fx2depth = new VsqBPList( 64, 0, 127 );

        gen = new VsqBPList( 64, 0, 127 );
        //GEN.add( 0, GEN.getDefault() );

        por = new VsqBPList( 64, 0, 127 );
        //POR.add( 0, POR.getDefault() );

        ope = new VsqBPList( 127, 0, 127 );
        //OPE.add( 0, OPE.getDefault() );

        if ( is_first_track ) {
            master = new VsqMaster( pre_measure );
        } else {
            master = null;
        }
        events = new VsqEventList();
        VsqID id = new VsqID( 0 );
        id.type = VsqIDType.Singer;
        id.iconHandle = new IconHandle();
        id.iconHandle.iconID = "$07010000";
        id.iconHandle.IDS = singer;
        id.iconHandle.original = 0;
        id.iconHandle.caption = "";
        id.iconHandle.length = 1;
        id.iconHandle.language = 0;
        id.iconHandle.program = 0;
        events.add( new VsqEvent( 0, id ) );
    }
    
    public VsqMetaText( TextMemoryStream sr ) throws IOException{
try{
        Vector<KeyValuePair<Integer, Integer>> t_event_list = new Vector<KeyValuePair<Integer, Integer>>();
        TreeMap<Integer, VsqID> __id = new TreeMap<Integer, VsqID>();
        TreeMap<Integer, VsqHandle> __handle = new TreeMap<Integer, VsqHandle>();
        pit = new VsqBPList( 0, -8192, 8191 );
        pbs = new VsqBPList( 2, 0, 24 );
        dyn = new VsqBPList( 64, 0, 127 );
        bre = new VsqBPList( 0 , 0, 127);
        bri = new VsqBPList( 64, 0, 127 );
        cle = new VsqBPList( 0, 0, 127 );
        reso1FreqBPList = new VsqBPList( 64, 0, 127 );
        reso2FreqBPList = new VsqBPList( 64, 0, 127 );
        reso3FreqBPList = new VsqBPList( 64, 0, 127 );
        reso4FreqBPList = new VsqBPList( 64, 0, 127 );
        reso1BWBPList = new VsqBPList( 64, 0, 127 );
        reso2BWBPList = new VsqBPList( 64, 0, 127 );
        reso3BWBPList = new VsqBPList( 64, 0, 127 );
        reso4BWBPList = new VsqBPList( 64, 0, 127 );
        reso1AmpBPList = new VsqBPList( 64, 0, 127 );
        reso2AmpBPList = new VsqBPList( 64, 0, 127 );
        reso3AmpBPList = new VsqBPList( 64, 0, 127 );
        reso4AmpBPList = new VsqBPList( 64, 0, 127 );
        harmonics = new VsqBPList( 64, 0, 127 );
        fx2depth = new VsqBPList( 64, 0, 127 );
        gen = new VsqBPList( 64, 0, 127 );
        por = new VsqBPList( 64, 0, 127 );
        ope = new VsqBPList( 127, 0, 127 );

        StringBuilder last_line = new StringBuilder();
        last_line.append( sr.readLine() );
        while ( true ) {
            //#region "TextMemoryStreamから順次読込み"
            if ( last_line.length() == 0 ) {
                break;
            }
            String s = last_line.toString();
            if( s.equals( "[Common]" ) ){
                common = new VsqCommon( sr, last_line );
            }else if( s.equals( "[Master]" ) ){
                master = new VsqMaster( sr, last_line );
            }else if( s.equals( "[Mixer]" ) ){
                mixer = new VsqMixer( sr, last_line );
            }else if( s.equals( "[EventList]" ) ){
                last_line.setLength( 0 );
                last_line.append( sr.readLine() );
                while ( !last_line.toString().startsWith( "[" ) ) {
                    String[] spl2 = last_line.toString().split( "=" );
                    int clock = Integer.parseInt( spl2[0] );
                    int id_number = -1;
                    if ( !spl2[1].equals( "EOS" ) ) {
                        String[] ids = spl2[1].split( "," );
                        for ( int i = 0; i < ids.length; i++ ) {
                            String[] spl3 = ids[i].split( "#" );
                            id_number = Integer.parseInt( spl3[1] );
                            t_event_list.add( new KeyValuePair<Integer, Integer>( clock, id_number ) );
                        }
                    } else {
                        t_event_list.add( new KeyValuePair<Integer, Integer>( clock, -1) );
                    }
                    if ( sr.peek() < 0 ) {
                        break;
                    } else {
                        last_line.setLength( 0 );
                        last_line.append( sr.readLine() );
                    }
                }
            }else if( s.equals( "[PitchBendBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( pit.appendFromText( sr ) );
            }else if( s.equals( "[PitchBendSensBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( pbs.appendFromText( sr ) );
            }else if( s.equals( "[DynamicsBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( dyn.appendFromText( sr ) );
            }else if( s.equals( "[EpRResidualBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( bre.appendFromText( sr ) );
            }else if( s.equals( "[EpRESlopeBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( bri.appendFromText( sr ) );
            }else if( s.equals( "[EpRESlopeDepthBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( cle.appendFromText( sr ) );
            }else if( s.equals( "[EpRSineBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( harmonics.appendFromText( sr ) );
            }else if( s.equals( "[VibTremDepthBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( fx2depth.appendFromText( sr ) );
            }else if( s.equals( "[Reso1FreqBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso1FreqBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso2FreqBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso2FreqBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso3FreqBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso3FreqBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso4FreqBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso4FreqBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso1BWBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso1BWBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso2BWBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso2BWBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso3BWBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso3BWBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso4BWBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso4BWBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso1AmpBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso1AmpBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso2AmpBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso2AmpBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso3AmpBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso3AmpBPList.appendFromText( sr ) );
            }else if( s.equals( "[Reso4AmpBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( reso4AmpBPList.appendFromText( sr ) );
            }else if( s.equals( "[GenderFactorBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( gen.appendFromText( sr ) );
            }else if( s.equals( "[PortamentoTimingBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( por.appendFromText( sr ) );
            }else if( s.equals(  "[OpeningBPList]" ) ){
                last_line.setLength( 0 );
                last_line.append( ope.appendFromText( sr ) );
            }else{
                String buffer = last_line.toString();
                buffer = buffer.replace( "[", "" );
                buffer = buffer.replace( "]", "" );
                String[] spl = buffer.split( "#" );
                int index = Integer.parseInt( spl[1] );
                if ( last_line.toString().startsWith( "[ID#" ) ) {
                    __id.put( index, new VsqID( sr, index, last_line ) );
                } else if ( last_line.toString().startsWith( "[h#" ) ) {
                    __handle.put( index, new VsqHandle( sr, index, last_line ) );
                }
            }

            if ( sr.peek() < 0 ) {
                break;
            }
        }

        // まずhandleをidに埋め込み
        //int c = __id.size();
        for( VsqID v : __id.values() ){
        //for ( int i = 0; i < c; i++ ) {
            //VsqID v = __id.get( i );
            if ( __handle.containsKey( v.iconHandleIndex ) ) {
                v.iconHandle = __handle.get( v.iconHandleIndex ).castToIconHandle();
            }
            if ( __handle.containsKey( v.lyricHandleIndex ) ) {
                v.lyricHandle = __handle.get( v.lyricHandleIndex ).castToLyricHandle();
            }
            if ( __handle.containsKey( v.vibratoHandleIndex ) ) {
                v.vibratoHandle = __handle.get( v.vibratoHandleIndex ).castToVibratoHandle();
            }
            if ( __handle.containsKey( v.noteHeadHandleIndex ) ) {
                v.noteHeadHandle = __handle.get( v.noteHeadHandleIndex ).castToNoteHeadHandle();
            }
        }

        // idをeventListに埋め込み
        events = new VsqEventList();
        for( KeyValuePair<Integer, Integer> item : t_event_list ){
        //c = t_event_list.size();
        //for ( int i = 0; i < c; i++ ) {
            //KeyValuePair<Integer, Integer> item = t_event_list.get( i );
            int clock = item.key;
            int id_number = item.value;
            if ( __id.containsKey( id_number ) ) {
                events.add( new VsqEvent( clock, (VsqID)__id.get( id_number ).clone() ) );
            }
        }
}catch( Exception ex ){
    System.out.println( "VsqMetaText(TextMemoryStream); ex=" + ex );
}
    }
}
