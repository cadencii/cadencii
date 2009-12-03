package org.kbinani.EditOtoIni;

//SECTION-BEGIN-IMPORT
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BProgressBar;

//SECTION-END-IMPORT
public class FormGenerateStf extends BForm {
    //SECTION-BEGIN-FIELD
	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private BLabel lblPercent = null;
	private BLabel lblTime = null;
	private BProgressBar progressBar = null;
	private BButton btnCancel = null;
	private BLabel jLabel2 = null;

	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public FormGenerateStf() {
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
		this.setSize(387, 162);
		this.setContentPane(getJContentPane());
		this.setTitle("JFrame");
	}

	/**
	 * This method initializes jContentPane
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getJContentPane() {
		if (jContentPane == null) {
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 0;
			gridBagConstraints4.weighty = 1.0D;
			gridBagConstraints4.gridy = 4;
			jLabel2 = new BLabel();
			jLabel2.setText("");
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 0;
			gridBagConstraints3.anchor = GridBagConstraints.EAST;
			gridBagConstraints3.insets = new Insets(8, 0, 0, 16);
			gridBagConstraints3.gridy = 3;
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 0;
			gridBagConstraints2.weightx = 1.0D;
			gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints2.insets = new Insets(2, 16, 0, 16);
			gridBagConstraints2.gridy = 2;
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.gridx = 0;
			gridBagConstraints1.anchor = GridBagConstraints.WEST;
			gridBagConstraints1.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints1.gridy = 1;
			lblTime = new BLabel();
			lblTime.setText("remaining 0s (elapsed 0s)");
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.insets = new Insets(16, 16, 2, 0);
			gridBagConstraints.anchor = GridBagConstraints.WEST;
			gridBagConstraints.gridy = 0;
			lblPercent = new BLabel();
			lblPercent.setText("0 %");
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(lblPercent, gridBagConstraints);
			jContentPane.add(lblTime, gridBagConstraints1);
			jContentPane.add(getProgressBar(), gridBagConstraints2);
			jContentPane.add(getBtnCancel(), gridBagConstraints3);
			jContentPane.add(jLabel2, gridBagConstraints4);
		}
		return jContentPane;
	}

	/**
	 * This method initializes progressBar	
	 * 	
	 * @return javax.swing.BProgressBar	
	 */
	private BProgressBar getProgressBar() {
		if (progressBar == null) {
			progressBar = new BProgressBar();
		}
		return progressBar;
	}

	/**
	 * This method initializes btnCancel	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnCancel() {
		if (btnCancel == null) {
			btnCancel = new BButton();
			btnCancel.setText("Cancel");
		}
		return btnCancel;
	}

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
