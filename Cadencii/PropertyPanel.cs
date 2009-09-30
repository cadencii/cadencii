/*
 * PropertyPanel.cs
 * Copyright (c) 2009 kbinani
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
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;

using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;
    using Integer = Int32;

    public delegate void CommandExecuteRequiredEventHandler( CadenciiCommand command );

    public partial class PropertyPanel : UserControl {
        public event CommandExecuteRequiredEventHandler CommandExecuteRequired;
        private Vector<VsqEventItemProxy> m_items;
        private int m_track;

        public PropertyPanel() {
            InitializeComponent();
            m_items = new Vector<VsqEventItemProxy>();
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public boolean Editing {
            get;
            set;
        }

        private void popGridItemExpandStatus() {
            if ( propertyGrid.SelectedGridItem == null ) {
                return;
            }

            GridItem root = findRootGridItem( propertyGrid.SelectedGridItem );
            if ( root == null ) {
                return;
            }

            popGridItemExpandStatusCore( root );
        }

        private void popGridItemExpandStatusCore( GridItem item ) {
            if ( item.Expandable ) {
                String s = getGridItemIdentifier( item );
                for ( Iterator itr = AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.iterator(); itr.hasNext(); ){
                    ValuePair<String, boolean> v = (ValuePair<String, boolean>)itr.next();
                    if ( v.Key.Equals( s ) ){
                        item.Expanded = v.Value;
                        break;
                    }
                }
            }
            foreach ( GridItem child in item.GridItems ) {
                popGridItemExpandStatusCore( child );
            }
        }

        private void pushGridItemExpandStatus() {
            if ( propertyGrid.SelectedGridItem == null ) {
                return;
            }

            GridItem root = findRootGridItem( propertyGrid.SelectedGridItem );
            if( root == null ){
                return;
            }

            pushGridItemExpandStatusCore( root );
        }

        private void pushGridItemExpandStatusCore( GridItem item ) {
            if ( item.Expandable ) {
                String s = getGridItemIdentifier( item );
                boolean found = false;
                for ( Iterator itr = AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.iterator(); itr.hasNext(); ){
                    ValuePair<String, boolean> v = (ValuePair<String, boolean>)itr.next();
                    if ( v.Key.Equals( s ) ) {
                        found = true;
                        v.Value = item.Expanded;
                    }
                }
                if ( !found ) {
                    AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.add( new ValuePair<String, boolean>( s, item.Expanded ) );
                }
            }
            foreach( GridItem child in item.GridItems ){
                pushGridItemExpandStatusCore( child );
            }
        }

        public void UpdateValue( int track ) {
            m_track = track;
            m_items.clear();

            // 現在のGridItemの展開状態を取得
            pushGridItemExpandStatus();

            // InternalIDを列挙
            Vector<Integer> items = new Vector<Integer>();
            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                SelectedEventEntry item = (SelectedEventEntry)itr.next();
                if ( item.track == track ) {
                    items.add( item.original.InternalID );
                }
            }

            // itemsの中身を列挙
            int count = 0;
            for ( Iterator itr = AppManager.getVsqFile().Track.get( m_track ).getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                if ( items.contains( ve.InternalID ) ) {
                    count++;
                    m_items.add( new VsqEventItemProxy( ve ) );
                }
                if ( count == items.size() ) {
                    break;
                }
            }

            object[] objs = new object[m_items.size()];
            for ( int i = 0; i < m_items.size(); i++ ) {
                objs[i] = m_items.get( i );
            }
            propertyGrid.SelectedObjects = objs;
            popGridItemExpandStatus();
            Editing = false;
        }

        private void propertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e ) {
            String name = e.ChangedItem.PropertyDescriptor.Name;
            object old_value = e.OldValue;
            int len = propertyGrid.SelectedObjects.Length;
            VsqEvent[] items = new VsqEvent[len];
            for ( int i = 0; i < len; i++ ) {
                VsqEventItemProxy proxy = (VsqEventItemProxy)propertyGrid.SelectedObjects[i];

                items[i] = proxy.GetItemDifference();

                VsqEventItemProxy item = m_items.get( i );
                item.original.Clock = proxy.Clock.Clock.getIntValue();
                item.original.ID.DEMaccent = proxy.Accent;
                item.original.ID.DEMdecGainRate = proxy.Decay;
                item.original.ID.Dynamics = proxy.Velocity;
                item.original.ID.Length = proxy.Length.getIntValue();
                item.original.ID.LyricHandle.L0.setPhoneticSymbol( proxy.PhoneticSymbol );
                item.original.ID.LyricHandle.L0.Phrase = proxy.Phrase;
                item.original.ID.Note = proxy.Note.noteNumber;
                item.original.ID.PMBendDepth = proxy.BendDepth;
                item.original.ID.PMBendLength = proxy.BendLength;
                item.original.ID.PMbPortamentoUse = proxy.GetPortamentoUsage();
                item.original.ID.VibratoDelay = VsqEventItemProxy.GetVibratoDelay( item.VibratoLength, item.original.ID.Length );
                item.original.UstEvent.PreUtterance = proxy.PreUtterance;
                item.original.UstEvent.VoiceOverlap = proxy.Overlap;
                item.original.UstEvent.Moduration = proxy.Moduration;
                item.original.ID.d4mean = proxy.d4mean;
                item.original.ID.pMeanEndingNote = proxy.pMeanEndingNote;
                item.original.ID.pMeanOnsetFirstNote = proxy.pMeanOnsetFirstNote;
                item.original.ID.vMeanNoteTransition = proxy.vMeanNoteTransition;
            }
            if ( CommandExecuteRequired != null ) {
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplaceRange( m_track, items ) );
                CommandExecuteRequired( run );
            }
            propertyGrid.Refresh();
            Editing = false;
        }

        /// <summary>
        /// itemが属しているGridItemツリーの基点にある親を探します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private GridItem findRootGridItem( GridItem item ) {
            if ( item.Parent == null ) {
                return item;
            } else {
                return findRootGridItem( item.Parent );
            }
        }

        /// <summary>
        /// itemが属しているGridItemツリーの中で，itemを特定するための文字列を取得します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private String getGridItemIdentifier( GridItem item ) {
            if ( item.Parent == null ) {
                if ( item.PropertyDescriptor != null ) {
                    return item.PropertyDescriptor.Name;
                } else {
                    return item.Label;
                }
            } else {
                if ( item.PropertyDescriptor != null ) {
                    return getGridItemIdentifier( item.Parent ) + "@" + item.PropertyDescriptor.Name;
                } else {
                    return getGridItemIdentifier( item.Parent ) + "@" + item.Label;
                }
            }
        }

        private void propertyGrid_SelectedGridItemChanged( object sender, SelectedGridItemChangedEventArgs e ) {
            Editing = true;
        }

        private void propertyGrid_Enter( object sender, EventArgs e ) {
            Editing = true;
        }

        private void propertyGrid_Leave( object sender, EventArgs e ) {
            Editing = false;
        }
    }

}
