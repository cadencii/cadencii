import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;

public class FormVibratoConfig extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private JPanel jPanel2 = null;
	private JButton btnOK = null;
	private JButton btnCancel = null;
	private JLabel lblVibratoLength = null;
	private JTextField txtVibratoLength = null;
	private JLabel jLabel1 = null;
	private JLabel lblVibratoType = null;
	private JComboBox comboVibratoType = null;

	/**
	 * This is the default constructor
	 */
	public FormVibratoConfig() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(339, 157);
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
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.gridx = 0;
			gridBagConstraints5.gridwidth = 3;
			gridBagConstraints5.weighty = 1.0D;
			gridBagConstraints5.anchor = GridBagConstraints.EAST;
			gridBagConstraints5.gridy = 2;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.fill = GridBagConstraints.NONE;
			gridBagConstraints4.gridy = 1;
			gridBagConstraints4.weightx = 0.0D;
			gridBagConstraints4.anchor = GridBagConstraints.WEST;
			gridBagConstraints4.gridwidth = 2;
			gridBagConstraints4.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints4.gridx = 1;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 0;
			gridBagConstraints3.anchor = GridBagConstraints.SOUTHWEST;
			gridBagConstraints3.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints3.gridy = 1;
			lblVibratoType = new JLabel();
			lblVibratoType.setText("Vibrato Type");
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 2;
			gridBagConstraints2.insets = new Insets(12, 3, 0, 0);
			gridBagConstraints2.anchor = GridBagConstraints.WEST;
			gridBagConstraints2.weightx = 1.0D;
			gridBagConstraints2.gridy = 0;
			jLabel1 = new JLabel();
			jLabel1.setText("% (0-100)");
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.fill = GridBagConstraints.NONE;
			gridBagConstraints1.gridy = 0;
			gridBagConstraints1.weightx = 0.0D;
			gridBagConstraints1.anchor = GridBagConstraints.WEST;
			gridBagConstraints1.insets = new Insets(12, 12, 3, 0);
			gridBagConstraints1.gridx = 1;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.anchor = GridBagConstraints.WEST;
			gridBagConstraints.insets = new Insets(12, 12, 3, 0);
			gridBagConstraints.gridy = 0;
			lblVibratoLength = new JLabel();
			lblVibratoLength.setText("Vibrato Length");
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(lblVibratoLength, gridBagConstraints);
			jContentPane.add(getTxtVibratoLength(), gridBagConstraints1);
			jContentPane.add(jLabel1, gridBagConstraints2);
			jContentPane.add(lblVibratoType, gridBagConstraints3);
			jContentPane.add(getComboVibratoType(), gridBagConstraints4);
			jContentPane.add(getJPanel2(), gridBagConstraints5);
		}
		return jContentPane;
	}

	/**
	 * This method initializes jPanel2	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel2() {
		if (jPanel2 == null) {
			GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			gridBagConstraints52.anchor = GridBagConstraints.SOUTHWEST;
			gridBagConstraints52.gridx = 1;
			gridBagConstraints52.gridy = 0;
			gridBagConstraints52.insets = new Insets(0, 0, 0, 16);
			GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			gridBagConstraints42.anchor = GridBagConstraints.WEST;
			gridBagConstraints42.gridx = 0;
			gridBagConstraints42.gridy = 0;
			gridBagConstraints42.insets = new Insets(0, 0, 0, 16);
			jPanel2 = new JPanel();
			jPanel2.setLayout(new GridBagLayout());
			jPanel2.add(getBtnOK(), gridBagConstraints42);
			jPanel2.add(getBtnCancel(), gridBagConstraints52);
		}
		return jPanel2;
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

	/**
	 * This method initializes txtVibratoLength	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtVibratoLength() {
		if (txtVibratoLength == null) {
			txtVibratoLength = new JTextField();
			txtVibratoLength.setPreferredSize(new Dimension(61, 19));
		}
		return txtVibratoLength;
	}

	/**
	 * This method initializes comboVibratoType	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboVibratoType() {
		if (comboVibratoType == null) {
			comboVibratoType = new JComboBox();
			comboVibratoType.setPreferredSize(new Dimension(167, 20));
		}
		return comboVibratoType;
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
