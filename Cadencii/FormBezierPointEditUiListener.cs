/*
 * FormBezierPointEditUiListener.cs
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

#else

using System;
using com.github.cadencii;

namespace com.github.cadencii
{

#endif

    public interface FormBezierPointEditUiListener
    {
        [PureVirtualFunction]
        void buttonOkClick();

        [PureVirtualFunction]
        void buttonCancelClick();

        [PureVirtualFunction]
        void buttonBackwardClick();

        [PureVirtualFunction]
        void buttonForwardClick();

        [PureVirtualFunction]
        void checkboxEnableSmoothCheckedChanged();

        [PureVirtualFunction]
        void buttonLeftMouseDown();

        [PureVirtualFunction]
        void buttonRightMouseDown();

        [PureVirtualFunction]
        void buttonCenterMouseDown();

        [PureVirtualFunction]
        void buttonsMouseUp();

        [PureVirtualFunction]
        void buttonsMouseMove();
    }

#if !JAVA
}
#endif
