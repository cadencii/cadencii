package org.kbinani.windows.forms;

import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GraphicsEnvironment;
import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;
import java.awt.Insets;
import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.JList;
import javax.swing.ListSelectionModel;
import javax.swing.JScrollPane;
import javax.swing.BorderFactory;
import javax.swing.border.TitledBorder;
import java.awt.Font;
import java.awt.Color;

public class BFontChooser extends BDialog {

    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;
    private JLabel lblName = null;
    private JPanel jPanel1 = null;
    private JTextField txtName = null;
    private JLabel lblStyle = null;
    private JTextField txtStyle = null;
    private JTextField txtSize = null;
    private JList listName = null;
    private JList listStyle = null;
    private JList listSize = null;
    private JLabel lblSize = null;
    private JLabel lblSample = null;
    private JScrollPane jScrollPane = null;
    private JScrollPane jScrollPane1 = null;
    private JScrollPane jScrollPane2 = null;
    private JPanel jPanel2 = null;
    private String[] styleNames = new String[]{ "PLAIN", "ITALIC", "BOLD", "BOLD ITALIC" };
    private int[] stylenum = new int[]{ Font.PLAIN, Font.ITALIC, Font.BOLD, Font.BOLD | Font.ITALIC };
    private String[] sizeNames = new String[]{ "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", 
                                               "26", "28", "36", "48", "72" };
    private int[] sizenum = new int[]{ 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
    private String[] names = new String[]{};

    public void setSelectedFont( Font font ){
        lblSample.setFont( font );
        String name = font.getName();
        txtName.setText( name );
        for( int i = 0; i < names.length; i++ ){
            if( name.equals( names[i] ) ){
                listName.setSelectedIndex( i );
                listName.ensureIndexIsVisible( i );
                break;
            }
        }
        
        int style = font.getStyle();
        for( int i = 0; i < stylenum.length; i++ ){
            if( style == stylenum[i] ){
                listStyle.setSelectedIndex( i );
                listStyle.ensureIndexIsVisible( i );
                txtStyle.setText( styleNames[i] );
                break;
            }
        }
        
        int size = font.getSize();
        txtSize.setText( size + "" );
        for( int i = 0; i < sizenum.length; i++ ){
            if( size == sizenum[i] ){
                listSize.setSelectedIndex( i );
                listSize.ensureIndexIsVisible( i );
                break;
            }
        }
        
    }
    
    public Font getSelectedFont(){
        return lblSample.getFont();
    }

    private void updateFont(){
        String name = txtName.getText();
        int indxStyle = listStyle.getSelectedIndex();
        if( indxStyle < 0 ){
            indxStyle = 0;
        }
        int style = stylenum[indxStyle];
        
        int indxSize = listSize.getSelectedIndex();
        int size = 10;
        if( indxSize >= 0 ){
            size = sizenum[indxSize];
        }
        Font f = new Font( name, style, size );
        lblSample.setFont( f );
    }
    
    /**
     * This method initializes 
     * 
     */
    public BFontChooser() {
    	super();
    	initialize();
    	names = GraphicsEnvironment.getLocalGraphicsEnvironment().getAvailableFontFamilyNames();
    	listName.setListData( names );
    	listStyle.setListData( styleNames );
    	listSize.setListData( sizeNames );
    	setSelectedFont( lblSample.getFont() );
    }

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        lblName = new JLabel();
        lblName.setText("Font Name");
        this.setSize(new Dimension(477, 300));
        this.setModal(true);
        this.setTitle("Font");
        this.setContentPane(getJPanel1());
    		
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.anchor = GridBagConstraints.WEST;
            gridBagConstraints5.gridx = 1;
            gridBagConstraints5.gridy = 0;
            gridBagConstraints5.insets = new Insets(0, 0, 0, 16);
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.anchor = GridBagConstraints.WEST;
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.gridy = 0;
            gridBagConstraints4.insets = new Insets(0, 0, 0, 16);
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getBtnOK(), gridBagConstraints4);
            jPanel.add(getBtnCancel(), gridBagConstraints5);
        }
        return jPanel;
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
            btnOK.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    setDialogResult( BDialogResult.OK );
                }
            });
        }
        return btnOK;
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
            btnCancel.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    setDialogResult( BDialogResult.CANCEL );
                }
            });
        }
        return btnCancel;
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
            gridBagConstraints41.gridx = 0;
            gridBagConstraints41.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints41.gridwidth = 3;
            gridBagConstraints41.anchor = GridBagConstraints.WEST;
            gridBagConstraints41.insets = new Insets(0, 12, 0, 12);
            gridBagConstraints41.weighty = 0.0D;
            gridBagConstraints41.gridy = 3;
            GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
            gridBagConstraints31.fill = GridBagConstraints.BOTH;
            gridBagConstraints31.gridy = 2;
            gridBagConstraints31.weightx = 2.0D;
            gridBagConstraints31.weighty = 1.0;
            gridBagConstraints31.insets = new Insets(2, 6, 12, 12);
            gridBagConstraints31.gridx = 2;
            GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
            gridBagConstraints21.fill = GridBagConstraints.BOTH;
            gridBagConstraints21.gridy = 2;
            gridBagConstraints21.weightx = 4.0D;
            gridBagConstraints21.weighty = 1.0;
            gridBagConstraints21.insets = new Insets(2, 6, 12, 6);
            gridBagConstraints21.gridx = 1;
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.fill = GridBagConstraints.BOTH;
            gridBagConstraints14.gridy = 2;
            gridBagConstraints14.weightx = 5.0D;
            gridBagConstraints14.weighty = 1.0;
            gridBagConstraints14.insets = new Insets(2, 12, 12, 6);
            gridBagConstraints14.gridx = 0;
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 0;
            gridBagConstraints13.gridwidth = 3;
            gridBagConstraints13.anchor = GridBagConstraints.EAST;
            gridBagConstraints13.insets = new Insets(16, 0, 16, 0);
            gridBagConstraints13.gridy = 4;
            lblSample = new JLabel();
            lblSample.setText("Aa�����A�@���F");
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 2;
            gridBagConstraints6.anchor = GridBagConstraints.WEST;
            gridBagConstraints6.insets = new Insets(12, 6, 0, 0);
            gridBagConstraints6.gridy = 0;
            lblSize = new JLabel();
            lblSize.setText("Size");
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.fill = GridBagConstraints.BOTH;
            gridBagConstraints8.gridy = 2;
            gridBagConstraints8.weightx = 5.0D;
            gridBagConstraints8.weighty = 1.0;
            gridBagConstraints8.insets = new Insets(2, 12, 12, 6);
            gridBagConstraints8.gridx = 0;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints7.gridy = 1;
            gridBagConstraints7.weightx = 1.0;
            gridBagConstraints7.insets = new Insets(0, 6, 0, 12);
            gridBagConstraints7.gridx = 2;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints3.gridy = 1;
            gridBagConstraints3.weightx = 1.0;
            gridBagConstraints3.insets = new Insets(0, 6, 0, 6);
            gridBagConstraints3.gridx = 1;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 1;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(12, 6, 0, 0);
            gridBagConstraints2.gridy = 0;
            lblStyle = new JLabel();
            lblStyle.setText("Style");
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.gridy = 1;
            gridBagConstraints1.weightx = 1.0;
            gridBagConstraints1.insets = new Insets(0, 12, 0, 6);
            gridBagConstraints1.gridx = 0;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.insets = new Insets(12, 12, 0, 0);
            gridBagConstraints.gridy = 0;
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(lblName, gridBagConstraints);
            jPanel1.add(getTxtName(), gridBagConstraints1);
            jPanel1.add(lblStyle, gridBagConstraints2);
            jPanel1.add(getTxtStyle(), gridBagConstraints3);
            jPanel1.add(getTxtSize(), gridBagConstraints7);
            jPanel1.add(lblSize, gridBagConstraints6);
            jPanel1.add(getJPanel(), gridBagConstraints13);
            jPanel1.add(getJScrollPane(), gridBagConstraints14);
            jPanel1.add(getJScrollPane1(), gridBagConstraints21);
            jPanel1.add(getJScrollPane2(), gridBagConstraints31);
            jPanel1.add(getJPanel2(), gridBagConstraints41);
        }
        return jPanel1;
    }

    /**
     * This method initializes txtName	
     * 	
     * @return javax.swing.JTextField	
     */
    private JTextField getTxtName() {
        if (txtName == null) {
            txtName = new JTextField();
            txtName.setEditable(false);
        }
        return txtName;
    }

    /**
     * This method initializes txtStyle	
     * 	
     * @return javax.swing.JTextField	
     */
    private JTextField getTxtStyle() {
        if (txtStyle == null) {
            txtStyle = new JTextField();
            txtStyle.setEditable(false);
        }
        return txtStyle;
    }

    /**
     * This method initializes txtSize	
     * 	
     * @return javax.swing.JTextField	
     */
    private JTextField getTxtSize() {
        if (txtSize == null) {
            txtSize = new JTextField();
            txtSize.setEditable(false);
        }
        return txtSize;
    }

    /**
     * This method initializes listName	
     * 	
     * @return javax.swing.JList	
     */
    private JList getListName() {
        if (listName == null) {
            listName = new JList();
            listName.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
            listName.addListSelectionListener(new javax.swing.event.ListSelectionListener() {
                        public void valueChanged(javax.swing.event.ListSelectionEvent e) {
                            Object obj = listName.getSelectedValue();
                            if( obj instanceof String ){
                                txtName.setText( (String)obj );
                                updateFont();
                            }
                        }
                    });
        }
        return listName;
    }

    /**
     * This method initializes listStyle	
     * 	
     * @return javax.swing.JList	
     */
    private JList getListStyle() {
        if (listStyle == null) {
            listStyle = new JList();
            listStyle.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
            listStyle.addListSelectionListener(new javax.swing.event.ListSelectionListener() {
                        public void valueChanged(javax.swing.event.ListSelectionEvent e) {
                            Object obj = listStyle.getSelectedValue();
                            if( obj instanceof String ){
                                txtStyle.setText( (String)obj );
                                updateFont();
                            }
                        }
                    });
        }
        return listStyle;
    }

    /**
     * This method initializes listSize	
     * 	
     * @return javax.swing.JList	
     */
    private JList getListSize() {
        if (listSize == null) {
            listSize = new JList();
            listSize.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
            listSize.addListSelectionListener(new javax.swing.event.ListSelectionListener() {
                        public void valueChanged(javax.swing.event.ListSelectionEvent e) {
                            Object obj = listSize.getSelectedValue();
                            if( obj instanceof String ){
                                txtSize.setText( (String)obj );
                                updateFont();
                            }
                        }
                    });
        }
        return listSize;
    }

    /**
     * This method initializes jScrollPane	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane() {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jScrollPane.setViewportView(getListName());
        }
        return jScrollPane;
    }

    /**
     * This method initializes jScrollPane1	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane1() {
        if (jScrollPane1 == null) {
            jScrollPane1 = new JScrollPane();
            jScrollPane1.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jScrollPane1.setViewportView(getListStyle());
        }
        return jScrollPane1;
    }

    /**
     * This method initializes jScrollPane2	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane2() {
        if (jScrollPane2 == null) {
            jScrollPane2 = new JScrollPane();
            jScrollPane2.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jScrollPane2.setViewportView(getListSize());
        }
        return jScrollPane2;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel2() {
        if (jPanel2 == null) {
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.anchor = GridBagConstraints.WEST;
            gridBagConstraints9.gridwidth = 2;
            gridBagConstraints9.gridx = 0;
            gridBagConstraints9.gridy = 0;
            gridBagConstraints9.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints9.weightx = 1.0D;
            gridBagConstraints9.weighty = 1.0D;
            gridBagConstraints9.insets = new Insets(0, 6, 0, 0);
            jPanel2 = new JPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.setBorder(BorderFactory.createTitledBorder(null, "Sample", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
            jPanel2.add(lblSample, gridBagConstraints9);
        }
        return jPanel2;
    }
    
}  //  @jve:decl-index=0:visual-constraint="10,10"
