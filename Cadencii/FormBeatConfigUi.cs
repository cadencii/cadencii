/*
 * FormBeatConfig.cs
 * Copyright © 2008-2011 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormBeatConfig.java

import java.awt.event.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.java.awt.event_;
using org.kbinani.apputil;
using org.kbinani;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
    using BEventHandler = System.EventHandler;
#endif

#if JAVA
    public interface FormBeatConfigUi extends UiBase
#else
    public interface FormBeatConfigUi : UiBase
#endif
    {
        void setFont( string fontName, float fontSize );

        void setTitle( string value );

        void setDialogResult( bool value );

        void setLocation( int x, int y );

        int getWidth();

        int getHeight();

        void close();


        void setTextBar1Label( string value );

        void setTextBar2Label( string value );

        void setTextStartLabel( string value );

        void setTextOkButton( string value );

        void setTextCancelButton( string value );

        void setTextBeatGroup( string value );

        void setTextPositionGroup( string value );


        void setEnabledStartNum( bool value );

        void setMinimumStartNum( int value );

        void setMaximumStartNum( int value );

        float getMaximumStartNum();

        float getMinimumStartNum();

        void setValueStartNum( float value );

        float getValueStartNum();



        void setEnabledEndNum( bool value );

        void setMinimumEndNum( int value );

        void setMaximumEndNum( int value );

        float getMaximumEndNum();

        float getMinimumEndNum();

        void setValueEndNum( float value );

        float getValueEndNum();


        bool isCheckedEndCheckbox();

        void setEnabledEndCheckbox( bool value );

        bool isEnabledEndCheckbox();

        void setTextEndCheckbox( string value );


        void removeAllItemsDenominatorCombobox();

        void addItemDenominatorCombobox( string value );

        void setSelectedIndexDenominatorCombobox( int value );

        int getSelectedIndexDenominatorCombobox();


        float getMaximumNumeratorNum();

        float getMinimumNumeratorNum();

        void setValueNumeratorNum( float value );

        float getValueNumeratorNum();
    }

#if !JAVA
}
#endif

