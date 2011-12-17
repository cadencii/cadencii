package org.kbinani.cadencii;

import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.BorderFactory;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.ListSelectionModel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BDialogResult;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BListView;
import org.kbinani.windows.forms.BPanel;

public class FormWordDictionaryUiImpl extends BDialog implements FormWordDictionaryUi
{
    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private BLabel lblAvailableDictionaries = null;
    private BPanel jPanel2 = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;
    private BPanel jPanel21 = null;
    private BButton btnUp = null;
    private BButton btnDown = null;
    private BListView listDictionaries = null;
    private JScrollPane jScrollPane = null;
    private BLabel lblSpacer = null;
    private BLabel lblSpacer1 = null;
    private FormWordDictionaryUiListener listener;

    /**
     * This method initializes
     * 
     */
    public FormWordDictionaryUiImpl( FormWordDictionaryUiListener listener )
    {
        super();
        this.listener = listener;
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
            lblAvailableDictionaries = new BLabel();
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
     * @return org.kbinani.windows.forms.BPanel
     */
    private BPanel getJPanel2()
    {
        if( jPanel2 == null ){
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.weightx = 1.0D;
            gridBagConstraints5.gridy = 0;
            lblSpacer1 = new BLabel();
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
            jPanel2 = new BPanel();
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
     * @return org.kbinani.windows.forms.BButton
     */
    private BButton getBtnOK()
    {
        if( btnOK == null ){
            btnOK = new BButton();
            btnOK.setText( "OK" );
            btnOK.setPreferredSize( new Dimension( 100, 29 ) );
        }
        return btnOK;
    }

    /**
     * This method initializes btnCancel
     * 
     * @return org.kbinani.windows.forms.BButton
     */
    private BButton getBtnCancel()
    {
        if( btnCancel == null ){
            btnCancel = new BButton();
            btnCancel.setText( "Cancel" );
            btnCancel.setPreferredSize( new Dimension( 100, 29 ) );
        }
        return btnCancel;
    }

    /**
     * This method initializes jPanel21
     * 
     * @return org.kbinani.windows.forms.BPanel
     */
    private BPanel getJPanel21()
    {
        if( jPanel21 == null ){
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.gridy = 0;
            lblSpacer = new BLabel();
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
            jPanel21 = new BPanel();
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
     * @return org.kbinani.windows.forms.BButton
     */
    private BButton getBtnUp()
    {
        if( btnUp == null ){
            btnUp = new BButton();
            btnUp.setText( "Up" );
            btnUp.setPreferredSize( new Dimension( 75, 29 ) );
        }
        return btnUp;
    }

    /**
     * This method initializes btnDown
     * 
     * @return org.kbinani.windows.forms.BButton
     */
    private BButton getBtnDown()
    {
        if( btnDown == null ){
            btnDown = new BButton();
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
    private BListView getListDictionaries()
    {
        if( listDictionaries == null ){
            listDictionaries = new BListView();
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

    @Override
    public int showDialog(
        Object parent_form )
    {
        BDialogResult ret = BDialogResult.CANCEL;
        if( parent_form == null || (parent_form != null && !(parent_form instanceof BDialog)) ){
            ret = super.showDialog( null );
        }else{
            BDialog form = (BDialog)parent_form;
            ret = super.showDialog( form );
        }
        if( ret == BDialogResult.OK || ret == BDialogResult.YES ){
            return 1;
        }else{
            return 0;
        }
    }

    @Override
    public void setDialogResult(
        boolean value )
    {
        super.setDialogResult( value ? BDialogResult.OK : BDialogResult.CANCEL );
    }

    @Override
    public int listDictionariesGetSelectedRow()
    {
        return this.listDictionaries.getSelectedRow();
    }

    @Override
    public int listDictionariesGetItemCountRow()
    {
        return this.listDictionaries.getItemCountRow();
    }

    @Override
    public void listDictionariesClear()
    {
        this.listDictionaries.clear();
    }

    @Override
    public String listDictionariesGetItemAt(
        int row )
    {
        return this.listDictionaries.getItemAt( row, 0 );
    }

    @Override
    public boolean listDictionariesIsRowChecked(
        int row )
    {
        return this.listDictionaries.isRowChecked( row );
    }

    @Override
    public void listDictionariesSetItemAt(
        int row,
        String value )
    {
        this.listDictionaries.setItemAt( row, 0, value );
    }

    @Override
    public void listDictionariesSetRowChecked(
        int row,
        boolean value )
    {
        this.listDictionaries.setRowChecked( row, value );
    }

    @Override
    public void listDictionariesSetSelectedRow(
        int row )
    {
        this.listDictionaries.setSelectedRow( row );
    }

    @Override
    public void listDictionariesClearSelection()
    {
        this.listDictionaries.clearSelection();
    }

    @Override
    public void listDictionariesAddRow(
        String value,
        boolean selected )
    {
        this.listDictionaries.addRow( new String[]{ value }, selected );
    }

    @Override
    public void labelAvailableDictionariesSetText(
        String value )
    {
        this.lblAvailableDictionaries.setText( value );
    }

    @Override
    public void buttonOkSetText(
        String value )
    {
        this.btnOK.setText( value );
    }

    @Override
    public void buttonCancelSetText(
        String value )
    {
        this.btnCancel.setText( value );
    }

    @Override
    public void buttonUpSetText(
        String value )
    {
        this.btnUp.setText( value );
    }

    @Override
    public void buttonDownSetText(
        String value )
    {
        this.btnDown.setText( value );
    }

}
