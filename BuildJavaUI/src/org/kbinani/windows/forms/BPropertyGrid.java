package org.kbinani.windows.forms;

import java.awt.Color;
import java.awt.Component;
import java.awt.Container;
import java.awt.Cursor;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import java.awt.LayoutManager;
import java.awt.Panel;
import java.awt.Point;
import java.awt.event.ActionListener;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import javax.swing.BorderFactory;
import javax.swing.DefaultListModel;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JComponent;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextField;
import javax.swing.JWindow;
import javax.swing.ListSelectionModel;
import javax.swing.border.EmptyBorder;
import org.kbinani.BEvent;
import org.kbinani.str;
import org.kbinani.componentmodel.IPropertyDescriptor;
import org.kbinani.componentmodel.PropertyDescriptor;
import org.kbinani.componentmodel.TypeConverter;
import org.kbinani.xml.XmlMember;
import org.kbinani.xml.XmlSerializer;

public class BPropertyGrid extends BPanel implements IBPropertyGridTag {
    public static final int UNIT_WIDTH = 16;
    private static final long serialVersionUID = -5937300027752664252L;

    public BEvent<BPropertyValueChangedEventHandler> propertyValueChangedEvent = new BEvent<BPropertyValueChangedEventHandler>();
    
    private Class<?> mClass;
    private Object[] mSelected;
    
    public BPropertyGrid(){
        super();
    }

    public Object[] getSelectedObjects(){
        return this.mSelected;
    }

    public void setSelectedObjects( Object[] objects ){
System.out.println( "BPropertyGrid#setSelectedObjects" );
        // objectsに入っているオブジェクトのクラス．どれか一つでも
        // 型の違うオブジェクトが入っているとnull．objects中のnullは単に無視される
        Class<?> cls = null;
        for( Object o : objects ){
            if( o != null ){
                if( cls == null ){
                    cls = o.getClass();
                }else{
                    if( !cls.equals( o.getClass() ) ){
                        cls = null;
                        break;
                    }
                }
            }
        }

        if( this.mClass == null ){
            this.mClass = cls;
            updateGridComponents();
        }else{
            if( !this.mClass.equals( cls ) ){
                this.mClass = cls;
                updateGridComponents();
            }
        }
        this.mSelected = objects;
        update();
    }

    private void update(){
        updateValue( mSelected );
    }
    
    public static void updateValueCore( IBPropertyGridTag container, Object[] values )
    {
        if( values == null ) return;
        if( values.length <= 0 ) return;
        XmlMember[] member = container.getXmlMember();
        if( member == null ) return;
        Component[] comps = container.getValueComponents();
        if( comps == null ) return;
        for( int i = 0; i <  member.length; i++ ){
            XmlMember xm = member[i];
            Class<?> member_type = xm.getType();
            /*if( member_type.equals( this.mClass ) ){
                continue;
            }*/
            String draft = null;
            boolean all_equals = true;
            Object[] objs = new Object[values.length];
            for( int j = 0; j < values.length; j++ ){
                Object o = values[j];
                if( o == null ){
                    continue;
                }
                Object value = xm.get( o );
                objs[j] = value;
                String s = value + "";
System.out.println( "BPropertyGrid#updatd; s=" + s );
                if( draft == null ){
                    draft = s;
                }else{
                    if( !draft.equals( s ) ){
                        all_equals = false;
                        draft = "";
                        break;
                    }
                }
            }

            Component c = comps[i];
            if( c instanceof JTextField ){
                JTextField jtf = (JTextField)c;
                jtf.setText( draft );
            }else if( c instanceof BPropertyGridComboBox ){
                BPropertyGridComboBox jcb = (BPropertyGridComboBox)c;
                if( all_equals ){
                    for( int j = 0; j < jcb.getItemCount(); j++ ){
                        Object item = jcb.getItemAt( j );
                        if( draft.equals( item + "" ) ){
                            jcb.setSelectedIndex( j );
                            break;
                        }
                    }
                }else{
                    jcb.setSelectedIndex( -1 );
                }
            }else if( c instanceof BPropertyGridPanel ){
                BPropertyGridPanel bpgp = (BPropertyGridPanel)c;
                updateValueCore( bpgp, objs );
            }
        }
    }
    
