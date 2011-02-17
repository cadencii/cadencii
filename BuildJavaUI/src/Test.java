import java.awt.Dimension;
import java.awt.GridBagLayout;
import java.awt.Point;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BForm;
import javax.swing.JComboBox;
import java.awt.GridBagConstraints;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import javax.swing.JTextField;
import javax.swing.JToolBar;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;


public class Test extends BForm {
    /**
     * 
     */
    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private JTextField jTextField = null;
    private JMenuBar menuBar = null;
    private JMenu menuA = null;
    private JMenuItem menuA1 = null;
    private JMenuItem menuA2 = null;
    private JMenu menuB = null;
    private JMenuItem menuB1 = null;
    private JMenuItem menuB2 = null;
    /**
     * This method initializes jTextField	
     * 	
     * @return javax.swing.JTextField	
     */
    private JTextField getJTextField() {
        if (jTextField == null) {
            jTextField = new JTextField();
            jTextField.addActionListener( new ActionListener(){

                @Override
                public void actionPerformed(ActionEvent arg0) {
                    System.out.println( "actionPerformed" );
                    // TODO Auto-generated method stub
                    
                }
                
            });
        }
        return jTextField;
    }

    private static String normalizePath( String path )
    {
        String ret = path;
        path = path.replace( "\\", "\\\\\\\\" );
        return path.replace( " ", "\\ " );
    }

    /**
     * This method initializes jJMenuBar	
     * 	
     * @return javax.swing.JMenuBar	
     */
    private JMenuBar getJJMenuBar() {
        if (menuBar == null) {
            menuBar = new JMenuBar();
            menuBar.add(getJMenu());
            menuBar.add(getJMenu1());
        }
        return menuBar;
    }

    /**
     * This method initializes jMenu	
     * 	
     * @return javax.swing.JMenu	
     */
    private JMenu getJMenu() {
        if (menuA == null) {
            menuA = new JMenu();
            menuA.setText("A");
            menuA.add(getJMenuItem());
            menuA.add(getJMenuItem1());
        }
        return menuA;
    }

    /**
     * This method initializes jMenuItem	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem() {
        if (menuA1 == null) {
            menuA1 = new JMenuItem(){
                @Override
                public Point getLocationOnScreen()
                {
                    return new Point( 0, 0 );
                }
            };
            menuA1.setText("A1");
            menuA1.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    System.out.println("menuA1; ctionPerformed(); ");
                    try{
                        System.out.println( "    menuA1: " + menuA1.getLocationOnScreen() );
                        System.out.println( "    menuA2: " + menuA2.getLocationOnScreen() );
                        System.out.println( "    menuB1: " + menuB1.getLocationOnScreen() );
                        System.out.println( "    menuB2: " + menuB2.getLocationOnScreen() );
                    }catch( Exception ex ){
                    }
                }
            });
            menuA1.addMouseListener(new java.awt.event.MouseAdapter() {   
            	public void mousePressed(java.awt.event.MouseEvent e) {    
            		System.out.println("menuA1; mousePressed()");
                    try{
                        System.out.println( "    menuA1: " + menuA1.getLocationOnScreen() );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
                    try{
                        System.out.println( "    menuA2: " + menuA2.getLocationOnScreen() );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
                    try{
                        System.out.println( "    menuB1: " + menuB1.getLocationOnScreen() );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
                    try{
                        System.out.println( "    menuB2: " + menuB2.getLocationOnScreen() );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
            	}
                public void mouseEntered(java.awt.event.MouseEvent e) {
                    System.out.println("menuA1; mouseEntered()"); // TODO Auto-generated Event stub mouseEntered()
                }
            });
        }
        return menuA1;
    }

    /**
     * This method initializes jMenuItem1	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem1() {
        if (menuA2 == null) {
            menuA2 = new JMenuItem();
            menuA2.setText("A2");
            menuA2.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    System.out.println("menuA2; ctionPerformed(); ");
                    try{
                        System.out.println( "    menuA1: " + menuA1.getLocationOnScreen() );
                        System.out.println( "    menuA2: " + menuA2.getLocationOnScreen() );
                        System.out.println( "    menuB1: " + menuB1.getLocationOnScreen() );
                        System.out.println( "    menuB2: " + menuB2.getLocationOnScreen() );
                    }catch( Exception ex ){
                    }
                }
            });
        }
        return menuA2;
    }

    /**
     * This method initializes jMenu1	
     * 	
     * @return javax.swing.JMenu	
     */
    private JMenu getJMenu1() {
        if (menuB == null) {
            menuB = new JMenu();
            menuB.setText("B");
            menuB.add(getJMenuItem2());
            menuB.add(getJMenuItem3());
        }
        return menuB;
    }

    /**
     * This method initializes jMenuItem2	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem2() {
        if (menuB1 == null) {
            menuB1 = new JMenuItem();
            menuB1.setText("B1");
            menuB1.addMouseListener(new java.awt.event.MouseAdapter() {   
            	public void mousePressed(java.awt.event.MouseEvent e) {    
                    System.out.println("menuB1; mousePressed()");
                    try{
                        System.out.println( "    menuA1: " + menuA1.getLocationOnScreen() );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
                    try{
                        System.out.println( "    menuA2: " + menuA2.getLocationOnScreen() );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
                    try{
                        System.out.println( "    menuB1: " + menuB1.getLocationOnScreen() );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
                    try{
                        System.out.println( "    menuB2: " + menuB2.getLocationOnScreen() );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
            	}
                public void mouseEntered(java.awt.event.MouseEvent e) {
                    System.out.println("menuB1; mouseEntered()");
                }
            });
            menuB1.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                }
            });
            menuB1.addComponentListener(new java.awt.event.ComponentAdapter() {
                public void componentShown(java.awt.event.ComponentEvent e) {
                    System.out.println("menuB1: componentShown()");
                }
            });
        }
        return menuB1;
    }

    /**
     * This method initializes jMenuItem3	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem3() {
        if (menuB2 == null) {
            menuB2 = new JMenuItem();
            menuB2.setText("B2");
            menuB2.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    System.out.println("menuB2; ctionPerformed(); ");
                    try{
                        System.out.println( "    menuA1: " + menuA1.getLocationOnScreen() );
                        System.out.println( "    menuA2: " + menuA2.getLocationOnScreen() );
                        System.out.println( "    menuB1: " + menuB1.getLocationOnScreen() );
                        System.out.println( "    menuB2: " + menuB2.getLocationOnScreen() );
                    }catch( Exception ex ){
                    }
                }
            });
        }
        return menuB2;
    }

    public static void main( String[] args ) throws Exception
    {
        String s = "C:\\Program Files\\Steinberg";
        System.out.println( s );
        System.out.println( normalizePath( s ) );
        Test t = new Test();
        t.setVisible( true );
    }
    
    /**
     * This method initializes 
     * 
     */
    public Test() {
    	super();
    	initialize();
    }
     
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(315, 240));
        this.setJMenuBar(getJJMenuBar());
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints.gridy = 0;
            gridBagConstraints.weightx = 1.0;
            gridBagConstraints.gridx = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getJTextField(), gridBagConstraints);
        }
        return jPanel;
    }

}  //  @jve:decl-index=0:visual-constraint="63,28"
