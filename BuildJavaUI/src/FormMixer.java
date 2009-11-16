import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BHScrollBar;
import org.kbinani.windows.forms.BPanel;

public class FormMixer extends BForm {

	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private BPanel panel1 = null;
	private BHScrollBar hScroll = null;
	private VolumeTracker volumeMaster = null;
	private BCheckBox chkTopmost = null;

	/**
	 * This is the default constructor
	 */
	public FormMixer() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(377, 653);
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
			gridBagConstraints5.gridx = 1;
			gridBagConstraints5.weightx = 0.0D;
			gridBagConstraints5.anchor = GridBagConstraints.SOUTH;
			gridBagConstraints5.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints5.gridy = 1;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 1;
			gridBagConstraints4.weightx = 0.0D;
			gridBagConstraints4.weighty = 1.0D;
			gridBagConstraints4.fill = GridBagConstraints.BOTH;
			gridBagConstraints4.anchor = GridBagConstraints.NORTH;
			gridBagConstraints4.gridy = 0;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 0;
			gridBagConstraints3.fill = GridBagConstraints.BOTH;
			gridBagConstraints3.weightx = 1.0D;
			gridBagConstraints3.weighty = 1.0D;
			gridBagConstraints3.gridy = 0;
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints1.gridy = 1;
			gridBagConstraints1.weighty = 0.0D;
			gridBagConstraints1.anchor = GridBagConstraints.SOUTH;
			gridBagConstraints1.weightx = 1.0D;
			gridBagConstraints1.gridx = 0;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.weighty = 1.0D;
			gridBagConstraints.fill = GridBagConstraints.VERTICAL;
			gridBagConstraints.anchor = GridBagConstraints.NORTH;
			gridBagConstraints.gridy = 0;
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(getHScroll(), gridBagConstraints1);
			jContentPane.add(getPanel1(), gridBagConstraints3);
			jContentPane.add(getVolumeMaster(), gridBagConstraints4);
			jContentPane.add(getChkTopmost(), gridBagConstraints5);
		}
		return jContentPane;
	}

	/**
	 * This method initializes panel1	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private BPanel getPanel1() {
		if (panel1 == null) {
			panel1 = new BPanel();
			panel1.setLayout(new GridBagLayout());
		}
		return panel1;
	}

	/**
	 * This method initializes hScroll	
	 * 	
	 * @return javax.swing.JScrollBar	
	 */
	private BHScrollBar getHScroll() {
		if (hScroll == null) {
			hScroll = new BHScrollBar();
		}
		return hScroll;
	}

	/**
	 * This method initializes volumeMaster	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getVolumeMaster() {
		if (volumeMaster == null) {
			volumeMaster = new VolumeTracker();
		}
		return volumeMaster;
	}

	/**
	 * This method initializes chkTopmost	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private BCheckBox getChkTopmost() {
		if (chkTopmost == null) {
			chkTopmost = new BCheckBox();
			chkTopmost.setText("Top most");
			chkTopmost.setText("Top most");
		}
		return chkTopmost;
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
