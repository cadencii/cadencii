package com.github.cadencii.ui;

import com.github.cadencii.apputil.Messaging;
import com.github.cadencii.windows.forms.BDialog;
import com.github.cadencii.windows.forms.BDialogResult;
import java.awt.Component;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import javax.swing.JLabel;
import java.awt.GridBagConstraints;
import javax.swing.JProgressBar;
import javax.swing.JButton;
import javax.swing.JScrollPane;
import java.awt.Insets;
import javax.swing.BorderFactory;
import java.awt.FlowLayout;
import java.awt.Color;

public class FormWorkerUi extends BDialog
{
    private static final long serialVersionUID = 8662165288686369139L;
    private FormWorker mControl = null;
    private boolean mDetailVisible = true;
    private JPanel jPanel = null;
    private JLabel jLabel = null;
    private JProgressBar jProgressBar = null;
    private JButton buttonDetail = null;
    private JButton buttonCancel = null;
    private JScrollPane jScrollPane = null;
    private JPanel jPanel1 = null;
    private JLabel labelSpacer = null;
    /**
     * This method initializes
     *
     */
    private FormWorkerUi() {
    	super();
    	initialize();
    }

    public FormWorkerUi( FormWorker control )
    {
        this();
        mControl = control;
        updateDetail();
    }

    private void updateDetail() {
        if( mDetailVisible ){
            GridBagConstraints g = new GridBagConstraints();
            g.fill = GridBagConstraints.BOTH;
            g.gridwidth = 2;
            g.gridx = 0;
            g.gridy = 3;
            g.insets = new Insets( 0, 16, 16, 16 );
            g.weightx = 1.0D;
            g.weighty = 1.0D;
            getContentPane().add( jScrollPane, g );
            setSize( 494, 296 );
        }else{
            getContentPane().remove( jScrollPane );
            setSize( 494, 138 );
        }
    }

    public void applyLanguage()
    {
        buttonCancel.setText( _( "Cancel" ) );
        buttonDetail.setText( _( "detail" ) );
    }

    private static String _( String id )
    {
        return Messaging.getMessage( id );
    }

    public void repaint()
    {
        super.repaint();
        jScrollPane.revalidate();
    }

    public void close( boolean value )
    {
        if( value ){
            setDialogResult( BDialogResult.CANCEL );
        }else{
            setDialogResult( BDialogResult.OK );
        }
    }

    public void removeProgressBar( ProgressBarWithLabelUi ui )
    {
        jPanel1.remove( ui );
    }

    public void setTotalProgress( int value )
    {
        if( value < jProgressBar.getMinimum() ) value = jProgressBar.getMinimum();
        if( jProgressBar.getMaximum() < value ) value = jProgressBar.getMaximum();
        jProgressBar.setValue( value );
    }

    public void setText( String value )
    {
        jLabel.setText( value );
    }

    public boolean showDialog( FormMain main_form )
    {
        if( super.showDialog( main_form ) == BDialogResult.CANCEL ){
            return true;
        }else{
            return false;
        }
    }

    public void show( Component comp )
    {
        super.showDialog( comp );
    }

    public void addProgressBar( ProgressBarWithLabelUi ui )
    {
        jPanel1.remove( labelSpacer );
        int count = jPanel1.getComponentCount();
        GridBagConstraints g = new GridBagConstraints();
        g.fill = GridBagConstraints.HORIZONTAL;
        g.weightx = 1.0D;
        g.gridy = count;
        g.anchor = GridBagConstraints.NORTH;
        jPanel1.add( ui, g );
        g.fill = GridBagConstraints.NONE;
        g.weightx = 0.0D;
        g.weighty = 1.0D;
        g.gridy = count + 1;
        jPanel1.add( labelSpacer, g );
    }

    /**
     * This method initializes this
     *
     */
    private void initialize() {
        this.setSize(new Dimension(494, 296));
        this.setContentPane(getJPanel());

    }

    /**
     * This method initializes jPanel
     *
     * @return javax.swing.JPanel
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.fill = GridBagConstraints.BOTH;
            gridBagConstraints4.gridy = 3;
            gridBagConstraints4.weightx = 1.0;
            gridBagConstraints4.weighty = 1.0;
            gridBagConstraints4.gridwidth = 2;
            gridBagConstraints4.insets = new Insets(0, 16, 16, 16);
            gridBagConstraints4.gridx = 0;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 1;
            gridBagConstraints3.weightx = 0.5D;
            gridBagConstraints3.anchor = GridBagConstraints.EAST;
            gridBagConstraints3.insets = new Insets(0, 0, 4, 16);
            gridBagConstraints3.gridy = 2;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.weightx = 0.5D;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(0, 16, 4, 0);
            gridBagConstraints2.gridy = 2;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.gridwidth = 2;
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.insets = new Insets(8, 16, 4, 16);
            gridBagConstraints1.gridy = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridwidth = 2;
            gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints.insets = new Insets(24, 16, 8, 16);
            gridBagConstraints.gridy = 0;
            jLabel = new JLabel();
            jLabel.setText(" ");
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(jLabel, gridBagConstraints);
            jPanel.add(getJProgressBar(), gridBagConstraints1);
            jPanel.add(getButtonDetail(), gridBagConstraints2);
            jPanel.add(getButtonCancel(), gridBagConstraints3);
            jPanel.add(getJScrollPane(), gridBagConstraints4);
        }
        return jPanel;
    }

    /**
     * This method initializes jProgressBar
     *
     * @return javax.swing.JProgressBar
     */
    private JProgressBar getJProgressBar() {
        if (jProgressBar == null) {
            jProgressBar = new JProgressBar();
        }
        return jProgressBar;
    }

    /**
     * This method initializes buttonDetail
     *
     * @return javax.swing.JButton
     */
    private JButton getButtonDetail() {
        if (buttonDetail == null) {
            buttonDetail = new JButton();
            buttonDetail.setText("detail");
            buttonDetail.setPreferredSize(new Dimension(100, 29));
            buttonDetail.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    mDetailVisible = !mDetailVisible;
                    updateDetail();
                }
            });
        }
        return buttonDetail;
    }

    /**
     * This method initializes buttonCancel
     *
     * @return javax.swing.JButton
     */
    private JButton getButtonCancel() {
        if (buttonCancel == null) {
            buttonCancel = new JButton();
            buttonCancel.setText("Cancel");
            buttonCancel.setPreferredSize(new Dimension(100, 29));
            buttonCancel.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    mControl.cancelJobSlot();
                    setDialogResult( BDialogResult.CANCEL );
                }
            });
        }
        return buttonCancel;
    }

    /**
     * This method initializes jScrollPane
     *
     * @return javax.swing.JScrollPane
     */
    private JScrollPane getJScrollPane() {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
            jScrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
            jScrollPane.setViewportBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jScrollPane.setBorder(BorderFactory.createLineBorder(Color.gray, 1));
            jScrollPane.setViewportView(getJPanel1());
        }
        return jScrollPane;
    }

    /**
     * This method initializes jPanel1
     *
     * @return javax.swing.JPanel
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.weighty = 1.0D;
            gridBagConstraints5.gridy = 0;
            labelSpacer = new JLabel();
            labelSpacer.setText(" ");
            labelSpacer.setPreferredSize(new Dimension(4, 4));
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jPanel1.add(labelSpacer, gridBagConstraints5);
        }
        return jPanel1;
    }
}  //  @jve:decl-index=0:visual-constraint="10,10"
