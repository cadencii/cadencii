package com.github.cadencii.windows.forms;

import java.awt.Color;
import java.awt.Component;
import java.awt.Container;
import java.awt.Cursor;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Image;
import java.awt.Insets;
import java.awt.ItemSelectable;
import java.awt.MouseInfo;
import java.awt.Point;
import java.awt.Window;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.FocusAdapter;
import java.awt.event.FocusEvent;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.util.Iterator;
import java.util.TreeMap;
import java.util.Vector;
import javax.imageio.ImageIO;
import javax.swing.BorderFactory;
import javax.swing.DefaultListModel;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JComponent;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextField;
import javax.swing.ListSelectionModel;
import javax.swing.SwingUtilities;
import javax.swing.border.EmptyBorder;
import javax.swing.event.AncestorEvent;
import javax.swing.event.AncestorListener;
import org.kbinani.BEvent;
import org.kbinani.str;
import org.kbinani.componentmodel.Category;
import org.kbinani.componentmodel.IPropertyDescriptor;
import org.kbinani.componentmodel.PropertyDescriptor;
import org.kbinani.componentmodel.TypeConverter;
import org.kbinani.componentmodel.TypeConverterAnnotation;
import org.kbinani.xml.XmlMember;
import org.kbinani.xml.XmlSerializer;

public class BPropertyGrid extends BPanel
{
    private static final long serialVersionUID = 413826982000655392L;

    public static final int DEFAULT_ROW_HEIGHT = 16;
    public static final int WIDTH = 14;
    public static final int SPACE = 14;
    public static final Color COLOR_SEPARATOR = new Color( 204, 204, 204 );
    public static final Color DEFAULT_BACKGROUND_EVEN = new Color( 255, 255, 255 );
    public static final Color DEFAULT_BACKGROUND_ODD = new Color( 237, 243, 254 );
    
    public final BEvent<BPropertyValueChangedEventHandler> propertyValueChangedEvent = new BEvent<BPropertyValueChangedEventHandler>();

    private Object[] mSelected;
    private TreeMap<String, BPropertyGridController> mCachedControllers = new TreeMap<String, BPropertyGridController>();
    private BPropertyGridController mController = null;
    private Color mColorEven = new Color( 255, 255, 255 );
    private Color mColorOdd = new Color( 237, 243, 254 );
    private int mRowHeight = 16;
    /**
     * 区切り線の現在の位置
     */
    private int mColumnWidth = 60;

    public BPropertyGrid()
    {
        super();
        this.setLayout( new GridBagLayout() );
        setSelectedObjects( null );
    }

    /**
     * グリッド１行の表示高さを取得します(単位:ピクセル)
     * @return グリッド１行の表示高さ
     */
    public int getRowHeight()
    {
        return mRowHeight;
    }
    
    /**
     * グリッド１行の表示高さを設定します(単位:ピクセル)
     */
    public void setRowHeight( int value )
    {
        if( value <= 0 ){
            value = 1;
        }
        mRowHeight = value;
        mController.changeRowHeight();
    }

    /**
     * 偶数行に位置するアイテムの背景色を取得します
     * @return 偶数行に位置するアイテムの背景色
     */
    public Color getBackgroundEven()
    {
        return mColorEven;
    }   
    
    /**
     * 偶数行に位置するアイテムの背景色を設定します
     * @param value 設定する色
     */
    public void setBackgroundEven( Color value )
    {
        mColorEven = value;
    }

    /**
     * 奇数行に位置するアイテムの背景色を取得します
     * @return 奇数行に位置するアイテムの背景色
     */
    public Color getBackgroundOdd()
    {
        return mColorOdd;
    }
    
    /**
     * 奇数行に位置するアイテムの背景色を設定します
     * @param value 設定する色
     */
    public void setBackgroundOdd( Color value )
    {
        mColorOdd = value;
    }

    /**
     * コントローラーを設定します
     * @param controller 新しく設定するコントローラー
     */
    private void setController( BPropertyGridController controller )
    {
        if( mController != null ){
            mController.setView( null );
        }
        mController = controller;
        this.removeAll();
        if( mController != null ){
            mController.changeRowHeight();
            mController.setView( this );
            mController.setContentPane( this );
            mController.updateBackground( true );
            mColumnWidth = mController.applyColumnWidth( mColumnWidth );
        }
    }

    /**
     * プロパティ名を表示するカラムの幅を設定します
     * @param value 新しく設定するカラムの幅
     */
    public void setColumnWidth( int value )
    {
        if( mController != null ){
            mColumnWidth = mController.applyColumnWidth( value );
        }else{
            mColumnWidth = value;
        }
    }
    
    /**
     * プロパティ名を表示するカラムの現在の幅を取得します
     * @return カラムの幅
     */
    public int getColumnWidth()
    {
        return mColumnWidth;
    }

    /**
     * グリッドに現在表示されているオブジェクトの一覧を取得します
     * @return オブジェクトの一覧
     */
    public Object[] getSelectedObjects()
    {
        return mSelected;
    }

    /**
     * グリッドに表示するオブジェクトの一覧を設定します
     * @param value 新しく設定するオブジェクトの一覧
     */
    public void setSelectedObjects( Object[] value )
    {
        mSelected = value;
        if( value != null && value.length > 0 ){
            String f = null;
            boolean all_eq = true;
            for( Object o : value ){
                if( o == null ){
                    continue;
                }
                if( f == null ){
                    f = o.getClass().getName();
                }else{
                    if( !f.equals( o.getClass().getName() ) ){
                        all_eq = false;
                        break;
                    }
                }
            }
            if( all_eq ){
                updateGridComponent( value[0].getClass() );
            }else{
                updateGridComponent( Object.class );
            }
        }else{
            updateGridComponent( Object.class );
        }
        if( mController != null ){
            mController.updateValue();
            mController.updateView();
        }
    }

    /**
     * 指定された型のためのグリッドを更新します
     * @param cls グリッドの構築元となる型
     */
    private void updateGridComponent( Class<?> cls )
    {
        if( cls == null ) return;
        BPropertyGridController controller = null;
        if( mCachedControllers.containsKey( cls.getName() ) && (mCachedControllers.get( cls.getName() ) != null) ){
            controller = mCachedControllers.get( cls.getName() );
        }else{
            controller = new BPropertyGridController( cls );
            mCachedControllers.put( cls.getName(), controller );
        }
        setController( controller );
    }
}

class BPropertyGridController
{
    /**
     * このコントローラーに属するコンポーネントの親．
     * ビューからこのコントローラーから切り離されるときは，このフィールドのインスタンスがビューから削除される．
     */
    private BPanel mContentPane = null;
    /**
     * 上からkey番目のコンポーネントのリスト，を保持するマップ
     */
    private TreeMap<Integer, Vector<Component>> mComponents;
    /**
     * タイトルのビューと値のビュー＆エディタを区切る区切り線を表現するのに使われているコンポーネントの一覧
     */
    private Vector<Component> mSplitters = new Vector<Component>();
    private Vector<Component> mLefts = new Vector<Component>();
    /**
     * このコントローラーにコンポーネントを追加していく際に，そのコンポーネントが上から何番目かを数え上げるためのカウンター
     */
    private int mRowCounter = 0;
    /**
     * このコントローラーを保持しているビューへの参照．このコントローラーが表示されていなければnullになる．
     */
    private BPropertyGrid mView = null;
    /**
     * このコントローラーにより，表示させられているグリッドの行数
     */
    private int mViewingRows = 0;
    /**
     * プロパティー・ツリーのルート
     */
    private BGridItem mRoot = null;
    /**
     * 推奨高さが指定されたアイテムの一覧
     */
    private Vector<Component> mPreferredHeightSpecified = new Vector<Component>();
    
    public BPropertyGridController( Class<?> cls )
    {
        mComponents = new TreeMap<Integer, Vector<Component>>();
        mContentPane = new BPanel();
        mContentPane.setLayout( new GridBagLayout() );
        mRoot = new BGridItem();

        // クラスのプロパティのメンバーを取得
        XmlMember[] members = XmlMember.extractMembers( cls );
    
        // 型記述子を取得
        PropertyDescriptor descriptor = getDescriptor( cls );
    
        // カテゴリに分類する
        TreeMap<String, Vector<XmlMember>> groups = new TreeMap<String, Vector<XmlMember>>();
        for( XmlMember xm : members ){
            String categ = null;
            Category ic = xm.getGetterAnnotation( Category.class );
            if( ic != null ){
                categ = ic.value();
            }
            if( categ == null ){
                ic = xm.getSetterAnnotation( Category.class );
                if( ic != null ){
                    categ = ic.value();
                }
            }
            if( categ == null ){
                categ = "";
            }
            Vector<XmlMember> dst = null;
            if( groups.containsKey( categ ) ){
                dst = groups.get( categ );
            }else{
                dst = new Vector<XmlMember>();
                groups.put( categ, dst );
            }
            dst.add( xm );
        }
            
        // カテゴリ毎に追加処理を行う
        int rows = 0;
        BPropertyGridSplitAdapter mouse_adapter = new BPropertyGridSplitAdapter( this );
        for( Iterator<String> itr = groups.keySet().iterator(); itr.hasNext(); ){
            String key = itr.next();
            Vector<XmlMember> m = groups.get( key );
            rows = prepareCategory( rows, key, m, descriptor, mouse_adapter );
        }
        
        // 末尾にスペーサを入れる
        BPropertyGridBottomSpacer sp_bottom = new BPropertyGridBottomSpacer( this );
        sp_bottom.setPreferredSize( new Dimension( 4, 1 ) );
        registPreferredHeightSpecified( sp_bottom );
        GridBagConstraints g = new GridBagConstraints();
        g.gridx = 0;
        g.gridy = rows;
        g.gridwidth = 4;
        g.gridheight = 1;
        g.weightx = 1.0D;
        g.weighty = 1.0D;
        g.fill = GridBagConstraints.BOTH;
        mContentPane.add( sp_bottom, g );
        mContentPane.addAncestorListener(new AncestorListener(){
            @Override
            public void ancestorAdded(AncestorEvent arg0) {
                updateView();
            }

            @Override
            public void ancestorMoved(AncestorEvent arg0) {
            }

            @Override
            public void ancestorRemoved(AncestorEvent arg0) {
            }            
        });
    }

