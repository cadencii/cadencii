package com.github.cadencii.ui;

import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.ListSelectionModel;

import com.github.cadencii.FormWordDictionaryUi;
import com.github.cadencii.FormWordDictionaryUiListener;

public class FormWordDictionaryUiImpl extends DialogBase implements FormWordDictionaryUi
{
    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private JLabel lblAvailableDictionaries = null;
    private JPanel jPanel2 = null;
    private JButton btnOK = null;
    private JButton btnCancel = null;
    private JPanel jPanel21 = null;
    private JButton btnUp = null;
    private JButton btnDown = null;
    private ListView listDictionaries = null;
    private JScrollPane jScrollPane = null;
    private JLabel lblSpacer = null;
    private JLabel lblSpacer1 = null;

    /**
     * This method initializes
     *
     */
    public FormWordDictionaryUiImpl( FormWordDictionaryUiListener listener )
    {
        super();
        initialize();
    }

    /**
     * This method initializes this
     *
     */
    private void initialize()
    {
        this.setSize( new Dimension( 327, 404 ) );
        this.setTitle( "User Dictionary Configuration" );
        this.setContentPane( getJPanel() );
        setCancelButton( btnCancel );
    }

    /**
     * This method initializes jPanel
     *
     * @return javax.swing.JPanel
     */
    private JPanel getJPanel()
    {
        if( jPanel == null ){
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.fill = GridBagConstraints.BOTH;
            gridBagConstraints2.gridy = 1;
            gridBagConstraints2.weightx = 1.0;
            gridBagConstraints2.weighty = 1.0;
            gridBagConstraints2.insets = new Insets( 6, 12, 6, 12 );
            gridBagConstraints2.gridx = 0;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.anchor = GridBagConstraints.EAST;
            gridBagConstraints4.insets = new Insets( 6, 0, 12, 12 );
            gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints4.gridy = 3;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.anchor = GridBagConstraints.EAST;
            gridBagConstraints3.insets = new Insets( 6, 0, 6, 12 );
            gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints3.gridy = 2;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.insets = new Insets( 12, 12, 6, 0 );
            gridBagConstraints.gridy = 0;
            lblAvailableDictionaries = new JLabel();
            lblAvailableDictionaries.setText( "Available Dictionaries" );
            jPanel = new JPanel();
            jPanel.setLayout( new GridBagLayout() );
            jPanel.add( lblAvailableDictionaries, gridBagConstraints );
            jPanel.add( getJPanel21(), gridBagConstraints3 );
            jPanel.add( getJPanel2(), gridBagConstraints4 );
            jPanel.add( getJScrollPane(), gridBagConstraints2 );
        }
        return jPanel;
    }

    /**
     * This method initializes jPanel2
     *
     * @return org.kbinani.windows.forms.JPanel
     */
    private JPanel getJPanel2()
    {
        if( jPanel2 == null ){
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.weightx = 1.0D;
            gridBagConstraints5.gridy = 0;
            lblSpacer1 = new JLabel();
            lblSpacer1.setPreferredSize( new Dimension( 4, 4 ) );
            lblSpacer1.setText( "" );
            GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
            gridBagConstraints52.anchor = GridBagConstraints.SOUTHWEST;
            gridBagConstraints52.gridx = 1;
            gridBagConstraints52.gridy = 0;
            gridBagConstraints52.insets = new Insets( 0, 0, 0, 0 );
            GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
            gridBagConstraints42.anchor = GridBagConstraints.WEST;
            gridBagConstraints42.gridx = 2;
            gridBagConstraints42.gridy = 0;
            gridBagConstraints42.insets = new Insets( 0, 0, 0, 0 );
            jPanel2 = new JPanel();
            jPanel2.setLayout( new GridBagLayout() );
            jPanel2.add( getBtnOK(), gridBagConstraints42 );
            jPanel2.add( getBtnCancel(), gridBagConstraints52 );
            jPanel2.add( lblSpacer1, gridBagConstraints5 );
        }
        return jPanel2;
    }

    /**
     * This method initializes btnOK
     *
     * @return org.kbinani.windows.forms.JButton
     */
    private JButton getBtnOK()
    {
        if( btnOK == null ){
            btnOK = new JButton();
            btnOK.setText( "OK" );
            btnOK.setPreferredSize( new Dimension( 100, 29 ) );
        }
        return btnOK;
    }

    /**
     * This method initializes btnCancel
     *
     * @return org.kbinani.windows.forms.JButton
     */
    private JButton getBtnCancel()
    {
        if( btnCancel == null ){
            btnCancel = new JButton();
            btnCancel.setText( "Cancel" );
            btnCancel.setPreferredSize( new Dimension( 100, 29 ) );
        }
        return btnCancel;
    }

