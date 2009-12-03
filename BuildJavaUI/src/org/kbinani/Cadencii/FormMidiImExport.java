package org.kbinani.Cadencii;

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
import javax.swing.border.TitledBorder;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BListView;

//SECTION-END-IMPORT
public class FormMidiImExport extends BForm {
    //SECTION-BEGIN-FIELD
	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private JPanel jPanel = null;
	private BButton btnCheckAll = null;
	private BButton btnUncheckAll = null;
	private JLabel jLabel = null;
	public BListView listTrack = null;
	private JPanel jPanel1 = null;
	private BCheckBox chkTempo = null;
	private JPanel jPanel2 = null;
	private BButton btnOK = null;
	private BButton btnCancel = null;
	private JPanel jPanel3 = null;
	private BCheckBox chkBeat = null;
	private BCheckBox chkLyric = null;
	private JLabel jLabel1 = null;
	private JPanel chkMetaText = null;
	private BCheckBox chkNote = null;
	private BCheckBox jCheckBox11 = null;
	private JLabel jLabel11 = null;
	private JPanel jPanel32 = null;
	private BCheckBox chkExportVocaloidNrpn = null;
	private BCheckBox chkPreMeasure = null;
	private JLabel jLabel12 = null;
	
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
		this.setSize(357, 488);
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
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 0;
			gridBagConstraints13.anchor = GridBagConstraints.EAST;
			gridBagConstraints13.insets = new Insets(12, 0, 12, 0);
			gridBagConstraints13.gridy = 3;
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.gridx = 0;
			gridBagConstraints12.weightx = 1.0D;
			gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints12.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints12.gridy = 2;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.fill = GridBagConstraints.BOTH;
			gridBagConstraints4.gridy = 1;
			gridBagConstraints4.weightx = 1.0;
			gridBagConstraints4.weighty = 1.0;
			gridBagConstraints4.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints4.gridx = 0;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.insets = new Insets(12, 12, 6, 12);
			gridBagConstraints3.gridy = 0;
			gridBagConstraints3.ipadx = 146;
			gridBagConstraints3.gridx = 0;
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(getJPanel(), gridBagConstraints3);
			jContentPane.add(getListTrack(), gridBagConstraints4);
			jContentPane.add(getJPanel1(), gridBagConstraints12);
			jContentPane.add(getJPanel2(), gridBagConstraints13);
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
			gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints2.insets = new Insets(0, 0, 0, 0);
			gridBagConstraints2.gridy = 0;
			jLabel = new JLabel();
			jLabel.setText(" ");
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.gridx = 1;
			gridBagConstraints1.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints1.gridy = 0;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.weighty = 0.0D;
			gridBagConstraints.fill = GridBagConstraints.NONE;
			gridBagConstraints.insets = new Insets(3, 3, 3, 3);
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
			btnCheckAll.setPreferredSize(new Dimension(87, 23));
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
			btnUncheckAll.setPreferredSize(new Dimension(101, 23));
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
	 * This method initializes jPanel1	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel1() {
		if (jPanel1 == null) {
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
			jPanel1 = new JPanel();
			jPanel1.setLayout(new GridBagLayout());
			jPanel1.setBorder(BorderFactory.createTitledBorder(null, "Option", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			jPanel1.add(getJPanel3(), gridBagConstraints9);
			jPanel1.add(getChkMetaText(), gridBagConstraints10);
			jPanel1.add(getJPanel32(), gridBagConstraints11);
		}
		return jPanel1;
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
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.insets = new Insets(0, 0, 0, 12);
			gridBagConstraints31.gridy = 0;
			gridBagConstraints31.gridx = 1;
			GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			gridBagConstraints21.insets = new Insets(0, 0, 0, 16);
			gridBagConstraints21.gridy = 0;
			gridBagConstraints21.gridx = 0;
			jPanel2 = new JPanel();
			jPanel2.setLayout(new GridBagLayout());
			jPanel2.add(getBtnOK(), gridBagConstraints21);
			jPanel2.add(getBtnCancel(), gridBagConstraints31);
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
	 * This method initializes chkMetaText	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getChkMetaText() {
		if (chkMetaText == null) {
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
			chkMetaText = new JPanel();
			chkMetaText.setLayout(new GridBagLayout());
			chkMetaText.add(getChkNote(), gridBagConstraints51);
			chkMetaText.add(getJCheckBox11(), gridBagConstraints61);
			chkMetaText.add(jLabel11, gridBagConstraints81);
		}
		return chkMetaText;
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
	 * This method initializes jCheckBox11	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getJCheckBox11() {
		if (jCheckBox11 == null) {
			jCheckBox11 = new BCheckBox();
			jCheckBox11.setText("vocaloid meta-text");
		}
		return jCheckBox11;
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

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
