package org.kbinani.cadencii;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JComboBox;
import javax.swing.JLabel;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BDialogResult;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BNumericUpDown;

public class FormBeatConfigUiImpl extends BDialog implements FormBeatConfigUi
{
    private static final long serialVersionUID = 4414859292940722020L;
    private FormBeatConfigUiListener listener;
    private JPanel jContentPane = null;
    private BGroupBox groupPosition = null;
    private JLabel lblStart = null;
    private BNumericUpDown numStart = null;
    private JLabel lblBar1 = null;
    private JCheckBox chkEnd = null;
    private BNumericUpDown numEnd = null;
    private JLabel lblBar2 = null;
    private BGroupBox groupBeat = null;
    private BNumericUpDown numNumerator = null;
    private JLabel jLabel = null;
    private JLabel jLabel1 = null;
    private JComboBox comboDenominator = null;
    private JPanel jPanel1 = null;
    private JLabel jLabel2 = null;
    private JButton btnOK = null;
    private JButton btnCancel = null;
    private JLabel jLabel4 = null;
    private JLabel jLabel7 = null;
    private JLabel jLabel8 = null;

    public FormBeatConfigUiImpl( FormBeatConfigUiListener l ){
        super();
        listener = l;
        initialize();
    }
    
    /**
     * This method initializes this
     * 
     * @return void
     */
    private void initialize() {
        this.setTitle("Beat Change");
        this.setSize(314, 287);
        this.setContentPane(getJContentPane());
        this.setTitle("JFrame");
        setCancelButton( btnCancel );
    }

    /**
     * This method initializes jContentPane
     * 
     * @return javax.swing.JPanel
     */
    private JPanel getJContentPane() {
        if (jContentPane == null) {
            GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
            gridBagConstraints18.insets = new Insets(0, 0, 1, 12);
            gridBagConstraints18.gridy = 2;
            gridBagConstraints18.ipadx = 180;
            gridBagConstraints18.ipady = 42;
            gridBagConstraints18.weightx = 1.0D;
            gridBagConstraints18.weighty = 0.0D;
            gridBagConstraints18.anchor = GridBagConstraints.NORTH;
            gridBagConstraints18.fill = GridBagConstraints.BOTH;
            gridBagConstraints18.gridx = 0;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 0;
            gridBagConstraints8.ipadx = 141;
            gridBagConstraints8.ipady = 42;
            gridBagConstraints8.fill = GridBagConstraints.BOTH;
            gridBagConstraints8.insets = new Insets(0, 12, 0, 12);
            gridBagConstraints8.weightx = 1.0D;
            gridBagConstraints8.anchor = GridBagConstraints.NORTH;
            gridBagConstraints8.weighty = 0.5D;
            gridBagConstraints8.gridy = 1;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.ipadx = 147;
            gridBagConstraints6.ipady = 41;
            gridBagConstraints6.insets = new Insets(12, 12, 12, 12);
            gridBagConstraints6.fill = GridBagConstraints.BOTH;
            gridBagConstraints6.weightx = 1.0D;
            gridBagConstraints6.anchor = GridBagConstraints.NORTH;
            gridBagConstraints6.weighty = 0.5D;
            gridBagConstraints6.gridy = 0;
            jContentPane = new JPanel();
            jContentPane.setLayout(new GridBagLayout());
            jContentPane.add(getGroupPosition(), gridBagConstraints6);
            jContentPane.add(getGroupBeat(), gridBagConstraints8);
            jContentPane.add(getBPanel1(), gridBagConstraints18);
        }
        return jContentPane;
    }

