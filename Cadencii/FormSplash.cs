/*
 * FormSplash.cs
 * Copyright (C) 2010 kbinani
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

import java.awt.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.java.awt;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormSplash extends BDialog {
#else
    public class FormSplash : BDialog {
#endif
        boolean mouseDowned = false;
        Point mouseDownedLocation = new Point( 0, 0 );

        public FormSplash() {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
        }

        #region helper methods
        private void setResources() {
#if !JAVA
            this.BackgroundImage = Resources.get_splash().image;
#endif
        }

        private void registerEventHandlers() {
#if JAVA
            //TODO: fixme: FormSplash#registerEventHandlers
#else
            mouseDownEvent.add( new BMouseEventHandler( this, "FormSplash_MouseDown" ) );
            mouseUpEvent.add( new BMouseEventHandler( this, "FormSplash_MouseUp" ) );
            mouseMoveEvent.add( new BMouseEventHandler( this, "FormSplash_MouseMove" ) );
#endif
        }
        #endregion

        #region event handlers
        public void FormSplash_MouseDown( Object sender, MouseEventArgs e ) {
            mouseDowned = true;
            mouseDownedLocation = new Point( e.X, e.Y );
        }

        public void FormSplash_MouseUp( Object sender, MouseEventArgs e ) {
            mouseDowned = false;
        }

        public void FormSplash_MouseMove( Object sender, MouseEventArgs e ) {
            if ( !mouseDowned ) return;
            Point globalMouseLocation = PortUtil.getMousePosition();
            this.setLocation( globalMouseLocation.x - mouseDownedLocation.x, globalMouseLocation.y - mouseDownedLocation.y );
        }
        #endregion

        #region UI implementation
#if JAVA
        private void initialize(){
            setSize( 500, 335 );
        }
#else
        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // FormSplash
            // 
            this.ClientSize = new System.Drawing.Size( 500, 335 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSplash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ResumeLayout( false );

        }
#endif
        #endregion

    }

#if !JAVA
}
#endif
