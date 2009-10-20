/*
 * PropertyPanelContainer.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Boare.Cadencii {
    public partial class PropertyPanelContainer : UserControl {
        public const int _TITLE_HEIGHT = 29;
        public event StateChangeRequiredEventHandler StateChangeRequired;

        public PropertyPanelContainer() {
            InitializeComponent();
        }

        public void Add( Control c ) {
            panelMain.Controls.Add( c );
            c.Dock = DockStyle.Fill;
        }

        private void panelTitle_MouseDoubleClick( object sender, MouseEventArgs e ) {
            if ( StateChangeRequired != null ) {
                StateChangeRequired( this, PanelState.Window );
            }
        }

        private void btnClose_Click( object sender, EventArgs e ) {
            if ( StateChangeRequired != null ) {
                StateChangeRequired( this, PanelState.Hidden );
            }
        }

        private void btnWindow_Click( object sender, EventArgs e ) {
            if ( StateChangeRequired != null ) {
                StateChangeRequired( this, PanelState.Window );
            }
        }

        private void panelMain_SizeChanged( object sender, EventArgs e ) {
            panelTitle.Left = 0;
            panelTitle.Top = 0;
            panelTitle.Height = _TITLE_HEIGHT;
            panelTitle.Width = this.Width;

            panelMain.Top = _TITLE_HEIGHT;
            panelMain.Left = 0;
            panelMain.Width = this.Width;
            panelMain.Height = this.Height - _TITLE_HEIGHT;
        }
    }
}
