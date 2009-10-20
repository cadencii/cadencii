import javax.swing.JPanel;
import javax.swing.JFrame;
import java.awt.GridBagLayout;
import javax.swing.JLabel;
import java.awt.GridBagConstraints;
import javax.swing.JTextField;
import javax.swing.JButton;
import java.awt.Insets;

public class FormDeleteBar extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private JLabel lblStart = null;
	private JTextField numStart = null;
	private JLabel label3 = null;
	private JLabel lblEnd = null;
	private JTextField numEnd = null;
	private JLabel label4 = null;
	private JPanel jPanel = null;
	private JButton btnOK = null;
	private JButton btnCancel = null;
	/**
	 * This is the default constructor
	 */
	public FormDeleteBar() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(210, 149);
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
			label4 = new JLabel();
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
			gridBagConstraints3.gridy = 1;
			lblEnd = new JLabel();
			lblEnd.setText("End");
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 2;
			gridBagConstraints2.insets = new Insets(8, 8, 0, 16);
			gridBagConstraints2.gridy = 0;
			label3 = new JLabel();
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
			gridBagConstraints.gridy = 0;
			lblStart = new JLabel();
			lblStart.setText("Start");
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
	 * @return javax.swing.JTextField	
	 */
	private JTextField getNumStart() {
		if (numStart == null) {
			numStart = new JTextField();
		}
		return numStart;
	}

	/**
	 * This method initializes numEnd	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getNumEnd() {
		if (numEnd == null) {
			numEnd = new JTextField();
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
			gridBagConstraints5.gridx = 1;
			gridBagConstraints5.anchor = GridBagConstraints.WEST;
			gridBagConstraints5.insets = new Insets(0, 0, 0, 16);
			gridBagConstraints5.gridy = 0;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 0;
			gridBagConstraints4.anchor = GridBagConstraints.WEST;
			gridBagConstraints4.insets = new Insets(0, 0, 0, 16);
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
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnOK() {
		if (btnOK == null) {
			btnOK = new JButton();
			btnOK.setText("OK");
		}
		return btnOK;
	}

	/**
	 * This method initializes btnCancel	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnCancel() {
		if (btnCancel == null) {
			btnCancel = new JButton();
			btnCancel.setText("Cancel");
		}
		return btnCancel;
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
