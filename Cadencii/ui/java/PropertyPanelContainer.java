package com.github.cadencii.ui;
//SECTION-BEGIN-IMPORT

import java.awt.Component;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Image;
import java.io.ByteArrayInputStream;
import javax.imageio.ImageIO;
import javax.swing.BorderFactory;
import javax.swing.JScrollPane;
import com.github.cadencii.windows.forms.BMouseEventArgs;
import com.github.cadencii.windows.forms.BMouseEventHandler;
import com.github.cadencii.windows.forms.BPaintEventArgs;
import com.github.cadencii.windows.forms.BPaintEventHandler;
import com.github.cadencii.windows.forms.BPanel;

//SECTION-END-IMPORT
public class PropertyPanelContainer extends BPanel {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 2607395876517475240L;
    final int CLOSE_BUTTON_X = 14;
    final int EXIT_BUTTON_X = 38;
    final int MOUSE_TOLERANCE = 3;
    private boolean mExitButtonMouseDown = false;
    private boolean mCloseButtonMouseDown = false;

    private BPanel panelTitle = null;
    private JScrollPane panelMain = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public PropertyPanelContainer()
    {
    	super();
    	initialize();
    	this.setMinimumSize( new Dimension( 0, 0 ) );
    }

    /**
     * ダミー関数．クローズボタンが押された時によばれる．
     * 実際にはProeprtyGridContainer.csに実装される
     */
    private void handleClose()
    {
    }
    
    /**
     * ダミー関数．ウィンドウ化ボタンが押され時に呼ばれる．
     * 実際にはPropertyGridContainer.csで実装される．
     */
    private void handleRestoreWindow()
    {
    }
    
    //SECTION-BEGIN-METHOD

    Image mCloseImage = null;  //  @jve:decl-index=0:
    private Image getCloseImage()
    {
        if( mCloseImage != null ){
            return mCloseImage;
        }
        try{
            mCloseImage = ImageIO.read( new ByteArrayInputStream( new byte[]{
                (byte)137, (byte)80,  (byte)78,  (byte)71,  (byte)13,  (byte)10,  (byte)26,  (byte)10,  (byte)0,   (byte)0,
                (byte)0,   (byte)13,  (byte)73,  (byte)72,  (byte)68,  (byte)82,  (byte)0,   (byte)0,   (byte)0,   (byte)15,
                (byte)0,   (byte)0,   (byte)0,   (byte)16,  (byte)8,   (byte)6,   (byte)0,   (byte)0,   (byte)0,   (byte)201,
                (byte)86,  (byte)37,  (byte)4,   (byte)0,   (byte)0,   (byte)0,   (byte)1,   (byte)115, (byte)82,  (byte)71,
                (byte)66,  (byte)0,   (byte)174, (byte)206, (byte)28,  (byte)233, (byte)0,   (byte)0,   (byte)0,   (byte)4,
                (byte)103, (byte)65,  (byte)77,  (byte)65,  (byte)0,   (byte)0,   (byte)177, (byte)143, (byte)11,  (byte)252,
                (byte)97,  (byte)5,   (byte)0,   (byte)0,   (byte)0,   (byte)32,  (byte)99,  (byte)72,  (byte)82,  (byte)77,
                (byte)0,   (byte)0,   (byte)122, (byte)38,  (byte)0,   (byte)0,   (byte)128, (byte)132, (byte)0,   (byte)0,
                (byte)250, (byte)0,   (byte)0,   (byte)0,   (byte)128, (byte)232, (byte)0,   (byte)0,   (byte)117, (byte)48,
                (byte)0,   (byte)0,   (byte)234, (byte)96,  (byte)0,   (byte)0,   (byte)58,  (byte)152, (byte)0,   (byte)0,
                (byte)23,  (byte)112, (byte)156, (byte)186, (byte)81,  (byte)60,  (byte)0,   (byte)0,   (byte)0,   (byte)146,
                (byte)73,  (byte)68,  (byte)65,  (byte)84,  (byte)56,  (byte)79,  (byte)99,  (byte)252, (byte)255, (byte)255,
                (byte)63,  (byte)3,   (byte)217, (byte)0,   (byte)164, (byte)153, (byte)92,  (byte)12,  (byte)215, (byte)248,
                (byte)241, (byte)227, (byte)71,  (byte)145, (byte)218, (byte)218, (byte)218, (byte)125, (byte)247, (byte)239,
                (byte)223, (byte)215, (byte)195, (byte)102, (byte)24,  (byte)72,  (byte)28,  (byte)36,  (byte)15,  (byte)82,
                (byte)7,   (byte)147, (byte)135, (byte)107, (byte)6,   (byte)73,  (byte)4,   (byte)6,   (byte)6,   (byte)254,
                (byte)143, (byte)139, (byte)139, (byte)123, (byte)141, (byte)110, (byte)0,   (byte)136, (byte)15,  (byte)18,
                (byte)7,   (byte)201, (byte)131, (byte)212, (byte)97,  (byte)104, (byte)70,  (byte)86,  (byte)128, (byte)108,
                (byte)0,   (byte)46,  (byte)113, (byte)112, (byte)88,  (byte)33,  (byte)59,  (byte)17,  (byte)93,  (byte)225,
                (byte)193, (byte)131, (byte)7,   (byte)163, (byte)96,  (byte)54,  (byte)98,  (byte)115, (byte)17,  (byte)70,
                (byte)96,  (byte)33,  (byte)27,  (byte)0,   (byte)114, (byte)38,  (byte)46,  (byte)175, (byte)96,  (byte)216,
                (byte)12,  (byte)115, (byte)197, (byte)129, (byte)3,   (byte)7,   (byte)98,  (byte)96,  (byte)26,  (byte)65,
                (byte)52,  (byte)200, (byte)5,   (byte)216, (byte)2,   (byte)145, (byte)122, (byte)54,  (byte)163, (byte)251,
                (byte)25,  (byte)228, (byte)2,   (byte)162, (byte)252, (byte)76,  (byte)81,  (byte)104, (byte)83,  (byte)20,
                (byte)207, (byte)20,  (byte)165, (byte)48,  (byte)114, (byte)210, (byte)55,  (byte)217, (byte)153, (byte)130,
                (byte)162, (byte)220, (byte)8,   (byte)202, (byte)198, (byte)0,   (byte)57,  (byte)245, (byte)60,  (byte)182,
                (byte)148, (byte)26,  (byte)129, (byte)215, (byte)0,   (byte)0,   (byte)0,   (byte)0,   (byte)73,  (byte)69,
                (byte)78,  (byte)68,  (byte)174, (byte)66,  (byte)96,  (byte)130, } ) );
        }catch( Exception ex ){
            mCloseImage = null;
        }
        return mCloseImage;
    }

