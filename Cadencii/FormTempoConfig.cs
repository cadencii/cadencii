/*
 * FormTempoConfig.cs
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

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\FormTempoConfig.java

import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class FormTempoConfig extends BForm{
#else
    class FormTempoConfig : BForm {
#endif
        public FormTempoConfig( int bar_count, int beat, int beat_max, int clock, int clock_max, float tempo, int pre_measure ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            numBar.setMinimum( -pre_measure + 1 );
            numBar.setMaximum( 100000 );
            numBar.setValue( bar_count );

            numBeat.setMinimum( 1 );
            numBeat.setMaximum( beat_max );
            numBeat.setValue( beat );
            numClock.setMinimum( 0 );
            numClock.setMaximum( clock_max );
            numClock.setValue( clock );
            numTempo.setValue( tempo );
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public void ApplyLanguage() {
            setTitle( _( "Global Tempo" ) );
            groupPosition.setTitle( _( "Position" ) );
            lblBar.setText( _( "Measure" ) + "(&M)" );
            lblBeat.setText( _( "Beat" ) + "(&B)" );
            lblClock.setText( _( "Clock" ) + "(&L)" );
            groupTempo.setTitle( _( "Tempo" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
        }

        public int getBeatCount() {
            return (int)numBeat.getValue();
        }

        public int getClock() {
            return (int)numClock.getValue();
        }

        public float getTempo() {
            return numTempo.getValue();
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        private void registerEventHandlers() {
#if JAVA
#else
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
#endif
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormTempoConfig.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormTempoConfig.java
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
            this.numClock = new Boare.Cadencii.NumericUpDownEx();
            this.numBeat = new Boare.Cadencii.NumericUpDownEx();
            this.numBar = new Boare.Cadencii.NumericUpDownEx();
            this.lblClock = new BLabel();
            this.lblBeat = new BLabel();
            this.lblBar = new BLabel();
            this.groupTempo = new BGroupBox();
            this.lblTempoRange = new BLabel();
            this.numTempo = new Boare.Cadencii.NumericUpDownEx();
            this.btnOK = new BButton();
            this.btnCancel = new BButton();
            this.groupPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBar)).BeginInit();
            this.groupTempo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTempo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPosition
            // 
            this.groupPosition.Controls.Add( this.numClock );
            this.groupPosition.Controls.Add( this.numBeat );
            this.groupPosition.Controls.Add( this.numBar );
            this.groupPosition.Controls.Add( this.lblClock );
            this.groupPosition.Controls.Add( this.lblBeat );
            this.groupPosition.Controls.Add( this.lblBar );
            this.groupPosition.Location = new System.Drawing.Point( 11, 10 );
            this.groupPosition.Name = "groupPosition";
            this.groupPosition.Size = new System.Drawing.Size( 147, 113 );
            this.groupPosition.TabIndex = 0;
            this.groupPosition.TabStop = false;
            this.groupPosition.Text = "Position";
            // 
            // numClock
            // 
            this.numClock.Location = new System.Drawing.Point( 90, 77 );
            this.numClock.Name = "numClock";
            this.numClock.Size = new System.Drawing.Size( 45, 19 );
            this.numClock.TabIndex = 5;
            // 
            // numBeat
            // 
            this.numBeat.Location = new System.Drawing.Point( 90, 48 );
            this.numBeat.Name = "numBeat";
            this.numBeat.Size = new System.Drawing.Size( 45, 19 );
            this.numBeat.TabIndex = 4;
            // 
            // numBar
            // 
            this.numBar.Enabled = false;
            this.numBar.Location = new System.Drawing.Point( 90, 19 );
            this.numBar.Name = "numBar";
            this.numBar.Size = new System.Drawing.Size( 45, 19 );
            this.numBar.TabIndex = 3;
            // 
            // lblClock
            // 
            this.lblClock.AutoSize = true;
            this.lblClock.Location = new System.Drawing.Point( 12, 79 );
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size( 48, 12 );
            this.lblClock.TabIndex = 2;
            this.lblClock.Text = "Clock(&L)";
            // 
            // lblBeat
            // 
            this.lblBeat.AutoSize = true;
            this.lblBeat.Location = new System.Drawing.Point( 12, 50 );
            this.lblBeat.Name = "lblBeat";
            this.lblBeat.Size = new System.Drawing.Size( 45, 12 );
            this.lblBeat.TabIndex = 1;
            this.lblBeat.Text = "Beat(&B)";
            // 
            // lblBar
            // 
            this.lblBar.AutoSize = true;
            this.lblBar.Location = new System.Drawing.Point( 12, 21 );
            this.lblBar.Name = "lblBar";
            this.lblBar.Size = new System.Drawing.Size( 65, 12 );
            this.lblBar.TabIndex = 0;
            this.lblBar.Text = "Measure(&M)";
            // 
            // groupTempo
            // 
            this.groupTempo.Controls.Add( this.lblTempoRange );
            this.groupTempo.Controls.Add( this.numTempo );
            this.groupTempo.Location = new System.Drawing.Point( 173, 10 );
            this.groupTempo.Name = "groupTempo";
            this.groupTempo.Size = new System.Drawing.Size( 143, 113 );
            this.groupTempo.TabIndex = 1;
            this.groupTempo.TabStop = false;
            this.groupTempo.Text = "Tempo";
            // 
            // lblTempoRange
            // 
            this.lblTempoRange.AutoSize = true;
            this.lblTempoRange.Location = new System.Drawing.Point( 78, 21 );
            this.lblTempoRange.Name = "lblTempoRange";
            this.lblTempoRange.Size = new System.Drawing.Size( 61, 12 );
            this.lblTempoRange.TabIndex = 6;
            this.lblTempoRange.Text = "(20 to 300)";
            // 
            // numTempo
            // 
            this.numTempo.DecimalPlaces = 2;
            this.numTempo.Location = new System.Drawing.Point( 13, 19 );
            this.numTempo.Maximum = new decimal( new int[] {
            300,
            0,
            0,
            0} );
            this.numTempo.Minimum = new decimal( new int[] {
            20,
            0,
            0,
            0} );
            this.numTempo.Name = "numTempo";
            this.numTempo.Size = new System.Drawing.Size( 59, 19 );
            this.numTempo.TabIndex = 5;
            this.numTempo.Value = new decimal( new int[] {
            120,
            0,
            0,
            0} );
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point( 156, 132 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 241, 132 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormTempoConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 332, 164 );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.groupTempo );
            this.Controls.Add( this.groupPosition );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTempoConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Global Tempo";
            this.groupPosition.ResumeLayout( false );
            this.groupPosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBar)).EndInit();
            this.groupTempo.ResumeLayout( false );
            this.groupTempo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTempo)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private BGroupBox groupPosition;
        private BLabel lblClock;
        private BLabel lblBeat;
        private BLabel lblBar;
        private BGroupBox groupTempo;
        private BButton btnOK;
        private BButton btnCancel;
        private NumericUpDownEx numBar;
        private NumericUpDownEx numClock;
        private NumericUpDownEx numBeat;
        private BLabel lblTempoRange;
        private NumericUpDownEx numTempo;
        #endregion
#endif
    }

#if !JAVA
}
#endif
