package org.kbinani.windows.forms;

import java.awt.Graphics;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Image;
import java.awt.Point;
import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JPanel;
import javax.swing.JTextField;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;
import java.awt.Dimension;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.io.ByteArrayInputStream;
import javax.swing.BorderFactory;

public class BNumericUpDown extends JPanel
                            implements BNumericUpDownStateChangeListener
{
    private static final long serialVersionUID = -8499996379673462967L;

    private BNumericUpDownButtons panelButtons = null;
    private JTextField txtValue = null;
	private JButton btnUp = null;
	private JButton btnDown = null;
	private float value = 0;
	private float maximum = 100;
	private float minimum = 0;
	private float increment = 1;
	private int decimalPlaces = 0;
	private float minimumStep = 1;

	public int getDecimalPlaces(){
	    return decimalPlaces;
	}
	
	public void setDecimalPlaces( int value ){
	    decimalPlaces = value;
	    minimumStep = (float)Math.pow( 0.1, value );
	    if( increment < minimumStep ){
	        increment = minimumStep;
	    }
	}
	
	public float getIncrement(){
	    return increment;
	}
	
	public void setIncrement( float value ){
	    increment = value;
	}
	
	public BEvent<BEventHandler> valueChangedEvent = new BEvent<BEventHandler>();  //  @jve:decl-index=0:
	private void update(){
	    getTxtValue().setText( value + "" );
	    try{
	        valueChangedEvent.raise( this, new BEventArgs() );
	    }catch( Exception ex ){
	        System.err.println( "BNumericUpDown#update; ex=" + ex );
	    }
	}
	
	public float getValue(){
	    return value;
	}
	
	public void setValue( float value ){
	    if( minimum <= value && value <= maximum ){
	        this.value = value; 
	        update();
	    }
	}
	
	public float getMaximum() {
        return maximum;
    }

    public void setMaximum( float maximum ){
        if( maximum < value ){
            value = maximum;
            update();
        }
        this.maximum = maximum;
    }

    public float getMinimum() {
        return minimum;
    }

    public void setMinimum( float minimum ){
        if( value < minimum ){
            value = minimum;
            update();
        }
        this.minimum = minimum;
    }

    /**
	 * This is the default constructor
	 */
	public BNumericUpDown() {
		super();
		initialize();
		boolean mac_os = true; //TODO:
		if( mac_os ){
		    panelButtons.removeAll();
		    panelButtons.setOwnerDraw( true );
		    panelButtons.setStateChangeListener( this );
		    panelButtons.setPreferredSize( new Dimension( panelButtons.getImageWidth(), panelButtons.getImageHeight() ) );
		}
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
		gridBagConstraints11.gridx = 1;
		gridBagConstraints11.fill = GridBagConstraints.VERTICAL;
		gridBagConstraints11.gridy = 0;
		GridBagConstraints gridBagConstraints = new GridBagConstraints();
		gridBagConstraints.gridx = 0;
		gridBagConstraints.fill = GridBagConstraints.BOTH;
		gridBagConstraints.gridy = 0;
		gridBagConstraints.fill = GridBagConstraints.BOTH;
		gridBagConstraints.gridy = 0;
		gridBagConstraints.weightx = 1.0;
		gridBagConstraints.gridheight = 1;
		gridBagConstraints.weighty = 1.0D;
		gridBagConstraints.gridx = 0;
		this.setSize(78, 33);
		this.setLayout(new GridBagLayout());
		this.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
		this.add(getTxtValue(), gridBagConstraints);
		this.add(getJPanel(), gridBagConstraints11);
	}

	/**
	 * This method initializes txtValue	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtValue() {
		if (txtValue == null) {
			txtValue = new JTextField();
			txtValue.setText("0");
			txtValue.setHorizontalAlignment(JTextField.RIGHT);
			txtValue.addKeyListener(new java.awt.event.KeyAdapter() {
			    public void keyTyped(java.awt.event.KeyEvent e) {
			        try{
			            float draft = Float.parseFloat( txtValue.getText() );
			            value = draft;
			            update();
			        }catch( Exception ex ){
			        }
			    }
			});
		}
		return txtValue;
	}

	/**
	 * This method initializes btnUp	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnUp() {
		if (btnUp == null) {
			btnUp = new JButton();
			btnUp.setText("");
			btnUp.setPreferredSize(new Dimension(20, 10));
			btnUp.addActionListener(new java.awt.event.ActionListener() {
			    public void actionPerformed(java.awt.event.ActionEvent e) {
			        up();
			    }
			});
		}
		return btnUp;
	}

	private void up(){
        if( value + increment <= maximum ){
            value += increment;
            update();
        }
	}
	
	private void down(){
        if( value - increment >= minimum ){
            value -= increment;
            update();
        }
	}
	
	/**
	 * This method initializes btnDown	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnDown() {
		if (btnDown == null) {
			btnDown = new JButton();
			btnDown.setText("");
			btnDown.setPreferredSize(new Dimension(20, 10));
			btnDown.addActionListener(new java.awt.event.ActionListener() {
			    public void actionPerformed(java.awt.event.ActionEvent e) {
			        down();
			    }
			});
		}
		return btnDown;
	}

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (panelButtons == null) {
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.anchor = GridBagConstraints.EAST;
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.gridy = 1;
            gridBagConstraints2.weighty = 0.5D;
            gridBagConstraints2.fill = GridBagConstraints.BOTH;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.anchor = GridBagConstraints.EAST;
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weighty = 0.5D;
            gridBagConstraints1.fill = GridBagConstraints.BOTH;
            panelButtons = new BNumericUpDownButtons();
            panelButtons.setLayout(new GridBagLayout());
            panelButtons.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            panelButtons.add(getBtnUp(), gridBagConstraints1);
            panelButtons.add(getBtnDown(), gridBagConstraints2);
        }
        return panelButtons;
    }

    public void changed(int delta) {
        if( delta == 1 ){
            up();
        }else if( delta == -1 ){
            down();
        }
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"

interface BNumericUpDownStateChangeListener{
    void changed( int delta );
}

class BNumericUpDownButtons extends JPanel
                            implements MouseListener
{
    private static final long serialVersionUID = 3446570954118067936L;

    private int mButtonState = 0;
    private Image mImg00 = null;
    private Image mImg01 = null;
    private Image mImg10 = null;
    private boolean mOwnerDraw = false;
    private int mImgWidth = 1;
    private int mImgHeight = 1;
    private BNumericUpDownStateChangeListener mListener;
    
    public BNumericUpDownButtons(){
        mImg00 = getImg00();
        mImg01 = getImg01();
        mImg10 = getImg10();
        mImgWidth = mImg00.getWidth( null );
        mImgHeight = mImg00.getHeight( null );
        setPreferredSize( new Dimension( mImgWidth, mImgHeight ) );
        addMouseListener( this );
    }

    public int getImageWidth(){
        return mImgWidth;
    }
    
    public int getImageHeight(){
        return mImgHeight;
    }
    
    public void setStateChangeListener( BNumericUpDownStateChangeListener l ){
        mListener = l;
    }
    
    public boolean isOwnerDraw(){
        return mOwnerDraw;
    }
    
    public void setOwnerDraw( boolean value ){
        mOwnerDraw = value;
    }
    
    public void paint( Graphics g ){
        if( mOwnerDraw ){
            if( mButtonState == 0 ){
                g.drawImage( mImg00, 0, 0, null );
            }else if( mButtonState == 1 ){
                g.drawImage( mImg01, 0, 0, null );
            }else if( mButtonState == 10 ){
                g.drawImage( mImg10, 0, 0, null );
            }
        }else{
            super.paint( g );
        }
    }

    public void mouseClicked(MouseEvent e) {
    }

    public void mouseEntered(MouseEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
    }

    public void mouseExited(MouseEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
    }

    public void mousePressed(MouseEvent e) {
        Point p = e.getPoint();
        if( p.y < mImgHeight / 2 ){
            mButtonState = 10;
        }else{
            mButtonState = 1;
        }
        repaint();
    }

    public void mouseReleased(MouseEvent e) {
        if( mListener != null ){
            if( mButtonState == 10 ){
                mListener.changed( 1 );
            }else if( mButtonState == 1 ){
                mListener.changed( -1 );
            }
        }
        mButtonState = 0;
        repaint();
    }

    private static Image getImg00()
    {
        try{
            return ImageIO.read( new ByteArrayInputStream( new byte[]{
                (byte)137, (byte)80,  (byte)78,  (byte)71,  (byte)13,  (byte)10,  (byte)26,  (byte)10,  (byte)0,   (byte)0,
                (byte)0,   (byte)13,  (byte)73,  (byte)72,  (byte)68,  (byte)82,  (byte)0,   (byte)0,   (byte)0,   (byte)17,
                (byte)0,   (byte)0,   (byte)0,   (byte)25,  (byte)8,   (byte)2,   (byte)0,   (byte)0,   (byte)0,   (byte)88,
                (byte)92,  (byte)82,  (byte)192, (byte)0,   (byte)0,   (byte)0,   (byte)1,   (byte)115, (byte)82,  (byte)71,
                (byte)66,  (byte)0,   (byte)174, (byte)206, (byte)28,  (byte)233, (byte)0,   (byte)0,   (byte)0,   (byte)4,
                (byte)103, (byte)65,  (byte)77,  (byte)65,  (byte)0,   (byte)0,   (byte)177, (byte)143, (byte)11,  (byte)252,
                (byte)97,  (byte)5,   (byte)0,   (byte)0,   (byte)0,   (byte)32,  (byte)99,  (byte)72,  (byte)82,  (byte)77,
                (byte)0,   (byte)0,   (byte)122, (byte)38,  (byte)0,   (byte)0,   (byte)128, (byte)132, (byte)0,   (byte)0,
                (byte)250, (byte)0,   (byte)0,   (byte)0,   (byte)128, (byte)232, (byte)0,   (byte)0,   (byte)117, (byte)48,
                (byte)0,   (byte)0,   (byte)234, (byte)96,  (byte)0,   (byte)0,   (byte)58,  (byte)152, (byte)0,   (byte)0,
                (byte)23,  (byte)112, (byte)156, (byte)186, (byte)81,  (byte)60,  (byte)0,   (byte)0,   (byte)2,   (byte)184,
                (byte)73,  (byte)68,  (byte)65,  (byte)84,  (byte)56,  (byte)79,  (byte)93,  (byte)148, (byte)205, (byte)75,
                (byte)162, (byte)81,  (byte)20,  (byte)198, (byte)109, (byte)16,  (byte)253, (byte)155, (byte)218, (byte)180,
                (byte)211, (byte)133, (byte)255, (byte)198, (byte)128, (byte)139, (byte)72,  (byte)152, (byte)12,  (byte)3,
                (byte)77,  (byte)33,  (byte)10,  (byte)130, (byte)16,  (byte)109, (byte)4,   (byte)177, (byte)133, (byte)109,
                (byte)52,  (byte)9,   (byte)194, (byte)69,  (byte)208, (byte)34,  (byte)8,   (byte)138, (byte)138, (byte)138,
                (byte)12,  (byte)45,  (byte)237, (byte)195, (byte)242, (byte)245, (byte)179, (byte)210, (byte)62,  (byte)156,
                (byte)50,  (byte)161, (byte)204, (byte)134, (byte)82,  (byte)102, (byte)126, (byte)47,  (byte)23,  (byte)237,
                (byte)189, (byte)157, (byte)213, (byte)185, (byte)207, (byte)121, (byte)158, (byte)115, (byte)207, (byte)185,
                (byte)247, (byte)220, (byte)59,  (byte)240, (byte)240, (byte)240, (byte)160, (byte)235, (byte)217, (byte)191,
                (byte)158, (byte)245, (byte)17,  (byte)156, (byte)129, (byte)158, (byte)125, (byte)129, (byte)104, (byte)176,
                (byte)251, (byte)251, (byte)251, (byte)235, (byte)235, (byte)107, (byte)159, (byte)207, (byte)55,  (byte)50,
                (byte)50,  (byte)242, (byte)75,  (byte)54,  (byte)187, (byte)221, (byte)238, (byte)247, (byte)251, (byte)137,
                (byte)194, (byte)17,  (byte)100, (byte)157, (byte)16,  (byte)84,  (byte)42,  (byte)21,  (byte)135, (byte)195,
                (byte)17,  (byte)143, (byte)199, (byte)111, (byte)111, (byte)111, (byte)255, (byte)202, (byte)86,  (byte)171,
                (byte)85,  (byte)151, (byte)151, (byte)151, (byte)81,  (byte)194, (byte)17,  (byte)50,  (byte)85,  (byte)67,
                (byte)142, (byte)217, (byte)217, (byte)217, (byte)64,  (byte)32,  (byte)240, (byte)250, (byte)250, (byte)218,
                (byte)233, (byte)116, (byte)68,  (byte)129, (byte)221, (byte)110, (byte)87,  (byte)56,  (byte)159, (byte)159,
                (byte)159, (byte)205, (byte)102, (byte)51,  (byte)240, (byte)59,  (byte)224, (byte)245, (byte)122, (byte)97,
                (byte)170, (byte)26,  (byte)164, (byte)103, (byte)103, (byte)103, (byte)195, (byte)195, (byte)195, (byte)91,
                (byte)91,  (byte)91,  (byte)141, (byte)70,  (byte)131, (byte)61,  (byte)186, (byte)157, (byte)238, (byte)91,
                (byte)235, (byte)205, (byte)98,  (byte)177, (byte)180, (byte)223, (byte)218, (byte)248, (byte)237, (byte)118,
                (byte)187, (byte)94,  (byte)175, (byte)111, (byte)108, (byte)108, (byte)80,  (byte)54,  (byte)76,  (byte)248,
                (byte)186, (byte)187, (byte)187, (byte)187, (byte)131, (byte)131, (byte)3,   (byte)214, (byte)199, (byte)199,
                (byte)199, (byte)20,  (byte)70,  (byte)202, (byte)143, (byte)143, (byte)143, (byte)9,   (byte)215, (byte)4,
                (byte)29,  (byte)187, (byte)221, (byte)110, (byte)252, (byte)231, (byte)198, (byte)243, (byte)205, (byte)205,
                (byte)13,  (byte)81,  (byte)155, (byte)205, (byte)6,   (byte)19,  (byte)190, (byte)14,  (byte)222, (byte)246,
                (byte)246, (byte)54,  (byte)229, (byte)166, (byte)211, (byte)233, (byte)106, (byte)181, (byte)202, (byte)86,
                (byte)56,  (byte)6,   (byte)131, (byte)1,   (byte)141, (byte)209, (byte)104, (byte)204, (byte)100, (byte)50,
                (byte)79,  (byte)79,  (byte)79,  (byte)87,  (byte)87,  (byte)87,  (byte)128, (byte)99,  (byte)99,  (byte)99,
                (byte)48,  (byte)225, (byte)171, (byte)26,  (byte)170, (byte)114, (byte)58,  (byte)157, (byte)251, (byte)251,
                (byte)251, (byte)148, (byte)251, (byte)248, (byte)248, (byte)56,  (byte)52,  (byte)52,  (byte)244, (byte)163,
                (byte)103, (byte)248, (byte)245, (byte)250, (byte)159, (byte)114, (byte)185, (byte)76,  (byte)212, (byte)229,
                (byte)114, (byte)193, (byte)252, (byte)210, (byte)76,  (byte)78,  (byte)78,  (byte)38,  (byte)147, (byte)73,
                (byte)98,  (byte)180, (byte)200, (byte)86,  (byte)90,  (byte)163, (byte)129, (byte)66,  (byte)161, (byte)64,
                (byte)116, (byte)106, (byte)106, (byte)74,  (byte)210, (byte)204, (byte)204, (byte)204, (byte)164, (byte)82,
                (byte)169, (byte)66,  (byte)177, (byte)64,  (byte)26,  (byte)58,  (byte)214, (byte)90,  (byte)173, (byte)86,
                (byte)203, (byte)231, (byte)243, (byte)68,  (byte)57,  (byte)91,  (byte)73,  (byte)195, (byte)109, (byte)30,
                (byte)29,  (byte)29,  (byte)145, (byte)143, (byte)118, (byte)233, (byte)82,  (byte)107, (byte)32,  (byte)104,
                (byte)136, (byte)206, (byte)205, (byte)205, (byte)73,  (byte)154, (byte)96,  (byte)48,  (byte)200, (byte)201,
                (byte)92,  (byte)92,  (byte)92,  (byte)228, (byte)21,  (byte)133, (byte)142, (byte)181, (byte)134, (byte)32,
                (byte)155, (byte)205, (byte)18,  (byte)13,  (byte)133, (byte)66,  (byte)146, (byte)38,  (byte)28,  (byte)14,
                (byte)131, (byte)94,  (byte)94,  (byte)94,  (byte)22,  (byte)139, (byte)69,  (byte)186, (byte)210, (byte)26,
                (byte)155, (byte)131, (byte)115, (byte)110, (byte)11,  (byte)11,  (byte)11,  (byte)146, (byte)38,  (byte)26,
                (byte)141, (byte)130, (byte)230, (byte)114, (byte)57,  (byte)216, (byte)20,  (byte)163, (byte)181, (byte)82,
                (byte)169, (byte)148, (byte)83,  (byte)20,  (byte)162, (byte)139, (byte)139, (byte)139, (byte)146, (byte)134,
                (byte)137, (byte)226, (byte)42,  (byte)40,  (byte)131, (byte)170, (byte)56,  (byte)40,  (byte)173, (byte)129,
                (byte)128, (byte)159, (byte)156, (byte)156, (byte)48,  (byte)141, (byte)146, (byte)102, (byte)101, (byte)101,
                (byte)5,   (byte)148, (byte)50,  (byte)184, (byte)86,  (byte)179, (byte)217, (byte)204, (byte)157, (byte)10,
                (byte)27,  (byte)28,  (byte)28,  (byte)100, (byte)79,  (byte)240, (byte)211, (byte)211, (byte)211, (byte)213,
                (byte)213, (byte)85,  (byte)73,  (byte)179, (byte)182, (byte)182, (byte)6,   (byte)74,  (byte)51,  (byte)156,
                (byte)24,  (byte)141, (byte)49,  (byte)1,   (byte)204, (byte)129, (byte)94,  (byte)175, (byte)223, (byte)219,
                (byte)219, (byte)227, (byte)244, (byte)193, (byte)153, (byte)180, (byte)245, (byte)245, (byte)117, (byte)73,
                (byte)179, (byte)185, (byte)185, (byte)9,   (byte)74,  (byte)233, (byte)220, (byte)204, (byte)251, (byte)251,
                (byte)187, (byte)199, (byte)227, (byte)65,  (byte)51,  (byte)58,  (byte)58,  (byte)138, (byte)207, (byte)45,
                (byte)131, (byte)19,  (byte)101, (byte)112, (byte)190, (byte)52,  (byte)59,  (byte)59,  (byte)59,  (byte)140,
                (byte)173, (byte)162, (byte)40,  (byte)164, (byte)108, (byte)181, (byte)90,  (byte)60,  (byte)7,   (byte)102,
                (byte)217, (byte)100, (byte)50,  (byte)189, (byte)188, (byte)188, (byte)224, (byte)131, (byte)176, (byte)57,
                (byte)45,  (byte)33,  (byte)128, (byte)169, (byte)206, (byte)14,  (byte)235, (byte)68,  (byte)34,  (byte)17,
                (byte)137, (byte)68,  (byte)72,  (byte)198, (byte)56,  (byte)246, (byte)223, (byte)79,  (byte)223, (byte)225,
                (byte)253, (byte)48,  (byte)74,  (byte)60,  (byte)56,  (byte)206, (byte)13,  (byte)166, (byte)58,  (byte)215,
                (byte)28,  (byte)209, (byte)249, (byte)249, (byte)249, (byte)252, (byte)252, (byte)60,  (byte)229, (byte)145,
                (byte)184, (byte)255, (byte)37,  (byte)104, (byte)29,  (byte)222, (byte)34,  (byte)133, (byte)193, (byte)129,
                (byte)169, (byte)190, (byte)31,  (byte)202, (byte)229, (byte)100, (byte)118, (byte)119, (byte)119, (byte)167,
                (byte)167, (byte)167, (byte)99,  (byte)177, (byte)24,  (byte)208, (byte)55,  (byte)25,  (byte)200, (byte)210,
                (byte)210, (byte)18,  (byte)81,  (byte)56,  (byte)48,  (byte)165, (byte)255, (byte)0,   (byte)136, (byte)76,
                (byte)227, (byte)227, (byte)227, (byte)86,  (byte)171, (byte)245, (byte)103, (byte)207, (byte)240, (byte)65,
                (byte)192, (byte)137, (byte)74,  (byte)255, (byte)129, (byte)248, (byte)70,  (byte)200, (byte)193, (byte)92,
                (byte)29,  (byte)30,  (byte)30,  (byte)18,  (byte)166, (byte)87,  (byte)97,  (byte)248, (byte)32,  (byte)224,
                (byte)68,  (byte)165, (byte)127, (byte)71,  (byte)252, (byte)64,  (byte)66,  (byte)249, (byte)109, (byte)168,
                (byte)197, (byte)178, (byte)207, (byte)22,  (byte)180, (byte)255, (byte)255, (byte)29,  (byte)200, (byte)75,
                (byte)230, (byte)135, (byte)58,  (byte)57,  (byte)0,   (byte)0,   (byte)0,   (byte)0,   (byte)73,  (byte)69,
                (byte)78,  (byte)68,  (byte)174, (byte)66,  (byte)96,  (byte)130, } ) );
        }catch( Exception ex ){
            return null;
        }
    }
    
    private static Image getImg10()
    {
        try{
            return ImageIO.read( new ByteArrayInputStream( new byte[]{
                (byte)137, (byte)80,  (byte)78,  (byte)71,  (byte)13,  (byte)10,  (byte)26,  (byte)10,  (byte)0,   (byte)0,
                (byte)0,   (byte)13,  (byte)73,  (byte)72,  (byte)68,  (byte)82,  (byte)0,   (byte)0,   (byte)0,   (byte)17,
                (byte)0,   (byte)0,   (byte)0,   (byte)25,  (byte)8,   (byte)2,   (byte)0,   (byte)0,   (byte)0,   (byte)88,
                (byte)92,  (byte)82,  (byte)192, (byte)0,   (byte)0,   (byte)0,   (byte)1,   (byte)115, (byte)82,  (byte)71,
                (byte)66,  (byte)0,   (byte)174, (byte)206, (byte)28,  (byte)233, (byte)0,   (byte)0,   (byte)0,   (byte)4,
                (byte)103, (byte)65,  (byte)77,  (byte)65,  (byte)0,   (byte)0,   (byte)177, (byte)143, (byte)11,  (byte)252,
                (byte)97,  (byte)5,   (byte)0,   (byte)0,   (byte)0,   (byte)32,  (byte)99,  (byte)72,  (byte)82,  (byte)77,
                (byte)0,   (byte)0,   (byte)122, (byte)38,  (byte)0,   (byte)0,   (byte)128, (byte)132, (byte)0,   (byte)0,
                (byte)250, (byte)0,   (byte)0,   (byte)0,   (byte)128, (byte)232, (byte)0,   (byte)0,   (byte)117, (byte)48,
                (byte)0,   (byte)0,   (byte)234, (byte)96,  (byte)0,   (byte)0,   (byte)58,  (byte)152, (byte)0,   (byte)0,
                (byte)23,  (byte)112, (byte)156, (byte)186, (byte)81,  (byte)60,  (byte)0,   (byte)0,   (byte)3,   (byte)35,
                (byte)73,  (byte)68,  (byte)65,  (byte)84,  (byte)56,  (byte)79,  (byte)101, (byte)148, (byte)93,  (byte)76,
                (byte)82,  (byte)97,  (byte)24,  (byte)199, (byte)117, (byte)51,  (byte)175, (byte)188, (byte)168, (byte)173,
                (byte)181, (byte)165, (byte)91,  (byte)45,  (byte)115, (byte)171, (byte)46,  (byte)234, (byte)170, (byte)146,
                (byte)173, (byte)173, (byte)162, (byte)188, (byte)202, (byte)139, (byte)62,  (byte)101, (byte)94,  (byte)180,
                (byte)188, (byte)72,  (byte)113, (byte)173, (byte)230, (byte)77,  (byte)69,  (byte)110, (byte)100, (byte)75,
                (byte)153, (byte)217, (byte)178, (byte)178, (byte)89,  (byte)64,  (byte)169, (byte)164, (byte)2,   (byte)249,
                (byte)89,  (byte)41,  (byte)126, (byte)16,  (byte)42,  (byte)134, (byte)196, (byte)20,  (byte)1,   (byte)93,
                (byte)138, (byte)128, (byte)31,  (byte)147, (byte)249, (byte)129, (byte)31,  (byte)168, (byte)83,  (byte)135,
                (byte)224, (byte)225, (byte)96,  (byte)58,  (byte)251, (byte)159, (byte)78,  (byte)32,  (byte)111, (byte)61,
                (byte)123, (byte)118, (byte)246, (byte)236, (byte)255, (byte)254, (byte)127, (byte)207, (byte)121, (byte)223,
                (byte)247, (byte)188, (byte)231, (byte)141, (byte)116, (byte)187, (byte)221, (byte)17,  (byte)193, (byte)216,
                (byte)14,  (byte)70,  (byte)72,  (byte)65,  (byte)17,  (byte)25,  (byte)140, (byte)29,  (byte)17,  (byte)12,
                (byte)98,  (byte)126, (byte)126, (byte)126, (byte)106, (byte)106, (byte)234, (byte)193, (byte)195, (byte)154,
                (byte)147, (byte)167, (byte)95,  (byte)28,  (byte)138, (byte)207, (byte)11,  (byte)79,  (byte)40,  (byte)208,
                (byte)49,  (byte)10,  (byte)15,  (byte)107, (byte)142, (byte)96,  (byte)129, (byte)137, (byte)137, (byte)137,
                (byte)115, (byte)23,  (byte)139, (byte)174, (byte)222, (byte)170, (byte)171, (byte)238, (byte)112, (byte)89,
                (byte)38,  (byte)253, (byte)225, (byte)89,  (byte)213, (byte)238, (byte)186, (byte)124, (byte)179, (byte)246,
                (byte)236, (byte)133, (byte)34,  (byte)120, (byte)88,  (byte)140, (byte)97,  (byte)208, (byte)35,  (byte)45,
                (byte)189, (byte)236, (byte)252, (byte)245, (byte)170, (byte)207, (byte)86,  (byte)186, (byte)211, (byte)185,
                (byte)101, (byte)116, (byte)109, (byte)35,  (byte)187, (byte)167, (byte)254, (byte)22,  (byte)218, (byte)241,
                (byte)205, (byte)234, (byte)159, (byte)212, (byte)153, (byte)43,  (byte)202, (byte)180, (byte)244, (byte)82,
                (byte)56,  (byte)25,  (byte)6,   (byte)232, (byte)208, (byte)208, (byte)80,  (byte)92,  (byte)194, (byte)211,
                (byte)123, (byte)210, (byte)17,  (byte)153, (byte)153, (byte)254, (byte)106, (byte)219, (byte)232, (byte)24,
                (byte)223, (byte)106, (byte)181, (byte)175, (byte)39,  (byte)114, (byte)47,  (byte)169, (byte)29,  (byte)20,
                (byte)234, (byte)47,  (byte)182, (byte)141, (byte)247, (byte)70,  (byte)42,  (byte)189, (byte)216, (byte)1,
                (byte)15,  (byte)156, (byte)240, (byte)71,  (byte)204, (byte)205, (byte)205, (byte)25,  (byte)141, (byte)198,
                (byte)152, (byte)88,  (byte)81,  (byte)134, (byte)194, (byte)93,  (byte)208, (byte)233, (byte)43,  (byte)53,
                (byte)7,   (byte)26,  (byte)29,  (byte)191, (byte)120, (byte)119, (byte)132, (byte)88,  (byte)113, (byte)234,
                (byte)221, (byte)28,  (byte)212, (byte)31,  (byte)76,  (byte)244, (byte)179, (byte)206, (byte)245, (byte)12,
                (byte)165, (byte)59,  (byte)38,  (byte)78,  (byte)4,   (byte)39,  (byte)252, (byte)17,  (byte)179, (byte)179,
                (byte)179, (byte)93,  (byte)93,  (byte)93,  (byte)187, (byte)19,  (byte)10,  (byte)249, (byte)74,  (byte)119,
                (byte)190, (byte)214, (byte)39,  (byte)233, (byte)161, (byte)95,  (byte)53,  (byte)218, (byte)162, (byte)118,
                (byte)69,  (byte)131, (byte)193, (byte)19,  (byte)245, (byte)187, (byte)30,  (byte)127, (byte)94,  (byte)187,
                (byte)143, (byte)255, (byte)201, (byte)189, (byte)39,  (byte)161, (byte)16,  (byte)78,  (byte)248, (byte)25,
                (byte)70,  (byte)167, (byte)211, (byte)237, (byte)61,  (byte)94,  (byte)156, (byte)89,  (byte)181, (byte)144,
                (byte)219, (byte)238, (byte)43,  (byte)238, (byte)166, (byte)143, (byte)157, (byte)226, (byte)134, (byte)246,
                (byte)23,  (byte)245, (byte)27,  (byte)131, (byte)95,  (byte)248, (byte)109, (byte)13,  (byte)163, (byte)251,
                (byte)78,  (byte)188, (byte)133, (byte)115, (byte)135, (byte)217, (byte)159, (byte)88,  (byte)146, (byte)89,
                (byte)189, (byte)248, (byte)88,  (byte)227, (byte)125, (byte)249, (byte)195, (byte)47,  (byte)54,  (byte)6,
                (byte)194, (byte)179, (byte)80,  (byte)239, (byte)23,  (byte)180, (byte)48,  (byte)76,  (byte)44,  (byte)167,
                (byte)132, (byte)96,  (byte)14,  (byte)112, (byte)229, (byte)60,  (byte)217, (byte)194, (byte)163, (byte)22,
                (byte)207, (byte)115, (byte)29,  (byte)245, (byte)218, (byte)64,  (byte)135, (byte)103, (byte)254, (byte)119,
                (byte)234, (byte)126, (byte)147, (byte)231, (byte)90,  (byte)233, (byte)252, (byte)65,  (byte)174, (byte)156,
                (byte)96,  (byte)226, (byte)147, (byte)107, (byte)121, (byte)31,  (byte)23,  (byte)5,   (byte)205, (byte)158,
                (byte)220, (byte)14,  (byte)170, (byte)64,  (byte)71,  (byte)100, (byte)78,  (byte)155, (byte)239, (byte)97,
                (byte)243, (byte)90,  (byte)138, (byte)108, (byte)225, (byte)112, (byte)114, (byte)29,  (byte)193, (byte)28,
                (byte)185, (byte)161, (byte)226, (byte)149, (byte)47,  (byte)101, (byte)183, (byte)122, (byte)225, (byte)120,
                (byte)66,  (byte)166, (byte)80,  (byte)227, (byte)203, (byte)86,  (byte)251, (byte)82,  (byte)203, (byte)151,
                (byte)142, (byte)166, (byte)52,  (byte)17,  (byte)140, (byte)88,  (byte)44,  (byte)238, (byte)237, (byte)237,
                (byte)29,  (byte)28,  (byte)28,  (byte)116, (byte)56,  (byte)28,  (byte)35,  (byte)100, (byte)216, (byte)237,
                (byte)246, (byte)129, (byte)129, (byte)1,   (byte)147, (byte)201, (byte)36,  (byte)145, (byte)72,  (byte)8,
                (byte)166, (byte)172, (byte)172, (byte)12,  (byte)140, (byte)213, (byte)106, (byte)133, (byte)127, (byte)156,
                (byte)140, (byte)225, (byte)225, (byte)97,  (byte)232, (byte)96,  (byte)100, (byte)50,  (byte)25,  (byte)193,
                (byte)200, (byte)229, (byte)114, (byte)168, (byte)248, (byte)204, (byte)99,  (byte)99,  (byte)99,  (byte)56,
                (byte)32,  (byte)225, (byte)1,   (byte)5,   (byte)186, (byte)217, (byte)108, (byte)86,  (byte)42,  (byte)149,
                (byte)4,   (byte)83,  (byte)83,  (byte)83,  (byte)3,   (byte)213, (byte)102, (byte)179, (byte)57,  (byte)157,
                (byte)206, (byte)164, (byte)164, (byte)164, (byte)232, (byte)96,  (byte)112, (byte)56,  (byte)28,  (byte)188,
                (byte)21,  (byte)186, (byte)197, (byte)98,  (byte)169, (byte)175, (byte)175, (byte)39,  (byte)152, (byte)134,
                (byte)134, (byte)6,   (byte)168, (byte)88,  (byte)204, (byte)228, (byte)228, (byte)36,  (byte)96,  (byte)32,
                (byte)204, (byte)57,  (byte)136, (byte)138, (byte)194, (byte)135, (byte)135, (byte)130, (byte)37,  (byte)245,
                (byte)245, (byte)245, (byte)169, (byte)84,  (byte)42,  (byte)130, (byte)81,  (byte)171, (byte)213, (byte)80,
                (byte)49,  (byte)117, (byte)151, (byte)203, (byte)229, (byte)245, (byte)122, (byte)133, (byte)66,  (byte)230,
                (byte)188, (byte)101, (byte)101, (byte)101, (byte)161, (byte)134, (byte)2,   (byte)189, (byte)191, (byte)191,
                (byte)191, (byte)173, (byte)173, (byte)109, (byte)135, (byte)209, (byte)235, (byte)245, (byte)96,  (byte)48,
                (byte)105, (byte)252, (byte)33,  (byte)171, (byte)171, (byte)171, (byte)52,  (byte)77,  (byte)123, (byte)60,
                (byte)30,  (byte)204, (byte)112, (byte)121, (byte)121, (byte)25,  (byte)53,  (byte)20,  (byte)188, (byte)10,
                (byte)211, (byte)211, (byte)104, (byte)52,  (byte)112, (byte)50,  (byte)103, (byte)7,   (byte)231, (byte)20,
                (byte)59,  (byte)134, (byte)61,  (byte)65,  (byte)51,  (byte)156, (byte)115, (byte)191, (byte)223, (byte)191,
                (byte)249, (byte)39,  (byte)2,   (byte)129, (byte)0,   (byte)91,  (byte)80,  (byte)20,  (byte)133, (byte)127,
                (byte)102, (byte)116, (byte)116, (byte)180, (byte)162, (byte)162, (byte)2,   (byte)78,  (byte)230, (byte)92,
                (byte)195, (byte)135, (byte)30,  (byte)82,  (byte)169, (byte)20,  (byte)175, (byte)70,  (byte)99,  (byte)152,
                (byte)66,  (byte)183, (byte)2,   (byte)91,  (byte)64,  (byte)89,  (byte)89,  (byte)89,  (byte)209, (byte)106,
                (byte)181, (byte)240, (byte)192, (byte)201, (byte)252, (byte)63,  (byte)232, (byte)49,  (byte)61,  (byte)61,
                (byte)109, (byte)48,  (byte)24,  (byte)68,  (byte)34,  (byte)81,  (byte)101, (byte)101, (byte)229, (byte)204,
                (byte)204, (byte)204, (byte)63,  (byte)12,  (byte)20,  (byte)133, (byte)66,  (byte)129, (byte)81,  (byte)120,
                (byte)224, (byte)36,  (byte)238, (byte)3,   (byte)72,  (byte)232, (byte)36,  (byte)16,  (byte)8,   (byte)248,
                (byte)124, (byte)254, (byte)237, (byte)96,  (byte)160, (byte)134, (byte)2,   (byte)29,  (byte)163, (byte)196,
                (byte)125, (byte)192, (byte)94,  (byte)35,  (byte)232, (byte)129, (byte)61,  (byte)197, (byte)151, (byte)197,
                (byte)48,  (byte)214, (byte)202, (byte)6,   (byte)106, (byte)40,  (byte)208, (byte)49,  (byte)74,  (byte)220,
                (byte)59,  (byte)236, (byte)13,  (byte)196, (byte)146, (byte)88,  (byte)226, (byte)255, (byte)17,  (byte)114,
                (byte)179, (byte)182, (byte)223, (byte)186, (byte)228, (byte)137, (byte)143, (byte)70,  (byte)103, (byte)207,
                (byte)64,  (byte)0,   (byte)0,   (byte)0,   (byte)0,   (byte)73,  (byte)69,  (byte)78,  (byte)68,  (byte)174,
                (byte)66,  (byte)96,  (byte)130, } ) );
        }catch( Exception ex ){
            return null;
        }
    }

    private static Image getImg01()
    {
        try{
            return ImageIO.read( new ByteArrayInputStream( new byte[]{
                (byte)137, (byte)80,  (byte)78,  (byte)71,  (byte)13,  (byte)10,  (byte)26,  (byte)10,  (byte)0,   (byte)0,
                (byte)0,   (byte)13,  (byte)73,  (byte)72,  (byte)68,  (byte)82,  (byte)0,   (byte)0,   (byte)0,   (byte)17,
                (byte)0,   (byte)0,   (byte)0,   (byte)25,  (byte)8,   (byte)2,   (byte)0,   (byte)0,   (byte)0,   (byte)88,
                (byte)92,  (byte)82,  (byte)192, (byte)0,   (byte)0,   (byte)0,   (byte)1,   (byte)115, (byte)82,  (byte)71,
                (byte)66,  (byte)0,   (byte)174, (byte)206, (byte)28,  (byte)233, (byte)0,   (byte)0,   (byte)0,   (byte)4,
                (byte)103, (byte)65,  (byte)77,  (byte)65,  (byte)0,   (byte)0,   (byte)177, (byte)143, (byte)11,  (byte)252,
                (byte)97,  (byte)5,   (byte)0,   (byte)0,   (byte)0,   (byte)32,  (byte)99,  (byte)72,  (byte)82,  (byte)77,
                (byte)0,   (byte)0,   (byte)122, (byte)38,  (byte)0,   (byte)0,   (byte)128, (byte)132, (byte)0,   (byte)0,
                (byte)250, (byte)0,   (byte)0,   (byte)0,   (byte)128, (byte)232, (byte)0,   (byte)0,   (byte)117, (byte)48,
                (byte)0,   (byte)0,   (byte)234, (byte)96,  (byte)0,   (byte)0,   (byte)58,  (byte)152, (byte)0,   (byte)0,
                (byte)23,  (byte)112, (byte)156, (byte)186, (byte)81,  (byte)60,  (byte)0,   (byte)0,   (byte)3,   (byte)43,
                (byte)73,  (byte)68,  (byte)65,  (byte)84,  (byte)56,  (byte)79,  (byte)93,  (byte)84,  (byte)251, (byte)75,
                (byte)83,  (byte)113, (byte)20,  (byte)159, (byte)207, (byte)253, (byte)23,  (byte)73,  (byte)209, (byte)139,
                (byte)68,  (byte)250, (byte)49,  (byte)40,  (byte)176, (byte)31,  (byte)2,   (byte)209, (byte)137, (byte)136,
                (byte)191, (byte)148, (byte)189, (byte)108, (byte)101, (byte)104, (byte)137, (byte)218, (byte)114, (byte)206,
                (byte)199, (byte)148, (byte)208, (byte)9,   (byte)11,  (byte)19,  (byte)31,  (byte)211, (byte)181, (byte)245,
                (byte)192, (byte)21,  (byte)154, (byte)162, (byte)217, (byte)12,  (byte)179, (byte)4,   (byte)65,  (byte)152,
                (byte)102, (byte)99,  (byte)115, (byte)15,  (byte)53,  (byte)230, (byte)4,   (byte)157, (byte)238, (byte)225,
                (byte)221, (byte)211, (byte)109, (byte)14,  (byte)83,  (byte)135, (byte)184, (byte)173, (byte)219, (byte)103,
                (byte)92,  (byte)166, (byte)247, (byte)118, (byte)56,  (byte)63,  (byte)156, (byte)251, (byte)57,  (byte)159,
                (byte)207, (byte)249, (byte)158, (byte)115, (byte)239, (byte)185, (byte)223, (byte)36,  (byte)175, (byte)215,
                (byte)203, (byte)74,  (byte)24,  (byte)153, (byte)176, (byte)99,  (byte)4,   (byte)65,  (byte)82,  (byte)194,
                (byte)78,  (byte)64,  (byte)104, (byte)96,  (byte)30,  (byte)143, (byte)199, (byte)225, (byte)112, (byte)116,
                (byte)118, (byte)118, (byte)86,  (byte)86,  (byte)86,  (byte)242, (byte)152, (byte)86,  (byte)83,  (byte)83,
                (byte)211, (byte)213, (byte)213, (byte)133, (byte)44,  (byte)56,  (byte)20,  (byte)153, (byte)69,  (byte)9,
                (byte)108, (byte)54,  (byte)155, (byte)64,  (byte)32,  (byte)24,  (byte)27,  (byte)27,  (byte)115, (byte)185,
                (byte)92,  (byte)135, (byte)76,  (byte)115, (byte)58,  (byte)137, (byte)209, (byte)209, (byte)81,  (byte)40,
                (byte)193, (byte)161, (byte)100, (byte)113, (byte)13,  (byte)106, (byte)180, (byte)183, (byte)183, (byte)75,
                (byte)36,  (byte)146, (byte)189, (byte)189, (byte)189, (byte)104, (byte)52,  (byte)74,  (byte)53,  (byte)24,
                (byte)139, (byte)197, (byte)168, (byte)32,  (byte)18,  (byte)137, (byte)132, (byte)66,  (byte)33,  (byte)73,
                (byte)143, (byte)164, (byte)163, (byte)163, (byte)3,   (byte)204, (byte)184, (byte)6,   (byte)82,  (byte)147,
                (byte)201, (byte)84,  (byte)81,  (byte)81,  (byte)161, (byte)82,  (byte)169, (byte)130, (byte)193, (byte)32,
                (byte)206, (byte)136, (byte)69,  (byte)99,  (byte)7,   (byte)251, (byte)7,   (byte)28,  (byte)14,  (byte)39,
                (byte)124, (byte)16,  (byte)70,  (byte)28,  (byte)14,  (byte)135, (byte)125, (byte)62,  (byte)223, (byte)204,
                (byte)204, (byte)12,  (byte)218, (byte)6,   (byte)19,  (byte)124, (byte)150, (byte)219, (byte)237, (byte)214,
                (byte)104, (byte)52,  (byte)120, (byte)94,  (byte)92,  (byte)92,  (byte)68,  (byte)99,  (byte)40,  (byte)121,
                (byte)116, (byte)116, (byte)212, (byte)36,  (byte)108, (byte)194, (byte)196, (byte)205, (byte)205, (byte)205,
                (byte)136, (byte)119, (byte)130, (byte)59,  (byte)91,  (byte)91,  (byte)91,  (byte)200, (byte)86,  (byte)85,
                (byte)85,  (byte)129, (byte)9,   (byte)62,  (byte)11,  (byte)188, (byte)217, (byte)217, (byte)89,  (byte)180,
                (byte)187, (byte)180, (byte)180, (byte)68,  (byte)16,  (byte)4,   (byte)142, (byte)66,  (byte)144, (byte)158,
                (byte)158, (byte)14,  (byte)13,  (byte)155, (byte)205, (byte)94,  (byte)94,  (byte)94,  (byte)14,  (byte)4,
                (byte)2,   (byte)118, (byte)187, (byte)29,  (byte)32,  (byte)159, (byte)207, (byte)7,   (byte)19,  (byte)252,
                (byte)184, (byte)6,   (byte)93,  (byte)53,  (byte)54,  (byte)54,  (byte)170, (byte)213, (byte)106, (byte)180,
                (byte)235, (byte)247, (byte)251, (byte)179, (byte)179, (byte)179, (byte)147, (byte)19,  (byte)134, (byte)216,
                (byte)231, (byte)219, (byte)182, (byte)90,  (byte)173, (byte)200, (byte)10,  (byte)133, (byte)66,  (byte)48,
                (byte)79,  (byte)52,  (byte)45,  (byte)45,  (byte)45,  (byte)122, (byte)189, (byte)30,  (byte)57,  (byte)140,
                (byte)136, (byte)163, (byte)232, (byte)134, (byte)1,   (byte)44,  (byte)22,  (byte)11,  (byte)178, (byte)34,
                (byte)145, (byte)136, (byte)161, (byte)17,  (byte)139, (byte)197, (byte)6,   (byte)131, (byte)193, (byte)178,
                (byte)97,  (byte)65,  (byte)25,  (byte)76,  (byte)76,  (byte)55,  (byte)167, (byte)211, (byte)185, (byte)190,
                (byte)190, (byte)142, (byte)44,  (byte)222, (byte)45,  (byte)67,  (byte)131, (byte)175, (byte)105, (byte)52,
                (byte)26,  (byte)81,  (byte)15,  (byte)227, (byte)98,  (byte)74,  (byte)186, (byte)1,   (byte)129, (byte)6,
                (byte)217, (byte)238, (byte)238, (byte)110, (byte)134, (byte)70,  (byte)42,  (byte)149, (byte)226, (byte)205,
                (byte)172, (byte)174, (byte)174, (byte)174, (byte)175, (byte)173, (byte)97,  (byte)98,  (byte)186, (byte)65,
                (byte)96,  (byte)54,  (byte)155, (byte)145, (byte)149, (byte)201, (byte)100, (byte)12,  (byte)205, (byte)169,
                (byte)34,  (byte)101, (byte)174, (byte)100, (byte)227, (byte)86,  (byte)63,  (byte)241, (byte)96,  (byte)208,
                (byte)93,  (byte)58,  (byte)236, (byte)165, (byte)123, (byte)201, (byte)160, (byte)251, (byte)102, (byte)63,
                (byte)145, (byte)215, (byte)187, (byte)145, (byte)81,  (byte)164, (byte)100, (byte)104, (byte)78,  (byte)23,
                (byte)79,  (byte)230, (byte)245, (byte)110, (byte)22,  (byte)43,  (byte)156, (byte)143, (byte)134, (byte)188,
                (byte)21,  (byte)99,  (byte)126, (byte)186, (byte)3,   (byte)41,  (byte)86,  (byte)64,  (byte)179, (byte)121,
                (byte)230, (byte)246, (byte)36,  (byte)67,  (byte)115, (byte)150, (byte)59,  (byte)205, (byte)233, (byte)179,
                (byte)222, (byte)249, (byte)224, (byte)42,  (byte)31,  (byte)217, (byte)174, (byte)253, (byte)22,  (byte)162,
                (byte)251, (byte)147, (byte)81,  (byte)255, (byte)221, (byte)143, (byte)238, (byte)252, (byte)190, (byte)205,
                (byte)115, (byte)15,  (byte)167, (byte)25,  (byte)154, (byte)11,  (byte)229, (byte)170, (byte)124, (byte)169,
                (byte)237, (byte)254, (byte)128, (byte)187, (byte)250, (byte)75,  (byte)224, (byte)252, (byte)149, (byte)220,
                (byte)148, (byte)52,  (byte)54,  (byte)229, (byte)25,  (byte)89,  (byte)215, (byte)170, (byte)149, (byte)126,
                (byte)224, (byte)5,   (byte)175, (byte)109, (byte)23,  (byte)159, (byte)66,  (byte)66,  (byte)251, (byte)62,
                (byte)151, (byte)158, (byte)169, (byte)11,  (byte)100, (byte)118, (byte)238, (byte)160, (byte)7,   (byte)39,
                (byte)8,   (byte)134, (byte)204, (byte)169, (byte)105, (byte)108, (byte)236, (byte)65,  (byte)114, (byte)74,
                (byte)42,  (byte)79,  (byte)161, (byte)231, (byte)79,  (byte)132, (byte)184, (byte)159, (byte)60,  (byte)5,
                (byte)50,  (byte)91,  (byte)38,  (byte)239, (byte)23,  (byte)67,  (byte)147, (byte)85,  (byte)175, (byte)43,
                (byte)124, (byte)227, (byte)120, (byte)60,  (byte)236, (byte)21,  (byte)78,  (byte)253, (byte)145, (byte)234,
                (byte)200, (byte)252, (byte)178, (byte)86,  (byte)104, (byte)110, (byte)220, (byte)171, (byte)69,  (byte)220,
                (byte)248, (byte)99,  (byte)23,  (byte)120, (byte)161, (byte)220, (byte)113, (byte)185, (byte)65,  (byte)119,
                (byte)162, (byte)153, (byte)155, (byte)155, (byte)187, (byte)46,  (byte)210, (byte)150, (byte)12,  (byte)184,
                (byte)27,  (byte)190, (byte)239, (byte)246, (byte)104, (byte)254, (byte)42,  (byte)76,  (byte)228, (byte)91,
                (byte)227, (byte)97,  (byte)230, (byte)213, (byte)28,  (byte)185, (byte)110, (byte)31,  (byte)49,  (byte)16,
                (byte)224, (byte)104, (byte)33,  (byte)231, (byte)165, (byte)30,  (byte)204, (byte)248, (byte)238, (byte)224,
                (byte)243, (byte)105, (byte)181, (byte)218, (byte)226, (byte)214, (byte)207, (byte)213, (byte)202, (byte)224,
                (byte)171, (byte)159, (byte)17,  (byte)133, (byte)153, (byte)28,  (byte)177, (byte)198, (byte)125, (byte)216,
                (byte)18,  (byte)165, (byte)2,   (byte)197, (byte)10,  (byte)9,   (byte)252, (byte)249, (byte)215, (byte)29,
                (byte)174, (byte)88,  (byte)9,   (byte)102, (byte)124, (byte)175, (byte)177, (byte)78,  (byte)43,  (byte)43,
                (byte)43,  (byte)114, (byte)185, (byte)156, (byte)167, (byte)48,  (byte)188, (byte)251, (byte)77,  (byte)142,
                (byte)187, (byte)200, (byte)169, (byte)0,   (byte)195, (byte)199, (byte)157, (byte)100, (byte)191, (byte)137,
                (byte)172, (byte)31,  (byte)48,  (byte)130, (byte)3,   (byte)102, (byte)252, (byte)255, (byte)193, (byte)82,
                (byte)98,  (byte)59,  (byte)230, (byte)231, (byte)231, (byte)219, (byte)218, (byte)218, (byte)94,  (byte)188,
                (byte)87,  (byte)78,  (byte)16,  (byte)30,  (byte)29,  (byte)73,  (byte)210, (byte)125, (byte)146, (byte)240,
                (byte)136, (byte)20,  (byte)74,  (byte)100, (byte)193, (byte)1,   (byte)147, (byte)113, (byte)31,  (byte)0,
                (byte)66,  (byte)165, (byte)186, (byte)186, (byte)186, (byte)178, (byte)178, (byte)178, (byte)210, (byte)132,
                (byte)33,  (byte)6,   (byte)2,   (byte)28,  (byte)89,  (byte)198, (byte)125, (byte)64,  (byte)93,  (byte)35,
                (byte)168, (byte)129, (byte)189, (byte)90,  (byte)88,  (byte)88,  (byte)64,  (byte)26,  (byte)179, (byte)82,
                (byte)134, (byte)24,  (byte)8,   (byte)112, (byte)100, (byte)25,  (byte)247, (byte)14,  (byte)117, (byte)3,
                (byte)81,  (byte)202, (byte)255, (byte)150, (byte)154, (byte)122, (byte)60,  (byte)102, (byte)83,  (byte)180,
                (byte)127, (byte)168, (byte)143, (byte)135, (byte)100, (byte)234, (byte)63,  (byte)39,  (byte)153, (byte)0,
                (byte)0,   (byte)0,   (byte)0,   (byte)73,  (byte)69,  (byte)78,  (byte)68,  (byte)174, (byte)66,  (byte)96,
                (byte)130, } ) );
        }catch( Exception ex ){
            return null;
        }
    }
}