    private Image mExitFullscreenImage = null;  //  @jve:decl-index=0:
    private Image getExitFullscreenImage()
    {
        if( mExitFullscreenImage != null ){
            return mExitFullscreenImage;
        }
        try{
            mExitFullscreenImage = ImageIO.read( new ByteArrayInputStream( new byte[]{
                (byte)137, (byte)80,  (byte)78,  (byte)71,  (byte)13,  (byte)10,  (byte)26,  (byte)10,  (byte)0,   (byte)0,
                (byte)0,   (byte)13,  (byte)73,  (byte)72,  (byte)68,  (byte)82,  (byte)0,   (byte)0,   (byte)0,   (byte)16,
                (byte)0,   (byte)0,   (byte)0,   (byte)15,  (byte)8,   (byte)6,   (byte)0,   (byte)0,   (byte)0,   (byte)237,
                (byte)115, (byte)79,  (byte)47,  (byte)0,   (byte)0,   (byte)0,   (byte)1,   (byte)115, (byte)82,  (byte)71,
                (byte)66,  (byte)0,   (byte)174, (byte)206, (byte)28,  (byte)233, (byte)0,   (byte)0,   (byte)0,   (byte)4,
                (byte)103, (byte)65,  (byte)77,  (byte)65,  (byte)0,   (byte)0,   (byte)177, (byte)143, (byte)11,  (byte)252,
                (byte)97,  (byte)5,   (byte)0,   (byte)0,   (byte)0,   (byte)32,  (byte)99,  (byte)72,  (byte)82,  (byte)77,
                (byte)0,   (byte)0,   (byte)122, (byte)38,  (byte)0,   (byte)0,   (byte)128, (byte)132, (byte)0,   (byte)0,
                (byte)250, (byte)0,   (byte)0,   (byte)0,   (byte)128, (byte)232, (byte)0,   (byte)0,   (byte)117, (byte)48,
                (byte)0,   (byte)0,   (byte)234, (byte)96,  (byte)0,   (byte)0,   (byte)58,  (byte)152, (byte)0,   (byte)0,
                (byte)23,  (byte)112, (byte)156, (byte)186, (byte)81,  (byte)60,  (byte)0,   (byte)0,   (byte)0,   (byte)148,
                (byte)73,  (byte)68,  (byte)65,  (byte)84,  (byte)56,  (byte)79,  (byte)189, (byte)147, (byte)97,  (byte)10,
                (byte)128, (byte)32,  (byte)12,  (byte)133, (byte)205, (byte)35,  (byte)121, (byte)139, (byte)160, (byte)63,
                (byte)222, (byte)168, (byte)219, (byte)120, (byte)132, (byte)14,  (byte)161, (byte)30,  (byte)41,  (byte)22,
                (byte)47,  (byte)120, (byte)33,  (byte)67,  (byte)114, (byte)81,  (byte)36,  (byte)12,  (byte)135, (byte)242,
                (byte)62,  (byte)159, (byte)110, (byte)78,  (byte)34,  (byte)226, (byte)74,  (byte)41,  (byte)139, (byte)247,
                (byte)126, (byte)15,  (byte)33,  (byte)108, (byte)238, (byte)233, (byte)200, (byte)57,  (byte)47,  (byte)49,
                (byte)70,  (byte)65,  (byte)164, (byte)148, (byte)86,  (byte)0,   (byte)17,  (byte)109, (byte)206, (byte)181,
                (byte)222, (byte)236, (byte)106, (byte)173, (byte)51,  (byte)1,   (byte)132, (byte)64,  (byte)140, (byte)252,
                (byte)78,  (byte)200, (byte)189, (byte)235, (byte)180, (byte)22,  (byte)194, (byte)220, (byte)12,  (byte)160,
                (byte)101, (byte)13,  (byte)249, (byte)15,  (byte)192, (byte)59,  (byte)191, (byte)114, (byte)96,  (byte)177,
                (byte)219, (byte)173, (byte)130, (byte)69,  (byte)216, (byte)123, (byte)96,  (byte)150, (byte)249, (byte)172,
                (byte)194, (byte)40,  (byte)52,  (byte)160, (byte)237, (byte)145, (byte)161, (byte)24,  (byte)240, (byte)207,
                (byte)1,   (byte)109, (byte)215, (byte)154, (byte)29,  (byte)192, (byte)182, (byte)174, (byte)22,  (byte)186,
                (byte)216, (byte)4,   (byte)208, (byte)127, (byte)132, (byte)87,  (byte)194, (byte)63,  (byte)50,  (byte)1,
                (byte)244, (byte)35,  (byte)227, (byte)100, (byte)136, (byte)177, (byte)126, (byte)0,   (byte)17,  (byte)67,
                (byte)105, (byte)22,  (byte)143, (byte)97,  (byte)44,  (byte)110, (byte)0,   (byte)0,   (byte)0,   (byte)0,
                (byte)73,  (byte)69,  (byte)78,  (byte)68,  (byte)174, (byte)66,  (byte)96,  (byte)130, } ) );
        }catch( Exception ex ){
            mExitFullscreenImage = null;
        }
        return mExitFullscreenImage;
    }

