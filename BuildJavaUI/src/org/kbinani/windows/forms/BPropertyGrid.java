package org.kbinani.windows.forms;

import java.awt.Color;
import java.awt.Component;
import java.awt.Container;
import java.awt.Cursor;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Label;
import java.awt.LayoutManager;
import java.awt.Panel;
import java.awt.TextField;
import javax.swing.JComboBox;
import javax.swing.JComponent;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.border.EmptyBorder;
import javax.swing.border.LineBorder;
import org.kbinani.xml.*;

public class BPropertyGrid extends Panel {
    private Class<?> _class;
    private Object[] _selected;
    private XmlMember[] _xml_members;
    private Component[] _components;
    
    public BPropertyGrid(){
        super();
    }

    public Object[] getSelectedObjects(){
        return this._selected;
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

        if( this._class == null ){
            this._class = cls;
            _updateGridComponents();
        }else{
            if( !this._class.equals( cls ) ){
                this._class = cls;
                _updateGridComponents();
            }
        }
        this._selected = objects;
        _update();
    }

    /**
     * 選択されたオブジェクトの値を，グリッドのコンポーネントの表示状態に反映させます
     */
    private void _update(){
        for( int i = 0; i < this._xml_members.length; i++ ){
            XmlMember xm = this._xml_members[i];
            Class<?> member_type = xm.getType();
            if( member_type.equals( this._class ) ){
                continue;
            }
            String draft = null;
            boolean all_equals = true;
            for( Object o : this._selected ){
                if( o == null ){
                    continue;
                }
                Object value = xm.get( o );
                String s = value + "";
System.out.println( "BPropertyGrid#_updatd; s=" + s );
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

            Component c = this._components[i];
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
    private void _updateGridComponents(){
        // まず現在登録されているコンポーネントをすべて破棄する
        this.removeAll();
        // プロパティーを追加．再帰的に
        int rows =_appendComponentTo( this, this._class, true );
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
    private int _appendComponentTo( Container container, Class<?> cls, boolean add_spacer ){
System.out.println( "BPropertyGrid#_appendComponentTo; cls=" + cls );
        // レイアウト適用
        GridBagLayout layout = new GridBagLayout();
        container.setLayout( layout );
        
        // プロパティを抽出
        this._xml_members = XmlMember.extractMembers( cls );
        this._components = new Component[this._xml_members.length];
System.out.println( "BPropertyGrid#_appendComponentTo; xml_members.length=" + this._xml_members.length );
        int rows = 0;
        for ( XmlMember xm : this._xml_members ){
            Class<?> member_type = xm.getType();
            if( member_type.equals( cls ) ){
                // 自分自身のフィールド等に同じ型のものがあると再帰になるので，スルーする
                continue;
            }
            String cli_name = XmlSerializer.getCliTypeName( member_type );
System.out.println( "BPropertyGrid#_appendComponentTo; member_type=" + member_type + "; cli_name=" + cli_name );
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
            propname.setText( xm.getName() );
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
            this._components[rows - 1] = comp;
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
