package com.github.cadencii.ui;

import com.github.cadencii.windows.forms.BPanel;
import java.awt.Dimension;
import javax.swing.JProgressBar;
import javax.swing.JLabel;
import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;
import java.awt.Insets;

public class ProgressBarWithLabelUi extends BPanel {
    private static final long serialVersionUID = -8546357483741470278L;
    private JProgressBar jProgressBar = null;
    private JLabel jLabel = null;
    /**
     * This method initializes 
     * 
     */
    public ProgressBarWithLabelUi() {
    	super();
    	initialize();
    }

    public void setProgress( int value )
    {
        if( value < jProgressBar.getMinimum() ) value = jProgressBar.getMinimum();
        if( jProgressBar.getMaximum() < value ) value = jProgressBar.getMaximum();
        jProgressBar.setValue( value );
        //revalidate();
    }
    
    public int getProgress()
    {
        return jProgressBar.getValue();
    }
    
    public void setText( String value )
    {
        jLabel.setText( value );
    }
    
    public String getText()
    {
        return jLabel.getText();
    }
    
    public void setWidth( int value )
    {
        // do nothing
    }
    
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
        gridBagConstraints1.gridx = 0;
        gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
        gridBagConstraints1.weightx = 1.0D;
        gridBagConstraints1.insets = new Insets(4, 8, 8, 8);
        gridBagConstraints1.gridy = 1;
        jLabel = new JLabel();
        jLabel.setText(" ");
        GridBagConstraints gridBagConstraints = new GridBagConstraints();
        gridBagConstraints.insets = new Insets(8, 8, 4, 8);
        gridBagConstraints.gridy = 0;
        gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
        gridBagConstraints.weightx = 1.0D;
        gridBagConstraints.gridx = 0;
        this.setLayout(new GridBagLayout());
        this.setSize(new Dimension(370, 59));
        this.add(getJProgressBar(), gridBagConstraints);
        this.add(jLabel, gridBagConstraints1);
    		
    }

    /**
     * This method initializes jProgressBar	
     * 	
     * @return javax.swing.JProgressBar	
     */
    private JProgressBar getJProgressBar() {
        if (jProgressBar == null) {
            jProgressBar = new JProgressBar();
            jProgressBar.setStringPainted(false);
            jProgressBar.setValue(0);
            jProgressBar.setPreferredSize(new Dimension(146, 12));
        }
        return jProgressBar;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"
