package org.kbinani.windows.forms;

import java.awt.Dialog;
import java.awt.FileDialog;
import java.awt.Frame;
import java.io.File;
import java.io.FilenameFilter;
import java.util.Vector;
import org.kbinani.fsys;
import org.kbinani.str;

public class BFileChooser{
    public static final int APPROVE_OPTION = 0;
    public static final int CANCEL_OPTION = 1;
    public static final int ERROR_OPTION = -1;
    private FileDialog m_dialog = null;
    private String mDirectory = "";
    private String mSelected = "";
    private String mTitle = "";
    private Vector<String> mExtensions = new Vector<String>();
    
    public BFileChooser( String currentDirectoryPath ){
        mDirectory = currentDirectoryPath;
    }
    
    public void setInitialDirectory( String path ){
        mDirectory = path;
    }
    
    public void addFileFilter( String filter ){
        int indx = filter.lastIndexOf( "." );
        if( indx >= 0 ){
            String ex = filter.substring( indx );
            mExtensions.add( ex );
        }
    }
    
    public String[] getChoosableFileFilter(){
        return new String[]{}; // TODO: [not implemented yet; BFileChooser#getChoosableFileFilter]
    }
    
    public String getFileFilter(){
        return ""; // TODO: [fake return at BFileChooser#getFileFilter]
    }
    
    public void setFileFilter( String value ){
        // TODO: [not implemented yet; BFileChooser#setFileFilter]
    }
    
    public void clearChoosableFileFilter() {
        // TODO: [not implemented yet; BFileChooser#clearChoosableFileFilter]
    }
    
    public String getSelectedFile(){
        if( m_dialog != null ){
            String file = m_dialog.getFile();
            String dir = m_dialog.getDirectory();
            if( file == null ){
                return "";
            }else{
                mSelected = fsys.combine( dir, file );
                return mSelected;
            }
        }else{
            return "";
        }
    }
    
    public void setSelectedFile( String file ){
        mSelected = file;
    }

    public int showOpenDialog( Frame parent )
    {
        return showDialogCore( parent, FileDialog.LOAD );
    }

    public int showOpenDialog( Dialog parent )
    {
        return showDialogCore( parent, FileDialog.LOAD );
    }
    
    public int showSaveDialog( Frame parent )
    {
        return showDialogCore( parent, FileDialog.SAVE );
    }
    
    public int showSaveDialog( Dialog parent )
    {
        return showDialogCore( parent, FileDialog.SAVE );
    }
    
    private int showDialogCore( Object obj, int mode )
    {
        if( obj instanceof Frame ){
            m_dialog = new FileDialog( (Frame)obj, mTitle, mode );
        }else if( obj instanceof Dialog ){
            m_dialog = new FileDialog( (Dialog)obj, mTitle, mode );
        }else{
            return BFileChooser.ERROR_OPTION;
        }
        m_dialog.setFilenameFilter( new ExtensionFilenameFilter( mExtensions ) );
        m_dialog.setDirectory( mDirectory );
        m_dialog.setFile( mSelected );
        m_dialog.setVisible( true );
        String file = m_dialog.getFile();
        if( file == null ){
            mSelected = "";
            return BFileChooser.CANCEL_OPTION;
        }else{
            return BFileChooser.APPROVE_OPTION;
        }
    }
    
    public void setDialogTitle( String dialogTitle ){
        mTitle = dialogTitle;
    }
}

class ExtensionFilenameFilter implements FilenameFilter
{
    public Vector<String> mExtensions = new Vector<String>();

    public ExtensionFilenameFilter( Vector<String> extensions )
    {
        mExtensions = extensions;
    }
    
    public boolean accept(File dir, String name) {
        int size = mExtensions.size();
        if( size == 0 ){
            return true;
        }
        for( int i = 0; i < size; i++ ){
            String e = mExtensions.get( i );
            if( str.compare( e, ".*" ) ){
                return true;
            }else if( name.endsWith( e ) ){
                return true;
            }
        }
        return false;
    }
    
}

