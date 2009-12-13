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
package org.kbinani.cadencii;

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\FormBeatConfig.java

import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree;
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
            return (int)numStart.getValue();
        }

        public boolean isEndSpecified() {
            return chkEnd.isSelected();
        }

        public int getEnd() {
            return (int)numEnd.getValue();
        }

        public int getNumerator() {
            return (int)numNumerator.getValue();
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
            super();
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

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        private void registerEventHandlers() {
            chkEnd.checkedChangedEvent.add( new BEventHandler( this, "chkEnd_CheckedChanged" ) );
            btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormBeatConfig.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormBeatConfig.java
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
            this.groupPosition = new bocoree.windows.forms.BGroupBox();
            this.lblBar2 = new bocoree.windows.forms.BLabel();
            this.lblBar1 = new bocoree.windows.forms.BLabel();
            this.numEnd = new Boare.Cadencii.NumericUpDownEx();
            this.numStart = new Boare.Cadencii.NumericUpDownEx();
            this.chkEnd = new bocoree.windows.forms.BCheckBox();
            this.lblStart = new bocoree.windows.forms.BLabel();
            this.groupBeat = new bocoree.windows.forms.BGroupBox();
            this.comboDenominator = new bocoree.windows.forms.BComboBox();
            this.label2 = new bocoree.windows.forms.BLabel();
            this.label1 = new bocoree.windows.forms.BLabel();
            this.numNumerator = new Boare.Cadencii.NumericUpDownEx();
            this.btnOK = new bocoree.windows.forms.BButton();
            this.btnCancel = new bocoree.windows.forms.BButton();
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