    /**
     * 選択されたオブジェクトの値を，グリッドのコンポーネントの表示状態に反映させます
     */
    public void updateValue( Object[] values ){
        updateValueCore( this, values );
    }
    
    /**
     * グリッドのコンポーネントを，選択されたオブジェクトの型に合わせて変更します
     */
    private void updateGridComponents(){
        // まず現在登録されているコンポーネントをすべて破棄する
        this.removeAll();
        // プロパティーを追加．再帰的に
        int rows = appendComponentTo( this, this.mClass, true );
        // 一番下にスペーサを設置
        LayoutManager lm = this.getLayout();
        if( lm == null || (lm != null && !(lm instanceof GridBagLayout)) ){
            lm = new GridBagLayout();
            this.setLayout( lm );
        }
        GridBagLayout gbl = (GridBagLayout)lm;
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.gridx = 0;
        gbc.gridy = rows;
        gbc.weightx = 0.0;
        gbc.weighty = 1.0;
        Panel dumy = new Panel();
        gbl.addLayoutComponent( dumy, gbc );
        this.add( dumy );
    }

    /**
     * 指定したクラスのプロパティを，指定したコンポーネントに追加します
     * @param container
     * @param cls
     * @param add_spacer
     */
    private <T extends Container & IBPropertyGridTag> int appendComponentTo( T container, Class<?> cls, boolean add_spacer ){
        if( cls == null ){
            return 0;
        }
System.out.println( "BPropertyGrid#appendComponentTo; cls=" + cls );
        // レイアウト適用
        GridBagLayout layout = new GridBagLayout();
        container.setLayout( layout );
        
        // プロパティを抽出
        XmlMember[] member = XmlMember.extractMembers( cls );
        container.setXmlMember( member );
        
        // プロパティ・デスクリプタを取得
        PropertyDescriptor descriptor = null;
        if( IPropertyDescriptor.class.isAssignableFrom( cls ) ){
            try{
                IPropertyDescriptor ipd = (IPropertyDescriptor)cls.newInstance();
                descriptor = ipd.getDescriptor();
            }catch( Exception ex ){
                descriptor = null;
            }
        }
        if( descriptor == null ){
            descriptor = new PropertyDescriptor();
        }
        container.setDescriptor( descriptor );

        int num_members = member.length;
        Component[] comps = new Component[num_members];
        container.setValueComponents( comps );
System.out.println( "BPropertyGrid#appendComponentTo; mXmlMembers.length=" + member.length );

        // 表示幅を調節するための，取っ手
        BPanel handle = new BPanel();
        handle.setTag( container );
        JLabel[] label_components = new JLabel[num_members];
        container.setLabelComponents( label_components );
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.gridx = 1;
        gbc.gridheight = num_members;
        gbc.weightx = 0.0;
        gbc.fill = GridBagConstraints.BOTH;
        handle.setPreferredSize( new Dimension( 2, 1 ) );
        handle.setCursor( new Cursor( Cursor.W_RESIZE_CURSOR | Cursor.E_RESIZE_CURSOR ) );
        handle.addMouseMotionListener( new MouseMotionListener(){
            @Override
            public void mouseDragged(MouseEvent arg0) {
                Component obj = arg0.getComponent();
                if ( obj instanceof BPanel ){
                    BPanel panel = (BPanel)obj;
                    Object tag = panel.getTag();
                    if( tag != null ){
                        if( (tag instanceof IBPropertyGridTag) &&
                            (tag instanceof Container ) ){
                            IBPropertyGridTag btag = (IBPropertyGridTag)tag;
                            Container cont = (Container)tag;
                            int x = arg0.getXOnScreen();
                            int delta = x - btag.getInitX();
                            int draft = btag.getInitLabelWidth() + delta;
                            if ( draft < 0 ){
                                draft = 0;
                            }
                            int width = cont.getWidth();
                            if ( width < draft ){
                                draft = width;
                            }
                            btag.setLabelWidth( draft );
                            JLabel[] label_components = btag.getLabelComponents();
                            if( label_components != null ){
                                for( JLabel jl : btag.getLabelComponents() ){
                                    if( jl == null ) continue;
                                    jl.setPreferredSize( new Dimension( draft, UNIT_WIDTH ) );
                                }
                            }
                            cont.doLayout();
                        }
                    }
                }

            }

            @Override
            public void mouseMoved(MouseEvent arg0) {
            }
            
        });
        handle.addMouseListener( new MouseListener(){
            @Override
            public void mouseClicked(MouseEvent arg0) {
            }
        
            @Override
            public void mouseEntered(MouseEvent arg0) {
            }
        
            @Override
            public void mouseExited(MouseEvent arg0) {
            }
        
            @Override
            public void mousePressed(MouseEvent arg0) {
                Component obj = arg0.getComponent();
                if ( obj instanceof BPanel ){
                    BPanel panel = (BPanel)obj;
                    Object tag = panel.getTag();
                    if( tag != null ){
                        if( (tag instanceof IBPropertyGridTag) ){
                            IBPropertyGridTag btag = (IBPropertyGridTag)tag;
                            btag.setInitX( arg0.getXOnScreen() );
                            btag.setInitLabelWidth( btag.getLabelWidth() );
                        }
                    }
                }
            }
        
            @Override
            public void mouseReleased(MouseEvent arg0) {
            }
        });
        layout.addLayoutComponent( handle, gbc );
        container.add( handle );

        int rows = 0;
        int ret_num_child = 0;
        gbc.gridheight = 1;
        for ( XmlMember xm : member ){
            Class<?> member_type = xm.getType();
            if( member_type.equals( cls ) ){
                // 自分自身のフィールド等に同じ型のものがあると再帰になるので，スルーする
                continue;
            }
            String cli_name = XmlSerializer.getCliTypeName( member_type );
            // プリミティブ型
            rows++;
            gbc.gridy = rows - 1;
            gbc.weightx = 0;
            gbc.weighty = 0;
            gbc.fill = GridBagConstraints.BOTH;
            gbc.anchor = GridBagConstraints.NORTHWEST;

            // プロパティの名前
            JLabel propname = new JLabel();
            propname.setText( descriptor.getDisplayName( xm.getName() ) );
            propname.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
            Color bgcolor = (rows % 2 != 0) ? Color.WHITE : new Color( 240, 240, 240 );
            if ( container == this ){
                propname.setBackground( bgcolor );
            }
            propname.setOpaque( true );
            propname.setPreferredSize( new Dimension( container.getLabelWidth(), UNIT_WIDTH ) );
            propname.setSize( new Dimension( container.getLabelWidth(), propname.getHeight() ) );
            label_components[rows - 1] = propname;
            gbc.gridx = 0;
            gbc.weightx = 0;
            gbc.fill = GridBagConstraints.BOTH;
            layout.addLayoutComponent( propname, gbc );
            container.add( propname );
            
            // 型コンバータを取得
            TypeConverter<?, ?> converter = descriptor.getTypeConverter( xm.getName() );
            
            // 値を入れるテキストボックス(など)
            JComponent comp = null;
            int num_child = 1;
            if( converter != null && converter.isStandardValuesSupported() ){
                BPropertyGridComboBox cbox = new BPropertyGridComboBox();
                Object[] items = converter.getStandardValues().toArray();
                for( Object o : items ){
                    cbox.addItem( o );
                }
                comp = cbox;
            }else if( member_type.isEnum() ){
                BPropertyGridComboBox cbox = new BPropertyGridComboBox();
                Object[] items = member_type.getEnumConstants();
                for( Object o : items ){
                    cbox.addItem( o );
                }
                comp = cbox;
            /*}else if( cli_name.equals( "" ) ){
                JPanel panel = new JPanel();
                rows += _appendComponentTo( panel, member_type, false );
                comp = panel;*/
            }else if( cli_name.equals( "bool" ) ){
                BPropertyGridComboBox cbox = new BPropertyGridComboBox();
                cbox.addItem( "True" );
                cbox.addItem( "False" );
                cbox.setUnitWidth( UNIT_WIDTH );
                comp = cbox;
            }else if( str.compare( cli_name, "string" ) ||
                      str.compare( cli_name, "double" ) ||
                      str.compare( cli_name, "int" ) ||
                      str.compare( cli_name, "long" ) ||
                      str.compare( cli_name, "short" ) ||
                      str.compare( cli_name, "float" ) ){
                JTextField text = new JTextField();
                comp = text;
            }else{
                BPropertyGridPanel bpgp = new BPropertyGridPanel();
                num_child = appendComponentTo( bpgp, member_type, false );
                comp = bpgp;
            }
            ret_num_child += num_child;
            comps[rows - 1] = comp;
            comp.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
            if( container == this ){
                comp.setBackground( bgcolor );
            }
            comp.setPreferredSize( new Dimension( 4, UNIT_WIDTH * num_child ) );
            gbc.gridx = 2;
            gbc.weightx = 1.0;
            gbc.fill = GridBagConstraints.BOTH;
            layout.addLayoutComponent( comp, gbc );
            container.add( comp );
        }
        
        return ret_num_child;
    }

