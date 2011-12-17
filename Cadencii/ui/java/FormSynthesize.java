package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BProgressBar;

//SECTION-END-IMPORT
public class FormSynthesize extends BDialog {
    //SECTION-BEGIN-FIELD
    
    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private BLabel lblSynthesizing = null;
    private BLabel lblProgress = null;
    private BLabel lblTime = null;
    private BProgressBar progressWhole = null;
    private BProgressBar progressOne = null;
    private BButton btnCancel = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormSynthesize() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(376, 197));
        this.setContentPane(getJPanel());
        setCancelButton( btnCancel );
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.gridwidth = 2;
            gridBagConstraints5.anchor = GridBagConstraints.NORTHEAST;
            gridBagConstraints5.weighty = 1.0D;
            gridBagConstraints5.insets = new Insets(12, 0, 12, 12);
            gridBagConstraints5.gridy = 4;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.anchor = GridBagConstraints.WEST;
            gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints4.gridwidth = 2;
            gridBagConstraints4.insets = new Insets(4, 12, 4, 12);
            gridBagConstraints4.gridy = 3;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.gridwidth = 2;
            gridBagConstraints3.anchor = GridBagConstraints.WEST;
            gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints3.insets = new Insets(4, 12, 4, 12);
            gridBagConstraints3.gridy = 2;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.gridwidth = 2;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints2.gridy = 1;
            lblTime = new BLabel();
            lblTime.setText("remaining 0s (elapsed 0s)");
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 1;
            gridBagConstraints1.anchor = GridBagConstraints.EAST;
            gridBagConstraints1.insets = new Insets(12, 0, 4, 12);
            gridBagConstraints1.gridy = 0;
            lblProgress = new BLabel();
            lblProgress.setText("1/1");
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.weightx = 1.0D;
            gridBagConstraints.insets = new Insets(12, 12, 4, 0);
            gridBagConstraints.gridy = 0;
            lblSynthesizing = new BLabel();
            lblSynthesizing.setText("now synthesizing...");
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblSynthesizing, gridBagConstraints);
            jPanel.add(lblProgress, gridBagConstraints1);
            jPanel.add(lblTime, gridBagConstraints2);
            jPanel.add(getProgressWhole(), gridBagConstraints3);
            jPanel.add(getProgressOne(), gridBagConstraints4);
            jPanel.add(getBtnCancel(), gridBagConstraints5);
        }
        return jPanel;
    }

    /**
     * This method initializes progressWhole	
     * 	
     * @return javax.swing.JProgressBar	
     */
    private BProgressBar getProgressWhole() {
        if (progressWhole == null) {
            progressWhole = new BProgressBar();
        }
        return progressWhole;
    }

    /**
     * This method initializes progressOne	
     * 	
     * @return javax.swing.JProgressBar	
     */
    private BProgressBar getProgressOne() {
        if (progressOne == null) {
            progressOne = new BProgressBar();
        }
        return progressOne;
    }

    /**
     * This method initializes btnCancel	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnCancel() {
        if (btnCancel == null) {
            btnCancel = new BButton();
            btnCancel.setText("Cancel");
            btnCancel.setPreferredSize(new Dimension(100, 29));
        }
        return btnCancel;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
