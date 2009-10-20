import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JPanel;
import javax.swing.JScrollBar;
import javax.swing.JScrollPane;
import javax.swing.JSeparator;
import javax.swing.JSplitPane;
import javax.swing.JTable;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.swing.table.DefaultTableModel;
import java.awt.event.KeyEvent;

public class FormUtauVoiceConfig extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel splitContainerOut = null;
	private JMenuBar jJMenuBar = null;
	private JMenu menuFile = null;
	private JMenuItem menuFileOpen = null;
	private JMenuItem menuFileSave = null;
	private JMenuItem menuFileSaveAs = null;
	private JSeparator jMenuItem3 = null;
	private JMenuItem menuFileQuit = null;
	private JSplitPane jSplitPane = null;
	private JSplitPane splitContainerIn = null;
	private JPanel panelLeft = null;
	private JScrollPane jScrollPane = null;
	private JTable jTable = null;
	private JPanel panelLeftBottom = null;
	private JButton buttonNext = null;
	private JLabel lblSearch = null;
	private JTextField txtSearch = null;
	private JButton buttonPrevious = null;
	private JLabel jLabel1 = null;
	private JPanel panelRight = null;
	private JLabel lblFileName = null;
	private JTextField txtFileName = null;
	private JLabel lblAlias = null;
	private JTextField txtAlias = null;
	private JLabel lblOffset = null;
	private JLabel lblConsonant = null;
	private JLabel lblBlank = null;
	private JLabel lblPreUtterance = null;
	private JLabel lblOverlap = null;
	private JButton btnRefreshFrq = null;
	private JButton btnRefreshStf = null;
	private JLabel jLabel3 = null;
	private JLabel jLabel4 = null;
	private JPanel panelBottom = null;
	private JPanel pictWave = null;
	private JPanel jPanel5 = null;
	private JScrollBar hScroll = null;
	private JButton btnMinus = null;
	private JButton btnPlus = null;
	private JMenu menuEdit = null;
	private JMenuItem menuEditGenerateFrq = null;
	private JMenuItem menuEditGenerateStf = null;
	private JMenu menuView = null;
	private JMenuItem menuViewSearchNext = null;
	private JMenuItem menuViewSearchPrevious = null;
	private DefaultTableModel listFiles = null;
	private String[] columnHeaders = new String[]{ "FIle Name", "Alias", "offste", "Consonant", "Blank", "pre Utrerance", "Overlap" };
	private JPanel jPanel = null;
	private JTextField txtOffset = null;
	private JLabel jLabel2 = null;
	private JLabel jLabel5 = null;
	private JPanel jPanel1 = null;
	private JTextField txtConsonant = null;
	private JLabel jLabel21 = null;
	private JLabel jLabel51 = null;
	private JPanel jPanel2 = null;
	private JTextField txtBlank = null;
	private JLabel jLabel22 = null;
	private JLabel jLabel52 = null;
	private JPanel jPanel3 = null;
	private JTextField txtPreUtterance = null;
	private JLabel jLabel23 = null;
	private JLabel jLabel53 = null;
	private JPanel jPanel4 = null;
	private JTextField txtOverlap = null;
	private JLabel jLabel24 = null;
	private JLabel jLabel54 = null;
	/**
	 * This is the default constructor
	 */
	public FormUtauVoiceConfig() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(712, 507);
		this.setJMenuBar(getJJMenuBar());
		this.setContentPane(getSplitContainerOut());
		this.setTitle("JFrame");
	}

	/**
	 * This method initializes splitContainerOut	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getSplitContainerOut() {
		if (splitContainerOut == null) {
			splitContainerOut = new JPanel();
			splitContainerOut.setLayout(new BorderLayout());
			splitContainerOut.add(getJSplitPane(), BorderLayout.CENTER);
		}
		return splitContainerOut;
	}

	/**
	 * This method initializes jJMenuBar	
	 * 	
	 * @return javax.swing.JMenuBar	
	 */
	private JMenuBar getJJMenuBar() {
		if (jJMenuBar == null) {
			jJMenuBar = new JMenuBar();
			jJMenuBar.add(getMenuFile());
			jJMenuBar.add(getMenuEdit());
			jJMenuBar.add(getMenuView());
		}
		return jJMenuBar;
	}

	/**
	 * This method initializes menuFile	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuFile() {
		if (menuFile == null) {
			menuFile = new JMenu();
			menuFile.setText("File");
			menuFile.setMnemonic(KeyEvent.VK_F);
			menuFile.add(getMenuFileOpen());
			menuFile.add(getMenuFileSave());
			menuFile.add(getMenuFileSaveAs());
			menuFile.add(getJMenuItem3());
			menuFile.add(getMenuFileQuit());
		}
		return menuFile;
	}

	/**
	 * This method initializes menuFileOpen	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuFileOpen() {
		if (menuFileOpen == null) {
			menuFileOpen = new JMenuItem();
			menuFileOpen.setText("Open");
		}
		return menuFileOpen;
	}

	/**
	 * This method initializes menuFileSave	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuFileSave() {
		if (menuFileSave == null) {
			menuFileSave = new JMenuItem();
			menuFileSave.setText("Save");
		}
		return menuFileSave;
	}

	/**
	 * This method initializes menuFileSaveAs	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuFileSaveAs() {
		if (menuFileSaveAs == null) {
			menuFileSaveAs = new JMenuItem();
			menuFileSaveAs.setText("Save As(&A)");
		}
		return menuFileSaveAs;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JSeparator getJMenuItem3() {
		if (jMenuItem3 == null) {
			jMenuItem3 = new JSeparator();
			jMenuItem3.setBorder(null);
		}
		return jMenuItem3;
	}

	/**
	 * This method initializes menuFileQuit	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuFileQuit() {
		if (menuFileQuit == null) {
			menuFileQuit = new JMenuItem();
			menuFileQuit.setText("Close");
		}
		return menuFileQuit;
	}

	/**
	 * This method initializes jSplitPane	
	 * 	
	 * @return javax.swing.JSplitPane	
	 */
	private JSplitPane getJSplitPane() {
		if (jSplitPane == null) {
			jSplitPane = new JSplitPane();
			jSplitPane.setOrientation(JSplitPane.VERTICAL_SPLIT);
			jSplitPane.setTopComponent(getSplitContainerIn());
			jSplitPane.setBottomComponent(getPanelBottom());
			jSplitPane.setDividerLocation(260);
		}
		return jSplitPane;
	}

	/**
	 * This method initializes splitContainerIn	
	 * 	
	 * @return javax.swing.JSplitPane	
	 */
	private JSplitPane getSplitContainerIn() {
		if (splitContainerIn == null) {
			splitContainerIn = new JSplitPane();
			splitContainerIn.setDividerLocation(450);
			splitContainerIn.setRightComponent(getPanelRight());
			splitContainerIn.setLeftComponent(getPanelLeft());
		}
		return splitContainerIn;
	}

	/**
	 * This method initializes panelLeft	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanelLeft() {
		if (panelLeft == null) {
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.gridx = 0;
			gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints1.gridy = 1;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.fill = GridBagConstraints.BOTH;
			gridBagConstraints.gridy = 0;
			gridBagConstraints.weightx = 1.0;
			gridBagConstraints.weighty = 1.0D;
			gridBagConstraints.gridheight = 1;
			gridBagConstraints.gridx = 0;
			panelLeft = new JPanel();
			panelLeft.setLayout(new GridBagLayout());
			panelLeft.add(getJScrollPane(), gridBagConstraints);
			panelLeft.add(getPanelLeftBottom(), gridBagConstraints1);
		}
		return panelLeft;
	}

	/**
	 * This method initializes jScrollPane	
	 * 	
	 * @return javax.swing.JScrollPane	
	 */
	private JScrollPane getJScrollPane() {
		if (jScrollPane == null) {
			jScrollPane = new JScrollPane();
			jScrollPane.setViewportView(getListFiles());
		}
		return jScrollPane;
	}

	/**
	 * This method initializes listFiles	
	 * 	
	 * @return javax.swing.JTable	
	 */
	private JTable getListFiles() {
		if (jTable == null) {
			jTable = new JTable();
			jTable.setModel( getTableModel() );
		}
		return jTable;
	}

	private DefaultTableModel getTableModel(){
		if( listFiles == null){
			listFiles = new DefaultTableModel( columnHeaders, 0  );
		}
		return listFiles;
	}
	
	/**
	 * This method initializes panelLeftBottom	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanelLeftBottom() {
		if (panelLeftBottom == null) {
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.gridx = 4;
			gridBagConstraints6.weightx = 1.0D;
			gridBagConstraints6.gridy = 0;
			jLabel1 = new JLabel();
			jLabel1.setText("");
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.gridx = 3;
			gridBagConstraints5.insets = new Insets(2, 8, 2, 0);
			gridBagConstraints5.gridy = 0;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.fill = GridBagConstraints.NONE;
			gridBagConstraints4.gridy = 0;
			gridBagConstraints4.weightx = 0.0D;
			gridBagConstraints4.insets = new Insets(2, 8, 2, 0);
			gridBagConstraints4.gridx = 1;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 2;
			gridBagConstraints3.insets = new Insets(2, 8, 2, 0);
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 0;
			gridBagConstraints2.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints2.gridy = 0;
			lblSearch = new JLabel();
			lblSearch.setText("Search:");
			lblSearch.setHorizontalAlignment(SwingConstants.LEFT);
			panelLeftBottom = new JPanel();
			panelLeftBottom.setLayout(new GridBagLayout());
			panelLeftBottom.add(getButtonNext(), gridBagConstraints3);
			panelLeftBottom.add(lblSearch, gridBagConstraints2);
			panelLeftBottom.add(getTxtSearch(), gridBagConstraints4);
			panelLeftBottom.add(getButtonPrevious(), gridBagConstraints5);
			panelLeftBottom.add(jLabel1, gridBagConstraints6);
		}
		return panelLeftBottom;
	}

	/**
	 * This method initializes buttonNext	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getButtonNext() {
		if (buttonNext == null) {
			buttonNext = new JButton();
			buttonNext.setText("Next");
			buttonNext.setHorizontalAlignment(SwingConstants.LEFT);
		}
		return buttonNext;
	}

	/**
	 * This method initializes txtSearch	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtSearch() {
		if (txtSearch == null) {
			txtSearch = new JTextField();
			txtSearch.setPreferredSize(new Dimension(100, 20));
			txtSearch.setHorizontalAlignment(JTextField.LEFT);
		}
		return txtSearch;
	}

	/**
	 * This method initializes buttonPrevious	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getButtonPrevious() {
		if (buttonPrevious == null) {
			buttonPrevious = new JButton();
			buttonPrevious.setText("Previous");
			buttonPrevious.setHorizontalAlignment(SwingConstants.LEFT);
		}
		return buttonPrevious;
	}

	/**
	 * This method initializes panelRight	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanelRight() {
		if (panelRight == null) {
			GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
			gridBagConstraints20.gridx = 1;
			gridBagConstraints20.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints20.anchor = GridBagConstraints.WEST;
			gridBagConstraints20.gridy = 7;
			GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			gridBagConstraints19.gridx = 1;
			gridBagConstraints19.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints19.anchor = GridBagConstraints.WEST;
			gridBagConstraints19.gridy = 6;
			GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
			gridBagConstraints18.gridx = 1;
			gridBagConstraints18.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints18.anchor = GridBagConstraints.WEST;
			gridBagConstraints18.gridy = 5;
			GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			gridBagConstraints14.gridx = 1;
			gridBagConstraints14.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints14.anchor = GridBagConstraints.WEST;
			gridBagConstraints14.gridy = 4;
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.gridx = 1;
			gridBagConstraints12.anchor = GridBagConstraints.WEST;
			gridBagConstraints12.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints12.gridy = 3;
			GridBagConstraints gridBagConstraints24 = new GridBagConstraints();
			gridBagConstraints24.gridx = 0;
			gridBagConstraints24.insets = new Insets(16, 0, 0, 0);
			gridBagConstraints24.gridy = 0;
			jLabel4 = new JLabel();
			jLabel4.setText("");
			GridBagConstraints gridBagConstraints23 = new GridBagConstraints();
			gridBagConstraints23.gridx = 0;
			gridBagConstraints23.weighty = 1.0D;
			gridBagConstraints23.gridy = 10;
			jLabel3 = new JLabel();
			jLabel3.setText("");
			GridBagConstraints gridBagConstraints22 = new GridBagConstraints();
			gridBagConstraints22.gridx = 1;
			gridBagConstraints22.anchor = GridBagConstraints.WEST;
			gridBagConstraints22.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints22.gridwidth = 1;
			gridBagConstraints22.gridy = 9;
			GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			gridBagConstraints21.gridx = 1;
			gridBagConstraints21.anchor = GridBagConstraints.WEST;
			gridBagConstraints21.insets = new Insets(2, 16, 2, 0);
			gridBagConstraints21.gridwidth = 1;
			gridBagConstraints21.gridy = 8;
			GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
			gridBagConstraints17.gridx = 0;
			gridBagConstraints17.anchor = GridBagConstraints.WEST;
			gridBagConstraints17.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints17.gridy = 7;
			lblOverlap = new JLabel();
			lblOverlap.setText("Overlap");
			GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			gridBagConstraints16.gridx = 0;
			gridBagConstraints16.anchor = GridBagConstraints.WEST;
			gridBagConstraints16.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints16.gridy = 6;
			lblPreUtterance = new JLabel();
			lblPreUtterance.setText("Pre Utterance");
			GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			gridBagConstraints15.gridx = 0;
			gridBagConstraints15.anchor = GridBagConstraints.WEST;
			gridBagConstraints15.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints15.gridy = 5;
			lblBlank = new JLabel();
			lblBlank.setText("Blank");
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 0;
			gridBagConstraints13.anchor = GridBagConstraints.WEST;
			gridBagConstraints13.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints13.gridy = 4;
			lblConsonant = new JLabel();
			lblConsonant.setText("Consonant");
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.gridx = 0;
			gridBagConstraints11.anchor = GridBagConstraints.WEST;
			gridBagConstraints11.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints11.gridy = 3;
			lblOffset = new JLabel();
			lblOffset.setText("Offset");
			GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			gridBagConstraints10.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints10.gridy = 2;
			gridBagConstraints10.weightx = 1.0;
			gridBagConstraints10.insets = new Insets(2, 16, 2, 16);
			gridBagConstraints10.anchor = GridBagConstraints.WEST;
			gridBagConstraints10.gridwidth = 1;
			gridBagConstraints10.gridx = 1;
			GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			gridBagConstraints9.gridx = 0;
			gridBagConstraints9.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints9.anchor = GridBagConstraints.WEST;
			gridBagConstraints9.gridy = 2;
			lblAlias = new JLabel();
			lblAlias.setText("Alias");
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints8.gridy = 1;
			gridBagConstraints8.weightx = 1.0D;
			gridBagConstraints8.anchor = GridBagConstraints.WEST;
			gridBagConstraints8.insets = new Insets(2, 16, 2, 16);
			gridBagConstraints8.gridwidth = 1;
			gridBagConstraints8.gridx = 1;
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.gridx = 0;
			gridBagConstraints7.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints7.anchor = GridBagConstraints.WEST;
			gridBagConstraints7.gridy = 1;
			lblFileName = new JLabel();
			lblFileName.setText("File Name");
			panelRight = new JPanel();
			panelRight.setLayout(new GridBagLayout());
			panelRight.add(lblFileName, gridBagConstraints7);
			panelRight.add(getTxtFileName(), gridBagConstraints8);
			panelRight.add(lblAlias, gridBagConstraints9);
			panelRight.add(getTxtAlias(), gridBagConstraints10);
			panelRight.add(lblOffset, gridBagConstraints11);
			panelRight.add(lblConsonant, gridBagConstraints13);
			panelRight.add(lblBlank, gridBagConstraints15);
			panelRight.add(lblPreUtterance, gridBagConstraints16);
			panelRight.add(lblOverlap, gridBagConstraints17);
			panelRight.add(getBtnRefreshFrq(), gridBagConstraints21);
			panelRight.add(getBtnRefreshStf(), gridBagConstraints22);
			panelRight.add(jLabel3, gridBagConstraints23);
			panelRight.add(jLabel4, gridBagConstraints24);
			panelRight.add(getJPanel(), gridBagConstraints12);
			panelRight.add(getJPanel1(), gridBagConstraints14);
			panelRight.add(getJPanel2(), gridBagConstraints18);
			panelRight.add(getJPanel3(), gridBagConstraints19);
			panelRight.add(getJPanel4(), gridBagConstraints20);
		}
		return panelRight;
	}

	/**
	 * This method initializes txtFileName	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtFileName() {
		if (txtFileName == null) {
			txtFileName = new JTextField();
			txtFileName.setPreferredSize(new Dimension(100, 20));
		}
		return txtFileName;
	}

	/**
	 * This method initializes txtAlias	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtAlias() {
		if (txtAlias == null) {
			txtAlias = new JTextField();
			txtAlias.setPreferredSize(new Dimension(100, 20));
		}
		return txtAlias;
	}

	/**
	 * This method initializes btnRefreshFrq	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnRefreshFrq() {
		if (btnRefreshFrq == null) {
			btnRefreshFrq = new JButton();
			btnRefreshFrq.setText("Refresh FRQ");
		}
		return btnRefreshFrq;
	}

	/**
	 * This method initializes btnRefreshStf	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnRefreshStf() {
		if (btnRefreshStf == null) {
			btnRefreshStf = new JButton();
			btnRefreshStf.setText("Refresh STF");
		}
		return btnRefreshStf;
	}

	/**
	 * This method initializes panelBottom	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanelBottom() {
		if (panelBottom == null) {
			GridBagConstraints gridBagConstraints27 = new GridBagConstraints();
			gridBagConstraints27.gridx = 0;
			gridBagConstraints27.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints27.weightx = 1.0D;
			gridBagConstraints27.anchor = GridBagConstraints.SOUTH;
			gridBagConstraints27.gridy = 1;
			GridBagConstraints gridBagConstraints25 = new GridBagConstraints();
			gridBagConstraints25.gridx = 0;
			gridBagConstraints25.weightx = 1.0D;
			gridBagConstraints25.weighty = 1.0D;
			gridBagConstraints25.fill = GridBagConstraints.BOTH;
			gridBagConstraints25.anchor = GridBagConstraints.NORTH;
			gridBagConstraints25.gridy = 0;
			panelBottom = new JPanel();
			panelBottom.setLayout(new GridBagLayout());
			panelBottom.add(getPictWave(), gridBagConstraints25);
			panelBottom.add(getJPanel5(), gridBagConstraints27);
		}
		return panelBottom;
	}

	/**
	 * This method initializes pictWave	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPictWave() {
		if (pictWave == null) {
			pictWave = new JPanel();
			pictWave.setLayout(new GridBagLayout());
		}
		return pictWave;
	}

	/**
	 * This method initializes jPanel5	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel5() {
		if (jPanel5 == null) {
			GridBagConstraints gridBagConstraints26 = new GridBagConstraints();
			gridBagConstraints26.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints26.gridx = 0;
			gridBagConstraints26.gridy = 0;
			gridBagConstraints26.weightx = 1.0D;
			gridBagConstraints26.weighty = 1.0;
			jPanel5 = new JPanel();
			jPanel5.setLayout(new GridBagLayout());
			jPanel5.add(getHScroll(), gridBagConstraints26);
			jPanel5.add(getBtnMinus(), new GridBagConstraints());
			jPanel5.add(getBtnPlus(), new GridBagConstraints());
		}
		return jPanel5;
	}

	/**
	 * This method initializes hScroll	
	 * 	
	 * @return javax.swing.JScrollBar	
	 */
	private JScrollBar getHScroll() {
		if (hScroll == null) {
			hScroll = new JScrollBar();
			hScroll.setOrientation(JScrollBar.HORIZONTAL);
		}
		return hScroll;
	}

	/**
	 * This method initializes btnMinus	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnMinus() {
		if (btnMinus == null) {
			btnMinus = new JButton();
			btnMinus.setText("-");
			btnMinus.setPreferredSize(new Dimension(38, 16));
		}
		return btnMinus;
	}

	/**
	 * This method initializes btnPlus	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnPlus() {
		if (btnPlus == null) {
			btnPlus = new JButton();
			btnPlus.setText("+");
			btnPlus.setPreferredSize(new Dimension(41, 16));
		}
		return btnPlus;
	}

	/**
	 * This method initializes menuEdit	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuEdit() {
		if (menuEdit == null) {
			menuEdit = new JMenu();
			menuEdit.setText("Edit");
			menuEdit.setMnemonic(KeyEvent.VK_E);
			menuEdit.add(getMenuEditGenerateFrq());
			menuEdit.add(getMenuEditGenerateStf());
		}
		return menuEdit;
	}

	/**
	 * This method initializes menuEditGenerateFrq	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuEditGenerateFrq() {
		if (menuEditGenerateFrq == null) {
			menuEditGenerateFrq = new JMenuItem();
			menuEditGenerateFrq.setText("Generate FRQ files");
		}
		return menuEditGenerateFrq;
	}

	/**
	 * This method initializes menuEditGenerateStf	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuEditGenerateStf() {
		if (menuEditGenerateStf == null) {
			menuEditGenerateStf = new JMenuItem();
			menuEditGenerateStf.setText("Generate STF files");
		}
		return menuEditGenerateStf;
	}

	/**
	 * This method initializes menuView	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuView() {
		if (menuView == null) {
			menuView = new JMenu();
			menuView.setText("View");
			menuView.setMnemonic(KeyEvent.VK_V);
			menuView.add(getMenuViewSearchNext());
			menuView.add(getMenuViewSearchPrevious());
		}
		return menuView;
	}

	/**
	 * This method initializes menuViewSearchNext	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuViewSearchNext() {
		if (menuViewSearchNext == null) {
			menuViewSearchNext = new JMenuItem();
			menuViewSearchNext.setText("Search Next");
		}
		return menuViewSearchNext;
	}

	/**
	 * This method initializes menuViewSearchPrevious	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuViewSearchPrevious() {
		if (menuViewSearchPrevious == null) {
			menuViewSearchPrevious = new JMenuItem();
			menuViewSearchPrevious.setText("Search Previous");
		}
		return menuViewSearchPrevious;
	}

	/**
	 * This method initializes jPanel	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel() {
		if (jPanel == null) {
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.gridx = 2;
			gridBagConstraints31.weightx = 1.0D;
			gridBagConstraints31.fill = GridBagConstraints.BOTH;
			gridBagConstraints31.gridy = 0;
			jLabel5 = new JLabel();
			jLabel5.setText("");
			GridBagConstraints gridBagConstraints30 = new GridBagConstraints();
			gridBagConstraints30.gridx = 1;
			gridBagConstraints30.gridy = 0;
			jLabel2 = new JLabel();
			jLabel2.setText("ms");
			GridBagConstraints gridBagConstraints29 = new GridBagConstraints();
			gridBagConstraints29.fill = GridBagConstraints.NONE;
			gridBagConstraints29.gridy = 0;
			gridBagConstraints29.weightx = 1.0;
			gridBagConstraints29.anchor = GridBagConstraints.WEST;
			gridBagConstraints29.gridx = 0;
			jPanel = new JPanel();
			jPanel.setLayout(new GridBagLayout());
			jPanel.add(getTxtOffset(), gridBagConstraints29);
			jPanel.add(jLabel2, gridBagConstraints30);
			jPanel.add(jLabel5, gridBagConstraints31);
		}
		return jPanel;
	}

	/**
	 * This method initializes txtOffset	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtOffset() {
		if (txtOffset == null) {
			txtOffset = new JTextField();
			txtOffset.setPreferredSize(new Dimension(59, 20));
		}
		return txtOffset;
	}

	/**
	 * This method initializes jPanel1	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel1() {
		if (jPanel1 == null) {
			GridBagConstraints gridBagConstraints311 = new GridBagConstraints();
			gridBagConstraints311.fill = GridBagConstraints.BOTH;
			gridBagConstraints311.gridy = 0;
			gridBagConstraints311.weightx = 1.0D;
			gridBagConstraints311.gridx = 2;
			jLabel51 = new JLabel();
			jLabel51.setText("");
			GridBagConstraints gridBagConstraints301 = new GridBagConstraints();
			gridBagConstraints301.gridx = 1;
			gridBagConstraints301.gridy = 0;
			jLabel21 = new JLabel();
			jLabel21.setText("ms");
			GridBagConstraints gridBagConstraints291 = new GridBagConstraints();
			gridBagConstraints291.anchor = GridBagConstraints.WEST;
			gridBagConstraints291.gridx = 0;
			gridBagConstraints291.gridy = 0;
			gridBagConstraints291.weightx = 1.0;
			gridBagConstraints291.fill = GridBagConstraints.NONE;
			jPanel1 = new JPanel();
			jPanel1.setLayout(new GridBagLayout());
			jPanel1.add(getTxtConsonant(), gridBagConstraints291);
			jPanel1.add(jLabel21, gridBagConstraints301);
			jPanel1.add(jLabel51, gridBagConstraints311);
		}
		return jPanel1;
	}

	/**
	 * This method initializes txtConsonant	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtConsonant() {
		if (txtConsonant == null) {
			txtConsonant = new JTextField();
			txtConsonant.setPreferredSize(new Dimension(59, 20));
		}
		return txtConsonant;
	}

	/**
	 * This method initializes jPanel2	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel2() {
		if (jPanel2 == null) {
			GridBagConstraints gridBagConstraints312 = new GridBagConstraints();
			gridBagConstraints312.fill = GridBagConstraints.BOTH;
			gridBagConstraints312.gridy = 0;
			gridBagConstraints312.weightx = 1.0D;
			gridBagConstraints312.gridx = 2;
			jLabel52 = new JLabel();
			jLabel52.setText("");
			GridBagConstraints gridBagConstraints302 = new GridBagConstraints();
			gridBagConstraints302.gridx = 1;
			gridBagConstraints302.gridy = 0;
			jLabel22 = new JLabel();
			jLabel22.setText("ms");
			GridBagConstraints gridBagConstraints292 = new GridBagConstraints();
			gridBagConstraints292.anchor = GridBagConstraints.WEST;
			gridBagConstraints292.gridx = 0;
			gridBagConstraints292.gridy = 0;
			gridBagConstraints292.weightx = 1.0;
			gridBagConstraints292.fill = GridBagConstraints.NONE;
			jPanel2 = new JPanel();
			jPanel2.setLayout(new GridBagLayout());
			jPanel2.add(getTxtBlank(), gridBagConstraints292);
			jPanel2.add(jLabel22, gridBagConstraints302);
			jPanel2.add(jLabel52, gridBagConstraints312);
		}
		return jPanel2;
	}

	/**
	 * This method initializes txtBlank	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtBlank() {
		if (txtBlank == null) {
			txtBlank = new JTextField();
			txtBlank.setPreferredSize(new Dimension(59, 20));
		}
		return txtBlank;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel3() {
		if (jPanel3 == null) {
			GridBagConstraints gridBagConstraints313 = new GridBagConstraints();
			gridBagConstraints313.fill = GridBagConstraints.BOTH;
			gridBagConstraints313.gridy = 0;
			gridBagConstraints313.weightx = 1.0D;
			gridBagConstraints313.gridx = 2;
			jLabel53 = new JLabel();
			jLabel53.setText("");
			GridBagConstraints gridBagConstraints303 = new GridBagConstraints();
			gridBagConstraints303.gridx = 1;
			gridBagConstraints303.gridy = 0;
			jLabel23 = new JLabel();
			jLabel23.setText("ms");
			GridBagConstraints gridBagConstraints293 = new GridBagConstraints();
			gridBagConstraints293.anchor = GridBagConstraints.WEST;
			gridBagConstraints293.gridx = 0;
			gridBagConstraints293.gridy = 0;
			gridBagConstraints293.weightx = 1.0;
			gridBagConstraints293.fill = GridBagConstraints.NONE;
			jPanel3 = new JPanel();
			jPanel3.setLayout(new GridBagLayout());
			jPanel3.add(getTxtPreUtterance(), gridBagConstraints293);
			jPanel3.add(jLabel23, gridBagConstraints303);
			jPanel3.add(jLabel53, gridBagConstraints313);
		}
		return jPanel3;
	}

	/**
	 * This method initializes txtPreUtterance	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtPreUtterance() {
		if (txtPreUtterance == null) {
			txtPreUtterance = new JTextField();
			txtPreUtterance.setPreferredSize(new Dimension(59, 20));
		}
		return txtPreUtterance;
	}

	/**
	 * This method initializes jPanel4	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel4() {
		if (jPanel4 == null) {
			GridBagConstraints gridBagConstraints314 = new GridBagConstraints();
			gridBagConstraints314.fill = GridBagConstraints.BOTH;
			gridBagConstraints314.gridy = 0;
			gridBagConstraints314.weightx = 1.0D;
			gridBagConstraints314.gridx = 2;
			jLabel54 = new JLabel();
			jLabel54.setText("");
			GridBagConstraints gridBagConstraints304 = new GridBagConstraints();
			gridBagConstraints304.gridx = 1;
			gridBagConstraints304.gridy = 0;
			jLabel24 = new JLabel();
			jLabel24.setText("ms");
			GridBagConstraints gridBagConstraints294 = new GridBagConstraints();
			gridBagConstraints294.anchor = GridBagConstraints.WEST;
			gridBagConstraints294.gridx = 0;
			gridBagConstraints294.gridy = 0;
			gridBagConstraints294.weightx = 1.0;
			gridBagConstraints294.fill = GridBagConstraints.NONE;
			jPanel4 = new JPanel();
			jPanel4.setLayout(new GridBagLayout());
			jPanel4.add(getTxtOverlap(), gridBagConstraints294);
			jPanel4.add(jLabel24, gridBagConstraints304);
			jPanel4.add(jLabel54, gridBagConstraints314);
		}
		return jPanel4;
	}

	/**
	 * This method initializes txtOverlap	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtOverlap() {
		if (txtOverlap == null) {
			txtOverlap = new JTextField();
			txtOverlap.setPreferredSize(new Dimension(59, 20));
		}
		return txtOverlap;
	}

}  //  @jve:decl-index=0:visual-constraint="30,22"
