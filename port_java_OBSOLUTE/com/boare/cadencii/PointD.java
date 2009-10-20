package com.boare.cadencii;

import java.awt.*;

public class PointD {
    public double X;
    public double Y;

    public PointD(){
        X = 0.0;
        Y = 0.0;
    }

    public PointD( double x, double y ){
        X = x;
        Y = y;
    }

    public Point toPoint() {
        return new Point( (int)X, (int)Y );
    }

    /*public System.Drawing.PointF ToPointF() {
        return new System.Drawing.PointF( (float)m_x, (float)m_y );
    }*/
}
