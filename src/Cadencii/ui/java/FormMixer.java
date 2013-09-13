package com.github.cadencii.ui;

//SECTION-BEGIN-IMPORT
import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import javax.swing.BorderFactory;
import javax.swing.JLabel;
import javax.swing.JMenuBar;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import com.github.cadencii.windows.forms.BDialog;
import com.github.cadencii.windows.forms.BMenu;
import com.github.cadencii.windows.forms.BMenuItem;
import com.github.cadencii.windows.forms.BPanel;

//SECTION-END-IMPORT
public class FormMixer extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private BPanel panelSlaves = null;
	private VolumeTracker volumeMaster = null;
	private JMenuBar menuMain = null;
    private BMenu menuVisual = null;
    private BMenuItem menuVisualReturn = null;
    private JScrollPane scrollSlaves = null;
    private JLabel jLabel = null;

	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public FormMixer() {
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
		this.setSize(190, 348);
		this.setJMenuBar(getMenuMain());
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
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 1;
			gridBagConstraints2.gridy = 1;
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.fill = GridBagConstraints.BOTH;
			gridBagConstraints11.gridy = 0;
			gridBagConstraints11.weightx = 1.0;
			gridBagConstraints11.weighty = 1.0;
			gridBagConstraints11.gridheight = 2;
			gridBagConstraints11.gridx = 0;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 1;
			gridBagConstraints4.weightx = 0.0D;
			gridBagConstraints4.weighty = 1.0D;
			gridBagConstraints4.fill = GridBagConstraints.BOTH;
			gridBagConstraints4.anchor = GridBagConstraints.NORTH;
			gridBagConstraints4.gridy = 0;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.weighty = 1.0D;
			gridBagConstraints.fill = GridBagConstraints.VERTICAL;
			gridBagConstraints.anchor = GridBagConstraints.NORTH;
			gridBagConstraints.gridy = 0;
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(getVolumeMaster(), gridBagConstraints4);
			jContentPane.add(getScrollSlaves(), gridBagConstraints11);
			jContentPane.add(jLabel, gridBagConstraints2);
		}
		return jContentPane;
	}

	/**
	 * This method initializes panelSlaves	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private BPanel getPanelSlaves() {
		if (panelSlaves == null) {
			panelSlaves = new BPanel();
			panelSlaves.setLayout(new GridBagLayout());
			panelSlaves.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
			panelSlaves.setBackground(new Color(180, 180, 180));
		}
		return panelSlaves;
	}

	/**
	 * This method initializes volumeMaster	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getVolumeMaster() {
		if (volumeMaster == null) {
			jLabel = new JLabel();
			jLabel.setText(" ");
			jLabel.setPreferredSize(new Dimension(4, 15));
			volumeMaster = new VolumeTracker();
		}
		return volumeMaster;
	}

	/**
     * This method initializes menuMain	
     * 	
     * @return javax.swing.JMenuBar	
     */
    private JMenuBar getMenuMain() {
        if (menuMain == null) {
            menuMain = new JMenuBar();
            menuMain.add(getMenuVisual());
        }
        return menuMain;
    }

    /**
     * This method initializes menuVisual	
     * 	
     * @return javax.swing.JMenu	
     */
    private BMenu getMenuVisual() {
        if (menuVisual == null) {
            menuVisual = new BMenu();
            menuVisual.add(getMenuVisualReturn());
        }
        return menuVisual;
    }

    /**
     * This method initializes menuVisualReturn	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuVisualReturn() {
        if (menuVisualReturn == null) {
            menuVisualReturn = new BMenuItem();
        }
        return menuVisualReturn;
    }

    /**
     * This method initializes scrollSlaves	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getScrollSlaves() {
        if (scrollSlaves == null) {
            scrollSlaves = new JScrollPane();
            scrollSlaves.setViewportBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            scrollSlaves.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
            scrollSlaves.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_NEVER);
            scrollSlaves.setBackground(new Color(180, 180, 180));
            scrollSlaves.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            scrollSlaves.setViewportView(getPanelSlaves());
        }
        return scrollSlaves;
    }

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
