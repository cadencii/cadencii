/*
 * FormBeatConfig.cs
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

import org.kbinani.*;
import org.kbinani.windows.forms.*;
import org.kbinani.apputil.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class FormBeatConfig extends BForm{
#else
    public class FormBeatConfig : BForm {
#endif
        public void ApplyLanguage() {
            setTitle( _( "Beat Change" ) );
            groupPosition.setTitle( _( "Position" ) );
            groupBeat.setTitle( _( "Beat" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            lblStart.setText( _( "From" ) + "(&F)" );
            chkEnd.setText( _( "To" ) + "(&T)" );
            lblBar1.setText( _( "Measure" ) );
            lblBar2.setText( _( "Measure" ) );
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public int getStart() {
            return numStart.getValue();
        }

        public boolean isEndSpecified() {
            return chkEnd.isSelected();
        }

        public int getEnd() {
            return numEnd.getValue();
        }

        public int getNumerator() {
            return numNumerator.getValue();
        }

        public int getDenominator() {
            int ret = 1;
            for ( int i = 0; i < comboDenominator.getSelectedIndex(); i++ ) {
                ret *= 2;
            }
            return ret;
        }

        public FormBeatConfig( int bar_count, int numerator, int denominator, boolean num_enabled, int pre_measure ) {
#if JAVA
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            //ClientSize = new Size( 278, 182 );

            numStart.setEnabled( num_enabled );
            numEnd.setEnabled( num_enabled );
            chkEnd.setEnabled( num_enabled );
            numStart.setMinimum( -pre_measure + 1 );
            numStart.setMaximum( int.MaxValue );
            numEnd.setMinimum( -pre_measure + 1 );
            numEnd.setMaximum( int.MaxValue );

            // 拍子の分母
            comboDenominator.removeAllItems();
            comboDenominator.addItem( "1" );
            int count = 1;
            for ( int i = 1; i <= 5; i++ ) {
                count *= 2;
                comboDenominator.addItem( count + "" );
            }
            count = 0;
            while ( denominator > 1 ) {
                count++;
                denominator /= 2;
            }
            comboDenominator.setSelectedItem( comboDenominator.getItemAt( count ) );

            // 拍子の分子
            if ( numerator < numNumerator.getMinimum() ) {
                numNumerator.setValue( numNumerator.getMinimum() );
            } else if ( numNumerator.getMaximum() < numerator ) {
                numNumerator.setValue( numNumerator.getMaximum() );
            } else {
                numNumerator.setValue( numerator );
            }

            // 始点
            if ( bar_count < numStart.getMinimum() ) {
                numStart.setValue( numStart.getMinimum() );
            } else if ( numStart.getMaximum() < bar_count ) {
                numStart.setValue( numStart.getMaximum() );
            } else {
                numStart.setValue( bar_count );
            }

            // 終点
            if ( bar_count < numEnd.getMinimum() ) {
                numEnd.setValue( numEnd.getMinimum() );
            } else if ( numEnd.getMaximum() < bar_count ) {
                numEnd.setValue( numEnd.getMaximum() );
            } else {
                numEnd.setValue( bar_count );
            }
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        private void chkEnd_CheckedChanged( Object sender, BEventArgs e ) {
            numEnd.setEnabled( chkEnd.isSelected() );
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        private void registerEventHandlers() {
#if JAVA
            chkEnd.checkedChangedEvent.add( new BEventHandler( this, "chkEnd_CheckedChanged" ) );
            btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
#else
            this.chkEnd.CheckedChanged += new System.EventHandler( this.chkEnd_CheckedChanged );
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
#endif
        }

        private void setResources() {
        }
#if JAVA
        #region UI Impl for Java
    private static final long serialVersionUID = 4414859292940722020L;
    private BPanel jContentPane = null;
    private BGroupBox groupPosition = null;
    private BLabel lblStart = null;
    private BComboBox numStart = null;
    private BLabel lblBar1 = null;
    private BCheckBox chkEnd = null;
    private BComboBox numEnd = null;
    private BLabel lblBar2 = null;
    private BGroupBox groupBeat = null;
    private BComboBox numNumerator = null;
    private BLabel jLabel = null;
    private BLabel jLabel1 = null;
    private BComboBox numDenominator = null;
    private BPanel jPanel1 = null;
    private BLabel jLabel2 = null;
    private BLabel jLabel3 = null;
    private BButton btnOk = null;
    private BButton btnCancel = null;
    private BLabel jLabel4 = null;
    private BLabel jLabel5 = null;
    private BLabel jLabel6 = null;
    private BLabel jLabel7 = null;
    private BLabel jLabel8 = null;
    
    /**
     * This method initializes this
     * 
     * @return void
     */
    private void initialize() {
        this.setTitle("Beat Change");
        this.setSize(310, 338);
        this.setContentPane(getJContentPane());
        this.setTitle("JFrame");
    }

    /**
     * This method initializes jContentPane
     * 
     * @return javax.swing.BPanel
     */
    private BPanel getJContentPane() {
        if (jContentPane == null) {
            GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
            gridBagConstraints18.insets = new Insets(0, 0, 1, 12);
            gridBagConstraints18.gridy = 2;
            gridBagConstraints18.ipadx = 180;
            gridBagConstraints18.ipady = 42;
            gridBagConstraints18.weightx = 1.0D;
            gridBagConstraints18.weighty = 1.0D;
            gridBagConstraints18.anchor = GridBagConstraints.NORTH;
            gridBagConstraints18.fill = GridBagConstraints.BOTH;
            gridBagConstraints18.gridx = 0;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 0;
            gridBagConstraints8.ipadx = 141;
            gridBagConstraints8.ipady = 42;
            gridBagConstraints8.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints8.insets = new Insets(0, 12, 0, 12);
            gridBagConstraints8.weightx = 1.0D;
            gridBagConstraints8.anchor = GridBagConstraints.NORTH;
            gridBagConstraints8.gridy = 1;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.ipadx = 147;
            gridBagConstraints6.ipady = 41;
            gridBagConstraints6.insets = new Insets(12, 12, 12, 12);
            gridBagConstraints6.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints6.weightx = 1.0D;
            gridBagConstraints6.anchor = GridBagConstraints.NORTH;
            gridBagConstraints6.gridy = 0;
            jContentPane = new BPanel();
            jContentPane.setLayout(new GridBagLayout());
            jContentPane.add(getGroupPosition(), gridBagConstraints6);
            jContentPane.add(getGroupBeat(), gridBagConstraints8);
            jContentPane.add(getBPanel1(), gridBagConstraints18);
        }
        return jContentPane;
    }

    /**
     * This method initializes groupPosition    
     *  
     * @return javax.swing.BPanel   
     */
    private BGroupBox getGroupPosition() {
        if (groupPosition == null) {
            GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
            gridBagConstraints61.gridx = 5;
            gridBagConstraints61.gridy = 2;
            jLabel8 = new BLabel();
            jLabel8.setText("     ");
            GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
            gridBagConstraints51.gridx = 5;
            gridBagConstraints51.gridy = 0;
            jLabel7 = new BLabel();
            jLabel7.setText("     ");
            GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
            gridBagConstraints41.gridx = 0;
            gridBagConstraints41.gridy = 2;
            jLabel6 = new BLabel();
            jLabel6.setText("     ");
            GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
            gridBagConstraints31.gridx = 0;
            gridBagConstraints31.anchor = GridBagConstraints.WEST;
            gridBagConstraints31.gridy = 0;
            jLabel5 = new BLabel();
            jLabel5.setText("     ");
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints9.gridy = -1;
            gridBagConstraints9.weightx = 1.0;
            gridBagConstraints9.gridx = -1;
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 4;
            gridBagConstraints5.anchor = GridBagConstraints.WEST;
            gridBagConstraints5.insets = new Insets(0, 9, 0, 0);
            gridBagConstraints5.gridy = 2;
            lblBar2 = new BLabel();
            lblBar2.setText("Beat");
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints4.gridy = 2;
            gridBagConstraints4.weightx = 1.0;
            gridBagConstraints4.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints4.gridx = 3;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 1;
            gridBagConstraints3.gridy = 2;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 4;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(0, 9, 0, 0);
            gridBagConstraints2.gridy = 0;
            lblBar1 = new BLabel();
            lblBar1.setText("Measure");
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 0.0D;
            gridBagConstraints1.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints1.gridx = 3;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 1;
            gridBagConstraints.gridy = 0;
            lblStart = new BLabel();
            lblStart.setText("From");
            groupPosition = new BGroupBox();
            groupPosition.setLayout(new GridBagLayout());
            groupPosition.setTitle("Position");
            groupPosition.add(lblStart, gridBagConstraints);
            groupPosition.add(getNumStart(), gridBagConstraints1);
            groupPosition.add(lblBar1, gridBagConstraints2);
            groupPosition.add(getChkEnd(), gridBagConstraints3);
            groupPosition.add(getNumEnd(), gridBagConstraints4);
            groupPosition.add(lblBar2, gridBagConstraints5);
            groupPosition.add(jLabel5, gridBagConstraints31);
            groupPosition.add(jLabel6, gridBagConstraints41);
            groupPosition.add(jLabel7, gridBagConstraints51);
            groupPosition.add(jLabel8, gridBagConstraints61);
        }
        return groupPosition;
    }

    /**
     * This method initializes numStart 
     *  
     * @return javax.swing.BComboBox    
     */
    private BComboBox getNumStart() {
        if (numStart == null) {
            numStart = new BComboBox();
            numStart.setPreferredSize(new Dimension(31, 20));
        }
        return numStart;
    }

    /**
     * This method initializes chkEnd   
     *  
     * @return javax.swing.BCheckBox    
     */
    private BCheckBox getChkEnd() {
        if (chkEnd == null) {
            chkEnd = new BCheckBox();
            chkEnd.setText("To");
        }
        return chkEnd;
    }

    /**
     * This method initializes numEnd   
     *  
     * @return javax.swing.BComboBox    
     */
    private BComboBox getNumEnd() {
        if (numEnd == null) {
            numEnd = new BComboBox();
            numEnd.setPreferredSize(new Dimension(31, 20));
        }
        return numEnd;
    }

    /**
     * This method initializes groupBeat    
     *  
     * @return javax.swing.BPanel   
     */
    private BGroupBox getGroupBeat() {
        if (groupBeat == null) {
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.gridx = 0;
            gridBagConstraints14.gridy = 0;
            jLabel3 = new BLabel();
            jLabel3.setText("     ");
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 5;
            gridBagConstraints13.gridy = 0;
            jLabel2 = new BLabel();
            jLabel2.setText("     ");
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints12.gridy = 0;
            gridBagConstraints12.weightx = 0.5D;
            gridBagConstraints12.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints12.gridx = 4;
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 3;
            gridBagConstraints11.gridy = 0;
            jLabel1 = new BLabel();
            jLabel1.setText(" /    ");
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 2;
            gridBagConstraints10.gridy = 0;
            jLabel = new BLabel();
            jLabel.setText(" (1-255) ");
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints7.gridy = 0;
            gridBagConstraints7.weightx = 0.5D;
            gridBagConstraints7.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints7.gridx = 1;
            groupBeat = new BGroupBox();
            groupBeat.setLayout(new GridBagLayout());
            groupBeat.setTitle("Position");
            groupBeat.add(getNumNumerator(), gridBagConstraints7);
            groupBeat.add(jLabel, gridBagConstraints10);
            groupBeat.add(jLabel1, gridBagConstraints11);
            groupBeat.add(getNumDenominator(), gridBagConstraints12);
            groupBeat.add(jLabel2, gridBagConstraints13);
            groupBeat.add(jLabel3, gridBagConstraints14);
        }
        return groupBeat;
    }

    /**
     * This method initializes numNumerator 
     *  
     * @return javax.swing.BComboBox    
     */
    private BComboBox getNumNumerator() {
        if (numNumerator == null) {
            numNumerator = new BComboBox();
            numNumerator.setPreferredSize(new Dimension(31, 20));
        }
        return numNumerator;
    }

    /**
     * This method initializes numDenominator   
     *  
     * @return javax.swing.BComboBox    
     */
    private BComboBox getNumDenominator() {
        if (numDenominator == null) {
            numDenominator = new BComboBox();
            numDenominator.setPreferredSize(new Dimension(31, 20));
        }
        return numDenominator;
    }

    /**
     * This method initializes jPanel1  
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getBPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
            gridBagConstraints17.gridx = 0;
            gridBagConstraints17.fill = GridBagConstraints.BOTH;
            gridBagConstraints17.weightx = 1.0D;
            gridBagConstraints17.gridy = 0;
            jLabel4 = new BLabel();
            jLabel4.setText(" ");
            GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
            gridBagConstraints16.gridx = 2;
            gridBagConstraints16.anchor = GridBagConstraints.EAST;
            gridBagConstraints16.gridy = 0;
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.gridx = 1;
            gridBagConstraints15.anchor = GridBagConstraints.EAST;
            gridBagConstraints15.gridy = 0;
            jPanel1 = new BPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getBtnOk(), gridBagConstraints15);
            jPanel1.add(getBtnCancel(), gridBagConstraints16);
            jPanel1.add(jLabel4, gridBagConstraints17);
        }
        return jPanel1;
    }

    /**
     * This method initializes btnOk    
     *  
     * @return javax.swing.BButton  
     */
    private BButton getBtnOk() {
        if (btnOk == null) {
            btnOk = new BButton();
            btnOk.setText("OK");
        }
        return btnOk;
    }

    /**
     * This method initializes btnCancel    
     *  
     * @return javax.swing.BButton  
     */
    private BButton getBtnCancel() {
        if (btnCancel == null) {
            btnCancel = new BButton();
            btnCancel.setText("Cancel");
        }
        return btnCancel;
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
            this.groupPosition = new BGroupBox();
            this.lblBar2 = new BLabel();
            this.lblBar1 = new BLabel();
            this.numEnd = new Boare.Cadencii.NumericUpDownEx();
            this.numStart = new Boare.Cadencii.NumericUpDownEx();
            this.chkEnd = new BCheckBox();
            this.lblStart = new BLabel();
            this.groupBeat = new BGroupBox();
            this.comboDenominator = new BComboBox();
            this.label2 = new BLabel();
            this.label1 = new BLabel();
            this.numNumerator = new Boare.Cadencii.NumericUpDownEx();
            this.btnOK = new BButton();
            this.btnCancel = new BButton();
            this.groupPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            this.groupBeat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumerator)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPosition
            // 
            this.groupPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPosition.Controls.Add( this.lblBar2 );
            this.groupPosition.Controls.Add( this.lblBar1 );
            this.groupPosition.Controls.Add( this.numEnd );
            this.groupPosition.Controls.Add( this.numStart );
            this.groupPosition.Controls.Add( this.chkEnd );
            this.groupPosition.Controls.Add( this.lblStart );
            this.groupPosition.Location = new System.Drawing.Point( 7, 7 );
            this.groupPosition.Name = "groupPosition";
            this.groupPosition.Size = new System.Drawing.Size( 261, 71 );
            this.groupPosition.TabIndex = 0;
            this.groupPosition.TabStop = false;
            this.groupPosition.Text = "Position";
            // 
            // lblBar2
            // 
            this.lblBar2.AutoSize = true;
            this.lblBar2.Location = new System.Drawing.Point( 176, 46 );
            this.lblBar2.Name = "lblBar2";
            this.lblBar2.Size = new System.Drawing.Size( 48, 12 );
            this.lblBar2.TabIndex = 5;
            this.lblBar2.Text = "Measure";
            // 
            // lblBar1
            // 
            this.lblBar1.AutoSize = true;
            this.lblBar1.Location = new System.Drawing.Point( 176, 20 );
            this.lblBar1.Name = "lblBar1";
            this.lblBar1.Size = new System.Drawing.Size( 48, 12 );
            this.lblBar1.TabIndex = 4;
            this.lblBar1.Text = "Measure";
            // 
            // numEnd
            // 
            this.numEnd.Enabled = false;
            this.numEnd.Location = new System.Drawing.Point( 97, 44 );
            this.numEnd.Maximum = new decimal( new int[] {
            999,
            0,
            0,
            0} );
            this.numEnd.Minimum = new decimal( new int[] {
            2,
            0,
            0,
            0} );
            this.numEnd.Name = "numEnd";
            this.numEnd.Size = new System.Drawing.Size( 73, 19 );
            this.numEnd.TabIndex = 3;
            this.numEnd.Value = new decimal( new int[] {
            2,
            0,
            0,
            0} );
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point( 97, 18 );
            this.numStart.Maximum = new decimal( new int[] {
            999,
            0,
            0,
            0} );
            this.numStart.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size( 73, 19 );
            this.numStart.TabIndex = 2;
            this.numStart.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // chkEnd
            // 
            this.chkEnd.AutoSize = true;
            this.chkEnd.Location = new System.Drawing.Point( 23, 45 );
            this.chkEnd.Name = "chkEnd";
            this.chkEnd.Size = new System.Drawing.Size( 52, 16 );
            this.chkEnd.TabIndex = 1;
            this.chkEnd.Text = "To(&T)";
            this.chkEnd.UseVisualStyleBackColor = true;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point( 21, 20 );
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size( 46, 12 );
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "From(&F)";
            // 
            // groupBeat
            // 
            this.groupBeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBeat.Controls.Add( this.comboDenominator );
            this.groupBeat.Controls.Add( this.label2 );
            this.groupBeat.Controls.Add( this.label1 );
            this.groupBeat.Controls.Add( this.numNumerator );
            this.groupBeat.Location = new System.Drawing.Point( 7, 84 );
            this.groupBeat.Name = "groupBeat";
            this.groupBeat.Size = new System.Drawing.Size( 261, 55 );
            this.groupBeat.TabIndex = 1;
            this.groupBeat.TabStop = false;
            this.groupBeat.Text = "Beat";
            // 
            // comboDenominator
            // 
            this.comboDenominator.FormattingEnabled = true;
            this.comboDenominator.Location = new System.Drawing.Point( 182, 20 );
            this.comboDenominator.Name = "comboDenominator";
            this.comboDenominator.Size = new System.Drawing.Size( 73, 20 );
            this.comboDenominator.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 160, 23 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 11, 12 );
            this.label2.TabIndex = 7;
            this.label2.Text = "/";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 102, 23 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 51, 12 );
            this.label1.TabIndex = 6;
            this.label1.Text = "(1 - 255)";
            // 
            // numNumerator
            // 
            this.numNumerator.Location = new System.Drawing.Point( 23, 21 );
            this.numNumerator.Maximum = new decimal( new int[] {
            255,
            0,
            0,
            0} );
            this.numNumerator.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numNumerator.Name = "numNumerator";
            this.numNumerator.Size = new System.Drawing.Size( 73, 19 );
            this.numNumerator.TabIndex = 4;
            this.numNumerator.Value = new decimal( new int[] {
            4,
            0,
            0,
            0} );
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point( 113, 150 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 194, 150 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormBeatConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 278, 182 );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.groupBeat );
            this.Controls.Add( this.groupPosition );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBeatConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Beat Change";
            this.groupPosition.ResumeLayout( false );
            this.groupPosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            this.groupBeat.ResumeLayout( false );
            this.groupBeat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumerator)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private BGroupBox groupPosition;
        private BGroupBox groupBeat;
        private BButton btnOK;
        private BButton btnCancel;
        private NumericUpDownEx numStart;
        private BCheckBox chkEnd;
        private BLabel lblStart;
        private BLabel lblBar2;
        private BLabel lblBar1;
        private NumericUpDownEx numEnd;
        private BLabel label1;
        private NumericUpDownEx numNumerator;
        private BLabel label2;
        private BComboBox comboDenominator;
        #endregion
#endif

    }

#if !JAVA
}
#endif