    /**
     * viewにcolumn_widthの変更を通知するようこのcontroller要求します
     */
    public void notifyColumnWidthToView( int value )
    {
        if( mView != null ){
            mView.setColumnWidth( value );
        }
    }
    
    /**
     * アイテムの推奨高さを更新します
     */
    public void changeRowHeight()
    {
        if( mView == null ){
            return;
        }
        int row_height = mView.getRowHeight();
        for( Component c : mPreferredHeightSpecified ){
            int w = c.getPreferredSize().width;
            Dimension d = new Dimension( w, row_height );
            c.setPreferredSize( d );
        }
        updateView();
    }
    
    /**
     * アイテムの高さが指定されているアイテムとしてコントローラーに登録する
     * @param comp 登録するコンポーネント
     */
    public void registPreferredHeightSpecified( Component comp )
    {
        mPreferredHeightSpecified.add( comp );
    }
    
    private static void updateValueCore( Object[] objs, BGridItem unit )
    {
        if( objs.length <= 0 ){
            return;
        }
        Object[] subobjs = new Object[objs.length];
        for( int i = 0; i < objs.length; i++ ){
            subobjs[i] = unit.member.get( objs[i] );
        }
        Object f = null;
        for( int i = 0; i < subobjs.length; i++ ){
            Object o = subobjs[i];
            if( o == null ){
                continue;
            }
            if( f == null ){
                f = o;
            }else{
                if( !o.equals( f ) ){
                    f = null;
                    break;
                }
            }
        }
        if( unit.editor instanceof JTextField ){
            if( unit.converter != null ){
                ((JTextField)unit.editor).setText( unit.converter.convertTo( f ) );
            }else{
                ((JTextField)unit.editor).setText( (f == null) ? "" : "" + f );
            }
        }else if( unit.editor instanceof BPropertyGridComboBox ){
            BPropertyGridComboBox cbox = (BPropertyGridComboBox)unit.editor;
            int indx = -1;
            if( f != null ){
                for( int i = 0; i < cbox.getItemCount(); i++ ){
                    Object o = cbox.getItemAt( i );
                    if( o == null ){
                        continue;
                    }
                    if( o.equals( f ) ){
                        indx = i;
                        break;
                    }
                }
            }
            cbox.setSelectedIndex( indx );
        }else{
            for( BGridItem u : unit.children ){
                updateValueCore( subobjs, u );
            }
        }
    }
    
    public void updateValue()
    {
        Object[] objs = (mView != null) ? mView.getSelectedObjects() : new Object[]{};
        for( BGridItem unit : mRoot.children ){
            updateValueCore( objs, unit );
        }
    }
    
    public int getViewingRows()
    {
        return mViewingRows;
    }
    
    public void setContentPane( Container container )
    {
        GridBagConstraints gc = new GridBagConstraints();
        gc.weightx = 1.0D;
        gc.weighty = 1.0D;
        gc.fill = GridBagConstraints.BOTH;
        container.add( mContentPane, gc );
    }

    /**
     * 
     * @param container
     * @param category
     * @param members
     * @param descriptor
     */
    private int prepareCategory(
        int row_start_index,
        String category, 
        Vector<XmlMember> members, 
        PropertyDescriptor descriptor,
        BPropertyGridSplitAdapter mouse_adapter )
    {
        GridBagConstraints g = new GridBagConstraints();
        /*
         |[ + ]|[     category name         ]|
         |[   ]|[prop. name]|[]|[prop. value]|
                     ...      ^
                              | spacer
         */
        // 行カウンター
        int rows = row_start_index;
        
        // カテゴリをエクスパンドするための＋ーボタン
        BPropertyGridExpandMark plus = new BPropertyGridExpandMark( this, true );
        g.gridx = 0;
        g.gridy = rows;
        g.weightx = 0.0D;
        g.fill = GridBagConstraints.BOTH;
        mContentPane.add( plus, g );
        register( plus );
        
        // カテゴリ名を表示するラベル
        BPropertyGridHeaderBase title = new BPropertyGridHeaderBase();
        title.setDrawBackground( true );
        String title_text = category;
        if( title_text == null || (title_text != null && title_text.equals( "" ) ) ){
            title_text = "Other";
        }
        title.setText( title_text );
        title.setPreferredSize( new Dimension( 4, BPropertyGrid.DEFAULT_ROW_HEIGHT ) );
        registPreferredHeightSpecified( title );
        plus.setAdditionalMouseTrigger( title );
        g.gridx = 1;
        g.gridy = rows;
        g.gridwidth = 1;
        g.weightx = 1.0D;
        g.fill = GridBagConstraints.BOTH;
        mContentPane.add( title, g );
        register( title );
        nextRow();
        rows++;
        
        // 各プロパティ毎の処理
        BPanel p = new BPanel();
        p.setLayout( new GridBagLayout() );
        BPanel left = new BPanel();
        BPanel splitter = new BPanel();
        splitter.setPreferredSize( new Dimension( 2, 4 ) );
        splitter.setCursor( new Cursor( Cursor.W_RESIZE_CURSOR | Cursor.E_RESIZE_CURSOR ) );
        splitter.setBackground( BPropertyGrid.COLOR_SEPARATOR );
        BPanel right = new BPanel();
        left.setLayout( new GridBagLayout() );
        right.setLayout( new GridBagLayout() );
        int internal_rows = 0;
        int i = 0;
        for( XmlMember xm : members ){
            internal_rows += prepareRowComponent( this, mRoot, left, right, xm, descriptor, i );
            i++;
        }
        g.gridx = 0;
        g.gridy = 0;
        g.gridwidth = 1;
        g.gridheight = 1;
        g.weightx = 0.0D;
        g.weighty = 0.0D;
        g.fill = GridBagConstraints.BOTH;
        left.setPreferredSize( new Dimension( mView == null ? 60 : mView.getColumnWidth(), 4 ) );
        p.add( left, g );
        g.gridx = 1;
        g.gridy = 0;
        g.gridwidth = 1;
        g.gridheight = 1;
        g.weightx = 0.0D;
        g.weighty = 0.0D;
        g.fill = GridBagConstraints.VERTICAL;
        p.add( splitter, g );
        g.gridx = 2;
        g.gridy = 0;
        g.gridwidth = 1;
        g.gridheight = 1;
        g.weightx = 1.0D;
        g.weighty = 0.0D;
        g.fill = GridBagConstraints.BOTH;
        p.add( right, g );
        GridBagConstraints g_p = new GridBagConstraints();
        g_p.gridx = 1;
        g_p.gridy = rows;
        g_p.gridwidth = 1;
        g_p.gridheight = 1;
        g_p.weightx = 1.0D;
        g_p.weighty = 0.0D;
        g_p.fill = GridBagConstraints.BOTH;
        mContentPane.add( p, g_p );
        GridBagConstraints g_sp = new GridBagConstraints();
        g_sp.gridx = 0;
        g_sp.gridy = rows;
        g_sp.gridwidth = 1;
        g_sp.gridheight = 1;
        g_sp.weightx = 0.0D;
        g_sp.weighty = 0.0D;
        g_sp.fill = GridBagConstraints.BOTH;
        BPropertyGridLeftSpacer sp = new BPropertyGridLeftSpacer( this, plus );
        mContentPane.add( sp, g_sp );
        
        plus.leftSpacerConfig = new BPropertyGridExpandComponentConfig( mContentPane, sp, g_sp );
        plus.leftComponentConfig = new BPropertyGridExpandComponentConfig( mContentPane, p, g_p );
        mouse_adapter.register( left, splitter );
        
        rows++;

        return rows;
    }

