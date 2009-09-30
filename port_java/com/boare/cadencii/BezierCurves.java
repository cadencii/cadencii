/*
 * BezierCurves.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

import java.util.*;

/// <summary>
/// AtatchedCurveをXMLシリアライズするためのクラス
/// </summary>
public class BezierCurves implements Cloneable {
    public Vector<BezierChain> Dynamics;
    public Vector<BezierChain> Brethiness;
    public Vector<BezierChain> Brightness;
    public Vector<BezierChain> Clearness;
    public Vector<BezierChain> Opening;
    public Vector<BezierChain> GenderFactor;
    public Vector<BezierChain> PortamentoTiming;
    public Vector<BezierChain> PitchBend;
    public Vector<BezierChain> PitchBendSensitivity;
    public Vector<BezierChain> VibratoRate;
    public Vector<BezierChain> VibratoDepth;
    public Vector<BezierChain> Harmonics;
    public Vector<BezierChain> FX2Depth;
    public Vector<BezierChain> Reso1Freq;
    public Vector<BezierChain> Reso1BW;
    public Vector<BezierChain> Reso1Amp;
    public Vector<BezierChain> Reso2Freq;
    public Vector<BezierChain> Reso2BW;
    public Vector<BezierChain> Reso2Amp;
    public Vector<BezierChain> Reso3Freq;
    public Vector<BezierChain> Reso3BW;
    public Vector<BezierChain> Reso3Amp;
    public Vector<BezierChain> Reso4Freq;
    public Vector<BezierChain> Reso4BW;
    public Vector<BezierChain> Reso4Amp;
   
    public BezierCurves() {
        Dynamics = new Vector<BezierChain>();
        Brethiness = new Vector<BezierChain>();
        Brightness = new Vector<BezierChain>();
        Clearness = new Vector<BezierChain>();
        Opening = new Vector<BezierChain>();
        GenderFactor = new Vector<BezierChain>();
        PortamentoTiming = new Vector<BezierChain>();
        PitchBend = new Vector<BezierChain>();
        PitchBendSensitivity = new Vector<BezierChain>();
        VibratoRate = new Vector<BezierChain>();
        VibratoDepth = new Vector<BezierChain>();
        Harmonics = new Vector<BezierChain>();
        FX2Depth = new Vector<BezierChain>();
        Reso1Freq = new Vector<BezierChain>();
        Reso1BW = new Vector<BezierChain>();
        Reso1Amp = new Vector<BezierChain>();
        Reso2Freq = new Vector<BezierChain>();
        Reso2BW = new Vector<BezierChain>();
        Reso2Amp = new Vector<BezierChain>();
        Reso3Freq = new Vector<BezierChain>();
        Reso3BW = new Vector<BezierChain>();
        Reso3Amp = new Vector<BezierChain>();
        Reso4Freq = new Vector<BezierChain>();
        Reso4BW = new Vector<BezierChain>();
        Reso4Amp = new Vector<BezierChain>();
    }

    public BezierChain getBezierChain( CurveType curve_type, int chain_id ) {
        Vector<BezierChain> list = this.get( curve_type );
        int c = list.size();
        for ( int i = 0; i < c; i++ ) {
            BezierChain bc = list.get( i );
            if ( bc.ID == chain_id ) {
                return bc;
            }
        }
        return null;
    }

    public void setBezierChain( CurveType curve_type, int chain_id, BezierChain item ) {
        Vector<BezierChain> list = this.get( curve_type );
        int c = list.size();
        for ( int i = 0; i < c; i++ ) {
            BezierChain bc = list.get( i );
            if ( bc.ID == chain_id ) {
                list.set( i, item );
                break;
            }
        }
    }

    /// <summary>
    /// 指定した種類のコントロールカーブにベジエ曲線を追加します。
    /// AddBezierChainとの違い、オーバーラップする部分があれば自動的に結合されます。
    /// chainには2個以上のデータ点が含まれている必要がある
    /// </summary>
    /// <param name="curve"></param>
    /// <param name="chain"></param>
    public void mergeBezierChain( CurveType curve, BezierChain chain ) {
        if ( chain.points.size() <= 1 ) {
            return;
        }
        int chain_start = (int)chain.getStart();
        int chain_end = (int)chain.getEnd();

        // まず、全削除する必要のあるBezierChainを検索
        Vector<Integer> delete_list = new Vector<Integer>();
        Vector<BezierChain> src = this.get( curve );
        for ( int j = 0; j < src.size(); j++ ) {
            BezierChain bc = src.get( j );
            if ( bc.points.size() <= 0 ) {
                continue;
            }
            int bc_start = (int)bc.getStart();
            int bc_end = (int)bc.getEnd();
            if ( chain_start <= bc_start && bc_end <= chain_end ) {
                delete_list.add( bc.ID );
            }
        }

        // 削除を実行
        for ( int id : delete_list ) {
            remove( curve, id );
        }

        // マージする必要があるかどうかを検査。
        boolean processed = true;
        while ( processed ) {
            processed = false;
            Vector<BezierChain> list = this.get( curve );
            for ( int j = 0; j < list.size(); j++ ) {
                BezierChain bc = list.get( j );
                int id = bc.ID;
                int start = (int)bc.getStart();
                int end = (int)bc.getEnd();

                // 被っている箇所が2箇所以上ある可能性があるので、ifでヒットしてもbreakしない
                if ( start < chain_start && chain_start <= end && end < chain_end ) {
                    // bcのchain_start ~ endを削除し、chain_startで結合
                    BezierChain bc_edit = bc.extractPartialBezier( start, chain_start );
                    bc_edit.ID = bc.ID;
                    int last = bc_edit.getCount() - 1;

                    // 接合部分では、制御点無しでステップ変化する
                    bc_edit.points.get( last ).setControlRightType( BezierControlType.None );
                    chain.points.get( 0 ).setControlLeftType( BezierControlType.None );

                    int copy_start = 0;
                    if ( bc_edit.points.get( last ).getBase().Y == chain.points.get( 0 ).getBase().Y ) {
                        // bcの終点とchainの始点の座標が一致している場合
                        if ( bc_edit.points.get( last ).getControlLeftType() != BezierControlType.None ) {
                            bc_edit.points.get( last ).setControlLeftType( BezierControlType.Master );
                        }
                        bc_edit.points.get( last ).setControlRight( chain.points.get( 0 ).internalControlLeft );
                        if ( chain.points.get( 0 ).getControlRightType() != BezierControlType.None ) {
                            bc_edit.points.get( last ).setControlLeftType( BezierControlType.Master );
                        }
                        copy_start = 1;
                    }
                    for ( int i = copy_start; i < chain.points.size(); i++ ) {
                        chain.points.get( i ).setIDInternal( bc_edit.getNextId() );
                        bc_edit.add( chain.points.get( i ) );
                    }
                    remove( curve, id );
                    chain = bc_edit;
                    chain_start = (int)chain.getStart();
                    chain_end = (int)chain.getEnd();
                    processed = true;
                    break;
                } else if ( chain_start < start && start <= chain_end && chain_end < end ) {
                    // bcのstart ~ chain_endを削除し、chain_endで結合
                    BezierChain bc_edit = bc.extractPartialBezier( chain_end, end );
                    bc_edit.ID = bc.ID;
                    int last = chain.getCount() - 1;

                    // 接合部分では、制御点無しでステップ変化する
                    bc_edit.points.get( 0 ).setControlLeftType( BezierControlType.None );
                    chain.points.get( last ).setControlRightType( BezierControlType.None );

                    int copy_end = last;
                    if ( chain.points.get( last ).getBase().Y == bc_edit.points.get( 0 ).getBase().Y ) {
                        // bcの終点とchainの始点の座標が一致している場合
                        if ( chain.get( last ).getControlLeftType() != BezierControlType.None ) {
                            chain.points.get( last ).setControlLeftType( BezierControlType.Master );
                        }
                        chain.points.get( last ).setControlRight( bc_edit.points.get( 0 ).internalControlLeft );
                        if ( bc_edit.points.get( 0 ).getControlRightType() != BezierControlType.None ) {
                            chain.points.get( last ).setControlLeftType( BezierControlType.Master );
                        }
                        copy_end = last - 1;
                    }
                    for ( int i = 0; i <= copy_end; i++ ) {
                        chain.points.get( i ).setIDInternal( bc_edit.getNextId() );
                        bc_edit.add( chain.points.get( i ) );
                    }
                    remove( curve, id );
                    chain = bc_edit;
                    chain_start = (int)chain.getStart();
                    chain_end = (int)chain.getEnd();
                    processed = true;
                    break;
                } else if ( start < chain_start && chain_end < end ) {
                    // bcのchain_start ~ chain_endをchainで置き換え
                    // left + chain + right
                    BezierChain left = bc.extractPartialBezier( start, chain_start );
                    BezierChain right = bc.extractPartialBezier( chain_end, end );
                    left.ID = bc.ID;

                    // 接合部ではステップ変化
                    left.points.get( left.getCount() - 1 ).setControlRightType( BezierControlType.None );
                    chain.points.get( 0 ).setControlLeftType( BezierControlType.None );
                    chain.points.get( chain.getCount() - 1 ).setControlRightType( BezierControlType.None );
                    right.points.get( 0 ).setControlLeftType( BezierControlType.None );

                    int copy_start = 0;
                    int copy_end = chain.getCount() - 1;

                    if ( left.points.get( left.getCount() - 1 ).getBase().Y == chain.points.get( 0 ).getBase().Y ) {
                        // bcの終点とchainの始点の座標が一致している場合
                        if ( left.points.get( left.getCount() - 1 ).getControlLeftType() != BezierControlType.None ) {
                            left.points.get( left.getCount() - 1 ).setControlLeftType( BezierControlType.Master );
                        }
                        left.points.get( left.getCount() - 1 ).setControlRight( chain.points.get( 0 ).internalControlLeft );
                        if ( chain.points.get( 0 ).getControlRightType() != BezierControlType.None ) {
                            left.points.get( left.getCount() - 1 ).setControlLeftType( BezierControlType.Master );
                        }
                        copy_start = 1;
                    }

                    if ( chain.points.get( chain.getCount() - 1 ).getBase().Y == right.points.get( 0 ).getBase().Y ) {
                        // bcの終点とchainの始点の座標が一致している場合
                        if ( chain.points.get( chain.getCount() - 1 ).getControlLeftType() != BezierControlType.None ) {
                            chain.points.get( chain.getCount() - 1 ).setControlLeftType( BezierControlType.Master );
                        }
                        chain.points.get( chain.getCount() - 1 ).setControlRight( right.points.get( 0 ).internalControlLeft );
                        if ( right.points.get( 0 ).getControlRightType() != BezierControlType.None ) {
                            chain.points.get( chain.getCount() - 1 ).setControlLeftType( BezierControlType.Master );
                        }
                        copy_end = chain.getCount() - 2;
                    }

                    // 追加
                    for ( int i = copy_start; i <= copy_end; i++ ) {
                        chain.points.get( i ).setIDInternal( left.getNextId() );
                        left.add( chain.points.get( i ) );
                    }
                    for ( int i = 0; i < right.points.size(); i++ ) {
                        right.points.get( i ).setIDInternal( left.getNextId() );
                        left.add( right.points.get( i ) );
                    }
                    remove( curve, id );
                    chain = left;
                    chain_start = (int)chain.getStart();
                    chain_end = (int)chain.getEnd();
                    processed = true;
                    break;
                }
            }
        }

        if ( !processed ) {
            chain.ID = this.getNextId( curve );
        }
        addBezierChain( curve, chain, chain.ID );
    }

    public boolean DeleteBeziers(
        Vector<CurveType> target_curve,
        int clock_start,
        int clock_end
    ) {
        boolean edited = false;
        for ( CurveType curve : target_curve ) {
            if ( curve.isScalar() || curve.isAttachNote() ) {
                continue;
            }
            Vector<BezierChain> tmp = new Vector<BezierChain>();
            for( BezierChain bc : this.get( curve ) ) {
                int len = bc.points.size();
                if ( len < 1 ) {
                    continue;
                }
                int chain_start = (int)bc.points.get( 0 ).getBase().X;
                int chain_end;
                if ( len < 2 ) {
                    chain_end = chain_start;
                } else {
                    chain_end = (int)bc.points.get( len - 1 ).getBase().X;
                }
                if ( clock_end < chain_start && chain_start < clock_end && clock_end < chain_end ) {
                    // end ~ chain_endを残す
                    BezierChain chain = bc.extractPartialBezier( clock_end, chain_end );
                    chain.ID = bc.ID;
                    tmp.add( chain );
                    edited = true;
                } else if ( chain_start <= clock_start && clock_end <= chain_end ) {
                    // chain_start ~ startとend ~ chain_endを残す
                    BezierChain chain1 = bc.extractPartialBezier( chain_start, clock_start );
                    chain1.ID = bc.ID;
                    BezierChain chain2 = bc.extractPartialBezier( clock_end, chain_end );
                    chain2.ID = -1;  // 後で番号をつける
                    tmp.add( chain1 );
                    tmp.add( chain2 );
                    edited = true;
                } else if ( chain_start < clock_start && clock_start < chain_end && clock_end <= chain_end ) {
                    // chain_start ~ startを残す
                    BezierChain chain = bc.extractPartialBezier( chain_start, clock_start );
                    chain.ID = bc.ID;
                    tmp.add( chain );
                    edited = true;
                } else if ( clock_start <= chain_start && chain_end <= clock_end ) {
                    // 全体を削除
                    edited = true;
                } else {
                    // 全体を残す
                    tmp.add( (BezierChain)bc.clone() );
                }
            }
            this.get( curve ).clear();
            for( BezierChain bc : tmp ){
                if ( bc.ID >= 0 ) {
                    addBezierChain( curve, bc, bc.ID );
                }
            }
            for( BezierChain bc : tmp ) {
                if ( bc.ID < 0 ) {
                    bc.ID = this.getNextId( curve );
                    addBezierChain( curve, bc, bc.ID );
                }
            }
        }
        return edited;
    }

    public void remove( CurveType curve_type, int chain_id ) {
        Vector<BezierChain> list = this.get( curve_type );
        for ( int i = 0; i < list.size(); i++ ) {
            if ( list.get( i ).ID == chain_id ) {
                list.removeElementAt( i );
                break;
            }
        }
    }

    /// <summary>
    /// 指定したコントロールカーブにベジエ曲線を追加します。
    /// </summary>
    /// <param name="curve_type"></param>
    /// <param name="chain"></param>
    public void addBezierChain( CurveType curve_type, BezierChain chain, int chain_id ) {
        int index = curve_type.getIndex();
        BezierChain add = (BezierChain)chain.clone();
        add.ID = chain_id;
        this.get( curve_type ).add( add );
    }

    public Object clone() {
        BezierCurves ret = new BezierCurves();
        for( CurveType ct : CurveType.CURVE_USAGE ){
            Vector<BezierChain> src = this.get( ct );
            ret.set( ct, new Vector<BezierChain>() );
            for ( int i = 0; i < src.size(); i++ ) {
                ret.get( ct ).add( (BezierChain)src.get( i ).clone() );
            }
        }
        return ret;
    }

    public int getNextId( CurveType curve_type ) {
        int index = curve_type.getIndex();
        Vector<BezierChain> bc = this.get( curve_type );
        int ret = bc.size();
        boolean found = true;
        while ( found ) {
            found = false;
            for ( int i = 0; i < bc.size(); i++ ) {
                if ( bc.get( i ).ID == ret ) {
                    found = true;
                    ret++;
                    break;
                }
            }
        }
        return ret;
    }

    public Vector<BezierChain> get( CurveType curve ){
        if ( curve == CurveType.BRE ) {
            return Brethiness;
        } else if ( curve == CurveType.BRI ) {
            return Brightness;
        } else if ( curve == CurveType.CLE ) {
            return Clearness;
        } else if ( curve == CurveType.DYN ) {
            return Dynamics;
        } else if ( curve == CurveType.fx2depth ) {
            return FX2Depth;
        } else if ( curve == CurveType.GEN ) {
            return GenderFactor;
        } else if ( curve == CurveType.harmonics ) {
            return Harmonics;
        } else if ( curve == CurveType.OPE ) {
            return Opening;
        } else if ( curve == CurveType.PIT ) {
            return PitchBend;
        }else if( curve.equals( CurveType.PBS ) ){
            return PitchBendSensitivity;
        } else if ( curve == CurveType.POR ) {
            return PortamentoTiming;
        } else if ( curve == CurveType.reso1amp ) {
            return Reso1Amp;
        } else if ( curve == CurveType.reso1bw ) {
            return Reso1BW;
        } else if ( curve == CurveType.reso1freq ) {
            return Reso1Freq;
        } else if ( curve == CurveType.reso2amp ) {
            return Reso2Amp;
        } else if ( curve == CurveType.reso2bw ) {
            return Reso2BW;
        } else if ( curve == CurveType.reso2freq ) {
            return Reso2Freq;
        } else if ( curve == CurveType.reso3amp ) {
            return Reso3Amp;
        } else if ( curve == CurveType.reso3bw ) {
            return Reso3BW;
        } else if ( curve == CurveType.reso3freq ) {
            return Reso3Freq;
        } else if ( curve == CurveType.reso4amp ) {
            return Reso4Amp;
        } else if ( curve == CurveType.reso4bw ) {
            return Reso4BW;
        } else if ( curve == CurveType.reso4freq ) {
            return Reso4Freq;
        } else if ( curve == CurveType.VibratoDepth ) {
            return VibratoDepth;
        } else if ( curve == CurveType.VibratoRate ) {
            return VibratoRate;
        } else {
            return null;
        }
    }

    public void set( CurveType curve, Vector<BezierChain> value ){
        if ( curve == CurveType.BRE ) {
            Brethiness = value;
        } else if ( curve == CurveType.BRI ) {
            Brightness = value;
        } else if ( curve == CurveType.CLE ) {
            Clearness = value;
        } else if ( curve == CurveType.DYN ) {
            Dynamics = value;
        } else if ( curve == CurveType.fx2depth ) {
            FX2Depth = value;
        } else if ( curve == CurveType.GEN ) {
            GenderFactor = value;
        } else if ( curve == CurveType.harmonics ) {
            Harmonics = value;
        } else if ( curve == CurveType.OPE ) {
            Opening = value;
        } else if ( curve == CurveType.PIT ) {
            PitchBend = value;
        }else if( curve.equals( CurveType.PBS ) ){
            PitchBendSensitivity = value;
        } else if ( curve == CurveType.POR ) {
            PortamentoTiming = value;
        } else if ( curve == CurveType.reso1amp ) {
            Reso1Amp = value;
        } else if ( curve == CurveType.reso1bw ) {
            Reso1BW = value;
        } else if ( curve == CurveType.reso1freq ) {
            Reso1Freq = value;
        } else if ( curve == CurveType.reso2amp ) {
            Reso2Amp = value;
        } else if ( curve == CurveType.reso2bw ) {
            Reso2BW = value;
        } else if ( curve == CurveType.reso2freq ) {
            Reso2Freq = value;
        } else if ( curve == CurveType.reso3amp ) {
            Reso3Amp = value;
        } else if ( curve == CurveType.reso3bw ) {
            Reso3BW = value;
        } else if ( curve == CurveType.reso3freq ) {
            Reso3Freq = value;
        } else if ( curve == CurveType.reso4amp ) {
            Reso4Amp = value;
        } else if ( curve == CurveType.reso4bw ) {
            Reso4BW = value;
        } else if ( curve == CurveType.reso4freq ) {
            Reso4Freq = value;
        } else if ( curve == CurveType.VibratoDepth ) {
            VibratoDepth = value;
        } else if ( curve == CurveType.VibratoRate ) {
            VibratoRate = value;
        }
    }
}
