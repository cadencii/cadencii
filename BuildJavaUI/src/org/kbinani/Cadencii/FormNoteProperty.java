package org.kbinani.Cadencii;

//SECTION-BEGIN-IMPORT
import org.kbinani.windows.forms.BForm;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;

//SECTION-END-IMPORT
public class FormNoteProperty extends BForm {
    //SECTION-BEGIN-FIELD

    private JPanel jPanel = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormNoteProperty() {
    	super();
    	initialize();
    }

    //SECTION-BEGIN-METHOD
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(217, 330));
        this.setContentPane(getJPanel());
        this.setTitle("Note Property");
    		
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
        }
        return jPanel;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
