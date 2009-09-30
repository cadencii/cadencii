/*
 * BPictureBox.cs
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
using System.Windows.Forms;

namespace Boare.Cadencii {

    /// <summary>
    /// KeyDownとKeyUpを受信できるPictureBox
    /// </summary>
    public class BPictureBox : PictureBox {
        public event KeyEventHandler BKeyDown;
        public event KeyEventHandler BKeyUp;
        
        protected override void OnKeyDown( KeyEventArgs e ) {
            if ( BKeyDown != null ) {
                BKeyDown( this, e );
            }
        }

        protected override void OnKeyUp( KeyEventArgs e ) {
            if ( BKeyUp != null ) {
                BKeyUp( this, e );
            }
        }

        protected override void OnMouseDown( MouseEventArgs e ) {
#if DEBUG
            //AppManager.DebugWriteLine( "BPictureBox+OnMouseDown" );
#endif
            base.OnMouseDown( e );
            this.Focus();
        }
    }

}