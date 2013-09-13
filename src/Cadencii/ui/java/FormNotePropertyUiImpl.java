package com.github.cadencii.ui;

import com.github.cadencii.FormNotePropertyUi;
import com.github.cadencii.FormNotePropertyUiListener;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import javax.swing.BorderFactory;
import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.KeyStroke;

import java.awt.event.WindowEvent;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowStateListener;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;

public class FormNotePropertyUiImpl extends JFrame implements FormNotePropertyUi
{
    private static final long serialVersionUID = 1L;
    private JPanel panelMain = null;
    private JMenuBar menuStrip = null;
    private JMenu menuWindow = null;
    private JMenuItem menuClose = null;
    private JScrollPane jScrollPane = null;
    private boolean isWindowMinimized = false;
    private FormNotePropertyUiListener listener;

    /**
     * This method initializes
     *
     */
    public FormNotePropertyUiImpl( FormNotePropertyUiListener l )
    {
        super();
        addComponentListener(new ComponentAdapter() {
            @Override
            public void componentMoved(ComponentEvent arg0) {
                listener.locationOrSizeChanged();
            }
            @Override
            public void componentResized(ComponentEvent e) {
                listener.locationOrSizeChanged();
            }
        });
        this.listener = l;
        addWindowStateListener(new WindowStateListener() {
            public void windowStateChanged(WindowEvent arg0) {
                listener.windowStateChanged();
            }
        });
        addWindowListener(new WindowAdapter() {
            @Override
            public void windowDeiconified(WindowEvent arg0) {
                isWindowMinimized = false;
            }
            @Override
            public void windowIconified(WindowEvent e) {
                isWindowMinimized = true;
            }
            @Override
            public void windowOpened(WindowEvent e) {
                listener.onLoad();
            }
            @Override
            public void windowClosing(WindowEvent arg0) {
                listener.formClosing();
            }
        });
        initialize();
    }

    /**
     * This method initializes this
     *
     */
    private void initialize() {
        this.setSize(new Dimension(217, 330));
        this.setJMenuBar(getMenuStrip());
        this.setContentPane(getPanelMain());
        this.setTitle("Note Property");
    }

    /**
     * This method initializes jPanel
     *
     * @return javax.swing.JPanel
     */
    private JPanel getPanelMain() {
        if (panelMain == null) {
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.BOTH;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 1.0;
            gridBagConstraints1.weighty = 1.0;
            gridBagConstraints1.gridx = 0;
            panelMain = new JPanel();
            panelMain.setLayout(new GridBagLayout());
            panelMain.add(getJScrollPane(), gridBagConstraints1);
        }
        return panelMain;
    }

    /**
     * This method initializes menuStrip
     *
     * @return javax.swing.JMenuBar
     */
    private JMenuBar getMenuStrip() {
        if (menuStrip == null) {
            menuStrip = new JMenuBar();
            menuStrip.setVisible(true);
            menuStrip.add(getMenuWindow());
        }
        return menuStrip;
    }

    /**
     * This method initializes menuWindow
     *
     * @return javax.swing.JMenu
     */
    private JMenu getMenuWindow() {
        if (menuWindow == null) {
            menuWindow = new JMenu();
            menuWindow.setText("Window");
            menuWindow.add(getMenuClose());
        }
        return menuWindow;
    }

    /**
     * This method initializes menuClose
     *
     * @return javax.swing.JMenuItem
     */
    private JMenuItem getMenuClose() {
        if (menuClose == null) {
            menuClose = new JMenuItem();
            menuClose.addActionListener(new ActionListener() {
                public void actionPerformed(ActionEvent arg0) {
                    listener.menuCloseClick();
                }
            });
            menuClose.setText("Close");
        }
        return menuClose;
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
            jScrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
            jScrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
        }
        return jScrollPane;
    }

    @Override
    public void addComponent( Object c )
    {
        if( c == null ){
            return;
        }
        if( !(c instanceof Component) ){
            return;
        }
        getJScrollPane().setViewportView( (Component)c );
    }

    @Override
    public boolean isWindowMinimized()
    {
        return this.isWindowMinimized;
    }

    @Override
    public void deiconfyWindow()
    {
        this.setExtendedState( JFrame.NORMAL );
    }

    @Override
    public void close()
    {
        this.setVisible( false );
        this.dispose();
    }

    @Override
    public void setMenuCloseAccelerator( KeyStroke value )
    {
        this.menuClose.setAccelerator( value );
    }

    @Override
    public int getWorkingAreaX()
    {
        return this.getGraphicsConfiguration().getBounds().x;
    }

    @Override
    public int getWorkingAreaY()
    {
        return this.getGraphicsConfiguration().getBounds().y;
    }

    @Override
    public int getWorkingAreaWidth()
    {
        return this.getGraphicsConfiguration().getBounds().width;
    }

    @Override
    public int getWorkingAreaHeight()
    {
        return this.getGraphicsConfiguration().getBounds().height;
    }

    @Override
    public void hideWindow()
    {
        this.close();
    }
}
