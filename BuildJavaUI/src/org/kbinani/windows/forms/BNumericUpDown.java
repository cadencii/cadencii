package org.kbinani.windows.forms;

import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import javax.swing.JButton;
import javax.swing.JPanel;
import javax.swing.JTextField;

public class BNumericUpDown extends JPanel{
    private static final long serialVersionUID = -8499996379673462967L;
    private JTextField txtValue = null;
	private JButton btnUp = null;
	private JButton btnDown = null;
	private int value = 0;
	private int maximum = 100;
	private int minimum = 0;

	private void update(){
	    getTxtValue().setText( value + "" );
	}
	
	public int getValue(){
	    return value;
	}
	
	public void setValue( int value ){
	    if( minimum <= value && value <= maximum ){
	        this.value = value; 
	        update();
	    }
	}
	
	public int getMaximum() {
        return maximum;
    }

    public void setMaximum(int maximum) {
        if( maximum < value ){
            value = maximum;
            update();
        }
        this.maximum = maximum;
    }

    public int getMinimum() {
        return minimum;
    }

    public void setMinimum(int minimum) {
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
			btnUp.addActionListener(new java.awt.event.ActionListener() {
			    public void actionPerformed(java.awt.event.ActionEvent e) {
			        if( value + 1 <= maximum ){
			            value++;
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
			btnDown.addActionListener(new java.awt.event.ActionListener() {
			    public void actionPerformed(java.awt.event.ActionEvent e) {
			        if( value - 1 >= minimum ){
			            value--;
			            update();
			        }
			    }
			});
		}
		return btnDown;
	}
}
