import java.awt.BorderLayout;
import javax.swing.JPanel;
import javax.swing.JFrame;
import java.awt.Dimension;
import javax.swing.JTabbedPane;
import java.awt.GridBagLayout;
import javax.swing.JButton;
import java.awt.GridBagConstraints;
import javax.swing.JLabel;
import java.awt.Insets;
import java.awt.Color;
import javax.swing.JComboBox;
import javax.swing.BorderFactory;
import javax.swing.border.TitledBorder;
import java.awt.Font;
import javax.swing.JCheckBox;

public class Preference extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel tabSequence = null;  //  @jve:decl-index=0:visual-constraint="444,43"
	private JTabbedPane jTabbedPane = null;
	private JLabel jLabel = null;
	private JPanel jPanel = null;
	private JLabel lblDynamics = null;
	private JComboBox comboDynamics = null;
	private JLabel lblAmplitude = null;
	private JComboBox comboAmplitude = null;
	private JLabel lblPeriod = null;
	private JComboBox comboPeriod = null;
	private JLabel jLabel1 = null;
	private JLabel jLabel11 = null;
	private JLabel jLabel12 = null;
	private JLabel jLabel2 = null;
	private JPanel jPanel1 = null;
	private JLabel lblDynamics1 = null;
	private JComboBox comboDynamics1 = null;
	private JLabel jLabel13 = null;
	private JPanel jPanel2 = null;
	private JPanel jPanel3 = null;
	private JCheckBox jCheckBox = null;
	private JLabel jLabel3 = null;
	private JComboBox jComboBox = null;
	private JLabel jLabel4 = null;
	private JPanel jPanel4 = null;
	private JLabel jLabel5 = null;
	private JComboBox comboDynamics11 = null;
	private JLabel jLabel51 = null;
	private JComboBox comboDynamics111 = null;
	private JPanel tabAnother = null;  //  @jve:decl-index=0:visual-constraint="443,402"
	private JLabel jLabel6 = null;
	private JComboBox jComboBox1 = null;
	private JLabel jLabel7 = null;
	/**
	 * This is the default constructor
	 */
	public Preference() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(412, 419);
		this.setContentPane(getJTabbedPane());
		this.setTitle("JFrame");
	}

	/**
	 * This method initializes tabSequence
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getTabSequence() {
		if (tabSequence == null) {
			GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
			gridBagConstraints20.gridx = 0;
			gridBagConstraints20.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints20.anchor = GridBagConstraints.NORTHWEST;
			gridBagConstraints20.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints20.weighty = 1.0D;
			gridBagConstraints20.gridy = 3;
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.gridx = 0;
			gridBagConstraints31.anchor = GridBagConstraints.WEST;
			gridBagConstraints31.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints31.weightx = 1.0D;
			gridBagConstraints31.gridy = 2;
			jLabel2 = new JLabel();
			jLabel2.setText("Vibrato Setting");
			GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			gridBagConstraints21.anchor = GridBagConstraints.WEST;
			gridBagConstraints21.gridy = 1;
			gridBagConstraints21.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints21.gridx = 0;
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.anchor = GridBagConstraints.WEST;
			gridBagConstraints1.gridx = 0;
			gridBagConstraints1.gridy = 0;
			gridBagConstraints1.weightx = 1.0D;
			gridBagConstraints1.insets = new Insets(3, 12, 3, 0);
			jLabel = new JLabel();
			jLabel.setText("Resolution(VSTi)");
			tabSequence = new JPanel();
			tabSequence.setLayout(new GridBagLayout());
			tabSequence.setSize(new Dimension(454, 351));
			tabSequence.add(jLabel, gridBagConstraints1);
			tabSequence.add(getJPanel(), gridBagConstraints21);
			tabSequence.add(jLabel2, gridBagConstraints31);
			tabSequence.add(getJPanel1(), gridBagConstraints20);
		}
		return tabSequence;
	}

	/**
	 * This method initializes jTabbedPane	
	 * 	
	 * @return javax.swing.JTabbedPane	
	 */
	private JTabbedPane getJTabbedPane() {
		if (jTabbedPane == null) {
			jTabbedPane = new JTabbedPane();
			jTabbedPane.setLayout(null);
		}
		return jTabbedPane;
	}

	/**
	 * This method initializes jPanel	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel() {
		if (jPanel == null) {
			GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			gridBagConstraints10.gridx = 4;
			gridBagConstraints10.anchor = GridBagConstraints.WEST;
			gridBagConstraints10.weightx = 1.0D;
			gridBagConstraints10.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints10.gridy = 3;
			jLabel12 = new JLabel();
			jLabel12.setText("clocks");
			GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			gridBagConstraints9.gridx = 4;
			gridBagConstraints9.anchor = GridBagConstraints.WEST;
			gridBagConstraints9.weightx = 1.0D;
			gridBagConstraints9.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints9.gridy = 1;
			jLabel11 = new JLabel();
			jLabel11.setText("clocks");
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.gridx = 4;
			gridBagConstraints8.weightx = 1.0D;
			gridBagConstraints8.anchor = GridBagConstraints.WEST;
			gridBagConstraints8.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints8.gridy = 0;
			jLabel1 = new JLabel();
			jLabel1.setText("clocks");
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.fill = GridBagConstraints.NONE;
			gridBagConstraints7.gridy = 3;
			gridBagConstraints7.weightx = 0.0D;
			gridBagConstraints7.anchor = GridBagConstraints.WEST;
			gridBagConstraints7.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints7.gridx = 3;
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.gridx = 0;
			gridBagConstraints6.anchor = GridBagConstraints.WEST;
			gridBagConstraints6.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints6.gridy = 3;
			lblPeriod = new JLabel();
			lblPeriod.setText("Vibrato Rate");
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.fill = GridBagConstraints.NONE;
			gridBagConstraints5.gridy = 1;
			gridBagConstraints5.weightx = 0.0D;
			gridBagConstraints5.anchor = GridBagConstraints.WEST;
			gridBagConstraints5.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints5.gridx = 3;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 0;
			gridBagConstraints4.anchor = GridBagConstraints.WEST;
			gridBagConstraints4.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints4.gridy = 1;
			lblAmplitude = new JLabel();
			lblAmplitude.setText("Vibrato Depth");
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.fill = GridBagConstraints.NONE;
			gridBagConstraints3.gridy = 0;
			gridBagConstraints3.weightx = 0.0D;
			gridBagConstraints3.anchor = GridBagConstraints.WEST;
			gridBagConstraints3.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints3.gridx = 3;
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 0;
			gridBagConstraints2.anchor = GridBagConstraints.WEST;
			gridBagConstraints2.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints2.gridy = 0;
			lblDynamics = new JLabel();
			lblDynamics.setText("Dynamics");
			jPanel = new JPanel();
			jPanel.setLayout(new GridBagLayout());
			jPanel.add(lblDynamics, gridBagConstraints2);
			jPanel.add(getComboDynamics(), gridBagConstraints3);
			jPanel.add(lblAmplitude, gridBagConstraints4);
			jPanel.add(getComboAmplitude(), gridBagConstraints5);
			jPanel.add(lblPeriod, gridBagConstraints6);
			jPanel.add(getComboPeriod(), gridBagConstraints7);
			jPanel.add(jLabel1, gridBagConstraints8);
			jPanel.add(jLabel11, gridBagConstraints9);
			jPanel.add(jLabel12, gridBagConstraints10);
		}
		return jPanel;
	}

	/**
	 * This method initializes comboDynamics	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboDynamics() {
		if (comboDynamics == null) {
			comboDynamics = new JComboBox();
			comboDynamics.setPreferredSize(new Dimension(101, 20));
		}
		return comboDynamics;
	}

	/**
	 * This method initializes comboAmplitude	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboAmplitude() {
		if (comboAmplitude == null) {
			comboAmplitude = new JComboBox();
			comboAmplitude.setPreferredSize(new Dimension(101, 20));
		}
		return comboAmplitude;
	}

	/**
	 * This method initializes comboPeriod	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboPeriod() {
		if (comboPeriod == null) {
			comboPeriod = new JComboBox();
			comboPeriod.setPreferredSize(new Dimension(101, 20));
		}
		return comboPeriod;
	}

	/**
	 * This method initializes jPanel1	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel1() {
		if (jPanel1 == null) {
			GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			gridBagConstraints19.gridx = 0;
			gridBagConstraints19.gridwidth = 5;
			gridBagConstraints19.anchor = GridBagConstraints.WEST;
			gridBagConstraints19.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints19.insets = new Insets(3, 24, 3, 12);
			gridBagConstraints19.gridy = 1;
			GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			gridBagConstraints81.anchor = GridBagConstraints.WEST;
			gridBagConstraints81.gridx = 4;
			gridBagConstraints81.gridy = 0;
			gridBagConstraints81.weightx = 1.0D;
			gridBagConstraints81.insets = new Insets(0, 12, 0, 0);
			jLabel13 = new JLabel();
			jLabel13.setText("%");
			GridBagConstraints gridBagConstraints32 = new GridBagConstraints();
			gridBagConstraints32.anchor = GridBagConstraints.WEST;
			gridBagConstraints32.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints32.gridx = 3;
			gridBagConstraints32.gridy = 0;
			gridBagConstraints32.weightx = 0.0D;
			gridBagConstraints32.fill = GridBagConstraints.NONE;
			GridBagConstraints gridBagConstraints22 = new GridBagConstraints();
			gridBagConstraints22.anchor = GridBagConstraints.WEST;
			gridBagConstraints22.gridx = 0;
			gridBagConstraints22.gridy = 0;
			gridBagConstraints22.insets = new Insets(0, 24, 0, 0);
			lblDynamics1 = new JLabel();
			lblDynamics1.setText("Default Vibrato Length");
			jPanel1 = new JPanel();
			jPanel1.setLayout(new GridBagLayout());
			jPanel1.add(lblDynamics1, gridBagConstraints22);
			jPanel1.add(getComboDynamics1(), gridBagConstraints32);
			jPanel1.add(jLabel13, gridBagConstraints81);
			jPanel1.add(getJPanel2(), gridBagConstraints19);
		}
		return jPanel1;
	}

	/**
	 * This method initializes comboDynamics1	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboDynamics1() {
		if (comboDynamics1 == null) {
			comboDynamics1 = new JComboBox();
			comboDynamics1.setPreferredSize(new Dimension(101, 20));
		}
		return comboDynamics1;
	}

	/**
	 * This method initializes jPanel2	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel2() {
		if (jPanel2 == null) {
			GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			gridBagConstraints51.gridx = 0;
			gridBagConstraints51.anchor = GridBagConstraints.WEST;
			gridBagConstraints51.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints51.gridy = 2;
			GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			gridBagConstraints14.gridx = 0;
			gridBagConstraints14.anchor = GridBagConstraints.WEST;
			gridBagConstraints14.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints14.gridy = 1;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.anchor = GridBagConstraints.WEST;
			gridBagConstraints.weightx = 1.0D;
			gridBagConstraints.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints.gridy = 0;
			jPanel2 = new JPanel();
			jPanel2.setLayout(new GridBagLayout());
			jPanel2.setBorder(BorderFactory.createTitledBorder(null, "Auto Vibrato Settings", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			jPanel2.add(getJCheckBox(), gridBagConstraints);
			jPanel2.add(getJPanel3(), gridBagConstraints14);
			jPanel2.add(getJPanel4(), gridBagConstraints51);
		}
		return jPanel2;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel3() {
		if (jPanel3 == null) {
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 2;
			gridBagConstraints13.weightx = 1.0D;
			gridBagConstraints13.anchor = GridBagConstraints.WEST;
			gridBagConstraints13.gridy = 0;
			jLabel4 = new JLabel();
			jLabel4.setText("beat");
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.fill = GridBagConstraints.NONE;
			gridBagConstraints12.gridy = 0;
			gridBagConstraints12.weightx = 0.0D;
			gridBagConstraints12.insets = new Insets(0, 12, 0, 12);
			gridBagConstraints12.gridx = 1;
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.gridx = 0;
			gridBagConstraints11.gridy = 0;
			jLabel3 = new JLabel();
			jLabel3.setText("Minimum note length for Automatic Vibrato");
			jPanel3 = new JPanel();
			jPanel3.setLayout(new GridBagLayout());
			jPanel3.add(jLabel3, gridBagConstraints11);
			jPanel3.add(getJComboBox(), gridBagConstraints12);
			jPanel3.add(jLabel4, gridBagConstraints13);
		}
		return jPanel3;
	}

	/**
	 * This method initializes jCheckBox	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getJCheckBox() {
		if (jCheckBox == null) {
			jCheckBox = new JCheckBox();
			jCheckBox.setText("Enable Automatic Vibrato");
		}
		return jCheckBox;
	}

	/**
	 * This method initializes jComboBox	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getJComboBox() {
		if (jComboBox == null) {
			jComboBox = new JComboBox();
			jComboBox.setPreferredSize(new Dimension(66, 20));
		}
		return jComboBox;
	}

	/**
	 * This method initializes jPanel4	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel4() {
		if (jPanel4 == null) {
			GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
			gridBagConstraints18.fill = GridBagConstraints.NONE;
			gridBagConstraints18.gridy = 1;
			gridBagConstraints18.weightx = 1.0;
			gridBagConstraints18.anchor = GridBagConstraints.WEST;
			gridBagConstraints18.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints18.gridx = 1;
			GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
			gridBagConstraints17.gridx = 0;
			gridBagConstraints17.gridy = 1;
			jLabel51 = new JLabel();
			jLabel51.setText("Vibrato Type VOCALOID2");
			GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			gridBagConstraints16.fill = GridBagConstraints.NONE;
			gridBagConstraints16.gridy = 0;
			gridBagConstraints16.weightx = 1.0;
			gridBagConstraints16.anchor = GridBagConstraints.WEST;
			gridBagConstraints16.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints16.gridx = 1;
			GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			gridBagConstraints15.gridx = 0;
			gridBagConstraints15.gridy = 0;
			jLabel5 = new JLabel();
			jLabel5.setText("Vibrato Type VOCALOID1");
			jPanel4 = new JPanel();
			jPanel4.setLayout(new GridBagLayout());
			jPanel4.add(jLabel5, gridBagConstraints15);
			jPanel4.add(getComboDynamics11(), gridBagConstraints16);
			jPanel4.add(jLabel51, gridBagConstraints17);
			jPanel4.add(getComboDynamics111(), gridBagConstraints18);
		}
		return jPanel4;
	}

	/**
	 * This method initializes comboDynamics11	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboDynamics11() {
		if (comboDynamics11 == null) {
			comboDynamics11 = new JComboBox();
			comboDynamics11.setPreferredSize(new Dimension(101, 20));
		}
		return comboDynamics11;
	}

	/**
	 * This method initializes comboDynamics111	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboDynamics111() {
		if (comboDynamics111 == null) {
			comboDynamics111 = new JComboBox();
			comboDynamics111.setPreferredSize(new Dimension(101, 20));
		}
		return comboDynamics111;
	}

	/**
	 * This method initializes tabAnother	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getTabAnother() {
		if (tabAnother == null) {
			GridBagConstraints gridBagConstraints25 = new GridBagConstraints();
			gridBagConstraints25.gridx = 0;
			gridBagConstraints25.anchor = GridBagConstraints.WEST;
			gridBagConstraints25.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints25.gridy = 1;
			jLabel7 = new JLabel();
			jLabel7.setText("Pre-Send time");
			GridBagConstraints gridBagConstraints24 = new GridBagConstraints();
			gridBagConstraints24.fill = GridBagConstraints.NONE;
			gridBagConstraints24.gridy = 0;
			gridBagConstraints24.weightx = 1.0;
			gridBagConstraints24.anchor = GridBagConstraints.WEST;
			gridBagConstraints24.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints24.gridx = 1;
			GridBagConstraints gridBagConstraints23 = new GridBagConstraints();
			gridBagConstraints23.gridx = 0;
			gridBagConstraints23.anchor = GridBagConstraints.WEST;
			gridBagConstraints23.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints23.gridy = 0;
			jLabel6 = new JLabel();
			jLabel6.setText("Default Singer");
			tabAnother = new JPanel();
			tabAnother.setLayout(new GridBagLayout());
			tabAnother.setSize(new Dimension(454, 351));
			tabAnother.add(jLabel6, gridBagConstraints23);
			tabAnother.add(getJComboBox1(), gridBagConstraints24);
			tabAnother.add(jLabel7, gridBagConstraints25);
		}
		return tabAnother;
	}

	/**
	 * This method initializes jComboBox1	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getJComboBox1() {
		if (jComboBox1 == null) {
			jComboBox1 = new JComboBox();
			jComboBox1.setPreferredSize(new Dimension(222, 20));
		}
		return jComboBox1;
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