    int mInitLabelWidth = 50;
    @Override
    public int getInitLabelWidth() {
        return mInitLabelWidth;
    }

    @Override
    public void setInitLabelWidth(int value) {
        mInitLabelWidth = value;
    }

    int mLabelWidth = 50;
    @Override
    public int getLabelWidth() {
        return mLabelWidth;
    }

    @Override
    public void setLabelWidth(int value) {
        mLabelWidth = value;
    }

    int mInitX;
    @Override
    public int getInitX() {
        return mInitX;
    }

    @Override
    public void setInitX(int value) {
        mInitX = value;
    }

    JLabel[] mLabelComponents = null;
    @Override
    public JLabel[] getLabelComponents() {
        return mLabelComponents;
    }

    @Override
    public void setLabelComponents(JLabel[] value) {
        mLabelComponents = value;
    }

    XmlMember[] mXmlMember = null;
    @Override
    public XmlMember[] getXmlMember() {
        return mXmlMember;
    }

    @Override
    public void setXmlMember(XmlMember[] value) {
        mXmlMember = value;
    }

    Component[] mComponent = null;
    @Override
    public Component[] getValueComponents() {
        return mComponent;
    }

    @Override
    public void setValueComponents(Component[] value) {
        mComponent = value;
    }

    private PropertyDescriptor mDescriptor;
    @Override
    public PropertyDescriptor getDescripter() {
        return mDescriptor;
    }

