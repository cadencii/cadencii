package org.kbinani.windows.forms;

import java.awt.Color;
import java.awt.Component;
import java.awt.Container;
import java.awt.Cursor;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.LayoutManager;
import java.awt.Panel;
import javax.swing.JComboBox;
import javax.swing.JComponent;
import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.border.EmptyBorder;
import org.kbinani.componentmodel.IPropertyDescripter;
import org.kbinani.componentmodel.PropertyDescripter;
import org.kbinani.xml.XmlMember;
import org.kbinani.xml.XmlSerializer;

public class BPropertyGrid extends Panel {
    private static final long serialVersionUID = -5937300027752664252L;
    private Class<?> mClass;
    private Object[] mSelected;
    private XmlMember[] mXmlMembers;
    private Component[] mComponents;
    private PropertyDescripter mDescripter;
    
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

    /**
     * 選択されたオブジェクトの値を，グリッドのコンポーネントの表示状態に反映させます
     */
    private void update(){
        for( int i = 0; i < this.mXmlMembers.length; i++ ){
            XmlMember xm = this.mXmlMembers[i];
            Class<?> member_type = xm.getType();
            if( member_type.equals( this.mClass ) ){
                continue;
            }
            String draft = null;
            boolean all_equals = true;
            for( Object o : this.mSelected ){
                if( o == null ){
                    continue;
                }
                Object value = xm.get( o );
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

            Component c = this.mComponents[i];
            if( c instanceof JTextField ){
                JTextField jtf = (JTextField)c;
                jtf.setText( draft );
            }else if( c instanceof JComboBox ){
                JComboBox jcb = (JComboBox)c;
                if( all_equals ){
                    for( int j = 0; j < jcb.getItemCount(); j++ ){
                        Object item = jcb.getItemAt( j );
                        if( draft.equals( item + "" ) ){
                            jcb.setSelectedIndex( j );
                            break;
                        }
                    }
                }else{
                    jcb.setSelectedItem( null );
                }
            }
        }
    }
    
    /**
     * グリッドのコンポーネントを，選択されたオブジェクトの型に合わせて変更します
     */
    private void updateGridComponents(){
        // まず現在登録されているコンポーネントをすべて破棄する
        this.removeAll();
        // プロパティーを追加．再帰的に
        int rows =appendComponentTo( this, this.mClass, true );
        // 一番下にスペーサを設置
        LayoutManager lm = this.getLayout();
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
     * @param comp
     * @param cls
     */
    private int appendComponentTo( Container container, Class<?> cls, boolean add_spacer ){
System.out.println( "BPropertyGrid#appendComponentTo; cls=" + cls );
        // レイアウト適用
        GridBagLayout layout = new GridBagLayout();
        container.setLayout( layout );
        
        // プロパティを抽出
        this.mXmlMembers = XmlMember.extractMembers( cls );
        
        // プロパティ・デスクリプタを取得
        if( cls.isAssignableFrom( IPropertyDescripter.class ) ){
            try{
                IPropertyDescripter ipd = (IPropertyDescripter)cls.newInstance();
                this.mDescripter = ipd.getDescripter();
            }catch( Exception ex ){
                this.mDescripter = null;
            }
        }
        if( this.mDescripter == null ){
            this.mDescripter = new PropertyDescripter();
        }

        this.mComponents = new Component[this.mXmlMembers.length];
System.out.println( "BPropertyGrid#appendComponentTo; mXmlMembers.length=" + this.mXmlMembers.length );
        int rows = 0;
        for ( XmlMember xm : this.mXmlMembers ){
            Class<?> member_type = xm.getType();
            if( member_type.equals( cls ) ){
                // 自分自身のフィールド等に同じ型のものがあると再帰になるので，スルーする
                continue;
            }
            String cli_name = XmlSerializer.getCliTypeName( member_type );
System.out.println( "BPropertyGrid#appendComponentTo; member_type=" + member_type + "; cli_name=" + cli_name );
            // プリミティブ型
            rows++;
            Color bgcolor = (rows % 2 != 0) ? Color.WHITE : new Color( 240, 240, 240 );
            GridBagConstraints gbc = new GridBagConstraints();
            gbc.gridy = rows - 1;
            gbc.weightx = 0;
            gbc.weighty = 0;
            gbc.fill = GridBagConstraints.BOTH;
            gbc.anchor = GridBagConstraints.NORTHWEST;

            int gx = -1;
            if( add_spacer ){
                // 左端のスペーサ
                Panel spacer = new Panel();
                gx++;
                gbc.gridx = gx;
                layout.addLayoutComponent( spacer, gbc );
                container.add( spacer );
            }
            
            // プロパティの名前
            JLabel propname = new JLabel();
            gx++;
            propname.setText( this.mDescripter.getDisplayName( xm.getName() ) );
            propname.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
            propname.setBackground( bgcolor );
            propname.setOpaque( true );
            gbc.gridx = gx;
            gbc.weightx = 0.5;
            gbc.fill = GridBagConstraints.BOTH;
            layout.addLayoutComponent( propname, gbc );
            container.add( propname );
            
            // 表示幅を調節するための，取っ手
            Panel handle = new Panel();
            gx++;
            gbc.gridx = gx;
            gbc.weightx = 0.0;
            gbc.fill = GridBagConstraints.BOTH;
            handle.setPreferredSize( new Dimension( 4, 1 ) );
            handle.setCursor( new Cursor( Cursor.W_RESIZE_CURSOR | Cursor.E_RESIZE_CURSOR ) );
            layout.addLayoutComponent( handle, gbc );
            container.add( handle );

            // 値を入れるテキストボックス(など)
            JComponent comp = null;
            gx++;
            if( member_type.isEnum() ){
                JComboBox cbox = new JComboBox();
                Object[] items = member_type.getEnumConstants();
                for( Object o : items ){
                    cbox.addItem( o.toString() );
                }
                comp = cbox;
            /*}else if( cli_name.equals( "" ) ){
                JPanel panel = new JPanel();
                rows += _appendComponentTo( panel, member_type, false );
                comp = panel;*/
            }else if( cli_name.equals( "bool" ) ){
                JComboBox cbox = new JComboBox();
                cbox.addItem( "True" );
                cbox.addItem( "False" );
                comp = cbox;
            }else{
                JTextField text = new JTextField();
                comp = text;
            }
            this.mComponents[rows - 1] = comp;
            comp.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
            comp.setBackground( bgcolor );
            gbc.gridx = gx;
            gbc.weightx = 1.0;
            gbc.fill = GridBagConstraints.BOTH;
            layout.addLayoutComponent( comp, gbc );
            container.add( comp );
        }
        return rows;
    }
}
