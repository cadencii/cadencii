package org.kbinani.windows.forms;

import java.awt.event.MouseEvent;
import java.awt.event.MouseWheelEvent;
import org.kbinani.BEventArgs;

public class BMouseEventArgs extends BEventArgs{
    public BMouseButtons Button;
    public int Clicks;
    public int X;
    public int Y;
    public int Delta;
    
    public BMouseEventArgs( BMouseButtons button, int clicks, int x, int y, int delta ){
        Button = button;
        Clicks = clicks;
        X = x;
        Y = y;
        Delta = delta;
    }
    
    public static BMouseEventArgs fromMouseEvent( MouseEvent e ){
        BMouseButtons btn = BMouseButtons.Left;
        switch( e.getButton() ){
            case MouseEvent.BUTTON1:
                btn = BMouseButtons.Left;
                break;
            case MouseEvent.BUTTON2:
                btn = BMouseButtons.Middle;
                break;
            case MouseEvent.BUTTON3:
                btn = BMouseButtons.Right;
                break;
        }
        return new BMouseEventArgs( btn, e.getClickCount(), e.getX(), e.getY(), 0 );
    }
    
    public static BMouseEventArgs fromMouseWheelEvent( MouseWheelEvent e ){
        BMouseEventArgs ret = fromMouseEvent( e );
        ret.Delta = e.getWheelRotation();
        return ret;
    }
}
