/*
 * VsqEventList.js
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
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.VsqEventList == undefined ){

    /// <summary>
    /// 固有ID付きのVsqEventのリストを取り扱う
    /// </summary>
    org.kbinani.vsq.VsqEventList = function(){
        /**
         * [Vector<VsqEvent>]
         */
        this.Events = new Array();
        /**
         * [Vector<int>]
         */
        this.m_ids = new Array();
    };

    org.kbinani.vsq.VsqEventList.prototype = {
        /**
         * @param internal_id [int]
         * @return [int]
         */
        findIndexFromID : function( internal_id ) {
            var c = this.Events.length;
            for ( var i = 0; i < c; i++ ) {
                var item = this.Events[i];
                if ( item.InternalID == internal_id ) {
                    return i;
                }
            }
            return -1;
        },

        /**
         * @param internal_id [int]
         * @return [VsqEvent]
         */
        findFromID : function( internal_id ) {
            var index = findIndexFromID( internal_id );
            if ( 0 <= index && index < this.Events.length ) {
                return this.Events[index];
            } else {
                return null;
            }
        },

        /**
         * @param internal_id [int]
         * @param value [VsqEvent]
         * @return [void]
         */
        setForID : function( internal_id, value ) {
            var c = this.Events.length;
            for ( var i = 0; i < c; i++ ) {
                if ( this.Events[i].InternalID == internal_id ) {
                    this.Events[i] = value;
                    break;
                }
            }
        },

        /**
         * @return [void]
         */
        sort : function() {
            //lock ( this )
            {
                this.Events.sort( org.kbinani.vsq.VsqEvent.compare );
                updateIDList();
            }
        },

        /**
         * @return [void]
         */
        public void clear() {
            var c = this.Events.length;
            for( var i = 0; i < c; i++ ){
                this.Events.pop();
                this._m_ids.pop();
            }
        },

        /**
         * @return [ArrayIterator(VsqEven)]
         */
        iterator : function() {
            updateIDList();
            return new org.kbinani.ArrayIterator( this.Events );
        },

        /**
         * overload1
         * @param item [VsqEvent]
         * @param internal_id [int]
         * @return [int]
         *
         * overload2
         * @param item [VsqEvent]
         * @return [int]
         */
        add : function() {
            if( arguments.length == 1 ){
                var item = arguments[0];
                var id = getNextId( 0 );
                _addCor( item, id );
                this.Events.sort( org.kbinani.vsq.VsqEvent.compare );
                var count = this.Events.length;
                for ( var i = 0; i < count; i++ ) {
                    this._m_ids[i] = this.Events[i].InternalID;
                }
                return id;
            }else if ( arguments.length == 2 ){
                var item = arguments[0];
                var internal_id = arguments[1];
                _addCor( item, internal_id );
                return internal_id;
            }
            return -1;
        },

        /**
         * @param item [VsqEvent]
         * @param internal_id [int]
         * @return [void]
         */
        _addCor : function( item, internal_id ){
            updateIDList();
            item.InternalID = internal_id;
            this.Events.push( item );
            this._m_ids.push( internal_id );
        },

        /**
         * @param index [int]
         * @return [void]
         */
        removeAt : function( index ) {
            updateIDList();
            this.Events.splice( index, 1 );
            this._m_ids.splice( index, 1 );
        },

        /**
         * @param next [int]
         * @return [int]
         */
        _getNextId : function( next ) {
            updateIDList();
            var index = -1;
            Vector<Integer> current = new Vector<Integer>( m_ids );
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