    /**
     * 
     */
    private int prepareRowComponent(
        BPropertyGridController controller,
        BGridItem tree,
        Container left,
        Container right,
        XmlMember xm,
        PropertyDescriptor pd,
        int row_index )
    {
        final BGridItem child = new BGridItem();
        child.member = xm;
        for( XmlMember x : tree.memberStack ){
            child.memberStack.add( x );
        }
        if( tree.member != null ){
            child.memberStack.add( tree.member );
        }
        
        String name = xm.getName();
        Class<?> member_type = xm.getType();
        final String cli_name = XmlSerializer.getCliTypeName( member_type );
        // 型コンバータを取得
        child.converter = null;
        TypeConverterAnnotation tca = member_type.getAnnotation( TypeConverterAnnotation.class );
        if( tca != null ){
            try {
                child.converter = (TypeConverter<?>)tca.value().newInstance();
            } catch (InstantiationException e) {
                e.printStackTrace();
                child.converter = null;
            } catch (IllegalAccessException e) {
                e.printStackTrace();
                child.converter = null;
            }
        }

        // 値を入れるテキストボックス(など)
        JComponent comp = null;
        if( child.converter != null ){
            if( child.converter.isStandardValuesSupported() ){
                // 型コンバータがデフォルト値の一覧を提供する場合
                // コンボボックスを表示する
                final BPropertyGridComboBox cbox = new BPropertyGridComboBox( controller );
                Object[] items = child.converter.getStandardValues().toArray();
                for( Object o : items ){
                    cbox.addItem( child.converter.convertTo( o ) );
                }
                cbox.setUnitSize( BPropertyGrid.WIDTH, BPropertyGrid.DEFAULT_ROW_HEIGHT );
                // イベントハンドラ
                cbox.addItemListener( new ItemListener(){
                    @Override
                    public void itemStateChanged(ItemEvent arg0) {
                        int state_change = arg0.getStateChange();
                        int indx = cbox.getSelectedIndex();
                        if( !(indx < 0 && state_change == ItemEvent.DESELECTED) && state_change != ItemEvent.SELECTED ){ 
                            return;
                        }
                        Object[] sels = mView.getSelectedObjects();
                        if( sels == null ){
                            return;
                        }
                        if( sels.length <= 0 ){
                            return;
                        }
                        String value = (String)cbox.getItemAt( indx );
                        Object new_value = child.converter.convertFrom( value );
                        setNewValue( child, sels, new_value );
                    }
                } );
                comp = cbox;
            }else{
                // デフォルト値一覧を提供しない場合
                // 型コンバータが文字列とインスタンスの相互変換機能を提供するので,
                // Viewはテキストフィールドにする
                final JTextField text = new JTextField();
                text.addActionListener( new ActionListener(){
                    @Override
                    public void actionPerformed( ActionEvent arg0 ) {
                        Object[] sels = mView.getSelectedObjects();
                        if( sels == null ){
                            return;
                        }
                        if( sels.length <= 0 ){
                            return;
                        }
                        String value = text.getText();
                        try{
                            // コンバータが例外をスローする場合がある
                            Object new_value = child.converter.convertFrom( value );
                            setNewValue( child, sels, new_value );
                        }catch( Exception ex ){
                        }
                    }
                } );
                comp = text;
            }
        }else if( member_type.isEnum() ){
            final BPropertyGridComboBox cbox = new BPropertyGridComboBox( controller );
            Object[] items = member_type.getEnumConstants();
            for( Object o : items ){
                cbox.addItem( o );
            }
            cbox.setUnitSize( BPropertyGrid.WIDTH, BPropertyGrid.DEFAULT_ROW_HEIGHT );
            cbox.addItemListener( new ItemListener(){
                @Override
                public void itemStateChanged(ItemEvent arg0) {
                    int state_change = arg0.getStateChange();
                    int indx = cbox.getSelectedIndex();
                    if( !(indx < 0 && state_change == ItemEvent.DESELECTED) && state_change != ItemEvent.SELECTED ){ 
                        return;
                    }
                    Object[] sels = mView.getSelectedObjects();
                    if( sels == null ){
                        return;
                    }
                    if( sels.length <= 0 ){
                        return;
                    }
                    // enumの場合，コンボボックスに直接enumの値を代入しているので，
                    // cbox.getItemAtの値をそのまま使ってOK
                    setNewValue( child, sels, cbox.getItemAt( indx ) );
                }
            } );
            comp = cbox;
        }else if( cli_name.equals( "bool" ) ){
            final BPropertyGridComboBox cbox = new BPropertyGridComboBox( controller );
            cbox.addItem( "True" );
            cbox.addItem( "False" );
            cbox.setUnitSize( BPropertyGrid.WIDTH, BPropertyGrid.DEFAULT_ROW_HEIGHT );
            cbox.addItemListener( new ItemListener(){
                @Override
                public void itemStateChanged(ItemEvent arg0) {
                    int state_change = arg0.getStateChange();
                    int indx = cbox.getSelectedIndex();
                    if( !(indx < 0 && state_change == ItemEvent.DESELECTED) && state_change != ItemEvent.SELECTED ){ 
                        return;
                    }
                    Object[] sels = mView.getSelectedObjects();
                    if( sels == null ){
                        return;
                    }
                    if( sels.length <= 0 ){
                        return;
                    }
                    String s = (String)cbox.getItemAt( indx );
                    Boolean v = Boolean.FALSE;
                    if( s.equals( "True" ) ){
                        v = Boolean.TRUE;
                    }
                    setNewValue( child, sels, v );
                }
            } );
            comp = cbox;
        }else if( str.compare( cli_name, "string" ) ||
                  str.compare( cli_name, "double" ) ||
                  str.compare( cli_name, "int" ) ||
                  str.compare( cli_name, "long" ) ||
                  str.compare( cli_name, "short" ) ||
                  str.compare( cli_name, "float" ) ){
            final JTextField text = new JTextField();
            text.addActionListener( new ActionListener(){
                @Override
                public void actionPerformed( ActionEvent arg0 ) {
                    Object[] sels = mView.getSelectedObjects();
                    if( sels == null ){
                        return;
                    }
                    if( sels.length <= 0 ){
                        return;
                    }
                    String value = text.getText();
                    if( str.compare( cli_name, "string" ) ){
                        setNewValue( child, sels, value );
                    }else if( str.compare( cli_name, "double" ) ){
                        try{
                            Double d = Double.parseDouble( value );
                            setNewValue( child, sels, d );
                        }catch( Exception ex ){
                        }
                    }else if( str.compare( cli_name, "int" ) ){
                        try{
                            Integer i = Integer.parseInt( value );
                            setNewValue( child, sels, i );
                        }catch( Exception ex ){
                        }
                    }else if( str.compare( cli_name, "long" ) ){
                        try{
                            Long l = Long.parseLong( value );
                            setNewValue( child, sels, l );
                        }catch( Exception ex ){
                        }
                    }else if( str.compare( cli_name, "short" ) ){
                        try{
                            Short s = Short.parseShort( value );
                            setNewValue( child, sels, s );
                        }catch( Exception ex ){
                        }
                    }else if( str.compare( cli_name, "float" ) ){
                        try{
                            Float f = Float.parseFloat( value );
                            setNewValue( child, sels, f );
                        }catch( Exception ex ){
                        }
                    }
                }
            } );
            comp = text;
        }
        
        GridBagConstraints g = new GridBagConstraints();
        
        int rows = 0;
        // 更なる展開が必要な型かどうかで分岐
        if( comp != null ){
            // 展開の必要なし
            // スペーサ
            JLabel sp = new JLabel();
            sp.setPreferredSize( new Dimension( BPropertyGrid.SPACE, BPropertyGrid.DEFAULT_ROW_HEIGHT ) );
            controller.registPreferredHeightSpecified( sp );
            g.gridx = 0;
            g.gridy = row_index;
            g.gridwidth = 1;
            g.gridheight = 1;
            g.weightx = 0.0D;
            g.weighty = 0.0D;
            g.fill = GridBagConstraints.BOTH;
            left.add( sp, g );
            controller.register( sp );
            // プロパティの名前
            JLabel title = new JLabel();
            title.setText( pd.getDisplayName( name ) );
            title.setPreferredSize( new Dimension( 4, BPropertyGrid.DEFAULT_ROW_HEIGHT ) );
            controller.registPreferredHeightSpecified( title );
            g.gridx = 1;
            g.gridy = row_index;
            g.gridwidth = 1;
            g.gridheight = 1;
            g.weightx = 1.0D;
            g.weighty = 0.0D;
            g.fill = GridBagConstraints.BOTH;
            left.add( title, g );
            controller.register( title );
            // 値を格納するコンポーネント
            comp.setPreferredSize( new Dimension( 4, BPropertyGrid.DEFAULT_ROW_HEIGHT ) );
            comp.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
            controller.registPreferredHeightSpecified( comp );
            g.gridx = 0;
            g.gridy = row_index;
            g.gridwidth = 1;
            g.gridheight = 1;
            g.weightx = 1.0D;
            g.weighty = 0.0D;
            g.fill = GridBagConstraints.BOTH;
            right.add( comp, g );
            controller.register( comp );
            rows++;
            controller.nextRow();
            child.editor = comp;
        }else{
            BPanel i_left = new BPanel();
            BPanel i_right = new BPanel();
            i_left.setLayout( new GridBagLayout() );
            i_right.setLayout( new GridBagLayout() );
            g.gridx = 0;
            g.gridy = row_index;
            g.gridwidth = 2;
            g.gridheight = 1;
            g.weightx = 1.0D;
            g.weighty = 1.0D;
            g.fill = GridBagConstraints.BOTH;
            left.add( i_left, g );
            g.gridx = 0;
            g.gridy = row_index;
            g.gridwidth = 1;
            g.gridheight = 1;
            g.weightx = 1.0D;
            g.weighty = 1.0D;
            g.fill = GridBagConstraints.BOTH;
            right.add( i_right, g );
            /*        i_left                  i_right
             |[n_sp]|[n_plus]|[n_title]|    |[n_value]|
             |      |[     n_left     ]|    |[n_right]|
             */
            BPanel n_left = new BPanel();
            BPanel n_right = new BPanel();
            n_left.setLayout( new GridBagLayout() );
            n_right.setLayout( new GridBagLayout() );
            Class<?> n_cls = xm.getType();
            XmlMember[] n_member = XmlMember.extractMembers( n_cls );
            PropertyDescriptor n_pd = getDescriptor( n_cls );
            // 左側
            // ▶
            BPropertyGridExpandMark n_plus = new BPropertyGridExpandMark( controller, false );
            child.expandMark = n_plus;
            g.gridx = 0;
            g.gridy = 0;
            g.gridwidth = 1;
            g.gridheight = 1;
            g.weightx = 0.0D;
            g.weighty = 0.0D;
            g.fill = GridBagConstraints.NONE;
            i_left.add( n_plus, g );
            controller.register( n_plus );
            // プロパティ名
            JLabel n_title = new JLabel();
            n_title.setText( n_pd.getDisplayName( name ) );
            n_title.setPreferredSize( new Dimension( 4, BPropertyGrid.DEFAULT_ROW_HEIGHT ) );
            controller.registPreferredHeightSpecified( n_title );
            n_plus.setAdditionalMouseTrigger( n_title );
            g.gridx = 1;
            g.gridy = 0;
            g.gridwidth = 1;
            g.gridheight = 1;
            g.weightx = 1.0D;
            g.weighty = 0.0D;
            g.fill = GridBagConstraints.BOTH;
            i_left.add( n_title, g );
            controller.register( n_title );
            // 右側
            JLabel n_value = new JLabel();
            n_value.setPreferredSize( new Dimension( 4, BPropertyGrid.DEFAULT_ROW_HEIGHT ) );
            controller.registPreferredHeightSpecified( n_value );
            g.gridx = 0;
            g.gridy = 0;
            g.gridwidth = 1;
            g.gridheight = 1;
            g.weightx = 1.0D;
            g.weighty = 0.0D;
            g.fill = GridBagConstraints.BOTH;
            i_right.add( n_value, g );
            controller.register( n_value );
            // サブプロパティ用のプロパティ名
            GridBagConstraints g_sp = new GridBagConstraints();
            BPropertyGridLeftSpacer sp = new BPropertyGridLeftSpacer( controller, n_plus );
            sp.setPreferredSize( new Dimension( 4, BPropertyGrid.DEFAULT_ROW_HEIGHT ) );
            controller.registPreferredHeightSpecified( sp );
            g_sp.gridx = 0;
            g_sp.gridy = 1;
            g_sp.gridwidth = 1;
            g_sp.gridheight = 1;
            g_sp.weightx = 0.0D;
            g_sp.weighty = 0.0D;
            g_sp.fill = GridBagConstraints.BOTH;
            i_left.add( sp, g_sp );
            GridBagConstraints g_n_left = new GridBagConstraints();
            g_n_left.gridx = 1;
            g_n_left.gridy = 1;
            g_n_left.gridwidth = 1;
            g_n_left.gridheight = 1;
            g_n_left.weightx = 1.0D;
            g_n_left.weighty = 0.0D;
            g_n_left.fill = GridBagConstraints.BOTH;
            i_left.add( n_left, g_n_left );
            // サブプロパティ用の値コンポーネント
            GridBagConstraints g_n_right = new GridBagConstraints();
            g_n_right.gridx = 0;
            g_n_right.gridy = 1;
            g_n_right.gridwidth = 1;
            g_n_right.gridheight = 1;
            g_n_right.weightx = 1.0D;
            g_n_right.weighty = 0.0D;
            g_n_right.fill = GridBagConstraints.BOTH;
            i_right.add( n_right, g_n_right );
            n_plus.leftComponentConfig = new BPropertyGridExpandComponentConfig( i_left, n_left, g_n_left );
            n_plus.rightComponentConfig = new BPropertyGridExpandComponentConfig( i_right, n_right, g_n_right );
            n_plus.leftSpacerConfig = new BPropertyGridExpandComponentConfig( i_left, sp, g_sp );
            // サブアイテム
            controller.nextRow();
            int i = 0;
            for( XmlMember n_xm : n_member ){
                if( n_xm.getType().equals( xm.getType() ) ){
                    continue;
                }
                rows += prepareRowComponent( controller, child, n_left, n_right, n_xm, n_pd, i ); 
                i++;
            }
            child.editor = n_value;
            rows++;
        }
        tree.children.add( child );

        return rows;
    }

