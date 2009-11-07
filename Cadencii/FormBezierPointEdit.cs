/*
 * FormBezierPointEdit.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.Cadencii;

import java.util.*;
import java.awt.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.awt;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormBezierPointEdit extends BForm {
#else
    class FormBezierPointEdit : BForm {
#endif
        private BezierPoint m_point;
        private int m_min;
        private int m_max;
        private Point m_last_mouse_global_location;
        private TrackSelector m_parent;
        private boolean m_btn_datapoint_downed = false;
        private double m_min_opacity = 0.4;
        private CurveType m_curve_type;
        private int m_track;
        private int m_chain_id = -1;
        private int m_point_id = -1;
        private BezierPickedSide m_picked_side = BezierPickedSide.BASE;

        public FormBezierPointEdit( TrackSelector parent,
                                    CurveType curve_type,
                                    int selected_chain_id,
                                    int selected_point_id ) {
#if JAVA
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            m_parent = parent;
            m_curve_type = curve_type;
            m_track = AppManager.getSelected();
            m_chain_id = selected_chain_id;
            m_point_id = selected_point_id;
            boolean found = false;
            Vector<BezierChain> chains = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).get( m_curve_type );
            for ( int i = 0; i < chains.size(); i++ ) {
                if ( chains.get( i ).id == m_chain_id ) {
                    found = true;
                    break;
                }
            }
            if ( !found ) {
                return;
            }
            boolean smooth = false;
            for ( Iterator itr = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id ).points.iterator(); itr.hasNext(); ) {
                BezierPoint bp = (BezierPoint)itr.next();
                if ( bp.getID() == m_point_id ) {
                    m_point = bp;
                    smooth = bp.getControlLeftType() != BezierControlType.None || bp.getControlRightType() != BezierControlType.None;
                    break;
                }
            }
            UpdateStatus();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        private void UpdateStatus() {
            txtDataPointClock.Text = m_point.getBase().getX().ToString();
            txtDataPointValue.Text = m_point.getBase().getY().ToString();
            txtLeftClock.Text = ((int)(m_point.getBase().getX() + m_point.controlLeft.getX())).ToString();
            txtLeftValue.Text = ((int)(m_point.getBase().getY() + m_point.controlLeft.getY())).ToString();
            txtRightClock.Text = ((int)(m_point.getBase().getX() + m_point.controlRight.getX())).ToString();
            txtRightValue.Text = ((int)(m_point.getBase().getY() + m_point.controlRight.getY())).ToString();
            boolean smooth = m_point.getControlLeftType() != BezierControlType.None || m_point.getControlRightType() != BezierControlType.None;
            chkEnableSmooth.Checked = smooth;
            btnLeft.Enabled = smooth;
            btnRight.Enabled = smooth;
            m_min = m_curve_type.getMinimum();
            m_max = m_curve_type.getMaximum();
        }

        private static String _( String message ) {
            return Messaging.getMessage( message );
        }

        public void ApplyLanguage() {
            Text = _( "Edit Bezier Data Point" );

            groupDataPoint.Text = _( "Data Poin" );
            lblDataPointClock.Text = _( "Clock" );
            lblDataPointValue.Text = _( "Value" );

            groupLeft.Text = _( "Left Control Point" );
            lblLeftClock.Text = _( "Clock" );
            lblLeftValue.Text = _( "Value" );

            groupRight.Text = _( "Right Control Point" );
            lblRightClock.Text = _( "Clock" );
            lblRightValue.Text = _( "Value" );

            chkEnableSmooth.Text = _( "Smooth" );
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            try {
                int x, y;
                x = PortUtil.parseInt( txtDataPointClock.Text );
                y = PortUtil.parseInt( txtDataPointValue.Text );
                if ( y < m_min || m_max < y ) {
                    AppManager.showMessageBox( _( "Invalid value" ), _( "Error" ), AppManager.MSGBOX_DEFAULT_OPTION, AppManager.MSGBOX_ERROR_MESSAGE );
                    return;
                }
                if ( chkEnableSmooth.Checked ) {
                    x = PortUtil.parseInt( txtLeftClock.Text );
                    y = PortUtil.parseInt( txtLeftValue.Text );
                    x = PortUtil.parseInt( txtRightClock.Text );
                    y = PortUtil.parseInt( txtRightValue.Text );
                }
                setDialogResult( BDialogResult.OK );
            } catch ( Exception ex ) {
                AppManager.showMessageBox( _( "Integer format error" ), _( "Error" ), AppManager.MSGBOX_DEFAULT_OPTION, AppManager.MSGBOX_ERROR_MESSAGE );
            }
        }

        private void chkEnableSmooth_CheckedChanged( Object sender, BEventArgs e ) {
            boolean value = chkEnableSmooth.Checked;
            txtLeftClock.Enabled = value;
            txtLeftValue.Enabled = value;
            btnLeft.Enabled = value;
            txtRightClock.Enabled = value;
            txtRightValue.Enabled = value;
            btnRight.Enabled = value;

            boolean old = m_point.getControlLeftType() != BezierControlType.None || m_point.getControlRightType() != BezierControlType.None;
            if ( value ) {
                m_point.setControlLeftType( BezierControlType.Normal );
                m_point.setControlRightType( BezierControlType.Normal );
            } else {
                m_point.setControlLeftType( BezierControlType.None );
                m_point.setControlRightType( BezierControlType.None );
            }
            txtLeftClock.Text = ((int)(m_point.getBase().getX() + m_point.controlLeft.getX())).ToString();
            txtLeftValue.Text = ((int)(m_point.getBase().getY() + m_point.controlLeft.getY())).ToString();
            txtRightClock.Text = ((int)(m_point.getBase().getX() + m_point.controlRight.getX())).ToString();
            txtRightValue.Text = ((int)(m_point.getBase().getY() + m_point.controlRight.getY())).ToString();
            m_parent.Invalidate();
        }

        private void btnDataPoint_MouseDown( Object sender, BMouseEventArgs e ) {
            this.Opacity = m_min_opacity;
            m_last_mouse_global_location = PortUtil.getMousePosition();
            System.Drawing.Point loc_on_trackselector = new System.Drawing.Point( AppManager.xCoordFromClocks( (int)m_point.getBase().getX() ),
                                                    m_parent.yCoordFromValue( (int)m_point.getBase().getY() ) );
            System.Drawing.Point loc_on_screen = m_parent.PointToScreen( loc_on_trackselector );
            Cursor.Position = loc_on_screen;
            BMouseEventArgs event_arg = new BMouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.BASE;
            m_btn_datapoint_downed = true;
        }

        private void btnLeft_MouseDown( Object sender, BMouseEventArgs e ) {
            this.Opacity = m_min_opacity;
            m_last_mouse_global_location = PortUtil.getMousePosition();
            System.Drawing.Point loc_on_trackselector = new System.Drawing.Point( AppManager.xCoordFromClocks( (int)m_point.getControlLeft().getX() ),
                                                    m_parent.yCoordFromValue( (int)m_point.getControlLeft().getY() ) );
            System.Drawing.Point loc_on_screen = m_parent.PointToScreen( loc_on_trackselector );
            Cursor.Position = loc_on_screen;
            BMouseEventArgs event_arg = new BMouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.LEFT;
            m_btn_datapoint_downed = true;
        }

        private void btnRight_MouseDown( Object sender, BMouseEventArgs e ) {
            this.Opacity = m_min_opacity;
            m_last_mouse_global_location = PortUtil.getMousePosition();
            System.Drawing.Point loc_on_trackselector = new System.Drawing.Point( AppManager.xCoordFromClocks( (int)m_point.getControlRight().getX() ),
                                                    m_parent.yCoordFromValue( (int)m_point.getControlRight().getY() ) );
            System.Drawing.Point loc_on_screen = m_parent.PointToScreen( loc_on_trackselector );
            Cursor.Position = loc_on_screen;
            BMouseEventArgs event_arg = new BMouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.RIGHT;
            m_btn_datapoint_downed = true;
        }

        private void common_MouseUp( Object sender, BMouseEventArgs e ) {
            m_btn_datapoint_downed = false;
            this.Opacity = 1.0;
            Point loc_on_screen = PortUtil.getMousePosition();
            System.Drawing.Point loc_on_trackselector = m_parent.PointToClient( new System.Drawing.Point( loc_on_screen.x, loc_on_screen.y ) );
            BMouseEventArgs event_arg = new BMouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
            m_parent.TrackSelector_MouseUp( this, event_arg );
            Cursor.Position = new System.Drawing.Point( m_last_mouse_global_location.x, m_last_mouse_global_location.y );
            m_parent.Invalidate();
        }

        private void common_MouseMove( Object sender, BMouseEventArgs e ) {
            if ( m_btn_datapoint_downed ) {
                Point loc_on_screen = PortUtil.getMousePosition();
                System.Drawing.Point loc_on_trackselector = m_parent.PointToClient( new System.Drawing.Point( loc_on_screen.x, loc_on_screen.y ) );
                BMouseEventArgs event_arg = new BMouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
                BezierPoint ret = m_parent.HandleMouseMoveForBezierMove( event_arg, m_picked_side );

                txtDataPointClock.Text = ((int)ret.getBase().getX()).ToString();
                txtDataPointValue.Text = ((int)ret.getBase().getY()).ToString();
                txtLeftClock.Text = ((int)ret.getControlLeft().getX()).ToString();
                txtLeftValue.Text = ((int)ret.getControlLeft().getY()).ToString();
                txtRightClock.Text = ((int)ret.getControlRight().getX()).ToString();
                txtRightValue.Text = ((int)ret.getControlRight().getY()).ToString();

                m_parent.Invalidate();
            }
        }

        private void btnBackward_Click( Object sender, BEventArgs e ) {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id );
            int index = -1;
            for ( int i = 0; i < target.size(); i++ ) {
                if ( target.points.get( i ).getID() == m_point_id ) {
                    index = i - 1;
                    break;
                }
            }
            if ( 0 <= index ) {
                m_point_id = target.points.get( index ).getID();
                m_point = target.points.get( index );
                UpdateStatus();
                m_parent.EditingPointID = m_point_id;
                m_parent.Invalidate();
            }
        }

        private void btnForward_Click( Object sender, BEventArgs e ) {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id );
            int index = -2;
            for ( int i = 0; i < target.size(); i++ ) {
                if ( target.points.get( i ).getID() == m_point_id ) {
                    index = i + 1;
                    break;
                }
            }
            if ( 0 <= index && index < target.size() ) {
                m_point_id = target.points.get( index ).getID();
                m_point = target.points.get( index );
                UpdateStatus();
                m_parent.EditingPointID = m_point_id;
                m_parent.Invalidate();
            }
        }

        private void registerEventHandlers() {
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            this.chkEnableSmooth.CheckedChanged += new System.EventHandler( this.chkEnableSmooth_CheckedChanged );
            this.btnLeft.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseDown );
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnDataPoint.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnDataPoint.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnDataPoint_MouseDown );
            this.btnDataPoint.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnRight.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseDown );
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            this.btnBackward.Click += new System.EventHandler( this.btnBackward_Click );
            this.btnForward.Click += new System.EventHandler( this.btnForward_Click );
        }

        private void setResources() {
            this.btnLeft.Image = Resources.get_target__pencil();
            this.btnDataPoint.Image = Resources.get_target__pencil();
            this.btnRight.Image = Resources.get_target__pencil();
        }

#if JAVA
        #region UI Impl for Java
	    private JPanel jContentPane = null;
	    private JButton btnBackward = null;
	    private JCheckBox chkEnableSmooth = null;
	    private JButton btnForward = null;
	    private JPanel groupLeft = null;
	    private JLabel lblLeftClock = null;
	    private JTextField jTextField = null;
	    private JLabel lblLeftValue = null;
	    private JTextField jTextField1 = null;
	    private JButton btnLeft = null;
	    private JPanel groupDataPoint = null;
	    private JLabel lblDataPointClock = null;
	    private JTextField jTextField2 = null;
	    private JLabel lblDataPointValue = null;
	    private JTextField jTextField11 = null;
	    private JButton btnDataPoint = null;
	    private JPanel groupRight = null;
	    private JLabel lblRightClock = null;
	    private JTextField jTextField3 = null;
	    private JLabel lblRightValue = null;
	    private JTextField jTextField12 = null;
	    private JButton btnRight = null;
	    private JButton btnOk = null;
	    private JButton btnCancel = null;
	    private JLabel jLabel4 = null;
	    private JLabel jLabel5 = null;
	    private JPanel jPanel3 = null;
	    private JLabel jLabel6 = null;
	    private JLabel jLabel7 = null;
	    private JLabel jLabel8 = null;
	    private JLabel jLabel10 = null;
	    private JLabel jLabel9 = null;
	    private JLabel jLabel13 = null;
	    private JLabel jLabel14 = null;
	    private JLabel jLabel15 = null;

	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(469, 266);
		    this.setContentPane(getJContentPane());
		    this.setTitle("JFrame");
	    }

	    /**
	     * This method initializes jContentPane
	     * 
	     * @return javax.swing.JPanel
	     */
	    private JPanel getJContentPane() {
		    if (jContentPane == null) {
			    GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
			    gridBagConstraints91.gridx = 0;
			    gridBagConstraints91.gridwidth = 3;
			    gridBagConstraints91.anchor = GridBagConstraints.EAST;
			    gridBagConstraints91.gridy = 4;
			    GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			    gridBagConstraints81.gridx = 0;
			    gridBagConstraints81.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints81.gridwidth = 3;
			    gridBagConstraints81.gridy = 3;
			    jLabel5 = new JLabel();
			    jLabel5.setText("    ");
			    GridBagConstraints gridBagConstraints73 = new GridBagConstraints();
			    gridBagConstraints73.gridx = 0;
			    gridBagConstraints73.gridwidth = 3;
			    gridBagConstraints73.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints73.gridy = 1;
			    jLabel4 = new JLabel();
			    jLabel4.setText("     ");
			    GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			    gridBagConstraints13.gridx = 2;
			    gridBagConstraints13.gridy = 1;
			    GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			    gridBagConstraints10.gridx = 2;
			    gridBagConstraints10.weightx = 1.0D;
			    gridBagConstraints10.fill = GridBagConstraints.BOTH;
			    gridBagConstraints10.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints10.gridy = 2;
			    GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			    gridBagConstraints9.gridx = 1;
			    gridBagConstraints9.weightx = 1.0D;
			    gridBagConstraints9.fill = GridBagConstraints.BOTH;
			    gridBagConstraints9.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints9.gridy = 2;
			    GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			    gridBagConstraints8.gridx = 0;
			    gridBagConstraints8.weightx = 1.0D;
			    gridBagConstraints8.fill = GridBagConstraints.BOTH;
			    gridBagConstraints8.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints8.gridy = 2;
			    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			    gridBagConstraints2.gridx = 2;
			    gridBagConstraints2.anchor = GridBagConstraints.WEST;
			    gridBagConstraints2.gridy = 0;
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.gridx = 1;
			    gridBagConstraints1.gridy = 0;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.gridx = 0;
			    gridBagConstraints.anchor = GridBagConstraints.EAST;
			    gridBagConstraints.gridy = 0;
			    jContentPane = new JPanel();
			    jContentPane.setLayout(new GridBagLayout());
			    jContentPane.add(getBtnBackward(), gridBagConstraints);
			    jContentPane.add(getChkEnableSmooth(), gridBagConstraints1);
			    jContentPane.add(getBtnForward(), gridBagConstraints2);
			    jContentPane.add(getGroupLeft(), gridBagConstraints8);
			    jContentPane.add(getGroupDataPoint(), gridBagConstraints9);
			    jContentPane.add(getGroupRight(), gridBagConstraints10);
			    jContentPane.add(jLabel4, gridBagConstraints73);
			    jContentPane.add(jLabel5, gridBagConstraints81);
			    jContentPane.add(getJPanel3(), gridBagConstraints91);
			    jContentPane.add(jLabel4, gridBagConstraints13);
		    }
		    return jContentPane;
	    }

	    /**
	     * This method initializes btnBackward	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnBackward() {
		    if (btnBackward == null) {
			    btnBackward = new JButton();
			    btnBackward.setText("<<");
		    }
		    return btnBackward;
	    }

	    /**
	     * This method initializes chkEnableSmooth	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkEnableSmooth() {
		    if (chkEnableSmooth == null) {
			    chkEnableSmooth = new JCheckBox();
			    chkEnableSmooth.setText("Smooth");
		    }
		    return chkEnableSmooth;
	    }

	    /**
	     * This method initializes btnForward	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnForward() {
		    if (btnForward == null) {
			    btnForward = new JButton();
			    btnForward.setText(">>");
		    }
		    return btnForward;
	    }

	    /**
	     * This method initializes groupLeft	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupLeft() {
		    if (groupLeft == null) {
			    GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
			    gridBagConstraints18.gridx = 4;
			    gridBagConstraints18.gridy = 0;
			    jLabel10 = new JLabel();
			    jLabel10.setText("     ");
			    GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			    gridBagConstraints16.gridx = 0;
			    gridBagConstraints16.gridy = 0;
			    jLabel8 = new JLabel();
			    jLabel8.setText("     ");
			    GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			    gridBagConstraints7.gridx = 1;
			    gridBagConstraints7.gridwidth = 3;
			    gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints7.weightx = 1.0D;
			    gridBagConstraints7.ipadx = 0;
			    gridBagConstraints7.ipady = 0;
			    gridBagConstraints7.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints7.gridy = 3;
			    GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			    gridBagConstraints6.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints6.gridy = 1;
			    gridBagConstraints6.weightx = 1.0;
			    gridBagConstraints6.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints6.gridx = 3;
			    GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			    gridBagConstraints5.gridx = 1;
			    gridBagConstraints5.gridy = 1;
			    lblLeftValue = new JLabel();
			    lblLeftValue.setText("Value");
			    GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			    gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints4.gridy = 0;
			    gridBagConstraints4.weightx = 1.0D;
			    gridBagConstraints4.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints4.gridx = 3;
			    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			    gridBagConstraints3.gridx = 1;
			    gridBagConstraints3.gridy = 0;
			    lblLeftClock = new JLabel();
			    lblLeftClock.setText("Clock");
			    groupLeft = new JPanel();
			    groupLeft.setLayout(new GridBagLayout());
			    groupLeft.setBorder(BorderFactory.createTitledBorder(null, "Left Control Point", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupLeft.add(lblLeftClock, gridBagConstraints3);
			    groupLeft.add(getJTextField(), gridBagConstraints4);
			    groupLeft.add(lblLeftValue, gridBagConstraints5);
			    groupLeft.add(getJTextField1(), gridBagConstraints6);
			    groupLeft.add(getBtnLeft(), gridBagConstraints7);
			    groupLeft.add(jLabel8, gridBagConstraints16);
			    groupLeft.add(jLabel10, gridBagConstraints18);
		    }
		    return groupLeft;
	    }

	    /**
	     * This method initializes jTextField	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getJTextField() {
		    if (jTextField == null) {
			    jTextField = new JTextField();
		    }
		    return jTextField;
	    }

	    /**
	     * This method initializes jTextField1	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getJTextField1() {
		    if (jTextField1 == null) {
			    jTextField1 = new JTextField();
		    }
		    return jTextField1;
	    }

	    /**
	     * This method initializes btnLeft	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnLeft() {
		    if (btnLeft == null) {
			    btnLeft = new JButton();
			    btnLeft.setText("");
			    btnLeft.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		    }
		    return btnLeft;
	    }

	    /**
	     * This method initializes groupDataPoint	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupDataPoint() {
		    if (groupDataPoint == null) {
			    GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			    gridBagConstraints19.gridx = 4;
			    gridBagConstraints19.gridy = 0;
			    jLabel13 = new JLabel();
			    jLabel13.setText("     ");
			    GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
			    gridBagConstraints17.gridx = 0;
			    gridBagConstraints17.gridy = 0;
			    jLabel9 = new JLabel();
			    jLabel9.setText("     ");
			    GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
			    gridBagConstraints71.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints71.gridx = 1;
			    gridBagConstraints71.gridy = 2;
			    gridBagConstraints71.weightx = 1.0D;
			    gridBagConstraints71.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints71.gridwidth = 3;
			    GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			    gridBagConstraints61.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints61.gridy = 1;
			    gridBagConstraints61.weightx = 1.0;
			    gridBagConstraints61.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints61.gridx = 3;
			    GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			    gridBagConstraints51.gridx = 1;
			    gridBagConstraints51.gridy = 1;
			    lblDataPointValue = new JLabel();
			    lblDataPointValue.setText("Value");
			    GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
			    gridBagConstraints41.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints41.gridy = 0;
			    gridBagConstraints41.weightx = 1.0D;
			    gridBagConstraints41.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints41.gridx = 3;
			    GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			    gridBagConstraints31.gridx = 1;
			    gridBagConstraints31.gridy = 0;
			    lblDataPointClock = new JLabel();
			    lblDataPointClock.setText("Clock");
			    groupDataPoint = new JPanel();
			    groupDataPoint.setLayout(new GridBagLayout());
			    groupDataPoint.setBorder(BorderFactory.createTitledBorder(null, "Data Point", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupDataPoint.add(lblDataPointClock, gridBagConstraints31);
			    groupDataPoint.add(getJTextField2(), gridBagConstraints41);
			    groupDataPoint.add(lblDataPointValue, gridBagConstraints51);
			    groupDataPoint.add(getJTextField11(), gridBagConstraints61);
			    groupDataPoint.add(getBtnDataPoint(), gridBagConstraints71);
			    groupDataPoint.add(jLabel9, gridBagConstraints17);
			    groupDataPoint.add(jLabel13, gridBagConstraints19);
		    }
		    return groupDataPoint;
	    }

	    /**
	     * This method initializes jTextField2	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getJTextField2() {
		    if (jTextField2 == null) {
			    jTextField2 = new JTextField();
		    }
		    return jTextField2;
	    }

	    /**
	     * This method initializes jTextField11	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getJTextField11() {
		    if (jTextField11 == null) {
			    jTextField11 = new JTextField();
		    }
		    return jTextField11;
	    }

	    /**
	     * This method initializes btnDataPoint	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnDataPoint() {
		    if (btnDataPoint == null) {
			    btnDataPoint = new JButton();
			    btnDataPoint.setText("");
			    btnDataPoint.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		    }
		    return btnDataPoint;
	    }

	    /**
	     * This method initializes groupRight	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupRight() {
		    if (groupRight == null) {
			    TitledBorder titledBorder = BorderFactory.createTitledBorder(null, "Left Control Point", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51));
			    titledBorder.setTitle("Right Control Point");
			    GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			    gridBagConstraints21.gridx = 4;
			    gridBagConstraints21.gridy = 0;
			    jLabel15 = new JLabel();
			    jLabel15.setText("     ");
			    GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
			    gridBagConstraints20.gridx = 0;
			    gridBagConstraints20.gridy = 0;
			    jLabel14 = new JLabel();
			    jLabel14.setText("     ");
			    GridBagConstraints gridBagConstraints72 = new GridBagConstraints();
			    gridBagConstraints72.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints72.gridx = 1;
			    gridBagConstraints72.gridy = 2;
			    gridBagConstraints72.weightx = 1.0D;
			    gridBagConstraints72.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints72.gridwidth = 3;
			    GridBagConstraints gridBagConstraints62 = new GridBagConstraints();
			    gridBagConstraints62.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints62.gridy = 1;
			    gridBagConstraints62.weightx = 1.0;
			    gridBagConstraints62.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints62.gridx = 3;
			    GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			    gridBagConstraints52.gridx = 1;
			    gridBagConstraints52.gridy = 1;
			    lblRightValue = new JLabel();
			    lblRightValue.setText("Value");
			    GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			    gridBagConstraints42.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints42.gridy = 0;
			    gridBagConstraints42.weightx = 1.0D;
			    gridBagConstraints42.insets = new Insets(5, 5, 5, 5);
			    gridBagConstraints42.gridx = 3;
			    GridBagConstraints gridBagConstraints32 = new GridBagConstraints();
			    gridBagConstraints32.gridx = 1;
			    gridBagConstraints32.gridy = 0;
			    lblRightClock = new JLabel();
			    lblRightClock.setText("Clock");
			    groupRight = new JPanel();
			    groupRight.setLayout(new GridBagLayout());
			    groupRight.setBorder(titledBorder);
			    groupRight.add(lblRightClock, gridBagConstraints32);
			    groupRight.add(getJTextField3(), gridBagConstraints42);
			    groupRight.add(lblRightValue, gridBagConstraints52);
			    groupRight.add(getJTextField12(), gridBagConstraints62);
			    groupRight.add(getBtnRight(), gridBagConstraints72);
			    groupRight.add(jLabel14, gridBagConstraints20);
			    groupRight.add(jLabel15, gridBagConstraints21);
		    }
		    return groupRight;
	    }

	    /**
	     * This method initializes jTextField3	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getJTextField3() {
		    if (jTextField3 == null) {
			    jTextField3 = new JTextField();
		    }
		    return jTextField3;
	    }

	    /**
	     * This method initializes jTextField12	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getJTextField12() {
		    if (jTextField12 == null) {
			    jTextField12 = new JTextField();
		    }
		    return jTextField12;
	    }

	    /**
	     * This method initializes btnRight	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnRight() {
		    if (btnRight == null) {
			    btnRight = new JButton();
			    btnRight.setText("");
			    btnRight.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		    }
		    return btnRight;
	    }

	    /**
	     * This method initializes btnOk	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnOk() {
		    if (btnOk == null) {
			    btnOk = new JButton();
			    btnOk.setText("OK");
		    }
		    return btnOk;
	    }

	    /**
	     * This method initializes btnCancel	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnCancel() {
		    if (btnCancel == null) {
			    btnCancel = new JButton();
			    btnCancel.setText("Cancel");
		    }
		    return btnCancel;
	    }

	    /**
	     * This method initializes jPanel3	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel3() {
		    if (jPanel3 == null) {
			    GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			    gridBagConstraints15.gridx = 1;
			    gridBagConstraints15.gridy = 0;
			    jLabel7 = new JLabel();
			    jLabel7.setText("     ");
			    GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			    gridBagConstraints14.gridx = 3;
			    gridBagConstraints14.gridy = 0;
			    jLabel6 = new JLabel();
			    jLabel6.setText("     ");
			    GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			    gridBagConstraints11.gridx = 0;
			    gridBagConstraints11.gridy = 0;
			    GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			    gridBagConstraints12.gridx = 2;
			    gridBagConstraints12.gridy = 0;
			    jPanel3 = new JPanel();
			    jPanel3.setLayout(new GridBagLayout());
			    jPanel3.add(getBtnCancel(), gridBagConstraints12);
			    jPanel3.add(getBtnOk(), gridBagConstraints11);
			    jPanel3.add(jLabel6, gridBagConstraints14);
			    jPanel3.add(jLabel7, gridBagConstraints15);
		    }
		    return jPanel3;
	    }
        #endregion
#else
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkEnableSmooth = new System.Windows.Forms.CheckBox();
            this.lblLeftValue = new System.Windows.Forms.Label();
            this.lblLeftClock = new System.Windows.Forms.Label();
            this.groupLeft = new System.Windows.Forms.GroupBox();
            this.btnLeft = new System.Windows.Forms.Button();
            this.groupDataPoint = new System.Windows.Forms.GroupBox();
            this.btnDataPoint = new System.Windows.Forms.Button();
            this.lblDataPointValue = new System.Windows.Forms.Label();
            this.lblDataPointClock = new System.Windows.Forms.Label();
            this.groupRight = new System.Windows.Forms.GroupBox();
            this.btnRight = new System.Windows.Forms.Button();
            this.lblRightValue = new System.Windows.Forms.Label();
            this.lblRightClock = new System.Windows.Forms.Label();
            this.btnBackward = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.txtRightClock = new Boare.Cadencii.NumberTextBox();
            this.txtRightValue = new Boare.Cadencii.NumberTextBox();
            this.txtDataPointClock = new Boare.Cadencii.NumberTextBox();
            this.txtDataPointValue = new Boare.Cadencii.NumberTextBox();
            this.txtLeftClock = new Boare.Cadencii.NumberTextBox();
            this.txtLeftValue = new Boare.Cadencii.NumberTextBox();
            this.groupLeft.SuspendLayout();
            this.groupDataPoint.SuspendLayout();
            this.groupRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 374, 163 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 293, 163 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // chkEnableSmooth
            // 
            this.chkEnableSmooth.AutoSize = true;
            this.chkEnableSmooth.Location = new System.Drawing.Point( 196, 12 );
            this.chkEnableSmooth.Name = "chkEnableSmooth";
            this.chkEnableSmooth.Size = new System.Drawing.Size( 62, 16 );
            this.chkEnableSmooth.TabIndex = 3;
            this.chkEnableSmooth.Text = "Smooth";
            this.chkEnableSmooth.UseVisualStyleBackColor = true;
            // 
            // lblLeftValue
            // 
            this.lblLeftValue.AutoSize = true;
            this.lblLeftValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblLeftValue.Name = "lblLeftValue";
            this.lblLeftValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblLeftValue.TabIndex = 16;
            this.lblLeftValue.Text = "Value";
            // 
            // lblLeftClock
            // 
            this.lblLeftClock.AutoSize = true;
            this.lblLeftClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblLeftClock.Name = "lblLeftClock";
            this.lblLeftClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblLeftClock.TabIndex = 15;
            this.lblLeftClock.Text = "Clock";
            // 
            // groupLeft
            // 
            this.groupLeft.Controls.Add( this.btnLeft );
            this.groupLeft.Controls.Add( this.lblLeftValue );
            this.groupLeft.Controls.Add( this.txtLeftClock );
            this.groupLeft.Controls.Add( this.lblLeftClock );
            this.groupLeft.Controls.Add( this.txtLeftValue );
            this.groupLeft.Location = new System.Drawing.Point( 14, 38 );
            this.groupLeft.Name = "groupLeft";
            this.groupLeft.Size = new System.Drawing.Size( 141, 113 );
            this.groupLeft.TabIndex = 17;
            this.groupLeft.TabStop = false;
            this.groupLeft.Text = "Left Control Point";
            // 
            // btnLeft
            // 
            this.btnLeft.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLeft.Location = new System.Drawing.Point( 18, 76 );
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size( 109, 27 );
            this.btnLeft.TabIndex = 18;
            this.btnLeft.UseVisualStyleBackColor = true;
            // 
            // groupDataPoint
            // 
            this.groupDataPoint.Controls.Add( this.btnDataPoint );
            this.groupDataPoint.Controls.Add( this.lblDataPointValue );
            this.groupDataPoint.Controls.Add( this.txtDataPointClock );
            this.groupDataPoint.Controls.Add( this.lblDataPointClock );
            this.groupDataPoint.Controls.Add( this.txtDataPointValue );
            this.groupDataPoint.Location = new System.Drawing.Point( 161, 38 );
            this.groupDataPoint.Name = "groupDataPoint";
            this.groupDataPoint.Size = new System.Drawing.Size( 141, 113 );
            this.groupDataPoint.TabIndex = 18;
            this.groupDataPoint.TabStop = false;
            this.groupDataPoint.Text = "Data Point";
            // 
            // btnDataPoint
            // 
            this.btnDataPoint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDataPoint.Location = new System.Drawing.Point( 18, 76 );
            this.btnDataPoint.Name = "btnDataPoint";
            this.btnDataPoint.Size = new System.Drawing.Size( 109, 27 );
            this.btnDataPoint.TabIndex = 17;
            this.btnDataPoint.UseVisualStyleBackColor = true;
            // 
            // lblDataPointValue
            // 
            this.lblDataPointValue.AutoSize = true;
            this.lblDataPointValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblDataPointValue.Name = "lblDataPointValue";
            this.lblDataPointValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointValue.TabIndex = 16;
            this.lblDataPointValue.Text = "Value";
            // 
            // lblDataPointClock
            // 
            this.lblDataPointClock.AutoSize = true;
            this.lblDataPointClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblDataPointClock.Name = "lblDataPointClock";
            this.lblDataPointClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointClock.TabIndex = 15;
            this.lblDataPointClock.Text = "Clock";
            // 
            // groupRight
            // 
            this.groupRight.Controls.Add( this.btnRight );
            this.groupRight.Controls.Add( this.lblRightValue );
            this.groupRight.Controls.Add( this.txtRightClock );
            this.groupRight.Controls.Add( this.lblRightClock );
            this.groupRight.Controls.Add( this.txtRightValue );
            this.groupRight.Location = new System.Drawing.Point( 308, 38 );
            this.groupRight.Name = "groupRight";
            this.groupRight.Size = new System.Drawing.Size( 141, 113 );
            this.groupRight.TabIndex = 19;
            this.groupRight.TabStop = false;
            this.groupRight.Text = "Right Control Point";
            // 
            // btnRight
            // 
            this.btnRight.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRight.Location = new System.Drawing.Point( 18, 76 );
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size( 109, 27 );
            this.btnRight.TabIndex = 18;
            this.btnRight.UseVisualStyleBackColor = true;
            // 
            // lblRightValue
            // 
            this.lblRightValue.AutoSize = true;
            this.lblRightValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblRightValue.Name = "lblRightValue";
            this.lblRightValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblRightValue.TabIndex = 16;
            this.lblRightValue.Text = "Value";
            // 
            // lblRightClock
            // 
            this.lblRightClock.AutoSize = true;
            this.lblRightClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblRightClock.Name = "lblRightClock";
            this.lblRightClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblRightClock.TabIndex = 15;
            this.lblRightClock.Text = "Clock";
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point( 99, 8 );
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size( 75, 23 );
            this.btnBackward.TabIndex = 20;
            this.btnBackward.Text = "<<";
            this.btnBackward.UseVisualStyleBackColor = true;
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point( 290, 9 );
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size( 75, 23 );
            this.btnForward.TabIndex = 21;
            this.btnForward.Text = ">>";
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // txtRightClock
            // 
            this.txtRightClock.Enabled = false;
            this.txtRightClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtRightClock.Name = "txtRightClock";
            this.txtRightClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtRightClock.TabIndex = 6;
            this.txtRightClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtRightValue
            // 
            this.txtRightValue.Enabled = false;
            this.txtRightValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtRightValue.Name = "txtRightValue";
            this.txtRightValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtRightValue.TabIndex = 7;
            this.txtRightValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDataPointClock
            // 
            this.txtDataPointClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtDataPointClock.Name = "txtDataPointClock";
            this.txtDataPointClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtDataPointClock.TabIndex = 1;
            this.txtDataPointClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDataPointValue
            // 
            this.txtDataPointValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtDataPointValue.Name = "txtDataPointValue";
            this.txtDataPointValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtDataPointValue.TabIndex = 2;
            this.txtDataPointValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtLeftClock
            // 
            this.txtLeftClock.BackColor = System.Drawing.SystemColors.Window;
            this.txtLeftClock.Enabled = false;
            this.txtLeftClock.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLeftClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtLeftClock.Name = "txtLeftClock";
            this.txtLeftClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtLeftClock.TabIndex = 4;
            this.txtLeftClock.Text = "0";
            this.txtLeftClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtLeftValue
            // 
            this.txtLeftValue.BackColor = System.Drawing.SystemColors.Window;
            this.txtLeftValue.Enabled = false;
            this.txtLeftValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLeftValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtLeftValue.Name = "txtLeftValue";
            this.txtLeftValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtLeftValue.TabIndex = 5;
            this.txtLeftValue.Text = "0";
            this.txtLeftValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // FormBezierPointEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 463, 201 );
            this.Controls.Add( this.btnForward );
            this.Controls.Add( this.btnBackward );
            this.Controls.Add( this.groupRight );
            this.Controls.Add( this.groupDataPoint );
            this.Controls.Add( this.groupLeft );
            this.Controls.Add( this.chkEnableSmooth );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBezierPointEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Bezier Data Point";
            this.groupLeft.ResumeLayout( false );
            this.groupLeft.PerformLayout();
            this.groupDataPoint.ResumeLayout( false );
            this.groupDataPoint.PerformLayout();
            this.groupRight.ResumeLayout( false );
            this.groupRight.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkEnableSmooth;
        private System.Windows.Forms.Label lblLeftValue;
        private System.Windows.Forms.Label lblLeftClock;
        private NumberTextBox txtLeftValue;
        private NumberTextBox txtLeftClock;
        private System.Windows.Forms.GroupBox groupLeft;
        private System.Windows.Forms.GroupBox groupDataPoint;
        private System.Windows.Forms.Label lblDataPointValue;
        private NumberTextBox txtDataPointClock;
        private System.Windows.Forms.Label lblDataPointClock;
        private NumberTextBox txtDataPointValue;
        private System.Windows.Forms.GroupBox groupRight;
        private System.Windows.Forms.Label lblRightValue;
        private NumberTextBox txtRightClock;
        private System.Windows.Forms.Label lblRightClock;
        private NumberTextBox txtRightValue;
        private System.Windows.Forms.Button btnDataPoint;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.Button btnForward;
        #endregion
#endif
    }

#if !JAVA
}
#endif
