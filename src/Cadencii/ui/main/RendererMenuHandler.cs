/*
 * RendererMenuHandler.cs
 * Copyright © 2013 kbinani
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace cadencii
{
    /// <summary>
    /// 合成器のメニュー項目へのアクションに応答するハンドラ
    /// </summary>
    class RendererMenuHandler
    {
        public RendererMenuHandler( RendererKind kind,
                                       ToolStripMenuItem track_menu,
                                       ToolStripMenuItem context_menu,
                                       ToolStripMenuItem vsti_ui_menu )
        {
            kind_ = kind;
            track_menu_ = track_menu;
            context_menu_ = context_menu;
            vsti_ui_menu_ = vsti_ui_menu;
        }

        /// <summary>
        /// 現在選択されている合成器を設定する。これにより、メニューのチェック状態が更新される
        /// </summary>
        /// <param name="kind">現在選択されている合成器の種類</param>
        public void updateCheckedState( RendererKind kind )
        {
            bool match = kind == kind_;
            if ( track_menu_ != null ) { track_menu_.Checked = match; }
            if ( context_menu_ != null ) { context_menu_.Checked = match; }
        }

        /// <summary>
        /// 合成器が使用可能かどうかを元に、メニューのアイコンを更新する
        /// </summary>
        /// <param name="config">エディタの設定情報</param>
        public void updateRendererAvailability( EditorConfig config )
        {
            string wine_prefix = config.WinePrefix;
            string wine_top = config.WineTop;
            Image icon = null;
            if ( !VSTiDllManager.isRendererAvailable( kind_, wine_prefix, wine_top ) ) {
                icon = Properties.Resources.slash;
            }
            if ( track_menu_ != null ) { track_menu_.Image = icon; }
            if ( context_menu_ != null ) { context_menu_.Image = icon; }
        }

        public bool isMatch( object menu ) { return (menu != null) && (menu == track_menu_ || menu == context_menu_); }

        public RendererKind RendererKind { get { return kind_; } }

        protected ToolStripMenuItem track_menu_;
        protected ToolStripMenuItem context_menu_;
        protected ToolStripMenuItem vsti_ui_menu_;
        protected readonly RendererKind kind_;
    }
}
