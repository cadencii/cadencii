package org.kbinani.editotoini;

//SECTION-BEGIN-IMPORT
import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import java.awt.event.KeyEvent;
import javax.swing.JScrollPane;
import javax.swing.JSeparator;
import javax.swing.JTable;
import javax.swing.SwingConstants;
import javax.swing.table.DefaultTableModel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BHScrollBar;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BMenu;
import org.kbinani.windows.forms.BMenuBar;
import org.kbinani.windows.forms.BMenuItem;
import org.kbinani.windows.forms.BPanel;
import org.kbinani.windows.forms.BSplitPane;
import org.kbinani.windows.forms.BTextBox;

//SECTION-END-IMPORT
public class FormUtauVoiceConfig extends BForm {
    //SECTION-BEGIN-FIELD
	private static final long serialVersionUID = 1L;
	private BPanel contentPanel = null;
	private BMenuBar jJMenuBar = null;
	private BMenu menuFile = null;
	private BMenuItem menuFileOpen = null;
	private BMenuItem menuFileSave = null;
	private BMenuItem menuFileSaveAs = null;
	private JSeparator jMenuItem3 = null;
	private BMenuItem menuFileQuit = null;
	private BSplitPane splitContainerOut = null;
	private BSplitPane splitContainerIn = null;
	private BPanel panelLeft = null;
	private JScrollPane jScrollPane = null;
	private JTable jTable = null;
	private BPanel panelLeftBottom = null;
	private BButton buttonNext = null;
	private BLabel lblSearch = null;
	private BTextBox txtSearch = null;
	private BButton buttonPrevious = null;
	private BLabel jLabel1 = null;
	private BPanel panelRight = null;
	private BLabel lblFileName = null;
	private BTextBox txtFileName = null;
	private BLabel lblAlias = null;
	private BTextBox txtAlias = null;
	private BLabel lblOffset = null;
	private BLabel lblConsonant = null;
	private BLabel lblBlank = null;
	private BLabel lblPreUtterance = null;
	private BLabel lblOverlap = null;
	private BButton btnRefreshFrq = null;
	private BButton btnRefreshStf = null;
	private BLabel jLabel3 = null;
	private BLabel jLabel4 = null;
	private BPanel panelBottom = null;
	private BPanel pictWave = null;
	private BPanel jPanel5 = null;
	private BHScrollBar hScroll = null;
	private BButton btnMinus = null;
	private BButton btnPlus = null;
	private BMenu menuEdit = null;
	private BMenuItem menuEditGenerateFrq = null;
	private BMenuItem menuEditGenerateStf = null;
	private BMenu menuView = null;
	private BMenuItem menuViewSearchNext = null;
	private BMenuItem menuViewSearchPrevious = null;
	private DefaultTableModel listFiles = null;
	private String[] columnHeaders = new String[]{ "FIle Name", "Alias", "offste", "Consonant", "Blank", "pre Utrerance", "Overlap" };
	private BPanel BPanel = null;
	private BTextBox txtOffset = null;
	private BLabel jLabel2 = null;
	private BLabel jLabel5 = null;
	private BPanel jPanel1 = null;
	private BTextBox txtConsonant = null;
	private BLabel jLabel21 = null;
	private BLabel jLabel51 = null;
	private BPanel jPanel2 = null;
	private BTextBox txtBlank = null;
	private BLabel jLabel22 = null;
	private BLabel jLabel52 = null;
	private BPanel jPanel3 = null;
	private BTextBox txtPreUtterance = null;
	private BLabel jLabel23 = null;
	private BLabel jLabel53 = null;
	private BPanel jPanel4 = null;
	private BTextBox txtOverlap = null;
	private BLabel jLabel24 = null;
	private BLabel jLabel54 = null;
	
	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public FormUtauVoiceConfig() {
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
		this.setSize(712, 507);
		this.setJMenuBar(getJJMenuBar());
		this.setContentPane(getContentPanel());
		this.setTitle("JFrame");
	}

	/**
	 * This method initializes contentPanel	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getContentPanel() {
		if (contentPanel == null) {
			contentPanel = new BPanel();
			contentPanel.setLayout(new BorderLayout());
			contentPanel.add(getJSplitPane(), BorderLayout.CENTER);
		}
		return contentPanel;
	}

	/**
	 * This method initializes jJMenuBar	
	 * 	
	 * @return org.kbinani.windows.forms.BMenuBar	
	 */
	private BMenuBar getJJMenuBar() {
		if (jJMenuBar == null) {
			jJMenuBar = new BMenuBar();
			jJMenuBar.add(getMenuFile());
			jJMenuBar.add(getMenuEdit());
			jJMenuBar.add(getMenuView());
		}
		return jJMenuBar;
	}

