package org.kbinani.windows.forms;

import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import javax.swing.JButton;
import javax.swing.JPanel;
import javax.swing.JTextField;
import java.awt.Dimension;

public class BNumericUpDown extends JPanel{
    private static final long serialVersionUID = -8499996379673462967L;
    private JTextField txtValue = null;
	private JButton btnUp = null;
	private JButton btnDown = null;
	private float value = 0;
	private float maximum = 100;
	private float minimum = 0;
	private float increment = 1;
	private int decimalPlaces = 0;
	private float minimumStep = 1;

	public int getDecimalPlaces(){
	    return decimalPlaces;
	}
	
	public void setDecimalPlaces( int value ){
	    decimalPlaces = value;
	    minimumStep = (float)Math.pow( 0.1, value );
	    if( increment < minimumStep ){
	        increment = minimumStep;
	    }
	}
	
	public float getIncrement(){
	    return increment;
	}
	
	public void setIncrement( float value ){
	    increment = value;
	}
	
	private void update(){
	    getTxtValue().setText( value + "" );
	}
	
	public float getValue(){
	    return value;
	}
	
	public void setValue( float value ){
	    if( minimum <= value && value <= maximum ){
	        this.value = value; 
	        update();
	    }
	}
	
	public float getMaximum() {
        return maximum;
    }

    public void setMaximum( float maximum ){
        if( maximum < value ){
            value = maximum;
            update();
        }
        this.maximum = maximum;
    }

    public float getMinimum() {
        return minimum;
    }

    public void setMinimum( float minimum ){
        if( value < minimum ){
            value = minimum;
            update();
        }
        this.minimum = minimum;
    }

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
	private JTextField getTxtValue() {
		if (txtValue == null) {
			txtValue = new JTextField();
			txtValue.setText("0");
			txtValue.setHorizontalAlignment(JTextField.RIGHT);
		}
		return txtValue;
	}

	/**
	 * This method initializes btnUp	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnUp() {
		if (btnUp == null) {
			btnUp = new JButton();
			btnUp.setText("");
			btnUp.setPreferredSize(new Dimension(20, 10));
			btnUp.addActionListener(new java.awt.event.ActionListener() {
			    public void actionPerformed(java.awt.event.ActionEvent e) {
			        if( value + increment <= maximum ){
			            value += increment;
			            update();
			        }
			    }
			});
		}
		return btnUp;
	}

	/**
	 * This method initializes btnDown	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnDown() {
		if (btnDown == null) {
			btnDown = new JButton();
			btnDown.setText("");
			btnDown.setPreferredSize(new Dimension(20, 10));
			btnDown.addActionListener(new java.awt.event.ActionListener() {
			    public void actionPerformed(java.awt.event.ActionEvent e) {
			        if( value - increment >= minimum ){
			            value -= increment;
			            update();
			        }
			    }
			});
		}
		return btnDown;
	}
}
