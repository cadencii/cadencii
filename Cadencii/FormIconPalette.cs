/*
 * FormIconPalette.cs
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

import org.kbinani.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani;
using org.kbinani.vsq;
using org.kbinani.java.awt;
using org.kbinani.java.awt.image;
using org.kbinani.java.util;
using org.kbinani.windows.forms;
using org.kbinani.javax.swing;
using org.kbinani.apputil;

namespace org.kbinani.cadencii {
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormIconPalette extends BForm {
#else
    public class FormIconPalette : BForm {
#endif
        private Vector<BButton> dynaffButtons = new Vector<BButton>();
        private Vector<BButton> crescendButtons = new Vector<BButton>();
        private Vector<BButton> decrescendButtons = new Vector<BButton>();
        private int buttonWidth = 40;

        public FormIconPalette() {
            InitializeComponent();
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            initialize();
            registerEventHandlers();
            TreeMap<String, BKeys[]> dict = AppManager.editorConfig.getShortcutKeysDictionary();
            if ( dict.containsKey( "menuVisualIconPalette" ) ) {
                BKeys[] keys = dict.get( "menuVisualIconPalette" );
                KeyStroke shortcut = PortUtil.getKeyStrokeFromBKeys( keys );
                menuWindowHide.setAccelerator( shortcut );
            }
        }

        private void applyLanguage() {
            setTitle( _( "Icon Palette" ) );
            chkTopMost.setText( _( "Top Most" ) );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public void applyShortcut( KeyStroke shortcut ) {
            menuWindowHide.setAccelerator( shortcut );
        }

        public void FormIconPalette_FormClosing( Object sender, BFormClosingEventArgs e ) {
            e.Cancel = true;
            setVisible( false );
        }

        private void registerEventHandlers() {
            formClosingEvent.add( new BFormClosingEventHandler( this, "FormIconPalette_FormClosing" ) );
            menuWindowHide.clickEvent.add( new BEventHandler( this, "menuWindowHide_Click" ) );
            chkTopMost.checkedChangedEvent.add( new BEventHandler( this, "chkTopMost_CheckedChanged" ) );
        }

        public void chkTopMost_CheckedChanged( Object sender, EventArgs e ) {
            setAlwaysOnTop( chkTopMost.isSelected() );
        }

        public void setTopMost( boolean value ) {
            chkTopMost.setSelected( value );
        }

        public void menuWindowHide_Click( Object sender, EventArgs e ) {
            close();
        }

        private void initialize() {
            for ( Iterator itr = VocaloSysUtil.dynamicsConfigIterator( SynthesizerType.VOCALOID1 ); itr.hasNext(); ) {
                IconDynamicsHandle handle = (IconDynamicsHandle)itr.next();
                String icon_id = handle.IconID;
                BButton btn = new BButton();
                btn.setName( icon_id );
                btn.setTag( handle );
                String buttonIconPath = handle.getButtonImageFullPath();

                boolean setimg = PortUtil.isFileExists( buttonIconPath );
                if ( setimg ) {
                    Image img = null;
#if JAVA
                    img = ImageIO.read( new File( buttonIconPath ) );
#else
                    img = new Image();
                    img.image = System.Drawing.Image.FromStream( new System.IO.FileStream( buttonIconPath, System.IO.FileMode.Open, System.IO.FileAccess.Read ) );
#endif
                    btn.setIcon( new ImageIcon( img ) );
                } else {
                    Image img = null;
                    String str = "";
                    String caption = handle.IDS;
                    if ( caption.Equals( "cresc_1" ) ) {
                        img = Resources.get_cresc1();
                    } else if ( caption.Equals( "cresc_2" ) ) {
                        img = Resources.get_cresc2();
                    } else if ( caption.Equals( "cresc_3" ) ) {
                        img = Resources.get_cresc3();
                    } else if ( caption.Equals( "cresc_4" ) ) {
                        img = Resources.get_cresc4();
                    } else if ( caption.Equals( "cresc_5" ) ) {
                        img = Resources.get_cresc5();
                    } else if ( caption.Equals( "dim_1" ) ) {
                        img = Resources.get_dim1();
                    } else if ( caption.Equals( "dim_2" ) ) {
                        img = Resources.get_dim2();
                    } else if ( caption.Equals( "dim_3" ) ) {
                        img = Resources.get_dim3();
                    } else if ( caption.Equals( "dim_4" ) ) {
                        img = Resources.get_dim4();
                    } else if ( caption.Equals( "dim_5" ) ) {
                        img = Resources.get_dim5();
                    } else if ( caption.Equals( "Dynaff11" ) ) {
                        str = "fff";
                    } else if ( caption.Equals( "Dynaff12" ) ) {
                        str = "ff";
                    } else if ( caption.Equals( "Dynaff13" ) ) {
                        str = "f";
                    } else if ( caption.Equals( "Dynaff21" ) ) {
                        str = "mf";
                    } else if ( caption.Equals( "Dynaff22" ) ) {
                        str = "mp";
                    } else if ( caption.Equals( "Dynaff31" ) ) {
                        str = "p";
                    } else if ( caption.Equals( "Dynaff32" ) ) {
                        str = "pp";
                    } else if ( caption.Equals( "Dynaff33" ) ) {
                        str = "ppp";
                    }
                    if ( img != null ) {
                        btn.setIcon( new ImageIcon( img ) );
                    } else {
                        btn.setText( str );
                    }
                }
                btn.mouseDownEvent.add( new BMouseEventHandler( this, "handleCommonMouseDown" ) );
                btn.setPreferredSize( new Dimension( buttonWidth, buttonWidth ) );
                int iw = 0;
                int ih = 0;
                if ( icon_id.StartsWith( "$0501" ) ) {
                    // dynaff
                    dynaffButtons.add( btn );
                    ih = 0;
                    iw = dynaffButtons.size() - 1;
                } else if ( icon_id.StartsWith( "$0502" ) ) {
                    // crescend
                    crescendButtons.add( btn );
                    ih = 1;
                    iw = crescendButtons.size() - 1;
                } else if ( icon_id.StartsWith( "$0503" ) ) {
                    // decrescend
                    decrescendButtons.add( btn );
                    ih = 2;
                    iw = decrescendButtons.size() - 1;
                } else {
                    continue;
                }
#if JAVA
#else
                btn.Location = new System.Drawing.Point( iw * buttonWidth, ih * buttonWidth );
                this.Controls.Add( btn );
                btn.BringToFront();
#endif
            }
            chkTopMost.setLocation( new Point( 0, 3 * buttonWidth ) );

            // ウィンドウのサイズを固定化する
            int height = 0;
            int width = 0;
            if ( dynaffButtons.size() > 0 ) {
                height += buttonWidth;
            }
            height += chkTopMost.getHeight();
            width = Math.Max( width, buttonWidth * dynaffButtons.size() );
            if ( crescendButtons.size() > 0 ) {
                height += buttonWidth;
            }
            width = Math.Max( width, buttonWidth * crescendButtons.size() );
            if ( decrescendButtons.size() > 0 ) {
                height += buttonWidth;
            }
            width = Math.Max( width, buttonWidth * decrescendButtons.size() );
#if !JAVA
            this.ClientSize = new System.Drawing.Size( width, height );
#endif
            Dimension size = getSize();
            setMaximumSize( size );
            setMinimumSize( size );
        }

        public void handleCommonMouseDown( Object sender, BMouseEventArgs e ) {
            if ( AppManager.getEditMode() != EditMode.NONE ) {
                return;
            }
            BButton btn = (BButton)sender;
            if ( AppManager.mainWindow != null ) {
                AppManager.mainWindow.toFront();
            }

            IconDynamicsHandle handle = (IconDynamicsHandle)btn.getTag();
            VsqEvent item = new VsqEvent();
            item.Clock = 0;
            item.ID.Note = 60;
            item.ID.type = VsqIDType.Aicon;
            item.ID.IconDynamicsHandle = (IconDynamicsHandle)handle.clone();
            int length = handle.getLength();
            if ( length <= 0 ) {
                length = 1;
            }
            item.ID.setLength( length );
            AppManager.addingEvent = item;

            btn.DoDragDrop( handle, System.Windows.Forms.DragDropEffects.All );
        }

        private void InitializeComponent() {
            this.menuBar = new org.kbinani.windows.forms.BMenuBar();
            this.menuWindow = new org.kbinani.windows.forms.BMenuItem();
            this.menuWindowHide = new org.kbinani.windows.forms.BMenuItem();
            this.chkTopMost = new org.kbinani.windows.forms.BCheckBox();
            this.menuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuWindow} );
            this.menuBar.Location = new System.Drawing.Point( 0, 0 );
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size( 458, 26 );
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "bMenuBar1";
            // 
            // menuWindow
            // 
            this.menuWindow.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuWindowHide} );
            this.menuWindow.Name = "menuWindow";
            this.menuWindow.Size = new System.Drawing.Size( 66, 22 );
            this.menuWindow.Text = "Window";
            // 
            // menuWindowHide
            // 
            this.menuWindowHide.Name = "menuWindowHide";
            this.menuWindowHide.Size = new System.Drawing.Size( 102, 22 );
            this.menuWindowHide.Text = "Hide";
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point( 0, 29 );
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size( 72, 16 );
            this.chkTopMost.TabIndex = 1;
            this.chkTopMost.Text = "Top Most";
            this.chkTopMost.UseVisualStyleBackColor = true;
            // 
            // FormIconPalette
            // 
            this.ClientSize = new System.Drawing.Size( 458, 342 );
            this.Controls.Add( this.chkTopMost );
            this.Controls.Add( this.menuBar );
            this.MainMenuStrip = this.menuBar;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormIconPalette";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Icon Palette";
            this.menuBar.ResumeLayout( false );
            this.menuBar.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        private BMenuBar menuBar;
        private BMenuItem menuWindow;
        private BMenuItem menuWindowHide;
        private BCheckBox chkTopMost;

    }

#if !JAVA
}
#endif
