package org.kbinani.cadencii;
//SECTION-BEGIN-IMPORT

import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.BorderFactory;
import javax.swing.JPanel;
import javax.swing.SwingConstants;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BListBox;
import org.kbinani.windows.forms.BPictureBox;
import org.kbinani.windows.forms.BTextBox;

//SECTION-END-IMPORT
public class FormVibratoPreset extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 5210609912644248288L;
    private JPanel jPanel1 = null;
    private JPanel jPanel3 = null;
    private BButton buttonCancel = null;
    private BButton buttonOk = null;
    private JPanel jPanel2 = null;
    private BButton buttonAdd = null;
    private BButton buttonDown = null;
    private BButton buttonRemove = null;
    private BButton buttonUp = null;
    private BGroupBox groupEdit = null;
    private BListBox listPresets = null;
    private JPanel jPanel = null;
    private BGroupBox groupPreview = null;
    private BLabel labelPresets = null;
    private BLabel labelRate = null;
    private BLabel labelDepth = null;
    private BLabel labelName = null;
    private BTextBox textRate = null;
    private BTextBox textDepth = null;
    private BTextBox textName = null;
    private JPanel jPanel5 = null;
    private BLabel labelResulting = null;
    private BPictureBox pictureResulting = null;
    private JPanel jPanel51 = null;
    private BLabel labelRateCurve = null;
    private BPictureBox pictureRate = null;
    private JPanel jPanel52 = null;
    private BLabel labelDepthCurve = null;
    private BPictureBox pictureDepth = null;
    private BLabel lblSpacer = null;

    //SECTION-END-FIELD
    public FormVibratoPreset() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    private void initialize() {
        this.setSize(new Dimension(502, 427));
        this.setTitle("Randomize");
        this.setContentPane(getJPanel1());
    		
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 1;
            gridBagConstraints8.fill = GridBagConstraints.BOTH;
            gridBagConstraints8.weighty = 1.0D;
            gridBagConstraints8.weightx = 1.0D;
            gridBagConstraints8.insets = new Insets(3, 12, 0, 12);
            gridBagConstraints8.gridy = 1;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.gridx = 1;
            gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints7.weighty = 0.0D;
            gridBagConstraints7.weightx = 1.0D;
            gridBagConstraints7.insets = new Insets(12, 12, 3, 12);
            gridBagConstraints7.gridy = 0;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.weighty = 1.0D;
            gridBagConstraints6.weightx = 0.0D;
            gridBagConstraints6.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints6.gridheight = 2;
            gridBagConstraints6.insets = new Insets(12, 12, 0, 0);
            gridBagConstraints6.gridy = 0;
            GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
            gridBagConstraints20.gridy = 2;
            gridBagConstraints20.gridheight = 1;
            gridBagConstraints20.gridwidth = 2;
            gridBagConstraints20.gridx = 0;
            gridBagConstraints20.gridx = 0;
            gridBagConstraints20.gridwidth = 3;
            gridBagConstraints20.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints20.anchor = GridBagConstraints.EAST;
            gridBagConstraints20.weightx = 0.0D;
            gridBagConstraints20.insets = new Insets(16, 0, 16, 12);
            gridBagConstraints20.weighty = 0.0D;
            gridBagConstraints20.gridy = 6;
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getJPanel(), gridBagConstraints6);
            jPanel1.add(getJPanel3(), gridBagConstraints20);
            jPanel1.add(getGroupEdit(), gridBagConstraints7);
            jPanel1.add(getGroupPreview(), gridBagConstraints8);
        }
        return jPanel1;
    }

    /**
     * This method initializes jPanel3	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel3() {
        if (jPanel3 == null) {
            GridBagConstraints gridBagConstraints22 = new GridBagConstraints();
            gridBagConstraints22.gridx = 0;
            gridBagConstraints22.weightx = 1.0D;
            gridBagConstraints22.gridy = 0;
            lblSpacer = new BLabel();
            lblSpacer.setPreferredSize(new Dimension(4, 4));
            lblSpacer.setText("");
            GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
            gridBagConstraints111.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints111.gridy = 0;
            gridBagConstraints111.gridx = 2;
            GridBagConstraints gridBagConstraints1211 = new GridBagConstraints();
            gridBagConstraints1211.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints1211.gridy = 0;
            gridBagConstraints1211.gridx = 1;
            jPanel3 = new JPanel();
            jPanel3.setLayout(new GridBagLayout());
            jPanel3.add(getButtonCancel(), gridBagConstraints1211);
            jPanel3.add(getButtonOk(), gridBagConstraints111);
            jPanel3.add(lblSpacer, gridBagConstraints22);
        }
        return jPanel3;
    }

    /**
     * This method initializes buttonCancel	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getButtonCancel() {
        if (buttonCancel == null) {
            buttonCancel = new BButton();
            buttonCancel.setText("Cancel");
            buttonCancel.setPreferredSize(new Dimension(100, 29));
        }
        return buttonCancel;
    }

    /**
     * This method initializes buttonOk	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getButtonOk() {
        if (buttonOk == null) {
            buttonOk = new BButton();
            buttonOk.setText("OK");
            buttonOk.setPreferredSize(new Dimension(100, 29));
        }
        return buttonOk;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel2() {
        if (jPanel2 == null) {
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 1;
            gridBagConstraints4.insets = new Insets(0, 3, 9, 0);
            gridBagConstraints4.anchor = GridBagConstraints.NORTHEAST;
            gridBagConstraints4.gridy = 0;
            gridBagConstraints4.gridx = 1;
            gridBagConstraints4.anchor = GridBagConstraints.NORTHEAST;
            gridBagConstraints4.insets = new Insets(0, 9, 2, 0);
            gridBagConstraints4.gridy = 0;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.insets = new Insets(3, 0, 0, 9);
            gridBagConstraints3.anchor = GridBagConstraints.SOUTHWEST;
            gridBagConstraints3.gridy = 2;
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.anchor = GridBagConstraints.SOUTHWEST;
            gridBagConstraints3.insets = new Insets(2, 0, 0, 9);
            gridBagConstraints3.gridy = 1;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 1;
            gridBagConstraints2.insets = new Insets(3, 9, 0, 0);
            gridBagConstraints2.anchor = GridBagConstraints.SOUTHEAST;
            gridBagConstraints2.gridy = 2;
            gridBagConstraints2.gridx = 1;
            gridBagConstraints2.anchor = GridBagConstraints.SOUTHEAST;
            gridBagConstraints2.insets = new Insets(2, 9, 0, 0);
            gridBagConstraints2.gridy = 1;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.insets = new Insets(0, 0, 3, 9);
            gridBagConstraints1.anchor = GridBagConstraints.NORTHWEST;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.anchor = GridBagConstraints.NORTHWEST;
            gridBagConstraints1.insets = new Insets(0, 0, 2, 9);
            gridBagConstraints1.gridy = 0;
            jPanel2 = new JPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.add(getButtonAdd(), gridBagConstraints1);
            jPanel2.add(getButtonDown(), gridBagConstraints2);
            jPanel2.add(getButtonRemove(), gridBagConstraints3);
            jPanel2.add(getButtonUp(), gridBagConstraints4);
        }
        return jPanel2;
    }

    /**
     * This method initializes buttonAdd	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getButtonAdd() {
        if (buttonAdd == null) {
            buttonAdd = new BButton();
            buttonAdd.setText("Add");
            buttonAdd.setVerticalAlignment(SwingConstants.CENTER);
            buttonAdd.setHorizontalAlignment(SwingConstants.CENTER);
            buttonAdd.setPreferredSize(new Dimension(75, 29));
        }
        return buttonAdd;
    }

    /**
     * This method initializes buttonDown	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getButtonDown() {
        if (buttonDown == null) {
            buttonDown = new BButton();
            buttonDown.setText("Down");
            buttonDown.setHorizontalAlignment(SwingConstants.CENTER);
            buttonDown.setPreferredSize(new Dimension(75, 29));
        }
        return buttonDown;
    }

    /**
     * This method initializes buttonRemove	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getButtonRemove() {
        if (buttonRemove == null) {
            buttonRemove = new BButton();
            buttonRemove.setText("Remove");
            buttonRemove.setHorizontalAlignment(SwingConstants.CENTER);
            buttonRemove.setPreferredSize(new Dimension(75, 29));
        }
        return buttonRemove;
    }

    /**
     * This method initializes buttonUp	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getButtonUp() {
        if (buttonUp == null) {
            buttonUp = new BButton();
            buttonUp.setText("Up");
            buttonUp.setHorizontalAlignment(SwingConstants.CENTER);
            buttonUp.setPreferredSize(new Dimension(75, 29));
        }
        return buttonUp;
    }

    /**
     * This method initializes groupEdit	
     * 	
     * @return javax.swing.JPanel	
     */
    private BGroupBox getGroupEdit() {
        if (groupEdit == null) {
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.fill = GridBagConstraints.NONE;
            gridBagConstraints14.gridy = 2;
            gridBagConstraints14.weightx = 1.0;
            gridBagConstraints14.anchor = GridBagConstraints.WEST;
            gridBagConstraints14.insets = new Insets(3, 24, 0, 0);
            gridBagConstraints14.gridx = 1;
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.fill = GridBagConstraints.NONE;
            gridBagConstraints13.gridy = 1;
            gridBagConstraints13.weightx = 1.0;
            gridBagConstraints13.anchor = GridBagConstraints.WEST;
            gridBagConstraints13.insets = new Insets(3, 24, 3, 0);
            gridBagConstraints13.gridx = 1;
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.fill = GridBagConstraints.NONE;
            gridBagConstraints12.gridy = 0;
            gridBagConstraints12.weightx = 1.0;
            gridBagConstraints12.anchor = GridBagConstraints.WEST;
            gridBagConstraints12.insets = new Insets(0, 24, 3, 0);
            gridBagConstraints12.gridx = 1;
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 0;
            gridBagConstraints11.anchor = GridBagConstraints.WEST;
            gridBagConstraints11.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints11.gridy = 2;
            labelName = new BLabel();
            labelName.setText("Name");
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 0;
            gridBagConstraints10.anchor = GridBagConstraints.WEST;
            gridBagConstraints10.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints10.gridy = 1;
            labelDepth = new BLabel();
            labelDepth.setText("Start depth");
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.gridx = 0;
            gridBagConstraints9.anchor = GridBagConstraints.WEST;
            gridBagConstraints9.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints9.gridy = 0;
            labelRate = new BLabel();
            labelRate.setText("Start rate");
            groupEdit = new BGroupBox();
            groupEdit.setLayout(new GridBagLayout());
            groupEdit.setTitle("Edit");
            groupEdit.add(labelRate, gridBagConstraints9);
            groupEdit.add(labelDepth, gridBagConstraints10);
            groupEdit.add(labelName, gridBagConstraints11);
            groupEdit.add(getTextRate(), gridBagConstraints12);
            groupEdit.add(getTextDepth(), gridBagConstraints13);
            groupEdit.add(getTextName(), gridBagConstraints14);
        }
        return groupEdit;
    }

    /**
     * This method initializes listPresets	
     * 	
     * @return javax.swing.JList	
     */
    private BListBox getListPresets() {
        if (listPresets == null) {
            listPresets = new BListBox();
        }
        return listPresets;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
            gridBagConstraints21.gridx = 0;
            gridBagConstraints21.anchor = GridBagConstraints.NORTHWEST;
            gridBagConstraints21.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints21.gridy = 0;
            labelPresets = new BLabel();
            labelPresets.setText("List of preset vibrato");
            labelPresets.setHorizontalAlignment(SwingConstants.LEFT);
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.weightx = 1.0D;
            gridBagConstraints5.gridy = 2;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.fill = GridBagConstraints.BOTH;
            gridBagConstraints.gridy = 1;
            gridBagConstraints.weightx = 1.0;
            gridBagConstraints.weighty = 1.0;
            gridBagConstraints.insets = new Insets(3, 0, 6, 0);
            gridBagConstraints.gridx = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(labelPresets, gridBagConstraints21);
            jPanel.add(getListPresets(), gridBagConstraints);
            jPanel.add(getJPanel2(), gridBagConstraints5);
        }
        return jPanel;
    }

    /**
     * This method initializes groupPreview	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupPreview() {
        if (groupPreview == null) {
            GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
            gridBagConstraints19.gridx = 0;
            gridBagConstraints19.gridwidth = 2;
            gridBagConstraints19.weightx = 1.0D;
            gridBagConstraints19.weighty = 1.0D;
            gridBagConstraints19.fill = GridBagConstraints.BOTH;
            gridBagConstraints19.insets = new Insets(6, 12, 0, 12);
            gridBagConstraints19.gridy = 1;
            GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
            gridBagConstraints18.gridx = 1;
            gridBagConstraints18.weightx = 0.5D;
            gridBagConstraints18.weighty = 1.0D;
            gridBagConstraints18.fill = GridBagConstraints.BOTH;
            gridBagConstraints18.insets = new Insets(0, 6, 6, 12);
            gridBagConstraints18.gridy = 0;
            GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
            gridBagConstraints17.gridx = 0;
            gridBagConstraints17.weightx = 0.5D;
            gridBagConstraints17.weighty = 1.0D;
            gridBagConstraints17.fill = GridBagConstraints.BOTH;
            gridBagConstraints17.insets = new Insets(0, 12, 6, 6);
            gridBagConstraints17.gridy = 0;
            groupPreview = new BGroupBox();
            groupPreview.setLayout(new GridBagLayout());
            groupPreview.setTitle("Preview");
            groupPreview.add(getJPanel51(), gridBagConstraints17);
            groupPreview.add(getJPanel52(), gridBagConstraints18);
            groupPreview.add(getJPanel5(), gridBagConstraints19);
        }
        return groupPreview;
    }

    /**
     * This method initializes textRate	
     * 	
     * @return javax.swing.JTextField	
     */
    private BTextBox getTextRate() {
        if (textRate == null) {
            textRate = new BTextBox();
            textRate.setPreferredSize(new Dimension(72, 19));
        }
        return textRate;
    }

    /**
     * This method initializes textDepth	
     * 	
     * @return javax.swing.JTextField	
     */
    private BTextBox getTextDepth() {
        if (textDepth == null) {
            textDepth = new BTextBox();
            textDepth.setPreferredSize(new Dimension(72, 19));
        }
        return textDepth;
    }

    /**
     * This method initializes textName	
     * 	
     * @return javax.swing.JTextField	
     */
    private BTextBox getTextName() {
        if (textName == null) {
            textName = new BTextBox();
            textName.setPreferredSize(new Dimension(169, 19));
        }
        return textName;
    }

    /**
     * This method initializes jPanel5	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel5() {
        if (jPanel5 == null) {
            GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
            gridBagConstraints16.gridx = 0;
            gridBagConstraints16.fill = GridBagConstraints.BOTH;
            gridBagConstraints16.weightx = 1.0D;
            gridBagConstraints16.weighty = 1.0D;
            gridBagConstraints16.gridy = 1;
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.gridx = 0;
            gridBagConstraints15.anchor = GridBagConstraints.WEST;
            gridBagConstraints15.insets = new Insets(0, 0, 3, 0);
            gridBagConstraints15.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints15.gridy = 0;
            labelResulting = new BLabel();
            labelResulting.setText("Resulting pitch bend");
            jPanel5 = new JPanel();
            jPanel5.setLayout(new GridBagLayout());
            jPanel5.add(labelResulting, gridBagConstraints15);
            jPanel5.add(getPictureResulting(), gridBagConstraints16);
        }
        return jPanel5;
    }

    /**
     * This method initializes pictureResulting	
     * 	
     * @return javax.swing.JButton	
     */
    private BPictureBox getPictureResulting() {
        if (pictureResulting == null) {
            pictureResulting = new BPictureBox();
            pictureResulting.setBorder(BorderFactory.createLineBorder(Color.gray, 1));
        }
        return pictureResulting;
    }

    /**
     * This method initializes jPanel51	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel51() {
        if (jPanel51 == null) {
            GridBagConstraints gridBagConstraints161 = new GridBagConstraints();
            gridBagConstraints161.gridx = 0;
            gridBagConstraints161.weightx = 1.0D;
            gridBagConstraints161.weighty = 1.0D;
            gridBagConstraints161.fill = GridBagConstraints.BOTH;
            gridBagConstraints161.gridy = 1;
            GridBagConstraints gridBagConstraints151 = new GridBagConstraints();
            gridBagConstraints151.gridx = 0;
            gridBagConstraints151.anchor = GridBagConstraints.WEST;
            gridBagConstraints151.insets = new Insets(0, 0, 3, 0);
            gridBagConstraints151.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints151.gridy = 0;
            labelRateCurve = new BLabel();
            labelRateCurve.setText("Rate curve");
            jPanel51 = new JPanel();
            jPanel51.setLayout(new GridBagLayout());
            jPanel51.add(labelRateCurve, gridBagConstraints151);
            jPanel51.add(getPictureRate(), gridBagConstraints161);
        }
        return jPanel51;
    }

    /**
     * This method initializes pictureRate	
     * 	
     * @return javax.swing.JButton	
     */
    private BPictureBox getPictureRate() {
        if (pictureRate == null) {
            pictureRate = new BPictureBox();
            pictureRate.setBorder(BorderFactory.createLineBorder(Color.gray, 1));
        }
        return pictureRate;
    }

    /**
     * This method initializes jPanel52	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel52() {
        if (jPanel52 == null) {
            GridBagConstraints gridBagConstraints162 = new GridBagConstraints();
            gridBagConstraints162.gridx = 0;
            gridBagConstraints162.weightx = 1.0D;
            gridBagConstraints162.weighty = 1.0D;
            gridBagConstraints162.fill = GridBagConstraints.BOTH;
            gridBagConstraints162.gridy = 1;
            GridBagConstraints gridBagConstraints152 = new GridBagConstraints();
            gridBagConstraints152.gridx = 0;
            gridBagConstraints152.anchor = GridBagConstraints.WEST;
            gridBagConstraints152.insets = new Insets(0, 0, 3, 0);
            gridBagConstraints152.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints152.gridy = 0;
            labelDepthCurve = new BLabel();
            labelDepthCurve.setText("Depth curve");
            jPanel52 = new JPanel();
            jPanel52.setLayout(new GridBagLayout());
            jPanel52.add(labelDepthCurve, gridBagConstraints152);
            jPanel52.add(getPictureDepth(), gridBagConstraints162);
        }
        return jPanel52;
    }

    /**
     * This method initializes pictureDepth	
     * 	
     * @return javax.swing.JButton	
     */
    private BPictureBox getPictureDepth() {
        if (pictureDepth == null) {
            pictureDepth = new BPictureBox();
            pictureDepth.setBorder(BorderFactory.createLineBorder(Color.gray, 1));
        }
        return pictureDepth;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="25,10"