    private void setNewValue( BGridItem unit, Object[] dst, Object new_value )
    {
        if( dst == null ){
            return;
        }
        if( dst.length <= 0 ){
            return;
        }
        Object[] old_value_collection = new Object[dst.length];
        for( int i = 0; i < dst.length; i++ ){
            Object o = dst[i];
            Object target = o;
            for( XmlMember xm : unit.memberStack ){
                target = xm.get( target );
            }
            old_value_collection[i] = unit.member.get( target );
            unit.member.set( target, new_value );
        }

        // イベントを発動
        BPropertyValueChangedEventArgs e = new BPropertyValueChangedEventArgs();
        e.GridItem = unit;
        Object f = null;
        for( Object o : old_value_collection ){
            if( o == null ){
                continue;
            }
            if( f == null ){
                f = o;
            }else{
                if( !o.equals( f ) ){
                    f = null;
                    break;
                }
            }
        }
        e.OldValue = f;
        try{
            mView.propertyValueChangedEvent.raise( mView, e );
        }catch( Exception ex ){
            ex.printStackTrace();
        }
    }
    
    /**
     * 指定した型のプロパティ記述子を取得します
     * @param cls 対象の型
     * @return プロパティ記述子
     */
    private static PropertyDescriptor getDescriptor( Class<?> cls )
    {
        PropertyDescriptor ret = null;
        if( IPropertyDescriptor.class.isAssignableFrom( cls ) ){
            try{
                IPropertyDescriptor ipd = (IPropertyDescriptor)cls.newInstance();
                ret = ipd.getDescriptor();
            }catch( Exception ex ){
                ret = null;
            }
        }
        if( ret == null ){
            ret = new PropertyDescriptor();
        }
        return ret;
    }

    public int applyColumnWidth( int value )
    {
        // -4は，BPropertyGridComboBoxのドロップダウンボタンの最小幅
        if( mView == null ){
            return value;
        }
        if( !mView.isShowing() ){
            return value;
        }
        if( mView.getWidth() - 4 < value ){
            value = mView.getWidth() - 4;
        }
        if( value < 0 ){
            value = 0;
        }
        for( Component c : mLefts ){
            c.setPreferredSize( new Dimension( value, 4 ) );
        }
        doLayoutRecurse( mView );
        return value;
    }

    public void setView( BPropertyGrid view )
    {
        mView = view;
    }
    
    public BPropertyGrid getView()
    {
        return mView;
    }

    public void registSplitter( Component comp )
    {
        mSplitters.add( comp );
    }
    
    public void registLeft( Component comp )
    {
        mLefts.add( comp );
    }
    
    /**
     * コンポーネントが何行目に表示されるかに応じて、コンポーネントの背景色を設定します
     * @param force コンポーネントが非表示であっても強制的に設定する場合にtrue、そうでなければfalseを指定します。
     */
    public void updateBackground( boolean force )
    {
        int actrow = -1;
        for( Iterator<Integer> itr = mComponents.keySet().iterator(); itr.hasNext(); ){
            int row = (int)itr.next();
            boolean visible = false;
            if( force ){
                visible = true;
            }else{
                // 見えてるアイテムが一つでもあるかどうか
                for( Component comp : mComponents.get( row ) ){
                    if( comp.isShowing() ){
                        visible = true;
                        break;
                    }
                }
            }
            if( !visible ){
                continue;
            }
            actrow++;
            Color c = actrow % 2 == 0 ? mView.getBackgroundEven() : mView.getBackgroundOdd();
            for( Component comp : mComponents.get( row ) ){
                if( comp instanceof JLabel ){
                    ((JLabel)comp).setOpaque( true );
                }
                if( comp instanceof BPropertyGridExpandMark ){
                    ((BPropertyGridExpandMark)comp).actualRowIndex = actrow;
                }
                if( comp instanceof BPropertyGridHeaderBase ){
                    if( ((BPropertyGridHeaderBase)comp).isDrawBackground() ){
                        continue;
                    }
                }
                comp.setBackground( c );
            }
        }
        mViewingRows = actrow + 1;
    }

    public void nextRow()
    {
        mRowCounter++;
    }
    
    public void register( Component comp )
    {
        Vector<Component> target = null;
        if( mComponents.containsKey( mRowCounter ) ){
            target = mComponents.get( mRowCounter );
        }else{
            target = new Vector<Component>();
            mComponents.put( mRowCounter, target );
        }
        target.add( comp );
    }

    public void updateView()
    {
        updateBackground( false );
        // 高さの+1は最下段のスペーサの分
        mView.setPreferredSize( new Dimension( 4, mViewingRows * BPropertyGrid.DEFAULT_ROW_HEIGHT + 1 ) );
        doLayoutRecurse( mView );
        mView.revalidate();
    }

    public static void doLayoutRecurse( Container root )
    {
        root.doLayout();
        for( Component c : root.getComponents() ){
            if( c == null ) continue;
            if( c instanceof Container ){
                Container ci = (Container)c;
                doLayoutRecurse( ci );
            }
        }
    }

    public int getColumnWidthFromView() {
        if( mView != null ){
            return mView.getColumnWidth();
        }else{
            return 60;
        }
    }
}

class BPropertyGridSplitAdapter implements MouseMotionListener, MouseListener
{
    private int mInitX;
    private int mInitWidth;
    private BPropertyGridController mController;
    
    public BPropertyGridSplitAdapter( BPropertyGridController controller )
    {
        mController = controller;
    }
    
    public void register( Component left, Component splitter )
    {
        mController.registSplitter( splitter );
        mController.registLeft( left );
        splitter.addMouseListener( this );
        splitter.addMouseMotionListener( this );
    }

    @Override
    public void mouseClicked( MouseEvent arg0 )
    {
    }

    @Override
    public void mouseEntered( MouseEvent arg0 )
    {
    }

    @Override
    public void mouseExited( MouseEvent arg0 )
    {
    }

    @Override
    public void mousePressed( MouseEvent arg0 )
    {
        mInitX = MouseInfo.getPointerInfo().getLocation().x;
        mInitWidth = mController.getColumnWidthFromView();
    }

    @Override
    public void mouseReleased( MouseEvent arg0 )
    {
    }

    @Override
    public void mouseDragged( MouseEvent arg0 )
    {
        int x = MouseInfo.getPointerInfo().getLocation().x;
        int delta = x - mInitX;
        int draft = mInitWidth + delta;
        mController.notifyColumnWidthToView( draft );
    }

    @Override
    public void mouseMoved( MouseEvent arg0 )
    {
    }
}

class BPropertyGridExpandComponentConfig
{
    public Component foldComponent;
    public Container foldContainer;
    public GridBagConstraints constraints;

    public BPropertyGridExpandComponentConfig( Container container, Component component, GridBagConstraints constraints )
    {
        this.foldContainer = container;
        this.foldComponent = component;
        this.constraints = constraints;
    }
    
