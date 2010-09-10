/*
 * CircuitView.cs
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

import java.util.*;
import java.awt.*;
import javax.swing.*;
import org.kbinani.windows.forms.*;
#else
using org.kbinani.java.util;
using org.kbinani.java.awt;
using org.kbinani.javax.swing;
using org.kbinani.windows.forms;

//TODO: draftが格上げになったら消すこと
using org.kbinani.cadencii.draft;

namespace org.kbinani.cadencii {
#endif

    /// <summary>
    /// シンセサイザ等の接続回路を編集するためのコンポーネント
    /// </summary>
#if JAVA
    public class CircuitView extends BPictureBox {
#else
    public class CircuitView : BPictureBox {
        private CircuitConfig mConfig = null;
#endif
        public void paint( Graphics g1 ) {
            Graphics2D g = (Graphics2D)g1;

        }

        public void setConfig( CircuitConfig value ) {
            mConfig = value;
        }
    }

#if !JAVA
}
#endif