    @Override
    public void setDescriptor(PropertyDescriptor value) {
        mDescriptor = value;
    }
}

class BPropertyGridPanel extends BPanel implements IBPropertyGridTag
{
    private static final long serialVersionUID = -1162744817095102693L;

    int mInitLabelWidth = 50;
    @Override
    public int getInitLabelWidth() {
        return mInitLabelWidth;
    }

    @Override
    public void setInitLabelWidth(int value) {
        mInitLabelWidth = value;
    }

    int mLabelWidth = 50;
    @Override
    public int getLabelWidth() {
        return mLabelWidth;
    }

    @Override
    public void setLabelWidth(int value) {
        mLabelWidth = value;
    }

    int mInitX;
    @Override
    public int getInitX() {
        return mInitX;
    }

    @Override
    public void setInitX(int value) {
        mInitX = value;
    }

    JLabel[] mLabelComponents = null;
    @Override
    public JLabel[] getLabelComponents() {
        return mLabelComponents;
    }

    @Override
    public void setLabelComponents(JLabel[] value) {
        mLabelComponents = value;
    }

    XmlMember[] mXmlMember = null;
    @Override
    public XmlMember[] getXmlMember() {
        return mXmlMember;
    }

    @Override
    public void setXmlMember(XmlMember[] value) {
        mXmlMember = value;
    }

    Component[] mComponent = null;
    @Override
    public Component[] getValueComponents() {
        return mComponent;
    }

    @Override
    public void setValueComponents(Component[] value) {
        mComponent = value;
    }

    PropertyDescriptor mDescriptor = null;
    @Override
    public PropertyDescriptor getDescripter() {
        return mDescriptor;
    }

