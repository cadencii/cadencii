package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import javax.swing.SwingConstants;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BNumericUpDown;

//SECTION-END-IMPORT
public class FormDeleteBar extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private BLabel lblStart = null;
	private BNumericUpDown numStart = null;
	private BLabel label3 = null;
	private BLabel lblEnd = null;
	private BNumericUpDown numEnd = null;
	private BLabel label4 = null;
	private JPanel jPanel = null;
	private BButton btnOK = null;
	private BButton btnCancel = null;
	
	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public FormDeleteBar() {
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
		this.setSize(311, 153);
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
			GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			gridBagConstraints14.gridx = 0;
			gridBagConstraints14.anchor = GridBagConstraints.EAST;
			gridBagConstraints14.gridwidth = 3;
			gridBagConstraints14.weightx = 1.0D;
			gridBagConstraints14.insets = new Insets(16, 0, 8, 0);
			gridBagConstraints14.gridy = 2;
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 2;
			gridBagConstraints13.insets = new Insets(4, 8, 0, 16);
			gridBagConstraints13.gridy = 1;
			label4 = new BLabel();
			label4.setText(":0:000");
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints12.gridy = 1;
			gridBagConstraints12.weightx = 1.0;
			gridBagConstraints12.insets = new Insets(4, 0, 0, 0);
			gridBagConstraints12.gridx = 1;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 0;
			gridBagConstraints3.anchor = GridBagConstraints.EAST;
			gridBagConstraints3.insets = new Insets(4, 16, 0, 8);
			gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints3.gridy = 1;
			lblEnd = new BLabel();
			lblEnd.setText("End");
			lblEnd.setHorizontalAlignment(SwingConstants.RIGHT);
			lblEnd.setHorizontalTextPosition(SwingConstants.RIGHT);
			lblEnd.setVerticalAlignment(SwingConstants.CENTER);
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 2;
			gridBagConstraints2.insets = new Insets(8, 8, 0, 16);
			gridBagConstraints2.gridy = 0;
			label3 = new BLabel();
			label3.setText(":0:000");
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints1.gridy = 0;
			gridBagConstraints1.weightx = 1.0;
			gridBagConstraints1.insets = new Insets(8, 0, 0, 0);
			gridBagConstraints1.gridx = 1;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.anchor = GridBagConstraints.EAST;
			gridBagConstraints.insets = new Insets(8, 16, 0, 8);
			gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints.gridy = 0;
			lblStart = new BLabel();
			lblStart.setText("Start");
			lblStart.setPreferredSize(new Dimension(30, 13));
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(lblStart, gridBagConstraints);
			jContentPane.add(getNumStart(), gridBagConstraints1);
			jContentPane.add(label3, gridBagConstraints2);
			jContentPane.add(lblEnd, gridBagConstraints3);
			jContentPane.add(getNumEnd(), gridBagConstraints12);
			jContentPane.add(label4, gridBagConstraints13);
			jContentPane.add(getJPanel(), gridBagConstraints14);
		}
		return jContentPane;
	}

	/**
	 * This method initializes numStart	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BNumericUpDown getNumStart() {
		if (numStart == null) {
			numStart = new BNumericUpDown();
		}
		return numStart;
	}

	/**
	 * This method initializes numEnd	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BNumericUpDown getNumEnd() {
		if (numEnd == null) {
			numEnd = new BNumericUpDown();
		}
		return numEnd;
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
			gridBagConstraints5.anchor = GridBagConstraints.WEST;
			gridBagConstraints5.insets = new Insets(0, 0, 0, 0);
			gridBagConstraints5.gridy = 0;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 1;
			gridBagConstraints4.anchor = GridBagConstraints.WEST;
			gridBagConstraints4.insets = new Insets(0, 0, 0, 12);
			gridBagConstraints4.gridy = 0;
			jPanel = new JPanel();
			jPanel.setLayout(new GridBagLayout());
			jPanel.add(getBtnOK(), gridBagConstraints4);
			jPanel.add(getBtnCancel(), gridBagConstraints5);
		}
		return jPanel;
	}

	/**
	 * This method initializes btnOK	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnOK() {
		if (btnOK == null) {
			btnOK = new BButton();
			btnOK.setText("OK");
			btnOK.setPreferredSize(new Dimension(100, 29));
		}
		return btnOK;
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
			btnCancel.setPreferredSize(new Dimension(100, 29));
		}
		return btnCancel;
	}

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
