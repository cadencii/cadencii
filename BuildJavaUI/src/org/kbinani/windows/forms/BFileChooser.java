package org.kbinani.windows.forms;

import java.awt.Component;
import java.io.File;
import javax.swing.JFileChooser;

public class BFileChooser{
    private JFileChooser m_dialog = null;
    
    public BFileChooser( String currentDirectoryPath ){
        m_dialog = new JFileChooser( currentDirectoryPath );
    }
    
    public void addFileFilter( String filter ){
        // TODO: [not implemented yet at BFileChooser#addFileFilter]
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
        File f = m_dialog.getSelectedFile();
        return f.getAbsolutePath();
    }
    
    public int showOpenDialog( Component parent ){
        return m_dialog.showOpenDialog( parent );
    }
    
    public int showSaveDialog( Component parent ){
        return m_dialog.showSaveDialog( parent );
    }
    
    public void setDialogTitle( String dialogTitle ){
        m_dialog.setDialogTitle( dialogTitle );
    }
}