    public void fold()
    {
        if( foldContainer != null && foldComponent != null ){
            foldContainer.remove( foldComponent );
        }
    }
    
    public void expand()
    {
        if( foldContainer != null && foldComponent != null && constraints != null ){
            foldContainer.add( foldComponent, constraints );
        }
    }
}

class BPropertyGridBottomSpacer extends JPanel
{
    private static final long serialVersionUID = -6766316508357991559L;

    private BPropertyGridController mController;
    
    public BPropertyGridBottomSpacer( BPropertyGridController controller )
    {
        mController = controller;
    }
    
    @Override
    public void paint( Graphics g )
    {
        int indx = mController.getViewingRows();
        BPropertyGrid view = mController.getView();
        int row_height = BPropertyGrid.DEFAULT_ROW_HEIGHT;
        if( view != null ){
            row_height = view.getRowHeight();
        }
        int num = getHeight() / row_height + 1;
        int width = getWidth();
        Color even = (view == null) ? BPropertyGrid.DEFAULT_BACKGROUND_EVEN : view.getBackgroundEven();
        Color odd = (view == null) ? BPropertyGrid.DEFAULT_BACKGROUND_ODD : view.getBackgroundOdd();
        for( int i = 0; i < num; i++ ){
            g.setColor( (indx % 2 == 0) ? even : odd );
            g.fillRect( 0, i * row_height, width, row_height );
            indx++;
        }
    }
}

class BPropertyGridHeaderBase extends JPanel
{
    private static final long serialVersionUID = 6794528401369651629L;
    private static final String PANGRAM = "cozy lummox gives smart squid who asks for job pen. 01234567890 THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS.";

    private Color COL_UPPER = new Color( 229, 232, 241 );
    private String mText = "";
    private int mFontHeight = -1;
    private int mFontOffset = 0;
    private boolean mIsDrawBackground = false;
    private Image mImage = null;

    public BPropertyGridHeaderBase()
    {
        setOpaque( true );
    }
    
    public boolean isDrawBackground()
    {
        return mIsDrawBackground;
    }

    public void setIcon( Image value )
    {
        mImage = value;
    }
    
    public void setDrawBackground( boolean value )
    {
        mIsDrawBackground = value;
    }
    
    /**
     * 指定された文字列を指定されたフォントで描画したときのサイズを計測します。
     * @param text 
     * @param font 
     * @return 
     */
    public static Dimension measureString( String text, Font font )
    {
        BufferedImage dumy = new BufferedImage( 1, 1, BufferedImage.TYPE_INT_BGR );
        Graphics2D g = dumy.createGraphics();
        g.setFont( font );
        FontMetrics fm = g.getFontMetrics();
        Dimension ret = new Dimension( fm.stringWidth( text ), fm.getHeight() );
        g = null;
        dumy = null;
        return ret;
    }

    /**
     * 指定したフォントを描画するとき、描画指定したy座標と、描かれる文字の中心線のズレを調べます
     * @param font 
     * @return 
     */
    private static int getStringDrawOffset( java.awt.Font font )
    {
        int ret = 0;
        java.awt.Dimension size = measureString( PANGRAM, font );
        if ( size.height <= 0 ) {
            return 0;
        }
        java.awt.image.BufferedImage b = null;
        java.awt.Graphics2D g = null;
        java.awt.image.BufferedImage b2 = null;
        try {
            int string_desty = size.height * 2; // 文字列が書き込まれるy座標
            int w = size.width * 4;
            int h = size.height * 4;
            b = new java.awt.image.BufferedImage( w, h, java.awt.image.BufferedImage.TYPE_INT_BGR );
            g = b.createGraphics();
            g.setColor( java.awt.Color.white );
            g.fillRect( 0, 0, w, h );
            g.setFont( font );
            g.setColor( java.awt.Color.black );
            g.drawString( PANGRAM, size.width, string_desty );

            b2 = b;
            // 上端に最初に現れる色つきピクセルを探す
            int firsty = 0;
            boolean found = false;
            for ( int y = 0; y < h; y++ ) {
                for ( int x = 0; x < w; x++ ) {
                    int ic = b2.getRGB( x, y );
                    Color c = new Color( ic );
                    if ( c.getRed() != 255 || c.getGreen() != 255 || c.getBlue() != 255 ) {
                        found = true;
                        firsty = y;
                        break;
                    }
                }
                if ( found ) {
                    break;
                }
            }

            // 下端
            int endy = h - 1;
            found = false;
            for ( int y = h - 1; y >= 0; y-- ) {
                for ( int x = 0; x < w; x++ ) {
                    int ic = b2.getRGB( x, y );
                    Color c = new Color( ic );
                    if ( c.getRed() != 255 || c.getGreen() != 255 || c.getBlue() != 255 ) {
                        found = true;
                        endy = y;
                        break;
                    }
                }
                if ( found ) {
                    break;
                }
            }

            int center = (firsty + endy) / 2;
            ret = center - string_desty;
        } catch ( Exception ex ) {
            System.err.println( "Util#getStringDrawOffset; ex=" + ex );
        } finally {
        }
        return ret;
    }

    public void setText( String value )
    {
        mText = value;
    }
    
    public void setFont( Font f )
    {
        super.setFont( f );
        mFontHeight = measureString( PANGRAM, f ).height;
        mFontOffset = getStringDrawOffset( f );
    }
    
    @Override
    public void paint( Graphics g )
    {
        int w = getWidth();
        int h = getHeight();
        if( mIsDrawBackground ){
            g.setColor( COL_UPPER );
            g.fillRect( 0, 0, w, h / 2 );
            double delta = h / 2.0;
            for( int i = 0; i < h / 2; i++ ){
                int r = (int)(196 + (218 - 196) / delta * i);
                int gr = (int)(204 + (222 - 204) / delta * i);
                int b = (int)(218 + (233 - 218) / delta * i);
                g.setColor( new Color( r, gr, b ) );
                g.drawLine( 0, h / 2 + i, w, h / 2 + i );
            }
            g.setColor( new Color( 143, 143, 143 ) );
            g.drawLine( 0, 0, w, 0 );
            g.drawLine( 0, h - 1, w, h - 1 );
            g.setColor( new Color( 70, 70, 70 ) );
            if( mFontHeight < 0 ){
                setFont( getFont() );
            }
            g.drawString( mText, 0, h / 2 - mFontOffset );
        }else{
            super.paint( g );
        }
        if( mImage != null ){
            g.drawImage( mImage, 0, h / 2 - mImage.getHeight( null ) / 2, null );
        }
    }
}

class BPropertyGridLeftSpacer extends JPanel
{
    private static final long serialVersionUID = 9187548066947509485L;

    private BPropertyGridExpandMark mOwner = null;
    private BPropertyGridController mController = null;
    
    public BPropertyGridLeftSpacer( BPropertyGridController controller, BPropertyGridExpandMark owner )
    {
        mController = controller;
        mOwner = owner;
    }
    
    @Override
    public void paint( Graphics g )
    {
        int indx = 0;
        if( mOwner != null ){
            indx = mOwner.actualRowIndex;
        }
        int height = this.getHeight();
        int width = this.getWidth();
        BPropertyGrid view = mController.getView();
        int row_height = BPropertyGrid.DEFAULT_ROW_HEIGHT;
        if( view != null ){
            row_height = view.getRowHeight();
        }
        int num_row = (height + 1) / row_height;
        Color even = (view == null) ? BPropertyGrid.DEFAULT_BACKGROUND_EVEN : view.getBackgroundEven();
        Color odd = (view == null) ? BPropertyGrid.DEFAULT_BACKGROUND_ODD : view.getBackgroundOdd();
        for( int i = 0; i < num_row; i++ ){
            indx++;
            g.setColor( (indx % 2 == 0) ? even : odd );
            g.fillRect( 0, i * row_height, width, row_height );
        }
    }
}

class BPropertyGridExpandMark extends BPropertyGridHeaderBase
{
    private static final long serialVersionUID = 9089772038220944897L;

    public BPropertyGridExpandComponentConfig leftComponentConfig;
    public BPropertyGridExpandComponentConfig rightComponentConfig;
    public BPropertyGridExpandComponentConfig leftSpacerConfig;
    private boolean mIsExpand = true;
    private Image mIconFold;
    private Image mIconExpand;
    private BPropertyGridController mController;
    public int actualRowIndex = 0;
    private Component mAdditionalMouseTrigger = null;
    private MouseListener mMouseListener = null;

   /**
    * ▶▼などと書かれたラベルを作る
    */
   public BPropertyGridExpandMark( final BPropertyGridController controller, boolean draw_background )
   {
       super();
       setDrawBackground( draw_background );
       mController = controller;
       mIconFold = getImageFold();
       mIconExpand = getImageExpand();
       setIcon( mIconExpand );
       this.setPreferredSize( new Dimension( BPropertyGrid.SPACE, BPropertyGrid.DEFAULT_ROW_HEIGHT ) );
       mController.registPreferredHeightSpecified( this );
       this.addMouseListener( new MouseAdapter(){
           @Override
           public void mouseClicked( MouseEvent e )
           {
               mIsExpand = !mIsExpand;
               if( mIsExpand ){
                   if( leftSpacerConfig != null ){
                       leftSpacerConfig.expand();
                   }
                   if( leftComponentConfig != null ){
                       leftComponentConfig.expand();
                   }
                   if( rightComponentConfig != null ){
                       rightComponentConfig.expand();
                   }
                   setIcon( mIconExpand );
               }else{
                   if( leftSpacerConfig != null ){
                       leftSpacerConfig.fold();
                   }
                   if( leftComponentConfig != null ){
                       leftComponentConfig.fold();
                   }
                   if( rightComponentConfig != null ){
                       rightComponentConfig.fold();
                   }
                   setIcon( mIconFold );
               }
               handleExpandStateChange();
           }
       } );
   }

