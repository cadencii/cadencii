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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    partial class FormBezierPointEdit : Form {
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
            InitializeComponent();
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
            for ( Iterator itr = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id ).points.iterator(); itr.hasNext(); ){
                BezierPoint bp = (BezierPoint)itr.next();
                if ( bp.ID == m_point_id ) {
                    m_point = bp;
                    smooth = bp.getControlLeftType() != BezierControlType.None || bp.getControlRightType() != BezierControlType.None;
                    break;
                }
            }
            UpdateStatus();
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        private void UpdateStatus() {
            txtDataPointClock.Text = m_point.getBase().X.ToString();
            txtDataPointValue.Text = m_point.getBase().Y.ToString();
            txtLeftClock.Text = ((int)(m_point.getBase().X + m_point.controlLeft.X)).ToString();
            txtLeftValue.Text = ((int)(m_point.getBase().Y + m_point.controlLeft.Y)).ToString();
            txtRightClock.Text = ((int)(m_point.getBase().X + m_point.controlRight.X)).ToString();
            txtRightValue.Text = ((int)(m_point.getBase().Y + m_point.controlRight.Y)).ToString();
            boolean smooth = m_point.getControlLeftType() != BezierControlType.None || m_point.getControlRightType() != BezierControlType.None;
            chkEnableSmooth.Checked = smooth;
            btnLeft.Enabled = smooth;
            btnRight.Enabled = smooth;
            m_min = m_curve_type.Minimum;
            m_max = m_curve_type.Maximum;
        }

        private static String _( String message ) {
            return Messaging.GetMessage( message );
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
        
        private void btnOK_Click( object sender, EventArgs e ) {
            try {
                int x, y;
                x = int.Parse( txtDataPointClock.Text );
                y = int.Parse( txtDataPointValue.Text );
                if ( y < m_min || m_max < y ) {
                    MessageBox.Show( _( "Invalid value" ), _( "Error" ), MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
                if ( chkEnableSmooth.Checked ) {
                    x = int.Parse( txtLeftClock.Text );
                    y = int.Parse( txtLeftValue.Text );
                    x = int.Parse( txtRightClock.Text );
                    y = int.Parse( txtRightValue.Text );
                }
                DialogResult = DialogResult.OK;
            } catch {
                MessageBox.Show( _( "Integer format error" ), _( "Error" ), MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

        private void chkEnableSmooth_CheckedChanged( object sender, EventArgs e ) {
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
            txtLeftClock.Text = ((int)(m_point.getBase().X + m_point.controlLeft.X)).ToString();
            txtLeftValue.Text = ((int)(m_point.getBase().Y + m_point.controlLeft.Y)).ToString();
            txtRightClock.Text = ((int)(m_point.getBase().X + m_point.controlRight.X)).ToString();
            txtRightValue.Text = ((int)(m_point.getBase().Y + m_point.controlRight.Y)).ToString();
            m_parent.Invalidate();
        }

        private void btnDataPoint_MouseDown( object sender, MouseEventArgs e ) {
            this.Opacity = m_min_opacity;
            m_last_mouse_global_location = Control.MousePosition;
            Point loc_on_trackselector = new Point( m_parent.xCoordFromClocks( (int)m_point.getBase().X ), 
                                                    m_parent.yCoordFromValue( (int)m_point.getBase().Y ) );
            Point loc_on_screen = m_parent.PointToScreen( loc_on_trackselector );
            Cursor.Position = loc_on_screen;
            MouseEventArgs event_arg = new MouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.BASE;
            m_btn_datapoint_downed = true;
        }

        private void btnLeft_MouseDown( object sender, MouseEventArgs e ) {
            this.Opacity = m_min_opacity;
            m_last_mouse_global_location = Control.MousePosition;
            Point loc_on_trackselector = new Point( m_parent.xCoordFromClocks( (int)m_point.getControlLeft().X ), 
                                                    m_parent.yCoordFromValue( (int)m_point.getControlLeft().Y ) );
            Point loc_on_screen = m_parent.PointToScreen( loc_on_trackselector );
            Cursor.Position = loc_on_screen;
            MouseEventArgs event_arg = new MouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.LEFT;
            m_btn_datapoint_downed = true;
        }

        private void btnRight_MouseDown( object sender, MouseEventArgs e ) {
            this.Opacity = m_min_opacity;
            m_last_mouse_global_location = Control.MousePosition;
            Point loc_on_trackselector = new Point( m_parent.xCoordFromClocks( (int)m_point.getControlRight().X ), 
                                                    m_parent.yCoordFromValue( (int)m_point.getControlRight().Y ) );
            Point loc_on_screen = m_parent.PointToScreen( loc_on_trackselector );
            Cursor.Position = loc_on_screen;
            MouseEventArgs event_arg = new MouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
            m_parent.TrackSelector_MouseDown( this, event_arg );
            m_picked_side = BezierPickedSide.RIGHT;
            m_btn_datapoint_downed = true;
        }

        private void common_MouseUp( object sender, MouseEventArgs e ) {
            m_btn_datapoint_downed = false;
            this.Opacity = 1.0;
            Point loc_on_screen = Control.MousePosition;
            Point loc_on_trackselector = m_parent.PointToClient( loc_on_screen );
            MouseEventArgs event_arg = new MouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
            m_parent.TrackSelector_MouseUp( this, event_arg );
            Cursor.Position = m_last_mouse_global_location;
            m_parent.Invalidate();
        }

        private void common_MouseMove( object sender, MouseEventArgs e ) {
            if ( m_btn_datapoint_downed ) {
                Point loc_on_screen = Control.MousePosition;
                Point loc_on_trackselector = m_parent.PointToClient( loc_on_screen );
                MouseEventArgs event_arg = new MouseEventArgs( MouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0 );
                BezierPoint ret = m_parent.HandleMouseMoveForBezierMove( event_arg, m_picked_side );

                txtDataPointClock.Text = ((int)ret.getBase().X).ToString();
                txtDataPointValue.Text = ((int)ret.getBase().Y).ToString();
                txtLeftClock.Text = ((int)ret.getControlLeft().X).ToString();
                txtLeftValue.Text = ((int)ret.getControlLeft().Y).ToString();
                txtRightClock.Text = ((int)ret.getControlRight().X).ToString();
                txtRightValue.Text = ((int)ret.getControlRight().Y).ToString();

                m_parent.Invalidate();
            }
        }

        private void btnBackward_Click( object sender, EventArgs e ) {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id );
            int index = -1;
            for ( int i = 0; i < target.size(); i++ ) {
                if ( target.points.get( i ).ID == m_point_id ) {
                    index = i - 1;
                    break;
                }
            }
            if ( 0 <= index ) {
                m_point_id = target.points.get( index ).ID;
                m_point = target.points.get( index );
                UpdateStatus();
                m_parent.EditingPointID = m_point_id;
                m_parent.Invalidate();
            }
        }

        private void btnForward_Click( object sender, EventArgs e ) {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( m_track - 1 ).getBezierChain( m_curve_type, m_chain_id );
            int index = -2;
            for ( int i = 0; i < target.size(); i++ ) {
                if ( target.points.get( i ).ID == m_point_id ) {
                    index = i + 1;
                    break;
                }
            }
            if ( 0 <= index && index < target.size()) {
                m_point_id = target.points.get( index ).ID;
                m_point = target.points.get( index );
                UpdateStatus();
                m_parent.EditingPointID = m_point_id;
                m_parent.Invalidate();
            }
        }
    }

}
