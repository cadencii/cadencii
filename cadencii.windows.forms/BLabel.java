package com.github.cadencii.windows.forms;

import java.awt.Component;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Insets;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;
import java.awt.font.FontRenderContext;
import java.awt.font.GlyphMetrics;
import java.awt.font.GlyphVector;
import java.awt.geom.Point2D;
import javax.swing.JLabel;

public class BLabel extends JLabel{
    private static final long serialVersionUID = -6416404129933688215L;
    private int drawCount = 0;
    private GlyphVector gvtext;
    private boolean autoEllipsis = false;

    public BLabel(){
        super();
        addComponentListener( new ComponentAdapter(){
            public void componentResized( ComponentEvent e ){
                drawCount = 0;
                repaint();
            }
        } );
    }

    public void setMnemonic( int value, Component comp )
    {
        String text = getText();
        int index = text.indexOf( value );
        if( index < 0 ){
            text += " (" + Character.toString( (char)value ) + ")";
            index = text.lastIndexOf( value );
            setText( text );
        }
        setDisplayedMnemonic( value );
        setDisplayedMnemonicIndex( index );
        setLabelFor( comp );
    }
    
    private GlyphVector getWrappedGlyphVector( String str, float wrapping, Font font, FontRenderContext frc ){
        Point2D gmPos = new Point2D.Double(0.0d, 0.0d);
        GlyphVector gv = font.createGlyphVector(frc, str);
        float lineheight = (float)gv.getLogicalBounds().getHeight();
        float xpos = 0.0f;
        float advance = 0.0f;
        int   lineCount = 0;
        GlyphMetrics gm;
        for( int i = 0; i < gv.getNumGlyphs(); i++ ){
            gm = gv.getGlyphMetrics( i );
            advance = gm.getAdvance();
            if( xpos < wrapping && wrapping <= xpos + advance ){
                lineCount++;
                xpos = 0.0f;
            }
            gmPos.setLocation( xpos, lineheight * lineCount );
            gv.setGlyphPosition( i, gmPos );
            xpos = xpos + advance;
        }
        return gv;
    }
    
    public void setAutoEllipsis( boolean value ){
        autoEllipsis = value;
    }
    
    public boolean getAutoEllipsis(){
        return autoEllipsis;
    }
    
    protected void paintComponent( Graphics g ){
        if( autoEllipsis ){
            Graphics2D g2 = (Graphics2D)g;
            if( drawCount == 0 ){
                Insets insets = getInsets();
                int wrap = getWidth() - insets.left - insets.right;
                FontRenderContext frc = g2.getFontRenderContext();
                gvtext = getWrappedGlyphVector( getText(), wrap, getFont(), frc );
                drawCount = 1;
            }
            g2.setPaint( getForeground() );
            g2.drawGlyphVector( gvtext,
                                getInsets().left,
                                getInsets().top + getFont().getSize() );
        }else{
            super.paintComponent( g );
        }
    }

}
