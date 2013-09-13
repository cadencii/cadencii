package com.github.cadencii.ui;

import java.awt.AWTEvent;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.AWTEventListener;
import java.awt.event.KeyEvent;
import java.awt.font.FontRenderContext;
import java.awt.font.GlyphMetrics;
import java.awt.font.GlyphVector;
import java.awt.geom.Point2D;

import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JDialog;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JSpinner;
import javax.swing.SpinnerNumberModel;
import javax.swing.UIManager;
import javax.swing.border.TitledBorder;

public class DialogBase extends JDialog
                   implements AWTEventListener
{
    private static final long serialVersionUID = 6813116345545558212L;

    /**
     * ESCを押したときにクリックするボタン
     */
    private JButton cancelButton = null;

    /**
     * ダイアログの結果
     */
    private boolean dialogResult = false;

    public DialogBase()
    {
        super();
        setDefaultCloseOperation( DO_NOTHING_ON_CLOSE );
        try{
            UIManager.getInstalledLookAndFeels();
            UIManager.setLookAndFeel( UIManager.getSystemLookAndFeelClassName() );
        }catch( Exception ex ){
            System.err.println( "DialogBase#.ctor; ex=" + ex );
        }
    }

    protected void setCancelButton( JButton button )
    {
        cancelButton = button;
    }

    public void eventDispatched( AWTEvent arg0 )
    {
        if ( cancelButton == null ){
            return;
        }
        if( !(arg0 instanceof KeyEvent ) ){
            return;
        }
        KeyEvent e = (KeyEvent)arg0;
        int state = e.getID();
        if ( state != KeyEvent.KEY_PRESSED ){
            return;
        }
        Object obj = e.getComponent();
        if ( obj == null ){
            return;
        }
        int code = e.getKeyCode();
        if ( code == KeyEvent.VK_ESCAPE ){
            if ( obj instanceof JComboBox ){
                JComboBox cb = (JComboBox)obj;
                if( cb.isPopupVisible() ){
                    // ポップアップが表示中の場合は何もしない
                    return;
                }
            }
            cancelButton.doClick();
        }
    }

    protected int doShowDialog( Object parent )
    {
        setModalityType( ModalityType.APPLICATION_MODAL );
        setVisible( true );
        return this.dialogResult ? 1 : 0;
    }

    public Dimension getClientSize()
    {
        return getContentPane().getSize();
    }

    protected void doClose()
    {
        setVisible( false );
        dispose();
    }

    public boolean getDialogResult()
    {
        return this.dialogResult;
    }

    protected void setDialogResult( boolean value )
    {
        this.dialogResult = value;
        setVisible( false );
    }

    static class AutoElipsisLabel
    {
        /**
         * はみ出した文字列を自動で改行する機能を付加した JLabel のインスタンスを作成する
         * @wbp.factory
         */
        public static JLabel createAutoEllipsisLabel()
        {
            JLabel label = new JLabel(){
                private static final long serialVersionUID = 3954232782610344326L;
                private GlyphVector glyphVector;
                private int lastWidth = 0;
                private int lastHeight = 0;
                private String lastText = "";

                @Override
                protected void paintComponent( Graphics g )
                {
                    Graphics2D g2 = (Graphics2D)g;
                    if( this.isResized() ){
                        int width = this.getWidth();
                        int height = this.getHeight();
                        String text = this.getText();
                        int wrap = width
                                 - this.getInsets().left
                                 - this.getInsets().right;
                        FontRenderContext frc = g2.getFontRenderContext();
                        this.glyphVector = getWrappedGlyphVector( text, wrap, this.getFont(), frc );
                        this.lastWidth = width;
                        this.lastHeight = height;
                        this.lastText = text;
                    }
                    g2.drawGlyphVector( this.glyphVector,
                                        this.getInsets().left,
                                        this.getInsets().top + this.getFont().getSize());
                }

                private boolean isResized()
                {
                    return (this.lastWidth != this.getWidth() ||
                        this.lastHeight != this.getHeight() ||
                        false == this.lastText.equals( this.getText() ));
                }
            };

            return label;
        }

        static private GlyphVector getWrappedGlyphVector( String str, float wrapping, Font font, FontRenderContext frc )
        {
            Point2D gmPos = new Point2D.Double( 0.0d, 0.0d );
            GlyphVector gv = font.createGlyphVector( frc, str );
            float lineheight = (float)gv.getLogicalBounds().getHeight();
            float xpos = 0.0f;
            float advance = 0.0f;
            int lineCount = 0;
            GlyphMetrics gm;
            for( int i = 0; i < gv.getNumGlyphs(); i++ ){
                gm = gv.getGlyphMetrics( i );
                advance = gm.getAdvance();
                if( xpos < wrapping && wrapping <= xpos + advance ){
                    lineCount++;
                    xpos = 0.0f;
                }
                gmPos.setLocation( xpos, lineheight * lineCount );
                gv.setGlyphPosition( i, gmPos );
                xpos = xpos + advance;
            }
            return gv;
        }
    }

    static class GroupBox
    {
        /**
         * @wbp.factory
         */
        static public JPanel create()
        {
            JPanel panel = new JPanel()
            {
                private static final long serialVersionUID = 1L;

                private JPanel initialize()
                {
                    this.setSize(new Dimension(352, 268));
                    setTitle( this, "" );
                    super.setBorder( getTitledBorder() );
                    return this;
                }

                /*public String getTitle(){
                    Object obj = super.getBorder();
                    if( obj == null ){
                        super.setBorder( getTitledBorder() );
                        return "";
                    }else{
                        if( obj instanceof TitledBorder ){
                            TitledBorder border = (TitledBorder)obj;
                            return border.getTitle();
                        }else{
                            super.setBorder( getTitledBorder() );
                            return "";
                        }
                    }
                }*/
            }.initialize();
            return panel;
        }

        /**
         * createGroupBox メソッドで生成した JPanel のインスタンスに対して、タイトル文字列を設定する
         * @param groupBox
         * @param title
         */
        static public void setTitle( JPanel groupBox, String title )
        {
            Object obj = groupBox.getBorder();
            if( obj == null ){
                TitledBorder border = getTitledBorder();
                border.setTitle( title );
                groupBox.setBorder( border );
            }else{
                if( obj instanceof TitledBorder ){
                    TitledBorder border = (TitledBorder)obj;
                    border.setTitle( title );
                }else{
                    TitledBorder border = getTitledBorder();
                    border.setTitle( title );
                    groupBox.setBorder( border );
                }
            }
        }

        /**
         * createGroupBox メソッドで生成した JPanel のインスタンスで使用する TitledBorder を生成する
         * @return
         */
        static private TitledBorder getTitledBorder()
        {
            return BorderFactory.createTitledBorder(
                null,
                "",
                TitledBorder.DEFAULT_JUSTIFICATION,
                TitledBorder.DEFAULT_POSITION,
                new Font( "Dialog", Font.BOLD, 12 ),
                new Color( 51, 51, 51 )
            );
        }
    }

    static class Spinner
    {
        /**
         * @wbp.factory
         * @return
         */
        static public JSpinner create()
        {
            JSpinner spinner = new JSpinner( new SpinnerNumberModel( 0.0, 0.0, 100.0, 1.0 ) )
            {
                private static final long serialVersionUID = -1678016159355102909L;

                public JSpinner initialize()
                {
                    setEditor( new JSpinner.NumberEditor( this, "0" ) );
                    return this;
                }

                /*public Object getValue()
                {
                    return Spinner.getFloatValue( this );
                }

                public void setValue( Object value )
                {
                    if( value == null ){
                        return;
                    }
                    if( value instanceof Number ){
                        Number n = (Number)value;
                        Spinner.setFloatValue( this, n.floatValue() );
                    }
                }*/
            }.initialize();
            return spinner;
        }

        static public float getFloatValue( JSpinner spinner )
        {
            Double d = (Double)spinner.getValue();
            return d.floatValue();
        }

        static public void setFloatValue( JSpinner spinner, float value )
        {
            Double d = Double.valueOf( value );
            spinner.setValue( d );
        }

        static public void setMaximum( JSpinner spinner, float value )
        {
            Double d = Double.valueOf( (double)value );
            ((SpinnerNumberModel)spinner.getModel()).setMaximum( d );
        }

        static public float getMaximum( JSpinner spinner )
        {
            Double d = (Double)((SpinnerNumberModel)spinner.getModel()).getMaximum();
            return d.floatValue();
        }

        static public void setMinimum( JSpinner spinner, float value )
        {
            Double d = Double.valueOf( value );
            ((SpinnerNumberModel)spinner.getModel()).setMinimum( d );
        }

        static public float getMinimum( JSpinner spinner )
        {
            Double d = (Double)((SpinnerNumberModel)spinner.getModel()).getMinimum();
            return (float)d.floatValue();
        }

        static public void setDecimalPlaces( JSpinner spinner, int value )
        {
            int decimalPlaces = 0;
            if( value < 0 ){
                decimalPlaces = 0;
            }else{
                decimalPlaces = value;
            }
            String format = "0";
            if( decimalPlaces > 0 ){
                format += ".";
                for( int i = 0; i < decimalPlaces; i++ ){
                    format += "0";
                }
            }
            JSpinner.NumberEditor editor = new JSpinner.NumberEditor( spinner, format );
            double stepsize = Math.pow( 10, -decimalPlaces );
            ((SpinnerNumberModel)spinner.getModel()).setStepSize( stepsize );
            spinner.setEditor( editor );
        }

        /**
         * @todo テスト
         * @param spinner
         * @return
         */
        static public int getDecimalPlaces( JSpinner spinner )
        {
            double stepsize = (Double)((SpinnerNumberModel)spinner.getModel()).getStepSize();
            return (int)(-Math.log10( stepsize ));
        }
    }
}
