package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.BorderFactory;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JRadioButton;
import javax.swing.border.TitledBorder;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BListView;
import org.kbinani.windows.forms.BRadioButton;
import org.kbinani.windows.forms.RadioButtonManager;
import javax.swing.JScrollPane;

//SECTION-END-IMPORT
public class FormMidiImExport extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private JPanel jPanel = null;
	private BButton btnCheckAll = null;
	private BButton btnUncheckAll = null;
	private JLabel jLabel = null;
	public BListView listTrack = null;
	private BGroupBox groupCommonOption = null;
	private BCheckBox chkTempo = null;
	private JPanel jPanel2 = null;
	private BButton btnOK = null;
	private BButton btnCancel = null;
	private JPanel jPanel3 = null;
	private BCheckBox chkBeat = null;
	private BCheckBox chkLyric = null;
	private JLabel jLabel1 = null;
	private JPanel panel2 = null;
	private BCheckBox chkNote = null;
	private BCheckBox chkMetaText = null;
	private JLabel jLabel11 = null;
	private JPanel jPanel32 = null;
	private BCheckBox chkExportVocaloidNrpn = null;
	private BCheckBox chkPreMeasure = null;
	private JLabel jLabel12 = null;
    private BGroupBox groupMode = null;
    private JPanel jPanel31 = null;
    private JLabel jLabel13 = null;
    private JPanel panel21 = null;
    private JLabel jLabel111 = null;
    private BRadioButton radioGateTime = null;
    private BRadioButton radioPlayTime = null;
    private BLabel lblOffset = null;
    private NumberTextBox txtOffset = null;
    private BLabel lblOffsetUnit = null;
    private JScrollPane jScrollPane = null;
    private RadioButtonManager mButtonManager = null;  //  @jve:decl-index=0:
    private BLabel lblRightValue = null;
	
	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public FormMidiImExport() {
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
		this.setSize(400, 449);
		this.setContentPane(getJContentPane());
		this.setTitle("JFrame");
		mButtonManager = new RadioButtonManager();
		mButtonManager.add( radioGateTime );
		mButtonManager.add( radioPlayTime );
		setCancelButton( btnCancel );
	}

	/**
	 * This method initializes jContentPane
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getJContentPane() {
		if (jContentPane == null) {
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.fill = GridBagConstraints.BOTH;
			gridBagConstraints4.weighty = 1.0D;
			gridBagConstraints4.gridx = 0;
			gridBagConstraints4.gridy = 1;
			gridBagConstraints4.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints4.weightx = 1.0;
			GridBagConstraints gridBagConstraints22 = new GridBagConstraints();
			gridBagConstraints22.gridx = 0;
			gridBagConstraints22.weightx = 1.0D;
			gridBagConstraints22.fill = GridBagConstraints.BOTH;
			gridBagConstraints22.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints22.gridy = 3;
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 0;
			gridBagConstraints13.anchor = GridBagConstraints.EAST;
			gridBagConstraints13.insets = new Insets(12, 0, 12, 0);
			gridBagConstraints13.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints13.gridy = 4;
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.gridx = 0;
			gridBagConstraints12.weightx = 1.0D;
			gridBagConstraints12.fill = GridBagConstraints.BOTH;
			gridBagConstraints12.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints12.weighty = 0.0D;
			gridBagConstraints12.gridy = 2;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.insets = new Insets(12, 12, 6, 12);
			gridBagConstraints3.gridy = 0;
			gridBagConstraints3.ipadx = 146;
			gridBagConstraints3.anchor = GridBagConstraints.WEST;
			gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints3.gridx = 0;
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.setPreferredSize(new Dimension(100, 100));
			jContentPane.add(getJPanel(), gridBagConstraints3);
			jContentPane.add(getJScrollPane(), gridBagConstraints4);
			jContentPane.add(getGroupCommonOption(), gridBagConstraints12);
			jContentPane.add(getJPanel2(), gridBagConstraints13);
			jContentPane.add(getGroupMode(), gridBagConstraints22);
		}
		return jContentPane;
	}

	/**
	 * This method initializes jPanel	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel() {
		if (jPanel == null) {
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 2;
			gridBagConstraints2.weightx = 1.0D;
			gridBagConstraints2.fill = GridBagConstraints.NONE;
			gridBagConstraints2.insets = new Insets(0, 0, 0, 0);
			gridBagConstraints2.gridy = 0;
			jLabel = new JLabel();
			jLabel.setText(" ");
			jLabel.setPreferredSize(new Dimension(4, 4));
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.gridx = 1;
			gridBagConstraints1.insets = new Insets(0, 0, 0, 0);
			gridBagConstraints1.gridy = 0;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.weighty = 0.0D;
			gridBagConstraints.fill = GridBagConstraints.NONE;
			gridBagConstraints.insets = new Insets(0, 0, 0, 0);
			gridBagConstraints.gridy = 0;
			jPanel = new JPanel();
			jPanel.setLayout(new GridBagLayout());
			jPanel.add(getBtnCheckAll(), gridBagConstraints);
			jPanel.add(getBtnUncheckAll(), gridBagConstraints1);
			jPanel.add(jLabel, gridBagConstraints2);
		}
		return jPanel;
	}

	/**
	 * This method initializes btnCheckAll	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnCheckAll() {
		if (btnCheckAll == null) {
			btnCheckAll = new BButton();
			btnCheckAll.setText("Check All");
			btnCheckAll.setName("btnCheckAll");
			btnCheckAll.setPreferredSize(new Dimension(120, 29));
		}
		return btnCheckAll;
	}

	/**
	 * This method initializes btnUncheckAll	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnUncheckAll() {
		if (btnUncheckAll == null) {
			btnUncheckAll = new BButton();
			btnUncheckAll.setText("Uncheck All");
			btnUncheckAll.setName("btnUncheckAll");
			btnUncheckAll.setPreferredSize(new Dimension(170, 29));
		}
		return btnUncheckAll;
	}

	/**
	 * This method initializes listTrack	
	 * 	
	 * @return javax.swing.JTable	
	 */
	private BListView getListTrack() {
		if (listTrack == null) {
			listTrack = new BListView();
		}
		return listTrack;
	}

	/**
	 * This method initializes groupCommonOption	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupCommonOption() {
		if (groupCommonOption == null) {
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.gridx = 0;
			gridBagConstraints11.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints11.weightx = 1.0D;
			gridBagConstraints11.anchor = GridBagConstraints.WEST;
			gridBagConstraints11.gridy = 2;
			GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			gridBagConstraints10.gridx = 0;
			gridBagConstraints10.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints10.weightx = 1.0D;
			gridBagConstraints10.anchor = GridBagConstraints.WEST;
			gridBagConstraints10.gridy = 1;
			GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			gridBagConstraints9.gridx = 0;
			gridBagConstraints9.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints9.weightx = 1.0D;
			gridBagConstraints9.gridy = 0;
			groupCommonOption = new BGroupBox();
			groupCommonOption.setLayout(new GridBagLayout());
			groupCommonOption.setBorder(BorderFactory.createTitledBorder(null, "Option", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupCommonOption.setPreferredSize(new Dimension(274, 85));
			groupCommonOption.add(getJPanel3(), gridBagConstraints9);
			groupCommonOption.add(getPanel2(), gridBagConstraints10);
			groupCommonOption.add(getJPanel32(), gridBagConstraints11);
		}
		return groupCommonOption;
	}

	/**
	 * This method initializes chkTempo	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkTempo() {
		if (chkTempo == null) {
			chkTempo = new BCheckBox();
			chkTempo.setText("Tempo");
			chkTempo.setName("chkTempo");
		}
		return chkTempo;
	}

	/**
	 * This method initializes jPanel2	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel2() {
		if (jPanel2 == null) {
			GridBagConstraints gridBagConstraints23 = new GridBagConstraints();
			gridBagConstraints23.gridx = 0;
			gridBagConstraints23.weightx = 1.0D;
			gridBagConstraints23.gridy = 0;
			lblRightValue = new BLabel();
			lblRightValue.setPreferredSize(new Dimension(4, 4));
			lblRightValue.setText("");
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.insets = new Insets(0, 0, 0, 0);
			gridBagConstraints31.gridy = 0;
			gridBagConstraints31.gridx = 1;
			GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			gridBagConstraints21.insets = new Insets(0, 0, 0, 12);
			gridBagConstraints21.gridy = 0;
			gridBagConstraints21.gridx = 2;
			jPanel2 = new JPanel();
			jPanel2.setLayout(new GridBagLayout());
			jPanel2.add(getBtnOK(), gridBagConstraints21);
			jPanel2.add(getBtnCancel(), gridBagConstraints31);
			jPanel2.add(lblRightValue, gridBagConstraints23);
		}
		return jPanel2;
	}

	/**
	 * This method initializes btnOK	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnOK() {
		if (btnOK == null) {
			btnOK = new BButton();
			btnOK.setText("OK");
			btnOK.setPreferredSize(new Dimension(100, 29));
		}
		return btnOK;
	}

	/**
	 * This method initializes btnCancel	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnCancel() {
		if (btnCancel == null) {
			btnCancel = new BButton();
			btnCancel.setText("Cancel");
			btnCancel.setPreferredSize(new Dimension(100, 29));
		}
		return btnCancel;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel3() {
		if (jPanel3 == null) {
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.gridx = 3;
			gridBagConstraints8.weightx = 1.0D;
			gridBagConstraints8.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints8.gridy = 0;
			jLabel1 = new JLabel();
			jLabel1.setText(" ");
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.gridx = 2;
			gridBagConstraints7.gridy = 0;
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.gridx = 1;
			gridBagConstraints6.gridy = 0;
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.gridx = 0;
			gridBagConstraints5.gridy = 0;
			jPanel3 = new JPanel();
			jPanel3.setLayout(new GridBagLayout());
			jPanel3.setPreferredSize(new Dimension(206, 25));
			jPanel3.add(getChkTempo(), gridBagConstraints5);
			jPanel3.add(getChkBeat(), gridBagConstraints6);
			jPanel3.add(getChkLyric(), gridBagConstraints7);
			jPanel3.add(jLabel1, gridBagConstraints8);
		}
		return jPanel3;
	}

	/**
	 * This method initializes chkBeat	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkBeat() {
		if (chkBeat == null) {
			chkBeat = new BCheckBox();
			chkBeat.setText("Beat");
		}
		return chkBeat;
	}

	/**
	 * This method initializes chkLyric	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkLyric() {
		if (chkLyric == null) {
			chkLyric = new BCheckBox();
			chkLyric.setText("Lyrics");
		}
		return chkLyric;
	}

	/**
	 * This method initializes panel2	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanel2() {
		if (panel2 == null) {
			GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			gridBagConstraints81.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints81.gridy = 0;
			gridBagConstraints81.weightx = 1.0D;
			gridBagConstraints81.gridx = 3;
			jLabel11 = new JLabel();
			jLabel11.setText(" ");
			GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			gridBagConstraints61.gridx = 1;
			gridBagConstraints61.gridy = 0;
			GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			gridBagConstraints51.gridx = 0;
			gridBagConstraints51.gridy = 0;
			panel2 = new JPanel();
			panel2.setLayout(new GridBagLayout());
			panel2.setPreferredSize(new Dimension(219, 25));
			panel2.add(getChkNote(), gridBagConstraints51);
			panel2.add(getChkMetaText(), gridBagConstraints61);
			panel2.add(jLabel11, gridBagConstraints81);
		}
		return panel2;
	}

	/**
	 * This method initializes chkNote	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkNote() {
		if (chkNote == null) {
			chkNote = new BCheckBox();
			chkNote.setText("Note");
		}
		return chkNote;
	}

	/**
	 * This method initializes chkMetaText	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkMetaText() {
		if (chkMetaText == null) {
			chkMetaText = new BCheckBox();
			chkMetaText.setText("vocaloid meta-text");
		}
		return chkMetaText;
	}

	/**
	 * This method initializes jPanel32	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel32() {
		if (jPanel32 == null) {
			GridBagConstraints gridBagConstraints82 = new GridBagConstraints();
			gridBagConstraints82.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints82.gridy = 0;
			gridBagConstraints82.weightx = 1.0D;
			gridBagConstraints82.gridx = 3;
			jLabel12 = new JLabel();
			jLabel12.setText(" ");
			GridBagConstraints gridBagConstraints62 = new GridBagConstraints();
			gridBagConstraints62.gridx = 1;
			gridBagConstraints62.gridy = 0;
			GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			gridBagConstraints52.gridx = 0;
			gridBagConstraints52.gridy = 0;
			jPanel32 = new JPanel();
			jPanel32.setLayout(new GridBagLayout());
			jPanel32.setPreferredSize(new Dimension(315, 25));
			jPanel32.add(getChkExportVocaloidNrpn(), gridBagConstraints52);
			jPanel32.add(getChkPreMeasure(), gridBagConstraints62);
			jPanel32.add(jLabel12, gridBagConstraints82);
		}
		return jPanel32;
	}

	/**
	 * This method initializes chkExportVocaloidNrpn	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkExportVocaloidNrpn() {
		if (chkExportVocaloidNrpn == null) {
			chkExportVocaloidNrpn = new BCheckBox();
			chkExportVocaloidNrpn.setText("vocaloid NRPN");
		}
		return chkExportVocaloidNrpn;
	}

	/**
	 * This method initializes chkPreMeasure	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkPreMeasure() {
		if (chkPreMeasure == null) {
			chkPreMeasure = new BCheckBox();
			chkPreMeasure.setText("Export pre-measure part");
		}
		return chkPreMeasure;
	}

    /**
     * This method initializes groupMode	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupMode() {
        if (groupMode == null) {
            GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
            gridBagConstraints20.gridx = 0;
            gridBagConstraints20.anchor = GridBagConstraints.WEST;
            gridBagConstraints20.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints20.weightx = 1.0D;
            gridBagConstraints20.gridy = 2;
            GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
            gridBagConstraints16.gridx = 0;
            gridBagConstraints16.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints16.anchor = GridBagConstraints.WEST;
            gridBagConstraints16.weightx = 1.0D;
            gridBagConstraints16.gridy = 1;
            groupMode = new BGroupBox();
            groupMode.setLayout(new GridBagLayout());
            groupMode.setBorder(BorderFactory.createTitledBorder(null, "Import Basis", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
            groupMode.setPreferredSize(new Dimension(168, 80));
            groupMode.add(getPanel21(), gridBagConstraints16);
            groupMode.add(getJPanel31(), gridBagConstraints20);
        }
        return groupMode;
    }

    /**
     * This method initializes jPanel31	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel31() {
        if (jPanel31 == null) {
            GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
            gridBagConstraints19.gridx = 6;
            gridBagConstraints19.gridy = 1;
            lblOffsetUnit = new BLabel();
            lblOffsetUnit.setText("clocks");
            GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
            gridBagConstraints18.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints18.gridy = 1;
            gridBagConstraints18.weightx = 1.0;
            gridBagConstraints18.insets = new Insets(3, 3, 3, 3);
            gridBagConstraints18.gridx = 5;
            GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
            gridBagConstraints17.gridx = 4;
            gridBagConstraints17.gridy = 1;
            lblOffset = new BLabel();
            lblOffset.setText("offset");
            GridBagConstraints gridBagConstraints83 = new GridBagConstraints();
            gridBagConstraints83.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints83.gridy = 1;
            gridBagConstraints83.weightx = 1.0D;
            gridBagConstraints83.gridx = 7;
            jLabel13 = new JLabel();
            jLabel13.setText(" ");
            jPanel31 = new JPanel();
            jPanel31.setLayout(new GridBagLayout());
            jPanel31.add(jLabel13, gridBagConstraints83);
            jPanel31.add(lblOffset, gridBagConstraints17);
            jPanel31.add(getTxtOffset(), gridBagConstraints18);
            jPanel31.add(lblOffsetUnit, gridBagConstraints19);
        }
        return jPanel31;
    }

    /**
     * This method initializes panel21	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getPanel21() {
        if (panel21 == null) {
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.gridx = 3;
            gridBagConstraints15.gridy = 0;
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.gridx = 2;
            gridBagConstraints14.gridy = 0;
            GridBagConstraints gridBagConstraints811 = new GridBagConstraints();
            gridBagConstraints811.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints811.gridy = 0;
            gridBagConstraints811.weightx = 1.0D;
            gridBagConstraints811.gridx = 4;
            jLabel111 = new JLabel();
            jLabel111.setText(" ");
            panel21 = new JPanel();
            panel21.setLayout(new GridBagLayout());
            panel21.add(jLabel111, gridBagConstraints811);
            panel21.add(getRadioGateTime(), gridBagConstraints14);
            panel21.add(getRadioPlayTime(), gridBagConstraints15);
        }
        return panel21;
    }

    /**
     * This method initializes radioGateTime	
     * 	
     * @return javax.swing.JRadioButton	
     */
    private JRadioButton getRadioGateTime() {
        if (radioGateTime == null) {
            radioGateTime = new BRadioButton();
            radioGateTime.setText("gate-time");
            radioGateTime.setSelected(true);
        }
        return radioGateTime;
    }

    /**
     * This method initializes radioPlayTime	
     * 	
     * @return javax.swing.JRadioButton	
     */
    private JRadioButton getRadioPlayTime() {
        if (radioPlayTime == null) {
            radioPlayTime = new BRadioButton();
            radioPlayTime.setText("play-time");
        }
        return radioPlayTime;
    }

    /**
     * This method initializes txtOffset	
     * 	
     * @return javax.swing.JTextField	
     */
    private NumberTextBox getTxtOffset() {
        if (txtOffset == null) {
            txtOffset = new NumberTextBox();
        }
        return txtOffset;
    }

    /**
     * This method initializes jScrollPane	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane() {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setPreferredSize(new Dimension(100, 100));
            jScrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
            jScrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
            jScrollPane.setViewportView(getListTrack());
        }
        return jScrollPane;
    }

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
