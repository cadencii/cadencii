/*
 * FormBezierPointEditUi.cs
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

//INCLUDE-SECTION IMPORT ./ui/java/FormBezierPointEdit.java

import java.awt.*;
import java.util.*;
import com.github.cadencii.*;
import com.github.cadencii.apputil.*;
import com.github.cadencii.windows.forms.*;
#else
using System;
using com.github.cadencii.apputil;
using com.github.cadencii;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.util;
using com.github.cadencii.windows.forms;

namespace com.github.cadencii
{
    using BEventArgs = System.EventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using BEventHandler = System.EventHandler;
    using BMouseEventHandler = System.Windows.Forms.MouseEventHandler;
    using boolean = System.Boolean;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
#endif

#if JAVA
    public interface FormBezierPointEditUi extends UiBase
#else
    public interface FormBezierPointEditUi : UiBase
#endif
    {
        [PureVirtualFunction]
        string getDataPointClockText();

        [PureVirtualFunction]
        string getDataPointValueText();

        [PureVirtualFunction]
        string getLeftClockText();

        [PureVirtualFunction]
        string getLeftValueText();

        [PureVirtualFunction]
        string getRightClockText();

        [PureVirtualFunction]
        string getRightValueText();

        [PureVirtualFunction]
        bool isEnableSmoothSelected();

        [PureVirtualFunction]
        void setEnableSmoothSelected( bool value );

        [PureVirtualFunction]
        void setLeftClockEnabled( bool value );

        [PureVirtualFunction]
        void setLeftValueEnabled( bool value );

        [PureVirtualFunction]
        void setLeftButtonEnabled( bool value );

        [PureVirtualFunction]
        void setRightClockEnabled( bool value );

        [PureVirtualFunction]
        void setRightValueEnabled( bool value );

        [PureVirtualFunction]
        void setRightButtonEnabled( bool value );

        [PureVirtualFunction]
        void setLeftClockText( string value );

        [PureVirtualFunction]
        void setLeftValueText( string value );

        [PureVirtualFunction]
        void setRightClockText( string value );

        [PureVirtualFunction]
        void setRightValueText( string value );

        [PureVirtualFunction]
        void setDataPointClockText( string value );

        [PureVirtualFunction]
        void setDataPointValueText( string value );

        [PureVirtualFunction]
        void setTitle( string value );

        [PureVirtualFunction]
        void setGroupDataPointTitle( string value );

        [PureVirtualFunction]
        void setLabelDataPointClockText( string value );

        [PureVirtualFunction]
        void setLabelDataPointValueText( string value );

        [PureVirtualFunction]
        void setGroupLeftTitle( string value );

        [PureVirtualFunction]
        void setLabelLeftClockText( string value );

        [PureVirtualFunction]
        void setLabelLeftValueText( string value );

        [PureVirtualFunction]
        void setGroupRightTitle( string value );

        [PureVirtualFunction]
        void setLabelRightClockText( string value );

        [PureVirtualFunction]
        void setLabelRightValueText( string value );

        [PureVirtualFunction]
        void setCheckboxEnableSmoothText( string value );

        [PureVirtualFunction]
        void setDialogResult( BDialogResult result );

        [PureVirtualFunction]
        void setOpacity( double opacity );

        [PureVirtualFunction]
        void close();
    }

#if !JAVA
}
#endif
