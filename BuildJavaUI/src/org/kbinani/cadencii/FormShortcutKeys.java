package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BComboBox;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BListView;
import javax.swing.JScrollPane;
import org.kbinani.windows.forms.BCheckBox;

//SECTION-END-IMPORT
public class FormShortcutKeys extends BDialog {
    //SECTION-BEGIN-FIELD
 
    private static final long serialVersionUID = 2743132471603994391L;
    private JPanel jPanel = null;
    private JPanel jPanel3 = null;
    private BButton btnLoadDefault = null;
    private BButton btnRevert = null;
    private JPanel jPanel31 = null;
    private BButton btnCancel = null;
    private BButton btnOK = null;
    private BListView list = null;
    private BLabel labelCategory = null;
    private BComboBox comboCategory = null;
    private JScrollPane jScrollPane = null;
    private BComboBox comboEditKey = null;
    private BLabel labelCommand = null;
    private BLabel labelEdit = null;
    private JPanel panelEdit = null;
    private BLabel labelEditKey = null;
    private BLabel labelEditModifier = null;
    private BCheckBox checkCommand = null;
    private BCheckBox checkShift = null;
    private BCheckBox checkControl = null;
    private BCheckBox checkOption = null;
    private BLabel labelEditKey1 = null;
    private BLabel labelEditKey11 = null;
    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormShortcutKeys() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(541, 572));
        this.setTitle("Shortcut Config");
        this.setContentPane(getJPanel());
        setCancelButton( btnCancel );
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
            gridBagConstraints41.gridx = 0;
            gridBagConstraints41.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints41.insets = new Insets(3, 36, 3, 12);
            gridBagConstraints41.gridy = 5;
            GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
            gridBagConstraints31.gridx = 0;
            gridBagConstraints31.insets = new Insets(3, 12, 3, 12);
            gridBagConstraints31.anchor = GridBagConstraints.WEST;
            gridBagConstraints31.gridy = 4;
            labelEdit = new BLabel();
            labelEdit.setText("Edit");
            GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
            gridBagConstraints21.gridx = 0;
            gridBagConstraints21.insets = new Insets(6, 12, 3, 12);
            gridBagConstraints21.anchor = GridBagConstraints.WEST;
            gridBagConstraints21.gridy = 2;
            labelCommand = new BLabel();
            labelCommand.setText("Command");
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.fill = GridBagConstraints.BOTH;
            gridBagConstraints11.gridy = 3;
            gridBagConstraints11.weightx = 1.0;
            gridBagConstraints11.weighty = 1.0;
            gridBagConstraints11.insets = new Insets(3, 36, 6, 17);
            gridBagConstraints11.anchor = GridBagConstraints.NORTHWEST;
            gridBagConstraints11.gridx = 0;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints3.gridy = 1;
            gridBagConstraints3.weightx = 1.0;
            gridBagConstraints3.insets = new Insets(3, 33, 3, 12);
            gridBagConstraints3.weighty = 0.0D;
            gridBagConstraints3.gridx = 0;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.insets = new Insets(12, 12, 3, 12);
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.gridy = 0;
            labelCategory = new BLabel();
            labelCategory.setText("Category");
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.anchor = GridBagConstraints.EAST;
            gridBagConstraints2.insets = new Insets(0, 0, 16, 12);
            gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints2.gridy = 7;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.anchor = GridBagConstraints.WEST;
            gridBagConstraints1.insets = new Insets(24, 10, 12, 0);
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.gridy = 6;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(labelCategory, gridBagConstraints);
            jPanel.add(getComboCategory(), gridBagConstraints3);
            jPanel.add(labelCommand, gridBagConstraints21);
            jPanel.add(getJScrollPane(), gridBagConstraints11);
            jPanel.add(getJPanel3(), gridBagConstraints1);
            jPanel.add(getJPanel31(), gridBagConstraints2);
            jPanel.add(labelEdit, gridBagConstraints31);
            jPanel.add(getPanelEdit(), gridBagConstraints41);
        }
        return jPanel;
    }

    /**
     * This method initializes jPanel3	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel3() {
        if (jPanel3 == null) {
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.gridx = 3;
            gridBagConstraints12.fill = GridBagConstraints.NONE;
            gridBagConstraints12.weightx = 1.0D;
            gridBagConstraints12.gridy = 0;
            labelEditKey1 = new BLabel();
            labelEditKey1.setText("");
            labelEditKey1.setPreferredSize(new Dimension(4, 4));
            GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
            gridBagConstraints111.insets = new Insets(0, 0, 0, 16);
            gridBagConstraints111.gridy = 0;
            gridBagConstraints111.anchor = GridBagConstraints.WEST;
            gridBagConstraints111.gridx = 0;
            GridBagConstraints gridBagConstraints121 = new GridBagConstraints();
            gridBagConstraints121.insets = new Insets(0, 0, 0, 16);
            gridBagConstraints121.gridy = 0;
            gridBagConstraints121.anchor = GridBagConstraints.WEST;
            gridBagConstraints121.gridx = 2;
            jPanel3 = new JPanel();
            jPanel3.setLayout(new GridBagLayout());
            jPanel3.setPreferredSize(new Dimension(239, 40));
            jPanel3.add(getBtnLoadDefault(), gridBagConstraints121);
            jPanel3.add(getBtnRevert(), gridBagConstraints111);
            jPanel3.add(labelEditKey1, gridBagConstraints12);
        }
        return jPanel3;
    }

    /**
     * This method initializes btnLoadDefault	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnLoadDefault() {
        if (btnLoadDefault == null) {
            btnLoadDefault = new BButton();
            btnLoadDefault.setText("Load Default");
        }
        return btnLoadDefault;
    }

    /**
     * This method initializes btnRevert	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnRevert() {
        if (btnRevert == null) {
            btnRevert = new BButton();
            btnRevert.setText("Revert");
        }
        return btnRevert;
    }

    /**
     * This method initializes jPanel31	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel31() {
        if (jPanel31 == null) {
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 0;
            gridBagConstraints13.weightx = 1.0D;
            gridBagConstraints13.gridy = 0;
            labelEditKey11 = new BLabel();
            labelEditKey11.setPreferredSize(new Dimension(4, 4));
            labelEditKey11.setText("");
            GridBagConstraints gridBagConstraints1111 = new GridBagConstraints();
            gridBagConstraints1111.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints1111.gridy = 0;
            gridBagConstraints1111.gridx = 2;
            GridBagConstraints gridBagConstraints1211 = new GridBagConstraints();
            gridBagConstraints1211.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints1211.gridy = 0;
            gridBagConstraints1211.fill = GridBagConstraints.NONE;
            gridBagConstraints1211.gridx = 1;
            jPanel31 = new JPanel();
            jPanel31.setLayout(new GridBagLayout());
            jPanel31.setPreferredSize(new Dimension(220, 40));
            jPanel31.add(getBtnCancel(), gridBagConstraints1211);
            jPanel31.add(getBtnOK(), gridBagConstraints1111);
            jPanel31.add(labelEditKey11, gridBagConstraints13);
        }
        return jPanel31;
    }

    /**
     * This method initializes btnCancel	
     * 	
     * @return org.kbinani.windows.forms.BButton	
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
     * This method initializes btnOK	
     * 	
     * @return org.kbinani.windows.forms.BButton	
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
     * This method initializes list	
     * 	
     * @return javax.swing.JTable	
     */
    private BListView getList() {
        if (list == null) {
            list = new BListView();
            list.setCheckBoxes(false);
            list.setShowGrid(false);
        }
        return list;
    }

    /**
     * This method initializes comboCategory	
     * 	
     * @return org.kbinani.windows.forms.BComboBox	
     */
    private BComboBox getComboCategory() {
        if (comboCategory == null) {
            comboCategory = new BComboBox();
            comboCategory.setPreferredSize(new Dimension(167, 27));
            comboCategory.setMaximumRowCount(10);
        }
        return comboCategory;
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
            jScrollPane.setViewportView(getList());
        }
        return jScrollPane;
    }

    /**
     * This method initializes comboEditKey	
     * 	
     * @return org.kbinani.windows.forms.BComboBox	
     */
    private BComboBox getComboEditKey() {
        if (comboEditKey == null) {
            comboEditKey = new BComboBox();
            comboEditKey.setPreferredSize(new Dimension(167, 27));
        }
        return comboEditKey;
    }

    /**
     * This method initializes panelEdit	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getPanelEdit() {
        if (panelEdit == null) {
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 4;
            gridBagConstraints10.anchor = GridBagConstraints.WEST;
            gridBagConstraints10.gridy = 1;
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.gridx = 3;
            gridBagConstraints9.anchor = GridBagConstraints.WEST;
            gridBagConstraints9.gridy = 1;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 2;
            gridBagConstraints8.anchor = GridBagConstraints.WEST;
            gridBagConstraints8.gridy = 1;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.gridx = 1;
            gridBagConstraints7.insets = new Insets(0, 6, 0, 0);
            gridBagConstraints7.anchor = GridBagConstraints.WEST;
            gridBagConstraints7.gridy = 1;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.anchor = GridBagConstraints.WEST;
            gridBagConstraints6.gridy = 1;
            labelEditModifier = new BLabel();
            labelEditModifier.setText("Modifier:");
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints5.gridx = 1;
            gridBagConstraints5.gridy = 0;
            gridBagConstraints5.weightx = 1.0;
            gridBagConstraints5.gridwidth = 4;
            gridBagConstraints5.insets = new Insets(3, 6, 3, 0);
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.anchor = GridBagConstraints.WEST;
            gridBagConstraints4.gridy = 0;
            labelEditKey = new BLabel();
            labelEditKey.setText("Key:");
            panelEdit = new JPanel();
            panelEdit.setLayout(new GridBagLayout());
            panelEdit.add(labelEditKey, gridBagConstraints4);
            panelEdit.add(getComboEditKey(), gridBagConstraints5);
            panelEdit.add(labelEditModifier, gridBagConstraints6);
            panelEdit.add(getCheckCommand(), gridBagConstraints7);
            panelEdit.add(getCheckShift(), gridBagConstraints8);
            panelEdit.add(getCheckControl(), gridBagConstraints9);
            panelEdit.add(getCheckOption(), gridBagConstraints10);
        }
        return panelEdit;
    }

    /**
     * This method initializes checkCommand	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getCheckCommand() {
        if (checkCommand == null) {
            checkCommand = new BCheckBox();
            checkCommand.setText("command");
        }
        return checkCommand;
    }

    /**
     * This method initializes checkShift	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getCheckShift() {
        if (checkShift == null) {
            checkShift = new BCheckBox();
            checkShift.setText("shift");
        }
        return checkShift;
    }

    /**
     * This method initializes checkControl	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getCheckControl() {
        if (checkControl == null) {
            checkControl = new BCheckBox();
            checkControl.setText("control");
        }
        return checkControl;
    }

    /**
     * This method initializes checkOption	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getCheckOption() {
        if (checkOption == null) {
            checkOption = new BCheckBox();
            checkOption.setText("option");
        }
        return checkOption;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
