/*
 * ItemSelectionModel.cs
 * Copyright © 2011 kbinani
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
#if JAVA

package org.kbinani.cadencii;

import java.util.*;
import org.kbinani.*;
import org.kbinani.vsq.*;

#else

namespace org{
    namespace kbinani{
        namespace cadencii{

#if __cplusplus
#else
            using System;
            using org.kbinani.java.util;
            using org.kbinani.vsq;
            using Integer = System.Int32;
            using Long = System.Int64;
#endif

#endif

            /// <summary>
            /// アイテムの選択状態を管理するクラスです．
            /// </summary>
            public class ItemSelectionModel
            {
                /// <summary>
                /// 選択されているベジエ点のリスト
                /// </summary>
                private Vector<SelectedBezierPoint> mBezier = new Vector<SelectedBezierPoint>();
                /// <summary>
                /// 最後に選択されたベジエ点
                /// </summary>
                private SelectedBezierPoint mSelectedBezier = new SelectedBezierPoint();
                /// <summary>
                /// 選択されている拍子変更イベントのリスト
                /// </summary>
                private TreeMap<Integer, SelectedTimesigEntry> mTimesig = new TreeMap<Integer, SelectedTimesigEntry>();
                private int mSelectedTimesig = -1;
                /// <summary>
                /// 選択されているテンポ変更イベントのリスト
                /// </summary>
                private TreeMap<Integer, SelectedTempoEntry> mTempo = new TreeMap<Integer, SelectedTempoEntry>();
                private int mLastTempo = -1;
                /// <summary>
                /// 選択されているイベントのリスト
                /// </summary>
                private Vector<SelectedEventEntry> mEvents = new Vector<SelectedEventEntry>();
                private Vector<Long> mPointIDs = new Vector<Long>();
                /// <summary>
                /// selectedPointIDsに格納されているデータ点の，CurveType
                /// </summary>
                private CurveType mPointCurveType = CurveType.Empty;

                /// <summary>
                /// 選択状態のアイテムが変化した時発生するイベント
                /// </summary>
#if JAVA
                public BEvent<SelectedEventChangedEventHandler> selectedEventChangedEvent = new BEvent<SelectedEventChangedEventHandler>();
#elif QT_VERSION
                public: signals: void selectedEventChanged( QObject sender, bool foo );
#else
                public event SelectedEventChangedEventHandler SelectedEventChanged;
#endif

                #region Bezier
                /// <summary>
                /// 選択されているベジエ曲線のデータ点を順に返す反復子を取得します。
                /// </summary>
                /// <returns></returns>
                public Iterator<SelectedBezierPoint> getBezierIterator()
                {
                    return mBezier.iterator();
                }

                /// <summary>
                /// 最後に選択状態となったベジエ曲線のデータ点を取得します。
                /// </summary>
                /// <returns>最後に選択状態となったベジエ曲線のデータ点を返します。選択状態となっているベジエ曲線がなければnullを返します。</returns>
                public SelectedBezierPoint getLastBezier()
                {
                    if ( mSelectedBezier.chainID < 0 || mSelectedBezier.pointID < 0 )
                    {
                        return null;
                    }
                    else
                    {
                        return mSelectedBezier;
                    }
                }

                /// <summary>
                /// 指定されたベジエ曲線のデータ点を選択状態にします。
                /// </summary>
                /// <param name="selected">選択状態にするデータ点。</param>
                public void addBezier( SelectedBezierPoint selected )
                {
                    mSelectedBezier = selected;
                    int index = -1;
                    for ( int i = 0; i < mBezier.size(); i++ )
                    {
                        if ( mBezier.get( i ).chainID == selected.chainID &&
                            mBezier.get( i ).pointID == selected.pointID )
                        {
                            index = i;
                            break;
                        }
                    }
                    if ( index >= 0 )
                    {
                        mBezier.set( index, selected );
                    }
                    else
                    {
                        mBezier.add( selected );
                    }
                    checkSelectedItemExistence();
                }

                /// <summary>
                /// すべてのベジエ曲線のデータ点の選択状態を解除します。
                /// </summary>
                public void clearBezier()
                {
                    mBezier.clear();
                    mSelectedBezier.chainID = -1;
                    mSelectedBezier.pointID = -1;
                    checkSelectedItemExistence();
                }
                #endregion

                #region Timesig
                /// <summary>
                /// 最後に選択状態となった拍子変更設定を取得します。
                /// </summary>
                /// <returns>最後に選択状態となった拍子変更設定を返します。選択状態となっている拍子変更設定が無ければnullを返します。</returns>
                public SelectedTimesigEntry getLastTimesig()
                {
                    if ( mTimesig.containsKey( mSelectedTimesig ) )
                    {
                        return mTimesig.get( mSelectedTimesig );
                    }
                    else
                    {
                        return null;
                    }
                }

                public int getLastTimesigBarcount()
                {
                    return mSelectedTimesig;
                }

                public void addTimesig( int barcount )
                {
                    clearEvent(); //ここ注意！
                    clearTempo();
                    mSelectedTimesig = barcount;
                    if ( !mTimesig.containsKey( barcount ) )
                    {
                        for ( Iterator<TimeSigTableEntry> itr = AppManager.getVsqFile().TimesigTable.iterator(); itr.hasNext(); )
                        {
                            TimeSigTableEntry tte = itr.next();
                            if ( tte.BarCount == barcount )
                            {
                                mTimesig.put( barcount, new SelectedTimesigEntry( tte, (TimeSigTableEntry)tte.clone() ) );
                                break;
                            }
                        }
                    }
                    checkSelectedItemExistence();
                }

                public void clearTimesig()
                {
                    mTimesig.clear();
                    mSelectedTimesig = -1;
                    checkSelectedItemExistence();
                }

                public int getTimesigCount()
                {
                    return mTimesig.size();
                }

                public Iterator<ValuePair<Integer, SelectedTimesigEntry>> getTimesigIterator()
                {
                    Vector<ValuePair<Integer, SelectedTimesigEntry>> list = new Vector<ValuePair<Integer, SelectedTimesigEntry>>();
                    for ( Iterator<Integer> itr = mTimesig.keySet().iterator(); itr.hasNext(); )
                    {
                        int clock = itr.next();
                        list.add( new ValuePair<Integer, SelectedTimesigEntry>( clock, mTimesig.get( clock ) ) );
                    }
                    return list.iterator();
                }

                public bool isTimesigContains( int barcount )
                {
                    return mTimesig.containsKey( barcount );
                }

                public SelectedTimesigEntry getTimesig( int barcount )
                {
                    if ( mTimesig.containsKey( barcount ) )
                    {
                        return mTimesig.get( barcount );
                    }
                    else
                    {
                        return null;
                    }
                }

                public void removeTimesig( int barcount )
                {
                    if ( mTimesig.containsKey( barcount ) )
                    {
                        mTimesig.remove( barcount );
                        checkSelectedItemExistence();
                    }
                }
                #endregion

                #region Tempo
                public SelectedTempoEntry getLastTempo()
                {
                    if ( mTempo.containsKey( mLastTempo ) )
                    {
                        return mTempo.get( mLastTempo );
                    }
                    else
                    {
                        return null;
                    }
                }

                public int getLastTempoClock()
                {
                    return mLastTempo;
                }

                public void addTempo( int clock )
                {
                    clearEvent(); //ここ注意！
                    clearTimesig();
                    mLastTempo = clock;
                    if ( !mTempo.containsKey( clock ) )
                    {
                        for ( Iterator<TempoTableEntry> itr = AppManager.getVsqFile().TempoTable.iterator(); itr.hasNext(); )
                        {
                            TempoTableEntry tte = itr.next();
                            if ( tte.Clock == clock )
                            {
                                mTempo.put( clock, new SelectedTempoEntry( tte, (TempoTableEntry)tte.clone() ) );
                                break;
                            }
                        }
                    }
                    checkSelectedItemExistence();
                }

                public void clearTempo()
                {
                    mTempo.clear();
                    mLastTempo = -1;
                    checkSelectedItemExistence();
                }

                public int getTempoCount()
                {
                    return mTempo.size();
                }

                public Iterator<ValuePair<Integer, SelectedTempoEntry>> getTempoIterator()
                {
                    Vector<ValuePair<Integer, SelectedTempoEntry>> list = new Vector<ValuePair<Integer, SelectedTempoEntry>>();
                    for ( Iterator<Integer> itr = mTempo.keySet().iterator(); itr.hasNext(); )
                    {
                        int clock = itr.next();
                        list.add( new ValuePair<Integer, SelectedTempoEntry>( clock, mTempo.get( clock ) ) );
                    }
                    return list.iterator();
                }

                public bool isTempoContains( int clock )
                {
                    return mTempo.containsKey( clock );
                }

                public SelectedTempoEntry getTempo( int clock )
                {
                    if ( mTempo.containsKey( clock ) )
                    {
                        return mTempo.get( clock );
                    }
                    else
                    {
                        return null;
                    }
                }

                public void removeTempo( int clock )
                {
                    if ( mTempo.containsKey( clock ) )
                    {
                        mTempo.remove( clock );
                        checkSelectedItemExistence();
                    }
                }
                #endregion

                #region Event
                public void removeEvent( int id )
                {
                    removeEventCor( id, false );
                    checkSelectedItemExistence();
                }

                public void removeEventSilent( int id )
                {
                    removeEventCor( id, true );
                    checkSelectedItemExistence();
                }

                private void removeEventCor( int id, bool silent )
                {
                    int count = mEvents.size();
                    for ( int i = 0; i < count; i++ )
                    {
                        if ( mEvents.get( i ).original.InternalID == id )
                        {
                            mEvents.removeElementAt( i );
                            break;
                        }
                    }
                    if ( !silent )
                    {
#if ENABLE_PROPERTY
                        AppManager.propertyPanel.updateValue( AppManager.getSelected() );
#endif
                    }
                }

                public void removeEventRange( int[] ids )
                {
                    Vector<Integer> v_ids = new Vector<Integer>( Arrays.asList( PortUtil.convertIntArray( ids ) ) );
                    Vector<Integer> index = new Vector<Integer>();
                    int count = mEvents.size();
                    for ( int i = 0; i < count; i++ )
                    {
                        if ( v_ids.contains( mEvents.get( i ).original.InternalID ) )
                        {
                            index.add( i );
                            if ( index.size() == ids.Length )
                            {
                                break;
                            }
                        }
                    }
                    count = index.size();
                    for ( int i = count - 1; i >= 0; i-- )
                    {
                        mEvents.removeElementAt( i );
                    }
#if ENABLE_PROPERTY
                    AppManager.propertyPanel.updateValue( AppManager.getSelected() );
#endif
                    checkSelectedItemExistence();
                }

                public void addEventAll( Vector<Integer> list )
                {
                    clearTempo();
                    clearTimesig();
                    VsqEvent[] index = new VsqEvent[list.size()];
                    int count = 0;
                    int c = list.size();
                    int selected = AppManager.getSelected();
                    for ( Iterator<VsqEvent> itr = AppManager.getVsqFile().Track.get( selected ).getEventIterator(); itr.hasNext(); )
                    {
                        VsqEvent ev = itr.next();
                        int find = -1;
                        for ( int i = 0; i < c; i++ )
                        {
                            if ( list.get( i ) == ev.InternalID )
                            {
                                find = i;
                                break;
                            }
                        }
                        if ( 0 <= find )
                        {
                            index[find] = ev;
                            count++;
                        }
                        if ( count == list.size() )
                        {
                            break;
                        }
                    }
                    for ( int i = 0; i < index.Length; i++ )
                    {
                        if ( !isEventContains( selected, index[i].InternalID ) )
                        {
                            mEvents.add( new SelectedEventEntry( selected, index[i], (VsqEvent)index[i].clone() ) );
                        }
                    }
#if ENABLE_PROPERTY
                    AppManager.propertyPanel.updateValue( selected );
#endif
                    checkSelectedItemExistence();
                }

                public void addEvent( int id )
                {
                    addEventCor( id, false );
                    checkSelectedItemExistence();
                }

                public void addEventSilent( int id )
                {
                    addEventCor( id, true );
                    checkSelectedItemExistence();
                }

                private void addEventCor( int id, bool silent )
                {
                    clearTempo();
                    clearTimesig();
                    int selected = AppManager.getSelected();
                    for ( Iterator<VsqEvent> itr = AppManager.getVsqFile().Track.get( selected ).getEventIterator(); itr.hasNext(); )
                    {
                        VsqEvent ev = itr.next();
                        if ( ev.InternalID == id )
                        {
                            if ( isEventContains( selected, id ) )
                            {
                                // すでに選択されていた場合
                                int count = mEvents.size();
                                for ( int i = 0; i < count; i++ )
                                {
                                    SelectedEventEntry item = mEvents.get( i );
                                    if ( item.original.InternalID == id )
                                    {
                                        mEvents.removeElementAt( i );
                                        break;
                                    }
                                }
                            }

                            mEvents.add( new SelectedEventEntry( selected, ev, (VsqEvent)ev.clone() ) );
                            if ( !silent )
                            {
                                invokeSelectedEventChangedEvent( false );
                            }
                            break;
                        }
                    }
                    if ( !silent )
                    {
#if ENABLE_PROPERTY
                        AppManager.propertyPanel.updateValue( selected );
#endif
                    }
                }

                public void clearEvent()
                {
                    mEvents.clear();
#if ENABLE_PROPERTY
                    AppManager.propertyPanel.updateValue( AppManager.getSelected() );
#endif
                    checkSelectedItemExistence();
                }

                public bool isEventContains( int track, int id )
                {
                    int count = mEvents.size();
                    for ( int i = 0; i < count; i++ )
                    {
                        SelectedEventEntry item = mEvents.get( i );
                        if ( item.original.InternalID == id && item.track == track )
                        {
                            return true;
                        }
                    }
                    return false;
                }

                public Iterator<SelectedEventEntry> getEventIterator()
                {
                    return mEvents.iterator();
                }

                public SelectedEventEntry getLastEvent()
                {
                    if ( mEvents.size() <= 0 )
                    {
                        return null;
                    }
                    else
                    {
                        return mEvents.get( mEvents.size() - 1 );
                    }
                }

                public int getEventCount()
                {
                    return mEvents.size();
                }
                #endregion

                #region Point
                public void clearPoint()
                {
                    mPointIDs.clear();
                    mPointCurveType = CurveType.Empty;
                    checkSelectedItemExistence();
                }

                public void addPoint( CurveType curve, long id )
                {
                    addPointAll( curve, new long[] { id } );
                    checkSelectedItemExistence();
                }

                public void addPointAll( CurveType curve, long[] ids )
                {
                    if ( !curve.equals( mPointCurveType ) )
                    {
                        mPointIDs.clear();
                        mPointCurveType = curve;
                    }
                    for ( int i = 0; i < ids.Length; i++ )
                    {
                        if ( !mPointIDs.contains( ids[i] ) )
                        {
                            mPointIDs.add( ids[i] );
                        }
                    }
                    checkSelectedItemExistence();
                }

                public bool isPointContains( long id )
                {
                    return mPointIDs.contains( id );
                }

                public CurveType getPointCurveType()
                {
                    return mPointCurveType;
                }

                public Iterator<Long> getPointIDIterator()
                {
                    return mPointIDs.iterator();
                }

                public int getPointIDCount()
                {
                    return mPointIDs.size();
                }

                public void removePoint( long id )
                {
                    mPointIDs.removeElement( id );
                    checkSelectedItemExistence();
                }
                #endregion

                /// <summary>
                /// 選択中のアイテムが編集された場合、編集にあわせてオブジェクトを更新する。
                /// </summary>
                public void updateSelectedEventInstance()
                {
                    VsqFileEx vsq = AppManager.getVsqFile();
                    if ( vsq == null )
                    {
                        return;
                    }
                    int selected = AppManager.getSelected();
                    VsqTrack vsq_track = vsq.Track.get( selected );

                    for ( int i = 0; i < mEvents.size(); i++ )
                    {
                        SelectedEventEntry item = mEvents.get( i );
                        VsqEvent ev = null;
                        if ( item.track == selected )
                        {
                            int internal_id = item.original.InternalID;
                            ev = vsq_track.findEventFromID( internal_id );
                        }
                        if ( ev != null )
                        {
                            mEvents.set( i, new SelectedEventEntry( selected, ev, (VsqEvent)ev.clone() ) );
                        }
                        else
                        {
                            mEvents.removeElementAt( i );
                            i--;
                        }
                    }
                }

                /// <summary>
                /// 現在選択されたアイテムが存在するかどうかを調べ，必要であればSelectedEventChangedイベントを発生させます
                /// </summary>
                private void checkSelectedItemExistence()
                {
                    bool ret = mBezier.size() == 0 &&
                               mEvents.size() == 0 &&
                               mTempo.size() == 0 &&
                               mTimesig.size() == 0 &&
                               mPointIDs.size() == 0;
                    invokeSelectedEventChangedEvent( ret );
                }

                /// <summary>
                /// SelectedEventChangedEventを発生させます．
                /// </summary>
                /// <param name="ret"></param>
                private void invokeSelectedEventChangedEvent( bool ret )
                {
                    try
                    {
#if JAVA
                        selectedEventChangedEvent.raise( typeof( ItemSelectionModel ), ret );
#elif QT_VERSION
                        selectedEventChanged( this, ret );
#else
                        if ( SelectedEventChanged != null )
                        {
                            SelectedEventChanged.Invoke( typeof( ItemSelectionModel ), ret );
                        }
#endif
                    }
                    catch ( Exception ex )
                    {
                        serr.println( "ItemSelectionModel#checkSelectedItemExistence; ex=" + ex );
                        Logger.write( typeof( ItemSelectionModel ) + ".checkSelectedItemExistence; ex=" + ex + "\n" );
                    }
                }

            }

#if !JAVA
        }
    }
}
#endif
