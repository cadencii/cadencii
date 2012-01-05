package com.github.cadencii.ui;

import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;

import com.github.cadencii.FormBezierPointEditUi;
import com.github.cadencii.FormBezierPointEditUiListener;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.awt.event.ItemListener;
import java.awt.event.ItemEvent;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.MouseMotionAdapter;

//SECTION-END-IMPORT
public class FormBezierPointEditUiImpl extends DialogBase implements FormBezierPointEditUi
{
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel jContentPane = null;
    private JButton btnBackward = null;
    private JCheckBox chkEnableSmooth = null;
    private JButton btnForward = null;
    private JPanel groupLeft = null;
    private JLabel lblLeftClock = null;
    private JTextField txtLeftClock = null;
    private JLabel lblLeftValue = null;
    private JTextField txtLeftValue = null;
    private JButton btnLeft = null;
    private JPanel groupDataPoint = null;
    private JLabel lblDataPointClock = null;
    private JTextField txtDataPointClock = null;
    private JLabel lblDataPointValue = null;
    private JTextField txtDataPointValue = null;
    private JButton btnDataPoint = null;
    private JPanel groupRight = null;
    private JLabel lblRightClock = null;
    private JTextField txtRightClock = null;
    private JLabel lblRightValue = null;
    private JTextField txtRightValue = null;
    private JButton btnRight = null;
    private JButton btnOK = null;
    private JButton btnCancel = null;
    private JLabel jLabel4 = null;
    private JLabel jLabel5 = null;
    private JPanel jPanel3 = null;
    private JLabel lblRightValue1 = null;
    private FormBezierPointEditUiListener listener;

