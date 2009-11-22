/*
 * FormSingerStypeConfig.cs
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
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;

#else
using System;
using System.Drawing;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormNoteExpressionConfig extends BForm {
#else
    public class FormNoteExpressionConfig : BForm {
#endif
        boolean m_apply_current_track = false;
        NoteHeadHandle m_note_head_handle = null;

        public NoteHeadHandle getEditedNoteHeadHandle() {
            return m_note_head_handle;
        }

        public void ApplyLanguage() {
            lblTemplate.setText( _( "Template" ) + "(&T)" );
            groupPitchControl.setTitle( _( "Pitch Control" ) );
            lblBendDepth.setText( _( "Bend Depth" ) + "(&B)" );
            lblBendLength.setText( _( "Bend Length" ) + "(&L)" );
            chkUpPortamento.setText( _( "Add portamento in rising movement" ) + "(&R)" );
            chkDownPortamento.setText( _( "Add portamento in falling movement" ) + "(&F)" );

            groupAttack.setTitle( _( "Attack Control (VOCALOID1)" ) );
            groupDynamicsControl.setTitle( _( "Dynamics Control (VOCALOID2)" ) );
            lblDecay.setText( _( "Decay" ) + "(&D)" );
            lblAccent.setText( _( "Accent" ) + "(&A)" );

            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );

#if !JAVA
            lblTemplate.Left = comboTemplate.Left - lblTemplate.Width;
#endif
            setTitle( _( "Expression control property" ) );
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public int getPMBendDepth() {
            return trackBendDepth.getValue();
        }

        public void setPMBendDepth( int value ) {
            trackBendDepth.setValue( value );
            txtBendDepth.setText( value + "" );
        }

        public int getPMBendLength() {
            return trackBendLength.getValue();
        }

        public void setPMBendLength( int value ) {
            trackBendLength.setValue( value );
            txtBendLength.setText( value + "" );
        }

        public int getPMbPortamentoUse() {
            int ret = 0;
            if ( chkUpPortamento.isSelected() ) {
                ret += 1;
            }
            if ( chkDownPortamento.isSelected() ) {
                ret += 2;
            }
            return ret;
        }

        public void setPMbPortamentoUse( int value ) {
            if ( value % 2 == 1 ) {
                chkUpPortamento.setSelected( true );
            } else {
                chkUpPortamento.setSelected( false );
            }
            if ( value >= 2 ) {
                chkDownPortamento.setSelected( true );
            } else {
                chkDownPortamento.setSelected( false );
            }
        }

        public int getDEMdecGainRate() {
            return trackDecay.getValue();
        }

        public void setDEMdecGainRate( int value ) {
            trackDecay.setValue( value );
            txtDecay.setText( value + "" );
        }

        public int getDEMaccent() {
            return trackAccent.getValue();
        }

        public void setDEMaccent( int value ) {
            trackAccent.setValue( value );
            txtAccent.setText( value + "" );
        }

        public FormNoteExpressionConfig( SynthesizerType type, NoteHeadHandle note_head_handle ) {
            if ( note_head_handle != null ) {
                m_note_head_handle = (NoteHeadHandle)note_head_handle.clone();
            }
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            ApplyLanguage();
            if ( type == SynthesizerType.VOCALOID1 ) {
#if JAVA
                getContentPane().remove( groupDynamicsControl );
                getContentPane().remove( panelVocaloid2Template );
                getContentPane().remove( groupPitchControl );
#else
                flowLayoutPanel.Controls.Remove( groupDynamicsControl );
                flowLayoutPanel.Controls.Remove( panelVocaloid2Template );
                flowLayoutPanel.Controls.Remove( groupPitchControl );
#endif
            } else {
#if JAVA
                getContentPane().remove( groupAttack );
#else
                flowLayoutPanel.Controls.Remove( groupAttack );
#endif
            }

            //comboAttackTemplateを更新
            AttackConfig empty = new AttackConfig();
            comboAttackTemplate.removeAllItems();
            empty.contents.IconID = "$01010000";
            empty.contents.Caption = "[Non Attack]";
            comboAttackTemplate.addItem( empty );
            comboAttackTemplate.setSelectedItem( empty );
            String icon_id = "";
            if ( m_note_head_handle != null ) {
                icon_id = m_note_head_handle.IconID;
                txtDuration.setText( m_note_head_handle.Duration + "" );
                txtDepth.setText( m_note_head_handle.Depth + "" );
            } else {
                txtDuration.setEnabled( false );
                txtDepth.setEnabled( false );
                trackDuration.setEnabled( false );
                trackDepth.setEnabled( false );
            }
            for ( Iterator itr = VocaloSysUtil.attackConfigIterator( SynthesizerType.VOCALOID1 ); itr.hasNext(); ) {
                AttackConfig item = (AttackConfig)itr.next();
                comboAttackTemplate.addItem( item );
                if ( item.contents.IconID.Equals( icon_id ) ) {
                    comboAttackTemplate.setSelectedItem( comboAttackTemplate.getItemAt( comboAttackTemplate.getItemCount() - 1 ) );
                }
            }
#if JAVA
            comboAttackTemplate.selectedIndexChangedEvent.add( new BEventHandler( this, "comboAttackTemplate_SelectedIndexChanged" ) );
#else
            comboAttackTemplate.SelectedIndexChanged += new EventHandler( comboAttackTemplate_SelectedIndexChanged );
#endif

#if !JAVA
            Size current_size = this.ClientSize;
            this.ClientSize = new Size( current_size.Width, flowLayoutPanel.ClientSize.Height + flowLayoutPanel.Top * 2 );
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
#endif
        }

        private void comboAttackTemplate_SelectedIndexChanged( Object sender, BEventArgs e ) {
            int index = comboAttackTemplate.getSelectedIndex();
            if ( index < 0 ) {
                return;
            }
            if ( index == 0 ) {
                m_note_head_handle = null;
                txtDuration.setEnabled( false );
                trackDuration.setEnabled( false );
                txtDepth.setEnabled( false );
                trackDepth.setEnabled( false );
                return;
            }
            txtDuration.setEnabled( true );
            trackDuration.setEnabled( true );
            txtDepth.setEnabled( true );
            trackDepth.setEnabled( true );
            AttackConfig aconfig = (AttackConfig)comboAttackTemplate.getSelectedItem();
            if ( m_note_head_handle == null ) {
                txtDuration.setText( aconfig.contents.Duration + "" );
                txtDepth.setText( aconfig.contents.Depth + "" );
            }
            m_note_head_handle = (NoteHeadHandle)aconfig.contents.clone();
            m_note_head_handle.Duration = trackDuration.getValue();
            m_note_head_handle.Depth = trackDepth.getValue();
        }

        private void trackBendDepth_Scroll( Object sender, BEventArgs e ) {
            txtBendDepth.setText( trackBendDepth.getValue() + "" );
        }

        private void txtBendDepth_TextChanged( Object sender, BEventArgs e ) {
            try {
                int draft = PortUtil.parseInt( txtBendDepth.getText() );
                if ( draft != trackBendDepth.getValue() ) {
                    if ( draft < trackBendDepth.getMinimum() ) {
                        draft = trackBendDepth.getMinimum();
                        txtBendDepth.setText( draft + "" );
                    } else if ( trackBendDepth.getMaximum() < draft ) {
                        draft = trackBendDepth.getMaximum();
                        txtBendDepth.setText( draft + "" );
                    }
                    trackBendDepth.setValue( draft );
                }
            } catch ( Exception ex ) {
                //txtBendDepth.Text = trackBendDepth.Value + "";
            }
        }

        private void trackBendLength_Scroll( Object sender, BEventArgs e ) {
            txtBendLength.setText( trackBendLength.getValue() + "" );
        }

        private void txtBendLength_TextChanged( Object sender, BEventArgs e ) {
            try {
                int draft = PortUtil.parseInt( txtBendLength.getText() );
                if ( draft != trackBendLength.getValue() ) {
                    if ( draft < trackBendLength.getMinimum() ) {
                        draft = trackBendLength.getMinimum();
                        txtBendLength.setText( draft + "" );
                    } else if ( trackBendLength.getMaximum() < draft ) {
                        draft = trackBendLength.getMaximum();
                        txtBendLength.setText( draft + "" );
                    }
                    trackBendLength.setValue( draft );
                }
            } catch ( Exception ex ) {
                //txtBendLength.Text = trackBendLength.Value + "";
            }
        }

        private void trackDecay_Scroll( Object sender, BEventArgs e ) {
            txtDecay.setText( trackDecay.getValue() + "" );
        }

        private void txtDecay_TextChanged( Object sender, BEventArgs e ) {
            try {
                int draft = PortUtil.parseInt( txtDecay.getText() );
                if ( draft != trackDecay.getValue() ) {
                    if ( draft < trackDecay.getMinimum() ) {
                        draft = trackDecay.getMinimum();
                        txtDecay.setText( draft + "" );
                    } else if ( trackDecay.getMaximum() < draft ) {
                        draft = trackDecay.getMaximum();
                        txtDecay.setText( draft + "" );
                    }
                    trackDecay.setValue( draft );
                }
            } catch ( Exception ex ) {
                //txtDecay.Text = trackDecay.Value + "";
            }
        }

        private void trackAccent_Scroll( Object sender, BEventArgs e ) {
            txtAccent.setText( trackAccent.getValue() + "" );
        }

        private void txtAccent_TextChanged( Object sender, BEventArgs e ) {
            try {
                int draft = PortUtil.parseInt( txtAccent.getText() );
                if ( draft != trackAccent.getValue() ) {
                    if ( draft < trackAccent.getMinimum() ) {
                        draft = trackAccent.getMinimum();
                        txtAccent.setText( draft + "" );
                    } else if ( trackAccent.getMaximum() < draft ) {
                        draft = trackAccent.getMaximum();
                        txtAccent.setText( draft + "" );
                    }
                    trackAccent.setValue( draft );
                }
            } catch ( Exception ex ) {
                //txtAccent.Text = trackAccent.Value + "";
            }
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        private void comboBox1_SelectedIndexChanged( Object sender, BEventArgs e ) {
            switch ( comboTemplate.getSelectedIndex() ) {
                case 1:
                    setPMBendDepth( 8 );
                    setPMBendLength( 0 );
                    setPMbPortamentoUse( 0 );
                    setDEMdecGainRate( 50 );
                    setDEMaccent( 50 );
                    break;
                case 2:
                    setPMBendDepth( 8 );
                    setPMBendLength( 0 );
                    setPMbPortamentoUse( 0 );
                    setDEMdecGainRate( 50 );
                    setDEMaccent( 68 );
                    break;
                case 3:
                    setPMBendDepth( 8 );
                    setPMBendLength( 0 );
                    setPMbPortamentoUse( 0 );
                    setDEMdecGainRate( 70 );
                    setDEMaccent( 80 );
                    break;
                case 4:
                    setPMBendDepth( 20 );
                    setPMBendLength( 0 );
                    setPMbPortamentoUse( 3 );
                    setDEMdecGainRate( 50 );
                    setDEMaccent( 42 );
                    break;
                case 5:
                    setPMBendDepth( 20 );
                    setPMBendLength( 0 );
                    setPMbPortamentoUse( 3 );
                    setDEMdecGainRate( 50 );
                    setDEMaccent( 25 );
                    break;
            }
        }

        private void btnApply_Click( Object sender, BEventArgs e ) {
            if ( AppManager.showMessageBox( _( "Would you like to change singer style for all events?" ),
                                  FormMain._APP_NAME,
                                  AppManager.MSGBOX_YES_NO_OPTION,
                                  AppManager.MSGBOX_WARNING_MESSAGE ) == BDialogResult.YES ) {
                m_apply_current_track = true;
                setDialogResult( BDialogResult.OK );
            }
        }

        public boolean getApplyCurrentTrack() {
            return m_apply_current_track;
        }

        private void trackDuration_Scroll( Object sender, BEventArgs e ) {
            txtDuration.setText( trackDuration.getValue() + "" );
            if ( m_note_head_handle != null ) {
                m_note_head_handle.Duration = trackDuration.getValue();
            }
        }

        private void trackDepth_Scroll( Object sender, BEventArgs e ) {
            txtDepth.setText( trackDepth.getValue() + "" );
            if ( m_note_head_handle != null ) {
                m_note_head_handle.Depth = trackDepth.getValue();
            }
        }

        private void txtDuration_TextChanged( Object sender, BEventArgs e ) {
            try {
                int draft = PortUtil.parseInt( txtDuration.getText() );
                if ( draft != trackDuration.getValue() ) {
                    if ( draft < trackDuration.getMinimum() ) {
                        draft = trackDuration.getMinimum();
                    } else if ( trackDuration.getMaximum() < draft ) {
                        draft = trackDuration.getMaximum();
                    }
                    txtDuration.setText( draft + "" );
                    trackDuration.setValue( draft );
                    if ( m_note_head_handle != null ) {
                        m_note_head_handle.Duration = draft;
                    }
                }
            } catch ( Exception ex ) {
            }
        }

        private void txtDepth_TextChanged( Object sender, BEventArgs e ) {
            try {
                int draft = PortUtil.parseInt( txtDepth.getText() );
                if ( draft != trackDepth.getValue() ) {
                    if ( draft < trackDepth.getMinimum() ) {
                        draft = trackDepth.getMinimum();
                    } else if ( trackDepth.getMaximum() < draft ) {
                        draft = trackDepth.getMaximum();
                    }
                    txtDepth.setText( draft + "" );
                    trackDepth.setValue( draft );
                    if ( m_note_head_handle != null ) {
                        m_note_head_handle.Depth = trackDepth.getValue();
                    }
                }
            } catch ( Exception ex ) {
            }
        }

        private void registerEventHandlers() {
#if JAVA
#else
            this.txtBendLength.TextChanged += new System.EventHandler( this.txtBendLength_TextChanged );
            this.txtBendDepth.TextChanged += new System.EventHandler( this.txtBendDepth_TextChanged );
            this.trackBendLength.Scroll += new System.EventHandler( this.trackBendLength_Scroll );
            this.trackBendDepth.Scroll += new System.EventHandler( this.trackBendDepth_Scroll );
            this.txtAccent.TextChanged += new System.EventHandler( this.txtAccent_TextChanged );
            this.txtDecay.TextChanged += new System.EventHandler( this.txtDecay_TextChanged );
            this.trackAccent.Scroll += new System.EventHandler( this.trackAccent_Scroll );
            this.trackDecay.Scroll += new System.EventHandler( this.trackDecay_Scroll );
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            this.comboTemplate.SelectedIndexChanged += new System.EventHandler( this.comboBox1_SelectedIndexChanged );
            this.txtDepth.TextChanged += new System.EventHandler( this.txtDepth_TextChanged );
            this.txtDuration.TextChanged += new System.EventHandler( this.txtDuration_TextChanged );
            this.trackDepth.Scroll += new System.EventHandler( this.trackDepth_Scroll );
            this.trackDuration.Scroll += new System.EventHandler( this.trackDuration_Scroll );
#endif
        }

        private void setResources() {
        }

#if JAVA
	    private JPanel jContentPane = null;
	    private JPanel panelVocaloid2Template = null;
	    private JLabel lblTemplate = null;
	    private JComboBox comboTemplate = null;
	    private JLabel jLabel1 = null;
	    private BGroupBox groupPitchControl = null;
	    private JLabel lblBendDepth = null;
	    private JSlider trackBendDepth = null;
	    private JTextField txtBendDepth = null;
	    private JLabel jLabel3 = null;
	    private JLabel lblBendLength = null;
	    private JSlider trackBendLength = null;
	    private JTextField txtBendLength = null;
	    private JLabel jLabel5 = null;
	    private JCheckBox chkUpPortamento = null;
	    private JCheckBox chkDownPortamento = null;
	    private JLabel jLabel6 = null;
	    private BGroupBox groupDynamicsControl = null;
	    private JLabel lblDecay = null;
	    private JSlider trackDecay = null;
	    private JTextField txtDecay = null;
	    private JLabel jLabel31 = null;
	    private JLabel lblAccent = null;
	    private JSlider trackAccent = null;
	    private JTextField txtAccent = null;
	    private JLabel jLabel51 = null;
	    private JLabel jLabel61 = null;
	    private BGroupBox groupAttack = null;
	    private JLabel lblDuration = null;
	    private JSlider trackDuration = null;
	    private JTextField txtDuration = null;
	    private JLabel jLabel311 = null;
	    private JLabel lblDepth = null;
	    private JSlider trackDepth = null;
	    private JTextField txtDepth = null;
	    private JLabel jLabel511 = null;
	    private JLabel jLabel611 = null;
	    private JLabel lblAttackTemplate = null;
	    private JComboBox comboAttackTemplate = null;
	    private JPanel jPanel2 = null;
	    private JButton btnOK = null;
	    private JButton btnCancel = null;

	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(403, 461);
		    this.setContentPane(getJContentPane());
		    this.setTitle("Default Singer Style");
	    }

	    /**
	     * This method initializes jContentPane
	     * 
	     * @return javax.swing.JPanel
	     */
	    private JPanel getJContentPane() {
		    if (jContentPane == null) {
			    GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
			    gridBagConstraints20.gridx = 0;
			    gridBagConstraints20.insets = new Insets(16, 0, 16, 0);
			    gridBagConstraints20.anchor = GridBagConstraints.SOUTHEAST;
			    gridBagConstraints20.weighty = 1.0D;
			    gridBagConstraints20.fill = GridBagConstraints.VERTICAL;
			    gridBagConstraints20.gridy = 4;
			    GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			    gridBagConstraints19.gridx = 0;
			    gridBagConstraints19.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints19.insets = new Insets(12, 12, 0, 12);
			    gridBagConstraints19.gridy = 3;
			    GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			    gridBagConstraints16.gridx = 0;
			    gridBagConstraints16.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints16.weightx = 1.0D;
			    gridBagConstraints16.insets = new Insets(12, 12, 0, 12);
			    gridBagConstraints16.gridy = 2;
			    GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			    gridBagConstraints15.gridx = 0;
			    gridBagConstraints15.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints15.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints15.insets = new Insets(12, 12, 0, 12);
			    gridBagConstraints15.gridy = 1;
			    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			    gridBagConstraints3.gridx = 0;
			    gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints3.weightx = 1.0D;
			    gridBagConstraints3.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints3.insets = new Insets(12, 12, 0, 12);
			    gridBagConstraints3.gridy = 0;
			    jContentPane = new JPanel();
			    jContentPane.setLayout(new GridBagLayout());
			    jContentPane.add(getPanelVocaloid2Template(), gridBagConstraints3);
			    jContentPane.add(getGroupPitchControl(), gridBagConstraints15);
			    jContentPane.add(getGroupDynamicsControl(), gridBagConstraints16);
			    jContentPane.add(getGroupAttack(), gridBagConstraints19);
			    jContentPane.add(getJPanel2(), gridBagConstraints20);
		    }
		    return jContentPane;
	    }

	    /**
	     * This method initializes panelVocaloid2Template	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPanelVocaloid2Template() {
		    if (panelVocaloid2Template == null) {
			    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			    gridBagConstraints2.gridx = 0;
			    gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints2.weightx = 1.0D;
			    gridBagConstraints2.gridy = 0;
			    jLabel1 = new JLabel();
			    jLabel1.setText("");
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.fill = GridBagConstraints.NONE;
			    gridBagConstraints1.gridy = 0;
			    gridBagConstraints1.weightx = 0.0D;
			    gridBagConstraints1.anchor = GridBagConstraints.EAST;
			    gridBagConstraints1.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints1.gridx = 2;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.gridx = 1;
			    gridBagConstraints.anchor = GridBagConstraints.EAST;
			    gridBagConstraints.gridy = 0;
			    lblTemplate = new JLabel();
			    lblTemplate.setText("Template");
			    panelVocaloid2Template = new JPanel();
			    panelVocaloid2Template.setLayout(new GridBagLayout());
			    panelVocaloid2Template.add(lblTemplate, gridBagConstraints);
			    panelVocaloid2Template.add(getComboTemplate(), gridBagConstraints1);
			    panelVocaloid2Template.add(jLabel1, gridBagConstraints2);
		    }
		    return panelVocaloid2Template;
	    }

	    /**
	     * This method initializes comboTemplate	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboTemplate() {
		    if (comboTemplate == null) {
			    comboTemplate = new JComboBox();
			    comboTemplate.setPreferredSize(new Dimension(121, 22));
		    }
		    return comboTemplate;
	    }

	    /**
	     * This method initializes groupPitchControl	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupPitchControl() {
		    if (groupPitchControl == null) {
			    GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			    gridBagConstraints14.gridx = 0;
			    gridBagConstraints14.fill = GridBagConstraints.BOTH;
			    gridBagConstraints14.weighty = 1.0D;
			    gridBagConstraints14.weightx = 1.0D;
			    gridBagConstraints14.gridwidth = 4;
			    gridBagConstraints14.gridy = 4;
			    jLabel6 = new JLabel();
			    jLabel6.setText("");
			    GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			    gridBagConstraints13.gridx = 0;
			    gridBagConstraints13.gridwidth = 4;
			    gridBagConstraints13.anchor = GridBagConstraints.WEST;
			    gridBagConstraints13.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints13.gridy = 3;
			    GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			    gridBagConstraints12.gridx = 0;
			    gridBagConstraints12.gridwidth = 4;
			    gridBagConstraints12.anchor = GridBagConstraints.WEST;
			    gridBagConstraints12.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints12.gridy = 2;
			    GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			    gridBagConstraints11.gridx = 3;
			    gridBagConstraints11.weightx = 1.0D;
			    gridBagConstraints11.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints11.anchor = GridBagConstraints.WEST;
			    gridBagConstraints11.gridy = 1;
			    jLabel5 = new JLabel();
			    jLabel5.setText("%");
			    GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			    gridBagConstraints10.fill = GridBagConstraints.NONE;
			    gridBagConstraints10.gridy = 1;
			    gridBagConstraints10.weightx = 1.0;
			    gridBagConstraints10.anchor = GridBagConstraints.WEST;
			    gridBagConstraints10.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints10.gridx = 2;
			    GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			    gridBagConstraints9.fill = GridBagConstraints.NONE;
			    gridBagConstraints9.gridy = 1;
			    gridBagConstraints9.weightx = 0.0D;
			    gridBagConstraints9.anchor = GridBagConstraints.WEST;
			    gridBagConstraints9.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints9.gridx = 1;
			    GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			    gridBagConstraints8.gridx = 0;
			    gridBagConstraints8.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints8.anchor = GridBagConstraints.WEST;
			    gridBagConstraints8.gridy = 1;
			    lblBendLength = new JLabel();
			    lblBendLength.setText("Bend Length");
			    GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			    gridBagConstraints7.gridx = 3;
			    gridBagConstraints7.anchor = GridBagConstraints.WEST;
			    gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints7.weightx = 1.0D;
			    gridBagConstraints7.gridy = 0;
			    jLabel3 = new JLabel();
			    jLabel3.setText("%");
			    GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			    gridBagConstraints6.fill = GridBagConstraints.NONE;
			    gridBagConstraints6.gridy = 0;
			    gridBagConstraints6.weightx = 1.0;
			    gridBagConstraints6.anchor = GridBagConstraints.WEST;
			    gridBagConstraints6.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints6.gridx = 2;
			    GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			    gridBagConstraints5.fill = GridBagConstraints.NONE;
			    gridBagConstraints5.gridy = 0;
			    gridBagConstraints5.weightx = 0.0D;
			    gridBagConstraints5.anchor = GridBagConstraints.WEST;
			    gridBagConstraints5.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints5.gridx = 1;
			    GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			    gridBagConstraints4.gridx = 0;
			    gridBagConstraints4.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints4.anchor = GridBagConstraints.WEST;
			    gridBagConstraints4.gridy = 0;
			    lblBendDepth = new JLabel();
			    lblBendDepth.setText("Bend Depth");
			    groupPitchControl = new BGroupBox();
			    groupPitchControl.setLayout(new GridBagLayout());
			    groupPitchControl.setTitle("Pitch Control (VOCALOID2)");
			    groupPitchControl.add(lblBendDepth, gridBagConstraints4);
			    groupPitchControl.add(getTrackBendDepth(), gridBagConstraints5);
			    groupPitchControl.add(getTxtBendDepth(), gridBagConstraints6);
			    groupPitchControl.add(jLabel3, gridBagConstraints7);
			    groupPitchControl.add(lblBendLength, gridBagConstraints8);
			    groupPitchControl.add(getTrackBendLength(), gridBagConstraints9);
			    groupPitchControl.add(getTxtBendLength(), gridBagConstraints10);
			    groupPitchControl.add(jLabel5, gridBagConstraints11);
			    groupPitchControl.add(getChkUpPortamento(), gridBagConstraints12);
			    groupPitchControl.add(getChkDownPortamento(), gridBagConstraints13);
			    groupPitchControl.add(jLabel6, gridBagConstraints14);
		    }
		    return groupPitchControl;
	    }

	    /**
	     * This method initializes trackBendDepth	
	     * 	
	     * @return javax.swing.JSlider	
	     */
	    private JSlider getTrackBendDepth() {
		    if (trackBendDepth == null) {
			    trackBendDepth = new JSlider();
			    trackBendDepth.setPreferredSize(new Dimension(156, 18));
			    trackBendDepth.setValue(8);
		    }
		    return trackBendDepth;
	    }

	    /**
	     * This method initializes txtBendDepth	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtBendDepth() {
		    if (txtBendDepth == null) {
			    txtBendDepth = new JTextField();
			    txtBendDepth.setText("8");
			    txtBendDepth.setHorizontalAlignment(JTextField.RIGHT);
			    txtBendDepth.setPreferredSize(new Dimension(39, 19));
		    }
		    return txtBendDepth;
	    }

	    /**
	     * This method initializes trackBendLength	
	     * 	
	     * @return javax.swing.JSlider	
	     */
	    private JSlider getTrackBendLength() {
		    if (trackBendLength == null) {
			    trackBendLength = new JSlider();
			    trackBendLength.setPreferredSize(new Dimension(156, 18));
			    trackBendLength.setValue(0);
		    }
		    return trackBendLength;
	    }

	    /**
	     * This method initializes txtBendLength	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtBendLength() {
		    if (txtBendLength == null) {
			    txtBendLength = new JTextField();
			    txtBendLength.setText("0");
			    txtBendLength.setPreferredSize(new Dimension(39, 19));
			    txtBendLength.setHorizontalAlignment(JTextField.RIGHT);
		    }
		    return txtBendLength;
	    }

	    /**
	     * This method initializes chkUpPortamento	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkUpPortamento() {
		    if (chkUpPortamento == null) {
			    chkUpPortamento = new JCheckBox();
			    chkUpPortamento.setText("Add portamento in rising movement");
		    }
		    return chkUpPortamento;
	    }

	    /**
	     * This method initializes chkDownPortamento	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkDownPortamento() {
		    if (chkDownPortamento == null) {
			    chkDownPortamento = new JCheckBox();
			    chkDownPortamento.setText("Add portamento in falling movement");
		    }
		    return chkDownPortamento;
	    }

	    /**
	     * This method initializes groupDynamicsControl	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupDynamicsControl() {
		    if (groupDynamicsControl == null) {
			    GridBagConstraints gridBagConstraints141 = new GridBagConstraints();
			    gridBagConstraints141.fill = GridBagConstraints.BOTH;
			    gridBagConstraints141.gridx = 0;
			    gridBagConstraints141.gridy = 4;
			    gridBagConstraints141.weightx = 1.0D;
			    gridBagConstraints141.weighty = 1.0D;
			    gridBagConstraints141.gridwidth = 4;
			    jLabel61 = new JLabel();
			    jLabel61.setText("");
			    GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
			    gridBagConstraints111.gridx = 3;
			    gridBagConstraints111.anchor = GridBagConstraints.WEST;
			    gridBagConstraints111.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints111.weightx = 1.0D;
			    gridBagConstraints111.gridy = 1;
			    jLabel51 = new JLabel();
			    jLabel51.setText("%");
			    GridBagConstraints gridBagConstraints101 = new GridBagConstraints();
			    gridBagConstraints101.fill = GridBagConstraints.NONE;
			    gridBagConstraints101.gridy = 1;
			    gridBagConstraints101.weightx = 0.0D;
			    gridBagConstraints101.anchor = GridBagConstraints.WEST;
			    gridBagConstraints101.gridx = 2;
			    GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
			    gridBagConstraints91.fill = GridBagConstraints.NONE;
			    gridBagConstraints91.gridy = 1;
			    gridBagConstraints91.weightx = 0.0D;
			    gridBagConstraints91.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints91.anchor = GridBagConstraints.WEST;
			    gridBagConstraints91.gridx = 1;
			    GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			    gridBagConstraints81.anchor = GridBagConstraints.WEST;
			    gridBagConstraints81.gridx = 0;
			    gridBagConstraints81.gridy = 1;
			    gridBagConstraints81.insets = new Insets(0, 12, 0, 0);
			    lblAccent = new JLabel();
			    lblAccent.setText("Accent");
			    GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
			    gridBagConstraints71.gridx = 3;
			    gridBagConstraints71.weightx = 1.0D;
			    gridBagConstraints71.anchor = GridBagConstraints.WEST;
			    gridBagConstraints71.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints71.gridy = 0;
			    jLabel31 = new JLabel();
			    jLabel31.setText("%");
			    GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			    gridBagConstraints61.fill = GridBagConstraints.NONE;
			    gridBagConstraints61.gridy = 0;
			    gridBagConstraints61.weightx = 0.0D;
			    gridBagConstraints61.anchor = GridBagConstraints.WEST;
			    gridBagConstraints61.gridx = 2;
			    GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			    gridBagConstraints51.fill = GridBagConstraints.NONE;
			    gridBagConstraints51.gridy = 0;
			    gridBagConstraints51.weightx = 0.0D;
			    gridBagConstraints51.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints51.anchor = GridBagConstraints.WEST;
			    gridBagConstraints51.gridx = 1;
			    GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
			    gridBagConstraints41.anchor = GridBagConstraints.WEST;
			    gridBagConstraints41.gridx = 0;
			    gridBagConstraints41.gridy = 0;
			    gridBagConstraints41.insets = new Insets(0, 12, 0, 0);
			    lblDecay = new JLabel();
			    lblDecay.setText("Decay");
			    groupDynamicsControl = new BGroupBox();
			    groupDynamicsControl.setLayout(new GridBagLayout());
			    groupDynamicsControl.setTitle("Dynamics Control (VOCALOID2)");
			    groupDynamicsControl.add(lblDecay, gridBagConstraints41);
			    groupDynamicsControl.add(getTrackDecay(), gridBagConstraints51);
			    groupDynamicsControl.add(getTxtDecay(), gridBagConstraints61);
			    groupDynamicsControl.add(jLabel31, gridBagConstraints71);
			    groupDynamicsControl.add(lblAccent, gridBagConstraints81);
			    groupDynamicsControl.add(getTrackAccent(), gridBagConstraints91);
			    groupDynamicsControl.add(getTxtAccent(), gridBagConstraints101);
			    groupDynamicsControl.add(jLabel51, gridBagConstraints111);
			    groupDynamicsControl.add(jLabel61, gridBagConstraints141);
		    }
		    return groupDynamicsControl;
	    }

	    /**
	     * This method initializes trackDecay	
	     * 	
	     * @return javax.swing.JSlider	
	     */
	    private JSlider getTrackDecay() {
		    if (trackDecay == null) {
			    trackDecay = new JSlider();
			    trackDecay.setPreferredSize(new Dimension(156, 18));
		    }
		    return trackDecay;
	    }

	    /**
	     * This method initializes txtDecay	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtDecay() {
		    if (txtDecay == null) {
			    txtDecay = new JTextField();
			    txtDecay.setPreferredSize(new Dimension(39, 19));
			    txtDecay.setHorizontalAlignment(JTextField.RIGHT);
			    txtDecay.setText("50");
		    }
		    return txtDecay;
	    }

	    /**
	     * This method initializes trackAccent	
	     * 	
	     * @return javax.swing.JSlider	
	     */
	    private JSlider getTrackAccent() {
		    if (trackAccent == null) {
			    trackAccent = new JSlider();
			    trackAccent.setPreferredSize(new Dimension(156, 18));
		    }
		    return trackAccent;
	    }

	    /**
	     * This method initializes txtAccent	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtAccent() {
		    if (txtAccent == null) {
			    txtAccent = new JTextField();
			    txtAccent.setPreferredSize(new Dimension(39, 19));
			    txtAccent.setHorizontalAlignment(JTextField.RIGHT);
			    txtAccent.setText("50");
		    }
		    return txtAccent;
	    }

	    /**
	     * This method initializes groupAttack	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupAttack() {
		    if (groupAttack == null) {
			    GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
			    gridBagConstraints18.fill = GridBagConstraints.NONE;
			    gridBagConstraints18.gridy = 0;
			    gridBagConstraints18.weightx = 1.0;
			    gridBagConstraints18.gridwidth = 3;
			    gridBagConstraints18.anchor = GridBagConstraints.WEST;
			    gridBagConstraints18.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints18.gridx = 1;
			    GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
			    gridBagConstraints17.gridx = 0;
			    gridBagConstraints17.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints17.gridy = 0;
			    lblAttackTemplate = new JLabel();
			    lblAttackTemplate.setText("Attack Variation");
			    GridBagConstraints gridBagConstraints1411 = new GridBagConstraints();
			    gridBagConstraints1411.fill = GridBagConstraints.BOTH;
			    gridBagConstraints1411.gridx = 0;
			    gridBagConstraints1411.gridy = 5;
			    gridBagConstraints1411.weightx = 1.0D;
			    gridBagConstraints1411.weighty = 1.0D;
			    gridBagConstraints1411.gridwidth = 4;
			    jLabel611 = new JLabel();
			    jLabel611.setText("");
			    GridBagConstraints gridBagConstraints1111 = new GridBagConstraints();
			    gridBagConstraints1111.gridx = 3;
			    gridBagConstraints1111.anchor = GridBagConstraints.WEST;
			    gridBagConstraints1111.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints1111.weightx = 1.0D;
			    gridBagConstraints1111.gridy = 2;
			    jLabel511 = new JLabel();
			    jLabel511.setText("%");
			    GridBagConstraints gridBagConstraints1011 = new GridBagConstraints();
			    gridBagConstraints1011.fill = GridBagConstraints.NONE;
			    gridBagConstraints1011.gridy = 2;
			    gridBagConstraints1011.weightx = 0.0D;
			    gridBagConstraints1011.anchor = GridBagConstraints.WEST;
			    gridBagConstraints1011.gridx = 2;
			    GridBagConstraints gridBagConstraints911 = new GridBagConstraints();
			    gridBagConstraints911.fill = GridBagConstraints.NONE;
			    gridBagConstraints911.gridy = 2;
			    gridBagConstraints911.weightx = 0.0D;
			    gridBagConstraints911.anchor = GridBagConstraints.WEST;
			    gridBagConstraints911.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints911.gridx = 1;
			    GridBagConstraints gridBagConstraints811 = new GridBagConstraints();
			    gridBagConstraints811.anchor = GridBagConstraints.WEST;
			    gridBagConstraints811.gridx = 0;
			    gridBagConstraints811.gridy = 2;
			    gridBagConstraints811.insets = new Insets(0, 12, 0, 0);
			    lblDepth = new JLabel();
			    lblDepth.setText("Depth");
			    GridBagConstraints gridBagConstraints711 = new GridBagConstraints();
			    gridBagConstraints711.gridx = 3;
			    gridBagConstraints711.weightx = 1.0D;
			    gridBagConstraints711.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints711.anchor = GridBagConstraints.WEST;
			    gridBagConstraints711.gridy = 1;
			    jLabel311 = new JLabel();
			    jLabel311.setText("%");
			    GridBagConstraints gridBagConstraints611 = new GridBagConstraints();
			    gridBagConstraints611.fill = GridBagConstraints.NONE;
			    gridBagConstraints611.gridy = 1;
			    gridBagConstraints611.weightx = 0.0D;
			    gridBagConstraints611.anchor = GridBagConstraints.WEST;
			    gridBagConstraints611.gridx = 2;
			    GridBagConstraints gridBagConstraints511 = new GridBagConstraints();
			    gridBagConstraints511.fill = GridBagConstraints.NONE;
			    gridBagConstraints511.gridy = 1;
			    gridBagConstraints511.weightx = 0.0D;
			    gridBagConstraints511.anchor = GridBagConstraints.WEST;
			    gridBagConstraints511.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints511.gridx = 1;
			    GridBagConstraints gridBagConstraints411 = new GridBagConstraints();
			    gridBagConstraints411.anchor = GridBagConstraints.WEST;
			    gridBagConstraints411.gridx = 0;
			    gridBagConstraints411.gridy = 1;
			    gridBagConstraints411.insets = new Insets(0, 12, 0, 0);
			    lblDuration = new JLabel();
			    lblDuration.setText("Duration");
			    groupAttack = new BGroupBox();
			    groupAttack.setLayout(new GridBagLayout());
			    groupAttack.setTitle("Attack (VOCALOID1)");
			    groupAttack.add(lblDuration, gridBagConstraints411);
			    groupAttack.add(getTrackDuration(), gridBagConstraints511);
			    groupAttack.add(getTxtDuration(), gridBagConstraints611);
			    groupAttack.add(jLabel311, gridBagConstraints711);
			    groupAttack.add(lblDepth, gridBagConstraints811);
			    groupAttack.add(getTrackDepth(), gridBagConstraints911);
			    groupAttack.add(getTxtDepth(), gridBagConstraints1011);
			    groupAttack.add(jLabel511, gridBagConstraints1111);
			    groupAttack.add(jLabel611, gridBagConstraints1411);
			    groupAttack.add(lblAttackTemplate, gridBagConstraints17);
			    groupAttack.add(getComboAttackTemplate(), gridBagConstraints18);
		    }
		    return groupAttack;
	    }

	    /**
	     * This method initializes trackDuration	
	     * 	
	     * @return javax.swing.JSlider	
	     */
	    private JSlider getTrackDuration() {
		    if (trackDuration == null) {
			    trackDuration = new JSlider();
			    trackDuration.setPreferredSize(new Dimension(156, 18));
			    trackDuration.setValue(64);
			    trackDuration.setMaximum(127);
		    }
		    return trackDuration;
	    }

	    /**
	     * This method initializes txtDuration	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtDuration() {
		    if (txtDuration == null) {
			    txtDuration = new JTextField();
			    txtDuration.setPreferredSize(new Dimension(39, 19));
			    txtDuration.setHorizontalAlignment(JTextField.RIGHT);
			    txtDuration.setText("64");
		    }
		    return txtDuration;
	    }

	    /**
	     * This method initializes trackDepth	
	     * 	
	     * @return javax.swing.JSlider	
	     */
	    private JSlider getTrackDepth() {
		    if (trackDepth == null) {
			    trackDepth = new JSlider();
			    trackDepth.setPreferredSize(new Dimension(156, 18));
			    trackDepth.setMaximum(127);
			    trackDepth.setValue(64);
		    }
		    return trackDepth;
	    }

	    /**
	     * This method initializes txtDepth	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtDepth() {
		    if (txtDepth == null) {
			    txtDepth = new JTextField();
			    txtDepth.setPreferredSize(new Dimension(39, 19));
			    txtDepth.setHorizontalAlignment(JTextField.RIGHT);
			    txtDepth.setText("64");
		    }
		    return txtDepth;
	    }

	    /**
	     * This method initializes comboAttackTemplate	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboAttackTemplate() {
		    if (comboAttackTemplate == null) {
			    comboAttackTemplate = new JComboBox();
			    comboAttackTemplate.setPreferredSize(new Dimension(143, 20));
		    }
		    return comboAttackTemplate;
	    }

	    /**
	     * This method initializes jPanel2	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel2() {
		    if (jPanel2 == null) {
			    GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			    gridBagConstraints52.anchor = GridBagConstraints.SOUTHWEST;
			    gridBagConstraints52.gridx = 1;
			    gridBagConstraints52.gridy = 0;
			    gridBagConstraints52.insets = new Insets(0, 0, 0, 16);
			    GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			    gridBagConstraints42.anchor = GridBagConstraints.WEST;
			    gridBagConstraints42.gridx = 0;
			    gridBagConstraints42.gridy = 0;
			    gridBagConstraints42.insets = new Insets(0, 0, 0, 16);
			    jPanel2 = new JPanel();
			    jPanel2.setLayout(new GridBagLayout());
			    jPanel2.add(getBtnOK(), gridBagConstraints42);
			    jPanel2.add(getBtnCancel(), gridBagConstraints52);
		    }
		    return jPanel2;
	    }

	    /**
	     * This method initializes btnOK	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnOK() {
		    if (btnOK == null) {
			    btnOK = new JButton();
			    btnOK.setText("OK");
		    }
		    return btnOK;
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
            this.groupPitchControl = new BGroupBox();
            this.label5 = new BLabel();
            this.label4 = new BLabel();
            this.txtBendLength = new Boare.Cadencii.NumberTextBox();
            this.txtBendDepth = new Boare.Cadencii.NumberTextBox();
            this.trackBendLength = new BSlider();
            this.trackBendDepth = new BSlider();
            this.chkDownPortamento = new BCheckBox();
            this.chkUpPortamento = new BCheckBox();
            this.lblBendLength = new BLabel();
            this.lblBendDepth = new BLabel();
            this.groupDynamicsControl = new BGroupBox();
            this.label7 = new BLabel();
            this.label6 = new BLabel();
            this.txtAccent = new Boare.Cadencii.NumberTextBox();
            this.txtDecay = new Boare.Cadencii.NumberTextBox();
            this.trackAccent = new BSlider();
            this.trackDecay = new BSlider();
            this.lblAccent = new BLabel();
            this.lblDecay = new BLabel();
            this.lblTemplate = new BLabel();
            this.btnCancel = new BButton();
            this.btnOK = new BButton();
            this.comboTemplate = new BComboBox();
            this.groupAttack = new BGroupBox();
            this.lblAttackTemplate = new BLabel();
            this.comboAttackTemplate = new BComboBox();
            this.txtDepth = new Boare.Cadencii.NumberTextBox();
            this.txtDuration = new Boare.Cadencii.NumberTextBox();
            this.trackDepth = new BSlider();
            this.trackDuration = new BSlider();
            this.lblDepth = new BLabel();
            this.lblDuration = new BLabel();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelVocaloid2Template = new BPanel();
            this.panelButtons = new BPanel();
            this.groupPitchControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendDepth)).BeginInit();
            this.groupDynamicsControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAccent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDecay)).BeginInit();
            this.groupAttack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDuration)).BeginInit();
            this.flowLayoutPanel.SuspendLayout();
            this.panelVocaloid2Template.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPitchControl
            // 
            this.groupPitchControl.Controls.Add( this.label5 );
            this.groupPitchControl.Controls.Add( this.label4 );
            this.groupPitchControl.Controls.Add( this.txtBendLength );
            this.groupPitchControl.Controls.Add( this.txtBendDepth );
            this.groupPitchControl.Controls.Add( this.trackBendLength );
            this.groupPitchControl.Controls.Add( this.trackBendDepth );
            this.groupPitchControl.Controls.Add( this.chkDownPortamento );
            this.groupPitchControl.Controls.Add( this.chkUpPortamento );
            this.groupPitchControl.Controls.Add( this.lblBendLength );
            this.groupPitchControl.Controls.Add( this.lblBendDepth );
            this.groupPitchControl.Location = new System.Drawing.Point( 3, 38 );
            this.groupPitchControl.Name = "groupPitchControl";
            this.groupPitchControl.Size = new System.Drawing.Size( 367, 130 );
            this.groupPitchControl.TabIndex = 0;
            this.groupPitchControl.TabStop = false;
            this.groupPitchControl.Text = "Pitch Control (VOCALOID2)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 345, 42 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 11, 12 );
            this.label5.TabIndex = 9;
            this.label5.Text = "%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 345, 16 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 11, 12 );
            this.label4.TabIndex = 8;
            this.label4.Text = "%";
            // 
            // txtBendLength
            // 
            this.txtBendLength.BackColor = System.Drawing.SystemColors.Window;
            this.txtBendLength.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBendLength.Location = new System.Drawing.Point( 300, 39 );
            this.txtBendLength.Name = "txtBendLength";
            this.txtBendLength.Size = new System.Drawing.Size( 39, 19 );
            this.txtBendLength.TabIndex = 5;
            this.txtBendLength.Text = "0";
            this.txtBendLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBendLength.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtBendDepth
            // 
            this.txtBendDepth.BackColor = System.Drawing.SystemColors.Window;
            this.txtBendDepth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBendDepth.Location = new System.Drawing.Point( 300, 13 );
            this.txtBendDepth.Name = "txtBendDepth";
            this.txtBendDepth.Size = new System.Drawing.Size( 39, 19 );
            this.txtBendDepth.TabIndex = 2;
            this.txtBendDepth.Text = "8";
            this.txtBendDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBendDepth.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // trackBendLength
            // 
            this.trackBendLength.AutoSize = false;
            this.trackBendLength.Location = new System.Drawing.Point( 138, 40 );
            this.trackBendLength.Maximum = 100;
            this.trackBendLength.Name = "trackBendLength";
            this.trackBendLength.Size = new System.Drawing.Size( 156, 18 );
            this.trackBendLength.TabIndex = 4;
            this.trackBendLength.TickFrequency = 10;
            // 
            // trackBendDepth
            // 
            this.trackBendDepth.AutoSize = false;
            this.trackBendDepth.Location = new System.Drawing.Point( 138, 14 );
            this.trackBendDepth.Maximum = 100;
            this.trackBendDepth.Name = "trackBendDepth";
            this.trackBendDepth.Size = new System.Drawing.Size( 156, 18 );
            this.trackBendDepth.TabIndex = 1;
            this.trackBendDepth.TickFrequency = 10;
            this.trackBendDepth.Value = 8;
            // 
            // chkDownPortamento
            // 
            this.chkDownPortamento.AutoSize = true;
            this.chkDownPortamento.Location = new System.Drawing.Point( 20, 96 );
            this.chkDownPortamento.Name = "chkDownPortamento";
            this.chkDownPortamento.Size = new System.Drawing.Size( 224, 16 );
            this.chkDownPortamento.TabIndex = 7;
            this.chkDownPortamento.Text = "Add portamento in falling movement(&F)";
            this.chkDownPortamento.UseVisualStyleBackColor = true;
            // 
            // chkUpPortamento
            // 
            this.chkUpPortamento.AutoSize = true;
            this.chkUpPortamento.Location = new System.Drawing.Point( 20, 71 );
            this.chkUpPortamento.Name = "chkUpPortamento";
            this.chkUpPortamento.Size = new System.Drawing.Size( 222, 16 );
            this.chkUpPortamento.TabIndex = 6;
            this.chkUpPortamento.Text = "Add portamento in rising movement(&R)";
            this.chkUpPortamento.UseVisualStyleBackColor = true;
            // 
            // lblBendLength
            // 
            this.lblBendLength.AutoSize = true;
            this.lblBendLength.Location = new System.Drawing.Point( 20, 46 );
            this.lblBendLength.Name = "lblBendLength";
            this.lblBendLength.Size = new System.Drawing.Size( 83, 12 );
            this.lblBendLength.TabIndex = 3;
            this.lblBendLength.Text = "Bend Length(&L)";
            // 
            // lblBendDepth
            // 
            this.lblBendDepth.AutoSize = true;
            this.lblBendDepth.Location = new System.Drawing.Point( 20, 20 );
            this.lblBendDepth.Name = "lblBendDepth";
            this.lblBendDepth.Size = new System.Drawing.Size( 81, 12 );
            this.lblBendDepth.TabIndex = 0;
            this.lblBendDepth.Text = "Bend Depth(&B)";
            // 
            // groupDynamicsControl
            // 
            this.groupDynamicsControl.Controls.Add( this.label7 );
            this.groupDynamicsControl.Controls.Add( this.label6 );
            this.groupDynamicsControl.Controls.Add( this.txtAccent );
            this.groupDynamicsControl.Controls.Add( this.txtDecay );
            this.groupDynamicsControl.Controls.Add( this.trackAccent );
            this.groupDynamicsControl.Controls.Add( this.trackDecay );
            this.groupDynamicsControl.Controls.Add( this.lblAccent );
            this.groupDynamicsControl.Controls.Add( this.lblDecay );
            this.groupDynamicsControl.Location = new System.Drawing.Point( 3, 174 );
            this.groupDynamicsControl.Name = "groupDynamicsControl";
            this.groupDynamicsControl.Size = new System.Drawing.Size( 367, 74 );
            this.groupDynamicsControl.TabIndex = 1;
            this.groupDynamicsControl.TabStop = false;
            this.groupDynamicsControl.Text = "Dynamics Control (VOCALOID2)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 345, 46 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 11, 12 );
            this.label7.TabIndex = 11;
            this.label7.Text = "%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 345, 20 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 11, 12 );
            this.label6.TabIndex = 10;
            this.label6.Text = "%";
            // 
            // txtAccent
            // 
            this.txtAccent.BackColor = System.Drawing.SystemColors.Window;
            this.txtAccent.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtAccent.Location = new System.Drawing.Point( 300, 43 );
            this.txtAccent.Name = "txtAccent";
            this.txtAccent.Size = new System.Drawing.Size( 39, 19 );
            this.txtAccent.TabIndex = 13;
            this.txtAccent.Text = "50";
            this.txtAccent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAccent.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDecay
            // 
            this.txtDecay.BackColor = System.Drawing.SystemColors.Window;
            this.txtDecay.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDecay.Location = new System.Drawing.Point( 300, 17 );
            this.txtDecay.Name = "txtDecay";
            this.txtDecay.Size = new System.Drawing.Size( 39, 19 );
            this.txtDecay.TabIndex = 10;
            this.txtDecay.Text = "50";
            this.txtDecay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDecay.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // trackAccent
            // 
            this.trackAccent.AutoSize = false;
            this.trackAccent.Location = new System.Drawing.Point( 138, 44 );
            this.trackAccent.Maximum = 100;
            this.trackAccent.Name = "trackAccent";
            this.trackAccent.Size = new System.Drawing.Size( 156, 18 );
            this.trackAccent.TabIndex = 12;
            this.trackAccent.TickFrequency = 10;
            this.trackAccent.Value = 50;
            // 
            // trackDecay
            // 
            this.trackDecay.AutoSize = false;
            this.trackDecay.Location = new System.Drawing.Point( 138, 18 );
            this.trackDecay.Maximum = 100;
            this.trackDecay.Name = "trackDecay";
            this.trackDecay.Size = new System.Drawing.Size( 156, 18 );
            this.trackDecay.TabIndex = 9;
            this.trackDecay.TickFrequency = 10;
            this.trackDecay.Value = 50;
            // 
            // lblAccent
            // 
            this.lblAccent.AutoSize = true;
            this.lblAccent.Location = new System.Drawing.Point( 18, 50 );
            this.lblAccent.Name = "lblAccent";
            this.lblAccent.Size = new System.Drawing.Size( 57, 12 );
            this.lblAccent.TabIndex = 11;
            this.lblAccent.Text = "Accent(&A)";
            // 
            // lblDecay
            // 
            this.lblDecay.AutoSize = true;
            this.lblDecay.Location = new System.Drawing.Point( 18, 24 );
            this.lblDecay.Name = "lblDecay";
            this.lblDecay.Size = new System.Drawing.Size( 53, 12 );
            this.lblDecay.TabIndex = 8;
            this.lblDecay.Text = "Decay(&D)";
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Location = new System.Drawing.Point( 165, 6 );
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size( 67, 12 );
            this.lblTemplate.TabIndex = 2;
            this.lblTemplate.Text = "Template(&T)";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 285, 12 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 78, 23 );
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 198, 12 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 78, 23 );
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // comboTemplate
            // 
            this.comboTemplate.FormattingEnabled = true;
            this.comboTemplate.Items.AddRange( new Object[] {
            "[Select a template]",
            "normal",
            "accent",
            "strong accent",
            "legato",
            "slow legato"} );
            this.comboTemplate.Location = new System.Drawing.Point( 242, 3 );
            this.comboTemplate.Name = "comboTemplate";
            this.comboTemplate.Size = new System.Drawing.Size( 121, 20 );
            this.comboTemplate.TabIndex = 0;
            this.comboTemplate.Text = "[Select a template]";
            // 
            // groupAttack
            // 
            this.groupAttack.Controls.Add( this.lblAttackTemplate );
            this.groupAttack.Controls.Add( this.comboAttackTemplate );
            this.groupAttack.Controls.Add( this.txtDepth );
            this.groupAttack.Controls.Add( this.txtDuration );
            this.groupAttack.Controls.Add( this.trackDepth );
            this.groupAttack.Controls.Add( this.trackDuration );
            this.groupAttack.Controls.Add( this.lblDepth );
            this.groupAttack.Controls.Add( this.lblDuration );
            this.groupAttack.Location = new System.Drawing.Point( 3, 254 );
            this.groupAttack.Name = "groupAttack";
            this.groupAttack.Size = new System.Drawing.Size( 367, 107 );
            this.groupAttack.TabIndex = 17;
            this.groupAttack.TabStop = false;
            this.groupAttack.Text = "Attack (VOCALOID1)";
            // 
            // lblAttackTemplate
            // 
            this.lblAttackTemplate.AutoSize = true;
            this.lblAttackTemplate.Location = new System.Drawing.Point( 18, 23 );
            this.lblAttackTemplate.Name = "lblAttackTemplate";
            this.lblAttackTemplate.Size = new System.Drawing.Size( 105, 12 );
            this.lblAttackTemplate.TabIndex = 2;
            this.lblAttackTemplate.Text = "Attack Variation(&V)";
            // 
            // comboAttackTemplate
            // 
            this.comboAttackTemplate.FormattingEnabled = true;
            this.comboAttackTemplate.Location = new System.Drawing.Point( 143, 20 );
            this.comboAttackTemplate.Name = "comboAttackTemplate";
            this.comboAttackTemplate.Size = new System.Drawing.Size( 121, 20 );
            this.comboAttackTemplate.TabIndex = 0;
            this.comboAttackTemplate.Text = "[Non Attack]";
            // 
            // txtDepth
            // 
            this.txtDepth.BackColor = System.Drawing.SystemColors.Window;
            this.txtDepth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDepth.Location = new System.Drawing.Point( 300, 72 );
            this.txtDepth.Name = "txtDepth";
            this.txtDepth.Size = new System.Drawing.Size( 39, 19 );
            this.txtDepth.TabIndex = 13;
            this.txtDepth.Text = "64";
            this.txtDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDepth.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDuration
            // 
            this.txtDuration.BackColor = System.Drawing.SystemColors.Window;
            this.txtDuration.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDuration.Location = new System.Drawing.Point( 300, 46 );
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size( 39, 19 );
            this.txtDuration.TabIndex = 10;
            this.txtDuration.Text = "64";
            this.txtDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDuration.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // trackDepth
            // 
            this.trackDepth.AutoSize = false;
            this.trackDepth.Location = new System.Drawing.Point( 138, 69 );
            this.trackDepth.Maximum = 127;
            this.trackDepth.Name = "trackDepth";
            this.trackDepth.Size = new System.Drawing.Size( 156, 18 );
            this.trackDepth.TabIndex = 12;
            this.trackDepth.TickFrequency = 10;
            this.trackDepth.Value = 64;
            // 
            // trackDuration
            // 
            this.trackDuration.AutoSize = false;
            this.trackDuration.Location = new System.Drawing.Point( 138, 43 );
            this.trackDuration.Maximum = 127;
            this.trackDuration.Name = "trackDuration";
            this.trackDuration.Size = new System.Drawing.Size( 156, 18 );
            this.trackDuration.TabIndex = 9;
            this.trackDuration.TickFrequency = 10;
            this.trackDuration.Value = 64;
            // 
            // lblDepth
            // 
            this.lblDepth.AutoSize = true;
            this.lblDepth.Location = new System.Drawing.Point( 18, 75 );
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size( 51, 12 );
            this.lblDepth.TabIndex = 11;
            this.lblDepth.Text = "Depth(&D)";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point( 18, 49 );
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size( 64, 12 );
            this.lblDuration.TabIndex = 8;
            this.lblDuration.Text = "Duration(&D)";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.Controls.Add( this.panelVocaloid2Template );
            this.flowLayoutPanel.Controls.Add( this.groupPitchControl );
            this.flowLayoutPanel.Controls.Add( this.groupDynamicsControl );
            this.flowLayoutPanel.Controls.Add( this.groupAttack );
            this.flowLayoutPanel.Controls.Add( this.panelButtons );
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point( 9, 9 );
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding( 0 );
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size( 373, 418 );
            this.flowLayoutPanel.TabIndex = 18;
            // 
            // panelVocaloid2Template
            // 
            this.panelVocaloid2Template.Controls.Add( this.comboTemplate );
            this.panelVocaloid2Template.Controls.Add( this.lblTemplate );
            this.panelVocaloid2Template.Location = new System.Drawing.Point( 3, 3 );
            this.panelVocaloid2Template.Name = "panelVocaloid2Template";
            this.panelVocaloid2Template.Size = new System.Drawing.Size( 367, 29 );
            this.panelVocaloid2Template.TabIndex = 19;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add( this.btnCancel );
            this.panelButtons.Controls.Add( this.btnOK );
            this.panelButtons.Location = new System.Drawing.Point( 3, 367 );
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size( 367, 48 );
            this.panelButtons.TabIndex = 19;
            // 
            // FormNoteExpressionConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 390, 514 );
            this.Controls.Add( this.flowLayoutPanel );
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size( 6000, 6000 );
            this.MinimizeBox = false;
            this.Name = "FormNoteExpressionConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Default Singer Style";
            this.groupPitchControl.ResumeLayout( false );
            this.groupPitchControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendDepth)).EndInit();
            this.groupDynamicsControl.ResumeLayout( false );
            this.groupDynamicsControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAccent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDecay)).EndInit();
            this.groupAttack.ResumeLayout( false );
            this.groupAttack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDuration)).EndInit();
            this.flowLayoutPanel.ResumeLayout( false );
            this.panelVocaloid2Template.ResumeLayout( false );
            this.panelVocaloid2Template.PerformLayout();
            this.panelButtons.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BGroupBox groupPitchControl;
        private BGroupBox groupDynamicsControl;
        private BLabel lblBendDepth;
        private BLabel lblTemplate;
        private BLabel lblBendLength;
        private BCheckBox chkDownPortamento;
        private BCheckBox chkUpPortamento;
        private BSlider trackBendDepth;
        private BSlider trackBendLength;
        private BSlider trackAccent;
        private BSlider trackDecay;
        private BLabel lblAccent;
        private BLabel lblDecay;
        private NumberTextBox txtBendLength;
        private NumberTextBox txtBendDepth;
        private NumberTextBox txtAccent;
        private NumberTextBox txtDecay;
        private BLabel label5;
        private BLabel label4;
        private BLabel label7;
        private BLabel label6;
        private BButton btnCancel;
        private BButton btnOK;
        private BComboBox comboTemplate;
        private BGroupBox groupAttack;
        private NumberTextBox txtDepth;
        private NumberTextBox txtDuration;
        private BSlider trackDepth;
        private BSlider trackDuration;
        private BLabel lblDepth;
        private BLabel lblDuration;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private BPanel panelButtons;
        private BPanel panelVocaloid2Template;
        private BComboBox comboAttackTemplate;
        private BLabel lblAttackTemplate;
        #endregion
#endif
    }

#if !JAVA
}
#endif