    /**
     * This method initializes groupPosition    
     *  
     * @return javax.swing.BPanel   
     */
    private BGroupBox getGroupPosition() {
        if (groupPosition == null) {
            GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
            gridBagConstraints61.gridx = 3;
            gridBagConstraints61.gridy = 2;
            jLabel8 = new JLabel();
            jLabel8.setText("     ");
            GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
            gridBagConstraints51.gridx = 3;
            gridBagConstraints51.gridy = 0;
            jLabel7 = new JLabel();
            jLabel7.setText("     ");
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints9.gridy = -1;
            gridBagConstraints9.weightx = 1.0;
            gridBagConstraints9.gridx = -1;
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 2;
            gridBagConstraints5.anchor = GridBagConstraints.WEST;
            gridBagConstraints5.insets = new Insets(0, 9, 0, 0);
            gridBagConstraints5.gridy = 2;
            lblBar2 = new JLabel();
            lblBar2.setText("Beat");
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints4.gridy = 2;
            gridBagConstraints4.weightx = 1.0;
            gridBagConstraints4.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints4.gridx = 1;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.insets = new Insets(0, 16, 0, 0);
            gridBagConstraints3.gridy = 2;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 2;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(0, 9, 0, 0);
            gridBagConstraints2.gridy = 0;
            lblBar1 = new JLabel();
            lblBar1.setText("Measure");
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 0.0D;
            gridBagConstraints1.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints1.gridx = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.insets = new Insets(0, 16, 0, 0);
            gridBagConstraints.gridy = 0;
            lblStart = new JLabel();
            lblStart.setText("From");
            groupPosition = new BGroupBox();
            groupPosition.setLayout(new GridBagLayout());
            groupPosition.setTitle("Position");
            groupPosition.add(lblStart, gridBagConstraints);
            groupPosition.add(getNumStart(), gridBagConstraints1);
            groupPosition.add(lblBar1, gridBagConstraints2);
            groupPosition.add(getChkEnd(), gridBagConstraints3);
            groupPosition.add(getNumEnd(), gridBagConstraints4);
            groupPosition.add(lblBar2, gridBagConstraints5);
            groupPosition.add(jLabel7, gridBagConstraints51);
            groupPosition.add(jLabel8, gridBagConstraints61);
        }
        return groupPosition;
    }

    /**
     * This method initializes numStart 
     *  
     * @return javax.swing.BComboBox    
     */
    private BNumericUpDown getNumStart() {
        if (numStart == null) {
            numStart = new BNumericUpDown();
            numStart.setPreferredSize(new Dimension(31, 29));
        }
        return numStart;
    }

    /**
     * This method initializes chkEnd   
     *  
     * @return javax.swing.JCheckBox    
     */
    private JCheckBox getChkEnd() {
        if (chkEnd == null) {
            chkEnd = new JCheckBox();
            chkEnd.setText("To");
            chkEnd.addItemListener( new java.awt.event.ItemListener()
            {
                public void itemStateChanged( java.awt.event.ItemEvent e )
                {
                    if( listener != null ){
                        listener.checkboxEndCheckedChangedSlot();
                    }
                }
            } );
        }
        return chkEnd;
    }

    /**
     * This method initializes numEnd   
     *  
     * @return javax.swing.BComboBox    
     */
    private BNumericUpDown getNumEnd() {
        if (numEnd == null) {
            numEnd = new BNumericUpDown();
            numEnd.setPreferredSize(new Dimension(31, 29));
            numEnd.setEnabled(false);
        }
        return numEnd;
    }

    /**
     * This method initializes groupBeat    
     *  
     * @return javax.swing.BPanel   
     */
    private BGroupBox getGroupBeat() {
        if (groupBeat == null) {
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 4;
            gridBagConstraints13.gridy = 0;
            jLabel2 = new JLabel();
            jLabel2.setText("     ");
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints12.gridy = 0;
            gridBagConstraints12.weightx = 0.5D;
            gridBagConstraints12.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints12.gridx = 3;
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 2;
            gridBagConstraints11.gridy = 0;
            jLabel1 = new JLabel();
            jLabel1.setText(" /    ");
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 1;
            gridBagConstraints10.gridy = 0;
            jLabel = new JLabel();
            jLabel.setText(" (1-255) ");
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints7.gridy = 0;
            gridBagConstraints7.weightx = 0.5D;
            gridBagConstraints7.insets = new Insets(3, 16, 3, 0);
            gridBagConstraints7.gridx = 0;
            groupBeat = new BGroupBox();
            groupBeat.setLayout(new GridBagLayout());
            groupBeat.setTitle("Position");
            groupBeat.add(getNumNumerator(), gridBagConstraints7);
            groupBeat.add(jLabel, gridBagConstraints10);
            groupBeat.add(jLabel1, gridBagConstraints11);
            groupBeat.add(getComboDenominator(), gridBagConstraints12);
            groupBeat.add(jLabel2, gridBagConstraints13);
        }
        return groupBeat;
    }

