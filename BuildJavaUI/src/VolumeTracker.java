import java.awt.GridBagLayout;
import javax.swing.JPanel;
import java.awt.Dimension;
import java.awt.Color;
import javax.swing.JTextField;
import java.awt.GridBagConstraints;
import javax.swing.JSlider;
import java.awt.Insets;
import javax.swing.JLabel;
import javax.swing.SwingConstants;

public class VolumeTracker extends JPanel {

	private static final long serialVersionUID = 1L;
	private JTextField txtFeder = null;
	private JSlider trackFeder = null;
	private JSlider trackPanpot = null;
	private JTextField txtPanpot = null;
	private JLabel lblTitle = null;
	/**
	 * This is the default constructor
	 */
	public VolumeTracker() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
		gridBagConstraints11.gridx = 0;
		gridBagConstraints11.fill = GridBagConstraints.BOTH;
		gridBagConstraints11.weightx = 1.0D;
		gridBagConstraints11.gridy = 4;
		lblTitle = new JLabel();
		lblTitle.setText("TITLE");
		lblTitle.setPreferredSize(new Dimension(85, 23));
		lblTitle.setBackground(Color.white);
		lblTitle.setHorizontalAlignment(SwingConstants.CENTER);
		GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
		gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
		gridBagConstraints3.gridy = 3;
		gridBagConstraints3.weightx = 1.0;
		gridBagConstraints3.insets = new Insets(0, 10, 10, 10);
		gridBagConstraints3.gridx = 0;
		GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
		gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
		gridBagConstraints2.gridy = 2;
		gridBagConstraints2.weightx = 1.0;
		gridBagConstraints2.weighty = 0.0D;
		gridBagConstraints2.insets = new Insets(3, 3, 3, 3);
		gridBagConstraints2.gridx = 0;
		GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
		gridBagConstraints1.fill = GridBagConstraints.VERTICAL;
		gridBagConstraints1.gridy = 1;
		gridBagConstraints1.weightx = 1.0;
		gridBagConstraints1.weighty = 1.0D;
		gridBagConstraints1.insets = new Insets(10, 0, 10, 0);
		gridBagConstraints1.gridx = 0;
		GridBagConstraints gridBagConstraints = new GridBagConstraints();
		gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
		gridBagConstraints.gridy = 0;
		gridBagConstraints.weightx = 1.0;
		gridBagConstraints.weighty = 0.0D;
		gridBagConstraints.insets = new Insets(10, 3, 0, 3);
		gridBagConstraints.gridx = 0;
		this.setSize(85, 261);
		this.setLayout(new GridBagLayout());
		this.setPreferredSize(new Dimension(85, 261));
		this.setBackground(new Color(180, 180, 180));
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
	private JTextField getTxtFeder() {
		if (txtFeder == null) {
			txtFeder = new JTextField();
			txtFeder.setPreferredSize(new Dimension(79, 19));
			txtFeder.setHorizontalAlignment(JTextField.CENTER);
			txtFeder.setText("0");
		}
		return txtFeder;
	}

	/**
	 * This method initializes trackFeder	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private JSlider getTrackFeder() {
		if (trackFeder == null) {
			trackFeder = new JSlider();
			trackFeder.setOrientation(JSlider.VERTICAL);
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
	private JSlider getTrackPanpot() {
		if (trackPanpot == null) {
			trackPanpot = new JSlider();
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
	private JTextField getTxtPanpot() {
		if (txtPanpot == null) {
			txtPanpot = new JTextField();
			txtPanpot.setHorizontalAlignment(JTextField.CENTER);
			txtPanpot.setText("0");
		}
		return txtPanpot;
	}

}
