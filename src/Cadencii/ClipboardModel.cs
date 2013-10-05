/*
 * ClipboardModel.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System.Collections.Generic;

namespace cadencii
{

    using System;
    using cadencii.vsq;
    using cadencii.java.util;
    using System.Windows.Forms;

    /// <summary>
    /// クリップボードを管理するクラスです．
    /// </summary>
    public class ClipboardModel
    {
#if CLIPBOARD_AS_TEXT
                /// <summary>
                /// OSのクリップボードに貼り付ける文字列の接頭辞．
                /// これがついていた場合，クリップボードの文字列をCadenciiが使用できると判断する．
                /// </summary>
                public const String CLIP_PREFIX = "CADENCIIOBJ";
#endif

#if CLIPBOARD_AS_TEXT
                /// <summary>
                /// オブジェクトをシリアライズし，クリップボードに格納するための文字列を作成します
                /// </summary>
                /// <param name="obj">シリアライズするオブジェクト</param>
                /// <returns>シリアライズされた文字列</returns>
                private String getSerializedText( Object obj )
                {
                    String str = "";
                    ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
                    ObjectOutputStream objectOutputStream = new ObjectOutputStream( outputStream );
                    objectOutputStream.writeObject( obj );

                    byte[] arr = outputStream.toByteArray();
                    str = CLIP_PREFIX + ":" + obj.GetType().FullName + ":" + Base64.encode( arr );
                    return str;
                }

                /// <summary>
                /// クリップボードに格納された文字列を元に，デシリアライズされたオブジェクトを取得します
                /// </summary>
                /// <param name="s"></param>
                /// <returns></returns>
                private Object getDeserializedObjectFromText( String s )
                {
                    if ( s.StartsWith( CLIP_PREFIX ) )
                    {
                        int index = s.IndexOf( ":" );
                        index = s.IndexOf( ":", index + 1 );
                        Object ret = null;
                        try
                        {
                            ByteArrayInputStream bais = new ByteArrayInputStream( Base64.decode( str.sub( s, index + 1 ) ) );
                            ObjectInputStream ois = new ObjectInputStream( bais );
                            ret = ois.readObject();
                        }
                        catch ( Exception ex )
                        {
                            ret = null;
                            Logger.write( typeof( ClipboardModel ) + ".getDeserializedObjectFromText; ex=" + ex + "\n" );
                        }
                        return ret;
                    }
                    else
                    {
                        return null;
                    }
                }
#endif

        /// <summary>
        /// クリップボードにオブジェクトを貼り付けます．
        /// </summary>
        /// <param name="item">貼り付けるオブジェクトを格納したClipboardEntryのインスタンス</param>
        public void setClipboard(ClipboardEntry item)
        {
#if CLIPBOARD_AS_TEXT
                    String clip = "";
                    try
                    {
                        clip = getSerializedText( item );
#if DEBUG
                        sout.println( "ClipboardModel#setClipboard; clip=" + clip );
#endif
                    }
                    catch ( Exception ex )
                    {
                        serr.println( "ClipboardModel#setClipboard; ex=" + ex );
                        Logger.write( typeof( ClipboardModel ) + ".setClipboard; ex=" + ex + "\n" );
                        return;
                    }
                    PortUtil.setClipboardText( clip );
#else
            Clipboard.SetDataObject(item, false);
#endif
        }

        /// <summary>
        /// クリップボードにオブジェクトを貼り付けるためのユーティリティ．
        /// </summary>
        /// <param name="events"></param>
        /// <param name="tempo"></param>
        /// <param name="timesig"></param>
        /// <param name="curve"></param>
        /// <param name="bezier"></param>
        /// <param name="copy_started_clock"></param>
        private void setClipboard(
            List<VsqEvent> events,
            List<TempoTableEntry> tempo,
            List<TimeSigTableEntry> timesig,
            SortedDictionary<CurveType, VsqBPList> curve,
            SortedDictionary<CurveType, List<BezierChain>> bezier,
            int copy_started_clock)
        {
            ClipboardEntry ce = new ClipboardEntry();
            ce.events = events;
            ce.tempo = tempo;
            ce.timesig = timesig;
            ce.points = curve;
            ce.beziers = bezier;
            ce.copyStartedClock = copy_started_clock;
#if CLIPBOARD_AS_TEXT
                    String clip = "";
                    try
                    {
                        clip = getSerializedText( ce );
#if DEBUG
                        sout.println( "ClipboardModel#setClipboard; clip=" + clip );
#endif
                    }
                    catch ( Exception ex )
                    {
                        serr.println( "ClipboardModel#setClipboard; ex=" + ex );
                        Logger.write( typeof( ClipboardModel ) + ".setClipboard; ex=" + ex + "\n" );
                        return;
                    }
                    PortUtil.setClipboardText( clip );
#else // CLIPBOARD_AS_TEXT
#if DEBUG
            // ClipboardEntryがシリアライズ可能かどうかを試すため，
            // この部分のコードは残しておくこと
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = null;
            System.IO.MemoryStream ms = null;
            try {
                bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                ms = new System.IO.MemoryStream();
                bf.Serialize(ms, ce);
            } catch (Exception ex) {
                sout.println("ClipboardModel#setClipboard; ex=" + ex);
            }
#endif // DEBUG
            Clipboard.SetDataObject(ce, false);
#endif // CLIPBOARD_AS_TEXT
        }

        /// <summary>
        /// クリップボードに貼り付けられたアイテムを取得します．
        /// </summary>
        /// <returns>クリップボードに貼り付けられたアイテムを格納したClipboardEntryのインスタンス</returns>
        public ClipboardEntry getCopiedItems()
        {
            ClipboardEntry ce = null;
#if CLIPBOARD_AS_TEXT
                    String clip = PortUtil.getClipboardText();
                    if ( clip != null && str.startsWith( clip, CLIP_PREFIX ) ) {
                        int index1 = clip.IndexOf( ":" );
                        int index2 = clip.IndexOf( ":", index1 + 1 );
                        String typename = str.sub( clip, index1 + 1, index2 - index1 - 1 );
#if DEBUG
                        sout.println( "ClipboardModel#getCopiedItems; typename=" + typename );
#endif
                        if ( typename.Equals( typeof( ClipboardEntry ).FullName ) ) {
                            try {
                                ce = (ClipboardEntry)getDeserializedObjectFromText( clip );
                            } catch ( Exception ex ) {
                                Logger.write( typeof( ClipboardModel ) + ".getCopiedItems; ex=" + ex + "\n" );
                            }
                        }
                    }
#else
            IDataObject dobj = Clipboard.GetDataObject();
            if (dobj != null) {
                Object obj = dobj.GetData(typeof(ClipboardEntry));
                if (obj != null && obj is ClipboardEntry) {
                    ce = (ClipboardEntry)obj;
                }
            }
#endif
            if (ce == null) {
                ce = new ClipboardEntry();
            }
            if (ce.beziers == null) {
                ce.beziers = new SortedDictionary<CurveType, List<BezierChain>>();
            }
            if (ce.events == null) {
                ce.events = new List<VsqEvent>();
            }
            if (ce.points == null) {
                ce.points = new SortedDictionary<CurveType, VsqBPList>();
            }
            if (ce.tempo == null) {
                ce.tempo = new List<TempoTableEntry>();
            }
            if (ce.timesig == null) {
                ce.timesig = new List<TimeSigTableEntry>();
            }
            return ce;
        }

        /// <summary>
        /// VsqEventのリストをクリップボードにセットします．
        /// </summary>
        /// <param name="item">セットするVsqEventのリスト</param>
        /// <param name="copy_started_clock"></param>
        public void setCopiedEvent(List<VsqEvent> item, int copy_started_clock)
        {
            setClipboard(item, null, null, null, null, copy_started_clock);
        }

        /// <summary>
        /// テンポ変更イベント(TempoTableEntry)のリストをクリップボードにセットします．
        /// </summary>
        /// <param name="item">セットするTempoTableEntryのリスト</param>
        /// <param name="copy_started_clock"></param>
        public void setCopiedTempo(List<TempoTableEntry> item, int copy_started_clock)
        {
            setClipboard(null, item, null, null, null, copy_started_clock);
        }

        /// <summary>
        /// 拍子変更イベント(TimeSigTableEntry)のリストをクリップボードにセットします．
        /// </summary>
        /// <param name="item">セットする拍子変更イベントのリスト</param>
        /// <param name="copy_started_clock"></param>
        public void setCopiedTimesig(List<TimeSigTableEntry> item, int copy_started_clock)
        {
            setClipboard(null, null, item, null, null, copy_started_clock);
        }

        /// <summary>
        /// コントロールカーブをクリップボードにセットします．
        /// </summary>
        /// <param name="item">セットするコントロールカーブ</param>
        /// <param name="copy_started_clock"></param>
        public void setCopiedCurve(SortedDictionary<CurveType, VsqBPList> item, int copy_started_clock)
        {
            setClipboard(null, null, null, item, null, copy_started_clock);
        }

        /// <summary>
        /// ベジエ曲線をクリップボードにセットします．
        /// </summary>
        /// <param name="item">セットするベジエ曲線</param>
        /// <param name="copy_started_clock"></param>
        public void setCopiedBezier(SortedDictionary<CurveType, List<BezierChain>> item, int copy_started_clock)
        {
            setClipboard(null, null, null, null, item, copy_started_clock);
        }
    }

}
