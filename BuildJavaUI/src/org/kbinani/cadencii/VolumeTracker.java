package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import javax.swing.SwingConstants;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BSlider;
import org.kbinani.windows.forms.BTextBox;
import javax.swing.JCheckBox;

//SECTION-END-IMPORT
public class VolumeTracker extends JPanel {
    //SECTION-BEGIN-FIELD
	private static final long serialVersionUID = 1L;
	private BTextBox txtFeder = null;
	private BSlider trackFeder = null;
	private BSlider trackPanpot = null;
	private BTextBox txtPanpot = null;
	private BLabel lblTitle = null;
    private BCheckBox chkMute = null;
    private BCheckBox chkSolo = null;
	
	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public VolumeTracker() {
		super();
		initialize();
	}

	//SECTION-BEGIN-METHOD
	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
		gridBagConstraints21.weightx = 1.0D;
		gridBagConstraints21.anchor = GridBagConstraints.WEST;
		GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
		gridBagConstraints12.weightx = 0.0D;
		gridBagConstraints12.anchor = GridBagConstraints.WEST;
		GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
		gridBagConstraints11.gridx = 0;
		gridBagConstraints11.fill = GridBagConstraints.BOTH;
		gridBagConstraints11.weightx = 1.0D;
		gridBagConstraints11.gridwidth = 2;
		gridBagConstraints11.gridy = 5;
		lblTitle = new BLabel();
		lblTitle.setText("TITLE");
		lblTitle.setPreferredSize(new Dimension(85, 23));
		lblTitle.setBackground(Color.white);
		lblTitle.setHorizontalAlignment(SwingConstants.CENTER);
		GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
		gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
		gridBagConstraints3.gridy = 4;
		gridBagConstraints3.weightx = 1.0;
		gridBagConstraints3.insets = new Insets(0, 10, 10, 10);
		gridBagConstraints3.gridwidth = 2;
		gridBagConstraints3.gridx = 0;
		GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
		gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
		gridBagConstraints2.gridy = 3;
		gridBagConstraints2.weightx = 1.0;
		gridBagConstraints2.weighty = 0.0D;
		gridBagConstraints2.insets = new Insets(3, 3, 3, 3);
		gridBagConstraints2.gridwidth = 2;
		gridBagConstraints2.gridx = 0;
		GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
		gridBagConstraints1.fill = GridBagConstraints.VERTICAL;
		gridBagConstraints1.gridy = 2;
		gridBagConstraints1.weightx = 1.0;
		gridBagConstraints1.weighty = 1.0D;
		gridBagConstraints1.insets = new Insets(10, 0, 10, 0);
		gridBagConstraints1.gridwidth = 2;
		gridBagConstraints1.gridx = 0;
		GridBagConstraints gridBagConstraints = new GridBagConstraints();
		gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
		gridBagConstraints.gridy = 1;
		gridBagConstraints.weightx = 1.0;
		gridBagConstraints.weighty = 0.0D;
		gridBagConstraints.insets = new Insets(10, 3, 0, 3);
		gridBagConstraints.gridwidth = 2;
		gridBagConstraints.gridx = 0;
		this.setSize(86, 284);
		this.setLayout(new GridBagLayout());
		this.setPreferredSize(new Dimension(85, 284));
		this.setBackground(new Color(180, 180, 180));
		this.add(getChkMute(), gridBagConstraints12);
		this.add(getChkSolo(), gridBagConstraints21);
		this.add(getTxtFeder(), gridBagConstraints);
		this.add(getTrackFeder(), gridBagConstraints1);
		this.add(getTrackPanpot(), gridBagConstraints2);
		this.add(getTxtPanpot(), gridBagConstraints3);
		this.add(lblTitle, gridBagConstraints11);
	}

	/**
	 * This method initializes txtFeder	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private BTextBox getTxtFeder() {
		if (txtFeder == null) {
			txtFeder = new BTextBox();
			txtFeder.setPreferredSize(new Dimension(79, 19));
			txtFeder.setHorizontalAlignment(BTextBox.CENTER);
			txtFeder.setText("0");
		}
		return txtFeder;
	}

	/**
	 * This method initializes trackFeder	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private BSlider getTrackFeder() {
		if (trackFeder == null) {
			trackFeder = new BSlider();
			trackFeder.setOrientation(BSlider.VERTICAL);
			trackFeder.setMinimum(26);
			trackFeder.setPreferredSize(new Dimension(45, 144));
			trackFeder.setValue(100);
			trackFeder.setBackground(new Color(180, 180, 180));
			trackFeder.setMajorTickSpacing(10);
			trackFeder.setMaximum(151);
		}
		return trackFeder;
	}

	/**
	 * This method initializes trackPanpot	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private BSlider getTrackPanpot() {
		if (trackPanpot == null) {
			trackPanpot = new BSlider();
			trackPanpot.setMaximum(64);
			trackPanpot.setValue(0);
			trackPanpot.setBackground(new Color(180, 180, 180));
			trackPanpot.setMajorTickSpacing(1);
			trackPanpot.setMinimum(-64);
		}
		return trackPanpot;
	}

	/**
	 * This method initializes txtPanpot	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private BTextBox getTxtPanpot() {
		if (txtPanpot == null) {
			txtPanpot = new BTextBox();
			txtPanpot.setHorizontalAlignment(BTextBox.CENTER);
			txtPanpot.setText("0");
		}
		return txtPanpot;
	}

    /**
     * This method initializes chkMute	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getChkMute() {
        if (chkMute == null) {
            chkMute = new BCheckBox();
            chkMute.setText("M");
        }
        return chkMute;
    }

    /**
     * This method initializes chkSolo	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getChkSolo() {
        if (chkSolo == null) {
            chkSolo = new BCheckBox();
            chkSolo.setText("S");
        }
        return chkSolo;
    }

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
