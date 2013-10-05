/*
 * XmlRectangle.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.xml.
 *
 * cadencii.xml is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.xml is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System.Xml.Serialization;
using cadencii.java.awt;

namespace cadencii.xml
{

    public class XmlRectangle
    {
        [XmlIgnore]
        public int x;
        [XmlIgnore]
        public int y;
        [XmlIgnore]
        public int width;
        [XmlIgnore]
        public int height;

        public XmlRectangle()
        {
        }

        public XmlRectangle(int x_, int y_, int width_, int height_)
        {
            x = x_;
            y = y_;
            width = width_;
            height = height_;
        }

        public XmlRectangle(Rectangle rc)
        {
            x = rc.x;
            y = rc.y;
            width = rc.width;
            height = rc.height;
        }

        public Rectangle toRectangle()
        {
            return new Rectangle(x, y, width, height);
        }

        public int getX()
        {
            return x;
        }

        public void setX(int value)
        {
            x = value;
        }

        public int getY()
        {
            return y;
        }

        public void setY(int value)
        {
            y = value;
        }

        public int getWidth()
        {
            return width;
        }

        public void setWidth(int value)
        {
            width = value;
        }

        public int getHeight()
        {
            return height;
        }

        public void setHeight(int value)
        {
            height = value;
        }

        public int X
        {
            get
            {
                return getX();
            }
            set
            {
                setX(value);
            }
        }

        public int Y
        {
            get
            {
                return getY();
            }
            set
            {
                setY(value);
            }
        }

        public int Width
        {
            get
            {
                return getWidth();
            }
            set
            {
                setWidth(value);
            }
        }

        public int Height
        {
            get
            {
                return getHeight();
            }
            set
            {
                setHeight(value);
            }
        }

    }

}
