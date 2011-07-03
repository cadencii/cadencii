#if ENABLE_PROPERTY
/*
 * PropertyPanel.cs
 * Copyright © 2009-2011 kbinani
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
import javax.swing.*;
import java.awt.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.apputil;
using org.kbinani.java.util;
using org.kbinani.vsq;

namespace org.kbinani.cadencii
{
    using BEventHandler = System.EventHandler;
    using boolean = System.Boolean;
    using BPropertyValueChangedEventHandler = System.Windows.Forms.PropertyValueChangedEventHandler;
    using BPropertyValueChangedEventArgs = System.Windows.Forms.PropertyValueChangedEventArgs;
#endif

#if JAVA
    public class PropertyPanel extends BPanel
#else
    public class PropertyPanel : UserControl
#endif
    {
#if JAVA
        public final BEvent<CommandExecuteRequiredEventHandler> commandExecuteRequiredEvent = new BEvent<CommandExecuteRequiredEventHandler>();
#else
        public event CommandExecuteRequiredEventHandler CommandExecuteRequired;
#endif
        private Vector<SelectedEventEntry> m_items;
        private int m_track;
        private boolean m_editing;

        public PropertyPanel()
        {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            m_items = new Vector<SelectedEventEntry>();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public boolean isEditing()
        {
            return m_editing;
        }

        public void setEditing( boolean value )
        {
            m_editing = value;
        }

        private void popGridItemExpandStatus()
        {
#if !JAVA
            if ( propertyGrid.SelectedGridItem == null ) {
                return;
            }

            GridItem root = findRootGridItem( propertyGrid.SelectedGridItem );
            if ( root == null ) {
                return;
            }

            popGridItemExpandStatusCore( root );
#endif
        }

#if !JAVA
        private void popGridItemExpandStatusCore( GridItem item )
        {
            if ( item.Expandable ) {
                String s = getGridItemIdentifier( item );
                for ( Iterator<ValuePairOfStringBoolean> itr = AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.iterator(); itr.hasNext(); ) {
                    ValuePairOfStringBoolean v = itr.next();
                    String key = v.getKey();
                    if ( key == null ) {
                        key = "";
                    }
                    if ( key.Equals( s ) ) {
                        item.Expanded = v.getValue();
                        break;
                    }
                }
            }
            foreach ( GridItem child in item.GridItems ) {
                popGridItemExpandStatusCore( child );
            }
        }
#endif

        private void pushGridItemExpandStatus()
        {
#if !JAVA
            if ( propertyGrid.SelectedGridItem == null ) {
                return;
            }

            GridItem root = findRootGridItem( propertyGrid.SelectedGridItem );
            if ( root == null ) {
                return;
            }

            pushGridItemExpandStatusCore( root );
#endif
        }

#if !JAVA
        private void pushGridItemExpandStatusCore( GridItem item )
        {
            if ( item.Expandable ) {
                String s = getGridItemIdentifier( item );
                boolean found = false;
                for ( Iterator<ValuePairOfStringBoolean> itr = AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.iterator(); itr.hasNext(); ) {
                    ValuePairOfStringBoolean v = itr.next();
                    String key = v.getKey();
                    if ( key == null ) {
                        continue;
                    }
                    if ( v.getKey().Equals( s ) ) {
                        found = true;
                        v.setValue( item.Expanded );
                    }
                }
                if ( !found ) {
                    AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.add( new ValuePairOfStringBoolean( s, item.Expanded ) );
                }
            }
            foreach ( GridItem child in item.GridItems ) {
                pushGridItemExpandStatusCore( child );
            }
        }
#endif

        public void updateValue( int track )
        {
            m_track = track;
            m_items.clear();

            // 現在のGridItemの展開状態を取得
            pushGridItemExpandStatus();

            Object[] objs = new Object[AppManager.itemSelection.getSelectedEventCount()];
            int i = -1;
            for ( Iterator<SelectedEventEntry> itr = AppManager.itemSelection.getSelectedEventIterator(); itr.hasNext(); ) {
                SelectedEventEntry item = itr.next();
                i++;
                objs[i] = item;
            }

#if JAVA
            propertyGrid.setSelectedObjects( objs );
#else
            propertyGrid.SelectedObjects = objs;
#endif
            popGridItemExpandStatus();
            setEditing( false );
        }

        public void propertyGrid_PropertyValueChanged( Object s, BPropertyValueChangedEventArgs e )
        {
#if JAVA
            Object[] selobj = propertyGrid.getSelectedObjects();
#else
            Object[] selobj = propertyGrid.SelectedObjects;
#endif
            int len = selobj.Length;
            VsqEvent[] items = new VsqEvent[len];
            for ( int i = 0; i < len; i++ ) {
                SelectedEventEntry proxy = (SelectedEventEntry)selobj[i];
                items[i] = proxy.editing;
            }
            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplaceRange( m_track, items ) );
#if JAVA
            try{
                commandExecuteRequiredEvent.raise( this, run );
            }catch( Exception ex ){
                serr.println( PropertyPanel.class + ".propertyGridPropertyValueChanged; ex=" + ex );
            }
#else
            if ( CommandExecuteRequired != null ) {
                CommandExecuteRequired( this, run );
            }
#endif
            for ( int i = 0; i < len; i++ ) {
                AppManager.itemSelection.addSelectedEvent( items[i].InternalID );
            }
#if JAVA
            propertyGrid.repaint();//.Refresh();
#else
            propertyGrid.Refresh();
#endif
            setEditing( false );
        }

#if !JAVA
        /// <summary>
        /// itemが属しているGridItemツリーの基点にある親を探します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private GridItem findRootGridItem( GridItem item )
        {
            if ( item.Parent == null ) {
                return item;
            } else {
                return findRootGridItem( item.Parent );
            }
        }
#endif

#if !JAVA
        /// <summary>
        /// itemが属しているGridItemツリーの中で，itemを特定するための文字列を取得します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private String getGridItemIdentifier( GridItem item )
        {
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
#endif

#if !JAVA
        private void propertyGrid_SelectedGridItemChanged( Object sender, SelectedGridItemChangedEventArgs e )
        {
            setEditing( true );
        }
#endif

        public void propertyGrid_Enter( Object sender, EventArgs e )
        {
            setEditing( true );
        }

        public void propertyGrid_Leave( Object sender, EventArgs e )
        {
            setEditing( false );
        }

        private void registerEventHandlers()
        {
#if !JAVA
            propertyGrid.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler( propertyGrid_SelectedGridItemChanged );
#endif
            propertyGrid.Leave += new BEventHandler( propertyGrid_Leave );
            propertyGrid.Enter += new BEventHandler( propertyGrid_Enter );
            propertyGrid.PropertyValueChanged += new BPropertyValueChangedEventHandler( propertyGrid_PropertyValueChanged );
        }

        private void setResources()
        {
        }

#if JAVA
        private BPropertyGrid propertyGrid;

        private void initialize(){
            if( propertyGrid == null ){
                propertyGrid = new BPropertyGrid();
                VsqEvent ve = new VsqEvent();
                ve.ID = new VsqID();
                propertyGrid.setSelectedObjects( 
                    new SelectedEventEntry[]{ new SelectedEventEntry( 0, ve, ve ) } );
                propertyGrid.setSelectedObjects( new Object[]{} );
                propertyGrid.setColumnWidth( 154 );
            }
            GridBagLayout lm = new GridBagLayout();
            this.setLayout( lm );
            GridBagConstraints gc = new GridBagConstraints();
            gc.fill = GridBagConstraints.BOTH;
            gc.weightx = 1.0D;
            gc.weighty = 1.0D;
            this.add( propertyGrid, gc );
        }
#else
        #region UI Impl for C#
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing )
        {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Location = new System.Drawing.Point( 0, 0 );
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid.Size = new System.Drawing.Size( 191, 298 );
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // PropertyPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add( this.propertyGrid );
            this.Name = "PropertyPanel";
            this.Size = new System.Drawing.Size( 191, 298 );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        #endregion
#endif
    }

#if !JAVA
}
#endif
#endif
