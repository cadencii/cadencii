/*
 * VsqEventList.cs
 * Copyright (C) 2008-2010 kbinani
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
import java.util.*;
#else
using System;
using System.Collections.Generic;

namespace org.kbinani.vsq {
    using Integer = System.Int32;
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 固有ID付きのVsqEventのリストを取り扱う
    /// </summary>
#if JAVA
    public class VsqEventList implements Serializable {
#else
    [Serializable]
    public class VsqEventList {
#endif
        public List<VsqEvent> Events;
        private List<Integer> m_ids;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VsqEventList() {
            Events = new List<VsqEvent>();
            m_ids = new List<Integer>();
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getXmlElementName( String name ) {
            return name;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティを，XMLシリアライズ時に無視するかどうかを表す
        /// ブール値を返します．デフォルトの実装では戻り値は全てfalseです．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static boolean isXmlIgnored( String name ) {
            return false;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティが総称型引数を用いる型である場合に，
        /// その型の限定名を返します．それ以外の場合は空文字を返します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getGenericTypeName( String name ) {
            if ( name != null ) {
                if ( name.Equals( "Events" ) ) {
                    return "org.kbinani.vsq.VsqEvent";
                }
            }
            return "";
        }

        public int findIndexFromID( int internal_id ) {
            int c = Events.size();
            for ( int i = 0; i < c; i++ ) {
                VsqEvent item = Events.get( i );
                if ( item.InternalID == internal_id ) {
                    return i;
                }
            }
            return -1;
        }

        public VsqEvent findFromID( int internal_id ) {
            int index = findIndexFromID( internal_id );
            if ( 0 <= index && index < Events.size() ) {
                return Events.get( index );
            } else {
                return null;
            }
        }

        public void setForID( int internal_id, VsqEvent value ) {
            int c = Events.size();
            for ( int i = 0; i < c; i++ ) {
                if ( Events.get( i ).InternalID == internal_id ) {
                    Events.set( i, value );
                    break;
                }
            }
        }

        public void sort() {
            lock ( this ) {
                Collections.sort( Events );
                updateIDList();
            }
        }

        public void clear() {
            Events.clear();
            m_ids.clear();
        }

        public Iterator<VsqEvent> iterator() {
            updateIDList();
            return Events.iterator();
        }

        public int add( VsqEvent item ) {
            int id = getNextId( 0 );
            add( item, id );
            Collections.sort( Events );
            int count = Events.size();
            for ( int i = 0; i < count; i++ ) {
                m_ids.set( i, Events.get( i ).InternalID );
            }
            return id;
        }

        public void add( VsqEvent item, int internal_id ) {
            updateIDList();
            item.InternalID = internal_id;
            Events.add( item );
            m_ids.add( internal_id );
        }

        public void removeAt( int index ) {
            updateIDList();
            Events.removeElementAt( index );
            m_ids.removeElementAt( index );
        }

        private int getNextId( int next ) {
            updateIDList();
            int index = -1;
            List<Integer> current = new List<Integer>( m_ids );
            int nfound = 0;
            while ( true ) {
                index++;
                if ( !current.contains( index ) ) {
                    nfound++;
                    if ( nfound == next + 1 ) {
                        return index;
                    } else {
                        current.add( index );
                    }
                }
            }
        }

        public int getCount() {
            return Events.size();
        }

        public VsqEvent getElement( int index ) {
            return Events.get( index );
        }

        public void setElement( int index, VsqEvent value ) {
            value.InternalID = Events.get( index ).InternalID;
            Events.set( index, value );
        }

        public void updateIDList() {
            if ( m_ids.size() != Events.size() ) {
                m_ids.clear();
                int count = Events.size();
                for ( int i = 0; i < count; i++ ) {
                    m_ids.add( Events.get( i ).InternalID );
                }
            } else {
                int count = Events.size();
                for ( int i = 0; i < count; i++ ) {
                    m_ids.set( i, Events.get( i ).InternalID );
                }
            }
        }
    }

#if !JAVA
}
#endif