    public void addComponent( Component comp ){
        getPanelMain().setViewportView( comp );
    }
    
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
        gridBagConstraints1.gridx = 0;
        gridBagConstraints1.weighty = 1.0D;
        gridBagConstraints1.weightx = 1.0D;
        gridBagConstraints1.fill = GridBagConstraints.BOTH;
        gridBagConstraints1.gridy = 1;
        GridBagConstraints gridBagConstraints = new GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.weightx = 1.0D;
        gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
        gridBagConstraints.gridy = 0;
        this.setLayout(new GridBagLayout());
        this.setSize(new Dimension(205, 223));
        this.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        this.add(getPanelTitle(), gridBagConstraints);
        this.add(getPanelMain(), gridBagConstraints1);

        panelTitle.paintEvent.add( new BPaintEventHandler( this, "panelTitle_Paint" ) );
    	panelTitle.mouseDownEvent.add( new BMouseEventHandler( this, "panelTitle_MouseDown" ) );
    	panelTitle.mouseClickEvent.add( new BMouseEventHandler( this, "panelTitle_MouseClick" ) );
    	panelTitle.mouseUpEvent.add( new BMouseEventHandler( this, "panelTitle_MouseUp" ) );
    }

    public void panelTitle_MouseUp( Object sender, BMouseEventArgs e )
    {
        mCloseButtonMouseDown = false;
        mExitButtonMouseDown = false;
        panelTitle.repaint();
    }
    
    public void panelTitle_MouseDown( Object sender, BMouseEventArgs e )
    {
        mCloseButtonMouseDown = false;
        mExitButtonMouseDown = false;

        int panel_height = panelTitle.getHeight();
        Image close = getCloseImage();
        if( close != null ){
            int w = close.getWidth( null );
            int h = close.getHeight( null );
            int x0 = CLOSE_BUTTON_X - w / 2 - MOUSE_TOLERANCE;
            int x1 = CLOSE_BUTTON_X + w / 2 + MOUSE_TOLERANCE;
            int y0 = panel_height / 2 - h / 2 - MOUSE_TOLERANCE;
            int y1 = panel_height / 2 + h / 2 + MOUSE_TOLERANCE;
            if( x0 <= e.X && e.X <= x1 && y0 <= e.Y && e.Y <= y1 ){
                mCloseButtonMouseDown = true;
            }
        }
        
        Image exit = getExitFullscreenImage();
        if( exit != null ){
            int w = exit.getWidth( null );
            int h = exit.getHeight( null );
            int x0 = EXIT_BUTTON_X - w / 2 - MOUSE_TOLERANCE;
            int x1 = EXIT_BUTTON_X + w / 2 + MOUSE_TOLERANCE;
            int y0 = panel_height / 2 - h / 2 - MOUSE_TOLERANCE;
            int y1 = panel_height / 2 + h / 2 + MOUSE_TOLERANCE;
            if( x0 <= e.X && e.X <= x1 && y0 <= e.Y && e.Y <= y1 ){
                mExitButtonMouseDown = true;
            }
        }
        panelTitle.repaint();
    }
    
    public void panelTitle_MouseClick( Object sender, BMouseEventArgs e )
    {
        int panel_height = panelTitle.getHeight();
        Image close = getCloseImage();
        if( close != null ){
            int w = close.getWidth( null );
            int h = close.getHeight( null );
            int x0 = CLOSE_BUTTON_X - w / 2 - MOUSE_TOLERANCE;
            int x1 = CLOSE_BUTTON_X + w / 2 + MOUSE_TOLERANCE;
            int y0 = panel_height / 2 - h / 2 - MOUSE_TOLERANCE;
            int y1 = panel_height / 2 + h / 2 + MOUSE_TOLERANCE;
            if( x0 <= e.X && e.X <= x1 && y0 <= e.Y && e.Y <= y1 ){
                handleClose();
            }
        }
        
        Image exit = getExitFullscreenImage();
        if( exit != null ){
            int w = exit.getWidth( null );
            int h = exit.getHeight( null );
            int x0 = EXIT_BUTTON_X - w / 2 - MOUSE_TOLERANCE;
            int x1 = EXIT_BUTTON_X + w / 2 + MOUSE_TOLERANCE;
            int y0 = panel_height / 2 - h / 2 - MOUSE_TOLERANCE;
            int y1 = panel_height / 2 + h / 2 + MOUSE_TOLERANCE;
            if( x0 <= e.X && e.X <= x1 && y0 <= e.Y && e.Y <= y1 ){
                handleRestoreWindow();
            }
        }
        
        mCloseButtonMouseDown = false;
        mExitButtonMouseDown = false;
        panelTitle.repaint();
    }
    
    public void panelTitle_Paint( Object sender, BPaintEventArgs e )
    {
        Image close = getCloseImage();
        int panel_height = panelTitle.getHeight();
        if( close != null ){
            int w = close.getWidth( null );
            int h = close.getHeight( null );
            int delta = mCloseButtonMouseDown ? 1 : 0;
            e.Graphics.drawImage(
                close, 
                CLOSE_BUTTON_X - w / 2 + delta, panel_height / 2 - h / 2 + delta,
                null );
        }
        Image exit = getExitFullscreenImage();
        if( exit != null ){
            int w = exit.getWidth( null );
            int h = exit.getHeight( null );
            int delta = mExitButtonMouseDown ? 1 : 0;
            e.Graphics.drawImage(
                exit,
                EXIT_BUTTON_X - w / 2 + delta, panel_height / 2 - h / 2 + delta,
                null );
        }
    }
    
    /**
     * This method initializes panelTitle	
     * 	
     * @return javax.swing.JPanel	
     */
    private BPanel getPanelTitle() {
        if (panelTitle == null) {
            panelTitle = new BPanel();
            panelTitle.setLayout(new GridBagLayout());
            panelTitle.setPreferredSize(new Dimension(4, 20));
            panelTitle.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        }
        return panelTitle;
    }

    /**
     * This method initializes panelMain	
     * 	
     * @return javax.swing.JScrollPane
     */
    private JScrollPane getPanelMain() {
        if (panelMain == null) {
            panelMain = new JScrollPane();
            panelMain.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            panelMain.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
            panelMain.setPreferredSize(new Dimension(4, 4));
            panelMain.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
        }
        return panelMain;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="51,-10"
