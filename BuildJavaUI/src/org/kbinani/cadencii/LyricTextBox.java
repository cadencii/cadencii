package org.kbinani.cadencii;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.Frame;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Point;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowFocusListener;
import java.awt.im.InputContext;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.JWindow;
import javax.swing.border.EmptyBorder;
import org.kbinani.BEvent;
import org.kbinani.windows.forms.BKeyEventArgs;
import org.kbinani.windows.forms.BKeyEventHandler;
import org.kbinani.windows.forms.BKeyPressEventArgs;
import org.kbinani.windows.forms.BKeyPressEventHandler;

public class LyricTextBox extends JWindow
                          implements WindowFocusListener, ComponentListener, KeyListener
{
    private static final long serialVersionUID = -8530774432912981644L;
    private JPanel mContentPane = null;
    private JTextField mTextField = null;
    private String mBufText;
    private boolean mPhoneticSymbolEditMode;

    public void requestFocus()
    {
        super.requestFocus();
        mTextField.requestFocus();
    }

    /**
     * 発音記号を編集するモードかどうかを表すブール値を取得します
     */
    public boolean isPhoneticSymbolEditMode()
    {
        return mPhoneticSymbolEditMode;
    }

    /**
     * 発音記号を編集するモードかどうかを表すブール値を設定します
     */
    public void setPhoneticSymbolEditMode( boolean value )
    {
        mPhoneticSymbolEditMode = value;
    }

    /**
     * バッファーテキストを取得します
     * (バッファーテキストには，発音記号モードでは歌詞，歌詞モードでは発音記号がそれぞれ格納される)
     */
    public String getBufferText()
    {
        return mBufText;
    }

    /**
     * バッファーテキストを設定します
     * (バッファーテキストには，発音記号モードでは歌詞，歌詞モードでは発音記号がそれぞれ格納される)
     */
    public void setBufferText( String value )
    {
        mBufText = value;
    }

    public void selectAll()
    {
        mTextField.selectAll();
    }
    
    public String getText()
    {
        return mTextField.getText();
    }

    public void setText( String value )
    {
        mTextField.setText( value );
    }
    
    public boolean isImeModeOn()
    {
        try{
            JTextField jtf = getJTextField();
            if( jtf == null ){
                return false;
            }
            InputContext ic = jtf.getInputContext();
            if( ic == null ){
                return false;
            }
            boolean ret = ic.isCompositionEnabled();
            return ret;
        }catch( Exception ex ){
            System.err.println( "TextBoxEx#isImeModeOn; ex=" + ex );
        }
        return false;
    }

    public void setImeModeOn( boolean value )
    {
        try{
            JTextField jtf = getJTextField();
            if( jtf == null ){
                return;
            }
            InputContext ic = jtf.getInputContext();
            if( ic == null ){
                return;
            }
            ic.setCompositionEnabled( value );
        }catch( Exception ex ){
            System.err.println( "TextBoxEx#setImeModeOn; ex=" + ex );
        }
    }

    /* REGION java.awt.Component */
    /* root implementation of java.awt.Component is in BForm.cs(java) */
    public final BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public final BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public final BEvent<BKeyPressEventHandler> keyPressEvent = new BEvent<BKeyPressEventHandler>();

    public void keyPressed( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyDownEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyPressed; ex=" + ex );
        }
    }

    public void keyReleased( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyUpEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyReleased; ex=" + ex );
        }
    }

    public void keyTyped( KeyEvent e0 ){
        try{
            BKeyPressEventArgs e = new BKeyPressEventArgs( e0 );
            keyPressEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyTyped; ex=" + ex );
        }
    }
    /* END REGION java.awt.Component */

    public void windowGainedFocus( WindowEvent e ){
        System.out.println( "focusGained" );
        mTextField.requestFocusInWindow();
        mTextField.selectAll();
    }
    
    public void windowLostFocus( WindowEvent e ){
        System.out.println( "focusLost" );
        super.requestFocus();
        mTextField.requestFocusInWindow();
        //setVisible( false );
    }

    public void componentHidden(ComponentEvent e){
    }

    public void componentMoved(ComponentEvent e){
        Component parent = e.getComponent();
        Point p = parent.getLocation();
        setLocation( p );
    }

    public void componentResized(ComponentEvent e){
    }

    public void componentShown(ComponentEvent e){
    }
     
    /*public static void main( String[] args ){
        JFrame empty = new JFrame( "empty owner" );
        OnScreenInputTextBox textbox = new OnScreenInputTextBox( empty );

        empty.setDefaultCloseOperation( JFrame.EXIT_ON_CLOSE );
        empty.setSize( 200, 100 );
        empty.setLocation( 100, 100 );
        empty.setVisible( true );
        empty.addComponentListener( textbox );
        
        Point loc = empty.getLocationOnScreen();
        textbox.setAlwaysOnTop( true );
        textbox.setLocation( loc );
        textbox.setVisible( true );
        textbox.requestFocus();
    }*/

    /**
     * @param owner
     */
    public LyricTextBox( Frame owner )
    {
        super( owner );
        initialize();
        addWindowFocusListener( this );
        mTextField.addKeyListener( this );
        Dimension d = new Dimension( 115, 22 );
        mTextField.setPreferredSize( d );
        mContentPane.setPreferredSize( d );
        pack();        
        mTextField.requestFocus();
    }

    /**
     * This method initializes this
     * 
     * @return void
     */
    private void initialize() {
        this.setSize(115, 22);
        this.setContentPane(getJContentPane());
    }

    /**
     * This method initializes jContentPane
     * 
     * @return javax.swing.JPanel
     */
    private JPanel getJContentPane() {
        if (mContentPane == null) {
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.fill = GridBagConstraints.BOTH;
            gridBagConstraints.gridy = 0;
            gridBagConstraints.ipadx = 0;
            gridBagConstraints.ipady = 0;
            gridBagConstraints.weightx = 1.0;
            gridBagConstraints.weighty = 1.0D;
            gridBagConstraints.gridx = 0;
            mContentPane = new JPanel();
            mContentPane.setLayout(new GridBagLayout());
            mContentPane.add(getJTextField(), gridBagConstraints);
        }
        return mContentPane;
    }

    /**
     * This method initializes jTextField   
     *  
     * @return javax.swing.JTextField   
     */
    private JTextField getJTextField() {
        if (mTextField == null) {
            mTextField = new JTextField();
            mTextField.setFocusTraversalKeysEnabled( false );
            mTextField.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
        }
        return mTextField;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"