	/**
	 * This method initializes menuFile	
	 * 	
	 * @return org.kbinani.windows.forms.BMenu	
	 */
	private BMenu getMenuFile() {
		if (menuFile == null) {
			menuFile = new BMenu();
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
	 * @return org.kbinani.windows.forms.BMenuItem	
	 */
	private BMenuItem getMenuFileOpen() {
		if (menuFileOpen == null) {
			menuFileOpen = new BMenuItem();
			menuFileOpen.setText("Open");
		}
		return menuFileOpen;
	}

	/**
	 * This method initializes menuFileSave	
	 * 	
	 * @return org.kbinani.windows.forms.BMenuItem	
	 */
	private BMenuItem getMenuFileSave() {
		if (menuFileSave == null) {
			menuFileSave = new BMenuItem();
			menuFileSave.setText("Save");
		}
		return menuFileSave;
	}

	/**
	 * This method initializes menuFileSaveAs	
	 * 	
	 * @return org.kbinani.windows.forms.BMenuItem	
	 */
	private BMenuItem getMenuFileSaveAs() {
		if (menuFileSaveAs == null) {
			menuFileSaveAs = new BMenuItem();
			menuFileSaveAs.setText("Save As(&A)");
		}
		return menuFileSaveAs;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return org.kbinani.windows.forms.BMenuItem	
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
	 * @return org.kbinani.windows.forms.BMenuItem	
	 */
	private BMenuItem getMenuFileQuit() {
		if (menuFileQuit == null) {
			menuFileQuit = new BMenuItem();
			menuFileQuit.setText("Close");
		}
		return menuFileQuit;
	}

	/**
	 * This method initializes BSplitPane	
	 * 	
	 * @return org.kbinani.windows.forms.BSplitPane	
	 */
	private BSplitPane getJSplitPane() {
		if (splitContainerOut == null) {
			splitContainerOut = new BSplitPane();
			splitContainerOut.setOrientation(BSplitPane.VERTICAL_SPLIT);
			splitContainerOut.setTopComponent(getSplitContainerIn());
			splitContainerOut.setBottomComponent(getPanelBottom());
			splitContainerOut.setDividerLocation(260);
		}
		return splitContainerOut;
	}

	/**
	 * This method initializes splitContainerIn	
	 * 	
	 * @return org.kbinani.windows.forms.BSplitPane	
	 */
	private BSplitPane getSplitContainerIn() {
		if (splitContainerIn == null) {
			splitContainerIn = new BSplitPane();
			splitContainerIn.setDividerLocation(450);
			splitContainerIn.setRightComponent(getPanelRight());
			splitContainerIn.setLeftComponent(getPanelLeft());
		}
		return splitContainerIn;
	}

	/**
	 * This method initializes panelLeft	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getPanelLeft() {
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
			panelLeft = new BPanel();
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
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getPanelLeftBottom() {
		if (panelLeftBottom == null) {
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.gridx = 4;
			gridBagConstraints6.weightx = 1.0D;
			gridBagConstraints6.gridy = 0;
			jLabel1 = new BLabel();
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
			lblSearch = new BLabel();
			lblSearch.setText("Search:");
			lblSearch.setHorizontalAlignment(SwingConstants.LEFT);
			panelLeftBottom = new BPanel();
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
	 * @return org.kbinani.windows.forms.BButton	
	 */
	private BButton getButtonNext() {
		if (buttonNext == null) {
			buttonNext = new BButton();
			buttonNext.setText("Next");
			buttonNext.setHorizontalAlignment(SwingConstants.LEFT);
		}
		return buttonNext;
	}

	/**
	 * This method initializes txtSearch	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtSearch() {
		if (txtSearch == null) {
			txtSearch = new BTextBox();
			txtSearch.setPreferredSize(new Dimension(100, 20));
			txtSearch.setHorizontalAlignment(BTextBox.LEFT);
		}
		return txtSearch;
	}

	/**
	 * This method initializes buttonPrevious	
	 * 	
	 * @return org.kbinani.windows.forms.BButton	
	 */
	private BButton getButtonPrevious() {
		if (buttonPrevious == null) {
			buttonPrevious = new BButton();
			buttonPrevious.setText("Previous");
			buttonPrevious.setHorizontalAlignment(SwingConstants.LEFT);
		}
		return buttonPrevious;
	}

	/**
	 * This method initializes panelRight	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getPanelRight() {
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
			jLabel4 = new BLabel();
			jLabel4.setText("");
			GridBagConstraints gridBagConstraints23 = new GridBagConstraints();
			gridBagConstraints23.gridx = 0;
			gridBagConstraints23.weighty = 1.0D;
			gridBagConstraints23.gridy = 10;
			jLabel3 = new BLabel();
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
			lblOverlap = new BLabel();
			lblOverlap.setText("Overlap");
			GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			gridBagConstraints16.gridx = 0;
			gridBagConstraints16.anchor = GridBagConstraints.WEST;
			gridBagConstraints16.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints16.gridy = 6;
			lblPreUtterance = new BLabel();
			lblPreUtterance.setText("Pre Utterance");
			GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			gridBagConstraints15.gridx = 0;
			gridBagConstraints15.anchor = GridBagConstraints.WEST;
			gridBagConstraints15.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints15.gridy = 5;
			lblBlank = new BLabel();
			lblBlank.setText("Blank");
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 0;
			gridBagConstraints13.anchor = GridBagConstraints.WEST;
			gridBagConstraints13.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints13.gridy = 4;
			lblConsonant = new BLabel();
			lblConsonant.setText("Consonant");
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.gridx = 0;
			gridBagConstraints11.anchor = GridBagConstraints.WEST;
			gridBagConstraints11.insets = new Insets(0, 16, 0, 0);
			gridBagConstraints11.gridy = 3;
			lblOffset = new BLabel();
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
			lblAlias = new BLabel();
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
			lblFileName = new BLabel();
			lblFileName.setText("File Name");
			panelRight = new BPanel();
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
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtFileName() {
		if (txtFileName == null) {
			txtFileName = new BTextBox();
			txtFileName.setPreferredSize(new Dimension(100, 20));
		}
		return txtFileName;
	}

	/**
	 * This method initializes txtAlias	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtAlias() {
		if (txtAlias == null) {
			txtAlias = new BTextBox();
			txtAlias.setPreferredSize(new Dimension(100, 20));
		}
		return txtAlias;
	}

	/**
	 * This method initializes btnRefreshFrq	
	 * 	
	 * @return org.kbinani.windows.forms.BButton	
	 */
	private BButton getBtnRefreshFrq() {
		if (btnRefreshFrq == null) {
			btnRefreshFrq = new BButton();
			btnRefreshFrq.setText("Refresh FRQ");
		}
		return btnRefreshFrq;
	}

	/**
	 * This method initializes btnRefreshStf	
	 * 	
	 * @return org.kbinani.windows.forms.BButton	
	 */
	private BButton getBtnRefreshStf() {
		if (btnRefreshStf == null) {
			btnRefreshStf = new BButton();
			btnRefreshStf.setText("Refresh STF");
		}
		return btnRefreshStf;
	}

	/**
	 * This method initializes panelBottom	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getPanelBottom() {
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
			panelBottom = new BPanel();
			panelBottom.setLayout(new GridBagLayout());
			panelBottom.add(getPictWave(), gridBagConstraints25);
			panelBottom.add(getJPanel5(), gridBagConstraints27);
		}
		return panelBottom;
	}

	/**
	 * This method initializes pictWave	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getPictWave() {
		if (pictWave == null) {
			pictWave = new BPanel();
			pictWave.setLayout(new GridBagLayout());
		}
		return pictWave;
	}

	/**
	 * This method initializes jPanel5	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getJPanel5() {
		if (jPanel5 == null) {
			GridBagConstraints gridBagConstraints26 = new GridBagConstraints();
			gridBagConstraints26.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints26.gridx = 0;
			gridBagConstraints26.gridy = 0;
			gridBagConstraints26.weightx = 1.0D;
			gridBagConstraints26.weighty = 1.0;
			jPanel5 = new BPanel();
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
	private BHScrollBar getHScroll() {
		if (hScroll == null) {
			hScroll = new BHScrollBar();
		}
		return hScroll;
	}

	/**
	 * This method initializes btnMinus	
	 * 	
	 * @return org.kbinani.windows.forms.BButton	
	 */
	private BButton getBtnMinus() {
		if (btnMinus == null) {
			btnMinus = new BButton();
			btnMinus.setText("-");
			btnMinus.setPreferredSize(new Dimension(38, 16));
		}
		return btnMinus;
	}

	/**
	 * This method initializes btnPlus	
	 * 	
	 * @return org.kbinani.windows.forms.BButton	
	 */
	private BButton getBtnPlus() {
		if (btnPlus == null) {
			btnPlus = new BButton();
			btnPlus.setText("+");
			btnPlus.setPreferredSize(new Dimension(41, 16));
		}
		return btnPlus;
	}

	/**
	 * This method initializes menuEdit	
	 * 	
	 * @return org.kbinani.windows.forms.BMenu	
	 */
	private BMenu getMenuEdit() {
		if (menuEdit == null) {
			menuEdit = new BMenu();
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
	 * @return org.kbinani.windows.forms.BMenuItem	
	 */
	private BMenuItem getMenuEditGenerateFrq() {
		if (menuEditGenerateFrq == null) {
			menuEditGenerateFrq = new BMenuItem();
			menuEditGenerateFrq.setText("Generate FRQ files");
		}
		return menuEditGenerateFrq;
	}

	/**
	 * This method initializes menuEditGenerateStf	
	 * 	
	 * @return org.kbinani.windows.forms.BMenuItem	
	 */
	private BMenuItem getMenuEditGenerateStf() {
		if (menuEditGenerateStf == null) {
			menuEditGenerateStf = new BMenuItem();
			menuEditGenerateStf.setText("Generate STF files");
		}
		return menuEditGenerateStf;
	}

	/**
	 * This method initializes menuView	
	 * 	
	 * @return org.kbinani.windows.forms.BMenu	
	 */
	private BMenu getMenuView() {
		if (menuView == null) {
			menuView = new BMenu();
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
	 * @return org.kbinani.windows.forms.BMenuItem	
	 */
	private BMenuItem getMenuViewSearchNext() {
		if (menuViewSearchNext == null) {
			menuViewSearchNext = new BMenuItem();
			menuViewSearchNext.setText("Search Next");
		}
		return menuViewSearchNext;
	}

	/**
	 * This method initializes menuViewSearchPrevious	
	 * 	
	 * @return org.kbinani.windows.forms.BMenuItem	
	 */
	private BMenuItem getMenuViewSearchPrevious() {
		if (menuViewSearchPrevious == null) {
			menuViewSearchPrevious = new BMenuItem();
			menuViewSearchPrevious.setText("Search Previous");
		}
		return menuViewSearchPrevious;
	}

	/**
	 * This method initializes BPanel	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getJPanel() {
		if (BPanel == null) {
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.gridx = 2;
			gridBagConstraints31.weightx = 1.0D;
			gridBagConstraints31.fill = GridBagConstraints.BOTH;
			gridBagConstraints31.gridy = 0;
			jLabel5 = new BLabel();
			jLabel5.setText("");
			GridBagConstraints gridBagConstraints30 = new GridBagConstraints();
			gridBagConstraints30.gridx = 1;
			gridBagConstraints30.gridy = 0;
			jLabel2 = new BLabel();
			jLabel2.setText("ms");
			GridBagConstraints gridBagConstraints29 = new GridBagConstraints();
			gridBagConstraints29.fill = GridBagConstraints.NONE;
			gridBagConstraints29.gridy = 0;
			gridBagConstraints29.weightx = 1.0;
			gridBagConstraints29.anchor = GridBagConstraints.WEST;
			gridBagConstraints29.gridx = 0;
			BPanel = new BPanel();
			BPanel.setLayout(new GridBagLayout());
			BPanel.add(getTxtOffset(), gridBagConstraints29);
			BPanel.add(jLabel2, gridBagConstraints30);
			BPanel.add(jLabel5, gridBagConstraints31);
		}
		return BPanel;
	}

	/**
	 * This method initializes txtOffset	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtOffset() {
		if (txtOffset == null) {
			txtOffset = new BTextBox();
			txtOffset.setPreferredSize(new Dimension(59, 20));
		}
		return txtOffset;
	}

	/**
	 * This method initializes jPanel1	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getJPanel1() {
		if (jPanel1 == null) {
			GridBagConstraints gridBagConstraints311 = new GridBagConstraints();
			gridBagConstraints311.fill = GridBagConstraints.BOTH;
			gridBagConstraints311.gridy = 0;
			gridBagConstraints311.weightx = 1.0D;
			gridBagConstraints311.gridx = 2;
			jLabel51 = new BLabel();
			jLabel51.setText("");
			GridBagConstraints gridBagConstraints301 = new GridBagConstraints();
			gridBagConstraints301.gridx = 1;
			gridBagConstraints301.gridy = 0;
			jLabel21 = new BLabel();
			jLabel21.setText("ms");
			GridBagConstraints gridBagConstraints291 = new GridBagConstraints();
			gridBagConstraints291.anchor = GridBagConstraints.WEST;
			gridBagConstraints291.gridx = 0;
			gridBagConstraints291.gridy = 0;
			gridBagConstraints291.weightx = 1.0;
			gridBagConstraints291.fill = GridBagConstraints.NONE;
			jPanel1 = new BPanel();
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
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtConsonant() {
		if (txtConsonant == null) {
			txtConsonant = new BTextBox();
			txtConsonant.setPreferredSize(new Dimension(59, 20));
		}
		return txtConsonant;
	}

	/**
	 * This method initializes jPanel2	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getJPanel2() {
		if (jPanel2 == null) {
			GridBagConstraints gridBagConstraints312 = new GridBagConstraints();
			gridBagConstraints312.fill = GridBagConstraints.BOTH;
			gridBagConstraints312.gridy = 0;
			gridBagConstraints312.weightx = 1.0D;
			gridBagConstraints312.gridx = 2;
			jLabel52 = new BLabel();
			jLabel52.setText("");
			GridBagConstraints gridBagConstraints302 = new GridBagConstraints();
			gridBagConstraints302.gridx = 1;
			gridBagConstraints302.gridy = 0;
			jLabel22 = new BLabel();
			jLabel22.setText("ms");
			GridBagConstraints gridBagConstraints292 = new GridBagConstraints();
			gridBagConstraints292.anchor = GridBagConstraints.WEST;
			gridBagConstraints292.gridx = 0;
			gridBagConstraints292.gridy = 0;
			gridBagConstraints292.weightx = 1.0;
			gridBagConstraints292.fill = GridBagConstraints.NONE;
			jPanel2 = new BPanel();
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
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtBlank() {
		if (txtBlank == null) {
			txtBlank = new BTextBox();
			txtBlank.setPreferredSize(new Dimension(59, 20));
		}
		return txtBlank;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getJPanel3() {
		if (jPanel3 == null) {
			GridBagConstraints gridBagConstraints313 = new GridBagConstraints();
			gridBagConstraints313.fill = GridBagConstraints.BOTH;
			gridBagConstraints313.gridy = 0;
			gridBagConstraints313.weightx = 1.0D;
			gridBagConstraints313.gridx = 2;
			jLabel53 = new BLabel();
			jLabel53.setText("");
			GridBagConstraints gridBagConstraints303 = new GridBagConstraints();
			gridBagConstraints303.gridx = 1;
			gridBagConstraints303.gridy = 0;
			jLabel23 = new BLabel();
			jLabel23.setText("ms");
			GridBagConstraints gridBagConstraints293 = new GridBagConstraints();
			gridBagConstraints293.anchor = GridBagConstraints.WEST;
			gridBagConstraints293.gridx = 0;
			gridBagConstraints293.gridy = 0;
			gridBagConstraints293.weightx = 1.0;
			gridBagConstraints293.fill = GridBagConstraints.NONE;
			jPanel3 = new BPanel();
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
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtPreUtterance() {
		if (txtPreUtterance == null) {
			txtPreUtterance = new BTextBox();
			txtPreUtterance.setPreferredSize(new Dimension(59, 20));
		}
		return txtPreUtterance;
	}

	/**
	 * This method initializes jPanel4	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getJPanel4() {
		if (jPanel4 == null) {
			GridBagConstraints gridBagConstraints314 = new GridBagConstraints();
			gridBagConstraints314.fill = GridBagConstraints.BOTH;
			gridBagConstraints314.gridy = 0;
			gridBagConstraints314.weightx = 1.0D;
			gridBagConstraints314.gridx = 2;
			jLabel54 = new BLabel();
			jLabel54.setText("");
			GridBagConstraints gridBagConstraints304 = new GridBagConstraints();
			gridBagConstraints304.gridx = 1;
			gridBagConstraints304.gridy = 0;
			jLabel24 = new BLabel();
			jLabel24.setText("ms");
			GridBagConstraints gridBagConstraints294 = new GridBagConstraints();
			gridBagConstraints294.anchor = GridBagConstraints.WEST;
			gridBagConstraints294.gridx = 0;
			gridBagConstraints294.gridy = 0;
			gridBagConstraints294.weightx = 1.0;
			gridBagConstraints294.fill = GridBagConstraints.NONE;
			jPanel4 = new BPanel();
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
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtOverlap() {
		if (txtOverlap == null) {
			txtOverlap = new BTextBox();
			txtOverlap.setPreferredSize(new Dimension(59, 20));
		}
		return txtOverlap;
	}

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="30,22"