    //SECTION-END-FIELD
    /**
     * This is the default constructor
     */
    public FormBezierPointEditUiImpl( FormBezierPointEditUiListener listener )
    {
        super();
        this.listener = listener;
        initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     *
     * @return void
     */
    private void initialize() {
        this.setSize(517, 284);
        this.setContentPane(getJContentPane());
        this.setTitle("Edit Bezier Data Point");
        setCancelButton( btnCancel );
    }

    /**
     * This method initializes jContentPane
     *
     * @return javax.swing.JPanel
     */
    private JPanel getJContentPane() {
        if (jContentPane == null) {
            GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
            gridBagConstraints91.gridx = 0;
            gridBagConstraints91.gridwidth = 3;
            gridBagConstraints91.anchor = GridBagConstraints.EAST;
            gridBagConstraints91.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints91.gridy = 4;
            GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
            gridBagConstraints81.gridx = 0;
            gridBagConstraints81.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints81.gridwidth = 3;
            gridBagConstraints81.gridy = 3;
            jLabel5 = new JLabel();
            jLabel5.setText("    ");
            jLabel4 = new JLabel();
            jLabel4.setText("     ");
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 2;
            gridBagConstraints13.gridy = 1;
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 2;
            gridBagConstraints10.weightx = 1.0D;
            gridBagConstraints10.fill = GridBagConstraints.BOTH;
            gridBagConstraints10.insets = new Insets(5, 5, 5, 5);
            gridBagConstraints10.gridy = 2;
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.gridx = 1;
            gridBagConstraints9.weightx = 1.0D;
            gridBagConstraints9.fill = GridBagConstraints.BOTH;
            gridBagConstraints9.insets = new Insets(5, 5, 5, 5);
            gridBagConstraints9.gridy = 2;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 0;
            gridBagConstraints8.weightx = 1.0D;
            gridBagConstraints8.fill = GridBagConstraints.BOTH;
            gridBagConstraints8.insets = new Insets(5, 5, 5, 5);
            gridBagConstraints8.gridy = 2;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.insets = new Insets(10, 0, 0, 0);
            gridBagConstraints2.gridx = 2;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.gridy = 0;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.insets = new Insets(10, 0, 0, 0);
            gridBagConstraints1.gridx = 1;
            gridBagConstraints1.gridy = 0;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.insets = new Insets(10, 0, 0, 0);
            gridBagConstraints.gridx = 0;
            gridBagConstraints.anchor = GridBagConstraints.EAST;
            gridBagConstraints.gridy = 0;
            jContentPane = new JPanel();
            jContentPane.setLayout(new GridBagLayout());
            jContentPane.add(getBtnBackward(), gridBagConstraints);
            jContentPane.add(getChkEnableSmooth(), gridBagConstraints1);
            jContentPane.add(getBtnForward(), gridBagConstraints2);
            jContentPane.add(getGroupLeft(), gridBagConstraints8);
            jContentPane.add(getGroupDataPoint(), gridBagConstraints9);
            jContentPane.add(getGroupRight(), gridBagConstraints10);
            jContentPane.add(jLabel5, gridBagConstraints81);
            jContentPane.add(getJPanel3(), gridBagConstraints91);
            jContentPane.add(jLabel4, gridBagConstraints13);
        }
        return jContentPane;
    }

    /**
     * This method initializes btnBackward
     *
     * @return javax.swing.JButton
     */
    private JButton getBtnBackward() {
        if (btnBackward == null) {
            btnBackward = new JButton();
            btnBackward.addActionListener(new ActionListener() {
                public void actionPerformed(ActionEvent e) {
                    listener.buttonBackwardClick();
                }
            });
            btnBackward.setText("<<");
        }
        return btnBackward;
    }

    /**
     * This method initializes chkEnableSmooth
     *
     * @return javax.swing.JCheckBox
     */
    private JCheckBox getChkEnableSmooth() {
        if (chkEnableSmooth == null) {
            chkEnableSmooth = new JCheckBox();
            chkEnableSmooth.addItemListener(new ItemListener() {
                public void itemStateChanged(ItemEvent arg0) {
                    listener.checkboxEnableSmoothCheckedChanged();
                }
            });
            chkEnableSmooth.setText("Smooth");
        }
        return chkEnableSmooth;
    }

    /**
     * This method initializes btnForward
     *
     * @return javax.swing.JButton
     */
    private JButton getBtnForward() {
        if (btnForward == null) {
            btnForward = new JButton();
            btnForward.addActionListener(new ActionListener() {
                public void actionPerformed(ActionEvent e) {
                    listener.buttonForwardClick();
                }
            });
            btnForward.setText(">>");
        }
        return btnForward;
    }

    /**
     * This method initializes groupLeft
     *
     * @return javax.swing.JPanel
     */
    private JPanel getGroupLeft() {
        if (groupLeft == null) {
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.gridx = 0;
            gridBagConstraints7.gridwidth = 2;
            gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints7.weightx = 1.0D;
            gridBagConstraints7.ipadx = 0;
            gridBagConstraints7.ipady = 0;
            gridBagConstraints7.insets = new Insets(5, 20, 5, 20);
            gridBagConstraints7.gridy = 2;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints6.gridy = 1;
            gridBagConstraints6.weightx = 1.0;
            gridBagConstraints6.insets = new Insets(5, 5, 5, 15);
            gridBagConstraints6.gridx = 1;
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.insets = new Insets(0, 15, 0, 0);
            gridBagConstraints5.gridy = 1;
            lblLeftValue = new JLabel();
            lblLeftValue.setText("Value");
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints4.gridy = 0;
            gridBagConstraints4.weightx = 1.0D;
            gridBagConstraints4.insets = new Insets(5, 5, 5, 15);
            gridBagConstraints4.gridx = 1;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.insets = new Insets(0, 15, 0, 0);
            gridBagConstraints3.gridy = 0;
            lblLeftClock = new JLabel();
            lblLeftClock.setText("Clock");
            groupLeft = createGroupLeft();
            groupLeft.setLayout(new GridBagLayout());
            groupLeft.add(lblLeftClock, gridBagConstraints3);
            groupLeft.add(getJTextField(), gridBagConstraints4);
            groupLeft.add(lblLeftValue, gridBagConstraints5);
            groupLeft.add(getTxtLeftValue(), gridBagConstraints6);
            groupLeft.add(getBtnLeft(), gridBagConstraints7);
        }
        return groupLeft;
    }

    /**
     * This method initializes JTextField
     *
     * @return javax.swing.JTextField
     */
    private JTextField getJTextField() {
        if (txtLeftClock == null) {
            txtLeftClock = new JTextField();
        }
        return txtLeftClock;
    }

    /**
     * This method initializes txtLeftValue
     *
     * @return javax.swing.JTextField
     */
    private JTextField getTxtLeftValue() {
        if (txtLeftValue == null) {
            txtLeftValue = new JTextField();
        }
        return txtLeftValue;
    }

    /**
     * This method initializes btnLeft
     *
     * @return javax.swing.JButton
     */
    private JButton getBtnLeft() {
        if (btnLeft == null) {
            btnLeft = new JButton();
            btnLeft.addMouseMotionListener(new MouseMotionAdapter() {
                @Override
                public void mouseDragged(MouseEvent arg0) {
                    listener.buttonsMouseMove();
                }
            });
            btnLeft.addMouseListener(new MouseAdapter() {
                @Override
                public void mousePressed(MouseEvent arg0) {
                    listener.buttonLeftMouseDown();
                }
                @Override
                public void mouseReleased(MouseEvent e) {
                    listener.buttonsMouseUp();
                }
            });
            btnLeft.setText("");
            btnLeft.setIcon(new ImageIcon(FormBezierPointEditUiImpl.class.getResource("/resources/target--pencil.png")));
        }
        return btnLeft;
    }

    /**
     * This method initializes groupDataPoint
     *
     * @return javax.swing.JPanel
     */
    private JPanel getGroupDataPoint() {
        if (groupDataPoint == null) {
            GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
            gridBagConstraints71.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints71.gridx = 0;
            gridBagConstraints71.gridy = 2;
            gridBagConstraints71.weightx = 1.0D;
            gridBagConstraints71.insets = new Insets(5, 20, 5, 20);
            gridBagConstraints71.gridwidth = 2;
            GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
            gridBagConstraints61.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints61.gridy = 1;
            gridBagConstraints61.weightx = 1.0;
            gridBagConstraints61.insets = new Insets(5, 5, 5, 15);
            gridBagConstraints61.gridx = 1;
            GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
            gridBagConstraints51.gridx = 0;
            gridBagConstraints51.anchor = GridBagConstraints.WEST;
            gridBagConstraints51.insets = new Insets(0, 15, 0, 0);
            gridBagConstraints51.gridy = 1;
            lblDataPointValue = new JLabel();
            lblDataPointValue.setText("Value");
            GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
            gridBagConstraints41.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints41.gridy = 0;
            gridBagConstraints41.weightx = 1.0D;
            gridBagConstraints41.insets = new Insets(5, 5, 5, 15);
            gridBagConstraints41.gridx = 1;
            GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
            gridBagConstraints31.gridx = 0;
            gridBagConstraints31.insets = new Insets(0, 15, 0, 0);
            gridBagConstraints31.anchor = GridBagConstraints.WEST;
            gridBagConstraints31.gridy = 0;
            lblDataPointClock = new JLabel();
            lblDataPointClock.setText("Clock");
            groupDataPoint = createGroupDataPoint();
            groupDataPoint.setLayout(new GridBagLayout());
            groupDataPoint.add(lblDataPointClock, gridBagConstraints31);
            groupDataPoint.add(getTxtDataPointClock(), gridBagConstraints41);
            groupDataPoint.add(lblDataPointValue, gridBagConstraints51);
            groupDataPoint.add(getTxtDataPointValue(), gridBagConstraints61);
            groupDataPoint.add(getBtnDataPoint(), gridBagConstraints71);
        }
        return groupDataPoint;
    }

    /**
     * This method initializes txtDataPointClock
     *
     * @return javax.swing.JTextField
     */
    private JTextField getTxtDataPointClock() {
        if (txtDataPointClock == null) {
            txtDataPointClock = new JTextField();
        }
        return txtDataPointClock;
    }

    /**
     * This method initializes txtDataPointValue
     *
     * @return javax.swing.JTextField
     */
    private JTextField getTxtDataPointValue() {
        if (txtDataPointValue == null) {
            txtDataPointValue = new JTextField();
        }
        return txtDataPointValue;
    }

    /**
     * This method initializes btnDataPoint
     *
     * @return javax.swing.JButton
     */
    private JButton getBtnDataPoint() {
        if (btnDataPoint == null) {
            btnDataPoint = new JButton();
            btnDataPoint.addMouseMotionListener(new MouseMotionAdapter() {
                @Override
                public void mouseDragged(MouseEvent e) {
                    listener.buttonsMouseMove();
                }
            });
            btnDataPoint.addMouseListener(new MouseAdapter() {
                @Override
                public void mousePressed(MouseEvent e) {
                    listener.buttonCenterMouseDown();
                }
                @Override
                public void mouseReleased(MouseEvent e) {
                    listener.buttonsMouseUp();
                }
            });
            btnDataPoint.setText("");
            btnDataPoint.setIcon(new ImageIcon(FormBezierPointEditUiImpl.class.getResource("/resources/target--pencil.png")));
        }
        return btnDataPoint;
    }

    /**
     * This method initializes groupRight
     *
     * @return javax.swing.JPanel
     */
    private JPanel getGroupRight() {
        if (groupRight == null) {
            GridBagConstraints gridBagConstraints72 = new GridBagConstraints();
            gridBagConstraints72.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints72.gridx = 0;
            gridBagConstraints72.gridy = 2;
            gridBagConstraints72.weightx = 1.0D;
            gridBagConstraints72.insets = new Insets(5, 20, 5, 20);
            gridBagConstraints72.gridwidth = 2;
            GridBagConstraints gridBagConstraints62 = new GridBagConstraints();
            gridBagConstraints62.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints62.gridy = 1;
            gridBagConstraints62.weightx = 1.0;
            gridBagConstraints62.insets = new Insets(5, 5, 5, 15);
            gridBagConstraints62.gridx = 1;
            GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
            gridBagConstraints52.gridx = 0;
            gridBagConstraints52.insets = new Insets(0, 15, 0, 0);
            gridBagConstraints52.gridy = 1;
            lblRightValue = new JLabel();
            lblRightValue.setText("Value");
            GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
            gridBagConstraints42.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints42.gridy = 0;
            gridBagConstraints42.weightx = 1.0D;
            gridBagConstraints42.insets = new Insets(5, 5, 5, 15);
            gridBagConstraints42.gridx = 1;
            GridBagConstraints gridBagConstraints32 = new GridBagConstraints();
            gridBagConstraints32.gridx = 0;
            gridBagConstraints32.insets = new Insets(0, 15, 0, 0);
            gridBagConstraints32.gridy = 0;
            lblRightClock = new JLabel();
            lblRightClock.setText("Clock");
            groupRight = createGroupRight();
            groupRight.setLayout(new GridBagLayout());
            groupRight.add(lblRightClock, gridBagConstraints32);
            groupRight.add(getTxtRightClock(), gridBagConstraints42);
            groupRight.add(lblRightValue, gridBagConstraints52);
            groupRight.add(getTxtRightValue(), gridBagConstraints62);
            groupRight.add(getBtnRight(), gridBagConstraints72);
        }
        return groupRight;
    }

    /**
     * This method initializes txtRightClock
     *
     * @return javax.swing.JTextField
     */
    private JTextField getTxtRightClock() {
        if (txtRightClock == null) {
            txtRightClock = new JTextField();
        }
        return txtRightClock;
    }

    /**
     * This method initializes txtRightValue
     *
     * @return javax.swing.JTextField
     */
    private JTextField getTxtRightValue() {
        if (txtRightValue == null) {
            txtRightValue = new JTextField();
        }
        return txtRightValue;
    }

    /**
     * This method initializes btnRight
     *
     * @return javax.swing.JButton
     */
    private JButton getBtnRight() {
        if (btnRight == null) {
            btnRight = new JButton();
            btnRight.addMouseMotionListener(new MouseMotionAdapter() {
                @Override
                public void mouseDragged(MouseEvent e) {
                    listener.buttonsMouseMove();
                }
            });
            btnRight.addMouseListener(new MouseAdapter() {
                @Override
                public void mousePressed(MouseEvent e) {
                    listener.buttonRightMouseDown();
                }
                @Override
                public void mouseReleased(MouseEvent e) {
                    listener.buttonsMouseUp();
                }
            });
            btnRight.setText("");
            btnRight.setIcon(new ImageIcon(FormBezierPointEditUiImpl.class.getResource("/resources/target--pencil.png")));
        }
        return btnRight;
    }

    /**
     * This method initializes btnOK
     *
     * @return javax.swing.JButton
     */
    private JButton getBtnOK() {
        if (btnOK == null) {
            btnOK = new JButton();
            btnOK.addActionListener(new ActionListener() {
                public void actionPerformed(ActionEvent arg0) {
                    listener.buttonOkClick();
                }
            });
            btnOK.setText("OK");
            btnOK.setPreferredSize(new Dimension(100, 29));
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
            btnCancel.addActionListener(new ActionListener() {
                public void actionPerformed(ActionEvent e) {
                    listener.buttonCancelClick();
                }
            });
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
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.gridx = 0;
            gridBagConstraints14.weightx = 1.0D;
            gridBagConstraints14.gridy = 0;
            lblRightValue1 = new JLabel();
            lblRightValue1.setText("");
            lblRightValue1.setPreferredSize(new Dimension(4, 4));
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 2;
            gridBagConstraints11.insets = new Insets(0, 0, 0, 12);
            gridBagConstraints11.gridy = 0;
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.gridx = 1;
            gridBagConstraints12.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints12.gridy = 0;
            jPanel3 = new JPanel();
            jPanel3.setLayout(new GridBagLayout());
            jPanel3.add(getBtnCancel(), gridBagConstraints12);
            jPanel3.add(getBtnOK(), gridBagConstraints11);
            jPanel3.add(lblRightValue1, gridBagConstraints14);
        }
        return jPanel3;
    }

    public int showDialog( Object parent_form )
    {
        return super.doShowDialog( parent_form );
    }

    public String getDataPointClockText()
    {
        return this.txtDataPointClock.getText();
    }

    public String getDataPointValueText()
    {
        return this.txtDataPointValue.getText();
    }

    public String getLeftClockText()
    {
        return this.txtLeftClock.getText();
    }

    public String getLeftValueText()
    {
        return this.txtLeftValue.getText();
    }

    public String getRightClockText()
    {
        return this.txtRightClock.getText();
    }

    public String getRightValueText()
    {
        return this.txtRightValue.getText();
    }

    public boolean isEnableSmoothSelected()
    {
        return this.chkEnableSmooth.isSelected();
    }

    public void setEnableSmoothSelected( boolean value )
    {
        this.chkEnableSmooth.setSelected( value );
    }

    public void setLeftClockEnabled( boolean value )
    {
        this.txtLeftClock.setEnabled( value );
    }

    public void setLeftValueEnabled( boolean value )
    {
        this.txtLeftValue.setEnabled( value );
    }

    public void setLeftButtonEnabled( boolean value )
    {
        this.btnLeft.setEnabled( value );
    }

    public void setRightClockEnabled( boolean value )
    {
        this.txtRightClock.setEnabled( value );
    }

    public void setRightValueEnabled( boolean value )
    {
        this.txtRightValue.setEnabled( value );
    }

    public void setRightButtonEnabled( boolean value )
    {
        this.btnRight.setEnabled( value );
    }

    public void setLeftClockText( String value )
    {
        this.txtLeftClock.setText( value );
    }

    public void setLeftValueText( String value )
    {
        this.txtLeftValue.setText( value );
    }

    public void setRightClockText( String value )
    {
        this.txtRightClock.setText( value );
    }

    public void setRightValueText( String value )
    {
        this.txtRightValue.setText( value );
    }

    public void setDataPointClockText( String value )
    {
        this.txtDataPointClock.setText( value );
    }

    public void setDataPointValueText( String value )
    {
        this.txtDataPointValue.setText( value );
    }

    public void setGroupDataPointTitle( String value )
    {
        DialogBase.GroupBox.setTitle( this.groupDataPoint, value );
    }

    public void setLabelDataPointClockText( String value )
    {
        this.lblDataPointClock.setText( value );
    }

    public void setLabelDataPointValueText( String value )
    {
        this.lblDataPointValue.setText( value );
    }

    public void setGroupLeftTitle( String value )
    {
        DialogBase.GroupBox.setTitle( this.groupLeft, value );
    }

    public void setLabelLeftClockText( String value )
    {
        this.lblLeftClock.setText( value );
    }

    public void setLabelLeftValueText( String value )
    {
        this.lblLeftValue.setText( value );
    }

    public void setGroupRightTitle( String value )
    {
        DialogBase.GroupBox.setTitle( this.groupRight, value );
    }

    public void setLabelRightClockText( String value )
    {
        this.lblRightClock.setText( value );
    }

    public void setLabelRightValueText( String value )
    {
        this.lblRightValue.setText( value );
    }

    public void setCheckboxEnableSmoothText( String value )
    {
        this.chkEnableSmooth.setText( value );
    }

    public void setOpacity( double opacity )
    {
        if( opacity >= 1.0 ){
            this.setVisible( true );
        }
    }

    public void setDialogResult( boolean result )
    {
        super.setDialogResult( result );
    }

    public void close()
    {
        super.doClose();
    }

    /**
     * @return
     * @wbp.factory
     */
    static private JPanel createGroupLeft()
    {
        JPanel panel = DialogBase.GroupBox.create();
        DialogBase.GroupBox.setTitle( panel, "Left Control Point" );
        return panel;
    }

    /**
     * @return
     * @wbp.factory
     */
    static private JPanel createGroupDataPoint()
    {
        JPanel panel = DialogBase.GroupBox.create();
        DialogBase.GroupBox.setTitle( panel, "Data Point" );
        return panel;
    }

    /**
     * @return
     * @wbp.factory
     */
    static private JPanel createGroupRight()
    {
        JPanel panel = DialogBase.GroupBox.create();
        DialogBase.GroupBox.setTitle( panel, "Right Control Point" );
        return panel;
    }
    //SECTION-END-METHOD
}
