/*
 * XmlRectangle.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package com.github.cadencii.xml;

import java.awt.*;
#else
using System.Xml.Serialization;
using com.github.cadencii.java.awt;

namespace com.github.cadencii.xml{
#endif

    public class XmlRectangle{
#if !JAVA
        [XmlIgnore]
#endif
        public int x;
#if !JAVA
        [XmlIgnore]
#endif
        public int y;
#if !JAVA
        [XmlIgnore]
#endif
        public int width;
#if !JAVA
        [XmlIgnore]
#endif
        public int height;

        public XmlRectangle()
        {
        }

        public XmlRectangle( int x_, int y_, int width_, int height_ ){
            x = x_;
            y = y_;
            width = width_;
            height = height_;
        }

        public XmlRectangle( Rectangle rc ) {
            x = rc.x;
            y = rc.y;
            width = rc.width;
            height = rc.height;
        }

        public Rectangle toRectangle() {
            return new Rectangle( x, y, width, height );
        }

        public int getX() {
            return x;
        }

        public void setX( int value ) {
            x = value;
        }

        public int getY() {
            return y;
        }

        public void setY( int value ) {
            y = value;
        }

        public int getWidth() {
            return width;
        }

        public void setWidth( int value ) {
            width = value;
        }

        public int getHeight() {
            return height;
        }

        public void setHeight( int value ) {
            height = value;
        }
#if !JAVA
        public int X{
            get{
                return getX();
            }
            set{
                setX( value );
            }
        }

        public int Y{
            get{
                return getY();
            }
            set{
                setY( value );
            }
        }

        public int Width{
            get{
                return getWidth();
            }
            set{
                setWidth( value );
            }
        }

        public int Height{
            get{
                return getHeight();
            }
            set{
                setHeight( value );
            }
        }
#endif

    }

#if !JAVA
}
#endif