    /**
     * This method initializes jPanel21
     *
     * @return org.kbinani.windows.forms.JPanel
     */
    private JPanel getJPanel21()
    {
        if( jPanel21 == null ){
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.gridy = 0;
            lblSpacer = new JLabel();
            lblSpacer.setPreferredSize( new Dimension( 4, 4 ) );
            lblSpacer.setText( "" );
            GridBagConstraints gridBagConstraints521 = new GridBagConstraints();
            gridBagConstraints521.anchor = GridBagConstraints.SOUTHWEST;
            gridBagConstraints521.gridx = 2;
            gridBagConstraints521.gridy = 0;
            gridBagConstraints521.insets = new Insets( 0, 0, 0, 0 );
            GridBagConstraints gridBagConstraints421 = new GridBagConstraints();
            gridBagConstraints421.anchor = GridBagConstraints.WEST;
            gridBagConstraints421.gridx = 1;
            gridBagConstraints421.gridy = 0;
            gridBagConstraints421.insets = new Insets( 0, 0, 0, 16 );
            jPanel21 = new JPanel();
            jPanel21.setLayout( new GridBagLayout() );
            jPanel21.add( getBtnUp(), gridBagConstraints421 );
            jPanel21.add( getBtnDown(), gridBagConstraints521 );
            jPanel21.add( lblSpacer, gridBagConstraints1 );
        }
        return jPanel21;
    }

    /**
     * This method initializes btnUp
     *
     * @return org.kbinani.windows.forms.JButton
     */
    private JButton getBtnUp()
    {
        if( btnUp == null ){
            btnUp = new JButton();
            btnUp.setText( "Up" );
            btnUp.setPreferredSize( new Dimension( 75, 29 ) );
        }
        return btnUp;
    }

    /**
     * This method initializes btnDown
     *
     * @return org.kbinani.windows.forms.JButton
     */
    private JButton getBtnDown()
    {
        if( btnDown == null ){
            btnDown = new JButton();
            btnDown.setText( "Down" );
            btnDown.setPreferredSize( new Dimension( 75, 29 ) );
        }
        return btnDown;
    }

    /**
     * This method initializes listDictionaries
     *
     * @return javax.swing.JPanel
     */
    private ListView getListDictionaries()
    {
        if( listDictionaries == null ){
            listDictionaries = new ListView();
            listDictionaries.setSelectionMode( ListSelectionModel.SINGLE_SELECTION );
            listDictionaries.setRowSelectionAllowed( true );
        }
        return listDictionaries;
    }

    /**
     * This method initializes jScrollPane
     *
     * @return javax.swing.JScrollPane
     */
    private JScrollPane getJScrollPane()
    {
        if( jScrollPane == null ){
            jScrollPane = new JScrollPane();
            jScrollPane.setPreferredSize( new Dimension( 100, 100 ) );
            jScrollPane.setHorizontalScrollBarPolicy( JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS );
            jScrollPane.setVerticalScrollBarPolicy( JScrollPane.VERTICAL_SCROLLBAR_ALWAYS );
            jScrollPane.setViewportBorder( BorderFactory.createEmptyBorder( 0, 0, 0, 0 ) );
            jScrollPane.setViewportView( getListDictionaries() );
        }
        return jScrollPane;
    }

    public int showDialog( Object parent_form )
    {
        return super.doShowDialog( parent_form );
    }

    public void setDialogResult( boolean value )
    {
        super.setDialogResult( value );
    }

    public int listDictionariesGetSelectedRow()
    {
        return this.listDictionaries.getSelectedRow();
    }

    public int listDictionariesGetItemCountRow()
    {
        return this.listDictionaries.getItemCountRow();
    }

    public void listDictionariesClear()
    {
        this.listDictionaries.clear();
    }

    public String listDictionariesGetItemAt( int row )
    {
        return this.listDictionaries.getItemAt( row, 0 );
    }

    public boolean listDictionariesIsRowChecked( int row )
    {
        return this.listDictionaries.isRowChecked( row );
    }

    public void listDictionariesSetItemAt( int row, String value )
    {
        this.listDictionaries.setItemAt( row, 0, value );
    }

    public void listDictionariesSetRowChecked( int row, boolean value )
    {
        this.listDictionaries.setRowChecked( row, value );
    }

    public void listDictionariesSetSelectedRow( int row )
    {
        this.listDictionaries.setSelectedRow( row );
    }

    public void listDictionariesClearSelection()
    {
        this.listDictionaries.clearSelection();
    }

    public void listDictionariesAddRow( String value, boolean selected )
    {
        this.listDictionaries.addRow( new String[]{ value }, selected );
    }

    public void labelAvailableDictionariesSetText( String value )
    {
        this.lblAvailableDictionaries.setText( value );
    }

    public void buttonOkSetText( String value )
    {
        this.btnOK.setText( value );
    }

    public void buttonCancelSetText( String value )
    {
        this.btnCancel.setText( value );
    }

    public void buttonUpSetText( String value )
    {
        this.btnUp.setText( value );
    }

    public void buttonDownSetText( String value )
    {
        this.btnDown.setText( value );
    }

    public void close()
    {
        super.doClose();
    }
}
