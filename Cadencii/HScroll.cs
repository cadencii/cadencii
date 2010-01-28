/*
 * HScroll.cs
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

import org.kbinani.windows.forms.*;
#else
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
#endif

    /// <summary>
    /// valueプロパティの値が正しくminimumからmaximumの間を動くスクロールバー
    /// </summary>
#if JAVA
    public class HScroll extends BHScrollBar{
#else
    public class HScroll : BHScrollBar {
#endif
        private int max = 100;

#if JAVA
        public int getMaximum(){
#else
        public new int getMaximum() {
#endif
            return max;
        }

#if JAVA
        public void setMaximum( int value ){
#else
        public new void setMaximum( int value ) {
#endif
            max = value;
            base.setMaximum( value + base.getVisibleAmount() );
        }

#if JAVA
        public int getVisibleAmount(){
#else
        public new int getVisibleAmount() {
#endif
            return base.getVisibleAmount();
        }

#if JAVA
        public void setVisibleAmount( int value ){
#else
        public new void setVisibleAmount( int value ) {
#endif
            base.setVisibleAmount( value );
            base.setMaximum( max + value );
        }
    }

#if !JAVA
}
#endif