   public boolean isExpanded()
   {
       return mIsExpand;
   }
   
   @Override
   public void addMouseListener( MouseListener l )
   {
       mMouseListener = l;
       super.addMouseListener( l );
       if( mAdditionalMouseTrigger != null ){
           mAdditionalMouseTrigger.addMouseListener( mMouseListener );
       }
   }
   
   public void setAdditionalMouseTrigger( Component comp )
   {
       if( mAdditionalMouseTrigger != null && mMouseListener != null ){
           mAdditionalMouseTrigger.removeMouseListener( mMouseListener );
       }
       mAdditionalMouseTrigger = comp;
       if( mMouseListener != null ){
           mAdditionalMouseTrigger.addMouseListener( mMouseListener );
       }
   }
   
   private void handleExpandStateChange()
   {
       mController.updateView();
       this.repaint();
   }
   
   private static Image getImageExpand()
   {
       try{
           return ImageIO.read( new ByteArrayInputStream( new byte[]{
               (byte)137, (byte)80,  (byte)78,  (byte)71,  (byte)13,  (byte)10,  (byte)26,  (byte)10,  (byte)0,   (byte)0,
               (byte)0,   (byte)13,  (byte)73,  (byte)72,  (byte)68,  (byte)82,  (byte)0,   (byte)0,   (byte)0,   (byte)9,
               (byte)0,   (byte)0,   (byte)0,   (byte)11,  (byte)8,   (byte)6,   (byte)0,   (byte)0,   (byte)0,   (byte)173,
               (byte)89,  (byte)167, (byte)27,  (byte)0,   (byte)0,   (byte)0,   (byte)1,   (byte)115, (byte)82,  (byte)71,
               (byte)66,  (byte)0,   (byte)174, (byte)206, (byte)28,  (byte)233, (byte)0,   (byte)0,   (byte)0,   (byte)4,
               (byte)103, (byte)65,  (byte)77,  (byte)65,  (byte)0,   (byte)0,   (byte)177, (byte)143, (byte)11,  (byte)252,
               (byte)97,  (byte)5,   (byte)0,   (byte)0,   (byte)0,   (byte)32,  (byte)99,  (byte)72,  (byte)82,  (byte)77,
               (byte)0,   (byte)0,   (byte)122, (byte)38,  (byte)0,   (byte)0,   (byte)128, (byte)132, (byte)0,   (byte)0,
               (byte)250, (byte)0,   (byte)0,   (byte)0,   (byte)128, (byte)232, (byte)0,   (byte)0,   (byte)117, (byte)48,
               (byte)0,   (byte)0,   (byte)234, (byte)96,  (byte)0,   (byte)0,   (byte)58,  (byte)152, (byte)0,   (byte)0,
               (byte)23,  (byte)112, (byte)156, (byte)186, (byte)81,  (byte)60,  (byte)0,   (byte)0,   (byte)0,   (byte)162,
               (byte)73,  (byte)68,  (byte)65,  (byte)84,  (byte)40,  (byte)83,  (byte)99,  (byte)252, (byte)255, (byte)255,
               (byte)63,  (byte)3,   (byte)65,  (byte)240, (byte)231, (byte)207, (byte)31,  (byte)118, (byte)66,  (byte)152,
               (byte)97,  (byte)209, (byte)162, (byte)69,  (byte)207, (byte)102, (byte)204, (byte)152, (byte)241, (byte)31,
               (byte)23,  (byte)94,  (byte)177, (byte)98,  (byte)197, (byte)117, (byte)134, (byte)59,  (byte)119, (byte)238,
               (byte)132, (byte)225, (byte)83,  (byte)244, (byte)244, (byte)233, (byte)83,  (byte)7,   (byte)6,   (byte)144,
               (byte)155, (byte)182, (byte)108, (byte)217, (byte)178, (byte)19,  (byte)155, (byte)194, (byte)189, (byte)123,
               (byte)247, (byte)46,  (byte)6,   (byte)187, (byte)25,  (byte)68,  (byte)124, (byte)248, (byte)240, (byte)65,
               (byte)117, (byte)214, (byte)172, (byte)89,  (byte)191, (byte)144, (byte)21,  (byte)206, (byte)155, (byte)55,
               (byte)239, (byte)195, (byte)215, (byte)175, (byte)95,  (byte)37,  (byte)224, (byte)138, (byte)64,  (byte)140,
               (byte)51,  (byte)103, (byte)206, (byte)212, (byte)33,  (byte)43,  (byte)186, (byte)114, (byte)229, (byte)74,
               (byte)22,  (byte)72,  (byte)28,  (byte)69,  (byte)209, (byte)239, (byte)223, (byte)191, (byte)57,  (byte)151,
               (byte)45,  (byte)91,  (byte)118, (byte)7,   (byte)164, (byte)112, (byte)237, (byte)218, (byte)181, (byte)167,
               (byte)255, (byte)253, (byte)251, (byte)199, (byte)132, (byte)161, (byte)8,   (byte)36,  (byte)240, (byte)232,
               (byte)209, (byte)35,  (byte)247, (byte)153, (byte)51,  (byte)103, (byte)254, (byte)121, (byte)245, (byte)234,
               (byte)149, (byte)49,  (byte)76,  (byte)1,   (byte)138, (byte)73,  (byte)48,  (byte)193, (byte)7,   (byte)15,
               (byte)30,  (byte)248, (byte)32,  (byte)43,  (byte)192, (byte)170, (byte)8,   (byte)93,  (byte)1,   (byte)136,
               (byte)15,  (byte)0,   (byte)40,  (byte)52,  (byte)8,   (byte)62,  (byte)28,  (byte)169, (byte)13,  (byte)232,
               (byte)0,   (byte)0,   (byte)0,   (byte)0,   (byte)73,  (byte)69,  (byte)78,  (byte)68,  (byte)174, (byte)66,
               (byte)96,  (byte)130, } ) );
       }catch( Exception ex ){
           return null;
       }
   }

   private static Image getImageFold()
   {
       try{
           return ImageIO.read( new ByteArrayInputStream( new byte[]{
               (byte)137, (byte)80,  (byte)78,  (byte)71,  (byte)13,  (byte)10,  (byte)26,  (byte)10,  (byte)0,   (byte)0,
               (byte)0,   (byte)13,  (byte)73,  (byte)72,  (byte)68,  (byte)82,  (byte)0,   (byte)0,   (byte)0,   (byte)9,
               (byte)0,   (byte)0,   (byte)0,   (byte)11,  (byte)8,   (byte)6,   (byte)0,   (byte)0,   (byte)0,   (byte)173,
               (byte)89,  (byte)167, (byte)27,  (byte)0,   (byte)0,   (byte)0,   (byte)1,   (byte)115, (byte)82,  (byte)71,
               (byte)66,  (byte)0,   (byte)174, (byte)206, (byte)28,  (byte)233, (byte)0,   (byte)0,   (byte)0,   (byte)4,
               (byte)103, (byte)65,  (byte)77,  (byte)65,  (byte)0,   (byte)0,   (byte)177, (byte)143, (byte)11,  (byte)252,
               (byte)97,  (byte)5,   (byte)0,   (byte)0,   (byte)0,   (byte)32,  (byte)99,  (byte)72,  (byte)82,  (byte)77,
               (byte)0,   (byte)0,   (byte)122, (byte)38,  (byte)0,   (byte)0,   (byte)128, (byte)132, (byte)0,   (byte)0,
               (byte)250, (byte)0,   (byte)0,   (byte)0,   (byte)128, (byte)232, (byte)0,   (byte)0,   (byte)117, (byte)48,
               (byte)0,   (byte)0,   (byte)234, (byte)96,  (byte)0,   (byte)0,   (byte)58,  (byte)152, (byte)0,   (byte)0,
               (byte)23,  (byte)112, (byte)156, (byte)186, (byte)81,  (byte)60,  (byte)0,   (byte)0,   (byte)0,   (byte)130,
               (byte)73,  (byte)68,  (byte)65,  (byte)84,  (byte)40,  (byte)83,  (byte)99,  (byte)252, (byte)255, (byte)255,
               (byte)63,  (byte)3,   (byte)16,  (byte)51,  (byte)49,  (byte)50,  (byte)50,  (byte)254, (byte)99,  (byte)192,
               (byte)5,   (byte)254, (byte)252, (byte)249, (byte)195, (byte)190, (byte)116, (byte)233, (byte)210, (byte)59,
               (byte)15,  (byte)31,  (byte)62,  (byte)244, (byte)132, (byte)106, (byte)0,   (byte)105, (byte)66,  (byte)193,
               (byte)12,  (byte)32,  (byte)69,  (byte)51,  (byte)102, (byte)204, (byte)248, (byte)3,   (byte)194, (byte)59,
               (byte)118, (byte)236, (byte)88,  (byte)247, (byte)233, (byte)211, (byte)39,  (byte)121, (byte)188, (byte)138,
               (byte)64,  (byte)10,  (byte)103, (byte)207, (byte)158, (byte)253, (byte)245, (byte)194, (byte)133, (byte)11,
               (byte)197, (byte)127, (byte)255, (byte)254, (byte)101, (byte)129, (byte)41,  (byte)70,  (byte)49,  (byte)9,
               (byte)102, (byte)34,  (byte)136, (byte)94,  (byte)181, (byte)106, (byte)213, (byte)133, (byte)231, (byte)207,
               (byte)159, (byte)91,  (byte)129, (byte)20,  (byte)226, (byte)84,  (byte)4,   (byte)82,  (byte)56,  (byte)125,
               (byte)250, (byte)244, (byte)255, (byte)15,  (byte)30,  (byte)60,  (byte)240, (byte)33,  (byte)207, (byte)36,
               (byte)130, (byte)110, (byte)194, (byte)235, (byte)59,  (byte)130, (byte)225, (byte)4,   (byte)114, (byte)253,
               (byte)191, (byte)127, (byte)255, (byte)152, (byte)112, (byte)5,   (byte)36,  (byte)72,  (byte)28,  (byte)0,
               (byte)0,   (byte)152, (byte)11,  (byte)213, (byte)45,  (byte)224, (byte)5,   (byte)144, (byte)0,   (byte)0,
               (byte)0,   (byte)0,   (byte)73,  (byte)69,  (byte)78,  (byte)68,  (byte)174, (byte)66,  (byte)96,  (byte)130,} ) );
       }catch( Exception ex ){
           return null;
       }
   }
}