    /**
     * This method initializes numNumerator 
     *  
     * @return javax.swing.BComboBox    
     */
    private BNumericUpDown getNumNumerator() {
        if (numNumerator == null) {
            numNumerator = new BNumericUpDown();
            numNumerator.setPreferredSize(new Dimension(31, 29));
        }
        return numNumerator;
    }

    /**
     * This method initializes comboDenominator   
     *  
     * @return javax.swing.JComboBox    
     */
    private JComboBox getComboDenominator() {
        if (comboDenominator == null) {
            comboDenominator = new JComboBox();
            comboDenominator.setPreferredSize(new Dimension(31, 27));
        }
        return comboDenominator;
    }

    /**
     * This method initializes jPanel1  
     *  
     * @return javax.swing.JPanel   
     */
    private JPanel getBPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
            gridBagConstraints17.gridx = 0;
            gridBagConstraints17.fill = GridBagConstraints.BOTH;
            gridBagConstraints17.weightx = 1.0D;
            gridBagConstraints17.gridy = 0;
            jLabel4 = new JLabel();
            jLabel4.setText(" ");
            GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
            gridBagConstraints16.gridx = 1;
            gridBagConstraints16.anchor = GridBagConstraints.EAST;
            gridBagConstraints16.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints16.gridy = 0;
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.gridx = 2;
            gridBagConstraints15.anchor = GridBagConstraints.EAST;
            gridBagConstraints15.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints15.gridy = 0;
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getBtnOk(), gridBagConstraints15);
            jPanel1.add(getBtnCancel(), gridBagConstraints16);
            jPanel1.add(jLabel4, gridBagConstraints17);
        }
        return jPanel1;
    }

    /**
     * This method initializes btnOk    
     *  
     * @return javax.swing.JButton  
     */
    private JButton getBtnOk() {
        if (btnOK == null) {
            btnOK = new JButton();
            btnOK.setText("OK");
            btnOK.setPreferredSize(new Dimension(100, 29));
            btnOK.addActionListener( new java.awt.event.ActionListener()
            {
                public void actionPerformed( java.awt.event.ActionEvent e )
                {
                    if( listener != null ){
                        listener.buttonOkClickedSlot();
                    }
                }
            } );
        }
        return btnOK;
    }

    /**
     * This method initializes btnCancel    
     *  
     * @return javax.swing.JButton  
     */
    private JButton getBtnCancel() {
        if (btnCancel == null) {
            btnCancel = new JButton();
            btnCancel.setText("Cancel");
            btnCancel.addActionListener( new java.awt.event.ActionListener()
            {
                public void actionPerformed( java.awt.event.ActionEvent e )
                {
                    if( listener != null ){
                        listener.buttonCancelClickedSlot();
                    }
                }
            } );
            btnCancel.setPreferredSize(new Dimension(100, 29));
        }
        return btnCancel;
    }

    @Override
    public int showDialog( Object parent_form )
    {
        if( parent_form == null ){
            return 0;
        }
        if( !(parent_form instanceof Component) ){
            return 0;
        }
        Component parent = (Component)parent_form;
        BDialogResult ret = super.showDialog( parent );
        if( ret == BDialogResult.OK || ret == BDialogResult.YES )
        {
            return 1;
        }else{
            return 0;
        }
    }

    @Override
    public void setFont( String fontName, float fontSize )
    {
        super.setFont( new Font( fontName, Font.PLAIN, (int)fontSize ) );
    }

    @Override
    public void setTitle( String value )
    {
        super.setTitle( value );
    }

    @Override
    public void setDialogResult( boolean value )
    {
        BDialogResult res = BDialogResult.CANCEL;
        if( value ){
            res = BDialogResult.OK;
        }
        super.setDialogResult( res );
    }

    @Override
    public void setLocation( int x, int y )
    {
        super.setLocation( x, y );
    }

    @Override
    public int getWidth()
    {
        return super.getWidth();
    }

    @Override
    public int getHeight()
    {
        return super.getHeight();
    }

    @Override
    public void close()
    {
        super.close();
    }

    @Override
    public void setTextBar1Label( String value )
    {
        this.lblBar1.setText( value );
    }

    @Override
    public void setTextBar2Label( String value )
    {
        this.lblBar2.setText( value );
    }

    @Override
    public void setTextStartLabel( String value )
    {
        this.lblStart.setText( value );
    }

    @Override
    public void setTextOkButton( String value )
    {
        this.btnOK.setText( value );
    }

    @Override
    public void setTextCancelButton( String value )
    {
        this.btnCancel.setText( value );
    }

    @Override
    public void setTextBeatGroup( String value )
    {
        this.groupBeat.setTitle( value );
    }

    @Override
    public void setTextPositionGroup( String value )
    {
        this.groupPosition.setTitle( value );
    }

    @Override
    public void setEnabledStartNum( boolean value )
    {
        this.numStart.setEnabled( value );
    }

    @Override
    public void setMinimumStartNum( int value )
    {
        this.numStart.setMinimum( value );
    }

    @Override
    public void setMaximumStartNum( int value )
    {
        this.numStart.setMaximum( value );
    }

    @Override
    public int getMaximumStartNum()
    {
        return (int)this.numStart.getMaximum();
    }

    @Override
    public int getMinimumStartNum()
    {
        return (int)this.numStart.getMinimum();
    }

    @Override
    public void setValueStartNum( int value )
    {
        this.numStart.setFloatValue( value );
    }

    @Override
    public int getValueStartNum()
    {
        return (int)this.numStart.getFloatValue();
    }

    @Override
    public void setEnabledEndNum( boolean value )
    {
        this.numEnd.setEnabled( value );
    }

    @Override
    public void setMinimumEndNum( int value )
    {
        this.numEnd.setMinimum( value );
    }

    @Override
    public void setMaximumEndNum( int value )
    {
        this.numEnd.setMaximum( value );
    }

    @Override
    public int getMaximumEndNum()
    {
        return (int)this.numEnd.getMaximum();
    }

    @Override
    public int getMinimumEndNum()
    {
        return (int)this.numEnd.getMinimum();
    }

    @Override
    public void setValueEndNum( int value )
    {
        this.numEnd.setFloatValue( value );
    }

    @Override
    public int getValueEndNum()
    {
        return (int)this.numEnd.getFloatValue();
    }

    @Override
    public boolean isCheckedEndCheckbox()
    {
        return this.chkEnd.isSelected();
    }

    @Override
    public void setEnabledEndCheckbox( boolean value )
    {
        this.chkEnd.setEnabled( value );
    }

    @Override
    public boolean isEnabledEndCheckbox()
    {
        return this.chkEnd.isEnabled();
    }

    @Override
    public void setTextEndCheckbox( String value )
    {
        this.chkEnd.setText( value );
    }

    @Override
    public void removeAllItemsDenominatorCombobox()
    {
        this.comboDenominator.removeAllItems();
    }

    @Override
    public void addItemDenominatorCombobox( String value )
    {
        this.comboDenominator.addItem( value );
    }

    @Override
    public void setSelectedIndexDenominatorCombobox( int value )
    {
        this.comboDenominator.setSelectedIndex( value );
    }

    @Override
    public int getSelectedIndexDenominatorCombobox()
    {
        return this.comboDenominator.getSelectedIndex();
    }

    @Override
    public int getMaximumNumeratorNum()
    {
        return (int)this.numNumerator.getMaximum();
    }

    @Override
    public int getMinimumNumeratorNum()
    {
        return (int)this.numNumerator.getMinimum();
    }

    @Override
    public void setValueNumeratorNum( int value )
    {
        this.numNumerator.setFloatValue( value );
    }

    @Override
    public int getValueNumeratorNum()
    {
        return (int)this.numNumerator.getFloatValue();
    }

}
