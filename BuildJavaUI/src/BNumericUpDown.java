import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BTextBox;

public class BNumericUpDown extends JPanel {
	private static final long serialVersionUID = 1L;
	private BTextBox txtValue = null;
	private BButton btnUp = null;
	private BButton btnDown = null;

	/**
	 * This is the default constructor
	 */
	public BNumericUpDown() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
		gridBagConstraints2.gridx = 1;
		gridBagConstraints2.weighty = 1.0D;
		gridBagConstraints2.anchor = GridBagConstraints.NORTH;
		gridBagConstraints2.gridy = 1;
		GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
		gridBagConstraints1.gridx = 1;
		gridBagConstraints1.weighty = 1.0D;
		gridBagConstraints1.anchor = GridBagConstraints.SOUTH;
		gridBagConstraints1.gridy = 0;
		GridBagConstraints gridBagConstraints = new GridBagConstraints();
		gridBagConstraints.fill = GridBagConstraints.BOTH;
		gridBagConstraints.gridy = 0;
		gridBagConstraints.weightx = 1.0;
		gridBagConstraints.gridheight = 2;
		gridBagConstraints.weighty = 1.0D;
		gridBagConstraints.gridx = 0;
		this.setSize(127, 23);
		this.setLayout(new GridBagLayout());
		this.add(getTxtValue(), gridBagConstraints);
		this.add(getBtnUp(), gridBagConstraints1);
		this.add(getBtnDown(), gridBagConstraints2);
	}

	/**
	 * This method initializes txtValue	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private BTextBox getTxtValue() {
		if (txtValue == null) {
			txtValue = new BTextBox();
			txtValue.setText("0");
			txtValue.setHorizontalAlignment(BTextBox.RIGHT);
		}
		return txtValue;
	}

	/**
	 * This method initializes btnUp	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private BButton getBtnUp() {
		if (btnUp == null) {
			btnUp = new BButton();
			btnUp.setText("");
		}
		return btnUp;
	}

	/**
	 * This method initializes btnDown	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private BButton getBtnDown() {
		if (btnDown == null) {
			btnDown = new BButton();
			btnDown.setText("");
		}
		return btnDown;
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
