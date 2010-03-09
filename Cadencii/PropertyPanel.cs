/*
 * PropertyPanel.cs
 * Copyright (C) 2009-2010 kbinani
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
#if ENABLE_PROPERTY
using System;
using System.Windows.Forms;
using org.kbinani.apputil;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;

    public delegate void CommandExecuteRequiredEventHandler( CadenciiCommand command );

    public class PropertyPanel : UserControl {
        public event CommandExecuteRequiredEventHandler CommandExecuteRequired;
        private Vector<SelectedEventEntry> m_items;
        private int m_track;
        private boolean m_editing;

        public PropertyPanel() {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            m_items = new Vector<SelectedEventEntry>();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public boolean isEditing() {
            return m_editing;
        }

        public void setEditing( boolean value ) {
            m_editing = value;
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
                for ( Iterator itr = AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.iterator(); itr.hasNext(); ) {
                    ValuePair<String, boolean> v = (ValuePair<String, boolean>)itr.next();
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

        private void pushGridItemExpandStatus() {
            if ( propertyGrid.SelectedGridItem == null ) {
                return;
            }

            GridItem root = findRootGridItem( propertyGrid.SelectedGridItem );
            if ( root == null ) {
                return;
            }

            pushGridItemExpandStatusCore( root );
        }

        private void pushGridItemExpandStatusCore( GridItem item ) {
            if ( item.Expandable ) {
                String s = getGridItemIdentifier( item );
                boolean found = false;
                for ( Iterator itr = AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.iterator(); itr.hasNext(); ) {
                    ValuePair<String, boolean> v = (ValuePair<String, boolean>)itr.next();
                    if ( v.getKey().Equals( s ) ) {
                        found = true;
                        v.setValue( item.Expanded );
                    }
                }
                if ( !found ) {
                    AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.add( new ValuePair<String, boolean>( s, item.Expanded ) );
                }
            }
            foreach ( GridItem child in item.GridItems ) {
                pushGridItemExpandStatusCore( child );
            }
        }

        public void UpdateValue( int track ) {
            m_track = track;
            m_items.clear();

            // 現在のGridItemの展開状態を取得
            pushGridItemExpandStatus();

            object[] objs = new object[AppManager.getSelectedEventCount()];
            int i = -1;
            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                SelectedEventEntry item = (SelectedEventEntry)itr.next();
                i++;
                objs[i] = item;
            }

            propertyGrid.SelectedObjects = objs;
            popGridItemExpandStatus();
            setEditing( false );
        }

        private void propertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e ) {
            int len = propertyGrid.SelectedObjects.Length;
            VsqEvent[] items = new VsqEvent[len];
            for ( int i = 0; i < len; i++ ) {
                SelectedEventEntry proxy = (SelectedEventEntry)propertyGrid.SelectedObjects[i];
                items[i] = proxy.editing;
            }
            if ( CommandExecuteRequired != null ) {
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplaceRange( m_track, items ) );
                CommandExecuteRequired( run );
            }
            for ( int i = 0; i < len; i++ ) {
                AppManager.addSelectedEvent( items[i].InternalID );
            }
            propertyGrid.Refresh();
            setEditing( false );
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
            setEditing( true );
        }

        private void propertyGrid_Enter( object sender, EventArgs e ) {
            setEditing( true );
        }

        private void propertyGrid_Leave( object sender, EventArgs e ) {
            setEditing( false );
        }

        private void registerEventHandlers() {
            this.propertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler( this.propertyGrid_SelectedGridItemChanged );
            this.propertyGrid.Leave += new System.EventHandler( this.propertyGrid_Leave );
            this.propertyGrid.Enter += new System.EventHandler( this.propertyGrid_Enter );
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler( this.propertyGrid_PropertyValueChanged );
        }

        private void setResources() {
        }

#if JAVA
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
        protected override void Dispose( boolean disposing ) {
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
        private void InitializeComponent() {
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

}
#endif
