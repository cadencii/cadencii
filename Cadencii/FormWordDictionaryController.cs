/*
 * FormWordDictionaryController.cs
 * Copyright © 2011 kbinani
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

package com.github.cadencii;

import java.util.*;
import com.github.cadencii.ui.*;
import com.github.cadencii.apputil.*;
import com.github.cadencii.vsq.*;

#else

namespace com.github
{
    namespace cadencii
    {
#if __cplusplus
#else
            using System;
            using System.Windows.Forms;
            using com.github.cadencii.apputil;
            using com.github.cadencii.vsq;
            using com.github.cadencii;
            using com.github.cadencii.java.util;
            using com.github.cadencii.windows.forms;
            using BEventArgs = System.EventArgs;
            using boolean = System.Boolean;
            using BEventHandler = System.EventHandler;
            using BFormClosingEventHandler = System.Windows.Forms.FormClosingEventHandler;
            using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
#endif

#endif

#if JAVA
            public class FormWordDictionaryController extends ControllerBase implements FormWordDictionaryUiListener
#else
            public class FormWordDictionaryController : ControllerBase, FormWordDictionaryUiListener
#endif
            {
                private FormWordDictionaryUiImpl ui;
                private static int mColumnWidth = 256;
                private static int mWidth = 327;
                private static int mHeight = 404;

                public FormWordDictionaryController()
                {
                    ui = new FormWordDictionaryUiImpl( this );
                    applyLanguage();
                    ui.setSize( mWidth, mHeight );
#if CSHARP
                    ui.listDictionariesSetColumnWidth( mColumnWidth );
#endif
                }


                #region FormWordDictionaryUiListenerの実装

                public void buttonCancelClick()
                {
                    ui.setDialogResult( false );
                }

                public void buttonDownClick()
                {
                    int index = ui.listDictionariesGetSelectedRow();
                    if ( 0 <= index && index + 1 < ui.listDictionariesGetItemCountRow() )
                    {
                        try
                        {
                            ui.listDictionariesClear();
                            String upper_name = ui.listDictionariesGetItemAt( index );
                            boolean upper_enabled = ui.listDictionariesIsRowChecked( index );
                            String lower_name = ui.listDictionariesGetItemAt( index + 1 );
                            boolean lower_enabled = ui.listDictionariesIsRowChecked( index + 1 );

                            ui.listDictionariesSetItemAt( index + 1, upper_name );
                            ui.listDictionariesSetRowChecked( index + 1, upper_enabled );
                            ui.listDictionariesSetItemAt( index, lower_name );
                            ui.listDictionariesSetRowChecked( index, lower_enabled );

                            ui.listDictionariesSetSelectedRow( index + 1 );
                        }
                        catch ( Exception ex )
                        {
                            serr.println( "FormWordDictionary#btnDown_Click; ex=" + ex );
                        }
                    }
                }

                public void buttonUpClick()
                {
                    int index = ui.listDictionariesGetSelectedRow();
                    if ( index >= 1 )
                    {
                        try
                        {
                            ui.listDictionariesClearSelection();
                            String upper_name = ui.listDictionariesGetItemAt( index - 1 );
                            boolean upper_enabled = ui.listDictionariesIsRowChecked( index - 1 );
                            String lower_name = ui.listDictionariesGetItemAt( index );
                            boolean lower_enabled = ui.listDictionariesIsRowChecked( index );

                            ui.listDictionariesSetItemAt( index - 1, lower_name );
                            ui.listDictionariesSetRowChecked( index - 1, lower_enabled );
                            ui.listDictionariesSetItemAt( index, upper_name );
                            ui.listDictionariesSetRowChecked( index, upper_enabled );

                            ui.listDictionariesSetSelectedRow( index - 1 );
                        }
                        catch ( Exception ex )
                        {
                            serr.println( "FormWordDictionary#btnUp_Click; ex=" + ex );
                        }
                    }
                }

                public void buttonOkClick()
                {
                    ui.setDialogResult( true );
                }

                public void formLoad()
                {
                    ui.listDictionariesClear();
                    for ( int i = 0; i < SymbolTable.getCount(); i++ )
                    {
                        String name = SymbolTable.getSymbolTable( i ).getName();
                        boolean enabled = SymbolTable.getSymbolTable( i ).isEnabled();
                        ui.listDictionariesAddRow( name, enabled );
                    }
                }

                public void formClosing()
                {
#if CSHARP
                    mColumnWidth = ui.listDictionariesGetColumnWidth();
#endif
                    mWidth = ui.getWidth();
                    mHeight = ui.getHeight();
                }

                #endregion


                #region public methods

                public void close()
                {
                    ui.close();
                }

                public UiBase getUi()
                {
                    return ui;
                }

                public int getWidth()
                {
                    return ui.getWidth();
                }

                public int getHeight()
                {
                    return ui.getHeight();
                }

                public void setLocation( int x, int y )
                {
                    ui.setLocation( x, y );
                }

                public void applyLanguage()
                {
                    ui.setTitle( _( "User Dictionary Configuration" ) );
                    ui.labelAvailableDictionariesSetText( _( "Available Dictionaries" ) );
                    ui.buttonOkSetText( _( "OK" ) );
                    ui.buttonCancelSetText( _( "Cancel" ) );
                    ui.buttonUpSetText( _( "Up" ) );
                    ui.buttonDownSetText( _( "Down" ) );
                }

                public Vector<ValuePair<String, Boolean>> getResult()
                {
                    Vector<ValuePair<String, Boolean>> ret = new Vector<ValuePair<String, Boolean>>();
                    int count = ui.listDictionariesGetItemCountRow();
#if DEBUG
                    sout.println( "FormWordDictionary#getResult; count=" + count );
#endif
                    for ( int i = 0; i < count; i++ )
                    {
                        String name = ui.listDictionariesGetItemAt( i );

                        ret.add( new ValuePair<String, Boolean>(
                            ui.listDictionariesGetItemAt( i ), ui.listDictionariesIsRowChecked( i ) ) );
                    }
                    return ret;
                }

                #endregion


                #region private methods

                private static String _( String id )
                {
                    return Messaging.getMessage( id );
                }

                #endregion
            }

#if !JAVA
    }
}
#endif