    @Override
    public void setDescriptor(PropertyDescriptor value) {
        mDescriptor = value;
    }
    
}

interface IBPropertyGridTag
{
    /**
     * マウスが降りた時のパネルの幅
     */
    public int getInitLabelWidth();
    public void setInitLabelWidth( int value );
    /**
     * 現在のラベルの幅
     */
    public int getLabelWidth();
    public void setLabelWidth( int value );
    /**
     * マウスが降りた時の、マウスのx座標。座標系はスクリーン座標
     */
    public int getInitX();
    public void setInitX( int value );
    /**
     * プロパティ名を表示しているコンポーネントの一覧
     */
    public JLabel[] getLabelComponents();
    public void setLabelComponents( JLabel[] value );
    /**
     * 値を表示しているコンポーネントの一覧
     * @return
     */
    public Component[] getValueComponents();
    public void setValueComponents( Component[] value );
    /**
     * 表示しているプロパティーのメンバー
     */
    public XmlMember[] getXmlMember();
    public void setXmlMember( XmlMember[] value );
    /**
     * プロパティのカテゴライズなどを担当する奴
     */
    public PropertyDescriptor getDescripter();
    public void setDescriptor( PropertyDescriptor value );
}

/**
 * 小さなコンボボックス
 */
class BPropertyGridComboBox extends JPanel implements ActionListener {
    private static final long serialVersionUID = -6834563484239011621L;
    private JLabel labelValue = null;
    private JButton buttonDropdown = null;
    private BPropertyGridComboBoxDropdown windowDropdown = null;  //  @jve:decl-index=0:visual-constraint="184,55"

    /**
     * This method initializes 
     * 
     */
    public BPropertyGridComboBox() {
        super();
        initialize();
        getWindowDropdown();
        this.addComponentListener( new ComponentListener(){
            @Override
            public void componentHidden(ComponentEvent arg0) {
            }

            @Override
            public void componentMoved(ComponentEvent arg0) {
            }

            @Override
            public void componentResized(ComponentEvent arg0) {
                doLayout();
            }

            @Override
            public void componentShown(ComponentEvent arg0) {
            }
        });
    }
    
    public void setUnitWidth( int width ){
        labelValue.setPreferredSize( new Dimension( 4, width ) );
        buttonDropdown.setPreferredSize( new Dimension( width, width ) );
    }
    
    public void setSelectedIndex( int value )
    {
        if ( value < 0 ){
            value = -1;
        }
        windowDropdown.setSelectedIndex( value );
        int indx = windowDropdown.getSelectedIndex();
        if ( indx < 0 ){
            labelValue.setText( "" );
        }else{
            labelValue.setText( "" + windowDropdown.getItemAt( indx ) );
        }
    }
    
    public int getItemCount()
    {
        return windowDropdown.getItemCount();
    }
    
    public int getSelectedIndex()
    {
        return getWindowDropdown().getSelectedIndex();
    }

    public void addItem( Object obj )
    {
        windowDropdown.addItem( obj );
        labelValue.setText( "" + windowDropdown.getItemAt( windowDropdown.getSelectedIndex() ) );
    }
    
    public Object getItemAt( int index )
    {
        return windowDropdown.getItemAt( index );
    }
    
