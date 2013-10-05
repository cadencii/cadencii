#if ENABLE_PROPERTY
/*
 * PropertyPanel.cs
 * Copyright © 2009-2011 kbinani
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
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using cadencii.apputil;
using cadencii.java.util;
using cadencii.vsq;

namespace cadencii
{

    public class PropertyPanel : UserControl
    {
        public event CommandExecuteRequiredEventHandler CommandExecuteRequired;
        private List<SelectedEventEntry> m_items;
        private int m_track;
        private bool m_editing;

        public PropertyPanel()
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            m_items = new List<SelectedEventEntry>();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }

        public bool isEditing()
        {
            return m_editing;
        }

        public void setEditing(bool value)
        {
            m_editing = value;
        }

        private void popGridItemExpandStatus()
        {
            if (propertyGrid.SelectedGridItem == null) {
                return;
            }

            GridItem root = findRootGridItem(propertyGrid.SelectedGridItem);
            if (root == null) {
                return;
            }

            popGridItemExpandStatusCore(root);
        }

        private void popGridItemExpandStatusCore(GridItem item)
        {
            if (item.Expandable) {
                string s = getGridItemIdentifier(item);
                foreach (var v in AppManager.editorConfig.PropertyWindowStatus.ExpandStatus) {
                    string key = v.getKey();
                    if (key == null) {
                        key = "";
                    }
                    if (key.Equals(s)) {
                        item.Expanded = v.getValue();
                        break;
                    }
                }
            }
            foreach (GridItem child in item.GridItems) {
                popGridItemExpandStatusCore(child);
            }
        }

        private void pushGridItemExpandStatus()
        {
            if (propertyGrid.SelectedGridItem == null) {
                return;
            }

            GridItem root = findRootGridItem(propertyGrid.SelectedGridItem);
            if (root == null) {
                return;
            }

            pushGridItemExpandStatusCore(root);
        }

        private void pushGridItemExpandStatusCore(GridItem item)
        {
            if (item.Expandable) {
                string s = getGridItemIdentifier(item);
                bool found = false;
                foreach (var v in AppManager.editorConfig.PropertyWindowStatus.ExpandStatus) {
                    string key = v.getKey();
                    if (key == null) {
                        continue;
                    }
                    if (v.getKey().Equals(s)) {
                        found = true;
                        v.setValue(item.Expanded);
                    }
                }
                if (!found) {
                    AppManager.editorConfig.PropertyWindowStatus.ExpandStatus.Add(new ValuePairOfStringBoolean(s, item.Expanded));
                }
            }
            foreach (GridItem child in item.GridItems) {
                pushGridItemExpandStatusCore(child);
            }
        }

        public void updateValue(int track)
        {
            m_track = track;
            m_items.Clear();

            // 現在のGridItemの展開状態を取得
            pushGridItemExpandStatus();

            Object[] objs = new Object[AppManager.itemSelection.getEventCount()];
            int i = -1;
            foreach (var item in AppManager.itemSelection.getEventIterator()) {
                i++;
                objs[i] = item;
            }

            propertyGrid.SelectedObjects = objs;
            popGridItemExpandStatus();
            setEditing(false);
        }

        public void propertyGrid_PropertyValueChanged(Object s, PropertyValueChangedEventArgs e)
        {
            Object[] selobj = propertyGrid.SelectedObjects;
            int len = selobj.Length;
            VsqEvent[] items = new VsqEvent[len];
            for (int i = 0; i < len; i++) {
                SelectedEventEntry proxy = (SelectedEventEntry)selobj[i];
                items[i] = proxy.editing;
            }
            CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventReplaceRange(m_track, items));
            if (CommandExecuteRequired != null) {
                CommandExecuteRequired(this, run);
            }
            for (int i = 0; i < len; i++) {
                AppManager.itemSelection.addEvent(items[i].InternalID);
            }
            propertyGrid.Refresh();
            setEditing(false);
        }

        /// <summary>
        /// itemが属しているGridItemツリーの基点にある親を探します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private GridItem findRootGridItem(GridItem item)
        {
            if (item.Parent == null) {
                return item;
            } else {
                return findRootGridItem(item.Parent);
            }
        }

        /// <summary>
        /// itemが属しているGridItemツリーの中で，itemを特定するための文字列を取得します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string getGridItemIdentifier(GridItem item)
        {
            if (item.Parent == null) {
                if (item.PropertyDescriptor != null) {
                    return item.PropertyDescriptor.Name;
                } else {
                    return item.Label;
                }
            } else {
                if (item.PropertyDescriptor != null) {
                    return getGridItemIdentifier(item.Parent) + "@" + item.PropertyDescriptor.Name;
                } else {
                    return getGridItemIdentifier(item.Parent) + "@" + item.Label;
                }
            }
        }

        private void propertyGrid_SelectedGridItemChanged(Object sender, SelectedGridItemChangedEventArgs e)
        {
            setEditing(true);
        }

        public void propertyGrid_Enter(Object sender, EventArgs e)
        {
            setEditing(true);
        }

        public void propertyGrid_Leave(Object sender, EventArgs e)
        {
            setEditing(false);
        }

        private void registerEventHandlers()
        {
            propertyGrid.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(propertyGrid_SelectedGridItemChanged);
            propertyGrid.Leave += new EventHandler(propertyGrid_Leave);
            propertyGrid.Enter += new EventHandler(propertyGrid_Enter);
            propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid_PropertyValueChanged);
        }

        private void setResources()
        {
        }

        #region UI Impl for C#
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
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
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid.Size = new System.Drawing.Size(191, 298);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // PropertyPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.propertyGrid);
            this.Name = "PropertyPanel";
            this.Size = new System.Drawing.Size(191, 298);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        #endregion
    }

}
#endif