class BPropertyGridComboBoxLabel extends JLabel
{
    /**
     * 現在表示している値
     */
    private Object mViewing = null;
    
    /**
     * 現在このラベルが表示している文字列の元となっているオブジェクトを取得します
     */
    public Object getViewing()
    {
        return mViewing;
    }
    
    /**
     * 現在このラベルが表示している文字列の元となっているオブジェクトを設定します
     */
    public void setViewing( Object value )
    {
        mViewing = value;
    }
}

/**
 * 小さなコンボボックス
 */
class BPropertyGridComboBox extends JPanel
                            implements ActionListener, ItemSelectable
{
    private static final long serialVersionUID = -6834563484239011621L;
    private BPropertyGridComboBoxLabel labelValue = null;
    private JButton buttonDropdown = null;
    private BPropertyGridComboBoxDropdown windowDropdown = null;  //  @jve:decl-index=0:visual-constraint="184,55"
    private Vector<ItemListener> mItemListeners = new Vector<ItemListener>();
    private int mSelectedIndex = -1;
    private BPropertyGridController mController = null;

    /**
     * This method initializes 
     * 
     */
    public BPropertyGridComboBox( BPropertyGridController controller )
    {
        super();
        mController = controller;
        initialize();
        getWindowDropdown();
        this.addComponentListener( new ComponentListener(){
            @Override
            public void componentHidden( ComponentEvent arg0 )
            {
            }

            @Override
            public void componentMoved( ComponentEvent arg0 )
            {
            }

            @Override
            public void componentResized( ComponentEvent arg0 )
            {
                if( windowDropdown.isVisible() ){
                    windowDropdown.setSize( getWidth(), windowDropdown.getHeight() );
                }
            }

            @Override
            public void componentShown( ComponentEvent arg0 )
            {
            }
        });
        labelValue.addMouseListener( new MouseListener(){
            @Override
            public void mouseClicked( MouseEvent arg0 )
            {
                if( getWindowDropdown().isVisible() ){
                    //hideDropdown();
                }else{
                    //showDropdown();
                }
            }

            @Override
            public void mouseEntered( MouseEvent arg0 )
            {
            }

            @Override
            public void mouseExited( MouseEvent arg0 )
            {
            }

            @Override
            public void mousePressed( MouseEvent arg0 )
            {
                if( getWindowDropdown().isVisible() ){
                    hideDropdown();
                }else{
                    showDropdown();
                }
            }

            @Override
            public void mouseReleased( MouseEvent arg0 )
            {
            }
        } );
    }

    private Image ddown2()
    {
        try{
            return ImageIO.read( new ByteArrayInputStream( new byte[]{
                (byte)137, (byte)80,  (byte)78,  (byte)71,  (byte)13,  (byte)10,  (byte)26,  (byte)10,  (byte)0,   (byte)0,
                (byte)0,   (byte)13,  (byte)73,  (byte)72,  (byte)68,  (byte)82,  (byte)0,   (byte)0,   (byte)0,   (byte)7,
                (byte)0,   (byte)0,   (byte)0,   (byte)6,   (byte)8,   (byte)6,   (byte)0,   (byte)0,   (byte)0,   (byte)15,
                (byte)14,  (byte)132, (byte)118, (byte)0,   (byte)0,   (byte)0,   (byte)1,   (byte)115, (byte)82,  (byte)71,
                (byte)66,  (byte)0,   (byte)174, (byte)206, (byte)28,  (byte)233, (byte)0,   (byte)0,   (byte)0,   (byte)4,
                (byte)103, (byte)65,  (byte)77,  (byte)65,  (byte)0,   (byte)0,   (byte)177, (byte)143, (byte)11,  (byte)252,
                (byte)97,  (byte)5,   (byte)0,   (byte)0,   (byte)0,   (byte)32,  (byte)99,  (byte)72,  (byte)82,  (byte)77,
                (byte)0,   (byte)0,   (byte)122, (byte)38,  (byte)0,   (byte)0,   (byte)128, (byte)132, (byte)0,   (byte)0,
                (byte)250, (byte)0,   (byte)0,   (byte)0,   (byte)128, (byte)232, (byte)0,   (byte)0,   (byte)117, (byte)48,
                (byte)0,   (byte)0,   (byte)234, (byte)96,  (byte)0,   (byte)0,   (byte)58,  (byte)152, (byte)0,   (byte)0,
                (byte)23,  (byte)112, (byte)156, (byte)186, (byte)81,  (byte)60,  (byte)0,   (byte)0,   (byte)0,   (byte)100,
                (byte)73,  (byte)68,  (byte)65,  (byte)84,  (byte)24,  (byte)87,  (byte)99,  (byte)252, (byte)255, (byte)255,
                (byte)63,  (byte)3,   (byte)78,  (byte)112, (byte)247, (byte)238, (byte)93,  (byte)199, (byte)230, (byte)230,
                (byte)230, (byte)247, (byte)245, (byte)245, (byte)245, (byte)223, (byte)97,  (byte)184, (byte)165, (byte)165,
                (byte)229, (byte)237, (byte)253, (byte)7,   (byte)15,  (byte)108, (byte)25,  (byte)64,  (byte)58,  (byte)15,
                (byte)28,  (byte)56,  (byte)80,  (byte)89,  (byte)93,  (byte)93,  (byte)253, (byte)31,  (byte)134, (byte)143,
                (byte)28,  (byte)57,  (byte)82,  (byte)8,   (byte)54,  (byte)17,  (byte)68,  (byte)128, (byte)240, (byte)194,
                (byte)133, (byte)11,  (byte)183, (byte)130, (byte)36,  (byte)151, (byte)47,  (byte)95,  (byte)190, (byte)10,
                (byte)38,  (byte)6,   (byte)151, (byte)124, (byte)244, (byte)232, (byte)145, (byte)197, (byte)244, (byte)233,
                (byte)211, (byte)79,  (byte)60,  (byte)121, (byte)242, (byte)196, (byte)4,   (byte)67,  (byte)18,  (byte)36,
                (byte)240, (byte)234, (byte)213, (byte)43,  (byte)13,  (byte)152, (byte)4,   (byte)136, (byte)6,   (byte)0,
                (byte)228, (byte)53,  (byte)100, (byte)110, (byte)59,  (byte)11,  (byte)255, (byte)234, (byte)0,   (byte)0,
                (byte)0,   (byte)0,   (byte)73,  (byte)69,  (byte)78,  (byte)68,  (byte)174, (byte)66,  (byte)96,  (byte)130,} ) );
        }catch( Exception ex ){
            return null;
        }
    }

    @Override
    public void addItemListener( ItemListener l )
    {
        if( l == null ){
            return;
        }
        if( mItemListeners.contains( l ) ){
            return;
        }
        mItemListeners.add( l );
    }
    
    @Override
    public void removeItemListener( ItemListener l )
    {
        if( l == null ){
            return;
        }
        mItemListeners.remove( l );
    }
    
    @Override
    public Object[] getSelectedObjects()
    {
        int indx = windowDropdown.getSelectedIndex();
        if( 0 <= indx && indx < windowDropdown.getItemCount() ){
            return new Object[]{ windowDropdown.getItemAt( indx ) };
        }else{
            return  null;
        }
    }

    public void setUnitSize( int width, int height )
    {
        labelValue.setPreferredSize( new Dimension( 4, width ) );
        buttonDropdown.setPreferredSize( new Dimension( width, height ) );
    }
    
    public void setSelectedIndex( int value )
    {
        if ( value < 0 ){
            value = -1;
        }
        windowDropdown.setSelectedIndex( value );
        int indx = windowDropdown.getSelectedIndex();
        Object sel = null;
        if ( indx < 0 ){
            labelValue.setText( "" );
        }else{
            sel = windowDropdown.getItemAt( indx );
            labelValue.setText( "" + sel );
        }
        labelValue.setViewing( sel );
        int old = mSelectedIndex;
        mSelectedIndex = value;
        if( old != mSelectedIndex ){
            Object oldv = null;
            if( 0 <= old && old < windowDropdown.getItemCount() ){
                oldv = windowDropdown.getItemAt( old );
            }
            Object newv = null;
            if( 0 <= mSelectedIndex && mSelectedIndex < windowDropdown.getItemCount() ){
                newv = windowDropdown.getItemAt( mSelectedIndex );
            }
            if( oldv != null ){
                for( ItemListener l : mItemListeners ){
                    ItemEvent ie = new ItemEvent( this, ItemEvent.ITEM_STATE_CHANGED, oldv, ItemEvent.DESELECTED );
                    l.itemStateChanged( ie );
                }
            }
            if( newv != null ){
                for( ItemListener l : mItemListeners ){
                    ItemEvent ie = new ItemEvent( this, ItemEvent.ITEM_STATE_CHANGED, newv, ItemEvent.SELECTED );
                    l.itemStateChanged( ie );
                }
            }
        }
    }
    
    public int getItemCount()
    {
        return windowDropdown.getItemCount();
    }
    
    public int getSelectedIndex()
    {
        return windowDropdown.getSelectedIndex();
    }

    public void addItem( Object obj )
    {
        windowDropdown.addItem( obj );
        // 表示をかえるために必要
        setSelectedIndex( windowDropdown.getSelectedIndex() );
    }
    
    public Object getItemAt( int index )
    {
        return windowDropdown.getItemAt( index );
    }

    private void showDropdown()
    {
        // ボタンのスクリーン上の位置を把握する
        BPropertyGridComboBoxDropdown dropdown = getWindowDropdown();
        Point loc = buttonDropdown.getLocationOnScreen();
        int y = loc.y + buttonDropdown.getHeight();
        int x = this.getLocationOnScreen().x;
        dropdown.setSize( this.getWidth(), dropdown.getHeight() );
        dropdown.setLocation( x, y );
        // アイテムの選択状態を更新
        int indx = windowDropdown.getSelectedIndex();
        if( 0 <= indx && indx < windowDropdown.getItemCount() ){
            Object sel = windowDropdown.getItemAt( indx );
            Object viewing = labelValue.getViewing();
            if( sel != null && viewing != null ){
                if( !sel.equals( viewing ) ){
                    for( int i = 0; i < windowDropdown.getItemCount(); i++ ){
                        Object o = windowDropdown.getItemAt( i );
                        if( o == null ) continue;
                        if( o.equals( viewing ) ){
                            windowDropdown.setSelectedIndex( i );
                            break;
                        }
                    }
                }
            }
        }
        Window w = SwingUtilities.getWindowAncestor( this );
        if( w != null ){
            ComponentAdapter ca = new ComponentAdapter(){
                @Override
                public void componentMoved(ComponentEvent e) {
                    hideDropdown();
                }
                @Override
                public void componentResized(ComponentEvent e )
                {
                    hideDropdown();
                }
            };
            w.addComponentListener( ca );
            windowDropdown.setTagRootWindow( w );
            windowDropdown.setTagRootWindowComponentAdapter( ca );
        }
        dropdown.setVisible( true );
        buttonDropdown.requestFocus();
    }

    private void hideDropdown()
    {
        BPropertyGridComboBoxDropdown dropdown = getWindowDropdown();
        dropdown.setVisible( false );
        Window w = windowDropdown.getTagRootWindow();
        ComponentAdapter ca = windowDropdown.getTagRootWindowComponentAdapter();
        if( w != null && ca != null ){
            w.removeComponentListener( ca );
        }
    }
    
    public void actionPerformed( java.awt.event.ActionEvent e )
    {
        if ( getWindowDropdown().isVisible() ){
            hideDropdown();
        }else{
            showDropdown();
        }
    }

    /**
     * This method initializes this
     * 
     */
    private void initialize()
    {
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
        labelValue = new BPropertyGridComboBoxLabel();
        labelValue.setText("");
        labelValue.setPreferredSize(new Dimension(16, 16));
        mController.registPreferredHeightSpecified( labelValue );
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
    private JButton getButtonDropdown()
    {
        if (buttonDropdown == null) {
            buttonDropdown = new JButton();
            buttonDropdown.setPreferredSize(new Dimension(16, 16));
            buttonDropdown.setIcon( new ImageIcon( ddown2() ) );
            mController.registPreferredHeightSpecified( buttonDropdown );
            buttonDropdown.addActionListener( this );
            buttonDropdown.addKeyListener( new KeyAdapter(){
                @Override
                public void keyPressed(KeyEvent arg0) {
                    int code = arg0.getKeyCode();
                    if( code == KeyEvent.VK_UP || code == KeyEvent.VK_DOWN || code == KeyEvent.VK_PAGE_UP || code == KeyEvent.VK_PAGE_DOWN ){
                        if( windowDropdown.isVisible() ){
                            // ドロップダウンが既に見えてるとき
                            int index = windowDropdown.getSelectedIndex();
                            int delta = 0;
                            switch( code ){
                                case KeyEvent.VK_UP:{
                                    delta = -1;
                                    break;
                                }
                                case KeyEvent.VK_DOWN:{
                                    delta = 1;
                                    break;
                                }
                                case KeyEvent.VK_PAGE_UP:{
                                    delta = -windowDropdown.getMaximumRowCount();
                                    break;
                                }
                                case KeyEvent.VK_PAGE_DOWN:{
                                    delta = windowDropdown.getMaximumRowCount();
                                    break;
                                }
                            }
                            index += delta;
                            if( windowDropdown.getItemCount() <= index ){
                                index = windowDropdown.getItemCount() - 1;
                            }
                            if( index < 0 ){
                                index = 0;
                            }
                            windowDropdown.setSelectedIndex( index );
                        }else{
                            showDropdown();
                        }
                    }else if( code == KeyEvent.VK_ESCAPE ){
                        hideDropdown();
                    }else if( code == KeyEvent.VK_ENTER ){
                        setSelectedIndex( windowDropdown.getSelectedIndex() );
                        hideDropdown();
                    }
                }
            } );
            buttonDropdown.addFocusListener( new FocusAdapter(){
                @Override
                public void focusLost(FocusEvent arg0) {
                    if( windowDropdown.isVisible() ){
                        hideDropdown();
                    }
                }
            });
        }
        return buttonDropdown;
    }

    /**
     * This method initializes windowDropdown   
     *  
     * @return javax.swing.JWindow  
     */
    private BPropertyGridComboBoxDropdown getWindowDropdown()
    {
        if (windowDropdown == null) {
            windowDropdown = new BPropertyGridComboBoxDropdown( mController );
            windowDropdown.setSize(new Dimension(97, 48));
            windowDropdown.listItems.addMouseListener( new MouseAdapter(){
                @Override
                public void mouseReleased(MouseEvent arg0) {
                    windowDropdown.setVisible( false );
                    int indx = windowDropdown.getSelectedIndex();
                    setSelectedIndex( indx );
                }

                @Override
                public void mousePressed(MouseEvent arg0) {
                    buttonDropdown.requestFocusInWindow();
                }
            });
        }
        return windowDropdown;
    }
}  //  @jve:decl-index=0:visual-constraint="10,10"

class BPropertyGridComboBoxDropdown extends Window
{
    private static final long serialVersionUID = -330369852089355660L;
    private static final int MAX_ROW_COUNT = 8; 

    public JList listItems = null;
    private JPanel jPanel = null;
    private DefaultListModel model = null;
    private JScrollPane scrollPane = null;
    private Window rootWindow = null;
    private ComponentAdapter rootWindowComponentAdapter = null;
    //private BPropertyGridController mController = null;

    /**
     * This method initializes 
     * 
     */
    public BPropertyGridComboBoxDropdown( BPropertyGridController controller )
    {
        super( null );
        initialize();
        //mController = controller;
        listItems.setFixedCellHeight( BPropertyGrid.DEFAULT_ROW_HEIGHT );
        setMaximumRowCount( 1 );
        addWindowListener( new WindowAdapter(){
            @Override
            public void windowOpened(WindowEvent arg0) {
                setAlwaysOnTop( true );
            }
        });
    }
    
    public int getMaximumRowCount()
    {
        return listItems.getVisibleRowCount();
    }

    private void updateHeight()
    {
        int c = model.getSize();
        if( listItems.getVisibleRowCount() < c ){
            c = listItems.getVisibleRowCount();
        }
        setSize( listItems.getWidth(), c * BPropertyGrid.DEFAULT_ROW_HEIGHT );
    }
    
    public void setMaximumRowCount( int value )
    {
        if( value <= 0 ){
            value = 1;
        }
        if( MAX_ROW_COUNT < value ){
            value = MAX_ROW_COUNT;
        }
        listItems.setVisibleRowCount( value );
        updateHeight();
    }

    public ComponentAdapter getTagRootWindowComponentAdapter()
    {
        return rootWindowComponentAdapter;
    }
    
    public void setTagRootWindowComponentAdapter( ComponentAdapter value )
    {
        rootWindowComponentAdapter = value;
    }
    
    public Window getTagRootWindow()
    {
        return rootWindow;
    }
    
    public void setTagRootWindow( Window value )
    {
        rootWindow = value;
    }
    
    public void setSelectedIndex( int value )
    {
        listItems.setSelectedIndex( value );
        listItems.ensureIndexIsVisible( value );
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
        model.addElement( obj );
        if( listItems.getSelectedIndex() < 0 ){
            listItems.setSelectedIndex( 0 );
        }
        setMaximumRowCount( model.getSize() );
        updateHeight();
    }
    
    /**
     * This method initializes this
     * 
     */
    private void initialize()
    {
        this.setSize(new Dimension(92, 125));
        this.add(getJPanel());
            
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
    private JList getListItems()
    {
        if (listItems == null) {
            listItems = new JList();
            listItems.setSelectionMode(ListSelectionModel.SINGLE_INTERVAL_SELECTION);
            listItems.setVisibleRowCount(1);
            listItems.setAutoscrolls( true );
            listItems.setModel( getModel() );
        }
        return listItems;
    }

    /**
     * This method initializes jPanel   
     *  
     * @return javax.swing.JPanel   
     */
    private JPanel getJPanel()
    {
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
            //jPanel.add(getListItems(), gridBagConstraints1);
            jPanel.add(getJScrollPane(), gridBagConstraints1);
        }
        return jPanel;
    }

    /**
     * This method initializes jScrollPane  
     *  
     * @return javax.swing.JScrollPane  
     */
    private JScrollPane getJScrollPane()
    {
        if (scrollPane == null) {
            scrollPane = new JScrollPane();
            scrollPane.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            scrollPane.setHorizontalScrollBarPolicy( JScrollPane.HORIZONTAL_SCROLLBAR_NEVER );
            scrollPane.setVerticalScrollBarPolicy( JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED );
            scrollPane.setViewportView(getListItems());
        }
        return scrollPane;
    }

}