    public void actionPerformed(java.awt.event.ActionEvent e) {
        BPropertyGridComboBoxDropdown dropdown = getWindowDropdown();
        if ( dropdown.isVisible() ){
            dropdown.setVisible( false );
        }else{
            // ボタンのスクリーン上の位置を把握する
            Point loc = buttonDropdown.getLocationOnScreen();
            int y = loc.y + buttonDropdown.getHeight();
            int x = this.getLocationOnScreen().x;
            dropdown.setSize( this.getWidth(), dropdown.getHeight() );
            dropdown.setLocation( x, y );
            dropdown.setVisible( true );
        }
    }

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
        gridBagConstraints1.gridx = 1;
        gridBagConstraints1.weighty = 1.0D;
        gridBagConstraints1.fill = GridBagConstraints.VERTICAL;
        gridBagConstraints1.anchor = GridBagConstraints.EAST;
        gridBagConstraints1.gridy = 0;
        GridBagConstraints gridBagConstraints = new GridBagConstraints();
        gridBagConstraints.insets = new Insets(0, 0, 0, 0);
        gridBagConstraints.gridy = 0;
        gridBagConstraints.fill = GridBagConstraints.BOTH;
        gridBagConstraints.weightx = 1.0D;
        gridBagConstraints.weighty = 1.0D;
        gridBagConstraints.gridx = 0;
        labelValue = new JLabel();
        labelValue.setText("");
        labelValue.setPreferredSize(new Dimension(16, 16));
        this.setLayout(new GridBagLayout());
        this.setSize(new Dimension(60, 16));
        this.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        this.add(labelValue, gridBagConstraints);
        this.add(getButtonDropdown(), gridBagConstraints1);
            
    }

    /**
     * This method initializes buttonDropdown   
     *  
     * @return javax.swing.JButton  
     */
    private JButton getButtonDropdown() {
        if (buttonDropdown == null) {
            buttonDropdown = new JButton();
            buttonDropdown.setPreferredSize(new Dimension(16, 16));
            buttonDropdown.addActionListener( this );
        }
        return buttonDropdown;
    }

    /**
     * This method initializes windowDropdown   
     *  
     * @return javax.swing.JWindow  
     */
    private BPropertyGridComboBoxDropdown getWindowDropdown() {
        if (windowDropdown == null) {
            windowDropdown = new BPropertyGridComboBoxDropdown();
            windowDropdown.setSize(new Dimension(97, 48));
            windowDropdown.listItems.addMouseListener( new MouseListener(){
                @Override
                public void mouseClicked(MouseEvent arg0) {
                    windowDropdown.setVisible( false );
                    labelValue.setText( "" + windowDropdown.getItemAt( windowDropdown.getSelectedIndex() ) );
                }

                @Override
                public void mouseEntered(MouseEvent arg0) {
                }

                @Override
                public void mouseExited(MouseEvent arg0) {
                }

                @Override
                public void mousePressed(MouseEvent arg0) {
                }

                @Override
                public void mouseReleased(MouseEvent arg0) {
                } 
            });
        }
        return windowDropdown;
    }
}  //  @jve:decl-index=0:visual-constraint="10,10"

class BPropertyGridComboBoxDropdown extends JWindow {
    private static final long serialVersionUID = -330369852089355660L;
    public JList listItems = null;
    private JPanel jPanel = null;
    private DefaultListModel model = null;
    private JScrollPane jScrollPane = null;

    /**
     * This method initializes 
     * 
     */
    public BPropertyGridComboBoxDropdown() {
        super();
        initialize();
    }
    
    public void setSelectedIndex( int value ){
        listItems.setSelectedIndex( value );
    }
    
    public int getItemCount()
    {
        return model.getSize();
    }

    public Object getItemAt( int index )
    {
        return model.getElementAt( index );
    }
    
    public int getSelectedIndex()
    {
        return listItems.getSelectedIndex();
    }
    
    public void addItem( Object obj )
    {
        getModel().addElement( obj );
        if( listItems.getSelectedIndex() < 0 ){
            listItems.setSelectedIndex( 0 );
        }
    }
    
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(92, 125));
        this.setContentPane(getJPanel());
            
    }

    private DefaultListModel getModel()
    {
        if ( model == null ){
            model = new DefaultListModel();
        }
        return model;
    }
    
    /**
     * This method initializes listItems    
     *  
     * @return javax.swing.JList    
     */
    private JList getListItems() {
        if (listItems == null) {
            listItems = new JList();
            listItems.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
            listItems.setVisibleRowCount(100);
            listItems.setModel( getModel() );
        }
        return listItems;
    }

    /**
     * This method initializes jPanel   
     *  
     * @return javax.swing.JPanel   
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.BOTH;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 1.0;
            gridBagConstraints1.weighty = 1.0;
            gridBagConstraints1.gridx = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jPanel.add(getJScrollPane(), gridBagConstraints1);
        }
        return jPanel;
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
            jScrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
            jScrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
            jScrollPane.setViewportView(getListItems());
        }
        return jScrollPane;
    }

}
