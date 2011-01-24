package org.kbinani;

import java.awt.Toolkit;
import java.awt.datatransfer.Clipboard;
import java.awt.datatransfer.DataFlavor;
import java.awt.datatransfer.StringSelection;
import java.awt.datatransfer.Transferable;
import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.util.*;
import org.kbinani.xml.XmlSerializer;

public class PortUtilTest
{
    public static void main( String[] args )
    {
        String text = "foo";
        setClipboardText( text );
        System.out.println( "main; isClipboardContainsText()=" + isClipboardContainsText() );
        System.out.println( "main; getClipboardText()=" + getClipboardText() );
        clearClipboard();
        System.out.println( "main; isClipboardContainsText()=" + isClipboardContainsText() );
        System.out.println( "main; getClipboardText()=" + getClipboardText() );
    }
    
    public static void setClipboardText( String value )
    {
        Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
        clip.setContents( new StringSelection( value ), null );
    }
    
    public static void clearClipboard()
    {
        Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
        clip.setContents( null, null );
    }
    
    public static boolean isClipboardContainsText()
    {
        Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
        Transferable data = clip.getContents( null );

        if( data == null ){
            return false;
        }else if( !data.isDataFlavorSupported( DataFlavor.stringFlavor ) ){
            return false;
        }else{
            return true;
        }
    }
    
    public static String getClipboardText()
    {
        Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
        Transferable data = clip.getContents( null );

        String str = null;
        if( data == null ){
            str = null;
        }else if( !data.isDataFlavorSupported( DataFlavor.stringFlavor ) ){
            str = null;
        }else{
            try {
                str = (String)data.getTransferData( DataFlavor.stringFlavor );
            }catch( Exception e ){
                str = null;
            }
        }
        return str;
    }
}
