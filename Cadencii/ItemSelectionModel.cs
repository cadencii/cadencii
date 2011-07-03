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
                private Vector<SelectedBezierPoint> mSelectedBezier = new Vector<SelectedBezierPoint>();
                /// <summary>
                /// 最後に選択されたベジエ点
                /// </summary>
                private SelectedBezierPoint mLastSelectedBezier = new SelectedBezierPoint();
                /// <summary>
                /// 選択されている拍子変更イベントのリスト
                /// </summary>
                private TreeMap<Integer, SelectedTimesigEntry> mSelectedTimesig = new TreeMap<Integer, SelectedTimesigEntry>();
                private int mLastSelectedTimesig = -1;
                /// <summary>
                /// 選択されているテンポ変更イベントのリスト
                /// </summary>
                private TreeMap<Integer, SelectedTempoEntry> mSelectedTempo = new TreeMap<Integer, SelectedTempoEntry>();
                private int mLastSelectedTempo = -1;
                /// <summary>
                /// 選択されているイベントのリスト
                /// </summary>
                private Vector<SelectedEventEntry> mSelectedEvents = new Vector<SelectedEventEntry>();
                private Vector<Long> mSelectedPointIDs = new Vector<Long>();
                /// <summary>
                /// selectedPointIDsに格納されているデータ点の，CurveType
                /// </summary>
                private CurveType mSelectedPointCurveType = CurveType.Empty;

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

                #region SelectedBezier
                /// <summary>
                /// 選択されているベジエ曲線のデータ点を順に返す反復子を取得します。
                /// </summary>
                /// <returns></returns>
                public Iterator<SelectedBezierPoint> getSelectedBezierIterator()
                {
                    return mSelectedBezier.iterator();
                }

                /// <summary>
                /// 最後に選択状態となったベジエ曲線のデータ点を取得します。
                /// </summary>
                /// <returns>最後に選択状態となったベジエ曲線のデータ点を返します。選択状態となっているベジエ曲線がなければnullを返します。</returns>
                public SelectedBezierPoint getLastSelectedBezier()
                {
                    if ( mLastSelectedBezier.chainID < 0 || mLastSelectedBezier.pointID < 0 )
                    {
                        return null;
                    }
                    else
                    {
                        return mLastSelectedBezier;
                    }
                }

                /// <summary>
                /// 指定されたベジエ曲線のデータ点を選択状態にします。
                /// </summary>
                /// <param name="selected">選択状態にするデータ点。</param>
                public void addSelectedBezier( SelectedBezierPoint selected )
                {
                    mLastSelectedBezier = selected;
                    int index = -1;
                    for ( int i = 0; i < mSelectedBezier.size(); i++ )
                    {
                        if ( mSelectedBezier.get( i ).chainID == selected.chainID &&
                            mSelectedBezier.get( i ).pointID == selected.pointID )
                        {
                            index = i;
                            break;
                        }
                    }
                    if ( index >= 0 )
                    {
                        mSelectedBezier.set( index, selected );
                    }
                    else
                    {
                        mSelectedBezier.add( selected );
                    }
                    checkSelectedItemExistence();
                }

                /// <summary>
                /// すべてのベジエ曲線のデータ点の選択状態を解除します。
                /// </summary>
                public void clearSelectedBezier()
                {
                    mSelectedBezier.clear();
                    mLastSelectedBezier.chainID = -1;
                    mLastSelectedBezier.pointID = -1;
                    checkSelectedItemExistence();
                }
                #endregion

                #region SelectedTimesig
                /// <summary>
                /// 最後に選択状態となった拍子変更設定を取得します。
                /// </summary>
                /// <returns>最後に選択状態となった拍子変更設定を返します。選択状態となっている拍子変更設定が無ければnullを返します。</returns>
                public SelectedTimesigEntry getLastSelectedTimesig()
                {
                    if ( mSelectedTimesig.containsKey( mLastSelectedTimesig ) )
                    {
                        return mSelectedTimesig.get( mLastSelectedTimesig );
                    }
                    else
                    {
                        return null;
                    }
                }

                public int getLastSelectedTimesigBarcount()
                {
                    return mLastSelectedTimesig;
                }

                public void addSelectedTimesig( int barcount )
                {
                    clearSelectedEvent(); //ここ注意！
                    clearSelectedTempo();
                    mLastSelectedTimesig = barcount;
                    if ( !mSelectedTimesig.containsKey( barcount ) )
                    {
                        for ( Iterator<TimeSigTableEntry> itr = AppManager.getVsqFile().TimesigTable.iterator(); itr.hasNext(); )
                        {
                            TimeSigTableEntry tte = itr.next();
                            if ( tte.BarCount == barcount )
                            {
                                mSelectedTimesig.put( barcount, new SelectedTimesigEntry( tte, (TimeSigTableEntry)tte.clone() ) );
                                break;
                            }
                        }
                    }
                    checkSelectedItemExistence();
                }

                public void clearSelectedTimesig()
                {
                    mSelectedTimesig.clear();
                    mLastSelectedTimesig = -1;
                    checkSelectedItemExistence();
                }

                public int getSelectedTimesigCount()
                {
                    return mSelectedTimesig.size();
                }

                public Iterator<ValuePair<Integer, SelectedTimesigEntry>> getSelectedTimesigIterator()
                {
                    Vector<ValuePair<Integer, SelectedTimesigEntry>> list = new Vector<ValuePair<Integer, SelectedTimesigEntry>>();
                    for ( Iterator<Integer> itr = mSelectedTimesig.keySet().iterator(); itr.hasNext(); )
                    {
                        int clock = itr.next();
                        list.add( new ValuePair<Integer, SelectedTimesigEntry>( clock, mSelectedTimesig.get( clock ) ) );
                    }
                    return list.iterator();
                }

                public bool isSelectedTimesigContains( int barcount )
                {
                    return mSelectedTimesig.containsKey( barcount );
                }

                public SelectedTimesigEntry getSelectedTimesig( int barcount )
                {
                    if ( mSelectedTimesig.containsKey( barcount ) )
                    {
                        return mSelectedTimesig.get( barcount );
                    }
                    else
                    {
                        return null;
                    }
                }

                public void removeSelectedTimesig( int barcount )
                {
                    if ( mSelectedTimesig.containsKey( barcount ) )
                    {
                        mSelectedTimesig.remove( barcount );
                        checkSelectedItemExistence();
                    }
                }
                #endregion

                #region SelectedTempo
                public SelectedTempoEntry getLastSelectedTempo()
                {
                    if ( mSelectedTempo.containsKey( mLastSelectedTempo ) )
                    {
                        return mSelectedTempo.get( mLastSelectedTempo );
                    }
                    else
                    {
                        return null;
                    }
                }

                public int getLastSelectedTempoClock()
                {
                    return mLastSelectedTempo;
                }

                public void addSelectedTempo( int clock )
                {
                    clearSelectedEvent(); //ここ注意！
                    clearSelectedTimesig();
                    mLastSelectedTempo = clock;
                    if ( !mSelectedTempo.containsKey( clock ) )
                    {
                        for ( Iterator<TempoTableEntry> itr = AppManager.getVsqFile().TempoTable.iterator(); itr.hasNext(); )
                        {
                            TempoTableEntry tte = itr.next();
                            if ( tte.Clock == clock )
                            {
                                mSelectedTempo.put( clock, new SelectedTempoEntry( tte, (TempoTableEntry)tte.clone() ) );
                                break;
                            }
                        }
                    }
                    checkSelectedItemExistence();
                }

                public void clearSelectedTempo()
                {
                    mSelectedTempo.clear();
                    mLastSelectedTempo = -1;
                    checkSelectedItemExistence();
                }

                public int getSelectedTempoCount()
                {
                    return mSelectedTempo.size();
                }

                public Iterator<ValuePair<Integer, SelectedTempoEntry>> getSelectedTempoIterator()
                {
                    Vector<ValuePair<Integer, SelectedTempoEntry>> list = new Vector<ValuePair<Integer, SelectedTempoEntry>>();
                    for ( Iterator<Integer> itr = mSelectedTempo.keySet().iterator(); itr.hasNext(); )
                    {
                        int clock = itr.next();
                        list.add( new ValuePair<Integer, SelectedTempoEntry>( clock, mSelectedTempo.get( clock ) ) );
                    }
                    return list.iterator();
                }

                public bool isSelectedTempoContains( int clock )
                {
                    return mSelectedTempo.containsKey( clock );
                }

                public SelectedTempoEntry getSelectedTempo( int clock )
                {
                    if ( mSelectedTempo.containsKey( clock ) )
                    {
                        return mSelectedTempo.get( clock );
                    }
                    else
                    {
                        return null;
                    }
                }

                public void removeSelectedTempo( int clock )
                {
                    if ( mSelectedTempo.containsKey( clock ) )
                    {
                        mSelectedTempo.remove( clock );
                        checkSelectedItemExistence();
                    }
                }
                #endregion

                #region SelectedEvent
                public void removeSelectedEvent( int id )
                {
                    removeSelectedEventCor( id, false );
                    checkSelectedItemExistence();
                }

                public void removeSelectedEventSilent( int id )
                {
                    removeSelectedEventCor( id, true );
                    checkSelectedItemExistence();
                }

                private void removeSelectedEventCor( int id, bool silent )
                {
                    int count = mSelectedEvents.size();
                    for ( int i = 0; i < count; i++ )
                    {
                        if ( mSelectedEvents.get( i ).original.InternalID == id )
                        {
                            mSelectedEvents.removeElementAt( i );
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

                public void removeSelectedEventRange( int[] ids )
                {
                    Vector<Integer> v_ids = new Vector<Integer>( Arrays.asList( PortUtil.convertIntArray( ids ) ) );
                    Vector<Integer> index = new Vector<Integer>();
                    int count = mSelectedEvents.size();
                    for ( int i = 0; i < count; i++ )
                    {
                        if ( v_ids.contains( mSelectedEvents.get( i ).original.InternalID ) )
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
                        mSelectedEvents.removeElementAt( i );
                    }
#if ENABLE_PROPERTY
                    AppManager.propertyPanel.updateValue( AppManager.getSelected() );
#endif
                    checkSelectedItemExistence();
                }

                public void addSelectedEventAll( Vector<Integer> list )
                {
                    clearSelectedTempo();
                    clearSelectedTimesig();
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
                        if ( !isSelectedEventContains( selected, index[i].InternalID ) )
                        {
                            mSelectedEvents.add( new SelectedEventEntry( selected, index[i], (VsqEvent)index[i].clone() ) );
                        }
                    }
#if ENABLE_PROPERTY
                    AppManager.propertyPanel.updateValue( selected );
#endif
                    checkSelectedItemExistence();
                }

                public void addSelectedEvent( int id )
                {
                    addSelectedEventCor( id, false );
                    checkSelectedItemExistence();
                }

                public void addSelectedEventSilent( int id )
                {
                    addSelectedEventCor( id, true );
                    checkSelectedItemExistence();
                }

                private void addSelectedEventCor( int id, bool silent )
                {
                    clearSelectedTempo();
                    clearSelectedTimesig();
                    int selected = AppManager.getSelected();
                    for ( Iterator<VsqEvent> itr = AppManager.getVsqFile().Track.get( selected ).getEventIterator(); itr.hasNext(); )
                    {
                        VsqEvent ev = itr.next();
                        if ( ev.InternalID == id )
                        {
                            if ( isSelectedEventContains( selected, id ) )
                            {
                                // すでに選択されていた場合
                                int count = mSelectedEvents.size();
                                for ( int i = 0; i < count; i++ )
                                {
                                    SelectedEventEntry item = mSelectedEvents.get( i );
                                    if ( item.original.InternalID == id )
                                    {
                                        mSelectedEvents.removeElementAt( i );
                                        break;
                                    }
                                }
                            }

                            mSelectedEvents.add( new SelectedEventEntry( selected, ev, (VsqEvent)ev.clone() ) );
                            if ( !silent )
                            {
                                try
                                {
#if JAVA
                                    selectedEventChangedEvent.raise( typeof( AppManager ), false );
#elif QT_VERSION
                                    selectedEventChanged( this, false );
#else
                                    if ( SelectedEventChanged != null )
                                    {
                                        SelectedEventChanged.Invoke( typeof( AppManager ), false );
                                    }
#endif
                                }
                                catch ( Exception ex )
                                {
                                    serr.println( "AppManager#addSelectedEventCor; ex=" + ex );
                                    Logger.write( typeof( AppManager ) + ".addSelectedCurveCor; ex=" + ex + "\n" );
                                }
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

                public void clearSelectedEvent()
                {
                    mSelectedEvents.clear();
#if ENABLE_PROPERTY
                    AppManager.propertyPanel.updateValue( AppManager.getSelected() );
#endif
                    checkSelectedItemExistence();
                }

                public bool isSelectedEventContains( int track, int id )
                {
                    int count = mSelectedEvents.size();
                    for ( int i = 0; i < count; i++ )
                    {
                        SelectedEventEntry item = mSelectedEvents.get( i );
                        if ( item.original.InternalID == id && item.track == track )
                        {
                            return true;
                        }
                    }
                    return false;
                }

                public Iterator<SelectedEventEntry> getSelectedEventIterator()
                {
                    return mSelectedEvents.iterator();
                }

                public SelectedEventEntry getLastSelectedEvent()
                {
                    if ( mSelectedEvents.size() <= 0 )
                    {
                        return null;
                    }
                    else
                    {
                        return mSelectedEvents.get( mSelectedEvents.size() - 1 );
                    }
                }

                public int getSelectedEventCount()
                {
                    return mSelectedEvents.size();
                }
                #endregion

                #region SelectedPoint
                public void clearSelectedPoint()
                {
                    mSelectedPointIDs.clear();
                    mSelectedPointCurveType = CurveType.Empty;
                    checkSelectedItemExistence();
                }

                public void addSelectedPoint( CurveType curve, long id )
                {
                    addSelectedPointAll( curve, new long[] { id } );
                    checkSelectedItemExistence();
                }

                public void addSelectedPointAll( CurveType curve, long[] ids )
                {
                    if ( !curve.equals( mSelectedPointCurveType ) )
                    {
                        mSelectedPointIDs.clear();
                        mSelectedPointCurveType = curve;
                    }
                    for ( int i = 0; i < ids.Length; i++ )
                    {
                        if ( !mSelectedPointIDs.contains( ids[i] ) )
                        {
                            mSelectedPointIDs.add( ids[i] );
                        }
                    }
                    checkSelectedItemExistence();
                }

                public bool isSelectedPointContains( long id )
                {
                    return mSelectedPointIDs.contains( id );
                }

                public CurveType getSelectedPointCurveType()
                {
                    return mSelectedPointCurveType;
                }

                public Iterator<Long> getSelectedPointIDIterator()
                {
                    return mSelectedPointIDs.iterator();
                }

                public int getSelectedPointIDCount()
                {
                    return mSelectedPointIDs.size();
                }

                public void removeSelectedPoint( long id )
                {
                    mSelectedPointIDs.removeElement( id );
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

                    for ( int i = 0; i < mSelectedEvents.size(); i++ )
                    {
                        SelectedEventEntry item = mSelectedEvents.get( i );
                        VsqEvent ev = null;
                        if ( item.track == selected )
                        {
                            int internal_id = item.original.InternalID;
                            ev = vsq_track.findEventFromID( internal_id );
                        }
                        if ( ev != null )
                        {
                            mSelectedEvents.set( i, new SelectedEventEntry( selected, ev, (VsqEvent)ev.clone() ) );
                        }
                        else
                        {
                            mSelectedEvents.removeElementAt( i );
                            i--;
                        }
                    }
                }

                /// <summary>
                /// 現在選択されたアイテムが存在するかどうかを調べ，必要であればSelectedEventChangedイベントを発生させます
                /// </summary>
                private void checkSelectedItemExistence()
                {
                    bool ret = mSelectedBezier.size() == 0 &&
                                  mSelectedEvents.size() == 0 &&
                                  mSelectedTempo.size() == 0 &&
                                  mSelectedTimesig.size() == 0 &&
                                  mSelectedPointIDs.size() == 0;
                    try
                    {
#if JAVA
                        selectedEventChangedEvent.raise( typeof( AppManager ), ret );
#elif QT_VERSION
                        selectedEventChanged( this, ret );
#else
                        if ( SelectedEventChanged != null )
                        {
                            SelectedEventChanged.Invoke( typeof( AppManager ), ret );
                        }
#endif
                    }
                    catch ( Exception ex )
                    {
                        serr.println( "AppManager#checkSelectedItemExistence; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".checkSelectedItemExistence; ex=" + ex + "\n" );
                    }
                }

            }

#if !JAVA
        }
    }
}
#endif
